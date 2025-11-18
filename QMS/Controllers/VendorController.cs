using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.SystemLogs;
using System.Globalization;

namespace QMS.Controllers
{
    public class VendorController : Controller
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly ISystemLogService _systemLogService;

        public VendorController(IVendorRepository vendorRepository, ISystemLogService systemLogService)
        {
            _vendorRepository = vendorRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult Vendor()
        {
            return View();
        }

        public async Task<IActionResult> VendorDetail(int id)
        {
            var model = new VendorViewModel();

            if (id > 0)
            {
                model = await _vendorRepository.GetByIdAsync(id);
                if (model == null)
                {
                    model = new VendorViewModel();
                }
            }
            else
            {
                model = new VendorViewModel();
            }

            return View(model); // Always return the model, whether new or fetched
        }


        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var vendorList = await _vendorRepository.GetListAsync();
            return Json(vendorList);
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int Id)
        {
            var vendor = await _vendorRepository.GetByIdAsync(Id);
            return Json(vendor);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(Vendor model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var operationResult = new OperationResult();
                    bool existingResult = await _vendorRepository.CheckDuplicate(model.Name.Trim(), 0);
                    if (!existingResult)
                    {
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        operationResult = await _vendorRepository.CreateAsync(model);

                        if (operationResult != null)
                        {
                            return Json(new { success = true, message = "Product Code saved successfully.", id = operationResult.ObjectId });
                        }

                        return Json(new { success = false, message = "Failed to save product code.", id = 0 });
                    }
                    else
                    {
                        operationResult.Success = false;
                        operationResult.Message = "Exist";
                        operationResult.Payload = existingResult;
                        return Json(operationResult);
                    }
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { Success = false, Errors = errors });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync(Vendor model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _vendorRepository.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var operationResult = await _vendorRepository.DeleteAsync(id);
            return Json(operationResult);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Invalid certificate ID." });

                var cert = await _vendorRepository.CertGetByIdAsync(id);
                if (cert == null)
                    return Json(new { success = false, message = "Certificate not found." });

                cert.Deleted = true;
                cert.UpdatedDate = DateTime.Now;
                cert.UpdatedBy = HttpContext.Session.GetString("FullName");

                var updateSuccess = await _vendorRepository.CertUpdateAsync(cert);

                return Json(new
                {
                    success = updateSuccess,
                    message = "Certificate deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"DeleteCertificate Error: {ex.Message}\n{ex.StackTrace}");
                return Json(new { success = false, message = "An error occurred while deleting the certificate." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificate([FromForm] CertificationDetail model, IFormFile certUpload)
        {
            try
            {
                bool isUpdate = model.Id > 0;
                const long maxFileSize = 100 * 1024 * 1024; // 100MB
                string fileRelativePath = null;

                // Insert record into the database first
                if (!isUpdate)
                {
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = HttpContext.Session.GetString("FullName");
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = model.CreatedBy;

                    // Save the record to the database (insert operation)
                    var result = await _vendorRepository.CertCreateAsync(model);

                    if (result == null || !result.Success || result.ObjectId <= 0)
                        return Json(new { success = false, message = "Failed to add certificate." });

                    model.Id = (int)result.ObjectId; // Get the generated Id from the result
                }

                // Now that the record is inserted, create the folder using the Id
                if (certUpload != null && certUpload.Length > 0)
                {
                    if (certUpload.Length > maxFileSize)
                        return Json(new { success = false, message = "File size exceeds the maximum limit of 100MB." });

                    var extension = Path.GetExtension(certUpload.FileName);
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var fileName = $"{model.VendorCode}_{timestamp}{extension}";

                    // Define the folder path inside wwwroot/CertificationDetail/{model.Id}
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CertificationDetail", model.VendorCode.ToString());

                    // Always check if the folder exists and create it if not
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath); // Ensure folder is created
                    }

                    var filePath = Path.Combine(folderPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await certUpload.CopyToAsync(stream);

                    // Set relative file path to store in the model
                    fileRelativePath = Path.Combine("CertificationDetail", model.VendorCode.ToString(), fileName).Replace("\\", "/");

                    // Save file path to the model
                    model.CertUpload = fileRelativePath;
                }

                // Update record if file was uploaded or if any field needs updating
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                if (isUpdate)
                {
                    var existing = await _vendorRepository.CertGetByIdAsync(model.Id); // Make sure this method exists
                    if (existing == null)
                        return Json(new { success = false, message = "Certificate not found for update." });

                    // Update fields
                    existing.CertificateMasterId = model.CertificateMasterId;
                    existing.IssueDate = model.IssueDate;
                    existing.ExpiryDate = model.ExpiryDate;
                    existing.UpdatedBy = model.UpdatedBy;
                    existing.UpdatedDate = model.UpdatedDate;
                    existing.ProductCode = model.ProductCode;
                    existing.Remarks = model.Remarks;
                    //existing.CertUpload = existing.CertUpload;
                    if (!string.IsNullOrEmpty(fileRelativePath))
                        existing.CertUpload = fileRelativePath;

                    await _vendorRepository.CertUpdateAsync(existing);

                    return Json(new { success = true, message = "Certificate updated successfully." });
                }
                else
                {
                    // If creating a new certificate, save the file path and return success
                    await _vendorRepository.CertUpdateAsync(model); // Ensure you update the path after creation

                    return Json(new { success = true, message = "Certificate added successfully.", id = model.Id });
                }
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"CreateCertificate Error: {ex.Message}\n{ex.StackTrace}");
                return Json(new { success = false, message = "Failed to process certificate." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadCertiExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<CertificationDetailViewModel>();

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

                    DateTime? issueDate = ParseExcelDate(worksheet.Cell(row, 4));
                    DateTime? expiryDate = ParseExcelDate(worksheet.Cell(row, 5));

                    var model = new CertificationDetailViewModel
                    {
                        CertificateName = worksheet.Cell(row, 1).GetString().Trim(),
                        ProductCode = worksheet.Cell(row, 2).GetString().Trim(),
                        VendorCode = worksheet.Cell(row, 3).GetString().Trim(),
                        IssueDate = issueDate,
                        ExpiryDate = expiryDate,
                        CertUpload = worksheet.Cell(row, 6).GetString().Trim(),
                        Remarks = worksheet.Cell(row, 7).GetString().Trim(),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _vendorRepository.BulkCertiCreateAsync(prRecordsToAdd, fileName, uploadedBy, "Certificate");

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Certificate Name";
                    failSheet.Cell(1, 2).Value = "Product Code";
                    failSheet.Cell(1, 3).Value = "Vendor";
                    failSheet.Cell(1, 4).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.CertificateName;
                        failSheet.Cell(i, 2).Value = fail.Record.ProductCode;
                        failSheet.Cell(i, 3).Value = fail.Record.VendorCode;
                        failSheet.Cell(i, 4).Value = fail.Reason;
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

        [HttpGet]
        public async Task<JsonResult> GetCodeSearchAsync(string search = "")
        {
            try
            {
                // Initialize processed search terms
                string processedSearch = string.Empty;

                if (!string.IsNullOrEmpty(search))
                {
                    if (search.Length >= 4)
                        processedSearch = search.Substring(0, 4); // First 4 characters
                }

                var productCodeDetailsList = await _vendorRepository.GetCodeSearchAsync(processedSearch);

                return Json(productCodeDetailsList);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //[HttpGet()]
        //public async Task<JsonResult> GetProductCode(string term = null)
        //{
        //    // Fetch via your repository
        //    var codes = await _vendorRepository.GetProductCodesAsync(term);

        //    // Shape into { id, text } for the dropdown
        //    var result = codes.Select(c => new
        //    {
        //        id = c.OldPart_No,
        //        text = c.Description
        //    });

        //    // Return as JSON
        //    return Json(result);
        //}

        [HttpGet]
        public async Task<JsonResult> GetCertificateData(int? venId)
        {
            try
            {
                var certList = await _vendorRepository.CertGetAllAsync(venId);
                return Json(new { success = true, data = certList });
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Failed to retrieve certificates." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTPTReport([FromForm] ThirdPartyTestReport model, IFormFile ReportUpload)
        {
            try
            {
                bool isUpdate = model.Id > 0;
                const long maxFileSize = 100 * 1024 * 1024; // 100MB
                string fileRelativePath = null;

                // Insert record into the database first
                if (!isUpdate)
                {
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = HttpContext.Session.GetString("FullName");
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = model.CreatedBy;

                    // Save the record to the database (insert operation)
                    var result = await _vendorRepository.ReportCreateAsync(model);

                    if (result == null || !result.Success || result.ObjectId <= 0)
                        return Json(new { success = false, message = "Failed to add Report." });

                    model.Id = (int)result.ObjectId; // Get the generated Id from the result
                }

                // Now that the record is inserted, create the folder using the Id
                if (ReportUpload != null && ReportUpload.Length > 0)
                {
                    if (ReportUpload.Length > maxFileSize)
                        return Json(new { success = false, message = "File size exceeds the maximum limit of 100MB." });

                    var extension = Path.GetExtension(ReportUpload.FileName);
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var fileName = $"{model.VendorCode}_{timestamp}{extension}";

                    // Define the folder path inside wwwroot/CertificationDetail/{model.Id}
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TPTestingReportAttachments", model.VendorCode.ToString());

                    if (Directory.Exists(folderPath))
                    {
                        // Delete all files in the folder
                        var files = Directory.GetFiles(folderPath);
                        foreach (var file in files)
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                    else
                    {
                        // Create the folder if it doesn't exist
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await ReportUpload.CopyToAsync(stream);

                    // Set relative file path to store in the model
                    fileRelativePath = Path.Combine("TPTestingReportAttachments", model.VendorCode.ToString(), fileName).Replace("\\", "/");

                    // Save file path to the model
                    model.ReportFileName = fileRelativePath;
                }

                // Update record if file was uploaded or if any field needs updating
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                if (isUpdate)
                {
                    var existing = await _vendorRepository.ReportGetByIdAsync(model.Id); // Make sure this method exists
                    if (existing == null)
                        return Json(new { success = false, message = "Report not found for update." });

                    // Update fields
                    existing.ThirdPartyReportID = model.ThirdPartyReportID;
                    existing.IssueDate = model.IssueDate;
                    existing.ExpiryDate = model.ExpiryDate;
                    existing.UpdatedBy = model.UpdatedBy;
                    existing.UpdatedDate = model.UpdatedDate;
                    existing.ProductCode = model.ProductCode;
                    existing.Remarks = model.Remarks;
                  //  existing.ReportFileName = model.ReportFileName;
                    if (!string.IsNullOrEmpty(fileRelativePath))
                        existing.ReportFileName = fileRelativePath;

                    await _vendorRepository.ReportUpdateAsync(existing);

                    return Json(new { success = true, message = "Report updated successfully." });
                }
                else
                {
                    
                    await _vendorRepository.ReportUpdateAsync(model); 
                    return Json(new { success = true, message = "Report added successfully.", id = model.Id });
                }
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"CreateReport Error: {ex.Message}\n{ex.StackTrace}");
                return Json(new { success = false, message = "Failed to process Report." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTPTReport(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Invalid Report ID." });

                var report = await _vendorRepository.ReportGetByIdAsync(id);
                if (report == null)
                    return Json(new { success = false, message = "Report not found." });

              //  report.Deleted = true;
             //   report.UpdatedDate = DateTime.Now;
                var UpdatedBy = HttpContext.Session.GetString("FullName");

                await _vendorRepository.ReportDeleteAsync(id, UpdatedBy);

                return Json(new
                {
                    success = true,
                    message = "Report deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog($"Delete Error: {ex.Message}\n{ex.StackTrace}");
                return Json(new { success = false, message = "An error occurred while deleting." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTPTReportData()
        {
            try
            {
                var certList = await _vendorRepository.ReportGetAllAsync();
                return Json(new { success = true, data = certList });
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                _systemLogService.WriteLog(ex.Message);
                return Json(new { success = false, message = "Failed to retrieve certificates." });
            }
        }


            // POST: /VenBISCertificate/CreateOrUpdate
            [HttpPost]
            public async Task<IActionResult> CreateOrUpdateBISCertificate([FromForm] VenBISCertificate model, IFormFile file)
            {
                try
                {
                    bool isUpdate = model.Id > 0;
                    const long maxFileSize = 100 * 1024 * 1024; // 100MB
                    string fileRelativePath = null;

                    // Insert record into the database first if it's a new certificate
                    if (!isUpdate)
                    {
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = HttpContext.Session.GetString("FullName");
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = HttpContext.Session.GetString("FullName");

                    // Save the record to the database (insert operation)
                    var result = await _vendorRepository.CreateOrUpdateBISCertificateAsync(model);

                        if (!result)
                            return Json(new { success = false, message = "Failed to add BIS Certificate." });

                        // Set model ID if inserted successfully
                        model.Id = model.Id;
                    }

                    // Handle file upload (if present)
                    if (file != null && file.Length > 0)
                    {
                        if (file.Length > maxFileSize)
                            return Json(new { success = false, message = "File size exceeds the maximum limit of 100MB." });

                        var extension = Path.GetExtension(file.FileName);
                        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        var fileName = $"{model.VendorCode}_{timestamp}{extension}";

                        // Define the folder path inside wwwroot/BISCertificateAttachments/{model.Id}
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BISCertificateAttachments", model.VendorCode.ToString());

                        if (Directory.Exists(folderPath))
                        {
                            // Delete all files in the folder
                            var files = Directory.GetFiles(folderPath);
                            foreach (var f in files)
                            {
                                System.IO.File.Delete(f);
                            }
                        }
                        else
                        {
                            // Create the folder if it doesn't exist
                            Directory.CreateDirectory(folderPath);
                        }

                        var filePath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                            await file.CopyToAsync(stream);

                        // Set relative file path to store in the model
                        fileRelativePath = Path.Combine("BISCertificateAttachments", model.VendorCode.ToString(), fileName).Replace("\\", "/");

                        // Save file path to the model
                        model.FileName = fileRelativePath;
                    }

                    // Update record if file was uploaded or if any field needs updating
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = HttpContext.Session.GetString("FullName");

                    if (isUpdate)
                    {
                        var existing = await _vendorRepository.GetBISCertificateByIdAsync(model.Id);
                        if (existing == null)
                            return Json(new { success = false, message = "BIS Certificate not found for update." });

                        // Update fields
//existing.FileName = model.FileName;
                        existing.ProductCode = model.ProductCode;
                        existing.IssueDate = model.IssueDate;
                        existing.ExpiryDate = model.ExpiryDate;
                        existing.Remarks = model.Remarks;
                    existing.RNumber = model.RNumber;
                    existing.CertificateDetail = model.CertificateDetail;
                    existing.BISSection = model.BISSection;
                    existing.ModelNo = model.ModelNo;

                        if (!string.IsNullOrEmpty(fileRelativePath))
                            existing.FileName = fileRelativePath;

                        await _vendorRepository.CreateOrUpdateBISCertificateAsync(existing);

                        return Json(new { success = true, message = "BIS Certificate updated successfully." });
                    }
                    else
                    {
                        // Save new BIS certificate
                        await _vendorRepository.CreateOrUpdateBISCertificateAsync(model);
                        return Json(new { success = true, message = "BIS Certificate added successfully.", id = model.Id });
                    }
                }
                catch (Exception ex)
                {
                    _systemLogService.WriteLog($"CreateOrUpdate Error: {ex.Message}\n{ex.StackTrace}");
                    return Json(new { success = false, message = "Failed to process BIS Certificate." });
                }
            }

            // POST: /VenBISCertificate/Delete
            [HttpPost]
            public async Task<IActionResult> DeleteBISCertificate(int id)
            {
                try
                {
                    if (id <= 0)
                        return Json(new { success = false, message = "Invalid BIS Certificate ID." });

                    var certificate = await _vendorRepository.GetBISCertificateByIdAsync(id);
                    if (certificate == null)
                        return Json(new { success = false, message = "BIS Certificate not found." });

                    var updatedBy = HttpContext.Session.GetString("FullName");
                    var result = await _vendorRepository.DeleteBISCertificateAsync(id, updatedBy);

                    return Json(new { success = result, message = result ? "BIS Certificate deleted successfully." : "Failed to delete BIS Certificate." });
                }
                catch (Exception ex)
                {
                    _systemLogService.WriteLog($"Delete Error: {ex.Message}\n{ex.StackTrace}");
                    return Json(new { success = false, message = "An error occurred while deleting." });
                }
            }

            // GET: /VenBISCertificate/GetAll
            [HttpGet]
            public async Task<JsonResult> GetAllBISCertificates(int? venId)
            {
                try
                {
                    var certificates = await _vendorRepository.GetAllBISCertificatesAsync(venId);
                    return Json(new { success = true, data = certificates });
                }
                catch (Exception ex)
                {
                    _systemLogService.WriteLog(ex.Message);
                    return Json(new { success = false, message = "Failed to retrieve BIS Certificates." });
                }
            }
        }
    }


