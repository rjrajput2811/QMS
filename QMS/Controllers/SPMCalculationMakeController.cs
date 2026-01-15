using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Repositories.SPMMakeRepository;
using QMS.Core.Repositories.ThirdPartyInspectionRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class SPMCalculationMakeController : Controller
    {
        private readonly ISPMMakeRepository _spmRepository;
        private readonly ISystemLogService _systemLogService;

        public SPMCalculationMakeController(ISPMMakeRepository spmRepository,ISystemLogService systemLogService)
        {
            _spmRepository = spmRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult SPMCalucationMakeDetails()
        {
            return View();
        }
        public IActionResult SPMCalucationMake()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var list = await _spmRepository.GetListAsync();
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(string Fy, List<string> Quater)
        {
            var bisProject = await _spmRepository.GetByIdAsync(Fy, Quater);
            return Json(bisProject);
        }


        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] SPM_Make model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid SPM Make data." });


                model.CreatedBy = HttpContext.Session.GetString("FullName");
                var result = await _spmRepository.CreateAsync(model, returnCreatedRecord: true);

                if (result.Success)
                {
                    return Json(new { success = true, message = "SPM Make Detail saved successfully.", id = result.ObjectId, payload = result.Payload });
                }

                return Json(new { success = false, message = "Failed to save third party inspectiont detail.", id = 0 });

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] SPM_Make model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _spmRepository.UpdateAsync(model, returnUpdatedRecord: true);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _spmRepository.DeleteAsync(id);
            return Json(operationResult);
        }
    }
}
