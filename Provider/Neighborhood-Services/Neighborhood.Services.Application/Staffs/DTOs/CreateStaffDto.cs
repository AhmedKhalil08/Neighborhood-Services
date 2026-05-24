namespace Neighborhood.Services.Application.Staffs.DTOs;

public class CreateStaffDto
{
    public int UserId { get; set; }

    public int CreatedByStaffId { get; set; }

    public int Role { get; set; }
}