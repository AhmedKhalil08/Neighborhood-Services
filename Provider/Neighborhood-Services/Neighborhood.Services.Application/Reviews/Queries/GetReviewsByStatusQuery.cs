using MediatR;
using Neighborhood.Services.Application.Reviews.DTOs;
using Neighborhood.Services.Domain.Reviews;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Reviews.Queries
{
    public record GetReviewsByStatusQuery(ReviewStatus Status): IRequest<IReadOnlyList<ReviewDto>>;
}
