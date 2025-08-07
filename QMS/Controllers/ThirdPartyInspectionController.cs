using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ThirdPartyInspectionRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class ThirdPartyInspectionController : Controller
    {
        private readonly IThirdPartyInspectionRepository _repository;
        private readonly ISystemLogService _systemLogService;

        public ThirdPartyInspectionController(
            IThirdPartyInspectionRepository repository,
            ISystemLogService systemLogService)
        {
            _repository = repository;
            _systemLogService = systemLogService;
        }

        public IActionResult ThirdPartyInspection()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _repository.GetListAsync(startDate, endDate);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var bisProject = await _repository.GetByIdAsync(Id);
            return Json(bisProject);
        }


        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] ThirdPartyInspection model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid Third Party Inspection Tracker data." });


                model.CreatedBy = HttpContext.Session.GetString("UserId");
                var result = await _repository.CreateAsync(model);

                if (result.Success)
                {
                    return Json(new { success = true, message = "Third Party Inspection Detail saved successfully." });
                }

                return Json(new { success = false, message = "Failed to save third party inspectiont detail.", id = 0 });

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] ThirdPartyInspection model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = HttpContext.Session.GetString("UserId");

                var operationResult = await _repository.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _repository.DeleteAsync(id);
            return Json(operationResult);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAttachment(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ThirdPartyTrac_Atta", id.ToString());
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
                await _repository.UpdateAttachmentAsync(id, newFileName);

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
