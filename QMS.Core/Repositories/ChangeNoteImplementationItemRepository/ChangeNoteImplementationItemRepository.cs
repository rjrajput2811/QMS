using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;

namespace QMS.Core.Repositories.ChangeNoteImplementationItemRepository;

public class ChangeNoteImplementationItemRepository : SqlTableRepository, IChangeNoteImplementationItemRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public ChangeNoteImplementationItemRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<ChangeNoteImplementationItemViewModel>> GetChangeNoteImplementationItemsDetailsAsync(int changeNoteId)
    {
        try
        {
            var parameters = new[] { new SqlParameter("@ChangeNoteId", changeNoteId) };
            var sql = @"EXEC sp_Get_ChangeNoteImplementationItems @ChangeNoteId";

            var result = await Task.Run(() => _dbContext.ChangeNoteImplementationItems.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new ChangeNoteImplementationItemViewModel
                {
                    Id = x.Id,
                    ChangeNoteId = x.ChangeNoteId,
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

    public async Task<OperationResult> InsertChangeNoteImplementationItemAsync(ChangeNoteImplementationItemViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ChangeNoteId", model.ChangeNoteId),
                new SqlParameter("@ActionPlanned", model.ActionPlanned ?? (object)DBNull.Value),
                new SqlParameter("@WhoWillDo", model.WhoWillDo ?? (object)DBNull.Value),
                new SqlParameter("@ProposedCutOffDate", model.ProposedCutOffDate ?? (object)DBNull.Value),
                new SqlParameter("@ActualDate", model.ActualDate ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_ChangeNoteImplementationItem @ChangeNoteId, @ActionPlanned, @WhoWillDo, @ProposedCutOffDate, @ActualDate",
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

    public async Task<OperationResult> UpdateChangeNoteImplementationItemAsync(ChangeNoteImplementationItemViewModel model)
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
                "EXEC sp_Update_ChangeNoteImplementationItem @Id, @ActionPlanned, @WhoWillDo, @ProposedCutOffDate, @ActualDate",
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

    public async Task<OperationResult> DeleteChangeNoteImplementationItemAsync(int changeNoteId)
    {
        try
        {
            var result = new OperationResult();
            var items = await _dbContext.ChangeNoteImplementationItems.Where(i => i.ChangeNoteId == changeNoteId).ToListAsync();
            foreach (var item in items)
            {
                result = await base.DeletePermanentlyAsync<ChangeNoteImplementationItem>(item.Id);
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
