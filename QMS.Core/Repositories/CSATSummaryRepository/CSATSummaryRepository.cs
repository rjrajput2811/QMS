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

namespace QMS.Core.Repositories.CSATSummaryRepository
{
    public class CSATSummaryRepository : SqlTableRepository, ICSATSummaryRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public CSATSummaryRepository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }
        public async Task<List<Csat_SummaryViewModel>> GetSummaryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var rows = await _dbContext.CSAT_Summary
                    .FromSqlRaw("EXEC sp_Get_CSAT_Summary")
                    .ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    rows = rows
                        .Where(data => data.CreatedDate.HasValue
                                    && data.CreatedDate.Value.Date >= startDate.Value.Date
                                    && data.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map to your ViewModel (1:1 property mapping)
                var list = rows.Select(data => new Csat_SummaryViewModel
                {
                    Id = data.Id,
                    Deleted = data.Deleted,

                    Q1Pc1_ReqSent = data.Q1Pc1_ReqSent,
                    Q1Pc1_ResRece = data.Q1Pc1_ResRece,
                    Q1Pc1_Promoter = data.Q1Pc1_Promoter,
                    Q1Pc1_Collection = data.Q1Pc1_Collection,
                    Q1Pc1_Detractor = data.Q1Pc1_Detractor,
                    Q1Pc1_Nps = data.Q1Pc1_Nps,
                    Q1Pc1_Detractor_Detail = data.Q1Pc1_Detractor_Detail,

                    Q1Pc2_ReqSent = data.Q1Pc2_ReqSent,
                    Q1Pc2_ResRece = data.Q1Pc2_ResRece,
                    Q1Pc2_Promoter = data.Q1Pc2_Promoter,
                    Q1Pc2_Collection = data.Q1Pc2_Collection,
                    Q1Pc2_Detractor = data.Q1Pc2_Detractor,
                    Q1Pc2_Nps = data.Q1Pc2_Nps,
                    Q1Pc2_Detractor_Detail = data.Q1Pc2_Detractor_Detail,

                    Q1Pc3_ReqSent = data.Q1Pc3_ReqSent,
                    Q1Pc3_ResRece = data.Q1Pc3_ResRece,
                    Q1Pc3_Promoter = data.Q1Pc3_Promoter,
                    Q1Pc3_Collection = data.Q1Pc3_Collection,
                    Q1Pc3_Detractor = data.Q1Pc3_Detractor,
                    Q1Pc3_Nps = data.Q1Pc3_Nps,
                    Q1Pc3_Detractor_Detail = data.Q1Pc3_Detractor_Detail,

                    Q1Q1_ReqSent = data.Q1Q1_ReqSent,
                    Q1Q1_ResRece = data.Q1Q1_ResRece,
                    Q1Q1_Promoter = data.Q1Q1_Promoter,
                    Q1Q1_Collection = data.Q1Q1_Collection,
                    Q1Q1_Detractor = data.Q1Q1_Detractor,
                    Q1Q1_Nps = data.Q1Q1_Nps,
                    Q1Q1_Detractor_Detail = data.Q1Q1_Detractor_Detail,

                    Q2Pc4_ReqSent = data.Q2Pc4_ReqSent,
                    Q2Pc4_ResRece = data.Q2Pc4_ResRece,
                    Q2Pc4_Promoter = data.Q2Pc4_Promoter,
                    Q2Pc4_Collection = data.Q2Pc4_Collection,
                    Q2Pc4_Detractor = data.Q2Pc4_Detractor,
                    Q2Pc4_Nps = data.Q2Pc4_Nps,
                    Q2Pc4_Detractor_Detail = data.Q2Pc4_Detractor_Detail,

                    Q2Pc5_ReqSent = data.Q2Pc5_ReqSent,
                    Q2Pc5_ResRece = data.Q2Pc5_ResRece,
                    Q2Pc5_Promoter = data.Q2Pc5_Promoter,
                    Q2Pc5_Collection = data.Q2Pc5_Collection,
                    Q2Pc5_Detractor = data.Q2Pc5_Detractor,
                    Q2Pc5_Nps = data.Q2Pc5_Nps,
                    Q2Pc5_Detractor_Detail = data.Q2Pc5_Detractor_Detail,

                    Q2Pc6_ReqSent = data.Q2Pc6_ReqSent,
                    Q2Pc6_ResRece = data.Q2Pc6_ResRece,
                    Q2Pc6_Promoter = data.Q2Pc6_Promoter,
                    Q2Pc6_Collection = data.Q2Pc6_Collection,
                    Q2Pc6_Detractor = data.Q2Pc6_Detractor,
                    Q2Pc6_Nps = data.Q2Pc6_Nps,
                    Q2Pc6_Detractor_Detail = data.Q2Pc6_Detractor_Detail,

                    Q2Q2_ReqSent = data.Q2Q2_ReqSent,
                    Q2Q2_ResRece = data.Q2Q2_ResRece,
                    Q2Q2_Promoter = data.Q2Q2_Promoter,
                    Q2Q2_Collection = data.Q2Q2_Collection,
                    Q2Q2_Detractor = data.Q2Q2_Detractor,
                    Q2Q2_Nps = data.Q2Q2_Nps,
                    Q2Q2_Detractor_Detail = data.Q2Q2_Detractor_Detail,

                    Q3Pc7_ReqSent = data.Q3Pc7_ReqSent,
                    Q3Pc7_ResRece = data.Q3Pc7_ResRece,
                    Q3Pc7_Promoter = data.Q3Pc7_Promoter,
                    Q3Pc7_Collection = data.Q3Pc7_Collection,
                    Q3Pc7_Detractor = data.Q3Pc7_Detractor,
                    Q3Pc7_Nps = data.Q3Pc7_Nps,
                    Q3Pc7_Detractor_Detail = data.Q3Pc7_Detractor_Detail,

                    Q3Pc8_ReqSent = data.Q3Pc8_ReqSent,
                    Q3Pc8_ResRece = data.Q3Pc8_ResRece,
                    Q3Pc8_Promoter = data.Q3Pc8_Promoter,
                    Q3Pc8_Collection = data.Q3Pc8_Collection,
                    Q3Pc8_Detractor = data.Q3Pc8_Detractor,
                    Q3Pc8_Nps = data.Q3Pc8_Nps,
                    Q3Pc8_Detractor_Detail = data.Q3Pc8_Detractor_Detail,

                    Q3Pc9_ReqSent = data.Q3Pc9_ReqSent,
                    Q3Pc9_ResRece = data.Q3Pc9_ResRece,
                    Q3Pc9_Promoter = data.Q3Pc9_Promoter,
                    Q3Pc9_Collection = data.Q3Pc9_Collection,
                    Q3Pc9_Detractor = data.Q3Pc9_Detractor,
                    Q3Pc9_Nps = data.Q3Pc9_Nps,
                    Q3Pc9_Detractor_Detail = data.Q3Pc9_Detractor_Detail,

                    Q3Q3_ReqSent = data.Q3Q3_ReqSent,
                    Q3Q3_ResRece = data.Q3Q3_ResRece,
                    Q3Q3_Promoter = data.Q3Q3_Promoter,
                    Q3Q3_Collection = data.Q3Q3_Collection,
                    Q3Q3_Detractor = data.Q3Q3_Detractor,
                    Q3Q3_Nps = data.Q3Q3_Nps,
                    Q3Q3_Detractor_Detail = data.Q3Q3_Detractor_Detail,

                    Q4Pc10_ReqSent = data.Q4Pc10_ReqSent,
                    Q4Pc10_ResRece = data.Q4Pc10_ResRece,
                    Q4Pc10_Promoter = data.Q4Pc10_Promoter,
                    Q4Pc10_Collection = data.Q4Pc10_Collection,
                    Q4Pc10_Detractor = data.Q4Pc10_Detractor,
                    Q4Pc10_Nps = data.Q4Pc10_Nps,
                    Q4Pc10_Detractor_Detail = data.Q4Pc10_Detractor_Detail,

                    Q4Pc11_ReqSent1 = data.Q4Pc11_ReqSent1,
                    Q4Pc11_ResRece1 = data.Q4Pc11_ResRece1,
                    Q4Pc11_Promoter1 = data.Q4Pc11_Promoter1,
                    Q4Pc11_Collection1 = data.Q4Pc11_Collection1,
                    Q4Pc11_Detractor1 = data.Q4Pc11_Detractor1,
                    Q4Pc11_Nps1 = data.Q4Pc11_Nps1,
                    Q4Pc11_Detractor_Detail1 = data.Q4Pc11_Detractor_Detail1,

                    Q4Pc12_ReqSent1 = data.Q4Pc12_ReqSent1,
                    Q4Pc12_ResRece1 = data.Q4Pc12_ResRece1,
                    Q4Pc12_Promoter1 = data.Q4Pc12_Promoter1,
                    Q4Pc12_Collection1 = data.Q4Pc12_Collection1,
                    Q4Pc12_Detractor1 = data.Q4Pc12_Detractor1,
                    Q4Pc12_Nps1 = data.Q4Pc12_Nps1,
                    Q4Pc12_Detractor_Detail1 = data.Q4Pc12_Detractor_Detail1,

                    Q4Q4_ReqSent1 = data.Q4Q4_ReqSent1,
                    Q4Q4_ResRece1 = data.Q4Q4_ResRece1,
                    Q4Q4_Promoter1 = data.Q4Q4_Promoter1,
                    Q4Q4_Collection1 = data.Q4Q4_Collection1,
                    Q4Q4_Detractor1 = data.Q4Q4_Detractor1,
                    Q4Q4_Nps1 = data.Q4Q4_Nps1,
                    Q4Q4_Detractor_Detail1 = data.Q4Q4_Detractor_Detail1,

                    Ytd_ReqSent11 = data.Ytd_ReqSent11,
                    Ytd_ResRece11 = data.Ytd_ResRece11,
                    Ytd_Promoter11 = data.Ytd_Promoter11,
                    Ytd_Collection11 = data.Ytd_Collection11,
                    Ytd_Detractor11 = data.Ytd_Detractor11,
                    Ytd_Nps11 = data.Ytd_Nps11,
                    Ytd_Detractor_Detail11 = data.Ytd_Detractor_Detail11,
                    CSAT_Business = data.CSAT_Business,

                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,
                    UpdatedDate = data.UpdatedDate,
                    UpdatedBy = data.UpdatedBy
                }).ToList();

                return list;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<Csat_SummaryViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CsatSum_Id", id),
                };

                var sql = @"EXEC sp_Get_CSAT_Summary_GetById @CsatSum_Id";

                var result = await _dbContext.CSAT_Summary.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new Csat_SummaryViewModel
                {
                    Id = data.Id,
                    Deleted = data.Deleted,

                    Q1Pc1_ReqSent = data.Q1Pc1_ReqSent,
                    Q1Pc1_ResRece = data.Q1Pc1_ResRece,
                    Q1Pc1_Promoter = data.Q1Pc1_Promoter,
                    Q1Pc1_Collection = data.Q1Pc1_Collection,
                    Q1Pc1_Detractor = data.Q1Pc1_Detractor,
                    Q1Pc1_Nps = data.Q1Pc1_Nps,
                    Q1Pc1_Detractor_Detail = data.Q1Pc1_Detractor_Detail,

                    Q1Pc2_ReqSent = data.Q1Pc2_ReqSent,
                    Q1Pc2_ResRece = data.Q1Pc2_ResRece,
                    Q1Pc2_Promoter = data.Q1Pc2_Promoter,
                    Q1Pc2_Collection = data.Q1Pc2_Collection,
                    Q1Pc2_Detractor = data.Q1Pc2_Detractor,
                    Q1Pc2_Nps = data.Q1Pc2_Nps,
                    Q1Pc2_Detractor_Detail = data.Q1Pc2_Detractor_Detail,

                    Q1Pc3_ReqSent = data.Q1Pc3_ReqSent,
                    Q1Pc3_ResRece = data.Q1Pc3_ResRece,
                    Q1Pc3_Promoter = data.Q1Pc3_Promoter,
                    Q1Pc3_Collection = data.Q1Pc3_Collection,
                    Q1Pc3_Detractor = data.Q1Pc3_Detractor,
                    Q1Pc3_Nps = data.Q1Pc3_Nps,
                    Q1Pc3_Detractor_Detail = data.Q1Pc3_Detractor_Detail,

                    Q1Q1_ReqSent = data.Q1Q1_ReqSent,
                    Q1Q1_ResRece = data.Q1Q1_ResRece,
                    Q1Q1_Promoter = data.Q1Q1_Promoter,
                    Q1Q1_Collection = data.Q1Q1_Collection,
                    Q1Q1_Detractor = data.Q1Q1_Detractor,
                    Q1Q1_Nps = data.Q1Q1_Nps,
                    Q1Q1_Detractor_Detail = data.Q1Q1_Detractor_Detail,

                    Q2Pc4_ReqSent = data.Q2Pc4_ReqSent,
                    Q2Pc4_ResRece = data.Q2Pc4_ResRece,
                    Q2Pc4_Promoter = data.Q2Pc4_Promoter,
                    Q2Pc4_Collection = data.Q2Pc4_Collection,
                    Q2Pc4_Detractor = data.Q2Pc4_Detractor,
                    Q2Pc4_Nps = data.Q2Pc4_Nps,
                    Q2Pc4_Detractor_Detail = data.Q2Pc4_Detractor_Detail,

                    Q2Pc5_ReqSent = data.Q2Pc5_ReqSent,
                    Q2Pc5_ResRece = data.Q2Pc5_ResRece,
                    Q2Pc5_Promoter = data.Q2Pc5_Promoter,
                    Q2Pc5_Collection = data.Q2Pc5_Collection,
                    Q2Pc5_Detractor = data.Q2Pc5_Detractor,
                    Q2Pc5_Nps = data.Q2Pc5_Nps,
                    Q2Pc5_Detractor_Detail = data.Q2Pc5_Detractor_Detail,

                    Q2Pc6_ReqSent = data.Q2Pc6_ReqSent,
                    Q2Pc6_ResRece = data.Q2Pc6_ResRece,
                    Q2Pc6_Promoter = data.Q2Pc6_Promoter,
                    Q2Pc6_Collection = data.Q2Pc6_Collection,
                    Q2Pc6_Detractor = data.Q2Pc6_Detractor,
                    Q2Pc6_Nps = data.Q2Pc6_Nps,
                    Q2Pc6_Detractor_Detail = data.Q2Pc6_Detractor_Detail,

                    Q2Q2_ReqSent = data.Q2Q2_ReqSent,
                    Q2Q2_ResRece = data.Q2Q2_ResRece,
                    Q2Q2_Promoter = data.Q2Q2_Promoter,
                    Q2Q2_Collection = data.Q2Q2_Collection,
                    Q2Q2_Detractor = data.Q2Q2_Detractor,
                    Q2Q2_Nps = data.Q2Q2_Nps,
                    Q2Q2_Detractor_Detail = data.Q2Q2_Detractor_Detail,

                    Q3Pc7_ReqSent = data.Q3Pc7_ReqSent,
                    Q3Pc7_ResRece = data.Q3Pc7_ResRece,
                    Q3Pc7_Promoter = data.Q3Pc7_Promoter,
                    Q3Pc7_Collection = data.Q3Pc7_Collection,
                    Q3Pc7_Detractor = data.Q3Pc7_Detractor,
                    Q3Pc7_Nps = data.Q3Pc7_Nps,
                    Q3Pc7_Detractor_Detail = data.Q3Pc7_Detractor_Detail,

                    Q3Pc8_ReqSent = data.Q3Pc8_ReqSent,
                    Q3Pc8_ResRece = data.Q3Pc8_ResRece,
                    Q3Pc8_Promoter = data.Q3Pc8_Promoter,
                    Q3Pc8_Collection = data.Q3Pc8_Collection,
                    Q3Pc8_Detractor = data.Q3Pc8_Detractor,
                    Q3Pc8_Nps = data.Q3Pc8_Nps,
                    Q3Pc8_Detractor_Detail = data.Q3Pc8_Detractor_Detail,

                    Q3Pc9_ReqSent = data.Q3Pc9_ReqSent,
                    Q3Pc9_ResRece = data.Q3Pc9_ResRece,
                    Q3Pc9_Promoter = data.Q3Pc9_Promoter,
                    Q3Pc9_Collection = data.Q3Pc9_Collection,
                    Q3Pc9_Detractor = data.Q3Pc9_Detractor,
                    Q3Pc9_Nps = data.Q3Pc9_Nps,
                    Q3Pc9_Detractor_Detail = data.Q3Pc9_Detractor_Detail,

                    Q3Q3_ReqSent = data.Q3Q3_ReqSent,
                    Q3Q3_ResRece = data.Q3Q3_ResRece,
                    Q3Q3_Promoter = data.Q3Q3_Promoter,
                    Q3Q3_Collection = data.Q3Q3_Collection,
                    Q3Q3_Detractor = data.Q3Q3_Detractor,
                    Q3Q3_Nps = data.Q3Q3_Nps,
                    Q3Q3_Detractor_Detail = data.Q3Q3_Detractor_Detail,

                    Q4Pc10_ReqSent = data.Q4Pc10_ReqSent,
                    Q4Pc10_ResRece = data.Q4Pc10_ResRece,
                    Q4Pc10_Promoter = data.Q4Pc10_Promoter,
                    Q4Pc10_Collection = data.Q4Pc10_Collection,
                    Q4Pc10_Detractor = data.Q4Pc10_Detractor,
                    Q4Pc10_Nps = data.Q4Pc10_Nps,
                    Q4Pc10_Detractor_Detail = data.Q4Pc10_Detractor_Detail,

                    Q4Pc11_ReqSent1 = data.Q4Pc11_ReqSent1,
                    Q4Pc11_ResRece1 = data.Q4Pc11_ResRece1,
                    Q4Pc11_Promoter1 = data.Q4Pc11_Promoter1,
                    Q4Pc11_Collection1 = data.Q4Pc11_Collection1,
                    Q4Pc11_Detractor1 = data.Q4Pc11_Detractor1,
                    Q4Pc11_Nps1 = data.Q4Pc11_Nps1,
                    Q4Pc11_Detractor_Detail1 = data.Q4Pc11_Detractor_Detail1,

                    Q4Pc12_ReqSent1 = data.Q4Pc12_ReqSent1,
                    Q4Pc12_ResRece1 = data.Q4Pc12_ResRece1,
                    Q4Pc12_Promoter1 = data.Q4Pc12_Promoter1,
                    Q4Pc12_Collection1 = data.Q4Pc12_Collection1,
                    Q4Pc12_Detractor1 = data.Q4Pc12_Detractor1,
                    Q4Pc12_Nps1 = data.Q4Pc12_Nps1,
                    Q4Pc12_Detractor_Detail1 = data.Q4Pc12_Detractor_Detail1,

                    Q4Q4_ReqSent1 = data.Q4Q4_ReqSent1,
                    Q4Q4_ResRece1 = data.Q4Q4_ResRece1,
                    Q4Q4_Promoter1 = data.Q4Q4_Promoter1,
                    Q4Q4_Collection1 = data.Q4Q4_Collection1,
                    Q4Q4_Detractor1 = data.Q4Q4_Detractor1,
                    Q4Q4_Nps1 = data.Q4Q4_Nps1,
                    Q4Q4_Detractor_Detail1 = data.Q4Q4_Detractor_Detail1,

                    Ytd_ReqSent11 = data.Ytd_ReqSent11,
                    Ytd_ResRece11 = data.Ytd_ResRece11,
                    Ytd_Promoter11 = data.Ytd_Promoter11,
                    Ytd_Collection11 = data.Ytd_Collection11,
                    Ytd_Detractor11 = data.Ytd_Detractor11,
                    Ytd_Nps11 = data.Ytd_Nps11,
                    Ytd_Detractor_Detail11 = data.Ytd_Detractor_Detail11,
                    CSAT_Business = data.CSAT_Business,

                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,
                    UpdatedDate = data.UpdatedDate,
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

        public async Task<OperationResult> CreateAsync(CSAT_Summary entity, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@IsDeleted", entity.Deleted),

                    new SqlParameter("@Q1Pc1_ReqSent", entity.Q1Pc1_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_ResRece", entity.Q1Pc1_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Promoter", entity.Q1Pc1_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Collection", entity.Q1Pc1_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Detractor", entity.Q1Pc1_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Nps", entity.Q1Pc1_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Detractor_Detail", entity.Q1Pc1_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q1Pc2_ReqSent", entity.Q1Pc2_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_ResRece", entity.Q1Pc2_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Promoter", entity.Q1Pc2_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Collection", entity.Q1Pc2_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Detractor", entity.Q1Pc2_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Nps", entity.Q1Pc2_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Detractor_Detail", entity.Q1Pc2_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q1Pc3_ReqSent", entity.Q1Pc3_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_ResRece", entity.Q1Pc3_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Promoter", entity.Q1Pc3_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Collection", entity.Q1Pc3_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Detractor", entity.Q1Pc3_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Nps", entity.Q1Pc3_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Detractor_Detail", entity.Q1Pc3_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q1Q1_ReqSent", entity.Q1Q1_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_ResRece", entity.Q1Q1_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Promoter", entity.Q1Q1_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Collection", entity.Q1Q1_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Detractor", entity.Q1Q1_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Nps", entity.Q1Q1_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Detractor_Detail", entity.Q1Q1_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q2Pc4_ReqSent", entity.Q2Pc4_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_ResRece", entity.Q2Pc4_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Promoter", entity.Q2Pc4_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Collection", entity.Q2Pc4_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Detractor", entity.Q2Pc4_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Nps", entity.Q2Pc4_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Detractor_Detail", entity.Q2Pc4_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q2Pc5_ReqSent", entity.Q2Pc5_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_ResRece", entity.Q2Pc5_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Promoter", entity.Q2Pc5_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Collection", entity.Q2Pc5_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Detractor", entity.Q2Pc5_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Nps", entity.Q2Pc5_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Detractor_Detail", entity.Q2Pc5_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q2Pc6_ReqSent", entity.Q2Pc6_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_ResRece", entity.Q2Pc6_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Promoter", entity.Q2Pc6_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Collection", entity.Q2Pc6_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Detractor", entity.Q2Pc6_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Nps", entity.Q2Pc6_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Detractor_Detail", entity.Q2Pc6_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q2Q2_ReqSent", entity.Q2Q2_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_ResRece", entity.Q2Q2_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Promoter", entity.Q2Q2_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Collection", entity.Q2Q2_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Detractor", entity.Q2Q2_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Nps", entity.Q2Q2_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Detractor_Detail", entity.Q2Q2_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q3Pc7_ReqSent", entity.Q3Pc7_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_ResRece", entity.Q3Pc7_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Promoter", entity.Q3Pc7_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Collection", entity.Q3Pc7_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Detractor", entity.Q3Pc7_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Nps", entity.Q3Pc7_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Detractor_Detail", entity.Q3Pc7_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q3Pc8_ReqSent", entity.Q3Pc8_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_ResRece", entity.Q3Pc8_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Promoter", entity.Q3Pc8_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Collection", entity.Q3Pc8_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Detractor", entity.Q3Pc8_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Nps", entity.Q3Pc8_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Detractor_Detail", entity.Q3Pc8_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q3Pc9_ReqSent", entity.Q3Pc9_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_ResRece", entity.Q3Pc9_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Promoter", entity.Q3Pc9_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Collection", entity.Q3Pc9_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Detractor", entity.Q3Pc9_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Nps", entity.Q3Pc9_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Detractor_Detail", entity.Q3Pc9_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q3Q3_ReqSent", entity.Q3Q3_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_ResRece", entity.Q3Q3_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Promoter", entity.Q3Q3_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Collection", entity.Q3Q3_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Detractor", entity.Q3Q3_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Nps", entity.Q3Q3_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Detractor_Detail", entity.Q3Q3_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q4Pc10_ReqSent", entity.Q4Pc10_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_ResRece", entity.Q4Pc10_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Promoter", entity.Q4Pc10_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Collection", entity.Q4Pc10_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Detractor", entity.Q4Pc10_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Nps", entity.Q4Pc10_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Detractor_Detail", entity.Q4Pc10_Detractor_Detail ?? (object?)DBNull.Value),

                    new SqlParameter("@Q4Pc11_ReqSent1", entity.Q4Pc11_ReqSent1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_ResRece1", entity.Q4Pc11_ResRece1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Promoter1", entity.Q4Pc11_Promoter1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Collection1", entity.Q4Pc11_Collection1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Detractor1", entity.Q4Pc11_Detractor1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Nps1", entity.Q4Pc11_Nps1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Detractor_Detail1", entity.Q4Pc11_Detractor_Detail1 ?? (object?)DBNull.Value),

                    new SqlParameter("@Q4Pc12_ReqSent1", entity.Q4Pc12_ReqSent1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_ResRece1", entity.Q4Pc12_ResRece1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Promoter1", entity.Q4Pc12_Promoter1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Collection1", entity.Q4Pc12_Collection1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Detractor1", entity.Q4Pc12_Detractor1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Nps1", entity.Q4Pc12_Nps1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Detractor_Detail1", entity.Q4Pc12_Detractor_Detail1 ?? (object?)DBNull.Value),

                    new SqlParameter("@Q4Q4_ReqSent1", entity.Q4Q4_ReqSent1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_ResRece1", entity.Q4Q4_ResRece1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Promoter1", entity.Q4Q4_Promoter1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Collection1", entity.Q4Q4_Collection1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Detractor1", entity.Q4Q4_Detractor1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Nps1", entity.Q4Q4_Nps1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Detractor_Detail1", entity.Q4Q4_Detractor_Detail1 ?? (object?)DBNull.Value),

                    new SqlParameter("@Ytd_ReqSent11", entity.Ytd_ReqSent11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_ResRece11", entity.Ytd_ResRece11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Promoter11", entity.Ytd_Promoter11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Collection11", entity.Ytd_Collection11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Detractor11", entity.Ytd_Detractor11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Nps11", entity.Ytd_Nps11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Detractor_Detail11", entity.Ytd_Detractor_Detail11 ?? (object?)DBNull.Value),
                    new SqlParameter("@CSAT_Business", entity.CSAT_Business ?? (object?)DBNull.Value),

                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object?)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_CSAT_Summary " +
                   "@IsDeleted, " +
                "@Q1Pc1_ReqSent, @Q1Pc1_ResRece, @Q1Pc1_Promoter, @Q1Pc1_Collection, @Q1Pc1_Detractor, @Q1Pc1_Nps, @Q1Pc1_Detractor_Detail, " +
                "@Q1Pc2_ReqSent, @Q1Pc2_ResRece, @Q1Pc2_Promoter, @Q1Pc2_Collection, @Q1Pc2_Detractor, @Q1Pc2_Nps, @Q1Pc2_Detractor_Detail, " +
                "@Q1Pc3_ReqSent, @Q1Pc3_ResRece, @Q1Pc3_Promoter, @Q1Pc3_Collection, @Q1Pc3_Detractor, @Q1Pc3_Nps, @Q1Pc3_Detractor_Detail, " +
                "@Q1Q1_ReqSent, @Q1Q1_ResRece, @Q1Q1_Promoter, @Q1Q1_Collection, @Q1Q1_Detractor, @Q1Q1_Nps, @Q1Q1_Detractor_Detail, " +
                "@Q2Pc4_ReqSent, @Q2Pc4_ResRece, @Q2Pc4_Promoter, @Q2Pc4_Collection, @Q2Pc4_Detractor, @Q2Pc4_Nps, @Q2Pc4_Detractor_Detail, " +
                "@Q2Pc5_ReqSent, @Q2Pc5_ResRece, @Q2Pc5_Promoter, @Q2Pc5_Collection, @Q2Pc5_Detractor, @Q2Pc5_Nps, @Q2Pc5_Detractor_Detail, " +
                "@Q2Pc6_ReqSent, @Q2Pc6_ResRece, @Q2Pc6_Promoter, @Q2Pc6_Collection, @Q2Pc6_Detractor, @Q2Pc6_Nps, @Q2Pc6_Detractor_Detail, " +
                "@Q2Q2_ReqSent, @Q2Q2_ResRece, @Q2Q2_Promoter, @Q2Q2_Collection, @Q2Q2_Detractor, @Q2Q2_Nps, @Q2Q2_Detractor_Detail, " +
                "@Q3Pc7_ReqSent, @Q3Pc7_ResRece, @Q3Pc7_Promoter, @Q3Pc7_Collection, @Q3Pc7_Detractor, @Q3Pc7_Nps, @Q3Pc7_Detractor_Detail, " +
                "@Q3Pc8_ReqSent, @Q3Pc8_ResRece, @Q3Pc8_Promoter, @Q3Pc8_Collection, @Q3Pc8_Detractor, @Q3Pc8_Nps, @Q3Pc8_Detractor_Detail, " +
                "@Q3Pc9_ReqSent, @Q3Pc9_ResRece, @Q3Pc9_Promoter, @Q3Pc9_Collection, @Q3Pc9_Detractor, @Q3Pc9_Nps, @Q3Pc9_Detractor_Detail, " +
                "@Q3Q3_ReqSent, @Q3Q3_ResRece, @Q3Q3_Promoter, @Q3Q3_Collection, @Q3Q3_Detractor, @Q3Q3_Nps, @Q3Q3_Detractor_Detail, " +
                "@Q4Pc10_ReqSent, @Q4Pc10_ResRece, @Q4Pc10_Promoter, @Q4Pc10_Collection, @Q4Pc10_Detractor, @Q4Pc10_Nps, @Q4Pc10_Detractor_Detail, " +
                "@Q4Pc11_ReqSent1, @Q4Pc11_ResRece1, @Q4Pc11_Promoter1, @Q4Pc11_Collection1, @Q4Pc11_Detractor1, @Q4Pc11_Nps1, @Q4Pc11_Detractor_Detail1, " +
                "@Q4Pc12_ReqSent1, @Q4Pc12_ResRece1, @Q4Pc12_Promoter1, @Q4Pc12_Collection1, @Q4Pc12_Detractor1, @Q4Pc12_Nps1, @Q4Pc12_Detractor_Detail1, " +
                "@Q4Q4_ReqSent1, @Q4Q4_ResRece1, @Q4Q4_Promoter1, @Q4Q4_Collection1, @Q4Q4_Detractor1, @Q4Q4_Nps1, @Q4Q4_Detractor_Detail1, " +
                "@Ytd_ReqSent11, @Ytd_ResRece11, @Ytd_Promoter11, @Ytd_Collection11, @Ytd_Detractor11, @Ytd_Nps11, @Ytd_Detractor_Detail11,@CSAT_Business, " +
                "@CreatedBy",
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

        public async Task<OperationResult> UpdateAsync(CSAT_Summary entity, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CsatSum_Id", entity.Id),
                    new SqlParameter("@IsDeleted", entity.Deleted),

                    new SqlParameter("@Q1Pc1_ReqSent", entity.Q1Pc1_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_ResRece", entity.Q1Pc1_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Promoter", entity.Q1Pc1_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Collection", entity.Q1Pc1_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Detractor", entity.Q1Pc1_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Nps", entity.Q1Pc1_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc1_Detractor_Detail", entity.Q1Pc1_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q1Pc2_ReqSent", entity.Q1Pc2_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_ResRece", entity.Q1Pc2_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Promoter", entity.Q1Pc2_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Collection", entity.Q1Pc2_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Detractor", entity.Q1Pc2_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Nps", entity.Q1Pc2_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc2_Detractor_Detail", entity.Q1Pc2_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q1Pc3_ReqSent", entity.Q1Pc3_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_ResRece", entity.Q1Pc3_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Promoter", entity.Q1Pc3_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Collection", entity.Q1Pc3_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Detractor", entity.Q1Pc3_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Nps", entity.Q1Pc3_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Pc3_Detractor_Detail", entity.Q1Pc3_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q1Q1_ReqSent", entity.Q1Q1_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_ResRece", entity.Q1Q1_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Promoter", entity.Q1Q1_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Collection", entity.Q1Q1_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Detractor", entity.Q1Q1_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Nps", entity.Q1Q1_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q1Q1_Detractor_Detail", entity.Q1Q1_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q2Pc4_ReqSent", entity.Q2Pc4_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_ResRece", entity.Q2Pc4_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Promoter", entity.Q2Pc4_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Collection", entity.Q2Pc4_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Detractor", entity.Q2Pc4_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Nps", entity.Q2Pc4_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc4_Detractor_Detail", entity.Q2Pc4_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q2Pc5_ReqSent", entity.Q2Pc5_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_ResRece", entity.Q2Pc5_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Promoter", entity.Q2Pc5_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Collection", entity.Q2Pc5_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Detractor", entity.Q2Pc5_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Nps", entity.Q2Pc5_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc5_Detractor_Detail", entity.Q2Pc5_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q2Pc6_ReqSent", entity.Q2Pc6_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_ResRece", entity.Q2Pc6_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Promoter", entity.Q2Pc6_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Collection", entity.Q2Pc6_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Detractor", entity.Q2Pc6_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Nps", entity.Q2Pc6_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Pc6_Detractor_Detail", entity.Q2Pc6_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q2Q2_ReqSent", entity.Q2Q2_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_ResRece", entity.Q2Q2_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Promoter", entity.Q2Q2_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Collection", entity.Q2Q2_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Detractor", entity.Q2Q2_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Nps", entity.Q2Q2_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q2Q2_Detractor_Detail", entity.Q2Q2_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q3Pc7_ReqSent", entity.Q3Pc7_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_ResRece", entity.Q3Pc7_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Promoter", entity.Q3Pc7_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Collection", entity.Q3Pc7_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Detractor", entity.Q3Pc7_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Nps", entity.Q3Pc7_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc7_Detractor_Detail", entity.Q3Pc7_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q3Pc8_ReqSent", entity.Q3Pc8_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_ResRece", entity.Q3Pc8_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Promoter", entity.Q3Pc8_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Collection", entity.Q3Pc8_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Detractor", entity.Q3Pc8_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Nps", entity.Q3Pc8_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc8_Detractor_Detail", entity.Q3Pc8_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q3Pc9_ReqSent", entity.Q3Pc9_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_ResRece", entity.Q3Pc9_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Promoter", entity.Q3Pc9_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Collection", entity.Q3Pc9_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Detractor", entity.Q3Pc9_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Nps", entity.Q3Pc9_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Pc9_Detractor_Detail", entity.Q3Pc9_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q3Q3_ReqSent", entity.Q3Q3_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_ResRece", entity.Q3Q3_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Promoter", entity.Q3Q3_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Collection", entity.Q3Q3_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Detractor", entity.Q3Q3_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Nps", entity.Q3Q3_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q3Q3_Detractor_Detail", entity.Q3Q3_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q4Pc10_ReqSent", entity.Q4Pc10_ReqSent ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_ResRece", entity.Q4Pc10_ResRece ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Promoter", entity.Q4Pc10_Promoter ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Collection", entity.Q4Pc10_Collection ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Detractor", entity.Q4Pc10_Detractor ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Nps", entity.Q4Pc10_Nps ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc10_Detractor_Detail", entity.Q4Pc10_Detractor_Detail ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q4Pc11_ReqSent1", entity.Q4Pc11_ReqSent1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_ResRece1", entity.Q4Pc11_ResRece1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Promoter1", entity.Q4Pc11_Promoter1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Collection1", entity.Q4Pc11_Collection1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Detractor1", entity.Q4Pc11_Detractor1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Nps1", entity.Q4Pc11_Nps1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc11_Detractor_Detail1", entity.Q4Pc11_Detractor_Detail1 ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q4Pc12_ReqSent1", entity.Q4Pc12_ReqSent1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_ResRece1", entity.Q4Pc12_ResRece1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Promoter1", entity.Q4Pc12_Promoter1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Collection1", entity.Q4Pc12_Collection1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Detractor1", entity.Q4Pc12_Detractor1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Nps1", entity.Q4Pc12_Nps1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Pc12_Detractor_Detail1", entity.Q4Pc12_Detractor_Detail1 ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Q4Q4_ReqSent1", entity.Q4Q4_ReqSent1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_ResRece1", entity.Q4Q4_ResRece1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Promoter1", entity.Q4Q4_Promoter1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Collection1", entity.Q4Q4_Collection1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Detractor1", entity.Q4Q4_Detractor1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Nps1", entity.Q4Q4_Nps1 ?? (object?)DBNull.Value),
                    new SqlParameter("@Q4Q4_Detractor_Detail1", entity.Q4Q4_Detractor_Detail1 ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@Ytd_ReqSent11", entity.Ytd_ReqSent11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_ResRece11", entity.Ytd_ResRece11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Promoter11", entity.Ytd_Promoter11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Collection11", entity.Ytd_Collection11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Detractor11", entity.Ytd_Detractor11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Nps11", entity.Ytd_Nps11 ?? (object?)DBNull.Value),
                    new SqlParameter("@Ytd_Detractor_Detail11", entity.Ytd_Detractor_Detail11 ?? (object?)DBNull.Value),
                    new SqlParameter("@CSAT_Business", entity.CSAT_Business ?? (object?)DBNull.Value),
                    
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value),
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Update_CSAT_Summary " +
                    "@CsatSum_Id,@IsDeleted, @Q1Pc1_ReqSent, @Q1Pc1_ResRece, @Q1Pc1_Promoter, @Q1Pc1_Collection, @Q1Pc1_Detractor, @Q1Pc1_Nps, @Q1Pc1_Detractor_Detail, " +
                "@Q1Pc2_ReqSent, @Q1Pc2_ResRece, @Q1Pc2_Promoter, @Q1Pc2_Collection, @Q1Pc2_Detractor, @Q1Pc2_Nps, @Q1Pc2_Detractor_Detail, " +
                "@Q1Pc3_ReqSent, @Q1Pc3_ResRece, @Q1Pc3_Promoter, @Q1Pc3_Collection, @Q1Pc3_Detractor, @Q1Pc3_Nps, @Q1Pc3_Detractor_Detail, " +
                "@Q1Q1_ReqSent, @Q1Q1_ResRece, @Q1Q1_Promoter, @Q1Q1_Collection, @Q1Q1_Detractor, @Q1Q1_Nps, @Q1Q1_Detractor_Detail, " +
                "@Q2Pc4_ReqSent, @Q2Pc4_ResRece, @Q2Pc4_Promoter, @Q2Pc4_Collection, @Q2Pc4_Detractor, @Q2Pc4_Nps, @Q2Pc4_Detractor_Detail, " +
                "@Q2Pc5_ReqSent, @Q2Pc5_ResRece, @Q2Pc5_Promoter, @Q2Pc5_Collection, @Q2Pc5_Detractor, @Q2Pc5_Nps, @Q2Pc5_Detractor_Detail, " +
                "@Q2Pc6_ReqSent, @Q2Pc6_ResRece, @Q2Pc6_Promoter, @Q2Pc6_Collection, @Q2Pc6_Detractor, @Q2Pc6_Nps, @Q2Pc6_Detractor_Detail, " +
                "@Q2Q2_ReqSent, @Q2Q2_ResRece, @Q2Q2_Promoter, @Q2Q2_Collection, @Q2Q2_Detractor, @Q2Q2_Nps, @Q2Q2_Detractor_Detail, " +
                "@Q3Pc7_ReqSent, @Q3Pc7_ResRece, @Q3Pc7_Promoter, @Q3Pc7_Collection, @Q3Pc7_Detractor, @Q3Pc7_Nps, @Q3Pc7_Detractor_Detail, " +
                "@Q3Pc8_ReqSent, @Q3Pc8_ResRece, @Q3Pc8_Promoter, @Q3Pc8_Collection, @Q3Pc8_Detractor, @Q3Pc8_Nps, @Q3Pc8_Detractor_Detail, " +
                "@Q3Pc9_ReqSent, @Q3Pc9_ResRece, @Q3Pc9_Promoter, @Q3Pc9_Collection, @Q3Pc9_Detractor, @Q3Pc9_Nps, @Q3Pc9_Detractor_Detail, " +
                "@Q3Q3_ReqSent, @Q3Q3_ResRece, @Q3Q3_Promoter, @Q3Q3_Collection, @Q3Q3_Detractor, @Q3Q3_Nps, @Q3Q3_Detractor_Detail, " +
                "@Q4Pc10_ReqSent, @Q4Pc10_ResRece, @Q4Pc10_Promoter, @Q4Pc10_Collection, @Q4Pc10_Detractor, @Q4Pc10_Nps, @Q4Pc10_Detractor_Detail, " +
                "@Q4Pc11_ReqSent1, @Q4Pc11_ResRece1, @Q4Pc11_Promoter1, @Q4Pc11_Collection1, @Q4Pc11_Detractor1, @Q4Pc11_Nps1, @Q4Pc11_Detractor_Detail1, " +
                "@Q4Pc12_ReqSent1, @Q4Pc12_ResRece1, @Q4Pc12_Promoter1, @Q4Pc12_Collection1, @Q4Pc12_Detractor1, @Q4Pc12_Nps1, @Q4Pc12_Detractor_Detail1, " +
                "@Q4Q4_ReqSent1, @Q4Q4_ResRece1, @Q4Q4_Promoter1, @Q4Q4_Collection1, @Q4Q4_Detractor1, @Q4Q4_Nps1, @Q4Q4_Detractor_Detail1, " +
                "@Ytd_ReqSent11, @Ytd_ResRece11, @Ytd_Promoter11, @Ytd_Collection11, @Ytd_Detractor11, @Ytd_Nps11, @Ytd_Detractor_Detail11,@CSAT_Business," +
                "@UpdatedBy",
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

    }
}
