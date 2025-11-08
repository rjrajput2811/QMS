using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class AHPNoteTracker : Controller
    {
        public IActionResult AHPNotetracker()
        {
            return View();
        }
        public IActionResult AHPNotetrackerDetails()
        {
            return View();
        }
    }
}
