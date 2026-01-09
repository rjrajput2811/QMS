using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class DeviationNoteTracker : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
