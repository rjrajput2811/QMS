using Microsoft.Data.SqlClient;
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

namespace QMS.Core.Repositories.ThirdPartyTestRepository
{
    public class ThirdPartyTestRepository :SqlTableRepository, IThirdPartyTestRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public ThirdPartyTestRepository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<ThirdPartyTestViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.ThirdPartyTesting.FromSqlRaw("EXEC sp_Get_ThirdParty_Test").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map results to ViewModel
                var viewModelList = result.Select(data => new ThirdPartyTestViewModel
                {
                    Id = data.Id,
                    Purpose = data.Purpose,
                    Project_Det = data.Project_Det,
                    Product_Det = data.Product_Det,
                    Wipro_Product_Code = data.Wipro_Product_Code,
                    Sample_Qty = data.Sample_Qty,
                    Test_Detail = data.Test_Detail,
                    Project_Initiator = data.Project_Initiator,
                    Vendor = data.Vendor,
                    Lab = data.Lab,
                    Sample_Status = data.Sample_Status,
                    Testing_Status = data.Testing_Status,
                    Lab_Contact_Person = data.Lab_Contact_Person,
                    Contact_Number = data.Contact_Number,
                    Email_Id = data.Email_Id,
                    Testing_Charge_offer = data.Testing_Charge_offer,
                    Final_Testing_Charge = data.Final_Testing_Charge,
                    Report = data.Report,
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

        public async Task<OperationResult> CreateAsync(ThirdPartyTesting newRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Purpose", newRecord.Purpose ?? (object)DBNull.Value),
                    new SqlParameter("@Project_Det", newRecord.Project_Det ?? (object)DBNull.Value),
                    new SqlParameter("@Product_Det", newRecord.Product_Det ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Product_Code", newRecord.Wipro_Product_Code ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Qty", newRecord.Sample_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Detail", newRecord.Test_Detail ?? (object)DBNull.Value),
                    new SqlParameter("@Project_Initiator", newRecord.Project_Initiator ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor", newRecord.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Lab", newRecord.Lab ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Status", newRecord.Sample_Status ?? (object)DBNull.Value),
                    new SqlParameter("@Testing_Status", newRecord.Testing_Status ?? (object)DBNull.Value),
                    new SqlParameter("@Lab_Contact_Person", newRecord.Lab_Contact_Person ?? (object)DBNull.Value),
                    new SqlParameter("@Contact_Number", newRecord.Contact_Number ?? (object)DBNull.Value),
                    new SqlParameter("@Email_Id", newRecord.Email_Id ?? (object)DBNull.Value),
                    new SqlParameter("@Testing_Charge_offer", newRecord.Testing_Charge_offer ?? (object)DBNull.Value),
                    new SqlParameter("@Final_Testing_Charge", newRecord.Final_Testing_Charge ?? (object)DBNull.Value),
                    new SqlParameter("@Report", newRecord.Report ?? (object)DBNull.Value),
                    new SqlParameter("CreatedBy", newRecord.CreatedBy ?? (object)DBNull.Value)
                };

                var sql = @"EXEC sp_Insert_ThirdPartyTest @Purpose,@Project_Det,@Product_Det,@Wipro_Product_Code,@Sample_Qty ,@Test_Detail ,@Project_Initiator,
                                @Vendor,@Lab ,@Sample_Status ,@Testing_Status,@Lab_Contact_Person ,@Contact_Number ,@Email_Id ,@Testing_Charge_offer ,@Final_Testing_Charge ,@Report ,@CreatedBy            ";

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

        public async Task<OperationResult> UpdateAsync(ThirdPartyTesting updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ThirdPartyTest_ID", updatedRecord.Id),
                    new SqlParameter("@Purpose", updatedRecord.Purpose ?? (object)DBNull.Value),
                    new SqlParameter("@Project_Det", updatedRecord.Project_Det ?? (object)DBNull.Value),
                    new SqlParameter("@Product_Det", updatedRecord.Product_Det ?? (object)DBNull.Value),
                    new SqlParameter("@Wipro_Product_Code", updatedRecord.Wipro_Product_Code ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Qty", updatedRecord.Sample_Qty ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Detail", updatedRecord.Test_Detail ?? (object)DBNull.Value),
                    new SqlParameter("@Project_Initiator", updatedRecord.Project_Initiator ?? (object)DBNull.Value),
                    new SqlParameter("@Vendor", updatedRecord.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Lab", updatedRecord.Lab ?? (object)DBNull.Value),
                    new SqlParameter("@Sample_Status", updatedRecord.Sample_Status ?? (object)DBNull.Value),
                    new SqlParameter("@Testing_Status", updatedRecord.Testing_Status ?? (object)DBNull.Value),
                    new SqlParameter("@Lab_Contact_Person", updatedRecord.Lab_Contact_Person ?? (object)DBNull.Value),
                    new SqlParameter("@Contact_Number", updatedRecord.Contact_Number ?? (object)DBNull.Value),
                    new SqlParameter("@Email_Id", updatedRecord.Email_Id ?? (object)DBNull.Value),
                    new SqlParameter("@Testing_Charge_offer", updatedRecord.Testing_Charge_offer ?? (object)DBNull.Value),
                    new SqlParameter("@Final_Testing_Charge", updatedRecord.Final_Testing_Charge ?? (object)DBNull.Value),
                    new SqlParameter("@Report", updatedRecord.Report ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value)
                };

                var sql = @"EXEC sp_Update_ThirdPartyTest @ThirdPartyTest_ID,@Purpose,@Project_Det,@Product_Det,@Wipro_Product_Code,@Sample_Qty,@Test_Detail,@Project_Initiator,
                                @Vendor,@Lab,@Sample_Status,@Testing_Status,@Lab_Contact_Person,@Contact_Number,@Email_Id,@Testing_Charge_offer,@Final_Testing_Charge,@Report,@UpdatedBy";

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
                return await base.DeleteAsync<ThirdPartyTesting>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<ThirdPartyTestViewModel?> GetByIdAsync(int ven_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ThirdPartyTest_ID", ven_Id),
                };

                var sql = @"EXEC sp_Get_ThirdPartyTest_ById @ThirdPartyTest_ID";

                var result = await _dbContext.ThirdPartyTesting.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new ThirdPartyTestViewModel
                {
                    Id = data.Id,
                    Purpose = data.Purpose,
                    Project_Det = data.Project_Det,
                    Product_Det = data.Product_Det,
                    Wipro_Product_Code = data.Wipro_Product_Code,
                    Sample_Qty = data.Sample_Qty,
                    Test_Detail = data.Test_Detail,
                    Project_Initiator = data.Project_Initiator,
                    Vendor = data.Vendor,
                    Lab = data.Lab,
                    Sample_Status = data.Sample_Status,
                    Testing_Status = data.Testing_Status,
                    Lab_Contact_Person = data.Lab_Contact_Person,
                    Contact_Number = data.Contact_Number,
                    Email_Id = data.Email_Id,
                    Testing_Charge_offer = data.Testing_Charge_offer,
                    Final_Testing_Charge = data.Final_Testing_Charge,
                    Report = data.Report,
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

        public async Task<bool> CheckDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.ThirdPartyTesting
                    .Where(x => x.Deleted == false && x.Purpose.ToString() == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.ThirdPartyTesting
                        .Where(x => x.Deleted == false &&
                               x.Purpose.ToString() == searchText
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

        public async Task<bool> UpdateAttachmentAsync(int id, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                var record = await _dbContext.ThirdPartyTesting.FindAsync(id);
                if (record == null)
                    return false;

                record.Report = fileName;

                // Only update the BIS_Attachment property
                _dbContext.Entry(record).Property(x => x.Report).IsModified = true;

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
