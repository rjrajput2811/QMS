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

namespace QMS.Core.Repositories.COPQComplaintDumpRepository
{
    public class ContractorRepository : SqlTableRepository, IContractorRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public ContractorRepository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<ContractorDetailViewModel>> GetListAsync()
        {
            try
            {
                var result = await _dbContext.ContractorDetails.FromSqlRaw("EXEC sp_Get_All_Contractor").ToListAsync();

                var viewModelList = result.Select(data => new ContractorDetailViewModel
                {
                    Id = data.Id,
                    Cont_Firm_Name = data.Cont_Firm_Name,
                    Cont_Name = data.Cont_Name,
                    Cont_Ven_Code = data.Cont_Ven_Code,
                    Pan_No = data.Pan_No,
                    Gst = data.Gst,
                    Cont_Valid_Date = data.Cont_Valid_Date,
                    Location = data.Location,
                    No_Tech = data.No_Tech,
                    Moblie = data.Moblie,
                    Monthly_Salary = data.Monthly_Salary,
                    Daily_Wages_Local = data.Daily_Wages_Local,
                    Conv_Fixed_Actual = data.Conv_Fixed_Actual,
                    Daily_Wages_Outstation = data.Daily_Wages_Outstation,
                    Dailywages_Ext_Manpower = data.Dailywages_Ext_Manpower,
                    OT_Charge_Full_Night = data.OT_Charge_Full_Night,
                    OT_Charge_Till_10 = data.OT_Charge_Till_10,
                    OT_Outstation_night_Travel = data.OT_Outstation_night_Travel,
                    Con_Cont_ESIC_Tech = data.Con_Cont_ESIC_Tech,
                    Con_Cont_PF_Tech = data.Con_Cont_PF_Tech,
                    Attchment = data.Attchment,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate,
                }).ToList();

                return viewModelList;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }


        public async Task<ContractorDetailViewModel?> GetByIdAsync(int cont_Id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Cont_Id", cont_Id),
                };

                var sql = @"EXEC sp_Get_Contractor_ById @Cont_Id";

                var result = await _dbContext.ContractorDetails.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new ContractorDetailViewModel
                {
                    Id = data.Id,
                    Cont_Firm_Name = data.Cont_Firm_Name,
                    Cont_Name = data.Cont_Name,
                    Cont_Ven_Code = data.Cont_Ven_Code,
                    Pan_No = data.Pan_No,
                    Gst = data.Gst,
                    Cont_Valid_Date = data.Cont_Valid_Date,
                    Location = data.Location,
                    No_Tech = data.No_Tech,
                    Moblie = data.Moblie,
                    Monthly_Salary = data.Monthly_Salary,
                    Daily_Wages_Local = data.Daily_Wages_Local,
                    Conv_Fixed_Actual = data.Conv_Fixed_Actual,
                    Daily_Wages_Outstation = data.Daily_Wages_Outstation,
                    Dailywages_Ext_Manpower = data.Dailywages_Ext_Manpower,
                    OT_Charge_Full_Night = data.OT_Charge_Full_Night,
                    OT_Charge_Till_10 = data.OT_Charge_Till_10,
                    OT_Outstation_night_Travel = data.OT_Outstation_night_Travel,
                    Con_Cont_ESIC_Tech = data.Con_Cont_ESIC_Tech,
                    Con_Cont_PF_Tech = data.Con_Cont_PF_Tech,
                    Attchment = data.Attchment,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate,

                }).ToList();

                return viewModelList.FirstOrDefault(); // Assuming you want a single view model based on the ID

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateAsync(ContractorDetail_Service newRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Cont_Firm_Name", newRecord.Cont_Firm_Name ?? (object)DBNull.Value),
                    new SqlParameter("@Cont_Name", newRecord.Cont_Name ?? (object)DBNull.Value),
                    new SqlParameter("@Cont_Ven_Code", newRecord.Cont_Ven_Code ?? (object)DBNull.Value),
                    new SqlParameter("@Pan_No", newRecord.Pan_No ?? (object)DBNull.Value),
                    new SqlParameter("@Gst", newRecord.Gst ?? (object)DBNull.Value),
                    new SqlParameter("@Cont_Valid_Date", newRecord.Cont_Valid_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Location", newRecord.Location ?? (object)DBNull.Value),
                    new SqlParameter("@No_Tech", newRecord.No_Tech ?? (object)DBNull.Value),
                    new SqlParameter("@Moblie", newRecord.Moblie ?? (object)DBNull.Value),
                    new SqlParameter("@Monthly_Salary", newRecord.Monthly_Salary ?? (object)DBNull.Value),
                    new SqlParameter("@Conv_Bike_User", newRecord.Conv_Bike_User ?? (object)DBNull.Value),
                    new SqlParameter("@Daily_Wages_Local", newRecord.Daily_Wages_Local ?? (object)DBNull.Value),
                    new SqlParameter("@Conv_Fixed_Actual", newRecord.Conv_Fixed_Actual ?? (object)DBNull.Value),
                    new SqlParameter("@Daily_Wages_Outstation", newRecord.Daily_Wages_Outstation ?? (object)DBNull.Value),
                    new SqlParameter("@Dailywages_Ext_Manpower", newRecord.Dailywages_Ext_Manpower ?? (object)DBNull.Value),
                    new SqlParameter("@OT_Charge_Full_Night", newRecord.OT_Charge_Full_Night ?? (object)DBNull.Value),
                    new SqlParameter("@OT_Charge_Till_10", newRecord.OT_Charge_Till_10 ?? (object)DBNull.Value),
                    new SqlParameter("@OT_Outstation_night_Travel", newRecord.OT_Outstation_night_Travel ?? (object)DBNull.Value),
                    new SqlParameter("@Con_Cont_ESIC_Tech", newRecord.Con_Cont_ESIC_Tech),
                    new SqlParameter("@Con_Cont_PF_Tech", newRecord.Con_Cont_PF_Tech),
                    new SqlParameter("@Attchment", newRecord.Attchment ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", newRecord.CreatedBy ?? (object)DBNull.Value)
                    
                };

                var sql = @"EXEC sp_Insert_Contractor @Cont_Firm_Name,@Cont_Name,@Cont_Ven_Code,@Pan_No,@Gst,@Cont_Valid_Date,@Location,@No_Tech,
                        @Moblie,@Monthly_Salary,@Conv_Bike_User,@Daily_Wages_Local,@Conv_Fixed_Actual,@Daily_Wages_Outstation,@Dailywages_Ext_Manpower,
                        @OT_Charge_Full_Night,@OT_Charge_Till_10,@OT_Outstation_night_Travel,@Con_Cont_ESIC_Tech,@Con_Cont_PF_Tech,@Attchment,@CreatedBy";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateAsync(ContractorDetail_Service updatedRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Cont_Id", updatedRecord.Id),
                    new SqlParameter("@Cont_Firm_Name", updatedRecord.Cont_Firm_Name ?? (object)DBNull.Value),
                    new SqlParameter("@Cont_Name", updatedRecord.Cont_Name ?? (object)DBNull.Value),
                    new SqlParameter("@Cont_Ven_Code", updatedRecord.Cont_Ven_Code ?? (object)DBNull.Value),
                    new SqlParameter("@Pan_No", updatedRecord.Pan_No ?? (object)DBNull.Value),
                    new SqlParameter("@Gst", updatedRecord.Gst ?? (object)DBNull.Value),
                    new SqlParameter("@Cont_Valid_Date", updatedRecord.Cont_Valid_Date ?? (object)DBNull.Value),
                    new SqlParameter("@Location", updatedRecord.Location ?? (object)DBNull.Value),
                    new SqlParameter("@No_Tech", updatedRecord.No_Tech ?? (object)DBNull.Value),
                    new SqlParameter("@Moblie", updatedRecord.Moblie ?? (object)DBNull.Value),
                    new SqlParameter("@Monthly_Salary", updatedRecord.Monthly_Salary ?? (object)DBNull.Value),
                    new SqlParameter("@Conv_Bike_User", updatedRecord.Conv_Bike_User ?? (object)DBNull.Value),
                    new SqlParameter("@Daily_Wages_Local", updatedRecord.Daily_Wages_Local ?? (object)DBNull.Value),
                    new SqlParameter("@Conv_Fixed_Actual", updatedRecord.Conv_Fixed_Actual ?? (object)DBNull.Value),
                    new SqlParameter("@Daily_Wages_Outstation", updatedRecord.Daily_Wages_Outstation ?? (object)DBNull.Value),
                    new SqlParameter("@Dailywages_Ext_Manpower", updatedRecord.Dailywages_Ext_Manpower ?? (object)DBNull.Value),
                    new SqlParameter("@OT_Charge_Full_Night", updatedRecord.OT_Charge_Full_Night ?? (object)DBNull.Value),
                    new SqlParameter("@OT_Charge_Till_10", updatedRecord.OT_Charge_Till_10 ?? (object)DBNull.Value),
                    new SqlParameter("@OT_Outstation_night_Travel", updatedRecord.OT_Outstation_night_Travel ?? (object)DBNull.Value),
                    new SqlParameter("@Con_Cont_ESIC_Tech", updatedRecord.Con_Cont_ESIC_Tech),
                    new SqlParameter("@Con_Cont_PF_Tech", updatedRecord.Con_Cont_PF_Tech),
                    new SqlParameter("@Attchment", updatedRecord.Attchment ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", updatedRecord.UpdatedBy ?? (object)DBNull.Value)
                };

                var sql = @"EXEC sp_Update_Contractor @Cont_Id,@Cont_Firm_Name,@Cont_Name,@Cont_Ven_Code,@Pan_No,@Gst,@Cont_Valid_Date,@Location,@No_Tech,
                        @Moblie,@Monthly_Salary,@Conv_Bike_User,@Daily_Wages_Local,@Conv_Fixed_Actual,@Daily_Wages_Outstation,@Dailywages_Ext_Manpower,
                        @OT_Charge_Full_Night,@OT_Charge_Till_10,@OT_Outstation_night_Travel,@Con_Cont_ESIC_Tech,@Con_Cont_PF_Tech,@Attchment,@UpdatedDate";

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

        public async Task<OperationResult> DeleteAsync(int userId)
        {
            try
            {
                return await base.DeleteAsync<ContractorDetail_Service>(userId);
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

                IQueryable<int> query = _dbContext.ContractorDetails
                    .Where(x => x.Deleted == false && x.Cont_Firm_Name.ToString() == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.ContractorDetails
                        .Where(x => x.Deleted == false &&
                               x.Cont_Firm_Name == searchText
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
    }
}
