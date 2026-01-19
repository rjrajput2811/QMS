using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.InstallationTrialRepository
{
    public class InstallationTrialRepository : SqlTableRepository, IInstallationTrialRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public InstallationTrialRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<InstallationTrialViewModel>> GetInstallationTrailAsync()
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

                var sql = @"EXEC sp_Get_InstallationTrial";

                var result = await Task.Run(() => _dbContext.InstallationTrials.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new InstallationTrialViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        BatchCode = x.BatchCode,
                        PKD = x.PKD,
                        SampleQty = x.SampleQty,
                        CheckedBy = x.CheckedBy,
                        VerifiedBy = x.VerifiedBy,
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

        public async Task<InstallationTrialViewModel> GetInstallationTrailByIdAsync(int Id)
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Id", Id),
            };

                var sql = @"EXEC sp_Get_InstallationTrial @Id";

                var result = await Task.Run(() => _dbContext.InstallationTrials.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new InstallationTrialViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ProductCatRef = x.ProductCatRef,
                        BatchCode = x.BatchCode,
                        ReportDate = x.ReportDate,
                        ProductDescription = x.ProductDescription,
                        PKD = x.PKD,
                        SampleQty = x.SampleQty,
                        ProductCategory_SampleDetails = x.ProductCategory_SampleDetails,
                        ProductCategory_Result = x.ProductCategory_Result,
                        InstallationSheet_SampleDetails = x.InstallationSheet_SampleDetails,
                        InstallationSheet_Result = x.InstallationSheet_Result,
                        MountingMechanism_SampleDetails = x.MountingMechanism_SampleDetails,
                        MountingMechanism_Result = x.MountingMechanism_Result,
                        DurationOfTest_SampleDetails = x.DurationOfTest_SampleDetails,
                        DurationOfTest_Result = x.DurationOfTest_Result,
                        InstallationWith4xLoad_SampleDetails = x.InstallationWith4xLoad_SampleDetails,
                        InstallationWith4xLoad_Result = x.InstallationWith4xLoad_Result,
                        InstallationWithSandBagLoad_SampleDetails = x.InstallationWithSandBagLoad_SampleDetails,
                        InstallationWithSandBagLoad_Result = x.InstallationWithSandBagLoad_Result,
                        InstallationWithSandBagLoad2_SampleDetails = x.InstallationWithSandBagLoad2_SampleDetails,
                        InstallationWithSandBagLoad2_Result = x.InstallationWithSandBagLoad2_Result,
                        Photo_WithLoad = x.Photo_WithLoad,
                        Photo_WithoutLoad = x.Photo_WithoutLoad,
                        OverallResult = x.OverallResult,
                        CheckedBy = x.CheckedBy,
                        VerifiedBy = x.VerifiedBy

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

        public async Task<OperationResult> InsertInstallationTrailAsync(InstallationTrialViewModel model)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@ReportNo", (object?)model.ReportNo ?? DBNull.Value),
            new SqlParameter("@ProductCatRef", (object?)model.ProductCatRef ?? DBNull.Value),
            new SqlParameter("@BatchCode", (object?)model.BatchCode ?? DBNull.Value),
            new SqlParameter("@ReportDate", (object?)model.ReportDate ?? DBNull.Value),
            new SqlParameter("@ProductDescription", (object?)model.ProductDescription ?? DBNull.Value),
            new SqlParameter("@PKD", (object?)model.PKD ?? DBNull.Value),
            new SqlParameter("@SampleQty", model.SampleQty),

            new SqlParameter("@ProductCategory_SampleDetails", (object?)model.ProductCategory_SampleDetails ?? DBNull.Value),
            new SqlParameter("@ProductCategory_Result", (object?)model.ProductCategory_Result ?? DBNull.Value),

            new SqlParameter("@InstallationSheet_SampleDetails", (object?)model.InstallationSheet_SampleDetails ?? DBNull.Value),
            new SqlParameter("@InstallationSheet_Result", (object?)model.InstallationSheet_Result ?? DBNull.Value),

            new SqlParameter("@MountingMechanism_SampleDetails", (object?)model.MountingMechanism_SampleDetails ?? DBNull.Value),
            new SqlParameter("@MountingMechanism_Result", (object?)model.MountingMechanism_Result ?? DBNull.Value),

            new SqlParameter("@DurationOfTest_SampleDetails", (object?)model.DurationOfTest_SampleDetails ?? DBNull.Value),
            new SqlParameter("@DurationOfTest_Result", (object?)model.DurationOfTest_Result ?? DBNull.Value),

            new SqlParameter("@InstallationWith4xLoad_SampleDetails", (object?)model.InstallationWith4xLoad_SampleDetails ?? DBNull.Value),
            new SqlParameter("@InstallationWith4xLoad_Result", (object?)model.InstallationWith4xLoad_Result ?? DBNull.Value),

            new SqlParameter("@InstallationWithSandBagLoad_SampleDetails", (object?)model.InstallationWithSandBagLoad_SampleDetails ?? DBNull.Value),
            new SqlParameter("@InstallationWithSandBagLoad_Result", (object?)model.InstallationWithSandBagLoad_Result ?? DBNull.Value),

            new SqlParameter("@InstallationWithSandBagLoad2_SampleDetails", (object?)model.InstallationWithSandBagLoad2_SampleDetails ?? DBNull.Value),
            new SqlParameter("@InstallationWithSandBagLoad2_Result", (object?)model.InstallationWithSandBagLoad2_Result ?? DBNull.Value),

            new SqlParameter("@Photo_WithLoad", (object?)model.Photo_WithLoad ?? DBNull.Value),
            new SqlParameter("@Photo_WithoutLoad", (object?)model.Photo_WithoutLoad ?? DBNull.Value),

            new SqlParameter("@OverallResult", (object?)model.OverallResult ?? DBNull.Value),
            new SqlParameter("@CheckedBy", (object?)model.CheckedBy ?? DBNull.Value),
            new SqlParameter("@VerifiedBy", (object?)model.VerifiedBy ?? DBNull.Value),

            new SqlParameter("@AddedBy", model.AddedBy),
            new SqlParameter("@AddedOn", (object)model.AddedOn) // AddedOn is non-nullable in your VM
        };

                var rows = await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.sp_Insert_InstallationTrial " +
                    "@ReportNo, @ProductCatRef, @BatchCode, @ReportDate, @ProductDescription, @PKD, @SampleQty, " +
                    "@ProductCategory_SampleDetails, @ProductCategory_Result, " +
                    "@InstallationSheet_SampleDetails, @InstallationSheet_Result, " +
                    "@MountingMechanism_SampleDetails, @MountingMechanism_Result, " +
                    "@DurationOfTest_SampleDetails, @DurationOfTest_Result, " +
                    "@InstallationWith4xLoad_SampleDetails, @InstallationWith4xLoad_Result, " +
                    "@InstallationWithSandBagLoad_SampleDetails, @InstallationWithSandBagLoad_Result, " +
                    "@InstallationWithSandBagLoad2_SampleDetails, @InstallationWithSandBagLoad2_Result, " +
                    "@Photo_WithLoad, @Photo_WithoutLoad, " +
                    "@OverallResult, @CheckedBy, @VerifiedBy, @AddedBy, @AddedOn",
                    parameters
                );

                return new OperationResult
                {
                    Success = rows > 0,
                    Message = rows > 0 ? "Inserted successfully." : "Insert failed (0 rows affected)."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                throw;
            }
        }

        public async Task<OperationResult> UpdateInstallationTrialAsync(InstallationTrialViewModel model)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),

                    new SqlParameter("@ReportNo", (object?)model.ReportNo ?? DBNull.Value),
                    new SqlParameter("@ProductCatRef", (object?)model.ProductCatRef ?? DBNull.Value),
                    new SqlParameter("@BatchCode", (object?)model.BatchCode ?? DBNull.Value),
                    new SqlParameter("@ReportDate", (object?)model.ReportDate ?? DBNull.Value),
                    new SqlParameter("@ProductDescription", (object?)model.ProductDescription ?? DBNull.Value),
                    new SqlParameter("@PKD", (object?)model.PKD ?? DBNull.Value),

                    new SqlParameter("@SampleQty", model.SampleQty),

                    new SqlParameter("@ProductCategory_SampleDetails", (object?)model.ProductCategory_SampleDetails ?? DBNull.Value),
                    new SqlParameter("@ProductCategory_Result", (object?)model.ProductCategory_Result ?? DBNull.Value),

                    new SqlParameter("@InstallationSheet_SampleDetails", (object?)model.InstallationSheet_SampleDetails ?? DBNull.Value),
                    new SqlParameter("@InstallationSheet_Result", (object?)model.InstallationSheet_Result ?? DBNull.Value),

                    new SqlParameter("@MountingMechanism_SampleDetails", (object?)model.MountingMechanism_SampleDetails ?? DBNull.Value),
                    new SqlParameter("@MountingMechanism_Result", (object?)model.MountingMechanism_Result ?? DBNull.Value),

                    new SqlParameter("@DurationOfTest_SampleDetails", (object?)model.DurationOfTest_SampleDetails ?? DBNull.Value),
                    new SqlParameter("@DurationOfTest_Result", (object?)model.DurationOfTest_Result ?? DBNull.Value),

                    new SqlParameter("@InstallationWith4xLoad_SampleDetails", (object?)model.InstallationWith4xLoad_SampleDetails ?? DBNull.Value),
                    new SqlParameter("@InstallationWith4xLoad_Result", (object?)model.InstallationWith4xLoad_Result ?? DBNull.Value),

                    new SqlParameter("@InstallationWithSandBagLoad_SampleDetails", (object?)model.InstallationWithSandBagLoad_SampleDetails ?? DBNull.Value),
                    new SqlParameter("@InstallationWithSandBagLoad_Result", (object?)model.InstallationWithSandBagLoad_Result ?? DBNull.Value),

                    new SqlParameter("@InstallationWithSandBagLoad2_SampleDetails", (object?)model.InstallationWithSandBagLoad2_SampleDetails ?? DBNull.Value),
                    new SqlParameter("@InstallationWithSandBagLoad2_Result", (object?)model.InstallationWithSandBagLoad2_Result ?? DBNull.Value),

                    new SqlParameter("@Photo_WithLoad", (object?)model.Photo_WithLoad ?? DBNull.Value),
                    new SqlParameter("@Photo_WithoutLoad", (object?)model.Photo_WithoutLoad ?? DBNull.Value),

                    new SqlParameter("@OverallResult", (object?)model.OverallResult ?? DBNull.Value),
                    new SqlParameter("@CheckedBy", (object?)model.CheckedBy ?? DBNull.Value),
                    new SqlParameter("@VerifiedBy", (object?)model.VerifiedBy ?? DBNull.Value),

                    // SP requires non-null UpdatedBy/UpdatedOn (INT, DATETIME)
                    new SqlParameter("@UpdatedBy", (object?)model.UpdatedBy ?? DBNull.Value),
                    new SqlParameter("@UpdatedOn", (object?)model.UpdatedOn ?? DBNull.Value),
                };

                var rows = await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.sp_Update_InstallationTrial " +
                    "@Id, @ReportNo, @ProductCatRef, @BatchCode, @ReportDate, @ProductDescription, @PKD, @SampleQty, " +
                    "@ProductCategory_SampleDetails, @ProductCategory_Result, " +
                    "@InstallationSheet_SampleDetails, @InstallationSheet_Result, " +
                    "@MountingMechanism_SampleDetails, @MountingMechanism_Result, " +
                    "@DurationOfTest_SampleDetails, @DurationOfTest_Result, " +
                    "@InstallationWith4xLoad_SampleDetails, @InstallationWith4xLoad_Result, " +
                    "@InstallationWithSandBagLoad_SampleDetails, @InstallationWithSandBagLoad_Result, " +
                    "@InstallationWithSandBagLoad2_SampleDetails, @InstallationWithSandBagLoad2_Result, " +
                    "@Photo_WithLoad, @Photo_WithoutLoad, " +
                    "@OverallResult, @CheckedBy, @VerifiedBy, @UpdatedBy, @UpdatedOn",
                    parameters
                );

                return new OperationResult
                {
                    Success = rows > 0,
                    Message = rows > 0 ? "Updated successfully." : "Update failed (0 rows affected)."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                throw;
            }
        }

        public async Task<OperationResult> DeleteInstallationTrailAsync(int Id)
        {
            try
            {
                var result = await base.DeleteAsync<InstallationTrial>(Id);
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
