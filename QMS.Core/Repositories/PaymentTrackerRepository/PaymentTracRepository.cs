using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.PaymentTrackerRepository
{
    public class PaymentTracRepository : SqlTableRepository, IPaymentTracRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public PaymentTracRepository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<PaymentTracViewModel>> GetListAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _dbContext.PaymentTracker.FromSqlRaw("EXEC sp_Get_PaymentTracker").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.Invoice_Date.HasValue &&
                                    x.Invoice_Date.Value.Date >= startDate.Value.Date &&
                                    x.Invoice_Date.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map results to ViewModel
                var viewModelList = result.Select(data => new PaymentTracViewModel
                {
                    Id = data.Id,
                    Fin_Year = data.Fin_Year,
                    Month = data.Month,
                    Lab = data.Lab,
                    Vendor = data.Vendor,
                    Type_Test = data.Type_Test,
                    Description = data.Description,
                    Bis_Id = data.Bis_Id,
                    Invoice_No = data.Invoice_No,
                    Invoice_Date = data.Invoice_Date,
                    Testing_Fee = data.Testing_Fee,
                    Approval_By = data.Approval_By,
                    Remark = data.Remark,
                    Attachment = data.Attachment,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<PaymentTracViewModel?> GetByIdAsync(int payTrac_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@PayTrac_Id", payTrac_Id),
                };

                var sql = @"EXEC sp_Get_PaymentTracker_ById @PayTrac_Id";

                var result = await _dbContext.PaymentTracker.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new PaymentTracViewModel
                {
                    Id = data.Id,
                    Fin_Year = data.Fin_Year,
                    Month = data.Month,
                    Lab = data.Lab,
                    Vendor = data.Vendor,
                    Type_Test = data.Type_Test,
                    Description = data.Description,
                    Bis_Id = data.Bis_Id,
                    Invoice_No = data.Invoice_No,
                    Invoice_Date = data.Invoice_Date,
                    Testing_Fee = data.Testing_Fee,
                    Approval_By = data.Approval_By,
                    Remark = data.Remark,
                    Attachment = data.Attachment,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate

                }).ToList();

                return viewModelList.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateAsync(Payment_Tracker newRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Fin_Year", newRecord.Fin_Year ?? (object)DBNull.Value),
                    new SqlParameter("@Month", newRecord.Month ?? (object)DBNull.Value),
                    new SqlParameter("@Lab", newRecord.Lab ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor", newRecord.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Type_Test", newRecord.Type_Test ?? (object)DBNull.Value),
                    new SqlParameter("@Description", newRecord.Description ?? (object)DBNull.Value),
                    new SqlParameter("@Bis_Id", newRecord.Bis_Id ?? (object)DBNull.Value),
                    new SqlParameter("@Invoice_No", newRecord.Invoice_No ?? (object)DBNull.Value),
                    new SqlParameter("@Invoice_Date", newRecord.Invoice_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Testing_Fee", newRecord.Testing_Fee),
                    new SqlParameter("@Approval_By", newRecord.Approval_By ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", newRecord.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", newRecord.CreatedBy ?? (object)DBNull.Value),
                };

                var sql = @"EXEC sp_Insert_PaymentTracker @Fin_Year,@Month,@Lab,@Vendor,@Type_Test,@Description,@Bis_Id,@Invoice_No,@Invoice_Date,
                        @Testing_Fee,@Approval_By,@Remark,@CreatedBy";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                if (returnCreatedRecord)
                {
                    return new OperationResult
                    {
                        Success = true,
                    };
                }

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateAsync(Payment_Tracker updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@PayTrac_Id", updatedRecord.Id),
                    new SqlParameter("@Fin_Year", updatedRecord.Fin_Year ?? (object)DBNull.Value),
                    new SqlParameter("@Month", updatedRecord.Month ?? (object)DBNull.Value),
                    new SqlParameter("@Lab", updatedRecord.Lab ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor", updatedRecord.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Type_Test", updatedRecord.Type_Test ?? (object)DBNull.Value),
                    new SqlParameter("@Description", updatedRecord.Description ?? (object)DBNull.Value),
                    new SqlParameter("@Bis_Id", updatedRecord.Bis_Id ?? (object)DBNull.Value),
                    new SqlParameter("@Invoice_No", updatedRecord.Invoice_No ?? (object)DBNull.Value),
                    new SqlParameter("@Invoice_Date", updatedRecord.Invoice_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Testing_Fee", updatedRecord.Testing_Fee),
                    new SqlParameter("@Approval_By", updatedRecord.Approval_By ?? (object)DBNull.Value),
                    new SqlParameter("@Remark", updatedRecord.Remark ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value)
                };

                var sql = @"EXEC sp_Update_PaymentTracker @PayTrac_Id,@Fin_Year,@Month,@Lab,@Vendor,@Type_Test,@Description,@Bis_Id,@Invoice_No,@Invoice_Date,
                        @Testing_Fee,@Approval_By,@Remark,@UpdatedBy";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                if (returnUpdatedRecord)
                {
                    return new OperationResult
                    {
                        Success = true,
                    };
                }

                return new OperationResult { Success = true };
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
                return await base.DeleteAsync<Payment_Tracker>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

       

        public async Task<bool> CheckDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.PaymentTracker
                    .Where(x => x.Deleted == false && x.Invoice_No.ToString() == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.PaymentTracker
                        .Where(x => x.Deleted == false &&
                               x.Invoice_No.ToString() == searchText
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

        public async Task<List<LabPaymentViewModel>> GetLabPaymentAsync()
        {
            try
            {
                var result = await (from pr in _dbContext.Lab_Payment
                                    where pr.Deleted == false // Add this condition
                                    select new LabPaymentViewModel
                                    {
                                        Id = pr.Id,
                                        Lab = pr.Lab,
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

        public async Task<Lab_Payment?> GetLabPaymentByIdAsync(int Id)
        {
            try
            {
                var result = await base.GetByIdAsync<Lab_Payment>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateLabPaymentAsync(LabPaymentViewModel newNatProjectRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var natToCreate = new Lab_Payment();
                natToCreate.Lab = newNatProjectRecord.Lab;
                natToCreate.CreatedBy = newNatProjectRecord.CreatedBy;
                natToCreate.CreatedDate = DateTime.Now;
                return await base.CreateAsync<Lab_Payment>(natToCreate, returnCreatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateLabPaymentAsync(LabPaymentViewModel updateNatProjectRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var natToCreate = await base.GetByIdAsync<Lab_Payment>(updateNatProjectRecord.Id);
                natToCreate.Lab = updateNatProjectRecord.Lab;
                natToCreate.UpdatedBy = updateNatProjectRecord.UpdatedBy;
                natToCreate.UpdatedDate = DateTime.Now;
                return await base.UpdateAsync<Lab_Payment>(natToCreate, returnUpdatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteLabPaymentAsync(int Id)
        {
            try
            {
                return await base.DeleteAsync<Lab_Payment>(Id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckLabPaymentDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.Lab_Payment
                    .Where(x => x.Deleted == false && x.Lab == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.Lab_Payment
                        .Where(x => x.Deleted == false &&
                               x.Lab == searchText
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

        public async Task<List<DropdownOptionViewModel>> GetLabDropdownAsync()
        {
            return await _dbContext.Lab_Payment
                .Where(v => !v.Deleted)
                .Select(v => new DropdownOptionViewModel
                {
                    Label = v.Lab,
                    Value = v.Id.ToString()
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> UpdateAttachmentAsync(int id, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                var record = await _dbContext.PaymentTracker.FindAsync(id);
                if (record == null)
                    return false;

                record.Attachment = fileName;

                // Only update the BIS_Attachment property
                _dbContext.Entry(record).Property(x => x.Attachment).IsModified = true;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
