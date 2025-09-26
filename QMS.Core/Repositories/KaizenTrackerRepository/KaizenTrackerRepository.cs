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
                    new SqlParameter("@CreatedBy", kaizen.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", kaizen.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_KaizenTracker @Vendor, @Kaizen_Theme, @Month, @Team, @Kaizen_Attch, @Remark, @FY, @CreatedBy, @IsDeleted",
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
                    new SqlParameter("@UpdatedBy", kaizen.UpdatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", kaizen.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Update_KaizenTracker @Kaizen_Id, @Vendor, @Kaizen_Theme, @Month, @Team, @Kaizen_Attch, @Remark, @FY, @UpdatedBy, @IsDeleted",parameters);

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
    }
}
