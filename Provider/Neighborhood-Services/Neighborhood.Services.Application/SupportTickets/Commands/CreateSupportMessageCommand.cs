using MediatR;
using Neighborhood.Services.Application.SupportTickets.DTOs;
using Neighborhood.Services.Domain.SupportTickets;

namespace Neighborhood.Services.Application.SupportTickets.Commands
{
    public class CreateSupportMessageCommand : IRequest<SupportMessageDto>
    {
        public int TicketId { get; set; }
        // public string SenderId { get; set; }
        public string Message { get; set; }

       // public string? email { set; get; }
        public MessageChannel Channel { get; set; } // is enum (Chat = 1, Mail = 2)
    }
}
