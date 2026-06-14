using MediatR;
using Neighborhood.Services.Application.Disputes.DTOs;
using Neighborhood.Services.Application.Disputes.Interfaces;
using Neighborhood.Services.Application.Disputes.Queries;
using Neighborhood.Services.Application.Shared.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Disputes.Handlers
{
    public class GetDisputesByStatusQueryHandler
    : IRequestHandler<GetDisputesByStatusQuery, IReadOnlyList<DisputeDto>>
    {
        private readonly IDisputeRepository _repository;

        public GetDisputesByStatusQueryHandler(IDisputeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<DisputeDto>> Handle(
            GetDisputesByStatusQuery request,
            CancellationToken cancellationToken)
        {
            var disputes = await _repository.GetByStatusAsync(
                request.Status,
                cancellationToken);

            return disputes
                .Select(DisputeMapper.MapToDto)
                .ToList();
        }
    }
}
