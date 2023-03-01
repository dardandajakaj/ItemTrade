using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Dto;
using API.Entity;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public MessageController(DataContext context, IMapper mapper)

        {
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> getUserMessages([FromQuery]string sender)
        {
            var receiver = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (receiver == null)
            {
                return BadRequest("Not Authenticated!");
            }

            var messages = await _context.Message
                .Where(x => (x.Receiver.UserName == receiver && x.Sender.UserName == sender) && !x.DeletedByReceiver)
                .OrderByDescending(m => m.SentOn)
                .ToListAsync();

            if (messages.Count > 0)
            {
                foreach (var msg in messages)
                {
                    msg.ReadOn = (msg.ReadOn==null)? DateTime.Now : msg.ReadOn;
                }
                _context.Message.UpdateRange(messages);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<List<Message>, List<MessageDto>>(messages));
            }
            

            return BadRequest("No Messages found");
        }

        [HttpPost("send")]
        public async Task<ActionResult> sendMessage(MessageDto message)
        {

            var sender = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(u => u.UserName == sender).FirstAsync();
            if (sender == null)
            {
                return BadRequest("Not Authenticated");
            }

            //validate message if needed

            if (user.Id != message.SenderId)
            {
                return BadRequest("Sender Mismatch!!!");
            }

            _context.Message.Add(_mapper.Map<MessageDto, Message>(message));

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Something went east");
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<bool>> deleteMessage([FromQuery]int id)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(u => u.UserName == username).FirstAsync();
            var message = await _context.Message.Where(m => m.Id == id && (m.SenderId == user.Id || m.ReceiverId == user.Id)).FirstOrDefaultAsync();

            if (message == null)
            {
                return BadRequest("not found");
            }

            if (user.Id == message.SenderId)
            {
                message.DeletedBySender = true;
            }
            else if (user.Id == message.ReceiverId)
            {
                message.DeletedByReceiver = true;
            }

            if (message.DeletedByReceiver && message.DeletedBySender)
            {
                _context.Message.Remove(message);
            }
            else
            {
                _context.Message.Update(message);
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }
    }
}
