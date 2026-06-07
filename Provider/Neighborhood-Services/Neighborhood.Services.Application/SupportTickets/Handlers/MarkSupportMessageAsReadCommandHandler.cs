using MediatR;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Application.Shared.Mappers;
using Neighborhood.Services.Application.SupportTickets.Commands;
using Neighborhood.Services.Application.SupportTickets.DTOs;
using Neighborhood.Services.Application.SupportTickets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.SupportTickets.Handlers
{
    public class MarkSupportMessageAsReadCommandHandler : IRequestHandler<MarkSupportMessageAsReadCommand, SupportMessageDto>
    {
        private readonly ISupportMessageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkSupportMessageAsReadCommandHandler(ISupportMessageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SupportMessageDto> Handle(
    MarkSupportMessageAsReadCommand request,
    CancellationToken cancellationToken)
        {
            var message = await _repository.GetByIdAsync(request.MessageId);

            if (message is null)
            {
                throw new Exception(
                    $"SupportMessage with id {request.MessageId} not found.");
            }

            if (message.ReadAt is null)
            {
                message.ReadAt = DateTime.UtcNow;

                await _repository.UpdateAsync(message);

                await _unitOfWork.SaveChangesAsync();
            }

            return SupportMapper.MapMessageToDto(message);
        }
    }
}
