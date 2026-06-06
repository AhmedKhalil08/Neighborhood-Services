using MediatR;
using Neighborhood.Services.Application.Reviews.DTOs;

namespace Neighborhood.Services.Application.Reviews.Commands
{
    public class CreateReviewCommand : IRequest<ReviewDto>
    {
        public int BookingId { get; set; }
        public string ReviewerId { get; set; }
        public string RevieweeId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

}
