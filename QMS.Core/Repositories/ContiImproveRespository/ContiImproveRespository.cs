using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.FIFOTrackerRepository;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.ContiImproveRespository
{
    public class ContiImproveRespository : SqlTableRepository, IContiImproveRespository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public ContiImproveRespository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<ContiImproveViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.Continual_Improves.FromSqlRaw("EXEC sp_Get_ContiImprove").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.Date.HasValue &&
                                    x.Date.Value.Date >= startDate.Value.Date &&
                                    x.Date.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map results to ViewModel
                var viewModelList = result.Select(data => new ContiImproveViewModel
                {
                    Id = data.Id,
                    Date = data.Date,
                    Conti_Improve_Plane = data.Conti_Improve_Plane,
                    Iso_9001 = data.Iso_9001,
                    Iso9001_Plane_Implement = data.Iso9001_Plane_Implement,
                    Iso_14001 = data.Iso_14001,
                    Iso14001_Plane_Implement = data.Iso14001_Plane_Implement,
                    Iso_45001 = data.Iso_45001,
                    Iso_45001_Plane_Implement = data.Iso_45001_Plane_Implement,
                    FY = data.FY,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
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

        public async Task<OperationResult> CreateAsync(Continual_Improve_Tracker newRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Date", newRecord.Date ?? (object)DBNull.Value),
                    new SqlParameter("@Conti_Improve_Plane", newRecord.Conti_Improve_Plane ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_9001", newRecord.Iso_9001 ?? (object)DBNull.Value),
                    new SqlParameter("@Iso9001_Plane_Implement", newRecord.Iso9001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_14001", newRecord.Iso14001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@Iso14001_Plane_Implement", newRecord.Iso14001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_45001", newRecord.Iso_45001 ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_45001_Plane_Implement", newRecord.Iso_45001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@FY", newRecord.FY ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", newRecord.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", newRecord.Deleted),
                };

                var sql = @"EXEC sp_Insert_Conti_Improve @Date,@Conti_Improve_Plane,@Iso_9001,@Iso9001_Plane_Implement,@Iso_14001,@Iso14001_Plane_Implement,@Iso_45001,@Iso_45001_Plane_Implement,
                                @FY,@CreatedBy,@IsDeleted";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                if (returnCreatedRecord)
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

        public async Task<OperationResult> UpdateAsync(Continual_Improve_Tracker updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Conti_Improve_Id", updatedRecord.Id),
                    new SqlParameter("@Date", updatedRecord.Date ?? (object)DBNull.Value),
                    new SqlParameter("@Conti_Improve_Plane", updatedRecord.Conti_Improve_Plane ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_9001", updatedRecord.Iso_9001 ?? (object)DBNull.Value),
                    new SqlParameter("@Iso9001_Plane_Implement", updatedRecord.Iso9001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_14001", updatedRecord.Iso14001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@Iso14001_Plane_Implement", updatedRecord.Iso14001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_45001", updatedRecord.Iso_45001 ?? (object)DBNull.Value),
                    new SqlParameter("@Iso_45001_Plane_Implement", updatedRecord.Iso_45001_Plane_Implement ?? (object)DBNull.Value),
                    new SqlParameter("@FY", updatedRecord.FY ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value)
                };

                var sql = @"EXEC sp_Update_ContiImprove @Conti_Improve_Id,@Date,@Conti_Improve_Plane,@Iso_9001,@Iso9001_Plane_Implement,@Iso_14001,@Iso14001_Plane_Implement,@Iso_45001,@Iso_45001_Plane_Implement,
                                @FY,@UpdatedBy";

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

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<Continual_Improve_Tracker>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<ContiImproveViewModel?> GetByIdAsync(int ven_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Conti_Improve_Id", ven_Id),
                };

                var sql = @"EXEC sp_Get_ContiImprove_GetById @Conti_Improve_Id";

                var result = await _dbContext.Continual_Improves.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new ContiImproveViewModel
                {
                    Id = data.Id,
                    Date = data.Date,
                    Conti_Improve_Plane = data.Conti_Improve_Plane,
                    Iso_9001 = data.Iso_9001,
                    Iso9001_Plane_Implement = data.Iso9001_Plane_Implement,
                    Iso_14001 = data.Iso_14001,
                    Iso14001_Plane_Implement = data.Iso14001_Plane_Implement,
                    Iso_45001 = data.Iso_45001,
                    Iso_45001_Plane_Implement = data.Iso_45001_Plane_Implement,
                    FY = data.FY,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate

                }).ToList();

                return viewModelList.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

    }
}
