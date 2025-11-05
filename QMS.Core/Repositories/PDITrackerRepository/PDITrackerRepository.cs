using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;

namespace QMS.Core.Repositories.PDITrackerRepository
{
    public class PDITrackerRepository : SqlTableRepository, IPDITrackerRepository
    {
        private readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public PDITrackerRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }
        public async Task<List<DropdownOptionViewModel>> GetVendorDropdownAsync()
        {
            return await _dbContext.Vendor
                .Where(v => !v.Deleted)
                .Select(v => new DropdownOptionViewModel
                {
                    Label = v.Name,
                    Value = v.Vendor_Code
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<PDITrackerViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.PDITracker
                    .FromSqlRaw("EXEC sp_Get_PDI_Tracker")
                    .ToListAsync();
                // Apply date filtering before projecting to view models
                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.DispatchDate.HasValue &&
                                    x.DispatchDate.Value.Date >= startDate.Value.Date &&
                                    x.DispatchDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }
                return result.Select(data => new PDITrackerViewModel
                {
                    Id = data.Id,
                    DispatchDate = data.DispatchDate,
                    PC = data.PC,
                    ProductCode = data.ProductCode,
                    ProductDescription = data.ProductDescription,
                    BatchCodeVendor = data.BatchCodeVendor,
                    PONo = data.PONo,
                    PDIDate = data.PDIDate,
                    PDIRefNo = data.PDIRefNo,
                    OfferedQty = data.OfferedQty,
                    ClearedQty = data.ClearedQty,
                    BISCompliance = data.BISCompliance,
                    InspectedBy = data.InspectedBy,
                    Remark = data.Remark,
                    Attahcment = data.Attahcment,
                    IsDelete = data.Deleted,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate
                }).ToList();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }



        public async Task<PDITracker?> GetByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.PDITracker
                    .FromSqlRaw("EXEC sp_Get_PDITracker_ByID @PDIId", new SqlParameter("@PDIId", id))
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateAsync(PDITracker entity, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@DispatchDate", entity.DispatchDate ?? (object)DBNull.Value),
                    new SqlParameter("@PC", entity.PC ?? (object)DBNull.Value),
                    new SqlParameter("@ProductCode", entity.ProductCode ?? (object)DBNull.Value),
                    new SqlParameter("@ProductDescription", entity.ProductDescription ?? (object)DBNull.Value),
                    new SqlParameter("@BatchCodeVendor", entity.BatchCodeVendor ?? (object)DBNull.Value),
                    new SqlParameter("@PONo", entity.PONo ?? (object)DBNull.Value),
                    new SqlParameter("@PDIDate", entity.PDIDate ?? (object)DBNull.Value),
                    new SqlParameter("@PDIRefNo", entity.PDIRefNo ?? (object)DBNull.Value),
                    new SqlParameter("@OfferedQty", entity.OfferedQty ?? (object)DBNull.Value),
                    new SqlParameter("@ClearedQty", entity.ClearedQty ?? (object)DBNull.Value),
                    new SqlParameter("@BISCompliance", entity.BISCompliance ?? (object)DBNull.Value),
                    new SqlParameter("@InspectedBy", entity.InspectedBy ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", entity.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@Attahcment", entity.Attahcment ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", entity.Deleted)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_PDITracker @DispatchDate, @PC, @ProductCode, @ProductDescription, @BatchCodeVendor, @PONo, @PDIDate, @PDIRefNo, @OfferedQty, @ClearedQty, @BISCompliance, @InspectedBy, @Remark, @Attahcment, @CreatedBy, @IsDeleted",
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

        public async Task<OperationResult> UpdateAsync(PDITracker entity, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@PDIId", entity.Id),
                    new SqlParameter("@DispatchDate", entity.DispatchDate ?? (object)DBNull.Value),
                    new SqlParameter("@PC", entity.PC ?? (object)DBNull.Value),
                    new SqlParameter("@ProductCode", entity.ProductCode ?? (object)DBNull.Value),
                    new SqlParameter("@ProductDescription", entity.ProductDescription ?? (object)DBNull.Value),
                    new SqlParameter("@BatchCodeVendor", entity.BatchCodeVendor ?? (object)DBNull.Value),
                    new SqlParameter("@PONo", entity.PONo ?? (object)DBNull.Value),
                    new SqlParameter("@PDIDate", entity.PDIDate ?? (object)DBNull.Value),
                    new SqlParameter("@PDIRefNo", entity.PDIRefNo ?? (object)DBNull.Value),
                    new SqlParameter("@OfferedQty", entity.OfferedQty ?? (object)DBNull.Value),
                    new SqlParameter("@ClearedQty", entity.ClearedQty ?? (object)DBNull.Value),
                    new SqlParameter("@BISCompliance", entity.BISCompliance ?? (object)DBNull.Value),
                    new SqlParameter("@InspectedBy", entity.InspectedBy ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", entity.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@Attahcment", entity.Attahcment ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value)

                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Update_PDITracker @PDIId, @DispatchDate, @PC, @ProductCode, @ProductDescription, @BatchCodeVendor, @PONo, @PDIDate, @PDIRefNo, @OfferedQty, @ClearedQty, @BISCompliance, @InspectedBy, @Remark, @Attahcment, @UpdatedBy",
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

        public async Task<List<ProductCodeDetailViewModel>> GetCodeSearchAsync(string search = "")
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@OldPartNo", search),
                };

                var sql = @"EXEC sp_GetProductCode_Detail_ByCode @OldPartNo";

                var result = await _dbContext.ProductCode.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new ProductCodeDetailViewModel
                {
                    PCDetails_Id = data.PCDetails_Id,
                    OldPart_No = data.OldPart_No,
                    Description = data.Description
                }).ToList();

                return viewModelList; // Assuming you want a single view model based on the ID
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<PDITracker>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
        public async Task<List<DropdownOptionViewModel>> GetCodeSelect2OptionsAsync()
        {
           

            var sql = @"EXEC sp_GetProductCode_Detail";
            var result = await _dbContext.ProductCode.FromSqlRaw(sql).ToListAsync();

            return result.Select(data => new DropdownOptionViewModel
            {
                    Label = data.OldPart_No,
                    Value = data.OldPart_No
                })
                .Distinct() 
                .ToList();
        }

        public async Task<bool> UpdateAttachmentAsync(int id, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                var record = await _dbContext.PDITracker.FindAsync(id);
                if (record == null)
                    return false;

                record.Attahcment = fileName;

                // Only update the BIS_Attachment property
                _dbContext.Entry(record).Property(x => x.Attahcment).IsModified = true;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<List<BatchCodePDIViewModel>> GetBatchCodePDIAsync()
        {
            try
            {
                var result = await (from pr in _dbContext.BatchCode
                                    where pr.Deleted == false // Add this condition
                                    select new BatchCodePDIViewModel
                                    {
                                        Id = pr.Id,
                                        Vendor = pr.Vendor,
                                        Batch_Code = pr.Batch_Code,
                                        CreatedBy = pr.CreatedBy,
                                        CreatedDate = pr.CreatedDate,
                                        UpdatedBy = pr.UpdatedBy,
                                        UpdatedDate = pr.UpdatedDate
                                    }).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<BatchCode_PDI?> GetBatchCodePDIByIdAsync(int Id)
        {
            try
            {
                var result = await base.GetByIdAsync<BatchCode_PDI>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateBatchCodePDIAsync(BatchCodePDIViewModel newNatProjectRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var natToCreate = new BatchCode_PDI();
                natToCreate.Vendor = newNatProjectRecord.Vendor;
                natToCreate.Batch_Code = newNatProjectRecord.Batch_Code;
                natToCreate.CreatedBy = newNatProjectRecord.CreatedBy;
                natToCreate.CreatedDate = DateTime.Now;
                return await base.CreateAsync<BatchCode_PDI>(natToCreate, returnCreatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateBatchCodePDIAsync(BatchCodePDIViewModel updateNatProjectRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var natToCreate = await base.GetByIdAsync<BatchCode_PDI>(updateNatProjectRecord.Id);
                natToCreate.Vendor = updateNatProjectRecord.Vendor;
                natToCreate.Batch_Code = updateNatProjectRecord.Batch_Code;
                natToCreate.UpdatedBy = updateNatProjectRecord.UpdatedBy;
                natToCreate.UpdatedDate = DateTime.Now;
                return await base.UpdateAsync<BatchCode_PDI>(natToCreate, returnUpdatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteBatchCodePDIAsync(int Id)
        {
            try
            {
                return await base.DeleteAsync<BatchCode_PDI>(Id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckBatchCodePDIDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.BatchCode
                    .Where(x => x.Deleted == false && x.Batch_Code == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.BatchCode
                        .Where(x => x.Deleted == false &&
                               x.Batch_Code == searchText
                               && x.Id != Id)
                        .Select(x => x.Id);
                }


                existingId = await query.FirstOrDefaultAsync();

                if (existingId != null && existingId > 0)
                {
                    existingflag = true;
                }

                return existingflag;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<List<DropdownOptionViewModel>> GetBatchCodeDropdownAsync()
        {
            return await _dbContext.BatchCode
                .Where(v => !v.Deleted)
                .Select(v => new DropdownOptionViewModel
                {
                    Label = v.Batch_Code,
                    Value = v.Batch_Code
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
