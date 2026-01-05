using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.ChangeNoteRepository;

public class ChangeNoteRepository : SqlTableRepository, IChangeNoteRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public ChangeNoteRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<ChangeNoteViewModel>> GetChangeNotesAsync()
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

            var sql = @"EXEC sp_Get_ChangeNote";

            var result = await Task.Run(() => _dbContext.ChangeNotes.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new ChangeNoteViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    VendorId = x.VendorId,
                    ChangeNoteCategory = x.ChangeNoteCategory,
                    AddedBy = x.AddedBy
                })
                .ToList());

            foreach (var rec in result)
            {
                rec.User = await _dbContext.User.Where(i => i.Id == rec.AddedBy).Select(x => x.Name).FirstOrDefaultAsync();
                rec.VendorName = await _dbContext.Vendor.Where(i => i.Id == rec.VendorId).Select(x => x.Name).FirstOrDefaultAsync();
            }

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<ChangeNoteViewModel> GetChangeNotesDetailsAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_ChangeNote @Id";

            var result = await Task.Run(() => _dbContext.ChangeNotes.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new ChangeNoteViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    VendorId = x.VendorId,
                    Remarks = x.Remarks,
                    ChangeNoteCategory = x.ChangeNoteCategory,
                    CopyTo = x.CopyTo,
                    SignatureFilePath = x.SignatureFilePath,
                    SignatureDate = x.SignatureDate,
                    ChangeNoteGroup = x.ChangeNoteGroup,
                    ChangeNoteRefNo = x.ChangeNoteRefNo,
                    DateOfIssue = x.DateOfIssue,
                    VendorQAInChargeId = x.VendorQAInChargeId,
                    FinalQARemarks = x.FinalQARemarks,
                    FinalSignatureFilePath = x.FinalSignatureFilePath
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

    public async Task<int> InsertChangeNoteAsync(ChangeNoteViewModel model)
    {
        try
        {
            var outIdParam = new SqlParameter("@NewId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
                new SqlParameter("@Description", model.Description ?? (object)DBNull.Value),
                new SqlParameter("@VendorId", model.VendorId),
                new SqlParameter("@Remarks", model.Remarks ?? (object)DBNull.Value),
                new SqlParameter("@ChangeNoteCategory", model.ChangeNoteCategory ?? (object)DBNull.Value),
                new SqlParameter("@CopyTo", model.CopyTo ?? (object)DBNull.Value),
                new SqlParameter("@SignatureFilePath", model.SignatureFilePath ?? (object)DBNull.Value),
                new SqlParameter("@SignatureDate", model.SignatureDate ?? (object)DBNull.Value),
                new SqlParameter("@ChangeNoteGroup", model.ChangeNoteGroup ?? (object)DBNull.Value),
                new SqlParameter("@ChangeNoteRefNo", model.ChangeNoteRefNo ?? (object)DBNull.Value),
                new SqlParameter("@DateOfIssue", model.DateOfIssue ?? (object)DBNull.Value),
                new SqlParameter("@VendorQAInChargeId", model.VendorQAInChargeId ?? (object)DBNull.Value),
                new SqlParameter("@FinalQARemarks", model.FinalQARemarks ?? (object)DBNull.Value),
                new SqlParameter("@FinalSignatureFilePath", model.FinalSignatureFilePath ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn),
                outIdParam // Output parameter to capture the new ID
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_ChangeNote " +
                "@Description, @VendorId, @Remarks, @ChangeNoteCategory, @CopyTo, " +
                "@SignatureFilePath, @SignatureDate, @ChangeNoteGroup, @ChangeNoteRefNo, " +
                "@DateOfIssue, @VendorQAInChargeId, @FinalQARemarks, @FinalSignatureFilePath, " +
                "@AddedBy, @AddedOn, @NewId OUT",
                parameters
            );

            // Retrieve the newly created ID
            return (int)outIdParam.Value;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<OperationResult> UpdateChangeNoteAsync(ChangeNoteViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@Description", model.Description ?? (object)DBNull.Value),
                new SqlParameter("@VendorId", model.VendorId),
                new SqlParameter("@Remarks", model.Remarks ?? (object)DBNull.Value),
                new SqlParameter("@ChangeNoteCategory", model.ChangeNoteCategory ?? (object)DBNull.Value),
                new SqlParameter("@CopyTo", model.CopyTo ?? (object)DBNull.Value),
                new SqlParameter("@SignatureFilePath", model.SignatureFilePath ?? (object)DBNull.Value),
                new SqlParameter("@SignatureDate", model.SignatureDate ?? (object)DBNull.Value),
                new SqlParameter("@ChangeNoteGroup", model.ChangeNoteGroup ?? (object)DBNull.Value),
                new SqlParameter("@ChangeNoteRefNo", model.ChangeNoteRefNo ?? (object)DBNull.Value),
                new SqlParameter("@DateOfIssue", model.DateOfIssue ?? (object)DBNull.Value),
                new SqlParameter("@VendorQAInChargeId", model.VendorQAInChargeId ?? (object)DBNull.Value),
                new SqlParameter("@FinalQARemarks", model.FinalQARemarks ?? (object)DBNull.Value),
                new SqlParameter("@FinalSignatureFilePath", model.FinalSignatureFilePath ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_ChangeNote " +
                "@Id, @Description, @VendorId, @Remarks, @ChangeNoteCategory, @CopyTo, " +
                "@SignatureFilePath, @SignatureDate, @ChangeNoteGroup, @ChangeNoteRefNo, " +
                "@DateOfIssue, @VendorQAInChargeId, @FinalQARemarks, @FinalSignatureFilePath, " +
                "@UpdatedBy, @UpdatedOn",
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

    public async Task<OperationResult> DeleteChangeNoteAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<ChangeNote>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
