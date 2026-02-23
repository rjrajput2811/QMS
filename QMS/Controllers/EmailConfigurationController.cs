using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.EmailConfigurationRepository;

namespace QMS.Controllers
{
    public class EmailConfigurationController : Controller
    {
        private readonly IEmailConfigurationRepository _emailConfigurationRepository;

        public EmailConfigurationController(IEmailConfigurationRepository emailConfigurationRepository)
        {
            _emailConfigurationRepository = emailConfigurationRepository;
        }

        public async Task<IActionResult> EmailConfiguration()
        {
            var model = new EmailConfigurationViewModel();
            model = await _emailConfigurationRepository.GetEmailConfiguration();
            return View(model);
        }

        public async Task<ActionResult> InsertUpdateEmailConfigurationAsync(EmailConfigurationViewModel model)
        {
            var response = new OperationResult();
            if (model.Id > 0)
            {
                model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                model.UpdatedOn = DateTime.Now;
                response = await _emailConfigurationRepository.UpdateEmailConfiguration(model);
            }
            else
            {
                model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
                model.AddedOn = DateTime.Now;
                response = await _emailConfigurationRepository.CreateEmailConfiguration(model);
            }

            return Json(response);
        }
    }
}
