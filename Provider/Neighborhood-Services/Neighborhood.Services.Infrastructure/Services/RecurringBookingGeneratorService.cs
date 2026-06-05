using Neighborhood.Services.Application.Bookings.Interface;
using Neighborhood.Services.Application.RecurringBookings.Interfaces;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.Bookings;
using Neighborhood.Services.Domain.RecurringBookings;

namespace Neighborhood.Services.Infrastructure.Services
{
    public class RecurringBookingGeneratorService
    {
        IRecurringBookingRepository _recurringBookingRepository;
        IBookingRepository _bookingRepository;
        IUnitOfWork _unitOfWork;
        public RecurringBookingGeneratorService(
            IRecurringBookingRepository recurringBookingRepository,
            IBookingRepository bookingRepository,
            IUnitOfWork unitOfWork)
        {
            _recurringBookingRepository = recurringBookingRepository;
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task GenerateBookings()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var dueRecurringBookings = await _recurringBookingRepository.GetDueRecurringBookingsAsync(today);
            foreach (var recurring in dueRecurringBookings)
            {
                var occurrenceDates = new List<DateOnly>();
                for(int i = 0; i <7; i++)
                {
                    var candidate = today.AddDays(i);
                    var isOccurence = recurring.Pattern switch
                    {
                        RecurringPattern.Daily=>true,
                        RecurringPattern.Weekly=>candidate.DayOfWeek==recurring.DayOfWeek,
                        RecurringPattern.Monthly=>candidate.Day==recurring.DayOfMonth,
                       _ =>false
                    };
                    if (isOccurence)
                    {
                        occurrenceDates.Add(candidate);
                    }
                }
                foreach (var date in occurrenceDates)
                {
                    var start = date.ToDateTime(recurring.TimeOfDay);
                    var end = start.AddMinutes(recurring.DurationMinutes);

                    // Skip if already generated for this occurrence
                    var alreadyExists = await _bookingRepository
                        .GetByConditionAsync(b =>
                            b.RecurringBookingId == recurring.Id &&
                            b.ScheduledAt == start &&
                            !b.IsDeleted);

                    if (alreadyExists.Any())
                        continue;

                    // Skip if slot is taken by another booking
                    var hasOverlap = await _bookingRepository
                        .HasOverlappingConfirmedBookingAsync(recurring.TechnicianId, start, end);

                    if (hasOverlap)
                        continue;

                    var booking = new Booking
                    {
                        CustomerId = recurring.CustomerId,
                        TechnicianId = recurring.TechnicianId,
                        ProblemTypeId = recurring.ProblemTypeId,
                        RecurringBookingId = recurring.Id,
                        BookingType = BookingType.Recurring,
                        Description = "Auto-generated recurring booking",
                        Address = recurring.Address,
                        ScheduledAt = start,
                        DurationMinutes = recurring.DurationMinutes,
                        EstimatedPrice = recurring.AgreedPrice ?? 0,
                        FinalPrice = 0,
                        Status = BookingStatus.Confirmed,
                        Location = recurring.Location,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _bookingRepository.AddAsync(booking);
                }
                await _unitOfWork.SaveChangesAsync(CancellationToken.None);
            }

        }
    }
}
