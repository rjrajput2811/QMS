using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;
namespace QMS.Core.Repositories.PhotometryRepository
{
    public class PhotometryTestRepository : SqlTableRepository, IPhotometryTestRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public PhotometryTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<OperationResult> InsertPhotometryTestReportAsync(PhotometryTestReportViewModel model)
        {
            try
            {
                var parameters = new[]
                    {
                        new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                        new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                        new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                        new SqlParameter("@DriverDetails", model.DriverDetails ?? (object)DBNull.Value),
                        new SqlParameter("@LedDetails", model.LedDetails ?? (object)DBNull.Value),
                        new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_InputWattage_Spec", model.Sphere_InputWattage_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample1", model.Sphere_InputWattage_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample2", model.Sphere_InputWattage_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample3", model.Sphere_InputWattage_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample4", model.Sphere_InputWattage_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample5", model.Sphere_InputWattage_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Result", model.Sphere_InputWattage_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_LuminousFlux_Spec", model.Sphere_LuminousFlux_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample1", model.Sphere_LuminousFlux_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample2", model.Sphere_LuminousFlux_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample3", model.Sphere_LuminousFlux_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample4", model.Sphere_LuminousFlux_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample5", model.Sphere_LuminousFlux_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Result", model.Sphere_LuminousFlux_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_CCT_Spec", model.Sphere_CCT_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample1", model.Sphere_CCT_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample2", model.Sphere_CCT_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample3", model.Sphere_CCT_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample4", model.Sphere_CCT_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample5", model.Sphere_CCT_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Result", model.Sphere_CCT_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_CRI_Spec", model.Sphere_CRI_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample1", model.Sphere_CRI_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample2", model.Sphere_CRI_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample3", model.Sphere_CRI_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample4", model.Sphere_CRI_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample5", model.Sphere_CRI_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Result", model.Sphere_CRI_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_SystemEfficacy_Spec", model.Sphere_SystemEfficacy_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample1", model.Sphere_SystemEfficacy_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample2", model.Sphere_SystemEfficacy_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample3", model.Sphere_SystemEfficacy_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample4", model.Sphere_SystemEfficacy_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample5", model.Sphere_SystemEfficacy_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Result", model.Sphere_SystemEfficacy_Result ?? (object)DBNull.Value),

                         // Gonio
                        new SqlParameter("@Gonio_InputWattage_Spec", model.Gonio_InputWattage_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample1", model.Gonio_InputWattage_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample2", model.Gonio_InputWattage_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample3", model.Gonio_InputWattage_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample4", model.Gonio_InputWattage_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample5", model.Gonio_InputWattage_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Result", model.Gonio_InputWattage_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_LuminousFlux_Spec", model.Gonio_LuminousFlux_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample1", model.Gonio_LuminousFlux_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample2", model.Gonio_LuminousFlux_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample3", model.Gonio_LuminousFlux_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample4", model.Gonio_LuminousFlux_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample5", model.Gonio_LuminousFlux_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Result", model.Gonio_LuminousFlux_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_CCT_Spec", model.Gonio_CCT_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample1", model.Gonio_CCT_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample2", model.Gonio_CCT_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample3", model.Gonio_CCT_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample4", model.Gonio_CCT_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample5", model.Gonio_CCT_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Result", model.Gonio_CCT_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_CRI_Spec", model.Gonio_CRI_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample1", model.Gonio_CRI_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample2", model.Gonio_CRI_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample3", model.Gonio_CRI_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample4", model.Gonio_CRI_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample5", model.Gonio_CRI_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Result", model.Gonio_CRI_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_SystemEfficacy_Spec", model.Gonio_SystemEfficacy_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample1", model.Gonio_SystemEfficacy_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample2", model.Gonio_SystemEfficacy_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample3", model.Gonio_SystemEfficacy_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample4", model.Gonio_SystemEfficacy_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample5", model.Gonio_SystemEfficacy_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Result", model.Gonio_SystemEfficacy_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_UGR_Spec", model.Gonio_UGR_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample1", model.Gonio_UGR_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample2", model.Gonio_UGR_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample3", model.Gonio_UGR_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample4", model.Gonio_UGR_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample5", model.Gonio_UGR_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Result", model.Gonio_UGR_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_Distribution_Spec", model.Gonio_Distribution_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample1", model.Gonio_Distribution_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample2", model.Gonio_Distribution_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample3", model.Gonio_Distribution_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample4", model.Gonio_Distribution_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample5", model.Gonio_Distribution_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Result", model.Gonio_Distribution_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_NABL79_Spec", model.Gonio_NABL79_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample1", model.Gonio_NABL79_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample2", model.Gonio_NABL79_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample3", model.Gonio_NABL79_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample4", model.Gonio_NABL79_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample5", model.Gonio_NABL79_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Result", model.Gonio_NABL79_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_NABL79_Other_Spec", model.Gonio_NABL79_Other_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample1", model.Gonio_NABL79_Other_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample2", model.Gonio_NABL79_Other_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample3", model.Gonio_NABL79_Other_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample4", model.Gonio_NABL79_Other_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample5", model.Gonio_NABL79_Other_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Result", model.Gonio_NABL79_Other_Result ?? (object)DBNull.Value),


                        new SqlParameter("@TestedBy", model.TestedBy ?? (object)DBNull.Value),
                        new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),
                        new SqlParameter("@AddedBy", model.AddedBy),
                        new SqlParameter("@AddedOn", model.AddedOn)
                    };

                await _dbContext.Database.ExecuteSqlRawAsync(
                @"EXEC sp_Insert_Photometry 
        @ProductCatRef, @ProductDescription, @ReportNo, @DriverDetails, @LedDetails, @ReportDate,
        @Sphere_InputWattage_Spec, @Sphere_InputWattage_Sample1, @Sphere_InputWattage_Sample2, @Sphere_InputWattage_Sample3, @Sphere_InputWattage_Sample4, @Sphere_InputWattage_Sample5, @Sphere_InputWattage_Result,
        @Sphere_LuminousFlux_Spec, @Sphere_LuminousFlux_Sample1, @Sphere_LuminousFlux_Sample2, @Sphere_LuminousFlux_Sample3, @Sphere_LuminousFlux_Sample4, @Sphere_LuminousFlux_Sample5, @Sphere_LuminousFlux_Result,
        @Sphere_CCT_Spec, @Sphere_CCT_Sample1, @Sphere_CCT_Sample2, @Sphere_CCT_Sample3, @Sphere_CCT_Sample4, @Sphere_CCT_Sample5, @Sphere_CCT_Result,
        @Sphere_CRI_Spec, @Sphere_CRI_Sample1, @Sphere_CRI_Sample2, @Sphere_CRI_Sample3, @Sphere_CRI_Sample4, @Sphere_CRI_Sample5, @Sphere_CRI_Result,
        @Sphere_SystemEfficacy_Spec, @Sphere_SystemEfficacy_Sample1, @Sphere_SystemEfficacy_Sample2, @Sphere_SystemEfficacy_Sample3, @Sphere_SystemEfficacy_Sample4, @Sphere_SystemEfficacy_Sample5, @Sphere_SystemEfficacy_Result,
        @Gonio_InputWattage_Spec, @Gonio_InputWattage_Sample1, @Gonio_InputWattage_Sample2, @Gonio_InputWattage_Sample3, @Gonio_InputWattage_Sample4, @Gonio_InputWattage_Sample5, @Gonio_InputWattage_Result,
        @Gonio_LuminousFlux_Spec, @Gonio_LuminousFlux_Sample1, @Gonio_LuminousFlux_Sample2, @Gonio_LuminousFlux_Sample3, @Gonio_LuminousFlux_Sample4, @Gonio_LuminousFlux_Sample5, @Gonio_LuminousFlux_Result,
        @Gonio_CCT_Spec, @Gonio_CCT_Sample1, @Gonio_CCT_Sample2, @Gonio_CCT_Sample3, @Gonio_CCT_Sample4, @Gonio_CCT_Sample5, @Gonio_CCT_Result,
        @Gonio_CRI_Spec, @Gonio_CRI_Sample1, @Gonio_CRI_Sample2, @Gonio_CRI_Sample3, @Gonio_CRI_Sample4, @Gonio_CRI_Sample5, @Gonio_CRI_Result,
        @Gonio_SystemEfficacy_Spec, @Gonio_SystemEfficacy_Sample1, @Gonio_SystemEfficacy_Sample2, @Gonio_SystemEfficacy_Sample3, @Gonio_SystemEfficacy_Sample4, @Gonio_SystemEfficacy_Sample5, @Gonio_SystemEfficacy_Result,
        @Gonio_UGR_Spec, @Gonio_UGR_Sample1, @Gonio_UGR_Sample2, @Gonio_UGR_Sample3, @Gonio_UGR_Sample4, @Gonio_UGR_Sample5, @Gonio_UGR_Result,
        @Gonio_Distribution_Spec, @Gonio_Distribution_Sample1, @Gonio_Distribution_Sample2, @Gonio_Distribution_Sample3, @Gonio_Distribution_Sample4, @Gonio_Distribution_Sample5, @Gonio_Distribution_Result,
        @Gonio_NABL79_Spec, @Gonio_NABL79_Sample1, @Gonio_NABL79_Sample2, @Gonio_NABL79_Sample3, @Gonio_NABL79_Sample4, @Gonio_NABL79_Sample5, @Gonio_NABL79_Result,
        @Gonio_NABL79_Other_Spec, @Gonio_NABL79_Other_Sample1, @Gonio_NABL79_Other_Sample2, @Gonio_NABL79_Other_Sample3, @Gonio_NABL79_Other_Sample4, @Gonio_NABL79_Other_Sample5, @Gonio_NABL79_Other_Result,
        @TestedBy, @VerifiedBy, @AddedBy, @AddedOn",
             parameters);

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdatePhotometryTestReportAsync(PhotometryTestReportViewModel model)
        {
            try
            {
                var parameters = new[]
                    {
                        new SqlParameter("@Id", model.Id),
                        new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                        new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                        new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                        new SqlParameter("@DriverDetails", model.DriverDetails ?? (object)DBNull.Value),
                        new SqlParameter("@LedDetails", model.LedDetails ?? (object)DBNull.Value),
                        new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_InputWattage_Spec", model.Sphere_InputWattage_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample1", model.Sphere_InputWattage_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample2", model.Sphere_InputWattage_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample3", model.Sphere_InputWattage_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample4", model.Sphere_InputWattage_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Sample5", model.Sphere_InputWattage_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_InputWattage_Result", model.Sphere_InputWattage_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_LuminousFlux_Spec", model.Sphere_LuminousFlux_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample1", model.Sphere_LuminousFlux_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample2", model.Sphere_LuminousFlux_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample3", model.Sphere_LuminousFlux_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample4", model.Sphere_LuminousFlux_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Sample5", model.Sphere_LuminousFlux_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_LuminousFlux_Result", model.Sphere_LuminousFlux_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_CCT_Spec", model.Sphere_CCT_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample1", model.Sphere_CCT_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample2", model.Sphere_CCT_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample3", model.Sphere_CCT_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample4", model.Sphere_CCT_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Sample5", model.Sphere_CCT_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CCT_Result", model.Sphere_CCT_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_CRI_Spec", model.Sphere_CRI_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample1", model.Sphere_CRI_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample2", model.Sphere_CRI_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample3", model.Sphere_CRI_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample4", model.Sphere_CRI_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Sample5", model.Sphere_CRI_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_CRI_Result", model.Sphere_CRI_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Sphere_SystemEfficacy_Spec", model.Sphere_SystemEfficacy_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample1", model.Sphere_SystemEfficacy_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample2", model.Sphere_SystemEfficacy_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample3", model.Sphere_SystemEfficacy_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample4", model.Sphere_SystemEfficacy_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Sample5", model.Sphere_SystemEfficacy_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Sphere_SystemEfficacy_Result", model.Sphere_SystemEfficacy_Result ?? (object)DBNull.Value),

                         // Gonio
                        new SqlParameter("@Gonio_InputWattage_Spec", model.Gonio_InputWattage_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample1", model.Gonio_InputWattage_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample2", model.Gonio_InputWattage_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample3", model.Gonio_InputWattage_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample4", model.Gonio_InputWattage_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Sample5", model.Gonio_InputWattage_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_InputWattage_Result", model.Gonio_InputWattage_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_LuminousFlux_Spec", model.Gonio_LuminousFlux_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample1", model.Gonio_LuminousFlux_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample2", model.Gonio_LuminousFlux_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample3", model.Gonio_LuminousFlux_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample4", model.Gonio_LuminousFlux_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Sample5", model.Gonio_LuminousFlux_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_LuminousFlux_Result", model.Gonio_LuminousFlux_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_CCT_Spec", model.Gonio_CCT_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample1", model.Gonio_CCT_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample2", model.Gonio_CCT_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample3", model.Gonio_CCT_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample4", model.Gonio_CCT_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Sample5", model.Gonio_CCT_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CCT_Result", model.Gonio_CCT_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_CRI_Spec", model.Gonio_CRI_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample1", model.Gonio_CRI_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample2", model.Gonio_CRI_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample3", model.Gonio_CRI_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample4", model.Gonio_CRI_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Sample5", model.Gonio_CRI_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_CRI_Result", model.Gonio_CRI_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_SystemEfficacy_Spec", model.Gonio_SystemEfficacy_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample1", model.Gonio_SystemEfficacy_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample2", model.Gonio_SystemEfficacy_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample3", model.Gonio_SystemEfficacy_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample4", model.Gonio_SystemEfficacy_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Sample5", model.Gonio_SystemEfficacy_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_SystemEfficacy_Result", model.Gonio_SystemEfficacy_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_UGR_Spec", model.Gonio_UGR_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample1", model.Gonio_UGR_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample2", model.Gonio_UGR_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample3", model.Gonio_UGR_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample4", model.Gonio_UGR_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Sample5", model.Gonio_UGR_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_UGR_Result", model.Gonio_UGR_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_Distribution_Spec", model.Gonio_Distribution_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample1", model.Gonio_Distribution_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample2", model.Gonio_Distribution_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample3", model.Gonio_Distribution_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample4", model.Gonio_Distribution_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Sample5", model.Gonio_Distribution_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_Distribution_Result", model.Gonio_Distribution_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_NABL79_Spec", model.Gonio_NABL79_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample1", model.Gonio_NABL79_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample2", model.Gonio_NABL79_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample3", model.Gonio_NABL79_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample4", model.Gonio_NABL79_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Sample5", model.Gonio_NABL79_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Result", model.Gonio_NABL79_Result ?? (object)DBNull.Value),

                        new SqlParameter("@Gonio_NABL79_Other_Spec", model.Gonio_NABL79_Other_Spec ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample1", model.Gonio_NABL79_Other_Sample1 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample2", model.Gonio_NABL79_Other_Sample2 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample3", model.Gonio_NABL79_Other_Sample3 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample4", model.Gonio_NABL79_Other_Sample4 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Sample5", model.Gonio_NABL79_Other_Sample5 ?? (object)DBNull.Value),
                        new SqlParameter("@Gonio_NABL79_Other_Result", model.Gonio_NABL79_Other_Result ?? (object)DBNull.Value),

                        new SqlParameter("@TestedBy", model.TestedBy ?? (object)DBNull.Value),
                        new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value),
                        new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                        new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
                    };

                await _dbContext.Database.ExecuteSqlRawAsync(
               "EXEC sp_Update_Photometry " + "@Id, " + "@ProductCatRef, " + "@ProductDescription, " + "@ReportNo, " +
               "@DriverDetails, " + "@LedDetails, " + "@ReportDate, " +

               "@Sphere_InputWattage_Spec, " + "@Sphere_InputWattage_Sample1, " + "@Sphere_InputWattage_Sample2, " +
               "@Sphere_InputWattage_Sample3, " + "@Sphere_InputWattage_Sample4, " + "@Sphere_InputWattage_Sample5, " +
               "@Sphere_InputWattage_Result, " +

               "@Sphere_LuminousFlux_Spec, " + "@Sphere_LuminousFlux_Sample1, " + "@Sphere_LuminousFlux_Sample2, " +
               "@Sphere_LuminousFlux_Sample3, " + "@Sphere_LuminousFlux_Sample4, " + "@Sphere_LuminousFlux_Sample5, " +
               "@Sphere_LuminousFlux_Result, " +

               "@Sphere_CCT_Spec, " + "@Sphere_CCT_Sample1, " + "@Sphere_CCT_Sample2, " + "@Sphere_CCT_Sample3, " +
               "@Sphere_CCT_Sample4, " + "@Sphere_CCT_Sample5, " + "@Sphere_CCT_Result, " +

               "@Sphere_CRI_Spec, " + "@Sphere_CRI_Sample1, " + "@Sphere_CRI_Sample2, " +
               "@Sphere_CRI_Sample3, " + "@Sphere_CRI_Sample4, " + "@Sphere_CRI_Sample5, " + "@Sphere_CRI_Result, " +

               "@Sphere_SystemEfficacy_Spec, " + "@Sphere_SystemEfficacy_Sample1, " + "@Sphere_SystemEfficacy_Sample2, " +
               "@Sphere_SystemEfficacy_Sample3, " + "@Sphere_SystemEfficacy_Sample4, " + "@Sphere_SystemEfficacy_Sample5, " +
               "@Sphere_SystemEfficacy_Result, " +

                /* -------- Gonio -------- */
                "@Gonio_InputWattage_Spec, " + "@Gonio_InputWattage_Sample1, " + "@Gonio_InputWattage_Sample2, " +
                "@Gonio_InputWattage_Sample3, " + "@Gonio_InputWattage_Sample4, " + "@Gonio_InputWattage_Sample5, " +
                "@Gonio_InputWattage_Result, " +

                "@Gonio_LuminousFlux_Spec, " + "@Gonio_LuminousFlux_Sample1, " + "@Gonio_LuminousFlux_Sample2, " +
                "@Gonio_LuminousFlux_Sample3, " + "@Gonio_LuminousFlux_Sample4, " + "@Gonio_LuminousFlux_Sample5, " +
                "@Gonio_LuminousFlux_Result, " +

                "@Gonio_CCT_Spec, " + "@Gonio_CCT_Sample1, " + "@Gonio_CCT_Sample2, " + "@Gonio_CCT_Sample3, " +
                "@Gonio_CCT_Sample4, " + "@Gonio_CCT_Sample5, " + "@Gonio_CCT_Result, " +

                "@Gonio_CRI_Spec, " + "@Gonio_CRI_Sample1, " + "@Gonio_CRI_Sample2, " +
                "@Gonio_CRI_Sample3, " + "@Gonio_CRI_Sample4, " + "@Gonio_CRI_Sample5, " + "@Gonio_CRI_Result, " +

                "@Gonio_SystemEfficacy_Spec, " + "@Gonio_SystemEfficacy_Sample1, " + "@Gonio_SystemEfficacy_Sample2, " +
                "@Gonio_SystemEfficacy_Sample3, " + "@Gonio_SystemEfficacy_Sample4, " + "@Gonio_SystemEfficacy_Sample5, " +
                "@Gonio_SystemEfficacy_Result, " +

                "@Gonio_UGR_Spec, " + "@Gonio_UGR_Sample1, " + "@Gonio_UGR_Sample2, " + "@Gonio_UGR_Sample3, " +
                "@Gonio_UGR_Sample4, " + "@Gonio_UGR_Sample5, " + "@Gonio_UGR_Result, " +

                "@Gonio_Distribution_Spec, " + "@Gonio_Distribution_Sample1, " + "@Gonio_Distribution_Sample2, " +
                "@Gonio_Distribution_Sample3, " + "@Gonio_Distribution_Sample4, " + "@Gonio_Distribution_Sample5, " +
                "@Gonio_Distribution_Result, " +

                "@Gonio_NABL79_Spec, " + "@Gonio_NABL79_Sample1, " + "@Gonio_NABL79_Sample2, " + "@Gonio_NABL79_Sample3, " +
                "@Gonio_NABL79_Sample4, " + "@Gonio_NABL79_Sample5, " + "@Gonio_NABL79_Result, " +

                "@Gonio_NABL79_Other_Spec, " + "@Gonio_NABL79_Other_Sample1, " +
                "@Gonio_NABL79_Other_Sample2, " + "@Gonio_NABL79_Other_Sample3, " + "@Gonio_NABL79_Other_Sample4, " +
                "@Gonio_NABL79_Other_Sample5, " + "@Gonio_NABL79_Other_Result, " +

               "@TestedBy, " + "@VerifiedBy, " + "@UpdatedBy, " + "@UpdatedOn",
               parameters
           );

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<List<PhotometryTestReportViewModel>> GetPhotometryTestReportAsync(DateTime? startDate = null, DateTime? endDate = null)
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

                var sql = @"EXEC sp_Get_Photometry";

                var result = await Task.Run(() => _dbContext.Photometries.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new PhotometryTestReportViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        DriverDetails = x.DriverDetails,
                        LedDetails = x.LedDetails,
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

        public async Task<OperationResult> DeletePhotometryTestAsync(int Id)
        {
            try
            {
                var result = await base.DeleteAsync<Photometry>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<PhotometryTestReportViewModel> GetPhotometryTestReportByIdAsync(int Id)
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Id", Id),
            };

                var sql = @"EXEC sp_Get_Photometry @Id";

                var result = await Task.Run(() => _dbContext.Photometries.FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new PhotometryTestReportViewModel
                    {
                        Id = x.Id,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        DriverDetails = x.DriverDetails,
                        LedDetails = x.LedDetails,

                        Sphere_InputWattage_Spec=x.Sphere_InputWattage_Spec,
                        Sphere_InputWattage_Sample1=x.Sphere_InputWattage_Sample1,
                        Sphere_InputWattage_Sample2 =x.Sphere_InputWattage_Sample2,
                        Sphere_InputWattage_Sample3 =x.Sphere_InputWattage_Sample3,
                        Sphere_InputWattage_Sample4 =x.Sphere_InputWattage_Sample4,
                        Sphere_InputWattage_Sample5 = x.Sphere_InputWattage_Sample5,
                        Sphere_InputWattage_Result = x.Sphere_InputWattage_Result,

                        Sphere_LuminousFlux_Spec = x.Sphere_LuminousFlux_Spec,
                        Sphere_LuminousFlux_Sample1 = x.Sphere_LuminousFlux_Sample1,
                        Sphere_LuminousFlux_Sample2 = x.Sphere_LuminousFlux_Sample2,
                        Sphere_LuminousFlux_Sample3 = x.Sphere_LuminousFlux_Sample3,
                        Sphere_LuminousFlux_Sample4 = x.Sphere_LuminousFlux_Sample4,
                        Sphere_LuminousFlux_Sample5 = x.Sphere_LuminousFlux_Sample5,
                        Sphere_LuminousFlux_Result = x.Sphere_LuminousFlux_Result,

                        Sphere_CCT_Spec = x.Sphere_CCT_Spec,
                        Sphere_CCT_Sample1 = x.Sphere_CCT_Sample1,
                        Sphere_CCT_Sample2 = x.Sphere_CCT_Sample2,
                        Sphere_CCT_Sample3 = x.Sphere_CCT_Sample3,
                        Sphere_CCT_Sample4 = x.Sphere_CCT_Sample4,
                        Sphere_CCT_Sample5 = x.Sphere_CCT_Sample5,
                        Sphere_CCT_Result = x.Sphere_CCT_Result,

                        Sphere_CRI_Spec = x.Sphere_CRI_Spec,
                        Sphere_CRI_Sample1 = x.Sphere_CRI_Sample1,
                        Sphere_CRI_Sample2 = x.Sphere_CRI_Sample2,
                        Sphere_CRI_Sample3 = x.Sphere_CRI_Sample3,
                        Sphere_CRI_Sample4 = x.Sphere_CRI_Sample4,
                        Sphere_CRI_Sample5 = x.Sphere_CRI_Sample5,
                        Sphere_CRI_Result = x.Sphere_CRI_Result,

                        Sphere_SystemEfficacy_Spec = x.Sphere_SystemEfficacy_Spec,
                        Sphere_SystemEfficacy_Sample1 = x.Sphere_SystemEfficacy_Sample1,
                        Sphere_SystemEfficacy_Sample2 = x.Sphere_SystemEfficacy_Sample2,
                        Sphere_SystemEfficacy_Sample3 = x.Sphere_SystemEfficacy_Sample3,
                        Sphere_SystemEfficacy_Sample4 = x.Sphere_SystemEfficacy_Sample4,
                        Sphere_SystemEfficacy_Sample5 = x.Sphere_SystemEfficacy_Sample5,
                        Sphere_SystemEfficacy_Result = x.Sphere_SystemEfficacy_Result,

                        Gonio_InputWattage_Spec = x.Gonio_InputWattage_Spec,
                        Gonio_InputWattage_Sample1 = x.Gonio_InputWattage_Sample1,
                        Gonio_InputWattage_Sample2 = x.Gonio_InputWattage_Sample2,
                        Gonio_InputWattage_Sample3 = x.Gonio_InputWattage_Sample3,
                        Gonio_InputWattage_Sample4 = x.Gonio_InputWattage_Sample4,
                        Gonio_InputWattage_Sample5 = x.Gonio_InputWattage_Sample5,
                        Gonio_InputWattage_Result = x.Gonio_InputWattage_Result,

                        Gonio_LuminousFlux_Spec = x.Gonio_LuminousFlux_Spec,
                        Gonio_LuminousFlux_Sample1 = x.Gonio_LuminousFlux_Sample1,
                        Gonio_LuminousFlux_Sample2 = x.Gonio_LuminousFlux_Sample2,
                        Gonio_LuminousFlux_Sample3 = x.Gonio_LuminousFlux_Sample3,
                        Gonio_LuminousFlux_Sample4 = x.Gonio_LuminousFlux_Sample4,
                        Gonio_LuminousFlux_Sample5 = x.Gonio_LuminousFlux_Sample5,
                        Gonio_LuminousFlux_Result = x.Gonio_LuminousFlux_Result,

                        Gonio_CCT_Spec = x.Gonio_CCT_Spec,
                        Gonio_CCT_Sample1 = x.Gonio_CCT_Sample1,
                        Gonio_CCT_Sample2 = x.Gonio_CCT_Sample2,
                        Gonio_CCT_Sample3 = x.Gonio_CCT_Sample3,
                        Gonio_CCT_Sample4 = x.Gonio_CCT_Sample4,
                        Gonio_CCT_Sample5 = x.Gonio_CCT_Sample5,
                        Gonio_CCT_Result = x.Gonio_CCT_Result,

                        Gonio_CRI_Spec = x.Gonio_CRI_Spec,
                        Gonio_CRI_Sample1 = x.Gonio_CRI_Sample1,
                        Gonio_CRI_Sample2 = x.Gonio_CRI_Sample2,
                        Gonio_CRI_Sample3 = x.Gonio_CRI_Sample3,
                        Gonio_CRI_Sample4 = x.Gonio_CRI_Sample4,
                        Gonio_CRI_Sample5 = x.Gonio_CRI_Sample5,
                        Gonio_CRI_Result = x.Gonio_CRI_Result,

                        Gonio_SystemEfficacy_Spec = x.Gonio_SystemEfficacy_Spec,
                        Gonio_SystemEfficacy_Sample1 = x.Gonio_SystemEfficacy_Sample1,
                        Gonio_SystemEfficacy_Sample2 = x.Gonio_SystemEfficacy_Sample2,
                        Gonio_SystemEfficacy_Sample3 = x.Gonio_SystemEfficacy_Sample3,
                        Gonio_SystemEfficacy_Sample4 = x.Gonio_SystemEfficacy_Sample4,
                        Gonio_SystemEfficacy_Sample5 = x.Gonio_SystemEfficacy_Sample5,
                        Gonio_SystemEfficacy_Result = x.Gonio_SystemEfficacy_Result,

                        Gonio_UGR_Spec = x.Gonio_UGR_Spec,
                        Gonio_UGR_Sample1 = x.Gonio_UGR_Sample1,
                        Gonio_UGR_Sample2 = x.Gonio_UGR_Sample2,
                        Gonio_UGR_Sample3 = x.Gonio_UGR_Sample3,
                        Gonio_UGR_Sample4 = x.Gonio_UGR_Sample4,
                        Gonio_UGR_Sample5 = x.Gonio_UGR_Sample5,
                        Gonio_UGR_Result = x.Gonio_UGR_Result,

                        Gonio_Distribution_Spec = x.Gonio_Distribution_Spec,
                        Gonio_Distribution_Sample1 = x.Gonio_Distribution_Sample1,
                        Gonio_Distribution_Sample2 = x.Gonio_Distribution_Sample2,
                        Gonio_Distribution_Sample3 = x.Gonio_Distribution_Sample3,
                        Gonio_Distribution_Sample4 = x.Gonio_Distribution_Sample4,
                        Gonio_Distribution_Sample5 = x.Gonio_Distribution_Sample5,
                        Gonio_Distribution_Result = x.Gonio_Distribution_Result,

                        Gonio_NABL79_Spec = x.Gonio_NABL79_Spec,
                        Gonio_NABL79_Sample1 = x.Gonio_NABL79_Sample1,
                        Gonio_NABL79_Sample2 = x.Gonio_NABL79_Sample2,
                        Gonio_NABL79_Sample3 = x.Gonio_NABL79_Sample3,
                        Gonio_NABL79_Sample4 = x.Gonio_NABL79_Sample4,
                        Gonio_NABL79_Sample5 = x.Gonio_NABL79_Sample5,
                        Gonio_NABL79_Result = x.Gonio_NABL79_Result,

                        Gonio_NABL79_Other_Spec = x.Gonio_NABL79_Other_Spec,
                        Gonio_NABL79_Other_Sample1 = x.Gonio_NABL79_Other_Sample1,
                        Gonio_NABL79_Other_Sample2 = x.Gonio_NABL79_Other_Sample2,
                        Gonio_NABL79_Other_Sample3 = x.Gonio_NABL79_Other_Sample3,
                        Gonio_NABL79_Other_Sample4 = x.Gonio_NABL79_Other_Sample4,
                        Gonio_NABL79_Other_Sample5 = x.Gonio_NABL79_Other_Sample5,
                        Gonio_NABL79_Other_Result = x.Gonio_NABL79_Other_Result,

                        VerifiedBy =x.VerifiedBy,
                        TestedBy=x.TestedBy,                        

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

        public async Task<bool> CheckDuplicate(string searchText, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.Photometries
                    .Where(x => x.Deleted == false && x.ReportNo == searchText)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.Photometries
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
