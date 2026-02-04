using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.GlowWireTestRepository
{
    public class GlowWireTestRepository : SqlTableRepository , IGlowWireTestRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;
        private readonly string _connStr;
        public GlowWireTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
            _connStr = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<GlowWireTestReportViewModel>> GetGlowWireTestAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Id", SqlDbType.Int)
                {
                    Value = 0
                }
            };

                var sql = @"EXEC sp_Get_GlowWireTestReport";

                var result = await Task.Run(() => _dbContext.GlowWireTestReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new GlowWireTestReportViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        BatchCode = x.BatchCode,
                        Quantity = x.Quantity,
                        PKD = x.PKD,
                        TestResult = x.TestResult,
                        TestedBy = x.TestedBy,
                        VerifiedBy = x.VerifiedBy,
                        AddedBy = x.AddedBy,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedOn = x.UpdatedOn,
                        AddedOn = x.AddedOn
                    })
                    .ToList());

                if (startDate.HasValue && endDate.HasValue)
                {
                    var s = startDate.Value.Date;
                    var e = endDate.Value.Date;

                    result = result
                        .Where(d => d.ReportDate?.Date >= s && d.ReportDate?.Date <= e)
                        .ToList();
                }

                foreach (var rec in result)
                {
                    rec.User = await _dbContext.User.Where(i => i.Id == rec.AddedBy).Select(x => x.Name).FirstOrDefaultAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<GlowWireTestReportViewModel?> GetGlowWireTestByIdAsync(int id)
        {
            await using var con = _dbContext.Database.GetDbConnection();

            var param = new DynamicParameters();
            param.Add("@Id", id, DbType.Int32);

            await con.OpenAsync();

            using var multi = await con.QueryMultipleAsync(
                "dbo.sp_Get_GlowWireTestReport_ById",
                param,
                commandType: CommandType.StoredProcedure
            );

            // 1) Parent (single row)
            var parent = await multi.ReadFirstOrDefaultAsync<GlowWireTestReportViewModel>();
            if (parent == null) return null;

            // 2) Details (list)
            var details = (await multi.ReadAsync<GlowWireTestReportDetailViewModel>()).AsList();

            
            parent.Details = details;

            return parent;
        }

        public async Task<OperationResult> InsertGlowWireTestAsync(GlowWireTestReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrWhiteSpace(model.ReportNo))
                    throw new Exception("ReportNo is required.");

                // 1) Build TVP DataTable (null safe)
                var dtDetails = BuildGlowWireDetailsTvp(model.Details ?? new List<GlowWireTestReportDetail>());

                // 2) Get EF connection and open
                var con = _dbContext.Database.GetDbConnection();
                if (con.State != ConnectionState.Open)
                    await con.OpenAsync();

                var param = new DynamicParameters();

                // ✅ Parent params (MUST match SP parameters)
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@CustomerProjectName", model.CustomerProjectName, DbType.String);

                // Send null safely (Dapper handles null as DBNull)
                param.Add("@ReportDate", model.ReportDate, DbType.DateTime);

                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@BatchCode", model.BatchCode, DbType.String);
                param.Add("@Quantity", model.Quantity, DbType.Int32);
                param.Add("@PartDescription", model.PartDescription, DbType.String);
                param.Add("@PKD", model.PKD, DbType.String);
                param.Add("@TestResult", model.TestResult, DbType.String);
                param.Add("@TestedBy", model.TestedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);
                param.Add("@AddedBy", model.AddedBy, DbType.Int32);

                // ✅ TVP param (MUST match TVP type name in SQL)
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.GlowWireTestDetail_TVP"));

                // 3) Execute SP and return new Id
                var insertedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Insert_GlowWireTestReport",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!insertedId.HasValue || insertedId.Value <= 0)
                    throw new Exception("Insert failed. No GlowTestId returned from stored procedure.");

                result.Success = true;
                result.ObjectId = insertedId.Value;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static DataTable BuildGlowWireDetailsTvp(List<GlowWireTestReportDetail> rows)
        {
            var dt = new DataTable();

            // ✅ MUST match dbo.GlowWireTestDetail_TVP columns EXACTLY (name + order + type)
            dt.Columns.Add("Test_Ref", typeof(string));
            dt.Columns.Add("Specified_Req", typeof(string));
            dt.Columns.Add("Observation", typeof(string));
            dt.Columns.Add("Result", typeof(string));
            dt.Columns.Add("Photo_During_Test", typeof(string));
            dt.Columns.Add("Photo_After_Test", typeof(string));

            if (rows == null || rows.Count == 0)
                return dt;

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                dr["Test_Ref"] = (object?)r?.Test_Ref ?? DBNull.Value;
                dr["Specified_Req"] = (object?)r?.Specified_Req ?? DBNull.Value;
                dr["Observation"] = (object?)r?.Observation ?? DBNull.Value;
                dr["Result"] = (object?)r?.Result ?? DBNull.Value;
                dr["Photo_During_Test"] = (object?)r?.Photo_During_Test ?? DBNull.Value;
                dr["Photo_After_Test"] = (object?)r?.Photo_After_Test ?? DBNull.Value;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public async Task<OperationResult> UpdateGlowWireTestAsync(GlowWireTestReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (model.Id <= 0) throw new Exception("Invalid Id.");
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                // Build TVP tables (null safe)
                var dtDetails = BuildGlowWireDetailsTvp(model.Details ?? new List<GlowWireTestReportDetail>());

                await using var con = _dbContext.Database.GetDbConnection();
                await con.OpenAsync();

                var param = new DynamicParameters();

                // Required Parent Id
                param.Add("@Id", model.Id, DbType.Int32);

                // Parent params (same names as SP)
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@CustomerProjectName", model.CustomerProjectName, DbType.String);

                // Send null safely (Dapper handles null as DBNull)
                param.Add("@ReportDate", model.ReportDate, DbType.DateTime);

                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@BatchCode", model.BatchCode, DbType.String);
                param.Add("@Quantity", model.Quantity, DbType.Int32);
                param.Add("@PartDescription", model.PartDescription, DbType.String);
                param.Add("@PKD", model.PKD, DbType.String);
                param.Add("@TestResult", model.TestResult, DbType.String);
                param.Add("@TestedBy", model.TestedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);
                param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);

                // TVP params
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.GlowWireTestDetail_TVP"));

                // SP returns: SELECT @Id AS GlowWireTest_Id;
                var updatedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Update_GlowWireTestReport",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!updatedId.HasValue || updatedId.Value <= 0)
                    throw new Exception("Update failed. No GlowWireTestId returned from stored procedure.");

                result.Success = true;
                result.ObjectId = updatedId.Value;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message; // change if your OperationResult uses different field
                return result;
            }
        }

        public async Task<OperationResult> DeleteGlowWireTestAsync(int id)
        {
            var result = new OperationResult();

            try
            {
                if (id <= 0) throw new Exception("Invalid Id.");

                await using var con = _dbContext.Database.GetDbConnection();
                await con.OpenAsync();

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32);

                var deletedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Delete_GlowWireTestReport",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!deletedId.HasValue || deletedId.Value <= 0)
                    throw new Exception("Delete failed.");

                result.Success = true;
                result.ObjectId = deletedId.Value;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<bool> CheckDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.GlowWireTestReports
                    .Where(x => x.Deleted == false && x.ReportNo == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.GlowWireTestReports
                        .Where(x => x.Deleted == false &&
                               x.ReportNo == searchText
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
