using MediatR;
using Neighborhood.Services.Application.Reviews.DTOs;
using Neighborhood.Services.Application.Reviews.Interfaces;
using Neighborhood.Services.Application.Reviews.Queries;
using Neighborhood.Services.Application.Shared.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Reviews.Handlers
{
    public class GetFlaggedReviewsQueryHandler
    : IRequestHandler<GetFlaggedReviewsQuery, IReadOnlyList<ReviewDto>>
    {
        private readonly IReviewRepository _repository;

        public GetFlaggedReviewsQueryHandler(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<ReviewDto>> Handle(
            GetFlaggedReviewsQuery request,
            CancellationToken cancellationToken)
        {
            var reviews = await _repository.GetFlaggedAsync(
                cancellationToken);

            return reviews
                .Select(ReviewMapper.MapToDto)
                .ToList();
        }
    }
}
