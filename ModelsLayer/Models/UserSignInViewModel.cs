using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Models;

public class UserSignInViewModel
{
    [Required(ErrorMessage = "Boş bırakılmaz")]
    public string password { get; set; }

    public bool RememberMe { get; set; }

    [Required(ErrorMessage = "Boş bırakılmaz")]
    public string UserName { get; set; }
}