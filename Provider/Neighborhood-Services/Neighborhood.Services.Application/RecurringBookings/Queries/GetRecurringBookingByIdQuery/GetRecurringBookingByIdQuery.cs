using MediatR;
using Neighborhood.Services.Application.RecurringBookings.DTOs;

namespace Neighborhood.Services.Application.RecurringBookings.Queries.GetRecurringBookingByIdQuery
{
    public class GetRecurringBookingByIdQuery : IRequest<RecurringBookingDto>
    {
        public int RecurringBookingId { get; set; }
    }
}
