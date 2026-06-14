using MediatR;
using Neighborhood.Services.Application.Exceptions;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Application.Shared.Mappers;
using Neighborhood.Services.Application.Staffs.Interfaces;
using Neighborhood.Services.Application.SupportTickets.Commands;
using Neighborhood.Services.Application.SupportTickets.DTOs;
using Neighborhood.Services.Application.SupportTickets.Interfaces;
using Neighborhood.Services.Domain.SupportTickets;

namespace Neighborhood.Services.Application.SupportTickets.Handlers
{
    public class CreateSupportMessageCommandHandler : IRequestHandler<CreateSupportMessageCommand, SupportMessageDto>
    {
        private readonly ISupportMessageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly ISupportTicketRepository _ticketRepository;
        private readonly IStaffRepository _staffRepository;
        public CreateSupportMessageCommandHandler(ISupportMessageRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser, ISupportTicketRepository ticketRepository , IStaffRepository staffRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _ticketRepository = ticketRepository;
            _staffRepository = staffRepository;
        }

        public async Task<SupportMessageDto> Handle(
      CreateSupportMessageCommand request,
      CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);

            if (ticket is null)
            {
                throw new Exception(
                    $"SupportTicket with id {request.TicketId} not found.");
            }

            if (ticket.Status == SupportTicketStatus.Resolved)
            {
                throw new Exception(
                    "Cannot send messages to a resolved ticket.");
            }

            var senderId = _currentUser.UserId!;

            var message = new SupportMessage
            {
                TicketId = request.TicketId,
                SenderId = senderId,
                Message = request.Message,
                Channel = request.Channel,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _repository.AddAsync(message);

            ticket.UpdatedAt = DateTime.UtcNow;

            // هل المرسل Staff ولا Customer ؟
            var staff = await _staffRepository.GetByUserIdAsync(senderId);

            var isCustomer = staff is null;

            // لو التذكرة WaitingOnCustomer والعميل رد
            if (ticket.Status == SupportTicketStatus.WaitingOnCustomer
                && isCustomer)
            {
                ticket.Status = SupportTicketStatus.InProgress;
            }

            await _ticketRepository.UpdateAsync(ticket);

            await _unitOfWork.SaveChangesAsync();

            return SupportMapper.MapMessageToDto(message);
        }
    }
}