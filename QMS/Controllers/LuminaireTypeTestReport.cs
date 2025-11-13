using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // for Session extensions
using QMS.Core.ViewModels;
using QMS.Core.Repositories.InternalTypeTestRepo; // contains IInternalTypeTestRepository
using QMS.Core.Services.SystemLogs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QMS.Controllers
{
    public class LuminaireTypeTestReportController : Controller
    {
        private readonly IInternalTypeTestRepository _internalTypeTestRepository;
        private readonly ISystemLogService _systemLogService;

        public LuminaireTypeTestReportController(
            IInternalTypeTestRepository internalTypeTestRepository,
            ISystemLogService systemLogService)
        {
            _internalTypeTestRepository = internalTypeTestRepository ?? throw new ArgumentNullException(nameof(internalTypeTestRepository));
            _systemLogService = systemLogService ?? throw new ArgumentNullException(nameof(systemLogService));
        }
        public IActionResult LuminaireTypeTest()
        {
            return View();
        }
        public async Task<ActionResult> GetInternalTypeTestListAsync()
        {
            var result = await _internalTypeTestRepository.GetInternalTypeTestsAsync();
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetInternalTypeTestDetailsAsync(int internalTypeId)
        {
            var result = await _internalTypeTestRepository.GetInternalTypeTestByIdAsync(internalTypeId);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> LuminaireTypeTestDetails(int? internalTypeId)
        {
            InternalTypeTestViewModel model;

            if (internalTypeId == null || internalTypeId == 0)
            {
                // Add New Mode
                model = new InternalTypeTestViewModel
                {
                    Date = DateTime.Now
                };
            }
            else
            {
                // Edit Mode
                model = await _internalTypeTestRepository.GetInternalTypeTestByIdAsync(internalTypeId.Value);

                if (model == null)
                {
                    return NotFound();
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertInternalTypeTestAsync(InternalTypeTestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { Success = false, Errors = errors });
                }

                model.CreatedBy = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "System";

                var result = await _internalTypeTestRepository.InsertInternalTypeTestAsync(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { Success = false, Errors = new[] { "Failed to insert internal type test." }, Exception = ex.Message });
            }
        }
    }
}
