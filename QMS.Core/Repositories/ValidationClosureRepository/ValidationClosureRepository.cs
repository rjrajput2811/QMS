using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ValidationClosureRepository;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using DataTable = System.Data.DataTable;

namespace QMS.Core.Repositories.ValidationClosureRepository
{
    public class ValidationClosureRepository : SqlTableRepository, IValidationClosureRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;
        private readonly string _connStr;
        public ValidationClosureRepository(QMSDbContext dbContext, ISystemLogService systemLogService, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
            _connStr = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<ValidClosureReportViewModel>> GetValidationClosureAsync(DateTime? startDate = null, DateTime? endDate = null)
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

                var sql = @"EXEC sp_Get_ValidationClosureReport";

                var result = await Task.Run(() => _dbContext.ValidationClosureReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new ValidClosureReportViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        BatchCode = x.BatchCode,
                        QuantityOffered = x.QuantityOffered,
                        PKD = x.PKD,
                        ValidationDoneBy = x.ValidationDoneBy,
                        ClosureDate = x.ClosureDate,
                        WiproQA_FinalComments = x.WiproQA_FinalComments,
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

        public async Task<ValidClosureReportViewModel?> GetValidationClosureByIdAsync(int id)
        {
            await using var con = _dbContext.Database.GetDbConnection();

            var param = new DynamicParameters();
            param.Add("@Id", id, DbType.Int32);

            await con.OpenAsync();

            using var multi = await con.QueryMultipleAsync(
                "dbo.sp_Get_ValidationClosureReport_ById",
                param,
                commandType: CommandType.StoredProcedure
            );

            // 1) Parent (single row)
            var parent = await multi.ReadFirstOrDefaultAsync<ValidClosureReportViewModel>();
            if (parent == null) return null;

            // 2) Details (list)
            var details = (await multi.ReadAsync<ValidationClosureDetailViewModel>()).AsList();


            parent.Details = details;

            return parent;
        }


        public async Task<OperationResult> InsertValidationClosureAsync(ValidationClosureReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrWhiteSpace(model.ReportNo))
                    throw new Exception("ReportNo is required.");

                // 1) Build TVP (null safe)
                var dtDetails = BuildValidationClosureDetailsTvp(model.Details ?? new List<ValidationClosureReport_Detail>());

                // 2) Get EF connection and open
                DbConnection con = _dbContext.Database.GetDbConnection();
                if (con.State != ConnectionState.Open)
                    await con.OpenAsync();

                var param = new DynamicParameters();

                // ✅ MUST match SP parameter names EXACTLY
                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@ReportDate", model.ReportDate, DbType.DateTime);

                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@ValidationDoneBy", model.ValidationDoneBy, DbType.String);
                param.Add("@BatchCode", model.BatchCode, DbType.String);
                param.Add("@PKD", model.PKD, DbType.String);

                // NOT NULL in SP/table
                param.Add("@QuantityOffered", model.QuantityOffered, DbType.Int32);

                param.Add("@ClosureDate", model.ClosureDate, DbType.DateTime);
                param.Add("@VendorQA_FinalComments", model.VendorQA_FinalComments, DbType.String);
                param.Add("@WiproQA_FinalComments", model.WiproQA_FinalComments, DbType.String);
                param.Add("@VendorQA_Signature", model.VendorQA_Signature, DbType.String);
                param.Add("@WiproQA_Signature", model.WiproQA_Signature, DbType.String);
                param.Add("@ReportAttached", model.ReportAttached, DbType.String);

                // NOT NULL in SP/table
                param.Add("@AddedBy", model.AddedBy, DbType.Int32);

                // SP will set GETDATE() if null
                param.Add("@AddedOn", model.AddedOn, DbType.DateTime);

                // ✅ TVP param (MUST match type name in SQL)
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.TVP_ValidationClosureDetail"));

                // 3) Execute SP and return new Id
                var insertedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Insert_ValidationClosure",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!insertedId.HasValue || insertedId.Value <= 0)
                    throw new Exception("Insert failed. No ValidClose_Id returned from stored procedure.");

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

        private static DataTable BuildValidationClosureDetailsTvp(List<ValidationClosureReport_Detail> rows)
        {
            var dt = new DataTable();

            dt.Columns.Add("Open_Point", typeof(string));
            dt.Columns.Add("Action_Taken", typeof(string));
            dt.Columns.Add("Stake_Holder", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Evidence", typeof(string));
            dt.Columns.Add("Attached", typeof(string));
            dt.Columns.Add("Delete", typeof(bool));

            if (rows == null || rows.Count == 0)
                return dt;

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                dr["Open_Point"] = (object?)r?.Open_Point ?? DBNull.Value;
                dr["Action_Taken"] = (object?)r?.Action_Taken ?? DBNull.Value;
                dr["Stake_Holder"] = (object?)r?.Stake_Holder ?? DBNull.Value;
                dr["Status"] = (object?)r?.Status ?? DBNull.Value;
                dr["Evidence"] = (object?)r?.Evidence ?? DBNull.Value;
                dr["Attached"] = (object?)r?.Attached ?? DBNull.Value;
                dr["Delete"] = (object?)r?.Delete ?? DBNull.Value;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public async Task<OperationResult> UpdateValidationClosureAsync(ValidationClosureReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (model.Id <= 0) throw new Exception("Invalid Id.");
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                // QuantityOffered is NOT NULL in SP/table
                // If you want to enforce > 0 then validate here
                // if (model.QuantityOffered <= 0) throw new Exception("QuantityOffered is required.");

                // Build TVP (null safe)
                var dtDetails = BuildValidationClosureDetailsTvp(model.Details ?? new List<ValidationClosureReport_Detail>());

                await using DbConnection con = _dbContext.Database.GetDbConnection();
                if (con.State != ConnectionState.Open)
                    await con.OpenAsync();

                var param = new DynamicParameters();

                // ✅ Required Parent Id (must match SP param)
                param.Add("@Id", model.Id, DbType.Int32);

                // ✅ Parent params (MUST match SP names exactly)
                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@ReportDate", model.ReportDate, DbType.DateTime);

                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@ValidationDoneBy", model.ValidationDoneBy, DbType.String);
                param.Add("@BatchCode", model.BatchCode, DbType.String);
                param.Add("@PKD", model.PKD, DbType.String);

                param.Add("@QuantityOffered", model.QuantityOffered, DbType.Int32);

                param.Add("@ClosureDate", model.ClosureDate, DbType.DateTime);
                param.Add("@VendorQA_FinalComments", model.VendorQA_FinalComments, DbType.String);
                param.Add("@WiproQA_FinalComments", model.WiproQA_FinalComments, DbType.String);
                param.Add("@VendorQA_Signature", model.VendorQA_Signature, DbType.String);
                param.Add("@WiproQA_Signature", model.WiproQA_Signature, DbType.String);
                param.Add("@ReportAttached", model.ReportAttached, DbType.String);

                // ✅ Update audit fields
                param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);

                // Pass null => SP will set GETDATE()
                param.Add("@UpdatedOn", model.UpdatedOn, DbType.DateTime);

                // ✅ TVP param (type name must match SQL)
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.TVP_ValidationClosureDetail"));

                // SP returns: SELECT @Id AS ValidClose_Id;
                var updatedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Update_ValidationClosure_Update",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!updatedId.HasValue || updatedId.Value <= 0)
                    throw new Exception("Update failed. No ValidClose_Id returned from stored procedure.");

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

        public async Task<OperationResult> DeleteValidationClosureAsync(int id)
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
                    "dbo.sp_Delete_ValidationClosure",
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

                IQueryable<int> query = _dbContext.ValidationClosureReports
                    .Where(x => x.Deleted == false && x.ReportNo == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.ValidationClosureReports
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
