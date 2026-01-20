using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;

namespace QMS.Core.Repositories.DeviationNoteItemsRepository;

public class DeviationNoteItemsRepository : SqlTableRepository, IDeviationNoteItemsRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public DeviationNoteItemsRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<DeviationNoteItemsViewModel>> GetDeviationNoteItemssDetailsAsync(int DeviationNoteId)
    {
        try
        {
            var parameters = new[] { new SqlParameter("@DeviationNoteId", DeviationNoteId) };
            var sql = @"EXEC sp_Get_DeviationNoteItems @DeviationNoteId";

            var result = await Task.Run(() => _dbContext.DeviationNoteItems.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new DeviationNoteItemsViewModel
                {
                    Id = x.Id,
                    DeviationNoteId = x.DeviationNoteId,
                    StandardPractice = x.StandardPractice,
                    Deviation = x.Deviation,
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

    public async Task<OperationResult> InsertDeviationNoteItemsAsync(DeviationNoteItemsViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@DeviationNoteId", model.DeviationNoteId),
                new SqlParameter("@StandardPractice", model.StandardPractice ?? (object)DBNull.Value),
                new SqlParameter("@Deviation", model.Deviation ?? (object)DBNull.Value),
                new SqlParameter("@Category", model.Category ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_DeviationNoteItem @DeviationNoteId, @StandardPractice, @Deviation, @Category",
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

    public async Task<OperationResult> UpdateDeviationNoteItemsAsync(DeviationNoteItemsViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@StandardPractice", model.StandardPractice ?? (object)DBNull.Value),
                new SqlParameter("@Deviation", model.Deviation ?? (object)DBNull.Value),
                new SqlParameter("@Category", model.Category ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_DeviationNoteItem @Id, @StandardPractice, @Deviation, @Category",
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

    public async Task<OperationResult> DeleteDeviationNoteItemsAsync(int DeviationNoteId)
    {
        try
        {
            var result = new OperationResult();
            var items = await _dbContext.DeviationNoteItems.Where(i => i.DeviationNoteId == DeviationNoteId).ToListAsync();
            if(items.Count == 0)
            {
                result.Success = true;
            }
            foreach (var item in items)
            {
                result = await base.DeletePermanentlyAsync<DeviationNoteItem>(item.Id);
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
