using Microsoft.AspNetCore.Mvc;
using QMS.Core.Repositories.MergeTrackerRepository;

namespace QMS.Controllers;

public class MergeTrackerController : Controller
{
    private readonly IMergeTrackerRepository _mergeTrackerRepository;
    public MergeTrackerController(IMergeTrackerRepository mergeTrackerRepository) 
    { 
        _mergeTrackerRepository = mergeTrackerRepository;    
    }

    public IActionResult MergeTracker()
    {
        return View();
    }

    public async Task<ActionResult> GetAllActivitySummaryAsync(DateTime fromDate, DateTime toDate)
    {
        var response = await _mergeTrackerRepository.GetSummaryOfAllActivityAsync(fromDate, toDate);
        return Json(response);
    }
}
