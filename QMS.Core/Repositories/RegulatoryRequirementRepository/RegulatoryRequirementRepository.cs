using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.RegulatoryRequirementRepository
{
    public class RegulatoryRequirementRepository : SqlTableRepository, IRegulatoryRequirementRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public RegulatoryRequirementRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<OperationResult> InsertRegulatoryRequirementAsync(RegulatoryRequirementViewModel model)
        {
            try
            {
                var parameters = new[]
                    {
                        new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                        new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                        new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                        new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),

                        new SqlParameter("@DriverBIS_Result1", model.DriverBIS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_Result2", model.DriverBIS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_Result3", model.DriverBIS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_Result4", model.DriverBIS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_UploadFile", model.DriverBIS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@LuminairesBIS_Result1", model.LuminairesBIS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_Result2", model.LuminairesBIS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_Result3", model.LuminairesBIS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_Result4", model.LuminairesBIS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_UploadFile", model.LuminairesBIS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CCL_Result1", model.CCL_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_Result2", model.CCL_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_Result3", model.CCL_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_Result4", model.CCL_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_UploadFile", model.CCL_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@NPIBuySheet_Result1", model.NPIBuySheet_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_Result2", model.NPIBuySheet_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_Result3", model.NPIBuySheet_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_Result4", model.NPIBuySheet_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_UploadFile", model.NPIBuySheet_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CPS_Result1", model.CPS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_Result2", model.CPS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_Result3", model.CPS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_Result4", model.CPS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_UploadFile", model.CPS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@PPS_Result1", model.PPS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_Result2", model.PPS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_Result3", model.PPS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_Result4", model.PPS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_UploadFile", model.PPS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@TDS_Result1", model.TDS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_Result2", model.TDS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_Result3", model.TDS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_Result4", model.TDS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_UploadFile", model.TDS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@DesignDocket_Result1", model.DesignDocket_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_Result2", model.DesignDocket_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_Result3", model.DesignDocket_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_Result4", model.DesignDocket_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_UploadFile", model.DesignDocket_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@InstallationSheet_Result1", model.InstallationSheet_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_Result2", model.InstallationSheet_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_Result3", model.InstallationSheet_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_Result4", model.InstallationSheet_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_UploadFile", model.InstallationSheet_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@ROHSCompliance_Result1", model.ROHSCompliance_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_Result2", model.ROHSCompliance_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_Result3", model.ROHSCompliance_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_Result4", model.ROHSCompliance_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_UploadFile", model.ROHSCompliance_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CIMFR_Result1", model.CIMFR_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_Result2", model.CIMFR_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_Result3", model.CIMFR_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_Result4", model.CIMFR_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_UploadFile", model.CIMFR_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@PESO_Result1", model.PESO_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_Result2", model.PESO_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_Result3", model.PESO_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_Result4", model.PESO_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_UploadFile", model.PESO_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@BOM_Result1", model.BOM_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_Result2", model.BOM_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_Result3", model.BOM_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_Result4", model.BOM_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_UploadFile", model.BOM_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@SpareCodeSAP_Result1", model.SpareCodeSAP_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_Result2", model.SpareCodeSAP_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_Result3", model.SpareCodeSAP_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_Result4", model.SpareCodeSAP_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_UploadFile", model.SpareCodeSAP_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CERegistration_Result1", model.CERegistration_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_Result2", model.CERegistration_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_Result3", model.CERegistration_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_Result4", model.CERegistration_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_UploadFile", model.CERegistration_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                        new SqlParameter("@CheckedBy", model.CheckedBy ?? (object)DBNull.Value),
                        new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),

                        new SqlParameter("@AddedBy", model.AddedBy),
                        new SqlParameter("@AddedOn", model.AddedOn),
                    };

                await _dbContext.Database.ExecuteSqlRawAsync(
                @"EXEC sp_Insert_LegalRegulatoryReport 
                @ReportNo, @ReportDate, @ProductCatRef, @ProductDescription,
                @DriverBIS_Result1, @DriverBIS_Result2, @DriverBIS_Result3, @DriverBIS_Result4, @DriverBIS_UploadFile,
                @LuminairesBIS_Result1, @LuminairesBIS_Result2, @LuminairesBIS_Result3, @LuminairesBIS_Result4, @LuminairesBIS_UploadFile,
                @CCL_Result1, @CCL_Result2, @CCL_Result3, @CCL_Result4, @CCL_UploadFile,
                @NPIBuySheet_Result1, @NPIBuySheet_Result2, @NPIBuySheet_Result3, @NPIBuySheet_Result4, @NPIBuySheet_UploadFile,
                @CPS_Result1, @CPS_Result2, @CPS_Result3, @CPS_Result4, @CPS_UploadFile,
                @PPS_Result1, @PPS_Result2, @PPS_Result3, @PPS_Result4, @PPS_UploadFile,
                @TDS_Result1, @TDS_Result2, @TDS_Result3, @TDS_Result4, @TDS_UploadFile,
                @DesignDocket_Result1, @DesignDocket_Result2, @DesignDocket_Result3, @DesignDocket_Result4, @DesignDocket_UploadFile,
                @InstallationSheet_Result1, @InstallationSheet_Result2, @InstallationSheet_Result3, @InstallationSheet_Result4, @InstallationSheet_UploadFile,
                @ROHSCompliance_Result1, @ROHSCompliance_Result2, @ROHSCompliance_Result3, @ROHSCompliance_Result4, @ROHSCompliance_UploadFile,
                @CIMFR_Result1, @CIMFR_Result2, @CIMFR_Result3, @CIMFR_Result4, @CIMFR_UploadFile,
                @PESO_Result1, @PESO_Result2, @PESO_Result3, @PESO_Result4, @PESO_UploadFile,
                @BOM_Result1, @BOM_Result2, @BOM_Result3, @BOM_Result4, @BOM_UploadFile,
                @SpareCodeSAP_Result1, @SpareCodeSAP_Result2, @SpareCodeSAP_Result3, @SpareCodeSAP_Result4, @SpareCodeSAP_UploadFile,
                @CERegistration_Result1, @CERegistration_Result2, @CERegistration_Result3, @CERegistration_Result4, @CERegistration_UploadFile,
                @OverallResult, @CheckedBy, @VerifiedBy,@AddedBy, @AddedOn",
             parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateRegulatoryRequirementAsync(RegulatoryRequirementViewModel model)
        {
            try
            {
                var parameters = new[]
                    {
                        new SqlParameter("@Id", model.Id),
                        new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                        new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                        new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                        new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),

                        new SqlParameter("@DriverBIS_Result1", model.DriverBIS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_Result2", model.DriverBIS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_Result3", model.DriverBIS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_Result4", model.DriverBIS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@DriverBIS_UploadFile", model.DriverBIS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@LuminairesBIS_Result1", model.LuminairesBIS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_Result2", model.LuminairesBIS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_Result3", model.LuminairesBIS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_Result4", model.LuminairesBIS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@LuminairesBIS_UploadFile", model.LuminairesBIS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CCL_Result1", model.CCL_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_Result2", model.CCL_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_Result3", model.CCL_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_Result4", model.CCL_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CCL_UploadFile", model.CCL_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@NPIBuySheet_Result1", model.NPIBuySheet_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_Result2", model.NPIBuySheet_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_Result3", model.NPIBuySheet_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_Result4", model.NPIBuySheet_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@NPIBuySheet_UploadFile", model.NPIBuySheet_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CPS_Result1", model.CPS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_Result2", model.CPS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_Result3", model.CPS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_Result4", model.CPS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CPS_UploadFile", model.CPS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@PPS_Result1", model.PPS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_Result2", model.PPS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_Result3", model.PPS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_Result4", model.PPS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@PPS_UploadFile", model.PPS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@TDS_Result1", model.TDS_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_Result2", model.TDS_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_Result3", model.TDS_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_Result4", model.TDS_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@TDS_UploadFile", model.TDS_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@DesignDocket_Result1", model.DesignDocket_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_Result2", model.DesignDocket_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_Result3", model.DesignDocket_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_Result4", model.DesignDocket_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@DesignDocket_UploadFile", model.DesignDocket_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@InstallationSheet_Result1", model.InstallationSheet_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_Result2", model.InstallationSheet_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_Result3", model.InstallationSheet_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_Result4", model.InstallationSheet_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@InstallationSheet_UploadFile", model.InstallationSheet_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@ROHSCompliance_Result1", model.ROHSCompliance_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_Result2", model.ROHSCompliance_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_Result3", model.ROHSCompliance_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_Result4", model.ROHSCompliance_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@ROHSCompliance_UploadFile", model.ROHSCompliance_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CIMFR_Result1", model.CIMFR_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_Result2", model.CIMFR_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_Result3", model.CIMFR_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_Result4", model.CIMFR_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CIMFR_UploadFile", model.CIMFR_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@PESO_Result1", model.PESO_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_Result2", model.PESO_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_Result3", model.PESO_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_Result4", model.PESO_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@PESO_UploadFile", model.PESO_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@BOM_Result1", model.BOM_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_Result2", model.BOM_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_Result3", model.BOM_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_Result4", model.BOM_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@BOM_UploadFile", model.BOM_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@SpareCodeSAP_Result1", model.SpareCodeSAP_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_Result2", model.SpareCodeSAP_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_Result3", model.SpareCodeSAP_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_Result4", model.SpareCodeSAP_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@SpareCodeSAP_UploadFile", model.SpareCodeSAP_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@CERegistration_Result1", model.CERegistration_Result1 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_Result2", model.CERegistration_Result2 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_Result3", model.CERegistration_Result3 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_Result4", model.CERegistration_Result4 ?? (object)DBNull.Value),
                        new SqlParameter("@CERegistration_UploadFile", model.CERegistration_UploadFile ?? (object)DBNull.Value),

                        new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                        new SqlParameter("@CheckedBy", model.CheckedBy ?? (object)DBNull.Value),
                        new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),

                        new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                        new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
                    };

                await _dbContext.Database.ExecuteSqlRawAsync(
                @"EXEC sp_Update_LegalRegulatoryReport 
                @Id,@ReportNo, @ReportDate, @ProductCatRef, @ProductDescription,
                @DriverBIS_Result1, @DriverBIS_Result2, @DriverBIS_Result3, @DriverBIS_Result4, @DriverBIS_UploadFile,
                @LuminairesBIS_Result1, @LuminairesBIS_Result2, @LuminairesBIS_Result3, @LuminairesBIS_Result4, @LuminairesBIS_UploadFile,
                @CCL_Result1, @CCL_Result2, @CCL_Result3, @CCL_Result4, @CCL_UploadFile,
                @NPIBuySheet_Result1, @NPIBuySheet_Result2, @NPIBuySheet_Result3, @NPIBuySheet_Result4, @NPIBuySheet_UploadFile,
                @CPS_Result1, @CPS_Result2, @CPS_Result3, @CPS_Result4, @CPS_UploadFile,
                @PPS_Result1, @PPS_Result2, @PPS_Result3, @PPS_Result4, @PPS_UploadFile,
                @TDS_Result1, @TDS_Result2, @TDS_Result3, @TDS_Result4, @TDS_UploadFile,
                @DesignDocket_Result1, @DesignDocket_Result2, @DesignDocket_Result3, @DesignDocket_Result4, @DesignDocket_UploadFile,
                @InstallationSheet_Result1, @InstallationSheet_Result2, @InstallationSheet_Result3, @InstallationSheet_Result4, @InstallationSheet_UploadFile,
                @ROHSCompliance_Result1, @ROHSCompliance_Result2, @ROHSCompliance_Result3, @ROHSCompliance_Result4, @ROHSCompliance_UploadFile,
                @CIMFR_Result1, @CIMFR_Result2, @CIMFR_Result3, @CIMFR_Result4, @CIMFR_UploadFile,
                @PESO_Result1, @PESO_Result2, @PESO_Result3, @PESO_Result4, @PESO_UploadFile,
                @BOM_Result1, @BOM_Result2, @BOM_Result3, @BOM_Result4, @BOM_UploadFile,
                @SpareCodeSAP_Result1, @SpareCodeSAP_Result2, @SpareCodeSAP_Result3, @SpareCodeSAP_Result4, @SpareCodeSAP_UploadFile,
                @CERegistration_Result1, @CERegistration_Result2, @CERegistration_Result3, @CERegistration_Result4, @CERegistration_UploadFile,
                @OverallResult, @CheckedBy, @VerifiedBy,@UpdatedBy, @UpdatedOn",
             parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }



        public async Task<List<RegulatoryRequirementViewModel>> GetRegulatoryRequirementAsync(DateTime? startDate = null, DateTime? endDate = null)
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

                var sql = @"EXEC sp_Get_LegalRegulatoryReport";

                var result = await Task.Run(() => _dbContext.LegalRegulatoryReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new RegulatoryRequirementViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        AddedBy = x.AddedBy
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

        public async Task<RegulatoryRequirementViewModel> GetRegulatoryRequirementByIdAsync(int Id)
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Id", Id),
            };

                var sql = @"EXEC sp_Get_LegalRegulatoryReport @Id";

                var result = await Task.Run(() => _dbContext.LegalRegulatoryReports.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new RegulatoryRequirementViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,

                        DriverBIS_Result1 = x.DriverBIS_Result1,
                        DriverBIS_Result2 = x.DriverBIS_Result2,
                        DriverBIS_Result3 = x.DriverBIS_Result3,
                        DriverBIS_Result4 = x.DriverBIS_Result4,
                        DriverBIS_UploadFile = x.DriverBIS_UploadFile,

                        LuminairesBIS_Result1 = x.LuminairesBIS_Result1,
                        LuminairesBIS_Result2 = x.LuminairesBIS_Result2,
                        LuminairesBIS_Result3 = x.LuminairesBIS_Result3,
                        LuminairesBIS_Result4 = x.LuminairesBIS_Result4,
                        LuminairesBIS_UploadFile = x.LuminairesBIS_UploadFile,

                        CCL_Result1 = x.CCL_Result1,
                        CCL_Result2 = x.CCL_Result2,
                        CCL_Result3 = x.CCL_Result3,
                        CCL_Result4 = x.CCL_Result4,
                        CCL_UploadFile = x.LuminairesBIS_UploadFile,

                        NPIBuySheet_Result1 = x.NPIBuySheet_Result1,
                        NPIBuySheet_Result2 = x.NPIBuySheet_Result2,
                        NPIBuySheet_Result3 = x.NPIBuySheet_Result3,
                        NPIBuySheet_Result4 = x.NPIBuySheet_Result4,
                        NPIBuySheet_UploadFile = x.NPIBuySheet_UploadFile,

                        CPS_Result1 = x.CPS_Result1,
                        CPS_Result2 = x.CPS_Result2,
                        CPS_Result3 = x.CPS_Result3,
                        CPS_Result4 = x.CPS_Result4,
                        CPS_UploadFile = x.CPS_UploadFile,

                        PPS_Result1 = x.PPS_Result1,
                        PPS_Result2 = x.PPS_Result2,
                        PPS_Result3 = x.PPS_Result3,
                        PPS_Result4 = x.PPS_Result4,
                        PPS_UploadFile = x.PPS_UploadFile,

                        TDS_Result1 = x.TDS_Result1,
                        TDS_Result2 = x.TDS_Result2,
                        TDS_Result3 = x.TDS_Result3,
                        TDS_Result4 = x.TDS_Result4,
                        TDS_UploadFile = x.TDS_UploadFile,

                        DesignDocket_Result1 = x.DesignDocket_Result1,
                        DesignDocket_Result2 = x.DesignDocket_Result2,
                        DesignDocket_Result3 = x.DesignDocket_Result3,
                        DesignDocket_Result4 = x.DesignDocket_Result4,
                        DesignDocket_UploadFile = x.DesignDocket_UploadFile,

                        InstallationSheet_Result1 = x.InstallationSheet_Result1,
                        InstallationSheet_Result2 = x.InstallationSheet_Result2,
                        InstallationSheet_Result3 = x.InstallationSheet_Result3,
                        InstallationSheet_Result4 = x.InstallationSheet_Result4,
                        InstallationSheet_UploadFile = x.InstallationSheet_UploadFile,

                        ROHSCompliance_Result1 = x.ROHSCompliance_Result1,
                        ROHSCompliance_Result2 = x.ROHSCompliance_Result2,
                        ROHSCompliance_Result3 = x.ROHSCompliance_Result3,
                        ROHSCompliance_Result4 = x.ROHSCompliance_Result4,
                        ROHSCompliance_UploadFile = x.ROHSCompliance_UploadFile,

                        CIMFR_Result1 = x.CIMFR_Result1,
                        CIMFR_Result2 = x.CIMFR_Result2,
                        CIMFR_Result3 = x.CIMFR_Result3,
                        CIMFR_Result4 = x.CIMFR_Result4,
                        CIMFR_UploadFile = x.CIMFR_UploadFile,

                        PESO_Result1 = x.PESO_Result1,
                        PESO_Result2 = x.PESO_Result2,
                        PESO_Result3 = x.PESO_Result3,
                        PESO_Result4 = x.PESO_Result4,
                        PESO_UploadFile = x.PESO_UploadFile,

                        BOM_Result1 = x.BOM_Result1,
                        BOM_Result2 = x.BOM_Result2,
                        BOM_Result3 = x.BOM_Result3,
                        BOM_Result4 = x.BOM_Result4,
                        BOM_UploadFile = x.BOM_UploadFile,

                        SpareCodeSAP_Result1 = x.SpareCodeSAP_Result1,
                        SpareCodeSAP_Result2 = x.SpareCodeSAP_Result2,
                        SpareCodeSAP_Result3 = x.SpareCodeSAP_Result3,
                        SpareCodeSAP_Result4 = x.SpareCodeSAP_Result4,
                        SpareCodeSAP_UploadFile = x.SpareCodeSAP_UploadFile,

                        CERegistration_Result1 = x.CERegistration_Result1,
                        CERegistration_Result2 = x.CERegistration_Result2,
                        CERegistration_Result3 = x.CERegistration_Result3,
                        CERegistration_Result4 = x.CERegistration_Result4,
                        CERegistration_UploadFile = x.CERegistration_UploadFile,

                        OverallResult = x.OverallResult,

                        VerifiedBy = x.VerifiedBy,
                        CheckedBy = x.CheckedBy,

                    })
                .FirstOrDefault());

                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }


        }

        public async Task<OperationResult> DeleteRegulatoryRequirementAsync(int Id)
        {
            try
            {
                var result = await base.DeleteAsync<LegalRegulatoryReport>(Id);
                return result;
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

                IQueryable<int> query = _dbContext.LegalRegulatoryReports
                    .Where(x => x.Deleted == false && x.ReportNo == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.LegalRegulatoryReports
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
