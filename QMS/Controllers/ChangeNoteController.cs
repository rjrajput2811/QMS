using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QMS.Core.Models;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.ChangeNoteService;
using System.Threading.Tasks;

namespace QMS.Controllers;

public class ChangeNoteController : Controller
{
    private readonly IChangeNoteService _changeNoteService;
    private readonly IVendorRepository _vendorRepository;

    public ChangeNoteController(IChangeNoteService changeNoteService,
                                IVendorRepository vendorRepository)
    {
        _changeNoteService = changeNoteService;
        _vendorRepository = vendorRepository;
    }

    public IActionResult ChangeNoteAsync()
    {
        return View();
    }

    public async Task<ActionResult> ChangeNoteListAsync()
    {
        var list = await _changeNoteService.GetChangeNotesListAsync();
        return Json(list);
    }

    public async Task<IActionResult> ChangeNoteDetailsAsync(int Id)
    {
        var model = new ChangeNoteViewModel();
        var vendorList = await _vendorRepository.GetListAsync();
        var vendors = vendorList.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        ViewBag.VendorList = vendors;
        if (Id > 0)
        {
            model = await _changeNoteService.GetChangeNotesDetailsAsync(Id);
        }
        return View(model);
    }

    public async Task<ActionResult> InsertUpdateChangeNoteAsync(ChangeNoteViewModel model)
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
            var result = await _changeNoteService.UpdateChangeNoteAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _changeNoteService.InsertChangeNoteAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DeleteChangeNoteAsync(int Id)
    {
        var result = await _changeNoteService.DeleteChangeNoteAsync(Id);
        return Json(result);
    }
}
