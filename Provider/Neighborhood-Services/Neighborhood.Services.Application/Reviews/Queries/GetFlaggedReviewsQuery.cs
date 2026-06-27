using MediatR;
using Neighborhood.Services.Application.Reviews.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Reviews.Queries
{
    public record GetFlaggedReviewsQuery(): IRequest<IReadOnlyList<ReviewDto>>;
}
