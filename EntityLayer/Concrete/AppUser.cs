using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Concrete;

public class AppUser : IdentityUser<int>
{
    public int Department { get; set; }
    public string? ImageURL { get; set; }

    public string NameSurname { get; set; }
}