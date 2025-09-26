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

namespace QMS.Core.Repositories.PDIAuthSignRepository
{
    public class PDIAuthSignRepository : SqlTableRepository, IPDIAuthSignRepository
    {
        private readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public PDIAuthSignRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<PDI_Auth_SignViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.PDI_Auth_Signatory.FromSqlRaw("EXEC sp_Get_PdiAuthSign").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                return result.Select(data => new PDI_Auth_SignViewModel
                {
                    Id = data.Id,
                    Vendor = data.Vendor,
                    Address = data.Address,
                    Pdi_Inspector = data.Pdi_Inspector,
                    Designation = data.Designation,
                    Photo_Inspector = data.Photo_Inspector,
                    Specimen_Sign = data.Specimen_Sign,
                    Remark = data.Remark,
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

        public async Task<PDI_Auth_SignViewModel?> GetByIdAsync(int ven_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Pdi_Auth_Id", ven_Id),
                };

                var sql = @"EXEC sp_Get_PdiAuth_ById @Pdi_Auth_Id";

                var result = await _dbContext.PDI_Auth_Signatory.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new PDI_Auth_SignViewModel
                {
                    Id = data.Id,
                    Vendor = data.Vendor,
                    Address = data.Address,
                    Pdi_Inspector = data.Pdi_Inspector,
                    Designation = data.Designation,
                    Photo_Inspector = data.Photo_Inspector,
                    Specimen_Sign = data.Specimen_Sign,
                    Remark = data.Remark,
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

        public async Task<OperationResult> CreateAsync(PDI_Auth_Signatory pdi_Auth, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Vendor", pdi_Auth.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Address", pdi_Auth.Address ?? (object)DBNull.Value),
                    new SqlParameter("@Pdi_Inspector", pdi_Auth.Pdi_Inspector ?? (object)DBNull.Value),
                    new SqlParameter("@Designation", pdi_Auth.Designation ?? (object)DBNull.Value),
                    new SqlParameter("@Photo_Inspector", pdi_Auth.Photo_Inspector ?? (object)DBNull.Value),
                    new SqlParameter("@Specimen_Sign", pdi_Auth.Specimen_Sign ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", pdi_Auth.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", pdi_Auth.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", pdi_Auth.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_PdiAuth @Vendor, @Address, @Pdi_Inspector, @Designation, @Photo_Inspector, @Specimen_Sign, @Remark, @CreatedBy, @IsDeleted",
                    parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateAsync(PDI_Auth_Signatory pdi_Auth, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Pdi_Auth_Id", pdi_Auth.Id),
                    new SqlParameter("@Vendor", pdi_Auth.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Address", pdi_Auth.Address ?? (object)DBNull.Value),
                    new SqlParameter("@Pdi_Inspector", pdi_Auth.Pdi_Inspector ?? (object)DBNull.Value),
                    new SqlParameter("@Designation", pdi_Auth.Designation ?? (object)DBNull.Value),
                    new SqlParameter("@Photo_Inspector", pdi_Auth.Photo_Inspector ?? (object)DBNull.Value),
                    new SqlParameter("@Specimen_Sign", pdi_Auth.Specimen_Sign ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", pdi_Auth.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", pdi_Auth.UpdatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", pdi_Auth.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Update_PdiAuth @Pdi_Auth_Id, @Vendor, @Address, @Pdi_Inspector, @Designation, @Photo_Inspector, @Specimen_Sign, @Remark, @UpdatedBy, @IsDeleted", parameters);

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
                return await base.DeleteAsync<PDI_Auth_Signatory>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAttachmentAsync(int id, string fileName, string type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                var record = await _dbContext.PDI_Auth_Signatory.FindAsync(id);
                if (record == null)
                    return false;
                if (type == "Photo")
                {
                    record.Photo_Inspector = fileName;
                    _dbContext.Entry(record).Property(x => x.Photo_Inspector).IsModified = true;
                }
                else
                {
                    record.Specimen_Sign = fileName;
                    _dbContext.Entry(record).Property(x => x.Specimen_Sign).IsModified = true;
                }

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
