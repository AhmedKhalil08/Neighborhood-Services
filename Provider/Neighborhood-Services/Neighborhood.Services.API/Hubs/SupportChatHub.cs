using Microsoft.AspNetCore.SignalR;

namespace Neighborhood.Services.API.Hubs
{
    public class SupportChatHub : Hub
    {
        // العميل بيجوين room خاصة بالـ ticket
        public async Task JoinTicket(string ticketId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        }

        public async Task LeaveTicket(string ticketId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        }

        // إرسال الرسالة الكاملة (بعد ما اتحفظت في الداتابيز عن طريق REST) لكل اللي فاتحين نفس التيكت
        public async Task SendMessage(string ticketId, object message)
        {
            await Clients.Group($"ticket-{ticketId}").SendAsync("ReceiveMessage", message);
        }
    }
}
