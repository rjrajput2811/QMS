using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.RCAReportRepository;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.RCAReportRepository
{
    public class RCAReportRepository : SqlTableRepository, IRCAReportRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public RCAReportRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<RCAReportViewModel>> GetRCAReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = @"EXEC sp_Get_RCA_Report";

                var rows = await _dbContext.RCAReport.FromSqlRaw(sql).ToListAsync();

                if (startDate.HasValue && endDate.HasValue)
                {
                    rows = rows
                        .Where(data => data.CreatedDate.HasValue
                                    && data.CreatedDate.Value.Date >= startDate.Value.Date
                                    && data.CreatedDate.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                var result = rows
                    .Select(x => new RCAReportViewModel
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
                return await base.DeleteAsync<RCAReport>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<RCAReportViewModel> GetRCAReportByIdAsync(int id)
        {
            try
            {
                var sql = "EXEC sp_Get_RCA_Report_ById @Rca_Id";

                var conn = _dbContext.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                using (var multi = await conn.QueryMultipleAsync(sql, new { Rca_Id = id }))
                {
                    var header = (await multi.ReadAsync<RCAReportViewModel>()).FirstOrDefault();
                    if (header == null)
                        return null;

                    var details = (await multi.ReadAsync<RCAReportDetailViewModel>()).ToList();

                    // Build final ViewModel WITH details
                    header.Details = details
                        .Select(d => new RCAReportDetailViewModel
                        {

                            RCa_Id = d.RCa_Id,
                            Parameter_Checked = d.Parameter_Checked,
                            Observations = d.Observations,
                            Init_Ana_Id = d.Init_Ana_Id
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

                IQueryable<int> query = _dbContext.RCAReport
                    .Where(x => x.Deleted == false && x.Complaint_No.ToString() == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.RCAReport
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

        private static DataTable BuildDetailListDataTable(IEnumerable<RCAReportDetailViewModel>? details)
        {
            var dt = new DataTable();

            // These names & types MUST match RCA_Report_DetailType
            dt.Columns.Add("Parameter_Checked", typeof(string));
            dt.Columns.Add("Observations", typeof(string));

            if (details == null)
                return dt;

            foreach (var d in details)
            {
                var row = dt.NewRow();

                row["Parameter_Checked"] =
                    string.IsNullOrWhiteSpace(d.Parameter_Checked)
                        ? (object)DBNull.Value
                        : d.Parameter_Checked!;

                row["Observations"] =
                    string.IsNullOrWhiteSpace(d.Observations)
                        ? (object)DBNull.Value
                        : d.Observations!;

                dt.Rows.Add(row);
            }

            return dt;
        }


        public async Task<OperationResult> InsertRCAReportAsync(RCAReportViewModel model)
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

                // Build TVP for @DetailList
                var detailTvp = BuildDetailListDataTable(model.Details);

                var parameters = new[]
                {
            new SqlParameter("@Complaint_No", SqlDbType.NVarChar, 600)
            {
                Value = (object?)model.Complaint_No ?? DBNull.Value
            },
            new SqlParameter("@Report_Date", SqlDbType.DateTime)
            {
                Value = (object?)model.Report_Date ?? DBNull.Value
            },
            new SqlParameter("@Cust_Complaints", SqlDbType.Bit)
            {
                Value = model.Cust_Complaints
            },
            new SqlParameter("@NPI_Validations", SqlDbType.Bit)
            {
                Value = model.NPI_Validations
            },
            new SqlParameter("@PDI_Obser", SqlDbType.Bit)
            {
                Value = model.PDI_Obser
            },
            new SqlParameter("@System", SqlDbType.Bit)
            {
                Value = model.System
            },
            new SqlParameter("@Cust_Name_Location", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Cust_Name_Location ?? DBNull.Value
            },
            new SqlParameter("@Source_Complaint", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Source_Complaint ?? DBNull.Value
            },
            new SqlParameter("@Prod_Code_Desc", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Prod_Code_Desc ?? DBNull.Value
            },
            new SqlParameter("@Desc_Complaint", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Desc_Complaint ?? DBNull.Value
            },
            new SqlParameter("@Batch_Code", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Batch_Code ?? DBNull.Value
            },
            new SqlParameter("@Pkd", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Pkd ?? DBNull.Value
            },
            new SqlParameter("@Supp_Qty", SqlDbType.Int)
            {
                Value = (object?)model.Supp_Qty ?? DBNull.Value
            },
            new SqlParameter("@Failure_Qty", SqlDbType.Int)
            {
                Value = (object?)model.Failure_Qty ?? DBNull.Value
            },
            new SqlParameter("@Failure", SqlDbType.Int)
            {
                Value = (object?)model.Failure ?? DBNull.Value
            },
            new SqlParameter("@Age_Install", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Age_Install ?? DBNull.Value
            },
            new SqlParameter("@Description", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Description ?? DBNull.Value
            },
            new SqlParameter("@Problem_State", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_State ?? DBNull.Value
            },
            new SqlParameter("@Problem_Visual_ImgA", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_Visual_ImgA ?? DBNull.Value
            },
            new SqlParameter("@Problem_Visual_ImgB", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_Visual_ImgB ?? DBNull.Value
            },
            new SqlParameter("@Problem_Visual_ImgC", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_Visual_ImgC ?? DBNull.Value
            },
            new SqlParameter("@Initial_Observ", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Initial_Observ ?? DBNull.Value
            },
            new SqlParameter("@Man_Issue_Prob", SqlDbType.Bit)
            {
                Value = model.Man_Issue_Prob
            },
            new SqlParameter("@Design_Prob", SqlDbType.Bit)
            {
                Value = model.Design_Prob
            },
            new SqlParameter("@Site_Issue_Prob", SqlDbType.Bit)
            {
                Value = model.Site_Issue_Prob
            },
            new SqlParameter("@Com_Gap_Prob", SqlDbType.Bit)
            {
                Value = model.Com_Gap_Prob
            },
            new SqlParameter("@Install_Issues_Prob", SqlDbType.Bit)
            {
                Value = model.Install_Issues_Prob
            },
            new SqlParameter("@Wrong_App_Prob", SqlDbType.Bit)
            {
                Value = model.Wrong_App_Prob
            },

            // ===== New RCA fields =====
            new SqlParameter("@Complaint_History", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Complaint_History ?? DBNull.Value
            },
            new SqlParameter("@Current_Process", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Current_Process ?? DBNull.Value
            },
            new SqlParameter("@Root_Cause_Anal", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Root_Cause_Anal ?? DBNull.Value
            },
            new SqlParameter("@Corrective_Action", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Corrective_Action ?? DBNull.Value
            },
            new SqlParameter("@Analysis_of_Defective100", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Analysis_of_Defective100 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples1", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples1 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples2", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples2 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples3", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples3 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples4", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples4 ?? DBNull.Value
            },
            new SqlParameter("@Images_Corrections", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Corrections ?? DBNull.Value
            },
            new SqlParameter("@Conclusion", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Conclusion ?? DBNull.Value
            },

            new SqlParameter("@Rca_Prepared_By", SqlDbType.NVarChar, 500)
            {
                Value = (object?)model.Rca_Prepared_By ?? DBNull.Value
            },
            new SqlParameter("@Name_Designation", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Name_Designation ?? DBNull.Value
            },
            new SqlParameter("@Date", SqlDbType.DateTime)
            {
                Value = (object?)model.Date ?? DBNull.Value
            },
            new SqlParameter("@Root_Cause_Date", SqlDbType.DateTime) {
                        Value = (object?)model.Root_Cause_Date ?? DBNull.Value
                    },
                    new SqlParameter("@Corrective_Action_Date", SqlDbType.DateTime) {
                        Value = (object?)model.Corrective_Action_Date ?? DBNull.Value
                    },
            new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 500)
            {
                Value = (object?)model.CreatedBy ?? DBNull.Value
            },

            // ========== TVP ==========
            new SqlParameter("@DetailList", SqlDbType.Structured)
            {
                TypeName = "dbo.RCA_Report_DetailType",
                Value = detailTvp
            }
        };

                var sql =
                    "EXEC sp_Insert_RCA_Report " +
                    "@Complaint_No, @Report_Date, @Cust_Complaints, @NPI_Validations, @PDI_Obser, @System, " +
                    "@Cust_Name_Location, @Source_Complaint, @Prod_Code_Desc, @Desc_Complaint, @Batch_Code, @Pkd, " +
                    "@Supp_Qty, @Failure_Qty, @Failure, @Age_Install, @Description, @Problem_State, " +
                    "@Problem_Visual_ImgA, @Problem_Visual_ImgB, @Problem_Visual_ImgC, @Initial_Observ, " +
                    "@Man_Issue_Prob, @Design_Prob, @Site_Issue_Prob, @Com_Gap_Prob, @Install_Issues_Prob, " +
                    "@Wrong_App_Prob, @Complaint_History, @Current_Process, @Root_Cause_Anal, @Corrective_Action, " +
                    "@Analysis_of_Defective100, @Images_Failed_Samples1, @Images_Failed_Samples2, " +
                    "@Images_Failed_Samples3, @Images_Failed_Samples4, @Images_Corrections, @Conclusion, " +
                    "@Rca_Prepared_By, @Name_Designation, @Date, @Root_Cause_Date, @Corrective_Action_Date, @CreatedBy, @DetailList";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult
                {
                    Success = true,
                    Message = "Customer RCA Report inserted successfully."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to insert Customer RCA Report: " + ex.Message
                };
            }
        }

        public async Task<OperationResult> UpdateRCAReportAsync(RCAReportViewModel model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Invalid RCA Report model or Id."
                    };
                }

                // Build TVP for @DetailList
                var detailTvp = BuildDetailListDataTable(model.Details);

                var parameters = new[]
                {
            new SqlParameter("@Rca_Id", SqlDbType.Int)
            {
                Value = model.Id
            },

            new SqlParameter("@Complaint_No", SqlDbType.NVarChar, 600)
            {
                Value = (object?)model.Complaint_No ?? DBNull.Value
            },
            new SqlParameter("@Report_Date", SqlDbType.DateTime)
            {
                Value = (object?)model.Report_Date ?? DBNull.Value
            },
            new SqlParameter("@Cust_Complaints", SqlDbType.Bit)
            {
                Value = model.Cust_Complaints
            },
            new SqlParameter("@NPI_Validations", SqlDbType.Bit)
            {
                Value = model.NPI_Validations
            },
            new SqlParameter("@PDI_Obser", SqlDbType.Bit)
            {
                Value = model.PDI_Obser
            },
            new SqlParameter("@System", SqlDbType.Bit)
            {
                Value = model.System
            },
            new SqlParameter("@Cust_Name_Location", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Cust_Name_Location ?? DBNull.Value
            },
            new SqlParameter("@Source_Complaint", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Source_Complaint ?? DBNull.Value
            },
            new SqlParameter("@Prod_Code_Desc", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Prod_Code_Desc ?? DBNull.Value
            },
            new SqlParameter("@Desc_Complaint", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Desc_Complaint ?? DBNull.Value
            },
            new SqlParameter("@Batch_Code", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Batch_Code ?? DBNull.Value
            },
            new SqlParameter("@Pkd", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Pkd ?? DBNull.Value
            },
            new SqlParameter("@Supp_Qty", SqlDbType.Int)
            {
                Value = (object?)model.Supp_Qty ?? DBNull.Value
            },
            new SqlParameter("@Failure_Qty", SqlDbType.Int)
            {
                Value = (object?)model.Failure_Qty ?? DBNull.Value
            },
            new SqlParameter("@Failure", SqlDbType.Int)
            {
                Value = (object?)model.Failure ?? DBNull.Value
            },
            new SqlParameter("@Age_Install", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Age_Install ?? DBNull.Value
            },
            new SqlParameter("@Description", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Description ?? DBNull.Value
            },
            new SqlParameter("@Problem_State", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_State ?? DBNull.Value
            },
            new SqlParameter("@Problem_Visual_ImgA", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_Visual_ImgA ?? DBNull.Value
            },
            new SqlParameter("@Problem_Visual_ImgB", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_Visual_ImgB ?? DBNull.Value
            },
            new SqlParameter("@Problem_Visual_ImgC", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Problem_Visual_ImgC ?? DBNull.Value
            },
            new SqlParameter("@Initial_Observ", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Initial_Observ ?? DBNull.Value
            },
            new SqlParameter("@Man_Issue_Prob", SqlDbType.Bit)
            {
                Value = model.Man_Issue_Prob
            },
            new SqlParameter("@Design_Prob", SqlDbType.Bit)
            {
                Value = model.Design_Prob
            },
            new SqlParameter("@Site_Issue_Prob", SqlDbType.Bit)
            {
                Value = model.Site_Issue_Prob
            },
            new SqlParameter("@Com_Gap_Prob", SqlDbType.Bit)
            {
                Value = model.Com_Gap_Prob
            },
            new SqlParameter("@Install_Issues_Prob", SqlDbType.Bit)
            {
                Value = model.Install_Issues_Prob
            },
            new SqlParameter("@Wrong_App_Prob", SqlDbType.Bit)
            {
                Value = model.Wrong_App_Prob
            },

            // ===== New RCA fields =====
            new SqlParameter("@Complaint_History", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Complaint_History ?? DBNull.Value
            },
            new SqlParameter("@Current_Process", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Current_Process ?? DBNull.Value
            },
            new SqlParameter("@Root_Cause_Anal", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Root_Cause_Anal ?? DBNull.Value
            },
            new SqlParameter("@Corrective_Action", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Corrective_Action ?? DBNull.Value
            },
            new SqlParameter("@Analysis_of_Defective100", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Analysis_of_Defective100 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples1", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples1 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples2", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples2 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples3", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples3 ?? DBNull.Value
            },
            new SqlParameter("@Images_Failed_Samples4", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Failed_Samples4 ?? DBNull.Value
            },
            new SqlParameter("@Images_Corrections", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Images_Corrections ?? DBNull.Value
            },
            new SqlParameter("@Conclusion", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Conclusion ?? DBNull.Value
            },

            new SqlParameter("@Rca_Prepared_By", SqlDbType.NVarChar, 500)
            {
                Value = (object?)model.Rca_Prepared_By ?? DBNull.Value
            },
            new SqlParameter("@Name_Designation", SqlDbType.NVarChar, -1)
            {
                Value = (object?)model.Name_Designation ?? DBNull.Value
            },
            new SqlParameter("@Date", SqlDbType.DateTime)
            {
                Value = (object?)model.Date ?? DBNull.Value
            },
             new SqlParameter("@Root_Cause_Date", SqlDbType.DateTime) {
                        Value = (object?)model.Root_Cause_Date ?? DBNull.Value
                    },
                    new SqlParameter("@Corrective_Action_Date", SqlDbType.DateTime) {
                        Value = (object?)model.Corrective_Action_Date ?? DBNull.Value
                    },
            new SqlParameter("@UpdatedBy", SqlDbType.NVarChar, 500)
            {
                Value = (object?)model.UpdatedBy ?? DBNull.Value
            },

            // ===== TVP =====
            new SqlParameter("@DetailList", SqlDbType.Structured)
            {
                TypeName = "dbo.RCA_Report_DetailType",
                Value = detailTvp
            }
        };

                var sql =
                    "EXEC sp_Update_RCA_Report " +
                    "@Rca_Id, @Complaint_No, @Report_Date, @Cust_Complaints, @NPI_Validations, @PDI_Obser, @System, " +
                    "@Cust_Name_Location, @Source_Complaint, @Prod_Code_Desc, @Desc_Complaint, @Batch_Code, @Pkd, " +
                    "@Supp_Qty, @Failure_Qty, @Failure, @Age_Install, @Description, @Problem_State, " +
                    "@Problem_Visual_ImgA, @Problem_Visual_ImgB, @Problem_Visual_ImgC, @Initial_Observ, " +
                    "@Man_Issue_Prob, @Design_Prob, @Site_Issue_Prob, @Com_Gap_Prob, @Install_Issues_Prob, " +
                    "@Wrong_App_Prob, @Complaint_History, @Current_Process, @Root_Cause_Anal, @Corrective_Action, " +
                    "@Analysis_of_Defective100, @Images_Failed_Samples1, @Images_Failed_Samples2, " +
                    "@Images_Failed_Samples3, @Images_Failed_Samples4, @Images_Corrections, @Conclusion, " +
                    "@Rca_Prepared_By, @Name_Designation, @Date,@Root_Cause_Date, @Corrective_Action_Date, @UpdatedBy, @DetailList";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult
                {
                    Success = true,
                    Message = "Customer RCA Report updated successfully."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.ToString());
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to update Customer RCA Report: " + ex.Message
                };
            }
        }

    }
}
