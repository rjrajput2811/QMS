using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.DropTestRepository;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.GeneralObservationRepository
{
    public class GeneralObservationRepository : SqlTableRepository, IGeneralObservationRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;
        private readonly string _connStr;
        public GeneralObservationRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<GeneralObservationViewModel>> GetGeneralObservationAsync(DateTime? startDate = null, DateTime? endDate = null)
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

                var sql = @"EXEC sp_Get_GeneralObservationReport";

                var result = await Task.Run(() => _dbContext.GeneralObservationReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new GeneralObservationViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        CheckedBy = x.CheckedBy,
                        AddedOn = x.AddedOn,
                        AddedBy = x.AddedBy,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedOn = x.UpdatedOn,
                        VerifiedBy = x.VerifiedBy
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

        public async Task<GeneralObservationViewModel?> GetGeneralObservationByIdAsync(int id)
        {
            await using var con = _dbContext.Database.GetDbConnection();

            var param = new DynamicParameters();
            param.Add("@GenObsId", id, DbType.Int32);

            await con.OpenAsync();

            using var multi = await con.QueryMultipleAsync(
                "dbo.sp_Get_GeneralObservation_ById",
                param,
                commandType: CommandType.StoredProcedure
            );

            var parent = await multi.ReadFirstOrDefaultAsync<GeneralObservationViewModel>();
            if (parent == null) return null;

            var details = (await multi.ReadAsync<GeneralObservationDetailViewModel>()).AsList();
            parent.Details = details;

            return parent;
        }

        public async Task<OperationResult> InsertGeneralObservationAsync(GeneralObservationReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                var dtDetails = BuildGeneralObsDetailsTvp(model.Details ?? new List<GeneralObservationReportDetail>());
                var con = _dbContext.Database.GetDbConnection();
                await con.OpenAsync();

                var param = new DynamicParameters();

                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@ReportDate", model.ReportDate?.Date, DbType.Date);
                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@CheckedBy", model.CheckedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);
                param.Add("@AddedBy", model.AddedBy, DbType.Int32);

                param.Add("@DetailList", dtDetails.AsTableValuedParameter("dbo.GenObservationDetail_TVP"));

                var insertedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Insert_GeneralObservation",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!insertedId.HasValue || insertedId.Value <= 0)
                    throw new Exception("Insert failed. No GenObs_Id returned from stored procedure.");

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
        private static DataTable BuildGeneralObsDetailsTvp(List<GeneralObservationReportDetail> rows)
        {
            var dt = new DataTable();

            // MUST match SQL TVP definition exactly
            dt.Columns.Add("Req_Spec", typeof(string));
            dt.Columns.Add("Actual_find", typeof(string));
            dt.Columns.Add("Open_Close", typeof(string));
            dt.Columns.Add("Closure_Respons", typeof(string));
            dt.Columns.Add("Attachment", typeof(string));

            if (rows == null || rows.Count == 0) return dt;

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                dr["Req_Spec"] = (object?)r?.Req_Spec ?? DBNull.Value;
                dr["Actual_find"] = (object?)r?.Actual_find ?? DBNull.Value;
                dr["Open_Close"] = (object?)r?.Open_Close ?? DBNull.Value;
                dr["Closure_Respons"] = (object?)r?.Closure_Respons ?? DBNull.Value;
                dr["Attachment"] = (object?)r?.Attachment ?? DBNull.Value;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public async Task<OperationResult> UpdateGeneralObservationAsync(GeneralObservationReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (model.Id <= 0) throw new Exception("Invalid Id.");
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                // Build TVP (null safe)
                var dtDetails = BuildGeneralObsDetailsTvp(model.Details ?? new List<GeneralObservationReportDetail>());

                await using var con = _dbContext.Database.GetDbConnection();
                await con.OpenAsync();

                var param = new DynamicParameters();

                // Required parent Id
                param.Add("@GenObsId", model.Id, DbType.Int32);

                // Parent params (must match your SP params)
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@ReportDate", model.ReportDate?.Date, DbType.Date);

                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);

                param.Add("@CheckedBy", model.CheckedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);

                // Nullable updated fields
                param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);

                // TVP param
                param.Add("@DetailList", dtDetails.AsTableValuedParameter("dbo.GenObservationDetail_TVP"));

                // SP should return: SELECT @Id AS GenObs_Id;
                var updatedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Update_GeneralObservation",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!updatedId.HasValue || updatedId.Value <= 0)
                    throw new Exception("Update failed. No GenObs_Id returned from stored procedure.");

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

        public async Task<OperationResult> DeleteGeneralObservationAsync(int id)
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
                    "dbo.sp_Delete_GeneralObservation",
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

                IQueryable<int> query = _dbContext.GeneralObservationReports
                    .Where(x => x.Deleted == false && x.ReportNo == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.GeneralObservationReports
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
