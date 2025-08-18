using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QMS.Core.Repositories.OpenPoRepository
{
    public class OpenPoReposiotry : SqlTableRepository, IOpenPoReposiotry
    {
        public new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public OpenPoReposiotry(QMSDbContext dbContext,
            ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<Open_PoViewModel>> GetListAsync()
        {
            try
            {

                var result = await _dbContext.OpenPo.FromSqlRaw("EXEC sp_Get_Open_PO_Details").ToListAsync();

                // Map results to ViewModel
                var viewModelList = result.Select(data => new Open_PoViewModel
                {
                    Id = data.Id,
                    Key = data.Key,
                    PR_Type = data.PR_Type,
                    PR_Desc = data.PR_Desc,
                    Requisitioner = data.Requisitioner,
                    Tracking_No = data.Tracking_No,
                    PR_No = data.PR_No,
                    Batch_No = data.Batch_No,
                    Reference_No = data.Reference_No,
                    Vendor = data.Vendor,
                    PO_No = data.PO_No,
                    PO_Date = data.PO_Date,
                    PO_Qty = data.PO_Qty,
                    Balance_Qty = data.Balance_Qty,
                    Destination = data.Destination,
                    Delivery_Date = data.Delivery_Date,
                    Balance_Value = data.Balance_Value,
                    Material = data.Material,
                    Hold_Date = data.Hold_Date,
                    Cleared_Date = data.Cleared_Date,

                    Key1 = data.Key1,
                    Comit_Date = data.Comit_Date,
                    Comit_Qty = data.Comit_Qty,
                    Comit_Planner_Qty = data.Comit_Planner_Qty,
                    Comit_Planner_date = data.Comit_Planner_date,
                    Comit_Vendor_Date = data.Comit_Vendor_Date,
                    Comit_Vendor_Qty = data.Comit_Vendor_Qty,
                    Comit_Planner_Remark = data.Comit_Planner_Remark,
                    Comit_Date1 = data.Comit_Date1,
                    Comit_Qty1 = data.Comit_Qty1,
                    Comit_Final_Date = data.Comit_Final_Date,
                    Comit_Final_Qty = data.Comit_Final_Qty,

                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,

                    UpdatedDate = data.UpdatedDate,
                    UpdatedBy = data.UpdatedBy
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<List<Open_PoViewModel>> GetVendorListAsync(string vendor)
        {
            try
            {

                var parameters = new[]
                {
                    new SqlParameter("@Vendor", vendor ?? (object)DBNull.Value),
                };

                //Execute the stored procedure and retrieve the results
                var result = await _dbContext.OpenPo
                    .FromSqlRaw("EXEC sp_Get_Open_PO_Details_By_Vendor @Vendor", parameters)
                    .ToListAsync();

                // Map results to ViewModel
                var viewModelList = result.Select(data => new Open_PoViewModel
                {
                    Id = data.Id,
                    Key = data.Key,
                    PR_Type = data.PR_Type,
                    PR_Desc = data.PR_Desc,
                    Requisitioner = data.Requisitioner,
                    Tracking_No = data.Tracking_No,
                    PR_No = data.PR_No,
                    Batch_No = data.Batch_No,
                    Reference_No = data.Reference_No,
                    Vendor = data.Vendor,
                    PO_No = data.PO_No,
                    PO_Date = data.PO_Date,
                    PO_Qty = data.PO_Qty,
                    Balance_Qty = data.Balance_Qty,
                    Destination = data.Destination,
                    Delivery_Date = data.Delivery_Date,
                    Balance_Value = data.Balance_Value,
                    Material = data.Material,
                    Hold_Date = data.Hold_Date,
                    Cleared_Date = data.Cleared_Date,

                    Key1 = data.Key1,
                    Comit_Date = data.Comit_Date,
                    Comit_Qty = data.Comit_Qty,
                    Comit_Planner_Qty = data.Comit_Planner_Qty,
                    Comit_Planner_date = data.Comit_Planner_date,
                    Comit_Vendor_Date = data.Comit_Vendor_Date,
                    Comit_Vendor_Qty = data.Comit_Vendor_Qty,
                    Comit_Planner_Remark = data.Comit_Planner_Remark,
                    Comit_Date1 = data.Comit_Date1,
                    Comit_Qty1 = data.Comit_Qty1,
                    Comit_Final_Date = data.Comit_Final_Date,
                    Comit_Final_Qty = data.Comit_Final_Qty,

                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,

                    UpdatedDate = data.UpdatedDate,
                    UpdatedBy = data.UpdatedBy
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        //public async Task<List<Open_PoViewModel>> GetListAsync(string vendor)
        //{
        //    try
        //    {

        //        var result = await _dbContext.OpenPo.FromSqlRaw("EXEC sp_Get_Open_PO_Details").ToListAsync();

        //        // Map results to ViewModel
        //        var viewModelList = result.Select(data => new Open_PoViewModel
        //        {
        //            Id = data.Id,
        //            Key = data.Key,
        //            PR_Type = data.PR_Type,
        //            PR_Desc = data.PR_Desc,
        //            Requisitioner = data.Requisitioner,
        //            Tracking_No = data.Tracking_No,
        //            PR_No = data.PR_No,
        //            Batch_No = data.Batch_No,
        //            Reference_No = data.Reference_No,
        //            Vendor = data.Vendor,
        //            PO_No = data.PO_No,
        //            PO_Date = data.PO_Date,
        //            PO_Qty = data.PO_Qty,
        //            Balance_Qty = data.Balance_Qty,
        //            Destination = data.Destination,
        //            Delivery_Date = data.Delivery_Date,
        //            Balance_Value = data.Balance_Value,
        //            Material = data.Material,
        //            Hold_Date = data.Hold_Date,
        //            Cleared_Date = data.Cleared_Date,
        //            Key1 = data.Key1,
        //            Comit_Date = data.Comit_Date,
        //            Comit_Qty = data.Comit_Qty,
        //            Comit_Planner_Qty = data.Comit_Planner_Qty,
        //            Comit_Planner_date = data.Comit_Planner_date,
        //            Comit_Vendor_Date = data.Comit_Vendor_Date,
        //            Comit_Vendor_Qty = data.Comit_Vendor_Qty,
        //            Comit_Planner_Remark = data.Comit_Planner_Remark,
        //            Comit_Date1 = data.Comit_Date1,
        //            Comit_Qty1 = data.Comit_Qty1,
        //            Comit_Final_Date = data.Comit_Final_Date,
        //            Comit_Final_Qty = data.Comit_Final_Qty,
        //            CreatedDate = data.CreatedDate,
        //            CreatedBy = data.CreatedBy
        //        }).ToList();

        //        return viewModelList;

        //    }
        //    catch (Exception ex)
        //    {
        //        _systemLogService.WriteLog(ex.Message);
        //        throw;
        //    }
        //}

        public async Task<BulkCreateLogResult> BulkCreateAsync(List<Open_PoViewModel> listOfData, string fileName, string uploadedBy)
        {
            var result = new BulkCreateLogResult();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var seenKeys = new HashSet<string>(); // for tracking in-batch duplicates

                foreach (var item in listOfData)
                {
                    item.Key = item.Key?.Trim();
                    item.PO_No = item.PO_No?.Trim();

                    var compositeKey = $"{item.Key}|{item.PO_No}";

                    if (string.IsNullOrWhiteSpace(item.Key) || string.IsNullOrWhiteSpace(item.PO_No))
                    {
                        result.FailedRecords.Add((item, "Missing Key or PO_No"));
                        continue;
                    }

                    // Check if this key combination has already been seen in the batch
                    if (seenKeys.Contains(compositeKey))
                    {
                        result.FailedRecords.Add((item, "Duplicate in uploaded file"));
                        continue;
                    }

                    seenKeys.Add(compositeKey); // Mark this combination as seen

                    // Now check against database
                    var existingEntity = await _dbContext.OpenPo.FirstOrDefaultAsync(x => x.PO_No == item.PO_No);

                    if (existingEntity != null)
                    {
                        // Compare fields one by one — if any field differs, update
                        bool isDifferent =
                            existingEntity.Key != item.Key ||
                            existingEntity.PR_Type != item.PR_Type ||
                            existingEntity.PR_Desc != item.PR_Desc ||
                            existingEntity.Requisitioner != item.Requisitioner ||
                            existingEntity.Tracking_No != item.Tracking_No ||
                            existingEntity.PR_No != item.PR_No ||
                            existingEntity.Batch_No != item.Batch_No ||
                            existingEntity.Reference_No != item.Reference_No ||
                            existingEntity.Vendor != item.Vendor ||
                            existingEntity.PO_Date != item.PO_Date ||
                            existingEntity.PO_Qty != item.PO_Qty ||
                            existingEntity.Balance_Qty != item.Balance_Qty ||
                            existingEntity.Destination != item.Destination ||
                            existingEntity.Delivery_Date != item.Delivery_Date ||
                            existingEntity.Balance_Value != item.Balance_Value ||
                            existingEntity.Material != item.Material ||
                            existingEntity.Hold_Date != item.Hold_Date ||
                            existingEntity.Cleared_Date != item.Cleared_Date;

                        if (isDifferent)
                        {
                            // Update existing entity
                            existingEntity.Key = item.Key;
                            existingEntity.PR_Type = item.PR_Type;
                            existingEntity.PR_Desc = item.PR_Desc;
                            existingEntity.Requisitioner = item.Requisitioner;
                            existingEntity.Tracking_No = item.Tracking_No;
                            existingEntity.PR_No = item.PR_No;
                            existingEntity.Batch_No = item.Batch_No;
                            existingEntity.Reference_No = item.Reference_No;
                            existingEntity.Vendor = item.Vendor;
                            existingEntity.PO_Date = item.PO_Date;
                            existingEntity.PO_Qty = item.PO_Qty;
                            existingEntity.Balance_Qty = item.Balance_Qty;
                            existingEntity.Destination = item.Destination;
                            existingEntity.Delivery_Date = item.Delivery_Date;
                            existingEntity.Balance_Value = item.Balance_Value;
                            existingEntity.Material = item.Material;
                            existingEntity.Hold_Date = item.Hold_Date;
                            existingEntity.Cleared_Date = item.Cleared_Date;
                            existingEntity.CreatedBy = uploadedBy;
                            existingEntity.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            result.FailedRecords.Add((item, "Duplicate in database with no changes"));
                        }
                    }
                    else
                    {
                        // Insert new record
                        var newEntity = new Open_Po
                        {
                            Key = item.Key,
                            PR_Type = item.PR_Type,
                            PR_Desc = item.PR_Desc,
                            Requisitioner = item.Requisitioner,
                            Tracking_No = item.Tracking_No,
                            PR_No = item.PR_No,
                            Batch_No = item.Batch_No,
                            Reference_No = item.Reference_No,
                            Vendor = item.Vendor,
                            PO_No = item.PO_No,
                            PO_Date = item.PO_Date,
                            PO_Qty = item.PO_Qty,
                            Balance_Qty = item.Balance_Qty,
                            Destination = item.Destination,
                            Delivery_Date = item.Delivery_Date,
                            Balance_Value = item.Balance_Value,
                            Material = item.Material,
                            Hold_Date = item.Hold_Date,
                            Cleared_Date = item.Cleared_Date,
                            Key1 = (item.Batch_No ?? string.Empty) + (item.Reference_No ?? string.Empty),
                            CreatedBy = uploadedBy,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.OpenPo.Add(newEntity);
                    }
                }

                await _dbContext.SaveChangesAsync();

                // Log the upload
                var importLog = new Open_Po_Log
                {
                    FileName = fileName,
                    TotalRecords = listOfData.Count,
                    ImportedRecords = listOfData.Count - result.FailedRecords.Count,
                    FailedRecords = result.FailedRecords.Count,
                    UploadedBy = uploadedBy,
                    UploadedAt = DateTime.Now
                };

                _dbContext.OpenPo_Log.Add(importLog);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Result = new OperationResult
                {
                    Success = true,
                    Message = result.FailedRecords.Any()
                        ? "Import completed with some skipped records."
                        : "All records imported successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Result = new OperationResult
                {
                    Success = false,
                    Message = "Error during import: " + ex.Message
                };
            }

            return result;
        }


        public async Task<Opne_Po_DeliverySchViewModel> GetByPOIdAsync(int poId)
        {

            var deliveries = await _dbContext.Opne_Po_Deliveries
        .Where(x => x.Ven_PoId == poId)
        .ToListAsync();

            if (deliveries == null || deliveries.Count == 0)
                return null;

            var header = deliveries.First();

            var viewModel = new Opne_Po_DeliverySchViewModel
            {
                Id = header.Id,
                Ven_PoId = header.Ven_PoId,
                Vendor = header.Vendor,
                PO_No = header.PO_No,
                PO_Date = header.PO_Date,
                PO_Qty = header.PO_Qty,
                Balance_Qty = header.Balance_Qty,
                DeliveryScheduleList = deliveries.Select(d => new DeliveryScheduleItem
                {
                    Delivery_Date = d.Delivery_Date,
                    Delivery_Qty = d.Delivery_Qty,
                    Delivery_Remark = d.Delivery_Remark
                }).ToList()
            };

            return viewModel;
        }

        public async Task SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel model, string updatedBy)
        {
            // First delete existing records
            var existing = await _dbContext.Opne_Po_Deliveries.Where(x => x.Ven_PoId == model.Ven_PoId).ToListAsync();
            _dbContext.Opne_Po_Deliveries.RemoveRange(existing);

            // Insert new records
            foreach (var item in model.DeliveryScheduleList)
            {
                var entity = new Opne_Po_DeliverySchedule
                {
                    Ven_PoId = model.Ven_PoId,
                    Vendor = model.Vendor,
                    PO_No = model.PO_No,
                    PO_Date = model.PO_Date,
                    PO_Qty = model.PO_Qty,
                    Balance_Qty = model.Balance_Qty,
                    Delivery_Date = item.Delivery_Date,
                    Delivery_Qty = item.Delivery_Qty,
                    Delivery_Remark = item.Delivery_Remark,
                    CreatedBy = updatedBy,
                    CreatedDate = DateTime.Now
                };

                _dbContext.Opne_Po_Deliveries.Add(entity);
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task<(List<Open_Po> poHeaders, List<Opne_Po_DeliverySchedule> deliverySchedules)> GetOpenPOWithDeliveryScheduleAsync(string vendor)
        {
            using (var connection = _dbContext.Database.GetDbConnection())
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync("[dbo].[sp_Get_OpenPO_With_DeliverySchedule]", new { Vendor = vendor }, commandType: CommandType.StoredProcedure))

                {
                    var poHeaders = (await multi.ReadAsync<Open_Po>()).ToList();
                    var deliverySchedules = (await multi.ReadAsync<Opne_Po_DeliverySchedule>()).ToList();

                    return (poHeaders, deliverySchedules);
                }
            }
        }



        public async Task<List<Sales_Order_ViewModel>> GetSalesOrderListAsync(string? type)
        {
            try
            {

                var result = await _dbContext.Sales_Order.FromSqlRaw("EXEC sp_Get_SalesOrder_SCM").ToListAsync();

                if (!string.IsNullOrEmpty(type))
                    result = result.Where(x => x.Item_Category_Latest == type).ToList();


                // Map results to ViewModel
                var viewModelList = result.Select(data => new Sales_Order_ViewModel
                {
                    Id = data.Id,
                    SO_No = data.SO_No,
                    SaleOrder_Type = data.SaleOrder_Type,
                    SO_Date = data.SO_Date,
                    Line_Item = data.Line_Item,
                    Indent_No = data.Indent_No,
                    Indent_Date = data.Indent_Date,
                    Order_Type = data.Order_Type,
                    Vertical = data.Vertical,
                    Region = data.Region,
                    Sales_Group = data.Sales_Group,
                    Sales_Group_desc = data.Sales_Group_desc,
                    Sales_Office = data.Sales_Office,
                    Sales_Office_Desc = data.Sales_Office_Desc,
                    Sale_Person = data.Sale_Person,
                    Project_Name = data.Project_Name,
                    Project_Name_Tag = data.Project_Name_Tag,
                    Priority_Tag = data.Priority_Tag,
                    Customer_Name = data.Customer_Name,
                    Customer_Code = data.Customer_Code,
                    Dealer_Direct = data.Dealer_Direct,
                    Inspection = data.Inspection,
                    Material = data.Material,
                    Old_Material_No = data.Old_Material_No,
                    Description = data.Description,
                    SO_Qty = data.SO_Qty,
                    SO_Value = data.SO_Value,
                    Rate = data.Rate,
                    Del_Qty = data.Del_Qty,
                    Open_Sale_Qty = data.Open_Sale_Qty,
                    Opne_Sale_Value = data.Opne_Sale_Value,
                    Plant = data.Plant,
                    Item_Category = data.Item_Category,
                    Item_Category_Latest = data.Item_Category_Latest,
                    Procurement_Type = data.Procurement_Type,
                    Vendor_Po_No = data.Vendor_Po_No,
                    Vendor_Po_Date = data.Vendor_Po_Date,
                    CPR_Number = data.CPR_Number,
                    Vendor = data.Vendor,
                    Planner = data.Planner,
                    Po_Release_Qty = data.Po_Release_Qty,
                    Allocated_Stock_Qty = data.Allocated_Stock_Qty,
                    Allocated_Stock_Value = data.Allocated_Stock_Value,
                    Net_Qty = data.Net_Qty,
                    Net_Value = data.Net_Value,
                    Qty_In_Week = data.Qty_In_Week,
                    Value_In_Week = data.Value_In_Week,
                    Qty_After_Week = data.Qty_After_Week,
                    Value_After_Week = data.Value_After_Week,
                    Check5 = data.Check5,
                    Indent_Status = data.Indent_Status,
                    Sales_Call_Point = data.Sales_Call_Point,
                    Free_Stock = data.Free_Stock,
                    Grn_Qty = data.Grn_Qty,
                    Last_Grn_Date = data.Last_Grn_Date,
                    Check1 = data.Check1,
                    Delivery_Schedule = data.Delivery_Schedule,
                    Readiness_Vendor_Released_Fr_Date = data.Readiness_Vendor_Released_Fr_Date,
                    Readiness_Vendor_Released_To_Date = data.Readiness_Vendor_Released_To_Date,
                    Readiness_Schedule_Vendor_Released = data.Readiness_Schedule_Vendor_Released,
                    Delivery_Schedule_PC_Breakup = data.Delivery_Schedule_PC_Breakup,
                    Check2 = data.Check2,
                    Line_Item_Schedule = data.Line_Item_Schedule,
                    R_B = data.R_B,
                    Schedule_Repeat = data.Schedule_Repeat,
                    Internal_Pending_Issue = data.Internal_Pending_Issue,
                    Pending_With = data.Pending_With,
                    Remark = data.Remark,
                    CRD_OverDue = data.CRD_OverDue,
                    Delivert_Date = data.Delivert_Date,
                    Process_Plan_On_Crd = data.Process_Plan_On_Crd,
                    Last_Week_PC = data.Last_Week_PC,
                    Schedule_Line_Qty1 = data.Schedule_Line_Qty1,
                    Schedule_Line_Date1 = data.Schedule_Line_Date1,
                    Schedule_Line_Qty2 = data.Schedule_Line_Qty2,
                    Schedule_Line_Date2 = data.Schedule_Line_Date2,
                    Schedule_Line_Qty3 = data.Schedule_Line_Qty3,
                    Schedule_Line_Date3 = data.Schedule_Line_Date3,
                    To_Consider = data.To_Consider,
                    Person_Name = data.Person_Name,
                    Visibility = data.Visibility,
                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<List<Sales_Order_ViewModel>> GetSalesOrdersQtyAsync(string? type)
        {
            // Get the connection string from configuration or your context
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<Sales_Order_ViewModel>(
                    "[dbo].[sp_Get_SalesQty_By_Month_OldMaterial]", new { Type = type },
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }

        public async Task<(List<MatchedRecordViewModel> matched, MatchSummaryViewModel? summary)> GetPO_SO_MatchReportAsync(string? type)
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync("sp_PO_SO_Match_Report", new { Type = type }, commandType: CommandType.StoredProcedure))
                {
                    var matched = (await multi.ReadAsync<MatchedRecordViewModel>()).ToList();
                    var summary = (await multi.ReadAsync<MatchSummaryViewModel>()).FirstOrDefault();

                    return (matched, summary);
                }
            }
        }

        public async Task<BulkSalesCreateLogResult> BulkSalesCreateAsync(List<Sales_Order_ViewModel> listOfData, string fileName, string uploadedBy)
        {
            var result = new BulkSalesCreateLogResult();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var seenKeys = new HashSet<string>();

                foreach (var item in listOfData)
                {
                    item.Indent_No = item.Indent_No?.Trim();
                    item.Old_Material_No = item.Old_Material_No?.Trim();

                    var compositeKey = $"{item.Indent_No}|{item.Old_Material_No}";

                    if (string.IsNullOrWhiteSpace(item.Indent_No) || string.IsNullOrWhiteSpace(item.Old_Material_No))
                    {
                        result.FailedRecords.Add((item, "Missing Indent_No or Old_Material_No"));
                        continue;
                    }

                    if (seenKeys.Contains(compositeKey))
                    {
                        result.FailedRecords.Add((item, "Duplicate in uploaded file"));
                        continue;
                    }

                    seenKeys.Add(compositeKey);

                    var existingEntity = await _dbContext.Sales_Order.FirstOrDefaultAsync(x => x.Indent_No == item.Indent_No && x.Old_Material_No == item.Old_Material_No);

                    if (existingEntity != null)
                    {
                        bool isDifferent =
                            existingEntity.SO_No != item.SO_No ||
                            existingEntity.SaleOrder_Type != item.SaleOrder_Type ||
                            existingEntity.SO_Date != item.SO_Date ||
                            existingEntity.Line_Item != item.Line_Item ||
                            existingEntity.Indent_No != item.Indent_No ||
                            existingEntity.Indent_Date != item.Indent_Date ||
                            existingEntity.Order_Type != item.Order_Type ||
                            existingEntity.Vertical != item.Vertical ||
                            existingEntity.Region != item.Region ||
                            existingEntity.Sales_Group != item.Sales_Group ||
                            existingEntity.Sales_Group_desc != item.Sales_Group_desc ||
                            existingEntity.Sales_Office != item.Sales_Office ||
                            existingEntity.Sales_Office_Desc != item.Sales_Office_Desc ||
                            existingEntity.Sale_Person != item.Sale_Person ||
                            existingEntity.Project_Name != item.Project_Name ||
                            existingEntity.Project_Name_Tag != item.Project_Name_Tag ||
                            existingEntity.Priority_Tag != item.Priority_Tag ||
                            existingEntity.Customer_Code != item.Customer_Code ||
                            existingEntity.Customer_Name != item.Customer_Name ||
                            existingEntity.Dealer_Direct != item.Dealer_Direct ||
                            existingEntity.Inspection != item.Inspection ||
                            existingEntity.Material != item.Material ||
                            existingEntity.Old_Material_No != item.Old_Material_No ||
                            existingEntity.Description != item.Description ||
                            existingEntity.SO_Qty != item.SO_Qty ||
                            existingEntity.SO_Value != item.SO_Value ||
                            existingEntity.Rate != item.Rate ||
                            existingEntity.Del_Qty != item.Del_Qty ||
                            existingEntity.Open_Sale_Qty != item.Open_Sale_Qty ||
                            existingEntity.Opne_Sale_Value != item.Opne_Sale_Value ||
                            existingEntity.Plant != item.Plant ||
                            existingEntity.Item_Category != item.Item_Category ||
                            existingEntity.Item_Category_Latest != item.Item_Category_Latest ||
                            existingEntity.Procurement_Type != item.Procurement_Type ||
                            existingEntity.Vendor_Po_No != item.Vendor_Po_No ||
                            existingEntity.Vendor_Po_Date != item.Vendor_Po_Date ||
                            existingEntity.CPR_Number != item.CPR_Number ||
                            existingEntity.Vendor != item.Vendor ||
                            existingEntity.Planner != item.Planner ||
                            existingEntity.Po_Release_Qty != item.Po_Release_Qty ||
                            existingEntity.Allocated_Stock_Qty != item.Allocated_Stock_Qty ||
                            existingEntity.Allocated_Stock_Value != item.Allocated_Stock_Value ||
                            existingEntity.Net_Qty != item.Net_Qty ||
                            existingEntity.Net_Value != item.Net_Value ||
                            existingEntity.Qty_In_Week != item.Qty_In_Week ||
                            existingEntity.Value_In_Week != item.Value_In_Week ||
                            existingEntity.Qty_After_Week != item.Qty_After_Week ||
                            existingEntity.Value_After_Week != item.Value_After_Week ||
                            existingEntity.Check5 != item.Check5 ||
                            existingEntity.Indent_Status != item.Indent_Status ||
                            existingEntity.Sales_Call_Point != item.Sales_Call_Point ||
                            existingEntity.Free_Stock != item.Free_Stock ||
                            existingEntity.Grn_Qty != item.Grn_Qty ||
                            existingEntity.Last_Grn_Date != item.Last_Grn_Date ||
                            existingEntity.Check1 != item.Check1 ||
                            existingEntity.Delivery_Schedule != item.Delivery_Schedule ||
                            existingEntity.Readiness_Vendor_Released_Fr_Date != item.Readiness_Vendor_Released_Fr_Date ||
                            existingEntity.Readiness_Vendor_Released_To_Date != item.Readiness_Vendor_Released_To_Date ||
                            existingEntity.Readiness_Schedule_Vendor_Released != item.Readiness_Schedule_Vendor_Released ||
                            existingEntity.Delivery_Schedule_PC_Breakup != item.Delivery_Schedule_PC_Breakup ||
                            existingEntity.Check2 != item.Check2 ||
                            existingEntity.Line_Item_Schedule != item.Line_Item_Schedule ||
                            existingEntity.R_B != item.R_B ||
                            existingEntity.Schedule_Repeat != item.Schedule_Repeat ||
                            existingEntity.Internal_Pending_Issue != item.Internal_Pending_Issue ||
                            existingEntity.Pending_With != item.Pending_With ||
                            existingEntity.Remark != item.Remark ||
                            existingEntity.CRD_OverDue != item.CRD_OverDue ||
                            existingEntity.Delivert_Date != item.Delivert_Date ||
                            existingEntity.Process_Plan_On_Crd != item.Process_Plan_On_Crd ||
                            existingEntity.Last_Week_PC != item.Last_Week_PC ||
                            existingEntity.Schedule_Line_Qty1 != item.Schedule_Line_Qty1 ||
                            existingEntity.Schedule_Line_Date1 != item.Schedule_Line_Date1 ||
                            existingEntity.Schedule_Line_Qty2 != item.Schedule_Line_Qty2 ||
                            existingEntity.Schedule_Line_Date2 != item.Schedule_Line_Date2 ||
                            existingEntity.Schedule_Line_Qty3 != item.Schedule_Line_Qty3 ||
                            existingEntity.Schedule_Line_Date3 != item.Schedule_Line_Date3 ||
                            existingEntity.To_Consider != item.To_Consider ||
                            existingEntity.Person_Name != item.Person_Name ||
                            existingEntity.Visibility != item.Visibility;

                        if (isDifferent)
                        {
                            existingEntity.SO_No = item.SO_No;
                            existingEntity.SaleOrder_Type = item.SaleOrder_Type;
                            existingEntity.SO_Date = item.SO_Date;
                            existingEntity.Line_Item = item.Line_Item;
                            existingEntity.Indent_No = item.Indent_No;
                            existingEntity.Indent_Date = item.Indent_Date;
                            existingEntity.Order_Type = item.Order_Type;
                            existingEntity.Vertical = item.Vertical;
                            existingEntity.Region = item.Region;
                            existingEntity.Sales_Group = item.Sales_Group;
                            existingEntity.Sales_Group_desc = item.Sales_Group_desc;
                            existingEntity.Sales_Office = item.Sales_Office;
                            existingEntity.Sales_Office_Desc = item.Sales_Office_Desc;
                            existingEntity.Sale_Person = item.Sale_Person;
                            existingEntity.Project_Name = item.Project_Name;
                            existingEntity.Project_Name_Tag = item.Project_Name_Tag;
                            existingEntity.Priority_Tag = item.Priority_Tag;
                            existingEntity.Customer_Code = item.Customer_Code;
                            existingEntity.Customer_Name = item.Customer_Name;
                            existingEntity.Dealer_Direct = item.Dealer_Direct;
                            existingEntity.Inspection = item.Inspection;
                            existingEntity.Material = item.Material;
                            existingEntity.Old_Material_No = item.Old_Material_No;
                            existingEntity.Description = item.Description;
                            existingEntity.SO_Qty = item.SO_Qty;
                            existingEntity.SO_Value = item.SO_Value;
                            existingEntity.Rate = item.Rate;
                            existingEntity.Del_Qty = item.Del_Qty;
                            existingEntity.Open_Sale_Qty = item.Open_Sale_Qty;
                            existingEntity.Opne_Sale_Value = item.Opne_Sale_Value;
                            existingEntity.Plant = item.Plant;
                            existingEntity.Item_Category = item.Item_Category;
                            existingEntity.Item_Category_Latest = item.Item_Category_Latest;
                            existingEntity.Procurement_Type = item.Procurement_Type;
                            existingEntity.Vendor_Po_No = item.Vendor_Po_No;
                            existingEntity.Vendor_Po_Date = item.Vendor_Po_Date;
                            existingEntity.CPR_Number = item.CPR_Number;
                            existingEntity.Vendor = item.Vendor;
                            existingEntity.Planner = item.Planner;
                            existingEntity.Po_Release_Qty = item.Po_Release_Qty;
                            existingEntity.Allocated_Stock_Qty = item.Allocated_Stock_Qty;
                            existingEntity.Allocated_Stock_Value = item.Allocated_Stock_Value;
                            existingEntity.Net_Qty = item.Net_Qty;
                            existingEntity.Net_Value = item.Net_Value;
                            existingEntity.Qty_In_Week = item.Qty_In_Week;
                            existingEntity.Value_In_Week = item.Value_In_Week;
                            existingEntity.Qty_After_Week = item.Qty_After_Week;
                            existingEntity.Value_After_Week = item.Value_After_Week;
                            existingEntity.Check5 = item.Check5;
                            existingEntity.Indent_Status = item.Indent_Status;
                            existingEntity.Sales_Call_Point = item.Sales_Call_Point;
                            existingEntity.Free_Stock = item.Free_Stock;
                            existingEntity.Grn_Qty = item.Grn_Qty;
                            existingEntity.Last_Grn_Date = item.Last_Grn_Date;
                            existingEntity.Check1 = item.Check1;
                            existingEntity.Delivery_Schedule = item.Delivery_Schedule;
                            existingEntity.Readiness_Vendor_Released_Fr_Date = item.Readiness_Vendor_Released_Fr_Date;
                            existingEntity.Readiness_Vendor_Released_To_Date = item.Readiness_Vendor_Released_To_Date;
                            existingEntity.Readiness_Schedule_Vendor_Released = item.Readiness_Schedule_Vendor_Released;
                            existingEntity.Delivery_Schedule_PC_Breakup = item.Delivery_Schedule_PC_Breakup;
                            existingEntity.Check2 = item.Check2;
                            existingEntity.Line_Item_Schedule = item.Line_Item_Schedule;
                            existingEntity.R_B = item.R_B;
                            existingEntity.Schedule_Repeat = item.Schedule_Repeat;
                            existingEntity.Internal_Pending_Issue = item.Internal_Pending_Issue;
                            existingEntity.Pending_With = item.Pending_With;
                            existingEntity.Remark = item.Remark;
                            existingEntity.CRD_OverDue = item.CRD_OverDue;
                            existingEntity.Delivert_Date = item.Delivert_Date;
                            existingEntity.Process_Plan_On_Crd = item.Process_Plan_On_Crd;
                            existingEntity.Last_Week_PC = item.Last_Week_PC;
                            existingEntity.Schedule_Line_Qty1 = item.Schedule_Line_Qty1;
                            existingEntity.Schedule_Line_Date1 = item.Schedule_Line_Date1;
                            existingEntity.Schedule_Line_Qty2 = item.Schedule_Line_Qty2;
                            existingEntity.Schedule_Line_Date2 = item.Schedule_Line_Date2;
                            existingEntity.Schedule_Line_Qty3 = item.Schedule_Line_Qty3;
                            existingEntity.Schedule_Line_Date3 = item.Schedule_Line_Date3;
                            existingEntity.To_Consider = item.To_Consider;
                            existingEntity.Person_Name = item.Person_Name;
                            existingEntity.Visibility = item.Visibility;
                            existingEntity.CreatedBy = uploadedBy;
                            existingEntity.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            result.FailedRecords.Add((item, "Duplicate in database with no changes"));
                        }
                    }
                    else
                    {
                        var newEntity = new Sales_Order_SCM
                        {
                            SO_No = item.SO_No,
                            SaleOrder_Type = item.SaleOrder_Type,
                            SO_Date = item.SO_Date,
                            Line_Item = item.Line_Item,
                            Indent_No = item.Indent_No,
                            Indent_Date = item.Indent_Date,
                            Order_Type = item.Order_Type,
                            Vertical = item.Vertical,
                            Region = item.Region,
                            Sales_Group = item.Sales_Group,
                            Sales_Group_desc = item.Sales_Group_desc,
                            Sales_Office = item.Sales_Office,
                            Sales_Office_Desc = item.Sales_Office_Desc,
                            Sale_Person = item.Sale_Person,
                            Project_Name = item.Project_Name,
                            Project_Name_Tag = item.Project_Name_Tag,
                            Priority_Tag = item.Priority_Tag,
                            Customer_Code = item.Customer_Code,
                            Customer_Name = item.Customer_Name,
                            Dealer_Direct = item.Dealer_Direct,
                            Inspection = item.Inspection,
                            Material = item.Material,
                            Old_Material_No = item.Old_Material_No,
                            Description = item.Description,
                            SO_Qty = item.SO_Qty,
                            SO_Value = item.SO_Value,
                            Rate = item.Rate,
                            Del_Qty = item.Del_Qty,
                            Open_Sale_Qty = item.Open_Sale_Qty,
                            Opne_Sale_Value = item.Opne_Sale_Value,
                            Plant = item.Plant,
                            Item_Category = item.Item_Category,
                            Item_Category_Latest = item.Item_Category_Latest,
                            Procurement_Type = item.Procurement_Type,
                            Vendor_Po_No = item.Vendor_Po_No,
                            Vendor_Po_Date = item.Vendor_Po_Date,
                            CPR_Number = item.CPR_Number,
                            Vendor = item.Vendor,
                            Planner = item.Planner,
                            Po_Release_Qty = item.Po_Release_Qty,
                            Allocated_Stock_Qty = item.Allocated_Stock_Qty,
                            Allocated_Stock_Value = item.Allocated_Stock_Value,
                            Net_Qty = item.Net_Qty,
                            Net_Value = item.Net_Value,
                            Qty_In_Week = item.Qty_In_Week,
                            Value_In_Week = item.Value_In_Week,
                            Qty_After_Week = item.Qty_After_Week,
                            Value_After_Week = item.Value_After_Week,
                            Check5 = item.Check5,
                            Indent_Status = item.Indent_Status,
                            Sales_Call_Point = item.Sales_Call_Point,
                            Free_Stock = item.Free_Stock,
                            Grn_Qty = item.Grn_Qty,
                            Last_Grn_Date = item.Last_Grn_Date,
                            Check1 = item.Check1,
                            Delivery_Schedule = item.Delivery_Schedule,
                            Readiness_Vendor_Released_Fr_Date = item.Readiness_Vendor_Released_Fr_Date,
                            Readiness_Vendor_Released_To_Date = item.Readiness_Vendor_Released_To_Date,
                            Readiness_Schedule_Vendor_Released = item.Readiness_Schedule_Vendor_Released,
                            Delivery_Schedule_PC_Breakup = item.Delivery_Schedule_PC_Breakup,
                            Check2 = item.Check2,
                            Line_Item_Schedule = item.Line_Item_Schedule,
                            R_B = item.R_B,
                            Schedule_Repeat = item.Schedule_Repeat,
                            Internal_Pending_Issue = item.Internal_Pending_Issue,
                            Pending_With = item.Pending_With,
                            Remark = item.Remark,
                            CRD_OverDue = item.CRD_OverDue,
                            Delivert_Date = item.Delivert_Date,
                            Process_Plan_On_Crd = item.Process_Plan_On_Crd,
                            Last_Week_PC = item.Last_Week_PC,
                            Schedule_Line_Qty1 = item.Schedule_Line_Qty1,
                            Schedule_Line_Date1 = item.Schedule_Line_Date1,
                            Schedule_Line_Qty2 = item.Schedule_Line_Qty2,
                            Schedule_Line_Date2 = item.Schedule_Line_Date2,
                            Schedule_Line_Qty3 = item.Schedule_Line_Qty3,
                            Schedule_Line_Date3 = item.Schedule_Line_Date3,
                            To_Consider = item.To_Consider,
                            Person_Name = item.Person_Name,
                            Visibility = item.Visibility,
                            CreatedBy = uploadedBy,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.Sales_Order.Add(newEntity);
                    }
                }

                await _dbContext.SaveChangesAsync();

                // Log the upload
                var importLog = new Open_Po_Log
                {
                    FileName = fileName,
                    TotalRecords = listOfData.Count,
                    ImportedRecords = listOfData.Count - result.FailedRecords.Count,
                    FailedRecords = result.FailedRecords.Count,
                    UploadedBy = uploadedBy,
                    UploadedAt = DateTime.Now
                };

                _dbContext.OpenPo_Log.Add(importLog);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Result = new OperationResult
                {
                    Success = true,
                    Message = result.FailedRecords.Any()
                        ? "Import completed with some skipped records."
                        : "All records imported successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Result = new OperationResult
                {
                    Success = false,
                    Message = "Error during import: " + ex.Message
                };
            }

            return result;
        }

        public async Task<OperationResult> UpdateAsync(Open_Po updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Ven_PoId", updatedRecord.Id),
                    new SqlParameter("@Comit_Date", updatedRecord.Comit_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Qty", updatedRecord.Comit_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Date1", updatedRecord.Comit_Date1 ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Qty1", updatedRecord.Comit_Qty1 ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Final_Date", updatedRecord.Comit_Final_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Final_Qty", updatedRecord.Comit_Final_Qty),
                    new SqlParameter("@Comit_Planner_Qty", updatedRecord.Comit_Planner_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Planner_Date", updatedRecord.Comit_Planner_date ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Planner_Remark", updatedRecord.Comit_Planner_Remark ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Vendor_Date", updatedRecord.Comit_Vendor_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Comit_Vendor_Qty", updatedRecord.Comit_Vendor_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value)
                };

                var sql = @"EXEC sp_Update_Open_PO @Ven_PoId,@Comit_Date,@Comit_Qty,@Comit_Date1,@Comit_Qty1,@Comit_Final_Date,@Comit_Final_Qty,@Comit_Planner_Qty,@Comit_Planner_Date,@Comit_Planner_Remark,
                        @Comit_Vendor_Date,@Comit_Vendor_Qty,@UpdatedBy";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                if (returnUpdatedRecord)
                {
                    return new OperationResult
                    {
                        Success = true,
                    };
                }

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

    }
}
