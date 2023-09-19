using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SampleProjectNoApi.Controllers;

public class DefaultController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private UserManager userManager = new(new EFUserDAL());

    public DefaultController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult AccessDenied()
    {
        return PartialView();
    }

    public PartialViewResult AdminHeader()
    {
        return PartialView();
    }

    public PartialViewResult AdminSidebar()
    {
        return PartialView();
    }
}