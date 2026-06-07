using MediatR;
using Neighborhood.Services.Application.Shared.Mappers;
using Neighborhood.Services.Application.Staffs.DTOs;
using Neighborhood.Services.Application.Staffs.Interfaces;
using Neighborhood.Services.Application.Staffs.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Staffs.Handlers
{
    public class GetStaffsByRoleQueryHandler
        : IRequestHandler<GetStaffsByRoleQuery, IReadOnlyList<StaffDto>>
    {
        private readonly IStaffRepository _repository;

        public GetStaffsByRoleQueryHandler(IStaffRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<StaffDto>> Handle(
            GetStaffsByRoleQuery request,
            CancellationToken cancellationToken)
        {
            var staffs = await _repository.GetByRoleAsync(
                request.Role,
                cancellationToken);

            return staffs
                .Select(StaffMapper.MapToDto)
                .ToList();
        }
    }
}
