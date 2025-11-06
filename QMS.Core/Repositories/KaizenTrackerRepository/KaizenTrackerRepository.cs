using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QMS.Core.Repositories.KaizenTrackerRepository
{
    public class KaizenTrackerRepository : SqlTableRepository, IKaizenTrackerRepository
    {
        private readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public KaizenTrackerRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<KaizenTracViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.Kaizen_Tracker.FromSqlRaw("EXEC sp_Get_KaizenTracker").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                return result.Select(data => new KaizenTracViewModel
                {
                    Id = data.Id,
                    Vendor = data.Vendor,
                    Kaizen_Theme = data.Kaizen_Theme,
                    Month = data.Month,
                    Team = data.Team,
                    Kaizen_Attch = data.Kaizen_Attch,
                    Remark = data.Remark,
                    FY = data.FY,
                    Categorised_Scope = data.Categorised_Scope,
                    CreatedDate = data.CreatedDate,
                    UpdatedDate = data.UpdatedDate,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.UpdatedBy
                }).ToList();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<KaizenTracViewModel?> GetByIdAsync(int ven_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Kaizen_Id", ven_Id),
                };

                var sql = @"EXEC sp_Get_KaizenTracker_ByID @Kaizen_Id";

                var result = await _dbContext.Kaizen_Tracker.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new KaizenTracViewModel
                {
                    Id = data.Id,
                    Vendor = data.Vendor,
                    Kaizen_Theme = data.Kaizen_Theme,
                    Month = data.Month,
                    Team = data.Team,
                    Kaizen_Attch = data.Kaizen_Attch,
                    Remark = data.Remark,
                    FY = data.FY,
                    Categorised_Scope = data.Categorised_Scope,
                    CreatedDate = data.CreatedDate,
                    UpdatedDate = data.UpdatedDate,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.UpdatedBy

                }).ToList();

                return viewModelList.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateAsync(Kaizen_Tracker kaizen, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Vendor", kaizen.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Kaizen_Theme", kaizen.Kaizen_Theme ?? (object)DBNull.Value),
                    new SqlParameter("@Month", kaizen.Month ?? (object)DBNull.Value),
                    new SqlParameter("@Team", kaizen.Team ?? (object)DBNull.Value),
                    new SqlParameter("@Kaizen_Attch", kaizen.Kaizen_Attch ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", kaizen.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@FY", kaizen.FY ?? (object)DBNull.Value),
                    new SqlParameter("@Categorised_Scope", kaizen.Categorised_Scope ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", kaizen.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", kaizen.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_KaizenTracker @Vendor, @Kaizen_Theme, @Month, @Team, @Kaizen_Attch, @Remark, @FY, @Categorised_Scope, @CreatedBy, @IsDeleted",
                    parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateAsync(Kaizen_Tracker kaizen, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Kaizen_Id", kaizen.Id),
                    new SqlParameter("@Vendor", kaizen.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Kaizen_Theme", kaizen.Kaizen_Theme ?? (object)DBNull.Value),
                    new SqlParameter("@Month", kaizen.Month ?? (object)DBNull.Value),
                    new SqlParameter("@Team", kaizen.Team ?? (object)DBNull.Value),
                    new SqlParameter("@Kaizen_Attch", kaizen.Kaizen_Attch ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", kaizen.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@FY", kaizen.FY ?? (object)DBNull.Value),
                    new SqlParameter("@Categorised_Scope", kaizen.Categorised_Scope ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", kaizen.UpdatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", kaizen.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Update_KaizenTracker @Kaizen_Id, @Vendor, @Kaizen_Theme, @Month, @Team, @Kaizen_Attch, @Remark, @FY, @Categorised_Scope,@UpdatedBy, @IsDeleted", parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
       
        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<Kaizen_Tracker>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAttachmentAsync(int id, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                var record = await _dbContext.Kaizen_Tracker.FindAsync(id);
                if (record == null)
                    return false;

                record.Remark = fileName;

                // Only update the BIS_Attachment property
                _dbContext.Entry(record).Property(x => x.Remark).IsModified = true;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<BulkKaizenCreateResult> BulkKaizenCreateAsync(List<KaizenTracViewModel> listOfData, string fileName, string uploadedBy, string recordType)
        {
            var result = new BulkKaizenCreateResult();
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // 1) Preload vendor master once
                var vendors = await _dbContext.Vendor
                    .Where(v => !v.Deleted) // keep only active vendors
                    .Select(v => new { v.Name, v.Vendor_Code })
                    .ToListAsync();

                static string Key(string s) => (s ?? string.Empty).Trim().ToUpperInvariant();
                var vendorByName = vendors
                    .GroupBy(v => Key(v.Name))
                    .ToDictionary(g => g.Key, g => g.First().Vendor_Code); // if dup names exist, you may want to fail instead
                var vendorByCode = vendors
                    .GroupBy(v => Key(v.Vendor_Code))
                    .ToDictionary(g => g.Key, g => g.First().Vendor_Code);

                var seenKeys = new HashSet<string>(); // for tracking in-batch duplicates

                foreach (var item in listOfData)
                {
                    // Normalize inputs
                    item.Kaizen_Theme = (item.Kaizen_Theme ?? string.Empty).Trim();
                    var incomingVendor = (item.Vendor ?? string.Empty).Trim();

                    // Basic validation
                    if (string.IsNullOrWhiteSpace(item.Kaizen_Theme))
                    {
                        result.FailedRecords.Add((item, "Missing Kaizen Theme"));
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(incomingVendor))
                    {
                        result.FailedRecords.Add((item, "Missing Vendor"));
                        continue;
                    }

                    // 3) Resolve Vendor → Vendor_Code (accept either name or code from file)
                    string vendorCode = null;
                    var key = Key(incomingVendor);
                    if (vendorByCode.TryGetValue(key, out var codeFromCode))
                        vendorCode = codeFromCode;
                    else if (vendorByName.TryGetValue(key, out var codeFromName))
                        vendorCode = codeFromName;

                    if (string.IsNullOrEmpty(vendorCode))
                    {
                        result.FailedRecords.Add((item, $"Unknown Vendor: '{incomingVendor}'"));
                        continue;
                    }

                    // In-batch duplicate check (optionally include Vendor/FY/Month if needed)
                    var compositeKey = $"{Key(item.Kaizen_Theme)}|{vendorCode}";
                    if (seenKeys.Contains(compositeKey))
                    {
                        result.FailedRecords.Add((item, "Duplicate in uploaded file"));
                        continue;
                    }
                    seenKeys.Add(compositeKey);

                    // 4) Database duplicate check
                    bool existsInDb = await _dbContext.Kaizen_Tracker.AnyAsync(x =>
                        x.Kaizen_Theme == item.Kaizen_Theme &&
                        x.Vendor == vendorCode); // assuming the column is named 'Vendor' and stores the code

                    if (existsInDb)
                    {
                        result.FailedRecords.Add((item, "Duplicate in database"));
                        continue;
                    }

                    // 5) Passed all checks — add to DB using vendorCode
                    var entity = new Kaizen_Tracker
                    {
                        Vendor = vendorCode,           // <-- store CODE here
                        Kaizen_Theme = item.Kaizen_Theme,
                        Month = item.Month?.Trim(),
                        Team = item.Team?.Trim(),
                        Kaizen_Attch = item.Kaizen_Attch?.Trim(),
                        Remark = item.Remark?.Trim(),
                        FY = item.FY?.Trim(),
                        CreatedBy = string.IsNullOrWhiteSpace(item.CreatedBy) ? uploadedBy : item.CreatedBy,
                        CreatedDate = item.CreatedDate == default ? DateTime.Now : item.CreatedDate
                    };

                    _dbContext.Kaizen_Tracker.Add(entity);
                }

                await _dbContext.SaveChangesAsync();

                // 6) Log the upload
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
