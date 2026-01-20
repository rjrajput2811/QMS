using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;
namespace QMS.Core.Repositories.SurgeTestReportRepository;

public class SurgeTestReportRepository : SqlTableRepository, ISurgeTestReportRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public SurgeTestReportRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<SurgeTestReportViewModel>> GetSurgeTestReportAsync()
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

            var sql = @"EXEC sp_Get_SurgeTestReport";

            var result = await Task.Run(() => _dbContext.SurgeTestReports.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new SurgeTestReportViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    ReportDate = x.ReportDate,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    DriverCode = x.DriverCode,
                    SPDCode = x.SPDCode,
                    LEDConfiguration = x.LEDConfiguration,
                    BatchCode = x.BatchCode,
                    PKDCode = x.PKDCode,
                    ReferenceStandard = x.ReferenceStandard,
                    AcceptanceNorm = x.AcceptanceNorm,
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

    public async Task<SurgeTestReportViewModel> GetSurgeTestReportByIdAsync(int id)
    {
        try
        {
            var parameters = new[]
            {
            new SqlParameter("@Id", SqlDbType.Int) { Value = id }
        };

            var sql = @"EXEC sp_Get_SurgeTestReport @Id";

            var model = await Task.Run(() => _dbContext.SurgeTestReports
                .FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new SurgeTestReportViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    ReportDate = x.ReportDate,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    DriverCode = x.DriverCode,
                    SPDCode = x.SPDCode,
                    LEDConfiguration = x.LEDConfiguration,
                    BatchCode = x.BatchCode,
                    PKDCode = x.PKDCode,
                    ReferenceStandard = x.ReferenceStandard,
                    AcceptanceNorm = x.AcceptanceNorm,
                    AddedBy = x.AddedBy,
                    AddedOn = x.AddedOn,
                    CheckedBy = x.CheckedBy,
                    VerifiedBy = x.VerifiedBy
                })
                .FirstOrDefault());

            if (model == null)
            {
                return null;
            }

            model.User = await _dbContext.User.Where(u => u.Id == model.AddedBy).Select(u => u.Name).FirstOrDefaultAsync();

            model.DetailRows = await _dbContext.SurgeTestReportDetails
                .Where(d => d.ReportId == id)
                .OrderBy(d => d.RowNo)
                .Select(d => new SurgeTestDetailVM
                {
                    //DetailId = d.Id,
                    ReportId = d.ReportId,
                    TestType = d.TestType,
                    RowNo = d.RowNo,
                    IsResult = d.IsResult,
                    Voltage_KV = d.Voltage_KV,
                    Mode = d.Mode,
                    L_N_DM_90 = d.L_N_DM_90,
                    L_N_DM_180 = d.L_N_DM_180,
                    L_N_DM_270 = d.L_N_DM_270,
                    L_N_DM_0 = d.L_N_DM_0,
                    L_E_CM_90 = d.L_E_CM_90,
                    L_E_CM_180 = d.L_E_CM_180,
                    L_E_CM_270 = d.L_E_CM_270,
                    L_E_CM_0 = d.L_E_CM_0,
                    N_E_CM_90 = d.N_E_CM_90,
                    N_E_CM_180 = d.N_E_CM_180,
                    N_E_CM_270 = d.N_E_CM_270,
                    N_E_CM_0 = d.N_E_CM_0,
                    Observation = d.Observation,
                    PassFail = d.PassFail,
                    SPD_OK = d.SPD_OK,
                    Driver_LED_PCB_OK = d.Driver_LED_PCB_OK
                }).ToListAsync();

            return model;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> InsertUpdateSurgeTestReportAsync(SurgeTestReportViewModel model)
    {
        var result = new OperationResult();
        try
        {
            // ----------------- Parent SP -----------------
            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                    new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                    new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                    new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                    new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                    new SqlParameter("@DriverCode", model.DriverCode ?? (object)DBNull.Value),
                    new SqlParameter("@SPDCode", model.SPDCode ?? (object)DBNull.Value),
                    new SqlParameter("@LEDConfiguration", model.LEDConfiguration ?? (object)DBNull.Value),
                    new SqlParameter("@PKDCode", model.PKDCode ?? (object)DBNull.Value),
                    new SqlParameter("@ReferenceStandard", model.ReferenceStandard ?? (object)DBNull.Value),
                    new SqlParameter("@AcceptanceNorm", model.AcceptanceNorm ?? (object)DBNull.Value),
                    new SqlParameter("@Surge_Photo", model.Surge_Photo ?? (object)DBNull.Value),
                    new SqlParameter("@CheckedBy", model.CheckedBy ?? (object)DBNull.Value),
                    new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),
                    new SqlParameter("@AddedBy", model.Id > 0 ? (object)DBNull.Value : model.AddedBy),
                    new SqlParameter("@AddedOn", model.Id > 0 ? (object)DBNull.Value : model.AddedOn),
                    new SqlParameter("@UpdatedBy", model.UpdatedBy),
                    new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value),
                    new SqlParameter
                    {
                        ParameterName = "@OutReportId",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC Usp_Insert_Update_SurgeTestReport" +
                " @Id, @ReportDate, @ReportNo, @BatchCode, @ProductCatRef, @ProductDescription, @DriverCode, @SPDCode, @LEDConfiguration, @PKDCode," +
                " @ReferenceStandard, @AcceptanceNorm, @Surge_Photo, @CheckedBy, @VerifiedBy, @AddedBy, @AddedOn, @UpdatedBy, @UpdatedOn, @OutReportId OUT",
                parameters.ToArray()
            );

            int reportId = (int)parameters.First(p => p.ParameterName == "@OutReportId").Value;

            // ----------------- Child SP -----------------
            if (model.DetailRows != null && model.DetailRows.Any())
            {

                foreach (var detail in model.DetailRows)
                {
                    var childParams = new List<SqlParameter>
                        {
                            new SqlParameter("@ReportId", reportId),
                            new SqlParameter("@TestType", detail.TestType ?? (object)DBNull.Value),
                            new SqlParameter("@RowNo", detail.RowNo),
                            new SqlParameter("@IsResult", detail.IsResult),

                            new SqlParameter("@Voltage_KV", detail.Voltage_KV ?? (object)DBNull.Value),
                            new SqlParameter("@Mode", detail.Mode ?? (object)DBNull.Value),

                            new SqlParameter("@L_N_DM_90", detail.L_N_DM_90 ?? (object)DBNull.Value),
                            new SqlParameter("@L_N_DM_180", detail.L_N_DM_180 ?? (object)DBNull.Value),
                            new SqlParameter("@L_N_DM_270", detail.L_N_DM_270 ?? (object)DBNull.Value),
                            new SqlParameter("@L_N_DM_0", detail.L_N_DM_0 ?? (object)DBNull.Value),

                            new SqlParameter("@L_E_CM_90", detail.L_E_CM_90 ?? (object)DBNull.Value),
                            new SqlParameter("@L_E_CM_180", detail.L_E_CM_180 ?? (object)DBNull.Value),
                            new SqlParameter("@L_E_CM_270", detail.L_E_CM_270 ?? (object)DBNull.Value),
                            new SqlParameter("@L_E_CM_0", detail.L_E_CM_0 ?? (object)DBNull.Value),

                            new SqlParameter("@N_E_CM_90", detail.N_E_CM_90 ?? (object)DBNull.Value),
                            new SqlParameter("@N_E_CM_180", detail.N_E_CM_180 ?? (object)DBNull.Value),
                            new SqlParameter("@N_E_CM_270", detail.N_E_CM_270 ?? (object)DBNull.Value),
                            new SqlParameter("@N_E_CM_0", detail.N_E_CM_0 ?? (object)DBNull.Value),

                            new SqlParameter("@Observation", detail.Observation ?? (object)DBNull.Value),
                            new SqlParameter("@PassFail", detail.PassFail ?? (object)DBNull.Value),
                            new SqlParameter("@SPD_OK", detail.SPD_OK ?? (object)DBNull.Value),
                            new SqlParameter("@Driver_LED_PCB_OK", detail.Driver_LED_PCB_OK ?? (object)DBNull.Value)
                        };

                    await _dbContext.Database.ExecuteSqlRawAsync(
                        "EXEC Usp_Insert_SurgeTestReportDetail " +
                        "@ReportId, @TestType, @RowNo, @IsResult, @Voltage_KV, @Mode," +
                        " @L_N_DM_90, @L_N_DM_180, @L_N_DM_270, @L_N_DM_0, " +
                        "@L_E_CM_90, @L_E_CM_180, @L_E_CM_270, @L_E_CM_0, " +
                        "@N_E_CM_90, @N_E_CM_180, @N_E_CM_270, @N_E_CM_0, " +
                        "@Observation, @PassFail, @SPD_OK, @Driver_LED_PCB_OK",
                        childParams.ToArray()
                    );
                }
            }

            return new OperationResult { Success = true };
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteSurgeTestReportAsync(int id)
    {
        try
        {
            var rows = await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC usp_DeleteSurgeTestReport @ReportId",
                new SqlParameter("@ReportId", id)
            );

            return new OperationResult
            {
                Success = rows > 0
            };
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            return new OperationResult
            {
                Success = false
            };
        }
    }
}

