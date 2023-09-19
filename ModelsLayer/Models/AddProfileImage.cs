using Microsoft.AspNetCore.Http;

namespace ModelsLayer.Models;

public class AddProfileImage
{
    public int Department { get; set; }
    public string Mail { get; set; }
    public string NameSurname { get; set; }
    public string password { get; set; }
    public IFormFile ProfileImage { get; set; }
    public string Username { get; set; }
}