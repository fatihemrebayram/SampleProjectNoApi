using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleProjectNoApi.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}