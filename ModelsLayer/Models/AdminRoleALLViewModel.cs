using EntityLayer.Concrete;

namespace ModelsLayer.Models;

public class AdminRoleALLViewModel
{
    public AdminRoleAssingView AdminRoleAssingView { get; set; }

    public List<AdminRoleAssingView> AdminRoleAssingViews { get; set; } = null!;
    public List<AppUser> AppUsers { get; set; }
    public UserUpdateViewModel UserUpdateViewModel { get; set; }
}