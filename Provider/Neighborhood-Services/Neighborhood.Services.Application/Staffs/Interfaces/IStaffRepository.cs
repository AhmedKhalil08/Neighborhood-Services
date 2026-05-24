

using Neighborhood.Services.Domain.Staffs;

namespace Neighborhood.Services.Application.Staffs.Interfaces;

public interface IStaffRepository
{
    Task AddAsync(Staff staff);

    Task<Staff?> GetByIdAsync(int id);

    Task<List<Staff>> GetAllAsync();

    Task SaveChangesAsync();
}