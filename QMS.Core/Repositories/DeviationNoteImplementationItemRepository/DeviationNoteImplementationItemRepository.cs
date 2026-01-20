using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;

namespace QMS.Core.Repositories.DeviationNoteImplementationItemRepository;

public class DeviationNoteImplementationItemRepository : SqlTableRepository, IDeviationNoteImplementationItemRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public DeviationNoteImplementationItemRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<DeviationNoteImplementationItemViewModel>> GetDeviationNoteImplementationItemsDetailsAsync(int DeviationNoteId)
    {
        try
        {
            var parameters = new[] { new SqlParameter("@DeviationNoteId", DeviationNoteId) };
            var sql = @"EXEC sp_Get_DeviationNoteImplementationItem @DeviationNoteId";

            var result = await Task.Run(() => _dbContext.DeviationNoteImplementationItems.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new DeviationNoteImplementationItemViewModel
                {
                    Id = x.Id,
                    DeviationNoteId = x.DeviationNoteId,
                    ActionPlanned = x.ActionPlanned,
                    WhoWillDo = x.WhoWillDo,
                    ProposedCutOffDate = x.ProposedCutOffDate,
                    ActualDate = x.ActualDate
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

    public async Task<OperationResult> InsertDeviationNoteImplementationItemAsync(DeviationNoteImplementationItemViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@DeviationNoteId", model.DeviationNoteId),
                new SqlParameter("@ActionPlanned", model.ActionPlanned ?? (object)DBNull.Value),
                new SqlParameter("@WhoWillDo", model.WhoWillDo ?? (object)DBNull.Value),
                new SqlParameter("@ProposedCutOffDate", model.ProposedCutOffDate ?? (object)DBNull.Value),
                new SqlParameter("@ActualDate", model.ActualDate ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_DeviationNoteImplementationItem @DeviationNoteId, @ActionPlanned, @WhoWillDo, @ProposedCutOffDate, @ActualDate",
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

    public async Task<OperationResult> UpdateDeviationNoteImplementationItemAsync(DeviationNoteImplementationItemViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@ActionPlanned", model.ActionPlanned ?? (object)DBNull.Value),
                new SqlParameter("@WhoWillDo", model.WhoWillDo ?? (object)DBNull.Value),
                new SqlParameter("@ProposedCutOffDate", model.ProposedCutOffDate ?? (object)DBNull.Value),
                new SqlParameter("@ActualDate", model.ActualDate ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_DeviationNoteImplementationItem @Id, @ActionPlanned, @WhoWillDo, @ProposedCutOffDate, @ActualDate",
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

    public async Task<OperationResult> DeleteDeviationNoteImplementationItemAsync(int DeviationNoteId)
    {
        try
        {
            var result = new OperationResult();
            var items = await _dbContext.DeviationNoteImplementationItems.Where(i => i.DeviationNoteId == DeviationNoteId).ToListAsync();
            if (items.Count == 0)
            {
                result.Success = true;
            }
            foreach (var item in items)
            {
                result = await base.DeletePermanentlyAsync<DeviationNoteImplementationItem>(item.Id);
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
