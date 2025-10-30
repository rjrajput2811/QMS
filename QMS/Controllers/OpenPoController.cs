using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.OpenPoRepository;
using QMS.Core.Services.SystemLogs;
using System.Globalization;

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

        public IActionResult UploadOpenPO()
        {
            return View();
        }

        public IActionResult Vendor()
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
        public async Task<JsonResult> GetAllPOLog()
        {
            var openPoLogDeatilsList = await _openPoReposiotry.GetPoLogListAsync();
            return Json(openPoLogDeatilsList);
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

                var importResult = await _openPoReposiotry.BulkCreateAsync_Dapper(prRecordsToAdd, fileName, uploadedBy);

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
                    return File(failStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", failedFileName);

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

        [HttpPost]
        public async Task<JsonResult> UpdateBuffScheduleAsync(int id,int buff)
        {
            if (ModelState.IsValid)
            {
                //model.UpdatedDate = DateTime.Now;
                //model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _openPoReposiotry.SaveBuffScheduleAsync(id, buff,"");

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }



        public IActionResult SalesOrder()
        {
            return View();
        }

        public IActionResult UploadOpenSO()
        {
            return View();
        }

        public IActionResult MTO()
        {
            return View();
        }

        public IActionResult MTA()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllSOLog()
        {
            var openPoLogDeatilsList = await _openPoReposiotry.GetSOLogListAsync();
            return Json(openPoLogDeatilsList);
        }

        //[HttpGet]
        //public async Task<JsonResult> GetSalesOrderAll(string? type)
        //{
        //    var openPoDeatilsList = await _openPoReposiotry.GetSalesOrderListAsync(type);
        //    return Json(openPoDeatilsList);
        //}

        [HttpGet]
        public async Task<JsonResult> GetSalesOrderAll(string type)
        {
            var result = await _openPoReposiotry.GetSOWithDeliveryScheduleAsync(type);

            var response = new
            {
                SOHeaders = result.soHeaders,
                DeliverySchedules = result.deliverySchedules
            };

            return Json(response);
        }

        [HttpGet]
        public async Task<JsonResult> GetSalesOrderDetail(string type)
        {
            var result = await _openPoReposiotry.GetSalesOrderListAsync(type);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetSalesOrdersQty(string? type)
        {
            var saleOrderQtyList = await _openPoReposiotry.GetSalesOrdersQtyAsync(type);
            return Json(saleOrderQtyList);
        }



        [HttpPost]
        public async Task<IActionResult> UploadSalesOrderDataExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<Sales_Order_ViewModel>();
            var errorList = new List<(int Row, string IndentNo, string OldMaterialNo, string Reason)>();

            try
            {
                var uploadedBy = HttpContext.Session.GetString("FullName");
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.LastRowUsed().RowNumber();  // Accurate count!

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        string GetStr(int col) => worksheet.Cell(row, col).GetValue<string>()?.Trim();
                        int? GetInt(int col)
                        {
                            var val = GetStr(col);
                            return int.TryParse(val, out var result) ? result : null;
                        }
                        double? GetDouble(int col)
                        {
                            var val = GetStr(col)?.Replace(",", "");
                            return double.TryParse(val, out var result) ? result : null;
                        }
                        DateTime? GetDate(int col)
                        {
                            return worksheet.Cell(row, col).GetValue<DateTime?>();
                        }

                        var model = new Sales_Order_ViewModel
                        {
                            SO_No = GetStr(1),
                            SaleOrder_Type = GetStr(2),
                            SO_Date = GetDate(3),
                            Line_Item = GetInt(4),
                            Indent_No = GetStr(5),
                            Indent_Date = GetDate(6),
                            //Order_Type = GetStr(7),
                            //Vertical = GetStr(8),
                            //Region = GetStr(9),
                            Sales_Group = GetStr(7),
                            Sales_Group_desc = GetStr(8),
                            Sales_Office = GetStr(9),
                            Sales_Office_Desc = GetStr(10),
                            Sale_Person = GetStr(11),
                            Project_Name = GetStr(12),
                            //Project_Name_Tag = GetStr(16),
                            //Priority_Tag = GetStr(17),
                            Customer_Code = GetStr(13),
                            Customer_Name = GetStr(14),
                            //Dealer_Direct = GetStr(20),
                            //Inspection = GetStr(21),
                            Material = GetStr(15),
                            Old_Material_No = GetStr(16),
                            Description = GetStr(17),
                            SO_Qty = GetInt(18),
                            SO_Value = GetDouble(19),
                            //Rate = GetDouble(27),
                            Del_Qty = GetInt(20),
                            Open_Sale_Qty = GetInt(21),
                            Opne_Sale_Value = GetDouble(22),
                            Plant = GetStr(23),
                            Item_Category = GetStr(24),
                            //Item_Category_Latest = GetStr(33),
                            Procurement_Type = GetStr(25),
                            Vendor_Po_No = GetStr(26),
                            Vendor_Po_Date = GetDate(27),
                            //CPR_Number = GetInt(37),
                            //Vendor = GetStr(38),
                            //Planner = GetStr(39),
                            Po_Release_Qty = GetInt(28),
                            Allocated_Stock_Qty = GetInt(29),
                            Allocated_Stock_Value = GetDouble(30),
                            //Net_Qty = GetInt(43),
                            //Net_Value = GetDouble(44),
                            //Qty_In_Week = GetInt(45),
                            //Value_In_Week = GetDouble(46),
                            //Qty_After_Week = GetInt(47),
                            //Value_After_Week = GetDouble(48),
                            //// Handle booleans robustly
                            //Check5 = GetStr(49)?.Equals("TRUE", StringComparison.OrdinalIgnoreCase) == true || GetStr(49) == "1",
                            Indent_Status = GetStr(31),
                            //Sales_Call_Point = GetStr(51),
                            //Free_Stock = GetInt(52),
                            //Grn_Qty = GetInt(53),
                            //Last_Grn_Date = GetDate(54),
                            //Check1 = GetStr(55),
                            Delivery_Schedule = GetStr(32),
                            //Readiness_Vendor_Released_Fr_Date = GetStr(57),
                            //Readiness_Vendor_Released_To_Date = GetStr(58),
                            //Readiness_Schedule_Vendor_Released = GetStr(59),
                            //Delivery_Schedule_PC_Breakup = GetStr(60),
                            //Check2 = GetStr(61),
                            //Line_Item_Schedule = GetStr(62),
                            //R_B = GetStr(63),
                            //Schedule_Repeat = GetStr(64),
                            //Internal_Pending_Issue = GetStr(65),
                            //Pending_With = GetStr(66),
                            //Remark = GetStr(67),
                            //CRD_OverDue = GetStr(68),
                            Delivert_Date = GetDate(33),
                            //Process_Plan_On_Crd = GetStr(70),
                            //Last_Week_PC = GetStr(71),
                            Schedule_Line_Qty1 = GetInt(34),
                            Schedule_Line_Date1 = GetDate(35),
                            Schedule_Line_Qty2 = GetInt(36),
                            Schedule_Line_Date2 = GetDate(37),
                            Schedule_Line_Qty3 = GetInt(38),
                            Schedule_Line_Date3 = GetDate(39),
                            //To_Consider = GetStr(78),
                            //Person_Name = GetStr(79),
                            //Visibility = GetStr(80),
                            CreatedBy = uploadedBy,
                            CreatedDate = DateTime.Now
                        };

                        prRecordsToAdd.Add(model);
                    }
                    catch (Exception ex)
                    {
                        // Log error with row, Indent_No, Old_Material_No
                        errorList.Add((row, worksheet.Cell(row, 5).GetValue<string>(), worksheet.Cell(row, 23).GetValue<string>(), ex.Message));
                    }
                }

                var importResult = await _openPoReposiotry.BulkSalesCreateAsync(prRecordsToAdd, fileName, uploadedBy);

                // If there are failed records, return file
                if (importResult.FailedRecords.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var failSheet = failWb.Worksheets.Add("Failed Records");

                    failSheet.Cell(1, 1).Value = "Indent No";
                    failSheet.Cell(1, 2).Value = "Old Material No";
                    failSheet.Cell(1, 3).Value = "Reason";

                    int i = 2;
                    foreach (var fail in importResult.FailedRecords)
                    {
                        failSheet.Cell(i, 1).Value = fail.Record.Indent_No;
                        failSheet.Cell(i, 2).Value = fail.Record.Old_Material_No;
                        failSheet.Cell(i, 3).Value = fail.Reason;
                        i++;
                    }

                    failWb.SaveAs(failStream);
                    failStream.Position = 0;

                    var failedFileName = $"FailedSO_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

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
        public async Task<JsonResult> GetPOSOMatchSummary(string? type = "MTO")
        {
            var result = await _openPoReposiotry.GetPO_SO_MatchReportAsync(type);

            var response = new
            {
                Matched = result.matched,
                DeliverySch = result.deliverySch,
                Summary = result.summary
            };

            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([FromBody] Open_Po model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = HttpContext.Session.GetString("FullName");

                var operationResult = await _openPoReposiotry.UpdateAsync(model);

                return Json(operationResult);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        public IActionResult PCCalendar()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetPCCalendar()
        {
            var openPoDeatilsList = await _openPoReposiotry.GetPCListAsync();
            return Json(openPoDeatilsList);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPCCalendarDataExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var rowsToInsert = new List<PCCalendarViewModel>();
            var parseErrors = new List<(int Row, string From, string To, string Reason)>();

            try
            {
                var uploadedBy = HttpContext.Session.GetString("FullName") ?? "System";
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Position = 0;

                using var wb = new XLWorkbook(ms);
                var ws = wb.Worksheet(1);

                // 1) Find header row (“PC”, “Week”, “FROM”, “TO”)
                int headerRow = 0;
                var lastUsed = ws.LastRowUsed()?.RowNumber() ?? 0;
                for (int r = 1; r <= Math.Min(lastUsed, 50); r++)
                {
                    string c1 = ws.Cell(r, 1).GetValue<string>().Trim();
                    string c2 = ws.Cell(r, 2).GetValue<string>().Trim();
                    string c3 = ws.Cell(r, 3).GetValue<string>().Trim().ToUpperInvariant();
                    string c4 = ws.Cell(r, 4).GetValue<string>().Trim().ToUpperInvariant();

                    if (string.Equals(c1, "PC", StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(c2, "Week", StringComparison.OrdinalIgnoreCase) &&
                        c3.Contains("FROM") && c4.Contains("TO"))
                    {
                        headerRow = r;
                        break;
                    }
                }
                if (headerRow == 0)
                    return Json(new { success = false, message = "Header row not found. Expected columns: PC | Week | FROM | TO | DAYS" });

                // helpers
                static int? ParseIntOrNull(string s)
                    => int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) ? v : (int?)null;

                static DateTime? GetCellDate(IXLCell cell)
                {
                    // numeric Excel date
                    if (cell.DataType == XLDataType.DateTime)
                        return cell.GetDateTime().Date;

                    var raw = cell.GetValue<string>()?.Trim();
                    if (string.IsNullOrWhiteSpace(raw)) return null;

                    // sometimes Excel shows overflow "#####"
                    if (raw.All(ch => ch == '#')) return null;

                    // try a few formats (handles “Tuesday, April 1, 2025” etc.)
                    string[] fmts = {
                "dddd, MMMM d, yyyy", "dddd, MMMM dd, yyyy",
                "MMMM d, yyyy", "MMMM dd, yyyy",
                "M/d/yyyy", "MM/dd/yyyy", "d/M/yyyy", "dd/MM/yyyy",
                "yyyy-MM-dd"
            };
                    if (DateTime.TryParseExact(raw, fmts, CultureInfo.InvariantCulture,
                                               DateTimeStyles.AllowWhiteSpaces, out var dt))
                        return dt.Date;

                    // final fallback
                    if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dt))
                        return dt.Date;

                    return null;
                }

                int? lastPc = null;

                // 2) Iterate data rows
                for (int r = headerRow + 1; r <= lastUsed; r++)
                {
                    try
                    {
                        string pcStr = ws.Cell(r, 1).GetValue<string>()?.Trim();
                        string weekStr = ws.Cell(r, 2).GetValue<string>()?.Trim();

                        // potential total/footer lines – ignore
                        string fromRaw = ws.Cell(r, 3).GetValue<string>()?.Trim();
                        string toRaw = ws.Cell(r, 4).GetValue<string>()?.Trim();
                        string daysStr = ws.Cell(r, 5).GetValue<string>()?.Trim();

                        bool looksLikeGrandTotal = (daysStr == "365");
                        bool looksLikeQuarterTotal =
                            daysStr is "90" or "91" or "92" or "26" or "27" or "28" or "36" or "37" or "38" // your sheet shows these as quarter/month block sums
                            && string.IsNullOrEmpty(fromRaw) && string.IsNullOrEmpty(toRaw);

                        // also ignore completely blank lines
                        bool blankLine = string.IsNullOrEmpty(pcStr) && string.IsNullOrEmpty(weekStr)
                                         && string.IsNullOrEmpty(fromRaw) && string.IsNullOrEmpty(toRaw) && string.IsNullOrEmpty(daysStr);

                        if (looksLikeGrandTotal || looksLikeQuarterTotal || blankLine)
                            continue;

                        // carry forward PC when merged/blank in sheet
                        int? pc = ParseIntOrNull(pcStr);
                        if (pc.HasValue) lastPc = pc;
                        else pc = lastPc; // carry

                        int? week = ParseIntOrNull(weekStr);

                        DateTime? from = GetCellDate(ws.Cell(r, 3));
                        DateTime? to = GetCellDate(ws.Cell(r, 4));

                        // if nothing meaningful, skip
                        if (!pc.HasValue && !week.HasValue && !from.HasValue && !to.HasValue)
                            continue;

                        // compute Days only for valid date rows
                        int? days = null;
                        if (from.HasValue && to.HasValue && to.Value >= from.Value)
                        {
                            days = (int)(to.Value - from.Value).TotalDays + 1;
                        }

                        var model = new PCCalendarViewModel
                        {
                            PC = pc,
                            Week = week,
                            From = from,
                            To = to,
                            Days = days, // ignore quarter/total numbers
                            CreatedBy = uploadedBy,
                            CreatedDate = DateTime.Now
                        };

                        rowsToInsert.Add(model);
                    }
                    catch (Exception exRow)
                    {
                        parseErrors.Add((r, ws.Cell(r, 3).GetValue<string>(), ws.Cell(r, 4).GetValue<string>(), "Row parse error: " + exRow.Message));
                    }
                }

                // Send to repository
                var importResult = await _openPoReposiotry.BulkPCCreateAsync(rowsToInsert, fileName, uploadedBy);

                // If repo returned failures or we had parse errors, build a Fail workbook
                if (importResult.FailedRecords.Any() || parseErrors.Any())
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var sheet = failWb.Worksheets.Add("Failed Records");

                    sheet.Cell(1, 1).Value = "Row";
                    sheet.Cell(1, 2).Value = "PC";
                    sheet.Cell(1, 3).Value = "Week";
                    sheet.Cell(1, 4).Value = "From";
                    sheet.Cell(1, 5).Value = "To";
                    sheet.Cell(1, 6).Value = "Reason";

                    int i = 2;
                    foreach (var f in importResult.FailedRecords)
                    {
                        sheet.Cell(i, 1).Value = i - 1;
                        sheet.Cell(i, 2).Value = f.Record.PC;
                        sheet.Cell(i, 3).Value = f.Record.Week;
                        sheet.Cell(i, 4).Value = f.Record.From;
                        sheet.Cell(i, 5).Value = f.Record.To;
                        sheet.Cell(i, 6).Value = f.Reason;
                        i++;
                    }
                    foreach (var e in parseErrors)
                    {
                        sheet.Cell(i, 1).Value = e.Row;
                        sheet.Cell(i, 4).Value = e.From;
                        sheet.Cell(i, 5).Value = e.To;
                        sheet.Cell(i, 6).Value = e.Reason;
                        i++;
                    }

                    failWb.SaveAs(failStream);
                    failStream.Position = 0;
                    var failedFileName = $"FailedPCCalendar_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    Response.Headers["Content-Disposition"] = $"attachment; filename={failedFileName}";
                    return File(failStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }

                return Json(new
                {
                    success = true,
                    message = importResult.Result?.Message ?? $"Import completed. Parsed {rowsToInsert.Count} rows."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Import failed: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPODeliDateByMatrialRefNo(string? material, string? oldMaterialNo, int soId)
        {
            var result = await _openPoReposiotry.GetPOListByMaterialRefNoAsync(material,oldMaterialNo,soId);
            var response = new
            {
                poResponse = result.openPo,
                soResponse = result.deliverySch
            };

            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> SOSaveDeliverySchedule([FromBody] So_DeliveryScheduleViewModel request)
        {
            try
            {
                string updatedBy = HttpContext.Session.GetString("FullName") ?? "System";
                await _openPoReposiotry.SOSaveDeliverySchAsync(request, updatedBy);

                return Json(new { success = true, message = "SO Delivery Schedule saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult MTAMaster()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetMTADetail()
        {
            var openPoDeatilsList = await _openPoReposiotry.GetMTAListAsync();
            return Json(openPoDeatilsList);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMTADataExcel(IFormFile file, string fileName, string uploadDate, int recordCount)
        {
            var prRecordsToAdd = new List<MTAMasterViewModel>();

            try
            {
                var uploadedBy = HttpContext.Session.GetString("FullName") ?? "System" ;
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.LastRowUsed().RowNumber();  // Accurate count!

                for (int row = 2; row <= rowCount; row++)
                {
                    string GetStr(int col) => worksheet.Cell(row, col).GetValue<string>()?.Trim();
                    int? GetInt(int col)
                    {
                        var val = GetStr(col);
                        return int.TryParse(val, out var result) ? result : null;
                    }
                    double? GetDouble(int col)
                    {
                        var val = GetStr(col)?.Replace(",", "");
                        return double.TryParse(val, out var result) ? result : null;
                    }
                    DateTime? GetDate(int col)
                    {
                        return worksheet.Cell(row, col).GetValue<DateTime?>();
                    }

                    var model = new MTAMasterViewModel
                    {
                        Material_No = GetStr(2),
                        Ref_Code = GetStr(3),
                        Material_Desc = GetStr(4),
                        Tog = GetInt(5),
                        Tor = GetInt(6),
                        Toy = GetInt(7),
                        Spike_Threshold = GetInt(8),
                        Material_Category = GetStr(9),
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now,
                    };

                    prRecordsToAdd.Add(model);
                }

                var importResult = await _openPoReposiotry.BulkMTACreateAsync(prRecordsToAdd, fileName, uploadedBy);

                // If there are failed records, return file
                //if (importResult.FailedRecords.Any())
                //{
                //    using var failStream = new MemoryStream();
                //    using var failWb = new XLWorkbook();
                //    var failSheet = failWb.Worksheets.Add("Failed Records");

                //    failSheet.Cell(1, 1).Value = "Material No";
                //    failSheet.Cell(1, 2).Value = "Ref Code";
                //    failSheet.Cell(1, 3).Value = "Reason";

                //    int i = 2;
                //    foreach (var fail in importResult.FailedRecords)
                //    {
                //        failSheet.Cell(i, 1).Value = fail.Record.Material_No;
                //        failSheet.Cell(i, 2).Value = fail.Record.Ref_Code;
                //        failSheet.Cell(i, 3).Value = fail.Reason;
                //        i++;
                //    }

                //    failWb.SaveAs(failStream);
                //    failStream.Position = 0;

                //    var failedFileName = $"FailedOpenPO_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                //    Response.Headers["Content-Disposition"] = $"attachment; filename={failedFileName}";
                //    return File(failStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                //    //string logFileName = $"FailedOpenPO_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                //    //return File(failStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", logFileName);
                //}

                if (importResult.FailedRecords.Any() )
                {
                    using var failStream = new MemoryStream();
                    using var failWb = new XLWorkbook();
                    var sheet = failWb.Worksheets.Add("Failed Records");

                    sheet.Cell(1, 1).Value = "Row";
                    sheet.Cell(1, 2).Value = "Material No";
                    sheet.Cell(1, 3).Value = "Ref Code";
                    sheet.Cell(1, 4).Value = "Reason";

                    int i = 2;

                    foreach (var f in importResult.FailedRecords)
                    {
                        sheet.Cell(i, 1).Value = i - 1;
                        sheet.Cell(i, 2).Value = f.Record.Material_No;
                        sheet.Cell(i, 3).Value = f.Record.Ref_Code;
                        sheet.Cell(i, 4).Value = f.Reason;
                        i++;
                    }

                    failWb.SaveAs(failStream);
                    failStream.Position = 0;
                    var failedFileName = $"FailedPCCalendar_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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
    }
}
