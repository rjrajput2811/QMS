using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Repositories.CSATCommentRepository;
using QMS.Core.Repositories.RMTCDetailsRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class CSATCommentController : Controller
    {
        private readonly ICSATCommentRepository _csatCommentRepository;
        private readonly ISystemLogService _systemLogService;

        public CSATCommentController(ICSATCommentRepository csatCommentRepository, ISystemLogService systemLogService)
        {
            _csatCommentRepository = csatCommentRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult CSATComment()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var list = await _csatCommentRepository.GetListAsync(startDate, endDate);
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
                var item = await _csatCommentRepository.GetByIdAsync(id);
                return Json(item);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error fetching record." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] CSAT_Comment model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data" });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");

                var result = await _csatCommentRepository.CreateAsync(model);
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
        public async Task<JsonResult> UpdateAsync([FromBody] CSAT_Comment model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid update data" });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var result = await _csatCommentRepository.UpdateAsync(model);
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
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var result = await _csatCommentRepository.DeleteAsync(id);
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
