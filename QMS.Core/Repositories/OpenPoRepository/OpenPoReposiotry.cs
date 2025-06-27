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

        public async Task<List<Open_PoViewModel>> GetListAsync(string vendor)
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
                    var existingEntity = await _dbContext.OpenPo.FirstOrDefaultAsync(x =>x.PO_No == item.PO_No);

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

                using (var multi = await connection.QueryMultipleAsync("[dbo].[sp_Get_OpenPO_With_DeliverySchedule]",new { Vendor = vendor },commandType: CommandType.StoredProcedure))

                {
                    var poHeaders = (await multi.ReadAsync<Open_Po>()).ToList();
                    var deliverySchedules = (await multi.ReadAsync<Opne_Po_DeliverySchedule>()).ToList();

                    return (poHeaders, deliverySchedules);
                }
            }
        }

    }
}
