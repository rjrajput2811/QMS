using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class GlowWireTest : Controller
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
