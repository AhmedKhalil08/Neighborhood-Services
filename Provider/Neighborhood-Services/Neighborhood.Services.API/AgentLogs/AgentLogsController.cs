using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByReferenceQuery;
using Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsByTypeQuery;
using Neighborhood.Services.Application.AgentLogs.Queries.GetAgentLogsPagedQuery;
using Neighborhood.Services.Application.Authorization;
using Neighborhood.Services.Domain.AgentLogs;
using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.API.AgentLogs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class AgentLogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AgentLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/agentlogs?type=Chatbot&search=&from=&to=&page=1&pageSize=20
        // Paged + filtered logs for one agent type — the admin "Agent Logs" viewer. Full-access only.
        [HttpGet]
        [HasPermission(PermissionType.FullAccess)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] AgentType type,
            [FromQuery] string? search,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAgentLogsPagedQuery
            {
                AgentType = type,
                Search = search,
                From = from,
                To = to,
                Page = page,
                PageSize = pageSize
            });
            return Ok(result);
        }

        // GET /api/agentlogs/type/{agentType}  (admin: all logs for a specific agent)
        [HttpGet("type/{agentType}")]
        public async Task<IActionResult> GetByType(AgentType agentType)
        {
            var result = await _mediator.Send(new GetAgentLogsByTypeQuery { AgentType = agentType });
            return Ok(result);
        }

        // GET /api/agentlogs/reference/{referenceType}/{referenceId}
        // e.g. GET /api/agentlogs/reference/Booking/42 → all agent activity on Booking #42
        [HttpGet("reference/{referenceType}/{referenceId:int}")]
        public async Task<IActionResult> GetByReference(AgentLogReferenceType referenceType, int referenceId)
        {
            var result = await _mediator.Send(new GetAgentLogsByReferenceQuery
            {
                ReferenceType = referenceType,
                ReferenceId = referenceId
            });
            return Ok(result);
        }
    }
}
