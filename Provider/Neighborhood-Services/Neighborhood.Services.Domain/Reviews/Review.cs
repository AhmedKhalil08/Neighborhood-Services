using Neighborhood.Services.Domain.ApplicationUsers;
using Neighborhood.Services.Domain.Bookings;
using Neighborhood.Services.Domain.Customers;
using Neighborhood.Services.Domain.Shared;
using Neighborhood.Services.Domain.Technicians;

namespace Neighborhood.Services.Domain.Reviews
{
    public class Review : BaseEntity<int>
    {
        public int BookingId { get; set; }

        public string ReviewerId { get; set; } = null!;

        public string RevieweeId { get; set; } = null!;

        public int Rating { get; set; }

        public string Comment { get; set; } = null!;

        public ReviewStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public ReviewType ReviewType { get; set; }
        public ApplicationUser Reviewer { get; set; } = null!;

        public ApplicationUser Reviewee { get; set; } = null!;

        public Booking Booking { get; set; } = null!;

        public ReviewAnalysis? Analysis { get; set; }
    }
}