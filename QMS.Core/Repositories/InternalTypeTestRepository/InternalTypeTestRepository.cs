using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.ViewModels;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;
using QMS.Core.Models;
using QMS.Core.DatabaseContext;
using Dapper;

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

  
        public async Task<OperationResult> InsertInternalTypeTestAsync(InternalTypeTestViewModel model)
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

  
                   new SqlParameter("@Details", SqlDbType.Structured)
                   {
                       TypeName = "dbo.tvp_TestDetail_InternalType",
                       Value = detailsTvp
                    }
                };


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
        public async Task<OperationResult> UpdateInternalTypeTestAsync(InternalTypeTestViewModel model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Invalid model. Internal_TypeId is required for update."
                    };
                }

                // Build TVP for child details (re-uses your existing helper)
                var detailsTvp = BuildDetailsDataTable(model.Details);

                var parameters = new[]
                {
            new SqlParameter("@Internal_TypeId", SqlDbType.Int) { Value = model.Id },

            new SqlParameter("@Report_No", SqlDbType.NVarChar, 500) { Value = model.Report_No ?? (object)DBNull.Value },
            new SqlParameter("@Date", SqlDbType.DateTime) { Value = model.Date ?? (object)DBNull.Value },
            new SqlParameter("@Cust_Name", SqlDbType.NVarChar, -1) { Value = model.Cust_Name ?? (object)DBNull.Value },
            new SqlParameter("@Samp_Identi_Lab", SqlDbType.NVarChar, 500) { Value = model.Samp_Identi_Lab ?? (object)DBNull.Value },
            new SqlParameter("@Samp_Desc", SqlDbType.NVarChar, 500) { Value = model.Samp_Desc ?? (object)DBNull.Value },
            new SqlParameter("@Prod_Cat_Code", SqlDbType.NVarChar, 500) { Value = model.Prod_Cat_Code ?? (object)DBNull.Value },
            new SqlParameter("@Input_Voltage", SqlDbType.NVarChar, 500) { Value = model.Input_Voltage ?? (object)DBNull.Value },
            new SqlParameter("@Ref_Standard", SqlDbType.NVarChar, -1) { Value = model.Ref_Standard ?? (object)DBNull.Value },
            new SqlParameter("@TestedBy", SqlDbType.NVarChar, 500) { Value = model.TestedBy ?? (object)DBNull.Value },

            // UpdatedBy (SP expects NVARCHAR(100) in your script)
            new SqlParameter("@UpdatedBy", SqlDbType.NVarChar, 100) { Value = model.UpdatedBy ?? (object)DBNull.Value },

            new SqlParameter("@Details", SqlDbType.Structured)
            {
                TypeName = "dbo.tvp_TestDetail_InternalType",
                Value = detailsTvp
            }
        };

                var sql = "EXEC sp_Update_InternalTypeTest " +
                          "@Internal_TypeId, @Report_No, @Date, @Cust_Name, @Samp_Identi_Lab, @Samp_Desc, " +
                          "@Prod_Cat_Code, @Input_Voltage, @Ref_Standard, @TestedBy, @UpdatedBy, @Details";

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult
                {
                    Success = true,
                    Message = "Internal type test updated successfully."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to update internal type test: " + ex.Message
                };
            }
        }

        private static DataTable BuildDetailsDataTable(System.Collections.Generic.IEnumerable<InternalTypeTestDetailViewModel>? details)
        {
            var dt = new DataTable();
            dt.Columns.Add("InternalType_DetId", typeof(int));
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
                row["InternalType_DetId"] = d.Id > 0 ? d.Id : DBNull.Value;
                //row["SeqNo"] = d.SeqNo.HasValue ? (object)d.SeqNo.Value : DBNull.Value;
                row["Perticular_Test"] = string.IsNullOrEmpty(d.Perticular_Test) ? (object)DBNull.Value : d.Perticular_Test!;
                row["Test_Method"] = string.IsNullOrEmpty(d.Test_Method) ? (object)DBNull.Value : d.Test_Method!;
                row["Test_Requirement"] = string.IsNullOrEmpty(d.Test_Requirement) ? (object)DBNull.Value : d.Test_Requirement!;
                row["Test_Result"] = string.IsNullOrEmpty(d.Test_Result) ? (object)DBNull.Value : d.Test_Result!;
                row["IsDeleted"] = false;
                dt.Rows.Add(row);
            }

            return dt;
        }

        public async Task<List<InternalTypeTestViewModel>> GetInternalTypeTestsAsync()
        {
            try
            {
                // If you have a stored procedure for listing, call it here.
                // Replace sp_Get_InternalTypeTests with your actual proc name.
                var sql = @"EXEC sp_Get_InternalTypeTest";

                var result = await Task.Run(() => _dbContext.InternalTypeTests
                    .FromSqlRaw(sql)
                    .AsEnumerable()
                    .Select(x => new InternalTypeTestViewModel
                    {
                        Id = x.Id,
                        Report_No = x.Report_No,
                        Date = x.Date,
                        Cust_Name = x.Cust_Name,
                        Samp_Identi_Lab = x.Samp_Identi_Lab,
                        Samp_Desc = x.Samp_Desc,
                        Prod_Cat_Code = x.Prod_Cat_Code,
                        Input_Voltage = x.Input_Voltage,
                        Ref_Standard = x.Ref_Standard,
                        TestedBy = x.TestedBy,
                        CreatedBy= x.CreatedBy,   
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        Deleted = x.Deleted,

                    })
                    .ToList());

                // Fill CreatedBy name (if CreatedBy is a user id) - same pattern as electrical
                foreach (var rec in result)
                {
                    // try to fetch the user name if CreatedBy stored as int user id
                    if (int.TryParse(rec.CreatedBy, out int userId))
                    {
                        rec.CreatedBy = await _dbContext.User
                            .Where(u => u.Id == userId)
                            .Select(u => u.Name)
                            .FirstOrDefaultAsync();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
        public async Task<OperationResult> DeleteInternalTypeTestAsync(int id)
        {
            try
            {
                var result = await base.DeleteAsync<InternalTypeTest>(id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
        public async Task<InternalTypeTestViewModel> GetInternalTypeTestByIdAsync(int internalTypeId)
        {
            try
            {
                var sql = "EXEC sp_Get_InternalTypeTest_ById @Internal_TypeId";

                var conn = _dbContext.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                using (var multi = await conn.QueryMultipleAsync(sql, new { Internal_TypeId = internalTypeId }))
                {
                    // PARENT (1st result set)
                    var header = (await multi.ReadAsync<InternalTypeTestViewModel>()).FirstOrDefault();
                    if (header == null)
                        return null;

                    // CHILD (2nd result set)
                    var details = (await multi.ReadAsync<InternalTypeTestDetailViewModel>()).ToList();

                    // Build final ViewModel WITH details
                    header.Details = details
                        .Select(d => new InternalTypeTestDetailViewModel
                        {

                            Id = d.Id,
                            Internal_TypeId = d.Internal_TypeId,
                            Perticular_Test = d.Perticular_Test,
                            Test_Method = d.Test_Method,
                            Test_Requirement = d.Test_Requirement,
                            Test_Result = d.Test_Result,
                            CreatedBy = d.CreatedBy,
                            CreatedDate = d.CreatedDate,
                            UpdatedBy = d.UpdatedBy,
                            UpdatedDate = d.UpdatedDate,
                            Deleted = d.Deleted
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




    }



}
