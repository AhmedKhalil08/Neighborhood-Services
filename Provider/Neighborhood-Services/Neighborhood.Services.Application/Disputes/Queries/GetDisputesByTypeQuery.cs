using MediatR;
using Neighborhood.Services.Application.Disputes.DTOs;
using Neighborhood.Services.Domain.Disputes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Disputes.Queries
{
    public class GetDisputesByTypeQuery : IRequest<IReadOnlyList<DisputeDto>>
    {
        public DisputeType Type { get; set; }
    }
}
