﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.FIFOTrackerRepository
{
    public class FIFOTrackerRepository : SqlTableRepository, IFIFOTrackerRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public FIFOTrackerRepository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<FIFOTrackerViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.FIFOTracker.FromSqlRaw("EXEC sp_Get_FifoTrac").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.Sample_Recv_Date.HasValue &&
                                    x.Sample_Recv_Date.Value.Date >= startDate.Value.Date &&
                                    x.Sample_Recv_Date.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map results to ViewModel
                var viewModelList = result.Select(data => new FIFOTrackerViewModel
                {
                    Id = data.Id,
                    Sample_Recv_Date = data.Sample_Recv_Date,
                    Sample_Cat_Ref = data.Sample_Cat_Ref,
                    Sample_Desc = data.Sample_Desc,
                    Vendor = data.Vendor,
                    Sample_Qty = data.Sample_Qty,
                    Test_Req = data.Test_Req,
                    Test_Status = data.Test_Status,
                    Responsbility = data.Responsbility,
                    Test_Completion_Date = data.Test_Completion_Date,
                    Report_Release_Date = data.Report_Release_Date,
                    NABL_Released_Date = data.NABL_Released_Date,
                    Final_Report = data.Final_Report,
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

        public async Task<OperationResult> CreateAsync(FIFOTracker newRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Sample_Recv_Date", newRecord.Sample_Recv_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Cat_Ref", newRecord.Sample_Cat_Ref ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Desc", newRecord.Sample_Desc ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor", newRecord.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Qty", newRecord.Sample_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Req", newRecord.Test_Req ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Status", newRecord.Test_Status ?? (object)DBNull.Value),
                    new SqlParameter("@Responsbility", newRecord.Responsbility ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Completion_Date", newRecord.Test_Completion_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Report_Release_Date", newRecord.Report_Release_Date ?? (object)DBNull.Value),
                    new SqlParameter("@NABL_Released_Date", newRecord.NABL_Released_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Final_Report", newRecord.Final_Report ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", newRecord.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", newRecord.Deleted),
                };

                var sql = @"EXEC sp_Insert_FifoTrac @Sample_Recv_Date,@Sample_Cat_Ref,@Sample_Desc,@Vendor,@Sample_Qty,@Test_Req,@Test_Status,@Responsbility,@Test_Completion_Date,
                        @Report_Release_Date,@NABL_Released_Date,@Final_Report,@CreatedBy,@IsDeleted";

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

        public async Task<OperationResult> UpdateAsync(FIFOTracker updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@FifoTrac_Id", updatedRecord.Id),
                    new SqlParameter("@Sample_Recv_Date", updatedRecord.Sample_Recv_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Cat_Ref", updatedRecord.Sample_Cat_Ref ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Desc", updatedRecord.Sample_Desc ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor", updatedRecord.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Qty", updatedRecord.Sample_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Req", updatedRecord.Test_Req ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Status", updatedRecord.Test_Status ?? (object)DBNull.Value),
                    new SqlParameter("@Responsbility", updatedRecord.Responsbility ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Completion_Date", updatedRecord.Test_Completion_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Report_Release_Date", updatedRecord.Report_Release_Date ?? (object)DBNull.Value),
                    new SqlParameter("@NABL_Released_Date", updatedRecord.NABL_Released_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Final_Report", updatedRecord.Final_Report ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", updatedRecord.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", updatedRecord.Deleted),
                };

                var sql = @"EXEC sp_Update_FifoTrac @FifoTrac_Id,@Sample_Recv_Date,@Sample_Cat_Ref,@Sample_Desc,@Vendor,@Sample_Qty,@Test_Req,@Test_Status,@Responsbility,@Test_Completion_Date,
                        @Report_Release_Date,@NABL_Released_Date,@Final_Report,@UpdatedBy,@IsDeleted";

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
                return await base.DeleteAsync<FIFOTracker>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<FIFOTrackerViewModel?> GetByIdAsync(int ven_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@FifoTrac_Id", ven_Id),
                };

                var sql = @"EXEC sp_Get_FifoTrac_ById @FifoTrac_Id";

                var result = await _dbContext.FIFOTracker.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new FIFOTrackerViewModel
                {
                    Id = data.Id,
                    Sample_Recv_Date = data.Sample_Recv_Date,
                    Sample_Cat_Ref = data.Sample_Cat_Ref,
                    Sample_Desc = data.Sample_Desc,
                    Vendor = data.Vendor,
                    Sample_Qty = data.Sample_Qty,
                    Test_Req = data.Test_Req,
                    Test_Status = data.Test_Status,
                    Responsbility = data.Responsbility,
                    Test_Completion_Date = data.Test_Completion_Date,
                    Report_Release_Date = data.Report_Release_Date,
                    NABL_Released_Date = data.NABL_Released_Date,
                    Final_Report = data.Final_Report,
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

        public async Task<bool> CheckDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.FIFOTracker
                    .Where(x => x.Deleted == false && x.Sample_Cat_Ref.ToString() == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.FIFOTracker
                        .Where(x => x.Deleted == false &&
                               x.Sample_Cat_Ref.ToString() == searchText
                               && x.Id != Id)
                        .Select(x => x.Id);
                }


                existingId = await query.FirstOrDefaultAsync();

                if (existingId != null && existingId > 0)
                {
                    existingflag = true;
                }

                return existingflag;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
