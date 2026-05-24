using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Domain.Customer
{
    public class Customer
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public int ApplicationUserId { get; set; }
    }
}
