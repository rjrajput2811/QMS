using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.CAReportRepository;
using QMS.Core.Repositories.RCAReportRepository;
using QMS.Core.Services.SystemLogs;

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

        [HttpGet]
        public async Task<ActionResult> GetRCAReportByIdAsync(int internalTypeId)
        {
            var result = await _rCAReportRepository.GetRCAReportByIdAsync(internalTypeId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CustomerRCADetails(int id)
        {
            RCAReportViewModel model;

            if (id == 0)
            {
                model = new RCAReportViewModel
                {
                    Date = DateTime.Now
                };
            }
            else
            {
                model = await _rCAReportRepository.GetRCAReportByIdAsync(id);

                if (model == null)
                {
                    return NotFound();
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

                bool exists = await _rCAReportRepository.CheckDuplicate(model.Complaint_No!.Trim(), 0);
                if (exists)
                {
                    return Json(new { Success = false, Errors = new[] { "RCA Report Detail already exists." } });
                }

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

        [HttpGet]
        public async Task<IActionResult> ExportCAReportToExcel(int id)
        {
            var model = await _rCAReportRepository.GetRCAReportByIdAsync(id);
            if (model == null)
                return NotFound();

            // 1. Load template
            var templatePath = Path.Combine(_env.WebRootPath, "templates", "15.Customer RCA.xlsx");

            if (!System.IO.File.Exists(templatePath))
                return NotFound("Customer RCA template not found at " + templatePath);

            using var wb = new XLWorkbook(templatePath);

            // Use sheet index or exact sheet name from template
            var ws = wb.Worksheet(1);  // or wb.Worksheet("CA Format");

            // 2. Fill fields – ONLY set .Value, don’t change style/merge/width

            // These cell addresses must match your template layout.
            // (Below are typical positions – adjust if any cell is different.)

            // Complaint No (top row, left)
            ws.Cell("B11").Value = "Complaint No:- " + model.Complaint_No ?? "";

            // Reported Date (top row, right)
            ws.Cell("G11").Value = "Reported Date :" + model.Report_Date?.ToString("dd-MMM-yyyy") ?? "";

            ws.Cell("B12").Value = model.Cust_Complaints
                ? "✓ Customer complaints" : "Customer complaintse";

            ws.Cell("F12").Value = model.NPI_Validations
                ? "✓ NPI Validations" : "NPI Validations";

            ws.Cell("G12").Value = model.PDI_Obser
                ? "✓ PDI Observations." : "PDI Observations.";

            ws.Cell("H12").Value = model.System
                ? "✓ System/Process Improvements." : "System/Process Improvements.";

            // Customer Name & Location
            ws.Cell("B13").Value = "Customer Name and Location: -  " + model.Cust_Name_Location ?? "";

            // Source of Complaint
            ws.Cell("B14").Value = "Source of Complaint:-" + model.Source_Complaint ?? "";

            // Product Code & Description
            ws.Cell("B15").Value = "Product Code and Description:- " + model.Prod_Code_Desc ?? "";

            ws.Cell("B16").Value = "Description of Complaint:-" + model.Desc_Complaint ?? "";

            // Batch Code
            ws.Cell("B17").Value = "Batch Code:- " + model.Batch_Code ?? "";

            // PKD
            ws.Cell("H17").Value = "PKD:- " + model.Pkd ?? "";

            // Supplied Qty
            ws.Cell("B18").Value = "Supplied Qty: - " + model.Supp_Qty?.ToString() ?? "";

            // Failure Qty
            ws.Cell("H18").Value = "Failure Qty: - " + model.Failure_Qty?.ToString() ?? "";

            // % Failure
            ws.Cell("B19").Value = "% Failure -   " + model.Failure?.ToString() ?? "";

            // Age of Installation
            ws.Cell("H19").Value = "Age of Installation: " + model.Age_Install ?? "";

            // Site Observations
            ws.Cell("B20").Value = "2. Site Observations \n" + model.Description ?? "";

            // Problem Statement
            ws.Cell("B22").Value = model.Problem_State ?? "";

            ws.Cell("B28").Value = "Initial observations:  \n" + model.Initial_Observ ?? "";

            ws.Cell("B29").Value = "5. Complaint  History :   \n" + model.Complaint_History ?? "";

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

            ws.Cell("F31").Value = model.Install_Issues_Prob
                ? "✓ Installation issues" : "Installation issues";

            ws.Cell("G31").Value = model.Wrong_App_Prob
                ? "✓ Wrong Application" : "Wrong Application";

            // Initial / Interim / RCA / Corrective etc.
            ws.Cell("A33").Value = model.Initial_Observ ?? "";
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
            var details = model.Details ?? new List<RCAReportDetailViewModel>(); // replace with your detail type
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
                //ws.Cell(row, 1).Value = i + 1;                                 // Sr. No
                //ws.Cell(row, 2).Value = d.Action_Plan ?? "";                   // Action Plan (merged B–E)
                //ws.Cell(row, 6).Value = d.Target_Date?.ToString("dd-MMM-yyyy") ?? "";
                //ws.Cell(row, 7).Value = d.Resp ?? "";
                //ws.Cell(row, 8).Value = d.Status ?? "";
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

            // Set "Photo / Photo"
            ws.Cell(photoRow, 1).Value = "Photo";
            ws.Cell(photoRow, 6).Value = "Photo";



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
