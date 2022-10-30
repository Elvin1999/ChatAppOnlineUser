using ChatApp.Entities;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class AccountController:Controller
    {
        private UserManager<CustomIdentityUser> _userManager;
        private RoleManager<CustomIdentityRole> _roleManager;
        private SignInManager<CustomIdentityUser> _signInManager;

        private IHttpContextAccessor _contextAccessor;
        private IWebHostEnvironment _webHost;

        public AccountController(UserManager<CustomIdentityUser> userManager, 
            RoleManager<CustomIdentityRole> roleManager, 
            SignInManager<CustomIdentityUser> signInManager,
            IHttpContextAccessor contextAccessor, IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _webHost = webHost;
        }



        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(loginViewModel.Username,
                    loginViewModel.Password, loginViewModel.RememberMe, false).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
                  //  UserHelper.CurrentUser = user;
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login");
            }
            return View(loginViewModel);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var helper = new ImageHelper(_webhost);
        //        registerViewModel.ImageUrl = await helper.SaveFile(registerViewModel.File);
        //        CustomIdentityUser user = new CustomIdentityUser
        //        {
        //            UserName = registerViewModel.Username,
        //            Email = registerViewModel.Email,
        //            ImageUrl = registerViewModel.ImageUrl
        //        };

        //        IdentityResult result = _userManager.CreateAsync(user, registerViewModel.Password).Result;
        //        if (result.Succeeded)
        //        {
        //            if (!_roleManager.RoleExistsAsync("Admin").Result)
        //            {
        //                CustomIdentityRole role = new CustomIdentityRole
        //                {
        //                    Name = "Admin"
        //                };

        //                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
        //                if (!roleResult.Succeeded)
        //                {
        //                    ModelState.AddModelError("", "We can not add the role");
        //                    return View(registerViewModel);
        //                }

        //            }

        //            _userManager.AddToRoleAsync(user, "Admin").Wait();
        //            return RedirectToAction("Login");
        //        }


        //    }
        //    return View(registerViewModel);
        //}


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
