using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.Application.Staffs.Interfaces
{
    public interface IStaffRepository:IGenericRepository<Staff,int>
    {
        // ── Queries ────────────────────────────────────────────────────────────
        Task<Staff?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Staff?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Staff>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Staff>> GetByRoleAsync(StaffRole role, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Staff>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> HasPermissionAsync(int staffId, PermissionType permission, CancellationToken cancellationToken = default);

        // ── Commands ───────────────────────────────────────────────────────────
       
        Task ReplacePermissionsAsync(int staffId,IEnumerable<PermissionType> permissions,CancellationToken cancellationToken = default);
    }
}
