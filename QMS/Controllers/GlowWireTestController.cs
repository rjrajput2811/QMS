using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class GlowWireTestController : Controller
    {
        public IActionResult GlowWireTestReport()
        {
            return View();
        }
        public IActionResult GlowWireTestReportDetails()
        {
            return View();
        }

    }
}
