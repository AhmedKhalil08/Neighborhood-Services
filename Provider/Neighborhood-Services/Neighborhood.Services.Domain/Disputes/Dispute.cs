using Neighborhood.Services.Domain.ApplicationUsers;
using Neighborhood.Services.Domain.Bookings;
using Neighborhood.Services.Domain.Shared;
using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.Domain.Disputes;

public class Dispute : BaseEntity<int>
{

    public int? ResolvedByStaffId { get; set; }

    public DisputeType DisputeType { get; set; }

    public string Reason { get; set; } = null!;

    public string? Resolution { get; set; }

    public DisputeStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }


    

    //navigation prop
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public string RaisedByUserId { get; set; } = null!;
    public ApplicationUser RaisedByUser { get; set; } = null!;

    public Staff? ResolvedByStaff { get; set; }

    // Empty Constructor For EF
    public Dispute()
    {
    }

}