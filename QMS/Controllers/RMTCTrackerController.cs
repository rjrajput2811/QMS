using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.RMTCDetailsRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class RMTCTrackerController : Controller
    {
        private readonly IRMTCDetailsRepository _rmtcRepository;
        private readonly ISystemLogService _systemLogService;

        public RMTCTrackerController(IRMTCDetailsRepository rmtcRepository, ISystemLogService systemLogService)
        {
            _rmtcRepository = rmtcRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult RMTCTracker()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var list = await _rmtcRepository.GetListAsync(startDate, endDate);
                return Json(list);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error fetching RMTC details." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var item = await _rmtcRepository.GetByIdAsync(id);
                return Json(item);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error fetching record." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] RM_TC_Tracker model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data" });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");

                var result = await _rmtcRepository.CreateAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "Saved successfully.", id = result.ObjectId });

                return Json(new { success = false, message = "Failed to save." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] RM_TC_Tracker model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid update data" });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var result = await _rmtcRepository.UpdateAsync(model);
                if (result.Success)
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
        public async Task<IActionResult> UploadRMTCAttachment(IFormFile file, int id, string field)
        {
            if (id <= 0)
                return Json(new UploadAttachmentResponse { success = false, message = "Invalid Id." });

            if (string.IsNullOrWhiteSpace(field))
                return Json(new UploadAttachmentResponse { success = false, message = "Field is required." });

            if (file == null || file.Length == 0)
                return Json(new UploadAttachmentResponse { success = false, message = "No file received." });

            // validate allowed MIME types
            var allowedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "application/pdf",
                "image/jpeg",
                "image/png",
                "image/gif",
                "image/bmp",
                "image/webp"
            };

            if (!allowedTypes.Contains(file.ContentType))
                return Json(new UploadAttachmentResponse { success = false, message = "Only PDF and image files are allowed." });

            // create folder: wwwroot/RMTCTrac_Attach/{id}
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "RMTCTrac_Attach", id.ToString());
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // sanitize filename
            var ext = Path.GetExtension(file.FileName);
            var safeExt = string.IsNullOrWhiteSpace(ext) ? "" : ext.ToLowerInvariant();

            // limit extensions (optional but recommended)
            var allowedExt = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            if (!allowedExt.Contains(safeExt))
                return Json(new UploadAttachmentResponse { success = false, message = "Invalid file extension." });

            // generate unique file name
            var uniqueName = $"{field}_{DateTime.Now:yyyyMMddHHmmssfff}{safeExt}";
            var fullPath = Path.Combine(folder, uniqueName);

            try
            {
                // save file
                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                // updatedBy from logged in user (adapt to your auth)
                var updatedBy = User?.Identity?.Name ?? "System";

                // update DB column field
                var ok = await _rmtcRepository.UpdateAttachmentFieldAsync(id, field, uniqueName, updatedBy);
                if (!ok)
                {
                    // rollback file save if DB update failed
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);

                    return Json(new UploadAttachmentResponse
                    {
                        success = false,
                        message = "DB update failed (invalid field or record not found)."
                    });
                }

                return Json(new UploadAttachmentResponse
                {
                    success = true,
                    message = "Uploaded",
                    id = id,
                    fileName = uniqueName,
                    field = field
                });
            }
            catch (Exception ex)
            {
                // optionally log ex
                return Json(new UploadAttachmentResponse
                {
                    success = false,
                    message = "Upload failed due to server error."
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var result = await _rmtcRepository.DeleteAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while deleting RMTC record." });
            }
        }
        
    }
}
