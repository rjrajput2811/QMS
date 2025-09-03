using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.DocumentConfiRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class DocumentDetController : Controller
    {
        private readonly IDocumentConfiRepository _docDetRepository;
        private readonly ISystemLogService _systemLogService;

        public DocumentDetController(IDocumentConfiRepository docDetRepository, ISystemLogService systemLogService)
        {
            _docDetRepository = docDetRepository;
            _systemLogService = systemLogService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetDocDetailByIdAsync(int Id)
        {
            var instId = await _docDetRepository.GetDocDetailByIdAsync(Id);
            return Json(instId);
        }

        [HttpGet]
        public async Task<JsonResult> GetDocDetailByTypeAsync(string type)
        {
            var instId = await _docDetRepository.GetDocDetailByTypeAsync(type);
            return Json(instId);
        }

        [HttpGet]
        public async Task<JsonResult> GetDocDetailAsync()
        {
            var instList = await _docDetRepository.GetDocDetailAsync();
            return Json(instList);
        }

        [HttpPost]
        public async Task<JsonResult> CreateDocDetailAsync(DocumentDetViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _docDetRepository.CheckDocDetailDuplicate(model.Document_No.Trim(),model.Type.Trim(), 0);
                    if (!existingResult)
                    {
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        model.CreatedDate = DateTime.Now;
                        operationResult = await _docDetRepository.CreateDocDetailAsync(model);
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
        public async Task<JsonResult> UpdateDocDetailAsync(DocumentDetViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _docDetRepository.CheckDocDetailDuplicate(model.Document_No.Trim(),model.Type.Trim(), model.Id);
                    if (!existingResult)
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = HttpContext.Session.GetString("FullName");
                        operationResult = await _docDetRepository.UpdateDocDetailAsync(model);
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
        public async Task<JsonResult> DeleteDocDetailAsync(int id)
        {
            try
            {
                var operationResult = await _docDetRepository.DeleteDocDetailAsync(id);
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
