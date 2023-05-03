using System.Security.Claims;
using API.Data;
using API.Dto;
using API.Entity;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ChatHub(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            int conversation = Int32.Parse(httpContext.Request.Query["conversationId"]);
            await Groups.AddToGroupAsync(Context.ConnectionId, conversation.ToString());

            var messages = await _context.Messages.Where(x => x.ConversationId == conversation).ToListAsync();

            await Clients.Group(conversation.ToString()).SendAsync("MessageReceived", messages);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(MessageDto message)
        {
            
            var user = await _context.Users.Where(x => x.Id == message.SenderId).FirstAsync();
            if (user == null)
            {
                throw new HubException("No user found!!!");
            }

            if (user.Id != message.SenderId)
            {
                throw new HubException("Not the sender error!");
            }
            _context.Messages.Add(_mapper.Map<MessageDto, Message>(message));

            if (await _context.SaveChangesAsync() > 0)
            {
                await Clients.Group(message.ConversationId.ToString()).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }
    }
}