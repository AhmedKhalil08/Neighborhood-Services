using MediatR;
using Neighborhood.Services.Application.AgentLogs.DTOs;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.AgentLogs;

namespace Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsPagedQuery
{
    // Paged + filtered agent logs for one agent type — backs the admin "Agent Logs" viewer.
    public class GetAgentLogsPagedQuery : IRequest<PagedResult<AgentLogDto>>
    {
        public AgentType AgentType { get; set; }
        public string? Search { get; set; }      // matches action / input / output
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
