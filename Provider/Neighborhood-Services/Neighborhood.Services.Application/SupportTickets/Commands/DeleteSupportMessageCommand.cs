using MediatR;

namespace Neighborhood.Services.Application.SupportTickets.Commands
{
    public class DeleteSupportMessageCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
