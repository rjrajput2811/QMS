using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.DeviationNoteRepository;

public class DeviationNoteRepository : SqlTableRepository, IDeviationNoteRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public DeviationNoteRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<DeviationNoteViewModel>> GetDeviationNotesAsync()
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

            var sql = @"EXEC sp_Get_DeviationNote";

            var result = await Task.Run(() => _dbContext.DeviationNotes.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new DeviationNoteViewModel
                {
                    Id = x.Id,
                    DateOfIssue = x.DateOfIssue,
                    DocumentNo = x.DocumentNo,
                    CatRefProduct = x.CatRefProduct,
                    DeviationDetails = x.DeviationDetails,
                    VendorId = x.VendorId,
                    AddedBy = x.AddedBy
                })
                .ToList());

            foreach (var rec in result)
            {
                rec.User = await _dbContext.User.Where(i => i.Id == rec.AddedBy).Select(x => x.Name).FirstOrDefaultAsync();
                rec.VendorName = await _dbContext.Vendor.Where(i => i.Id == rec.VendorId).Select(x => x.Name).FirstOrDefaultAsync();
                rec.DeviationNoteCategory = await _dbContext.DeviationNoteItems.Where(i => i.DeviationNoteId == rec.Id).Select(x => x.Category).FirstOrDefaultAsync();
            }

            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }

    public async Task<DeviationNoteViewModel> GetDeviationNotesDetailsAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_DeviationNote @Id";

            var result = await Task.Run(() => _dbContext.DeviationNotes.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new DeviationNoteViewModel
                {
                    Id = x.Id,
                    DocumentNo = x.DocumentNo,
                    CatRefProduct = x.CatRefProduct,
                    DeviationDetails = x.DeviationDetails,
                    VendorId = x.VendorId,
                    VendorName = _dbContext.Vendor.Where(i => i.Id == x.VendorId).Select(x => x.Name).FirstOrDefault(),
                    Remarks = x.Remarks,
                    CopyTo = x.CopyTo,
                    SignatureDate = x.SignatureDate,
                    DeviationNoteGroup = x.DeviationNoteGroup,
                    DeviationNoteRefNo = x.DeviationNoteRefNo,
                    DateOfIssue = x.DateOfIssue,
                    VendorQAInChargeId = x.VendorQAInChargeId,
                    VendorQAInChargeName = _dbContext.Vendor.Where(i => i.Id == x.VendorQAInChargeId).Select(x => x.Name).FirstOrDefault(),
                    FinalQARemarks = x.FinalQARemarks
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

    public async Task<int> InsertDeviationNoteAsync(DeviationNoteViewModel model)
    {
        try
        {
            var outIdParam = new SqlParameter("@NewId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
                new SqlParameter("@DocumentNo", model.DocumentNo ?? (object)DBNull.Value),
                new SqlParameter("@CatRefProduct", model.CatRefProduct ?? (object)DBNull.Value),
                new SqlParameter("@DeviationDetails", model.DeviationDetails ?? (object)DBNull.Value),
                new SqlParameter("@VendorId", model.VendorId),
                new SqlParameter("@Remarks", model.Remarks ?? (object)DBNull.Value),
                new SqlParameter("@CopyTo", model.CopyTo ?? (object)DBNull.Value),
                new SqlParameter("@SignatureDate", model.SignatureDate ?? (object)DBNull.Value),
                new SqlParameter("@DeviationNoteGroup", model.DeviationNoteGroup ?? (object)DBNull.Value),
                new SqlParameter("@DeviationNoteRefNo", model.DeviationNoteRefNo ?? (object)DBNull.Value),
                new SqlParameter("@DateOfIssue", model.DateOfIssue ?? (object)DBNull.Value),
                new SqlParameter("@VendorQAInChargeId", model.VendorQAInChargeId ?? (object)DBNull.Value),
                new SqlParameter("@FinalQARemarks", model.FinalQARemarks ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn),
                outIdParam // Output parameter to capture the new ID
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_DeviationNote " +
                "@DocumentNo, @CatRefProduct, @DeviationDetails, @VendorId, @Remarks, @CopyTo, " +
                "@SignatureDate, @DeviationNoteGroup, @DeviationNoteRefNo, @DateOfIssue, " +
                "@VendorQAInChargeId, @FinalQARemarks, @AddedBy, @AddedOn, @NewId OUT",
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

    public async Task<OperationResult> UpdateDeviationNoteAsync(DeviationNoteViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@DocumentNo", model.DocumentNo ?? (object)DBNull.Value),
                new SqlParameter("@CatRefProduct", model.CatRefProduct ?? (object)DBNull.Value),
                new SqlParameter("@DeviationDetails", model.DeviationDetails ?? (object)DBNull.Value),
                new SqlParameter("@VendorId", model.VendorId),
                new SqlParameter("@Remarks", model.Remarks ?? (object)DBNull.Value),
                new SqlParameter("@CopyTo", model.CopyTo ?? (object)DBNull.Value),
                new SqlParameter("@SignatureDate", model.SignatureDate ?? (object)DBNull.Value),
                new SqlParameter("@DeviationNoteGroup", model.DeviationNoteGroup ?? (object)DBNull.Value),
                new SqlParameter("@DeviationNoteRefNo", model.DeviationNoteRefNo ?? (object)DBNull.Value),
                new SqlParameter("@DateOfIssue", model.DateOfIssue ?? (object)DBNull.Value),
                new SqlParameter("@VendorQAInChargeId", model.VendorQAInChargeId ?? (object)DBNull.Value),
                new SqlParameter("@FinalQARemarks", model.FinalQARemarks ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_DeviationNote " +
                "@Id, @DocumentNo, @CatRefProduct, @DeviationDetails, @VendorId, @Remarks, @CopyTo, " +
                "@SignatureDate, @DeviationNoteGroup, @DeviationNoteRefNo, @DateOfIssue, " +
                "@VendorQAInChargeId, @FinalQARemarks, @UpdatedBy, @UpdatedOn",
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

    public async Task<OperationResult> DeleteDeviationNoteAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<DeviationNote>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}