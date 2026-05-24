namespace Neighborhood.Services.Application.Staffs.DTOs;

public class StaffDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Role { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}