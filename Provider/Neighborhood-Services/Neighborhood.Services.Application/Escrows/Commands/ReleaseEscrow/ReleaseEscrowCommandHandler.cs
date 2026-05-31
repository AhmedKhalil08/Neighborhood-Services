using MediatR;
using Neighborhood.Services.Application.Escrows.DTOs;
using Neighborhood.Services.Application.Escrows.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Escrows;
namespace Neighborhood.Services.Application.Escrows.Commands.ReleaseEscrow
{
    public class ReleaseEscrowCommandHandler : IRequestHandler<ReleaseEscrowCommand, EscrowResponseDto>
    {
        private readonly IEscrowRepository _escrowRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ReleaseEscrowCommandHandler(IEscrowRepository escrowRepository, IUnitOfWork unitOfWork)
        {
            _escrowRepository = escrowRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<EscrowResponseDto> Handle(ReleaseEscrowCommand request, CancellationToken cancellationToken)
        {
            var escrow = await _escrowRepository.GetByIdAsync(request.EscrowId)
                ?? throw new KeyNotFoundException($"Escrow with ID {request.EscrowId} not found.");

            if (escrow.Status != EscrowStatus.Held)
                throw new InvalidOperationException($"Escrow is not in Held status");

            await _escrowRepository.ReleaseAsync(request.EscrowId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new EscrowResponseDto
            {
                Id = escrow.Id,
                BookingId = escrow.BookingId,
                WalletId = escrow.WalletId,
                Amount = escrow.Amount,
                Status = EscrowStatus.Released,
                HeldAt = escrow.HeldAt,
                ReleasedAt = DateTime.UtcNow
            };
        }
    }
}