using MediatR;
using Neighborhood.Services.Application.AgentLogs.DTOs;
using Neighborhood.Services.Application.AgentLogs.Interfaces;
using Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByTypeQuery;
using Neighborhood.Services.Application.Shared;

namespace Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsPagedQuery
{
    public class GetAgentLogsPagedQueryHandler
        : IRequestHandler<GetAgentLogsPagedQuery, PagedResult<AgentLogDto>>
    {
        private readonly IAgentLogRepository _agentLogRepository;

        public GetAgentLogsPagedQueryHandler(IAgentLogRepository agentLogRepository)
        {
            _agentLogRepository = agentLogRepository;
        }

        public async Task<PagedResult<AgentLogDto>> Handle(
            GetAgentLogsPagedQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize is < 1 or > 100 ? 20 : request.PageSize;

            var (items, total) = await _agentLogRepository.GetPagedByAgentTypeAsync(
                request.AgentType, request.Search, request.From, request.To, page, pageSize);

            // Reuse the existing entity->DTO mapping so the two queries never drift.
            var dtos = items.Select(GetAgentLogsByTypeQueryHandler.MapToDto).ToList();
            return new PagedResult<AgentLogDto>(dtos, total, page, pageSize);
        }
    }
}
