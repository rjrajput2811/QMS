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
                    return Json(new { success = false, message = "Invalid Third Party Test Tracker data." });


                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _thirdPartyTestRepository.CreateAsync(model);

                if (result.Success)
                {
                    return Json(new { success = true, message = "Third Party Test Detail saved successfully." });
                }

                return Json(new { success = false, message = "Failed to save third party test detail.", id = 0 });

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

        [HttpGet]
        public async Task<IActionResult> GetPurposeDropdown()
        {
            try
            {
                var purposeList = await _thirdPartyTestRepository.GetPurposeDropdownAsync();
                return Json(purposeList);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return StatusCode(500, "Error retrieving Lab dropdown.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPurposeTPTByIdAsync(int Id)
        {
            var instId = await _thirdPartyTestRepository.GetPurposeTPTByIdAsync(Id);
            return Json(instId);
        }

        [HttpGet]
        public async Task<JsonResult> GetPurposeTPTAsync()
        {
            var instList = await _thirdPartyTestRepository.GetPurposeTPTAsync();
            return Json(instList);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePurposeTPTAsync([FromBody] PurposeTPTViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _thirdPartyTestRepository.CheckPurposeTPTDuplicate(model.Purpose.Trim(), 0);
                    if (!existingResult)
                    {
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        model.CreatedDate = DateTime.Now;
                        operationResult = await _thirdPartyTestRepository.CreatePurposeTPTAsync(model);
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
        public async Task<JsonResult> UpdatePurposeTPTAsync([FromBody] PurposeTPTViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _thirdPartyTestRepository.CheckPurposeTPTDuplicate(model.Purpose.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = HttpContext.Session.GetString("FullName");
                        operationResult = await _thirdPartyTestRepository.UpdatePurposeTPTAsync(model);
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
        public async Task<JsonResult> DeletePurposeTPTAsync(int id)
        {
            try
            {
                var operationResult = await _thirdPartyTestRepository.DeletePurposeTPTAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }

        }



        [HttpGet]
        public async Task<IActionResult> GetProjectInitDropdown()
        {
            try
            {
                var purposeList = await _thirdPartyTestRepository.GetProjectInitDropdownAsync();
                return Json(purposeList);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return StatusCode(500, "Error retrieving Lab dropdown.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProjectInitByIdAsync(int Id)
        {
            var instId = await _thirdPartyTestRepository.GetProjectInitByIdAsync(Id);
            return Json(instId);
        }

        [HttpGet]
        public async Task<JsonResult> GetProjectInitAsync()
        {
            var instList = await _thirdPartyTestRepository.GetProjectInitAsync();
            return Json(instList);
        }

        [HttpPost]
        public async Task<JsonResult> CreateProjectInitAsync([FromBody] ProjectInitTPTViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _thirdPartyTestRepository.CheckProjectInitDuplicate(model.Project_Init.Trim(), 0);
                    if (!existingResult)
                    {
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        model.CreatedDate = DateTime.Now;
                        operationResult = await _thirdPartyTestRepository.CreateProjectInitAsync(model);
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
        public async Task<JsonResult> UpdateProjectInitAsync([FromBody] ProjectInitTPTViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _thirdPartyTestRepository.CheckProjectInitDuplicate(model.Project_Init.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = HttpContext.Session.GetString("FullName");
                        operationResult = await _thirdPartyTestRepository.UpdateProjectInitAsync(model);
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
        public async Task<JsonResult> DeleteProjectInitAsync(int id)
        {
            try
            {
                var operationResult = await _thirdPartyTestRepository.DeleteProjectInitAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }

        }



        [HttpGet]
        public async Task<IActionResult> GetTestDetDropdown()
        {
            try
            {
                var purposeList = await _thirdPartyTestRepository.GetTestDetDropdownAsync();
                return Json(purposeList);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return StatusCode(500, "Error retrieving Lab dropdown.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTestDetTPTByIdAsync(int Id)
        {
            var instId = await _thirdPartyTestRepository.GetTestDetTPTByIdAsync(Id);
            return Json(instId);
        }

        [HttpGet]
        public async Task<JsonResult> GetTestDetTPTAsync()
        {
            var instList = await _thirdPartyTestRepository.GetTestDetTPTAsync();
            return Json(instList);
        }

        [HttpPost]
        public async Task<JsonResult> CreateTestDetTPTAsync([FromBody] TestDetTPTViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _thirdPartyTestRepository.CheckTestDetTPTDuplicate(model.Test_Det.Trim(), 0);
                    if (!existingResult)
                    {
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        model.CreatedDate = DateTime.Now;
                        operationResult = await _thirdPartyTestRepository.CreateTestDetTPTAsync(model);
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
        public async Task<JsonResult> UpdateTestDetTPTAsync([FromBody] TestDetTPTViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _thirdPartyTestRepository.CheckTestDetTPTDuplicate(model.Test_Det.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = HttpContext.Session.GetString("FullName");
                        operationResult = await _thirdPartyTestRepository.UpdateTestDetTPTAsync(model);
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
        public async Task<JsonResult> DeleteTestDetTPTAsync(int id)
        {
            try
            {
                var operationResult = await _thirdPartyTestRepository.DeleteTestDetTPTAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }

        }
    }
}
