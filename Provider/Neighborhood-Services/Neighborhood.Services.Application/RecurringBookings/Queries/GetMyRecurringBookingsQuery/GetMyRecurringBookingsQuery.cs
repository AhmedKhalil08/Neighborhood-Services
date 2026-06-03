using MediatR;
using Neighborhood.Services.Application.RecurringBookings.DTOs;

namespace Neighborhood.Services.Application.RecurringBookings.Queries.GetMyRecurringBookingsQuery
{
    // Returns the authenticated user's recurring bookings (customer or technician)
    public class GetMyRecurringBookingsQuery : IRequest<IEnumerable<RecurringBookingDto>>
    {
    }
}
