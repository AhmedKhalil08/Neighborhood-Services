using MediatR;
using Neighborhood.Services.Application.Bookings.Interface;
using Neighborhood.Services.Application.Reviews.Commands;
using Neighborhood.Services.Application.Reviews.DTOs;
using Neighborhood.Services.Application.Reviews.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Application.Shared.Mappers;
using Neighborhood.Services.Domain.Bookings;
using Neighborhood.Services.Domain.Reviews;


namespace Neighborhood.Services.Application.Reviews.Handlers
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IReviewRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;
        public CreateReviewCommandHandler(IReviewRepository repository, IUnitOfWork unitOfWork, IBookingRepository bookingRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _bookingRepository = bookingRepository;
        }

        public async Task<ReviewDto> Handle(
     CreateReviewCommand request,
     CancellationToken cancellationToken)
        {
            if (request.ReviewerId == request.RevieweeId)
                throw new Exception("User cannot review himself.");

            if (request.Rating < 1 || request.Rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            


            var booking = await _bookingRepository.GetBookingWithDetailsAsync(request.BookingId);
            if (booking is null)
                throw new Exception("Booking not found");
            var customerUserId = booking.Customer.ApplicationUserId;
            var technicianUserId = booking.Technician.ApplicationUserId;

            ReviewType direction;

            if (request.ReviewerId == customerUserId &&
                request.RevieweeId == technicianUserId)
            {
                direction = ReviewType.CustomerToTechnician;
            }
            else if (request.ReviewerId == technicianUserId &&
                     request.RevieweeId == customerUserId)
            {
                direction = ReviewType.TechnicianToCustomer;
            }
            else
            {
                throw new Exception("Invalid review direction");
            }

            var ReviewExists = await _repository.ExistsByDirectionAsync( request.BookingId, direction);

            if (ReviewExists)
                throw new Exception("Review already exists for this direction");
            var review = new Review
            {
                BookingId = request.BookingId,
                ReviewerId = request.ReviewerId,
                RevieweeId = request.RevieweeId,
                Rating = request.Rating,
                Comment = request.Comment,
                Status = ReviewStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(review);

            await _unitOfWork.SaveChangesAsync();

            return ReviewMapper.MapToDto(review);
        }
    }
}
