using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Repositories.TemperatureRiseTestRepo;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories;

public class TemperatureRiseTestRepository : SqlTableRepository, ITemperatureRiseTestRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public TemperatureRiseTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<TemperatureRiseTestViewModel>> GetTemperatureRiseTestAsync()
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

            var sql = @"EXEC sp_Get_TemperatureRiseTest";

            var result = await Task.Run(() => _dbContext.TemperatureRiseTests.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new TemperatureRiseTestViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    ReportDate = x.ReportDate,
                    TestingLocation = x.TestingLocation,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    BatchCode = x.BatchCode,
                    PKD = x.PKD,
                    HeatSinkMaterial = x.HeatSinkMaterial,
                    HeatSinkWeight = x.HeatSinkWeight,
                    LensDetails = x.LensDetails,
                    ThermalPasteDetails = x.ThermalPasteDetails,
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

    public async Task<TemperatureRiseTestViewModel> GetTemperatureRiseTestByIdAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_TemperatureRiseTest";

            var result = await Task.Run(() => _dbContext.TemperatureRiseTests.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new TemperatureRiseTestViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    ReportDate = x.ReportDate,
                    TestingLocation = x.TestingLocation,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    BatchCode = x.BatchCode,
                    PKD = x.PKD,
                    HeatSinkMaterial = x.HeatSinkMaterial,
                    HeatSinkWeight = x.HeatSinkWeight,
                    LensDetails = x.LensDetails,
                    ThermalPasteDetails = x.ThermalPasteDetails,
                    NominalOperatingVoltage = x.NominalOperatingVoltage,
                    DriverUsed = x.DriverUsed,
                    NoOfDrivers = x.NoOfDrivers,
                    DriverOutputVoltage = x.DriverOutputVoltage,
                    DriverOutputCurrent = x.DriverOutputCurrent,
                    DriverAllowableTc = x.DriverAllowableTc,
                    DriverAllowableTa = x.DriverAllowableTa,
                    PcbMaterialMake = x.PcbMaterialMake,
                    PcbSizeQty = x.PcbSizeQty,
                    LedUsed = x.LedUsed,
                    NoOfLeds = x.NoOfLeds,
                    NoOfLedsInSeries = x.NoOfLedsInSeries,
                    NoOfLedsInParallel = x.NoOfLedsInParallel,
                    LedRjTH = x.LedRjTH,
                    LedVf = x.LedVf,
                    LedIf = x.LedIf,
                    LedWdc = x.LedWdc,
                    ProbeT1_Desc = x.ProbeT1_Desc,
                    ProbeT2_Desc = x.ProbeT2_Desc,
                    ProbeT3_Desc = x.ProbeT3_Desc,
                    ProbeT4_Desc = x.ProbeT4_Desc,
                    ProbeT5_Desc = x.ProbeT5_Desc,
                    ProbeT6_Desc = x.ProbeT6_Desc,
                    ProbeT7_Desc = x.ProbeT7_Desc,
                    ProbeT8_Desc = x.ProbeT8_Desc,
                    ProbeT9_Desc = x.ProbeT9_Desc,
                    ProbeT10_Desc = x.ProbeT10_Desc,
                    ProbeT11_Desc = x.ProbeT11_Desc,
                    ProbeT12_Desc = x.ProbeT12_Desc,
                    ProbeT13_Desc = x.ProbeT13_Desc,
                    ProbeT14_Desc = x.ProbeT14_Desc,
                    ProbeT15_Desc = x.ProbeT15_Desc,
                    ProbeT16_Desc = x.ProbeT16_Desc,
                    R1_TimeHrs = x.R1_TimeHrs,
                    R1_T1 = x.R1_T1,
                    R1_T2 = x.R1_T2,
                    R1_T3 = x.R1_T3,
                    R1_T4 = x.R1_T4,
                    R1_T5 = x.R1_T5,
                    R1_T6 = x.R1_T6,
                    R1_T7 = x.R1_T7,
                    R1_T8 = x.R1_T8,
                    R1_T9 = x.R1_T9,
                    R1_T10 = x.R1_T10,
                    R1_T11 = x.R1_T11,
                    R1_T12 = x.R1_T12,
                    R1_T13 = x.R1_T13,
                    R1_T14 = x.R1_T14,
                    R1_T15 = x.R1_T15,
                    R1_T16 = x.R1_T16,
                    R1_VIN = x.R1_VIN,
                    R1_IIN = x.R1_IIN,
                    R1_PIN = x.R1_PIN,
                    R1_TJ = x.R1_TJ,
                    R2_TimeHrs = x.R2_TimeHrs,
                    R2_T1 = x.R2_T1,
                    R2_T2 = x.R2_T2,
                    R2_T3 = x.R2_T3,
                    R2_T4 = x.R2_T4,
                    R2_T5 = x.R2_T5,
                    R2_T6 = x.R2_T6,
                    R2_T7 = x.R2_T7,
                    R2_T8 = x.R2_T8,
                    R2_T9 = x.R2_T9,
                    R2_T10 = x.R2_T10,
                    R2_T11 = x.R2_T11,
                    R2_T12 = x.R2_T12,
                    R2_T13 = x.R2_T13,
                    R2_T14 = x.R2_T14,
                    R2_T15 = x.R2_T15,
                    R2_T16 = x.R2_T16,
                    R2_VIN = x.R2_VIN,
                    R2_IIN = x.R2_IIN,
                    R2_PIN = x.R2_PIN,
                    R2_TJ = x.R2_TJ,
                    R3_TimeHrs = x.R3_TimeHrs,
                    R3_T1 = x.R3_T1,
                    R3_T2 = x.R3_T2,
                    R3_T3 = x.R3_T3,
                    R3_T4 = x.R3_T4,
                    R3_T5 = x.R3_T5,
                    R3_T6 = x.R3_T6,
                    R3_T7 = x.R3_T7,
                    R3_T8 = x.R3_T8,
                    R3_T9 = x.R3_T9,
                    R3_T10 = x.R3_T10,
                    R3_T11 = x.R3_T11,
                    R3_T12 = x.R3_T12,
                    R3_T13 = x.R3_T13,
                    R3_T14 = x.R3_T14,
                    R3_T15 = x.R3_T15,
                    R3_T16 = x.R3_T16,
                    R3_VIN = x.R3_VIN,
                    R3_IIN = x.R3_IIN,
                    R3_PIN = x.R3_PIN,
                    R3_TJ = x.R3_TJ,
                    R4_TimeHrs = x.R4_TimeHrs,
                    R4_T1 = x.R4_T1,
                    R4_T2 = x.R4_T2,
                    R4_T3 = x.R4_T3,
                    R4_T4 = x.R4_T4,
                    R4_T5 = x.R4_T5,
                    R4_T6 = x.R4_T6,
                    R4_T7 = x.R4_T7,
                    R4_T8 = x.R4_T8,
                    R4_T9 = x.R4_T9,
                    R4_T10 = x.R4_T10,
                    R4_T11 = x.R4_T11,
                    R4_T12 = x.R4_T12,
                    R4_T13 = x.R4_T13,
                    R4_T14 = x.R4_T14,
                    R4_T15 = x.R4_T15,
                    R4_T16 = x.R4_T16,
                    R4_VIN = x.R4_VIN,
                    R4_IIN = x.R4_IIN,
                    R4_PIN = x.R4_PIN,
                    R4_TJ = x.R4_TJ,
                    R5_TimeHrs = x.R5_TimeHrs,
                    R5_T1 = x.R5_T1,
                    R5_T2 = x.R5_T2,
                    R5_T3 = x.R5_T3,
                    R5_T4 = x.R5_T4,
                    R5_T5 = x.R5_T5,
                    R5_T6 = x.R5_T6,
                    R5_T7 = x.R5_T7,
                    R5_T8 = x.R5_T8,
                    R5_T9 = x.R5_T9,
                    R5_T10 = x.R5_T10,
                    R5_T11 = x.R5_T11,
                    R5_T12 = x.R5_T12,
                    R5_T13 = x.R5_T13,
                    R5_T14 = x.R5_T14,
                    R5_T15 = x.R5_T15,
                    R5_T16 = x.R5_T16,
                    R5_VIN = x.R5_VIN,
                    R5_IIN = x.R5_IIN,
                    R5_PIN = x.R5_PIN,
                    R5_TJ = x.R5_TJ,
                    R6_TimeHrs = x.R6_TimeHrs,
                    R6_T1 = x.R6_T1,
                    R6_T2 = x.R6_T2,
                    R6_T3 = x.R6_T3,
                    R6_T4 = x.R6_T4,
                    R6_T5 = x.R6_T5,
                    R6_T6 = x.R6_T6,
                    R6_T7 = x.R6_T7,
                    R6_T8 = x.R6_T8,
                    R6_T9 = x.R6_T9,
                    R6_T10 = x.R6_T10,
                    R6_T11 = x.R6_T11,
                    R6_T12 = x.R6_T12,
                    R6_T13 = x.R6_T13,
                    R6_T14 = x.R6_T14,
                    R6_T15 = x.R6_T15,
                    R6_T16 = x.R6_T16,
                    R6_VIN = x.R6_VIN,
                    R6_IIN = x.R6_IIN,
                    R6_PIN = x.R6_PIN,
                    R6_TJ = x.R6_TJ,
                    R7_TimeHrs = x.R7_TimeHrs,
                    R7_T1 = x.R7_T1,
                    R7_T2 = x.R7_T2,
                    R7_T3 = x.R7_T3,
                    R7_T4 = x.R7_T4,
                    R7_T5 = x.R7_T5,
                    R7_T6 = x.R7_T6,
                    R7_T7 = x.R7_T7,
                    R7_T8 = x.R7_T8,
                    R7_T9 = x.R7_T9,
                    R7_T10 = x.R7_T10,
                    R7_T11 = x.R7_T11,
                    R7_T12 = x.R7_T12,
                    R7_T13 = x.R7_T13,
                    R7_T14 = x.R7_T14,
                    R7_T15 = x.R7_T15,
                    R7_T16 = x.R7_T16,
                    R7_VIN = x.R7_VIN,
                    R7_IIN = x.R7_IIN,
                    R7_PIN = x.R7_PIN,
                    R7_TJ = x.R7_TJ,
                    R8_TimeHrs = x.R8_TimeHrs,
                    R8_T1 = x.R8_T1,
                    R8_T2 = x.R8_T2,
                    R8_T3 = x.R8_T3,
                    R8_T4 = x.R8_T4,
                    R8_T5 = x.R8_T5,
                    R8_T6 = x.R8_T6,
                    R8_T7 = x.R8_T7,
                    R8_T8 = x.R8_T8,
                    R8_T9 = x.R8_T9,
                    R8_T10 = x.R8_T10,
                    R8_T11 = x.R8_T11,
                    R8_T12 = x.R8_T12,
                    R8_T13 = x.R8_T13,
                    R8_T14 = x.R8_T14,
                    R8_T15 = x.R8_T15,
                    R8_T16 = x.R8_T16,
                    R8_VIN = x.R8_VIN,
                    R8_IIN = x.R8_IIN,
                    R8_PIN = x.R8_PIN,
                    R8_TJ = x.R8_TJ,
                    R9_TimeHrs = x.R9_TimeHrs,
                    R9_T1 = x.R9_T1,
                    R9_T2 = x.R9_T2,
                    R9_T3 = x.R9_T3,
                    R9_T4 = x.R9_T4,
                    R9_T5 = x.R9_T5,
                    R9_T6 = x.R9_T6,
                    R9_T7 = x.R9_T7,
                    R9_T8 = x.R9_T8,
                    R9_T9 = x.R9_T9,
                    R9_T10 = x.R9_T10,
                    R9_T11 = x.R9_T11,
                    R9_T12 = x.R9_T12,
                    R9_T13 = x.R9_T13,
                    R9_T14 = x.R9_T14,
                    R9_T15 = x.R9_T15,
                    R9_T16 = x.R9_T16,
                    R9_VIN = x.R9_VIN,
                    R9_IIN = x.R9_IIN,
                    R9_PIN = x.R9_PIN,
                    R9_TJ = x.R9_TJ,
                    R10_TimeHrs = x.R10_TimeHrs,
                    R10_T1 = x.R10_T1,
                    R10_T2 = x.R10_T2,
                    R10_T3 = x.R10_T3,
                    R10_T4 = x.R10_T4,
                    R10_T5 = x.R10_T5,
                    R10_T6 = x.R10_T6,
                    R10_T7 = x.R10_T7,
                    R10_T8 = x.R10_T8,
                    R10_T9 = x.R10_T9,
                    R10_T10 = x.R10_T10,
                    R10_T11 = x.R10_T11,
                    R10_T12 = x.R10_T12,
                    R10_T13 = x.R10_T13,
                    R10_T14 = x.R10_T14,
                    R10_T15 = x.R10_T15,
                    R10_T16 = x.R10_T16,
                    R10_VIN = x.R10_VIN,
                    R10_IIN = x.R10_IIN,
                    R10_PIN = x.R10_PIN,
                    R10_TJ = x.R10_TJ,
                    R11_TimeHrs = x.R11_TimeHrs,
                    R11_T1 = x.R11_T1,
                    R11_T2 = x.R11_T2,
                    R11_T3 = x.R11_T3,
                    R11_T4 = x.R11_T4,
                    R11_T5 = x.R11_T5,
                    R11_T6 = x.R11_T6,
                    R11_T7 = x.R11_T7,
                    R11_T8 = x.R11_T8,
                    R11_T9 = x.R11_T9,
                    R11_T10 = x.R11_T10,
                    R11_T11 = x.R11_T11,
                    R11_T12 = x.R11_T12,
                    R11_T13 = x.R11_T13,
                    R11_T14 = x.R11_T14,
                    R11_T15 = x.R11_T15,
                    R11_T16 = x.R11_T16,
                    R11_VIN = x.R11_VIN,
                    R11_IIN = x.R11_IIN,
                    R11_PIN = x.R11_PIN,
                    R11_TJ = x.R11_TJ,
                    R12_TimeHrs = x.R12_TimeHrs,
                    R12_T1 = x.R12_T1,
                    R12_T2 = x.R12_T2,
                    R12_T3 = x.R12_T3,
                    R12_T4 = x.R12_T4,
                    R12_T5 = x.R12_T5,
                    R12_T6 = x.R12_T6,
                    R12_T7 = x.R12_T7,
                    R12_T8 = x.R12_T8,
                    R12_T9 = x.R12_T9,
                    R12_T10 = x.R12_T10,
                    R12_T11 = x.R12_T11,
                    R12_T12 = x.R12_T12,
                    R12_T13 = x.R12_T13,
                    R12_T14 = x.R12_T14,
                    R12_T15 = x.R12_T15,
                    R12_T16 = x.R12_T16,
                    R12_VIN = x.R12_VIN,
                    R12_IIN = x.R12_IIN,
                    R12_PIN = x.R12_PIN,
                    R12_TJ = x.R12_TJ,
                    MaxVal_T1 = x.MaxVal_T1,
                    MaxVal_T2 = x.MaxVal_T2,
                    MaxVal_T3 = x.MaxVal_T3,
                    MaxVal_T4 = x.MaxVal_T4,
                    MaxVal_T5 = x.MaxVal_T5,
                    MaxVal_T6 = x.MaxVal_T6,
                    MaxVal_T7 = x.MaxVal_T7,
                    MaxVal_T8 = x.MaxVal_T8,
                    MaxVal_T9 = x.MaxVal_T9,
                    MaxVal_T10 = x.MaxVal_T10,
                    MaxVal_T11 = x.MaxVal_T11,
                    MaxVal_T12 = x.MaxVal_T12,
                    MaxVal_T13 = x.MaxVal_T13,
                    MaxVal_T14 = x.MaxVal_T14,
                    MaxVal_T15 = x.MaxVal_T15,
                    MaxVal_T16 = x.MaxVal_T16,
                    MaxVal_TJ = x.MaxVal_TJ,
                    Conclusion_MaxRecordedTJ = x.Conclusion_MaxRecordedTJ,
                    Conclusion_AllowableTJ = x.Conclusion_AllowableTJ,
                    Conclusion_MaxRecordedDriverTc = x.Conclusion_MaxRecordedDriverTc,
                    Conclusion_AllowableDriverTc = x.Conclusion_AllowableDriverTc,
                    Conclusion_MaxRecordedLensTemp = x.Conclusion_MaxRecordedLensTemp,
                    Conclusion_OverThermalCutoff = x.Conclusion_OverThermalCutoff,
                    Conclusion_Result = x.Conclusion_Result,
                    ConductedBy = x.ConductedBy,
                    WitnessBy = x.WitnessBy,
                    ApprovedBy = x.ApprovedBy
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

    public async Task<OperationResult> InsertTemperatureRiseTestAsync(TemperatureRiseTestViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                new SqlParameter("@TestingLocation", model.TestingLocation ?? (object)DBNull.Value),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
                new SqlParameter("@HeatSinkMaterial", model.HeatSinkMaterial ?? (object)DBNull.Value),
                new SqlParameter("@HeatSinkWeight", model.HeatSinkWeight ?? (object)DBNull.Value),
                new SqlParameter("@LensDetails", model.LensDetails ?? (object)DBNull.Value),
                new SqlParameter("@ThermalPasteDetails", model.ThermalPasteDetails ?? (object)DBNull.Value),
                new SqlParameter("@NominalOperatingVoltage", model.NominalOperatingVoltage ?? (object)DBNull.Value),
                new SqlParameter("@DriverUsed", model.DriverUsed ?? (object)DBNull.Value),
                new SqlParameter("@NoOfDrivers", model.NoOfDrivers ?? (object)DBNull.Value),
                new SqlParameter("@DriverOutputVoltage", model.DriverOutputVoltage ?? (object)DBNull.Value),
                new SqlParameter("@DriverOutputCurrent", model.DriverOutputCurrent ?? (object)DBNull.Value),
                new SqlParameter("@DriverAllowableTc", model.DriverAllowableTc ?? (object)DBNull.Value),
                new SqlParameter("@DriverAllowableTa", model.DriverAllowableTa ?? (object)DBNull.Value),
                new SqlParameter("@PcbMaterialMake", model.PcbMaterialMake ?? (object)DBNull.Value),
                new SqlParameter("@PcbSizeQty", model.PcbSizeQty ?? (object)DBNull.Value),
                new SqlParameter("@LedUsed", model.LedUsed ?? (object)DBNull.Value),
                new SqlParameter("@NoOfLeds", model.NoOfLeds ?? (object)DBNull.Value),
                new SqlParameter("@NoOfLedsInSeries", model.NoOfLedsInSeries ?? (object)DBNull.Value),
                new SqlParameter("@NoOfLedsInParallel", model.NoOfLedsInParallel ?? (object)DBNull.Value),
                new SqlParameter("@LedRjTH", model.LedRjTH ?? (object)DBNull.Value),
                new SqlParameter("@LedVf", model.LedVf ?? (object)DBNull.Value),
                new SqlParameter("@LedIf", model.LedIf ?? (object)DBNull.Value),
                new SqlParameter("@LedWdc", model.LedWdc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT1_Desc", model.ProbeT1_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT2_Desc", model.ProbeT2_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT3_Desc", model.ProbeT3_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT4_Desc", model.ProbeT4_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT5_Desc", model.ProbeT5_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT6_Desc", model.ProbeT6_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT7_Desc", model.ProbeT7_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT8_Desc", model.ProbeT8_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT9_Desc", model.ProbeT9_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT10_Desc", model.ProbeT10_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT11_Desc", model.ProbeT11_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT12_Desc", model.ProbeT12_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT13_Desc", model.ProbeT13_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT14_Desc", model.ProbeT14_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT15_Desc", model.ProbeT15_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT16_Desc", model.ProbeT16_Desc ?? (object)DBNull.Value),
                new SqlParameter("@R1_TimeHrs", model.R1_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R1_T1", model.R1_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T2", model.R1_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T3", model.R1_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T4", model.R1_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T5", model.R1_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T6", model.R1_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T7", model.R1_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T8", model.R1_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T9", model.R1_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T10", model.R1_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T11", model.R1_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T12", model.R1_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T13", model.R1_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T14", model.R1_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T15", model.R1_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T16", model.R1_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R1_VIN", model.R1_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R1_IIN", model.R1_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R1_PIN", model.R1_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R1_TJ", model.R1_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R2_TimeHrs", model.R2_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R2_T1", model.R2_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T2", model.R2_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T3", model.R2_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T4", model.R2_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T5", model.R2_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T6", model.R2_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T7", model.R2_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T8", model.R2_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T9", model.R2_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T10", model.R2_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T11", model.R2_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T12", model.R2_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T13", model.R2_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T14", model.R2_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T15", model.R2_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T16", model.R2_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R2_VIN", model.R2_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R2_IIN", model.R2_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R2_PIN", model.R2_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R2_TJ", model.R2_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R3_TimeHrs", model.R3_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R3_T1", model.R3_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T2", model.R3_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T3", model.R3_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T4", model.R3_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T5", model.R3_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T6", model.R3_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T7", model.R3_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T8", model.R3_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T9", model.R3_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T10", model.R3_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T11", model.R3_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T12", model.R3_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T13", model.R3_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T14", model.R3_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T15", model.R3_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T16", model.R3_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R3_VIN", model.R3_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R3_IIN", model.R3_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R3_PIN", model.R3_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R3_TJ", model.R3_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R4_TimeHrs", model.R4_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R4_T1", model.R4_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T2", model.R4_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T3", model.R4_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T4", model.R4_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T5", model.R4_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T6", model.R4_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T7", model.R4_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T8", model.R4_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T9", model.R4_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T10", model.R4_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T11", model.R4_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T12", model.R4_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T13", model.R4_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T14", model.R4_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T15", model.R4_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T16", model.R4_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R4_VIN", model.R4_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R4_IIN", model.R4_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R4_PIN", model.R4_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R4_TJ", model.R4_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R5_TimeHrs", model.R5_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R5_T1", model.R5_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T2", model.R5_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T3", model.R5_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T4", model.R5_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T5", model.R5_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T6", model.R5_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T7", model.R5_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T8", model.R5_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T9", model.R5_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T10", model.R5_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T11", model.R5_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T12", model.R5_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T13", model.R5_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T14", model.R5_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T15", model.R5_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T16", model.R5_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R5_VIN", model.R5_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R5_IIN", model.R5_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R5_PIN", model.R5_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R5_TJ", model.R5_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R6_TimeHrs", model.R6_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R6_T1", model.R6_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T2", model.R6_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T3", model.R6_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T4", model.R6_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T5", model.R6_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T6", model.R6_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T7", model.R6_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T8", model.R6_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T9", model.R6_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T10", model.R6_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T11", model.R6_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T12", model.R6_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T13", model.R6_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T14", model.R6_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T15", model.R6_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T16", model.R6_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R6_VIN", model.R6_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R6_IIN", model.R6_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R6_PIN", model.R6_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R6_TJ", model.R6_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R7_TimeHrs", model.R7_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R7_T1", model.R7_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T2", model.R7_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T3", model.R7_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T4", model.R7_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T5", model.R7_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T6", model.R7_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T7", model.R7_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T8", model.R7_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T9", model.R7_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T10", model.R7_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T11", model.R7_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T12", model.R7_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T13", model.R7_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T14", model.R7_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T15", model.R7_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T16", model.R7_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R7_VIN", model.R7_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R7_IIN", model.R7_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R7_PIN", model.R7_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R7_TJ", model.R7_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R8_TimeHrs", model.R8_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R8_T1", model.R8_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T2", model.R8_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T3", model.R8_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T4", model.R8_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T5", model.R8_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T6", model.R8_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T7", model.R8_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T8", model.R8_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T9", model.R8_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T10", model.R8_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T11", model.R8_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T12", model.R8_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T13", model.R8_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T14", model.R8_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T15", model.R8_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T16", model.R8_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R8_VIN", model.R8_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R8_IIN", model.R8_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R8_PIN", model.R8_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R8_TJ", model.R8_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R9_TimeHrs", model.R9_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R9_T1", model.R9_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T2", model.R9_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T3", model.R9_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T4", model.R9_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T5", model.R9_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T6", model.R9_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T7", model.R9_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T8", model.R9_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T9", model.R9_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T10", model.R9_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T11", model.R9_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T12", model.R9_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T13", model.R9_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T14", model.R9_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T15", model.R9_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T16", model.R9_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R9_VIN", model.R9_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R9_IIN", model.R9_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R9_PIN", model.R9_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R9_TJ", model.R9_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R10_TimeHrs", model.R10_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R10_T1", model.R10_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T2", model.R10_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T3", model.R10_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T4", model.R10_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T5", model.R10_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T6", model.R10_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T7", model.R10_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T8", model.R10_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T9", model.R10_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T10", model.R10_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T11", model.R10_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T12", model.R10_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T13", model.R10_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T14", model.R10_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T15", model.R10_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T16", model.R10_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R10_VIN", model.R10_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R10_IIN", model.R10_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R10_PIN", model.R10_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R10_TJ", model.R10_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R11_TimeHrs", model.R11_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R11_T1", model.R11_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T2", model.R11_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T3", model.R11_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T4", model.R11_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T5", model.R11_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T6", model.R11_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T7", model.R11_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T8", model.R11_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T9", model.R11_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T10", model.R11_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T11", model.R11_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T12", model.R11_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T13", model.R11_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T14", model.R11_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T15", model.R11_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T16", model.R11_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R11_VIN", model.R11_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R11_IIN", model.R11_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R11_PIN", model.R11_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R11_TJ", model.R11_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R12_TimeHrs", model.R12_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R12_T1", model.R12_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T2", model.R12_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T3", model.R12_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T4", model.R12_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T5", model.R12_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T6", model.R12_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T7", model.R12_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T8", model.R12_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T9", model.R12_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T10", model.R12_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T11", model.R12_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T12", model.R12_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T13", model.R12_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T14", model.R12_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T15", model.R12_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T16", model.R12_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R12_VIN", model.R12_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R12_IIN", model.R12_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R12_PIN", model.R12_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R12_TJ", model.R12_TJ ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T1", model.MaxVal_T1 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T2", model.MaxVal_T2 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T3", model.MaxVal_T3 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T4", model.MaxVal_T4 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T5", model.MaxVal_T5 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T6", model.MaxVal_T6 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T7", model.MaxVal_T7 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T8", model.MaxVal_T8 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T9", model.MaxVal_T9 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T10", model.MaxVal_T10 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T11", model.MaxVal_T11 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T12", model.MaxVal_T12 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T13", model.MaxVal_T13 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T14", model.MaxVal_T14 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T15", model.MaxVal_T15 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T16", model.MaxVal_T16 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_TJ", model.MaxVal_TJ ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_MaxRecordedTJ", model.Conclusion_MaxRecordedTJ ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_AllowableTJ", model.Conclusion_AllowableTJ ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_MaxRecordedDriverTc", model.Conclusion_MaxRecordedDriverTc ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_AllowableDriverTc", model.Conclusion_AllowableDriverTc ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_MaxRecordedLensTemp", model.Conclusion_MaxRecordedLensTemp ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_OverThermalCutoff", model.Conclusion_OverThermalCutoff ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_Result", model.Conclusion_Result ?? (object)DBNull.Value),
                new SqlParameter("@ConductedBy", model.ConductedBy ?? (object)DBNull.Value),
                new SqlParameter("@WitnessBy", model.WitnessBy ?? (object)DBNull.Value),
                new SqlParameter("@ApprovedBy", model.ApprovedBy ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_TemperatureRiseTest " +
                    "@ReportNo, " +
                    "@ReportDate, " +
                    "@TestingLocation, " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@BatchCode, " +
                    "@PKD, " +
                    "@HeatSinkMaterial, " +
                    "@HeatSinkWeight, " +
                    "@LensDetails, " +
                    "@ThermalPasteDetails, " +
                    "@NominalOperatingVoltage, " +
                    "@DriverUsed, " +
                    "@NoOfDrivers, " +
                    "@DriverOutputVoltage, " +
                    "@DriverOutputCurrent, " +
                    "@DriverAllowableTc, " +
                    "@DriverAllowableTa, " +
                    "@PcbMaterialMake, " +
                    "@PcbSizeQty, " +
                    "@LedUsed, " +
                    "@NoOfLeds, " +
                    "@NoOfLedsInSeries, " +
                    "@NoOfLedsInParallel, " +
                    "@LedRjTH, " +
                    "@LedVf, " +
                    "@LedIf, " +
                    "@LedWdc, " +
                    "@ProbeT1_Desc, " +
                    "@ProbeT2_Desc, " +
                    "@ProbeT3_Desc, " +
                    "@ProbeT4_Desc, " +
                    "@ProbeT5_Desc, " +
                    "@ProbeT6_Desc, " +
                    "@ProbeT7_Desc, " +
                    "@ProbeT8_Desc, " +
                    "@ProbeT9_Desc, " +
                    "@ProbeT10_Desc, " +
                    "@ProbeT11_Desc, " +
                    "@ProbeT12_Desc, " +
                    "@ProbeT13_Desc, " +
                    "@ProbeT14_Desc, " +
                    "@ProbeT15_Desc, " +
                    "@ProbeT16_Desc, " +
                    "@R1_TimeHrs, " +
                    "@R1_T1, " +
                    "@R1_T2, " +
                    "@R1_T3, " +
                    "@R1_T4, " +
                    "@R1_T5, " +
                    "@R1_T6, " +
                    "@R1_T7, " +
                    "@R1_T8, " +
                    "@R1_T9, " +
                    "@R1_T10, " +
                    "@R1_T11, " +
                    "@R1_T12, " +
                    "@R1_T13, " +
                    "@R1_T14, " +
                    "@R1_T15, " +
                    "@R1_T16, " +
                    "@R1_VIN, " +
                    "@R1_IIN, " +
                    "@R1_PIN, " +
                    "@R1_TJ, " +
                    "@R2_TimeHrs, " +
                    "@R2_T1, " +
                    "@R2_T2, " +
                    "@R2_T3, " +
                    "@R2_T4, " +
                    "@R2_T5, " +
                    "@R2_T6, " +
                    "@R2_T7, " +
                    "@R2_T8, " +
                    "@R2_T9, " +
                    "@R2_T10, " +
                    "@R2_T11, " +
                    "@R2_T12, " +
                    "@R2_T13, " +
                    "@R2_T14, " +
                    "@R2_T15, " +
                    "@R2_T16, " +
                    "@R2_VIN, " +
                    "@R2_IIN, " +
                    "@R2_PIN, " +
                    "@R2_TJ, " +
                    "@R3_TimeHrs, " +
                    "@R3_T1, " +
                    "@R3_T2, " +
                    "@R3_T3, " +
                    "@R3_T4, " +
                    "@R3_T5, " +
                    "@R3_T6, " +
                    "@R3_T7, " +
                    "@R3_T8, " +
                    "@R3_T9, " +
                    "@R3_T10, " +
                    "@R3_T11, " +
                    "@R3_T12, " +
                    "@R3_T13, " +
                    "@R3_T14, " +
                    "@R3_T15, " +
                    "@R3_T16, " +
                    "@R3_VIN, " +
                    "@R3_IIN, " +
                    "@R3_PIN, " +
                    "@R3_TJ, " +
                    "@R4_TimeHrs, " +
                    "@R4_T1, " +
                    "@R4_T2, " +
                    "@R4_T3, " +
                    "@R4_T4, " +
                    "@R4_T5, " +
                    "@R4_T6, " +
                    "@R4_T7, " +
                    "@R4_T8, " +
                    "@R4_T9, " +
                    "@R4_T10, " +
                    "@R4_T11, " +
                    "@R4_T12, " +
                    "@R4_T13, " +
                    "@R4_T14, " +
                    "@R4_T15, " +
                    "@R4_T16, " +
                    "@R4_VIN, " +
                    "@R4_IIN, " +
                    "@R4_PIN, " +
                    "@R4_TJ, " +
                    "@R5_TimeHrs, " +
                    "@R5_T1, " +
                    "@R5_T2, " +
                    "@R5_T3, " +
                    "@R5_T4, " +
                    "@R5_T5, " +
                    "@R5_T6, " +
                    "@R5_T7, " +
                    "@R5_T8, " +
                    "@R5_T9, " +
                    "@R5_T10, " +
                    "@R5_T11, " +
                    "@R5_T12, " +
                    "@R5_T13, " +
                    "@R5_T14, " +
                    "@R5_T15, " +
                    "@R5_T16, " +
                    "@R5_VIN, " +
                    "@R5_IIN, " +
                    "@R5_PIN, " +
                    "@R5_TJ, " +
                    "@R6_TimeHrs, " +
                    "@R6_T1, " +
                    "@R6_T2, " +
                    "@R6_T3, " +
                    "@R6_T4, " +
                    "@R6_T5, " +
                    "@R6_T6, " +
                    "@R6_T7, " +
                    "@R6_T8, " +
                    "@R6_T9, " +
                    "@R6_T10, " +
                    "@R6_T11, " +
                    "@R6_T12, " +
                    "@R6_T13, " +
                    "@R6_T14, " +
                    "@R6_T15, " +
                    "@R6_T16, " +
                    "@R6_VIN, " +
                    "@R6_IIN, " +
                    "@R6_PIN, " +
                    "@R6_TJ, " +
                    "@R7_TimeHrs, " +
                    "@R7_T1, " +
                    "@R7_T2, " +
                    "@R7_T3, " +
                    "@R7_T4, " +
                    "@R7_T5, " +
                    "@R7_T6, " +
                    "@R7_T7, " +
                    "@R7_T8, " +
                    "@R7_T9, " +
                    "@R7_T10, " +
                    "@R7_T11, " +
                    "@R7_T12, " +
                    "@R7_T13, " +
                    "@R7_T14, " +
                    "@R7_T15, " +
                    "@R7_T16, " +
                    "@R7_VIN, " +
                    "@R7_IIN, " +
                    "@R7_PIN, " +
                    "@R7_TJ, " +
                    "@R8_TimeHrs, " +
                    "@R8_T1, " +
                    "@R8_T2, " +
                    "@R8_T3, " +
                    "@R8_T4, " +
                    "@R8_T5, " +
                    "@R8_T6, " +
                    "@R8_T7, " +
                    "@R8_T8, " +
                    "@R8_T9, " +
                    "@R8_T10, " +
                    "@R8_T11, " +
                    "@R8_T12, " +
                    "@R8_T13, " +
                    "@R8_T14, " +
                    "@R8_T15, " +
                    "@R8_T16, " +
                    "@R8_VIN, " +
                    "@R8_IIN, " +
                    "@R8_PIN, " +
                    "@R8_TJ, " +
                    "@R9_TimeHrs, " +
                    "@R9_T1, " +
                    "@R9_T2, " +
                    "@R9_T3, " +
                    "@R9_T4, " +
                    "@R9_T5, " +
                    "@R9_T6, " +
                    "@R9_T7, " +
                    "@R9_T8, " +
                    "@R9_T9, " +
                    "@R9_T10, " +
                    "@R9_T11, " +
                    "@R9_T12, " +
                    "@R9_T13, " +
                    "@R9_T14, " +
                    "@R9_T15, " +
                    "@R9_T16, " +
                    "@R9_VIN, " +
                    "@R9_IIN, " +
                    "@R9_PIN, " +
                    "@R9_TJ, " +
                    "@R10_TimeHrs, " +
                    "@R10_T1, " +
                    "@R10_T2, " +
                    "@R10_T3, " +
                    "@R10_T4, " +
                    "@R10_T5, " +
                    "@R10_T6, " +
                    "@R10_T7, " +
                    "@R10_T8, " +
                    "@R10_T9, " +
                    "@R10_T10, " +
                    "@R10_T11, " +
                    "@R10_T12, " +
                    "@R10_T13, " +
                    "@R10_T14, " +
                    "@R10_T15, " +
                    "@R10_T16, " +
                    "@R10_VIN, " +
                    "@R10_IIN, " +
                    "@R10_PIN, " +
                    "@R10_TJ, " +
                    "@R11_TimeHrs, " +
                    "@R11_T1, " +
                    "@R11_T2, " +
                    "@R11_T3, " +
                    "@R11_T4, " +
                    "@R11_T5, " +
                    "@R11_T6, " +
                    "@R11_T7, " +
                    "@R11_T8, " +
                    "@R11_T9, " +
                    "@R11_T10, " +
                    "@R11_T11, " +
                    "@R11_T12, " +
                    "@R11_T13, " +
                    "@R11_T14, " +
                    "@R11_T15, " +
                    "@R11_T16, " +
                    "@R11_VIN, " +
                    "@R11_IIN, " +
                    "@R11_PIN, " +
                    "@R11_TJ, " +
                    "@R12_TimeHrs, " +
                    "@R12_T1, " +
                    "@R12_T2, " +
                    "@R12_T3, " +
                    "@R12_T4, " +
                    "@R12_T5, " +
                    "@R12_T6, " +
                    "@R12_T7, " +
                    "@R12_T8, " +
                    "@R12_T9, " +
                    "@R12_T10, " +
                    "@R12_T11, " +
                    "@R12_T12, " +
                    "@R12_T13, " +
                    "@R12_T14, " +
                    "@R12_T15, " +
                    "@R12_T16, " +
                    "@R12_VIN, " +
                    "@R12_IIN, " +
                    "@R12_PIN, " +
                    "@R12_TJ, " +
                    "@MaxVal_T1, " +
                    "@MaxVal_T2, " +
                    "@MaxVal_T3, " +
                    "@MaxVal_T4, " +
                    "@MaxVal_T5, " +
                    "@MaxVal_T6, " +
                    "@MaxVal_T7, " +
                    "@MaxVal_T8, " +
                    "@MaxVal_T9, " +
                    "@MaxVal_T10, " +
                    "@MaxVal_T11, " +
                    "@MaxVal_T12, " +
                    "@MaxVal_T13, " +
                    "@MaxVal_T14, " +
                    "@MaxVal_T15, " +
                    "@MaxVal_T16, " +
                    "@MaxVal_TJ, " +
                    "@Conclusion_MaxRecordedTJ, " +
                    "@Conclusion_AllowableTJ, " +
                    "@Conclusion_MaxRecordedDriverTc, " +
                    "@Conclusion_AllowableDriverTc, " +
                    "@Conclusion_MaxRecordedLensTemp, " +
                    "@Conclusion_OverThermalCutoff, " +
                    "@Conclusion_Result, " +
                    "@ConductedBy, " +
                    "@WitnessBy, " +
                    "@ApprovedBy, " +
                    "@AddedBy, " +
                    "@AddedOn",
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

    public async Task<OperationResult> UpdateTemperatureRiseTestAsync(TemperatureRiseTestViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                new SqlParameter("@TestingLocation", model.TestingLocation ?? (object)DBNull.Value),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
                new SqlParameter("@HeatSinkMaterial", model.HeatSinkMaterial ?? (object)DBNull.Value),
                new SqlParameter("@HeatSinkWeight", model.HeatSinkWeight ?? (object)DBNull.Value),
                new SqlParameter("@LensDetails", model.LensDetails ?? (object)DBNull.Value),
                new SqlParameter("@ThermalPasteDetails", model.ThermalPasteDetails ?? (object)DBNull.Value),
                new SqlParameter("@NominalOperatingVoltage", model.NominalOperatingVoltage ?? (object)DBNull.Value),
                new SqlParameter("@DriverUsed", model.DriverUsed ?? (object)DBNull.Value),
                new SqlParameter("@NoOfDrivers", model.NoOfDrivers ?? (object)DBNull.Value),
                new SqlParameter("@DriverOutputVoltage", model.DriverOutputVoltage ?? (object)DBNull.Value),
                new SqlParameter("@DriverOutputCurrent", model.DriverOutputCurrent ?? (object)DBNull.Value),
                new SqlParameter("@DriverAllowableTc", model.DriverAllowableTc ?? (object)DBNull.Value),
                new SqlParameter("@DriverAllowableTa", model.DriverAllowableTa ?? (object)DBNull.Value),
                new SqlParameter("@PcbMaterialMake", model.PcbMaterialMake ?? (object)DBNull.Value),
                new SqlParameter("@PcbSizeQty", model.PcbSizeQty ?? (object)DBNull.Value),
                new SqlParameter("@LedUsed", model.LedUsed ?? (object)DBNull.Value),
                new SqlParameter("@NoOfLeds", model.NoOfLeds ?? (object)DBNull.Value),
                new SqlParameter("@NoOfLedsInSeries", model.NoOfLedsInSeries ?? (object)DBNull.Value),
                new SqlParameter("@NoOfLedsInParallel", model.NoOfLedsInParallel ?? (object)DBNull.Value),
                new SqlParameter("@LedRjTH", model.LedRjTH ?? (object)DBNull.Value),
                new SqlParameter("@LedVf", model.LedVf ?? (object)DBNull.Value),
                new SqlParameter("@LedIf", model.LedIf ?? (object)DBNull.Value),
                new SqlParameter("@LedWdc", model.LedWdc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT1_Desc", model.ProbeT1_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT2_Desc", model.ProbeT2_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT3_Desc", model.ProbeT3_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT4_Desc", model.ProbeT4_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT5_Desc", model.ProbeT5_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT6_Desc", model.ProbeT6_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT7_Desc", model.ProbeT7_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT8_Desc", model.ProbeT8_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT9_Desc", model.ProbeT9_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT10_Desc", model.ProbeT10_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT11_Desc", model.ProbeT11_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT12_Desc", model.ProbeT12_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT13_Desc", model.ProbeT13_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT14_Desc", model.ProbeT14_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT15_Desc", model.ProbeT15_Desc ?? (object)DBNull.Value),
                new SqlParameter("@ProbeT16_Desc", model.ProbeT16_Desc ?? (object)DBNull.Value),
                new SqlParameter("@R1_TimeHrs", model.R1_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R1_T1", model.R1_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T2", model.R1_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T3", model.R1_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T4", model.R1_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T5", model.R1_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T6", model.R1_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T7", model.R1_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T8", model.R1_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T9", model.R1_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T10", model.R1_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T11", model.R1_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T12", model.R1_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T13", model.R1_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T14", model.R1_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T15", model.R1_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R1_T16", model.R1_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R1_VIN", model.R1_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R1_IIN", model.R1_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R1_PIN", model.R1_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R1_TJ", model.R1_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R2_TimeHrs", model.R2_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R2_T1", model.R2_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T2", model.R2_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T3", model.R2_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T4", model.R2_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T5", model.R2_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T6", model.R2_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T7", model.R2_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T8", model.R2_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T9", model.R2_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T10", model.R2_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T11", model.R2_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T12", model.R2_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T13", model.R2_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T14", model.R2_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T15", model.R2_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R2_T16", model.R2_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R2_VIN", model.R2_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R2_IIN", model.R2_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R2_PIN", model.R2_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R2_TJ", model.R2_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R3_TimeHrs", model.R3_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R3_T1", model.R3_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T2", model.R3_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T3", model.R3_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T4", model.R3_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T5", model.R3_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T6", model.R3_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T7", model.R3_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T8", model.R3_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T9", model.R3_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T10", model.R3_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T11", model.R3_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T12", model.R3_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T13", model.R3_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T14", model.R3_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T15", model.R3_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R3_T16", model.R3_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R3_VIN", model.R3_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R3_IIN", model.R3_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R3_PIN", model.R3_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R3_TJ", model.R3_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R4_TimeHrs", model.R4_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R4_T1", model.R4_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T2", model.R4_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T3", model.R4_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T4", model.R4_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T5", model.R4_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T6", model.R4_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T7", model.R4_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T8", model.R4_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T9", model.R4_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T10", model.R4_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T11", model.R4_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T12", model.R4_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T13", model.R4_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T14", model.R4_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T15", model.R4_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R4_T16", model.R4_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R4_VIN", model.R4_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R4_IIN", model.R4_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R4_PIN", model.R4_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R4_TJ", model.R4_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R5_TimeHrs", model.R5_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R5_T1", model.R5_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T2", model.R5_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T3", model.R5_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T4", model.R5_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T5", model.R5_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T6", model.R5_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T7", model.R5_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T8", model.R5_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T9", model.R5_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T10", model.R5_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T11", model.R5_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T12", model.R5_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T13", model.R5_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T14", model.R5_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T15", model.R5_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R5_T16", model.R5_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R5_VIN", model.R5_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R5_IIN", model.R5_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R5_PIN", model.R5_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R5_TJ", model.R5_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R6_TimeHrs", model.R6_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R6_T1", model.R6_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T2", model.R6_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T3", model.R6_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T4", model.R6_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T5", model.R6_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T6", model.R6_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T7", model.R6_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T8", model.R6_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T9", model.R6_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T10", model.R6_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T11", model.R6_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T12", model.R6_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T13", model.R6_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T14", model.R6_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T15", model.R6_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R6_T16", model.R6_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R6_VIN", model.R6_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R6_IIN", model.R6_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R6_PIN", model.R6_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R6_TJ", model.R6_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R7_TimeHrs", model.R7_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R7_T1", model.R7_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T2", model.R7_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T3", model.R7_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T4", model.R7_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T5", model.R7_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T6", model.R7_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T7", model.R7_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T8", model.R7_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T9", model.R7_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T10", model.R7_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T11", model.R7_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T12", model.R7_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T13", model.R7_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T14", model.R7_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T15", model.R7_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R7_T16", model.R7_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R7_VIN", model.R7_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R7_IIN", model.R7_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R7_PIN", model.R7_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R7_TJ", model.R7_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R8_TimeHrs", model.R8_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R8_T1", model.R8_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T2", model.R8_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T3", model.R8_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T4", model.R8_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T5", model.R8_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T6", model.R8_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T7", model.R8_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T8", model.R8_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T9", model.R8_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T10", model.R8_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T11", model.R8_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T12", model.R8_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T13", model.R8_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T14", model.R8_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T15", model.R8_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R8_T16", model.R8_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R8_VIN", model.R8_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R8_IIN", model.R8_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R8_PIN", model.R8_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R8_TJ", model.R8_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R9_TimeHrs", model.R9_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R9_T1", model.R9_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T2", model.R9_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T3", model.R9_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T4", model.R9_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T5", model.R9_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T6", model.R9_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T7", model.R9_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T8", model.R9_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T9", model.R9_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T10", model.R9_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T11", model.R9_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T12", model.R9_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T13", model.R9_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T14", model.R9_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T15", model.R9_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R9_T16", model.R9_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R9_VIN", model.R9_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R9_IIN", model.R9_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R9_PIN", model.R9_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R9_TJ", model.R9_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R10_TimeHrs", model.R10_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R10_T1", model.R10_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T2", model.R10_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T3", model.R10_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T4", model.R10_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T5", model.R10_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T6", model.R10_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T7", model.R10_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T8", model.R10_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T9", model.R10_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T10", model.R10_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T11", model.R10_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T12", model.R10_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T13", model.R10_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T14", model.R10_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T15", model.R10_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R10_T16", model.R10_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R10_VIN", model.R10_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R10_IIN", model.R10_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R10_PIN", model.R10_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R10_TJ", model.R10_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R11_TimeHrs", model.R11_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R11_T1", model.R11_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T2", model.R11_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T3", model.R11_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T4", model.R11_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T5", model.R11_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T6", model.R11_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T7", model.R11_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T8", model.R11_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T9", model.R11_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T10", model.R11_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T11", model.R11_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T12", model.R11_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T13", model.R11_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T14", model.R11_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T15", model.R11_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R11_T16", model.R11_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R11_VIN", model.R11_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R11_IIN", model.R11_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R11_PIN", model.R11_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R11_TJ", model.R11_TJ ?? (object)DBNull.Value),
                new SqlParameter("@R12_TimeHrs", model.R12_TimeHrs ?? (object)DBNull.Value),
                new SqlParameter("@R12_T1", model.R12_T1 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T2", model.R12_T2 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T3", model.R12_T3 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T4", model.R12_T4 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T5", model.R12_T5 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T6", model.R12_T6 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T7", model.R12_T7 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T8", model.R12_T8 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T9", model.R12_T9 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T10", model.R12_T10 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T11", model.R12_T11 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T12", model.R12_T12 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T13", model.R12_T13 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T14", model.R12_T14 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T15", model.R12_T15 ?? (object)DBNull.Value),
                new SqlParameter("@R12_T16", model.R12_T16 ?? (object)DBNull.Value),
                new SqlParameter("@R12_VIN", model.R12_VIN ?? (object)DBNull.Value),
                new SqlParameter("@R12_IIN", model.R12_IIN ?? (object)DBNull.Value),
                new SqlParameter("@R12_PIN", model.R12_PIN ?? (object)DBNull.Value),
                new SqlParameter("@R12_TJ", model.R12_TJ ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T1", model.MaxVal_T1 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T2", model.MaxVal_T2 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T3", model.MaxVal_T3 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T4", model.MaxVal_T4 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T5", model.MaxVal_T5 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T6", model.MaxVal_T6 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T7", model.MaxVal_T7 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T8", model.MaxVal_T8 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T9", model.MaxVal_T9 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T10", model.MaxVal_T10 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T11", model.MaxVal_T11 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T12", model.MaxVal_T12 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T13", model.MaxVal_T13 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T14", model.MaxVal_T14 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T15", model.MaxVal_T15 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_T16", model.MaxVal_T16 ?? (object)DBNull.Value),
                new SqlParameter("@MaxVal_TJ", model.MaxVal_TJ ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_MaxRecordedTJ", model.Conclusion_MaxRecordedTJ ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_AllowableTJ", model.Conclusion_AllowableTJ ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_MaxRecordedDriverTc", model.Conclusion_MaxRecordedDriverTc ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_AllowableDriverTc", model.Conclusion_AllowableDriverTc ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_MaxRecordedLensTemp", model.Conclusion_MaxRecordedLensTemp ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_OverThermalCutoff", model.Conclusion_OverThermalCutoff ?? (object)DBNull.Value),
                new SqlParameter("@Conclusion_Result", model.Conclusion_Result ?? (object)DBNull.Value),
                new SqlParameter("@ConductedBy", model.ConductedBy ?? (object)DBNull.Value),
                new SqlParameter("@WitnessBy", model.WitnessBy ?? (object)DBNull.Value),
                new SqlParameter("@ApprovedBy", model.ApprovedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_TemperatureRiseTest " +
                    "@Id, " +
                    "@ReportNo, " +
                    "@ReportDate, " +
                    "@TestingLocation, " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@BatchCode, " +
                    "@PKD, " +
                    "@HeatSinkMaterial, " +
                    "@HeatSinkWeight, " +
                    "@LensDetails, " +
                    "@ThermalPasteDetails, " +
                    "@NominalOperatingVoltage, " +
                    "@DriverUsed, " +
                    "@NoOfDrivers, " +
                    "@DriverOutputVoltage, " +
                    "@DriverOutputCurrent, " +
                    "@DriverAllowableTc, " +
                    "@DriverAllowableTa, " +
                    "@PcbMaterialMake, " +
                    "@PcbSizeQty, " +
                    "@LedUsed, " +
                    "@NoOfLeds, " +
                    "@NoOfLedsInSeries, " +
                    "@NoOfLedsInParallel, " +
                    "@LedRjTH, " +
                    "@LedVf, " +
                    "@LedIf, " +
                    "@LedWdc, " +
                    "@ProbeT1_Desc, " +
                    "@ProbeT2_Desc, " +
                    "@ProbeT3_Desc, " +
                    "@ProbeT4_Desc, " +
                    "@ProbeT5_Desc, " +
                    "@ProbeT6_Desc, " +
                    "@ProbeT7_Desc, " +
                    "@ProbeT8_Desc, " +
                    "@ProbeT9_Desc, " +
                    "@ProbeT10_Desc, " +
                    "@ProbeT11_Desc, " +
                    "@ProbeT12_Desc, " +
                    "@ProbeT13_Desc, " +
                    "@ProbeT14_Desc, " +
                    "@ProbeT15_Desc, " +
                    "@ProbeT16_Desc, " +
                    "@R1_TimeHrs, " +
                    "@R1_T1, " +
                    "@R1_T2, " +
                    "@R1_T3, " +
                    "@R1_T4, " +
                    "@R1_T5, " +
                    "@R1_T6, " +
                    "@R1_T7, " +
                    "@R1_T8, " +
                    "@R1_T9, " +
                    "@R1_T10, " +
                    "@R1_T11, " +
                    "@R1_T12, " +
                    "@R1_T13, " +
                    "@R1_T14, " +
                    "@R1_T15, " +
                    "@R1_T16, " +
                    "@R1_VIN, " +
                    "@R1_IIN, " +
                    "@R1_PIN, " +
                    "@R1_TJ, " +
                    "@R2_TimeHrs, " +
                    "@R2_T1, " +
                    "@R2_T2, " +
                    "@R2_T3, " +
                    "@R2_T4, " +
                    "@R2_T5, " +
                    "@R2_T6, " +
                    "@R2_T7, " +
                    "@R2_T8, " +
                    "@R2_T9, " +
                    "@R2_T10, " +
                    "@R2_T11, " +
                    "@R2_T12, " +
                    "@R2_T13, " +
                    "@R2_T14, " +
                    "@R2_T15, " +
                    "@R2_T16, " +
                    "@R2_VIN, " +
                    "@R2_IIN, " +
                    "@R2_PIN, " +
                    "@R2_TJ, " +
                    "@R3_TimeHrs, " +
                    "@R3_T1, " +
                    "@R3_T2, " +
                    "@R3_T3, " +
                    "@R3_T4, " +
                    "@R3_T5, " +
                    "@R3_T6, " +
                    "@R3_T7, " +
                    "@R3_T8, " +
                    "@R3_T9, " +
                    "@R3_T10, " +
                    "@R3_T11, " +
                    "@R3_T12, " +
                    "@R3_T13, " +
                    "@R3_T14, " +
                    "@R3_T15, " +
                    "@R3_T16, " +
                    "@R3_VIN, " +
                    "@R3_IIN, " +
                    "@R3_PIN, " +
                    "@R3_TJ, " +
                    "@R4_TimeHrs, " +
                    "@R4_T1, " +
                    "@R4_T2, " +
                    "@R4_T3, " +
                    "@R4_T4, " +
                    "@R4_T5, " +
                    "@R4_T6, " +
                    "@R4_T7, " +
                    "@R4_T8, " +
                    "@R4_T9, " +
                    "@R4_T10, " +
                    "@R4_T11, " +
                    "@R4_T12, " +
                    "@R4_T13, " +
                    "@R4_T14, " +
                    "@R4_T15, " +
                    "@R4_T16, " +
                    "@R4_VIN, " +
                    "@R4_IIN, " +
                    "@R4_PIN, " +
                    "@R4_TJ, " +
                    "@R5_TimeHrs, " +
                    "@R5_T1, " +
                    "@R5_T2, " +
                    "@R5_T3, " +
                    "@R5_T4, " +
                    "@R5_T5, " +
                    "@R5_T6, " +
                    "@R5_T7, " +
                    "@R5_T8, " +
                    "@R5_T9, " +
                    "@R5_T10, " +
                    "@R5_T11, " +
                    "@R5_T12, " +
                    "@R5_T13, " +
                    "@R5_T14, " +
                    "@R5_T15, " +
                    "@R5_T16, " +
                    "@R5_VIN, " +
                    "@R5_IIN, " +
                    "@R5_PIN, " +
                    "@R5_TJ, " +
                    "@R6_TimeHrs, " +
                    "@R6_T1, " +
                    "@R6_T2, " +
                    "@R6_T3, " +
                    "@R6_T4, " +
                    "@R6_T5, " +
                    "@R6_T6, " +
                    "@R6_T7, " +
                    "@R6_T8, " +
                    "@R6_T9, " +
                    "@R6_T10, " +
                    "@R6_T11, " +
                    "@R6_T12, " +
                    "@R6_T13, " +
                    "@R6_T14, " +
                    "@R6_T15, " +
                    "@R6_T16, " +
                    "@R6_VIN, " +
                    "@R6_IIN, " +
                    "@R6_PIN, " +
                    "@R6_TJ, " +
                    "@R7_TimeHrs, " +
                    "@R7_T1, " +
                    "@R7_T2, " +
                    "@R7_T3, " +
                    "@R7_T4, " +
                    "@R7_T5, " +
                    "@R7_T6, " +
                    "@R7_T7, " +
                    "@R7_T8, " +
                    "@R7_T9, " +
                    "@R7_T10, " +
                    "@R7_T11, " +
                    "@R7_T12, " +
                    "@R7_T13, " +
                    "@R7_T14, " +
                    "@R7_T15, " +
                    "@R7_T16, " +
                    "@R7_VIN, " +
                    "@R7_IIN, " +
                    "@R7_PIN, " +
                    "@R7_TJ, " +
                    "@R8_TimeHrs, " +
                    "@R8_T1, " +
                    "@R8_T2, " +
                    "@R8_T3, " +
                    "@R8_T4, " +
                    "@R8_T5, " +
                    "@R8_T6, " +
                    "@R8_T7, " +
                    "@R8_T8, " +
                    "@R8_T9, " +
                    "@R8_T10, " +
                    "@R8_T11, " +
                    "@R8_T12, " +
                    "@R8_T13, " +
                    "@R8_T14, " +
                    "@R8_T15, " +
                    "@R8_T16, " +
                    "@R8_VIN, " +
                    "@R8_IIN, " +
                    "@R8_PIN, " +
                    "@R8_TJ, " +
                    "@R9_TimeHrs, " +
                    "@R9_T1, " +
                    "@R9_T2, " +
                    "@R9_T3, " +
                    "@R9_T4, " +
                    "@R9_T5, " +
                    "@R9_T6, " +
                    "@R9_T7, " +
                    "@R9_T8, " +
                    "@R9_T9, " +
                    "@R9_T10, " +
                    "@R9_T11, " +
                    "@R9_T12, " +
                    "@R9_T13, " +
                    "@R9_T14, " +
                    "@R9_T15, " +
                    "@R9_T16, " +
                    "@R9_VIN, " +
                    "@R9_IIN, " +
                    "@R9_PIN, " +
                    "@R9_TJ, " +
                    "@R10_TimeHrs, " +
                    "@R10_T1, " +
                    "@R10_T2, " +
                    "@R10_T3, " +
                    "@R10_T4, " +
                    "@R10_T5, " +
                    "@R10_T6, " +
                    "@R10_T7, " +
                    "@R10_T8, " +
                    "@R10_T9, " +
                    "@R10_T10, " +
                    "@R10_T11, " +
                    "@R10_T12, " +
                    "@R10_T13, " +
                    "@R10_T14, " +
                    "@R10_T15, " +
                    "@R10_T16, " +
                    "@R10_VIN, " +
                    "@R10_IIN, " +
                    "@R10_PIN, " +
                    "@R10_TJ, " +
                    "@R11_TimeHrs, " +
                    "@R11_T1, " +
                    "@R11_T2, " +
                    "@R11_T3, " +
                    "@R11_T4, " +
                    "@R11_T5, " +
                    "@R11_T6, " +
                    "@R11_T7, " +
                    "@R11_T8, " +
                    "@R11_T9, " +
                    "@R11_T10, " +
                    "@R11_T11, " +
                    "@R11_T12, " +
                    "@R11_T13, " +
                    "@R11_T14, " +
                    "@R11_T15, " +
                    "@R11_T16, " +
                    "@R11_VIN, " +
                    "@R11_IIN, " +
                    "@R11_PIN, " +
                    "@R11_TJ, " +
                    "@R12_TimeHrs, " +
                    "@R12_T1, " +
                    "@R12_T2, " +
                    "@R12_T3, " +
                    "@R12_T4, " +
                    "@R12_T5, " +
                    "@R12_T6, " +
                    "@R12_T7, " +
                    "@R12_T8, " +
                    "@R12_T9, " +
                    "@R12_T10, " +
                    "@R12_T11, " +
                    "@R12_T12, " +
                    "@R12_T13, " +
                    "@R12_T14, " +
                    "@R12_T15, " +
                    "@R12_T16, " +
                    "@R12_VIN, " +
                    "@R12_IIN, " +
                    "@R12_PIN, " +
                    "@R12_TJ, " +
                    "@MaxVal_T1, " +
                    "@MaxVal_T2, " +
                    "@MaxVal_T3, " +
                    "@MaxVal_T4, " +
                    "@MaxVal_T5, " +
                    "@MaxVal_T6, " +
                    "@MaxVal_T7, " +
                    "@MaxVal_T8, " +
                    "@MaxVal_T9, " +
                    "@MaxVal_T10, " +
                    "@MaxVal_T11, " +
                    "@MaxVal_T12, " +
                    "@MaxVal_T13, " +
                    "@MaxVal_T14, " +
                    "@MaxVal_T15, " +
                    "@MaxVal_T16, " +
                    "@MaxVal_TJ, " +
                    "@Conclusion_MaxRecordedTJ, " +
                    "@Conclusion_AllowableTJ, " +
                    "@Conclusion_MaxRecordedDriverTc, " +
                    "@Conclusion_AllowableDriverTc, " +
                    "@Conclusion_MaxRecordedLensTemp, " +
                    "@Conclusion_OverThermalCutoff, " +
                    "@Conclusion_Result, " +
                    "@ConductedBy, " +
                    "@WitnessBy, " +
                    "@ApprovedBy, " +
                    "@UpdatedBy, " +
                    "@UpdatedOn",
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

    public async Task<OperationResult> DeleteTemperatureRiseTestAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<TemperatureRiseTest>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
