using FusePortal.Application.Interfaces.Services;
using FusePortal.Infrastructure.Services.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FusePortal.Infrastructure.Services.SignalR
{
    public class SignalRMessageStreamer(
            IHubContext<ChatHub> hub
            ) : IMessageStreamer
    {
        private readonly IHubContext<ChatHub> _hub = hub;

        public Task StreamAsync(Guid chatId, string chunk, CancellationToken ct = default)
        {
            return _hub.Clients.Group(chatId.ToString())
                .SendAsync("messageReceived", chatId, chunk);
        }

    }
}
