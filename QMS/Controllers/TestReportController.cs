using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class TestReportController : Controller
    {
        public IActionResult TestReport()
        {
            return View();
        }
        public IActionResult TestReportDetails()
        {
            return View();
        }
    }
}
