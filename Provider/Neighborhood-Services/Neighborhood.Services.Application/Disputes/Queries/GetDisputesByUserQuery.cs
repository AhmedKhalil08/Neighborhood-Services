using MediatR;
using Neighborhood.Services.Application.Disputes.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Disputes.Queries
{
    public class GetDisputesByUserQuery : IRequest<IReadOnlyList<DisputeDto>>
    {
        public string UserId { get; set; }
    }
}
