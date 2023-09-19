using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace SampleProjectNoApi.ViewComponents.AdminHeader;

public class AdminHeaderProfile : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var context = new Context();

        var Values = context.AppUsers
            .Where(x => x.UserName == User.Identity.Name)
            .Distinct()
            .OrderBy(y => y)
            .ToList();
        //   var user = context.Set<AppUser>().Find(Values);
        return View(Values);
    }
}