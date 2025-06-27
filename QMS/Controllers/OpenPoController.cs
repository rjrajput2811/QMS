using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;
using QMS.Core.Repositories.OpenPoRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class OpenPoController : Controller
    {

        private readonly IOpenPoReposiotry _openPoReposiotry;
        private readonly ISystemLogService _systemLogService;

        public OpenPoController(ISystemLogService systemLogService, IOpenPoReposiotry openPoReposiotry)
        {
            _openPoReposiotry = openPoReposiotry;
            _systemLogService = systemLogService;
        }
        public IActionResult OpenPo()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var openPoDeatilsList = await _openPoReposiotry.GetListAsync();
            return Json(openPoDeatilsList);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllVendor(string vendor)
        {
            var openPoDeatilsList = await _openPoReposiotry.GetVendorListAsync(vendor);
            return Json(openPoDeatilsList);
        }

        [HttpPost]
        public async Task<IActionResult> UploadOpenPoDataExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<Open_PoViewModel>();

            try
            {
                var uploadedBy = HttpContext.Session.GetString("FullName");
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.RowsUsed().Count();

                for (int row = 2; row <= rowCount; row++)
                {
                    var model = new Open_PoViewModel
                    {
                        Key = worksheet.Cell(row, 1).GetString().Trim(),
                        PR_Type = worksheet.Cell(row, 2).GetString().Trim(),
                        PR_Desc = worksheet.Cell(row, 3).GetString().Trim(),
                        Requisitioner = worksheet.Cell(row, 4).GetString().Trim(),
                        Tracking_No = worksheet.Cell(row, 5).GetString().Trim(),
                        PR_No = worksheet.Cell(row, 6).GetString().Trim(),
                        Batch_No = worksheet.Cell(row, 7).GetString().Trim(),
                        Reference_No = worksheet.Cell(row, 8).GetString().Trim(),
                        Vendor = worksheet.Cell(row, 9).GetString().Trim(),
                        PO_No = worksheet.Cell(row, 10).GetString().Trim(),
                        PO_Date = worksheet.Cell(row, 11).TryGetValue(out DateTime poDate) ? poDate : null,
                        PO_Qty = worksheet.Cell(row, 12).GetValue<int?>(),
                        Balance_Qty = worksheet.Cell(row, 13).GetValue<int?>(),
                        Destination = worksheet.Cell(row, 14).GetString().Trim(),
                        Delivery_Date = worksheet.Cell(row, 15).TryGetValue(out DateTime delDate) ? delDate : null,
                        Balance_Value = worksheet.Cell(row, 16).GetValue<decimal?>(),
                        Material = worksheet.Cell(row, 17).GetString().Trim(),
                        Hold_Date = worksheet.Cell(row, 18).TryGetValue(out DateTime hold) ? hold : null,
                        Cleared_Date = worksheet.Cell(row, 19).TryGetValue(out DateTime clear) ? clear : null,
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _openPoReposiotry.BulkCreateAsync(prRecordsToAdd, fileName, uploadedBy);

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Key";
                    failSheet.Cell(1, 2).Value = "PO_No";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Key;
                        failSheet.Cell(i, 2).Value = fail.Record.PO_No;
                        failSheet.Cell(i, 3).Value = fail.Reason;
                        i++;
                    }

                    failWb.SaveAs(failStream);
                    failStream.Position = 0;

                    var failedFileName = $"FailedOpenPO_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

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

        [HttpGet]
        public async Task<JsonResult> GetDeliverySchedule(int poId)
        {
            var data = await _openPoReposiotry.GetByPOIdAsync(poId);
            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> SaveDeliverySchedule([FromBody]Opne_Po_DeliverySchViewModel request)
        {
            try
            {
                string updatedBy = HttpContext.Session.GetString("FullName") ?? "System";
                await _openPoReposiotry.SaveDeliveryScheduleAsync(request, updatedBy);

                return Json(new { success = true, message = "Delivery Schedule saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetOpenPOWithDelivery(string vendor)
        {
            // If you still want to read vendor from session:
            if (string.IsNullOrEmpty(vendor))
            {
                vendor = HttpContext.Session.GetString("VendorName") ?? "";
            }

            var result = await _openPoReposiotry.GetOpenPOWithDeliveryScheduleAsync(vendor);

            var response = new
            {
                POHeaders = result.poHeaders,
                DeliverySchedules = result.deliverySchedules
            };

            return Json(response);
        }
    }
}
