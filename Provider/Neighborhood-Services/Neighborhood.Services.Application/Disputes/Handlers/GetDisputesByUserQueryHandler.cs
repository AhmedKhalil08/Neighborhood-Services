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
    public class GetDisputesByUserQueryHandler
     : IRequestHandler<GetDisputesByUserQuery, IReadOnlyList<DisputeDto>>
    {
        private readonly IDisputeRepository _repository;

        public GetDisputesByUserQueryHandler(IDisputeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<DisputeDto>> Handle(
            GetDisputesByUserQuery request,
            CancellationToken cancellationToken)
        {
            var disputes = await _repository.GetByRaisedByAsync(
                request.UserId,
                cancellationToken);

            return disputes
                .Select(DisputeMapper.MapToDto)
                .ToList();
        }
    }
}
