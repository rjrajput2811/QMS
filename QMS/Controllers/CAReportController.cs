using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class CAReportController : Controller
    {
        public IActionResult CAFormatee()
        {
            return View();
        }
        public IActionResult CAFormateDetails()
        {
            return View();
        }
    }
}
