using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;

namespace QMS.Core.Repositories.ChangeNoteItemsRepository;

public class ChangeNoteItemsRepository : SqlTableRepository, IChangeNoteItemsRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public ChangeNoteItemsRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<ChangeNoteItemsViewModel>> GetChangeNoteItemssDetailsAsync(int changeNoteId)
    {
        try
        {
            var parameters = new[] { new SqlParameter("@ChangeNoteId", changeNoteId) };
            var sql = @"EXEC sp_Get_ChangeNoteItems @ChangeNoteId";

            var result = await Task.Run(() => _dbContext.ChangeNoteItems.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new ChangeNoteItemsViewModel
                {
                    Id = x.Id,
                    ChangeNoteId = x.ChangeNoteId,
                    ChangeFrom = x.ChangeFrom,
                    ChangeTo = x.ChangeTo,
                    Category = x.Category
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

    public async Task<OperationResult> InsertChangeNoteItemsAsync(ChangeNoteItemsViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ChangeNoteId", model.ChangeNoteId),
                new SqlParameter("@ChangeFrom", model.ChangeFrom ?? (object)DBNull.Value),
                new SqlParameter("@ChangeTo", model.ChangeTo ?? (object)DBNull.Value),
                new SqlParameter("@Category", model.Category ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_ChangeNoteItem @ChangeNoteId, @ChangeFrom, @ChangeTo, @Category",
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

    public async Task<OperationResult> UpdateChangeNoteItemsAsync(ChangeNoteItemsViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ItemId", model.Id),
                new SqlParameter("@ChangeFrom", model.ChangeFrom ?? (object)DBNull.Value),
                new SqlParameter("@ChangeTo", model.ChangeTo ?? (object)DBNull.Value),
                new SqlParameter("@Category", model.Category ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_ChangeNoteItem @ItemId, @ChangeFrom, @ChangeTo, @Category",
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

    public async Task<OperationResult> DeleteChangeNoteItemsAsync(int changeNoteId)
    {
        try
        {
            var result = new OperationResult();
            var items = await _dbContext.ChangeNoteItems.Where(i => i.ChangeNoteId == changeNoteId).ToListAsync();
            if (items.Count == 0)
            {
                result.Success = true;
            }
            foreach (var item in items)
            {
                result = await base.DeletePermanentlyAsync<ChangeNoteItem>(item.Id);
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
