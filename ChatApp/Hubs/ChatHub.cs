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
        private CustomIdentityDbContext _context;

        public ChatHub(UserManager<CustomIdentityUser> userManager, IHttpContextAccessor httpContextAccessor, CustomIdentityDbContext context)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task SendMessage(string user,string message)
        {
            var currentUser = UserHelper.CurrentUser;
            var userId = UserHelper.ReceiverUser.Id;

            await Clients.User(userId).SendAsync("ReceiveMessage",UserHelper.CurrentUser,message);
        }

        public async Task SendMessageUser(string receiverId, string senderId,string message)
        {
            //var currentUser = UserHelper.CurrentUser;
            //var userId = UserHelper.ReceiverUser.Id;

            var chat=_context.Chats.FirstOrDefault(c=>c.SenderId==senderId && c.ReceiverId==receiverId || c.ReceiverId == senderId && c.SenderId == receiverId);

            chat.Messages.Add(new Message
            {
                 SenderId = senderId,
                 ReceiverId = receiverId,
                 Content = message,
                 DateTime=DateTime.Now
            });
            _context.Update(chat);
            await _context.SaveChangesAsync();

            var receiver = userManager.Users.FirstOrDefault(u => u.Id == receiverId);
            var sender = userManager.Users.FirstOrDefault(u => u.Id == senderId);

            //params  sender,receiver,message
            await Clients.User(receiver.Id).SendAsync("ReceiveMessage", DateTime.Now.ToLongTimeString(), message);
            await Clients.User(sender.Id).SendAsync("ReceiveMessage", DateTime.Now.ToLongTimeString(), message);
        }

        public override async Task OnConnectedAsync()
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
            user.ConnectTime = DateTime.Now.ToShortTimeString();
            UserHelper.ActiveUsers.Add(user);
            string info = user.UserName + " connected Succesfully";
            await Clients.Others.SendAsync("Connect","");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
            var userRemoved = UserHelper.ActiveUsers.SingleOrDefault(u => u.Id == user.Id);
            if (userRemoved != null)
            {
                UserHelper.ActiveUsers.RemoveAll(u => u.Id == userRemoved.Id);
                var disconnectedUser=userManager.Users.FirstOrDefault(u => u.Id == userRemoved.Id);
                disconnectedUser.DisConnectTime = DateTime.Now;
                await userManager.UpdateAsync(disconnectedUser);
                string info = user.UserName + " disconnected";
                await Clients.Others.SendAsync("Disconnect", "");
            }
        }
    }
}
