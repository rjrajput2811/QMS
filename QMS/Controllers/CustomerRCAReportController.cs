using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class CustomerRCAReportController : Controller
    {
        public IActionResult CustomerRCA()
        {
            return View();
        }
        public IActionResult CustomerRCADetails()
        {
            return View();
        }
    }
}
