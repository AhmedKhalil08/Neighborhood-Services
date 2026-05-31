using MediatR;
using Neighborhood.Services.Application.Escrows.DTOs;
using Neighborhood.Services.Application.Escrows.Interfaces;
using Neighborhood.Services.Application.Shared;
namespace Neighborhood.Services.Application.Transactions.Commands.RefundEscrow
{
    public class RefundEscrowCommandHandler : IRequestHandler<RefundEscrowCommand, EscrowResponseDto>
    {
        private readonly IEscrowRepository _escrowRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefundEscrowCommandHandler(IEscrowRepository escrowRepository, IUnitOfWork unitOfWork)
        {
            _escrowRepository = escrowRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EscrowResponseDto> Handle(RefundEscrowCommand request, CancellationToken cancellationToken)
        {
            var escrow = await _escrowRepository.GetByIdAsync(request.EscrowId)
                ?? throw new KeyNotFoundException($"Escrow with ID {request.EscrowId} not found.");

            if (escrow.Status != Domain.Escrows.EscrowStatus.Held)
                throw new InvalidOperationException($"Escrow is not in Held status");

            await _escrowRepository.RefundAsync(request.EscrowId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new EscrowResponseDto
            {
                Id = escrow.Id,
                BookingId = escrow.BookingId,
                WalletId = escrow.WalletId,
                Amount = escrow.Amount,
                Status = Domain.Escrows.EscrowStatus.Refunded,
                HeldAt = escrow.HeldAt,
                ReleasedAt = DateTime.UtcNow
            };
        }
    }
}