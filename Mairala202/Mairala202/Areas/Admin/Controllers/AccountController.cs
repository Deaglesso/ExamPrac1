using Mairala202.Areas.Admin.ViewModels;
using Mairala202.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mairala202.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = new AppUser 
            {
                Email = vm.Email,
                Name = vm.Name,
                Surname = vm.Surname,
                UserName = vm.Username
            };
            var res = await _userManager.CreateAsync(user, vm.Password);
            if (!res.Succeeded)
            {
                foreach (var item in res.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(vm);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("Index","Dashboard", new { area="" });

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Dashboard", new { area = "" });

        }

        /*public async Task<IActionResult> Create()
        {
            IdentityRole role = new IdentityRole
            {
                Name="Admin"
            };
            await _roleManager.CreateAsync(role);
            return RedirectToAction("Index", "Dashboard");

        }*/


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            if (user is null) 
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
                if (user is null) 
                {
                    ModelState.AddModelError("", "Wrong credentials");
                    return View(vm);
                }


            }
            var res = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemembered, true);
            if (res.IsLockedOut)
            {
                ModelState.AddModelError("", "Locked out");
                return View(vm);
            }
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "Wrong credentials");
                return View(vm);
            }
            return RedirectToAction("Index", "Dashboard", new { area = "" });


        }
    }
}
