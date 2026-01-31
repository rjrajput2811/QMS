using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.ElectricalPerformanceRepo;
using QMS.Core.Repositories.ElectricalProtectionRepo;
using QMS.Core.Repositories.ImpactTestRepository;
using QMS.Core.Repositories.PhotometryRepository;
using QMS.Core.Repositories.ProductValidationRepo;
using QMS.Core.Repositories.RippleTestReportRepo;
using Color = System.Drawing.Color;
using Path = System.IO.Path;
using QMS.Core.Repositories.SurgeTestReportRepository;
using System.Drawing;
using System.Threading.Tasks;
using QMS.Core.Repositories.InstallationTrialRepository;
using QMS.Core.Repositories.TemperatureRiseTestRepo;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers;

public class ProductValidationController : Controller
{
    private readonly IPhysicalCheckAndVisualInspectionRepository _physicalCheckAndVisualInspectionRepository;
    private readonly IElectricalPerformanceRepository _electricalPerformanceRepository;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IElectricalProtectionRepository _electricalProtectionRepository;
    private readonly IRippleTestReportRepository _rippleTestReportRepository;
    private readonly ISurgeTestReportRepository _surgeTestRepository;
    private readonly IPhotometryTestRepository _photometryTestRepository;
    private readonly IImpactTestRepository _impactTestRepository;
    private readonly IInstallationTrialRepository _installationTrialRepository;
    private readonly ISystemLogService _systemLogService;
    private readonly ITemperatureRiseTestRepository _temperatureRiseTestRepository;

    public ProductValidationController(
        IPhysicalCheckAndVisualInspectionRepository physicalCheckAndVisualInspectionRepository,
        IElectricalPerformanceRepository electricalPerformanceRepository,
        IWebHostEnvironment hostEnvironment,
        IElectricalProtectionRepository electricalProtectionRepository,
        IRippleTestReportRepository rippleTestReportRepository,
        ISurgeTestReportRepository surgeTestRepository ,
        IPhotometryTestRepository photometryTestRepository,
        IImpactTestRepository impactTestRepository,
        ISystemLogService systemLogService,
        IInstallationTrialRepository installationTrialRepository,
        ITemperatureRiseTestRepository temperatureRiseTestRepository)
    {
        _physicalCheckAndVisualInspectionRepository = physicalCheckAndVisualInspectionRepository;
        _electricalPerformanceRepository = electricalPerformanceRepository;
        _hostEnvironment = hostEnvironment;
        _electricalProtectionRepository = electricalProtectionRepository;
        _rippleTestReportRepository = rippleTestReportRepository;
        _installationTrialRepository = installationTrialRepository;
        _surgeTestRepository = surgeTestRepository;
        _photometryTestRepository = photometryTestRepository;
        _impactTestRepository = impactTestRepository;
        _systemLogService = systemLogService;
        _temperatureRiseTestRepository = temperatureRiseTestRepository;
    }



    public IActionResult Index()
    {
        return View();
    }

    #region PhysicalCheckAndVisualInspection

    public IActionResult PhysicalCheckAndVisualInspection()
    {
        return View();
    }

    public async Task<ActionResult> GetPhysicalCheckAndVisualInspectionListAsync()
    {
        var result = await _physicalCheckAndVisualInspectionRepository.GetPhysicalCheckAndVisualInspectionsAsync();
        return Json(result);
    }

    public IActionResult Electricalprotection()
    {
        return View();
    }
    public async Task<IActionResult> PhysicalCheckAndVisualInspectionDetails(int Id)
    {
        var model = new PhysicalCheckAndVisualInspectionViewModel();
        if (Id > 0)
        {
            model = await _physicalCheckAndVisualInspectionRepository.GetPhysicalCheckAndVisualInspectionsByIdAsync(Id);
        }
        else
        {
            model.Report_Date = DateTime.Now;
        }
        return View(model);
    }

    public async Task<IActionResult> ElectricalProtectionDetails(int Id)
    {
        var model = new ElectricalProtectionViewModel();

        if (Id > 0)
        {
            model = await _electricalProtectionRepository.GetElectricalProtectionByIdAsync(Id);

        }
        else
        {
            model.ReportDate = DateTime.Now;
        }

        return View(model);
    }

    public IActionResult ElectricalPerformance()
    {
        return View();
    }

    public async Task<ActionResult> GetElectricalPerformanceList(DateTime? startDate = null, DateTime? endDate = null)
    {
        var result = await _electricalPerformanceRepository.GetElectricalPerformancesAsync(startDate, endDate);
        return Json(result);
    }

    public async Task<IActionResult> ElectricalPerformanceDetails(int Id)
    {
        var model = new ElectricalPerformanceViewModel();

        if (Id > 0)
        {
            model = await _electricalPerformanceRepository.GetElectricalPerformancesByIdAsync(Id);

        }
        else
        {
            model.ReportDate = DateTime.Now;
        }

        return View(model);
    }


    [HttpPost]
    public async Task<ActionResult> InsertUpdateElectricalPerformanceDetails(ElectricalPerformanceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return Json(new { Success = false, Errors = errors });
        }

        if (model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _electricalPerformanceRepository.UpdateElectricalPerformancesAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _electricalPerformanceRepository.InsertElectricalPerformancesAsync(model);
            return Json(result);
        }
    }

    [HttpPost]
    public async Task<ActionResult> DeleteElectricalPerformance(int id)
    {
        var result = await _electricalPerformanceRepository.DeleteElectricalPerformancesAsync(id);
        return Json(result);
    }

    public IActionResult RippleTestReport()
    {
        return View();
    }

    public async Task<ActionResult> GetRippleTestReportListAsync()
    {
        var result = await _rippleTestReportRepository.GetRippleTestReportsAsync();
        return Json(result);
    }

    public async Task<IActionResult> RippletestreportdetailsAsync(int Id)
    {
        var model = new RippleTestReportViewModel();
        if (Id > 0)
        {
            model = await _rippleTestReportRepository.GetRippleTestReportsByIdAsync(Id);
        }
        return View(model);
    }

    public async Task<ActionResult> InsertUpdateRippleTestReportAsync(RippleTestReportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };


        string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RippleTest_Attach", model.ReportNo.ToString());
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        List<string> newSavedPaths = new List<string>();

        if (model.RippleTestFileAttachedFile != null && model.RippleTestFileAttachedFile.Count > 0)
        {
            foreach (var file in model.RippleTestFileAttachedFile)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName.Replace(",", "_");
                    string savePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    newSavedPaths.Add(savePath);
                }
            }
        }

        string finalPaths = "";

        if (!string.IsNullOrEmpty(model.RemainingImages))
        {
            finalPaths = model.RemainingImages;
        }

        if (newSavedPaths.Count > 0)
        {
            string newPathsJoined = string.Join(",", newSavedPaths);

            if (!string.IsNullOrEmpty(finalPaths))
            {
                finalPaths += "," + newPathsJoined;
            }
            else
            {
                finalPaths = newPathsJoined;
            }
        }

        model.RippleTestFileAttachedPath = finalPaths;

        if (model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _rippleTestReportRepository.UpdateRippleTestReportsAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _rippleTestReportRepository.InsertRippleTestReportsAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DelteRippleTestReportAsync(int id)
    {
        var result = await _rippleTestReportRepository.DeleteRippleTestReportsAsync(id);
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
            var result = await _physicalCheckAndVisualInspectionRepository.UpdatePhysicalCheckAndVisualInspectionsAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _physicalCheckAndVisualInspectionRepository.InsertPhysicalCheckAndVisualInspectionsAsync(model);
            return Json(result);
        }
    }

    

    public async Task<ActionResult> DeletePhysicalCheckAndVisualInspectionAsync(int Id)
    {
        var result = await _physicalCheckAndVisualInspectionRepository.DeletePhysicalCheckAndVisualInspectionsAsync(Id);
        return Json(result);
    }

    public async Task<ActionResult> ExportRippleTestReportToPDFAsync(int Id)
    {
        try
        {
            var model = await _rippleTestReportRepository.GetRippleTestReportsByIdAsync(Id);

            using (MemoryStream stream = new MemoryStream())
            {
                // 2. Initialize PDF Writer & Document
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4);
                document.SetMargins(20, 20, 20, 20);

                // Create Bold Font (Reusing your existing logic)
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // --- MAIN TABLE (One wrapper for everything to get the outer border) ---
                // Columns: Label (1 part) vs Value (2 parts)
                Table mainTable = new Table(new float[] { 1, 2 }).UseAllAvailableWidth();

                // ================= HEADER SECTION =================
                // Create a single cell that spans both columns (1, 2)
                Cell headerContainer = new Cell(1, 2).SetPadding(0);

                // Nested table for Title (Left) and Logo (Right)
                Table innerHeader = new Table(new float[] { 3, 1 }).UseAllAvailableWidth();

                // 1. Title
                Cell titleCell = new Cell().Add(new Paragraph("RIPPLE TEST REPORT")
                    .SetFontSize(18)
                    .SetFont(boldFont)
                    .SetFontColor(ColorConstants.BLACK));
                titleCell.SetBorder(Border.NO_BORDER);
                titleCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                titleCell.SetTextAlignment(TextAlignment.CENTER);
                innerHeader.AddCell(titleCell);

                // 2. Logo
                Cell logoCell = new Cell();
                var webRootPath = _hostEnvironment.WebRootPath;
                var imagePath = Path.Combine(webRootPath, "images", "wipro-logo.png");

                if (System.IO.File.Exists(imagePath))
                {
                    iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(imagePath));
                    logo.SetAutoScale(true);
                    logo.SetMaxHeight(20); // <--- FIXED: Limits height so it remains small and seeable
                    logoCell.Add(logo);
                }
                else
                {
                    logoCell.Add(new Paragraph("Logo"));
                }

                logoCell.SetBorder(Border.NO_BORDER);
                logoCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                logoCell.SetTextAlignment(TextAlignment.RIGHT);
                innerHeader.AddCell(logoCell);

                // Add the nested header table to the main container cell
                headerContainer.Add(innerHeader);
                mainTable.AddCell(headerContainer);

                // ================= DATA ROWS =================
                // Add rows directly to mainTable so they share the border
                AddRow(mainTable, "Report No:", model.ReportNo, boldFont);
                AddRow(mainTable, "Testing date :", model.TestingDate.HasValue ? model.TestingDate.Value.ToString("dd/MM/yyyy") : "", boldFont);
                AddRow(mainTable, "Measuring instrument :", model.MeasuringInstrument, boldFont);
                AddRow(mainTable, "Product Cat Ref", model.ProductCatRef, boldFont);
                AddRow(mainTable, "Product Description :", model.ProductDescription, boldFont);
                AddRow(mainTable, "Batch Code", model.BatchCode, boldFont);
                AddRow(mainTable, "PKD :", model.PKD, boldFont);
                AddRow(mainTable, "LED Details", model.LEDDetails, boldFont);
                AddRow(mainTable, "LED DRIVER", model.LEDDriver, boldFont);
                AddRow(mainTable, "LED COMBINATION", model.LEDCombination, boldFont);

                // Spacer Row (Empty but with side borders to maintain the box look)
                mainTable.AddCell(new Cell(1, 2).SetHeight(10).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));

                // ================= CALCULATION SECTION =================
                var fontPath = Path.Combine(_hostEnvironment.WebRootPath, "fonts", "arialbd.ttf");
                PdfFont boldUnicodeFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                AddCalculationRow(mainTable, "Δ =", model.DeltaValue.HasValue ? model.DeltaValue.Value.ToString("0.00") : "", boldUnicodeFont);
                AddCalculationRow(mainTable, "RMS =", model.RMSValue.HasValue ? model.RMSValue.Value.ToString("0.00") : "", boldFont);
                AddCalculationRow(mainTable, "RIPPLE % =", model.RipplePercentage.HasValue ? model.RipplePercentage.Value.ToString("0.00") : "", boldFont);

                // ================= PHOTOS SECTION =================
                Cell photoCell = new Cell(1, 2); // Do NOT set fixed height here, let it adapt
                photoCell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                photoCell.SetTextAlignment(TextAlignment.CENTER);

                if (!string.IsNullOrEmpty(model.RippleTestFileAttachedPath))
                {
                    var photoPaths = model.RippleTestFileAttachedPath.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // 1. Create a "Gallery" Table with 4 Columns (Adjust number to prefered size)
                    // This forces images to share the width (25% each)
                    Table galleryTable = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
                    bool imageAdded = false;

                    foreach (var rawPath in photoPaths)
                    {
                        // 2. Clean Path Logic (Same as before)
                        string dbPath = rawPath.Trim().Trim('\'').Trim('"');
                        string finalPathToUse = "";

                        if (System.IO.File.Exists(dbPath))
                        {
                            finalPathToUse = dbPath;
                        }
                        else
                        {
                            string fileName = Path.GetFileName(dbPath);
                            string constructedPath = Path.Combine(webRootPath, "RippleTest_Attach", model.ReportNo, fileName);
                            if (System.IO.File.Exists(constructedPath)) finalPathToUse = constructedPath;
                        }

                        // 3. Add Image to Grid
                        if (!string.IsNullOrEmpty(finalPathToUse))
                        {
                            try
                            {
                                byte[] imgBytes = System.IO.File.ReadAllBytes(finalPathToUse);
                                iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(imgBytes));

                                // AUTO-SCALE: This fits the image into the small grid column
                                img.SetAutoScale(true);

                                // Optional: Ensure they aren't too tall if vertical
                                img.SetMaxHeight(100);

                                // Add to a borderless cell in the gallery table
                                Cell imgCell = new Cell().Add(img);
                                imgCell.SetBorder(Border.NO_BORDER);
                                imgCell.SetPadding(5); // Space between photos
                                imgCell.SetTextAlignment(TextAlignment.CENTER);

                                galleryTable.AddCell(imgCell);
                                imageAdded = true;
                            }
                            catch { }
                        }
                    }

                    // 4. Add the Grid to the main Photo Cell
                    if (imageAdded)
                    {
                        // Check if we need to pad the gallery with empty cells to finish the row?
                        // (iText handles this automatically usually, but looks cleaner if we just add the table)
                        photoCell.Add(galleryTable);

                        // Ensure minimum height for consistency
                        photoCell.SetMinHeight(200);
                    }
                    else
                    {
                        photoCell.Add(new Paragraph(new Text("Space for photos")).SetFontColor(ColorConstants.GRAY));
                        photoCell.SetHeight(200);
                    }
                }
                else
                {
                    photoCell.Add(new Paragraph(new Text("Space for photos")).SetFontColor(ColorConstants.GRAY));
                    photoCell.SetHeight(200);
                }

                mainTable.AddCell(photoCell);

                // ================= RESULT SECTION =================
                Cell resultLabelCell = new Cell().Add(new Paragraph("Result ( Pass / Fail )")
                    .SetFontColor(ColorConstants.RED)
                    .SetFont(boldFont));
                mainTable.AddCell(resultLabelCell);

                Cell resultValueCell = new Cell().Add(new Paragraph(model.Result ?? ""));
                mainTable.AddCell(resultValueCell);

                // ================= FOOTER SIGNATURES =================
                Cell footerContainer = new Cell(1, 2).SetPadding(0);
                Table innerFooter = new Table(new float[] { 1, 1 }).UseAllAvailableWidth();

                // Helper to Create Signature Cells
                Cell CreateSignatureCell(string label, string name, bool addRightBorder)
                {
                    Cell cell = new Cell();
                    cell.SetHeight(45); // INCREASED HEIGHT from 50 to 65 to prevent clipping

                    // 1. Label at the TOP
                    cell.Add(new Paragraph(label).SetFontSize(10).SetFont(boldFont));


                    // 3. Name at the BOTTOM (Use check for null)
                    string displayName = !string.IsNullOrEmpty(name) ? name : "____________________";
                    cell.Add(new Paragraph(displayName).SetFontSize(11));

                    // Styling
                    cell.SetVerticalAlignment(VerticalAlignment.BOTTOM); // Aligns the last element to bottom
                    cell.SetBorder(Border.NO_BORDER);

                    if (addRightBorder)
                    {
                        cell.SetBorderRight(new SolidBorder(ColorConstants.BLACK, 1));
                    }

                    return cell;
                }

                // Add Tested By (Left)
                innerFooter.AddCell(CreateSignatureCell("Tested By", model.TestedBy, true));

                // Add Verified By (Right)
                innerFooter.AddCell(CreateSignatureCell("Verified By", model.VerifiedBy, false));

                footerContainer.Add(innerFooter);
                mainTable.AddCell(footerContainer);

                // Add the single main table to doc
                document.Add(mainTable);
                document.Close();

                return File(stream.ToArray(), "application/pdf", $"RippleTestReport_{Id}.pdf");
            }
        }
        catch (Exception ex)
        {
            var exc = new OperationResult
            {
                Success = false,
                Message = ex.Message
            };
            return Json(exc);
        }
    }

    // --- HELPER METHODS (Put these inside your controller class) ---

    private void AddRow(Table table, string label, string value, PdfFont font)
    {
        // Label
        Cell c1 = new Cell().Add(new Paragraph(label).SetFont(font));
        table.AddCell(c1);

        // Value
        Cell c2 = new Cell().Add(new Paragraph(value ?? ""));
        table.AddCell(c2);
    }

    private void AddCalculationRow(Table table, string label, string value, PdfFont font)
    {
        // Label aligned Right
        Cell c1 = new Cell().Add(new Paragraph(label).SetFont(font));
        c1.SetTextAlignment(TextAlignment.RIGHT);
        table.AddCell(c1);

        // Value
        Cell c2 = new Cell().Add(new Paragraph(value ?? ""));
        table.AddCell(c2);
    }

    #endregion
    public async Task<ActionResult> GetElectricalProtectionListAsync()
    {
        var result = await _electricalProtectionRepository.GetElectricalProtectionsAsync();
        return Json(result);
    }

    public async Task<ActionResult> GetElectricalProtectionDetailsAsync(int Id)
    {
        var result = await _electricalProtectionRepository.GetElectricalProtectionByIdAsync(Id);
        return Json(result);
    }
    [HttpPost]
    public async Task<ActionResult> InsertUpdateElectricalProtectionAsync(ElectricalProtectionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return Json(new { Success = false, Errors = errors });
        }

        string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };


        string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ElectricProt_Att", model.ReportNo.ToString());
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);


        if (model.TestedByFile != null)
        {
            string fileName = Path.GetFileName(model.TestedByFile.FileName);
            string ext = Path.GetExtension(fileName).ToLower();

            if (!allowedExtensions.Contains(ext))
            {
                return Json(new { Success = false, Message = "Only .png, .jpg, .jpeg" });
            }


            model.TestedBySignature = fileName;

            string savePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await model.TestedByFile.CopyToAsync(stream);
            }
        }


        if (model.VerifiedByFile != null)
        {
            string fileName = Path.GetFileName(model.VerifiedByFile.FileName);
            string ext = Path.GetExtension(fileName).ToLower();

            if (!allowedExtensions.Contains(ext))
            {
                return Json(new { Success = false, Message = "Only .png, .jpg, .jpeg" });
            }

            model.VerifiedBySignature = fileName;

            string savePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await model.VerifiedByFile.CopyToAsync(stream);
            }
        }


        if (model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _electricalProtectionRepository.UpdateElectricalProtectionAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _electricalProtectionRepository.InsertElectricalProtectionAsync(model);
            return Json(result);
        }
    }
    public async Task<ActionResult> DeleteElectricalProtectionAsync(int Id)
    {
        var result = await _electricalProtectionRepository.DeleteElectricalProtectionAsync(Id);
        return Json(result);
    }
    public async Task<ActionResult> ExportElectricalProtectionToExcelAsync(int Id)
    {
        try
        {
            var data = await _electricalProtectionRepository.GetElectricalProtectionByIdAsync(Id);

            const int COL_SL = 1;
            const int COL_PARAM = 2;
            const int COL_SPEC = 3;
            const int COL_S1 = 4;
            const int COL_S5 = 8;
            const int COL_RESULT = 9;

            // 1. Setup Workbook and Worksheet (using ClosedXML)
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Electrical Protection Report");

            // --- General Report Formatting ---
            worksheet.SheetView.ZoomScale = 82;
            worksheet.Style.Font.FontName = "Aptos Narrow";
            worksheet.Style.Font.FontSize = 12;
            // Setting fixed column widths as requested
            worksheet.Columns("A").Width = 14.80;
            worksheet.Columns("B").Width = 39.10;
            worksheet.Columns("C").Width = 28.40;
            worksheet.Columns("D").Width = 19.80;
            worksheet.Columns("E").Width = 20.20;
            worksheet.Columns("F").Width = 21.30;
            worksheet.Columns("G").Width = 26.10;
            worksheet.Columns("H").Width = 26.10;
            worksheet.Columns("I").Width = 22.80;

            // --- Header Generation (Rows 1-6) ---
            int currentRow = 1;


            const int COL_IMAGE = 9;
            const int COL_TITLE_END = COL_RESULT - 1;


            worksheet.Row(currentRow).Height = 73.20;
            var titleRange = worksheet.Range(currentRow, 1, currentRow, COL_TITLE_END);
            titleRange.Merge();
            titleRange.Value = "Electrical Protection, Safety Test & Reliability Test";
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 22;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            titleRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);



            var webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, "images", "wipro-logo.png");
            if (System.IO.File.Exists(imagePath))
            { var picture = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell(currentRow, COL_IMAGE), 45, 12).WithPlacement(XLPicturePlacement.Move).Scale(0.9); }

            var imageRange = worksheet.Range(currentRow, 9, currentRow, 9);
            imageRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetLeftBorder(XLBorderStyleValues.Thin); currentRow++;


            worksheet.Range(currentRow, 1, currentRow, 3).Merge();
            worksheet.Range(currentRow, 1, currentRow, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Product Cat Ref : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.ProductCatRef);

            worksheet.Range(currentRow, 4, currentRow, 6).Merge();
            worksheet.Range(currentRow, 4, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 4).GetRichText().AddText("Product Description : ").SetBold();
            worksheet.Cell(currentRow, 4).GetRichText().AddText(data.ProductDescription);

            worksheet.Range(currentRow, 7, currentRow, 8).Merge();
            worksheet.Range(currentRow, 7, currentRow, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 7).GetRichText().AddText("Report No. : ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.ReportNo);

            currentRow++;

            // Row 2: Light Source, Driver, Date
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Light Source Details : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.LightSourceDetails);

            worksheet.Range(currentRow, 3, currentRow, 4).Merge();
            worksheet.Range(currentRow, 3, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 3).GetRichText().AddText("Driver Details & Qty : ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.DriverDetailsQty);

            worksheet.Range(currentRow, 5, currentRow, 6).Merge();
            worksheet.Range(currentRow, 5, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Date : ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.ReportDate?.ToShortDateString() ?? "Not Available");

            currentRow++;

            // Row 3: PCB, LED, Batch
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("PCB Details & Qty : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.PCBDetailsQty);

            worksheet.Range(currentRow, 3, currentRow, 4).Merge();
            worksheet.Range(currentRow, 3, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 3).GetRichText().AddText("LED Combinations : ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.LEDCombinations);

            worksheet.Range(currentRow, 5, currentRow, 6).Merge();
            worksheet.Range(currentRow, 5, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Batch Code : ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.BatchCode);

            currentRow++;

            // Row 4: Sensor, Lamp, PKD
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Sensor Details & Qty : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.SensorDetailsQty);

            worksheet.Range(currentRow, 3, currentRow, 4).Merge();
            worksheet.Range(currentRow, 3, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 3).GetRichText().AddText("Lamp Details : ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.LampDetails);

            worksheet.Range(currentRow, 5, currentRow, 6).Merge();
            worksheet.Range(currentRow, 5, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("PKD : ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.PKD);


            // --- 3. Main Table Headers (Rows 7-8) ---
            int headerRowStart = currentRow;

            // Row 7 (Merged Headers)
            worksheet.Range(currentRow, COL_SL, currentRow + 1, COL_SL).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "SL. No.";
            worksheet.Range(currentRow, COL_PARAM, currentRow + 1, COL_PARAM).Merge();
            worksheet.Cell(currentRow, COL_PARAM).Value = "Description Testing";
            worksheet.Range(currentRow, COL_SPEC, currentRow + 1, COL_SPEC).Merge();
            worksheet.Cell(currentRow, COL_SPEC).Value = "Minimum Technical Requirement";
            worksheet.Range(currentRow, COL_S1, currentRow, COL_S5).Merge();
            worksheet.Cell(currentRow, COL_RESULT).Value = "Result - Pass/ Fail";
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.FontColor = XLColor.FromColor(Color.Red);

            currentRow++; // Row 8 (Sample Headers)

            worksheet.Cell(currentRow, COL_S1).Value = "Sample-1";
            worksheet.Cell(currentRow, COL_S1 + 1).Value = "Sample-2";
            worksheet.Cell(currentRow, COL_S1 + 2).Value = "Sample-3";
            worksheet.Cell(currentRow, COL_S1 + 3).Value = "Sample-4";
            worksheet.Cell(currentRow, COL_S1 + 4).Value = "Sample-5";

            // Apply header style to Rows 7 and 8
            var headerRange = worksheet.Range(headerRowStart, COL_SL, currentRow, COL_RESULT);
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerRange.Style.Alignment.WrapText = true;
                headerRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                headerRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            }

            currentRow++; // Starting data entry at Row 9

            // --- 4. Data Entry ---
            int slNo = 1;
            int dataStartRow = currentRow;

            // Helper to insert data columns for one row (SL.No/Parameter handled externally)
            Func<int, string, string, string?, string?, string?, string?, string?, string?, int> AddDataRow =
                (slNumber, paramText1, paramText2, s1, s2, s3, s4, s5, result) =>
                {
                    worksheet.Cell(currentRow, COL_SL).Value = slNumber;
                    // Set the Description Testing and Minimum Technical Requirement texts
                    worksheet.Cell(currentRow, COL_PARAM).Value = paramText1; // Description Testing
                    worksheet.Cell(currentRow, COL_SPEC).Value = paramText2; // Minimum Technical Requirement

                    // Set the sample data and result
                    worksheet.Cell(currentRow, COL_S1).Value = s1 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 1).Value = s2 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 2).Value = s3 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 3).Value = s4 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 4).Value = s5 ?? "";
                    worksheet.Cell(currentRow, COL_RESULT).Value = result ?? "";

                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.WrapText = true;

                    currentRow++;
                    return currentRow;
                };

            // Adding Under Voltage test data with the description as "Under Voltage Cut off & auto recovery" and "As per Design Documents"
            AddDataRow(slNo++, "Under Voltage Cut off & auto recovery", "As per Design Documents",
                       data.UnderVoltage_Sample1, data.UnderVoltage_Sample2,
                       data.UnderVoltage_Sample3, data.UnderVoltage_Sample4,
                       data.UnderVoltage_Sample5, data.UnderVoltage_Result);

            // Adding Over Voltage test data with the description as "Over Voltage Cut off & auto recovery" and "As per Design Documents"
            AddDataRow(slNo++, "Over Voltage Cut off & auto recovery", "As per Design Documents",
                       data.OverVoltage_Sample1, data.OverVoltage_Sample2,
                       data.OverVoltage_Sample3, data.OverVoltage_Sample4,
                       data.OverVoltage_Sample5, data.OverVoltage_Result);

            // Adding Open Circuit Protection test data with the description as "As per Design Documents"
            AddDataRow(slNo++, "Open Circuit Protection", "As per Design Documents",
                       data.OpenCircuit_Sample1, data.OpenCircuit_Sample2,
                       data.OpenCircuit_Sample3, data.OpenCircuit_Sample4,
                       data.OpenCircuit_Sample5, data.OpenCircuit_Result);

            // Adding Short Circuit Protection test data with the description as "Driver goes to shutdown mode"
            AddDataRow(slNo++, "Short Circuit Protection", "Driver goes to shutdown mode",
                       data.ShortCircuit_Sample1, data.ShortCircuit_Sample2,
                       data.ShortCircuit_Sample3, data.ShortCircuit_Sample4,
                       data.ShortCircuit_Sample5, data.ShortCircuit_Result);

            // Adding Reverse Polarity Protection test data with the description as "Driver goes to shutdown mode"
            AddDataRow(slNo++, "Reverse Polarity Protection", "Driver goes to shutdown mode",
                       data.ReversePolarity_Sample1, data.ReversePolarity_Sample2,
                       data.ReversePolarity_Sample3, data.ReversePolarity_Sample4,
                       data.ReversePolarity_Sample5, data.ReversePolarity_Result);

            // Adding Over Load Protection test data with the description as "Driver goes to shutdown mode"
            AddDataRow(slNo++, "Over Load Protection", "Driver goes to shutdown mode",
                       data.OverLoad_Sample1, data.OverLoad_Sample2,
                       data.OverLoad_Sample3, data.OverLoad_Sample4,
                       data.OverLoad_Sample5, data.OverLoad_Result);

            // Adding Over Thermal Cut Off test data with the description as "As per Design Documents"
            AddDataRow(slNo++, "Over Thermal Cut Off", "As per Design Documents",
                       data.OverThermal_Sample1, data.OverThermal_Sample2,
                       data.OverThermal_Sample3, data.OverThermal_Sample4,
                       data.OverThermal_Sample5, data.OverThermal_Result);

            // Adding Earth Fault Protection test data with the description as "Driver should not fail"
            AddDataRow(slNo++, "Earth Fault Protection", "Driver should not fail",
                       data.EarthFault_Sample1, data.EarthFault_Sample2,
                       data.EarthFault_Sample3, data.EarthFault_Sample4,
                       data.EarthFault_Sample5, data.EarthFault_Result);

            // Adding Driver Isolation - Input - Output test data with the description as "As per Design Documents"
            AddDataRow(slNo++, "Driver Isolation - Input - Output", "As per Design Documents",
                       data.DriverIsolation_Sample1, data.DriverIsolation_Sample2,
                       data.DriverIsolation_Sample3, data.DriverIsolation_Sample4,
                       data.DriverIsolation_Sample5, data.DriverIsolation_Result);

            // Adding High Voltage test data with the description as "Should pass as per Driver data sheet"
            AddDataRow(slNo++, "High Voltage -440 V / 320V / 350 V etc.", "Should pass as per Driver data sheet",
                       data.HighVoltage_Sample1, data.HighVoltage_Sample2,
                       data.HighVoltage_Sample3, data.HighVoltage_Sample4,
                       data.HighVoltage_Sample5, data.HighVoltage_Result);

            // Adding HV test data with the description "a) Class 1:- 1.5 kv for a period of 1 Min b) Class 2:- 3.72Kv for a period of 1 Min"
            AddDataRow(slNo++, "HV",
                       "a) Class 1:- 1.5 kv for a period of 1 Min b) Class 2:- 3.72Kv for a period of 1 Min",
                       data.HV_Sample1, data.HV_Sample2,
                       data.HV_Sample3, data.HV_Sample4,
                       data.HV_Sample5, data.HV_Result);

            // Adding Insulation Resistance test data with the description "> 2 MΩ"
            AddDataRow(slNo++, "Insulation Resistance", "> 2 MΩ",
                       data.InsulationResistance_Sample1, data.InsulationResistance_Sample2,
                       data.InsulationResistance_Sample3, data.InsulationResistance_Sample4,
                       data.InsulationResistance_Sample5, data.InsulationResistance_Result);

            // Adding Earth Continuity test data with the description "Continuity"
            AddDataRow(slNo++, "Earth Continuity", "Continuity",
                       data.EarthContinuity_Sample1, data.EarthContinuity_Sample2,
                       data.EarthContinuity_Sample3, data.EarthContinuity_Sample4,
                       data.EarthContinuity_Sample5, data.EarthContinuity_Result);

            // Adding SELV Protection test data with the description "As per Design Documents"
            AddDataRow(slNo++, "SELV Protection", "As per Design Documents",
                       data.SELVProtection_Sample1, data.SELVProtection_Sample2,
                       data.SELVProtection_Sample3, data.SELVProtection_Sample4,
                       data.SELVProtection_Sample5, data.SELVProtection_Result);

            // Adding Leakage Current test data with the description "< 1 Mili Amp"
            AddDataRow(slNo++, "Leakage Current", "< 1 Mili Amp",
                       data.LeakageCurrent_Sample1, data.LeakageCurrent_Sample2,
                       data.LeakageCurrent_Sample3, data.LeakageCurrent_Sample4,
                       data.LeakageCurrent_Sample5, data.LeakageCurrent_Result);

            // Adding Creepage & Clearance test data with the description "As per IS 10322"
            AddDataRow(slNo++, "Creepage & Clearance", "As per IS 10322",
                       data.CreepageClearance_Sample1, data.CreepageClearance_Sample2,
                       data.CreepageClearance_Sample3, data.CreepageClearance_Sample4,
                       data.CreepageClearance_Sample5, data.CreepageClearance_Result);

            // Adding Hi-pot on MCPCB test data with the description "Withstand specified voltages"
            AddDataRow(slNo++, "Hi-pot on MCPCB", "Withstand specified voltages",
                       data.HiPotMCPCB_Sample1, data.HiPotMCPCB_Sample2,
                       data.HiPotMCPCB_Sample3, data.HiPotMCPCB_Sample4,
                       data.HiPotMCPCB_Sample5, data.HiPotMCPCB_Result);

            // Adding On/off switching for 4hrs at 50deg C at 240V AC test data with the description "Driver / PCB should not failed"
            AddDataRow(slNo++, "On/off (5Sec On/5Sec Off) switching for 4hrs at 50deg C at 240V AC",
                       "Driver / PCB should not failed",
                       data.OnOffSwitching_Sample1, data.OnOffSwitching_Sample2,
                       data.OnOffSwitching_Sample3, data.OnOffSwitching_Sample4,
                       data.OnOffSwitching_Sample5, data.OnOffSwitching_Result);

            // Adding Soaking / Ageing at Room temperature test data with the description 
            // "At 240V AC, 60Min: Burning. At 240V AC, 30Min: 10Sec on/off. At 270V AC, 45Min: Burning. At 140V AC, 45Min: Burning."
            AddDataRow(slNo++, "Soaking / Ageing at Room temperature",
                       "At 240V AC, 60Min: Burning\nAt 240V AC, 30Min: 10Sec on/off\nAt 270V AC, 45Min: Burning\nAt 140V AC, 45Min: Burning",
                       data.SoakingAgeing_Sample1, data.SoakingAgeing_Sample2,
                       data.SoakingAgeing_Sample3, data.SoakingAgeing_Sample4,
                       data.SoakingAgeing_Sample5, data.SoakingAgeing_Result);

            // Adding Rolling Endurance Test (Continuous glow for 24x7 days) test data with the description "Daily observations to be taken."
            AddDataRow(slNo++, "Rolling Test/Endurance Test (Continuous glow at least 24x7 days)",
                       "Daily observations to be taken. (PCB & driver should not failed. Check burning of SPDs, LED lens, wires, plastic parts etc.)",
                       data.RollingEndurance_Sample1, data.RollingEndurance_Sample2,
                       data.RollingEndurance_Sample3, data.RollingEndurance_Sample4,
                       data.RollingEndurance_Sample5, data.RollingEndurance_Result);

            // Adding Glow Test test data with the description "Should Lit Up"
            AddDataRow(slNo++, "Glow Test", "Should Lit Up",
                       data.GlowTest_Sample1, data.GlowTest_Sample2,
                       data.GlowTest_Sample3, data.GlowTest_Sample4,
                       data.GlowTest_Sample5, data.GlowTest_Result);

            // Adding Lamp Accommodation test data with the description "No Looseness or Tight"
            AddDataRow(slNo++, "Lamp Accommodation (for Tube)", "No Looseness or Tight",
                       data.LampAccommodation_Sample1, data.LampAccommodation_Sample2,
                       data.LampAccommodation_Sample3, data.LampAccommodation_Sample4,
                       data.LampAccommodation_Sample5, data.LampAccommodation_Result);

            // Adding Dali Function test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Dali Function", "As per Design Documents",
                       data.DaliFunction_Sample1, data.DaliFunction_Sample2,
                       data.DaliFunction_Sample3, data.DaliFunction_Sample4,
                       data.DaliFunction_Sample5, data.DaliFunction_Result);

            // Adding Tuneable CCT test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Tuneable CCT", "As per Design Documents",
                       data.TuneableCCT_Sample1, data.TuneableCCT_Sample2,
                       data.TuneableCCT_Sample3, data.TuneableCCT_Sample4,
                       data.TuneableCCT_Sample5, data.TuneableCCT_Result);

            // Adding Battery Backup test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Battery Back Up", "As per Design Documents",
                       data.BatteryBackup_Sample1, data.BatteryBackup_Sample2,
                       data.BatteryBackup_Sample3, data.BatteryBackup_Sample4,
                       data.BatteryBackup_Sample5, data.BatteryBackup_Result);

            // Adding Smart Lighting test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Smart Lighting (POE, BLE, RF- NEMA etc.)", "As per Design Documents",
                       data.SmartLighting_Sample1, data.SmartLighting_Sample2,
                       data.SmartLighting_Sample3, data.SmartLighting_Sample4,
                       data.SmartLighting_Sample5, data.SmartLighting_Result);

            // Adding Sensor Function test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Sensor Function", "As per Design Documents",
                       data.SensorFunction_Sample1, data.SensorFunction_Sample2,
                       data.SensorFunction_Sample3, data.SensorFunction_Sample4,
                       data.SensorFunction_Sample5, data.SensorFunction_Result);



            int dataEndRow = currentRow - 1; // The row where the last data item was placed

            // Final Result Row (Electronic Details Result - Pass/Fail)
            worksheet.Range(currentRow, COL_SL, currentRow, COL_RESULT).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "Electronic Details Result - Pass/ Fail : ";
            worksheet.Cell(currentRow, COL_SL).Style.Font.Bold = true;
            worksheet.Cell(currentRow, COL_SL).Style.Font.FontColor = XLColor.FromColor(Color.Red);
            worksheet.Cell(currentRow, COL_RESULT).Value = data.OverallReportResult;  // Your result field
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.Bold = true;

            worksheet.Range(7, COL_SL, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            currentRow++; // Move to next row

            // --- Apply All Borders and Styles to the Main Table Data ---
            var dataRange = worksheet.Range(dataStartRow, COL_SL, dataEndRow, COL_RESULT);
            {
                dataRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Alignment.WrapText = true;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            }

            // Apply borders to the Overall Result row
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

            // --- Signatures (Bottom of Report) ---
            currentRow++; // Move to next row

            // Tested By
            worksheet.Range(currentRow, 1, currentRow, 4).Merge();
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Tested By: ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.TestedByName ?? "");  // Your field

            // Verified By
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Verified By: ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.VerifiedByName ?? "");  // Your field




            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            var filename = $"Electrical_Protection_Report_{Id}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    public async Task<ActionResult> ExportPhyCheckToExcelAsync(int Id)
    {
        try
        {
            var data = await _physicalCheckAndVisualInspectionRepository.GetPhysicalCheckAndVisualInspectionsByIdAsync(Id);
            const int COL_SL = 1;
            const int COL_PARAM = 2;
            const int COL_SPEC = 3;
            const int COL_S1 = 4;
            const int COL_S5 = 8;
            const int COL_RESULT = 9;

            // 1. Setup Workbook and Worksheet (using ClosedXML)
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Physical Check Report");

            // --- General Report Formatting ---
            worksheet.SheetView.ZoomScale = 82;
            worksheet.Style.Font.FontName = "Aptos Narrow";
            worksheet.Style.Font.FontSize = 12;
            // Setting fixed column widths as requested
            worksheet.Columns("A").Width = 14.80;
            worksheet.Columns("B").Width = 39.10;
            worksheet.Columns("C").Width = 28.40;
            worksheet.Columns("D").Width = 19.80;
            worksheet.Columns("E").Width = 20.20;
            worksheet.Columns("F").Width = 21.30;
            worksheet.Columns("G").Width = 26.10;
            worksheet.Columns("H").Width = 26.10;
            worksheet.Columns("I").Width = 22.80;

            // --- Header Generation (Rows 1-6) ---
            int currentRow = 1;

            // Defining constants for the title row with image offset
            const int COL_IMAGE = 9;
            const int COL_TITLE_END = COL_RESULT - 1; // Column H (8) for the end of the merged title

            // 1. Adjust the Main Title Merge Range: A1 to H1 (one column less)
            worksheet.Row(currentRow).Height = 73.20;
            var titleRange = worksheet.Range(currentRow, 1, currentRow, COL_TITLE_END);
            titleRange.Merge();
            titleRange.Value = "Physical Check/Visual Inspection Report";
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 22;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            titleRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);

            // Optional: Increase the height of the row to ensure the image fits well.
            worksheet.Row(currentRow).Height = 73.20;

            // 2. Add the Image to the I column (I1)
            var webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, "images", "wipro-logo.png");

            if (System.IO.File.Exists(imagePath))
            {
                var picture = worksheet.AddPicture(imagePath)
                    .MoveTo(worksheet.Cell(currentRow, COL_IMAGE), 45, 12)
                    .WithPlacement(XLPicturePlacement.Move)
                    .Scale(0.9);
            }

            var imageRange = worksheet.Range(currentRow, 9, currentRow, 9);
            imageRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);

            currentRow++; // Row 2
                          // Report No.
            worksheet.Range(currentRow, 1, currentRow, 6).Merge();
            worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 7).GetRichText().AddText("Report No.  ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.Report_No);
            worksheet.Cell(currentRow, 7).Style.Font.FontName = "Arial";
            worksheet.Cell(currentRow, 7).Style.Font.FontSize = 12;
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(currentRow).Height = 22.20;


            currentRow++; // Row 3, 4
                          // Customer/Project Name and Date
            worksheet.Range(currentRow, 1, currentRow + 1, 6).Merge();
            worksheet.Range(currentRow, 1, currentRow + 1, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow + 1, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Customer/Project Name :  ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.Project_Name);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Merge();
            worksheet.Cell(currentRow, 7).GetRichText().AddText("Date :  ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.Report_Date.HasValue
                ? data.Report_Date.Value.ToShortDateString()
                : string.Empty);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 1, currentRow + 1, COL_RESULT).Style.Font.FontName = "Arial";
            worksheet.Range(currentRow, 1, currentRow + 1, COL_RESULT).Style.Font.FontSize = 10;
            worksheet.Row(currentRow + 1).Height = 19.80;

            currentRow += 2; // Row 5
                             // Product Details
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Product Cat Ref :  ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.Product_Cat_Ref);
            worksheet.Range(currentRow, 3, currentRow, 5).Merge();
            worksheet.Range(currentRow, 3, currentRow, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 3, currentRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 3).GetRichText().AddText("Product Description :  ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.Product_Description);
            worksheet.Cell(currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 6).GetRichText().AddText("Batch Code :  ").SetBold();
            worksheet.Cell(currentRow, 6).GetRichText().AddText(data.Batch_Code);
            worksheet.Cell(currentRow, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 7).GetRichText().AddText("PKD :  ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.PKD);
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 8).GetRichText().AddText("Quantity :  ").SetBold();
            worksheet.Cell(currentRow, 8).GetRichText().AddText((data.Quantity ?? 0).ToString());
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Font.FontName = "Arial";
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Font.FontSize = 10;
            worksheet.Row(currentRow).Height = 18.00;

            currentRow++; // Row 6

            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(currentRow).Height = 13.80;


            currentRow++; // Starts main table at Row 7

            // --- 3. Main Table Headers (Rows 7-8) ---
            int headerRowStart = currentRow;

            // Row 7 (Merged Headers)
            worksheet.Range(currentRow, COL_SL, currentRow + 1, COL_SL).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "SL. No.";
            worksheet.Range(currentRow, COL_PARAM, currentRow + 1, COL_PARAM).Merge();
            worksheet.Cell(currentRow, COL_PARAM).Value = "Parameter";
            worksheet.Range(currentRow, COL_SPEC, currentRow + 1, COL_SPEC).Merge();
            worksheet.Cell(currentRow, COL_SPEC).Value = "Specification";
            worksheet.Range(currentRow, COL_S1, currentRow, COL_S5).Merge();
            worksheet.Cell(currentRow, COL_S1).Value = "Observations";
            worksheet.Range(currentRow, COL_RESULT, currentRow + 1, COL_RESULT).Merge();
            worksheet.Cell(currentRow, COL_RESULT).Value = "Result- Pass/ Fail";
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.FontColor = XLColor.FromColor(Color.Red);

            currentRow++; // Row 8 (Sample Headers)

            worksheet.Cell(currentRow, COL_S1).Value = "Sample-1";
            worksheet.Cell(currentRow, COL_S1 + 1).Value = "Sample-2";
            worksheet.Cell(currentRow, COL_S1 + 2).Value = "Sample-3";
            worksheet.Cell(currentRow, COL_S1 + 3).Value = "Sample-4";
            worksheet.Cell(currentRow, COL_S1 + 4).Value = "Sample-5";

            // Apply header style to Rows 7 and 8
            var headerRange = worksheet.Range(headerRowStart, COL_SL, currentRow, COL_RESULT);
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerRange.Style.Alignment.WrapText = true;
                headerRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                headerRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            }

            currentRow++; // Starting data entry at Row 9

            // --- 4. Data Entry ---
            int slNo = 1;
            int dataStartRow = currentRow;

            // Helper to insert data columns for one row (SL.No/Parameter handled externally)
            Func<string, string?, string?, string?, string?, string?, string?, int> AddDataRow =
                (spec, s1, s2, s3, s4, s5, result) =>
                {
                    worksheet.Cell(currentRow, COL_SPEC).Value = spec;
                    worksheet.Cell(currentRow, COL_S1).Value = s1 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 1).Value = s2 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 2).Value = s3 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 3).Value = s4 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 4).Value = s5 ?? "";
                    worksheet.Cell(currentRow, COL_RESULT).Value = result ?? "";
                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.WrapText = true;

                    currentRow++;
                    return currentRow;
                };

            // Helper to merge the SL.No and Parameter columns
            Action<int, int, string, string> MergeColumns = (startRow, endRow, param, slVal) =>
            {
                // SL No.
                worksheet.Range(startRow, COL_SL, endRow, COL_SL).Merge();
                worksheet.Cell(startRow, COL_SL).Value = slVal;
                worksheet.Cell(startRow, COL_SL).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(startRow, COL_SL).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Parameter
                worksheet.Range(startRow, COL_PARAM, endRow, COL_PARAM).Merge();
                worksheet.Cell(startRow, COL_PARAM).Value = param;
                worksheet.Cell(startRow, COL_PARAM).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(startRow, COL_PARAM).Style.Alignment.WrapText = true;

                // Set the row height to adjust contents for the merged cells
                // NOTE: Using a loop to adjust row height after content is added
                for (int i = startRow; i <= endRow; i++)
                {
                    worksheet.Row(i).AdjustToContents();
                }
            };

            // Helper for single-row sections
            Action<string, string, string?, string?, string?, string?, string?, string?> AddSingleRowSection =
                (param, spec, s1, s2, s3, s4, s5, result) =>
                {
                    int startRow = currentRow;
                    AddDataRow(spec, s1, s2, s3, s4, s5, result);
                    MergeColumns(startRow, currentRow - 1, param, slNo.ToString());
                    slNo++;
                };


            // 1. Wipro Branding (Single Row)
            AddSingleRowSection("Wipro Branding on Product & Driver , SPD", "As per design docket",
                data.WiproBranding_Sample1, data.WiproBranding_Sample2, data.WiproBranding_Sample3,
                data.WiproBranding_Sample4, data.WiproBranding_Sample5, data.WiproBranding_Result);

            // 2. Product Label/Driver Label / SPD Labels (Single Row)
            AddSingleRowSection("Product Label/Driver Label / SPD Labels",
                "As per BIS , Dockets/ NPI and readable , Non Erasable, non tearable, or by Laser Sticker should not come out easily.",
                data.ProductDriverLabels_Sample1, data.ProductDriverLabels_Sample2, data.ProductDriverLabels_Sample3,
                data.ProductDriverLabels_Sample4, data.ProductDriverLabels_Sample5, data.ProductDriverLabels_Result);

            // 3. Packing Stickers / Dimension / FIFO Sticker (Single Row)
            AddSingleRowSection("Packing Stickers / Dimension / FIFO Sticker",
                "As per BIS , Dockets/ NPI and readable , Non Erasable, Sticker should not come out easily.",
                data.PackingStickers_Sample1, data.PackingStickers_Sample2, data.PackingStickers_Sample3,
                data.PackingStickers_Sample4, data.PackingStickers_Sample5, data.PackingStickers_Result);

            // 4. Dimensions (Single Row)
            AddSingleRowSection("Dimensions",
                "Length –\nBreadth –\nHeight -\nCut out -\nHousing Pole ID -\nMaterial Thickness-\nMounting bracket dimension-",
                data.Dimensions_Sample1, data.Dimensions_Sample2, data.Dimensions_Sample3,
                data.Dimensions_Sample4, data.Dimensions_Sample5, data.Dimensions_Result);

            // 5. Surface Finish (Multi-Row: 2 rows)
            int startRow_5 = currentRow;

            // 5a. General Surface Finish
            AddDataRow(
                "Free from Burr, Dent , Sharp Edges, No Powder Coating Peel off, Dust free etc. Colour Shade should match",
                data.SurfaceFinish_Sample1, data.SurfaceFinish_Sample2, data.SurfaceFinish_Sample3,
                data.SurfaceFinish_Sample4, data.SurfaceFinish_Sample5, data.SurfaceFinish_Result);

            // 5b. LED/COB Surface Finish
            AddDataRow(
                "LED/COB should be free from dust.",
                data.SurfaceFinishLED_Sample1, data.SurfaceFinishLED_Sample2, data.SurfaceFinishLED_Sample3,
                data.SurfaceFinishLED_Sample4, data.SurfaceFinishLED_Sample5, data.SurfaceFinishLED_Result);

            MergeColumns(startRow_5, currentRow - 1, "Surface Finish", slNo.ToString());
            slNo++;

            // 6. DFT (Single Row)
            AddSingleRowSection("DFT (Dry Film Thickness)",
                "50-120µ",
                data.DFT_Sample1, data.DFT_Sample2, data.DFT_Sample3,
                data.DFT_Sample4, data.DFT_Sample5, data.DFT_Result);

            // 7. Visual (Multi-Row: 2 rows)
            int startRow_7 = currentRow;

            // 7a. Visual (Gaps)
            AddDataRow(
                "Free from gaps at Joinery, Edges, In-between Diffuser/Lens and Housing.",
                data.Visual_Sample1, data.Visual_Sample2, data.Visual_Sample3,
                data.Visual_Sample4, data.Visual_Sample5, data.Visual_Result);

            // 7b. Visual at Lit up Condition
            AddDataRow(
                "No Black Spots Visibility, Band, dark patch /Shadow, Uniformity , NO LED Visibility, NO light leakage, NO blinking, flickering",
                data.VisualLitUp_Sample1, data.VisualLitUp_Sample2, data.VisualLitUp_Sample3,
                data.VisualLitUp_Sample4, data.VisualLitUp_Sample5, data.VisualLitUp_Result);

            MergeColumns(startRow_7, currentRow - 1, "Visual", slNo.ToString());
            slNo++;

            // 8. PCB/COB fitment (Multi-Row: 5 rows)
            int startRow_8 = currentRow;

            // 8a. Thermal Paste
            AddDataRow(
                "Thermal Paste should not be dry, cover entire surface of PCB mounting area",
                data.PcbCobFitment_Sample1, data.PcbCobFitment_Sample2, data.PcbCobFitment_Sample3,
                data.PcbCobFitment_Sample4, data.PcbCobFitment_Sample5, data.PcbCobFitment_Result);

            // 8b. No Gaps
            AddDataRow(
                "No gaps in between Housing and PCB.",
                data.PcbCobFitmentNoGaps_Sample1, data.PcbCobFitmentNoGaps_Sample2, data.PcbCobFitmentNoGaps_Sample3,
                data.PcbCobFitmentNoGaps_Sample4, data.PcbCobFitmentNoGaps_Sample5, data.PcbCobFitmentNoGaps_Result);

            // 8c. Screw
            AddDataRow(
                "PCB holding screw / Button should be intact.",
                data.PcbCobFitmentScrew_Sample1, data.PcbCobFitmentScrew_Sample2, data.PcbCobFitmentScrew_Sample3,
                data.PcbCobFitmentScrew_Sample4, data.PcbCobFitmentScrew_Sample5, data.PcbCobFitmentScrew_Result);

            // 8d. Washer
            AddDataRow(
                "screw or washers should not touches LED",
                data.PcbCobFitmentWasher_Sample1, data.PcbCobFitmentWasher_Sample2, data.PcbCobFitmentWasher_Sample3,
                data.PcbCobFitmentWasher_Sample4, data.PcbCobFitmentWasher_Sample5, data.PcbCobFitmentWasher_Result);

            // 8e. Drawing
            AddDataRow(
                "PCB tracks and dimensions as per drawing.",
                data.PcbCobFitmentDrawing_Sample1, data.PcbCobFitmentDrawing_Sample2, data.PcbCobFitmentDrawing_Sample3,
                data.PcbCobFitmentDrawing_Sample4, data.PcbCobFitmentDrawing_Sample5, data.PcbCobFitmentDrawing_Result);

            MergeColumns(startRow_8, currentRow - 1, "PCB/COB fitment", slNo.ToString());
            slNo++;

            // 9. Soldering (Multi-Row: 3 rows)
            int startRow_9 = currentRow;

            // 9a. Dry
            AddDataRow("Free from Dry soldering",
                data.Soldering_Sample1, data.Soldering_Sample2, data.Soldering_Sample3,
                data.Soldering_Sample4, data.Soldering_Sample5, data.Soldering_Result);

            // 9b. Spatter
            AddDataRow("Free from Spatter",
                data.SolderingSpatter_Sample1, data.SolderingSpatter_Sample2, data.SolderingSpatter_Sample3,
                data.SolderingSpatter_Sample4, data.SolderingSpatter_Sample5, data.SolderingSpatter_Result);

            // 9c. Globule
            AddDataRow("Globule formation with Shiny finish",
                data.SolderingGlobule_Sample1, data.SolderingGlobule_Sample2, data.SolderingGlobule_Sample3,
                data.SolderingGlobule_Sample4, data.SolderingGlobule_Sample5, data.SolderingGlobule_Result);

            MergeColumns(startRow_9, currentRow - 1, "Soldering", slNo.ToString());
            slNo++;

            // 10. Wiring Dressing (Single Row)
            AddSingleRowSection("Wiring & dressing",
                "As per design docket/drawing and without stress.",
                data.WiringDressing_Sample1, data.WiringDressing_Sample2, data.WiringDressing_Sample3,
                data.WiringDressing_Sample4, data.WiringDressing_Sample5, data.WiringDressing_Result);

            // 11. Mechanical Fitment (Single Row)
            AddSingleRowSection("Mechanical fitment with mating part",
                "Smooth and secure fitment as per design",
                data.MechanicalFitment_Sample1, data.MechanicalFitment_Sample2, data.MechanicalFitment_Sample3,
                data.MechanicalFitment_Sample4, data.MechanicalFitment_Sample5, data.MechanicalFitment_Result);

            // 12. LED Lens Gap (Single Row)
            AddSingleRowSection("Gap between LED & Lens",
                "Gap should be there, as per drawing.",
                data.LedLensGap_Sample1, data.LedLensGap_Sample2, data.LedLensGap_Sample3,
                data.LedLensGap_Sample4, data.LedLensGap_Sample5, data.LedLensGap_Result);

            // 13. Gasket (Single Row)
            AddSingleRowSection("Gasket",
                "Fitment & Hardness, proper seating",
                data.Gasket_Sample1, data.Gasket_Sample2, data.Gasket_Sample3,
                data.Gasket_Sample4, data.Gasket_Sample5, data.Gasket_Result);

            // 14. Fragmentation for toughen glass (Single Row)
            AddSingleRowSection("Fragmentation for toughen glass",
                "As per Docket.",
                data.GlassFragmentation_Sample1, data.GlassFragmentation_Sample2, data.GlassFragmentation_Sample3,
                data.GlassFragmentation_Sample4, data.GlassFragmentation_Sample5, data.GlassFragmentation_Result);

            int dataEndRow = currentRow - 1; // The row where the last data item was placed

            // Final Result Row (Row after last data line) - FIXED LABEL HERE
            worksheet.Range(currentRow, COL_SL, currentRow, COL_RESULT).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "Result- Pass/ Fail : ";
            worksheet.Cell(currentRow, COL_SL).Style.Font.Bold = true;
            worksheet.Cell(currentRow, COL_SL).Style.Font.FontColor = XLColor.FromColor(Color.Red);
            worksheet.Cell(currentRow, COL_RESULT).Value = data.Final_Result;
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.Bold = true;


            worksheet.Range(7, COL_SL, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);

            currentRow++;

            // --- 6. Apply All Borders and Styles to the Main Table Data ---
            var dataRange = worksheet.Range(dataStartRow, COL_SL, dataEndRow, COL_RESULT);
            {
                dataRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Alignment.WrapText = true;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            }

            // Apply borders to the Overall Result row
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


            // --- 5. Signatures (Bottom of Report) ---
            currentRow++;

            // Tested By
            worksheet.Range(currentRow, 1, currentRow, 4).Merge();
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Tested by :  ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.TestedBy ?? "");

            // Verified By
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Verified by :  ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.VerifiedBy ?? "");

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            var filename = Id + ".xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
        catch (Exception ex)
        {
            var exc = new OperationResult
            {
                Success = false,
                Message = ex.Message
            };
            return Json(exc);
        }
    }


    #region SurgeTestReport

    public IActionResult SurgeTestReport()
    {
        return View();
    }
    public async Task<IActionResult> SurgeTestReportDetails(int Id)
    {

        var model = new SurgeTestReportViewModel();
        if (Id > 0)
        {
            model = await _surgeTestRepository.GetSurgeTestReportByIdAsync(Id);
        }
        else
        {
            model.ReportDate = DateTime.Now;
        }
        return View(model);
    }

    public async Task<ActionResult> GetSurgeTestReportList()
    {
        var result = await _surgeTestRepository.GetSurgeTestReportAsync();
        return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> InsertUpdateSurgeTestReport(SurgeTestReportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (model.Id > 0)
        {
            // Update
            model.UpdatedBy = userId;
            model.UpdatedOn = DateTime.Now;
        }
        else
        {
            // Insert
            model.AddedBy = userId;
            model.AddedOn = DateTime.Now;
        }

        var result = await _surgeTestRepository.InsertUpdateSurgeTestReportAsync(model);
        return Json(result);
    }

    public async Task<ActionResult> DeleteSurgeTestReport(int Id)
    {
        var result = await _surgeTestRepository.DeleteSurgeTestReportAsync(Id);
        return Json(result);
    }

    #endregion



    #region PhotometryTestReport 
    public IActionResult PhotometryTestReport()
    {
        return View();
    }
    public async Task<ActionResult> PhotometryTestReportDetails(int Id)
    {
        var model = new PhotometryTestReportViewModel();
        if (Id > 0)
        {
            model = await _photometryTestRepository.GetPhotometryTestReportByIdAsync(Id);
        }
        else
        {
            model.ReportDate = DateTime.Now;
        }
        return View(model);
    }

    public async Task<ActionResult> InsertUpdatePhotometryTestAsync(PhotometryTestReportViewModel model)
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
            var result = await _photometryTestRepository.UpdatePhotometryTestReportAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _photometryTestRepository.InsertPhotometryTestReportAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> GetPhotometryTestReportAsync()
    {
        var result = await _photometryTestRepository.GetPhotometryTestReportAsync();
        return Json(result);
    }

    public async Task<ActionResult> DeletePhotometryTestAsync(int Id)
    {
        var result = await _photometryTestRepository.DeletePhotometryTestAsync(Id);
        return Json(result);
    }

    #endregion
    #region InstallationTrial

    public IActionResult InstallationTrialReport()
    {

        return View();
    }

    //public IActionResult InstallationTrialReportDetails()
    //{
    //    return View();
    //}

    public async Task<ActionResult> GetInstallationTrailAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var result = await _installationTrialRepository.GetInstallationTrailAsync(startDate, endDate);
        return Json(result);
    }

    public async Task<IActionResult> InstallationTrialReportDetails(int Id)
    {
        var model = new InstallationTrialViewModel();
        if (Id > 0)
        {
            model = await _installationTrialRepository.GetInstallationTrailByIdAsync(Id);
        }
        else
        {
            model.ReportDate = DateTime.Now;
        }
        return View(model);
    }

    //[HttpPost]
    //public async Task<ActionResult> InsertUpdateInstallationTrailAsync(InstallationTrialViewModel model)
    //{
    //    try
    //    {
    //        if (model == null)
    //            return Json(new { Success = false, Errors = new[] { "Model cannot be null." } });

    //        if (!ModelState.IsValid)
    //        {
    //            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
    //            return Json(new { Success = false, Errors = errors });
    //        }

    //        // =============================
    //        // UPDATE
    //        // =============================
    //        if (model.Id > 0)
    //        {
    //            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
    //            model.UpdatedOn = DateTime.Now;

    //            // Save uploaded images (if any) into /InstallationTrial_Attach/{Id}/
    //            if (model.Photo_WithLoadFile != null && model.Photo_WithLoadFile.Length > 0)
    //            {
    //                model.Photo_WithLoad = await SaveInstallationTrialImageAsync(
    //                    model.Photo_WithLoadFile, "WithLoad", model.Id);
    //            }

    //            if (model.Photo_WithoutLoadFile != null && model.Photo_WithoutLoadFile.Length > 0)
    //            {
    //                model.Photo_WithoutLoad = await SaveInstallationTrialImageAsync(
    //                    model.Photo_WithoutLoadFile, "WithoutLoad", model.Id);
    //            }

    //            var result = await _installationTrialRepository.UpdateInstallationTrailAsync(model);
    //            return Json(result);
    //        }

    //        // =============================
    //        // INSERT
    //        // =============================
    //        model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
    //        model.AddedOn = DateTime.Now;

    //        // 1) Insert first to get NEW ID
    //        var insertResult = await _installationTrialRepository.InsertInstallationTrailAsync(model);

    //        if (insertResult == null)
    //            return Json(new { Success = false, Message = "Insert failed." });

    //        // 2) Get newly generated Id from insertResult
    //        int newId = 0;
    //        try
    //        {
    //            var idProp = insertResult.GetType().GetProperty("Id");
    //            if (idProp != null)
    //                newId = Convert.ToInt32(idProp.GetValue(insertResult));
    //        }
    //        catch { /* ignore */ }

    //        if (newId <= 0)
    //        {
    //            // If your repo doesn't return Id, you MUST return it, otherwise cannot create Id folder.
    //            return Json(new { Success = false, Message = "Insert succeeded but new Id not returned from repository." });
    //        }

    //        model.Id = newId;

    //        // 3) Save images now (if any)
    //        bool anyImageSaved = false;

    //        if (model.Photo_WithLoadFile != null && model.Photo_WithLoadFile.Length > 0)
    //        {
    //            model.Photo_WithLoad = await SaveInstallationTrialImageAsync(
    //                model.Photo_WithLoadFile, "WithLoad", model.Id);
    //            anyImageSaved = true;
    //        }

    //        if (model.Photo_WithoutLoadFile != null && model.Photo_WithoutLoadFile.Length > 0)
    //        {
    //            model.Photo_WithoutLoad = await SaveInstallationTrialImageAsync(
    //                model.Photo_WithoutLoadFile, "WithoutLoad", model.Id);
    //            anyImageSaved = true;
    //        }

    //        // 4) Update DB with image paths (only if saved)
    //        if (anyImageSaved)
    //        {
    //            model.UpdatedBy = model.AddedBy;
    //            model.UpdatedOn = DateTime.Now;

    //            await _installationTrialRepository.UpdateInstallationTrailAsync(model);
    //        }

    //        return Json(insertResult);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Json(new
    //        {
    //            Success = false,
    //            Errors = new[] { "Failed to save Installation Trial detail." },
    //            Exception = ex.Message
    //        });
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> InsertUpdateInstallationTrailAsync(InstallationTrialViewModel model)
    {
        try
        {
            if (model == null)
            {
                return Json(new { Success = false, Errors = new[] { "Model cannot be null." } });
            }

            // ---- Save uploaded images to folder and set string paths ----
            if (model.Photo_WithLoadFile != null && model.Photo_WithLoadFile.Length > 0)
            {
                model.Photo_WithLoad = await SaveImageAsync(
                    model.Photo_WithLoadFile,"InstallationTrial_Attach", "WithLoad", model.ReportNo);
            }

            if (model.Photo_WithoutLoadFile != null && model.Photo_WithoutLoadFile.Length > 0)
            {
                model.Photo_WithoutLoad = await SaveImageAsync(
                    model.Photo_WithoutLoadFile, "InstallationTrial_Attach", "WithoutLoad", model.ReportNo);
            }
           
            // ------------------------------------------------------------

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { Success = false, Errors = errors });
            }

            bool exists;

            if (model.Id > 0)
            {
                // UPDATE: exclude same Id record
                exists = await _installationTrialRepository.CheckDuplicate(
                    model.ReportNo!.Trim(),
                    model.Id
                );
            }
            else
            {
                // INSERT: check if complaint already used anywhere
                exists = await _installationTrialRepository.CheckDuplicate(
                    model.ReportNo!.Trim(),
                    0
                );
            }

            if (exists)
            {
                return Json(new
                {
                    Success = false,
                    Errors = new[] { $"Duplicate Report No '{model.ReportNo}' already exists." }
                });
            }

            var user = HttpContext.Session.GetString("FullName") ?? "System";
            OperationResult result;

            if (model.Id > 0)
            {
                model.UpdatedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
                model.UpdatedOn = DateTime.Now;

                result = await _installationTrialRepository.UpdateInstallationTrailAsync(model)
                         ?? new OperationResult { Success = false, Message = "Update failed." };

                if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                    result.Message = "Installation Trail Detail updated successfully.";
            }
            else
            {
                model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
                model.AddedOn = DateTime.Now;

                result = await _installationTrialRepository.InsertInstallationTrailAsync(model)
                         ?? new OperationResult { Success = false, Message = "Insert failed." };

                if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                    result.Message = "Installation Trail Detail created successfully.";
            }

            return Json(new
            {
                Success = result.Success,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.ToString());
            return Json(new
            {
                Success = false,
                Errors = new[] { "Failed to save installation trail detail." },
                Exception = ex.Message
            });
        }
    }

    //private async Task<string> SaveImageAsync(IFormFile file, string prefix, string complaintNo)
    //{
    //    if (file == null || file.Length == 0)
    //        return string.Empty;

    //    // Make complaint no safe for folder/file name
    //    var safeComplaintNo = (complaintNo ?? string.Empty)
    //        .Replace(" ", "_")
    //        .Replace("/", "_")
    //        .Replace("\\", "_")
    //        .Replace(":", "_");

    //    // Physical folder path: wwwroot/CAReport_Attach/{ComplaintNo}
    //    var folderPhysical = Path.Combine(
    //        Directory.GetCurrentDirectory(),
    //        "wwwroot",
    //        "InstallationTrial_Attach",
    //        safeComplaintNo
    //    );

    //    if (!Directory.Exists(folderPhysical))
    //        Directory.CreateDirectory(folderPhysical);

    //    var ext = Path.GetExtension(file.FileName);
    //    if (string.IsNullOrWhiteSpace(ext))
    //        ext = ".jpg";

    //    var fileName = $"{safeComplaintNo}_{prefix}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
    //    var fullPath = Path.Combine(folderPhysical, fileName);

    //    using (var stream = new FileStream(fullPath, FileMode.Create))
    //    {
    //        await file.CopyToAsync(stream);
    //    }


    //    var relativeForDb = $"/InstallationTrial_Attach/{safeComplaintNo}/{fileName}";

    //    return relativeForDb;
    //}

    private async Task<string> SaveImageAsync(IFormFile file, string baseFolder, string prefix, string referenceNo)
    {
        if (file == null || file.Length == 0)
            return string.Empty;

        var safeRefNo = MakeSafe(referenceNo);

        // Physical: wwwroot/{baseFolder}/{safeRefNo}
        var folderPhysical = Path.Combine(Directory.GetCurrentDirectory(), baseFolder, safeRefNo);

        if (!Directory.Exists(folderPhysical))
            Directory.CreateDirectory(folderPhysical);

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(ext))
            ext = ".jpg";

        var fileName = $"{safeRefNo}_{prefix}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
        var fullPath = Path.Combine(folderPhysical, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // DB path / URL path
        return $"/{baseFolder}/{safeRefNo}/{fileName}".Replace("\\", "/");
    }

    private static string MakeSafe(string input)
    {
        var s = (input ?? string.Empty).Trim();

        foreach (var c in Path.GetInvalidFileNameChars())
            s = s.Replace(c.ToString(), "_");

        s = s.Replace(" ", "_")
             .Replace("/", "_")
             .Replace("\\", "_")
             .Replace(":", "_");

        return string.IsNullOrWhiteSpace(s) ? "NA" : s;
    }

    //private async Task<string> SaveInstallationTrialImageAsync(IFormFile file, string prefix, int installationTrialId)
    //{
    //    if (file == null || file.Length == 0)
    //        return string.Empty;

    //    // Folder: wwwroot/InstallationTrial_Attach/{Id}
    //    var folderPhysical = Path.Combine(
    //        Directory.GetCurrentDirectory(),
    //        "wwwroot",
    //        "InstallationTrial_Attach",
    //        installationTrialId.ToString()
    //    );

    //    if (!Directory.Exists(folderPhysical))
    //        Directory.CreateDirectory(folderPhysical);

    //    var ext = Path.GetExtension(file.FileName);
    //    if (string.IsNullOrWhiteSpace(ext))
    //        ext = ".jpg";

    //    var fileName = $"{installationTrialId}_{prefix}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
    //    var fullPath = Path.Combine(folderPhysical, fileName);

    //    using (var stream = new FileStream(fullPath, FileMode.Create))
    //    {
    //        await file.CopyToAsync(stream);
    //    }

    //    // Return browser path for DB
    //    return $"/InstallationTrial_Attach/{installationTrialId}/{fileName}";
    //}


    public async Task<ActionResult> DeleteInstallationTrailAsync(int Id)
    {
        var result = await _installationTrialRepository.DeleteInstallationTrailAsync(Id);
        return Json(result);
    }

    #endregion
    #region IngressProtection

    public IActionResult impactTest()
    {
        return View();
    }
    public async Task<ActionResult> impactTestDetails(int Id)
    {
        var model = new ImpactTestViewModel();
        if (Id > 0)
        {
            model = await _impactTestRepository.GetImpactTestReportByIdAsync(Id);
        }
        else
        {
            model.ReportDate = DateTime.Now;
        }
        return View(model);
    }

    public async Task<ActionResult> GetImpactTestReportAsync()
    {
        var result = await _impactTestRepository.GetImpactTestReportAsync();
        return Json(result);
    }

    public async Task<ActionResult> InsertUpdateImpactTestAsync(ImpactTestViewModel model)
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
            var result = await _impactTestRepository.UpdateImpactTestReportAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _impactTestRepository.InsertImpactTestReportAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DeleteImpactTestAsync(int Id)
    {
        var result = await _impactTestRepository.DeleteImpactTestAsync(Id);
        return Json(result);
    }

    #endregion
    #region IngressProtection

    public IActionResult IngressProtetction()
    {
        return View();
    }
    public IActionResult IngressProtetctionDetails()
    {
        return View();
    }

    #endregion



    public IActionResult TemperatureRiseTestOfLuminaire()
    {
        return View();
    }

    public async Task<ActionResult> GetTemperatureRiseList(DateTime? startDate = null, DateTime? endDate = null)
    {
        var result = await _electricalPerformanceRepository.GetElectricalPerformancesAsync(startDate, endDate);
        return Json(result);
    }

    public async Task<IActionResult> TemperatureRiseTestOfLuminaireDetails(int Id)
    {
        var model = new TemperatureRiseTestViewModel();

        if (Id > 0)
        {
            model = await _temperatureRiseTestRepository.GetTemperatureRiseTestByIdAsync(Id);

        }
        else
        {
            model.ReportDate = DateTime.Now;
        }

        return View(model);
    }


    [HttpPost]
    public async Task<ActionResult> InsertUpdateTemperatureRiseDetails(TemperatureRiseTestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return Json(new { Success = false, Errors = errors });
        }

        if (model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _temperatureRiseTestRepository.UpdateTemperatureRiseTestAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _temperatureRiseTestRepository.InsertTemperatureRiseTestAsync(model);
            return Json(result);
        }
    }

    [HttpPost]
    public async Task<ActionResult> DeleteTemperatureRise(int id)
    {
        var result = await _temperatureRiseTestRepository.DeleteTemperatureRiseTestAsync(id);
        return Json(result);
    }


    //#endregion

    #region Regulatory
    public IActionResult RegulatoryRequirements()
    {

        return View();
    }

    public IActionResult RegulatoryRequirementsDetails()
    {
        return View();
    }
    #endregion

    #region GlowWireTestReport
    public IActionResult GlowWireTestReport()
    {
        return View();
    }
    public IActionResult GlowWireTestReportDetails()
    {
        return View();
    }

    #endregion

    #region HydraulicTestReport

    public IActionResult HydraulicTestReport()
    {

        return View();
    }

    public IActionResult HydraulicTestReportDetails()
    {

        return View();
    }

    #endregion

    #region NeedleFlameTestReport
    public IActionResult NeedleFlameTestReportDetails()
    {

        return View();
    }
    public IActionResult NeedleFlameTestReport()
    {
        return View();
    }

    #region DropTestReport
    public IActionResult DroptTestDetails()
    {

        return View();
    }
    public IActionResult DropTestReport()
    {
        return View();
    }

    #endregion

    #region GeneralObservation
    public IActionResult GeneralObservationDetails()
    {

        return View();
    }
    public IActionResult GeneralObservation()
    {
        return View();
    }

    #endregion

    #region ValidationReport
    public IActionResult ValidationReport()
    {
        return View();
    }
    public IActionResult ValidationReportDetails()
    {
        return View();
    }
    #endregion

    #region OpenPointClousre

    public IActionResult OpenPointClosureReport()
    {

        return View();
    }

    public IActionResult OpenPointClosureReportDetails()
    {

        return View();
    }

    #endregion
    #endregion
}



