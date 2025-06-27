using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.COPQComplaintDumpRepository;
using QMS.Core.Services.SystemLogs;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace QMS.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IComplaintIndentDumpRepository _copqRepository;
        private readonly ISystemLogService _systemLogService;
        private readonly IJobWorkTracRepository _jobWorkRepository;

        public ServiceController(IComplaintIndentDumpRepository copqRepository,
            ISystemLogService systemLogService, IJobWorkTracRepository jobWorkRepository)
        {
            _copqRepository = copqRepository;
            _systemLogService = systemLogService;
            _jobWorkRepository = jobWorkRepository;
        }

        // Main page/view
        public IActionResult Service()
        {
            return View();
        }


        //// ----------------- Complaint Dump ------------------- ////

        // Get all records with optional filtering by date range
        [HttpGet]
        public async Task<JsonResult> GetAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _copqRepository.GetListAsync(startDate, endDate);
            return Json(list);
        }

        // Get a record by id
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var item = await _copqRepository.GetByIdAsync(id);
            return Json(item);
        }

        // Create a new record
        [HttpPost]
        [Route("Service/CreateAsync")]
        public async Task<JsonResult> CreateAsync([FromBody] ComplaintDump_Service model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data" });

                // Calculate TotalCompletionDays if both dates are present
                int? totalCompletionDays = null;
                if (model.CCCNDate.HasValue && model.Completion.HasValue)
                {
                    totalCompletionDays = (model.Completion.Value - model.CCCNDate.Value).Days;
                }

                model.TotalDays_Close = totalCompletionDays;

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                model.Deleted = false;

                var operationResult = await _copqRepository.CreateAsync(model);

                if (operationResult != null && operationResult.Success)
                    return Json(new { success = true, message = "Saved successfully." });

                return Json(new { success = false, message = "Failed to save." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while saving." });
            }
        }

        // Update existing record
        [HttpPost]
        [Route("Service/UpdateAsync")]
        public async Task<JsonResult> UpdateAsync([FromBody] ComplaintDump_Service model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid update data" });

                int? totalCompletionDays = null;
                if (model.CCCNDate.HasValue && model.Completion.HasValue)
                {
                    totalCompletionDays = (model.Completion.Value - model.CCCNDate.Value).Days;
                }

                model.TotalDays_Close = totalCompletionDays;

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var result = await _copqRepository.UpdateAsync(model);

                if (result != null && result.Success)
                    return Json(new { success = true, message = "Updated successfully." });

                return Json(new { success = false, message = "Update failed." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error during update." });
            }
        }

        // Delete a record by id
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var operationResult = await _copqRepository.DeleteAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while deleting record." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UploadComplaintDumpExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<ComplaintViewModel>();

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

                    DateTime? cccnDate = ParseExcelDate(worksheet.Cell(row, 2));
                    DateTime? completionDate = ParseExcelDate(worksheet.Cell(row, 9));

                    var model = new ComplaintViewModel
                    {
                        CCN_No = worksheet.Cell(row, 1).GetString().Trim(),
                        CCCNDate = cccnDate,
                        ReportedBy = worksheet.Cell(row, 3).GetString().Trim(),
                        CLocation = worksheet.Cell(row, 4).GetString().Trim(),
                        CustName = worksheet.Cell(row, 5).GetString().Trim(),
                        DealerName = worksheet.Cell(row, 6).GetString().Trim(),
                        CDescription = worksheet.Cell(row, 7).GetString().Trim(),
                        CStatus = worksheet.Cell(row, 8).GetString().Trim(),
                        Completion = completionDate,
                        Remarks = worksheet.Cell(row, 10).GetString().Trim(),
                        TotalDays_Close = worksheet.Cell(row, 11).GetValue<int?>(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _copqRepository.BulkCreateAsync(prRecordsToAdd, fileName, uploadedBy, "ComplaintDump");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "CCNNO";
                    failSheet.Cell(1, 2).Value = "CustName";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.CCN_No;
                        failSheet.Cell(i, 2).Value = fail.Record.CustName;
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


        //// ----------------- Complaint Dump ------------------- ////


        //// ----------------- Po List ------------------- ////

        // Get PO list with optional date range
        [HttpGet]
        public async Task<JsonResult> GetAllPO(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var list = await _copqRepository.GetPOListAsync(startDate, endDate);
                return Json(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetAll PO Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to load PO list." });
            }
        }

        // GET: /PO/GetById/5
        [HttpGet]
        public async Task<JsonResult> GetByIdPO(int id)
        {
            try
            {
                var po = await _copqRepository.GetPOByIdAsync(id);
                if (po == null)
                    return Json(new { success = false, message = "PO not found." });

                return Json(new { success = true, data = po });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetById PO Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to get PO details." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreatePO([FromBody] PendingPo_Service model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid PO data." });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName") ?? "System";
                model.Deleted = false;

                var result = await _copqRepository.CreatePOAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "PO created successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to create PO." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Create PO Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while creating PO." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdatePO([FromBody] PendingPo_Service model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid PO update data." });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName") ?? "System";

                var result = await _copqRepository.UpdatePOAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "PO updated successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to update PO." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Update PO Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while updating PO." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> PODelete(int id)
        {
            try
            {
                var result = await _copqRepository.DeletePOAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Delete PO Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while deleting PO." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPoDumpExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<PendingPoViewModel>();

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
                    var model = new PendingPoViewModel
                    {
                        Vendor = worksheet.Cell(row, 1).GetString().Trim(),
                        Material = worksheet.Cell(row, 2).GetString().Trim(),
                        ReferenceNo = worksheet.Cell(row, 3).GetString().Trim(),
                        PONo = worksheet.Cell(row, 4).GetString().Trim(),
                        PODate = worksheet.Cell(row, 5).TryGetValue(out DateTime poDate) ? poDate : null,
                        BatchNo = worksheet.Cell(row, 6).GetString().Trim(),
                        POQty = worksheet.Cell(row, 7).GetString().Trim(),
                        BalanceQty = worksheet.Cell(row, 8).GetString().Trim(),
                        Destination = worksheet.Cell(row, 9).GetString().Trim(),
                        BalanceValue = worksheet.Cell(row, 10).GetString().Trim(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _copqRepository.BulkCreatePoAsync(prRecordsToAdd, fileName, uploadedBy, "PoDump");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Vendor";
                    failSheet.Cell(1, 2).Value = "PONo";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Vendor;
                        failSheet.Cell(i, 2).Value = fail.Record.PONo;
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


        //// ----------------- Po List ------------------- ////
        
        
        //// ----------------- Indant Dump ------------------- ////

        // Get PO list with optional date range
        [HttpGet]
        public async Task<JsonResult> GetAllIndent(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var list = await _copqRepository.GetIndentListAsync(startDate, endDate);
                return Json(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetAll Indent Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to load Indent list." });
            }
        }

        // GET: /PO/GetById/5
        [HttpGet]
        public async Task<JsonResult> GetByIdIndent(int id)
        {
            try
            {
                var po = await _copqRepository.GetIndentByIdAsync(id);
                if (po == null)
                    return Json(new { success = false, message = "Indent not found." });

                return Json(new { success = true, data = po });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetById PO Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to get PO details." });
            }
        }
        [HttpPost]
        public async Task<JsonResult> CreateIndent([FromBody] IndentDump_Service model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid Indent data." });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName") ?? "System";
                model.Deleted = false;

                var result = await _copqRepository.CreateIndentAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "Indent Dump created successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to create Indent." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Create PO Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while creating Indent." });
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateIndent([FromBody] IndentDump_Service model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid Indent update data." });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName") ?? "System";

                var result = await _copqRepository.UpdateIndentAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "Indent Dump updated successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to update Indent." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Update PO Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while updating Indent." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> IndentDelete(int id)
        {
            try
            {
                var result = await _copqRepository.DeleteIndentAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Delete Indent Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while deleting Indent." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadIndentDumpExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<IndentDumpViewModel>();

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
                    var model = new IndentDumpViewModel
                    {
                        Indent_No = worksheet.Cell(row, 1).GetString().Trim(),
                        Indent_Date = worksheet.Cell(row, 2).TryGetValue(out DateTime inDate) ? inDate : null,
                        Business_Unit = worksheet.Cell(row, 3).GetString().Trim(),
                        Vertical = worksheet.Cell(row, 4).GetString().Trim(),
                        Branch = worksheet.Cell(row, 5).GetString().Trim(),
                        Indent_Status = worksheet.Cell(row, 6).GetString().Trim(),
                        End_Cust_Name = worksheet.Cell(row, 7).GetString().Trim(),
                        CCN_No = worksheet.Cell(row, 8).GetString().Trim(),
                        Customer_Code = worksheet.Cell(row, 9).GetString().Trim(),
                        Customer_Name = worksheet.Cell(row, 10).GetString().Trim(),
                        Bill_Req_Date = worksheet.Cell(row, 11).TryGetValue(out DateTime billDate) ? billDate : null,
                        Created_By = worksheet.Cell(row, 12).GetString().Trim(),
                        Wipro_Commit_Date = worksheet.Cell(row, 13).TryGetValue(out DateTime wpDate) ? wpDate : null,
                        Material_No = worksheet.Cell(row, 14).GetString().Trim(),
                        Item_Description = worksheet.Cell(row, 15).GetString().Trim(),
                        Quantity = worksheet.Cell(row, 16).GetValue<int?>(),
                        Price = worksheet.Cell(row, 17).GetString().Trim(),
                        Discount = worksheet.Cell(row, 18).GetString().Trim(),
                        Final_Price = worksheet.Cell(row, 19).GetString().Trim(),
                        SapSoNo = worksheet.Cell(row, 20).GetString().Trim(),
                        CreateSoQty = worksheet.Cell(row, 21).GetValue<int?>(),
                        Inv_Qty = worksheet.Cell(row, 22).GetValue<int?>(),
                        Inv_Value = worksheet.Cell(row, 23).GetString().Trim(),
                        WiproCatelog_No = worksheet.Cell(row, 24).GetString().Trim(),
                        Batch_Code = worksheet.Cell(row, 25).GetString().Trim(),
                        Batch_Date = worksheet.Cell(row, 26).TryGetValue(out DateTime btDate) ? btDate : null,
                        Main_Prodcode = worksheet.Cell(row, 27).GetString().Trim(),
                        User_Name = worksheet.Cell(row, 28).GetString().Trim(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _copqRepository.BulkCreateIndentAsync(prRecordsToAdd, fileName, uploadedBy, "IndentDump");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Material No";
                    failSheet.Cell(1, 2).Value = "Customer Name";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Material_No;
                        failSheet.Cell(i, 2).Value = fail.Record.Customer_Name;
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


        //// ----------------- Indant Dump ------------------- ////

        
        //// ----------------- Invoice List ------------------- ////

        [HttpGet]
        public async Task<JsonResult> GetAllInvoice(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var list = await _copqRepository.GetInvoiceListAsync(startDate, endDate);
                return Json(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetAll Invoice Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to load Invoice list." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetByIdInvoice(int id)
        {
            try
            {
                var po = await _copqRepository.GetInvoiceByIdAsync(id);
                if (po == null)
                    return Json(new { success = false, message = "Invoice not found." });

                return Json(new { success = true, data = po });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetById PO Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to get PO details." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateInvoice([FromBody] Invoice_Service model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid Invoice data." });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName") ?? "System";
                model.Deleted = false;

                var result = await _copqRepository.CreateInvoiceAsync(model);

                if (result.Success)
                    return Json(new { success = true, message = "Invoice created successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to create Invoice." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Create PO Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while creating Indent." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateInvoice([FromBody] Invoice_Service model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid Invoice update data." });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName") ?? "System";

                var result = await _copqRepository.UpdateInvoiceAsync(model);

                if (result.Success)
                    return Json(new { success = true, message = "Invoice updated successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to update Invoice." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Update PO Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while updating Invoice." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> InvoiceDelete(int id)
        {
            try
            {
                var result = await _copqRepository.DeleteInvoiceAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Delete Invoice Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while deleting Invoice." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadInvoiceExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<InvoiceListViewModel>();

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
                    var model = new InvoiceListViewModel
                    {
                        Key = worksheet.Cell(row, 1).GetString().Trim(),
                        Inv_No = worksheet.Cell(row, 2).GetString().Trim(),
                        Inv_Type = worksheet.Cell(row, 3).GetString().Trim(),
                        Sales_Order = worksheet.Cell(row, 4).GetString().Trim(),
                        Plant_Code = worksheet.Cell(row, 5).GetString().Trim(),
                        Plant_Name = worksheet.Cell(row, 6).GetString().Trim(),
                        Material_No = worksheet.Cell(row, 7).GetString().Trim(),
                        Dealer_Name = worksheet.Cell(row, 8).GetString().Trim(),
                        End_Customer = worksheet.Cell(row, 9).GetString().Trim(),
                        Collective_No = worksheet.Cell(row, 10).GetString().Trim(),
                        Indent_No = worksheet.Cell(row, 11).GetString().Trim(),
                        Inv_Date = worksheet.Cell(row, 12).TryGetValue(out DateTime invDate) ? invDate : null,
                        Quantity = worksheet.Cell(row, 13).GetString().Trim(),
                        Cost = worksheet.Cell(row, 14).GetString().Trim(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _copqRepository.BulkCreateInvoiceAsync(prRecordsToAdd, fileName, uploadedBy, "InvoiceList");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Key";
                    failSheet.Cell(1, 2).Value = "Inv_No";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Key;
                        failSheet.Cell(i, 2).Value = fail.Record.Inv_No;
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


        //// ----------------- Invoice List ------------------- ////


        
        //// ----------------- Pc Chart ------------------- ////

        // Get PO list with optional date range
        [HttpGet]
        public async Task<JsonResult> GetAllPc(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var list = await _copqRepository.GetPcListAsync(startDate, endDate);
                return Json(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetAll Pc Chart Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to load Pc Chart." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetByIdPc(int id)
        {
            try
            {
                var po = await _copqRepository.GetPcByIdAsync(id);
                if (po == null)
                    return Json(new { success = false, message = "Pc Chart not found." });

                return Json(new { success = true, data = po });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetById Pc Chart Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to get Pc Chart." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreatePc([FromBody] PcChart_Service model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid Pc Chart data." });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName") ?? "System";
                model.Deleted = false;

                var result = await _copqRepository.CreatePcAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "Pc Chart created successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to create Pc Chart." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Create Pc Chart Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while creating Pc Chart." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdatePc([FromBody] PcChart_Service model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid Pc Chart update data." });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName") ?? "System";

                var result = await _copqRepository.UpdatePcAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "Pc Chart updated successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to update Pc Chart." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Update Pc Chart Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while updating Pc Chart." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> PcDelete(int id)
        {
            try
            {
                var result = await _copqRepository.DeletePcAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Delete Pc Chart Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while deleting Pc Chart." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPcChartExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<PcChartViewModel>();

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
                    var model = new PcChartViewModel
                    {
                        Date = worksheet.Cell(row, 1).TryGetValue(out DateTime date) ? date : null,
                        PC = worksheet.Cell(row, 2).GetString().Trim(),
                        FY = worksheet.Cell(row, 3).GetString().Trim(),
                        Qtr = worksheet.Cell(row, 4).GetString().Trim(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _copqRepository.BulkCreatePcAsync(prRecordsToAdd, fileName, uploadedBy, "PcChart");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Date";
                    failSheet.Cell(1, 2).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Date;
                        failSheet.Cell(i, 2).Value = fail.Reason;
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


        //// ----------------- Pc Chart ------------------- ////


       
        //// ----------------- Region ------------------- ////


        [HttpGet]
        public async Task<JsonResult> GetAllRegion(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var list = await _copqRepository.GetRegListAsync(startDate, endDate);
                return Json(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetAll Region Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to load Region." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetByIdRegion(int id)
        {
            try
            {
                var po = await _copqRepository.GetRegByIdAsync(id);
                if (po == null)
                    return Json(new { success = false, message = "Region not found." });

                return Json(new { success = true, data = po });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"GetById Region Error: {ex.Message}");
                return Json(new { success = false, message = "Failed to get Region." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateRegion([FromBody] Region_Service model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid Region data." });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName") ?? "System";
                model.Deleted = false;

                var result = await _copqRepository.CreateRegAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "Region created successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to create Region." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Create Region Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while creating Region." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRegion([FromBody] Region_Service model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid Region update data." });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName") ?? "System";

                var result = await _copqRepository.UpdateRegAsync(model);
                if (result.Success)
                    return Json(new { success = true, message = "Region updated successfully." });

                return Json(new { success = false, message = result.Message ?? "Failed to update Region." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Update Region Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while updating Region." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> RegionDelete(int id)
        {
            try
            {
                var result = await _copqRepository.DeleteRegAsync(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Delete Region Error: {ex.Message}");
                return Json(new { success = false, message = "Error occurred while deleting Region." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadRegionExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<RegionViewModel>();

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
                    var model = new RegionViewModel
                    {
                        Location = worksheet.Cell(row, 1).GetString().Trim(),
                        Region = worksheet.Cell(row, 2).GetString().Trim(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _copqRepository.BulkCreateRegAsync(prRecordsToAdd, fileName, uploadedBy, "Region");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Location";
                    failSheet.Cell(1, 2).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Location;
                        failSheet.Cell(i, 2).Value = fail.Reason;
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


        //// ----------------- Region ------------------- ////



        public IActionResult JobWorkTracking()
        {
            return View();
        }

        //// ----------------- Job Work Tracker ------------------- ////

        [HttpGet]
        public async Task<JsonResult> GetJobWorkAll(DateTime? startDate, DateTime? endDate)
        {
            var list = await _jobWorkRepository.GetJobListAsync(startDate, endDate);
            return Json(list);
        }

        [HttpGet]
        public async Task<JsonResult> GetJobWorkById(int id)
        {
            var item = await _jobWorkRepository.GetJobByIdAsync(id);
            return Json(item);
        }

        [HttpPost]
        public async Task<JsonResult> CreateJobWorkAsync([FromBody] JobWork_Tracking_Service model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data" });

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = HttpContext.Session.GetString("FullName");
                model.Deleted = false;

                var operationResult = await _jobWorkRepository.CreateJobAsync(model);

                if (operationResult != null && operationResult.Success)
                    return Json(new { success = true, message = "Saved successfully." });

                return Json(new { success = false, message = "Failed to save." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while saving." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateJobWorkAsync([FromBody] JobWork_Tracking_Service model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return Json(new { success = false, message = "Invalid update data" });

                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var result = await _jobWorkRepository.UpdateJobAsync(model);

                if (result != null && result.Success)
                    return Json(new { success = true, message = "Updated successfully." });

                return Json(new { success = false, message = "Update failed." });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error during update." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> JobWorkDelete(int id)
        {
            try
            {
                var operationResult = await _jobWorkRepository.DeleteJobAsync(id);
                return Json(operationResult);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Error occurred while deleting record." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadJobWorkExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<JobWork_TracViewModel>();

            try
            {
                var uploadedBy = HttpContext.Session.GetString("FullName");
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.RowsUsed().Count();

                for (int row = 8; row <= rowCount; row++)
                {
                    var model = new JobWork_TracViewModel
                    {
                        Vendor = worksheet.Cell(row, 1).GetString().Trim(),
                        Wipro_Dc_No = worksheet.Cell(row, 2).GetString().Trim(),
                        Wipro_Dc_Date = worksheet.Cell(row, 3).TryGetValue(out DateTime dcDate) ? dcDate : null,
                        Dc_Sap_Code = worksheet.Cell(row, 4).GetString().Trim(),
                        Qty_Wipro_Dc = worksheet.Cell(row, 5).GetString().Trim(),
                        Wipro_Transporter = worksheet.Cell(row, 6).GetString().Trim(),
                        Wipro_LR_No = worksheet.Cell(row, 7).GetString().Trim(),
                        Wipro_LR_Date = worksheet.Cell(row, 8).TryGetValue(out DateTime lrDate) ? lrDate : null,
                        Actu_Rece_Qty = worksheet.Cell(row, 9).GetString().Trim(),
                        Dispatch_Dc = worksheet.Cell(row, 9).GetString().Trim(),
                        Dispatch_Invoice = worksheet.Cell(row, 9).GetString().Trim(),
                        Non_Repairable = worksheet.Cell(row, 9).GetString().Trim(),
                        Grand_Total = worksheet.Cell(row, 9).GetString().Trim(),
                        To_Process = worksheet.Cell(row, 9).GetString().Trim(),
                        Remark = worksheet.Cell(row, 9).GetString().Trim(),
                        Vendor_Transporter = worksheet.Cell(row, 9).GetString().Trim(),
                        Vendor_LR_No = worksheet.Cell(row, 9).GetString().Trim(),
                        Vendor_LR_Date = worksheet.Cell(row, 9).TryGetValue(out DateTime vLRDate) ? vLRDate : null,
                        Write_Off_Approved = worksheet.Cell(row, 9).GetString().Trim(),
                        Write_Off_Date = worksheet.Cell(row, 9).TryGetValue(out DateTime wDate) ? wDate : null,
                        Pending_Write_Off = worksheet.Cell(row, 9).GetString().Trim(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _jobWorkRepository.BulkCreateJobAsync(prRecordsToAdd, fileName, uploadedBy, "JobWorkTrac");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Vendor";
                    failSheet.Cell(1, 2).Value = "Wipro Dc Date";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Vendor;
                        failSheet.Cell(i, 2).Value = fail.Record.Wipro_Dc_Date;
                        failSheet.Cell(i, 3).Value = fail.Reason;
                        i++;
                    }

                    failWb.SaveAs(failStream);
                    failStream.Position = 0;

                    var failedFileName = $"Failed_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                    Response.Headers["Content-Disposition"] = $"attachment; filename={failedFileName}";
                    return File(failStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

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


        //// ----------------- Job Work Tracker ------------------- ////



        [HttpGet]
        public async Task<IActionResult> GetVendor()
        {
            try
            {
                var vendorList = await _copqRepository.GetVendorDropdownAsync();
                return Json(vendorList);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return StatusCode(500, "Error retrieving vendor dropdown.");
            }
        }

       
    }
}
