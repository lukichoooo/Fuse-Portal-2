using Microsoft.AspNetCore.SignalR;

namespace FusePortal.Infrastructure.Services.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public Task JoinChat(string chatId)
            => Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        public Task LeaveChat(string chatId)
            => Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}
