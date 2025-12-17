using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QMS.Core.Models;
using QMS.Core.Repositories.AHPNoteReposotory;
using QMS.Core.Repositories.VendorRepository;

namespace QMS.Controllers;

public class AHPNoteTrackerController : Controller
{
    private readonly IAHPNoteReposotory _ahpNoteRepository;
    private readonly IVendorRepository _vendorRepository;

    public AHPNoteTrackerController(IAHPNoteReposotory ahpNoteRepository, IVendorRepository vendorRepository)
    {
        _ahpNoteRepository = ahpNoteRepository;
        _vendorRepository = vendorRepository;
    }

    public IActionResult AHPNotetracker()
    {
        return View();
    }

    public async Task<ActionResult> GetAHPNoteListAsync(int financialYear)
    {
        var result = await _ahpNoteRepository.GetAHPNotesAsync(financialYear);
        return Json(result);
    }

    public async Task<IActionResult> AHPNotetrackerDetailsAsync(int Id, int financialYear)
    {
        var model = new AHPNoteViewModel();
        var supplierList = await _vendorRepository.GetListAsync();
        var suppliers = supplierList.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        ViewBag.SupplierList = suppliers;
        if(financialYear > 0) { model.FinancialYear = financialYear; }
        if(Id > 0)
        {
            model = await _ahpNoteRepository.GetAHPNotesByIdAsync(Id);
        }
        return View(model);
    }

    public async Task<ActionResult> InsertUpdateAHPNoteAsync(AHPNoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        if(model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _ahpNoteRepository.UpdateAHPNotesAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _ahpNoteRepository.InsertAHPNotesAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DeleteAHPNoteAsync(int Id)
    {
        var result = await _ahpNoteRepository.DeleteAHPNotesAsync(Id);
        return Json(result);
    }
}
