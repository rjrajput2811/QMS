using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // for Session extensions
using QMS.Core.ViewModels;
using QMS.Core.Repositories.InternalTypeTestRepo; // contains IInternalTypeTestRepository
using QMS.Core.Services.SystemLogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using QMS.Core.Models;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using ClosedXML.Excel.Drawings;
using ClosedXML.Excel;

namespace QMS.Controllers
{
    public class LuminaireTypeTestReportController : Controller
    {
        private readonly IInternalTypeTestRepository _internalTypeTestRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IWebHostEnvironment _hostEnvironment;    // <- correctly typed

        public LuminaireTypeTestReportController(
            IInternalTypeTestRepository internalTypeTestRepository,
            ISystemLogService systemLogService,
            IWebHostEnvironment hostEnvironment)    // injected by DI
        {
            _internalTypeTestRepository = internalTypeTestRepository ?? throw new ArgumentNullException(nameof(internalTypeTestRepository));
            _systemLogService = systemLogService ?? throw new ArgumentNullException(nameof(systemLogService));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        public IActionResult LuminaireTypeTest()
        {
            return View();
        }
        public async Task<ActionResult> GetInternalTypeTestListAsync()
        {
            var result = await _internalTypeTestRepository.GetInternalTypeTestsAsync();
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetInternalTypeTestDetailsAsync(int internalTypeId)
        {
            var result = await _internalTypeTestRepository.GetInternalTypeTestByIdAsync(internalTypeId);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> LuminaireTypeTestDetails(int? internalTypeId)
        {
            InternalTypeTestViewModel model;

            if (internalTypeId == null || internalTypeId == 0)
            {
                // Add New Mode
                model = new InternalTypeTestViewModel
                {
                    Date = DateTime.Now
                };
            }
            else
            {
                // Edit Mode
                model = await _internalTypeTestRepository.GetInternalTypeTestByIdAsync(internalTypeId.Value);

                if (model == null)
                {
                    return NotFound();
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteInternalTypeTest(int id)
        {
            try
            {
                var result = await _internalTypeTestRepository.DeleteInternalTypeTestAsync(id);

                if (result.Success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Record deleted successfully."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error deleting record: " + ex.Message
                });
            }
        }
        [HttpPost]

        public async Task<IActionResult> ExportInternalTypeTestExcel(int id)
        {
            try
            {
                var data = await _internalTypeTestRepository.GetInternalTypeTestByIdAsync(id);

                if (data == null)
                    return BadRequest("Data not found.");

                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Luminaire Report");

                    int row = 1;

                    // ---------------------------------------------------------
                    //         WIPRO HEADER BLOCK (AS PER YOUR IMAGE)
                    // ---------------------------------------------------------

                    ws.Row(row).Height = 25;
                    ws.Row(row + 1).Height = 20;
                    ws.Row(row + 2).Height = 20;

                    // Center company text (Columns B → F)
                    ws.Range(row, 2, row + 2, 6).Merge();
                    var headerText = ws.Cell(row, 2);

                    headerText.Value =
                        "Wipro Enterprises Pvt.Ltd.\n" +
                        "( Consumer Care and Lighting )\n" +
                        "Waluj, Aurangabad:- 431136";

                    headerText.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerText.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    headerText.Style.Alignment.WrapText = true;
                    headerText.Style.Font.Bold = true;
                    headerText.Style.Font.FontSize = 13;

                    // Wipro logo (right side)
                    try
                    {
                        var webroot = _hostEnvironment.WebRootPath;
                        var logoPath = Path.Combine(webroot, "images", "wipro-logo.png");

                        if (System.IO.File.Exists(logoPath))
                        {
                            var picture = ws.AddPicture(logoPath)
                                            .MoveTo(ws.Cell(row, 7))
                                            .WithPlacement(XLPicturePlacement.FreeFloating);

                            picture.ScaleHeight(0.50);
                            picture.ScaleWidth(0.50);
                        }
                    }
                    catch { }

                    // side borders
                    ws.Range(row, 2, row + 2, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    ws.Range(row, 6, row + 2, 6).Style.Border.RightBorder = XLBorderStyleValues.Thick;

                    row += 4; // leave gap after header


                    // ---------------------------------------------------------
                    //                YOUR ORIGINAL TITLE (UNCHANGED)
                    // ---------------------------------------------------------
                    ws.Range(row, 1, row, 5).Merge();
                    ws.Cell(row, 1).Value = "Luminaire Type Test Report";
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    ws.Cell(row, 1).Style.Font.FontSize = 16;
                    ws.Cell(row, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    row += 2;


                    // ---------------------------------------------------------
                    //            YOUR ORIGINAL HEADER BLOCK (UNCHANGED)
                    // ---------------------------------------------------------

                    ws.Range(row, 1, row, 3).Merge();
                    ws.Range(row, 4, row, 5).Merge();
                    ws.Cell(row, 1).Value = "Report No: " + (data.Report_No ?? "");
                    ws.Cell(row, 4).Value = "Date: " + (data.Date?.ToString("dd/MM/yyyy") ?? "");
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    ws.Cell(row, 4).Style.Font.Bold = true;
                    row++;

                    ws.Range(row, 1, row, 3).Merge();
                    ws.Range(row, 4, row, 5).Merge();
                    ws.Cell(row, 1).Value = "Customer Name & Address: " + (data.Cust_Name ?? "");
                    ws.Cell(row, 4).Value = "Sample Identification For Lab: " + (data.Samp_Identi_Lab ?? "");
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    ws.Cell(row, 4).Style.Font.Bold = true;
                    row++;

                    ws.Range(row, 1, row, 3).Merge();
                    ws.Range(row, 4, row, 5).Merge();
                    ws.Cell(row, 1).Value = "Sample Description: " + (data.Samp_Desc ?? "");
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    row++;

                    ws.Range(row, 1, row, 3).Merge();
                    ws.Range(row, 4, row, 5).Merge();
                    ws.Cell(row, 1).Value = "Product CAT Code: " + (data.Prod_Cat_Code ?? "");
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    row++;

                    ws.Range(row, 1, row, 3).Merge();
                    ws.Range(row, 4, row, 5).Merge();
                    ws.Cell(row, 1).Value = "Input Voltage: " + (data.Input_Voltage ?? "");
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    row += 2;


                    // ---------------------------------------------------------
                    //                CHILD DETAILS TABLE
                    // ---------------------------------------------------------
                    ws.Cell(row, 1).Value = "Sr. No.";
                    ws.Cell(row, 2).Value = "Particular Of Test";
                    ws.Cell(row, 3).Value = "Test Method";
                    ws.Cell(row, 4).Value = "Test Requirement";
                    ws.Cell(row, 5).Value = "Test Result";
                    ws.Range(row, 1, row, 5).Style.Font.Bold = true;
                    row++;

                    int sr = 1;
                    foreach (var d in data.Details)
                    {
                        ws.Cell(row, 1).Value = sr++;
                        ws.Cell(row, 2).Value = d.Perticular_Test ?? "";
                        ws.Cell(row, 3).Value = d.Test_Method ?? "";
                        ws.Cell(row, 4).Value = StripHtml(d.Test_Requirement ?? "");
                        ws.Cell(row, 5).Value = d.Test_Result ?? "";
                        row++;
                    }

                    ws.Column(2).Width = 40;
                    ws.Column(4).Width = 45;
                    ws.Column(2).Style.Alignment.WrapText = true;
                    ws.Column(4).Style.Alignment.WrapText = true;


                    // ---------------------------------------------------------
                    //                 RETURN FILE
                    // ---------------------------------------------------------

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;

                        return File(
                            stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"Luminaire_Type_Test_{id}.xlsx"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error exporting Excel: " + ex.Message);
            }
        }


        // Remove HTML tags from Requirement column
        private string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";
            return Regex.Replace(html, "<.*?>", string.Empty);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
  
        public async Task<IActionResult> InsertInternalTypeTestAsync(InternalTypeTestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { Success = false, Errors = errors });
                }

                // Get current user id from session (fall back to "System")
                var userId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "System";

                OperationResult result;

                if (model == null)
                {
                    return Json(new { Success = false, Errors = new[] { "Model cannot be null." } });
                }

                if (model.Id > 0)
                {
                    // Update path
                    model.UpdatedBy = userId;
                    model.UpdatedDate = DateTime.Now;

                    // Make sure repository has UpdateInternalTypeTestAsync implemented
                    result = await _internalTypeTestRepository.UpdateInternalTypeTestAsync(model);
                    if (result == null) // defensive
                        result = new OperationResult { Success = false, Message = "Update failed." };

                    // Optionally adjust message
                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "Record updated successfully.";
                }
                else
                {
                    // Insert path
                    model.CreatedBy = userId;
                    model.CreatedDate = DateTime.Now;

                    result = await _internalTypeTestRepository.InsertInternalTypeTestAsync(model);
                    if (result == null)
                        result = new OperationResult { Success = false, Message = "Insert failed." };

                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "Record created successfully.";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { Success = false, Errors = new[] { "Failed to save internal type test." }, Exception = ex.Message });
            }
        }
    }
}
