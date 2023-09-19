using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;

namespace SampleProjectNoApi.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index(string returnUrl = null)
    {
        ViewBag.Url = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(UserSignInViewModel user, [FromForm] string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.password, user.RememberMe, true);
            if (result.Succeeded)
            {
                // Log the successful login attempt
                Logger.LogMessage("Sisteme giriş yapıldı. Giriş Yapılan Kullanıcı: {UserName}", user.UserName,
                    LogLevel.Information, HttpContext);

                // Get the roles for the current user
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                var roles = await _userManager.GetRolesAsync(appUser);
                if (roles.Contains("User"))
                    // Redirect to the Rehber page for Iletisim and Rehber roles
                    return RedirectToAction("Index", "Dashboard");

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", "Dashboard");
                return Redirect(returnUrl);
            }

            // Log the failed login attempt
            Logger.LogMessage("Sisteme giriş yapılmaya çalışıldı ancak başarılı olunamadı.", user.UserName,
                LogLevel.Error, HttpContext);

            ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
            return RedirectToAction("Index", "Login");
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Login");
    }
}