using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.CAReportRepository;
using QMS.Core.Repositories.RCAReportRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class CustomerRCAReportController : Controller
    {
        private readonly IRCAReportRepository _rCAReportRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IWebHostEnvironment _env;

        public CustomerRCAReportController(IRCAReportRepository rCAReportRepository, ISystemLogService systemLogService, IWebHostEnvironment env)
        {
            _rCAReportRepository = rCAReportRepository;
            _systemLogService = systemLogService;
            _env = env;
        }
        public IActionResult CustomerRCA()
        {
            return View();
        }
        //public IActionResult CustomerRCADetails()
        //{
        //    return View();
        //}

        public async Task<JsonResult> GetRCAReportAsync(DateTime? startDate, DateTime? endDate)
        {
            var result = await _rCAReportRepository.GetRCAReportAsync(startDate, endDate);
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetRCAReportByIdAsync(int internalTypeId)
        {
            var result = await _rCAReportRepository.GetRCAReportByIdAsync(internalTypeId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CustomerRCADetails(int id)
        {
            RCAReportViewModel model;

            if (id == 0)
            {
                model = new RCAReportViewModel
                {
                    Date = DateTime.Now
                };
            }
            else
            {
                model = await _rCAReportRepository.GetRCAReportByIdAsync(id);

                if (model == null)
                {
                    return NotFound();
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _rCAReportRepository.DeleteAsync(id);
            return Json(operationResult);
        }

        [HttpPost]
        public async Task<IActionResult> InsertRCAReportAsync(RCAReportViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Success = false, Errors = new[] { "Model cannot be null." } });
                }

                // ---- Save uploaded images to folder and set string paths ----
                if (model.Problem_Visual_ImgAFile != null && model.Problem_Visual_ImgAFile.Length > 0)
                {
                    model.Problem_Visual_ImgA = await SaveImageAsync(
                        model.Problem_Visual_ImgAFile, "ProbA", model.Complaint_No);
                }

                if (model.Problem_Visual_ImgBFile != null && model.Problem_Visual_ImgBFile.Length > 0)
                {
                    model.Problem_Visual_ImgB = await SaveImageAsync(
                        model.Problem_Visual_ImgBFile, "ProbB", model.Complaint_No);
                }

                if (model.Problem_Visual_ImgCFile != null && model.Problem_Visual_ImgCFile.Length > 0)
                {
                    model.Problem_Visual_ImgC = await SaveImageAsync(
                        model.Problem_Visual_ImgCFile, "ProbC", model.Complaint_No);
                }

                if (model.Images_Failed_Samples1File != null && model.Images_Failed_Samples1File.Length > 0)
                {
                    model.Images_Failed_Samples1 = await SaveImageAsync(
                        model.Images_Failed_Samples1File, "Faile1", model.Complaint_No);
                }

                if (model.Images_Failed_Samples2File != null && model.Images_Failed_Samples2File.Length > 0)
                {
                    model.Images_Failed_Samples2 = await SaveImageAsync(
                        model.Images_Failed_Samples2File, "Faile2", model.Complaint_No);
                }

                if (model.Images_Failed_Samples3File != null && model.Images_Failed_Samples3File.Length > 0)
                {
                    model.Images_Failed_Samples3 = await SaveImageAsync(
                        model.Images_Failed_Samples3File, "Faile3", model.Complaint_No);
                }

                if (model.Images_Failed_Samples4File != null && model.Images_Failed_Samples4File.Length > 0)
                {
                    model.Images_Failed_Samples4 = await SaveImageAsync(
                        model.Images_Failed_Samples4File, "Faile4", model.Complaint_No);
                }
                // ------------------------------------------------------------

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { Success = false, Errors = errors });
                }

                bool exists = await _rCAReportRepository.CheckDuplicate(model.Complaint_No!.Trim(), 0);
                if (exists)
                {
                    return Json(new { Success = false, Errors = new[] { "RCA Report Detail already exists." } });
                }

                var user = HttpContext.Session.GetString("FullName") ?? "System";
                OperationResult result;

                if (model.Id > 0)
                {
                    model.UpdatedBy = user;

                    result = await _rCAReportRepository.UpdateRCAReportAsync(model)
                             ?? new OperationResult { Success = false, Message = "Update failed." };

                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "Customer RCA Report Detail updated successfully.";
                }
                else
                {
                    model.CreatedBy = user;

                    result = await _rCAReportRepository.InsertRCAReportAsync(model)
                             ?? new OperationResult { Success = false, Message = "Insert failed." };

                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "Customer RCA Report Detail created successfully.";
                }

                return Json(new
                {
                    Success = result.Success,
                    Message = result.Message
                });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                return Json(new
                {
                    Success = false,
                    Errors = new[] { "Failed to save CA report detail." },
                    Exception = ex.Message
                });
            }
        }

        private async Task<string> SaveImageAsync(IFormFile file, string prefix, string complaintNo)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            // Make complaint no safe for folder/file name
            var safeComplaintNo = (complaintNo ?? string.Empty)
                .Replace(" ", "_")
                .Replace("/", "_")
                .Replace("\\", "_")
                .Replace(":", "_");

            // Physical folder path: wwwroot/CAReport_Attach/{ComplaintNo}
            var folderPhysical = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "RCAReport_Attach",
                safeComplaintNo
            );

            if (!Directory.Exists(folderPhysical))
                Directory.CreateDirectory(folderPhysical);

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext))
                ext = ".jpg";

            var fileName = $"{safeComplaintNo}_{prefix}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
            var fullPath = Path.Combine(folderPhysical, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Correct relative path for browser
            // /CAReport_Attach/Complaint123/Complaint123_ProbA_2025.....
            var relativeForDb = $"/RCAReport_Attach/{safeComplaintNo}/{fileName}";

            return relativeForDb;
        }

    }
}
