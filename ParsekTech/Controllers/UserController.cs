using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace SampleProjectNoApi.Controllers;

public class UserController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public UserController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new UserProfileCRUDViewModel();
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        model.AppUser = user;
        var userUpdateViewModel = new UserUpdateViewModel();
        userUpdateViewModel.Mail = user.Email;
        userUpdateViewModel.NameSurname = user.NameSurname;
        userUpdateViewModel.ImageURL = user.ImageURL;
        userUpdateViewModel.Username = user.UserName;
        userUpdateViewModel.Department = user.Department;
        model.UserUpdateViewModel = userUpdateViewModel;
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
            Logger.LogMessage(values.UserName + " kullanıcının şifresi kendi tarafından değiştirildi.",
                User.Identity.Name, LogLevel.Information, HttpContext);
        }

        var result = await _userManager.UpdateAsync(values);
        return RedirectToAction("Index", "User");
    }
}