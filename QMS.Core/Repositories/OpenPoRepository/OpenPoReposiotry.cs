using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QMS.Core.Repositories.OpenPoRepository
{
    public class OpenPoReposiotry : SqlTableRepository, IOpenPoReposiotry
    {
        public new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public OpenPoReposiotry(QMSDbContext dbContext,
            ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<Open_PoViewModel>> GetListAsync()
        {
            try
            {

                var result = await _dbContext.OpenPo.FromSqlRaw("EXEC sp_Get_Open_PO_Details").ToListAsync();

                // Map results to ViewModel
                var viewModelList = result.Select(data => new Open_PoViewModel
                {
                    Id = data.Id,
                    Key = data.Key,
                    PR_Type = data.PR_Type,
                    PR_Desc = data.PR_Desc,
                    Requisitioner = data.Requisitioner,
                    Tracking_No = data.Tracking_No,
                    PR_No = data.PR_No,
                    Batch_No = data.Batch_No,
                    Reference_No = data.Reference_No,
                    Vendor = data.Vendor,
                    PO_No = data.PO_No,
                    PO_Date = data.PO_Date,
                    PO_Qty = data.PO_Qty,
                    Balance_Qty = data.Balance_Qty,
                    Destination = data.Destination,
                    Delivery_Date = data.Delivery_Date,
                    Balance_Value = data.Balance_Value,
                    Material = data.Material,
                    Hold_Date = data.Hold_Date,
                    Cleared_Date = data.Cleared_Date,
                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<BulkCreateLogResult> BulkCreateAsync(List<Open_PoViewModel> listOfData, string fileName, string uploadedBy)
        {
            var result = new BulkCreateLogResult();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var seenKeys = new HashSet<string>(); // for tracking in-batch duplicates

                foreach (var item in listOfData)
                {
                    item.Key = item.Key?.Trim();
                    item.PO_No = item.PO_No?.Trim();

                    var compositeKey = $"{item.Key}|{item.PO_No}";

                    if (string.IsNullOrWhiteSpace(item.Key) || string.IsNullOrWhiteSpace(item.PO_No))
                    {
                        result.FailedRecords.Add((item, "Missing Key or PO_No"));
                        continue;
                    }

                    // Check if this key combination has already been seen in the batch
                    if (seenKeys.Contains(compositeKey))
                    {
                        result.FailedRecords.Add((item, "Duplicate in uploaded file"));
                        continue;
                    }

                    seenKeys.Add(compositeKey); // Mark this combination as seen

                    // Now check against database
                    bool existsInDb = await _dbContext.OpenPo
                        .AnyAsync(x => x.Key == item.Key && x.PO_No == item.PO_No);

                    if (existsInDb)
                    {
                        result.FailedRecords.Add((item, "Duplicate in database"));
                        continue;
                    }

                    // Passed all checks — add to DB
                    var entity = new Open_Po
                    {
                        Key = item.Key,
                        PR_Type = item.PR_Type,
                        PR_Desc = item.PR_Desc,
                        Requisitioner = item.Requisitioner,
                        Tracking_No = item.Tracking_No,
                        PR_No = item.PR_No,
                        Batch_No = item.Batch_No,
                        Reference_No = item.Reference_No,
                        Vendor = item.Vendor,
                        PO_No = item.PO_No,
                        PO_Date = item.PO_Date,
                        PO_Qty = item.PO_Qty,
                        Balance_Qty = item.Balance_Qty,
                        Destination = item.Destination,
                        Delivery_Date = item.Delivery_Date,
                        Balance_Value = item.Balance_Value,
                        Material = item.Material,
                        Hold_Date = item.Hold_Date,
                        Cleared_Date = item.Cleared_Date,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    };

                    _dbContext.OpenPo.Add(entity);
                }

                await _dbContext.SaveChangesAsync();

                // Log the upload
                var importLog = new Open_Po_Log
                {
                    FileName = fileName,
                    TotalRecords = listOfData.Count,
                    ImportedRecords = listOfData.Count - result.FailedRecords.Count,
                    FailedRecords = result.FailedRecords.Count,
                    UploadedBy = uploadedBy,
                    UploadedAt = DateTime.Now
                };

                _dbContext.OpenPo_Log.Add(importLog);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Result = new OperationResult
                {
                    Success = true,
                    Message = result.FailedRecords.Any()
                        ? "Import completed with some skipped records."
                        : "All records imported successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Result = new OperationResult
                {
                    Success = false,
                    Message = "Error during import: " + ex.Message
                };
            }

            return result;
        }


    }
}
