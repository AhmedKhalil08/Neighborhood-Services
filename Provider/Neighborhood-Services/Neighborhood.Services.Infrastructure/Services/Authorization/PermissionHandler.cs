using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Neighborhood.Services.Application.Authorization;
using Neighborhood.Services.Application.Staffs.Interfaces;
using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.Infrastructure.Services.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IStaffRepository _staffRepository;

        public PermissionHandler(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return;

            var staff = await _staffRepository.GetByUserIdAsync(userId);

            if (staff == null)
                return;

            var hasPermission = await _staffRepository.HasPermissionAsync(
                staff.Id,
                requirement.Permission);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}