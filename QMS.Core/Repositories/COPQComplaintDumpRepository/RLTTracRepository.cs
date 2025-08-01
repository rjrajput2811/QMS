using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

namespace QMS.Core.Repositories.COPQComplaintDumpRepository
{
    public class RLTTracRepository : SqlTableRepository, IRLTTracRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public RLTTracRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<RLT_TracViewModel>> GetRLTListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.RLTTrac
                    .FromSqlRaw("EXEC sp_Get_RLTTrack")
                    .ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.Po_Date.HasValue &&
                                    x.Po_Date.Value.Date >= startDate.Value.Date &&
                                    x.Po_Date.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                return result.Select(x => new RLT_TracViewModel
                {
                    Id = x.Id,
                    Deleted = x.Deleted,
                    Vendor = x.Vendor,
                    Material = x.Material,
                    Ref_No = x.Ref_No,
                    Po_No = x.Po_No,
                    Po_Date = x.Po_Date,
                    PR_No = x.PR_No,
                    Batch_No = x.Batch_No,
                    Po_Qty = x.Po_Qty,
                    Balance_Qty = x.Balance_Qty,
                    Destination = x.Destination,
                    Balance_Value = x.Balance_Value,
                    Lead_Time = x.Lead_Time,
                    Lead_Time_Range = x.Lead_Time_Range,
                    Dispatch_Date = x.Dispatch_Date,
                    Remark = x.Remark,
                    Wipro_Remark = x.Wipro_Remark,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate

                }).ToList();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<RLT_TracViewModel?> GetRLTByIdAsync(int id)
        {
            try
            {
                var parameters = new[] { new SqlParameter("@RLT_Id", id) };

                var result = await _dbContext.RLTTrac
                    .FromSqlRaw("EXEC sp_Get_RLTTrack_ById @RLT_Id", parameters)
                    .ToListAsync();

                return result.Select(x => new RLT_TracViewModel
                {
                    Id = x.Id,
                    Deleted = x.Deleted,
                    Vendor = x.Vendor,
                    Material = x.Material,
                    Ref_No = x.Ref_No,
                    Po_No = x.Po_No,
                    Po_Date = x.Po_Date,
                    PR_No = x.PR_No,
                    Batch_No = x.Batch_No,
                    Po_Qty = x.Po_Qty,
                    Balance_Qty = x.Balance_Qty,
                    Destination = x.Destination,
                    Balance_Value = x.Balance_Value,
                    Lead_Time = x.Lead_Time,
                    Lead_Time_Range = x.Lead_Time_Range,
                    Dispatch_Date = x.Dispatch_Date,
                    Remark = x.Remark,
                    Wipro_Remark = x.Wipro_Remark,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateRLTAsync(RLT_Tracking_Service record, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Vendor", record.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Material", record.Material ?? (object)DBNull.Value),
                    new SqlParameter("@Ref_No", record.Ref_No ?? (object)DBNull.Value),
                    new SqlParameter("@Po_No", record.Po_No ?? (object)DBNull.Value),
                    new SqlParameter("@Po_Date", record.Po_Date ?? (object)DBNull.Value),
                    new SqlParameter("@PR_No", record.PR_No ?? (object)DBNull.Value),
                    new SqlParameter("@Batch_No", record.Batch_No ?? (object)DBNull.Value),
                    new SqlParameter("@Po_Qty", record.Po_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Balance_Qty", record.Balance_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Destination", record.Destination ?? (object)DBNull.Value),
                    new SqlParameter("@Balance_Value", record.Balance_Value ?? (object)DBNull.Value),
                    new SqlParameter("@Lead_Time", record.Lead_Time ?? (object)DBNull.Value),
                    new SqlParameter("@Lead_Time_Range", record.Lead_Time_Range ?? (object)DBNull.Value),
                    new SqlParameter("@Dispatch_Date", record.Dispatch_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", record.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Remark", record.Wipro_Remark ?? (object)DBNull.Value),
                    new SqlParameter("@CreatdBy", record.CreatedBy ?? (object)DBNull.Value),
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_Insert_RLTTrac 
                    @Vendor, @Material, @Ref_No, @Po_No, @Po_Date, @PR_No, @Batch_No, @Po_Qty, @Balance_Qty, @Destination, @Balance_Value,
                    @Lead_Time, @Lead_Time_Range, @Dispatch_Date, @Remark, @Wipro_Remark, @CreatdBy",
                    parameters
                );

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateRLTAsync(RLT_Tracking_Service record, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@RLT_Id", record.Id),
                    new SqlParameter("@Vendor", record.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Material", record.Material ?? (object)DBNull.Value),
                    new SqlParameter("@Ref_No", record.Ref_No ?? (object)DBNull.Value),
                    new SqlParameter("@Po_No", record.Po_No ?? (object)DBNull.Value),
                    new SqlParameter("@Po_Date", record.Po_Date ?? (object)DBNull.Value),
                    new SqlParameter("@PR_No", record.PR_No ?? (object)DBNull.Value),
                    new SqlParameter("@Batch_No", record.Batch_No ?? (object)DBNull.Value),
                    new SqlParameter("@Po_Qty", record.Po_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Balance_Qty", record.Balance_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Destination", record.Destination ?? (object)DBNull.Value),
                    new SqlParameter("@Balance_Value", record.Balance_Value ?? (object)DBNull.Value),
                    new SqlParameter("@Lead_Time", record.Lead_Time ?? (object)DBNull.Value),
                    new SqlParameter("@Lead_Time_Range", record.Lead_Time_Range ?? (object)DBNull.Value),
                    new SqlParameter("@Dispatch_Date", record.Dispatch_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", record.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Remark", record.Wipro_Remark ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", record.UpdatedBy ?? (object)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_Update_RLTTrack 
                        @RLT_Id,@Vendor, @Material, @Ref_No, @Po_No, @Po_Date, @PR_No, @Batch_No, @Po_Qty, @Balance_Qty, @Destination, @Balance_Value,
                        @Lead_Time, @Lead_Time_Range, @Dispatch_Date, @Remark, @Wipro_Remark, @UpdatedBy",
                    parameters
                );

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteRLTAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<RLT_Tracking_Service>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }



        public async Task<BulkCreateRLTResult> BulkCreateRLTAsync(List<RLT_TracViewModel> listOfData, string fileName, string uploadedBy, string recordType)
        {
            var result = new BulkCreateRLTResult();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var seenKeys = new HashSet<string>();

                foreach (var item in listOfData)
                {
                    item.Vendor = item.Vendor;
                    item.Ref_No = item.Ref_No?.Trim();
                    item.Po_No = item.Po_No?.Trim();

                    var compositeKey = $"{item.Vendor}|{item.Ref_No}|{item.Po_No}";

                    if (string.IsNullOrWhiteSpace(item.Vendor) || string.IsNullOrWhiteSpace(item.Ref_No) || string.IsNullOrWhiteSpace(item.Po_No))
                    {
                        result.FailedRecords.Add((item, "Missing Vendor or Ref. No or Po No."));
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
                    bool existsInDb = await _dbContext.RLTTrac
                        .AnyAsync(x => x.Vendor == item.Vendor && x.Ref_No == item.Ref_No && x.Po_No == item.Po_No);

                    if (existsInDb)
                    {
                        result.FailedRecords.Add((item, "Duplicate in database"));
                        continue;
                    }

                    // Passed all checks — add to DB
                    var entity = new RLT_Tracking_Service
                    {
                        Vendor = item.Vendor,
                        Material = item.Material,
                        Ref_No = item.Ref_No,
                        Po_No = item.Po_No,
                        Po_Date = item.Po_Date,
                        PR_No = item.PR_No,
                        Batch_No = item.Batch_No,
                        Po_Qty = item.Po_Qty,
                        Balance_Qty = item.Balance_Qty,
                        Destination = item.Destination,
                        Balance_Value = item.Balance_Value,
                        Lead_Time = item.Lead_Time,
                        Lead_Time_Range = item.Lead_Time_Range,
                        Dispatch_Date = item.Dispatch_Date,
                        Remark = item.Remark,
                        Wipro_Remark = item.Wipro_Remark,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    };

                    _dbContext.RLTTrac.Add(entity);
                }

                await _dbContext.SaveChangesAsync();

                // Log the upload
                var importLog = new FailedRecord_Log
                {
                    FileName = fileName,
                    TotalRecords = listOfData.Count,
                    ImportedRecords = listOfData.Count - result.FailedRecords.Count,
                    FailedRecords = result.FailedRecords.Count,
                    RecordType = recordType,
                    UploadedBy = uploadedBy,
                    UploadedAt = DateTime.Now
                };

                _dbContext.FailedRecord_Log.Add(importLog);
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

        public async Task<List<FinalRLTOutput>> GetFinalRLTListAsync()
        {
            // Get the connection string from configuration or your context
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<FinalRLTOutput>(
                    "[dbo].[sp_Final_RLTOutput]", commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }
    }
}
