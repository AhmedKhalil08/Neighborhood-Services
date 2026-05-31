using MediatR;
using Neighborhood.Services.Application.Escrows.DTOs;
using Neighborhood.Services.Application.Escrows.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Escrows;
namespace Neighborhood.Services.Application.Escrows.Commands.CreateEscrow
{
    public class CreateEscrowCommandHandler : IRequestHandler<CreateEscrowCommand, EscrowResponseDto>
    {
        private readonly IEscrowRepository _escrowRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEscrowCommandHandler(IEscrowRepository escrowRepository, IUnitOfWork unitOfWork)
        {
            _escrowRepository = escrowRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EscrowResponseDto> Handle(CreateEscrowCommand request, CancellationToken cancellationToken)
        {
            var existing = await _escrowRepository.GetByBookingIdAsync(request.BookingId);

            if (existing is not null)
                throw new InvalidOperationException($"Escrow for booking {request.BookingId} already exists.");

            var escrow = new Escrow
            {
                BookingId = request.BookingId,
                WalletId = request.WalletId,
                Amount = request.Amount,
                Status = EscrowStatus.Held,
                HeldAt = DateTime.UtcNow
            };

            await _escrowRepository.AddAsync(escrow);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new EscrowResponseDto
            {
                Id = escrow.Id,
                BookingId = escrow.BookingId,
                WalletId = escrow.WalletId,
                Amount = escrow.Amount,
                Status = escrow.Status,
                HeldAt = escrow.HeldAt,
                ReleasedAt = escrow.ReleasedAt
            };
        }
    }
}