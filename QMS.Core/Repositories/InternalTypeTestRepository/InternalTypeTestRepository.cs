using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.ViewModels;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;
using QMS.Core.Models;
using QMS.Core.DatabaseContext;

namespace QMS.Core.Repositories.InternalTypeTestRepo
{
    public class InternalTypeTestRepository : SqlTableRepository, IInternalTypeTestRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public InternalTypeTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        /// <summary>
        /// Insert parent + child rows using sp_Insert_InternalTypeTest which accepts a TVP for children.
        /// </summary>
        public async Task<OperationResult> InsertInternalTypeTestAsync(InternalTypeTestViewModel model)
        {
            try
            {
                // ✅ Null check
                if (model == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Model cannot be null."
                    };
                }

                // Build TVP DataTable from model.Details
                var detailsTvp = BuildDetailsDataTable(model.Details);

                var parameters = new[]
                {
                    new SqlParameter("@Report_No", SqlDbType.NVarChar, 500) { Value = model.Report_No ?? (object)DBNull.Value },
                    new SqlParameter("@Date", SqlDbType.DateTime) { Value = model.Date ?? (object)DBNull.Value },
                    new SqlParameter("@Cust_Name", SqlDbType.NVarChar, -1) { Value = model.Cust_Name ?? (object)DBNull.Value },
                    new SqlParameter("@Samp_Identi_Lab", SqlDbType.NVarChar, 500) { Value = model.Samp_Identi_Lab ?? (object)DBNull.Value },
                    new SqlParameter("@Samp_Desc", SqlDbType.NVarChar, 500) { Value = model.Samp_Desc ?? (object)DBNull.Value },
                    new SqlParameter("@Prod_Cat_Code", SqlDbType.NVarChar, 500) { Value = model.Prod_Cat_Code ?? (object)DBNull.Value },
                    new SqlParameter("@Input_Voltage", SqlDbType.NVarChar, 500) { Value = model.Input_Voltage ?? (object)DBNull.Value },
                    new SqlParameter("@Ref_Standard", SqlDbType.NVarChar, -1) { Value = model.Ref_Standard ?? (object)DBNull.Value },
                    new SqlParameter("@TestedBy", SqlDbType.NVarChar, 500) { Value = model.TestedBy ?? (object)DBNull.Value },
                    new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 500) { Value = model.CreatedBy ?? (object)DBNull.Value },

                    // TVP parameter
                    new SqlParameter("@Details", SqlDbType.Structured)
                    {
                        TypeName = "dbo.tvp_TestDetail_InternalTypeTest",
                        Value = detailsTvp
                    }
                };

                // Execute stored procedure
                var sql = "EXEC sp_Insert_InternalTypeTest " +
                          "@Report_No, @Date, @Cust_Name, @Samp_Identi_Lab, @Samp_Desc, " +
                          "@Prod_Cat_Code, @Input_Voltage, @Ref_Standard, @TestedBy, @CreatedBy, @Details";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult
                {
                    Success = true,
                    Message = "Internal type test inserted successfully."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to insert internal type test: " + ex.Message
                };
            }
        }

        /// <summary>
        /// Creates a DataTable matching dbo.tvp_TestDetail_InternalTypeTest
        /// Columns: SeqNo INT, Perticular_Test NVARCHAR(400), Test_Method NVARCHAR(400),
        /// Test_Requirement NVARCHAR(800), Test_Result NVARCHAR(800), IsDeleted BIT
        /// </summary>
        private static DataTable BuildDetailsDataTable(System.Collections.Generic.IEnumerable<InternalTypeTestDetailViewModel>? details)
        {
            var dt = new DataTable();
            dt.Columns.Add("SeqNo", typeof(int));
            dt.Columns.Add("Perticular_Test", typeof(string));
            dt.Columns.Add("Test_Method", typeof(string));
            dt.Columns.Add("Test_Requirement", typeof(string));
            dt.Columns.Add("Test_Result", typeof(string));
            dt.Columns.Add("IsDeleted", typeof(bool));

            if (details == null)
                return dt;

            foreach (var d in details)
            {
                var row = dt.NewRow();
                row["SeqNo"] = d.SeqNo.HasValue ? (object)d.SeqNo.Value : DBNull.Value;
                row["Perticular_Test"] = string.IsNullOrEmpty(d.Perticular_Test) ? (object)DBNull.Value : d.Perticular_Test!;
                row["Test_Method"] = string.IsNullOrEmpty(d.Test_Method) ? (object)DBNull.Value : d.Test_Method!;
                row["Test_Requirement"] = string.IsNullOrEmpty(d.Test_Requirement) ? (object)DBNull.Value : d.Test_Requirement!;
                row["Test_Result"] = string.IsNullOrEmpty(d.Test_Result) ? (object)DBNull.Value : d.Test_Result!;
                row["IsDeleted"] = false;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }

    // Interface

}
