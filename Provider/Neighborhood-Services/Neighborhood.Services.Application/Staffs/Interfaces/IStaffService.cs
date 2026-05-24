using Neighborhood.Services.Application.Staffs.DTOs;

namespace Neighborhood.Services.Application.Staffs.Interfaces;

public interface IStaffService
{
    Task CreateAsync(CreateStaffDto dto);

    Task<List<StaffDto>> GetAllAsync();

    Task<StaffDto?> GetByIdAsync(int id);

    Task ActivateAsync(int id);

    Task DeactivateAsync(int id);
}