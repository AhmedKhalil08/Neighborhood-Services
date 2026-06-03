using MediatR;
using Neighborhood.Services.Application.Customers.Interfaces;
using Neighborhood.Services.Application.Exceptions;
using Neighborhood.Services.Application.RecurringBookings.DTOs;
using Neighborhood.Services.Application.RecurringBookings.Interfaces;
using Neighborhood.Services.Application.RecurringBookings.Queries.GetRecurringBookingByIdQuery;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Application.Technicians.Interfaces;

namespace Neighborhood.Services.Application.RecurringBookings.Queries.GetMyRecurringBookingsQuery
{
    public class GetMyRecurringBookingsQueryHandler : IRequestHandler<GetMyRecurringBookingsQuery, IEnumerable<RecurringBookingDto>>
    {
        private readonly IRecurringBookingRepository _repository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyRecurringBookingsQueryHandler(
            IRecurringBookingRepository repository,
            ICustomerRepository customerRepository,
            ITechnicianRepository technicianRepository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _customerRepository = customerRepository;
            _technicianRepository = technicianRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<RecurringBookingDto>> Handle(GetMyRecurringBookingsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated.");

            var customer = await _customerRepository.GetByUserIdAsync(userId);
            if (customer != null)
            {
                var items = await _repository.GetCustomerRecurringBookingsAsync(customer.Id);
                return items.Select(GetRecurringBookingByIdQueryHandler.MapToDto);
            }

            var technician = await _technicianRepository.GetByUserIdAsync(userId);
            if (technician != null)
            {
                var items = await _repository.GetTechnicianRecurringBookingsAsync(technician.Id);
                return items.Select(GetRecurringBookingByIdQueryHandler.MapToDto);
            }

            throw new ForbiddenException("User is not a customer or technician.");
        }
    }
}
