using Dapper;
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

namespace QMS.Core.Repositories.IngressProtectionRepository
{
    public class IngressProtectionRepository: SqlTableRepository, IIngressProtectionRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;
        private readonly string _connStr;
        public IngressProtectionRepository(QMSDbContext dbContext, ISystemLogService systemLogService, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
            _connStr = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<IngressProtectionTestViewModel>> GetIngressProtectionAsync()
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

                var sql = @"EXEC sp_Get_IngressProtectionTestReport";

                var result = await Task.Run(() => _dbContext.IngressProtectionTestReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new IngressProtectionTestViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        CustomerProjectName = x.CustomerProjectName,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        BatchCode = x.BatchCode,
                        Quantity = x.Quantity,
                        IPRating = x.IPRating,
                        PKD = x.PKD,
                        TestResult = x.TestResult,
                        TestedBy = x.TestedBy,
                        VerifiedBy = x.VerifiedBy,
                        AddedBy = x.AddedBy,
                        AddedOn = x.AddedOn,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedOn = x.UpdatedOn
                    })
                    .ToList());

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

        public async Task<IngressProtectionTestViewModel?> GetIngressProtectionByIdAsync(int id)
        {
            await using var con =  _dbContext.Database.GetDbConnection();

            var param = new DynamicParameters();
            param.Add("@Id", id, DbType.Int32);

            await con.OpenAsync();

            using var multi = await con.QueryMultipleAsync(
                "dbo.sp_Get_IngressProtectionTestReport_ById",
                param,
                commandType: CommandType.StoredProcedure
            );

            // 1) Parent (single row)
            var parent = await multi.ReadFirstOrDefaultAsync<IngressProtectionTestViewModel>();
            if (parent == null) return null;

            // 2) Details (list)
            var details = (await multi.ReadAsync<IngressProtectionTestDetailViewModel>()).AsList();

            parent.Details = details;

            return parent;
        }

        public async Task<OperationResult> InsertIngressProtectionAsync(IngressProtectionTestReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                // TVP
                var dtDetails = BuildIPDetailsTvp(model.Details ?? new List<IngressProtectionTest_Detail>());

                await using var con =  _dbContext.Database.GetDbConnection();
                await con.OpenAsync();

                var param = new DynamicParameters();

                // Header params - MUST match SP names
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@CustomerProjectName", model.CustomerProjectName, DbType.String);
                param.Add("@ReportDate", model.ReportDate?.Date, DbType.Date);
                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@BatchCode", model.BatchCode, DbType.String);
                param.Add("@Quantity", model.Quantity, DbType.Int32);
                param.Add("@IPRating", model.IPRating, DbType.String);
                param.Add("@PKD", model.PKD, DbType.String);
                param.Add("@TestResult", model.TestResult, DbType.String);
                param.Add("@TestedBy", model.TestedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);

                // ✅ AddedBy is INT (UserId)
                param.Add("@AddedBy", model.AddedBy, DbType.Int32);

                // TVP param
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.IPTest_Details_Type"));

                // SP returns: SELECT @NewId AS Id;
                var insertedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Insert_IngressProtectionTestReport",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!insertedId.HasValue || insertedId.Value <= 0)
                    throw new Exception("Insert failed. No Id returned from stored procedure.");

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

        private static DataTable BuildIPDetailsTvp(List<IngressProtectionTest_Detail> rows)
        {
            var dt = new DataTable();

            // MUST match dbo.IPTest_Details_Type
            dt.Columns.Add("Ip_Test", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Photo_During_Test", typeof(string));
            dt.Columns.Add("Photo_After_Test", typeof(string));
            dt.Columns.Add("Observation", typeof(string));
            dt.Columns.Add("Result", typeof(string));

            if (rows == null || rows.Count == 0) return dt;

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                dr["Ip_Test"] = (object?)r?.Ip_Test ?? DBNull.Value;
                dr["Description"] = (object?)r?.Description ?? DBNull.Value;
                dr["Photo_During_Test"] = (object?)r?.Photo_During_Test ?? DBNull.Value;
                dr["Photo_After_Test"] = (object?)r?.Photo_After_Test ?? DBNull.Value;
                dr["Observation"] = (object?)r?.Observation ?? DBNull.Value;
                dr["Result"] = (object?)r?.Result ?? DBNull.Value;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public async Task<OperationResult> UpdateIngressProtectionAsync(IngressProtectionTestReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (model.Id <= 0) throw new Exception("Invalid Id.");
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                // Build TVP (null safe)
                var dtDetails = BuildIPDetailsTvp(model.Details ?? new List<IngressProtectionTest_Detail>());

                await using var con =  _dbContext.Database.GetDbConnection();
                await con.OpenAsync();

                var param = new DynamicParameters();

                // Required
                param.Add("@Id", model.Id, DbType.Int32);

                // Header (same names as SP)
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@CustomerProjectName", model.CustomerProjectName, DbType.String);
                param.Add("@ReportDate", model.ReportDate?.Date, DbType.Date);
                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@BatchCode", model.BatchCode, DbType.String);
                param.Add("@Quantity", model.Quantity, DbType.Int32);
                param.Add("@IPRating", model.IPRating, DbType.String);
                param.Add("@PKD", model.PKD, DbType.String);
                param.Add("@TestResult", model.TestResult, DbType.String);
                param.Add("@TestedBy", model.TestedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);

                // ✅ UpdatedBy as INT user id
                param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);

                // TVP
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.IPTest_Details_Type"));

                // SP returns: SELECT @Id AS Id;
                var updatedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Update_IngressProtectionTestReport",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!updatedId.HasValue || updatedId.Value <= 0)
                    throw new Exception("Update failed. No Id returned from stored procedure.");

                result.Success = true;
                result.ObjectId = updatedId.Value;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<OperationResult> DeleteIngressProtectionAsync(int id)
        {
            var result = new OperationResult();

            try
            {
                if (id <= 0) throw new Exception("Invalid Id.");

                await using var con =  _dbContext.Database.GetDbConnection();
                await con.OpenAsync();

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32);

                var deletedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Delete_IngressProtectionTestReport",
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

                IQueryable<int> query = _dbContext.IngressProtectionTestReports
                    .Where(x => x.Deleted == false && x.ReportNo == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.IngressProtectionTestReports
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
