using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.ProductValidationRepo;

public class PhysicalCheckAndVisualInspectionRepository : SqlTableRepository, IPhysicalCheckAndVisualInspectionRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public PhysicalCheckAndVisualInspectionRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<PhysicalCheckAndVisualInspectionViewModel>> GetPhysicalCheckAndVisualInspectionsAsync()
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

            var sql = @"EXEC sp_Get_PhysicalCheckAndVisualInspection";

            var result = await Task.Run(() => _dbContext.PhysicalCheckAndVisualInspections.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new PhysicalCheckAndVisualInspectionViewModel
                {
                    Id = x.Id,
                    Report_No = x.Report_No,
                    Report_Date = x.Report_Date,
                    Project_Name = x.Project_Name,
                    Product_Cat_Ref = x.Product_Cat_Ref,
                    Batch_Code = x.Batch_Code,
                    PKD = x.PKD,
                    Quantity = x.Quantity,
                    AddedBy = x.AddedBy
                })
                .ToList());

            foreach(var rec in result)
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

    public async Task<PhysicalCheckAndVisualInspectionViewModel> GetPhysicalCheckAndVisualInspectionsByIdAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_PhysicalCheckAndVisualInspection";

            var result = await Task.Run(() => _dbContext.PhysicalCheckAndVisualInspections.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new PhysicalCheckAndVisualInspectionViewModel
                {
                    Id = x.Id,
                    Report_No = x.Report_No,
                    Project_Name = x.Project_Name,
                    Report_Date = x.Report_Date,
                    Product_Cat_Ref = x.Product_Cat_Ref,
                    Product_Description = x.Product_Description,
                    Batch_Code = x.Batch_Code,
                    PKD = x.PKD,
                    Quantity = x.Quantity,
                    WiproBranding_Sample1 = x.WiproBranding_Sample1,
                    WiproBranding_Sample2 = x.WiproBranding_Sample2,
                    WiproBranding_Sample3 = x.WiproBranding_Sample3,
                    WiproBranding_Sample4 = x.WiproBranding_Sample4,
                    WiproBranding_Sample5 = x.WiproBranding_Sample5,
                    WiproBranding_Result = x.WiproBranding_Result,
                    ProductDriverLabels_Sample1 = x.ProductDriverLabels_Sample1,
                    ProductDriverLabels_Sample2 = x.ProductDriverLabels_Sample2,
                    ProductDriverLabels_Sample3 = x.ProductDriverLabels_Sample3,
                    ProductDriverLabels_Sample4 = x.ProductDriverLabels_Sample4,
                    ProductDriverLabels_Sample5 = x.ProductDriverLabels_Sample5,
                    ProductDriverLabels_Result = x.ProductDriverLabels_Result,
                    PackingStickers_Sample1 = x.PackingStickers_Sample1,
                    PackingStickers_Sample2 = x.PackingStickers_Sample2,
                    PackingStickers_Sample3 = x.PackingStickers_Sample3,
                    PackingStickers_Sample4 = x.PackingStickers_Sample4,
                    PackingStickers_Sample5 = x.PackingStickers_Sample5,
                    PackingStickers_Result = x.PackingStickers_Result,
                    Dimensions_Sample1 = x.Dimensions_Sample1,
                    Dimensions_Sample2 = x.Dimensions_Sample2,
                    Dimensions_Sample3 = x.Dimensions_Sample3,
                    Dimensions_Sample4 = x.Dimensions_Sample4,
                    Dimensions_Sample5 = x.Dimensions_Sample5,
                    Dimensions_Result = x.Dimensions_Result,
                    SurfaceFinish_Sample1 = x.SurfaceFinish_Sample1,
                    SurfaceFinish_Sample2 = x.SurfaceFinish_Sample2,
                    SurfaceFinish_Sample3 = x.SurfaceFinish_Sample3,
                    SurfaceFinish_Sample4 = x.SurfaceFinish_Sample4,
                    SurfaceFinish_Sample5 = x.SurfaceFinish_Sample5,
                    SurfaceFinish_Result = x.SurfaceFinish_Result,
                    SurfaceFinishLED_Sample1 = x.SurfaceFinishLED_Sample1,
                    SurfaceFinishLED_Sample2 = x.SurfaceFinishLED_Sample2,
                    SurfaceFinishLED_Sample3 = x.SurfaceFinishLED_Sample3,
                    SurfaceFinishLED_Sample4 = x.SurfaceFinishLED_Sample4,
                    SurfaceFinishLED_Sample5 = x.SurfaceFinishLED_Sample5,
                    SurfaceFinishLED_Result = x.SurfaceFinishLED_Result,
                    DFT_Sample1 = x.DFT_Sample1,
                    DFT_Sample2 = x.DFT_Sample2,
                    DFT_Sample3 = x.DFT_Sample3,
                    DFT_Sample4 = x.DFT_Sample4,
                    DFT_Sample5 = x.DFT_Sample5,
                    DFT_Result = x.DFT_Result,
                    Visual_Sample1 = x.Visual_Sample1,
                    Visual_Sample2 = x.Visual_Sample2,
                    Visual_Sample3 = x.Visual_Sample3,
                    Visual_Sample4 = x.Visual_Sample4,
                    Visual_Sample5 = x.Visual_Sample5,
                    Visual_Result = x.Visual_Result,
                    VisualLitUp_Sample1 = x.VisualLitUp_Sample1,
                    VisualLitUp_Sample2 = x.VisualLitUp_Sample2,
                    VisualLitUp_Sample3 = x.VisualLitUp_Sample3,
                    VisualLitUp_Sample4 = x.VisualLitUp_Sample4,
                    VisualLitUp_Sample5 = x.VisualLitUp_Sample5,
                    VisualLitUp_Result = x.VisualLitUp_Result,
                    PcbCobFitment_Sample1 = x.PcbCobFitment_Sample1,
                    PcbCobFitment_Sample2 = x.PcbCobFitment_Sample2,
                    PcbCobFitment_Sample3 = x.PcbCobFitment_Sample3,
                    PcbCobFitment_Sample4 = x.PcbCobFitment_Sample4,
                    PcbCobFitment_Sample5 = x.PcbCobFitment_Sample5,
                    PcbCobFitment_Result = x.PcbCobFitment_Result,
                    PcbCobFitmentNoGaps_Sample1 = x.PcbCobFitmentNoGaps_Sample1,
                    PcbCobFitmentNoGaps_Sample2 = x.PcbCobFitmentNoGaps_Sample2,
                    PcbCobFitmentNoGaps_Sample3 = x.PcbCobFitmentNoGaps_Sample3,
                    PcbCobFitmentNoGaps_Sample4 = x.PcbCobFitmentNoGaps_Sample4,
                    PcbCobFitmentNoGaps_Sample5 = x.PcbCobFitmentNoGaps_Sample5,
                    PcbCobFitmentNoGaps_Result = x.PcbCobFitmentNoGaps_Result,
                    PcbCobFitmentScrew_Sample1 = x.PcbCobFitmentScrew_Sample1,
                    PcbCobFitmentScrew_Sample2 = x.PcbCobFitmentScrew_Sample2,
                    PcbCobFitmentScrew_Sample3 = x.PcbCobFitmentScrew_Sample3,
                    PcbCobFitmentScrew_Sample4 = x.PcbCobFitmentScrew_Sample4,
                    PcbCobFitmentScrew_Sample5 = x.PcbCobFitmentScrew_Sample5,
                    PcbCobFitmentScrew_Result = x.PcbCobFitmentScrew_Result,
                    PcbCobFitmentWasher_Sample1 = x.PcbCobFitmentWasher_Sample1,
                    PcbCobFitmentWasher_Sample2 = x.PcbCobFitmentWasher_Sample2,
                    PcbCobFitmentWasher_Sample3 = x.PcbCobFitmentWasher_Sample3,
                    PcbCobFitmentWasher_Sample4 = x.PcbCobFitmentWasher_Sample4,
                    PcbCobFitmentWasher_Sample5 = x.PcbCobFitmentWasher_Sample5,
                    PcbCobFitmentWasher_Result = x.PcbCobFitmentWasher_Result,
                    PcbCobFitmentDrawing_Sample1 = x.PcbCobFitmentDrawing_Sample1,
                    PcbCobFitmentDrawing_Sample2 = x.PcbCobFitmentDrawing_Sample2,
                    PcbCobFitmentDrawing_Sample3 = x.PcbCobFitmentDrawing_Sample3,
                    PcbCobFitmentDrawing_Sample4 = x.PcbCobFitmentDrawing_Sample4,
                    PcbCobFitmentDrawing_Sample5 = x.PcbCobFitmentDrawing_Sample5,
                    PcbCobFitmentDrawing_Result = x.PcbCobFitmentDrawing_Result,
                    Soldering_Sample1 = x.Soldering_Sample1,
                    Soldering_Sample2 = x.Soldering_Sample2,
                    Soldering_Sample3 = x.Soldering_Sample3,
                    Soldering_Sample4 = x.Soldering_Sample4,
                    Soldering_Sample5 = x.Soldering_Sample5,
                    Soldering_Result = x.Soldering_Result,
                    SolderingSpatter_Sample1 = x.SolderingSpatter_Sample1,
                    SolderingSpatter_Sample2 = x.SolderingSpatter_Sample2,
                    SolderingSpatter_Sample3 = x.SolderingSpatter_Sample3,
                    SolderingSpatter_Sample4 = x.SolderingSpatter_Sample4,
                    SolderingSpatter_Sample5 = x.SolderingSpatter_Sample5,
                    SolderingSpatter_Result = x.SolderingSpatter_Result,
                    SolderingGlobule_Sample1 = x.SolderingGlobule_Sample1,
                    SolderingGlobule_Sample2 = x.SolderingGlobule_Sample2,
                    SolderingGlobule_Sample3 = x.SolderingGlobule_Sample3,
                    SolderingGlobule_Sample4 = x.SolderingGlobule_Sample4,
                    SolderingGlobule_Sample5 = x.SolderingGlobule_Sample5,
                    SolderingGlobule_Result = x.SolderingGlobule_Result,
                    WiringDressing_Sample1 = x.WiringDressing_Sample1,
                    WiringDressing_Sample2 = x.WiringDressing_Sample2,
                    WiringDressing_Sample3 = x.WiringDressing_Sample3,
                    WiringDressing_Sample4 = x.WiringDressing_Sample4,
                    WiringDressing_Sample5 = x.WiringDressing_Sample5,
                    WiringDressing_Result = x.WiringDressing_Result,
                    MechanicalFitment_Sample1 = x.MechanicalFitment_Sample1,
                    MechanicalFitment_Sample2 = x.MechanicalFitment_Sample2,
                    MechanicalFitment_Sample3 = x.MechanicalFitment_Sample3,
                    MechanicalFitment_Sample4 = x.MechanicalFitment_Sample4,
                    MechanicalFitment_Sample5 = x.MechanicalFitment_Sample5,
                    MechanicalFitment_Result = x.MechanicalFitment_Result,
                    LedLensGap_Sample1 = x.LedLensGap_Sample1,
                    LedLensGap_Sample2 = x.LedLensGap_Sample2,
                    LedLensGap_Sample3 = x.LedLensGap_Sample3,
                    LedLensGap_Sample4 = x.LedLensGap_Sample4,
                    LedLensGap_Sample5 = x.LedLensGap_Sample5,
                    LedLensGap_Result = x.LedLensGap_Result,
                    Gasket_Sample1 = x.Gasket_Sample1,
                    Gasket_Sample2 = x.Gasket_Sample2,
                    Gasket_Sample3 = x.Gasket_Sample3,
                    Gasket_Sample4 = x.Gasket_Sample4,
                    Gasket_Sample5 = x.Gasket_Sample5,
                    Gasket_Result = x.Gasket_Result,
                    GlassFragmentation_Sample1 = x.GlassFragmentation_Sample1,
                    GlassFragmentation_Sample2 = x.GlassFragmentation_Sample2,
                    GlassFragmentation_Sample3 = x.GlassFragmentation_Sample3,
                    GlassFragmentation_Sample4 = x.GlassFragmentation_Sample4,
                    GlassFragmentation_Sample5 = x.GlassFragmentation_Sample5,
                    GlassFragmentation_Result = x.GlassFragmentation_Result,
                    TestedBy = x.TestedBy,
                    VerifiedBy = x.VerifiedBy
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

    public async Task<OperationResult> InsertPhysicalCheckAndVisualInspectionsAsync(PhysicalCheckAndVisualInspectionViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Report_No", model.Report_No ?? (object)DBNull.Value),
                new SqlParameter("@Project_Name", model.Project_Name ?? (object)DBNull.Value),
                new SqlParameter("@Report_Date", model.Report_Date ?? (object)DBNull.Value),
                new SqlParameter("@Product_Cat_Ref", model.Product_Cat_Ref ?? (object)DBNull.Value),
                new SqlParameter("@Product_Description", model.Product_Description ?? (object)DBNull.Value),
                new SqlParameter("@Batch_Code", model.Batch_Code ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
                new SqlParameter("@Quantity", model.Quantity ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample1", model.WiproBranding_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample2", model.WiproBranding_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample3", model.WiproBranding_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample4", model.WiproBranding_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample5", model.WiproBranding_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Result", model.WiproBranding_Result ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample1", model.ProductDriverLabels_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample2", model.ProductDriverLabels_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample3", model.ProductDriverLabels_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample4", model.ProductDriverLabels_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample5", model.ProductDriverLabels_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Result", model.ProductDriverLabels_Result ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample1", model.PackingStickers_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample2", model.PackingStickers_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample3", model.PackingStickers_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample4", model.PackingStickers_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample5", model.PackingStickers_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Result", model.PackingStickers_Result ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample1", model.Dimensions_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample2", model.Dimensions_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample3", model.Dimensions_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample4", model.Dimensions_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample5", model.Dimensions_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Result", model.Dimensions_Result ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample1", model.SurfaceFinish_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample2", model.SurfaceFinish_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample3", model.SurfaceFinish_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample4", model.SurfaceFinish_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample5", model.SurfaceFinish_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Result", model.SurfaceFinish_Result ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample1", model.SurfaceFinishLED_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample2", model.SurfaceFinishLED_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample3", model.SurfaceFinishLED_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample4", model.SurfaceFinishLED_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample5", model.SurfaceFinishLED_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Result", model.SurfaceFinishLED_Result ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample1", model.DFT_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample2", model.DFT_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample3", model.DFT_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample4", model.DFT_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample5", model.DFT_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Result", model.DFT_Result ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample1", model.Visual_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample2", model.Visual_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample3", model.Visual_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample4", model.Visual_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample5", model.Visual_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Result", model.Visual_Result ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample1", model.VisualLitUp_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample2", model.VisualLitUp_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample3", model.VisualLitUp_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample4", model.VisualLitUp_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample5", model.VisualLitUp_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Result", model.VisualLitUp_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample1", model.PcbCobFitment_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample2", model.PcbCobFitment_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample3", model.PcbCobFitment_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample4", model.PcbCobFitment_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample5", model.PcbCobFitment_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Result", model.PcbCobFitment_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample1", model.PcbCobFitmentNoGaps_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample2", model.PcbCobFitmentNoGaps_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample3", model.PcbCobFitmentNoGaps_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample4", model.PcbCobFitmentNoGaps_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample5", model.PcbCobFitmentNoGaps_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Result", model.PcbCobFitmentNoGaps_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample1", model.PcbCobFitmentScrew_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample2", model.PcbCobFitmentScrew_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample3", model.PcbCobFitmentScrew_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample4", model.PcbCobFitmentScrew_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample5", model.PcbCobFitmentScrew_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Result", model.PcbCobFitmentScrew_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample1", model.PcbCobFitmentWasher_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample2", model.PcbCobFitmentWasher_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample3", model.PcbCobFitmentWasher_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample4", model.PcbCobFitmentWasher_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample5", model.PcbCobFitmentWasher_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Result", model.PcbCobFitmentWasher_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample1", model.PcbCobFitmentDrawing_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample2", model.PcbCobFitmentDrawing_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample3", model.PcbCobFitmentDrawing_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample4", model.PcbCobFitmentDrawing_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample5", model.PcbCobFitmentDrawing_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Result", model.PcbCobFitmentDrawing_Result ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample1", model.Soldering_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample2", model.Soldering_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample3", model.Soldering_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample4", model.Soldering_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample5", model.Soldering_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Result", model.Soldering_Result ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample1", model.SolderingSpatter_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample2", model.SolderingSpatter_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample3", model.SolderingSpatter_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample4", model.SolderingSpatter_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample5", model.SolderingSpatter_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Result", model.SolderingSpatter_Result ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample1", model.SolderingGlobule_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample2", model.SolderingGlobule_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample3", model.SolderingGlobule_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample4", model.SolderingGlobule_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample5", model.SolderingGlobule_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Result", model.SolderingGlobule_Result ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample1", model.WiringDressing_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample2", model.WiringDressing_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample3", model.WiringDressing_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample4", model.WiringDressing_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample5", model.WiringDressing_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Result", model.WiringDressing_Result ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample1", model.MechanicalFitment_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample2", model.MechanicalFitment_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample3", model.MechanicalFitment_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample4", model.MechanicalFitment_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample5", model.MechanicalFitment_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Result", model.MechanicalFitment_Result ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample1", model.LedLensGap_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample2", model.LedLensGap_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample3", model.LedLensGap_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample4", model.LedLensGap_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample5", model.LedLensGap_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Result", model.LedLensGap_Result ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample1", model.Gasket_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample2", model.Gasket_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample3", model.Gasket_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample4", model.Gasket_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample5", model.Gasket_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Result", model.Gasket_Result ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample1", model.GlassFragmentation_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample2", model.GlassFragmentation_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample3", model.GlassFragmentation_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample4", model.GlassFragmentation_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample5", model.GlassFragmentation_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Result", model.GlassFragmentation_Result ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn),
                new SqlParameter("@Final_Result", model.Final_Result ?? (object)DBNull.Value),
                new SqlParameter("@TestedBy", model.TestedBy ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedBy", model.VerifiedBy ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_PhysicalCheckAndVisualInspection " +
                    "@Report_No, " +
                    "@Project_Name, " +
                    "@Report_Date, " +
                    "@Product_Cat_Ref, " +
                    "@Product_Description, " +
                    "@Batch_Code, " +
                    "@PKD, " +
                    "@Quantity, " +
                    "@WiproBranding_Sample1, " +
                    "@WiproBranding_Sample2, " +
                    "@WiproBranding_Sample3, " +
                    "@WiproBranding_Sample4, " +
                    "@WiproBranding_Sample5, " +
                    "@WiproBranding_Result, " +
                    "@ProductDriverLabels_Sample1, " +
                    "@ProductDriverLabels_Sample2, " +
                    "@ProductDriverLabels_Sample3, " +
                    "@ProductDriverLabels_Sample4, " +
                    "@ProductDriverLabels_Sample5, " +
                    "@ProductDriverLabels_Result, " +
                    "@PackingStickers_Sample1, " +
                    "@PackingStickers_Sample2, " +
                    "@PackingStickers_Sample3, " +
                    "@PackingStickers_Sample4, " +
                    "@PackingStickers_Sample5, " +
                    "@PackingStickers_Result, " +
                    "@Dimensions_Sample1, " +
                    "@Dimensions_Sample2, " +
                    "@Dimensions_Sample3, " +
                    "@Dimensions_Sample4, " +
                    "@Dimensions_Sample5, " +
                    "@Dimensions_Result, " +
                    "@SurfaceFinish_Sample1, " +
                    "@SurfaceFinish_Sample2, " +
                    "@SurfaceFinish_Sample3, " +
                    "@SurfaceFinish_Sample4, " +
                    "@SurfaceFinish_Sample5, " +
                    "@SurfaceFinish_Result, " +
                    "@SurfaceFinishLED_Sample1, " +
                    "@SurfaceFinishLED_Sample2, " +
                    "@SurfaceFinishLED_Sample3, " +
                    "@SurfaceFinishLED_Sample4, " +
                    "@SurfaceFinishLED_Sample5, " +
                    "@SurfaceFinishLED_Result, " +
                    "@DFT_Sample1, " +
                    "@DFT_Sample2, " +
                    "@DFT_Sample3, " +
                    "@DFT_Sample4, " +
                    "@DFT_Sample5, " +
                    "@DFT_Result, " +
                    "@Visual_Sample1, " +
                    "@Visual_Sample2, " +
                    "@Visual_Sample3, " +
                    "@Visual_Sample4, " +
                    "@Visual_Sample5, " +
                    "@Visual_Result, " +
                    "@VisualLitUp_Sample1, " +
                    "@VisualLitUp_Sample2, " +
                    "@VisualLitUp_Sample3, " +
                    "@VisualLitUp_Sample4, " +
                    "@VisualLitUp_Sample5, " +
                    "@VisualLitUp_Result, " +
                    "@PcbCobFitment_Sample1, " +
                    "@PcbCobFitment_Sample2, " +
                    "@PcbCobFitment_Sample3, " +
                    "@PcbCobFitment_Sample4, " +
                    "@PcbCobFitment_Sample5, " +
                    "@PcbCobFitment_Result, " +
                    "@PcbCobFitmentNoGaps_Sample1, " +
                    "@PcbCobFitmentNoGaps_Sample2, " +
                    "@PcbCobFitmentNoGaps_Sample3, " +
                    "@PcbCobFitmentNoGaps_Sample4, " +
                    "@PcbCobFitmentNoGaps_Sample5, " +
                    "@PcbCobFitmentNoGaps_Result, " +
                    "@PcbCobFitmentScrew_Sample1, " +
                    "@PcbCobFitmentScrew_Sample2, " +
                    "@PcbCobFitmentScrew_Sample3, " +
                    "@PcbCobFitmentScrew_Sample4, " +
                    "@PcbCobFitmentScrew_Sample5, " +
                    "@PcbCobFitmentScrew_Result, " +
                    "@PcbCobFitmentWasher_Sample1, " +
                    "@PcbCobFitmentWasher_Sample2, " +
                    "@PcbCobFitmentWasher_Sample3, " +
                    "@PcbCobFitmentWasher_Sample4, " +
                    "@PcbCobFitmentWasher_Sample5, " +
                    "@PcbCobFitmentWasher_Result, " +
                    "@PcbCobFitmentDrawing_Sample1, " +
                    "@PcbCobFitmentDrawing_Sample2, " +
                    "@PcbCobFitmentDrawing_Sample3, " +
                    "@PcbCobFitmentDrawing_Sample4, " +
                    "@PcbCobFitmentDrawing_Sample5, " +
                    "@PcbCobFitmentDrawing_Result, " +
                    "@Soldering_Sample1, " +
                    "@Soldering_Sample2, " +
                    "@Soldering_Sample3, " +
                    "@Soldering_Sample4, " +
                    "@Soldering_Sample5, " +
                    "@Soldering_Result, " +
                    "@SolderingSpatter_Sample1, " +
                    "@SolderingSpatter_Sample2, " +
                    "@SolderingSpatter_Sample3, " +
                    "@SolderingSpatter_Sample4, " +
                    "@SolderingSpatter_Sample5, " +
                    "@SolderingSpatter_Result, " +
                    "@SolderingGlobule_Sample1, " +
                    "@SolderingGlobule_Sample2, " +
                    "@SolderingGlobule_Sample3, " +
                    "@SolderingGlobule_Sample4, " +
                    "@SolderingGlobule_Sample5, " +
                    "@SolderingGlobule_Result, " +
                    "@WiringDressing_Sample1, " +
                    "@WiringDressing_Sample2, " +
                    "@WiringDressing_Sample3, " +
                    "@WiringDressing_Sample4, " +
                    "@WiringDressing_Sample5, " +
                    "@WiringDressing_Result, " +
                    "@MechanicalFitment_Sample1, " +
                    "@MechanicalFitment_Sample2, " +
                    "@MechanicalFitment_Sample3, " +
                    "@MechanicalFitment_Sample4, " +
                    "@MechanicalFitment_Sample5, " +
                    "@MechanicalFitment_Result, " +
                    "@LedLensGap_Sample1, " +
                    "@LedLensGap_Sample2, " +
                    "@LedLensGap_Sample3, " +
                    "@LedLensGap_Sample4, " +
                    "@LedLensGap_Sample5, " +
                    "@LedLensGap_Result, " +
                    "@Gasket_Sample1, " +
                    "@Gasket_Sample2, " +
                    "@Gasket_Sample3, " +
                    "@Gasket_Sample4, " +
                    "@Gasket_Sample5, " +
                    "@Gasket_Result, " +
                    "@GlassFragmentation_Sample1, " +
                    "@GlassFragmentation_Sample2, " +
                    "@GlassFragmentation_Sample3, " +
                    "@GlassFragmentation_Sample4, " +
                    "@GlassFragmentation_Sample5, " +
                    "@GlassFragmentation_Result, " +
                    "@AddedBy, " +
                    "@AddedOn, " +
                    "@Final_Result, " +
                    "@TestedBy, " +
                    "@VerifiedBy",
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

    public async Task<OperationResult> UpdatePhysicalCheckAndVisualInspectionsAsync(PhysicalCheckAndVisualInspectionViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@Report_No", model.Report_No ?? (object)DBNull.Value),
                new SqlParameter("@Project_Name", model.Project_Name ?? (object)DBNull.Value),
                new SqlParameter("@Report_Date", model.Report_Date ?? (object)DBNull.Value),
                new SqlParameter("@Product_Cat_Ref", model.Product_Cat_Ref ?? (object)DBNull.Value),
                new SqlParameter("@Product_Description", model.Product_Description ?? (object)DBNull.Value),
                new SqlParameter("@Batch_Code", model.Batch_Code ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
                new SqlParameter("@Quantity", model.Quantity ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample1", model.WiproBranding_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample2", model.WiproBranding_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample3", model.WiproBranding_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample4", model.WiproBranding_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Sample5", model.WiproBranding_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@WiproBranding_Result", model.WiproBranding_Result ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample1", model.ProductDriverLabels_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample2", model.ProductDriverLabels_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample3", model.ProductDriverLabels_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample4", model.ProductDriverLabels_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Sample5", model.ProductDriverLabels_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@ProductDriverLabels_Result", model.ProductDriverLabels_Result ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample1", model.PackingStickers_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample2", model.PackingStickers_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample3", model.PackingStickers_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample4", model.PackingStickers_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Sample5", model.PackingStickers_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PackingStickers_Result", model.PackingStickers_Result ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample1", model.Dimensions_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample2", model.Dimensions_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample3", model.Dimensions_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample4", model.Dimensions_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Sample5", model.Dimensions_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Dimensions_Result", model.Dimensions_Result ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample1", model.SurfaceFinish_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample2", model.SurfaceFinish_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample3", model.SurfaceFinish_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample4", model.SurfaceFinish_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Sample5", model.SurfaceFinish_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinish_Result", model.SurfaceFinish_Result ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample1", model.SurfaceFinishLED_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample2", model.SurfaceFinishLED_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample3", model.SurfaceFinishLED_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample4", model.SurfaceFinishLED_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Sample5", model.SurfaceFinishLED_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SurfaceFinishLED_Result", model.SurfaceFinishLED_Result ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample1", model.DFT_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample2", model.DFT_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample3", model.DFT_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample4", model.DFT_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Sample5", model.DFT_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@DFT_Result", model.DFT_Result ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample1", model.Visual_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample2", model.Visual_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample3", model.Visual_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample4", model.Visual_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Sample5", model.Visual_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Visual_Result", model.Visual_Result ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample1", model.VisualLitUp_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample2", model.VisualLitUp_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample3", model.VisualLitUp_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample4", model.VisualLitUp_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Sample5", model.VisualLitUp_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@VisualLitUp_Result", model.VisualLitUp_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample1", model.PcbCobFitment_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample2", model.PcbCobFitment_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample3", model.PcbCobFitment_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample4", model.PcbCobFitment_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Sample5", model.PcbCobFitment_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitment_Result", model.PcbCobFitment_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample1", model.PcbCobFitmentNoGaps_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample2", model.PcbCobFitmentNoGaps_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample3", model.PcbCobFitmentNoGaps_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample4", model.PcbCobFitmentNoGaps_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Sample5", model.PcbCobFitmentNoGaps_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentNoGaps_Result", model.PcbCobFitmentNoGaps_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample1", model.PcbCobFitmentScrew_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample2", model.PcbCobFitmentScrew_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample3", model.PcbCobFitmentScrew_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample4", model.PcbCobFitmentScrew_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Sample5", model.PcbCobFitmentScrew_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentScrew_Result", model.PcbCobFitmentScrew_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample1", model.PcbCobFitmentWasher_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample2", model.PcbCobFitmentWasher_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample3", model.PcbCobFitmentWasher_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample4", model.PcbCobFitmentWasher_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Sample5", model.PcbCobFitmentWasher_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentWasher_Result", model.PcbCobFitmentWasher_Result ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample1", model.PcbCobFitmentDrawing_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample2", model.PcbCobFitmentDrawing_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample3", model.PcbCobFitmentDrawing_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample4", model.PcbCobFitmentDrawing_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Sample5", model.PcbCobFitmentDrawing_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@PcbCobFitmentDrawing_Result", model.PcbCobFitmentDrawing_Result ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample1", model.Soldering_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample2", model.Soldering_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample3", model.Soldering_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample4", model.Soldering_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Sample5", model.Soldering_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Soldering_Result", model.Soldering_Result ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample1", model.SolderingSpatter_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample2", model.SolderingSpatter_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample3", model.SolderingSpatter_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample4", model.SolderingSpatter_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Sample5", model.SolderingSpatter_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingSpatter_Result", model.SolderingSpatter_Result ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample1", model.SolderingGlobule_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample2", model.SolderingGlobule_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample3", model.SolderingGlobule_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample4", model.SolderingGlobule_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Sample5", model.SolderingGlobule_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@SolderingGlobule_Result", model.SolderingGlobule_Result ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample1", model.WiringDressing_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample2", model.WiringDressing_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample3", model.WiringDressing_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample4", model.WiringDressing_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Sample5", model.WiringDressing_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@WiringDressing_Result", model.WiringDressing_Result ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample1", model.MechanicalFitment_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample2", model.MechanicalFitment_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample3", model.MechanicalFitment_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample4", model.MechanicalFitment_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Sample5", model.MechanicalFitment_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@MechanicalFitment_Result", model.MechanicalFitment_Result ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample1", model.LedLensGap_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample2", model.LedLensGap_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample3", model.LedLensGap_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample4", model.LedLensGap_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Sample5", model.LedLensGap_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@LedLensGap_Result", model.LedLensGap_Result ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample1", model.Gasket_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample2", model.Gasket_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample3", model.Gasket_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample4", model.Gasket_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Sample5", model.Gasket_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@Gasket_Result", model.Gasket_Result ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample1", model.GlassFragmentation_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample2", model.GlassFragmentation_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample3", model.GlassFragmentation_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample4", model.GlassFragmentation_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Sample5", model.GlassFragmentation_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@GlassFragmentation_Result", model.GlassFragmentation_Result ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value),
                new SqlParameter("@Final_Result", model.Final_Result ?? (object)DBNull.Value),
                new SqlParameter("@TestedBy", model.TestedBy),
                new SqlParameter("@VerifiedBy", model.VerifiedBy)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_PhysicalCheckAndVisualInspection " +
                    "@Id, " +
                    "@Report_No, " +
                    "@Project_Name, " +
                    "@Report_Date, " +
                    "@Product_Cat_Ref, " +
                    "@Product_Description, " +
                    "@Batch_Code, " +
                    "@PKD, " +
                    "@Quantity, " +
                    "@WiproBranding_Sample1, " +
                    "@WiproBranding_Sample2, " +
                    "@WiproBranding_Sample3, " +
                    "@WiproBranding_Sample4, " +
                    "@WiproBranding_Sample5, " +
                    "@WiproBranding_Result, " +
                    "@ProductDriverLabels_Sample1, " +
                    "@ProductDriverLabels_Sample2, " +
                    "@ProductDriverLabels_Sample3, " +
                    "@ProductDriverLabels_Sample4, " +
                    "@ProductDriverLabels_Sample5, " +
                    "@ProductDriverLabels_Result, " +
                    "@PackingStickers_Sample1, " +
                    "@PackingStickers_Sample2, " +
                    "@PackingStickers_Sample3, " +
                    "@PackingStickers_Sample4, " +
                    "@PackingStickers_Sample5, " +
                    "@PackingStickers_Result, " +
                    "@Dimensions_Sample1, " +
                    "@Dimensions_Sample2, " +
                    "@Dimensions_Sample3, " +
                    "@Dimensions_Sample4, " +
                    "@Dimensions_Sample5, " +
                    "@Dimensions_Result, " +
                    "@SurfaceFinish_Sample1, " +
                    "@SurfaceFinish_Sample2, " +
                    "@SurfaceFinish_Sample3, " +
                    "@SurfaceFinish_Sample4, " +
                    "@SurfaceFinish_Sample5, " +
                    "@SurfaceFinish_Result, " +
                    "@SurfaceFinishLED_Sample1, " +
                    "@SurfaceFinishLED_Sample2, " +
                    "@SurfaceFinishLED_Sample3, " +
                    "@SurfaceFinishLED_Sample4, " +
                    "@SurfaceFinishLED_Sample5, " +
                    "@SurfaceFinishLED_Result, " +
                    "@DFT_Sample1, " +
                    "@DFT_Sample2, " +
                    "@DFT_Sample3, " +
                    "@DFT_Sample4, " +
                    "@DFT_Sample5, " +
                    "@DFT_Result, " +
                    "@Visual_Sample1, " +
                    "@Visual_Sample2, " +
                    "@Visual_Sample3, " +
                    "@Visual_Sample4, " +
                    "@Visual_Sample5, " +
                    "@Visual_Result, " +
                    "@VisualLitUp_Sample1, " +
                    "@VisualLitUp_Sample2, " +
                    "@VisualLitUp_Sample3, " +
                    "@VisualLitUp_Sample4, " +
                    "@VisualLitUp_Sample5, " +
                    "@VisualLitUp_Result, " +
                    "@PcbCobFitment_Sample1, " +
                    "@PcbCobFitment_Sample2, " +
                    "@PcbCobFitment_Sample3, " +
                    "@PcbCobFitment_Sample4, " +
                    "@PcbCobFitment_Sample5, " +
                    "@PcbCobFitment_Result, " +
                    "@PcbCobFitmentNoGaps_Sample1, " +
                    "@PcbCobFitmentNoGaps_Sample2, " +
                    "@PcbCobFitmentNoGaps_Sample3, " +
                    "@PcbCobFitmentNoGaps_Sample4, " +
                    "@PcbCobFitmentNoGaps_Sample5, " +
                    "@PcbCobFitmentNoGaps_Result, " +
                    "@PcbCobFitmentScrew_Sample1, " +
                    "@PcbCobFitmentScrew_Sample2, " +
                    "@PcbCobFitmentScrew_Sample3, " +
                    "@PcbCobFitmentScrew_Sample4, " +
                    "@PcbCobFitmentScrew_Sample5, " +
                    "@PcbCobFitmentScrew_Result, " +
                    "@PcbCobFitmentWasher_Sample1, " +
                    "@PcbCobFitmentWasher_Sample2, " +
                    "@PcbCobFitmentWasher_Sample3, " +
                    "@PcbCobFitmentWasher_Sample4, " +
                    "@PcbCobFitmentWasher_Sample5, " +
                    "@PcbCobFitmentWasher_Result, " +
                    "@PcbCobFitmentDrawing_Sample1, " +
                    "@PcbCobFitmentDrawing_Sample2, " +
                    "@PcbCobFitmentDrawing_Sample3, " +
                    "@PcbCobFitmentDrawing_Sample4, " +
                    "@PcbCobFitmentDrawing_Sample5, " +
                    "@PcbCobFitmentDrawing_Result, " +
                    "@Soldering_Sample1, " +
                    "@Soldering_Sample2, " +
                    "@Soldering_Sample3, " +
                    "@Soldering_Sample4, " +
                    "@Soldering_Sample5, " +
                    "@Soldering_Result, " +
                    "@SolderingSpatter_Sample1, " +
                    "@SolderingSpatter_Sample2, " +
                    "@SolderingSpatter_Sample3, " +
                    "@SolderingSpatter_Sample4, " +
                    "@SolderingSpatter_Sample5, " +
                    "@SolderingSpatter_Result, " +
                    "@SolderingGlobule_Sample1, " +
                    "@SolderingGlobule_Sample2, " +
                    "@SolderingGlobule_Sample3, " +
                    "@SolderingGlobule_Sample4, " +
                    "@SolderingGlobule_Sample5, " +
                    "@SolderingGlobule_Result, " +
                    "@WiringDressing_Sample1, " +
                    "@WiringDressing_Sample2, " +
                    "@WiringDressing_Sample3, " +
                    "@WiringDressing_Sample4, " +
                    "@WiringDressing_Sample5, " +
                    "@WiringDressing_Result, " +
                    "@MechanicalFitment_Sample1, " +
                    "@MechanicalFitment_Sample2, " +
                    "@MechanicalFitment_Sample3, " +
                    "@MechanicalFitment_Sample4, " +
                    "@MechanicalFitment_Sample5, " +
                    "@MechanicalFitment_Result, " +
                    "@LedLensGap_Sample1, " +
                    "@LedLensGap_Sample2, " +
                    "@LedLensGap_Sample3, " +
                    "@LedLensGap_Sample4, " +
                    "@LedLensGap_Sample5, " +
                    "@LedLensGap_Result, " +
                    "@Gasket_Sample1, " +
                    "@Gasket_Sample2, " +
                    "@Gasket_Sample3, " +
                    "@Gasket_Sample4, " +
                    "@Gasket_Sample5, " +
                    "@Gasket_Result, " +
                    "@GlassFragmentation_Sample1, " +
                    "@GlassFragmentation_Sample2, " +
                    "@GlassFragmentation_Sample3, " +
                    "@GlassFragmentation_Sample4, " +
                    "@GlassFragmentation_Sample5, " +
                    "@GlassFragmentation_Result, " +
                    "@UpdatedBy, " +
                    "@UpdatedOn, " +
                    "@Final_Result, " +
                    "@TestedBy, " +
                    "@VerifiedBy",
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

    public async Task<OperationResult> DeletePhysicalCheckAndVisualInspectionsAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<PhysicalCheckAndVisualInspection>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
