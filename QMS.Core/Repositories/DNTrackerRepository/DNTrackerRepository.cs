using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QMS.Core.Repositories.DNTrackerRepository
{
    public class DNTrackerRepository : SqlTableRepository, IDNTrackerRepository
    {
        private readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _log;

        public DNTrackerRepository(QMSDbContext dbContext, ISystemLogService log)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _log = log;
        }

        public async Task<List<DNTrackerViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.DeviationNote
                    .FromSqlRaw("EXEC sp_Get_DN_Tracker")
                    .ToListAsync();
                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

               
                return result.Select(x => new DNTrackerViewModel
                {
                    Id = x.Id,
                    DNoteNumber = x.DNoteNumber,
                    DNoteCategory = x.DNoteCategory,
                    ProductCode = x.ProductCode,
                    ProdDesc = x.ProdDesc,
                    Wattage = x.Wattage,
                    DQty = x.DQty,
                    DRequisitionBy = x.DRequisitionBy,
                    Vendor = x.Vendor,
                    Remark = x.Remark,
                    IsDeleted = x.Deleted,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate
                }).ToList();
               
            
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<DNTracker?> GetByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.DeviationNote
                    .FromSqlRaw("EXEC sp_Get_DN_Tracker_ByID @DNoteId", new SqlParameter("@DNoteId", id))
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateAsync(DNTracker entity)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@DNoteNumber", entity.DNoteNumber ?? (object)DBNull.Value),
            new SqlParameter("@DNoteCategory", entity.DNoteCategory ?? (object)DBNull.Value),
            new SqlParameter("@ProductCode", entity.ProductCode ?? (object)DBNull.Value),
            new SqlParameter("@ProdDesc", entity.ProdDesc ?? (object)DBNull.Value),
            new SqlParameter("@Wattage", entity.Wattage ?? (object)DBNull.Value),
            new SqlParameter("@DQty", entity.DQty ?? (object)DBNull.Value),
            new SqlParameter("@DRequisitionBy", entity.DRequisitionBy ?? (object)DBNull.Value),
            new SqlParameter("@Vendor", entity.Vendor ?? (object)DBNull.Value),
            new SqlParameter("@Remark", entity.Remark ?? (object)DBNull.Value),
            new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value),
            new SqlParameter("@CreatedDate", entity.CreatedDate),
            new SqlParameter("@IsDeleted", entity.Deleted)
        };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_DN_Tracker @DNoteNumber, @DNoteCategory, @ProductCode, @ProdDesc, @Wattage, @DQty, @DRequisitionBy, @Vendor, @Remark, @CreatedBy, @CreatedDate, @IsDeleted",
                    parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateAsync(DNTracker entity)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@DNoteId", entity.Id),
                    new SqlParameter("@DNoteNumber", entity.DNoteNumber ?? (object)DBNull.Value),
                    new SqlParameter("@DNoteCategory", entity.DNoteCategory ?? (object)DBNull.Value),
                    new SqlParameter("@ProductCode", entity.ProductCode ?? (object)DBNull.Value),
                    new SqlParameter("@ProdDesc", entity.ProdDesc ?? (object)DBNull.Value),
                    new SqlParameter("@Wattage", entity.Wattage ?? (object)DBNull.Value),
                    new SqlParameter("@DQty", entity.DQty ?? (object)DBNull.Value),
                    new SqlParameter("@DRequisitionBy", entity.DRequisitionBy ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor", entity.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", entity.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value),
            new SqlParameter("@UpdatedDate", entity.UpdatedDate)
                };

                await _dbContext.Database.ExecuteSqlRawAsync("EXEC sp_Update_DN_Tracker @DNoteId, @DNoteNumber, @DNoteCategory, @ProductCode, @ProdDesc, @Wattage, @DQty, @DRequisitionBy, @Vendor, @Remark,@UpdatedBy,@UpdatedDate", parameters);
                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<DNTracker>(id);
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAttachmentAsync(int id, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                var record = await _dbContext.DeviationNote.FindAsync(id);
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
                _log.WriteLog(ex.Message);
                throw;
            }
        }

    }
}
