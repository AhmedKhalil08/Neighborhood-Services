using MediatR;
using Neighborhood.Services.Application.Exceptions;
using Neighborhood.Services.Application.ServiceRequests.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.ServiceRequests;
using NetTopologySuite.Geometries;

namespace Neighborhood.Services.Application.ServiceRequests.Commands.CreateService
{
    public class CreateServiceRequestCommandHandler : IRequestHandler<CreateServiceRequestCommand, int>
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateServiceRequestCommandHandler(IServiceRequestRepository serviceRequestRepository, IUnitOfWork unitOfWork)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken)
        {
            // Budget must be positive
            if (request.Budget <= 0)
                throw new ValidationException("Budget must be greater than zero");

            // Description must not be empty
            if (string.IsNullOrWhiteSpace(request.Description))
                throw new BadRequestException("Description is required");

            // Desired service time must be in the future
            if (request.ScheduledAt <= DateTime.UtcNow)
                throw new ValidationException("Scheduled time cannot be in the past");

            var serviceRequest = new ServiceRequest
            {
                CustomerId = request.CustomerId,
                CategoryId = request.CategoryId,
                ProblemTypeId = request.ProblemTypeId,
                Description = request.Description,
                Address = request.Address,
                Budget = request.Budget,
                Image = request.Image,
                ScheduledAt = request.ScheduledAt,
                Status = ServiceRequestStatus.Open,
                Location = new Point(request.Longitude, request.Latitude) { SRID = 4326 },
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _serviceRequestRepository.AddAsync(serviceRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return serviceRequest.Id;
        }
    }
}
