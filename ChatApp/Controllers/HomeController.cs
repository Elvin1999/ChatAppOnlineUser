using ChatApp.Entities;
using ChatApp.Helpers;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ChatApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private UserManager<CustomIdentityUser> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        private CustomIdentityDbContext _context;
        public HomeController(UserManager<CustomIdentityUser> userManager, IHttpContextAccessor httpContextAccessor, CustomIdentityDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            UserHelper.CurrentUser = user;
            ViewBag.User = user;

            var users = _userManager.Users.Where(u => u.Id != user.Id).ToList();

            foreach (var item in users)
            {
                var onlineUser = UserHelper.ActiveUsers.FirstOrDefault(u => u.Id == item.Id);
                if (onlineUser != null)
                {
                    item.IsOnline = true;
                }
            }

            var model = new HomeViewModel
            {
                AllUsers = users,
                ActiveUsers = UserHelper.ActiveUsers
            };
            return View(model);
        }



        public async Task<IActionResult> FindCurrentUser(string id)
        {
            UserHelper.ReceiverUser = _userManager.Users.FirstOrDefault(u => u.Id == id);
            return RedirectToAction("MessageChat");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = _context.FriendRequests.FirstOrDefault(u => u.Id == id);
            if (request != null)
            {
                _context.FriendRequests.Remove(request);
                await _context.SaveChangesAsync();

                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> UnFollow(int id)
        {
            var request = _context.Friends.FirstOrDefault(u => u.Id == id);
            if (request != null)
            {
                _context.FriendRequests.Add(new FriendRequest
                {
                    Status = "Notification",
                    ReceiverId = request.YourFriendId,
                    SenderId = request.OwnId,
                    Content = "You unfollowed by your friend",
                });

                var request2 = _context.Friends.FirstOrDefault(u => u.OwnId == request.YourFriendId);
                _context.Friends.Remove(request);
                _context.Friends.Remove(request2);

                await _context.SaveChangesAsync();

                return Ok();
            }
            return BadRequest();
        }

        public IActionResult MessageChat()
        {
            var model = new ChatModel
            {
                ReceiverUser = UserHelper.ReceiverUser,
                SenderUser = UserHelper.CurrentUser
            };
            return View(model);
        }


        public async Task<IActionResult> GetAllActiveUsers()
        {
            return Ok(UserHelper.ActiveUsers.DistinctBy(u => u.Id));
        }


        public async Task<IActionResult> GetMyFriends()
        {
            var users = _userManager.Users.Include("Friends");
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var currentUser = users.FirstOrDefault(u => u.Id == user.Id);
            currentUser.Friends = _context.Friends.Include("YourFriend").Where(f => f.OwnId == currentUser.Id).ToList();

            return Ok(currentUser.Friends);
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var myFriends = _context.Friends.Include("YourFriend").Where(f => f.OwnId == user.Id).ToList();

            var users = _userManager.Users.Where(u => u.Id != user.Id).ToList();

            var requests = _context.FriendRequests.ToList();
            foreach (var f in myFriends)
            {
                var data = users.FirstOrDefault(u => u.Id == f.YourFriendId);
                users.Remove(data);
            }
            foreach (var item in users)
            {
                var onlineUser = UserHelper.ActiveUsers.FirstOrDefault(u => u.Id == item.Id);
                if (onlineUser != null)
                {
                    item.IsOnline = true;
                    item.ConnectTime=onlineUser.ConnectTime;    
                }

                var request = requests.FirstOrDefault(r => r.ReceiverId == item.Id && r.ReceiverId != r.SenderId && r.Status == "Request");
                if (request != null)
                {
                    item.HasRequestPending = true;
                }
                else
                {
                    item.HasRequestPending = false;
                }

            }
            return Ok(users.OrderByDescending(u => u.IsOnline));
        }

        public async Task<IActionResult> SendFollow(string id)
        {
            var sender = await _userManager.GetUserAsync(HttpContext.User);

            var receiveruser = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (receiveruser != null)
            {

                var newsender = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == sender.Id);
                receiveruser.FriendRequests.Add(new FriendRequest
                {
                    Content = $"{sender.UserName} send friend Request at {DateTime.Now.ToLongDateString()}",
                    SenderId = sender.Id,
                    CustomIdentityUser = newsender,
                    Status = "Request",
                    ReceiverId = id
                });

                await _userManager.UpdateAsync(receiveruser);
            }
            return Ok();
        }



        public async Task<IActionResult> AcceptRequest(string id, int requestId)
        {
            var users = _context.Users.Include(nameof(CustomIdentityUser.FriendRequests));

            var s = await _userManager.GetUserAsync(HttpContext.User);

            var sender = users.FirstOrDefault(u => u.Id == s.Id);

            var receiveruser = _userManager.Users.FirstOrDefault(u => u.Id == id);

            var deleteRequest = sender.FriendRequests.FirstOrDefault(f => f.Id == requestId);
            sender.FriendRequests.Remove(deleteRequest);
            _context.FriendRequests.Remove(deleteRequest);

            await _userManager.UpdateAsync(sender);

            if (receiveruser != null)
            {
                receiveruser.FriendRequests.Add(new FriendRequest
                {
                    Content = $"{sender.UserName} accept friend Request at {DateTime.Now.ToLongDateString()}",
                    SenderId = sender.Id,
                    CustomIdentityUser = sender,
                    Status = "Notification",
                    ReceiverId = receiveruser.Id
                });
                var receiverFriend = new Friend
                {
                    OwnId = receiveruser.Id,
                    YourFriendId = sender.Id,
                };
                _context.Friends.Add(receiverFriend);

                var senderFriend = new Friend
                {
                    OwnId = sender.Id,
                    YourFriendId = receiveruser.Id,
                };

                receiveruser.Friends.Add(senderFriend);
                sender.Friends.Add(receiverFriend);

                _context.Friends.Add(senderFriend);

                receiveruser.HasRequestPending = false;

                await _context.SaveChangesAsync();

                await _userManager.UpdateAsync(sender);
                await _userManager.UpdateAsync(receiveruser);
            }
            return Ok();
        }

        public async Task<IActionResult> DeclineRequest(string idSender, int requestId)
        {

            var users = _context.Users.Include(nameof(CustomIdentityUser.FriendRequests));

            var s = await _userManager.GetUserAsync(HttpContext.User);

            var sender = users.FirstOrDefault(u => u.Id == s.Id);

            var receiveruser = _userManager.Users.FirstOrDefault(u => u.Id == idSender);

            var deleteRequest = sender.FriendRequests.FirstOrDefault(f => f.Id == requestId);
            sender.FriendRequests.Remove(deleteRequest);
            _context.FriendRequests.Remove(deleteRequest);

            await _userManager.UpdateAsync(sender);

            if (receiveruser != null)
            {
                receiveruser.FriendRequests.Add(new FriendRequest
                {
                    Content = $"{sender.UserName} decline friend Request at {DateTime.Now.ToLongDateString()}",
                    SenderId = sender.Id,
                    CustomIdentityUser = sender,
                    Status = "Notification",
                    ReceiverId = receiveruser.Id
                });

                receiveruser.HasRequestPending = false;


                await _userManager.UpdateAsync(receiveruser);
            }
            return Ok();
        }

        public async Task<IActionResult> GetAllRequests()
        {
            var current = await _userManager.GetUserAsync(HttpContext.User);
            var users = _context.Users.Include(nameof(CustomIdentityUser.FriendRequests));
            var user = users.FirstOrDefault(u => u.Id == current.Id);
            var items = user.FriendRequests.Where(r => r.ReceiverId == user.Id);
            return Ok(items);
        }

    }
}