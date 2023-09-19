using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Models;

public class UserSignUpViewModel
{
    [Display(Name = "Şifre Tekrar")]
    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Mail Adresi")]
    [Required(ErrorMessage = "Mail Giriniz")]
    public string Mail { get; set; }

    [Display(Name = "Ad Soyad")]
    [Required(ErrorMessage = "Ad Soyad Giriniz")]
    public string NameSurname { get; set; }

    [Display(Name = "Şifre")]
    [Required(ErrorMessage = "Şifre Giriniz")]
    public string Password { get; set; }

    [Display(Name = "Kullanıcı Adı")]
    [Required(ErrorMessage = "Kullanıcı Ad Giriniz")]
    public string UserName { get; set; }
}