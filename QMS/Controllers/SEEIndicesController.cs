using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    public class SEEIndicesController: Controller
    {
        public IActionResult SEEIndicees()
        {
            return View();
        }
        public IActionResult SEEIndiceesdetail()
        {
            return View(); 
        }
    }
}
