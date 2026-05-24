using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using System.Text;

namespace Neighborhood.Services.Domain.ApplicationUser
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password_Hash { get; set; } = string.Empty;
        public ApplicationUserRole ApplicationUserRole { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public Point Location { get; set; } = null!;
        public string RefferalCode { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
