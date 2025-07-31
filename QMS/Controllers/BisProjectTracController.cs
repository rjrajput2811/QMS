using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class BisProjectTracController : Controller
    {
        private readonly IBisProjectTracRepository _bisProjectRepository;
        private readonly ISystemLogService _systemLogService;

        public BisProjectTracController(IBisProjectTracRepository bisProjectRepository, ISystemLogService systemLogService)
        {
            _bisProjectRepository = bisProjectRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult BisProjectTracker()
        {
            return View();
        }

        public async Task<IActionResult> BisProjectTrackerDetail(int id)
        {
            var model = new BisProjectTracViewModel();

            if (id > 0)
            {
                model = await _bisProjectRepository.GetByIdAsync(id);
                if (model == null)
                {
                    model = new BisProjectTracViewModel();
                }
            }
            else
            {
                model = new BisProjectTracViewModel();
            }

            return View(model); // Always return the model, whether new or fetched
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var bisProjectList = await _bisProjectRepository.GetListAsync();
            return Json(bisProjectList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var bisProject = await _bisProjectRepository.GetByIdAsync(Id);
            return Json(bisProject);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] BisProject_Tracker model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid Bis Project Tracker data." });


                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _bisProjectRepository.CreateAsync(model);

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
        public async Task<JsonResult> UpdateAsync([FromBody] BisProject_Tracker model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _bisProjectRepository.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _bisProjectRepository.DeleteAsync(id);
            return Json(operationResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetNatProjectDropdown()
        {
            try
            {
                var vendorList = await _bisProjectRepository.GetNatProjectDropdownAsync();
                return Json(vendorList);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return StatusCode(500, "Error retrieving Nat.Project dropdown.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetNatProjectByIdAsync(int Id)
        {
            var instId = await _bisProjectRepository.GetNatProjectByIdAsync(Id);
            return Json(instId);
        }

        [HttpGet]
        public async Task<JsonResult> GetNatProjectAsync()
        {
            var instList = await _bisProjectRepository.GetNatProjectAsync();
            return Json(instList);
        }

        [HttpPost]
        public async Task<JsonResult> CreateNatProjectAsync([FromBody] NatProjectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _bisProjectRepository.CheckNatProjectDuplicate(model.Nat_Project.Trim(), 0);
                    if (!existingResult)
                    {
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        model.CreatedDate = DateTime.Now;
                        operationResult = await _bisProjectRepository.CreateNatProjectAsync(model);
                        return Json(operationResult);
                    }
                    else
                    {
                        operationResult.Message = "Exist";
                        return Json(operationResult);
                    }
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { Success = false, Errors = errors });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateNatProjectAsync([FromBody] NatProjectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _bisProjectRepository.CheckNatProjectDuplicate(model.Nat_Project.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = HttpContext.Session.GetString("FullName");
                        operationResult = await _bisProjectRepository.UpdateNatProjectAsync(model);
                        return Json(operationResult);
                    }
                    else
                    {
                        operationResult.Message = "Exist";
                        return Json(operationResult);
                    }
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { Success = false, Errors = errors });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteNatProjectAsync(int id)
        {
            try
            {
                var operationResult = await _bisProjectRepository.DeleteNatProjectAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadBISAttachment(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BISTrac_Attach",id.ToString());
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
                await _bisProjectRepository.UpdateAttachmentAsync(id, newFileName);

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
