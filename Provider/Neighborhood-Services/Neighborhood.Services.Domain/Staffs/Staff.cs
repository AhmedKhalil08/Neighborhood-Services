namespace Neighborhood.Services.Domain.Staffs;

public class Staff
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public StaffRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public int? CreatedByStaffId { get; private set; }

    public DateTime CreatedAt { get; private set; }


    // Navigation Property
    public ICollection<StaffPermission> Permissions { get; private set; }


    // Business Methods

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}