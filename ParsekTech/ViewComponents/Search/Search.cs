using Microsoft.AspNetCore.Mvc;

namespace SampleProjectNoApi.ViewComponents.Search;

public class Search : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}