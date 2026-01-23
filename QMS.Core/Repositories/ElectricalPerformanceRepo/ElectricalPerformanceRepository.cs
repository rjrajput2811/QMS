using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.ElectricalPerformanceRepo;

public class ElectricalPerformanceRepository : SqlTableRepository, IElectricalPerformanceRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public ElectricalPerformanceRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<ElectricalPerformanceViewModel>> GetElectricalPerformancesAsync(DateTime? startDate = null, DateTime? endDate = null)
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

            var sql = @"EXEC sp_Get_ElectricalPerformance";

            var result = await Task.Run(() => _dbContext.ElectricalPerformances.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new ElectricalPerformanceViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    ReportDate = x.ReportDate,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    LightSourceDetails = x.LightSourceDetails,
                    DriverDetails = x.DriverDetails,
                    PCBDetails = x.PCBDetails,
                    LEDCombinations = x.LEDCombinations,
                    BatchCode = x.BatchCode,
                    SensorDetails = x.SensorDetails,
                    LampDetails = x.LampDetails,
                    PKD = x.PKD,
                    AddedBy = x.AddedBy,
                    UpdatedBy = x.UpdatedBy,
                    AddedOn = x.AddedOn,
                    UpdatedOn = x.UpdatedOn
                })
                .ToList());

                if (startDate.HasValue && endDate.HasValue)
                {
                    var s = startDate.Value.Date;
                    var e = endDate.Value.Date;

                result = result
                        .Where(d => d.AddedOn.Date >= s && d.AddedOn.Date <= e)
                        .ToList();
                }

            var userIds = result.Select(x => x.AddedBy).Distinct().ToList();

            var userMap = await _dbContext.User
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Name);

            foreach (var rec in result)
            {
                // rec.User = await _dbContext.User.Where(i => i.Id == rec.AddedBy).Select(x => x.Name).FirstOrDefaultAsync();
                rec.User = userMap.TryGetValue(rec.AddedBy, out var name) ? name : "";
            }

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<ElectricalPerformanceViewModel> GetElectricalPerformancesByIdAsync(int Id)
    {
        var model = new ElectricalPerformanceViewModel();

        using var connection = _dbContext.Database.GetDbConnection();
        using var command = connection.CreateCommand();

        command.CommandText = "sp_Get_ElectricalPerformance";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@Id", Id));

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        // -----------------------------
        // 1️⃣ FIRST RESULT SET (Parent)
        // -----------------------------
        if (await reader.ReadAsync())
        {
            model.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            model.ReportNo = reader["ReportNo"] as string;
            model.ReportDate = reader["ReportDate"] as DateTime?;
            model.ProductCatRef = reader["ProductCatRef"] as string;
            model.ProductDescription = reader["ProductDescription"] as string;
            model.LightSourceDetails = reader["LightSourceDetails"] as string;
            model.DriverDetails = reader["DriverDetails"] as string;
            model.PCBDetails = reader["PCBDetails"] as string;
            model.LEDCombinations = reader["LEDCombinations"] as string;
            model.BatchCode = reader["BatchCode"] as string;
            model.SensorDetails = reader["SensorDetails"] as string;
            model.LampDetails = reader["LampDetails"] as string;
            model.PKD = reader["PKD"] as string;
            model.OverallResult = reader["OverallResult"] as string;
            model.TestedByName = reader["TestedByName"] as string;
            model.VerifiedByName = reader["VerifiedByName"] as string;
        }

        // -----------------------------
        // 2️⃣ SECOND RESULT SET (Child)
        // -----------------------------
        if (await reader.NextResultAsync())
        {
            while (await reader.ReadAsync())
            {
                model.Details.Add(new ElectricalPerDetailsViewModal
                {
                    ElId = reader.GetInt32(reader.GetOrdinal("ElId")),
                    SampleNo = reader.GetInt32(reader.GetOrdinal("SampleNo")),
                    ConditionType = reader["ConditionType"] as string,
                    RowNo = reader.GetInt32(reader.GetOrdinal("RowNo")),
                    Vac = reader["Vac"] as string,
                    IacA = reader["IacA"] as string,
                    Wac = reader["Wac"] as string,
                    PF = reader["PF"] as string,
                    ATHD = reader["ATHD"] as string,
                    Vdc = reader["Vdc"] as string,
                    IdcA = reader["IdcA"] as string,
                    Wdc = reader["Wdc"] as string,
                    Eff = reader["Eff"] as string,
                    NoLoadV = reader["NoLoadV"] as string,
                    StartV = reader["StartV"] as string,
                    Result = reader["Result"] as string
                });
            }
        }

        return model;
    }


    public async Task<OperationResult> InsertElectricalPerformancesAsync(ElectricalPerformanceViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", System.Data.SqlDbType.DateTime)
                {
                    Value = model.ReportDate.HasValue ? model.ReportDate.Value : (object)DBNull.Value
                },
                new SqlParameter("@LightSourceDetails", model.LightSourceDetails ?? (object)DBNull.Value),
                new SqlParameter("@DriverDetails", model.DriverDetails ?? (object)DBNull.Value),

                new SqlParameter("@PCBDetails", model.PCBDetails ?? (object)DBNull.Value),
                new SqlParameter("@LEDCombinations", model.LEDCombinations ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@SensorDetails", model.SensorDetails ?? (object)DBNull.Value),
                new SqlParameter("@LampDetails", model.LampDetails ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),

                new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                new SqlParameter("@TestedByName", model.TestedByName ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedByName", model.VerifiedByName ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", System.Data.SqlDbType.DateTime)
                {
                    Value = model.AddedOn
                },

                    // ===== DETAILS STRING =====
                new SqlParameter("@ElectricalDetails", SqlDbType.NVarChar)
                {
                    Value = model.ElectricalDetails ?? (object)DBNull.Value
                }
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_ElectricalPerformance " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@ReportNo, " +
                    "@ReportDate, " +
                    "@LightSourceDetails, " +
                    "@DriverDetails, " +
                    "@PCBDetails, " +
                    "@LEDCombinations, " +
                    "@BatchCode, " +
                    "@SensorDetails, " +
                    "@LampDetails, " +
                    "@PKD, " +

                    "@OverallResult, " +
                    "@TestedByName, " +
                    "@VerifiedByName, " +
                    "@AddedBy, " +
                    "@AddedOn," +
                    "@ElectricalDetails",
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

    public async Task<OperationResult> UpdateElectricalPerformancesAsync(ElectricalPerformanceViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@LightSourceDetails", model.LightSourceDetails ?? (object)DBNull.Value),
                new SqlParameter("@DriverDetails", model.DriverDetails ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", System.Data.SqlDbType.DateTime)
                {
                    Value = model.ReportDate.HasValue ? model.ReportDate.Value : (object)DBNull.Value
                },
                new SqlParameter("@PCBDetails", model.PCBDetails ?? (object)DBNull.Value),
                new SqlParameter("@LEDCombinations", model.LEDCombinations ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@SensorDetails", model.SensorDetails ?? (object)DBNull.Value),
                new SqlParameter("@LampDetails", model.LampDetails ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),

                new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                new SqlParameter("@TestedByName", model.TestedByName ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedByName", model.VerifiedByName ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value),

                                    // ===== DETAILS STRING =====
                new SqlParameter("@ElectricalDetails", SqlDbType.NVarChar)
                {
                    Value = model.ElectricalDetails ?? (object)DBNull.Value
                }
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_ElectricalPerformance " +
                    "@Id, " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@ReportNo, " +
                    "@LightSourceDetails, " +
                    "@DriverDetails, " +
                    "@ReportDate, " +
                    "@PCBDetails, " +
                    "@LEDCombinations, " +
                    "@BatchCode, " +
                    "@SensorDetails, " +
                    "@LampDetails, " +
                    "@PKD, " +
                    "@OverallResult, " +
                    "@TestedByName, " +
                    "@VerifiedByName, " +
                    "@UpdatedBy, " +
                    "@UpdatedOn," +
                    "@ElectricalDetails",

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

    public async Task<OperationResult> DeleteElectricalPerformancesAsync(int Id)
    {

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ElectricalId", Id ),
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Delete_ElectricalPerformance " +
                    "@ElectricalId",  
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
