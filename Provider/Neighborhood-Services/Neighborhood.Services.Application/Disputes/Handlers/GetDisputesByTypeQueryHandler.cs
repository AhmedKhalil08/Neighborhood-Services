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
    public class GetDisputesByTypeQueryHandler
      : IRequestHandler<GetDisputesByTypeQuery, IReadOnlyList<DisputeDto>>
    {
        private readonly IDisputeRepository _repository;

        public GetDisputesByTypeQueryHandler(IDisputeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<DisputeDto>> Handle(
            GetDisputesByTypeQuery request,
            CancellationToken cancellationToken)
        {
            var disputes = await _repository.GetByTypeAsync(
                request.Type,
                cancellationToken);

            return disputes
                .Select(DisputeMapper.MapToDto)
                .ToList();
        }
    }
}
