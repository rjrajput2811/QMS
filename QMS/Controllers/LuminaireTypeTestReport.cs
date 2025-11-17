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

 
                    // Make room for a taller logo
                    ws.Row(row).Height = 30;
                    ws.Row(row + 1).Height = 30;
                    ws.Row(row + 2).Height = 30;

                    // Create a single rectangular box spanning rows row..row+2 and cols A..E (1..5)
                    var headerBox = ws.Range(row, 1, row + 2, 5);
                    headerBox.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                    headerBox.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Put the header text centered in the merged center area (cols A..D)
                    ws.Range(row, 1, row + 2, 4).Merge();
                    var headerCell = ws.Cell(row, 1);

                    headerCell.Value =
                        "Wipro Enterprises Pvt. Ltd.\n" +
                        "( Consumer Care and Lighting )\n" +
                        "Waluj, Aurangabad:- 431136";

                    headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    headerCell.Style.Alignment.WrapText = true;
                    headerCell.Style.Font.Bold = true;
                    headerCell.Style.Font.FontSize = 13;
                    headerCell.Style.Font.FontName = "Aptos Narrow";

                    // The rightmost column of the box (col 5) will hold the large logo inside the same box
                    try
                    {
                        var webroot = _hostEnvironment.WebRootPath ?? "";
                        var logoPath = Path.Combine(webroot, "images", "wipro-logo.png");

                        if (System.IO.File.Exists(logoPath))
                        {
                            // Make a white background behind the logo to hide Excel grid lines
                            var logoBackground = ws.Range(row, 5, row + 2, 5);
                            logoBackground.Style.Fill.BackgroundColor = XLColor.White;

                            var logoAnchor = ws.Cell(row, 5);

                            var picture = ws.AddPicture(logoPath)
                                            .MoveTo(logoAnchor, -10, 6)
                                            .WithPlacement(XLPicturePlacement.Move);

                            picture.ScaleHeight(1.35);
                            picture.ScaleWidth(1.05);
                        }
                    }
                    
                    catch
                    {
                        // ignore image errors so export still works
                    }

  
                    int titleRow = row + 3; // immediately below headerBox (which used row..row+2)
                    ws.Row(titleRow).Height = 18;

                    // Merge across the same columns A..E
                    var titleRange = ws.Range(titleRow, 1, titleRow, 5);
                    titleRange.Merge();

                    // Make the top border of title match header box bottom (thick)
                    titleRange.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                    titleRange.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    titleRange.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                    // Give a medium bottom border to visually separate title from the rest
                    titleRange.Style.Border.BottomBorder = XLBorderStyleValues.Medium;

                    // Title text centered (uppercase like screenshot)
                    var titleCell = ws.Cell(titleRow, 1);
                    titleCell.Value = "LUMINAIRE TYPE TEST REPORT";
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.Font.FontSize = 12;
                    titleCell.Style.Font.FontName = "Aptos Narrow";
                    titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    titleCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    // Move past the title row to the next row for details box
                    row = titleRow + 1; // next available row

                    int detailsStartRow = row;
                    int detailsEndRow = detailsStartRow + 4; // 5 rows for the fields (ReportNo..InputVoltage)

                    // Set consistent row heights for the details box
                    for (int r = detailsStartRow; r <= detailsEndRow; r++)
                        ws.Row(r).Height = 20;

                    // Create outer box (A..E)
                    var detailsBox = ws.Range(detailsStartRow, 1, detailsEndRow, 5);
                    detailsBox.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                    detailsBox.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    detailsBox.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    detailsBox.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                    detailsBox.Style.Border.InsideBorder = XLBorderStyleValues.Thin;


                    // Row 0: Report No (A-C) and Page or other info (D-E)
                    ws.Range(detailsStartRow, 1, detailsStartRow, 3).Merge();
                    ws.Cell(detailsStartRow, 1).Value = "Report No: " + (data.Report_No ?? "");
                    ws.Cell(detailsStartRow, 1).Style.Font.Bold = true;
                    ws.Cell(detailsStartRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(detailsStartRow, 1).Style.Alignment.WrapText = true;


                    ws.Range(detailsStartRow, 4, detailsStartRow, 5).Merge();


                    ws.Range(detailsStartRow + 1, 1, detailsStartRow + 1, 3).Merge();
                    ws.Cell(detailsStartRow + 1, 1).Value = "Customer Name & Address: " + (data.Cust_Name ?? "");
                    ws.Cell(detailsStartRow + 1, 1).Style.Font.Bold = true;
                    ws.Cell(detailsStartRow + 1, 1).Style.Alignment.WrapText = true;

                    ws.Range(detailsStartRow + 1, 4, detailsStartRow + 1, 5).Merge();
                    ws.Cell(detailsStartRow + 1, 4).Value = "Date: " + (data.Date?.ToString("dd/MM/yyyy") ?? "");
                    ws.Cell(detailsStartRow + 1, 4).Style.Font.Bold = true;
                    ws.Cell(detailsStartRow + 1, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Range(detailsStartRow + 2, 4, detailsStartRow + 4, 5).Merge(); // vertical merge for right-hand block
                    ws.Cell(detailsStartRow + 2, 4).Value = "Sample Identification For Lab: " + (data.Samp_Identi_Lab ?? "");
                    ws.Cell(detailsStartRow + 2, 4).Style.Font.Bold = true;
                    ws.Cell(detailsStartRow + 2, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                    ws.Cell(detailsStartRow + 2, 4).Style.Alignment.WrapText = true;

                    ws.Range(detailsStartRow + 2, 1, detailsStartRow + 2, 3).Merge();
                    ws.Cell(detailsStartRow + 2, 1).Value = "Sample Description: " + (data.Samp_Desc ?? "");
                    ws.Cell(detailsStartRow + 2, 1).Style.Font.Bold = true;
                    ws.Cell(detailsStartRow + 2, 1).Style.Alignment.WrapText = true;
                    ws.Cell(detailsStartRow + 2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                    // Left: Product CAT Code (next row)
                    ws.Range(detailsStartRow + 3, 1, detailsStartRow + 3, 3).Merge();
                    ws.Cell(detailsStartRow + 3, 1).Value = "Product CAT Code: " + (data.Prod_Cat_Code ?? "");
                    ws.Cell(detailsStartRow + 3, 1).Style.Font.Bold = true;
                    ws.Cell(detailsStartRow + 3, 1).Style.Alignment.WrapText = true;

                    // Left: Input Voltage (last row)
                    ws.Range(detailsStartRow + 4, 1, detailsStartRow + 4, 3).Merge();
                    ws.Cell(detailsStartRow + 4, 1).Value = "Input Voltage: " + (data.Input_Voltage ?? "");
                    ws.Cell(detailsStartRow + 4, 1).Style.Font.Bold = true;
                    ws.Cell(detailsStartRow + 4, 1).Style.Alignment.WrapText = true;

                    // Optional: set column widths to make the layout like your screenshot
                    ws.Column(1).Width = 6; // A — adjust as necessary
                    ws.Column(2).Width = 12; // adjust
                    ws.Column(3).Width = 12; // adjust
                    ws.Column(4).Width = 20; // right column wider
                    ws.Column(5).Width = 12;


                    // right side empty
                    ws.Range(detailsStartRow + 4, 4, detailsStartRow + 4, 5).Merge();
                    // Move row pointer to immediately after details box
                    row = detailsEndRow + 1;

                    int refRow = row;
                    ws.Row(refRow).Height = 20;

                    var refRange = ws.Range(refRow, 1, refRow, 5);
                    refRange.Merge();
                    refRange.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                    refRange.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    refRange.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    refRange.Style.Border.RightBorder = XLBorderStyleValues.Thick;

                    string refText = "Reference Standard : " + (data.Ref_Standard ?? "");
                    var refCell = ws.Cell(refRow, 1);
                    refCell.Value = refText;
                    refCell.Style.Font.Bold = true;
                    refCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    refCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    // Move row pointer below the new ref box
                    row = refRow + 1;

                    int tableStartRow = row;

                    ws.Cell(row, 1).Value = "Sr. No.";
                    ws.Cell(row, 2).Value = "Perticular Of Test And  Specification No.";
                    ws.Cell(row, 3).Value = "Test Method";
                    ws.Cell(row, 4).Value = "Test Requirement";
                    ws.Cell(row, 5).Value = "Test Result";
                    ws.Range(row, 1, row, 5).Style.Font.Bold = true;
                    ws.Range(row, 1, row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    row++;

                    int sr = 1;
                    if (data.Details != null)
                    {
                        foreach (var d in data.Details)
                        {
                            ws.Cell(row, 1).Value = sr++;
                            ws.Cell(row, 2).Value = d.Perticular_Test ?? "";
                            ws.Cell(row, 3).Value = d.Test_Method ?? "";
                            ws.Cell(row, 4).Value = StripHtml(d.Test_Requirement ?? "");
                            ws.Cell(row, 5).Value = d.Test_Result ?? "";
                            row++;
                        }
                    }

                    // After writing rows record end row
                    int tableEndRow = row - 1;

                    // Draw a rectangular border around the entire table (parent data box)
                    var tableBox = ws.Range(tableStartRow, 1, tableEndRow, 5);
                    tableBox.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                    tableBox.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    tableBox.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    tableBox.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                    tableBox.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Column widths & wrapping
                    ws.Column(1).Width = 10;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 28;
                    ws.Column(4).Width = 45;
                    ws.Column(5).Width = 22;
                    ws.Column(2).Style.Alignment.WrapText = true;
                    ws.Column(4).Style.Alignment.WrapText = true;

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
