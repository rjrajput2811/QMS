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

namespace QMS.Core.Repositories.DropTestRepository
{
    public class DropTestRepository : SqlTableRepository, IDropTestRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;
        private readonly string _connStr;
        public DropTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
            _connStr = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<DropTestViewModel>> GetDropTestAsync()
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

                var sql = @"EXEC sp_Get_DropTestReport";

                var result = await Task.Run(() => _dbContext.DropTestReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new DropTestViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        CaseLot = x.CaseLot,
                        PackingBox_MasterCarton_Dimension = x.PackingBox_MasterCarton_Dimension,
                        PackingBox_InnerCarton_Dimension = x.PackingBox_InnerCarton_Dimension,
                        InnerPaddingDimension = x.InnerPaddingDimension,
                        GrossWeight_Kg = x.GrossWeight_Kg,
                        HeightForTest_IS9000 = x.HeightForTest_IS9000,
                        Glow_Test = x.Glow_Test,
                        OverallResult = x.OverallResult,
                        TestedBy = x.TestedBy,
                        VerifiedBy = x.VerifiedBy,
                        AddedBy = x.AddedBy
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

        public async Task<DropTestViewModel?> GetDropTestByIdAsync(int id)
        {
            await using var con = new SqlConnection(_connStr);

            var param = new DynamicParameters();
            param.Add("@Id", id, DbType.Int32);

            await con.OpenAsync();

            using var multi = await con.QueryMultipleAsync(
                "dbo.sp_Get_DropTestReport_ById",
                param,
                commandType: CommandType.StoredProcedure
            );

            // 1) Parent (single row)
            var parent = await multi.ReadFirstOrDefaultAsync<DropTestViewModel>();
            if (parent == null) return null;

            // 2) Details (list)
            var details = (await multi.ReadAsync<DropTestReportDetailViewModel>()).AsList();

            // 3) ImageDetails (list)
            var images = (await multi.ReadAsync<DropTestReportImgDetailViewModel>()).AsList();

            parent.Details = details;
            parent.ImgDetails = images;

            return parent;
        }

        public async Task<OperationResult> InsertDropTestAsync(DropTestReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                // 1) Build TVP DataTables (null safe)
                var dtDetails = BuildDetailsTvp(model.Details ?? new List<DropTestReportDetail>());
                var dtImgs = BuildImgsTvp(model.ImgDetails ?? new List<DropTestReportImgDetail>());

                await using var con = new SqlConnection(_connStr);
                await con.OpenAsync();

                var param = new DynamicParameters();

                // Parent params (same names as SP)
                param.Add("@ReportNo", model.ReportNo, DbType.String);

                // IMPORTANT: send DBNull when null
                param.Add("@ReportDate", model.ReportDate?.Date, DbType.Date);

                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@CaseLot", model.CaseLot, DbType.String);

                param.Add("@PackingBox_MasterCarton_Dimension", model.PackingBox_MasterCarton_Dimension, DbType.String);
                param.Add("@PackingBox_InnerCarton_Dimension", model.PackingBox_InnerCarton_Dimension, DbType.String);
                param.Add("@InnerPaddingDimension", model.InnerPaddingDimension, DbType.String);

                param.Add("@GrossWeight_Kg", model.GrossWeight_Kg, DbType.String);
                param.Add("@HeightForTest_IS9000", model.HeightForTest_IS9000, DbType.String);

                param.Add("@OverallResult", model.OverallResult, DbType.String);
                param.Add("@TestedBy", model.TestedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);

                param.Add("@Glow_Test", model.Glow_Test, DbType.String);
                param.Add("@AddedBy", model.AddedBy, DbType.Int32);

                // TVP params
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.DropTestDetails_TVP"));
                param.Add("@ImgDetails", dtImgs.AsTableValuedParameter("dbo.DropTestImgDetails_TVP"));

                // SP returns: SELECT @DropTest_Id AS DropTest_Id;
                var insertedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Insert_DropTestReport",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!insertedId.HasValue || insertedId.Value <= 0)
                    throw new Exception("Insert failed. No DropTest_Id returned from stored procedure.");

                result.Success = true;
                result.ObjectId = insertedId.Value;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message; // keep whatever property you use
                return result;
            }
        }

        private static DataTable BuildDetailsTvp(List<DropTestReportDetail> rows)
        {
            var dt = new DataTable();

            // MUST match TVP definition in SQL
            dt.Columns.Add("Test", typeof(string));
            dt.Columns.Add("Parameter", typeof(string));
            dt.Columns.Add("Acceptance_Criteria", typeof(string));
            dt.Columns.Add("Observations", typeof(string));

            if (rows == null || rows.Count == 0) return dt;

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                dr["Test"] = (object?)r?.Test ?? DBNull.Value;
                dr["Parameter"] = (object?)r?.Parameter ?? DBNull.Value;
                dr["Acceptance_Criteria"] = (object?)r?.Acceptance_Criteria ?? DBNull.Value;
                dr["Observations"] = (object?)r?.Observations ?? DBNull.Value;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static DataTable BuildImgsTvp(List<DropTestReportImgDetail> rows)
        {
            var dt = new DataTable();

            // MUST match TVP definition in SQL
            dt.Columns.Add("Before_Img", typeof(string));
            dt.Columns.Add("After_Img", typeof(string));

            if (rows == null || rows.Count == 0) return dt;

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                dr["Before_Img"] = (object?)r?.Before_Img ?? DBNull.Value;
                dr["After_Img"] = (object?)r?.After_Img ?? DBNull.Value;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public async Task<OperationResult> UpdateDropTestAsync(DropTestReport model)
        {
            var result = new OperationResult();

            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (model.Id <= 0) throw new Exception("Invalid Id.");
                if (string.IsNullOrWhiteSpace(model.ReportNo)) throw new Exception("ReportNo is required.");

                // Build TVP tables (null safe)
                var dtDetails = BuildDetailsTvp(model.Details ?? new List<DropTestReportDetail>());
                var dtImgs = BuildImgsTvp(model.ImgDetails ?? new List<DropTestReportImgDetail>());

                await using var con = new SqlConnection(_connStr);
                await con.OpenAsync();

                var param = new DynamicParameters();

                // Required Parent Id
                param.Add("@Id", model.Id, DbType.Int32);

                // Parent params (same names as SP)
                param.Add("@ReportNo", model.ReportNo, DbType.String);
                param.Add("@ReportDate", model.ReportDate?.Date, DbType.Date);

                param.Add("@ProductCatRef", model.ProductCatRef, DbType.String);
                param.Add("@ProductDescription", model.ProductDescription, DbType.String);
                param.Add("@CaseLot", model.CaseLot, DbType.String);

                param.Add("@PackingBox_MasterCarton_Dimension", model.PackingBox_MasterCarton_Dimension, DbType.String);
                param.Add("@PackingBox_InnerCarton_Dimension", model.PackingBox_InnerCarton_Dimension, DbType.String);
                param.Add("@InnerPaddingDimension", model.InnerPaddingDimension, DbType.String);

                param.Add("@GrossWeight_Kg", model.GrossWeight_Kg, DbType.String);
                param.Add("@HeightForTest_IS9000", model.HeightForTest_IS9000, DbType.String);

                param.Add("@OverallResult", model.OverallResult, DbType.String);
                param.Add("@TestedBy", model.TestedBy, DbType.String);
                param.Add("@VerifiedBy", model.VerifiedBy, DbType.String);

                param.Add("@Glow_Test", model.Glow_Test, DbType.String);
                param.Add("@UpdatedBy", model.UpdatedBy, DbType.Int32);

                // TVP params
                param.Add("@Details", dtDetails.AsTableValuedParameter("dbo.DropTestDetails_TVP"));
                param.Add("@ImgDetails", dtImgs.AsTableValuedParameter("dbo.DropTestImgDetails_TVP"));

                // SP returns: SELECT @Id AS DropTest_Id;
                var updatedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Update_DropTestReport",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 120
                );

                if (!updatedId.HasValue || updatedId.Value <= 0)
                    throw new Exception("Update failed. No DropTest_Id returned from stored procedure.");

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

        public async Task<OperationResult> DeleteDropTestAsync(int id)
        {
            var result = new OperationResult();

            try
            {
                if (id <= 0) throw new Exception("Invalid Id.");

                await using var con = new SqlConnection(_connStr);
                await con.OpenAsync();

                var param = new DynamicParameters();
                param.Add("@Id", id, DbType.Int32);

                var deletedId = await con.ExecuteScalarAsync<int?>(
                    "dbo.sp_Delete_DropTestReport",
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

                IQueryable<int> query = _dbContext.DropTestReports
                    .Where(x => x.Deleted == false && x.ReportNo == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.DropTestReports
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
