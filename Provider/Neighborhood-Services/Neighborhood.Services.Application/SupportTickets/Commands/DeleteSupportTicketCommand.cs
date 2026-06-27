using MediatR;

namespace Neighborhood.Services.Application.SupportTickets.Commands
{
    public class DeleteSupportTicketCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
