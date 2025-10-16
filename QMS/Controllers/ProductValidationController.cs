using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.ProductValidationRepo;
using System.Threading.Tasks;

namespace QMS.Controllers;

public class ProductValidationController : Controller
{
    private readonly IProductValidationRepository _productValidationRepository;

    public ProductValidationController(IProductValidationRepository productValidationRepository)
    {
        _productValidationRepository = productValidationRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult PhysicalCheckAndVisualInspection()
    {
        return View();
    }

    public async Task<IActionResult> PhysicalCheckAndVisualInspectionDetails(int Id)
    {
        var model = new PhysicalCheckAndVisualInspectionViewModel();
        if(Id > 0)
        {
            model = await _productValidationRepository.GetPhysicalCheckAndVisualInspectionsByIdAsync(Id);
        }
        else
        {
            model.Report_Date = DateTime.Now;
        }
            return View(model);
    }

    public async Task<ActionResult> GetPhysicalCheckAndVisualInspectionListAsync()
    {
        var result = await _productValidationRepository.GetPhysicalCheckAndVisualInspectionsAsync();
        return Json(result);
    }

    public async Task<ActionResult> GetPhysicalCheckAndVisualInspectionDetailsAsync(int Id)
    {
        var result = await _productValidationRepository.GetPhysicalCheckAndVisualInspectionsByIdAsync(Id);
        return Json(result);
    }

    public async Task<ActionResult> InsertUpdatePhysicalCheckAndVisualInspectionDetailsAsync(PhysicalCheckAndVisualInspectionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        if (model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _productValidationRepository.UpdatePhysicalCheckAndVisualInspectionsAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _productValidationRepository.InsertPhysicalCheckAndVisualInspectionsAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DeletePhysicalCheckAndVisualInspectionAsync(int Id)
    {
        var result = await _productValidationRepository.DeletePhysicalCheckAndVisualInspectionsAsync(Id);
        return Json(result);
    }
}
