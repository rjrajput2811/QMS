using Dapper;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.MergeTrackerRepository;

public class MergeTrackerRepository : IMergeTrackerRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly ISystemLogService _systemLogService;

    public MergeTrackerRepository (IDbConnection dbConnection, ISystemLogService systemLogService)
    {
        _dbConnection = dbConnection;
        _systemLogService = systemLogService;
    }

    public async Task<MergeTrackerViewModel> GetSummaryOfAllActivityAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);

            var result = new MergeTrackerViewModel();

            using var multi = await _dbConnection.QueryMultipleAsync("sp_Get_Summary_Of_All_Activity", parameters, commandType: CommandType.StoredProcedure);
            result.Summary = await multi.ReadFirstOrDefaultAsync<ConsolidatedSummaryModel>();
            result.FifoDetails = (await multi.ReadAsync<FifoTrackerModel>()).ToList();
            result.CsatDetails = (await multi.ReadAsync<CsatSummaryModel>()).ToList();

            return result;

        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
