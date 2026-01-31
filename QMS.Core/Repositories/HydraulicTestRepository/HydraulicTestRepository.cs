using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.HydraulicTestRepository;

public class HydraulicTestRepository : SqlTableRepository, IHydraulicTestRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public HydraulicTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<HydraulicTestReportViewModel>> GetHydraulicTestReportAsync()
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", SqlDbType.Int)
                {
                    Value = 0
                },
            };

            var sql = @"EXEC sp_Get_HydraulicTestReport";

            var result = await Task.Run(() => _dbContext.HydraulicTestReports.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new HydraulicTestReportViewModel
                {
                    Id = x.Id,
                    ReportDate = x.ReportDate,
                    ReportNo = x.ReportNo,
                    ProductCatRef = x.ProductCatRef,
                    CustomerProjectName = x.CustomerProjectName,
                    ProductDescription = x.ProductDescription,
                    BatchCode = x.BatchCode,
                    Quantity = x.Quantity,
                    HydraulicTestPressure = x.HydraulicTestPressure,
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

    public async Task<HydraulicTestReportViewModel> GetHydraulicTestReportDetailsAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_HydraulicTestReport @Id";

            var result = await Task.Run(() => _dbContext.HydraulicTestReports.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new HydraulicTestReportViewModel
                {
                    Id = x.Id,
                    ReportDate = x.ReportDate,
                    ReportNo = x.ReportNo,
                    ProductCatRef = x.ProductCatRef,
                    CustomerProjectName = x.CustomerProjectName,
                    ProductDescription = x.ProductDescription,
                    BatchCode = x.BatchCode,
                    Quantity = x.Quantity,
                    HydraulicTestPressure = x.HydraulicTestPressure,
                    OverallResult = x.OverallResult,
                    TestedBy = x.TestedBy,
                    VerifiedBy = x.VerifiedBy,
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

    public async Task<int> InsertHydraulicTestReportAsync(HydraulicTestReportViewModel model)
    {
        try
        {
            var outIdParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var parameters = new[]
            {
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@CustomerProjectName", model.CustomerProjectName ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@Quantity", model.Quantity),
                new SqlParameter("@HydraulicTestPressure", model.HydraulicTestPressure ?? (object)DBNull.Value),
                new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                new SqlParameter("@TestedBy", model.TestedBy ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn),
                outIdParam
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_HydraulicTestReport @ReportNo, @CustomerProjectName, @ReportDate, @ProductCatRef, " +
                "@ProductDescription, @BatchCode, @Quantity, @HydraulicTestPressure, @OverallResult, " +
                "@TestedBy, @VerifiedBy, @AddedBy, @AddedOn, @NewId OUT",
                parameters);

            return (int)outIdParam.Value;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateHydraulicTestReportAsync(HydraulicTestReportViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@CustomerProjectName", model.CustomerProjectName ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@Quantity", model.Quantity),
                new SqlParameter("@HydraulicTestPressure", model.HydraulicTestPressure ?? (object)DBNull.Value),
                new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                new SqlParameter("@TestedBy", model.TestedBy ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_HydraulicTestReport @Id, @ReportNo, @CustomerProjectName, @ReportDate, " +
                "@ProductCatRef, @ProductDescription, @BatchCode, @Quantity, @HydraulicTestPressure, " +
                "@OverallResult, @TestedBy, @VerifiedBy, @UpdatedBy, @UpdatedOn",
                parameters);

            return new OperationResult { Success = true };
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteHydraulicTestReportAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<HydraulicTestReport>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
