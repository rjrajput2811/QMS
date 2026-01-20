using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.SixSigmaIndicesRepo;
namespace QMS.Controllers
{
    public class SEEIndicesController : Controller
    {
        private readonly ISixSigmaIndicesRepository _sixSigmaIndicesRepository;
        private readonly IWebHostEnvironment _env;


        public SEEIndicesController(ISixSigmaIndicesRepository sixSigmaIndicesRepository,IWebHostEnvironment env)
        {
            _sixSigmaIndicesRepository = sixSigmaIndicesRepository;
            _env = env;
        }

        // LOAD VIEW
        public IActionResult SEEIndices()
        {
            return View();
        }

        public async Task<IActionResult> SEEIndicesDetails(int Id)
        {
            var model = new SixSigmaIndicesViewModel();

            if (Id > 0)
            {
              
                model = await _sixSigmaIndicesRepository.GetSixSigmaIndicesByIdAsync(Id);
            }
            else
            {
          
                model.CreatedDate = DateTime.Now;
            }

            return View(model);
        }


        // LIST ALL
        [HttpGet]
        public async Task<ActionResult> GetSEEIndicesListAsync()
        {
            var result = await _sixSigmaIndicesRepository.GetSixSigmaIndicesAsync();
            return Json(result);
        }

        // DETAILS for EDIT
        [HttpGet]
        public async Task<ActionResult> GetSEEIndicesDetailsByIdAsync(int id)
        {
            var result = await _sixSigmaIndicesRepository.GetSixSigmaIndicesByIdAsync(id);
            return Json(result);
        }

        // CREATE & UPDATE
        [HttpPost]
        public async Task<ActionResult> InsertUpdateSEEIndicesAsync(SixSigmaIndicesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return Json(new { Success = false, Errors = errors });
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (model.Id > 0)
            {
                // UPDATE
                model.UpdatedBy = userId?.ToString();
                model.UpdatedDate = DateTime.Now;

                var result = await _sixSigmaIndicesRepository.UpdateSixSigmaIndicesAsync(model);
                return Json(result);
            }
            else
            {
                // INSERT
                model.CreatedBy = userId?.ToString();
                model.CreatedDate = DateTime.Now;

                var result = await _sixSigmaIndicesRepository.InsertSixSigmaIndicesAsync(model);
                return Json(result);
            }
        }

        // DELETE
        [HttpPost]
        public async Task<ActionResult> DeleteSEEIndicesAsync(int id)
        {
            var result = await _sixSigmaIndicesRepository.DeleteSixSigmaIndicesAsync(id);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> ExportSixSigmaIndicesToExcel(int id)
        {
            var model = await _sixSigmaIndicesRepository.GetSixSigmaIndicesByIdAsync(id);
            if (model == null) return NotFound("Record not found.");

            // ✅ Put template path + exists check in Controller (as you want)
            var templatePath = Path.Combine(_env.WebRootPath, "templates", "53.SEE Indices Format.xlsx");

            if (!System.IO.File.Exists(templatePath))
                return NotFound("Excel template not found at: " + templatePath);

            var bytes = _sixSigmaIndicesRepository.Build(templatePath, model, out var fileName);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }

}
