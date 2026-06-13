using MediatR;
using Neighborhood.Services.Application.Exceptions;
using Neighborhood.Services.Application.TechnitianPricing.DTOs;
using Neighborhood.Services.Application.TechnitianPricing.Interface;

namespace Neighborhood.Services.Application.TechnitianPricing.Queries
{
    public class GetPricingForProblemTypeQueryHandler : IRequestHandler<GetPricingForProblemTypeQuery, IReadOnlyList<TechnicianPricingDto>>
    {
        private readonly ITechnicianPricingRepository _technicianPricingRepo;

        public GetPricingForProblemTypeQueryHandler(ITechnicianPricingRepository technicianPricingRepo)
        {
            _technicianPricingRepo = technicianPricingRepo;
        }

        public async Task<IReadOnlyList<TechnicianPricingDto>> Handle(GetPricingForProblemTypeQuery request, CancellationToken cancellationToken)
        {

            var lang = request.Lang.ToLower();
            var pricing = await _technicianPricingRepo.GetByConditionAsync(TP => (!TP.IsDeleted)  &&  TP.TechnicianId == request.TechnicianId, "ProblemType,Technician");

            if (pricing == null || !pricing.Any())
            {
                throw new ValidationException("No pricing found for this technician.");
            }

            return pricing
                .Select(TP => new TechnicianPricingDto
                {
                    Id = TP.Id,
                    NationalId = TP.Technician.NationalId,
                    Experience  = TP.Technician.Experience,
                    Rating = TP.Technician.Rating,
                    MaxTravelDistance =TP.Technician.MaxTravelDistance,
                    VerificationStatus = TP.Technician.VerificationStatus,
                    ProblemTypeId = TP.ProblemTypeId,
                    ProblemTypeDescription = lang == "en" ? TP.ProblemType.DescriptionEn : TP.ProblemType.DescriptionAr,
                    ProblemTypeName = lang == "en" ?  TP.ProblemType.NameEn : TP.ProblemType.NameAr,
                    TechMaxPrice = TP.MaxPrice,
                    TechMinPrice = TP.MinPrice ,
                })
                .ToList();
        }
    }
}
