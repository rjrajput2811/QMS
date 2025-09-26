using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Repositories.KaizenTrackerRepository;
using QMS.Core.Repositories.PDIAuthSignRepository;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class PDIAuthSignController : Controller
    {
        private readonly IPDIAuthSignRepository _pdiAuthSignRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IVendorRepository _vendorRepository;

        public PDIAuthSignController(IPDIAuthSignRepository pdiAuthSignRepository,
                                       ISystemLogService systemLogService,
                                       IVendorRepository vendorRepository)
        {
            _pdiAuthSignRepository = pdiAuthSignRepository;
            _systemLogService = systemLogService;
            _vendorRepository = vendorRepository;
        }
        public IActionResult PDIAuthSignatory()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _pdiAuthSignRepository.GetListAsync(startDate, endDate);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var data = await _pdiAuthSignRepository.GetByIdAsync(id);
            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] PDI_Auth_Signatory model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid PDI Authorised Signatory data." });


                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _pdiAuthSignRepository.CreateAsync(model);

                if (result.Success)
                {
                    return Json(new { success = true, message = "PDI Authorised Signatory Detail saved successfully." });
                }

                return Json(new { success = false, message = "Failed to save pdi authorised signatory detail.", id = 0 });

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] PDI_Auth_Signatory model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _pdiAuthSignRepository.UpdateAsync(model);

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
                var result = await _pdiAuthSignRepository.DeleteAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while deleting PDI Authorised Signatory record." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadKaizenAttachment(IFormFile file, int id,string type)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PDIAuthSign_Attach", id.ToString());
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
                await _pdiAuthSignRepository.UpdateAttachmentAsync(id, newFileName, type);

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
