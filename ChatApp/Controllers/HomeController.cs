﻿using ChatApp.Entities;
using ChatApp.Helpers;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChatApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private UserManager<CustomIdentityUser> _userManager;
        private IHttpContextAccessor _httpContextAccessor;

        public HomeController(UserManager<CustomIdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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


    }
}