using System.Security.Claims;
using API.Data;
using API.Dto;
using API.Entity;
using AutoMapper;
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
        [HttpGet("conversations/{productId}")]
        public async Task<ActionResult<IEnumerable<Conversation>>> getConversations(int productId)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (username == "")
            {
                return BadRequest("Not Authenticated!");
            }

            var user = await _context.Users.Where(x => x.UserName == username).FirstAsync();

            if (user == null)
            {
                return BadRequest("Not found");
            }

            var conversations = await _context.Conversations.Where(x => x.ProductId == productId).ToListAsync();
            if (conversations.Count > 0)
            {
                return Ok(_mapper.Map<List<Conversation>, List<ConversationDto>>(conversations));
            }
            return BadRequest("Unexpected Error!!!");
        }

        [HttpGet("{conversationId}")]
        public async Task<ActionResult<IEnumerable<Conversation>>> getUserMessages(int conversationId)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstAsync();
            if (user == null)
            {
                return BadRequest("User not found!");
            }

            var msg = await _context.Messages.Where(x => x.ConversationId == conversationId).ToListAsync();
            if (msg.Count > 0)
            {
                return Ok(_mapper.Map<List<Message>, List<MessageDto>>(msg));
            }
            return BadRequest("Unexpected Error");
        }

        [HttpPost("conversation/create")]
        public async Task<ActionResult<Conversation>> createConversation([FromBody] ConversationDto conversation)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstAsync();
            if (user == null)
            {
                BadRequest("Not Authenticated");
            }

            if (await _context.Users.AnyAsync(x => x.Id != conversation.SenderId || x.Id != conversation.ReceiverId))
            {
                BadRequest("No Sender/Receipient found");
            }

            if (await _context.Products.AnyAsync(x => x.ProductId != conversation.ProductId))
            {
                BadRequest("No Product found");
            }

            var conv = _mapper.Map<ConversationDto, Conversation>(conversation);
            _context.Conversations.Add(conv);
            if (await _context.SaveChangesAsync() > 0){
                return Ok(_mapper.Map<Conversation, ConversationDto>(conv));
            }
            return BadRequest("Something went east");
        }

        [HttpPost("send")]
        public async Task<ActionResult<bool>> sendMessage([FromBody] MessageDto message)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstAsync();
            if (user == null)
            {
                return BadRequest("No user found!!!");
            }

            if (user.Id == message.SenderId)
            {
                return BadRequest("Not the sender error!");
            }
            _context.Messages.Add(_mapper.Map<MessageDto, Message>(message));

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Message Sent!");
            }
            return BadRequest("Something went east");
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> deleteMessage(int id)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstAsync();
            if (user == null)
            {
                return BadRequest("Authenticate First Sir!");
            }

            var msg = await _context.Messages.Where(x => x.Id == id && x.SenderId == user.Id).FirstAsync();
            if (msg == null)
            {
                return BadRequest("Not your message to be deleted!");
            }

            _context.Messages.Remove(msg);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(true);
            }
            return BadRequest("Something went west!");
        }
    }
}
