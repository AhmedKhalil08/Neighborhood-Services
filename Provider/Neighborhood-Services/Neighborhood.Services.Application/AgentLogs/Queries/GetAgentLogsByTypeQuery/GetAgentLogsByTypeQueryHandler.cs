using MediatR;
using Neighborhood.Services.Application.AgentLogs.DTOs;
using Neighborhood.Services.Application.AgentLogs.Interfaces;

namespace Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByTypeQuery
{
    public class GetAgentLogsByTypeQueryHandler : IRequestHandler<GetAgentLogsByTypeQuery, IEnumerable<AgentLogDto>>
    {
        private readonly IAgentLogRepository _agentLogRepository;

        public GetAgentLogsByTypeQueryHandler(IAgentLogRepository agentLogRepository)
        {
            _agentLogRepository = agentLogRepository;
        }

        public async Task<IEnumerable<AgentLogDto>> Handle(GetAgentLogsByTypeQuery request, CancellationToken cancellationToken)
        {
            var logs = await _agentLogRepository.GetByAgentTypeAsync(request.AgentType);
            return logs.Select(MapToDto);
        }

        internal static AgentLogDto MapToDto(Domain.AgentLogs.AgentLog log) => new()
        {
            Id = log.Id,
            AgentType = log.AgentType,
            Action = log.Action,
            Input = log.Input,
            Output = log.Output,
            ReferenceType = log.ReferenceType,
            ReferenceId = log.ReferenceId,
            CreatedAt = log.CreatedAt
        };
    }
}
