using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using QMS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.CAReportRepository
{
    public class CAReportRepository : SqlTableRepository, ICAReportRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public CAReportRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<OperationResult> InsertCAReportAsync(CAReportViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Model cannot be null."
                    };
                }

                var moniPlansTvp = BuildMoniPlansDataTable(model.Details);

                var parameters = new[]
                {
                    new SqlParameter("@Complaint_No", SqlDbType.NVarChar, 600) {
                        Value = (object?)model.Complaint_No ?? DBNull.Value
                    },
                    new SqlParameter("@Report_Date", SqlDbType.DateTime) {
                        Value = (object?)model.Report_Date ?? DBNull.Value
                    },
                    new SqlParameter("@Cust_Complaints", SqlDbType.Bit) {
                        Value = model.Cust_Complaints
                    },
                    new SqlParameter("@NPI_Validations", SqlDbType.Bit) {
                        Value = model.NPI_Validations
                    },
                    new SqlParameter("@PDI_Obser", SqlDbType.Bit) {
                        Value = model.PDI_Obser
                    },
                    new SqlParameter("@System", SqlDbType.Bit) {
                        Value = model.System
                    },
                    new SqlParameter("@Cust_Name_Location", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Cust_Name_Location ?? DBNull.Value
                    },
                    new SqlParameter("@Source_Complaint", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Source_Complaint ?? DBNull.Value
                    },
                    new SqlParameter("@Prod_Code_Desc", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Prod_Code_Desc ?? DBNull.Value
                    },
                    new SqlParameter("@Desc_Complaint", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Desc_Complaint ?? DBNull.Value
                    },
                    new SqlParameter("@Batch_Code", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Batch_Code ?? DBNull.Value
                    },
                    new SqlParameter("@Pkd", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Pkd ?? DBNull.Value
                    },
                    new SqlParameter("@Supp_Qty", SqlDbType.Int) {
                        Value = (object?)model.Supp_Qty ?? DBNull.Value
                    },
                    new SqlParameter("@Failure_Qty", SqlDbType.Int) {
                        Value = (object?)model.Failure_Qty ?? DBNull.Value
                    },
                    new SqlParameter("@Failure", SqlDbType.Int) {
                        Value = (object?)model.Failure ?? DBNull.Value
                    },
                    new SqlParameter("@Age_Install", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Age_Install ?? DBNull.Value
                    },
                    new SqlParameter("@Description", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Description ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_State", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_State ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_Visual_ImgA", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_Visual_ImgA ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_Visual_ImgB", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_Visual_ImgB ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_Visual_ImgC", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_Visual_ImgC ?? DBNull.Value
                    },
                    new SqlParameter("@Initial_Observ", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Initial_Observ ?? DBNull.Value
                    },
                    new SqlParameter("@Man_Issue_Prob", SqlDbType.Bit) {
                        Value = model.Man_Issue_Prob
                    },
                    new SqlParameter("@Design_Prob", SqlDbType.Bit) {
                        Value = model.Design_Prob
                    },
                    new SqlParameter("@Site_Issue_Prob", SqlDbType.Bit) {
                        Value = model.Site_Issue_Prob
                    },
                    new SqlParameter("@Com_Gap_Prob", SqlDbType.Bit) {
                        Value = model.Com_Gap_Prob
                    },
                    new SqlParameter("@Install_Issues_Prov", SqlDbType.Bit) {
                        Value = model.Install_Issues_Prov
                    },
                    new SqlParameter("@Wrong_App_Prob", SqlDbType.Bit) {
                        Value = model.Wrong_App_Prob
                    },
                    new SqlParameter("@Interim_Corrective", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Interim_Corrective ?? DBNull.Value
                    },
                    new SqlParameter("@Root_Cause_Anal", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Root_Cause_Anal ?? DBNull.Value
                    },
                    new SqlParameter("@Corrective_Action", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Corrective_Action ?? DBNull.Value
                    },
                    new SqlParameter("@Before_Photo", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Before_Photo ?? DBNull.Value
                    },
                    new SqlParameter("@After_Photo", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.After_Photo ?? DBNull.Value
                    },
                    new SqlParameter("@Rca_Prepared_By", SqlDbType.NVarChar, 500) {
                        Value = (object?)model.Rca_Prepared_By ?? DBNull.Value
                    },
                    new SqlParameter("@Name_Designation", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Name_Designation ?? DBNull.Value
                    },
                    new SqlParameter("@Date", SqlDbType.DateTime) {
                        Value = (object?)model.Date ?? DBNull.Value
                    },
                    new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 500) {
                        Value = (object?)model.CreatedBy ?? DBNull.Value
                    },
                    new SqlParameter("@MoniPlans", SqlDbType.Structured)
                    {
                        TypeName = "dbo.MoniPlanCaType",
                        Value = moniPlansTvp
                    }
                };

                var sql =
                    "EXEC sp_Insert_CA_Report " +
                    "@Complaint_No, @Report_Date, @Cust_Complaints, @NPI_Validations, @PDI_Obser, @System, " +
                    "@Cust_Name_Location, @Source_Complaint, @Prod_Code_Desc, @Desc_Complaint, @Batch_Code, @Pkd, " +
                    "@Supp_Qty, @Failure_Qty, @Failure, @Age_Install, @Description, @Problem_State, " +
                    "@Problem_Visual_ImgA, @Problem_Visual_ImgB, @Problem_Visual_ImgC, @Initial_Observ, " +
                    "@Man_Issue_Prob, @Design_Prob, @Site_Issue_Prob, @Com_Gap_Prob, @Install_Issues_Prov, " +
                    "@Wrong_App_Prob, @Interim_Corrective, @Root_Cause_Anal, @Corrective_Action, " +
                    "@Before_Photo, @After_Photo, @Rca_Prepared_By, @Name_Designation, @Date, @CreatedBy, " +
                    "@MoniPlans";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult
                {
                    Success = true,
                    Message = "CA Report inserted successfully."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to insert CA Report: " + ex.Message
                };
            }
        }

        public async Task<OperationResult> UpdateCAReportAsync(CAReportViewModel model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Invalid CA Report model or Id."
                    };
                }

                var moniPlansTvp = BuildMoniPlansDataTable(model.Details);

                var parameters = new[]
                {
                    new SqlParameter("@Ca_Id", SqlDbType.Int) {
                        Value = model.Id
                    },

                    new SqlParameter("@Complaint_No", SqlDbType.NVarChar, 600) {
                        Value = (object?)model.Complaint_No ?? DBNull.Value
                    },
                    new SqlParameter("@Report_Date", SqlDbType.DateTime) {
                        Value = (object?)model.Report_Date ?? DBNull.Value
                    },
                    new SqlParameter("@Cust_Complaints", SqlDbType.Bit) {
                        Value = model.Cust_Complaints
                    },
                    new SqlParameter("@NPI_Validations", SqlDbType.Bit) {
                        Value = model.NPI_Validations
                    },
                    new SqlParameter("@PDI_Obser", SqlDbType.Bit) {
                        Value = model.PDI_Obser
                    },
                    new SqlParameter("@System", SqlDbType.Bit) {
                        Value = model.System
                    },
                    new SqlParameter("@Cust_Name_Location", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Cust_Name_Location ?? DBNull.Value
                    },
                    new SqlParameter("@Source_Complaint", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Source_Complaint ?? DBNull.Value
                    },
                    new SqlParameter("@Prod_Code_Desc", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Prod_Code_Desc ?? DBNull.Value
                    },
                    new SqlParameter("@Desc_Complaint", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Desc_Complaint ?? DBNull.Value
                    },
                    new SqlParameter("@Batch_Code", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Batch_Code ?? DBNull.Value
                    },
                    new SqlParameter("@Pkd", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Pkd ?? DBNull.Value
                    },
                    new SqlParameter("@Supp_Qty", SqlDbType.Int) {
                        Value = (object?)model.Supp_Qty ?? DBNull.Value
                    },
                    new SqlParameter("@Failure_Qty", SqlDbType.Int) {
                        Value = (object?)model.Failure_Qty ?? DBNull.Value
                    },
                    new SqlParameter("@Failure", SqlDbType.Int) {
                        Value = (object?)model.Failure ?? DBNull.Value
                    },
                    new SqlParameter("@Age_Install", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Age_Install ?? DBNull.Value
                    },
                    new SqlParameter("@Description", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Description ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_State", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_State ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_Visual_ImgA", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_Visual_ImgA ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_Visual_ImgB", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_Visual_ImgB ?? DBNull.Value
                    },
                    new SqlParameter("@Problem_Visual_ImgC", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Problem_Visual_ImgC ?? DBNull.Value
                    },
                    new SqlParameter("@Initial_Observ", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Initial_Observ ?? DBNull.Value
                    },
                    new SqlParameter("@Man_Issue_Prob", SqlDbType.Bit) {
                        Value = model.Man_Issue_Prob
                    },
                    new SqlParameter("@Design_Prob", SqlDbType.Bit) {
                        Value = model.Design_Prob
                    },
                    new SqlParameter("@Site_Issue_Prob", SqlDbType.Bit) {
                        Value = model.Site_Issue_Prob
                    },
                    new SqlParameter("@Com_Gap_Prob", SqlDbType.Bit) {
                        Value = model.Com_Gap_Prob
                    },
                    new SqlParameter("@Install_Issues_Prov", SqlDbType.Bit) {
                        Value = model.Install_Issues_Prov
                    },
                    new SqlParameter("@Wrong_App_Prob", SqlDbType.Bit) {
                        Value = model.Wrong_App_Prob
                    },
                    new SqlParameter("@Interim_Corrective", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Interim_Corrective ?? DBNull.Value
                    },
                    new SqlParameter("@Root_Cause_Anal", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Root_Cause_Anal ?? DBNull.Value
                    },
                    new SqlParameter("@Corrective_Action", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Corrective_Action ?? DBNull.Value
                    },
                    new SqlParameter("@Before_Photo", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Before_Photo ?? DBNull.Value
                    },
                    new SqlParameter("@After_Photo", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.After_Photo ?? DBNull.Value
                    },
                    new SqlParameter("@Rca_Prepared_By", SqlDbType.NVarChar, 500) {
                        Value = (object?)model.Rca_Prepared_By ?? DBNull.Value
                    },
                    new SqlParameter("@Name_Designation", SqlDbType.NVarChar, -1) {
                        Value = (object?)model.Name_Designation ?? DBNull.Value
                    },
                    new SqlParameter("@Date", SqlDbType.DateTime) {
                        Value = (object?)model.Date ?? DBNull.Value
                    },
                    new SqlParameter("@UpdatedBy", SqlDbType.NVarChar, 500) {
                        Value = (object?)model.UpdatedBy ?? DBNull.Value
                    },
                    new SqlParameter("@MoniPlans", SqlDbType.Structured)
                    {
                        TypeName = "dbo.MoniPlanCaType",
                        Value = moniPlansTvp
                    }
                };

                var sql =
                    "EXEC sp_Update_CA_Report " +
                    "@Ca_Id, @Complaint_No, @Report_Date, @Cust_Complaints, @NPI_Validations, @PDI_Obser, @System, " +
                    "@Cust_Name_Location, @Source_Complaint, @Prod_Code_Desc, @Desc_Complaint, @Batch_Code, @Pkd, " +
                    "@Supp_Qty, @Failure_Qty, @Failure, @Age_Install, @Description, @Problem_State, " +
                    "@Problem_Visual_ImgA, @Problem_Visual_ImgB, @Problem_Visual_ImgC, @Initial_Observ, " +
                    "@Man_Issue_Prob, @Design_Prob, @Site_Issue_Prob, @Com_Gap_Prob, @Install_Issues_Prov, " +
                    "@Wrong_App_Prob, @Interim_Corrective, @Root_Cause_Anal, @Corrective_Action, " +
                    "@Before_Photo, @After_Photo, @Rca_Prepared_By, @Name_Designation, @Date, @UpdatedBy, " +
                    "@MoniPlans";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult
                {
                    Success = true,
                    Message = "CA Report updated successfully."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to update CA Report: " + ex.Message
                };
            }
        }


        private static DataTable BuildMoniPlansDataTable(IEnumerable<CAReportDetailViewModel>? details)
        {
            var dt = new DataTable();
            dt.Columns.Add("Action_Plan", typeof(string));
            dt.Columns.Add("Target_Date", typeof(DateTime));
            dt.Columns.Add("Resp", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            if (details == null)
                return dt;

            foreach (var d in details)
            {
                var row = dt.NewRow();
                row["Action_Plan"] = string.IsNullOrWhiteSpace(d.Action_Plan) ? (object)DBNull.Value : d.Action_Plan!;
                row["Target_Date"] = d.Target_Date.HasValue ? (object)d.Target_Date.Value : DBNull.Value;
                row["Resp"] = string.IsNullOrWhiteSpace(d.Resp) ? (object)DBNull.Value : d.Resp!;
                row["Status"] = string.IsNullOrWhiteSpace(d.Status) ? (object)DBNull.Value : d.Status!;
                dt.Rows.Add(row);
            }

            return dt;
        }

        public async Task<List<CAReportViewModel>> GetCAReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = @"EXEC sp_Get_CA_Report";

                var rows = await _dbContext.CAReport.FromSqlRaw(sql).ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    rows = rows
                        .Where(data => data.CreatedDate.HasValue
                                    && data.CreatedDate.Value.Date >= startDate.Value.Date
                                    && data.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                var result = rows
                    .Select(x => new CAReportViewModel
                    {
                        Id = x.Id,
                        Complaint_No = x.Complaint_No,
                        Report_Date = x.Report_Date,
                        Batch_Code = x.Batch_Code,
                        Cust_Name_Location = x.Cust_Name_Location,
                        Source_Complaint = x.Source_Complaint,
                        Supp_Qty = x.Supp_Qty,
                        Failure_Qty = x.Failure_Qty,
                        Failure = x.Failure,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                throw;
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<CAReport>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<CAReportViewModel> GetCAReportByIdAsync(int cAId)
        {
            try
            {
                var sql = "EXEC sp_Get_CA_Report_ById @Ca_Id";

                var conn = _dbContext.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                using (var multi = await conn.QueryMultipleAsync(sql, new { Ca_Id = cAId }))
                {
                    var header = (await multi.ReadAsync<CAReportViewModel>()).FirstOrDefault();
                    if (header == null)
                        return null;

                    var details = (await multi.ReadAsync<CAReportDetailViewModel>()).ToList();

                    // Build final ViewModel WITH details
                    header.Details = details
                        .Select(d => new CAReportDetailViewModel
                        {

                            Ca_Id = d.Ca_Id,
                            Moni_Plan_Id = d.Moni_Plan_Id,
                            Action_Plan = d.Action_Plan,
                            Target_Date = d.Target_Date,
                            Resp = d.Resp,
                            Status = d.Status,
                        })
                        .ToList();

                    return header;
                }
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

                IQueryable<int> query = _dbContext.CAReport
                    .Where(x => x.Deleted == false && x.Complaint_No.ToString() == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.CAReport
                        .Where(x => x.Deleted == false &&
                               x.Complaint_No.ToString() == searchText
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
