using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.DNTrackerRepository;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class DNTrackerController: Controller
    {
        private readonly IDNTrackerRepository _deviationNoteRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IVendorRepository _vendorRepository;
        public DNTrackerController(IDNTrackerRepository deviationNoteRepository, ISystemLogService systemLogService, IVendorRepository vendorRepository)
        {
            _deviationNoteRepository = deviationNoteRepository;
            _systemLogService = systemLogService;
            _vendorRepository = vendorRepository;
        }

        public IActionResult DNTracker()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _deviationNoteRepository.GetListAsync(startDate, endDate);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var item = await _deviationNoteRepository.GetByIdAsync(id);
            return Json(item);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] DNTracker model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data" });

                var operationResult = new OperationResult();
                bool exists = false; // Optional: Add duplicate check here

                if (!exists)
                {
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = HttpContext.Session.GetString("FullName");

                    operationResult = await _deviationNoteRepository.CreateAsync(model);

                    if (operationResult != null && operationResult.Success)
                        return Json(new { success = true, message = "Saved successfully.", id = operationResult.ObjectId });

                    return Json(new { success = false, message = "Failed to save.", id = 0 });
                }
                else
                {
                    return Json(new { success = false, message = "Duplicate entry." });
                }
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while saving." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] DNTracker model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid update data" });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var result = await _deviationNoteRepository.UpdateAsync(model);

                if (result != null && result.Success)
                    return Json(new { success = true, message = "Updated successfully." });

                return Json(new { success = false, message = "Update failed." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error during update." });
            }
        }
       
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var operationResult = await _deviationNoteRepository.DeleteAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while deleting Deviation Note." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadDNAttachment(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DNTrac_Attach", id.ToString());
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
                await _deviationNoteRepository.UpdateAttachmentAsync(id, newFileName);

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
