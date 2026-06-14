using Microsoft.AspNetCore.Authorization;
using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.Application.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(PermissionType permission)
        {
            Policy = $"Permission:{permission}";
        }
    }
}
