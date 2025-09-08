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

        public async Task<List<PurposeTPTViewModel>> GetPurposeTPTAsync()
        {
            try
            {
                var result = await (from pr in _dbContext.PurposeTPT
                                    where pr.Deleted == false // Add this condition
                                    select new PurposeTPTViewModel
                                    {
                                        Id = pr.Id,
                                        Purpose = pr.Purpose,
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

        public async Task<Purpose_TPT?> GetPurposeTPTByIdAsync(int Id)
        {
            try
            {
                var result = await base.GetByIdAsync<Purpose_TPT>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreatePurposeTPTAsync(PurposeTPTViewModel newPurposeTPTRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var natToCreate = new Purpose_TPT();
                natToCreate.Purpose = newPurposeTPTRecord.Purpose;
                natToCreate.CreatedBy = newPurposeTPTRecord.CreatedBy;
                natToCreate.CreatedDate = DateTime.Now;
                return await base.CreateAsync<Purpose_TPT>(natToCreate, returnCreatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdatePurposeTPTAsync(PurposeTPTViewModel updatePurposeTPTRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var natToCreate = await base.GetByIdAsync<Purpose_TPT>(updatePurposeTPTRecord.Id);
                natToCreate.Purpose = updatePurposeTPTRecord.Purpose;
                natToCreate.UpdatedBy = updatePurposeTPTRecord.UpdatedBy;
                natToCreate.UpdatedDate = DateTime.Now;
                return await base.UpdateAsync<Purpose_TPT>(natToCreate, returnUpdatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeletePurposeTPTAsync(int Id)
        {
            try
            {
                return await base.DeleteAsync<Purpose_TPT>(Id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckPurposeTPTDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.PurposeTPT
                    .Where(x => x.Deleted == false && x.Purpose == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.PurposeTPT
                        .Where(x => x.Deleted == false &&
                               x.Purpose == searchText
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

        public async Task<List<DropdownOptionViewModel>> GetPurposeDropdownAsync()
        {
            return await _dbContext.PurposeTPT
                .Where(v => !v.Deleted)
                .Select(v => new DropdownOptionViewModel
                {
                    Label = v.Purpose,
                    Value = v.Id.ToString()
                })
                .Distinct()
                .ToListAsync();
        }



        public async Task<List<TestDetTPTViewModel>> GetTestDetTPTAsync()
        {
            try
            {
                var result = await (from pr in _dbContext.TestDetTPT
                                    where pr.Deleted == false // Add this condition
                                    select new TestDetTPTViewModel
                                    {
                                        Id = pr.Id,
                                        Test_Det = pr.Test_Det,
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

        public async Task<TestDet_TPT?> GetTestDetTPTByIdAsync(int Id)
        {
            try
            {
                var result = await base.GetByIdAsync<TestDet_TPT>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateTestDetTPTAsync(TestDetTPTViewModel newTestDetTPTRecord, bool returnCreatedRecord = false
        {
            try
            {
                var natToCreate = new TestDet_TPT();
                natToCreate.Test_Det = newTestDetTPTRecord.Test_Det;
                natToCreate.CreatedBy = newTestDetTPTRecord.CreatedBy;
                natToCreate.CreatedDate = DateTime.Now;
                return await base.CreateAsync<TestDet_TPT>(natToCreate, returnCreatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateTestDetTPTAsync(TestDetTPTViewModel updateTestDetTPTRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var natToCreate = await base.GetByIdAsync<TestDet_TPT>(updateTestDetTPTRecord.Id);
                natToCreate.Test_Det = updateTestDetTPTRecord.Test_Det;
                natToCreate.UpdatedBy = updateTestDetTPTRecord.UpdatedBy;
                natToCreate.UpdatedDate = DateTime.Now;
                return await base.UpdateAsync<TestDet_TPT>(natToCreate, returnUpdatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteTestDetTPTAsync(int Id)
        {
            try
            {
                return await base.DeleteAsync<TestDet_TPT>(Id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckTestDetTPTDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.TestDetTPT
                    .Where(x => x.Deleted == false && x.Test_Det == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.TestDetTPT
                        .Where(x => x.Deleted == false &&
                               x.Test_Det == searchText
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

        public async Task<List<DropdownOptionViewModel>> GetTestDetDropdownAsync()
        {
            return await _dbContext.TestDetTPT
                .Where(v => !v.Deleted)
                .Select(v => new DropdownOptionViewModel
                {
                    Label = v.Test_Det,
                    Value = v.Id.ToString()
                })
                .Distinct()
                .ToListAsync();
        }




        public async Task<List<ProjectInitTPTViewModel>> GetProjectInitAsync()
        {
            try
            {
                var result = await (from pr in _dbContext.ProjectInitTPT
                                    where pr.Deleted == false // Add this condition
                                    select new ProjectInitTPTViewModel
                                    {
                                        Id = pr.Id,
                                        Project_Init = pr.Project_Init,
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

        public async Task<ProjectInit_TPT?> GetProjectInitByIdAsync(int Id)
        {
            try
            {
                var result = await base.GetByIdAsync<ProjectInit_TPT>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateProjectInitAsync(ProjectInitTPTViewModel newProjectInitRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var natToCreate = new ProjectInit_TPT();
                natToCreate.Project_Init = newProjectInitRecord.Project_Init;
                natToCreate.CreatedBy = newProjectInitRecord.CreatedBy;
                natToCreate.CreatedDate = DateTime.Now;
                return await base.CreateAsync<ProjectInit_TPT>(natToCreate, returnCreatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateProjectInitAsync(ProjectInitTPTViewModel updateProjectInitRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var natToCreate = await base.GetByIdAsync<ProjectInit_TPT>(updateProjectInitRecord.Id);
                natToCreate.Project_Init = updateProjectInitRecord.Project_Init;
                natToCreate.UpdatedBy = updateProjectInitRecord.UpdatedBy;
                natToCreate.UpdatedDate = DateTime.Now;
                return await base.UpdateAsync<ProjectInit_TPT>(natToCreate, returnUpdatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteProjectInitAsync(int Id)
        {
            try
            {
                return await base.DeleteAsync<ProjectInit_TPT>(Id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckProjectInitDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.ProjectInitTPT
                    .Where(x => x.Deleted == false && x.Project_Init == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.ProjectInitTPT
                        .Where(x => x.Deleted == false &&
                               x.Project_Init == searchText
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

        public async Task<List<DropdownOptionViewModel>> GetProjectInitDropdownAsync()
        {
            return await _dbContext.ProjectInitTPT
                .Where(v => !v.Deleted)
                .Select(v => new DropdownOptionViewModel
                {
                    Label = v.Project_Init,
                    Value = v.Id.ToString()
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
