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
    }
}
