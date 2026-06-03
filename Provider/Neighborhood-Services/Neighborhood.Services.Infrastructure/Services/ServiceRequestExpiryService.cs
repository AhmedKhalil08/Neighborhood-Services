using Neighborhood.Services.Application.ServiceRequests.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Offers;
using Neighborhood.Services.Domain.ServiceRequests;

namespace Neighborhood.Services.Infrastructure.Services
{
    public class ServiceRequestExpiryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        public ServiceRequestExpiryService(IUnitOfWork unitOfWork, IServiceRequestRepository serviceRequestRepository)
        {
            _unitOfWork = unitOfWork;
            _serviceRequestRepository = serviceRequestRepository;
        }
        public async Task ExpireOpenRequestAndOffer()
        {
            var today = DateTime.UtcNow;
            var openServices = await _serviceRequestRepository.GetByConditionAsync(s => s.Status == ServiceRequestStatus.Open
                                                                                    && s.ExpiresAt < today
                                                                                    && !s.IsDeleted, includeProperties: "Offers");
            foreach (var Open in openServices)
            {
                Open.Status = ServiceRequestStatus.Expired;
                foreach (var item in Open.Offers.Where(o => o.Status == OfferStatus.Pending))
                {

                    item.Status = OfferStatus.Expired;
                }
            }
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        }
    }
}
