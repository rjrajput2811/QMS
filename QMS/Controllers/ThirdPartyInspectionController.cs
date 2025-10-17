using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ThirdPartyInspectionRepository;
using QMS.Core.Services.SystemLogs;
using System.Globalization;

namespace QMS.Controllers
{
    public class ThirdPartyInspectionController : Controller
    {
        private readonly IThirdPartyInspectionRepository _repository;
        private readonly ISystemLogService _systemLogService;

        public ThirdPartyInspectionController(
            IThirdPartyInspectionRepository repository,
            ISystemLogService systemLogService)
        {
            _repository = repository;
            _systemLogService = systemLogService;
        }

        public IActionResult ThirdPartyInspection()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _repository.GetListAsync(startDate, endDate);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var bisProject = await _repository.GetByIdAsync(Id);
            return Json(bisProject);
        }


        [HttpPost]
        public async Task<JsonResult> CreateAsync([FromBody] ThirdPartyInspection model)
        {
            try
            {

                if (model == null)
                    return Json(new { success = false, message = "Invalid Third Party Inspection Tracker data." });


                model.CreatedBy = HttpContext.Session.GetString("UserId");
                var result = await _repository.CreateAsync(model);

                if (result.Success)
                {
                    return Json(new { success = true, message = "Third Party Inspection Detail saved successfully." });
                }

                return Json(new { success = false, message = "Failed to save third party inspectiont detail.", id = 0 });

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] ThirdPartyInspection model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = HttpContext.Session.GetString("UserId");

                var operationResult = await _repository.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _repository.DeleteAsync(id);
            return Json(operationResult);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAttachment(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file selected." });

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "Only PDF and image files are allowed." });

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ThirdPartyTrac_Atta", id.ToString());
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var nameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
                var newFileName = $"{nameWithoutExt}_{timestamp}{extension}";
                var filePath = Path.Combine(uploadsPath, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Optional: update DB with the new file name
                await _repository.UpdateAttachmentAsync(id, newFileName);

                return Json(new { success = true, id = id, fileName = newFileName });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> UploadTPIExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<ThirdPartyInspectionViewModel>();

            try
            {
                var uploadedBy = HttpContext.Session.GetString("FullName");
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.LastRowUsed().RowNumber();

                for (int row = 2; row <= rowCount; row++)
                {

                    DateTime? insDate = ParseExcelDate(worksheet.Cell(row, 1));
                    DateTime? momDate = ParseExcelDate(worksheet.Cell(row, 15));

                    var model = new ThirdPartyInspectionViewModel
                    {
                        InspectionDate = insDate,
                        Pc = worksheet.Cell(row, 2).GetString().Trim(),
                        ProjectName = worksheet.Cell(row, 3).GetString().Trim(),
                        InspName = worksheet.Cell(row, 4).GetString().Trim(),
                        ProductCode = worksheet.Cell(row, 5).GetString().Trim(),
                        ProdDesc = worksheet.Cell(row, 6).GetString().Trim(),
                        LOTQty = worksheet.Cell(row, 7).GetString().Trim(),
                        ProjectValue = worksheet.Cell(row, 8).GetString().Trim(),
                        Tpi_Duration = worksheet.Cell(row, 9).GetString().Trim(),
                        Location = worksheet.Cell(row, 10).GetString().Trim(),
                        Mode = worksheet.Cell(row, 11).GetString().Trim(),
                        FirstAttempt = worksheet.Cell(row, 12).GetString().Trim(),
                        Remark = worksheet.Cell(row, 13).GetString().Trim(),
                        ActionPlan = worksheet.Cell(row, 14).GetString().Trim(),
                        MOMDate = momDate,
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _repository.BulkTPICreateAsync(prRecordsToAdd, fileName, uploadedBy, "ThirdPartyInspection");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Project Name";
                    failSheet.Cell(1, 2).Value = "Product Code";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.ProjectName;
                        failSheet.Cell(i, 2).Value = fail.Record.ProductCode;
                        failSheet.Cell(i, 3).Value = fail.Reason;
                        i++;
                    }

                    failWb.SaveAs(failStream);
                    failStream.Position = 0;

                    var failedFileName = $"Failed_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                    Response.Headers["Content-Disposition"] = $"attachment; filename={failedFileName}";
                    return File(failStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                    //string logFileName = $"FailedOpenPO_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    //return File(failStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", logFileName);
                }

                return Json(new
                {
                    success = true,
                    message = $"Import completed. Total: {recordCount}, Saved: {prRecordsToAdd.Count}, Duplicates: 0"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Import failed: " + ex.Message
                });
            }
        }

        // Helper function: Universal Excel date parser
        private DateTime? ParseExcelDate(IXLCell cell)
        {
            try
            {
                if (cell == null || cell.IsEmpty())
                    return null;

                if (cell.DataType == XLDataType.DateTime)
                {
                    return cell.GetDateTime();
                }
                else if (cell.DataType == XLDataType.Number)
                {
                    return DateTime.FromOADate(cell.GetDouble());
                }
                else
                {
                    var strVal = cell.GetString().Trim();

                    string[] formats = { "dd-MMM-yy", "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };

                    if (DateTime.TryParseExact(strVal, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    {
                        if (dt.Year < 50) dt = dt.AddYears(2000 - dt.Year);  // handle 2-digit year case
                        return dt;
                    }
                    else if (DateTime.TryParse(strVal, out dt))
                    {
                        return dt;
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}
