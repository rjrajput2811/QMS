using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class TrackerOutputController : Controller
    {
        public IActionResult CAReportOutput()
        {
            return View();
        }
    }
}
