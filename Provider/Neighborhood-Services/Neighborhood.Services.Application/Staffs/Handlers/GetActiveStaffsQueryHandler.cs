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
    public class GetActiveStaffsQueryHandler
       : IRequestHandler<GetActiveStaffsQuery, IReadOnlyList<StaffDto>>
    {
        private readonly IStaffRepository _repository;

        public GetActiveStaffsQueryHandler(IStaffRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<StaffDto>> Handle(
            GetActiveStaffsQuery request,
            CancellationToken cancellationToken)
        {
            var staffs = await _repository.GetActiveAsync(
                cancellationToken);

            return staffs
                .Select(StaffMapper.MapToDto)
                .ToList();
        }
    }
}
