using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;

namespace QMS.Core.Repositories.HydraulicTestObservationRepository;

public class HydraulicTestObservationRepository : SqlTableRepository, IHydraulicTestObservationRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public HydraulicTestObservationRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<HydraulicTestObservationReportViewModel>> GetHydraulicTestObservationDetailsAsync(int hydraulicTestReportId)
    {
        try
        {
            var parameters = new[] { new SqlParameter("@HydraulicTestReportId", hydraulicTestReportId) };
            var sql = @"EXEC sp_Get_HydraulicTestObservation @HydraulicTestReportId";

            var result = await Task.Run(() => _dbContext.HydraulicTestObservationReports.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new HydraulicTestObservationReportViewModel
                {
                    Id = x.Id,
                    PhotoBeforeTest = x.PhotoBeforeTest,
                    PhotoAfterTest = x.PhotoAfterTest,
                    Observation = x.Observation,
                    Result = x.Result
                })
                .ToList());

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> InsertHydraulicTestObservationAsync(HydraulicTestObservationReportViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@HydraulicTestReport_Id", model.HydraulicTestReport_Id),
                new SqlParameter("@PhotoBeforeTest", model.PhotoBeforeTest ?? (object)DBNull.Value),
                new SqlParameter("@PhotoAfterTest", model.PhotoAfterTest ?? (object)DBNull.Value),
                new SqlParameter("@Observation", model.Observation ?? (object)DBNull.Value),
                new SqlParameter("@Result", model.Result ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_HydraulicTestObservation @HydraulicTestReport_Id, @PhotoBeforeTest, " +
                "@PhotoAfterTest, @Observation, @Result",
                parameters);

            return new OperationResult { Success = true };
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> DeleteHydraulicTestObservationAsync(int hydraulicTestReportId)
    {
        try
        {
            var result = new OperationResult();
            var items = await _dbContext.HydraulicTestObservationReports.Where(i => i.HydraulicTestReport_Id == hydraulicTestReportId).ToListAsync();
            if (items.Count == 0)
            {
                result.Success = true;
            }
            foreach (var item in items)
            {
                result = await base.DeletePermanentlyAsync<HydraulicTestObservationReport>(item.Id);
                if (!result.Success) { return result; }
            }
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
