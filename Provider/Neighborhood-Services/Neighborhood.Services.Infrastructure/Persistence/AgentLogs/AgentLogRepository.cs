using Neighborhood.Services.Application.AgentLogs.Interfaces;
using Neighborhood.Services.Domain.AgentLogs;
using Neighborhood.Services.Infrastructure.Persistence.Context;
using Neighborhood.Services.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Infrastructure.Persistence.AgentLogs
{
    public class AgentLogRepository :GenericRepository<AgentLog,int>,IAgentLogRepository
    {
        public AgentLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AgentLog>> GetByAgentTypeAsync(AgentType agentType)
        {
            return await _context.AgentLogs
                .Where(a => a.AgentType == agentType)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgentLog>> GetByReferenceAsync(int referenceId, AgentLogReferenceType referenceType)
        {
            return await _context.AgentLogs
                .Where(a => a.ReferenceId == referenceId
                    && a.ReferenceType == referenceType)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<(IReadOnlyList<AgentLog> Items, int Total)> GetPagedByAgentTypeAsync(
            AgentType agentType, string? search, DateTime? from, DateTime? to, int page, int pageSize)
        {
            var query = _context.AgentLogs
                .AsNoTracking()
                .Where(a => a.AgentType == agentType);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(a =>
                    a.Action.Contains(s) || a.Input.Contains(s) || a.Output.Contains(s));
            }

            if (from.HasValue)
                query = query.Where(a => a.CreatedAt >= from.Value);
            if (to.HasValue)
                query = query.Where(a => a.CreatedAt < to.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

    }
}
