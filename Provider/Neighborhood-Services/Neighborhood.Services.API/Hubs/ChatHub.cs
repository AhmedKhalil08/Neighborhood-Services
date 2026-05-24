using MediatR;
using Microsoft.AspNetCore.SignalR;
using Neighborhood.Services.Application.Modules.Messages;
using Neighborhood.Services.Application.Modules.Messages.Commands;
using Neighborhood.Services.Application.Modules.Messages.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Neighborhood.Services.API.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IMessageRepository _messageRepo;
        private IMediator _mediator;

        public ChatHub(IMessageRepository messageRepo, IMediator mediator)
        {
            _messageRepo = messageRepo;
            _mediator = mediator;
        }
        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }
        public async Task SendMessage( int roomId,string message)
        {
            var userId = Context.UserIdentifier;

            MessageCreatedDto savedMessage = await _mediator.Send(new CreateMessageCommand() {
            SenderId =int.Parse(userId) ,ConversationId=roomId,content=message});
                   

            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage",savedMessage);
        }
    }
}

