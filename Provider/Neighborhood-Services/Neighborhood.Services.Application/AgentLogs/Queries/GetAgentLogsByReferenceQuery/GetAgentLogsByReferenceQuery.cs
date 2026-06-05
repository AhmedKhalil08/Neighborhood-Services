using MediatR;
using Neighborhood.Services.Application.AgentLogs.DTOs;
using Neighborhood.Services.Domain.AgentLogs;

namespace Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByReferenceQuery
{
    public class GetAgentLogsByReferenceQuery : IRequest<IEnumerable<AgentLogDto>>
    {
        public int ReferenceId { get; set; }
        public AgentLogReferenceType ReferenceType { get; set; }
    }
}
