using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.DatabaseContext.Shared;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.RippleTestReportRepo;

public class RippleTestReportRepository : SqlTableRepository, IRippleTestReportRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public RippleTestReportRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<RippleTestReportViewModel>> GetRippleTestReportsAsync()
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

            var sql = @"EXEC sp_Get_RippleTestReport";

            var result = await Task.Run(() => _dbContext.RippleTestReports.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new RippleTestReportViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    TestingDate = x.TestingDate,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    BatchCode = x.BatchCode,
                    PKD = x.PKD,
                    LEDDetails = x.LEDDetails,
                    LEDDriver = x.LEDDriver,
                    LEDCombination = x.LEDCombination,
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

    public async Task<RippleTestReportViewModel> GetRippleTestReportsByIdAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_RippleTestReport";

            var result = await Task.Run(() => _dbContext.RippleTestReports.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new RippleTestReportViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    TestingDate = x.TestingDate,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    BatchCode = x.BatchCode,
                    PKD = x.PKD,
                    LEDDetails = x.LEDDetails,
                    LEDDriver = x.LEDDriver,
                    LEDCombination = x.LEDCombination,
                    DeltaValue = x.DeltaValue,
                    RMSValue = x.RMSValue,
                    Calculation = x.Calculation,
                    RipplePercentage = x.RipplePercentage,
                    Result = x.Result,
                    TestedBy = x.TestedBy,
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

    public async Task<OperationResult> InsertRippleTestReportsAsync(RippleTestReportViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ReportNo", model.ReportNo  ?? (object)DBNull.Value),
                new SqlParameter("@TestingDate", model.TestingDate  ?? (object)DBNull.Value),
                new SqlParameter("@MeasuringInstrument", model.MeasuringInstrument  ?? (object)DBNull.Value),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD  ?? (object)DBNull.Value),
                new SqlParameter("@LEDDetails", model.LEDDetails?? (object)DBNull.Value),
                new SqlParameter("@LEDDriver", model.LEDDriver ?? (object)DBNull.Value),
                new SqlParameter("@LEDCombination", model.LEDCombination?? (object)DBNull.Value),
                new SqlParameter("@DeltaValue", model.DeltaValue?? (object)DBNull.Value),
                new SqlParameter("@RMSValue", model.RMSValue  ?? (object)DBNull.Value),
                new SqlParameter("@Calculation", model.Calculation  ?? (object)DBNull.Value),
                new SqlParameter("@RipplePercentage", model.RipplePercentage  ?? (object)DBNull.Value),
                new SqlParameter("@Result", model.Result?? (object)DBNull.Value),
                new SqlParameter("@TestedBy", model.TestedBy  ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedBy", model.VerifiedBy?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_RippleTestReport " +
                    "@ReportNo, " +
                    "@TestingDate, " +
                    "@MeasuringInstrument, " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@BatchCode, " +
                    "@PKD, " +
                    "@LEDDetails, " +
                    "@LEDDriver, " +
                    "@LEDCombination, " +
                    "@DeltaValue, " +
                    "@RMSValue, " +
                    "@Calculation, " +
                    "@RipplePercentage, " +
                    "@Result, " +
                    "@TestedBy, " +
                    "@VerifiedBy, " +
                    "@AddedBy, " +
                    "@AddedOn",
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

    public async Task<OperationResult> UpdateRippleTestReportsAsync(RippleTestReportViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@ReportNo", model.ReportNo  ?? (object)DBNull.Value),
                new SqlParameter("@TestingDate", model.TestingDate  ?? (object)DBNull.Value),
                new SqlParameter("@MeasuringInstrument", model.MeasuringInstrument  ?? (object)DBNull.Value),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD  ?? (object)DBNull.Value),
                new SqlParameter("@LEDDetails", model.LEDDetails?? (object)DBNull.Value),
                new SqlParameter("@LEDDriver", model.LEDDriver ?? (object)DBNull.Value),
                new SqlParameter("@LEDCombination", model.LEDCombination?? (object)DBNull.Value),
                new SqlParameter("@DeltaValue", model.DeltaValue?? (object)DBNull.Value),
                new SqlParameter("@RMSValue", model.RMSValue  ?? (object)DBNull.Value),
                new SqlParameter("@Calculation", model.Calculation  ?? (object)DBNull.Value),
                new SqlParameter("@RipplePercentage", model.RipplePercentage  ?? (object)DBNull.Value),
                new SqlParameter("@Result", model.Result?? (object)DBNull.Value),
                new SqlParameter("@TestedBy", model.TestedBy  ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedBy", model.VerifiedBy?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_RippleTestReport " +
                    "@Id, " +
                    "@ReportNo, " +
                    "@TestingDate, " +
                    "@MeasuringInstrument, " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@BatchCode, " +
                    "@PKD, " +
                    "@LEDDetails, " +
                    "@LEDDriver, " +
                    "@LEDCombination, " +
                    "@DeltaValue, " +
                    "@RMSValue, " +
                    "@Calculation, " +
                    "@RipplePercentage, " +
                    "@Result, " +
                    "@TestedBy, " +
                    "@VerifiedBy, " +
                    "@UpdatedBy, " +
                    "@UpdatedOn",
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

    public async Task<OperationResult> DeleteRippleTestReportsAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<RippleTestReport>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}