using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ThirdPartyTestRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class ThirdPartyTestController : Controller
    {

        private readonly IThirdPartyTestRepository _thirdPartyTestRepository;
        private readonly ISystemLogService _systemLogService;

        public ThirdPartyTestController(IThirdPartyTestRepository thirdPartyTestRepository, ISystemLogService systemLogService)
        {
            _thirdPartyTestRepository = thirdPartyTestRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult ThirdPartyTestTrac()
        {
            return View();
        }

        public async Task<IActionResult> BisProjectTrackerDetail(int id)
        {
            var model = new ThirdPartyTestViewModel();

            if (id > 0)
            {
                model = await _thirdPartyTestRepository.GetByIdAsync(id);
                if (model == null)
                {
                    model = new ThirdPartyTestViewModel();
                }
            }
            else
            {
                model = new ThirdPartyTestViewModel();
            }

            return View(model); // Always return the model, whether new or fetched
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var thirdPartyList = await _thirdPartyTestRepository.GetListAsync(startDate, endDate);
            return Json(thirdPartyList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var thirdPartyTest = await _thirdPartyTestRepository.GetByIdAsync(Id);
            return Json(thirdPartyTest);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] ThirdPartyTesting model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid Bis Project Tracker data." });


                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _thirdPartyTestRepository.CreateAsync(model);

                if (result.Success)
                {
                    return Json(new { success = true, message = "Bis Project Detail saved successfully." });
                }

                return Json(new { success = false, message = "Failed to save bis project detail.", id = 0 });

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] ThirdPartyTesting model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _thirdPartyTestRepository.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _thirdPartyTestRepository.DeleteAsync(id);
            return Json(operationResult);
        }

        public async Task<IActionResult> UploadTPTAttachment(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ThirdParTestTrac_Attach", id.ToString());
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var nameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
                var newFileName = $"{nameWithoutExt}_{timestamp}{extension}";
                var filePath = Path.Combine(uploadsPath, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Optional: update DB with the new file name
                await _thirdPartyTestRepository.UpdateAttachmentAsync(id, newFileName);

                return Json(new { success = true, id = id, fileName = newFileName });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
