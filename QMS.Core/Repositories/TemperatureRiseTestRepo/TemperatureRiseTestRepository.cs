using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ElectricalPerformanceRepo;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;
using System.Net.NetworkInformation;

namespace QMS.Core.Repositories.TemperatureRiseTestRepo
{
    public class TemperatureRiseTestRepository : SqlTableRepository, ITemperatureRiseTestRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public TemperatureRiseTestRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<TemperatureRiseTestViewModel>> GetTemperatureRiseTestAsync(DateTime? startDate = null, DateTime? endDate = null)
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
                        ProductCatRef = x.ProductCatRef,
                        ProductDescription = x.ProductDescription,
                        TestingLocation = x.TestingLocation,
                        HeatSinkMaterial = x.HeatSinkMaterial,
                        HeatSinkWeight = x.HeatSinkWeight,
                        LensDetails = x.LensDetails,
                        ThermalPasteDetails = x.ThermalPasteDetails,
                        //DriverDetails = x.DriverDetails,
                        //PCBDetails = x.PCBDetails,
                        //LEDCombinations = x.LEDCombinations,
                        BatchCode = x.BatchCode,
                        //SensorDetails = x.SensorDetails,
                        //LampDetails = x.LampDetails,
                        PKD = x.PKD,
                        AddedBy = x.AddedBy,
                        UpdatedBy = x.UpdatedBy,
                        AddedOn = x.AddedOn,
                        UpdatedOn = x.UpdatedOn
                    })
                    .ToList());

                if (startDate.HasValue && endDate.HasValue)
                {
                    var s = startDate.Value.Date;
                    var e = endDate.Value.Date;

                    result = result
                            .Where(d => d.AddedOn.Date >= s && d.AddedOn.Date <= e)
                            .ToList();
                }

                var userIds = result.Select(x => x.AddedBy).Distinct().ToList();

                var userMap = await _dbContext.User
                    .AsNoTracking()
                    .Where(u => userIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id, u => u.Name);

                foreach (var rec in result)
                {
                    // rec.User = await _dbContext.User.Where(i => i.Id == rec.AddedBy).Select(x => x.Name).FirstOrDefaultAsync();
                    rec.User = userMap.TryGetValue(rec.AddedBy, out var name) ? name : "";
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
            var model = new TemperatureRiseTestViewModel();

            using var connection = _dbContext.Database.GetDbConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_Get_TemperatureRiseTest";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Id", Id));

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            // -----------------------------
            // 1️⃣ FIRST RESULT SET (Parent)
            // -----------------------------
            if (await reader.ReadAsync())
            {
                model.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                model.ReportNo = reader["ReportNo"] as string;
                model.ReportDate = reader["ReportDate"] as DateTime?;
                model.TestingLocation = reader["TestingLocation"] as string;
                model.ProductCatRef = reader["ProductCatRef"] as string;
                model.ProductDescription = reader["ProductDescription"] as string;
                model.BatchCode = reader["BatchCode"] as string;
                model.PKD = reader["PKD"] as string;
                model.HeatSinkMaterial = reader["HeatSinkMaterial"] as string;
                model.HeatSinkWeight = reader["HeatSinkWeight"] as string;
                model.LensDetails = reader["LensDetails"] as string;
                model.ThermalPasteDetails = reader["ThermalPasteDetails"] as string;
                model.NominalOperatingVoltage = reader["NominalOperatingVoltage"] as decimal?;
                model.DriverUsed = reader["DriverUsed"] as string;
                model.NoOfDrivers = reader["NoOfDrivers"] as int?;
                model.DriverOutputVoltage = reader["DriverOutputVoltage"] as decimal?;
                model.DriverOutputCurrent = reader["DriverOutputCurrent"] as decimal?;
                model.DriverAllowableTc = reader["DriverAllowableTc"] as decimal?;
                model.DriverAllowableTa = reader["DriverAllowableTa"] as decimal?;
                model.PcbMaterialMake = reader["PcbMaterialMake"] as string;
                model.PcbSizeQty = reader["PcbSizeQty"] as string;
                model.LedUsed = reader["LedUsed"] as string;
                model.NoOfLeds = reader["NoOfLeds"] as int?;
                model.NoOfLedsInSeries = reader["NoOfLedsInSeries"] as int?;
                model.NoOfLedsInParallel = reader["NoOfLedsInParallel"] as int?;
                model.LedRjTH = reader["LedRjTH"] as decimal?;
                model.LedVf = reader["LedVf"] as decimal?;
                model.LedIf = reader["LedIf"] as decimal?;
                model.LedWdc = reader["LedWdc"] as decimal?;
                model.ProbeT1_Desc = reader["ProbeT1_Desc"] as string;
                model.ProbeT2_Desc = reader["ProbeT2_Desc"] as string;
                model.ProbeT3_Desc = reader["ProbeT3_Desc"] as string;
                model.ProbeT4_Desc = reader["ProbeT4_Desc"] as string;
                model.ProbeT5_Desc = reader["ProbeT5_Desc"] as string;
                model.ProbeT6_Desc = reader["ProbeT6_Desc"] as string;
                model.ProbeT7_Desc = reader["ProbeT7_Desc"] as string;
                model.ProbeT8_Desc = reader["ProbeT8_Desc"] as string;
                model.ProbeT9_Desc = reader["ProbeT9_Desc"] as string;
                model.ProbeT10_Desc = reader["ProbeT10_Desc"] as string;
                model.ProbeT11_Desc = reader["ProbeT11_Desc"] as string;
                model.ProbeT12_Desc = reader["ProbeT12_Desc"] as string;
                model.ProbeT13_Desc = reader["ProbeT13_Desc"] as string;
                model.ProbeT14_Desc = reader["ProbeT14_Desc"] as string;
                model.ProbeT15_Desc = reader["ProbeT15_Desc"] as string;
                model.ProbeT16_Desc = reader["ProbeT16_Desc"] as string;
                model.MaxVal_T2 = reader.GetDecimal(reader.GetOrdinal("Max_T2"));
                model.MaxVal_T3 = reader.GetDecimal(reader.GetOrdinal("Max_T3"));
                model.MaxVal_T4 = reader.GetDecimal(reader.GetOrdinal("Max_T4"));
                model.MaxVal_T5 = reader.GetDecimal(reader.GetOrdinal("Max_T5"));
                model.MaxVal_T6 = reader.GetDecimal(reader.GetOrdinal("Max_T6"));
                model.MaxVal_T7 = reader.GetDecimal(reader.GetOrdinal("Max_T7"));
                model.MaxVal_T8 = reader.GetDecimal(reader.GetOrdinal("Max_T8"));
                model.MaxVal_T9 = reader.GetDecimal(reader.GetOrdinal("Max_T9"));
                model.MaxVal_T10 = reader.GetDecimal(reader.GetOrdinal("Max_T10"));
                model.MaxVal_T11 = reader.GetDecimal(reader.GetOrdinal("Max_T11"));
                model.MaxVal_T12 = reader.GetDecimal(reader.GetOrdinal("Max_T12"));
                model.MaxVal_T13 = reader.GetDecimal(reader.GetOrdinal("Max_T13"));
                model.MaxVal_T14 = reader.GetDecimal(reader.GetOrdinal("Max_T14"));
                model.MaxVal_T15 = reader.GetDecimal(reader.GetOrdinal("Max_T15"));
                model.MaxVal_T16 = reader.GetDecimal(reader.GetOrdinal("Max_T16"));
                model.Conclusion_MaxRecordedTJ = reader["Conclusion_MaxRecordedTJ"] as decimal?;
                model.Conclusion_AllowableTJ = reader["Conclusion_AllowableTJ"] as decimal?;
                model.Conclusion_MaxRecordedDriverTc = reader["Conclusion_MaxRecordedDriverTc"] as decimal?;
                model.Conclusion_AllowableDriverTc = reader["Conclusion_AllowableDriverTc"] as decimal?;
                model.Conclusion_MaxRecordedLensTemp = reader["Conclusion_MaxRecordedLensTemp"] as decimal?;
                model.Conclusion_OverThermalCutoff = reader["Conclusion_OverThermalCutoff"] as string;
                model.Conclusion_Result = reader["Conclusion_Result"] as string;
                model.ConductedBy = reader["ConductedBy"] as string;
                model.WitnessBy = reader["WitnessBy"] as string;
                model.ApprovedBy = reader["ApprovedBy"] as string;
                model.ConductedBySecnd = reader["ConductedBySecnd"] as string;
                model.WitnessBySecnd = reader["WitnessBySecnd"] as string;
                model.ApprovedBySecnd = reader["ApprovedBySecnd"] as string;
                model.AddedBy = reader.GetInt32(reader.GetOrdinal("AddedBy"));
                model.AddedOn = reader.GetDateTime(reader.GetOrdinal("AddedOn"));
                model.UpdatedBy = reader["UpdatedBy"] as int?;
                model.UpdatedOn = reader["UpdatedOn"] as DateTime?;
                //model.User = reader["User"] as string;

            }

            // -----------------------------
            // 2️⃣ SECOND RESULT SET (Child)
            // -----------------------------
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())

                    
                    model.Details.Add(new TemperatureRiseDetailModal
                    {
                        TRId = reader.GetInt32(reader.GetOrdinal("TR_Id")),
                         TimeHrs = reader.GetDecimal(reader.GetOrdinal("TimeHrs")),
                         T1 = reader.GetDecimal(reader.GetOrdinal("T1")),
                         T2 = reader.GetDecimal(reader.GetOrdinal("T2")),
                         T3 = reader.GetDecimal(reader.GetOrdinal("T3")),
                         T4 = reader.GetDecimal(reader.GetOrdinal("T4")),
                         T5 = reader.GetDecimal(reader.GetOrdinal("T5")),
                         T6 = reader.GetDecimal(reader.GetOrdinal("T6")),
                         T7 = reader.GetDecimal(reader.GetOrdinal("T7")),
                         T8 = reader.GetDecimal(reader.GetOrdinal("T8")),
                         T9 = reader.GetDecimal(reader.GetOrdinal("T9")),
                         T10 = reader.GetDecimal(reader.GetOrdinal("T10")),
                         T11 = reader.GetDecimal(reader.GetOrdinal("T11")),
                         T12 = reader.GetDecimal(reader.GetOrdinal("T12")),
                         T13 = reader.GetDecimal(reader.GetOrdinal("T13")),
                         T14 = reader.GetDecimal(reader.GetOrdinal("T14")),
                         T15 = reader.GetDecimal(reader.GetOrdinal("T15")),
                         T16 = reader.GetDecimal(reader.GetOrdinal("T16")),
                         VIN = reader.GetDecimal(reader.GetOrdinal("VIN")),
                         IIN = reader.GetDecimal(reader.GetOrdinal("IIN")),
                         PIN = reader.GetDecimal(reader.GetOrdinal("PIN")),
                         TJ = reader.GetDecimal(reader.GetOrdinal("TJ")),
                    });
                }
            

            return model;
        }


        public async Task<OperationResult> InsertTemperatureRiseTestAsync(TemperatureRiseTestViewModel model)
        {
            try
            {
                var parameters = new[]
                {
                
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", System.Data.SqlDbType.DateTime)
                {
                    Value = model.ReportDate.HasValue ? model.ReportDate.Value : (object)DBNull.Value
                },
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

                new SqlParameter("@Max_T2", model.MaxVal_T2 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T3", model.MaxVal_T3 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T4", model.MaxVal_T4 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T5", model.MaxVal_T5 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T6", model.MaxVal_T6 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T7", model.MaxVal_T7 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T8", model.MaxVal_T8 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T9", model.MaxVal_T9 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T10", model.MaxVal_T10 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T11", model.MaxVal_T11 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T12", model.MaxVal_T12 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T13", model.MaxVal_T13 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T14", model.MaxVal_T14 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T15", model.MaxVal_T15 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T16", model.MaxVal_T16 ?? (object)DBNull.Value),

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
                new SqlParameter("@ConductedBySecnd", model.ConductedBySecnd ?? (object)DBNull.Value),
                new SqlParameter("@WitnessBySecnd", model.WitnessBySecnd ?? (object)DBNull.Value),
                new SqlParameter("@ApprovedBySecnd", model.ApprovedBySecnd ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", System.Data.SqlDbType.DateTime)
                {
                    Value = model.AddedOn
                },

                new SqlParameter("@User", model.User ?? (object)DBNull.Value),
                                    // ===== DETAILS STRING =====
                new SqlParameter("@TempRiseDetails", SqlDbType.NVarChar)
                {
                    Value = model.ThermalReadingsData ?? (object)DBNull.Value
                }
                    // ===== DETAILS STRING =====
                //new SqlParameter("@ElectricalDetails", SqlDbType.NVarChar)
                //{
                //    Value = model.ElectricalDetails ?? (object)DBNull.Value
                //}
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

                        "@Max_T2, " +
                        "@Max_T3, " +
                        "@Max_T4, " +
                        "@Max_T5, " +
                        "@Max_T6, " +
                        "@Max_T7, " +
                        "@Max_T8, " +
                        "@Max_T9, " +
                        "@Max_T10, " +
                        "@Max_T11, " +
                        "@Max_T12, " +
                        "@Max_T13, " +
                        "@Max_T14, " +
                        "@Max_T15, " +
                        "@Max_T16, " +

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
                        "@ConductedBySecnd, " +
                        "@WitnessBySecnd, " +
                        "@ApprovedBySecnd, " +
                        "@AddedBy, " +
                        "@AddedOn, " +
                        "@User, " +
                        "@TempRiseDetails",
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
                var parameters = new[] {
                new SqlParameter("@Id", model.Id),

                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", System.Data.SqlDbType.DateTime)
                {
                    Value = model.ReportDate.HasValue ? model.ReportDate.Value : (object)DBNull.Value
                },
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

                new SqlParameter("@Max_T2", model.MaxVal_T2 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T3", model.MaxVal_T3 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T4", model.MaxVal_T4 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T5", model.MaxVal_T5 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T6", model.MaxVal_T6 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T7", model.MaxVal_T7 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T8", model.MaxVal_T8 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T9", model.MaxVal_T9 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T10", model.MaxVal_T10 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T11", model.MaxVal_T11 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T12", model.MaxVal_T12 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T13", model.MaxVal_T13 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T14", model.MaxVal_T14 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T15", model.MaxVal_T15 ?? (object)DBNull.Value),
                new SqlParameter("@Max_T16", model.MaxVal_T16 ?? (object)DBNull.Value),

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
                new SqlParameter("@ConductedBySecnd", model.ConductedBySecnd ?? (object)DBNull.Value),
                new SqlParameter("@WitnessBySecnd", model.WitnessBySecnd ?? (object)DBNull.Value),
                new SqlParameter("@ApprovedBySecnd", model.ApprovedBySecnd ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy),
                new SqlParameter("@UpdatedOn", System.Data.SqlDbType.DateTime)
                {
                    Value = model.UpdatedOn
                },
            };

                await _dbContext.Database.ExecuteSqlRawAsync(
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
                        "@Max_T2, " +
                        "@Max_T3, " +
                        "@Max_T4, " +
                        "@Max_T5, " +
                        "@Max_T6, " +
                        "@Max_T7, " +
                        "@Max_T8, " +
                        "@Max_T9, " +
                        "@Max_T10, " +
                        "@Max_T11, " +
                        "@Max_T12, " +
                        "@Max_T13, " +
                        "@Max_T14, " +
                        "@Max_T15, " +
                        "@Max_T16, " +


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
                        "@ConductedBySecnd, " +
                        "@WitnessBySecnd, " +
                        "@ApprovedBySecnd, " +

                        "@UpdatedBy, " +
                        "@UpdatedOn, ",
                        
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
                var parameters = new[]
                {
                new SqlParameter("@TemperatureId", Id ),
            };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Delete_TemperatureRise " +
                        "@TemperatureId",
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

        //Task<List<TemperatureRiseTestViewModel>> ITemperatureRiseTestRepository.GetTemperatureRiseTestAsync(DateTime? startDate, DateTime? endDate)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<TemperatureRiseTestViewModel> ITemperatureRiseTestRepository.GetTemperatureRiseTestByIdAsync(int Id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}