using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.ImpactTestRepository
{
    public class ImpactTestRepository : SqlTableRepository, IImpactTestRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public ImpactTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<OperationResult> InsertImpactTestReportAsync(ImpactTestViewModel model)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CustomerProjectName", model.CustomerProjectName ?? (object)DBNull.Value),
                    new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                    new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                    new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                    new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                    new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                    new SqlParameter("@Quantity", model.Quantity),

                    // Weight (kg)
                    new SqlParameter("@Weight_kg_IK01", model.Weight_kg_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK02", model.Weight_kg_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK03", model.Weight_kg_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK04", model.Weight_kg_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK05", model.Weight_kg_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK06", model.Weight_kg_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK07", model.Weight_kg_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK08", model.Weight_kg_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK09", model.Weight_kg_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK10", model.Weight_kg_IK10 ?? (object)DBNull.Value),

                    // Weight Material
                    new SqlParameter("@Weight_Material_IK01", model.Weight_Material_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK02", model.Weight_Material_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK03", model.Weight_Material_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK04", model.Weight_Material_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK05", model.Weight_Material_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK06", model.Weight_Material_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK07", model.Weight_Material_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK08", model.Weight_Material_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK09", model.Weight_Material_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK10", model.Weight_Material_IK10 ?? (object)DBNull.Value),

                    // Distance
                    new SqlParameter("@Distance_cm_IK01", model.Distance_cm_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK02", model.Distance_cm_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK03", model.Distance_cm_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK04", model.Distance_cm_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK05", model.Distance_cm_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK06", model.Distance_cm_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK07", model.Distance_cm_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK08", model.Distance_cm_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK09", model.Distance_cm_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK10", model.Distance_cm_IK10 ?? (object)DBNull.Value),

                    // Impact Energy
                    new SqlParameter("@ImpactEnergy_joules_IK01", model.ImpactEnergy_joules_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK02", model.ImpactEnergy_joules_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK03", model.ImpactEnergy_joules_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK04", model.ImpactEnergy_joules_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK05", model.ImpactEnergy_joules_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK06", model.ImpactEnergy_joules_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK07", model.ImpactEnergy_joules_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK08", model.ImpactEnergy_joules_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK09", model.ImpactEnergy_joules_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK10", model.ImpactEnergy_joules_IK10 ?? (object)DBNull.Value),

                    // Applicable Test
                    new SqlParameter("@ApplicableTest_IK01", model.ApplicableTest_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK02", model.ApplicableTest_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK03", model.ApplicableTest_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK04", model.ApplicableTest_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK05", model.ApplicableTest_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK06", model.ApplicableTest_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK07", model.ApplicableTest_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK08", model.ApplicableTest_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK09", model.ApplicableTest_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK10", model.ApplicableTest_IK10 ?? (object)DBNull.Value),

                    // Observations
                    new SqlParameter("@Observation_DamageObserved", model.Observation_DamageObserved ?? (object)DBNull.Value),
                    new SqlParameter("@Observation_LivePartsAccessibility", model.Observation_LivePartsAccessibility ?? (object)DBNull.Value),
                    new SqlParameter("@Observation_Photo", model.Observation_Photo ?? (object)DBNull.Value),

                    new SqlParameter("@TestResult", model.TestResult ?? (object)DBNull.Value),
                    new SqlParameter("@TestedBy", model.TestedBy ?? (object)DBNull.Value),
                    new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),

                    new SqlParameter("@AddedBy", model.AddedBy),
                    new SqlParameter("@AddedOn", model.AddedOn)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                @"EXEC sp_Insert_ImpectTestReport
                @CustomerProjectName, @ReportNo, @ReportDate, @ProductCatRef,
                @ProductDescription, @BatchCode,@Quantity,

                @Weight_kg_IK01, @Weight_kg_IK02, @Weight_kg_IK03, @Weight_kg_IK04, @Weight_kg_IK05,
                @Weight_kg_IK06, @Weight_kg_IK07, @Weight_kg_IK08, @Weight_kg_IK09, @Weight_kg_IK10,

                @Weight_Material_IK01, @Weight_Material_IK02, @Weight_Material_IK03, @Weight_Material_IK04, @Weight_Material_IK05,
                @Weight_Material_IK06, @Weight_Material_IK07, @Weight_Material_IK08, @Weight_Material_IK09, @Weight_Material_IK10,

                @Distance_cm_IK01, @Distance_cm_IK02, @Distance_cm_IK03, @Distance_cm_IK04, @Distance_cm_IK05,
                @Distance_cm_IK06, @Distance_cm_IK07, @Distance_cm_IK08, @Distance_cm_IK09, @Distance_cm_IK10,

                @ImpactEnergy_joules_IK01, @ImpactEnergy_joules_IK02, @ImpactEnergy_joules_IK03, @ImpactEnergy_joules_IK04, @ImpactEnergy_joules_IK05,
                @ImpactEnergy_joules_IK06, @ImpactEnergy_joules_IK07, @ImpactEnergy_joules_IK08, @ImpactEnergy_joules_IK09, @ImpactEnergy_joules_IK10,

                @ApplicableTest_IK01, @ApplicableTest_IK02, @ApplicableTest_IK03, @ApplicableTest_IK04, @ApplicableTest_IK05,
                @ApplicableTest_IK06, @ApplicableTest_IK07, @ApplicableTest_IK08, @ApplicableTest_IK09, @ApplicableTest_IK10,

                @Observation_DamageObserved,@Observation_LivePartsAccessibility,
                @Observation_Photo, @TestResult,
                @TestedBy,@VerifiedBy, @AddedBy,@AddedOn",

                parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateImpactTestReportAsync(ImpactTestViewModel model)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@CustomerProjectName", model.CustomerProjectName ?? (object)DBNull.Value),
                    new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                    new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                    new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                    new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                    new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                    new SqlParameter("@Quantity", model.Quantity),

                    // Weight (kg)
                    new SqlParameter("@Weight_kg_IK01", model.Weight_kg_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK02", model.Weight_kg_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK03", model.Weight_kg_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK04", model.Weight_kg_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK05", model.Weight_kg_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK06", model.Weight_kg_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK07", model.Weight_kg_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK08", model.Weight_kg_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK09", model.Weight_kg_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_kg_IK10", model.Weight_kg_IK10 ?? (object)DBNull.Value),

                    // Weight Material
                    new SqlParameter("@Weight_Material_IK01", model.Weight_Material_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK02", model.Weight_Material_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK03", model.Weight_Material_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK04", model.Weight_Material_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK05", model.Weight_Material_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK06", model.Weight_Material_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK07", model.Weight_Material_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK08", model.Weight_Material_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK09", model.Weight_Material_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@Weight_Material_IK10", model.Weight_Material_IK10 ?? (object)DBNull.Value),

                    // Distance
                    new SqlParameter("@Distance_cm_IK01", model.Distance_cm_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK02", model.Distance_cm_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK03", model.Distance_cm_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK04", model.Distance_cm_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK05", model.Distance_cm_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK06", model.Distance_cm_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK07", model.Distance_cm_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK08", model.Distance_cm_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK09", model.Distance_cm_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@Distance_cm_IK10", model.Distance_cm_IK10 ?? (object)DBNull.Value),

                    // Impact Energy
                    new SqlParameter("@ImpactEnergy_joules_IK01", model.ImpactEnergy_joules_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK02", model.ImpactEnergy_joules_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK03", model.ImpactEnergy_joules_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK04", model.ImpactEnergy_joules_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK05", model.ImpactEnergy_joules_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK06", model.ImpactEnergy_joules_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK07", model.ImpactEnergy_joules_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK08", model.ImpactEnergy_joules_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK09", model.ImpactEnergy_joules_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@ImpactEnergy_joules_IK10", model.ImpactEnergy_joules_IK10 ?? (object)DBNull.Value),

                    // Applicable Test
                    new SqlParameter("@ApplicableTest_IK01", model.ApplicableTest_IK01 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK02", model.ApplicableTest_IK02 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK03", model.ApplicableTest_IK03 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK04", model.ApplicableTest_IK04 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK05", model.ApplicableTest_IK05 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK06", model.ApplicableTest_IK06 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK07", model.ApplicableTest_IK07 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK08", model.ApplicableTest_IK08 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK09", model.ApplicableTest_IK09 ?? (object)DBNull.Value),
                    new SqlParameter("@ApplicableTest_IK10", model.ApplicableTest_IK10 ?? (object)DBNull.Value),

                    // Observations
                    new SqlParameter("@Observation_DamageObserved", model.Observation_DamageObserved ?? (object)DBNull.Value),
                    new SqlParameter("@Observation_LivePartsAccessibility", model.Observation_LivePartsAccessibility ?? (object)DBNull.Value),
                    new SqlParameter("@Observation_Photo", model.Observation_Photo ?? (object)DBNull.Value),

                    new SqlParameter("@TestResult", model.TestResult ?? (object)DBNull.Value),
                    new SqlParameter("@TestedBy", model.TestedBy ?? (object)DBNull.Value),
                    new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),

                   new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                   new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                @"EXEC sp_Update_ImpectTestReport
                @Id, @CustomerProjectName, @ReportNo, @ReportDate, @ProductCatRef,
                @ProductDescription, @BatchCode,@Quantity,

                @Weight_kg_IK01, @Weight_kg_IK02, @Weight_kg_IK03, @Weight_kg_IK04, @Weight_kg_IK05,
                @Weight_kg_IK06, @Weight_kg_IK07, @Weight_kg_IK08, @Weight_kg_IK09, @Weight_kg_IK10,

                @Weight_Material_IK01, @Weight_Material_IK02, @Weight_Material_IK03, @Weight_Material_IK04, @Weight_Material_IK05,
                @Weight_Material_IK06, @Weight_Material_IK07, @Weight_Material_IK08, @Weight_Material_IK09, @Weight_Material_IK10,

                @Distance_cm_IK01, @Distance_cm_IK02, @Distance_cm_IK03, @Distance_cm_IK04, @Distance_cm_IK05,
                @Distance_cm_IK06, @Distance_cm_IK07, @Distance_cm_IK08, @Distance_cm_IK09, @Distance_cm_IK10,

                @ImpactEnergy_joules_IK01, @ImpactEnergy_joules_IK02, @ImpactEnergy_joules_IK03, @ImpactEnergy_joules_IK04, @ImpactEnergy_joules_IK05,
                @ImpactEnergy_joules_IK06, @ImpactEnergy_joules_IK07, @ImpactEnergy_joules_IK08, @ImpactEnergy_joules_IK09, @ImpactEnergy_joules_IK10,

                @ApplicableTest_IK01, @ApplicableTest_IK02, @ApplicableTest_IK03, @ApplicableTest_IK04, @ApplicableTest_IK05,
                @ApplicableTest_IK06, @ApplicableTest_IK07, @ApplicableTest_IK08, @ApplicableTest_IK09, @ApplicableTest_IK10,

                @Observation_DamageObserved,@Observation_LivePartsAccessibility,
                @Observation_Photo, @TestResult,
                @TestedBy,@VerifiedBy, @UpdatedBy,@UpdatedOn",

                parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<List<ImpactTestViewModel>> GetImpactTestReportAsync()
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Id", SqlDbType.Int)
                {
                    Value = 0
                }
            };

                var sql = @"EXEC sp_Get_ImpectTestReport";

                var result = await Task.Run(() => _dbContext.ImpectTestReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new ImpactTestViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        CustomerProjectName = x.CustomerProjectName,
                        BatchCode = x.BatchCode,
                        Quantity = x.Quantity,
                        AddedBy = x.AddedBy
                    })
                    .ToList());

                foreach (var rec in result)
                {
                    rec.User = await _dbContext.User.Where(i => i.Id == rec.AddedBy).Select(x => x.Name).FirstOrDefaultAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<ImpactTestViewModel> GetImpactTestReportByIdAsync(int Id)
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Id", SqlDbType.Int)
                {
                    Value = 0
                }
            };

                var sql = @"EXEC sp_Get_ImpectTestReport";

                var result = await Task.Run(() => _dbContext.ImpectTestReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new ImpactTestViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        CustomerProjectName = x.CustomerProjectName,
                        BatchCode = x.BatchCode,
                        Quantity = x.Quantity,

                        Weight_kg_IK01 = x.Weight_kg_IK01,
                        Weight_kg_IK02 = x.Weight_kg_IK02,
                        Weight_kg_IK03 = x.Weight_kg_IK03,
                        Weight_kg_IK04 = x.Weight_kg_IK04,
                        Weight_kg_IK05 = x.Weight_kg_IK05,
                        Weight_kg_IK06 = x.Weight_kg_IK06,
                        Weight_kg_IK07 = x.Weight_kg_IK07,
                        Weight_kg_IK08 = x.Weight_kg_IK08,
                        Weight_kg_IK09 = x.Weight_kg_IK09,
                        Weight_kg_IK10 = x.Weight_kg_IK10,

                        Weight_Material_IK01 = x.Weight_Material_IK01,
                        Weight_Material_IK02 = x.Weight_Material_IK02,
                        Weight_Material_IK03 = x.Weight_Material_IK03,
                        Weight_Material_IK04 = x.Weight_Material_IK04,
                        Weight_Material_IK05 = x.Weight_Material_IK05,
                        Weight_Material_IK06 = x.Weight_Material_IK06,
                        Weight_Material_IK07 = x.Weight_Material_IK07,
                        Weight_Material_IK08 = x.Weight_Material_IK08,
                        Weight_Material_IK09 = x.Weight_Material_IK09,
                        Weight_Material_IK10 = x.Weight_Material_IK10,

                        Distance_cm_IK01 = x.Distance_cm_IK01,
                        Distance_cm_IK02 = x.Distance_cm_IK02,
                        Distance_cm_IK03 = x.Distance_cm_IK03,
                        Distance_cm_IK04 = x.Distance_cm_IK04,
                        Distance_cm_IK05 = x.Distance_cm_IK05,
                        Distance_cm_IK06 = x.Distance_cm_IK06,
                        Distance_cm_IK07 = x.Distance_cm_IK07,
                        Distance_cm_IK08 = x.Distance_cm_IK08,
                        Distance_cm_IK09 = x.Distance_cm_IK09,
                        Distance_cm_IK10 = x.Distance_cm_IK10,

                        ImpactEnergy_joules_IK01 = x.ImpactEnergy_joules_IK01,
                        ImpactEnergy_joules_IK02 = x.ImpactEnergy_joules_IK02,
                        ImpactEnergy_joules_IK03 = x.ImpactEnergy_joules_IK03,
                        ImpactEnergy_joules_IK04 = x.ImpactEnergy_joules_IK04,
                        ImpactEnergy_joules_IK05 = x.ImpactEnergy_joules_IK05,
                        ImpactEnergy_joules_IK06 = x.ImpactEnergy_joules_IK06,
                        ImpactEnergy_joules_IK07 = x.ImpactEnergy_joules_IK07,
                        ImpactEnergy_joules_IK08 = x.ImpactEnergy_joules_IK08,
                        ImpactEnergy_joules_IK09 = x.ImpactEnergy_joules_IK09,
                        ImpactEnergy_joules_IK10 = x.ImpactEnergy_joules_IK10,

                        ApplicableTest_IK01 = x.ApplicableTest_IK01,
                        ApplicableTest_IK02 = x.ApplicableTest_IK02,
                        ApplicableTest_IK03 = x.ApplicableTest_IK03,
                        ApplicableTest_IK04 = x.ApplicableTest_IK04,
                        ApplicableTest_IK05 = x.ApplicableTest_IK05,
                        ApplicableTest_IK06 = x.ApplicableTest_IK06,
                        ApplicableTest_IK07 = x.ApplicableTest_IK07,
                        ApplicableTest_IK08 = x.ApplicableTest_IK08,
                        ApplicableTest_IK09 = x.ApplicableTest_IK09,
                        ApplicableTest_IK10 = x.ApplicableTest_IK10,

                        Observation_DamageObserved = x.Observation_DamageObserved,
                        Observation_LivePartsAccessibility = x.Observation_LivePartsAccessibility,
                        TestResult = x.TestResult,

                        VerifiedBy = x.VerifiedBy,
                        TestedBy = x.TestedBy,
                    })
                .FirstOrDefault());
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteImpactTestAsync(int Id)
        {
            try
            {
                var result = await base.DeleteAsync<ImpectTestReport>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
