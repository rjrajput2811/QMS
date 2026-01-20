using ClosedXML.Excel.Drawings;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using QMS.Core.Models;
using QMS.Core.ViewModels;
using System.Text.RegularExpressions;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Services.SystemLogs;
using QMS.Core.Repositories.CAReportRepository;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
//using static System.Net.Mime.MediaTypeNames;
//using System.Drawing;


namespace QMS.Controllers
{
    public class CAReportController : Controller
    {
        private readonly ICAReportRepository _cAReportRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IWebHostEnvironment _env;

        public CAReportController(ICAReportRepository cAReportRepository, ISystemLogService systemLogService, IWebHostEnvironment env)
        {
            _cAReportRepository = cAReportRepository;
            _systemLogService = systemLogService;
            _env = env;
        }
        public IActionResult CAFormatee()
        {
            return View();
        }


        public async Task<JsonResult> GetCAReportAsync(DateTime? startDate, DateTime? endDate)
        {
            var result = await _cAReportRepository.GetCAReportAsync(startDate, endDate);
            return Json(result);
        }

        public async Task<JsonResult> GetCSOTrackingAsync(DateTime? startDate, DateTime? endDate)
        {
            var result = await _cAReportRepository.GetCSOTrackingAsync(startDate, endDate);
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetCAReportByIdAsync(int internalTypeId)
        {
            var result = await _cAReportRepository.GetCAReportByIdAsync(internalTypeId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CAFormateDetails(int caReportId)
        {
            // Always initialize a model first
            CAReportViewModel model = new CAReportViewModel();

            if (caReportId > 0)
            {
                // Fetch existing record
                model = await _cAReportRepository.GetCAReportByIdAsync(caReportId);

                if (model == null)
                {
                    return NotFound($"CA Report not found for Id: {caReportId}");
                }
            }

            // Return View with model (either new or fetched)
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _cAReportRepository.DeleteAsync(id);
            return Json(operationResult);
        }

        //[HttpPost]
        //public async Task<IActionResult> InsertCAReportAsync(CAReportViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            var errors = ModelState.Values
        //                .SelectMany(v => v.Errors)
        //                .Select(e => e.ErrorMessage)
        //                .ToList();
        //            return Json(new { Success = false, Errors = errors });
        //        }

        //        bool exists = await _cAReportRepository.CheckDuplicate(model.Complaint_No.Trim(), 0);
        //        if (exists)
        //        {
        //            return Json(new { success = false, message = "CA Report Detail already exists." });
        //        }

        //        var user = HttpContext.Session.GetString("FullName") ?? "System";

        //        OperationResult result;

        //        if (model == null)
        //        {
        //            return Json(new { Success = false, Errors = new[] { "Model cannot be null." } });
        //        }

        //        if (model.Id > 0)
        //        {
        //            model.UpdatedBy = user;

        //            result = await _cAReportRepository.UpdateCAReportAsync(model);
        //            if (result == null) 
        //                result = new OperationResult { Success = false, Message = "Update failed." };

        //            if (result.Success && string.IsNullOrWhiteSpace(result.Message))
        //                result.Message = "CA Report Detail updated successfully.";
        //        }
        //        else
        //        {
        //            // Insert path
        //            model.CreatedBy = user;

        //            result = await _cAReportRepository.InsertCAReportAsync(model);
        //            if (result == null)
        //                result = new OperationResult { Success = false, Message = "Insert failed." };

        //            if (result.Success && string.IsNullOrWhiteSpace(result.Message))
        //                result.Message = "CA Report Detail created successfully.";
        //        }

        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _systemLogService.WriteLog(ex.Message);
        //        return Json(new { Success = false, Errors = new[] { "Failed to save ca report detail." }, Exception = ex.Message });
        //    }
        //}


        [HttpPost]
        public async Task<IActionResult> InsertCAReportAsync(CAReportViewModel model)
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

                if (model.Before_PhotoFile != null && model.Before_PhotoFile.Length > 0)
                {
                    model.Before_Photo = await SaveImageAsync(
                        model.Before_PhotoFile, "Before", model.Complaint_No);
                }

                if (model.After_PhotoFile != null && model.After_PhotoFile.Length > 0)
                {
                    model.After_Photo = await SaveImageAsync(
                        model.After_PhotoFile, "After", model.Complaint_No);
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
                    exists = await _cAReportRepository.CheckDuplicate(
                        model.Complaint_No!.Trim(),
                        model.Id
                    );
                }
                else
                {
                    // INSERT: check if complaint already used anywhere
                    exists = await _cAReportRepository.CheckDuplicate(
                        model.Complaint_No!.Trim(),
                        0
                    );
                }

                if (exists)
                {
                    return Json(new
                    {
                        Success = false,
                        Errors = new[] { $"Duplicate CA Complaint No '{model.Complaint_No}' already exists." }
                    });
                }

                var user = HttpContext.Session.GetString("FullName") ?? "System";
                OperationResult result;

                if (model.Id > 0)
                {
                    model.UpdatedBy = user;

                    result = await _cAReportRepository.UpdateCAReportAsync(model)
                             ?? new OperationResult { Success = false, Message = "Update failed." };

                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "CA Report Detail updated successfully.";
                }
                else
                {
                    model.CreatedBy = user;

                    result = await _cAReportRepository.InsertCAReportAsync(model)
                             ?? new OperationResult { Success = false, Message = "Insert failed." };

                    if (result.Success && string.IsNullOrWhiteSpace(result.Message))
                        result.Message = "CA Report Detail created successfully.";
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
                "CAReport_Attach",
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
            var relativeForDb = $"/CAReport_Attach/{safeComplaintNo}/{fileName}";

            return relativeForDb;
        }

        [HttpGet]
        public async Task<IActionResult> ExportCAReportToExcel(int id)
        {
            var model = await _cAReportRepository.GetCAReportByIdAsync(id);
            if (model == null)
                return NotFound();

            // 1. Load template
            var templatePath = Path.Combine(_env.WebRootPath, "templates", "01.CA_Format.xlsx");

            if (!System.IO.File.Exists(templatePath))
                return NotFound("CA template not found at " + templatePath);

            using var wb = new XLWorkbook(templatePath);

            // Use sheet index or exact sheet name from template
            var ws = wb.Worksheet(1);  // or wb.Worksheet("CA Format");

            // 2. Fill fields – ONLY set .Value, don’t change style/merge/width

            // These cell addresses must match your template layout.
            // (Below are typical positions – adjust if any cell is different.)

            // Complaint No (top row, left)
            ws.Cell("A11").Value = "Complaint No:- " + model.Complaint_No ?? "";

            // Reported Date (top row, right)
            ws.Cell("F11").Value = "Reported Date :" + model.Report_Date?.ToString("dd-MMM-yyyy") ?? "";

            ws.Cell("A12").Value = model.Cust_Complaints
                ? "✓ Customer complaints" : "Customer complaints";

            ws.Cell("E12").Value = model.NPI_Validations
                ? "✓ NPI Validations" : "NPI Validations";

            ws.Cell("F12").Value = model.PDI_Obser
                ? "✓ PDI Observations" : "PDI Observations";

            ws.Cell("G12").Value = model.System
                ? "✓ System/Process Improvements" : "System/Process Improvements";

            // Customer Name & Location
            ws.Cell("A13").Value = "Customer Name and Location: -  " + model.Cust_Name_Location ?? "";

            // Source of Complaint
            ws.Cell("A14").Value = "Source of Complaint:-" + model.Source_Complaint ?? "";

            // Product Code & Description
            ws.Cell("A15").Value = "Product Code and Description:- " + model.Prod_Code_Desc ?? "";

            ws.Cell("A16").Value = "Description of Complaint:-" + model.Desc_Complaint ?? "";

            // Batch Code
            ws.Cell("A17").Value = "Batch Code:- " + model.Batch_Code ?? "";

            // PKD
            ws.Cell("G17").Value = "PKD:- " + model.Pkd ?? "";

            // Supplied Qty
            ws.Cell("A18").Value = "Supplied Qty: - " + model.Supp_Qty?.ToString() ?? "";

            // Failure Qty
            ws.Cell("G18").Value = "Failure Qty: - " + model.Failure_Qty?.ToString() ?? "";

            // % Failure
            ws.Cell("A19").Value = "% Failure -   " + model.Failure?.ToString() ?? "";

            // Age of Installation
            ws.Cell("G19").Value = "Age of Installation: " + model.Age_Install ?? "";

            // Site Observations
            ws.Cell("A20").Value = model.Description ?? "";

            // Problem Statement
            ws.Cell("A22").Value = model.Problem_State ?? "";

            ws.Cell("A28").Value = "Initial observations:  \n" + model.Initial_Observ ?? "";

            // ---- Problem Basket (checkbox row) ----
            // Here we just write "✓" before text if true.
            ws.Cell("A31").Value = model.Man_Issue_Prob
                ? "✓ Manufacturing Issue" : "Manufacturing Issue";

            ws.Cell("B31").Value = model.Design_Prob
                ? "✓ Design" : "Design";

            ws.Cell("D31").Value = model.Site_Issue_Prob
                ? "✓ Site Issue" : "Site Issue";

            ws.Cell("E31").Value = model.Com_Gap_Prob
                ? "✓ Communication gap" : "Communication gap";

            ws.Cell("F31").Value = model.Install_Issues_Prov
                ? "✓ Installation issues" : "Installation issues";

            ws.Cell("G31").Value = model.Wrong_App_Prob
                ? "✓ Wrong Application" : "Wrong Application";

            // Initial / Interim / RCA / Corrective etc.
            ws.Cell("A33").Value = model.Interim_Corrective ?? "";
            ws.Cell("A36").Value = model.Root_Cause_Anal ?? "";
            ws.Cell("A38").Value = model.Corrective_Action ?? "";

            // ======================================================
            // 4. Monitoring of Action Plan – DYNAMIC ROWS
            //    Template:
            //      row 40: title
            //      row 41: header
            //      row 42: detail row template (Sr. No / Action Plan / Target date / Resp / Status)
            //      row 43: "Before / After"
            //      row 44: "Photo / Photo"
            //      row 45: "RCA Prepared By / Date"
            // ======================================================

            const int templateDetailRow = 42; // first detail row (template)
            var details = model.Details ?? new List<CAReportDetailViewModel>(); // replace with your detail type
            int detailCount = details.Count;

            var templateMergedRanges = new List<(int firstCol, int lastCol)>();

            foreach (var range in ws.MergedRanges)
            {
                if (range.FirstRow().RowNumber() == templateDetailRow &&
                    range.LastRow().RowNumber() == templateDetailRow)
                {
                    templateMergedRanges.Add(
                        (range.FirstColumn().ColumnNumber(),
                         range.LastColumn().ColumnNumber())
                    );
                }
            }

            // 2. Insert extra rows for N details
            if (detailCount > 1)
            {
                ws.Row(templateDetailRow).InsertRowsBelow(detailCount - 1);
            }

            // 3. Re-apply merge structure + fill values in every detail row
            for (int i = 0; i < detailCount; i++)
            {
                int row = templateDetailRow + i;
                var d = details[i];

                // Recreate template merges for this row
                foreach (var m in templateMergedRanges)
                {
                    ws.Range(row, m.firstCol, row, m.lastCol).Merge();
                }

                // Fill values
                ws.Cell(row, 1).Value = i + 1;                                 // Sr. No
                ws.Cell(row, 2).Value = d.Action_Plan ?? "";                   // Action Plan (merged B–E)
                ws.Cell(row, 6).Value = d.Target_Date?.ToString("dd-MMM-yyyy") ?? "";
                ws.Cell(row, 7).Value = d.Resp ?? "";
                ws.Cell(row, 8).Value = d.Status ?? "";
            }

            // ======================================================
            // 5. Position "Before / Photo / RCA" AFTER last detail
            // ======================================================

            // After insertion:
            //  - last detail row index:
            int lastDetailRow = templateDetailRow + detailCount - 1;

            //  - "Before / After" row should be just after details
            //  - "Photo / Photo" row next
            //  - "RCA Prepared By / Date" row next
            int beforeRow = lastDetailRow + 1;
            int photoRow = lastDetailRow + 2;
            int rcaRow = lastDetailRow + 3;

            // Set "Before / After"
            ws.Cell(beforeRow, 1).Value = "Before";
            ws.Cell(beforeRow, 6).Value = "After";

            // helper to convert "/CAReport_Attach/....png" -> physical path
            // convert "/CAReport_Attach/....png" -> physical path under wwwroot
            string? MapWebPathToPhysical(string? path)
            {
                if (string.IsNullOrWhiteSpace(path)) return null;

                // "/CAReport_Attach/2000_2526/x.png" -> "CAReport_Attach\2000_2526\x.png"
                path = path.Replace("~", "").TrimStart('/', '\\');
                path = path.Replace("/", Path.DirectorySeparatorChar.ToString());

                return Path.Combine(_env.WebRootPath, path);
            }

            IXLRange GetMergedRangeForCell(IXLWorksheet sheet, IXLCell cell)
            {
                foreach (var mr in sheet.MergedRanges)
                    if (mr.Contains(cell))
                        return mr;

                return sheet.Range(cell.Address, cell.Address);
            }

            double ColWidthToPixels(double excelColWidth) => excelColWidth * 7.0;
            double RowPointsToPixels(double points) => points * 1.333;

            (double wPx, double hPx) GetBoxPixels(IXLRange box)
            {
                double w = 0;
                for (int c = box.FirstColumn().ColumnNumber(); c <= box.LastColumn().ColumnNumber(); c++)
                    w += ColWidthToPixels(box.Worksheet.Column(c).Width);

                double h = 0;
                for (int r = box.FirstRow().RowNumber(); r <= box.LastRow().RowNumber(); r++)
                {
                    var row = box.Worksheet.Row(r);
                    double pt = row.Height > 0 ? row.Height : 15.0;
                    h += RowPointsToPixels(pt);
                }

                // padding so it doesn't touch borders
                w = Math.Max(10, w - 10);
                h = Math.Max(10, h - 10);

                return (w, h);
            }

            void InsertImageCentered(IXLWorksheet sheet, string? webPath, IXLRange box)
            {
                var physical = MapWebPathToPhysical(webPath);

                // If file doesn't exist, skip silently (or log)
                if (string.IsNullOrWhiteSpace(physical) || !System.IO.File.Exists(physical))
                    return;

                using var img = Image.FromFile(physical);
                if (img.Width <= 0 || img.Height <= 0)
                    return;

                var (boxW, boxH) = GetBoxPixels(box);

                // Fit inside box
                double scale = Math.Min(boxW / img.Width, boxH / img.Height);
                if (scale > 1.0) scale = 1.0;

                int finalW = Math.Max(1, (int)(img.Width * scale));
                int finalH = Math.Max(1, (int)(img.Height * scale));

                // Center offsets
                int offsetX = Math.Max(0, (int)((boxW - finalW) / 2));
                int offsetY = Math.Max(0, (int)((boxH - finalH) / 2));

                var pic = sheet.AddPicture(physical);
                pic.MoveTo(box.FirstCell(), offsetX, offsetY);
                pic.WithSize(finalW, finalH);
            }

            // =====================================================
            // 6) FIND "Photo" BOXES FROM TEMPLATE & INSERT IMAGES
            //    (robust even after inserting rows)
            // =====================================================
            var photoCells = ws.CellsUsed(c => c.GetString().Trim().Equals("Photo", StringComparison.OrdinalIgnoreCase))
                               .OrderBy(c => c.Address.RowNumber)
                               .ThenBy(c => c.Address.ColumnNumber)
                               .Take(2)
                               .ToList();

            // If your template has exactly 2 "Photo" texts (Before/After)
            if (photoCells.Count == 2)
            {
                var beforeBox = GetMergedRangeForCell(ws, photoCells[0]);
                var afterBox = GetMergedRangeForCell(ws, photoCells[1]);

                InsertImageCentered(ws, model.Before_Photo, beforeBox);
                InsertImageCentered(ws, model.After_Photo, afterBox);
            }
            else
            {
                // Fallback if "Photo" text not found / different wording
                // Adjust these columns/rows once to match your template
                var beforeBox = ws.Range(photoRow, 1, photoRow + 2, 5);   // A..E
                var afterBox = ws.Range(photoRow, 6, photoRow + 2, 10);  // F..J

                InsertImageCentered(ws, model.Before_Photo, beforeBox);
                InsertImageCentered(ws, model.After_Photo, afterBox);
            }

            IXLCell? FindCellContains(IXLWorksheet sheet, string containsText)
            {
                return sheet.CellsUsed(c =>
                {
                    var s = c.GetString();
                    return !string.IsNullOrWhiteSpace(s) &&
                           s.Trim().IndexOf(containsText, StringComparison.OrdinalIgnoreCase) >= 0;
                }).FirstOrDefault();
            }

            void InsertProblemVisualizationImages(string? imgA, string? imgB, string? imgC)
            {
                // 🔎 Find the header cell by text (NOT by address)
                var headerCell = FindCellContains(ws, "Problem Visualization");
                if (headerCell == null) return;

                // Header is usually merged A23:H23
                var headerRange = GetMergedRangeForCell(ws, headerCell);

                int headerRow = headerCell.Address.RowNumber;
                int firstCol = headerRange.FirstColumn().ColumnNumber();
                int lastCol = headerRange.LastColumn().ColumnNumber();

                // Image area is below header: 2 rows height (same like your template)
                int imgTopRow = headerRow + 1;
                int imgBottomRow = headerRow + 2;

                // Total columns in the section
                int totalCols = lastCol - firstCol + 1;

                // Split into 3 boxes (try best equal)
                int box1Cols = totalCols / 3;
                int box2Cols = totalCols / 3;
                int box3Cols = totalCols - box1Cols - box2Cols;

                int box1Start = firstCol;
                int box1End = box1Start + box1Cols - 1;

                int box2Start = box1End + 1;
                int box2End = box2Start + box2Cols - 1;

                int box3Start = box2End + 1;
                int box3End = lastCol;

                // Build ranges
                var boxA = ws.Range(imgTopRow, box1Start, imgBottomRow, box1End);
                var boxB = ws.Range(imgTopRow, box2Start, imgBottomRow, box2End);
                var boxC = ws.Range(imgTopRow, box3Start, imgBottomRow, box3End);

                // Insert images centered + auto-fit
                InsertImageCentered(ws, imgA, boxA);
                InsertImageCentered(ws, imgB, boxB);
                InsertImageCentered(ws, imgC, boxC);
            }

            // ✅ Call it (use your actual properties)
            InsertProblemVisualizationImages(
                model.Problem_Visual_ImgA,   // replace with your property
                model.Problem_Visual_ImgB,   // replace with your property
                model.Problem_Visual_ImgC    // replace with your property
            );


            // Footer – RCA Prepared By / Name & Designation / Date
            ws.Cell(rcaRow, 1).Value =
       "RCA Prepared By:- " + (model.Rca_Prepared_By ?? "") +
       Environment.NewLine +
       "Name and Designation : " + (model.Name_Designation ?? "");

            ws.Cell(rcaRow, 6).Value =
                "Date:- " + (model.Date?.ToString("dd-MM-yyyy") ?? "");

            // Optional: clear anything below RCA if template had extra text
            for (int r = rcaRow + 1; r <= rcaRow + 10; r++)
            {
                ws.Row(r).Cells().Clear(XLClearOptions.Contents);
            }


            // 3. Return file
            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"CA_{model.Complaint_No}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }



    }
}
