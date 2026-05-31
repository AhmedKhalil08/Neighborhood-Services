using MediatR;
using Neighborhood.Services.Application.PromoCodes.DTOs;
using Neighborhood.Services.Application.PromoCodes.Interface;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.PromoCodes;
namespace Neighborhood.Services.Application.PromoCodes.Commands.CreatePromoCode
{
    public class CreatePromoCodeCommandHandler : IRequestHandler<CreatePromoCodeCommand, PromoCodeResponseDto>
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePromoCodeCommandHandler(IPromoCodeRepository promoCodeRepository, IUnitOfWork unitOfWork)
        {
            _promoCodeRepository = promoCodeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PromoCodeResponseDto> Handle(CreatePromoCodeCommand request, CancellationToken cancellationToken)
        {
            var exists = await _promoCodeRepository.GetByCodeAsync(request.Code);

            if (exists is not null)
                throw new InvalidOperationException($"Promo Code with code {request.Code} already exists.");

            var promoCode = new PromoCode
            {
                Code = request.Code,
                DiscountPercentage = request.DiscountPercentage,
                MaxUses = request.MaxUses,
                ExpiresAt = request.ExpiresAt,
                IsActive = true,
                UsedCount = 0
            };

            await _promoCodeRepository.AddAsync(promoCode);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PromoCodeResponseDto
            {
                Id = promoCode.Id,
                Code = promoCode.Code,
                DiscountPercentage = promoCode.DiscountPercentage,
                MaxUses = promoCode.MaxUses,
                UsedCount = promoCode.UsedCount,
                ExpiresAt = promoCode.ExpiresAt,
                IsActive = promoCode.IsActive,
                CreatedAt = promoCode.CreatedAt,
            };
        }
    }
}