using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.PaymentTrackerRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class PaymentTrackerController : Controller
    {
        private readonly IPaymentTracRepository _paymentTracRepository;
        private readonly ISystemLogService _systemLogService;

        public PaymentTrackerController(IPaymentTracRepository paymentTracRepository, ISystemLogService systemLogService)
        {
            _paymentTracRepository = paymentTracRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult PaymentTracker()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var bisProjectList = await _paymentTracRepository.GetListAsync();
            return Json(bisProjectList);
        }


        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var bisProject = await _paymentTracRepository.GetByIdAsync(Id);
            return Json(bisProject);
        }


        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] Payment_Tracker model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid Payment Tracker data." });


                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _paymentTracRepository.CreateAsync(model);

                if (result.Success)
                {
                    return Json(new { success = true, message = "Payment Detail saved successfully." });
                }

                return Json(new { success = false, message = "Failed to save payment detail.", id = 0 });

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }


        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] Payment_Tracker model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _paymentTracRepository.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }


        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _paymentTracRepository.DeleteAsync(id);
            return Json(operationResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetLabDropdown()
        {
            try
            {
                var vendorList = await _paymentTracRepository.GetLabDropdownAsync();
                return Json(vendorList);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return StatusCode(500, "Error retrieving Lab dropdown.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetLabPaymentByIdAsync(int Id)
        {
            var instId = await _paymentTracRepository.GetLabPaymentByIdAsync(Id);
            return Json(instId);
        }

        [HttpGet]
        public async Task<JsonResult> GetLabPaymentAsync()
        {
            var instList = await _paymentTracRepository.GetLabPaymentAsync();
            return Json(instList);
        }

        [HttpPost]
        public async Task<JsonResult> CreateLabPaymentAsync([FromBody] LabPaymentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _paymentTracRepository.CheckLabPaymentDuplicate(model.Lab.Trim(), 0);
                    if (!existingResult)
                    {
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        model.CreatedDate = DateTime.Now;
                        operationResult = await _paymentTracRepository.CreateLabPaymentAsync(model);
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
        public async Task<JsonResult> UpdateLabPaymentAsync([FromBody] LabPaymentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _paymentTracRepository.CheckLabPaymentDuplicate(model.Lab.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = HttpContext.Session.GetString("FullName");
                        operationResult = await _paymentTracRepository.UpdateLabPaymentAsync(model);
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
        public async Task<JsonResult> DeleteLabPaymentAsync(int id)
        {
            try
            {
                var operationResult = await _paymentTracRepository.DeleteLabPaymentAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadPaymentAttachment(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PaymentTrac_Attach", id.ToString());
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
                await _paymentTracRepository.UpdateAttachmentAsync(id, newFileName);

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
