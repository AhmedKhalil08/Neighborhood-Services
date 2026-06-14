using MediatR;
using Neighborhood.Services.Application.Disputes.Commands;
using Neighborhood.Services.Application.Disputes.DTOs;
using Neighborhood.Services.Application.Disputes.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Disputes;
using Neighborhood.Services.Application.Shared.Mappers;

namespace Neighborhood.Services.Application.Disputes.Handlers
{
    public class UpdateDisputeCommandHandler
     : IRequestHandler<UpdateDisputeCommand, DisputeDto>
    {
        private readonly IDisputeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDisputeCommandHandler(
            IDisputeRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DisputeDto> Handle(
            UpdateDisputeCommand request,
            CancellationToken cancellationToken)
        {
            var dispute = await _repository.GetByIdAsync(request.Id);

            if (dispute is null)
                throw new Exception($"Dispute with id {request.Id} not found.");

            // 1. Validate status transition
            if (!IsValidTransition(dispute.Status, request.Status))
            {
                throw new Exception(
                    $"Invalid dispute status transition from {dispute.Status} to {request.Status}");
            }

            // 2. Apply status
            dispute.Status = request.Status;

            // 3. Optional resolution
            if (!string.IsNullOrWhiteSpace(request.Resolution))
            {
                dispute.Resolution = request.Resolution;
            }

            // 4. When resolving → enforce rules
            if (request.Status == DisputeStatus.Resolved)
            {
                if (!request.ResolvedByStaffId.HasValue)
                    throw new Exception("ResolvedByStaffId is required when resolving a dispute.");

                if (string.IsNullOrWhiteSpace(request.Resolution))
                    throw new Exception("Resolution is required when resolving a dispute.");

                dispute.ResolvedByStaffId = request.ResolvedByStaffId;
                dispute.ResolvedAt = DateTime.UtcNow;
            }

            // 5. Save
            await _repository.UpdateAsync(dispute);
            await _unitOfWork.SaveChangesAsync();

            return DisputeMapper.MapToDto(dispute);
        }

        // 🔥 Business Rule: valid status transitions only
        private static bool IsValidTransition(
            DisputeStatus currentStatus,
            DisputeStatus newStatus)
        {
            if (currentStatus == newStatus)
                return true;

            return currentStatus switch
            {
                DisputeStatus.Open =>
                    newStatus == DisputeStatus.UnderReview,

                DisputeStatus.UnderReview =>
                    newStatus == DisputeStatus.Resolved,

                DisputeStatus.Resolved =>
                    false,

                _ => false
            };
        }
    }
}