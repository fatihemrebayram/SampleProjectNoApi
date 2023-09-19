using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramewok;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace SampleProjectNoApi.Controllers;

[Authorize(Roles = "Admin")]
public class ViewLogsController : Controller
{
    private readonly LogManager _logManager = new(new EFLogsDAL());

    public IActionResult Index(int page = 1, string SearchFor = "")
    {
        var values = _logManager
            .GetListFilteredBL(SearchFor)
            .OrderByDescending(y => y.TimeStamp)
            .ToPagedList(page, 10);

        return View(values);
    }

    public IActionResult SearchResults([FromQuery] string query, int page = 1)
    {
        var context = new Context();
        var SearchResult = context.Logs
            .Where(r =>
                r.Message.Contains(query)
                || r.UserDomainNamePC.Contains(query)
                || r.Username.Contains(query)
                || r.UserNamePC.Contains(query)
                || r.Level.Contains(query)
                || r.ComputerName.Contains(query))
            .OrderByDescending(y => y.TimeStamp)
            .ToList();
        ViewBag.SearchResultCount = SearchResult.Count();
        ViewBag.Page = page;
        return View(SearchResult);
    }
}