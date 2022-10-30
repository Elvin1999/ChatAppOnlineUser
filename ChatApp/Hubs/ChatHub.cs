using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(Message message)
        {
            await Clients.Others.SendAsync("receiveMessage",message);
        }
    }
}
