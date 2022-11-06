﻿using ChatApp.Entities;
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
                if(onlineUser != null)
                {
                    item.IsOnline = true;
                }
            }

            var model = new HomeViewModel
            {
                AllUsers = users,
                ActiveUsers=UserHelper.ActiveUsers
            };
            return View(model);
        }



        public async Task<IActionResult> FindCurrentUser(string id)
        {
            UserHelper.ReceiverUser = _userManager.Users.FirstOrDefault(u => u.Id == id);
            return RedirectToAction("MessageChat");
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

        public async Task<IActionResult> GetAllUsers()
        {
            var user=await _userManager.GetUserAsync(HttpContext.User);
            var users=_userManager.Users.Where(u => u.Id != user.Id);
            foreach (var item in users)
            {
                var onlineUser = UserHelper.ActiveUsers.FirstOrDefault(u => u.Id == item.Id);
                if (onlineUser != null)
                {
                    item.IsOnline = true;
                }
            }
            return Ok(users);
        }

        public async Task<IActionResult> SendFollow(string id)
        {
            var sender = UserHelper.CurrentUser;
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.FriendRequests.Add(new FriendRequest
                {
                     Content=$"{sender.UserName} send friend Request at {DateTime.Now.ToLongDateString()}",
                      SenderId = sender.Id,
                       CustomIdentityUser = sender
                });

                await _userManager.UpdateAsync(user);
            }
            return Ok();
        }

        public async Task<IActionResult> GetAllRequests()
        {
            var current = UserHelper.CurrentUser;
            var currentUser = _context.Users.Include(nameof(CustomIdentityUser.FriendRequests)).FirstOrDefault(u => u.Id == current.Id);
            var items = currentUser.FriendRequests;
            return Ok(items);
        }

    }
}