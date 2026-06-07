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
    public class GetReviewsByStatusQueryHandler: IRequestHandler<GetReviewsByStatusQuery, IReadOnlyList<ReviewDto>>
    {
        private readonly IReviewRepository _repository;

        public GetReviewsByStatusQueryHandler(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<ReviewDto>> Handle(
            GetReviewsByStatusQuery request,
            CancellationToken cancellationToken)
        {
            var reviews = await _repository.GetByStatusAsync(
                request.Status,
                cancellationToken);

            return reviews
                .Select(ReviewMapper.MapToDto)
                .ToList();
        }
    }
}
