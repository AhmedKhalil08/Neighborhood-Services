using Microsoft.Identity.Client;
using Neighborhood.Services.Domain.Shared;
using Neighborhood.Services.Domain.ApplicationUser;
using Neighborhood.Services.Domain.Technicians;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Domain.favorites
{
    public class Favorites :BaseEntity<int>
    {
        public string UserId { set; get; }
        public int TechnicianId { set; get; }
        public DateTime addedAt { get; } = DateTime.Now;

        //Nav probs
        public ApplicationUser.ApplicationUser User { get; set; } = null;
        //public User
        //public Technician
        public Technician Technician { get; set; } = null;

    }
}
