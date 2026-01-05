using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.BisProjectTracRepository
{
    public class BisProjectTracRepository : SqlTableRepository, IBisProjectTracRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public BisProjectTracRepository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<BisProjectTracViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.BisProject_Tracker.FromSqlRaw("EXEC sp_Get_BIS_Project").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map results to ViewModel
                var viewModelList = result.Select(data => new BisProjectTracViewModel
                {
                    Id = data.Id,
                    Financial_Year = data.Financial_Year,
                    Mon_Pc = data.Mon_Pc,
                    Nat_Project = data.Nat_Project,
                    Lea_Model_No = data.Lea_Model_No,
                    No_Seri_Add = data.No_Seri_Add,
                    Cat_Ref_Lea_Model = data.Cat_Ref_Lea_Model,
                    Section = data.Section,
                    Manuf_Location = data.Manuf_Location,
                    BIS_Project_Id = data.BIS_Project_Id,
                    Lab = data.Lab,
                    Report_Owner = data.Report_Owner,
                    Ven_Sample_Sub_Date = data.Ven_Sample_Sub_Date,
                    Start_Date = data.Start_Date,
                    Comp_Date = data.Comp_Date,
                    Test_Duration = data.Test_Duration,
                    Submitted_Date = data.Submitted_Date,
                    Received_Date = data.Received_Date,
                    Bis_Duration = data.Bis_Duration,
                    Current_Status = data.Current_Status,
                    BIS_Attachment = data.BIS_Attachment,
                    Document_No = data.Document_No,
                    Effective_Date = data.Effective_Date,
                    Revision_No = data.Revision_No,
                    Revision_Date = data.Revision_Date,
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

        public async Task<List<BisProjectTracViewModel>> GetBisProjectTrackerAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.BisProject_Tracker.FromSqlRaw("EXEC sp_Get_BisProject_Tracker").ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.CreatedDate.HasValue &&
                                    x.CreatedDate.Value.Date >= startDate.Value.Date &&
                                    x.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                // Map results to ViewModel
                var viewModelList = result.Select(data => new BisProjectTracViewModel
                {
                    Id = data.Id,
                    Financial_Year = data.Financial_Year,
                    Mon_Pc = data.Mon_Pc,
                    Nat_Project = data.Nat_Project,
                    Lea_Model_No = data.Lea_Model_No,
                    No_Seri_Add = data.No_Seri_Add,
                    Cat_Ref_Lea_Model = data.Cat_Ref_Lea_Model,
                    Section = data.Section,
                    Manuf_Location = data.Manuf_Location,
                    BIS_Project_Id = data.BIS_Project_Id,
                    Lab = data.Lab,
                    Report_Owner = data.Report_Owner,
                    Ven_Sample_Sub_Date = data.Ven_Sample_Sub_Date,
                    Start_Date = data.Start_Date,
                    Comp_Date = data.Comp_Date,
                    Test_Duration = data.Test_Duration,
                    Submitted_Date = data.Submitted_Date,
                    Received_Date = data.Received_Date,
                    Bis_Duration = data.Bis_Duration,
                    Current_Status = data.Current_Status,
                    BIS_Attachment = data.BIS_Attachment,
                    Document_No = data.Document_No,
                    Effective_Date = data.Effective_Date,
                    Revision_No = data.Revision_No,
                    Revision_Date = data.Revision_Date,
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

        public async Task<OperationResult> CreateAsync(BisProject_Tracker newRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Financial_Year", newRecord.Financial_Year ?? (object)DBNull.Value),
                    new SqlParameter("@Mon_Pc", newRecord.Mon_Pc ?? (object)DBNull.Value),
                    new SqlParameter("@Nat_Project", newRecord.Nat_Project ?? (object)DBNull.Value),
                    new SqlParameter("@Lea_Model_No", newRecord.Lea_Model_No ?? (object)DBNull.Value),
                    new SqlParameter("@No_Seri_Add", newRecord.No_Seri_Add ?? (object)DBNull.Value),
                    new SqlParameter("@Cat_Ref_Lea_Model", newRecord.Cat_Ref_Lea_Model ?? (object)DBNull.Value),
                    new SqlParameter("@Section", newRecord.Section ?? (object)DBNull.Value),
                    new SqlParameter("@Manuf_Location", newRecord.Manuf_Location ?? (object)DBNull.Value),
                    new SqlParameter("@BIS_Project_Id", newRecord.BIS_Project_Id ?? (object)DBNull.Value),
                    new SqlParameter("@Lab", newRecord.Lab ?? (object)DBNull.Value),
                    new SqlParameter("@Report_Owner", newRecord.Report_Owner ?? (object)DBNull.Value),
                    new SqlParameter("@Start_Date", newRecord.Start_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Comp_Date", newRecord.Comp_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Duration", newRecord.Test_Duration ?? (object)DBNull.Value),
                    new SqlParameter("@Submitted_Date", newRecord.Submitted_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Received_Date", newRecord.Received_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Bis_Duration", newRecord.Bis_Duration ?? (object)DBNull.Value),
                    new SqlParameter("@Ven_Sample_Sub_Date", newRecord.Ven_Sample_Sub_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Document_No", newRecord.Document_No ?? (object)DBNull.Value),
                    new SqlParameter("@Effective_Date", newRecord.Effective_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Revision_No", newRecord.Revision_No ?? (object)DBNull.Value),
                    new SqlParameter("@Revision_Date", newRecord.Revision_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Current_Status", newRecord.Current_Status ?? (object)DBNull.Value),
                    new SqlParameter("@BIS_Attachment", newRecord.BIS_Attachment ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", newRecord.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", newRecord.Deleted),
                };

                var sql = @"EXEC sp_Insert_BIS_Project @Financial_Year,@Mon_Pc,@Nat_Project,@Lea_Model_No,@No_Seri_Add,@Cat_Ref_Lea_Model,@Section,@Manuf_Location,@BIS_Project_Id,
                        @Lab,@Report_Owner,@Start_Date,@Comp_Date,@Test_Duration,@Submitted_Date,@Received_Date,@Bis_Duration,@Ven_Sample_Sub_Date,@Document_No,@Effective_Date,@Revision_No,
                        @Revision_Date,@Current_Status,@BIS_Attachment,@CreatedBy,@IsDeleted";

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

        public async Task<OperationResult> UpdateAsync(BisProject_Tracker updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@BisProj_Id", updatedRecord.Id),
                    new SqlParameter("@Financial_Year", updatedRecord.Financial_Year ?? (object)DBNull.Value),
                    new SqlParameter("@Mon_Pc", updatedRecord.Mon_Pc ?? (object)DBNull.Value),
                    new SqlParameter("@Nat_Project", updatedRecord.Nat_Project ?? (object)DBNull.Value),
                    new SqlParameter("@Lea_Model_No", updatedRecord.Lea_Model_No ?? (object)DBNull.Value),
                    new SqlParameter("@No_Seri_Add", updatedRecord.No_Seri_Add ?? (object)DBNull.Value),
                    new SqlParameter("@Cat_Ref_Lea_Model", updatedRecord.Cat_Ref_Lea_Model ?? (object)DBNull.Value),
                    new SqlParameter("@Section", updatedRecord.Section ?? (object)DBNull.Value),
                    new SqlParameter("@Manuf_Location", updatedRecord.Manuf_Location ?? (object)DBNull.Value),
                    new SqlParameter("@BIS_Project_Id", updatedRecord.BIS_Project_Id ?? (object)DBNull.Value),
                    new SqlParameter("@Lab", updatedRecord.Lab ?? (object)DBNull.Value),
                    new SqlParameter("@Report_Owner", updatedRecord.Report_Owner ?? (object)DBNull.Value),
                    new SqlParameter("@Start_Date", updatedRecord.Start_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Comp_Date", updatedRecord.Comp_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Test_Duration", updatedRecord.Test_Duration ?? (object)DBNull.Value),
                    new SqlParameter("@Submitted_Date", updatedRecord.Submitted_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Received_Date", updatedRecord.Received_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Bis_Duration", updatedRecord.Bis_Duration ?? (object)DBNull.Value),
                    new SqlParameter("@Ven_Sample_Sub_Date", updatedRecord.Ven_Sample_Sub_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Document_No", updatedRecord.Document_No ?? (object)DBNull.Value),
                    new SqlParameter("@Effective_Date", updatedRecord.Effective_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Revision_No", updatedRecord.Revision_No ?? (object)DBNull.Value),
                    new SqlParameter("@Revision_Date", updatedRecord.Revision_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Current_Status", updatedRecord.Current_Status ?? (object)DBNull.Value),
                    new SqlParameter("@BIS_Attachment", updatedRecord.BIS_Attachment ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", updatedRecord.Deleted),
                };

                var sql = @"EXEC sp_Update_BIS_Project @BisProj_Id,@Financial_Year,@Mon_Pc,@Nat_Project,@Lea_Model_No,@No_Seri_Add,@Cat_Ref_Lea_Model,@Section,@Manuf_Location,@BIS_Project_Id,
                        @Lab,@Report_Owner,@Start_Date,@Comp_Date,@Test_Duration,@Submitted_Date,@Received_Date,@Bis_Duration,@Ven_Sample_Sub_Date,@Document_No,@Effective_Date,@Revision_No,
                        @Revision_Date,@Current_Status,@BIS_Attachment,@UpdatedBy,@IsDeleted";

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
                return await base.DeleteAsync<BisProject_Tracker>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<BisProjectTracViewModel?> GetByIdAsync(int ven_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@BisProj_Id", ven_Id),
                };

                var sql = @"EXEC sp_GetBIS_Project_By_Id @BisProj_Id";

                var result = await _dbContext.BisProject_Tracker.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new BisProjectTracViewModel
                {
                    Id = data.Id,
                    Financial_Year = data.Financial_Year,
                    Mon_Pc = data.Mon_Pc,
                    Nat_Project = data.Nat_Project,
                    Lea_Model_No = data.Lea_Model_No,
                    No_Seri_Add = data.No_Seri_Add,
                    Cat_Ref_Lea_Model = data.Cat_Ref_Lea_Model,
                    Section = data.Section,
                    Manuf_Location = data.Manuf_Location,
                    BIS_Project_Id = data.BIS_Project_Id,
                    Lab = data.Lab,
                    Report_Owner = data.Report_Owner,
                    Ven_Sample_Sub_Date = data.Ven_Sample_Sub_Date,
                    Start_Date = data.Start_Date,
                    Comp_Date = data.Comp_Date,
                    Test_Duration = data.Test_Duration,
                    Submitted_Date = data.Submitted_Date,
                    Received_Date = data.Received_Date,
                    Bis_Duration = data.Bis_Duration,
                    Current_Status = data.Current_Status,
                    BIS_Attachment = data.BIS_Attachment,
                    Document_No = data.Document_No,
                    Effective_Date = data.Effective_Date,
                    Revision_No = data.Revision_No,
                    Revision_Date = data.Revision_Date,
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

                IQueryable<int> query = _dbContext.BisProject_Tracker
                    .Where(x => x.Deleted == false && x.Nat_Project.ToString() == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.BisProject_Tracker
                        .Where(x => x.Deleted == false &&
                               x.Nat_Project.ToString() == searchText
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


        public async Task<List<NatProjectViewModel>> GetNatProjectAsync()
        {
            try
            {
                var result = await (from pr in _dbContext.NatProject_BIS
                                    where pr.Deleted == false // Add this condition
                                    select new NatProjectViewModel
                                    {
                                        Id = pr.Id,
                                        Nat_Project = pr.Nat_Project,
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

        public async Task<NatProject_BIS?> GetNatProjectByIdAsync(int Id)
        {
            try
            {
                var result = await base.GetByIdAsync<NatProject_BIS>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateNatProjectAsync(NatProjectViewModel newNatProjectRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var natToCreate = new NatProject_BIS();
                natToCreate.Nat_Project = newNatProjectRecord.Nat_Project;
                natToCreate.CreatedBy = newNatProjectRecord.CreatedBy;
                natToCreate.CreatedDate = DateTime.Now;
                return await base.CreateAsync<NatProject_BIS>(natToCreate, returnCreatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateNatProjectAsync(NatProjectViewModel updateNatProjectRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var natToCreate = await base.GetByIdAsync<NatProject_BIS>(updateNatProjectRecord.Id);
                natToCreate.Nat_Project = updateNatProjectRecord.Nat_Project;
                natToCreate.UpdatedBy = updateNatProjectRecord.UpdatedBy;
                natToCreate.UpdatedDate = DateTime.Now;
                return await base.UpdateAsync<NatProject_BIS>(natToCreate, returnUpdatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteNatProjectAsync(int Id)
        {
            try
            {
                return await base.DeleteAsync<NatProject_BIS>(Id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckNatProjectDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.NatProject_BIS
                    .Where(x => x.Deleted == false && x.Nat_Project == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.NatProject_BIS
                        .Where(x => x.Deleted == false &&
                               x.Nat_Project == searchText
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

        public async Task<List<DropdownOptionViewModel>> GetNatProjectDropdownAsync()
        {
            return await _dbContext.NatProject_BIS
                .Where(v => !v.Deleted)
                .Select(v => new DropdownOptionViewModel
                {
                    Label = v.Nat_Project,
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

                var record = await _dbContext.BisProject_Tracker.FindAsync(id);
                if (record == null)
                    return false;

                record.BIS_Attachment = fileName;

                // Only update the BIS_Attachment property
                _dbContext.Entry(record).Property(x => x.BIS_Attachment).IsModified = true;

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
