using Neighborhood.Services.Domain.AgentLogs;

namespace Neighborhood.Services.Application.AgentLogs.DTOs
{
    public class AgentLogDto
    {
        public int Id { get; set; }
        public AgentType AgentType { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Input { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public AgentLogReferenceType ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
