using MediatR;
using Neighborhood.Services.Application.PromoCodes.DTOs;
using Neighborhood.Services.Application.PromoCodes.Interface;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.PromoCodes;
namespace Neighborhood.Services.Application.PromoCodes.Commands.ApplyPromoCode
{
    public class ApplyPromoCodeCommandHandler : IRequestHandler<ApplyPromoCodeCommand, PromoCodeResponseDto>
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IPromoCodeUsageRepository _promoCodeUsageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApplyPromoCodeCommandHandler(IPromoCodeRepository promoCodeRepository, IPromoCodeUsageRepository promoCodeUsageRepository, IUnitOfWork unitOfWork)
        {
            _promoCodeRepository = promoCodeRepository;
            _promoCodeUsageRepository = promoCodeUsageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PromoCodeResponseDto> Handle(ApplyPromoCodeCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _promoCodeRepository.IsValidAsync(request.Code);

            if (!isValid)
                throw new InvalidOperationException($"Promo code '{request.Code}' is invalid or expired.");

            var promoCode = await _promoCodeRepository.GetByCodeAsync(request.Code)
                ?? throw new KeyNotFoundException($"Promo code '{request.Code}' not found.");

            var alreadyUsed = await _promoCodeUsageRepository.HasUserUsedPromoAsync(request.UserId, promoCode.Id);

            if (alreadyUsed)
                throw new InvalidOperationException($"Your have already used this promo code.");

            var usage = new PromoCodeUsage
            {
                PromoCodeId = promoCode.Id,
                UserId = request.UserId,
                BookingId = request.BookingId,
                UsedAt = DateTime.UtcNow
            };

            await _promoCodeUsageRepository.AddAsync(usage);
            await _promoCodeRepository.IncrementUsageAsync(promoCode.Id);
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
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}