using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class ChangeNoteController : Controller
    {
        public IActionResult ChangeNoteDetails()
        {
            return View();
        }
        public IActionResult ChangeNotee()
        {
            return View();
        }
    }
}
