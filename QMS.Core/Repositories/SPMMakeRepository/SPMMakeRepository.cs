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
using static System.Net.Mime.MediaTypeNames;

namespace QMS.Core.Repositories.SPMMakeRepository
{
    public class SPMMakeRepository : SqlTableRepository, ISPMMakeRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public SPMMakeRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<SPM_MakeViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.SPM_Make.FromSqlRaw("EXEC sp_Get_SPM_Make").ToListAsync();


                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map results to ViewModel
                var viewModelList = result.Select(data => new SPM_MakeViewModel
                {
                    Id = data.Id,
                    Supp_Name = data.Supp_Name,
                    Quater = data.Quater,
                    Fy = data.Fy,
                    Month = data.Month,
                    Pc = data.Pc,
                    Location = data.Location,
                    Sqa = data.Sqa,
                    Ppm = data.Ppm,
                    Delivery = data.Delivery,
                    Capa = data.Capa,
                    Audit = data.Audit,
                    Cost = data.Cost,
                    Npi_Resp = data.Npi_Resp,
                    Rep_Lead_Time = data.Rep_Lead_Time,
                    Ppm_Rating = data.Ppm_Rating,
                    Delivery_Rating = data.Delivery_Rating,
                    Capa_Rating = data.Capa_Rating,
                    Audit_Rating = data.Audit_Rating,
                    Cost_Rating = data.Cost_Rating,
                    Npi_Resp_Rating = data.Npi_Resp_Rating,
                    Rep_Lead_Time_Rating = data.Rep_Lead_Time_Rating,
                    Total = data.Total,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate,
                }).ToList();

                return viewModelList;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<SPM_Make> GetByIdAsync(int spmMakeId)
        {
            try
            {
                var parameters = new[] { new SqlParameter("@SpmMake_Id", spmMakeId) };

                var sql = @"EXEC sp_Get_SPM_Make_ById @SpmMake_Id";

                var result = await _dbContext.SPM_Make.FromSqlRaw(sql, parameters).ToListAsync();

                var viewList = result.Select(data => new SPM_Make
                {
                    Id = data.Id,
                    Supp_Name = data.Supp_Name,
                    Quater = data.Quater,
                    Fy = data.Fy,
                    Month = data.Month,
                    Pc = data.Pc,
                    Location = data.Location,
                    Sqa = data.Sqa,
                    Ppm = data.Ppm,
                    Delivery = data.Delivery,
                    Capa = data.Capa,
                    Audit = data.Audit,
                    Cost = data.Cost,
                    Npi_Resp = data.Npi_Resp,
                    Rep_Lead_Time = data.Rep_Lead_Time,
                    Ppm_Rating = data.Ppm_Rating,
                    Delivery_Rating = data.Delivery_Rating,
                    Capa_Rating = data.Capa_Rating,
                    Audit_Rating = data.Audit_Rating,
                    Cost_Rating = data.Cost_Rating,
                    Npi_Resp_Rating = data.Npi_Resp_Rating,
                    Rep_Lead_Time_Rating = data.Rep_Lead_Time_Rating,
                    Total = data.Total,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate,
                }).ToList();

                return viewList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateAsync(SPM_Make newRecord, bool returnCreatedRecord = false)
        {
            var operationResult = new OperationResult();

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@IsDeleted", newRecord.Deleted),
                    new SqlParameter("@Supp_Name", newRecord.Supp_Name ?? (object)DBNull.Value),
                    new SqlParameter("@Quater", newRecord.Quater ?? (object)DBNull.Value),
                    new SqlParameter("@Fy", newRecord.Fy ?? (object)DBNull.Value),
                    new SqlParameter("@Month", newRecord.Month ?? (object)DBNull.Value),
                    new SqlParameter("@Pc", newRecord.Pc ?? (object)DBNull.Value),
                    new SqlParameter("@Location", newRecord.Location ?? (object)DBNull.Value),
                    new SqlParameter("@Sqa", newRecord.Sqa ?? (object)DBNull.Value),
                    new SqlParameter("@Ppm", newRecord.Ppm ?? (object)DBNull.Value),
                    new SqlParameter("@Delivery", newRecord.Delivery ?? (object)DBNull.Value),
                    new SqlParameter("@Capa", newRecord.Capa ?? (object)DBNull.Value),
                    new SqlParameter("@Audit", newRecord.Audit ?? (object)DBNull.Value),
                    new SqlParameter("@Cost", newRecord.Cost ?? (object)DBNull.Value),
                    new SqlParameter("@Npi_Resp", newRecord.Npi_Resp ?? (object)DBNull.Value),
                    new SqlParameter("@Rep_Lead_Time", newRecord.Rep_Lead_Time ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", newRecord.CreatedBy ?? (object)DBNull.Value),
                };


                var sql = @"EXEC sp_SPM_Make_Insert @IsDeleted,@Supp_Name,@Quater,@Fy,@Month,@Pc,@Location,@Sqa,
                            @Ppm,@Delivery,@Capa,@Audit,@Cost,@Npi_Resp,@Rep_Lead_Time,@CreatedBy";

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

        public async Task<OperationResult> UpdateAsync(SPM_Make updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@SpmMake_Id", updatedRecord.Id),
                    new SqlParameter("@IsDeleted", updatedRecord.Deleted),
                    new SqlParameter("@Supp_Name", updatedRecord.Supp_Name ?? (object)DBNull.Value),
                    new SqlParameter("@Quater", updatedRecord.Quater ?? (object)DBNull.Value),
                    new SqlParameter("@Fy", updatedRecord.Fy ?? (object)DBNull.Value),
                    new SqlParameter("@Month", updatedRecord.Month ?? (object)DBNull.Value),
                    new SqlParameter("@Pc", updatedRecord.Pc ?? (object)DBNull.Value),
                    new SqlParameter("@Location", updatedRecord.Location ?? (object)DBNull.Value),
                    new SqlParameter("@Sqa", updatedRecord.Sqa ?? (object)DBNull.Value),
                    new SqlParameter("@Ppm", updatedRecord.Ppm ?? (object)DBNull.Value),
                    new SqlParameter("@Delivery", updatedRecord.Delivery ?? (object)DBNull.Value),
                    new SqlParameter("@Capa", updatedRecord.Capa ?? (object)DBNull.Value),
                    new SqlParameter("@Audit", updatedRecord.Audit ?? (object)DBNull.Value),
                    new SqlParameter("@Cost", updatedRecord.Cost ?? (object)DBNull.Value),
                    new SqlParameter("@Npi_Resp", updatedRecord.Npi_Resp ?? (object)DBNull.Value),
                    new SqlParameter("@Rep_Lead_Time", updatedRecord.Rep_Lead_Time ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value),
                };

                var sql = @"EXEC sp_SPM_Make_Update @SpmMake_Id,@IsDeleted,@Supp_Name,@Quater,@Fy,@Month,@Pc,@Location,@Sqa,
                            @Ppm,@Delivery,@Capa,@Audit,@Cost,@Npi_Resp,@Rep_Lead_Time, @UpdatedBy";

                //await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                if (returnUpdatedRecord)
                {
                    var rows = await _dbContext.Set<SPM_Make>()
                                    .FromSqlRaw(sql, parameters)
                                    .AsNoTracking()
                                    .ToListAsync();

                    var row = rows.FirstOrDefault();

                    return new OperationResult
                    {
                        Success = row != null,
                        Payload = row
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
                return await base.DeleteAsync<ThirdPartyInspection>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckDuplicate(string sup_Name,string qtr, int spmId)
        {
            try
            {
                bool existingFlag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.SPM_Make
                    .Where(x => x.Deleted == false && x.Supp_Name == sup_Name && x.Quater == qtr)
                    .Select(x => x.Id);

                if (spmId != 0)
                {
                    query = _dbContext.SPM_Make
                        .Where(x => x.Deleted == false && x.Supp_Name == sup_Name && x.Quater == qtr && x.Id != spmId)
                        .Select(x => x.Id);
                }

                existingId = await query.FirstOrDefaultAsync();

                if (existingId != null && existingId > 0)
                {
                    existingFlag = true;
                }

                return existingFlag;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
