using MediatR;
using Neighborhood.Services.Application.Bookings.Interface;
using Neighborhood.Services.Application.Escrows.Commands.ReleaseEscrow;
using Neighborhood.Services.Application.Escrows.Interfaces;
using Neighborhood.Services.Application.Exceptions;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Bookings;
using Neighborhood.Services.Domain.Escrows;

namespace Neighborhood.Services.Application.Bookings.Commands.ConfirmBookingCommands
{
    public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IEscrowRepository _escrowRepository;

        public ConfirmBookingCommandHandler(
            IBookingRepository bookingRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IEscrowRepository escrowRepository)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _escrowRepository = escrowRepository;
        }

        public async Task<bool> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            if (booking is null)
                throw new NotFoundException(nameof(Booking), request.BookingId);

            if (booking.Status != BookingStatus.Completed)
                throw new BadRequestException("Booking can only be confirmed by the client after it is marked Completed by the technician.");

            if (booking.ClientConfirmed)
                throw new BadRequestException("Booking has already been confirmed by the client.");
            // TODO: Authorization check once current user service is ready
            // Only the customer who created this booking can confirm it
            // booking.Customer.UserId == requestingUserId

            booking.ClientConfirmed = true;
            booking.ConfirmedAt = DateTime.UtcNow;
            booking.UpdatedAt = DateTime.UtcNow;

            var escrow = await _escrowRepository.GetByBookingIdAsync(booking.Id);
            if (escrow is not null && escrow.Status == EscrowStatus.Held)
                await _mediator.Send(new ReleaseEscrowCommand { EscrowId = escrow.Id }, cancellationToken);

            await _bookingRepository.UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
