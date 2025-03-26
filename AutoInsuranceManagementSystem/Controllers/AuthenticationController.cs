using AutoInsuranceManagementSystem.Models.AuthenticationViewModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AutoInsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Azure.Core.Pipeline;

namespace AutoInsuranceManagementSystem.Controllers
{
    public class AuthenticationController(UserManager<UserEntityModel> userManager) : Controller
    {

        private readonly UserManager<UserEntityModel> _userManager = userManager;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Passwords do not match");
                return View(registerViewModel);
            }
            UserEntityModel user = new UserEntityModel
            {
                FullName = registerViewModel.FullName,
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                Role = Roles.CUSTOMER
            };
            var result = _userManager.CreateAsync(user, registerViewModel.Password ?? "Open@1234").Result;
            await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return View(registerViewModel);
            }
            else
            {
                var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName ??""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role,"CUSTOMER")
                };
                await (_userManager.AddClaimsAsync(user, Claims));
            }
            return View("Success");
        }

        public IActionResult Success()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByEmailAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt, user not found");
                return View(loginViewModel);
            }
            var result = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "invalid login attempt, password is incorrect");
                return View(loginViewModel);
            }
            else
            {
                var claims = user != null ? await _userManager.GetClaimsAsync(user) : null;
                if (claims != null)
                {
                    ViewData["ReturnUrl"] = ReturnUrl;
                    var scheme = IdentityConstants.ApplicationScheme;
                    var claimsIdentity = new ClaimsIdentity(claims, scheme);
                    var principle = new ClaimsPrincipal(claimsIdentity);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                    };
                    await HttpContext.SignInAsync(scheme, principle, authenticationProperties);

                    if(user.Role == Roles.ADMIN)
                    {
                        return RedirectToAction("Index", "Home", new{area = "Admin"});
                    }
                    else if(user.Role == Roles.AGENT)
                    {
                        return RedirectToAction("Index", "Home", new{area = "Agent"});
                    }

                    return Redirect(ReturnUrl);

                }

            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return Redirect("/Home/Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
