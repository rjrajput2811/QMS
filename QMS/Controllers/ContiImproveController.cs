using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Repositories.ContiImproveRespository;
using QMS.Core.Repositories.FIFOTrackerRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class ContiImproveController : Controller
    {
        private readonly IContiImproveRespository _contiImproveRespository;
        private readonly ISystemLogService _systemLogService;

        public ContiImproveController(IContiImproveRespository contiImproveRespository, ISystemLogService systemLogService)
        {
            _contiImproveRespository = contiImproveRespository;
            _systemLogService = systemLogService;
        }
        public IActionResult ContinualImprovementTrac()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _contiImproveRespository.GetListAsync(startDate, endDate);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> ContiImproveSummary(string fY)
        {
            var list = await _contiImproveRespository.ContiImproveSummaryAsync(fY);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var data = await _contiImproveRespository.GetByIdAsync(id);
            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] Continual_Improve_Tracker model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid FIFO Tracker data." });


                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _contiImproveRespository.CreateAsync(model);

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
        public async Task<JsonResult> UpdateAsync([FromBody] Continual_Improve_Tracker model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _contiImproveRespository.UpdateAsync(model);

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
                var result = await _contiImproveRespository.DeleteAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while deleting Kaizen record." });
            }
        }
    }
}
