using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Models;

public class AdminRoleViewModel
{
    [Required(ErrorMessage = "Boş bırakılmaz")]
    public string Name { get; set; }
}