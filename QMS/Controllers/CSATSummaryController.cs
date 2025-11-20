using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.CSATSummaryRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class CSATSummaryController : Controller
    {
        private readonly ICSATSummaryRepository _csatSummaryRepository;
        private readonly ISystemLogService _systemLogService;

        public CSATSummaryController(ICSATSummaryRepository csatSummaryRepository, ISystemLogService systemLogService)
        {
            _csatSummaryRepository = csatSummaryRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult CSATSUMMARY()
        {
            return View();
        }

        //public IActionResult CSATSummaryDetail()
        //{
        //    return View();
        //}

        public async Task<IActionResult> CSATSummaryDetail(int id)
        {
            var model = new Csat_SummaryViewModel();

            if (id > 0)
            {
                model = await _csatSummaryRepository.GetByIdAsync(id);
                if (model == null)
                {
                    model = new Csat_SummaryViewModel();
                }
            }
            else
            {
                model = new Csat_SummaryViewModel();
            }

            return View(model); // Always return the model, whether new or fetched
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var list = await _csatSummaryRepository.GetSummaryAsync(startDate, endDate);
                return Json(list);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error fetching Csat Comment details." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var item = await _csatSummaryRepository.GetByIdAsync(id);
                return Json(item);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error fetching record." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] CSAT_Summary model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data" });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");

                var result = await _csatSummaryRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync([FromBody] CSAT_Summary model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid update data" });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var result = await _csatSummaryRepository.UpdateAsync(model);
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
    }
}
