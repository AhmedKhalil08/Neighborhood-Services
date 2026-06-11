using MediatR;
using Neighborhood.Services.Application.Exceptions;
using Neighborhood.Services.Application.ServiceRequests.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.ServiceRequests;

namespace Neighborhood.Services.Application.ServiceRequests.Commands.ReviewFlaggedServiceRequest
{
    public class ReviewFlaggedServiceRequestCommandHandler : IRequestHandler<ReviewFlaggedServiceRequestCommand, bool>
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewFlaggedServiceRequestCommandHandler(
            IServiceRequestRepository serviceRequestRepository,
            IUnitOfWork unitOfWork)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ReviewFlaggedServiceRequestCommand request, CancellationToken cancellationToken)
        {
            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(request.ServiceRequestId)
                ?? throw new NotFoundException("ServiceRequest", request.ServiceRequestId);

            // Only flagged requests can be reviewed — guards against double-handling.
            if (serviceRequest.Status != ServiceRequestStatus.Flagged)
                throw new BadRequestException("Only flagged requests can be reviewed.");

            serviceRequest.Status = request.Approved
                ? ServiceRequestStatus.Open
                : ServiceRequestStatus.Closed;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return request.Approved;
        }
    }
}
