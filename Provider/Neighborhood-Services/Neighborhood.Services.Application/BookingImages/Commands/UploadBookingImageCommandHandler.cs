using MediatR;
using Neighborhood.Services.Application.BookingImages.Interface;
using Neighborhood.Services.Application.Bookings.Interface;
using Neighborhood.Services.Application.Exceptions;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.BookingImages;
using Neighborhood.Services.Domain.Bookings;

namespace Neighborhood.Services.Application.BookingImages.Commands
{
    public class UploadBookingImageCommandHandler : IRequestHandler<UploadBookingImageCommand, int>
    {
        private readonly IBookingImageRepository _bookingImageRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UploadBookingImageCommandHandler(
            IBookingImageRepository bookingImageRepository,
            IBookingRepository bookingRepository,
            IUnitOfWork unitOfWork)
        {
            _bookingImageRepository = bookingImageRepository;
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UploadBookingImageCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            if (booking is null)
                throw new NotFoundException(nameof(Booking), request.BookingId);

            if (!IsValidHttpUrl(request.ImageUrl))
                throw new BadRequestException("ImageUrl must be a valid absolute http/https URL.");

            var image = new BookingImage
            {
                BookingId = request.BookingId,
                ImageUrl = request.ImageUrl,
                Type = request.Type,
                UploadedBy = request.UploadedBy,
                UploadedAt = DateTime.UtcNow
            };

            await _bookingImageRepository.AddAsync(image);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return image.Id;
        }

        private static bool IsValidHttpUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
