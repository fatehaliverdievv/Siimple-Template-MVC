using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Siimple.Models;
using Siimple.Utilies.Enum;
using Siimple.ViewModels;

namespace Siimple.Controllers
{
    public class AccountController : Controller
    {
        RoleManager<IdentityRole> _roleManager { get; }
        UserManager<AppUser> _userManager{ get; }
        SignInManager<AppUser> _signInManager{ get; }

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM userVM,string? ReturnUrl)
        {
            if(!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(userVM.UsernameorEmail);
            if (user == null)
            {
                user=await _userManager.FindByNameAsync(userVM.UsernameorEmail);
                if(user == null)
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, userVM.RememberMe, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }
            if (ReturnUrl != null)
            {
                 return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM registerUserVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                Name = registerUserVM.Name,
                Email = registerUserVM.Email,
                Surname = registerUserVM.Surname,
                UserName = registerUserVM.Username
            };
            var result = await _userManager.CreateAsync(user, registerUserVM.Password);
            if(!result.Succeeded) 
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user,Roles.Member.ToString());
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if(!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
