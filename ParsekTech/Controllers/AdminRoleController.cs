using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace SampleProjectNoApi.Controllers;

[Authorize(Roles = "Admin")]
public class AdminRoleController : Controller
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public AdminRoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult AddRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(AdminRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = new AppRole
            {
                Name = model.Name
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var item in result.Errors) ModelState.AddModelError("", item.Description);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AssignRole(int id)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
        var roles = _roleManager.Roles.ToList();
        TempData["UserId"] = user.Id;
        var userroles = await _userManager.GetRolesAsync(user);
        var model = new List<AdminRoleAssingView>();
        foreach (var item in roles)
        {
            var AdminRole = new AdminRoleAssingView();
            AdminRole.RoleId = item.Id;
            AdminRole.Name = item.Name;
            AdminRole.Exists = userroles.Contains(item.Name);
            model.Add(AdminRole);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(AdminRoleALLViewModel model)
    {
        var userId = (int)TempData["UserId"];
        var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

        foreach (var item in model.AdminRoleAssingViews)
            if (item.Exists)
                await _userManager.AddToRoleAsync(user, item.Name);
            else
                await _userManager.RemoveFromRoleAsync(user, item.Name);
        Logger.LogMessage(user.UserName + " kullanıcının rolü değişti.", User.Identity.Name, LogLevel.Information,
            HttpContext);
        return RedirectToAction("Index", "AdminRole");
    }

    public async Task<IActionResult> DeleteRole(int id)
    {
        var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);

        var result = await _roleManager.DeleteAsync(values);
        if (result.Succeeded) return RedirectToAction("Index");
        return View();
    }

    public async Task<IActionResult> Index(string username = "0")
    {
        var compositeViewModel = new AdminRoleALLViewModel
        {
            UserUpdateViewModel = new UserUpdateViewModel(),
            AdminRoleAssingViews = new List<AdminRoleAssingView>(),
            AppUsers = _userManager.Users.ToList()
        };

        ViewBag.username = username;
        if (username != "0")
        {
            ViewBag.Edit = "open";
            var user = await _userManager.FindByNameAsync(username);
            compositeViewModel.UserUpdateViewModel.Mail = user.Email;
            compositeViewModel.UserUpdateViewModel.NameSurname = user.NameSurname;
            compositeViewModel.UserUpdateViewModel.ImageURL = user.ImageURL;
            compositeViewModel.UserUpdateViewModel.Username = user.UserName;
            compositeViewModel.UserUpdateViewModel.Department = user.Department;
            var roles = _roleManager.Roles.ToList();
            TempData["UserId"] = user.Id;
            var userroles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                var AdminRole = new AdminRoleAssingView();
                AdminRole.RoleId = item.Id;
                AdminRole.Name = item.Name;
                AdminRole.Exists = userroles.Contains(item.Name);
                compositeViewModel.AdminRoleAssingViews.Add(AdminRole);
            }
        }

        // Populate the necessary data in the compositeViewModel

        return View(compositeViewModel);
    }

    public IActionResult RoleList()
    {
        var values = _roleManager.Roles.ToList();
        return View(values);
    }

    [HttpGet]
    public IActionResult UpdateRole(int id)
    {
        var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
        var model = new AdminRoleUpdateModel
        {
            Id = values.Id,
            Name = values.Name
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRole(AdminRoleUpdateModel model)
    {
        var values = _roleManager.Roles.Where(x => x.Id == model.Id).FirstOrDefault();
        values.Name = model.Name;
        var result = await _roleManager.UpdateAsync(values);
        if (result.Succeeded) return RedirectToAction("Index");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UserSettings(AddProfileImage p)
    {
        var values = await _userManager.FindByNameAsync(p.Username);
        var userModel = new UserUpdateViewModel();
        if (p.ProfileImage != null)
        {
            var fileName = @"wwwroot" + values.ImageURL;
            var file = new FileInfo(fileName);
            if (file.Exists) //check file exsit or not
                file.Delete();

            var extension = Path.GetExtension(p.ProfileImage.FileName);
            var newimagename = Guid.NewGuid() + extension;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/ImageFiles/", newimagename);

            // Open the uploaded image using ImageSharp
            using (var image = Image.Load(p.ProfileImage.OpenReadStream()))
            {
                // Resize the image to a maximum of 800 pixels wide or high, maintaining the aspect ratio
                var size = new Size(500, 600);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(size.Width, size.Height),
                    Mode = ResizeMode.Stretch
                }));

                // Optimize the image and save it to disk
                var encoder = new JpegEncoder { Quality = 70 };
                image.Save(location, encoder);
            }

            userModel.ImageURL = "/Admin/ImageFiles/" + newimagename;
            values.ImageURL = userModel.ImageURL;
        }

        userModel.Username = p.Username;
        userModel.Mail = p.Mail;
        userModel.password = p.password;
        userModel.NameSurname = p.NameSurname;
        userModel.Department = p.Department;

        values.NameSurname = userModel.NameSurname;
        values.Department = userModel.Department;

        values.Email = userModel.Mail;
        if (userModel.password != null)
        {
            values.PasswordHash = _userManager.PasswordHasher.HashPassword(values, userModel.password);
            Logger.LogMessage(
                values.UserName + " kullanıcının şifresi " + User.Identity.Name + " tarafından değiştirildi.",
                User.Identity.Name, LogLevel.Information, HttpContext);
        }

        var result = await _userManager.UpdateAsync(values);
        return RedirectToAction("Index", "AdminRole");
    }
}