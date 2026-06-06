using MediatR;
using Neighborhood.Services.Application.AgentLogs.DTOs;
using Neighborhood.Services.Domain.AgentLogs;

namespace Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByTypeQuery
{
    public class GetAgentLogsByTypeQuery : IRequest<IEnumerable<AgentLogDto>>
    {
        public AgentType AgentType { get; set; }
    }
}
