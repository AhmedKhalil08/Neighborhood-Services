using MediatR;
using Neighborhood.Services.Application.AgentLogs.DTOs;
using Neighborhood.Services.Application.AgentLogs.Interfaces;
using Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByTypeQuery;

namespace Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByReferenceQuery
{
    public class GetAgentLogsByReferenceQueryHandler : IRequestHandler<GetAgentLogsByReferenceQuery, IEnumerable<AgentLogDto>>
    {
        private readonly IAgentLogRepository _agentLogRepository;

        public GetAgentLogsByReferenceQueryHandler(IAgentLogRepository agentLogRepository)
        {
            _agentLogRepository = agentLogRepository;
        }

        public async Task<IEnumerable<AgentLogDto>> Handle(GetAgentLogsByReferenceQuery request, CancellationToken cancellationToken)
        {
            var logs = await _agentLogRepository.GetByReferenceAsync(request.ReferenceId, request.ReferenceType);
            return logs.Select(GetAgentLogsByTypeQueryHandler.MapToDto);
        }
    }
}
