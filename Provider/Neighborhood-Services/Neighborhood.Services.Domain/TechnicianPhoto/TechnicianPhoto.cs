using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Domain.TechnicianPhoto
{
    public class TechnicianPhoto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }


        public int ApplicationUserId { get; set; }
    }
}
