using Microsoft.AspNetCore.Authorization;
using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.Application.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionType Permission { get; }

        public PermissionRequirement(PermissionType permission)
        {
            Permission = permission;
        }
    }
}
