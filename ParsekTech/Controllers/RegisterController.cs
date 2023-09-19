using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;

namespace SampleProjectNoApi.Controllers;

[Authorize(Roles = "Admin")]
public class RegisterController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(UserSignUpViewModel userSign)
    {
        if (ModelState.IsValid)
        {
            var appUser = new AppUser
            {
                Email = userSign.Mail,
                UserName = userSign.UserName,
                NameSurname = userSign.NameSurname,
                ImageURL = "/Admin/assets/images/users/2.jpg"
            };

            var result = await _userManager.CreateAsync(appUser, userSign.Password);

            if (result.Succeeded)
            {
                Logger.LogMessage("Sisteme " + userSign.UserName + " kullanıcısı eklendi", User.Identity.Name,
                    LogLevel.Information, HttpContext);

                return RedirectToAction("Index", "Dashboard");
            }

            foreach (var item in result.Errors)
            {
                Logger.LogMessage(
                    userSign.UserName + " adlı personel sisteme eklenirken hata oluştu:" + item.Description,
                    User.Identity.Name, LogLevel.Error, HttpContext);
                ModelState.AddModelError("", item.Description);
            }
        }

        return View();
    }
}