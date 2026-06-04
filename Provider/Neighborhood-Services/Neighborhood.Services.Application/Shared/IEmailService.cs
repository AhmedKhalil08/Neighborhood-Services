using Neighborhood.Services.Application.Shared;

namespace Neighborhood.Services.Application.Shared
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
        void SendAuthEmail(EmailDto request);
        void SendBookingEmail(EmailDto request);
        void SendWelcomeEmail(EmailDto request);
    }
}
