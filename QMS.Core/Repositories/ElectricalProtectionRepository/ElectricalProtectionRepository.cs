using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ElectricalProtectionRepo;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.ElectricalProtectionRepository
{
    public class ElectricalProtectionRepository : SqlTableRepository, IElectricalProtectionRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;
        public ElectricalProtectionRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
     : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }
        public async Task<List<ElectricalProtectionViewModel>> GetElectricalProtectionsAsync()
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@Id", SqlDbType.Int) { Value = 0 }
        };

                var sql = @"EXEC sp_Get_ElectricalProtection";

                var result = await Task.Run(() => _dbContext.ElectricalProtections
                    .FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new ElectricalProtectionViewModel
                    {
                        Id = x.Id,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        BatchCode = x.BatchCode,
                        PKD = x.PKD,
                        LightSourceDetails = x.LightSourceDetails,
                        DriverDetailsQty = x.DriverDetailsQty,
                        PCBDetailsQty = x.PCBDetailsQty,
                        LEDCombinations = x.LEDCombinations,
                        SensorDetailsQty = x.SensorDetailsQty,
                        LampDetails = x.LampDetails,
                        UnderVoltage_Sample1 = x.UnderVoltage_Sample1,
                        UnderVoltage_Sample2 = x.UnderVoltage_Sample2,
                        UnderVoltage_Sample3 = x.UnderVoltage_Sample3,
                        UnderVoltage_Sample4 = x.UnderVoltage_Sample4,
                        UnderVoltage_Sample5 = x.UnderVoltage_Sample5,
                        UnderVoltage_Result = x.UnderVoltage_Result,

                        OverVoltage_Sample1 = x.OverVoltage_Sample1,
                        OverVoltage_Sample2 = x.OverVoltage_Sample2,
                        OverVoltage_Sample3 = x.OverVoltage_Sample3,
                        OverVoltage_Sample4 = x.OverVoltage_Sample4,
                        OverVoltage_Sample5 = x.OverVoltage_Sample5,
                        OverVoltage_Result = x.OverVoltage_Result,

                        OpenCircuit_Sample1 = x.OpenCircuit_Sample1,
                        OpenCircuit_Sample2 = x.OpenCircuit_Sample2,
                        OpenCircuit_Sample3 = x.OpenCircuit_Sample3,
                        OpenCircuit_Sample4 = x.OpenCircuit_Sample4,
                        OpenCircuit_Sample5 = x.OpenCircuit_Sample5,
                        OpenCircuit_Result = x.OpenCircuit_Result,

                        ShortCircuit_Sample1 = x.ShortCircuit_Sample1,
                        ShortCircuit_Sample2 = x.ShortCircuit_Sample2,
                        ShortCircuit_Sample3 = x.ShortCircuit_Sample3,
                        ShortCircuit_Sample4 = x.ShortCircuit_Sample4,
                        ShortCircuit_Sample5 = x.ShortCircuit_Sample5,
                        ShortCircuit_Result = x.ShortCircuit_Result,

                        ReversePolarity_Sample1 = x.ReversePolarity_Sample1,
                        ReversePolarity_Sample2 = x.ReversePolarity_Sample2,
                        ReversePolarity_Sample3 = x.ReversePolarity_Sample3,
                        ReversePolarity_Sample4 = x.ReversePolarity_Sample4,
                        ReversePolarity_Sample5 = x.ReversePolarity_Sample5,
                        ReversePolarity_Result = x.ReversePolarity_Result,

                        OverLoad_Sample1 = x.OverLoad_Sample1,
                        OverLoad_Sample2 = x.OverLoad_Sample2,
                        OverLoad_Sample3 = x.OverLoad_Sample3,
                        OverLoad_Sample4 = x.OverLoad_Sample4,
                        OverLoad_Sample5 = x.OverLoad_Sample5,
                        OverLoad_Result = x.OverLoad_Result,

                        OverThermal_Sample1 = x.OverThermal_Sample1,
                        OverThermal_Sample2 = x.OverThermal_Sample2,
                        OverThermal_Sample3 = x.OverThermal_Sample3,
                        OverThermal_Sample4 = x.OverThermal_Sample4,
                        OverThermal_Sample5 = x.OverThermal_Sample5,
                        OverThermal_Result = x.OverThermal_Result,

                        EarthFault_Sample1 = x.EarthFault_Sample1,
                        EarthFault_Sample2 = x.EarthFault_Sample2,
                        EarthFault_Sample3 = x.EarthFault_Sample3,
                        EarthFault_Sample4 = x.EarthFault_Sample4,
                        EarthFault_Sample5 = x.EarthFault_Sample5,
                        EarthFault_Result = x.EarthFault_Result,

                        DriverIsolation_Sample1 = x.DriverIsolation_Sample1,
                        DriverIsolation_Sample2 = x.DriverIsolation_Sample2,
                        DriverIsolation_Sample3 = x.DriverIsolation_Sample3,
                        DriverIsolation_Sample4 = x.DriverIsolation_Sample4,
                        DriverIsolation_Sample5 = x.DriverIsolation_Sample5,
                        DriverIsolation_Result = x.DriverIsolation_Result,

                        HighVoltage_Sample1 = x.HighVoltage_Sample1,
                        HighVoltage_Sample2 = x.HighVoltage_Sample2,
                        HighVoltage_Sample3 = x.HighVoltage_Sample3,
                        HighVoltage_Sample4 = x.HighVoltage_Sample4,
                        HighVoltage_Sample5 = x.HighVoltage_Sample5,
                        HighVoltage_Result = x.HighVoltage_Result,

                        HV_Sample1 = x.HV_Sample1,
                        HV_Sample2 = x.HV_Sample2,
                        HV_Sample3 = x.HV_Sample3,
                        HV_Sample4 = x.HV_Sample4,
                        HV_Sample5 = x.HV_Sample5,
                        HV_Result = x.HV_Result,

                        InsulationResistance_Sample1 = x.InsulationResistance_Sample1,
                        InsulationResistance_Sample2 = x.InsulationResistance_Sample2,
                        InsulationResistance_Sample3 = x.InsulationResistance_Sample3,
                        InsulationResistance_Sample4 = x.InsulationResistance_Sample4,
                        InsulationResistance_Sample5 = x.InsulationResistance_Sample5,
                        InsulationResistance_Result = x.InsulationResistance_Result,

                        EarthContinuity_Sample1 = x.EarthContinuity_Sample1,
                        EarthContinuity_Sample2 = x.EarthContinuity_Sample2,
                        EarthContinuity_Sample3 = x.EarthContinuity_Sample3,
                        EarthContinuity_Sample4 = x.EarthContinuity_Sample4,
                        EarthContinuity_Sample5 = x.EarthContinuity_Sample5,
                        EarthContinuity_Result = x.EarthContinuity_Result,

                        SELVProtection_Sample1 = x.SELVProtection_Sample1,
                        SELVProtection_Sample2 = x.SELVProtection_Sample2,
                        SELVProtection_Sample3 = x.SELVProtection_Sample3,
                        SELVProtection_Sample4 = x.SELVProtection_Sample4,
                        SELVProtection_Sample5 = x.SELVProtection_Sample5,
                        SELVProtection_Result = x.SELVProtection_Result,

                        LeakageCurrent_Sample1 = x.LeakageCurrent_Sample1,
                        LeakageCurrent_Sample2 = x.LeakageCurrent_Sample2,
                        LeakageCurrent_Sample3 = x.LeakageCurrent_Sample3,
                        LeakageCurrent_Sample4 = x.LeakageCurrent_Sample4,
                        LeakageCurrent_Sample5 = x.LeakageCurrent_Sample5,
                        LeakageCurrent_Result = x.LeakageCurrent_Result,

                        CreepageClearance_Sample1 = x.CreepageClearance_Sample1,
                        CreepageClearance_Sample2 = x.CreepageClearance_Sample2,
                        CreepageClearance_Sample3 = x.CreepageClearance_Sample3,
                        CreepageClearance_Sample4 = x.CreepageClearance_Sample4,
                        CreepageClearance_Sample5 = x.CreepageClearance_Sample5,
                        CreepageClearance_Result = x.CreepageClearance_Result,

                        HiPotMCPCB_Sample1 = x.HiPotMCPCB_Sample1,
                        HiPotMCPCB_Sample2 = x.HiPotMCPCB_Sample2,
                        HiPotMCPCB_Sample3 = x.HiPotMCPCB_Sample3,
                        HiPotMCPCB_Sample4 = x.HiPotMCPCB_Sample4,
                        HiPotMCPCB_Sample5 = x.HiPotMCPCB_Sample5,
                        HiPotMCPCB_Result = x.HiPotMCPCB_Result,

                        OnOffSwitching_Sample1 = x.OnOffSwitching_Sample1,
                        OnOffSwitching_Sample2 = x.OnOffSwitching_Sample2,
                        OnOffSwitching_Sample3 = x.OnOffSwitching_Sample3,
                        OnOffSwitching_Sample4 = x.OnOffSwitching_Sample4,
                        OnOffSwitching_Sample5 = x.OnOffSwitching_Sample5,
                        OnOffSwitching_Result = x.OnOffSwitching_Result,

                        SoakingAgeing_Sample1 = x.SoakingAgeing_Sample1,
                        SoakingAgeing_Sample2 = x.SoakingAgeing_Sample2,
                        SoakingAgeing_Sample3 = x.SoakingAgeing_Sample3,
                        SoakingAgeing_Sample4 = x.SoakingAgeing_Sample4,
                        SoakingAgeing_Sample5 = x.SoakingAgeing_Sample5,
                        SoakingAgeing_Result = x.SoakingAgeing_Result,

                        RollingEndurance_Sample1 = x.RollingEndurance_Sample1,
                        RollingEndurance_Sample2 = x.RollingEndurance_Sample2,
                        RollingEndurance_Sample3 = x.RollingEndurance_Sample3,
                        RollingEndurance_Sample4 = x.RollingEndurance_Sample4,
                        RollingEndurance_Sample5 = x.RollingEndurance_Sample5,
                        RollingEndurance_Result = x.RollingEndurance_Result,

                        GlowTest_Sample1 = x.GlowTest_Sample1,
                        GlowTest_Sample2 = x.GlowTest_Sample2,
                        GlowTest_Sample3 = x.GlowTest_Sample3,
                        GlowTest_Sample4 = x.GlowTest_Sample4,
                        GlowTest_Sample5 = x.GlowTest_Sample5,
                        GlowTest_Result = x.GlowTest_Result,

                        LampAccommodation_Sample1 = x.LampAccommodation_Sample1,
                        LampAccommodation_Sample2 = x.LampAccommodation_Sample2,
                        LampAccommodation_Sample3 = x.LampAccommodation_Sample3,
                        LampAccommodation_Sample4 = x.LampAccommodation_Sample4,
                        LampAccommodation_Sample5 = x.LampAccommodation_Sample5,
                        LampAccommodation_Result = x.LampAccommodation_Result,

                        DaliFunction_Sample1 = x.DaliFunction_Sample1,
                        DaliFunction_Sample2 = x.DaliFunction_Sample2,
                        DaliFunction_Sample3 = x.DaliFunction_Sample3,
                        DaliFunction_Sample4 = x.DaliFunction_Sample4,
                        DaliFunction_Sample5 = x.DaliFunction_Sample5,
                        DaliFunction_Result = x.DaliFunction_Result,

                        TuneableCCT_Sample1 = x.TuneableCCT_Sample1,
                        TuneableCCT_Sample2 = x.TuneableCCT_Sample2,
                        TuneableCCT_Sample3 = x.TuneableCCT_Sample3,
                        TuneableCCT_Sample4 = x.TuneableCCT_Sample4,
                        TuneableCCT_Sample5 = x.TuneableCCT_Sample5,
                        TuneableCCT_Result = x.TuneableCCT_Result,

                        BatteryBackup_Sample1 = x.BatteryBackup_Sample1,
                        BatteryBackup_Sample2 = x.BatteryBackup_Sample2,
                        BatteryBackup_Sample3 = x.BatteryBackup_Sample3,
                        BatteryBackup_Sample4 = x.BatteryBackup_Sample4,
                        BatteryBackup_Sample5 = x.BatteryBackup_Sample5,
                        BatteryBackup_Result = x.BatteryBackup_Result,

                        SmartLighting_Sample1 = x.SmartLighting_Sample1,
                        SmartLighting_Sample2 = x.SmartLighting_Sample2,
                        SmartLighting_Sample3 = x.SmartLighting_Sample3,
                        SmartLighting_Sample4 = x.SmartLighting_Sample4,
                        SmartLighting_Sample5 = x.SmartLighting_Sample5,
                        SmartLighting_Result = x.SmartLighting_Result,

                        SensorFunction_Sample1 = x.SensorFunction_Sample1,
                        SensorFunction_Sample2 = x.SensorFunction_Sample2,
                        SensorFunction_Sample3 = x.SensorFunction_Sample3,
                        SensorFunction_Sample4 = x.SensorFunction_Sample4,
                        SensorFunction_Sample5 = x.SensorFunction_Sample5,
                        SensorFunction_Result = x.SensorFunction_Result,

                        OverallReportResult = x.OverallReportResult,
                        TestedByName = x.TestedByName,
                        VerifiedByName = x.VerifiedByName,
                        TestedBySignature =x.TestedBySignature,
                        VerifiedBySignature =x.VerifiedBySignature,

                        AddedBy = x.AddedBy,
                        AddedOn = x.AddedOn,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedOn = x.UpdatedOn
                    })
                    .ToList());

                foreach (var rec in result)
                {
                    rec.User = await _dbContext.User
                        .Where(u => u.Id == rec.AddedBy)
                        .Select(u => u.Name)
                        .FirstOrDefaultAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
        public async Task<ElectricalProtectionViewModel> GetElectricalProtectionByIdAsync(int id)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@Id", id),
        };

                var sql = @"EXEC sp_Get_ElectricalProtection @Id";

                    var result = await Task.Run(() => _dbContext.ElectricalProtections
                        .FromSqlRaw(sql, parameters)
                        .AsEnumerable()
                        .Select(x => new ElectricalProtectionViewModel
                        {
                            Id = x.Id,
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        ReportNo = x.ReportNo,
                        ReportDate = x.ReportDate,
                        BatchCode = x.BatchCode,
                        PKD = x.PKD,
                        LightSourceDetails = x.LightSourceDetails,
                        DriverDetailsQty = x.DriverDetailsQty,
                        PCBDetailsQty = x.PCBDetailsQty,
                        LEDCombinations = x.LEDCombinations,
                        SensorDetailsQty = x.SensorDetailsQty,
                        LampDetails = x.LampDetails,
                        UnderVoltage_Sample1 = x.UnderVoltage_Sample1,
                        UnderVoltage_Sample2 = x.UnderVoltage_Sample2,
                        UnderVoltage_Sample3 = x.UnderVoltage_Sample3,
                        UnderVoltage_Sample4 = x.UnderVoltage_Sample4,
                        UnderVoltage_Sample5 = x.UnderVoltage_Sample5,
                        UnderVoltage_Result = x.UnderVoltage_Result,

                        OverVoltage_Sample1 = x.OverVoltage_Sample1,
                        OverVoltage_Sample2 = x.OverVoltage_Sample2,
                        OverVoltage_Sample3 = x.OverVoltage_Sample3,
                        OverVoltage_Sample4 = x.OverVoltage_Sample4,
                        OverVoltage_Sample5 = x.OverVoltage_Sample5,
                        OverVoltage_Result = x.OverVoltage_Result,

                        OpenCircuit_Sample1 = x.OpenCircuit_Sample1,
                        OpenCircuit_Sample2 = x.OpenCircuit_Sample2,
                        OpenCircuit_Sample3 = x.OpenCircuit_Sample3,
                        OpenCircuit_Sample4 = x.OpenCircuit_Sample4,
                        OpenCircuit_Sample5 = x.OpenCircuit_Sample5,
                        OpenCircuit_Result = x.OpenCircuit_Result,

                        ShortCircuit_Sample1 = x.ShortCircuit_Sample1,
                        ShortCircuit_Sample2 = x.ShortCircuit_Sample2,
                        ShortCircuit_Sample3 = x.ShortCircuit_Sample3,
                        ShortCircuit_Sample4 = x.ShortCircuit_Sample4,
                        ShortCircuit_Sample5 = x.ShortCircuit_Sample5,
                        ShortCircuit_Result = x.ShortCircuit_Result,

                        ReversePolarity_Sample1 = x.ReversePolarity_Sample1,
                        ReversePolarity_Sample2 = x.ReversePolarity_Sample2,
                        ReversePolarity_Sample3 = x.ReversePolarity_Sample3,
                        ReversePolarity_Sample4 = x.ReversePolarity_Sample4,
                        ReversePolarity_Sample5 = x.ReversePolarity_Sample5,
                        ReversePolarity_Result = x.ReversePolarity_Result,

                        OverLoad_Sample1 = x.OverLoad_Sample1,
                        OverLoad_Sample2 = x.OverLoad_Sample2,
                        OverLoad_Sample3 = x.OverLoad_Sample3,
                        OverLoad_Sample4 = x.OverLoad_Sample4,
                        OverLoad_Sample5 = x.OverLoad_Sample5,
                        OverLoad_Result = x.OverLoad_Result,

                        OverThermal_Sample1 = x.OverThermal_Sample1,
                        OverThermal_Sample2 = x.OverThermal_Sample2,
                        OverThermal_Sample3 = x.OverThermal_Sample3,
                        OverThermal_Sample4 = x.OverThermal_Sample4,
                        OverThermal_Sample5 = x.OverThermal_Sample5,
                        OverThermal_Result = x.OverThermal_Result,

                        EarthFault_Sample1 = x.EarthFault_Sample1,
                        EarthFault_Sample2 = x.EarthFault_Sample2,
                        EarthFault_Sample3 = x.EarthFault_Sample3,
                        EarthFault_Sample4 = x.EarthFault_Sample4,
                        EarthFault_Sample5 = x.EarthFault_Sample5,
                        EarthFault_Result = x.EarthFault_Result,

                        DriverIsolation_Sample1 = x.DriverIsolation_Sample1,
                        DriverIsolation_Sample2 = x.DriverIsolation_Sample2,
                        DriverIsolation_Sample3 = x.DriverIsolation_Sample3,
                        DriverIsolation_Sample4 = x.DriverIsolation_Sample4,
                        DriverIsolation_Sample5 = x.DriverIsolation_Sample5,
                        DriverIsolation_Result = x.DriverIsolation_Result,

                        HighVoltage_Sample1 = x.HighVoltage_Sample1,
                        HighVoltage_Sample2 = x.HighVoltage_Sample2,
                        HighVoltage_Sample3 = x.HighVoltage_Sample3,
                        HighVoltage_Sample4 = x.HighVoltage_Sample4,
                        HighVoltage_Sample5 = x.HighVoltage_Sample5,
                        HighVoltage_Result = x.HighVoltage_Result,

                        HV_Sample1 = x.HV_Sample1,
                        HV_Sample2 = x.HV_Sample2,
                        HV_Sample3 = x.HV_Sample3,
                        HV_Sample4 = x.HV_Sample4,
                        HV_Sample5 = x.HV_Sample5,
                        HV_Result = x.HV_Result,

                        InsulationResistance_Sample1 = x.InsulationResistance_Sample1,
                        InsulationResistance_Sample2 = x.InsulationResistance_Sample2,
                        InsulationResistance_Sample3 = x.InsulationResistance_Sample3,
                        InsulationResistance_Sample4 = x.InsulationResistance_Sample4,
                        InsulationResistance_Sample5 = x.InsulationResistance_Sample5,
                        InsulationResistance_Result = x.InsulationResistance_Result,

                        EarthContinuity_Sample1 = x.EarthContinuity_Sample1,
                        EarthContinuity_Sample2 = x.EarthContinuity_Sample2,
                        EarthContinuity_Sample3 = x.EarthContinuity_Sample3,
                        EarthContinuity_Sample4 = x.EarthContinuity_Sample4,
                        EarthContinuity_Sample5 = x.EarthContinuity_Sample5,
                        EarthContinuity_Result = x.EarthContinuity_Result,

                        SELVProtection_Sample1 = x.SELVProtection_Sample1,
                        SELVProtection_Sample2 = x.SELVProtection_Sample2,
                        SELVProtection_Sample3 = x.SELVProtection_Sample3,
                        SELVProtection_Sample4 = x.SELVProtection_Sample4,
                        SELVProtection_Sample5 = x.SELVProtection_Sample5,
                        SELVProtection_Result = x.SELVProtection_Result,

                        LeakageCurrent_Sample1 = x.LeakageCurrent_Sample1,
                        LeakageCurrent_Sample2 = x.LeakageCurrent_Sample2,
                        LeakageCurrent_Sample3 = x.LeakageCurrent_Sample3,
                        LeakageCurrent_Sample4 = x.LeakageCurrent_Sample4,
                        LeakageCurrent_Sample5 = x.LeakageCurrent_Sample5,
                        LeakageCurrent_Result = x.LeakageCurrent_Result,

                        CreepageClearance_Sample1 = x.CreepageClearance_Sample1,
                        CreepageClearance_Sample2 = x.CreepageClearance_Sample2,
                        CreepageClearance_Sample3 = x.CreepageClearance_Sample3,
                        CreepageClearance_Sample4 = x.CreepageClearance_Sample4,
                        CreepageClearance_Sample5 = x.CreepageClearance_Sample5,
                        CreepageClearance_Result = x.CreepageClearance_Result,

                        HiPotMCPCB_Sample1 = x.HiPotMCPCB_Sample1,
                        HiPotMCPCB_Sample2 = x.HiPotMCPCB_Sample2,
                        HiPotMCPCB_Sample3 = x.HiPotMCPCB_Sample3,
                        HiPotMCPCB_Sample4 = x.HiPotMCPCB_Sample4,
                        HiPotMCPCB_Sample5 = x.HiPotMCPCB_Sample5,
                        HiPotMCPCB_Result = x.HiPotMCPCB_Result,

                        OnOffSwitching_Sample1 = x.OnOffSwitching_Sample1,
                        OnOffSwitching_Sample2 = x.OnOffSwitching_Sample2,
                        OnOffSwitching_Sample3 = x.OnOffSwitching_Sample3,
                        OnOffSwitching_Sample4 = x.OnOffSwitching_Sample4,
                        OnOffSwitching_Sample5 = x.OnOffSwitching_Sample5,
                        OnOffSwitching_Result = x.OnOffSwitching_Result,

                        SoakingAgeing_Sample1 = x.SoakingAgeing_Sample1,
                        SoakingAgeing_Sample2 = x.SoakingAgeing_Sample2,
                        SoakingAgeing_Sample3 = x.SoakingAgeing_Sample3,
                        SoakingAgeing_Sample4 = x.SoakingAgeing_Sample4,
                        SoakingAgeing_Sample5 = x.SoakingAgeing_Sample5,
                        SoakingAgeing_Result = x.SoakingAgeing_Result,

                        RollingEndurance_Sample1 = x.RollingEndurance_Sample1,
                        RollingEndurance_Sample2 = x.RollingEndurance_Sample2,
                        RollingEndurance_Sample3 = x.RollingEndurance_Sample3,
                        RollingEndurance_Sample4 = x.RollingEndurance_Sample4,
                        RollingEndurance_Sample5 = x.RollingEndurance_Sample5,
                        RollingEndurance_Result = x.RollingEndurance_Result,

                        GlowTest_Sample1 = x.GlowTest_Sample1,
                        GlowTest_Sample2 = x.GlowTest_Sample2,
                        GlowTest_Sample3 = x.GlowTest_Sample3,
                        GlowTest_Sample4 = x.GlowTest_Sample4,
                        GlowTest_Sample5 = x.GlowTest_Sample5,
                        GlowTest_Result = x.GlowTest_Result,

                        LampAccommodation_Sample1 = x.LampAccommodation_Sample1,
                        LampAccommodation_Sample2 = x.LampAccommodation_Sample2,
                        LampAccommodation_Sample3 = x.LampAccommodation_Sample3,
                        LampAccommodation_Sample4 = x.LampAccommodation_Sample4,
                        LampAccommodation_Sample5 = x.LampAccommodation_Sample5,
                        LampAccommodation_Result = x.LampAccommodation_Result,

                        DaliFunction_Sample1 = x.DaliFunction_Sample1,
                        DaliFunction_Sample2 = x.DaliFunction_Sample2,
                        DaliFunction_Sample3 = x.DaliFunction_Sample3,
                        DaliFunction_Sample4 = x.DaliFunction_Sample4,
                        DaliFunction_Sample5 = x.DaliFunction_Sample5,
                        DaliFunction_Result = x.DaliFunction_Result,

                        TuneableCCT_Sample1 = x.TuneableCCT_Sample1,
                        TuneableCCT_Sample2 = x.TuneableCCT_Sample2,
                        TuneableCCT_Sample3 = x.TuneableCCT_Sample3,
                        TuneableCCT_Sample4 = x.TuneableCCT_Sample4,
                        TuneableCCT_Sample5 = x.TuneableCCT_Sample5,
                        TuneableCCT_Result = x.TuneableCCT_Result,

                        BatteryBackup_Sample1 = x.BatteryBackup_Sample1,
                        BatteryBackup_Sample2 = x.BatteryBackup_Sample2,
                        BatteryBackup_Sample3 = x.BatteryBackup_Sample3,
                        BatteryBackup_Sample4 = x.BatteryBackup_Sample4,
                        BatteryBackup_Sample5 = x.BatteryBackup_Sample5,
                        BatteryBackup_Result = x.BatteryBackup_Result,

                        SmartLighting_Sample1 = x.SmartLighting_Sample1,
                        SmartLighting_Sample2 = x.SmartLighting_Sample2,
                        SmartLighting_Sample3 = x.SmartLighting_Sample3,
                        SmartLighting_Sample4 = x.SmartLighting_Sample4,
                        SmartLighting_Sample5 = x.SmartLighting_Sample5,
                        SmartLighting_Result = x.SmartLighting_Result,

                        SensorFunction_Sample1 = x.SensorFunction_Sample1,
                        SensorFunction_Sample2 = x.SensorFunction_Sample2,
                        SensorFunction_Sample3 = x.SensorFunction_Sample3,
                        SensorFunction_Sample4 = x.SensorFunction_Sample4,
                        SensorFunction_Sample5 = x.SensorFunction_Sample5,
                        SensorFunction_Result = x.SensorFunction_Result,

                        OverallReportResult = x.OverallReportResult,
                        TestedByName = x.TestedByName,
                        VerifiedByName = x.VerifiedByName,
                        TestedBySignature =x.TestedBySignature,
                        VerifiedBySignature =x.VerifiedBySignature,

                        AddedBy = x.AddedBy,
                        AddedOn = x.AddedOn,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedOn = x.UpdatedOn
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
        public async Task<OperationResult> InsertElectricalProtectionAsync(ElectricalProtectionViewModel model)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
            new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
            new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
            new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
            new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
            new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
            new SqlParameter("@LightSourceDetails", model.LightSourceDetails ?? (object)DBNull.Value),
            new SqlParameter("@DriverDetailsQty", model.DriverDetailsQty ?? (object)DBNull.Value),
            new SqlParameter("@PCBDetailsQty", model.PCBDetailsQty ?? (object)DBNull.Value),
            new SqlParameter("@LEDCombinations", model.LEDCombinations ?? (object)DBNull.Value),
            new SqlParameter("@SensorDetailsQty", model.SensorDetailsQty ?? (object)DBNull.Value),
            new SqlParameter("@LampDetails", model.LampDetails ?? (object)DBNull.Value),

            // ===== Electrical Protection Test =====
            new SqlParameter("@UnderVoltage_Sample1", model.UnderVoltage_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@UnderVoltage_Sample2", model.UnderVoltage_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@UnderVoltage_Sample3", model.UnderVoltage_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@UnderVoltage_Sample4", model.UnderVoltage_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@UnderVoltage_Sample5", model.UnderVoltage_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@UnderVoltage_Result", model.UnderVoltage_Result ?? (object)DBNull.Value),

            new SqlParameter("@OverVoltage_Sample1", model.OverVoltage_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@OverVoltage_Sample2", model.OverVoltage_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@OverVoltage_Sample3", model.OverVoltage_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@OverVoltage_Sample4", model.OverVoltage_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@OverVoltage_Sample5", model.OverVoltage_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@OverVoltage_Result", model.OverVoltage_Result ?? (object)DBNull.Value),

            new SqlParameter("@OpenCircuit_Sample1", model.OpenCircuit_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@OpenCircuit_Sample2", model.OpenCircuit_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@OpenCircuit_Sample3", model.OpenCircuit_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@OpenCircuit_Sample4", model.OpenCircuit_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@OpenCircuit_Sample5", model.OpenCircuit_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@OpenCircuit_Result", model.OpenCircuit_Result ?? (object)DBNull.Value),

            new SqlParameter("@ShortCircuit_Sample1", model.ShortCircuit_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@ShortCircuit_Sample2", model.ShortCircuit_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@ShortCircuit_Sample3", model.ShortCircuit_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@ShortCircuit_Sample4", model.ShortCircuit_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@ShortCircuit_Sample5", model.ShortCircuit_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@ShortCircuit_Result", model.ShortCircuit_Result ?? (object)DBNull.Value),

            new SqlParameter("@ReversePolarity_Sample1", model.ReversePolarity_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@ReversePolarity_Sample2", model.ReversePolarity_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@ReversePolarity_Sample3", model.ReversePolarity_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@ReversePolarity_Sample4", model.ReversePolarity_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@ReversePolarity_Sample5", model.ReversePolarity_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@ReversePolarity_Result", model.ReversePolarity_Result ?? (object)DBNull.Value),

            new SqlParameter("@OverLoad_Sample1", model.OverLoad_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@OverLoad_Sample2", model.OverLoad_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@OverLoad_Sample3", model.OverLoad_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@OverLoad_Sample4", model.OverLoad_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@OverLoad_Sample5", model.OverLoad_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@OverLoad_Result", model.OverLoad_Result ?? (object)DBNull.Value),

            new SqlParameter("@OverThermal_Sample1", model.OverThermal_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@OverThermal_Sample2", model.OverThermal_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@OverThermal_Sample3", model.OverThermal_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@OverThermal_Sample4", model.OverThermal_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@OverThermal_Sample5", model.OverThermal_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@OverThermal_Result", model.OverThermal_Result ?? (object)DBNull.Value),

            new SqlParameter("@EarthFault_Sample1", model.EarthFault_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@EarthFault_Sample2", model.EarthFault_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@EarthFault_Sample3", model.EarthFault_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@EarthFault_Sample4", model.EarthFault_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@EarthFault_Sample5", model.EarthFault_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@EarthFault_Result", model.EarthFault_Result ?? (object)DBNull.Value),

            new SqlParameter("@DriverIsolation_Sample1", model.DriverIsolation_Sample1 ?? (object)DBNull.Value),
            new SqlParameter("@DriverIsolation_Sample2", model.DriverIsolation_Sample2 ?? (object)DBNull.Value),
            new SqlParameter("@DriverIsolation_Sample3", model.DriverIsolation_Sample3 ?? (object)DBNull.Value),
            new SqlParameter("@DriverIsolation_Sample4", model.DriverIsolation_Sample4 ?? (object)DBNull.Value),
            new SqlParameter("@DriverIsolation_Sample5", model.DriverIsolation_Sample5 ?? (object)DBNull.Value),
            new SqlParameter("@DriverIsolation_Result", model.DriverIsolation_Result ?? (object)DBNull.Value),

    new SqlParameter("@HighVoltage_Sample1", model.HighVoltage_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@HighVoltage_Sample2", model.HighVoltage_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@HighVoltage_Sample3", model.HighVoltage_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@HighVoltage_Sample4", model.HighVoltage_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@HighVoltage_Sample5", model.HighVoltage_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@HighVoltage_Result", model.HighVoltage_Result ?? (object)DBNull.Value),

            // High Voltage Test
new SqlParameter("@HV_Sample1", model.HV_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@HV_Sample2", model.HV_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@HV_Sample3", model.HV_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@HV_Sample4", model.HV_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@HV_Sample5", model.HV_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@HV_Result", model.HV_Result ?? (object)DBNull.Value),

// Insulation Resistance Test
new SqlParameter("@InsulationResistance_Sample1", model.InsulationResistance_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@InsulationResistance_Sample2", model.InsulationResistance_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@InsulationResistance_Sample3", model.InsulationResistance_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@InsulationResistance_Sample4", model.InsulationResistance_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@InsulationResistance_Sample5", model.InsulationResistance_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@InsulationResistance_Result", model.InsulationResistance_Result ?? (object)DBNull.Value),

// Earth Continuity Test
new SqlParameter("@EarthContinuity_Sample1", model.EarthContinuity_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@EarthContinuity_Sample2", model.EarthContinuity_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@EarthContinuity_Sample3", model.EarthContinuity_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@EarthContinuity_Sample4", model.EarthContinuity_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@EarthContinuity_Sample5", model.EarthContinuity_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@EarthContinuity_Result", model.EarthContinuity_Result ?? (object)DBNull.Value),

// SELV Protection Test
new SqlParameter("@SELVProtection_Sample1", model.SELVProtection_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@SELVProtection_Sample2", model.SELVProtection_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@SELVProtection_Sample3", model.SELVProtection_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@SELVProtection_Sample4", model.SELVProtection_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@SELVProtection_Sample5", model.SELVProtection_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@SELVProtection_Result", model.SELVProtection_Result ?? (object)DBNull.Value),

// Leakage Current Test
new SqlParameter("@LeakageCurrent_Sample1", model.LeakageCurrent_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@LeakageCurrent_Sample2", model.LeakageCurrent_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@LeakageCurrent_Sample3", model.LeakageCurrent_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@LeakageCurrent_Sample4", model.LeakageCurrent_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@LeakageCurrent_Sample5", model.LeakageCurrent_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@LeakageCurrent_Result", model.LeakageCurrent_Result ?? (object)DBNull.Value),

// Creepage/Clearance Test
new SqlParameter("@CreepageClearance_Sample1", model.CreepageClearance_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@CreepageClearance_Sample2", model.CreepageClearance_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@CreepageClearance_Sample3", model.CreepageClearance_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@CreepageClearance_Sample4", model.CreepageClearance_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@CreepageClearance_Sample5", model.CreepageClearance_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@CreepageClearance_Result", model.CreepageClearance_Result ?? (object)DBNull.Value),

// HiPot MCPCB Test
new SqlParameter("@HiPotMCPCB_Sample1", model.HiPotMCPCB_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@HiPotMCPCB_Sample2", model.HiPotMCPCB_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@HiPotMCPCB_Sample3", model.HiPotMCPCB_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@HiPotMCPCB_Sample4", model.HiPotMCPCB_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@HiPotMCPCB_Sample5", model.HiPotMCPCB_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@HiPotMCPCB_Result", model.HiPotMCPCB_Result ?? (object)DBNull.Value),

// On/Off Switching Test
new SqlParameter("@OnOffSwitching_Sample1", model.OnOffSwitching_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@OnOffSwitching_Sample2", model.OnOffSwitching_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@OnOffSwitching_Sample3", model.OnOffSwitching_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@OnOffSwitching_Sample4", model.OnOffSwitching_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@OnOffSwitching_Sample5", model.OnOffSwitching_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@OnOffSwitching_Result", model.OnOffSwitching_Result ?? (object)DBNull.Value),

// Soaking / Ageing Test
new SqlParameter("@SoakingAgeing_Sample1", model.SoakingAgeing_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@SoakingAgeing_Sample2", model.SoakingAgeing_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@SoakingAgeing_Sample3", model.SoakingAgeing_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@SoakingAgeing_Sample4", model.SoakingAgeing_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@SoakingAgeing_Sample5", model.SoakingAgeing_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@SoakingAgeing_Result", model.SoakingAgeing_Result ?? (object)DBNull.Value),

// Rolling Endurance Test
new SqlParameter("@RollingEndurance_Sample1", model.RollingEndurance_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@RollingEndurance_Sample2", model.RollingEndurance_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@RollingEndurance_Sample3", model.RollingEndurance_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@RollingEndurance_Sample4", model.RollingEndurance_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@RollingEndurance_Sample5", model.RollingEndurance_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@RollingEndurance_Result", model.RollingEndurance_Result ?? (object)DBNull.Value),

// Glow test
new SqlParameter("@GlowTest_Sample1", model.GlowTest_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@GlowTest_Sample2", model.GlowTest_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@GlowTest_Sample3", model.GlowTest_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@GlowTest_Sample4", model.GlowTest_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@GlowTest_Sample5", model.GlowTest_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@GlowTest_Result", model.GlowTest_Result ?? (object)DBNull.Value),

// Lamp Accommodation Test
new SqlParameter("@LampAccommodation_Sample1", model.LampAccommodation_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@LampAccommodation_Sample2", model.LampAccommodation_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@LampAccommodation_Sample3", model.LampAccommodation_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@LampAccommodation_Sample4", model.LampAccommodation_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@LampAccommodation_Sample5", model.LampAccommodation_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@LampAccommodation_Result", model.LampAccommodation_Result ?? (object)DBNull.Value),

// Dali Function
new SqlParameter("@DaliFunction_Sample1", model.DaliFunction_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@DaliFunction_Sample2", model.DaliFunction_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@DaliFunction_Sample3", model.DaliFunction_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@DaliFunction_Sample4", model.DaliFunction_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@DaliFunction_Sample5", model.DaliFunction_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@DaliFunction_Result", model.DaliFunction_Result ?? (object)DBNull.Value),

// Tuneable CCT
new SqlParameter("@TuneableCCT_Sample1", model.TuneableCCT_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@TuneableCCT_Sample2", model.TuneableCCT_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@TuneableCCT_Sample3", model.TuneableCCT_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@TuneableCCT_Sample4", model.TuneableCCT_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@TuneableCCT_Sample5", model.TuneableCCT_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@TuneableCCT_Result", model.TuneableCCT_Result ?? (object)DBNull.Value),

// Battery Backup
new SqlParameter("@BatteryBackup_Sample1", model.BatteryBackup_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@BatteryBackup_Sample2", model.BatteryBackup_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@BatteryBackup_Sample3", model.BatteryBackup_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@BatteryBackup_Sample4", model.BatteryBackup_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@BatteryBackup_Sample5", model.BatteryBackup_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@BatteryBackup_Result", model.BatteryBackup_Result ?? (object)DBNull.Value),

// Smart Lighting
new SqlParameter("@SmartLighting_Sample1", model.SmartLighting_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@SmartLighting_Sample2", model.SmartLighting_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@SmartLighting_Sample3", model.SmartLighting_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@SmartLighting_Sample4", model.SmartLighting_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@SmartLighting_Sample5", model.SmartLighting_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@SmartLighting_Result", model.SmartLighting_Result ?? (object)DBNull.Value),

// Sensor Function
new SqlParameter("@SensorFunction_Sample1", model.SensorFunction_Sample1 ?? (object)DBNull.Value),
new SqlParameter("@SensorFunction_Sample2", model.SensorFunction_Sample2 ?? (object)DBNull.Value),
new SqlParameter("@SensorFunction_Sample3", model.SensorFunction_Sample3 ?? (object)DBNull.Value),
new SqlParameter("@SensorFunction_Sample4", model.SensorFunction_Sample4 ?? (object)DBNull.Value),
new SqlParameter("@SensorFunction_Sample5", model.SensorFunction_Sample5 ?? (object)DBNull.Value),
new SqlParameter("@SensorFunction_Result", model.SensorFunction_Result ?? (object)DBNull.Value),


            new SqlParameter("@OverallReportResult", model.OverallReportResult ?? (object)DBNull.Value),
            new SqlParameter("@TestedByName", model.TestedByName ?? (object)DBNull.Value),
            new SqlParameter("@VerifiedByName", model.VerifiedByName ?? (object)DBNull.Value),
            new SqlParameter("@TestedBySignature", model.TestedBySignature ?? (object)DBNull.Value),
            new SqlParameter("@VerifiedBySignature", model.VerifiedBySignature ?? (object)DBNull.Value),
            new SqlParameter("@AddedBy", model.AddedBy),
            new SqlParameter("@AddedOn", model.AddedOn)
        };

                await _dbContext.Database.ExecuteSqlRawAsync("EXEC sp_Insert_ElectricalProtection " +
                 "@ProductCatRef, @ProductDescription, @ReportNo, @ReportDate, @BatchCode, @PKD, " +
"@LightSourceDetails, @DriverDetailsQty, @PCBDetailsQty, @LEDCombinations, @SensorDetailsQty, @LampDetails, @UnderVoltage_Sample1, @UnderVoltage_Sample2, @UnderVoltage_Sample3, @UnderVoltage_Sample4, @UnderVoltage_Sample5, @UnderVoltage_Result, " +
"@OverVoltage_Sample1, @OverVoltage_Sample2, @OverVoltage_Sample3, @OverVoltage_Sample4, @OverVoltage_Sample5, @OverVoltage_Result, " +
"@OpenCircuit_Sample1, @OpenCircuit_Sample2, @OpenCircuit_Sample3, @OpenCircuit_Sample4, @OpenCircuit_Sample5, @OpenCircuit_Result, " +
"@ShortCircuit_Sample1, @ShortCircuit_Sample2, @ShortCircuit_Sample3, @ShortCircuit_Sample4, @ShortCircuit_Sample5, @ShortCircuit_Result, " +
"@ReversePolarity_Sample1, @ReversePolarity_Sample2, @ReversePolarity_Sample3, @ReversePolarity_Sample4, @ReversePolarity_Sample5, @ReversePolarity_Result, " +
"@OverLoad_Sample1, @OverLoad_Sample2, @OverLoad_Sample3, @OverLoad_Sample4, @OverLoad_Sample5, @OverLoad_Result, " +
"@OverThermal_Sample1, @OverThermal_Sample2, @OverThermal_Sample3, @OverThermal_Sample4, @OverThermal_Sample5, @OverThermal_Result, " +
"@EarthFault_Sample1, @EarthFault_Sample2, @EarthFault_Sample3, @EarthFault_Sample4, @EarthFault_Sample5, @EarthFault_Result, " +
"@DriverIsolation_Sample1, @DriverIsolation_Sample2, @DriverIsolation_Sample3, @DriverIsolation_Sample4, @DriverIsolation_Sample5, @DriverIsolation_Result, " +
"@HighVoltage_Sample1, @HighVoltage_Sample2,@HighVoltage_Sample3,@HighVoltage_Sample4,@HighVoltage_Sample5,@HighVoltage_Result,@HV_Sample1, @HV_Sample2, @HV_Sample3, @HV_Sample4, @HV_Sample5, @HV_Result,@InsulationResistance_Sample1, @InsulationResistance_Sample2, @InsulationResistance_Sample3, @InsulationResistance_Sample4, @InsulationResistance_Sample5, @InsulationResistance_Result, " +
"@EarthContinuity_Sample1, @EarthContinuity_Sample2, @EarthContinuity_Sample3, @EarthContinuity_Sample4, @EarthContinuity_Sample5, @EarthContinuity_Result, " +
"@SELVProtection_Sample1, @SELVProtection_Sample2, @SELVProtection_Sample3, @SELVProtection_Sample4, @SELVProtection_Sample5, @SELVProtection_Result, " +
"@LeakageCurrent_Sample1, @LeakageCurrent_Sample2, @LeakageCurrent_Sample3, @LeakageCurrent_Sample4, @LeakageCurrent_Sample5, @LeakageCurrent_Result, " +
"@CreepageClearance_Sample1, @CreepageClearance_Sample2, @CreepageClearance_Sample3, @CreepageClearance_Sample4, @CreepageClearance_Sample5, @CreepageClearance_Result, " +
"@HiPotMCPCB_Sample1, @HiPotMCPCB_Sample2, @HiPotMCPCB_Sample3, @HiPotMCPCB_Sample4, @HiPotMCPCB_Sample5, @HiPotMCPCB_Result, " +
"@OnOffSwitching_Sample1, @OnOffSwitching_Sample2, @OnOffSwitching_Sample3, @OnOffSwitching_Sample4, @OnOffSwitching_Sample5, @OnOffSwitching_Result, " +
"@SoakingAgeing_Sample1, @SoakingAgeing_Sample2, @SoakingAgeing_Sample3, @SoakingAgeing_Sample4, @SoakingAgeing_Sample5, @SoakingAgeing_Result, " +
"@RollingEndurance_Sample1, @RollingEndurance_Sample2, @RollingEndurance_Sample3, @RollingEndurance_Sample4, @RollingEndurance_Sample5, @RollingEndurance_Result, " +
"@GlowTest_Sample1, @GlowTest_Sample2, @GlowTest_Sample3, @GlowTest_Sample4, @GlowTest_Sample5, @GlowTest_Result, " +
"@LampAccommodation_Sample1, @LampAccommodation_Sample2, @LampAccommodation_Sample3, @LampAccommodation_Sample4, @LampAccommodation_Sample5, @LampAccommodation_Result, " +
"@DaliFunction_Sample1, @DaliFunction_Sample2, @DaliFunction_Sample3, @DaliFunction_Sample4, @DaliFunction_Sample5, @DaliFunction_Result, " +
"@TuneableCCT_Sample1, @TuneableCCT_Sample2, @TuneableCCT_Sample3, @TuneableCCT_Sample4, @TuneableCCT_Sample5, @TuneableCCT_Result, " +
"@BatteryBackup_Sample1, @BatteryBackup_Sample2, @BatteryBackup_Sample3, @BatteryBackup_Sample4, @BatteryBackup_Sample5, @BatteryBackup_Result, " +
"@SmartLighting_Sample1, @SmartLighting_Sample2, @SmartLighting_Sample3, @SmartLighting_Sample4, @SmartLighting_Sample5, @SmartLighting_Result, " +
"@SensorFunction_Sample1, @SensorFunction_Sample2, @SensorFunction_Sample3, @SensorFunction_Sample4, @SensorFunction_Sample5, @SensorFunction_Result, " +
"@OverallReportResult, @TestedByName, @VerifiedByName,@TestedBySignature,@VerifiedBySignature, @AddedBy, @AddedOn",

                    parameters) ;

                return new OperationResult { Success = true, Message = "Record inserted successfully." };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

            public async Task<OperationResult> UpdateElectricalProtectionAsync(ElectricalProtectionViewModel model)
            {
                try
                {
                    var parameters = new[]
                    {
                new SqlParameter("@Id", model.Id),

                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
                new SqlParameter("@LightSourceDetails", model.LightSourceDetails ?? (object)DBNull.Value),
                new SqlParameter("@DriverDetailsQty", model.DriverDetailsQty ?? (object)DBNull.Value),
                new SqlParameter("@PCBDetailsQty", model.PCBDetailsQty ?? (object)DBNull.Value),
                new SqlParameter("@LEDCombinations", model.LEDCombinations ?? (object)DBNull.Value),
                new SqlParameter("@SensorDetailsQty", model.SensorDetailsQty ?? (object)DBNull.Value),
                new SqlParameter("@LampDetails", model.LampDetails ?? (object)DBNull.Value),

                // ===== Electrical Protection Test =====
                new SqlParameter("@UnderVoltage_Sample1", model.UnderVoltage_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@UnderVoltage_Sample2", model.UnderVoltage_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@UnderVoltage_Sample3", model.UnderVoltage_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@UnderVoltage_Sample4", model.UnderVoltage_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@UnderVoltage_Sample5", model.UnderVoltage_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@UnderVoltage_Result", model.UnderVoltage_Result ?? (object)DBNull.Value),

                new SqlParameter("@OverVoltage_Sample1", model.OverVoltage_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@OverVoltage_Sample2", model.OverVoltage_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@OverVoltage_Sample3", model.OverVoltage_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@OverVoltage_Sample4", model.OverVoltage_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@OverVoltage_Sample5", model.OverVoltage_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@OverVoltage_Result", model.OverVoltage_Result ?? (object)DBNull.Value),

                new SqlParameter("@OpenCircuit_Sample1", model.OpenCircuit_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@OpenCircuit_Sample2", model.OpenCircuit_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@OpenCircuit_Sample3", model.OpenCircuit_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@OpenCircuit_Sample4", model.OpenCircuit_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@OpenCircuit_Sample5", model.OpenCircuit_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@OpenCircuit_Result", model.OpenCircuit_Result ?? (object)DBNull.Value),

                new SqlParameter("@ShortCircuit_Sample1", model.ShortCircuit_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@ShortCircuit_Sample2", model.ShortCircuit_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@ShortCircuit_Sample3", model.ShortCircuit_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@ShortCircuit_Sample4", model.ShortCircuit_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@ShortCircuit_Sample5", model.ShortCircuit_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@ShortCircuit_Result", model.ShortCircuit_Result ?? (object)DBNull.Value),

                new SqlParameter("@ReversePolarity_Sample1", model.ReversePolarity_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@ReversePolarity_Sample2", model.ReversePolarity_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@ReversePolarity_Sample3", model.ReversePolarity_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@ReversePolarity_Sample4", model.ReversePolarity_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@ReversePolarity_Sample5", model.ReversePolarity_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@ReversePolarity_Result", model.ReversePolarity_Result ?? (object)DBNull.Value),

                new SqlParameter("@OverLoad_Sample1", model.OverLoad_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@OverLoad_Sample2", model.OverLoad_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@OverLoad_Sample3", model.OverLoad_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@OverLoad_Sample4", model.OverLoad_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@OverLoad_Sample5", model.OverLoad_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@OverLoad_Result", model.OverLoad_Result ?? (object)DBNull.Value),

                new SqlParameter("@OverThermal_Sample1", model.OverThermal_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@OverThermal_Sample2", model.OverThermal_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@OverThermal_Sample3", model.OverThermal_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@OverThermal_Sample4", model.OverThermal_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@OverThermal_Sample5", model.OverThermal_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@OverThermal_Result", model.OverThermal_Result ?? (object)DBNull.Value),

                new SqlParameter("@EarthFault_Sample1", model.EarthFault_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@EarthFault_Sample2", model.EarthFault_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@EarthFault_Sample3", model.EarthFault_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@EarthFault_Sample4", model.EarthFault_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@EarthFault_Sample5", model.EarthFault_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@EarthFault_Result", model.EarthFault_Result ?? (object)DBNull.Value),

                new SqlParameter("@DriverIsolation_Sample1", model.DriverIsolation_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@DriverIsolation_Sample2", model.DriverIsolation_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@DriverIsolation_Sample3", model.DriverIsolation_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@DriverIsolation_Sample4", model.DriverIsolation_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@DriverIsolation_Sample5", model.DriverIsolation_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@DriverIsolation_Result", model.DriverIsolation_Result ?? (object)DBNull.Value),

                new SqlParameter("@HighVoltage_Sample1", model.HighVoltage_Sample1 ?? (object)DBNull.Value),
                new SqlParameter("@HighVoltage_Sample2", model.HighVoltage_Sample2 ?? (object)DBNull.Value),
                new SqlParameter("@HighVoltage_Sample3", model.HighVoltage_Sample3 ?? (object)DBNull.Value),
                new SqlParameter("@HighVoltage_Sample4", model.HighVoltage_Sample4 ?? (object)DBNull.Value),
                new SqlParameter("@HighVoltage_Sample5", model.HighVoltage_Sample5 ?? (object)DBNull.Value),
                new SqlParameter("@HighVoltage_Result", model.HighVoltage_Result ?? (object)DBNull.Value),
                new SqlParameter("@HV_Sample1", model.HV_Sample1 ?? (object)DBNull.Value),

    new SqlParameter("@HV_Sample2", model.HV_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@HV_Sample3", model.HV_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@HV_Sample4", model.HV_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@HV_Sample5", model.HV_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@HV_Result", model.HV_Result ?? (object)DBNull.Value),

    // Insulation Resistance Test
    new SqlParameter("@InsulationResistance_Sample1", model.InsulationResistance_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@InsulationResistance_Sample2", model.InsulationResistance_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@InsulationResistance_Sample3", model.InsulationResistance_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@InsulationResistance_Sample4", model.InsulationResistance_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@InsulationResistance_Sample5", model.InsulationResistance_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@InsulationResistance_Result", model.InsulationResistance_Result ?? (object)DBNull.Value),

    // Earth Continuity Test
    new SqlParameter("@EarthContinuity_Sample1", model.EarthContinuity_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@EarthContinuity_Sample2", model.EarthContinuity_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@EarthContinuity_Sample3", model.EarthContinuity_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@EarthContinuity_Sample4", model.EarthContinuity_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@EarthContinuity_Sample5", model.EarthContinuity_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@EarthContinuity_Result", model.EarthContinuity_Result ?? (object)DBNull.Value),

    // SELV Protection Test
    new SqlParameter("@SELVProtection_Sample1", model.SELVProtection_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@SELVProtection_Sample2", model.SELVProtection_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@SELVProtection_Sample3", model.SELVProtection_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@SELVProtection_Sample4", model.SELVProtection_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@SELVProtection_Sample5", model.SELVProtection_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@SELVProtection_Result", model.SELVProtection_Result ?? (object)DBNull.Value),

    // Leakage Current Test
    new SqlParameter("@LeakageCurrent_Sample1", model.LeakageCurrent_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@LeakageCurrent_Sample2", model.LeakageCurrent_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@LeakageCurrent_Sample3", model.LeakageCurrent_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@LeakageCurrent_Sample4", model.LeakageCurrent_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@LeakageCurrent_Sample5", model.LeakageCurrent_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@LeakageCurrent_Result", model.LeakageCurrent_Result ?? (object)DBNull.Value),

    // Creepage/Clearance Test
    new SqlParameter("@CreepageClearance_Sample1", model.CreepageClearance_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@CreepageClearance_Sample2", model.CreepageClearance_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@CreepageClearance_Sample3", model.CreepageClearance_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@CreepageClearance_Sample4", model.CreepageClearance_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@CreepageClearance_Sample5", model.CreepageClearance_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@CreepageClearance_Result", model.CreepageClearance_Result ?? (object)DBNull.Value),

    // HiPot MCPCB Test
    new SqlParameter("@HiPotMCPCB_Sample1", model.HiPotMCPCB_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@HiPotMCPCB_Sample2", model.HiPotMCPCB_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@HiPotMCPCB_Sample3", model.HiPotMCPCB_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@HiPotMCPCB_Sample4", model.HiPotMCPCB_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@HiPotMCPCB_Sample5", model.HiPotMCPCB_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@HiPotMCPCB_Result", model.HiPotMCPCB_Result ?? (object)DBNull.Value),

    // On/Off Switching Test
    new SqlParameter("@OnOffSwitching_Sample1", model.OnOffSwitching_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@OnOffSwitching_Sample2", model.OnOffSwitching_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@OnOffSwitching_Sample3", model.OnOffSwitching_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@OnOffSwitching_Sample4", model.OnOffSwitching_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@OnOffSwitching_Sample5", model.OnOffSwitching_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@OnOffSwitching_Result", model.OnOffSwitching_Result ?? (object)DBNull.Value),

    // Soaking / Ageing Test
    new SqlParameter("@SoakingAgeing_Sample1", model.SoakingAgeing_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@SoakingAgeing_Sample2", model.SoakingAgeing_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@SoakingAgeing_Sample3", model.SoakingAgeing_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@SoakingAgeing_Sample4", model.SoakingAgeing_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@SoakingAgeing_Sample5", model.SoakingAgeing_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@SoakingAgeing_Result", model.SoakingAgeing_Result ?? (object)DBNull.Value),

    // Rolling Endurance Test
    new SqlParameter("@RollingEndurance_Sample1", model.RollingEndurance_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@RollingEndurance_Sample2", model.RollingEndurance_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@RollingEndurance_Sample3", model.RollingEndurance_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@RollingEndurance_Sample4", model.RollingEndurance_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@RollingEndurance_Sample5", model.RollingEndurance_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@RollingEndurance_Result", model.RollingEndurance_Result ?? (object)DBNull.Value),

    // Glow test
    new SqlParameter("@GlowTest_Sample1", model.GlowTest_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@GlowTest_Sample2", model.GlowTest_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@GlowTest_Sample3", model.GlowTest_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@GlowTest_Sample4", model.GlowTest_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@GlowTest_Sample5", model.GlowTest_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@GlowTest_Result", model.GlowTest_Result ?? (object)DBNull.Value),

    // Lamp Accommodation Test
    new SqlParameter("@LampAccommodation_Sample1", model.LampAccommodation_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@LampAccommodation_Sample2", model.LampAccommodation_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@LampAccommodation_Sample3", model.LampAccommodation_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@LampAccommodation_Sample4", model.LampAccommodation_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@LampAccommodation_Sample5", model.LampAccommodation_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@LampAccommodation_Result", model.LampAccommodation_Result ?? (object)DBNull.Value),

    // Dali Function
    new SqlParameter("@DaliFunction_Sample1", model.DaliFunction_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@DaliFunction_Sample2", model.DaliFunction_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@DaliFunction_Sample3", model.DaliFunction_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@DaliFunction_Sample4", model.DaliFunction_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@DaliFunction_Sample5", model.DaliFunction_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@DaliFunction_Result", model.DaliFunction_Result ?? (object)DBNull.Value),

    // Tuneable CCT
    new SqlParameter("@TuneableCCT_Sample1", model.TuneableCCT_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@TuneableCCT_Sample2", model.TuneableCCT_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@TuneableCCT_Sample3", model.TuneableCCT_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@TuneableCCT_Sample4", model.TuneableCCT_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@TuneableCCT_Sample5", model.TuneableCCT_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@TuneableCCT_Result", model.TuneableCCT_Result ?? (object)DBNull.Value),

    // Battery Backup
    new SqlParameter("@BatteryBackup_Sample1", model.BatteryBackup_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@BatteryBackup_Sample2", model.BatteryBackup_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@BatteryBackup_Sample3", model.BatteryBackup_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@BatteryBackup_Sample4", model.BatteryBackup_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@BatteryBackup_Sample5", model.BatteryBackup_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@BatteryBackup_Result", model.BatteryBackup_Result ?? (object)DBNull.Value),

    // Smart Lighting
    new SqlParameter("@SmartLighting_Sample1", model.SmartLighting_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@SmartLighting_Sample2", model.SmartLighting_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@SmartLighting_Sample3", model.SmartLighting_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@SmartLighting_Sample4", model.SmartLighting_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@SmartLighting_Sample5", model.SmartLighting_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@SmartLighting_Result", model.SmartLighting_Result ?? (object)DBNull.Value),

    // Sensor Function
    new SqlParameter("@SensorFunction_Sample1", model.SensorFunction_Sample1 ?? (object)DBNull.Value),
    new SqlParameter("@SensorFunction_Sample2", model.SensorFunction_Sample2 ?? (object)DBNull.Value),
    new SqlParameter("@SensorFunction_Sample3", model.SensorFunction_Sample3 ?? (object)DBNull.Value),
    new SqlParameter("@SensorFunction_Sample4", model.SensorFunction_Sample4 ?? (object)DBNull.Value),
    new SqlParameter("@SensorFunction_Sample5", model.SensorFunction_Sample5 ?? (object)DBNull.Value),
    new SqlParameter("@SensorFunction_Result", model.SensorFunction_Result ?? (object)DBNull.Value),

                new SqlParameter("@OverallReportResult", model.OverallReportResult ?? (object)DBNull.Value),
                new SqlParameter("@TestedByName", model.TestedByName ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedByName", model.VerifiedByName ?? (object)DBNull.Value),
                new SqlParameter("@TestedBySignature", model.TestedBySignature ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedBySignature", model.VerifiedBySignature ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy),
                new SqlParameter("@UpdatedOn", model.UpdatedOn),
            };

                await _dbContext.Database.ExecuteSqlRawAsync(
     "EXEC sp_Update_ElectricalProtection " +
     "@Id, @ProductCatRef, @ProductDescription, @ReportNo, @ReportDate, @BatchCode, @PKD, " +
     "@LightSourceDetails, @DriverDetailsQty, @PCBDetailsQty, @LEDCombinations, @SensorDetailsQty, @LampDetails, " +
     "@UnderVoltage_Sample1, @UnderVoltage_Sample2, @UnderVoltage_Sample3, @UnderVoltage_Sample4, @UnderVoltage_Sample5, @UnderVoltage_Result, " +
     "@OverVoltage_Sample1, @OverVoltage_Sample2, @OverVoltage_Sample3, @OverVoltage_Sample4, @OverVoltage_Sample5, @OverVoltage_Result, " +
     "@OpenCircuit_Sample1, @OpenCircuit_Sample2, @OpenCircuit_Sample3, @OpenCircuit_Sample4, @OpenCircuit_Sample5, @OpenCircuit_Result, " +
     "@ShortCircuit_Sample1, @ShortCircuit_Sample2, @ShortCircuit_Sample3, @ShortCircuit_Sample4, @ShortCircuit_Sample5, @ShortCircuit_Result, " +
     "@ReversePolarity_Sample1, @ReversePolarity_Sample2, @ReversePolarity_Sample3, @ReversePolarity_Sample4, @ReversePolarity_Sample5, @ReversePolarity_Result, " +
     "@OverLoad_Sample1, @OverLoad_Sample2, @OverLoad_Sample3, @OverLoad_Sample4, @OverLoad_Sample5, @OverLoad_Result, " +
     "@OverThermal_Sample1, @OverThermal_Sample2, @OverThermal_Sample3, @OverThermal_Sample4, @OverThermal_Sample5, @OverThermal_Result, " +
     "@EarthFault_Sample1, @EarthFault_Sample2, @EarthFault_Sample3, @EarthFault_Sample4, @EarthFault_Sample5, @EarthFault_Result, " +
     "@DriverIsolation_Sample1, @DriverIsolation_Sample2, @DriverIsolation_Sample3, @DriverIsolation_Sample4, @DriverIsolation_Sample5, @DriverIsolation_Result, " +
     "@HighVoltage_Sample1, @HighVoltage_Sample2, @HighVoltage_Sample3, @HighVoltage_Sample4, @HighVoltage_Sample5, @HighVoltage_Result, " +
     "@HV_Sample1, @HV_Sample2, @HV_Sample3, @HV_Sample4, @HV_Sample5, @HV_Result, " +
     "@InsulationResistance_Sample1, @InsulationResistance_Sample2, @InsulationResistance_Sample3, @InsulationResistance_Sample4, @InsulationResistance_Sample5, @InsulationResistance_Result, " +
     "@EarthContinuity_Sample1, @EarthContinuity_Sample2, @EarthContinuity_Sample3, @EarthContinuity_Sample4, @EarthContinuity_Sample5, @EarthContinuity_Result, " +
     "@SELVProtection_Sample1, @SELVProtection_Sample2, @SELVProtection_Sample3, @SELVProtection_Sample4, @SELVProtection_Sample5, @SELVProtection_Result, " +
     "@LeakageCurrent_Sample1, @LeakageCurrent_Sample2, @LeakageCurrent_Sample3, @LeakageCurrent_Sample4, @LeakageCurrent_Sample5, @LeakageCurrent_Result, " +
     "@CreepageClearance_Sample1, @CreepageClearance_Sample2, @CreepageClearance_Sample3, @CreepageClearance_Sample4, @CreepageClearance_Sample5, @CreepageClearance_Result, " +
     "@HiPotMCPCB_Sample1, @HiPotMCPCB_Sample2, @HiPotMCPCB_Sample3, @HiPotMCPCB_Sample4, @HiPotMCPCB_Sample5, @HiPotMCPCB_Result, " +
     "@OnOffSwitching_Sample1, @OnOffSwitching_Sample2, @OnOffSwitching_Sample3, @OnOffSwitching_Sample4, @OnOffSwitching_Sample5, @OnOffSwitching_Result, " +
     "@SoakingAgeing_Sample1, @SoakingAgeing_Sample2, @SoakingAgeing_Sample3, @SoakingAgeing_Sample4, @SoakingAgeing_Sample5, @SoakingAgeing_Result, " +
     "@RollingEndurance_Sample1, @RollingEndurance_Sample2, @RollingEndurance_Sample3, @RollingEndurance_Sample4, @RollingEndurance_Sample5, @RollingEndurance_Result, " +
     "@GlowTest_Sample1, @GlowTest_Sample2, @GlowTest_Sample3, @GlowTest_Sample4, @GlowTest_Sample5, @GlowTest_Result, " +
     "@LampAccommodation_Sample1, @LampAccommodation_Sample2, @LampAccommodation_Sample3, @LampAccommodation_Sample4, @LampAccommodation_Sample5, @LampAccommodation_Result, " +
     "@DaliFunction_Sample1, @DaliFunction_Sample2, @DaliFunction_Sample3, @DaliFunction_Sample4, @DaliFunction_Sample5, @DaliFunction_Result, " +
     "@TuneableCCT_Sample1, @TuneableCCT_Sample2, @TuneableCCT_Sample3, @TuneableCCT_Sample4, @TuneableCCT_Sample5, @TuneableCCT_Result, " +
     "@BatteryBackup_Sample1, @BatteryBackup_Sample2, @BatteryBackup_Sample3, @BatteryBackup_Sample4, @BatteryBackup_Sample5, @BatteryBackup_Result, " +
     "@SmartLighting_Sample1, @SmartLighting_Sample2, @SmartLighting_Sample3, @SmartLighting_Sample4, @SmartLighting_Sample5, @SmartLighting_Result, " +
     "@SensorFunction_Sample1, @SensorFunction_Sample2, @SensorFunction_Sample3, @SensorFunction_Sample4, @SensorFunction_Sample5, @SensorFunction_Result, " +
     "@OverallReportResult, @TestedByName, @VerifiedByName,@TestedBySignature,@VerifiedBySignature,@UpdatedBy, @UpdatedOn",
     parameters);


                return new OperationResult { Success = true, Message = "Record updated successfully." };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }



        public async Task<OperationResult> DeleteElectricalProtectionAsync(int id)
        {
            try
            {
                var result = await base.DeleteAsync<ElectricalProtection>(id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }





    }


    }
