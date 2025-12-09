using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.SixSigmaIndicesRepo;
namespace QMS.Controllers
{
    public class SEEIndicesController : Controller
    {
        private readonly ISixSigmaIndicesRepository _sixSigmaIndicesRepository;

        public SEEIndicesController(ISixSigmaIndicesRepository sixSigmaIndicesRepository)
        {
            _sixSigmaIndicesRepository = sixSigmaIndicesRepository;
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
    }
}
