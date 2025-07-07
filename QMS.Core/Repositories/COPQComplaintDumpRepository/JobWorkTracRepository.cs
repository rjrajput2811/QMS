using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.COPQComplaintDumpRepository
{
    public class JobWorkTracRepository : SqlTableRepository, IJobWorkTracRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public JobWorkTracRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<JobWork_TracViewModel>> GetJobListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.JobWorkTrac
                    .FromSqlRaw("EXEC sp_Get_JobTracking")
                    .ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.Wipro_Dc_Date.HasValue &&
                                    x.Wipro_Dc_Date.Value.Date >= startDate.Value.Date &&
                                    x.Wipro_Dc_Date.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                return result.Select(x => new JobWork_TracViewModel
                {
                    Id = x.Id,
                    Deleted = x.Deleted,
                    Vendor = x.Vendor,
                    Wipro_Dc_No = x.Wipro_Dc_No,
                    Wipro_Dc_Date = x.Wipro_Dc_Date,
                    Dc_Sap_Code = x.Dc_Sap_Code,
                    Qty_Wipro_Dc = x.Qty_Wipro_Dc,
                    Wipro_Transporter = x.Wipro_Transporter,
                    Wipro_LR_No = x.Wipro_LR_No,
                    Wipro_LR_Date = x.Wipro_LR_Date,
                    Actu_Rece_Qty = x.Actu_Rece_Qty,
                    Dispatch_Dc = x.Dispatch_Dc,
                    Dispatch_Invoice = x.Dispatch_Invoice,
                    Non_Repairable = x.Non_Repairable,
                    Grand_Total = x.Grand_Total,
                    To_Process = x.To_Process,
                    Remark = x.Remark,
                    Vendor_Transporter = x.Vendor_Transporter,
                    Vendor_LR_No = x.Vendor_LR_No,
                    Vendor_LR_Date = x.Vendor_LR_Date,
                    Write_Off_Approved = x.Write_Off_Approved,
                    Write_Off_Date = x.Write_Off_Date,
                    Pending_Write_Off = x.Pending_Write_Off,
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

        public async Task<JobWork_TracViewModel?> GetJobByIdAsync(int id)
        {
            try
            {
                var parameters = new[] { new SqlParameter("@JobTrac_Id", id) };

                var result = await _dbContext.JobWorkTrac
                    .FromSqlRaw("EXEC sp_Get_JobWorkTrac_ById @JobTrac_Id", parameters)
                    .ToListAsync();

                return result.Select(x => new JobWork_TracViewModel
                {
                    Id = x.Id,
                    Deleted = x.Deleted,
                    Vendor = x.Vendor,
                    Wipro_Dc_No = x.Wipro_Dc_No,
                    Wipro_Dc_Date = x.Wipro_Dc_Date,
                    Dc_Sap_Code = x.Dc_Sap_Code,
                    Qty_Wipro_Dc = x.Qty_Wipro_Dc,
                    Wipro_Transporter = x.Wipro_Transporter,
                    Wipro_LR_No = x.Wipro_LR_No,
                    Wipro_LR_Date = x.Wipro_LR_Date,
                    Actu_Rece_Qty = x.Actu_Rece_Qty,
                    Dispatch_Dc = x.Dispatch_Dc,
                    Dispatch_Invoice = x.Dispatch_Invoice,
                    Non_Repairable = x.Non_Repairable,
                    Grand_Total = x.Grand_Total,
                    To_Process = x.To_Process,
                    Remark = x.Remark,
                    Vendor_Transporter = x.Vendor_Transporter,
                    Vendor_LR_No = x.Vendor_LR_No,
                    Vendor_LR_Date = x.Vendor_LR_Date,
                    Write_Off_Approved = x.Write_Off_Approved,
                    Write_Off_Date = x.Write_Off_Date,
                    Pending_Write_Off = x.Pending_Write_Off,
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

        public async Task<OperationResult> CreateJobAsync(JobWork_Tracking_Service record, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Vendor", record.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Dc_No", record.Wipro_Dc_No ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Dc_Date", record.Wipro_Dc_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Dc_Sap_Code", record.Dc_Sap_Code ?? (object)DBNull.Value),
                    new SqlParameter("@Qty_Wipro_Dc", record.Qty_Wipro_Dc ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Transporter", record.Wipro_Transporter ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_LR_No", record.Wipro_LR_No ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_LR_Date", record.Wipro_LR_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Actu_Rece_Qty", record.Actu_Rece_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Dispatch_Dc", record.Dispatch_Dc ?? (object)DBNull.Value),
                    new SqlParameter("@Dispatch_Invoice", record.Dispatch_Invoice ?? (object)DBNull.Value),
                    new SqlParameter("@Non_Repairable", record.Non_Repairable ?? (object)DBNull.Value),
                    new SqlParameter("@Grand_Total", record.Grand_Total ?? (object)DBNull.Value),
                    new SqlParameter("@To_Process", record.To_Process ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", record.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor_Transporter", record.Vendor_Transporter ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor_LR_No", record.Vendor_LR_No ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor_LR_Date", record.Vendor_LR_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Write_Off_Approved", record.Write_Off_Approved ?? (object)DBNull.Value),
                    new SqlParameter("@Write_Off_Date", record.Write_Off_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Pending_Write_Off", record.Pending_Write_Off ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", record.CreatedBy ?? (object)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_Insert_JobWorkTrac 
                    @Vendor, @Wipro_Dc_No, @Wipro_Dc_Date, @Dc_Sap_Code, @Qty_Wipro_Dc, @Wipro_Transporter, @Wipro_LR_No, @Wipro_LR_Date, @Actu_Rece_Qty, @Dispatch_Dc, @Dispatch_Invoice,
                    @Non_Repairable, @Grand_Total, @To_Process, @Remark, @Vendor_Transporter, @Vendor_LR_No, @Vendor_LR_Date, @Write_Off_Approved,@Write_Off_Date, @Pending_Write_Off, @CreatedBy",
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

        public async Task<OperationResult> UpdateJobAsync(JobWork_Tracking_Service record, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@JobTrac_Id", record.Id),
                    new SqlParameter("@Vendor", record.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Dc_No", record.Wipro_Dc_No ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Dc_Date", record.Wipro_Dc_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Dc_Sap_Code", record.Dc_Sap_Code ?? (object)DBNull.Value),
                    new SqlParameter("@Qty_Wipro_Dc", record.Qty_Wipro_Dc ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Transporter", record.Wipro_Transporter ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_LR_No", record.Wipro_LR_No ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_LR_Date", record.Wipro_LR_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Actu_Rece_Qty", record.Actu_Rece_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Dispatch_Dc", record.Dispatch_Dc ?? (object)DBNull.Value),
                    new SqlParameter("@Dispatch_Invoice", record.Dispatch_Invoice ?? (object)DBNull.Value),
                    new SqlParameter("@Non_Repairable", record.Non_Repairable ?? (object)DBNull.Value),
                    new SqlParameter("@Grand_Total", record.Grand_Total ?? (object)DBNull.Value),
                    new SqlParameter("@To_Process", record.To_Process ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", record.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor_Transporter", record.Vendor_Transporter ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor_LR_No", record.Vendor_LR_No ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor_LR_Date", record.Vendor_LR_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Write_Off_Approved", record.Write_Off_Approved ?? (object)DBNull.Value),
                    new SqlParameter("@Write_Off_Date", record.Write_Off_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Pending_Write_Off", record.Pending_Write_Off ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", record.UpdatedBy ?? (object)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_Update_JobWrokTrac 
                        @Vendor, @Wipro_Dc_No, @Wipro_Dc_Date, @Dc_Sap_Code, @Qty_Wipro_Dc, @Wipro_Transporter, @Wipro_LR_No, @Wipro_LR_Date, @Actu_Rece_Qty, @Dispatch_Dc, @Dispatch_Invoice,
                        @Non_Repairable, @Grand_Total, @To_Process, @Remark, @Vendor_Transporter, @Vendor_LR_No, @Vendor_LR_Date, @Write_Off_Approved,@Write_Off_Date, @Pending_Write_Off, @UpdatedBy",
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

        public async Task<OperationResult> DeleteJobAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<JobWork_Tracking_Service>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

       

        public async Task<BulkCreateJobResult> BulkCreateJobAsync(List<JobWork_TracViewModel> listOfData, string fileName, string uploadedBy, string recordType)
        {
            var result = new BulkCreateJobResult();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var seenKeys = new HashSet<string>(); // for tracking in-batch duplicates

                foreach (var item in listOfData)
                {
                    item.Vendor = item.Vendor;
                    item.Wipro_Dc_No = item.Wipro_Dc_No?.Trim();

                    var compositeKey = $"{item.Vendor}|{item.Wipro_Dc_No}";

                    if (string.IsNullOrWhiteSpace(item.Vendor.ToString()) || string.IsNullOrWhiteSpace(item.Wipro_Dc_No))
                    {
                        result.FailedRecords.Add((item, "Missing Vendor or Wipro Dc No."));
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
                    bool existsInDb = await _dbContext.JobWorkTrac
                        .AnyAsync(x => x.Vendor == item.Vendor && x.Wipro_Dc_No == item.Wipro_Dc_No);

                    if (existsInDb)
                    {
                        result.FailedRecords.Add((item, "Duplicate in database"));
                        continue;
                    }

                    // Passed all checks — add to DB
                    var entity = new JobWork_Tracking_Service
                    {
                        Vendor = item.Vendor,
                        Wipro_Dc_No = item.Wipro_Dc_No,
                        Wipro_Dc_Date = item.Wipro_Dc_Date,
                        Dc_Sap_Code = item.Dc_Sap_Code,
                        Qty_Wipro_Dc = item.Qty_Wipro_Dc,
                        Wipro_Transporter = item.Wipro_Transporter,
                        Wipro_LR_No = item.Wipro_LR_No,
                        Wipro_LR_Date = item.Wipro_LR_Date,
                        Actu_Rece_Qty = item.Actu_Rece_Qty,
                        Dispatch_Dc = item.Dispatch_Dc,
                        Dispatch_Invoice = item.Dispatch_Invoice,
                        Non_Repairable = item.Non_Repairable,
                        Grand_Total = item.Grand_Total,
                        To_Process = item.To_Process,
                        Remark = item.Remark,
                        Vendor_Transporter = item.Vendor_Transporter,
                        Vendor_LR_No = item.Vendor_LR_No,
                        Vendor_LR_Date = item.Vendor_LR_Date,
                        Write_Off_Approved = item.Write_Off_Approved,
                        Write_Off_Date = item.Write_Off_Date,
                        Pending_Write_Off = item.Pending_Write_Off,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    };

                    _dbContext.JobWorkTrac.Add(entity);
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
        
    }
}
