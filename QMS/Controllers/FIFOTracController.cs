using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.FIFOTrackerRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class FIFOTracController : Controller
    {
        private readonly IFIFOTrackerRepository _fIFOTrackerRepository;
        private readonly ISystemLogService _systemLogService;

        public FIFOTracController(IFIFOTrackerRepository fIFOTrackerRepository, ISystemLogService systemLogService)
        {
            _fIFOTrackerRepository = fIFOTrackerRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult FIFOTracker()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _fIFOTrackerRepository.GetListAsync(startDate, endDate);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var data = await _fIFOTrackerRepository.GetByIdAsync(id);
            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] FIFOTracker model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid FIFO Tracker data." });


                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _fIFOTrackerRepository.CreateAsync(model);

                if (result.Success)
                {
                    return Json(new { success = true, message = "FIFO Tracker Detail saved successfully." });
                }

                return Json(new { success = false, message = "Failed to save fifo tracker detail.", id = 0 });

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] FIFOTracker model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _fIFOTrackerRepository.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }


        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var result = await _fIFOTrackerRepository.DeleteAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while deleting Kaizen record." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFIFOAttachment(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FIFOTrac_Attach", id.ToString());
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
                await _fIFOTrackerRepository.UpdateAttachmentAsync(id, newFileName);

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
