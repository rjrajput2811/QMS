using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.RMTCDetailsRepository;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.CSATCommentRepository
{
    public class CSATCommentRepository : SqlTableRepository, ICSATCommentRepository
    {
        private readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public CSATCommentRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<Csat_CommentViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.CSAT_Comment
                    .FromSqlRaw("EXEC sp_Get_CSAT_Comments")
                    .ToListAsync();

                // Apply optional date filtering based on RMTCDate
                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                return result.Select(data => new Csat_CommentViewModel
                {
                    Id = data.Id,
                    Quarter = data.Quarter,
                    Organisation = data.Organisation,
                    Region = data.Region,
                    Q1 = data.Q1,
                    Q2 = data.Q2,
                    Q3 = data.Q3,
                    Q4 = data.Q4,
                    Q5 = data.Q5,
                    Q6 = data.Q6,
                    Q7 = data.Q7,
                    Q8 = data.Q8,
                    Q9 = data.Q9,
                    Q10 = data.Q10,
                    Q11 = data.Q11,
                    Q12 = data.Q12,
                    Q13 = data.Q13,
                    Cust_Critical_Aspect = data.Cust_Critical_Aspect,
                    Comment = data.Comment,
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

        public async Task<Csat_CommentViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Csat_Id", id),
                };

                var sql = @"EXEC sp_Get_CSAT_Comment_ById @Csat_Id";

                var result = await _dbContext.CSAT_Comment.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new Csat_CommentViewModel
                {
                    Id = data.Id,
                    Quarter = data.Quarter,
                    Organisation = data.Organisation,
                    Region = data.Region,
                    Q1 = data.Q1,
                    Q2 = data.Q2,
                    Q3 = data.Q3,
                    Q4 = data.Q4,
                    Q5 = data.Q5,
                    Q6 = data.Q6,
                    Q7 = data.Q7,
                    Q8 = data.Q8,
                    Q9 = data.Q9,
                    Q10 = data.Q10,
                    Q11 = data.Q11,
                    Q12 = data.Q12,
                    Q13 = data.Q13,
                    Cust_Critical_Aspect = data.Cust_Critical_Aspect,
                    Comment = data.Comment,
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

        public async Task<OperationResult> CreateAsync(CSAT_Comment entity, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Quarter", entity.Quarter ?? (object)DBNull.Value),
                    new SqlParameter("@Organisation", entity.Organisation ?? (object)DBNull.Value),
                    new SqlParameter("@Region", entity.Region ?? (object)DBNull.Value),
                    new SqlParameter("@Q1", entity.Q1 ?? (object)DBNull.Value),
                    new SqlParameter("@Q2", entity.Q2 ?? (object)DBNull.Value),
                    new SqlParameter("@Q3", entity.Q3 ?? (object)DBNull.Value),
                    new SqlParameter("@Q4", entity.Q4 ?? (object)DBNull.Value),
                    new SqlParameter("@Q5", entity.Q5 ?? (object)DBNull.Value),
                    new SqlParameter("@Q6", entity.Q6 ?? (object)DBNull.Value),
                    new SqlParameter("@Q7", entity.Q7 ?? (object)DBNull.Value),
                    new SqlParameter("@Q8", entity.Q8 ?? (object)DBNull.Value),
                    new SqlParameter("@Q9", entity.Q9 ?? (object)DBNull.Value),
                    new SqlParameter("@Q10", entity.Q10 ?? (object)DBNull.Value),
                    new SqlParameter("@Q11", entity.Q11 ?? (object)DBNull.Value),
                    new SqlParameter("@Q12", entity.Q12 ?? (object)DBNull.Value),
                    new SqlParameter("@Q13", entity.Q13 ?? (object)DBNull.Value),
                    new SqlParameter("@Cust_Critical_Aspect", entity.Cust_Critical_Aspect ?? (object)DBNull.Value),
                    new SqlParameter("@Comment", entity.Comment ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", entity.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_CSAT_Comment " +
                    "@Quarter, @Organisation, @Region, @Q1, @Q2, @Q3, " +
                    "@Q4, @Q5, @Q6, @Q7, @Q8, @Q9, " +
                    "@Q10, @Q11, @Q12, @Q13,@Cust_Critical_Aspect, @Comment, @CreatedBy, @IsDeleted",
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

        public async Task<OperationResult> UpdateAsync(CSAT_Comment entity, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Csat_Id", entity.Id),
                    new SqlParameter("@Quarter", entity.Quarter ?? (object)DBNull.Value),
                    new SqlParameter("@Organisation", entity.Organisation ?? (object)DBNull.Value),
                    new SqlParameter("@Region", entity.Region ?? (object)DBNull.Value),
                    new SqlParameter("@Q1", entity.Q1 ?? (object)DBNull.Value),
                    new SqlParameter("@Q2", entity.Q2 ?? (object)DBNull.Value),
                    new SqlParameter("@Q3", entity.Q3 ?? (object)DBNull.Value),
                    new SqlParameter("@Q4", entity.Q4 ?? (object)DBNull.Value),
                    new SqlParameter("@Q5", entity.Q5 ?? (object)DBNull.Value),
                    new SqlParameter("@Q6", entity.Q6 ?? (object)DBNull.Value),
                    new SqlParameter("@Q7", entity.Q7 ?? (object)DBNull.Value),
                    new SqlParameter("@Q8", entity.Q8 ?? (object)DBNull.Value),
                    new SqlParameter("@Q9", entity.Q9 ?? (object)DBNull.Value),
                    new SqlParameter("@Q10", entity.Q10 ?? (object)DBNull.Value),
                    new SqlParameter("@Q11", entity.Q11 ?? (object)DBNull.Value),
                    new SqlParameter("@Q12", entity.Q12 ?? (object)DBNull.Value),
                    new SqlParameter("@Q13", entity.Q13 ?? (object)DBNull.Value),
                    new SqlParameter("@Cust_Critical_Aspect", entity.Cust_Critical_Aspect ?? (object)DBNull.Value),
                    new SqlParameter("@Comment", entity.Comment ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", entity.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Update_CSAT_Comment " +
                    "@Csat_Id, @Quarter, @Organisation, @Region, @Q1, @Q2, @Q3, " +
                    "@Q4, @Q5, @Q6, @Q7, @Q8, @Q9, " +
                    "@Q10, @Q11, @Q12, @Q13,@Cust_Critical_Aspect, @Comment, @UpdatedBy, @IsDeleted",
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

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<CSAT_Comment>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

    }
}
