using ChatApp.Entities;
using ChatApp.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub:Hub
    {
        private UserManager<CustomIdentityUser> userManager;
        private IHttpContextAccessor httpContextAccessor;

        public ChatHub(UserManager<CustomIdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task SendMessage(string user,string message)
        {
            var currentUser = UserHelper.CurrentUser;
            var userId = UserHelper.ReceiverUser.Id;

            await Clients.User(userId).SendAsync("ReceiveMessage",UserHelper.CurrentUser,message);
        }

        public override async Task OnConnectedAsync()
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
            UserHelper.ActiveUsers.Add(user);
            string info = user.UserName + " connected Succesfully";
            await Clients.Others.SendAsync("Connect",info);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
            var userRemoved = UserHelper.ActiveUsers.SingleOrDefault(u => u.Id == user.Id);
            if (userRemoved != null)
            {
                UserHelper.ActiveUsers.RemoveAll(u => u.Id == userRemoved.Id);

                string info = user.UserName + " disconnected";
                await Clients.Others.SendAsync("Disconnect", info);
            }
        }
    }
}
