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

        

        public IActionResult SalesOrder()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetSalesOrderAll(string? type)
        {
            var openPoDeatilsList = await _openPoReposiotry.GetSalesOrderListAsync(type);
            return Json(openPoDeatilsList);
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
                            Order_Type = GetStr(7),
                            Vertical = GetStr(8),
                            Region = GetStr(9),
                            Sales_Group = GetStr(10),
                            Sales_Group_desc = GetStr(11),
                            Sales_Office = GetStr(12),
                            Sales_Office_Desc = GetStr(13),
                            Sale_Person = GetStr(14),
                            Project_Name = GetStr(15),
                            Project_Name_Tag = GetStr(16),
                            Priority_Tag = GetStr(17),
                            Customer_Code = GetStr(18),
                            Customer_Name = GetStr(19),
                            Dealer_Direct = GetStr(20),
                            Inspection = GetStr(21),
                            Material = GetStr(22),
                            Old_Material_No = GetStr(23),
                            Description = GetStr(24),
                            SO_Qty = GetInt(25),
                            SO_Value = GetDouble(26),
                            Rate = GetDouble(27),
                            Del_Qty = GetInt(28),
                            Open_Sale_Qty = GetInt(29),
                            Opne_Sale_Value = GetDouble(30),
                            Plant = GetStr(31),
                            Item_Category = GetStr(32),
                            Item_Category_Latest = GetStr(33),
                            Procurement_Type = GetStr(34),
                            Vendor_Po_No = GetStr(35),
                            Vendor_Po_Date = GetDate(36),
                            CPR_Number = GetInt(37),
                            Vendor = GetStr(38),
                            Planner = GetStr(39),
                            Po_Release_Qty = GetInt(40),
                            Allocated_Stock_Qty = GetInt(41),
                            Allocated_Stock_Value = GetDouble(42),
                            Net_Qty = GetInt(43),
                            Net_Value = GetDouble(44),
                            Qty_In_Week = GetInt(45),
                            Value_In_Week = GetDouble(46),
                            Qty_After_Week = GetInt(47),
                            Value_After_Week = GetDouble(48),
                            // Handle booleans robustly
                            Check5 = GetStr(49)?.Equals("TRUE", StringComparison.OrdinalIgnoreCase) == true || GetStr(49) == "1",
                            Indent_Status = GetStr(50),
                            Sales_Call_Point = GetStr(51),
                            Free_Stock = GetInt(52),
                            Grn_Qty = GetInt(53),
                            Last_Grn_Date = GetDate(54),
                            Check1 = GetStr(55),
                            Delivery_Schedule = GetStr(56),
                            Readiness_Vendor_Released_Fr_Date = GetStr(57),
                            Readiness_Vendor_Released_To_Date = GetStr(58),
                            Readiness_Schedule_Vendor_Released = GetStr(59),
                            Delivery_Schedule_PC_Breakup = GetStr(60),
                            Check2 = GetStr(61),
                            Line_Item_Schedule = GetStr(62),
                            R_B = GetStr(63),
                            Schedule_Repeat = GetStr(64),
                            Internal_Pending_Issue = GetStr(65),
                            Pending_With = GetStr(66),
                            Remark = GetStr(67),
                            CRD_OverDue = GetStr(68),
                            Delivert_Date = GetDate(69),
                            Process_Plan_On_Crd = GetStr(70),
                            Last_Week_PC = GetStr(71),
                            Schedule_Line_Qty1 = GetInt(72),
                            Schedule_Line_Date1 = GetDate(73),
                            Schedule_Line_Qty2 = GetInt(74),
                            Schedule_Line_Date2 = GetDate(75),
                            Schedule_Line_Qty3 = GetInt(76),
                            Schedule_Line_Date3 = GetDate(77),
                            To_Consider = GetStr(78),
                            Person_Name = GetStr(79),
                            Visibility = GetStr(80),
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
        public async Task<JsonResult> GetPOSOMatchSummary(string? type)
        {
            var result = await _openPoReposiotry.GetPO_SO_MatchReportAsync(type);

            var response = new
            {
                Matched = result.matched,
                Summary = result.summary
            };

            return Json(response);
        }
    }
}
