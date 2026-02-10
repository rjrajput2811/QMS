using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
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
                    TOC_BRYG_Color = data.TOC_BRYG_Color,
                    Vendor_Dispatch_Date = data.Vendor_Dispatch_Date,

                    Key1 = data.Key1,
                    Comit_Date = data.Comit_Date,
                    Comit_Qty = data.Comit_Qty,
                    Comit_Planner_Qty = data.Comit_Planner_Qty,
                    Comit_Planner_date = data.Comit_Planner_date,
                    PCWeekDate = data.PCWeekDate,
                    Comit_Vendor_Date = data.Comit_Vendor_Date,
                    Comit_Vendor_Qty = data.Comit_Vendor_Qty,
                    Comit_Planner_Remark = data.Comit_Planner_Remark,
                    Comit_Date1 = data.Comit_Date1,
                    Comit_Qty1 = data.Comit_Qty1,
                    Comit_Final_Date = data.Comit_Final_Date,
                    Comit_Final_Qty = data.Comit_Final_Qty,
                    Buffer_Day = data.Buffer_Day,

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

        public async Task<List<Open_Po_LogViewModel>> GetPoLogListAsync()
        {
            try
            {

                var result = await _dbContext.OpenPo_Log.FromSqlRaw("EXEC sp_Get_PO_Log_Details").ToListAsync();

                // Map results to ViewModel
                var viewModelList = result.Select(data => new Open_Po_LogViewModel
                {
                    Id = data.Id,
                    FileName = data.FileName,
                    TotalRecords = data.TotalRecords,
                    ImportedRecords = data.ImportedRecords,
                    FailedRecords = data.FailedRecords,
                    UploadedAt = data.UploadedAt,
                    UploadedBy = data.UploadedBy,
                    FileType = data.FileType
                }).Where(x => x.FileType == "PO").ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<List<Open_Po_LogViewModel>> GetSOLogListAsync()
        {
            try
            {

                var result = await _dbContext.OpenPo_Log.FromSqlRaw("EXEC sp_Get_PO_Log_Details").ToListAsync();

                // Map results to ViewModel
                var viewModelList = result.Select(data => new Open_Po_LogViewModel
                {
                    Id = data.Id,
                    FileName = data.FileName,
                    TotalRecords = data.TotalRecords,
                    ImportedRecords = data.ImportedRecords,
                    FailedRecords = data.FailedRecords,
                    UploadedAt = data.UploadedAt,
                    UploadedBy = data.UploadedBy,
                    FileType = data.FileType
                }).Where(x => x.FileType == "SO").ToList();

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
                    TOC_BRYG_Color = data.TOC_BRYG_Color,
                    Vendor_Dispatch_Date = data.Vendor_Dispatch_Date,

                    Key1 = data.Key1,
                    Comit_Date = data.Comit_Date,
                    Comit_Qty = data.Comit_Qty,
                    Comit_Planner_Qty = data.Comit_Planner_Qty,
                    Comit_Planner_date = data.Comit_Planner_date,
                    PCWeekDate = data.PCWeekDate,
                    Comit_Vendor_Date = data.Comit_Vendor_Date,
                    Comit_Vendor_Qty = data.Comit_Vendor_Qty,
                    Comit_Planner_Remark = data.Comit_Planner_Remark,
                    Comit_Date1 = data.Comit_Date1,
                    Comit_Qty1 = data.Comit_Qty1,
                    Comit_Final_Date = data.Comit_Final_Date,
                    Comit_Final_Qty = data.Comit_Final_Qty,
                    Buffer_Day = data.Buffer_Day,

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

        // Working ///
        //public async Task<BulkCreateLogResult> BulkCreateAsync(List<Open_PoViewModel> listOfData, string fileName, string uploadedBy)
        //{
        //    var result = new BulkCreateLogResult();
        //    using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var batchId = Guid.NewGuid();
        //        var uploadTs = DateTime.Now;

        //        var seenKeys = new HashSet<string>(); // for tracking in-batch duplicates

        //        foreach (var item in listOfData)
        //        {
        //            item.Key = item.Key?.Trim();
        //            item.PO_No = item.PO_No?.Trim();

        //            var compositeKey = $"{item.Key}|{item.PO_No}";

        //            if (string.IsNullOrWhiteSpace(item.Key) || string.IsNullOrWhiteSpace(item.PO_No))
        //            {
        //                result.FailedRecords.Add((item, "Missing Key or PO_No"));
        //                continue;
        //            }

        //            // Check if this key combination has already been seen in the batch
        //            if (seenKeys.Contains(compositeKey))
        //            {
        //                result.FailedRecords.Add((item, "Duplicate in uploaded file"));
        //                continue;
        //            }

        //            seenKeys.Add(compositeKey); // Mark this combination as seen

        //            // Now check against database
        //            var existingEntity = await _dbContext.OpenPo.FirstOrDefaultAsync(x => x.PO_No == item.PO_No);

        //            if (existingEntity != null)
        //            {
        //                // Compare fields one by one — if any field differs, update
        //                bool isDifferent =
        //                    existingEntity.Key != item.Key ||
        //                    existingEntity.PR_Type != item.PR_Type ||
        //                    existingEntity.PR_Desc != item.PR_Desc ||
        //                    existingEntity.Requisitioner != item.Requisitioner ||
        //                    existingEntity.Tracking_No != item.Tracking_No ||
        //                    existingEntity.PR_No != item.PR_No ||
        //                    existingEntity.Batch_No != item.Batch_No ||
        //                    existingEntity.Reference_No != item.Reference_No ||
        //                    existingEntity.Vendor != item.Vendor ||
        //                    existingEntity.PO_Date != item.PO_Date ||
        //                    existingEntity.PO_Qty != item.PO_Qty ||
        //                    existingEntity.Balance_Qty != item.Balance_Qty ||
        //                    existingEntity.Destination != item.Destination ||
        //                    existingEntity.Delivery_Date != item.Delivery_Date ||
        //                    existingEntity.Balance_Value != item.Balance_Value ||
        //                    existingEntity.Material != item.Material ||
        //                    existingEntity.Hold_Date != item.Hold_Date ||
        //                    existingEntity.Cleared_Date != item.Cleared_Date;

        //                if (isDifferent)
        //                {
        //                    // Update existing entity
        //                    existingEntity.Key = item.Key;
        //                    existingEntity.PR_Type = item.PR_Type;
        //                    existingEntity.PR_Desc = item.PR_Desc;
        //                    existingEntity.Requisitioner = item.Requisitioner;
        //                    existingEntity.Tracking_No = item.Tracking_No;
        //                    existingEntity.PR_No = item.PR_No;
        //                    existingEntity.Batch_No = item.Batch_No;
        //                    existingEntity.Reference_No = item.Reference_No;
        //                    existingEntity.Vendor = item.Vendor;
        //                    existingEntity.PO_Date = item.PO_Date;
        //                    existingEntity.PO_Qty = item.PO_Qty;
        //                    existingEntity.Balance_Qty = item.Balance_Qty;
        //                    existingEntity.Destination = item.Destination;
        //                    existingEntity.Delivery_Date = item.Delivery_Date;
        //                    existingEntity.Balance_Value = item.Balance_Value;
        //                    existingEntity.Material = item.Material;
        //                    existingEntity.Hold_Date = item.Hold_Date;
        //                    existingEntity.Cleared_Date = item.Cleared_Date;
        //                    existingEntity.CreatedBy = uploadedBy;
        //                    existingEntity.CreatedDate = DateTime.Now;
        //                    existingEntity.LastUploadBatchId = batchId;
        //                    existingEntity.LastUploadedAt = uploadTs;
        //                    existingEntity.SuppressChildDelivery = true;
        //                }
        //                else
        //                {
        //                    result.FailedRecords.Add((item, "Duplicate in database with no changes"));
        //                }

        //                existingEntity.LastUploadBatchId = batchId;
        //                existingEntity.LastUploadedAt = uploadTs;
        //                existingEntity.SuppressChildDelivery = true;
        //            }
        //            else
        //            {
        //                // Insert new record
        //                var newEntity = new Open_Po
        //                {
        //                    Key = item.Key,
        //                    PR_Type = item.PR_Type,
        //                    PR_Desc = item.PR_Desc,
        //                    Requisitioner = item.Requisitioner,
        //                    Tracking_No = item.Tracking_No,
        //                    PR_No = item.PR_No,
        //                    Batch_No = item.Batch_No,
        //                    Reference_No = item.Reference_No,
        //                    Vendor = item.Vendor,
        //                    PO_No = item.PO_No,
        //                    PO_Date = item.PO_Date,
        //                    PO_Qty = item.PO_Qty,
        //                    Balance_Qty = item.Balance_Qty,
        //                    Destination = item.Destination,
        //                    Delivery_Date = item.Delivery_Date,
        //                    Balance_Value = item.Balance_Value,
        //                    Material = item.Material,
        //                    Hold_Date = item.Hold_Date,
        //                    Cleared_Date = item.Cleared_Date,
        //                    Key1 = (item.Batch_No ?? string.Empty) + (item.Reference_No ?? string.Empty),
        //                    CreatedBy = uploadedBy,
        //                    CreatedDate = DateTime.Now,
        //                    LastUploadBatchId = batchId,
        //                    LastUploadedAt = uploadTs,
        //                    SuppressChildDelivery = true
        //                };
        //                _dbContext.OpenPo.Add(newEntity);
        //            }
        //        }

        //        await _dbContext.SaveChangesAsync();

        //        // Log the upload
        //        var importLog = new Open_Po_Log
        //        {
        //            FileName = fileName,
        //            TotalRecords = listOfData.Count,
        //            ImportedRecords = listOfData.Count - result.FailedRecords.Count,
        //            FailedRecords = result.FailedRecords.Count,
        //            UploadedBy = uploadedBy,
        //            UploadedAt = uploadTs,
        //            FileType = "PO"
        //        };

        //        _dbContext.OpenPo_Log.Add(importLog);
        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        result.Result = new OperationResult
        //        {
        //            Success = true,
        //            Message = result.FailedRecords.Any()
        //                ? "Import completed with some skipped records."
        //                : "All records imported successfully."
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        result.Result = new OperationResult
        //        {
        //            Success = false,
        //            Message = "Error during import: " + ex.Message
        //        };
        //    }

        //    return result;
        //}


        public async Task<BulkCreateLogResult> BulkCreateAsync_Dapper(IList<Open_PoViewModel> listOfData, string fileName, string uploadedBy)
        {
            var result = new BulkCreateLogResult();
            var batchId = Guid.NewGuid();
            var uploadTs = DateTime.Now;

            // Build TVP DataTable
            var tvp = new DataTable();
            tvp.Columns.AddRange(new[]
            {
                new DataColumn("Id", typeof(int)),
                new DataColumn("Deleted", typeof(bool)),
                new DataColumn("Key", typeof(string)),
                new DataColumn("PR_Type", typeof(string)),
                new DataColumn("PR_Desc", typeof(string)),
                new DataColumn("Requisitioner", typeof(string)),
                new DataColumn("Tracking_No", typeof(string)),
                new DataColumn("PR_No", typeof(string)),
                new DataColumn("Batch_No", typeof(string)),
                new DataColumn("Reference_No", typeof(string)),
                new DataColumn("Vendor", typeof(string)),
                new DataColumn("PO_No", typeof(string)),
                new DataColumn("PO_Date", typeof(DateTime)),
                new DataColumn("PO_Qty", typeof(int)),
                new DataColumn("Balance_Qty", typeof(int)),
                new DataColumn("Destination", typeof(string)),
                new DataColumn("Delivery_Date", typeof(DateTime)),
                new DataColumn("Balance_Value", typeof(decimal)),
                new DataColumn("Material", typeof(string)),
                new DataColumn("Hold_Date", typeof(DateTime)),
                new DataColumn("Cleared_Date", typeof(DateTime)),

                new DataColumn("Key1", typeof(string)),
                new DataColumn("Comit_Date", typeof(DateTime)),
                new DataColumn("Comit_Qty", typeof(int)),
                new DataColumn("Comit_Planner_Qty", typeof(int)),
                new DataColumn("Comit_Planner_date", typeof(DateTime)),
                new DataColumn("PCWeekDate", typeof(string)),
                new DataColumn("Comit_Vendor_Date", typeof(DateTime)),
                new DataColumn("Comit_Vendor_Qty", typeof(int)),
                new DataColumn("Comit_Planner_Remark", typeof(string)),
                new DataColumn("Comit_Date1", typeof(DateTime)),
                new DataColumn("Comit_Qty1", typeof(int)),
                new DataColumn("Comit_Final_Date", typeof(DateTime)),
                new DataColumn("Comit_Final_Qty", typeof(int)),

                new DataColumn("Buffer_Day", typeof(int)),
                new DataColumn("CreatedDate", typeof(DateTime)),
                new DataColumn("CreatedBy", typeof(string)),
                new DataColumn("UpdatedDate", typeof(DateTime)),
                new DataColumn("UpdatedBy", typeof(string)),
                new DataColumn("LastUploadedAt", typeof(DateTime)),
                new DataColumn("LastUploadBatchId", typeof(Guid)),
                new DataColumn("SuppressChildDelivery", typeof(bool)),
                new DataColumn("TOC_BRYG_Color", typeof(string)),
                new DataColumn("Vendor_Dispatch_Date", typeof(string)),
            });

            foreach (var x in listOfData)
            {
                // Normalize to match the SP’s behavior (trim on client side too)
                string trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

                tvp.Rows.Add(
                    x.Id,
                    x.Deleted,
                    trim(x.Key),
                    trim(x.PR_Type),
                    trim(x.PR_Desc),
                    trim(x.Requisitioner),
                    trim(x.Tracking_No),
                    trim(x.PR_No),
                    trim(x.Batch_No),
                    trim(x.Reference_No),
                    trim(x.Vendor),
                    trim(x.PO_No),
                    x.PO_Date.HasValue ? x.PO_Date.Value : (object)DBNull.Value,
                    x.PO_Qty.HasValue ? x.PO_Qty.Value : (object)DBNull.Value,
                    x.Balance_Qty.HasValue ? x.Balance_Qty.Value : (object)DBNull.Value,
                    trim(x.Destination),
                    x.Delivery_Date.HasValue ? x.Delivery_Date.Value : (object)DBNull.Value,
                    x.Balance_Value.HasValue ? x.Balance_Value.Value : (object)DBNull.Value,
                    trim(x.Material),
                    x.Hold_Date.HasValue ? x.Hold_Date.Value : (object)DBNull.Value,
                    x.Cleared_Date.HasValue ? x.Cleared_Date.Value : (object)DBNull.Value,

                    trim(x.Key1),
                    x.Comit_Date.HasValue ? x.Comit_Date.Value : (object)DBNull.Value,
                    x.Comit_Qty.HasValue ? x.Comit_Qty.Value : (object)DBNull.Value,
                    x.Comit_Planner_Qty.HasValue ? x.Comit_Planner_Qty.Value : (object)DBNull.Value,
                    x.Comit_Planner_date.HasValue ? x.Comit_Planner_date.Value : (object)DBNull.Value,
                    trim(x.PCWeekDate),
                    x.Comit_Vendor_Date.HasValue ? x.Comit_Vendor_Date.Value : (object)DBNull.Value,
                    x.Comit_Vendor_Qty.HasValue ? x.Comit_Vendor_Qty.Value : (object)DBNull.Value,
                    trim(x.Comit_Planner_Remark),
                    x.Comit_Date1.HasValue ? x.Comit_Date1.Value : (object)DBNull.Value,
                    x.Comit_Qty1.HasValue ? x.Comit_Qty1.Value : (object)DBNull.Value,
                    x.Comit_Final_Date.HasValue ? x.Comit_Final_Date.Value : (object)DBNull.Value,
                    x.Comit_Final_Qty.HasValue ? x.Comit_Final_Qty.Value : (object)DBNull.Value,

                    x.Buffer_Day.HasValue ? x.Buffer_Day.Value : (object)DBNull.Value,
                    x.CreatedDate.HasValue ? x.CreatedDate.Value : (object)DBNull.Value,
                    trim(x.CreatedBy) ?? uploadedBy,
                    uploadTs, // UpdatedDate
                    uploadedBy, // UpdatedBy
                    uploadTs,
                    batchId,
                    true,
                    trim(x.TOC_BRYG_Color),
                    x.Vendor_Dispatch_Date.HasValue ? x.Vendor_Dispatch_Date.Value : (object)DBNull.Value
                );
            }

            try
            {
                //var connectionString = _dbContext.Database.GetDbConnection();
                using var conn = _dbContext.Database.GetDbConnection();
                await conn.OpenAsync();

                var dp = new DynamicParameters();
                dp.Add("@Rows", tvp.AsTableValuedParameter("dbo.OpenPo_ImportRowBulk"));
                dp.Add("@FileName", fileName, DbType.String, size: 260);
                dp.Add("@UploadedBy", uploadedBy, DbType.String, size: 100);
                dp.Add("@BatchId", batchId, DbType.Guid);
                dp.Add("@UploadTs", uploadTs, DbType.DateTime2);
                dp.Add("@TOC_BRYG_Color", uploadTs, DbType.String);
                dp.Add("@Vendor_Dispatch_Date", uploadTs, DbType.DateTime);

                using var grid = await conn.QueryMultipleAsync(
                    sql: "dbo.sp_OpenPo_BulkUpsert",
                    param: dp,
                    commandType: CommandType.StoredProcedure);

                // 1) Summary
                var summary = (await grid.ReadAsync()).FirstOrDefault();
                int total = (int)summary?.TotalRecords;
                int imported = (int)summary?.ImportedRecords;
                int failed = (int)summary?.FailedRecords;

                // 2) Failed rows
                var failedRows = (await grid.ReadAsync<FailedRowDto>()).ToList();

                foreach (var f in failedRows)
                {
                    // Attempt to map back to original row by Key|PO_No
                    var row = listOfData.FirstOrDefault(r =>
                        string.Equals((r.Key ?? "").Trim(), (f.Key ?? "").Trim(), StringComparison.OrdinalIgnoreCase) &&
                        string.Equals((r.PO_No ?? "").Trim(), (f.PO_No ?? "").Trim(), StringComparison.OrdinalIgnoreCase));

                    result.FailedRecords.Add((row ?? new Open_PoViewModel { Key = f.Key, PO_No = f.PO_No }, f.Reason));
                }

                result.Result = new OperationResult
                {
                    Success = true,
                    Message = failedRows.Any()
                        ? $"Import completed: {imported}/{total} applied, {failed} failed."
                        : $"All {imported}/{total} records imported successfully."
                };
            }
            catch (Exception ex)
            {
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

            var deliveries = await _dbContext.Opne_Po_Deliveries.Where(x => x.Ven_PoId == poId && x.Deleted == false).ToListAsync();

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
                Key = header.Key,
                Key1 = header.Key1,
                DeliveryScheduleList = deliveries.Select(d => new DeliveryScheduleItem
                {
                    Delivery_Date = d.Delivery_Date,
                    Delivery_Qty = d.Delivery_Qty,
                    Delivery_Remark = d.Delivery_Remark,
                    Date_PC_Week = d.Date_PC_Week
                }).Where(x => x.Delivery_Date != null).ToList()
            };

            return viewModel;
        }

        //public async Task SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel model, string updatedBy)
        //{
        //    // First delete existing records
        //    var existing = await _dbContext.Opne_Po_Deliveries.Where(x => x.Ven_PoId == model.Ven_PoId).ToListAsync();
        //    _dbContext.Opne_Po_Deliveries.RemoveRange(existing);

        //    // Insert new records
        //    foreach (var item in model.DeliveryScheduleList)
        //    {
        //        var entity = new Opne_Po_DeliverySchedule
        //        {
        //            Ven_PoId = model.Ven_PoId,
        //            Vendor = model.Vendor,
        //            PO_No = model.PO_No,
        //            PO_Date = model.PO_Date,
        //            PO_Qty = model.PO_Qty,
        //            Balance_Qty = model.Balance_Qty,
        //            Delivery_Date = item.Delivery_Date,
        //            Delivery_Qty = item.Delivery_Qty,
        //            Delivery_Remark = item.Delivery_Remark,
        //            Date_PC_Week = item.Date_PC_Week,
        //            Key = model.Key,
        //            Key1 = model.Key1,
        //            CreatedBy = updatedBy,
        //            CreatedDate = DateTime.Now
        //        };

        //        _dbContext.Opne_Po_Deliveries.Add(entity);
        //    }

        //    await _dbContext.SaveChangesAsync();
        //}


        /// working
        //public async Task SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel model, string updatedBy)
        //{
        //    // Guard
        //    if (model == null) throw new ArgumentNullException(nameof(model));
        //    if (model.DeliveryScheduleList == null) model.DeliveryScheduleList = new List<DeliveryScheduleItem>();

        //    using var tx = await _dbContext.Database.BeginTransactionAsync();

        //    bool anyRowSaved = false;

        //    foreach (var item in model.DeliveryScheduleList)
        //    {
        //        // Find an existing row for this Ven_PoId + Key + Key1 (tweak if your uniqueness differs)
        //        var entity = await _dbContext.Opne_Po_Deliveries
        //            .FirstOrDefaultAsync(x =>
        //                x.Ven_PoId == model.Ven_PoId &&
        //                x.Key == model.Key &&
        //                x.Key1 == model.Key1);

        //        if (entity == null)
        //        {
        //            // Create the row and set the first commit
        //            entity = new Opne_Po_DeliverySchedule
        //            {
        //                Ven_PoId = model.Ven_PoId,
        //                Vendor = model.Vendor,
        //                PO_No = model.PO_No,
        //                PO_Date = model.PO_Date,
        //                PO_Qty = model.PO_Qty,
        //                Balance_Qty = model.Balance_Qty,

        //                // Optional: mirror the "incoming" values into Delivery_* if you still want to store them
        //                Delivery_Date = item.Delivery_Date,
        //                Delivery_Qty = item.Delivery_Qty,
        //                Delivery_Remark = item.Delivery_Remark,

        //                Date_PC_Week = item.Date_PC_Week,
        //                Key = model.Key,
        //                Key1 = model.Key1,

        //                // First commit lands in Comit_*
        //                Comit_Date = item.Delivery_Date,
        //                Comit_Qty = item.Delivery_Qty,

        //                CreatedBy = updatedBy,
        //                CreatedDate = DateTime.Now
        //            };

        //            _dbContext.Opne_Po_Deliveries.Add(entity);

        //            anyRowSaved = true;
        //        }
        //        else
        //        {
        //            // Update common PO/meta fields (if they can change)
        //            entity.Vendor = model.Vendor;
        //            entity.PO_No = model.PO_No;
        //            entity.PO_Date = model.PO_Date;
        //            entity.PO_Qty = model.PO_Qty;
        //            entity.Balance_Qty = model.Balance_Qty;

        //            // Optional: keep Delivery_* as the "incoming" values for reference
        //            entity.Delivery_Date = item.Delivery_Date;
        //            entity.Delivery_Qty = item.Delivery_Qty;
        //            entity.Delivery_Remark = item.Delivery_Remark;
        //            entity.Date_PC_Week = item.Date_PC_Week;

        //            // Apply the rotation with the new incoming values
        //            ApplyCommitRotation(entity, item.Delivery_Date, item.Delivery_Qty);
        //            anyRowSaved = true;
        //        }
        //    }

        //    await _dbContext.SaveChangesAsync();

        //    if (anyRowSaved)
        //    {
        //        var parent = await _dbContext.OpenPo.FirstOrDefaultAsync(p => p.Id == model.Ven_PoId);
        //        if (parent != null && parent.SuppressChildDelivery)
        //        {
        //            parent.SuppressChildDelivery = false;
        //            parent.UpdatedBy = updatedBy;
        //            parent.UpdatedDate = DateTime.Now;
        //            await _dbContext.SaveChangesAsync();
        //        }
        //    }
        //    await tx.CommitAsync();
        //}

        /////// <summary>
        /////// Rotates Comit_* fields with a 3-slot queue:
        /////// Final <- Comit_Date1 <- Comit_Date <- (newDate,newQty)
        /////// Handles empty/first/second/third+ updates safely.
        /////// </summary>
        ////private static void ApplyCommitRotation(Opne_Po_DeliverySchedule e, DateTime? newDate, int? newQty)
        ////{
        ////    // If nothing new, no-op
        ////    bool hasNew = newDate.HasValue || (newQty.HasValue && newQty.Value != 0);
        ////    if (!hasNew) return;

        ////    // If nothing ever set, first fill Comit_*
        ////    if (!e.Comit_Date.HasValue && !e.Comit_Qty.HasValue)
        ////    {
        ////        e.Comit_Date = newDate;
        ////        e.Comit_Qty = newQty;
        ////        return;
        ////    }

        ////    // If the new value equals the current Comit_*, skip (prevents duplicate churn)
        ////    bool sameAsCurrent =
        ////        Nullable.Equals(e.Comit_Date, newDate) &&
        ////        ((e.Comit_Qty ?? 0) == (newQty ?? 0));

        ////    if (sameAsCurrent) return;

        ////    // General rotation:
        ////    // Move Comit_Date1/Qty1 -> Final
        ////    e.Comit_Final_Date = e.Comit_Date1;
        ////    e.Comit_Final_Qty = e.Comit_Qty1;

        ////    // Move Comit_Date/Qty -> Comit_Date1/Qty1
        ////    e.Comit_Date1 = e.Comit_Date;
        ////    e.Comit_Qty1 = e.Comit_Qty;

        ////    // New -> Comit_Date/Qty
        ////    e.Comit_Date = newDate;
        ////    e.Comit_Qty = newQty;
        ////}

        ///// <summary>
        ///// Rotation rules requested:
        ///// 1) 1st commit → Comit_Date/Qty
        ///// 2) 2nd commit → move Comit_Date → Comit_Date1; new → Comit_Date
        ///// 3) 3rd commit → Comit_Date1 → Comit_Final_Date; Comit_Date → Comit_Date1; new → Comit_Date
        ///// 4th+ commits → DO NOT change Comit_Date1 or Comit_Final_Date; only set Comit_Date to the new value
        ///// </summary>
        //private static void ApplyCommitRotation(Opne_Po_DeliverySchedule e, DateTime? newDate, int? newQty)
        //{
        //    // nothing to do if no incoming data
        //    bool hasNew = newDate.HasValue || (newQty.HasValue && newQty.Value != 0);
        //    if (!hasNew) return;

        //    // If nothing ever set -> first fill current
        //    if (!e.Comit_Date.HasValue && !e.Comit_Qty.HasValue)
        //    {
        //        e.Comit_Date = newDate;
        //        e.Comit_Qty = newQty;
        //        return;
        //    }

        //    // If the new value is identical to current, skip (idempotent)
        //    bool sameAsCurrent =
        //        Nullable.Equals(e.Comit_Date, newDate) &&
        //        ((e.Comit_Qty ?? 0m) == (newQty ?? 0m));
        //    if (sameAsCurrent) return;

        //    // If we haven't yet filled Comit_Date1 (2nd commit)
        //    if (!e.Comit_Date1.HasValue && !e.Comit_Qty1.HasValue)
        //    {
        //        e.Comit_Date1 = e.Comit_Date;
        //        e.Comit_Qty1 = e.Comit_Qty;

        //        e.Comit_Date = newDate;
        //        e.Comit_Qty = newQty;
        //        return;
        //    }

        //    // If we haven't yet filled Comit_Final (3rd commit)
        //    if (!e.Comit_Final_Date.HasValue && !e.Comit_Final_Qty.HasValue)
        //    {
        //        e.Comit_Final_Date = e.Comit_Date1;
        //        e.Comit_Final_Qty = e.Comit_Qty1;

        //        e.Comit_Date1 = e.Comit_Date;
        //        e.Comit_Qty1 = e.Comit_Qty;

        //        e.Comit_Date = newDate;
        //        e.Comit_Qty = newQty;
        //        return;
        //    }

        //    // 4th+ commits: freeze Comit_Date1 & Comit_Final_*; only update current
        //    e.Comit_Date = newDate;
        //    e.Comit_Qty = newQty;
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////

        // latest working
        //public async Task SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel model, string updatedBy)
        //{
        //    if (model == null) throw new ArgumentNullException(nameof(model));
        //    if (model.DeliveryScheduleList == null) model.DeliveryScheduleList = new List<DeliveryScheduleItem>();

        //    using var tx = await _dbContext.Database.BeginTransactionAsync();

        //    // 1) Load all rows for this key into memory ONCE
        //    var rows = await _dbContext.Opne_Po_Deliveries
        //        .Where(x => x.Ven_PoId == model.Ven_PoId && x.Key == model.Key && x.Key1 == model.Key1)
        //        .ToListAsync(); // IQueryable -> List (in-memory)

        //    // 2) Partition IN MEMORY (LINQ-to-Objects)
        //    var finalRows = rows.Where(IsFinalRow).ToList();
        //    var comit1Rows = rows.Where(IsComit1Row).ToList();
        //    var comitRows = rows.Where(IsComitRow).ToList();
        //    var deliveryRows = rows.Where(IsDeliveryRow).ToList();

        //    bool hasFinal = finalRows.Count > 0;

        //    // === ROTATION ===
        //    // Promote Comit1 -> Final (only if Final empty)
        //    if (!hasFinal && comit1Rows.Count > 0)
        //    {
        //        foreach (var r in comit1Rows)
        //        {
        //            var clone = CloneMeta(r);
        //            clone.Comit_Final_Date = r.Comit_Date1;
        //            clone.Comit_Final_Qty = r.Comit_Qty1;
        //            _dbContext.Opne_Po_Deliveries.Add(clone);
        //            _dbContext.Opne_Po_Deliveries.Remove(r);
        //        }
        //        await _dbContext.SaveChangesAsync();

        //        // refresh list in-memory after changes
        //        rows = await _dbContext.Opne_Po_Deliveries
        //            .Where(x => x.Ven_PoId == model.Ven_PoId && x.Key == model.Key && x.Key1 == model.Key1)
        //            .ToListAsync();

        //        finalRows = rows.Where(IsFinalRow).ToList();
        //        comit1Rows = rows.Where(IsComit1Row).ToList();
        //        comitRows = rows.Where(IsComitRow).ToList();
        //        deliveryRows = rows.Where(IsDeliveryRow).ToList();
        //        hasFinal = finalRows.Count > 0;
        //    }

        //    // Comit -> Comit1
        //    if (comitRows.Count > 0)
        //    {
        //        foreach (var r in comitRows)
        //        {
        //            var clone = CloneMeta(r);
        //            clone.Comit_Date1 = r.Comit_Date;
        //            clone.Comit_Qty1 = r.Comit_Qty;
        //            _dbContext.Opne_Po_Deliveries.Add(clone);
        //            _dbContext.Opne_Po_Deliveries.Remove(r);
        //        }
        //        await _dbContext.SaveChangesAsync();

        //        rows = await _dbContext.Opne_Po_Deliveries
        //            .Where(x => x.Ven_PoId == model.Ven_PoId && x.Key == model.Key && x.Key1 == model.Key1)
        //            .ToListAsync();

        //        finalRows = rows.Where(IsFinalRow).ToList();
        //        comit1Rows = rows.Where(IsComit1Row).ToList();
        //        comitRows = rows.Where(IsComitRow).ToList();
        //        deliveryRows = rows.Where(IsDeliveryRow).ToList();
        //    }

        //    // Delivery -> Comit
        //    if (deliveryRows.Count > 0)
        //    {
        //        foreach (var r in deliveryRows)
        //        {
        //            var clone = CloneMeta(r);
        //            clone.Comit_Date = r.Delivery_Date;
        //            clone.Comit_Qty = r.Delivery_Qty;
        //            _dbContext.Opne_Po_Deliveries.Add(clone);
        //            _dbContext.Opne_Po_Deliveries.Remove(r);
        //        }
        //        await _dbContext.SaveChangesAsync();
        //    }

        //    // Insert NEW Delivery rows (the incoming batch)
        //    foreach (var item in model.DeliveryScheduleList)
        //    {
        //        if (!item.Delivery_Date.HasValue && (!item.Delivery_Qty.HasValue || item.Delivery_Qty == 0))
        //            continue;

        //        _dbContext.Opne_Po_Deliveries.Add(new Opne_Po_DeliverySchedule
        //        {
        //            Ven_PoId = model.Ven_PoId,
        //            Vendor = model.Vendor,
        //            PO_No = model.PO_No,
        //            PO_Date = model.PO_Date,
        //            PO_Qty = model.PO_Qty,
        //            Balance_Qty = model.Balance_Qty,
        //            Key = model.Key,
        //            Key1 = model.Key1,
        //            Delivery_Date = item.Delivery_Date,
        //            Delivery_Qty = item.Delivery_Qty,
        //            Delivery_Remark = item.Delivery_Remark,
        //            Date_PC_Week = item.Date_PC_Week,
        //            CreatedBy = updatedBy,
        //            CreatedDate = DateTime.Now
        //        });
        //    }

        //    await _dbContext.SaveChangesAsync();

        //    // parent flag logic (unchanged)
        //    var parent = await _dbContext.OpenPo.FirstOrDefaultAsync(p => p.Id == model.Ven_PoId);
        //    if (parent != null && parent.SuppressChildDelivery)
        //    {
        //        parent.SuppressChildDelivery = false;
        //        parent.UpdatedBy = updatedBy;
        //        parent.UpdatedDate = DateTime.Now;
        //        await _dbContext.SaveChangesAsync();
        //    }

        //    await tx.CommitAsync();
        //}

        //// Helpers (unchanged)
        //private static bool IsFinalRow(Opne_Po_DeliverySchedule r)
        //    => r.Comit_Final_Date.HasValue || (r.Comit_Final_Qty.HasValue && r.Comit_Final_Qty.Value != 0);

        //private static bool IsComit1Row(Opne_Po_DeliverySchedule r)
        //    => (r.Comit_Date1.HasValue || (r.Comit_Qty1.HasValue && r.Comit_Qty1.Value != 0))
        //       && !IsFinalRow(r);

        //private static bool IsComitRow(Opne_Po_DeliverySchedule r)
        //    => (r.Comit_Date.HasValue || (r.Comit_Qty.HasValue && r.Comit_Qty.Value != 0))
        //       && !IsComit1Row(r) && !IsFinalRow(r);

        //private static bool IsDeliveryRow(Opne_Po_DeliverySchedule r)
        //    => (r.Delivery_Date.HasValue || (r.Delivery_Qty.HasValue && r.Delivery_Qty.Value != 0))
        //       && !IsComitRow(r) && !IsComit1Row(r) && !IsFinalRow(r);

        //private static Opne_Po_DeliverySchedule CloneMeta(Opne_Po_DeliverySchedule r)
        //{
        //    return new Opne_Po_DeliverySchedule
        //    {
        //        Ven_PoId = r.Ven_PoId,
        //        Vendor = r.Vendor,
        //        PO_No = r.PO_No,
        //        PO_Date = r.PO_Date,
        //        PO_Qty = r.PO_Qty,
        //        Balance_Qty = r.Balance_Qty,
        //        Key = r.Key,
        //        Key1 = r.Key1,
        //        CreatedBy = r.CreatedBy,
        //        CreatedDate = DateTime.Now,
        //    };
        //}


        ///////////////////////////////////////////////////////////////////////////////////////////////
        ///

        //public async Task SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel model, string updatedBy)
        //{
        //    if (model == null) throw new ArgumentNullException(nameof(model));
        //    model.DeliveryScheduleList ??= new List<DeliveryScheduleItem>();

        //    using var tx = await _dbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        bool anyRowSaved = false;

        //        // Normalize once for header keys (use item-level keys if you support them)
        //        string normKey = (model.Key ?? string.Empty).Trim().ToUpperInvariant();
        //        string normKey1 = (model.Key1 ?? string.Empty).Trim().ToUpperInvariant();

        //        foreach (var item in model.DeliveryScheduleList)
        //        {
        //            // Fetch active old rows for same group
        //            var oldRecords = await _dbContext.Opne_Po_Deliveries
        //                .Where(x =>
        //                    x.Ven_PoId == model.Ven_PoId &&
        //                    ((x.Key ?? "").Trim().ToUpper() == normKey) &&
        //                    ((x.Key1 ?? "").Trim().ToUpper() == normKey1) &&
        //                    (x.Deleted == false || x.Deleted == null))
        //                .ToListAsync();

        //            DateTime? prevComitDate = null;
        //            int? prevComitQty = null;

        //            if (oldRecords.Count > 0)
        //            {
        //                var latest = oldRecords
        //                    .OrderByDescending(x => x.CreatedDate)
        //                    .ThenByDescending(x => x.Id)
        //                    .First();

        //                // NOTE: single 't' column names
        //                prevComitDate = latest.Comit_Date;
        //                prevComitQty = latest.Comit_Qty;

        //                foreach (var old in oldRecords)
        //                {
        //                    old.Deleted = true;
        //                }
        //            }

        //            // Insert the new child row
        //            var entity = new Opne_Po_DeliverySchedule
        //            {
        //                Ven_PoId = model.Ven_PoId,
        //                Vendor = model.Vendor,
        //                PO_No = model.PO_No,
        //                PO_Date = model.PO_Date,
        //                PO_Qty = model.PO_Qty,
        //                Balance_Qty = model.Balance_Qty,
        //                Delivery_Date = item.Delivery_Date,
        //                Delivery_Qty = item.Delivery_Qty,
        //                Delivery_Remark = item.Delivery_Remark,
        //                Date_PC_Week = item.Date_PC_Week,
        //                Key = model.Key?.Trim(),
        //                Key1 = model.Key1?.Trim(),
        //                // inherit previous commit data
        //                Comit_Date = prevComitDate,
        //                Comit_Qty = prevComitQty,
        //                Deleted = false,
        //                CreatedBy = updatedBy,
        //                CreatedDate = DateTime.UtcNow
        //            };

        //            _dbContext.Opne_Po_Deliveries.Add(entity);
        //            anyRowSaved = true;
        //        }

        //        // Flip parent.SuppressChildDelivery only if we actually saved at least one new row
        //        if (anyRowSaved)
        //        {
        //            var parent = await _dbContext.OpenPo.FirstOrDefaultAsync(p => p.Id == model.Ven_PoId);
        //            if (parent != null && parent.SuppressChildDelivery)
        //            {
        //                parent.SuppressChildDelivery = false;
        //                parent.UpdatedBy = updatedBy;
        //                parent.UpdatedDate = DateTime.UtcNow;
        //            }
        //        }

        //        // Single save for everything
        //        await _dbContext.SaveChangesAsync();
        //        await tx.CommitAsync();
        //    }
        //    catch
        //    {
        //        await tx.RollbackAsync();
        //        throw;
        //    }
        //}



        public async Task<int> SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel model, string updatedBy)
        {
            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                model.DeliveryScheduleList ??= new List<DeliveryScheduleItem>();

                // Build DataTable for TVP
                var tvp = new DataTable();
                tvp.Columns.Add("Delivery_Date", typeof(DateTime));
                tvp.Columns.Add("Delivery_Qty", typeof(int));
                tvp.Columns.Add("Delivery_Remark", typeof(string));
                tvp.Columns.Add("Date_PC_Week", typeof(string));

                foreach (var item in model.DeliveryScheduleList)
                {
                    var row = tvp.NewRow();
                    row["Delivery_Date"] = (object?)item.Delivery_Date ?? DBNull.Value;
                    row["Delivery_Qty"] = (object?)item.Delivery_Qty ?? DBNull.Value;
                    row["Delivery_Remark"] = (object?)item.Delivery_Remark ?? DBNull.Value;
                    row["Date_PC_Week"] = (object?)item.Date_PC_Week ?? DBNull.Value;
                    tvp.Rows.Add(row);
                }

                //using var conn = new SqlConnection(connectionString)
                using var conn = _dbContext.Database.GetDbConnection();
                await conn.OpenAsync();

                var dp = new DynamicParameters();
                dp.Add("@Ven_PoId", model.Ven_PoId, DbType.Int32);
                dp.Add("@Vendor", model.Vendor, DbType.String);
                dp.Add("@PO_No", model.PO_No, DbType.String);
                dp.Add("@PO_Date", model.PO_Date, DbType.Date);
                dp.Add("@PO_Qty", model.PO_Qty, DbType.Int32);
                dp.Add("@Balance_Qty", model.Balance_Qty, DbType.Int32);
                dp.Add("@Key", model.Key, DbType.String);
                dp.Add("@Key1", model.Key1, DbType.String);
                dp.Add("@UpdatedBy", updatedBy, DbType.String);

                // Table-Valued Parameter
                dp.Add("@Items", tvp.AsTableValuedParameter("dbo.DeliveryScheduleItemType"));

                // Output
                dp.Add("@RowsInserted", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    sql: "dbo.sp_SavePO_DeliverySchedule",
                    param: dp,
                    commandType: CommandType.StoredProcedure
                );

                return dp.Get<int>("@RowsInserted");
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }


        public async Task<(List<Open_Po> poHeaders, List<Opne_Po_DeliverySchedule> deliverySchedules)> GetOpenPOWithDeliveryScheduleAsync(string vendor)
        {
            try
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
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<(List<Open_Po> poHeaders, List<Opne_Po_DeliverySchedule> deliverySchedules)> GetOpenPOWithDeliveryScheduleVendorAsync(string vendor)
        {
            try
            {
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync("[dbo].[sp_Get_OpenPO_With_DeliverySchedule_ByVendor]", new { Vendor = vendor }, commandType: CommandType.StoredProcedure))

                    {
                        var poHeaders = (await multi.ReadAsync<Open_Po>()).ToList();
                        var deliverySchedules = (await multi.ReadAsync<Opne_Po_DeliverySchedule>()).ToList();

                        return (poHeaders, deliverySchedules);
                    }
                }
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> SaveBuffScheduleAsync(int id, int buff, string updatedBy, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@DeliverySchu_Id", id),
                    new SqlParameter("@Ven_PoId", ""),
                    new SqlParameter("@Buffer_Day", buff),
                };

                var sql = @"EXEC sp_Update_OpenPo_Buffer @DeliverySchu_Id,@Ven_PoId,@Buffer_Day";

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

        public async Task<List<Sales_Order_ViewModel>> GetSalesOrderListAsync(string? type)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@type", type ?? (object)DBNull.Value),
                };

                var result = await _dbContext.Sales_Order
                    .FromSqlRaw("EXEC sp_Get_All_SalesOrder @type", parameters)
                    .ToListAsync();


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
                    CreatedBy = data.CreatedBy,
                    Key = data.Key,
                    Key1 = data.Key1,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate
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

        //public async Task<(List<MatchedRecordViewModel> matched, MatchSummaryViewModel? summary)> GetPO_SO_MatchReportAsync(string? type)
        //{
        //    var connectionString = _dbContext.Database.GetConnectionString();

        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();

        //        using (var multi = await connection.QueryMultipleAsync("sp_PO_SO_Match_Report", new { Type = type }, commandType: CommandType.StoredProcedure))
        //        {
        //            var matched = (await multi.ReadAsync<MatchedRecordViewModel>()).ToList();
        //            var summary = (await multi.ReadAsync<MatchSummaryViewModel>()).FirstOrDefault();

        //            return (matched, summary);
        //        }
        //    }
        //}

        public async Task<(List<MatchedRecordViewModel> matched, List<MatchDeliverySchViewModel> deliverySch, MatchSummaryViewModel? summary)> GetPO_SO_MatchReportAsync(string? type)
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync("sp_PO_SO_Match_Report_1", new { Type = type }, commandType: CommandType.StoredProcedure))
                {
                    var matched = (await multi.ReadAsync<MatchedRecordViewModel>()).ToList();
                    var deliverySch = (await multi.ReadAsync<MatchDeliverySchViewModel>()).ToList();
                    var summary = (await multi.ReadAsync<MatchSummaryViewModel>()).FirstOrDefault();

                    return (matched, deliverySch, summary);
                }
            }
        }

        //public async Task<BulkSalesCreateLogResult> BulkSalesCreateAsync(List<Sales_Order_ViewModel> listOfData, string fileName, string uploadedBy)
        //{
        //    var result = new BulkSalesCreateLogResult();
        //    using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var seenKeys = new HashSet<string>();

        //        var matsNeedingDerive = listOfData
        //   .Where(x => string.IsNullOrWhiteSpace(x.Item_Category))
        //   .Select(x => (x.Material ?? string.Empty).Trim())
        //   .Where(m => !string.IsNullOrEmpty(m))
        //   .Distinct(StringComparer.OrdinalIgnoreCase)
        //   .ToList();

        //        var mtaMaterials = new HashSet<string>(await _dbContext.MTAMaster.Where(m => !m.Deleted && matsNeedingDerive.Contains(m.Material_No)).Select(m => m.Material_No).ToListAsync(), StringComparer.OrdinalIgnoreCase);

        //        foreach (var item in listOfData)
        //        {
        //            item.Indent_No = item.Indent_No?.Trim();
        //            item.Old_Material_No = item.Old_Material_No?.Trim();
        //            item.Material = item.Material?.Trim();   // ensure trimmed
        //            item.Item_Category = item.Item_Category?.Trim();

        //            if (string.IsNullOrWhiteSpace(item.Item_Category))
        //            {
        //                if (!string.IsNullOrEmpty(item.Material) && mtaMaterials.Contains(item.Material))
        //                    item.Item_Category = "MTA";
        //                else
        //                    item.Item_Category = "MTO";
        //            }

        //            var compositeKey = $"{item.Indent_No}|{item.Old_Material_No}";

        //            if (string.IsNullOrWhiteSpace(item.Indent_No) || string.IsNullOrWhiteSpace(item.Old_Material_No))
        //            {
        //                result.FailedRecords.Add((item, "Missing Indent_No or Old_Material_No"));
        //                continue;
        //            }

        //            if (seenKeys.Contains(compositeKey))
        //            {
        //                result.FailedRecords.Add((item, "Duplicate in uploaded file"));
        //                continue;
        //            }

        //            seenKeys.Add(compositeKey);

        //            var existingEntity = await _dbContext.Sales_Order.FirstOrDefaultAsync(x => x.Indent_No == item.Indent_No && x.Old_Material_No == item.Old_Material_No);

        //            if (existingEntity != null)
        //            {
        //                bool isDifferent =
        //                    existingEntity.SO_No != item.SO_No ||
        //                    existingEntity.SaleOrder_Type != item.SaleOrder_Type ||
        //                    existingEntity.SO_Date != item.SO_Date ||
        //                    existingEntity.Line_Item != item.Line_Item ||
        //                    existingEntity.Indent_No != item.Indent_No ||
        //                    existingEntity.Indent_Date != item.Indent_Date ||
        //                    //existingEntity.Order_Type != item.Order_Type ||
        //                    //existingEntity.Vertical != item.Vertical ||
        //                    //existingEntity.Region != item.Region ||
        //                    existingEntity.Sales_Group != item.Sales_Group ||
        //                    existingEntity.Sales_Group_desc != item.Sales_Group_desc ||
        //                    existingEntity.Sales_Office != item.Sales_Office ||
        //                    existingEntity.Sales_Office_Desc != item.Sales_Office_Desc ||
        //                    existingEntity.Sale_Person != item.Sale_Person ||
        //                    existingEntity.Project_Name != item.Project_Name ||
        //                    //existingEntity.Project_Name_Tag != item.Project_Name_Tag ||
        //                    //existingEntity.Priority_Tag != item.Priority_Tag ||
        //                    existingEntity.Customer_Code != item.Customer_Code ||
        //                    existingEntity.Customer_Name != item.Customer_Name ||
        //                    //existingEntity.Dealer_Direct != item.Dealer_Direct ||
        //                    //existingEntity.Inspection != item.Inspection ||
        //                    existingEntity.Material != item.Material ||
        //                    existingEntity.Old_Material_No != item.Old_Material_No ||
        //                    existingEntity.Description != item.Description ||
        //                    existingEntity.SO_Qty != item.SO_Qty ||
        //                    existingEntity.SO_Value != item.SO_Value ||
        //                    //existingEntity.Rate != item.Rate ||
        //                    existingEntity.Del_Qty != item.Del_Qty ||
        //                    existingEntity.Open_Sale_Qty != item.Open_Sale_Qty ||
        //                    existingEntity.Opne_Sale_Value != item.Opne_Sale_Value ||
        //                    existingEntity.Plant != item.Plant ||
        //                    existingEntity.Item_Category != item.Item_Category ||
        //                    //existingEntity.Item_Category_Latest != item.Item_Category_Latest ||
        //                    existingEntity.Procurement_Type != item.Procurement_Type ||
        //                    existingEntity.Vendor_Po_No != item.Vendor_Po_No ||
        //                    existingEntity.Vendor_Po_Date != item.Vendor_Po_Date ||
        //                    //existingEntity.CPR_Number != item.CPR_Number ||
        //                    //existingEntity.Vendor != item.Vendor ||
        //                    //existingEntity.Planner != item.Planner ||
        //                    existingEntity.Po_Release_Qty != item.Po_Release_Qty ||
        //                    existingEntity.Allocated_Stock_Qty != item.Allocated_Stock_Qty ||
        //                    existingEntity.Allocated_Stock_Value != item.Allocated_Stock_Value ||
        //                    //existingEntity.Net_Qty != item.Net_Qty ||
        //                    //existingEntity.Net_Value != item.Net_Value ||
        //                    //existingEntity.Qty_In_Week != item.Qty_In_Week ||
        //                    //existingEntity.Value_In_Week != item.Value_In_Week ||
        //                    //existingEntity.Qty_After_Week != item.Qty_After_Week ||
        //                    //existingEntity.Value_After_Week != item.Value_After_Week ||
        //                    //existingEntity.Check5 != item.Check5 ||
        //                    existingEntity.Indent_Status != item.Indent_Status ||
        //                    //existingEntity.Sales_Call_Point != item.Sales_Call_Point ||
        //                    //existingEntity.Free_Stock != item.Free_Stock ||
        //                    //existingEntity.Grn_Qty != item.Grn_Qty ||
        //                    //existingEntity.Last_Grn_Date != item.Last_Grn_Date ||
        //                    //existingEntity.Check1 != item.Check1 ||
        //                    //existingEntity.Delivery_Schedule != item.Delivery_Schedule ||
        //                    //existingEntity.Readiness_Vendor_Released_Fr_Date != item.Readiness_Vendor_Released_Fr_Date ||
        //                    //existingEntity.Readiness_Vendor_Released_To_Date != item.Readiness_Vendor_Released_To_Date ||
        //                    //existingEntity.Readiness_Schedule_Vendor_Released != item.Readiness_Schedule_Vendor_Released ||
        //                    //existingEntity.Delivery_Schedule_PC_Breakup != item.Delivery_Schedule_PC_Breakup ||
        //                    //existingEntity.Check2 != item.Check2 ||
        //                    //existingEntity.Line_Item_Schedule != item.Line_Item_Schedule ||
        //                    //existingEntity.R_B != item.R_B ||
        //                    //existingEntity.Schedule_Repeat != item.Schedule_Repeat ||
        //                    //existingEntity.Internal_Pending_Issue != item.Internal_Pending_Issue ||
        //                    //existingEntity.Pending_With != item.Pending_With ||
        //                    //existingEntity.Remark != item.Remark ||
        //                    //existingEntity.CRD_OverDue != item.CRD_OverDue ||
        //                    existingEntity.Delivert_Date != item.Delivert_Date ||
        //                    //existingEntity.Process_Plan_On_Crd != item.Process_Plan_On_Crd ||
        //                    //existingEntity.Last_Week_PC != item.Last_Week_PC ||
        //                    existingEntity.Schedule_Line_Qty1 != item.Schedule_Line_Qty1 ||
        //                    existingEntity.Schedule_Line_Date1 != item.Schedule_Line_Date1 ||
        //                    existingEntity.Schedule_Line_Qty2 != item.Schedule_Line_Qty2 ||
        //                    existingEntity.Schedule_Line_Date2 != item.Schedule_Line_Date2 ||
        //                    existingEntity.Schedule_Line_Qty3 != item.Schedule_Line_Qty3 ||
        //                    existingEntity.Schedule_Line_Date3 != item.Schedule_Line_Date3;
        //                //existingEntity.To_Consider != item.To_Consider ||
        //                //existingEntity.Person_Name != item.Person_Name ||
        //                //existingEntity.Visibility != item.Visibility;

        //                if (isDifferent)
        //                {
        //                    existingEntity.SO_No = item.SO_No;
        //                    existingEntity.SaleOrder_Type = item.SaleOrder_Type;
        //                    existingEntity.SO_Date = item.SO_Date;
        //                    existingEntity.Line_Item = item.Line_Item;
        //                    existingEntity.Indent_No = item.Indent_No;
        //                    existingEntity.Indent_Date = item.Indent_Date;
        //                    //existingEntity.Order_Type = item.Order_Type;
        //                    //existingEntity.Vertical = item.Vertical;
        //                    //existingEntity.Region = item.Region;
        //                    existingEntity.Sales_Group = item.Sales_Group;
        //                    existingEntity.Sales_Group_desc = item.Sales_Group_desc;
        //                    existingEntity.Sales_Office = item.Sales_Office;
        //                    existingEntity.Sales_Office_Desc = item.Sales_Office_Desc;
        //                    existingEntity.Sale_Person = item.Sale_Person;
        //                    existingEntity.Project_Name = item.Project_Name;
        //                    //existingEntity.Project_Name_Tag = item.Project_Name_Tag;
        //                    //existingEntity.Priority_Tag = item.Priority_Tag;
        //                    existingEntity.Customer_Code = item.Customer_Code;
        //                    existingEntity.Customer_Name = item.Customer_Name;
        //                    //existingEntity.Dealer_Direct = item.Dealer_Direct;
        //                    //existingEntity.Inspection = item.Inspection;
        //                    existingEntity.Material = item.Material;
        //                    existingEntity.Old_Material_No = item.Old_Material_No;
        //                    existingEntity.Description = item.Description;
        //                    existingEntity.SO_Qty = item.SO_Qty;
        //                    existingEntity.SO_Value = item.SO_Value;
        //                    //existingEntity.Rate = item.Rate;
        //                    existingEntity.Del_Qty = item.Del_Qty;
        //                    existingEntity.Open_Sale_Qty = item.Open_Sale_Qty;
        //                    existingEntity.Opne_Sale_Value = item.Opne_Sale_Value;
        //                    existingEntity.Plant = item.Plant;
        //                    existingEntity.Item_Category = item.Item_Category;
        //                    //existingEntity.Item_Category_Latest = item.Item_Category_Latest;
        //                    existingEntity.Procurement_Type = item.Procurement_Type;
        //                    //existingEntity.Vendor_Po_No = item.Vendor_Po_No;
        //                    //existingEntity.Vendor_Po_Date = item.Vendor_Po_Date;
        //                    //existingEntity.CPR_Number = item.CPR_Number;
        //                    //existingEntity.Vendor = item.Vendor;
        //                    //existingEntity.Planner = item.Planner;
        //                    existingEntity.Po_Release_Qty = item.Po_Release_Qty;
        //                    existingEntity.Allocated_Stock_Qty = item.Allocated_Stock_Qty;
        //                    existingEntity.Allocated_Stock_Value = item.Allocated_Stock_Value;
        //                    //existingEntity.Net_Qty = item.Net_Qty;
        //                    //existingEntity.Net_Value = item.Net_Value;
        //                    //existingEntity.Qty_In_Week = item.Qty_In_Week;
        //                    //existingEntity.Value_In_Week = item.Value_In_Week;
        //                    //existingEntity.Qty_After_Week = item.Qty_After_Week;
        //                    //existingEntity.Value_After_Week = item.Value_After_Week;
        //                    //existingEntity.Check5 = item.Check5;
        //                    existingEntity.Indent_Status = item.Indent_Status;
        //                    //existingEntity.Sales_Call_Point = item.Sales_Call_Point;
        //                    //existingEntity.Free_Stock = item.Free_Stock;
        //                    //existingEntity.Grn_Qty = item.Grn_Qty;
        //                    //existingEntity.Last_Grn_Date = item.Last_Grn_Date;
        //                    //existingEntity.Check1 = item.Check1;
        //                    //existingEntity.Delivery_Schedule = item.Delivery_Schedule;
        //                    //existingEntity.Readiness_Vendor_Released_Fr_Date = item.Readiness_Vendor_Released_Fr_Date;
        //                    //existingEntity.Readiness_Vendor_Released_To_Date = item.Readiness_Vendor_Released_To_Date;
        //                    //existingEntity.Readiness_Schedule_Vendor_Released = item.Readiness_Schedule_Vendor_Released;
        //                    //existingEntity.Delivery_Schedule_PC_Breakup = item.Delivery_Schedule_PC_Breakup;
        //                    //existingEntity.Check2 = item.Check2;
        //                    //existingEntity.Line_Item_Schedule = item.Line_Item_Schedule;
        //                    //existingEntity.R_B = item.R_B;
        //                    //existingEntity.Schedule_Repeat = item.Schedule_Repeat;
        //                    //existingEntity.Internal_Pending_Issue = item.Internal_Pending_Issue;
        //                    //existingEntity.Pending_With = item.Pending_With;
        //                    //existingEntity.Remark = item.Remark;
        //                    //existingEntity.CRD_OverDue = item.CRD_OverDue;
        //                    existingEntity.Delivert_Date = item.Delivert_Date;
        //                    //existingEntity.Process_Plan_On_Crd = item.Process_Plan_On_Crd;
        //                    //existingEntity.Last_Week_PC = item.Last_Week_PC;
        //                    existingEntity.Schedule_Line_Qty1 = item.Schedule_Line_Qty1;
        //                    existingEntity.Schedule_Line_Date1 = item.Schedule_Line_Date1;
        //                    existingEntity.Schedule_Line_Qty2 = item.Schedule_Line_Qty2;
        //                    existingEntity.Schedule_Line_Date2 = item.Schedule_Line_Date2;
        //                    existingEntity.Schedule_Line_Qty3 = item.Schedule_Line_Qty3;
        //                    existingEntity.Schedule_Line_Date3 = item.Schedule_Line_Date3;
        //                    //existingEntity.To_Consider = item.To_Consider;
        //                    //existingEntity.Person_Name = item.Person_Name;
        //                    //existingEntity.Visibility = item.Visibility;
        //                    existingEntity.CreatedBy = uploadedBy;
        //                    existingEntity.CreatedDate = DateTime.Now;
        //                }
        //                else
        //                {
        //                    result.FailedRecords.Add((item, "Duplicate in database with no changes"));
        //                }
        //            }
        //            else
        //            {
        //                var netQty = item.Open_Sale_Qty - item.Allocated_Stock_Qty;
        //                var netValue = (item.SO_Qty > 0) ? (item.SO_Value / item.SO_Qty) * netQty : 0;

        //                string readinessMessage = null;

        //                // ✅ Condition 1: All qty allocated
        //                if (netValue == 0)
        //                {
        //                    readinessMessage = $"All qty allocated on {DateTime.Now:dd-MMM-yyyy}";
        //                }
        //                // ✅ Condition 2: When SO_Qty - Net_Qty == 0
        //                else if ((item.SO_Qty - netQty) == 0)
        //                {
        //                    readinessMessage = "Refer daily delivery visibility";
        //                }

        //                var newEntity = new Sales_Order_SCM
        //                {
        //                    SO_No = item.SO_No,
        //                    SaleOrder_Type = item.SaleOrder_Type,
        //                    SO_Date = item.SO_Date,
        //                    Line_Item = item.Line_Item,
        //                    Indent_No = item.Indent_No,
        //                    Indent_Date = item.Indent_Date,
        //                    //Order_Type = item.Order_Type,
        //                    //Vertical = item.Vertical,
        //                    //Region = item.Region,
        //                    Sales_Group = item.Sales_Group,
        //                    Sales_Group_desc = item.Sales_Group_desc,
        //                    Sales_Office = item.Sales_Office,
        //                    Sales_Office_Desc = item.Sales_Office_Desc,
        //                    Sale_Person = item.Sale_Person,
        //                    Project_Name = item.Project_Name,
        //                    //Project_Name_Tag = item.Project_Name_Tag,
        //                    //Priority_Tag = item.Priority_Tag,
        //                    Customer_Code = item.Customer_Code,
        //                    Customer_Name = item.Customer_Name,
        //                    //Dealer_Direct = item.Dealer_Direct,
        //                    //Inspection = item.Inspection,
        //                    Material = item.Material,
        //                    Old_Material_No = item.Old_Material_No,
        //                    Description = item.Description,
        //                    SO_Qty = item.SO_Qty,
        //                    SO_Value = item.SO_Value,
        //                    //Rate = item.Rate,
        //                    Del_Qty = item.Del_Qty,
        //                    Open_Sale_Qty = item.Open_Sale_Qty,
        //                    Opne_Sale_Value = item.Opne_Sale_Value,
        //                    Plant = item.Plant,
        //                    Item_Category = item.Item_Category,
        //                    //Item_Category_Latest = item.Item_Category_Latest,
        //                    Procurement_Type = item.Procurement_Type,
        //                    Vendor_Po_No = item.Vendor_Po_No,
        //                    Vendor_Po_Date = item.Vendor_Po_Date,
        //                    //CPR_Number = item.CPR_Number,
        //                    //Vendor = item.Vendor,
        //                    //Planner = item.Planner,
        //                    Po_Release_Qty = item.Po_Release_Qty,
        //                    Allocated_Stock_Qty = item.Allocated_Stock_Qty,
        //                    Allocated_Stock_Value = item.Allocated_Stock_Value,

        //                    Net_Qty = netQty,
        //                    Net_Value = netValue,

        //                    //Qty_In_Week = item.Qty_In_Week,
        //                    //Value_In_Week = item.Value_In_Week,
        //                    //Qty_After_Week = item.Qty_After_Week,
        //                    //Value_After_Week = item.Value_After_Week,
        //                    //Check5 = item.Check5,
        //                    Indent_Status = item.Indent_Status,
        //                    //Sales_Call_Point = item.Sales_Call_Point,
        //                    //Free_Stock = item.Free_Stock,
        //                    //Grn_Qty = item.Grn_Qty,
        //                    //Last_Grn_Date = item.Last_Grn_Date, 
        //                    //Check1 = item.Check1,
        //                    Delivery_Schedule = item.Delivery_Schedule,
        //                    //Readiness_Vendor_Released_Fr_Date = item.Readiness_Vendor_Released_Fr_Date,
        //                    //Readiness_Vendor_Released_To_Date = item.Readiness_Vendor_Released_To_Date,
        //                    Readiness_Schedule_Vendor_Released = readinessMessage,
        //                    //Delivery_Schedule_PC_Breakup = item.Delivery_Schedule_PC_Breakup,
        //                    //Check2 = item.Check2,
        //                    //Line_Item_Schedule = item.Line_Item_Schedule,
        //                    //R_B = item.R_B,
        //                    //Schedule_Repeat = item.Schedule_Repeat,
        //                    //Internal_Pending_Issue = item.Internal_Pending_Issue,
        //                    //Pending_With = item.Pending_With,
        //                    //Remark = item.Remark,
        //                    //CRD_OverDue = item.CRD_OverDue,
        //                    Delivert_Date = item.Delivert_Date,
        //                    //Process_Plan_On_Crd = item.Process_Plan_On_Crd,
        //                    //Last_Week_PC = item.Last_Week_PC,
        //                    Schedule_Line_Qty1 = item.Schedule_Line_Qty1,
        //                    Schedule_Line_Date1 = item.Schedule_Line_Date1,
        //                    Schedule_Line_Qty2 = item.Schedule_Line_Qty2,
        //                    Schedule_Line_Date2 = item.Schedule_Line_Date2,
        //                    Schedule_Line_Qty3 = item.Schedule_Line_Qty3,
        //                    Schedule_Line_Date3 = item.Schedule_Line_Date3,
        //                    //To_Consider = item.To_Consider,
        //                    //Person_Name = item.Person_Name,
        //                    //Visibility = item.Visibility,
        //                    Key = (item.Indent_No ?? string.Empty) + (item.Old_Material_No ?? string.Empty) + (item.Vendor_Po_No ?? string.Empty),
        //                    Key1 = (item.Indent_No ?? string.Empty) + (item.Old_Material_No ?? string.Empty),
        //                    CreatedBy = uploadedBy,
        //                    CreatedDate = DateTime.Now
        //                };
        //                _dbContext.Sales_Order.Add(newEntity);
        //            }
        //        }

        //        await _dbContext.SaveChangesAsync();

        //        // Log the upload
        //        var importLog = new Open_Po_Log
        //        {
        //            FileName = fileName,
        //            TotalRecords = listOfData.Count,
        //            ImportedRecords = listOfData.Count - result.FailedRecords.Count,
        //            FailedRecords = result.FailedRecords.Count,
        //            UploadedBy = uploadedBy,
        //            UploadedAt = DateTime.Now,
        //            FileType = "SO"
        //        };

        //        _dbContext.OpenPo_Log.Add(importLog);
        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        result.Result = new OperationResult
        //        {
        //            Success = true,
        //            Message = result.FailedRecords.Any()
        //                ? "Import completed with some skipped records."
        //                : "All records imported successfully."
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        result.Result = new OperationResult
        //        {
        //            Success = false,
        //            Message = "Error during import: " + ex.Message
        //        };
        //    }

        //    return result;
        //}


        public async Task<BulkSalesCreateLogResult> BulkSalesCreateAsync(IList<Sales_Order_ViewModel> listOfData, string fileName, string uploadedBy)
        {
            var result = new BulkSalesCreateLogResult();
            var batchId = Guid.NewGuid();
            var uploadTs = DateTime.Now; // or DateTime.UtcNow

            var dt = new DataTable();
            // Column definitions MUST match dbo.SalesOrderImportRow
            dt.Columns.Add("SO_No", typeof(string));
            dt.Columns.Add("SaleOrder_Type", typeof(string));
            dt.Columns.Add("SO_Date", typeof(DateTime));
            dt.Columns.Add("Line_Item", typeof(int));
            dt.Columns.Add("Indent_No", typeof(string));
            dt.Columns.Add("Indent_Date", typeof(DateTime));

            dt.Columns.Add("Sales_Group", typeof(string));
            dt.Columns.Add("Sales_Group_Desc", typeof(string));
            dt.Columns.Add("Sales_Office", typeof(string));
            dt.Columns.Add("Sales_Office_Desc", typeof(string));
            dt.Columns.Add("Sale_Person", typeof(string));
            dt.Columns.Add("Project_Name", typeof(string));

            dt.Columns.Add("Customer_Code", typeof(string));
            dt.Columns.Add("Customer_Name", typeof(string));

            dt.Columns.Add("Material", typeof(string));
            dt.Columns.Add("Old_Material_No", typeof(string));
            dt.Columns.Add("Description", typeof(string));

            dt.Columns.Add("SO_Qty", typeof(int));
            dt.Columns.Add("SO_Value", typeof(decimal));

            dt.Columns.Add("Del_Qty", typeof(int));
            dt.Columns.Add("Open_Sale_Qty", typeof(int));
            dt.Columns.Add("Opne_Sale_Value", typeof(decimal));

            dt.Columns.Add("Plant", typeof(string));
            dt.Columns.Add("Item_Category", typeof(string));
            dt.Columns.Add("Procurement_Type", typeof(string));

            dt.Columns.Add("Vendor_Po_No", typeof(string));
            dt.Columns.Add("Vendor_Po_Date", typeof(DateTime));

            dt.Columns.Add("Po_Release_Qty", typeof(int));
            dt.Columns.Add("Allocated_Stock_Qty", typeof(int));
            dt.Columns.Add("Allocated_Stock_Value", typeof(decimal));

            dt.Columns.Add("Indent_Status", typeof(string));
            dt.Columns.Add("Delivert_Date", typeof(DateTime));

            dt.Columns.Add("Schedule_Line_Qty1", typeof(int));
            dt.Columns.Add("Schedule_Line_Date1", typeof(DateTime));
            dt.Columns.Add("Schedule_Line_Qty2", typeof(int));
            dt.Columns.Add("Schedule_Line_Date2", typeof(DateTime));
            dt.Columns.Add("Schedule_Line_Qty3", typeof(int));
            dt.Columns.Add("Schedule_Line_Date3", typeof(DateTime));

            foreach (var x in listOfData)
            {
                dt.Rows.Add(
                    (object?)x.SO_No ?? DBNull.Value,
                    (object?)x.SaleOrder_Type ?? DBNull.Value,
                    (object?)x.SO_Date ?? DBNull.Value,
                    (object?)x.Line_Item ?? DBNull.Value,

                    (object?)x.Indent_No ?? DBNull.Value,
                    (object?)x.Indent_Date ?? DBNull.Value,

                    (object?)x.Sales_Group ?? DBNull.Value,
                    (object?)x.Sales_Group_desc ?? DBNull.Value,
                    (object?)x.Sales_Office ?? DBNull.Value,
                    (object?)x.Sales_Office_Desc ?? DBNull.Value,
                    (object?)x.Sale_Person ?? DBNull.Value,
                    (object?)x.Project_Name ?? DBNull.Value,

                    (object?)x.Customer_Code ?? DBNull.Value,
                    (object?)x.Customer_Name ?? DBNull.Value,

                    (object?)x.Material ?? DBNull.Value,
                    (object?)x.Old_Material_No ?? DBNull.Value,
                    (object?)x.Description ?? DBNull.Value,

                    (object?)x.SO_Qty ?? DBNull.Value,
                    (object?)x.SO_Value ?? DBNull.Value,

                    (object?)x.Del_Qty ?? DBNull.Value,
                    (object?)x.Open_Sale_Qty ?? DBNull.Value,
                    (object?)x.Opne_Sale_Value ?? DBNull.Value,

                    (object?)x.Plant ?? DBNull.Value,
                    (object?)x.Item_Category ?? DBNull.Value,      // may be blank; SP derives if blank
                    (object?)x.Procurement_Type ?? DBNull.Value,

                    (object?)x.Vendor_Po_No ?? DBNull.Value,
                    (object?)x.Vendor_Po_Date ?? DBNull.Value,

                    (object?)x.Po_Release_Qty ?? DBNull.Value,
                    (object?)x.Allocated_Stock_Qty ?? DBNull.Value,
                    (object?)x.Allocated_Stock_Value ?? DBNull.Value,

                    (object?)x.Indent_Status ?? DBNull.Value,
                    (object?)x.Delivert_Date ?? DBNull.Value,

                    (object?)x.Schedule_Line_Qty1 ?? DBNull.Value,
                    (object?)x.Schedule_Line_Date1 ?? DBNull.Value,
                    (object?)x.Schedule_Line_Qty2 ?? DBNull.Value,
                    (object?)x.Schedule_Line_Date2 ?? DBNull.Value,
                    (object?)x.Schedule_Line_Qty3 ?? DBNull.Value,
                    (object?)x.Schedule_Line_Date3 ?? DBNull.Value
                );
            }



            //var connectionString = _dbContext.Database.GetDbConnection();
            using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction(); // local transaction for safety

            try
            {
                var dp = new DynamicParameters();
                dp.Add("@Rows", dt.AsTableValuedParameter("dbo.SalesOrderImportRow"));
                dp.Add("@FileName", fileName, DbType.String);
                dp.Add("@UploadedBy", uploadedBy, DbType.String);
                dp.Add("@BatchId", batchId, DbType.Guid);
                dp.Add("@UploadTs", uploadTs, DbType.DateTime2);

                using var multi = await conn.QueryMultipleAsync(
                    sql: "dbo.sp_SalesOrder_BulkUpsert",
                    param: dp,
                    commandType: CommandType.StoredProcedure,
                    transaction: tran
                );

                var summary = await multi.ReadFirstAsync<(int TotalRecords, int ImportedRecords, int FailedRecords)>();
                result.TotalRecords = summary.TotalRecords;
                result.ImportedRecords = summary.ImportedRecords;
                result.FailedCount = summary.FailedRecords;

                // Second result set: per-row failures (Indent_No, Old_Material_No, Reason)
                var failures = (await multi.ReadAsync<(string Indent_No, string Old_Material_No, string Reason)>()).ToList();

                // Map failures back to a minimal model envelope
                foreach (var f in failures)
                {
                    var vm = new Sales_Order_ViewModel
                    {
                        Indent_No = f.Indent_No,
                        Old_Material_No = f.Old_Material_No
                    };
                    result.FailedRecords.Add((vm, f.Reason));
                }

                await tran.CommitAsync();

                result.Result = new OperationResult
                {
                    Success = true,
                    Message = failures.Any()
                        ? "Import completed with some skipped records."
                        : "All records imported successfully."
                };
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
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
                    new SqlParameter("@PCWeekDate", updatedRecord.PCWeekDate ?? (object)DBNull.Value),
                    new SqlParameter("@Buffer_Day ", updatedRecord.Buffer_Day ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value)
                };

                var sql = @"EXEC sp_Update_Open_PO @Ven_PoId,@Comit_Date,@Comit_Qty,@Comit_Date1,@Comit_Qty1,@Comit_Final_Date,@Comit_Final_Qty,@Comit_Planner_Qty,@Comit_Planner_Date,@Comit_Planner_Remark,
                        @Comit_Vendor_Date,@Comit_Vendor_Qty,@PCWeekDate,@Buffer_Day, @UpdatedBy";

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

        public async Task<List<PCCalendarViewModel>> GetPCListAsync()
        {
            try
            {
                var result = await _dbContext.PC_Calendar.FromSqlRaw("EXEC sp_Get_PcCalendar").ToListAsync();

                var viewModelList = result.Select(data => new PCCalendarViewModel
                {
                    Id = data.Id,
                    PC = data.PC,
                    Week = data.Week,
                    From = data.From,
                    To = data.To,
                    Days = data.Days,
                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<BulkPCCreateLogResult> BulkPCCreateAsync(List<PCCalendarViewModel> listOfData, string fileName, string uploadedBy)
        {
            var res = new BulkPCCreateLogResult();
            if (listOfData == null || listOfData.Count == 0)
            {
                res.Result = new OperationResult { Success = true, Message = "No rows to import." };
                return res;
            }

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string KeyOf(PCCalendarViewModel x) =>
                $"{x.PC}|{x.Week}|{x.From?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}|{x.To?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";

            int inserted = 0;

            await using var tx = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in listOfData)
                {
                    // Skip lines that don't have a coherent range
                    if (!item.PC.HasValue || !item.Week.HasValue || !item.From.HasValue || !item.To.HasValue)
                    {
                        res.FailedRecords.Add((item, "Missing one of PC/Week/From/To."));
                        continue;
                    }
                    if (item.From.Value.Date > item.To.Value.Date)
                    {
                        res.FailedRecords.Add((item, "From date is after To date."));
                        continue;
                    }

                    // compute Days if not present
                    if (!item.Days.HasValue)
                        item.Days = (int)(item.To.Value.Date - item.From.Value.Date).TotalDays + 1;

                    // de-dup inside this file
                    var key = KeyOf(item);
                    if (!seen.Add(key))
                    {
                        res.FailedRecords.Add((item, "Duplicate in uploaded file (same PC|Week|From|To)."));
                        continue;
                    }

                    int pc = item.PC.Value;
                    int wk = item.Week.Value;
                    var from = item.From.Value.Date;
                    var to = item.To.Value.Date;

                    // exact dup in DB
                    bool exists = await _dbContext.PC_Calendar
                        .AnyAsync(x => x.PC == pc && x.Week == wk && x.From == from && x.To == to);
                    if (exists)
                    {
                        res.FailedRecords.Add((item, "Duplicate in database (same PC|Week|From|To)."));
                        continue;
                    }

                    // overlap on same PC (any week)
                    bool overlaps = await _dbContext.PC_Calendar
                        .AnyAsync(x => x.PC == pc && x.From <= to && x.To >= from);
                    if (overlaps)
                    {
                        res.FailedRecords.Add((item, "Overlapping date range exists for the same PC."));
                        continue;
                    }

                    var entity = new PC_Calendar_SCM
                    {
                        PC = pc,
                        Week = wk,
                        From = from,
                        To = to,
                        Days = item.Days,
                        CreatedBy = uploadedBy,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.PC_Calendar.Add(entity);
                    inserted++;
                }

                await _dbContext.SaveChangesAsync();
                await tx.CommitAsync();

                res.Result = new OperationResult
                {
                    Success = true,
                    Message = res.FailedRecords.Any()
                        ? $"Imported {inserted} of {listOfData.Count} rows. Some rows skipped."
                        : $"All {inserted} rows imported successfully."
                };
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                res.Result = new OperationResult { Success = false, Message = "Error during import: " + ex.Message };
            }

            return res;
        }

        public async Task<(List<OpenPODetailViewModel> openPo, List<So_DeliveryScheduleViewModel> deliverySch)> GetPOListByMaterialRefNoAsync(string? material, string? oldMaterialNo, int soId)
        {
            try
            {
                // Get the connection string from configuration or your context
                var connectionString = _dbContext.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    //var result = await connection.QueryAsync<OpenPODetailViewModel>("[dbo].[sp_Get_OpenPO_Delivery_ByMaterial]", new { Material = material, Old_Material_No = oldMaterialNo },commandType: CommandType.StoredProcedure);

                    using (var multi = await connection.QueryMultipleAsync("[dbo].[sp_Get_OpenPO_Delivery_ByMaterial]", new { Material = material, Old_Material_No = oldMaterialNo, SaleOrder_Id = soId }, commandType: CommandType.StoredProcedure))

                    {
                        var openPo = (await multi.ReadAsync<OpenPODetailViewModel>()).ToList();
                        //var deliverySch = (await multi.ReadAsync<So_DeliveryScheduleViewModel>()).ToList();

                        var rawRows = await multi.ReadAsync<dynamic>();

                        // Convert flat rows into grouped structure
                        var grouped = rawRows.GroupBy(r => new
                        {
                            Material = (string?)r.Material,
                            OldMaterial = (string?)r.Old_Material_No,
                            SO_Id = (int?)r.SaleOrder_Id
                        });

                        var deliverySch = new List<So_DeliveryScheduleViewModel>();

                        foreach (var grp in grouped)
                        {
                            var first = grp.First();

                            var parent = new So_DeliveryScheduleViewModel
                            {
                                Id = first.Id ?? 0,
                                Deleted = first.Deleted ?? false,
                                SaleOrder_Id = grp.Key.SO_Id,
                                Vendor = first.Vendor ?? "",
                                SO_No = first.SO_No ?? "",
                                SO_Date = first.SO_Date == null ? (DateTime?)null : Convert.ToDateTime(first.SO_Date),
                                SO_Qty = first.SO_Qty ?? 0,
                                Key = first.Key ?? "",
                                Key1 = first.Key1 ?? "",
                                Material = grp.Key.Material ?? "",
                                Old_Material_No = grp.Key.OldMaterial ?? "",
                                CreatedBy = first.CreatedBy ?? "",
                                CreatedDate = first.CreatedDate == null ? (DateTime?)null : Convert.ToDateTime(first.CreatedDate)
                            };

                            // Child list
                            int sr = 1;
                            foreach (var r in grp)
                            {
                                parent.SODeliveryScheduleList.Add(new So_DeliveryScheduleItem
                                {
                                    SrNo = sr++,
                                    Delivery_Date = r.Delivery_Date == null ? (DateTime?)null : Convert.ToDateTime(r.Delivery_Date),
                                    Delivery_Qty = r.Delivery_Qty ?? 0,
                                    Delivery_Remark = r.Delivery_Remark ?? "",
                                    Date_PC_Week = r.Date_PC_Week ?? ""
                                });
                            }

                            deliverySch.Add(parent);
                        }

                        return (openPo, deliverySch);
                    }
                }
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task SOSaveDeliverySchAsync(So_DeliveryScheduleViewModel model, string updatedBy)
        {
            try
            {
                // First delete existing records
                var existing = await _dbContext.So_Deliveries.Where(x => x.SaleOrder_Id == model.SaleOrder_Id).ToListAsync();
                _dbContext.So_Deliveries.RemoveRange(existing);

                // Insert new records
                foreach (var item in model.SODeliveryScheduleList)
                {
                    var entity = new So_DeliverySchedule
                    {
                        SaleOrder_Id = model.SaleOrder_Id,
                        Vendor = model.Vendor,
                        SO_No = model.SO_No,
                        SO_Date = model.SO_Date,
                        SO_Qty = model.SO_Qty,
                        Delivery_Date = item.Delivery_Date,
                        Delivery_Qty = item.Delivery_Qty,
                        Delivery_Remark = item.Delivery_Remark,
                        Date_PC_Week = item.Date_PC_Week,
                        Key = model.Key,
                        Key1 = model.Key1,
                        Material = model.Material,
                        Old_Material_No = model.Old_Material_No,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.So_Deliveries.Add(entity);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<(List<MatchedRecordViewModel> soHeaders, List<MatchSODeliverySchViewModel> deliverySchedules)> GetSOWithDeliveryScheduleAsync(string type)
        {
            try
            {
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync("[dbo].[sp_Get_SO_OpenPO_Snapshots_ByType]", new { Type = type }, commandType: CommandType.StoredProcedure))

                    {
                        var soHeaders = (await multi.ReadAsync<MatchedRecordViewModel>()).ToList();
                        var deliverySchedules = (await multi.ReadAsync<MatchSODeliverySchViewModel>()).ToList();

                        return (soHeaders, deliverySchedules);
                    }
                }
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<BulkMTACreateResult> BulkMTACreateAsync(List<MTAMasterViewModel> listOfData, string fileName, string uploadedBy)
        {
            var result = new BulkMTACreateResult();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var seenKeys = new HashSet<string>(); // for tracking in-batch duplicates

                foreach (var item in listOfData)
                {
                    item.Material_No = item.Material_No?.Trim();
                    item.Ref_Code = item.Ref_Code?.Trim();

                    var compositeKey = $"{item.Material_No}|{item.Ref_Code}";

                    if (string.IsNullOrWhiteSpace(item.Material_No) || string.IsNullOrWhiteSpace(item.Ref_Code))
                    {
                        result.FailedRecords.Add((item, "Missing Material_No or Ref_Code"));
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
                    var existingEntity = await _dbContext.MTAMaster.FirstOrDefaultAsync(x => x.Material_No == item.Material_No && x.Ref_Code == item.Ref_Code);

                    if (existingEntity != null)
                    {
                        // Compare fields one by one — if any field differs, update
                        bool isDifferent =
                            existingEntity.Material_No != item.Material_No ||
                            existingEntity.Ref_Code != item.Ref_Code ||
                            existingEntity.Material_Desc != item.Material_Desc ||
                            existingEntity.Tog != item.Tog ||
                            existingEntity.Tor != item.Tor ||
                            existingEntity.Toy != item.Toy ||
                            existingEntity.Spike_Threshold != item.Spike_Threshold ||
                            existingEntity.Material_Category != item.Material_Category;

                        if (isDifferent)
                        {
                            // Update existing entity
                            existingEntity.Material_No = item.Material_No;
                            existingEntity.Ref_Code = item.Ref_Code;
                            existingEntity.Material_Desc = item.Material_Desc;
                            existingEntity.Tog = item.Tog;
                            existingEntity.Tor = item.Tor;
                            existingEntity.Toy = item.Toy;
                            existingEntity.Spike_Threshold = item.Spike_Threshold;
                            existingEntity.Material_Category = item.Material_Category;
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
                        var newEntity = new MTAMaster_SCM
                        {
                            Material_No = item.Material_No,
                            Ref_Code = item.Ref_Code,
                            Material_Desc = item.Material_Desc,
                            Tog = item.Tog,
                            Tor = item.Tor,
                            Toy = item.Toy,
                            Spike_Threshold = item.Spike_Threshold,
                            Material_Category = item.Material_Category,
                            CreatedBy = uploadedBy,
                            CreatedDate = DateTime.Now,

                        };
                        _dbContext.MTAMaster.Add(newEntity);
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
                    UploadedAt = DateTime.Now,
                    FileType = "MTA - File"
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

        public async Task<List<MTAMasterViewModel>> GetMTAListAsync()
        {
            try
            {
                var result = await _dbContext.MTAMaster.FromSqlRaw("EXEC sp_Get_MTAMaster").ToListAsync();

                var viewModelList = result.Select(data => new MTAMasterViewModel
                {
                    Id = data.Id,
                    Material_No = data.Material_No,
                    Ref_Code = data.Ref_Code,
                    Material_Desc = data.Material_Desc,
                    Tog = data.Tog,
                    Tor = data.Tor,
                    Toy = data.Toy,
                    Spike_Threshold = data.Spike_Threshold,
                    Material_Category = data.Material_Category,
                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }


        public async Task<BulkOpenPoDeliveryResult> BulkCreateDeliveryScheduleAsync_Dapper(List<OpenPoDeliveryExcelRow> listOfData, string uploadedBy,bool status)
        {
            var result = new BulkOpenPoDeliveryResult();

            // Build TVP DataTable
            var tvp = new DataTable();
            tvp.Columns.AddRange(new[]
            {
                new DataColumn("ExcelRowNo", typeof(int)),
                new DataColumn("Key", typeof(string)),
                new DataColumn("Vendor", typeof(string)),
                new DataColumn("PO_No", typeof(string)),
                new DataColumn("PO_Date", typeof(DateTime)),
                new DataColumn("PO_Qty", typeof(int)),
                new DataColumn("BalanceQty", typeof(int)),
                new DataColumn("Delivery_Date", typeof(DateTime)),
                new DataColumn("Date_PC_Week", typeof(string)),
                new DataColumn("Qty", typeof(int)),
                new DataColumn("Remark", typeof(string))
            });

            string? trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

            foreach (var x in listOfData)
            {
                tvp.Rows.Add(
                    x.ExcelRowNo,
                    trim(x.Key) ?? "",
                    trim(x.Vendor),
                    trim(x.PO_No),
                    x.PO_Date.HasValue ? x.PO_Date.Value : (object)DBNull.Value,
                    x.PO_Qty.HasValue ? x.PO_Qty.Value : (object)DBNull.Value,
                    x.BalanceQty.HasValue ? x.BalanceQty.Value : (object)DBNull.Value,
                    x.Delivery_Date.HasValue ? x.Delivery_Date.Value : (object)DBNull.Value,
                    trim(x.Date_PC_Week),
                    x.Qty.HasValue ? x.Qty.Value : (object)DBNull.Value,
                    trim(x.Remark)
                );
            }

            try
            {
                using var conn = _dbContext.Database.GetDbConnection();
                await conn.OpenAsync();

                var dp = new DynamicParameters();
                dp.Add("@Rows", tvp.AsTableValuedParameter("dbo.OpenPoDelivery_ImportRow"));
                dp.Add("@UploadedBy", uploadedBy, DbType.String, size: 100);
                dp.Add("@Status", status, DbType.Boolean);

                using var grid = await conn.QueryMultipleAsync(
                    sql: "dbo.sp_OpenPoDeliverySch_BulkImport",
                    param: dp,
                    commandType: CommandType.StoredProcedure);

                // 1) Summary
                 var summary = (await grid.ReadAsync<ImportSummaryDto>()).FirstOrDefault()
                          ?? new ImportSummaryDto();
                int total = (int)(summary?.TotalRecords ?? 0);
                int imported = (int)(summary?.ImportedRecords ?? 0);
                int failed = (int)(summary?.FailedRecords ?? 0);

                // 2) Failed rows
                var failedRows = (await grid.ReadAsync<FailedRowDto>()).ToList();

                foreach (var f in failedRows)
                {
                    // Map back to original by ExcelRowNo (best) else Key|PO_No
                    OpenPoDeliveryExcelRow? row = null;

                    if (f.ExcelRowNo.HasValue)
                    {
                        row = listOfData.FirstOrDefault(r => r.ExcelRowNo == f.ExcelRowNo.Value);
                    }

                    if (row == null)
                    {
                        row = listOfData.FirstOrDefault(r =>
                            string.Equals((r.Key ?? "").Trim(), (f.Key ?? "").Trim(), StringComparison.OrdinalIgnoreCase) &&
                            string.Equals((r.PO_No ?? "").Trim(), (f.PO_No ?? "").Trim(), StringComparison.OrdinalIgnoreCase));
                    }

                    result.FailedRecords.Add((row ?? new OpenPoDeliveryExcelRow
                    {
                        ExcelRowNo = f.ExcelRowNo ?? 0,
                        Key = f.Key ?? "",
                        PO_No = f.PO_No
                    }, f.Reason ?? "Validation failed"));
                }

                // Success rule: partial success allowed
                result.Result = new OperationResult
                {
                    Success = result.ImportedRecords > 0,
                    Message = failedRows.Any()
                        ? $"Import completed: {result.ImportedRecords}/{result.TotalRecords} imported, {result.FailedCount} failed."
                        : $"All {result.ImportedRecords}/{result.TotalRecords} records imported successfully."
                };
            }
            catch (Exception ex)
            {
                result.Result = new OperationResult
                {
                    Success = false,
                    Message = "Error during import: " + ex.Message
                };
            }

            return result;
        }

        public async Task<OperationResult> IsSubmittedAsync(IEnumerable<int> ids, bool returnCreatedRecord = false)
        {
            if (ids == null || !ids.Any())
                return new OperationResult { Success = false, Message = "No data was selected!." };

            var idList = ids.Distinct().ToList();

            var updated = await _dbContext.Opne_Po_Deliveries
                .Where(x => !x.Deleted && idList.Contains(x.Ven_PoId))
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, true));

            if (updated == 0)
                return new OperationResult { Success = false, Message = "No matching records found." };

            return new OperationResult { Success = true, Message = $"Updated successfully! Rows updated: {updated}." };
        }


    }
}
