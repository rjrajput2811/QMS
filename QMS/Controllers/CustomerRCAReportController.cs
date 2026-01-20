using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.CAReportRepository;
using QMS.Core.Repositories.RCAReportRepository;
using QMS.Core.Services.SystemLogs;
using System.Drawing;

namespace QMS.Controllers
{
    public class CustomerRCAReportController : Controller
    {
        private readonly IRCAReportRepository _rCAReportRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IWebHostEnvironment _env;

        public CustomerRCAReportController(IRCAReportRepository rCAReportRepository, ISystemLogService systemLogService, IWebHostEnvironment env)
        {
            _rCAReportRepository = rCAReportRepository;
            _systemLogService = systemLogService;
            _env = env;
        }
        public IActionResult CustomerRCA()
        {
            return View();
        }
        //public IActionResult CustomerRCADetails()
        //{
        //    return View();
        //}

        public async Task<JsonResult> GetRCAReportAsync(DateTime? startDate, DateTime? endDate)
        {
            var result = await _rCAReportRepository.GetRCAReportAsync(startDate, endDate);
            return Json(result);
        }

        public async Task<JsonResult> GetRCATrackingAsync(DateTime? startDate, DateTime? endDate)
        {
            var result = await _rCAReportRepository.GetRCATrackingAsync(startDate, endDate);
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetRCAReportByIdAsync(int internalTypeId)
        {
            var result = await _rCAReportRepository.GetRCAReportByIdAsync(internalTypeId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CustomerRCADetails(int id)
        {
            RCAReportViewModel model = new RCAReportViewModel();

            //if (id == 0)
            //{
            //    model = new RCAReportViewModel
            //    {
            //        Date = DateTime.Now
            //    };
            //}
            //else
            //{
            //    model = await _rCAReportRepository.GetRCAReportByIdAsync(id);

            //    if (model == null)
            //    {
            //        return NotFound();
            //    }
            //}

            if (id > 0)
            {
                // Fetch existing record
                model = await _rCAReportRepository.GetRCAReportByIdAsync(id);

                if (model == null)
                {
                    return NotFound($"CA Report not found for Id: {id}");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _rCAReportRepository.DeleteAsync(id);
            return Json(operationResult);
        }

        [HttpPost]
        public async Task<IActionResult> InsertRCAReportAsync(RCAReportViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Success = false, Errors = new[] { "Model cannot be null." } });
                }

                // ---- Save uploaded images to folder and set string paths ----
                if (model.Problem_Visual_ImgAFile != null && model.Problem_Visual_ImgAFile.Length > 0)
                {
                    model.Problem_Visual_ImgA = await SaveImageAsync(
                        model.Problem_Visual_ImgAFile, "ProbA", model.Complaint_No);
                }

                if (model.Problem_Visual_ImgBFile != null && model.Problem_Visual_ImgBFile.Length > 0)
                {
                    model.Problem_Visual_ImgB = await SaveImageAsync(
                        model.Problem_Visual_ImgBFile, "ProbB", model.Complaint_No);
                }

                if (model.Problem_Visual_ImgCFile != null && model.Problem_Visual_ImgCFile.Length > 0)
                {
                    model.Problem_Visual_ImgC = await SaveImageAsync(
                        model.Problem_Visual_ImgCFile, "ProbC", model.Complaint_No);
                }

                if (model.Images_Failed_Samples1File != null && model.Images_Failed_Samples1File.Length > 0)
                {
                    model.Images_Failed_Samples1 = await SaveImageAsync(
                        model.Images_Failed_Samples1File, "Faile1", model.Complaint_No);
                }

                if (model.Images_Failed_Samples2File != null && model.Images_Failed_Samples2File.Length > 0)
                {
                    model.Images_Failed_Samples2 = await SaveImageAsync(
                        model.Images_Failed_Samples2File, "Faile2", model.Complaint_No);
                }

                if (model.Images_Failed_Samples3File != null && model.Images_Failed_Samples3File.Length > 0)
                {
                    model.Images_Failed_Samples3 = await SaveImageAsync(
                        model.Images_Failed_Samples3File, "Faile3", model.Complaint_No);
                }

                if (model.Images_Failed_Samples4File != null && model.Images_Failed_Samples4File.Length > 0)
                {
                    model.Images_Failed_Samples4 = await SaveImageAsync(
                        model.Images_Failed_Samples4File, "Faile4", model.Complaint_No);
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
                    exists = await _rCAReportRepository.CheckDuplicate(
                        model.Complaint_No!.Trim(),
                        model.Id
                    );
                }
                else
                {
                    // INSERT: check if complaint already used anywhere
                    exists = await _rCAReportRepository.CheckDuplicate(
                        model.Complaint_No!.Trim(),
                        0
                    );
                }

                if (exists)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new[] { $"Duplicate RCA Complaint No '{model.Complaint_No}' already exists." }
                    });
                }
                //bool exists = await _rCAReportRepository.CheckDuplicate(model.Complaint_No!.Trim(), 0);
                //if (exists)
                //{
                //    return Json(new { Success = false, Errors = new[] { "RCA Report Detail already exists." } });
                //}

                var user = HttpContext.Session.GetString("FullName") ?? "System";
                OperationResult result;

                if (model.Id > 0)
                {
                    model.UpdatedBy = user;

                    result = await _rCAReportRepository.UpdateRCAReportAsync(model)
                             ?? new OperationResult { Success = false, Message = "Update failed." };

                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "Customer RCA Report Detail updated successfully.";
                }
                else
                {
                    model.CreatedBy = user;

                    result = await _rCAReportRepository.InsertRCAReportAsync(model)
                             ?? new OperationResult { Success = false, Message = "Insert failed." };

                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "Customer RCA Report Detail created successfully.";
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
                    Errors = new[] { "Failed to save CA report detail." },
                    Exception = ex.Message
                });
            }
        }

        private async Task<string> SaveImageAsync(IFormFile file, string prefix, string complaintNo)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            // Make complaint no safe for folder/file name
            var safeComplaintNo = (complaintNo ?? string.Empty)
                .Replace(" ", "_")
                .Replace("/", "_")
                .Replace("\\", "_")
                .Replace(":", "_");

            // Physical folder path: wwwroot/CAReport_Attach/{ComplaintNo}
            var folderPhysical = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "RCAReport_Attach",
                safeComplaintNo
            );

            if (!Directory.Exists(folderPhysical))
                Directory.CreateDirectory(folderPhysical);

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext))
                ext = ".jpg";

            var fileName = $"{safeComplaintNo}_{prefix}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
            var fullPath = Path.Combine(folderPhysical, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Correct relative path for browser
            // /CAReport_Attach/Complaint123/Complaint123_ProbA_2025.....
            var relativeForDb = $"/RCAReport_Attach/{safeComplaintNo}/{fileName}";

            return relativeForDb;
        }

        // [HttpGet]
        // public async Task<IActionResult> ExportCAReportToExcel(int id)
        // {
        //     var model = await _rCAReportRepository.GetRCAReportByIdAsync(id);
        //     if (model == null)
        //         return NotFound();

        //     // 1. Load template
        //     var templatePath = Path.Combine(_env.WebRootPath, "templates", "15.Customer RCA.xlsx");

        //     if (!System.IO.File.Exists(templatePath))
        //         return NotFound("Customer RCA template not found at " + templatePath);

        //     using var wb = new XLWorkbook(templatePath);

        //     // Use sheet index or exact sheet name from template
        //     var ws = wb.Worksheet(1);  // or wb.Worksheet("CA Format");

        //     // 2. Fill fields – ONLY set .Value, don’t change style/merge/width

        //     // These cell addresses must match your template layout.
        //     // (Below are typical positions – adjust if any cell is different.)

        //     // Complaint No (top row, left)
        //     ws.Cell("B11").Value = "Complaint No:- " + model.Complaint_No ?? "";

        //     // Reported Date (top row, right)
        //     ws.Cell("G11").Value = "Reported Date :" + model.Report_Date?.ToString("dd-MMM-yyyy") ?? "";

        //     ws.Cell("B12").Value = model.Cust_Complaints
        //         ? "✓ Customer complaints" : "Customer complaintse";

        //     ws.Cell("F12").Value = model.NPI_Validations
        //         ? "✓ NPI Validations" : "NPI Validations";

        //     ws.Cell("G12").Value = model.PDI_Obser
        //         ? "✓ PDI Observations." : "PDI Observations.";

        //     ws.Cell("H12").Value = model.System
        //         ? "✓ System/Process Improvements." : "System/Process Improvements.";

        //     // Customer Name & Location
        //     ws.Cell("B13").Value = "Customer Name and Location: -  " + model.Cust_Name_Location ?? "";

        //     // Source of Complaint
        //     ws.Cell("B14").Value = "Source of Complaint:-" + model.Source_Complaint ?? "";

        //     // Product Code & Description
        //     ws.Cell("B15").Value = "Product Code and Description:- " + model.Prod_Code_Desc ?? "";

        //     ws.Cell("B16").Value = "Description of Complaint:-" + model.Desc_Complaint ?? "";

        //     // Batch Code
        //     ws.Cell("B17").Value = "Batch Code:- " + model.Batch_Code ?? "";

        //     // PKD
        //     ws.Cell("H17").Value = "PKD:- " + model.Pkd ?? "";

        //     // Supplied Qty
        //     ws.Cell("B18").Value = "Supplied Qty: - " + model.Supp_Qty?.ToString() ?? "";

        //     // Failure Qty
        //     ws.Cell("H18").Value = "Failure Qty: - " + model.Failure_Qty?.ToString() ?? "";

        //     // % Failure
        //     ws.Cell("B19").Value = "% Failure -   " + model.Failure?.ToString() ?? "";

        //     // Age of Installation
        //     ws.Cell("H19").Value = "Age of Installation: " + model.Age_Install ?? "";

        //     // Site Observations
        //     ws.Cell("B20").Value = "2. Site Observations \n" + model.Description ?? "";

        //     // Problem Statement
        //     ws.Cell("B22").Value = model.Problem_State ?? "";

        //     ws.Cell("B28").Value = "Initial observations:  \n" + model.Initial_Observ ?? "";

        //     ws.Cell("B29").Value = "5. Complaint  History :   \n" + model.Complaint_History ?? "";

        //     //// ---- Problem Basket (checkbox row) ----
        //     //// Here we just write "✓" before text if true.
        //     //ws.Cell("A31").Value = model.Man_Issue_Prob
        //     //    ? "✓ Manufacturing Issue" : "Manufacturing Issue";

        //     //ws.Cell("B31").Value = model.Design_Prob
        //     //    ? "✓ Design" : "Design";

        //     //ws.Cell("D31").Value = model.Site_Issue_Prob
        //     //    ? "✓ Site Issue" : "Site Issue";

        //     //ws.Cell("E31").Value = model.Com_Gap_Prob
        //     //    ? "✓ Communication gap" : "Communication gap";

        //     //ws.Cell("F31").Value = model.Install_Issues_Prob
        //     //    ? "✓ Installation issues" : "Installation issues";

        //     //ws.Cell("G31").Value = model.Wrong_App_Prob
        //     //    ? "✓ Wrong Application" : "Wrong Application";

        //     //// Initial / Interim / RCA / Corrective etc.
        //     //ws.Cell("A33").Value = model.Initial_Observ ?? "";
        //     //ws.Cell("A36").Value = model.Root_Cause_Anal ?? "";
        //     //ws.Cell("A38").Value = model.Corrective_Action ?? "";

        //     // ======================================================
        //     // 4. Monitoring of Action Plan – DYNAMIC ROWS
        //     //    Template:
        //     //      row 40: title
        //     //      row 41: header
        //     //      row 42: detail row template (Sr. No / Action Plan / Target date / Resp / Status)
        //     //      row 43: "Before / After"
        //     //      row 44: "Photo / Photo"
        //     //      row 45: "RCA Prepared By / Date"
        //     // ======================================================

        //     const int templateDetailRow = 42; // first detail row (template)
        //     var details = model.Details ?? new List<RCAReportDetailViewModel>(); // replace with your detail type
        //     int detailCount = details.Count;

        //     var templateMergedRanges = new List<(int firstCol, int lastCol)>();

        //     foreach (var range in ws.MergedRanges)
        //     {
        //         if (range.FirstRow().RowNumber() == templateDetailRow &&
        //             range.LastRow().RowNumber() == templateDetailRow)
        //         {
        //             templateMergedRanges.Add(
        //                 (range.FirstColumn().ColumnNumber(),
        //                  range.LastColumn().ColumnNumber())
        //             );
        //         }
        //     }

        //     // 2. Insert extra rows for N details
        //     if (detailCount > 1)
        //     {
        //         ws.Row(templateDetailRow).InsertRowsBelow(detailCount - 1);
        //     }

        //     // 3. Re-apply merge structure + fill values in every detail row
        //     for (int i = 0; i < detailCount; i++)
        //     {
        //         int row = templateDetailRow + i;
        //         var d = details[i];

        //         // Recreate template merges for this row
        //         foreach (var m in templateMergedRanges)
        //         {
        //             ws.Range(row, m.firstCol, row, m.lastCol).Merge();
        //         }

        //         // Fill values
        //         //ws.Cell(row, 1).Value = i + 1;                                 // Sr. No
        //         //ws.Cell(row, 2).Value = d.Action_Plan ?? "";                   // Action Plan (merged B–E)
        //         //ws.Cell(row, 6).Value = d.Target_Date?.ToString("dd-MMM-yyyy") ?? "";
        //         //ws.Cell(row, 7).Value = d.Resp ?? "";
        //         //ws.Cell(row, 8).Value = d.Status ?? "";
        //     }

        //     // ======================================================
        //     // 5. Position "Before / Photo / RCA" AFTER last detail
        //     // ======================================================

        //     // After insertion:
        //     //  - last detail row index:
        //     int lastDetailRow = templateDetailRow + detailCount - 1;

        //     //  - "Before / After" row should be just after details
        //     //  - "Photo / Photo" row next
        //     //  - "RCA Prepared By / Date" row next
        //     int beforeRow = lastDetailRow + 1;
        //     int photoRow = lastDetailRow + 2;
        //     int rcaRow = lastDetailRow + 3;

        //     // Set "Before / After"
        //     ws.Cell(beforeRow, 1).Value = "Before";
        //     ws.Cell(beforeRow, 6).Value = "After";

        //     // Set "Photo / Photo"
        //     ws.Cell(photoRow, 1).Value = "Photo";
        //     ws.Cell(photoRow, 6).Value = "Photo";



        //     // Footer – RCA Prepared By / Name & Designation / Date
        //     ws.Cell(rcaRow, 1).Value =
        //"RCA Prepared By:- " + (model.Rca_Prepared_By ?? "") +
        //Environment.NewLine +
        //"Name and Designation : " + (model.Name_Designation ?? "");

        //     ws.Cell(rcaRow, 6).Value =
        //         "Date:- " + (model.Date?.ToString("dd-MM-yyyy") ?? "");

        //     // Optional: clear anything below RCA if template had extra text
        //     for (int r = rcaRow + 1; r <= rcaRow + 10; r++)
        //     {
        //         ws.Row(r).Cells().Clear(XLClearOptions.Contents);
        //     }


        //     // 3. Return file
        //     using var stream = new MemoryStream();
        //     wb.SaveAs(stream);
        //     stream.Position = 0;

        //     var fileName = $"RCA_{model.Complaint_No}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        //     return File(stream.ToArray(),
        //         "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //         fileName);
        // }

        //[HttpGet]
        //public async Task<IActionResult> ExportCAReportToExcel(int id)
        //{
        //    // ✅ 15.Customer RCA.xlsx export
        //    // ✅ Initial Analysis: 1 template row, rest dynamic
        //    // ✅ Failed Samples section has 4 images
        //    // ✅ Corrections section has ONLY TEXT (no images)
        //    // ✅ All “below parts” are set by finding text (row shift safe)

        //    var model = await _rCAReportRepository.GetRCAReportByIdAsync(id);
        //    if (model == null) return NotFound();

        //    var templatePath = Path.Combine(_env.WebRootPath, "templates", "15.Customer RCA.xlsx");
        //    if (!System.IO.File.Exists(templatePath))
        //        return NotFound("Customer RCA template not found at " + templatePath);

        //    using var wb = new XLWorkbook(templatePath);
        //    var ws = wb.Worksheet(1);

        //    // =========================================================
        //    // Helpers
        //    // =========================================================
        //    static string S(string? v) => v ?? "";
        //    static string DateStr(DateTime? dt, string fmt = "dd-MMM-yyyy") => dt.HasValue ? dt.Value.ToString(fmt) : "";
        //    static string IntStr(int? v) => v.HasValue ? v.Value.ToString() : "";

        //    static int FindRowByContains(IXLWorksheet sheet, int col, string containsText)
        //    {
        //        var last = sheet.LastRowUsed()?.RowNumber() ?? 1;
        //        for (int r = 1; r <= last; r++)
        //        {
        //            var txt = sheet.Cell(r, col).GetString();
        //            if (!string.IsNullOrWhiteSpace(txt) &&
        //                txt.IndexOf(containsText, StringComparison.OrdinalIgnoreCase) >= 0)
        //                return r;
        //        }
        //        return -1;
        //    }

        //    static void SetSameLineKeepTitle(IXLWorksheet sheet, string address, string value)
        //    {
        //        var cell = sheet.Cell(address);
        //        var title = cell.GetString() ?? "";
        //        cell.Value = $"{title}{value}";
        //    }

        //    static void SetNewLineKeepTitle(IXLWorksheet sheet, int row, int col, string value)
        //    {
        //        if (row <= 0) return;
        //        var cell = sheet.Cell(row, col);
        //        var title = cell.GetString() ?? "";
        //        cell.Value = $"{title}\n{value}";
        //        cell.Style.Alignment.WrapText = true;
        //    }

        //    static void ClearCell(IXLWorksheet sheet, string address)
        //    {
        //        sheet.Cell(address).Clear(XLClearOptions.Contents);
        //    }

        //    // ---------------- IMAGE HELPERS ----------------
        //    string? MapWebPathToPhysical(string? path)
        //    {
        //        if (string.IsNullOrWhiteSpace(path)) return null;
        //        path = path.Replace("~", "").TrimStart('/', '\\');
        //        path = path.Replace("/", Path.DirectorySeparatorChar.ToString());
        //        return Path.Combine(_env.WebRootPath, path);
        //    }

        //    static double ColWidthToPixels(double excelColWidth) => excelColWidth * 7.0;
        //    static double RowPointsToPixels(double points) => points * 1.333;

        //    static (double wPx, double hPx) GetBoxPixels(IXLRange box)
        //    {
        //        double w = 0;
        //        for (int c = box.FirstColumn().ColumnNumber(); c <= box.LastColumn().ColumnNumber(); c++)
        //            w += ColWidthToPixels(box.Worksheet.Column(c).Width);

        //        double h = 0;
        //        for (int r = box.FirstRow().RowNumber(); r <= box.LastRow().RowNumber(); r++)
        //        {
        //            var row = box.Worksheet.Row(r);
        //            var pt = row.Height > 0 ? row.Height : 15.0;
        //            h += RowPointsToPixels(pt);
        //        }

        //        w = Math.Max(10, w - 10);
        //        h = Math.Max(10, h - 10);
        //        return (w, h);
        //    }

        //    void InsertImageCentered(string? webPath, IXLRange box)
        //    {
        //        var physical = MapWebPathToPhysical(webPath);
        //        if (string.IsNullOrWhiteSpace(physical) || !System.IO.File.Exists(physical))
        //            return;

        //        using var img = Image.FromFile(physical);
        //        if (img.Width <= 0 || img.Height <= 0) return;

        //        var (boxW, boxH) = GetBoxPixels(box);

        //        var scale = Math.Min(boxW / img.Width, boxH / img.Height);
        //        if (scale > 1.0) scale = 1.0;

        //        int finalW = Math.Max(1, (int)(img.Width * scale));
        //        int finalH = Math.Max(1, (int)(img.Height * scale));

        //        int offsetX = Math.Max(0, (int)((boxW - finalW) / 2));
        //        int offsetY = Math.Max(0, (int)((boxH - finalH) / 2));

        //        var pic = ws.AddPicture(physical);
        //        pic.MoveTo(box.FirstCell(), offsetX, offsetY);
        //        pic.WithSize(finalW, finalH);
        //    }

        //    IXLRange GetMergedRangeForCell(IXLWorksheet sheet, IXLCell cell)
        //    {
        //        foreach (var mr in sheet.MergedRanges)
        //            if (mr.Contains(cell)) return mr;

        //        return sheet.Range(cell.Address, cell.Address);
        //    }

        //    IXLCell? FindCellContains(IXLWorksheet sheet, string containsText)
        //    {
        //        return sheet.CellsUsed(c =>
        //        {
        //            var s = c.GetString();
        //            return !string.IsNullOrWhiteSpace(s) &&
        //                   s.IndexOf(containsText, StringComparison.OrdinalIgnoreCase) >= 0;
        //        }).FirstOrDefault();
        //    }

        //    /// <summary>
        //    /// Finds merged image boxes under a header text. Returns boxes sorted left->right then top->bottom.
        //    /// Works even if template has 4 boxes in a grid.
        //    /// </summary>
        //    List<IXLRange> FindImageBoxesUnderHeader(string headerContains, int scanRows = 6)
        //    {
        //        var headerCell = FindCellContains(ws, headerContains);
        //        if (headerCell == null) return new List<IXLRange>();

        //        var headerRange = GetMergedRangeForCell(ws, headerCell);

        //        int startRow = headerCell.Address.RowNumber + 1;
        //        int endRow = startRow + Math.Max(1, scanRows) - 1;

        //        int minC = headerRange.FirstColumn().ColumnNumber();
        //        int maxC = headerRange.LastColumn().ColumnNumber();

        //        var boxes = ws.MergedRanges
        //            .Select(m => ws.Range(m.RangeAddress.FirstAddress, m.RangeAddress.LastAddress))
        //            .Where(r =>
        //            {
        //                var a = r.RangeAddress;
        //                return a.FirstAddress.RowNumber >= startRow &&
        //                       a.LastAddress.RowNumber <= endRow &&
        //                       a.FirstAddress.ColumnNumber >= minC &&
        //                       a.LastAddress.ColumnNumber <= maxC;
        //            })
        //            .OrderBy(r => r.FirstRow().RowNumber())
        //            .ThenBy(r => r.FirstColumn().ColumnNumber())
        //            .ToList();

        //        return boxes;
        //    }

        //    void InsertImagesIntoHeaderBoxes(string headerContains, IEnumerable<string?> webPaths, int scanRows = 6)
        //    {
        //        var boxes = FindImageBoxesUnderHeader(headerContains, scanRows);
        //        var imgs = webPaths.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
        //        if (boxes.Count == 0 || imgs.Count == 0) return;

        //        int count = Math.Min(boxes.Count, imgs.Count);
        //        for (int i = 0; i < count; i++)
        //            InsertImageCentered(imgs[i], boxes[i]);
        //    }

        //    // =========================================================
        //    // 1) Fill main fields
        //    // =========================================================
        //    ws.Cell("B11").Value = $"1. Complaint No:- {S(model.Complaint_No)}";
        //    ws.Cell("G11").Value = $"Reported Date : {DateStr(model.Report_Date)}";

        //    ws.Cell("B12").Value = model.Cust_Complaints ? "✓ Customer complaints" : "Customer complaints";
        //    ws.Cell("F12").Value = model.NPI_Validations ? "✓ NPI Validations" : " NPI Validations";
        //    ws.Cell("G12").Value = model.PDI_Obser ? "✓ PDI Observations." : " PDI Observations.";
        //    ws.Cell("H12").Value = model.System ? "✓ System/Process Improvements." : "System/Process Improvements. ";

        //    SetSameLineKeepTitle(ws, "B13", S(model.Cust_Name_Location));
        //    SetSameLineKeepTitle(ws, "B14", S(model.Source_Complaint));
        //    SetSameLineKeepTitle(ws, "B15", S(model.Prod_Code_Desc));
        //    SetSameLineKeepTitle(ws, "B16", S(model.Desc_Complaint));
        //    SetSameLineKeepTitle(ws, "B17", S(model.Batch_Code));
        //    SetSameLineKeepTitle(ws, "H17", S(model.Pkd));

        //    SetSameLineKeepTitle(ws, "B18", IntStr(model.Supp_Qty));
        //    SetSameLineKeepTitle(ws, "H18", IntStr(model.Failure)); // rename if required
        //    SetSameLineKeepTitle(ws, "B19", IntStr(model.Failure_Qty));
        //    SetSameLineKeepTitle(ws, "H19", S(model.Age_Install));

        //    ws.Cell("B20").Value = $"2. Site Observations\n{S(model.Description)}";
        //    ws.Cell("B20").Style.Alignment.WrapText = true;

        //    ws.Cell("B22").Value = S(model.Problem_State);

        //    ws.Cell("B28").Value = $"Initial observations:  \n{S(model.Initial_Observ)}";
        //    ws.Cell("B28").Style.Alignment.WrapText = true;

        //    ws.Cell("B29").Value = $"5. Complaint  History : \n{S(model.Complaint_History)}";
        //    ws.Cell("B29").Style.Alignment.WrapText = true;
        //    ClearCell(ws, "B30");

        //    // =========================================================
        //    // 2) Problem Visualization images (if present)
        //    // =========================================================
        //    InsertImagesIntoHeaderBoxes(
        //        headerContains: "Problem Visualization",
        //        webPaths: new[] { model.Problem_Visual_ImgA, model.Problem_Visual_ImgB, model.Problem_Visual_ImgC },
        //        scanRows: 4
        //    );

        //    // =========================================================
        //    // 3) Failed Samples Images (✅ 4 images)
        //    //    Your template has 4 boxes under "Images of failed samples:"
        //    // =========================================================
        //    InsertImagesIntoHeaderBoxes(
        //        headerContains: "Images of failed samples",
        //        webPaths: new[] { model.Images_Failed_Samples1, model.Images_Failed_Samples2, model.Images_Failed_Samples3, model.Images_Failed_Samples4 },
        //        scanRows: 8
        //    );



        //    // =========================================================
        //    // 5) Initial Analysis (1 template row + dynamic)
        //    // =========================================================
        //    var iaList = model.Details ?? new List<RCAReportDetailViewModel>();

        //    int iaHeaderRow = FindRowByContains(ws, 3, "Parameter Checked during internal Validation");
        //    if (iaHeaderRow > 0)
        //    {
        //        int templateDetailRow = iaHeaderRow + 1;
        //        int desiredCount = iaList.Count;
        //        if (desiredCount <= 0) desiredCount = 1;

        //        var templateMerges = ws.MergedRanges
        //            .Where(r => r.RangeAddress.FirstAddress.RowNumber == templateDetailRow &&
        //                        r.RangeAddress.LastAddress.RowNumber == templateDetailRow)
        //            .Select(r => new
        //            {
        //                FirstCol = r.RangeAddress.FirstAddress.ColumnNumber,
        //                LastCol = r.RangeAddress.LastAddress.ColumnNumber
        //            })
        //            .ToList();

        //        if (desiredCount > 1)
        //            ws.Row(templateDetailRow).InsertRowsBelow(desiredCount - 1);

        //        for (int i = 0; i < desiredCount; i++)
        //        {
        //            int row = templateDetailRow + i;

        //            if (i > 0)
        //                ws.Row(templateDetailRow).CopyTo(ws.Row(row));

        //            foreach (var m in templateMerges)
        //            {
        //                var rng = ws.Range(row, m.FirstCol, row, m.LastCol);
        //                if (!rng.IsMerged()) rng.Merge();
        //            }

        //            ws.Cell(row, 2).Value = i + 1;

        //            if (iaList.Count > 0)
        //            {
        //                ws.Cell(row, 3).Value = S(iaList[i].Parameter_Checked);
        //                ws.Cell(row, 8).Value = S(iaList[i].Observations);
        //            }
        //            else
        //            {
        //                ws.Cell(row, 3).Value = "";
        //                ws.Cell(row, 8).Value = "";
        //            }

        //            ws.Cell(row, 3).Style.Alignment.WrapText = true;
        //            ws.Cell(row, 8).Style.Alignment.WrapText = true;
        //        }
        //    }

        //    // =========================================================
        //    // 6) Below Parts + Conclusion
        //    // =========================================================
        //    int row62 = FindRowByContains(ws, 2, "6.2 Current");
        //    SetNewLineKeepTitle(ws, row62, 2, S(model.Current_Process));

        //    int rowInitConclusion = FindRowByContains(ws, 2, "Initial Conclusion");
        //    SetNewLineKeepTitle(ws, rowInitConclusion, 2, S(model.Conclusion));

        //    int rowDefectives = FindRowByContains(ws, 2, "7.Analysis of Defectives 100nos samples:");
        //    SetNewLineKeepTitle(ws, rowDefectives, 2, S(model.Analysis_of_Defective100));

        //    int rowRootCause = FindRowByContains(ws, 2, "Root cause analysis");
        //    SetNewLineKeepTitle(ws, rowRootCause, 2, S(model.Root_Cause_Anal));

        //    int pbTitleRow = FindRowByContains(ws, 2, "Problem Basket");
        //    if (pbTitleRow > 0)
        //    {
        //        int pbOptionsRow = pbTitleRow + 1;

        //        ws.Cell(pbOptionsRow, 2).Value = model.Man_Issue_Prob ? "✓ manufacturing Issue" : "manufacturing Issue";
        //        ws.Cell(pbOptionsRow, 3).Value = model.Design_Prob ? "✓ design" : "design";
        //        ws.Cell(pbOptionsRow, 5).Value = model.Site_Issue_Prob ? "✓ Site Issue" : "Site Issue";
        //        ws.Cell(pbOptionsRow, 6).Value = model.Com_Gap_Prob ? "✓ Communication gap." : "Commun...cation gap.";
        //        ws.Cell(pbOptionsRow, 7).Value = model.Install_Issues_Prob ? "✓ Installation issues." : "Installation issues.";
        //        ws.Cell(pbOptionsRow, 8).Value = model.Wrong_App_Prob ? "✓ Wrong Application" : "Wrong Application";
        //    }

        //    int rowCorrectiveAction = FindRowByContains(ws, 2, "Corrective Action");
        //    if (rowCorrectiveAction > 0)
        //    {
        //        ws.Cell(rowCorrectiveAction + 1, 2).Value = S(model.Corrective_Action);
        //        ws.Cell(rowCorrectiveAction + 1, 2).Style.Alignment.WrapText = true;
        //    }

        //    int rowCorrections = FindRowByContains(ws, 2, "Images of corrections");
        //    if (rowCorrections > 0)
        //    {
        //        ws.Cell(rowCorrections + 1, 2).Value = S(model.Images_Corrections);
        //        ws.Cell(rowCorrections + 1, 2).Style.Alignment.WrapText = true;
        //    }

        //    int rowConclusion = FindRowByContains(ws, 2, "Conclusion");
        //    if (rowConclusion > 0)
        //    {
        //        ws.Cell(rowConclusion + 1, 2).Value = S(model.Conclusion);
        //        ws.Cell(rowConclusion + 1, 2).Style.Alignment.WrapText = true;
        //    }



        //    // =========================================================
        //    // 7) Return file
        //    // =========================================================
        //    using var stream = new MemoryStream();
        //    wb.SaveAs(stream);
        //    stream.Position = 0;

        //    var safeComplaint = (model.Complaint_No ?? "NA").Replace("/", "-").Replace("\\", "-");
        //    var fileName = $"RCA_{safeComplaint}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

        //    return File(stream.ToArray(),
        //        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //        fileName);
        //}


        [HttpGet]
        public async Task<IActionResult> ExportCAReportToExcel(int id)
        {
            // ✅ 15.Customer RCA.xlsx export
            // ✅ Initial Analysis: 1 template row, rest dynamic
            // ✅ Failed Samples section has 4 images
            // ✅ Corrections section has ONLY TEXT (no images)
            // ✅ All "below parts" are set by finding text (row shift safe)

            var model = await _rCAReportRepository.GetRCAReportByIdAsync(id);
            if (model == null) return NotFound();

            var templatePath = Path.Combine(_env.WebRootPath, "templates", "15.Customer RCA.xlsx");
            if (!System.IO.File.Exists(templatePath))
                return NotFound("Customer RCA template not found at " + templatePath);

            using var wb = new XLWorkbook(templatePath);
            var ws = wb.Worksheet(1);

            // =========================================================
            // Helpers
            // =========================================================
            static string S(string? v) => v ?? "";
            static string DateStr(DateTime? dt, string fmt = "dd-MMM-yyyy") => dt.HasValue ? dt.Value.ToString(fmt) : "";
            static string IntStr(int? v) => v.HasValue ? v.Value.ToString() : "";

            static int FindRowByContains(IXLWorksheet sheet, int col, string containsText)
            {
                var last = sheet.LastRowUsed()?.RowNumber() ?? 1;
                for (int r = 1; r <= last; r++)
                {
                    var txt = sheet.Cell(r, col).GetString();
                    if (!string.IsNullOrWhiteSpace(txt) &&
                        txt.IndexOf(containsText, StringComparison.OrdinalIgnoreCase) >= 0)
                        return r;
                }
                return -1;
            }

            static void SetSameLineKeepTitle(IXLWorksheet sheet, string address, string value)
            {
                var cell = sheet.Cell(address);
                var title = cell.GetString() ?? "";
                cell.Value = $"{title}{value}";
            }

            static void SetNewLineKeepTitle(IXLWorksheet sheet, int row, int col, string value)
            {
                if (row <= 0) return;
                var cell = sheet.Cell(row, col);
                var title = cell.GetString() ?? "";
                cell.Value = $"{title}\n{value}";
                cell.Style.Alignment.WrapText = true;
            }

            static void ClearCell(IXLWorksheet sheet, string address)
            {
                sheet.Cell(address).Clear(XLClearOptions.Contents);
            }

            // ---------------- IMAGE HELPERS ----------------
            string? MapWebPathToPhysical(string? path)
            {
                if (string.IsNullOrWhiteSpace(path)) return null;
                path = path.Replace("~", "").TrimStart('/', '\\');
                path = path.Replace("/", Path.DirectorySeparatorChar.ToString());
                return Path.Combine(_env.WebRootPath, path);
            }

            static double ColWidthToPixels(double excelColWidth) => excelColWidth * 7.0;
            static double RowPointsToPixels(double points) => points * 1.333;

            static (double wPx, double hPx) GetBoxPixels(IXLRange box)
            {
                double w = 0;
                for (int c = box.FirstColumn().ColumnNumber(); c <= box.LastColumn().ColumnNumber(); c++)
                    w += ColWidthToPixels(box.Worksheet.Column(c).Width);

                double h = 0;
                for (int r = box.FirstRow().RowNumber(); r <= box.LastRow().RowNumber(); r++)
                {
                    var row = box.Worksheet.Row(r);
                    var pt = row.Height > 0 ? row.Height : 15.0;
                    h += RowPointsToPixels(pt);
                }

                w = Math.Max(10, w - 10);
                h = Math.Max(10, h - 10);
                return (w, h);
            }

            void InsertImageCentered(string? webPath, IXLRange box)
            {
                var physical = MapWebPathToPhysical(webPath);
                if (string.IsNullOrWhiteSpace(physical) || !System.IO.File.Exists(physical))
                    return;

                using var img = Image.FromFile(physical);
                if (img.Width <= 0 || img.Height <= 0) return;

                var (boxW, boxH) = GetBoxPixels(box);

                var scale = Math.Min(boxW / img.Width, boxH / img.Height);
                if (scale > 1.0) scale = 1.0;

                int finalW = Math.Max(1, (int)(img.Width * scale));
                int finalH = Math.Max(1, (int)(img.Height * scale));

                int offsetX = Math.Max(0, (int)((boxW - finalW) / 2));
                int offsetY = Math.Max(0, (int)((boxH - finalH) / 2));

                var pic = ws.AddPicture(physical);
                pic.MoveTo(box.FirstCell(), offsetX, offsetY);
                pic.WithSize(finalW, finalH);
            }

            IXLRange GetMergedRangeForCell(IXLWorksheet sheet, IXLCell cell)
            {
                foreach (var mr in sheet.MergedRanges)
                    if (mr.Contains(cell)) return mr;

                return sheet.Range(cell.Address, cell.Address);
            }

            IXLCell? FindCellContains(IXLWorksheet sheet, string containsText)
            {
                return sheet.CellsUsed(c =>
                {
                    var s = c.GetString();
                    return !string.IsNullOrWhiteSpace(s) &&
                           s.IndexOf(containsText, StringComparison.OrdinalIgnoreCase) >= 0;
                }).FirstOrDefault();
            }

            /// <summary>
            /// Finds merged image boxes under a header text. Returns boxes sorted left->right then top->bottom.
            /// Works even if template has 4 boxes in a grid.
            /// </summary>
            List<IXLRange> FindImageBoxesUnderHeader(string headerContains, int scanRows = 6)
            {
                var headerCell = FindCellContains(ws, headerContains);
                if (headerCell == null) return new List<IXLRange>();

                var headerRange = GetMergedRangeForCell(ws, headerCell);

                // ✅ Start searching AFTER the header ends
                int startRow = headerRange.LastRow().RowNumber() + 1;
                int endRow = startRow + Math.Max(1, scanRows) - 1;

                int minC = headerRange.FirstColumn().ColumnNumber();
                int maxC = headerRange.LastColumn().ColumnNumber();

                var boxes = ws.MergedRanges
                    .Select(m => ws.Range(m.RangeAddress.FirstAddress, m.RangeAddress.LastAddress))
                    .Where(r =>
                    {
                        var a = r.RangeAddress;
                        // ✅ Check that box starts within scan range and overlaps with header columns
                        return a.FirstAddress.RowNumber >= startRow &&
                               a.FirstAddress.RowNumber <= endRow &&
                               a.FirstAddress.ColumnNumber >= minC &&
                               a.LastAddress.ColumnNumber <= maxC;
                    })
                    .OrderBy(r => r.FirstRow().RowNumber())
                    .ThenBy(r => r.FirstColumn().ColumnNumber())
                    .ToList();

                return boxes;
            }

            void InsertImagesIntoHeaderBoxes(string headerContains, IEnumerable<string?> webPaths, int scanRows = 6)
            {
                var boxes = FindImageBoxesUnderHeader(headerContains, scanRows);
                var imgs = webPaths.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                if (boxes.Count == 0 || imgs.Count == 0) return;

                int count = Math.Min(boxes.Count, imgs.Count);
                for (int i = 0; i < count; i++)
                    InsertImageCentered(imgs[i], boxes[i]);
            }

            // =========================================================
            // 1) Fill main fields
            // =========================================================
            ws.Cell("B11").Value = $"1. Complaint No:- {S(model.Complaint_No)}";
            ws.Cell("G11").Value = $"Reported Date : {DateStr(model.Report_Date)}";

            ws.Cell("B12").Value = model.Cust_Complaints ? "✓ Customer complaints" : "Customer complaints";
            ws.Cell("F12").Value = model.NPI_Validations ? "✓ NPI Validations" : " NPI Validations";
            ws.Cell("G12").Value = model.PDI_Obser ? "✓ PDI Observations." : " PDI Observations.";
            ws.Cell("H12").Value = model.System ? "✓ System/Process Improvements." : "System/Process Improvements. ";

            SetSameLineKeepTitle(ws, "B13", S(model.Cust_Name_Location));
            SetSameLineKeepTitle(ws, "B14", S(model.Source_Complaint));
            SetSameLineKeepTitle(ws, "B15", S(model.Prod_Code_Desc));
            SetSameLineKeepTitle(ws, "B16", S(model.Desc_Complaint));
            SetSameLineKeepTitle(ws, "B17", S(model.Batch_Code));
            SetSameLineKeepTitle(ws, "H17", S(model.Pkd));

            SetSameLineKeepTitle(ws, "B18", IntStr(model.Supp_Qty));
            SetSameLineKeepTitle(ws, "H18", IntStr(model.Failure));
            SetSameLineKeepTitle(ws, "B19", IntStr(model.Failure_Qty));
            SetSameLineKeepTitle(ws, "H19", S(model.Age_Install));

            ws.Cell("B20").Value = $"2. Site Observations\n{S(model.Description)}";
            ws.Cell("B20").Style.Alignment.WrapText = true;

            ws.Cell("B22").Value = S(model.Problem_State);

            ws.Cell("B28").Value = $"Initial observations:  \n{S(model.Initial_Observ)}";
            ws.Cell("B28").Style.Alignment.WrapText = true;

            ws.Cell("B29").Value = $"5. Complaint  History : \n{S(model.Complaint_History)}";
            ws.Cell("B29").Style.Alignment.WrapText = true;
            ClearCell(ws, "B30");

            // =========================================================
            // 2) Problem Visualization images (if present)
            // =========================================================
            InsertImagesIntoHeaderBoxes(
                headerContains: "Problem Visualization",
                webPaths: new[] { model.Problem_Visual_ImgA, model.Problem_Visual_ImgB, model.Problem_Visual_ImgC },
                scanRows: 4
            );

            // =========================================================
            // 3) Failed Samples Images (✅ 4 images)
            //    Your template has 4 boxes under "Images of failed samples:"
            // =========================================================
            InsertImagesIntoHeaderBoxes(
                headerContains: "Images of failed samples",
                webPaths: new[] {
            model.Images_Failed_Samples1,
            model.Images_Failed_Samples2,
            model.Images_Failed_Samples3,
            model.Images_Failed_Samples4
                },
                scanRows: 10  // ✅ Increased to ensure all 4 boxes are found
            );

            // =========================================================
            // 5) Initial Analysis (1 template row + dynamic)
            // =========================================================
            var iaList = model.Details ?? new List<RCAReportDetailViewModel>();

            int iaHeaderRow = FindRowByContains(ws, 3, "Parameter Checked during internal Validation");
            if (iaHeaderRow > 0)
            {
                int templateDetailRow = iaHeaderRow + 1;
                int desiredCount = iaList.Count;
                if (desiredCount <= 0) desiredCount = 1;

                var templateMerges = ws.MergedRanges
                    .Where(r => r.RangeAddress.FirstAddress.RowNumber == templateDetailRow &&
                                r.RangeAddress.LastAddress.RowNumber == templateDetailRow)
                    .Select(r => new
                    {
                        FirstCol = r.RangeAddress.FirstAddress.ColumnNumber,
                        LastCol = r.RangeAddress.LastAddress.ColumnNumber
                    })
                    .ToList();

                if (desiredCount > 1)
                    ws.Row(templateDetailRow).InsertRowsBelow(desiredCount - 1);

                for (int i = 0; i < desiredCount; i++)
                {
                    int row = templateDetailRow + i;

                    if (i > 0)
                        ws.Row(templateDetailRow).CopyTo(ws.Row(row));

                    foreach (var m in templateMerges)
                    {
                        var rng = ws.Range(row, m.FirstCol, row, m.LastCol);
                        if (!rng.IsMerged()) rng.Merge();
                    }

                    ws.Cell(row, 2).Value = i + 1;

                    if (iaList.Count > 0)
                    {
                        ws.Cell(row, 3).Value = S(iaList[i].Parameter_Checked);
                        ws.Cell(row, 8).Value = S(iaList[i].Observations);
                    }
                    else
                    {
                        ws.Cell(row, 3).Value = "";
                        ws.Cell(row, 8).Value = "";
                    }

                    ws.Cell(row, 3).Style.Alignment.WrapText = true;
                    ws.Cell(row, 8).Style.Alignment.WrapText = true;
                }
            }

            // =========================================================
            // 6) Below Parts + Conclusion
            // =========================================================
            int row62 = FindRowByContains(ws, 2, "6.2 Current");
            SetNewLineKeepTitle(ws, row62, 2, S(model.Current_Process));

            int rowInitConclusion = FindRowByContains(ws, 2, "Initial Conclusion");
            SetNewLineKeepTitle(ws, rowInitConclusion, 2, S(model.Conclusion));

            int rowDefectives = FindRowByContains(ws, 2, "7.Analysis of Defectives 100nos samples:");
            SetNewLineKeepTitle(ws, rowDefectives, 2, S(model.Analysis_of_Defective100));

            int rowRootCause = FindRowByContains(ws, 2, "Root cause analysis");
            SetNewLineKeepTitle(ws, rowRootCause, 2, S(model.Root_Cause_Anal));

            int pbTitleRow = FindRowByContains(ws, 2, "Problem Basket");
            if (pbTitleRow > 0)
            {
                int pbOptionsRow = pbTitleRow + 1;

                ws.Cell(pbOptionsRow, 2).Value = model.Man_Issue_Prob ? "✓ manufacturing Issue" : "manufacturing Issue";
                ws.Cell(pbOptionsRow, 3).Value = model.Design_Prob ? "✓ design" : "design";
                ws.Cell(pbOptionsRow, 5).Value = model.Site_Issue_Prob ? "✓ Site Issue" : "Site Issue";
                ws.Cell(pbOptionsRow, 6).Value = model.Com_Gap_Prob ? "✓ Communication gap." : "Communication gap.";
                ws.Cell(pbOptionsRow, 7).Value = model.Install_Issues_Prob ? "✓ Installation issues." : "Installation issues.";
                ws.Cell(pbOptionsRow, 8).Value = model.Wrong_App_Prob ? "✓ Wrong Application" : "Wrong Application";
            }

            int rowCorrectiveAction = FindRowByContains(ws, 2, "Corrective Action");
            if (rowCorrectiveAction > 0)
            {
                ws.Cell(rowCorrectiveAction + 1, 2).Value = S(model.Corrective_Action);
                ws.Cell(rowCorrectiveAction + 1, 2).Style.Alignment.WrapText = true;
            }

            int rowCorrections = FindRowByContains(ws, 2, "Images of corrections");
            if (rowCorrections > 0)
            {
                ws.Cell(rowCorrections + 1, 2).Value = S(model.Images_Corrections);
                ws.Cell(rowCorrections + 1, 2).Style.Alignment.WrapText = true;
            }

            int rowConclusion = FindRowByContains(ws, 2, "Conclusion");
            if (rowConclusion > 0)
            {
                ws.Cell(rowConclusion + 1, 2).Value = S(model.Conclusion);
                ws.Cell(rowConclusion + 1, 2).Style.Alignment.WrapText = true;
            }

            // =========================================================
            // 7) Return file
            // =========================================================
            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;

            var safeComplaint = (model.Complaint_No ?? "NA").Replace("/", "-").Replace("\\", "-");
            var fileName = $"RCA_{safeComplaint}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }


    }
}
