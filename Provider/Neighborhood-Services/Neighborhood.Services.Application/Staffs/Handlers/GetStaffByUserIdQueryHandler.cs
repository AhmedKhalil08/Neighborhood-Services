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
    public class GetStaffByUserIdQueryHandler
        : IRequestHandler<GetStaffByUserIdQuery, StaffDto>
    {
        private readonly IStaffRepository _repository;

        public GetStaffByUserIdQueryHandler(IStaffRepository repository)
        {
            _repository = repository;
        }

        public async Task<StaffDto> Handle(
            GetStaffByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var staff = await _repository.GetByUserIdAsync(
                request.UserId,
                cancellationToken);

            if (staff is null)
                throw new Exception(
                    $"Staff with user id {request.UserId} not found.");

            return StaffMapper.MapToDto(staff);
        }
    }
}
