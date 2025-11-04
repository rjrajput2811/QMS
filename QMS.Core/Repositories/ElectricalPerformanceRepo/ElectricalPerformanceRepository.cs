using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

namespace QMS.Core.Repositories.ElectricalPerformanceRepo;

public class ElectricalPerformanceRepository : SqlTableRepository, IElectricalPerformanceRepository
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public ElectricalPerformanceRepository(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<ElectricalPerformanceViewModel>> GetElectricalPerformancesAsync()
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

            var sql = @"EXEC sp_Get_ElectricalPerformance";

            var result = await Task.Run(() => _dbContext.ElectricalPerformances.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new ElectricalPerformanceViewModel
                {
                    Id = x.Id,
                    ReportNo = x.ReportNo,
                    ReportDate = x.ReportDate,
                    ProductCatRef = x.ProductCatRef,
                    ProductDescription = x.ProductDescription,
                    LightSourceDetails = x.LightSourceDetails,
                    DriverDetails = x.DriverDetails,
                    PCBDetails = x.PCBDetails,
                    LEDCombinations = x.LEDCombinations,
                    BatchCode = x.BatchCode,
                    SensorDetails = x.SensorDetails,
                    LampDetails = x.LampDetails,
                    PKD = x.PKD,
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

    public async Task<ElectricalPerformanceViewModel> GetElectricalPerformancesByIdAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_ElectricalPerformance";

            var result = await Task.Run(() => _dbContext.ElectricalPerformances.FromSqlRaw(sql, parameters)
               .AsEnumerable()
               .Select(x => new ElectricalPerformanceViewModel
               {
                   Id = x.Id,
                   ReportNo = x.ReportNo,
                   ReportDate = x.ReportDate,
                   ProductCatRef = x.ProductCatRef,
                   ProductDescription = x.ProductDescription,
                   LightSourceDetails = x.LightSourceDetails,
                   DriverDetails = x.DriverDetails,
                   PCBDetails = x.PCBDetails,
                   LEDCombinations = x.LEDCombinations,
                   BatchCode = x.BatchCode,
                   SensorDetails = x.SensorDetails,
                   LampDetails = x.LampDetails,
                   PKD = x.PKD,
                   S1_BS_Vac = x.S1_BS_Vac,
                   S1_BS_IacA = x.S1_BS_IacA,
                   S1_BS_Wac = x.S1_BS_Wac,
                   S1_BS_PF = x.S1_BS_PF,
                   S1_BS_ATHD = x.S1_BS_ATHD,
                   S1_BS_Vdc = x.S1_BS_Vdc,
                   S1_BS_IdcA = x.S1_BS_IdcA,
                   S1_BS_Wdc = x.S1_BS_Wdc,
                   S1_BS_Eff = x.S1_BS_Eff,
                   S1_BS_NoLoadV = x.S1_BS_NoLoadV,
                   S1_BS_StartV = x.S1_BS_StartV,
                   S1_BS_Result = x.S1_BS_Result,
                   S1_AS_Vac = x.S1_AS_Vac,
                   S1_AS_IacA = x.S1_AS_IacA,
                   S1_AS_Wac = x.S1_AS_Wac,
                   S1_AS_PF = x.S1_AS_PF,
                   S1_AS_ATHD = x.S1_AS_ATHD,
                   S1_AS_Vdc = x.S1_AS_Vdc,
                   S1_AS_IdcA = x.S1_AS_IdcA,
                   S1_AS_Wdc = x.S1_AS_Wdc,
                   S1_AS_Eff = x.S1_AS_Eff,
                   S1_AS_NoLoadV = x.S1_AS_NoLoadV,
                   S1_AS_StartV = x.S1_AS_StartV,
                   S1_AS_Result = x.S1_AS_Result,
                   S2_BS_Vac = x.S2_BS_Vac,
                   S2_BS_IacA = x.S2_BS_IacA,
                   S2_BS_Wac = x.S2_BS_Wac,
                   S2_BS_PF = x.S2_BS_PF,
                   S2_BS_ATHD = x.S2_BS_ATHD,
                   S2_BS_Vdc = x.S2_BS_Vdc,
                   S2_BS_IdcA = x.S2_BS_IdcA,
                   S2_BS_Wdc = x.S2_BS_Wdc,
                   S2_BS_Eff = x.S2_BS_Eff,
                   S2_BS_NoLoadV = x.S2_BS_NoLoadV,
                   S2_BS_StartV = x.S2_BS_StartV,
                   S2_BS_Result = x.S2_BS_Result,
                   S2_AS_Vac = x.S2_AS_Vac,
                   S2_AS_IacA = x.S2_AS_IacA,
                   S2_AS_Wac = x.S2_AS_Wac,
                   S2_AS_PF = x.S2_AS_PF,
                   S2_AS_ATHD = x.S2_AS_ATHD,
                   S2_AS_Vdc = x.S2_AS_Vdc,
                   S2_AS_IdcA = x.S2_AS_IdcA,
                   S2_AS_Wdc = x.S2_AS_Wdc,
                   S2_AS_Eff = x.S2_AS_Eff,
                   S2_AS_NoLoadV = x.S2_AS_NoLoadV,
                   S2_AS_StartV = x.S2_AS_StartV,
                   S2_AS_Result = x.S2_AS_Result,
                   S3_BS_Vac = x.S3_BS_Vac,
                   S3_BS_IacA = x.S3_BS_IacA,
                   S3_BS_Wac = x.S3_BS_Wac,
                   S3_BS_PF = x.S3_BS_PF,
                   S3_BS_ATHD = x.S3_BS_ATHD,
                   S3_BS_Vdc = x.S3_BS_Vdc,
                   S3_BS_IdcA = x.S3_BS_IdcA,
                   S3_BS_Wdc = x.S3_BS_Wdc,
                   S3_BS_Eff = x.S3_BS_Eff,
                   S3_BS_NoLoadV = x.S3_BS_NoLoadV,
                   S3_BS_StartV = x.S3_BS_StartV,
                   S3_BS_Result = x.S3_BS_Result,
                   S3_AS_Vac = x.S3_AS_Vac,
                   S3_AS_IacA = x.S3_AS_IacA,
                   S3_AS_Wac = x.S3_AS_Wac,
                   S3_AS_PF = x.S3_AS_PF,
                   S3_AS_ATHD = x.S3_AS_ATHD,
                   S3_AS_Vdc = x.S3_AS_Vdc,
                   S3_AS_IdcA = x.S3_AS_IdcA,
                   S3_AS_Wdc = x.S3_AS_Wdc,
                   S3_AS_Eff = x.S3_AS_Eff,
                   S3_AS_NoLoadV = x.S3_AS_NoLoadV,
                   S3_AS_StartV = x.S3_AS_StartV,
                   S3_AS_Result = x.S3_AS_Result,
                   S4_BS_Vac = x.S4_BS_Vac,
                   S4_BS_IacA = x.S4_BS_IacA,
                   S4_BS_Wac = x.S4_BS_Wac,
                   S4_BS_PF = x.S4_BS_PF,
                   S4_BS_ATHD = x.S4_BS_ATHD,
                   S4_BS_Vdc = x.S4_BS_Vdc,
                   S4_BS_IdcA = x.S4_BS_IdcA,
                   S4_BS_Wdc = x.S4_BS_Wdc,
                   S4_BS_Eff = x.S4_BS_Eff,
                   S4_BS_NoLoadV = x.S4_BS_NoLoadV,
                   S4_BS_StartV = x.S4_BS_StartV,
                   S4_BS_Result = x.S4_BS_Result,
                   S4_AS_Vac = x.S4_AS_Vac,
                   S4_AS_IacA = x.S4_AS_IacA,
                   S4_AS_Wac = x.S4_AS_Wac,
                   S4_AS_PF = x.S4_AS_PF,
                   S4_AS_ATHD = x.S4_AS_ATHD,
                   S4_AS_Vdc = x.S4_AS_Vdc,
                   S4_AS_IdcA = x.S4_AS_IdcA,
                   S4_AS_Wdc = x.S4_AS_Wdc,
                   S4_AS_Eff = x.S4_AS_Eff,
                   S4_AS_NoLoadV = x.S4_AS_NoLoadV,
                   S4_AS_StartV = x.S4_AS_StartV,
                   S4_AS_Result = x.S4_AS_Result,
                   S5_BS_Vac = x.S5_BS_Vac,
                   S5_BS_IacA = x.S5_BS_IacA,
                   S5_BS_Wac = x.S5_BS_Wac,
                   S5_BS_PF = x.S5_BS_PF,
                   S5_BS_ATHD = x.S5_BS_ATHD,
                   S5_BS_Vdc = x.S5_BS_Vdc,
                   S5_BS_IdcA = x.S5_BS_IdcA,
                   S5_BS_Wdc = x.S5_BS_Wdc,
                   S5_BS_Eff = x.S5_BS_Eff,
                   S5_BS_NoLoadV = x.S5_BS_NoLoadV,
                   S5_BS_StartV = x.S5_BS_StartV,
                   S5_BS_Result = x.S5_BS_Result,
                   S5_AS_Vac = x.S5_AS_Vac,
                   S5_AS_IacA = x.S5_AS_IacA,
                   S5_AS_Wac = x.S5_AS_Wac,
                   S5_AS_PF = x.S5_AS_PF,
                   S5_AS_ATHD = x.S5_AS_ATHD,
                   S5_AS_Vdc = x.S5_AS_Vdc,
                   S5_AS_IdcA = x.S5_AS_IdcA,
                   S5_AS_Wdc = x.S5_AS_Wdc,
                   S5_AS_Eff = x.S5_AS_Eff,
                   S5_AS_NoLoadV = x.S5_AS_NoLoadV,
                   S5_AS_StartV = x.S5_AS_StartV,
                   S5_AS_Result = x.S5_AS_Result,
                   OverallResult = x.OverallResult,
                   TestedByName = x.TestedByName,
                   VerifiedByName = x.VerifiedByName
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

    public async Task<OperationResult> InsertElectricalPerformancesAsync(ElectricalPerformanceViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@LightSourceDetails", model.LightSourceDetails ?? (object)DBNull.Value),
                new SqlParameter("@DriverDetails", model.DriverDetails ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                new SqlParameter("@PCBDetails", model.PCBDetails ?? (object)DBNull.Value),
                new SqlParameter("@LEDCombinations", model.LEDCombinations ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@SensorDetails", model.SensorDetails ?? (object)DBNull.Value),
                new SqlParameter("@LampDetails", model.LampDetails ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Vac", model.S1_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_IacA", model.S1_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Wac", model.S1_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_PF", model.S1_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_ATHD", model.S1_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Vdc", model.S1_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_IdcA", model.S1_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Wdc", model.S1_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Eff", model.S1_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_NoLoadV", model.S1_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_StartV", model.S1_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Result", model.S1_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Vac", model.S1_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_IacA", model.S1_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Wac", model.S1_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_PF", model.S1_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_ATHD", model.S1_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Vdc", model.S1_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_IdcA", model.S1_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Wdc", model.S1_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Eff", model.S1_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_NoLoadV", model.S1_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_StartV", model.S1_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Result", model.S1_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Vac", model.S2_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_IacA", model.S2_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Wac", model.S2_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_PF", model.S2_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_ATHD", model.S2_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Vdc", model.S2_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_IdcA", model.S2_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Wdc", model.S2_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Eff", model.S2_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_NoLoadV", model.S2_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_StartV", model.S2_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Result", model.S2_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Vac", model.S2_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_IacA", model.S2_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Wac", model.S2_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_PF", model.S2_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_ATHD", model.S2_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Vdc", model.S2_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_IdcA", model.S2_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Wdc", model.S2_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Eff", model.S2_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_NoLoadV", model.S2_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_StartV", model.S2_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Result", model.S2_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Vac", model.S3_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_IacA", model.S3_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Wac", model.S3_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_PF", model.S3_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_ATHD", model.S3_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Vdc", model.S3_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_IdcA", model.S3_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Wdc", model.S3_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Eff", model.S3_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_NoLoadV", model.S3_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_StartV", model.S3_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Result", model.S3_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Vac", model.S3_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_IacA", model.S3_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Wac", model.S3_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_PF", model.S3_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_ATHD", model.S3_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Vdc", model.S3_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_IdcA", model.S3_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Wdc", model.S3_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Eff", model.S3_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_NoLoadV", model.S3_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_StartV", model.S3_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Result", model.S3_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Vac", model.S4_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_IacA", model.S4_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Wac", model.S4_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_PF", model.S4_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_ATHD", model.S4_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Vdc", model.S4_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_IdcA", model.S4_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Wdc", model.S4_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Eff", model.S4_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_NoLoadV", model.S4_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_StartV", model.S4_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Result", model.S4_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Vac", model.S4_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_IacA", model.S4_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Wac", model.S4_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_PF", model.S4_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_ATHD", model.S4_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Vdc", model.S4_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_IdcA", model.S4_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Wdc", model.S4_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Eff", model.S4_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_NoLoadV", model.S4_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_StartV", model.S4_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Result", model.S4_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Vac", model.S5_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_IacA", model.S5_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Wac", model.S5_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_PF", model.S5_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_ATHD", model.S5_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Vdc", model.S5_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_IdcA", model.S5_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Wdc", model.S5_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Eff", model.S5_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_NoLoadV", model.S5_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_StartV", model.S5_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Result", model.S5_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Vac", model.S5_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_IacA", model.S5_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Wac", model.S5_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_PF", model.S5_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_ATHD", model.S5_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Vdc", model.S5_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_IdcA", model.S5_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Wdc", model.S5_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Eff", model.S5_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_NoLoadV", model.S5_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_StartV", model.S5_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Result", model.S5_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                new SqlParameter("@TestedByName", model.TestedByName ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedByName", model.VerifiedByName ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_ElectricalPerformance " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@ReportNo, " +
                    "@LightSourceDetails, " +
                    "@DriverDetails, " +
                    "@ReportDate, " +
                    "@PCBDetails, " +
                    "@LEDCombinations, " +
                    "@BatchCode, " +
                    "@SensorDetails, " +
                    "@LampDetails, " +
                    "@PKD, " +
                    "@S1_BS_Vac, " +
                    "@S1_BS_IacA, " +
                    "@S1_BS_Wac, " +
                    "@S1_BS_PF, " +
                    "@S1_BS_ATHD, " +
                    "@S1_BS_Vdc, " +
                    "@S1_BS_IdcA, " +
                    "@S1_BS_Wdc, " +
                    "@S1_BS_Eff, " +
                    "@S1_BS_NoLoadV, " +
                    "@S1_BS_StartV, " +
                    "@S1_BS_Result, " +
                    "@S1_AS_Vac, " +
                    "@S1_AS_IacA, " +
                    "@S1_AS_Wac, " +
                    "@S1_AS_PF, " +
                    "@S1_AS_ATHD, " +
                    "@S1_AS_Vdc, " +
                    "@S1_AS_IdcA, " +
                    "@S1_AS_Wdc, " +
                    "@S1_AS_Eff, " +
                    "@S1_AS_NoLoadV, " +
                    "@S1_AS_StartV, " +
                    "@S1_AS_Result, " +
                    "@S2_BS_Vac, " +
                    "@S2_BS_IacA, " +
                    "@S2_BS_Wac, " +
                    "@S2_BS_PF, " +
                    "@S2_BS_ATHD, " +
                    "@S2_BS_Vdc, " +
                    "@S2_BS_IdcA, " +
                    "@S2_BS_Wdc, " +
                    "@S2_BS_Eff, " +
                    "@S2_BS_NoLoadV, " +
                    "@S2_BS_StartV, " +
                    "@S2_BS_Result, " +
                    "@S2_AS_Vac, " +
                    "@S2_AS_IacA, " +
                    "@S2_AS_Wac, " +
                    "@S2_AS_PF, " +
                    "@S2_AS_ATHD, " +
                    "@S2_AS_Vdc, " +
                    "@S2_AS_IdcA, " +
                    "@S2_AS_Wdc, " +
                    "@S2_AS_Eff, " +
                    "@S2_AS_NoLoadV, " +
                    "@S2_AS_StartV, " +
                    "@S2_AS_Result, " +
                    "@S3_BS_Vac, " +
                    "@S3_BS_IacA, " +
                    "@S3_BS_Wac, " +
                    "@S3_BS_PF, " +
                    "@S3_BS_ATHD, " +
                    "@S3_BS_Vdc, " +
                    "@S3_BS_IdcA, " +
                    "@S3_BS_Wdc, " +
                    "@S3_BS_Eff, " +
                    "@S3_BS_NoLoadV, " +
                    "@S3_BS_StartV, " +
                    "@S3_BS_Result, " +
                    "@S3_AS_Vac, " +
                    "@S3_AS_IacA, " +
                    "@S3_AS_Wac, " +
                    "@S3_AS_PF, " +
                    "@S3_AS_ATHD, " +
                    "@S3_AS_Vdc, " +
                    "@S3_AS_IdcA, " +
                    "@S3_AS_Wdc, " +
                    "@S3_AS_Eff, " +
                    "@S3_AS_NoLoadV, " +
                    "@S3_AS_StartV, " +
                    "@S3_AS_Result, " +
                    "@S4_BS_Vac, " +
                    "@S4_BS_IacA, " +
                    "@S4_BS_Wac, " +
                    "@S4_BS_PF, " +
                    "@S4_BS_ATHD, " +
                    "@S4_BS_Vdc, " +
                    "@S4_BS_IdcA, " +
                    "@S4_BS_Wdc, " +
                    "@S4_BS_Eff, " +
                    "@S4_BS_NoLoadV, " +
                    "@S4_BS_StartV, " +
                    "@S4_BS_Result, " +
                    "@S4_AS_Vac, " +
                    "@S4_AS_IacA, " +
                    "@S4_AS_Wac, " +
                    "@S4_AS_PF, " +
                    "@S4_AS_ATHD, " +
                    "@S4_AS_Vdc, " +
                    "@S4_AS_IdcA, " +
                    "@S4_AS_Wdc, " +
                    "@S4_AS_Eff, " +
                    "@S4_AS_NoLoadV, " +
                    "@S4_AS_StartV, " +
                    "@S4_AS_Result, " +
                    "@S5_BS_Vac, " +
                    "@S5_BS_IacA, " +
                    "@S5_BS_Wac, " +
                    "@S5_BS_PF, " +
                    "@S5_BS_ATHD, " +
                    "@S5_BS_Vdc, " +
                    "@S5_BS_IdcA, " +
                    "@S5_BS_Wdc, " +
                    "@S5_BS_Eff, " +
                    "@S5_BS_NoLoadV, " +
                    "@S5_BS_StartV, " +
                    "@S5_BS_Result, " +
                    "@S5_AS_Vac, " +
                    "@S5_AS_IacA, " +
                    "@S5_AS_Wac, " +
                    "@S5_AS_PF, " +
                    "@S5_AS_ATHD, " +
                    "@S5_AS_Vdc, " +
                    "@S5_AS_IdcA, " +
                    "@S5_AS_Wdc, " +
                    "@S5_AS_Eff, " +
                    "@S5_AS_NoLoadV, " +
                    "@S5_AS_StartV, " +
                    "@S5_AS_Result, " +
                    "@OverallResult, " +
                    "@TestedByName, " +
                    "@VerifiedByName, " +
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

    public async Task<OperationResult> UpdateElectricalPerformancesAsync(ElectricalPerformanceViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@ProductCatRef", model.ProductCatRef ?? (object)DBNull.Value),
                new SqlParameter("@ProductDescription", model.ProductDescription ?? (object)DBNull.Value),
                new SqlParameter("@ReportNo", model.ReportNo ?? (object)DBNull.Value),
                new SqlParameter("@LightSourceDetails", model.LightSourceDetails ?? (object)DBNull.Value),
                new SqlParameter("@DriverDetails", model.DriverDetails ?? (object)DBNull.Value),
                new SqlParameter("@ReportDate", model.ReportDate ?? (object)DBNull.Value),
                new SqlParameter("@PCBDetails", model.PCBDetails ?? (object)DBNull.Value),
                new SqlParameter("@LEDCombinations", model.LEDCombinations ?? (object)DBNull.Value),
                new SqlParameter("@BatchCode", model.BatchCode ?? (object)DBNull.Value),
                new SqlParameter("@SensorDetails", model.SensorDetails ?? (object)DBNull.Value),
                new SqlParameter("@LampDetails", model.LampDetails ?? (object)DBNull.Value),
                new SqlParameter("@PKD", model.PKD ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Vac", model.S1_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_IacA", model.S1_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Wac", model.S1_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_PF", model.S1_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_ATHD", model.S1_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Vdc", model.S1_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_IdcA", model.S1_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Wdc", model.S1_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Eff", model.S1_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_NoLoadV", model.S1_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_StartV", model.S1_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S1_BS_Result", model.S1_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Vac", model.S1_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_IacA", model.S1_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Wac", model.S1_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_PF", model.S1_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_ATHD", model.S1_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Vdc", model.S1_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_IdcA", model.S1_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Wdc", model.S1_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Eff", model.S1_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_NoLoadV", model.S1_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_StartV", model.S1_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S1_AS_Result", model.S1_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Vac", model.S2_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_IacA", model.S2_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Wac", model.S2_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_PF", model.S2_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_ATHD", model.S2_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Vdc", model.S2_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_IdcA", model.S2_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Wdc", model.S2_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Eff", model.S2_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_NoLoadV", model.S2_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_StartV", model.S2_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S2_BS_Result", model.S2_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Vac", model.S2_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_IacA", model.S2_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Wac", model.S2_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_PF", model.S2_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_ATHD", model.S2_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Vdc", model.S2_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_IdcA", model.S2_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Wdc", model.S2_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Eff", model.S2_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_NoLoadV", model.S2_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_StartV", model.S2_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S2_AS_Result", model.S2_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Vac", model.S3_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_IacA", model.S3_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Wac", model.S3_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_PF", model.S3_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_ATHD", model.S3_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Vdc", model.S3_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_IdcA", model.S3_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Wdc", model.S3_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Eff", model.S3_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_NoLoadV", model.S3_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_StartV", model.S3_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S3_BS_Result", model.S3_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Vac", model.S3_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_IacA", model.S3_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Wac", model.S3_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_PF", model.S3_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_ATHD", model.S3_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Vdc", model.S3_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_IdcA", model.S3_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Wdc", model.S3_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Eff", model.S3_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_NoLoadV", model.S3_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_StartV", model.S3_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S3_AS_Result", model.S3_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Vac", model.S4_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_IacA", model.S4_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Wac", model.S4_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_PF", model.S4_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_ATHD", model.S4_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Vdc", model.S4_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_IdcA", model.S4_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Wdc", model.S4_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Eff", model.S4_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_NoLoadV", model.S4_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_StartV", model.S4_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S4_BS_Result", model.S4_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Vac", model.S4_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_IacA", model.S4_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Wac", model.S4_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_PF", model.S4_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_ATHD", model.S4_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Vdc", model.S4_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_IdcA", model.S4_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Wdc", model.S4_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Eff", model.S4_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_NoLoadV", model.S4_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_StartV", model.S4_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S4_AS_Result", model.S4_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Vac", model.S5_BS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_IacA", model.S5_BS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Wac", model.S5_BS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_PF", model.S5_BS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_ATHD", model.S5_BS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Vdc", model.S5_BS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_IdcA", model.S5_BS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Wdc", model.S5_BS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Eff", model.S5_BS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_NoLoadV", model.S5_BS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_StartV", model.S5_BS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S5_BS_Result", model.S5_BS_Result ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Vac", model.S5_AS_Vac ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_IacA", model.S5_AS_IacA ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Wac", model.S5_AS_Wac ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_PF", model.S5_AS_PF ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_ATHD", model.S5_AS_ATHD ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Vdc", model.S5_AS_Vdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_IdcA", model.S5_AS_IdcA ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Wdc", model.S5_AS_Wdc ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Eff", model.S5_AS_Eff ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_NoLoadV", model.S5_AS_NoLoadV ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_StartV", model.S5_AS_StartV ?? (object)DBNull.Value),
                new SqlParameter("@S5_AS_Result", model.S5_AS_Result ?? (object)DBNull.Value),
                new SqlParameter("@OverallResult", model.OverallResult ?? (object)DBNull.Value),
                new SqlParameter("@TestedByName", model.TestedByName ?? (object)DBNull.Value),
                new SqlParameter("@VerifiedByName", model.VerifiedByName ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_ElectricalPerformance " +
                    "@Id, " +
                    "@ProductCatRef, " +
                    "@ProductDescription, " +
                    "@ReportNo, " +
                    "@LightSourceDetails, " +
                    "@DriverDetails, " +
                    "@ReportDate, " +
                    "@PCBDetails, " +
                    "@LEDCombinations, " +
                    "@BatchCode, " +
                    "@SensorDetails, " +
                    "@LampDetails, " +
                    "@PKD, " +
                    "@S1_BS_Vac, " +
                    "@S1_BS_IacA, " +
                    "@S1_BS_Wac, " +
                    "@S1_BS_PF, " +
                    "@S1_BS_ATHD, " +
                    "@S1_BS_Vdc, " +
                    "@S1_BS_IdcA, " +
                    "@S1_BS_Wdc, " +
                    "@S1_BS_Eff, " +
                    "@S1_BS_NoLoadV, " +
                    "@S1_BS_StartV, " +
                    "@S1_BS_Result, " +
                    "@S1_AS_Vac, " +
                    "@S1_AS_IacA, " +
                    "@S1_AS_Wac, " +
                    "@S1_AS_PF, " +
                    "@S1_AS_ATHD, " +
                    "@S1_AS_Vdc, " +
                    "@S1_AS_IdcA, " +
                    "@S1_AS_Wdc, " +
                    "@S1_AS_Eff, " +
                    "@S1_AS_NoLoadV, " +
                    "@S1_AS_StartV, " +
                    "@S1_AS_Result, " +
                    "@S2_BS_Vac, " +
                    "@S2_BS_IacA, " +
                    "@S2_BS_Wac, " +
                    "@S2_BS_PF, " +
                    "@S2_BS_ATHD, " +
                    "@S2_BS_Vdc, " +
                    "@S2_BS_IdcA, " +
                    "@S2_BS_Wdc, " +
                    "@S2_BS_Eff, " +
                    "@S2_BS_NoLoadV, " +
                    "@S2_BS_StartV, " +
                    "@S2_BS_Result, " +
                    "@S2_AS_Vac, " +
                    "@S2_AS_IacA, " +
                    "@S2_AS_Wac, " +
                    "@S2_AS_PF, " +
                    "@S2_AS_ATHD, " +
                    "@S2_AS_Vdc, " +
                    "@S2_AS_IdcA, " +
                    "@S2_AS_Wdc, " +
                    "@S2_AS_Eff, " +
                    "@S2_AS_NoLoadV, " +
                    "@S2_AS_StartV, " +
                    "@S2_AS_Result, " +
                    "@S3_BS_Vac, " +
                    "@S3_BS_IacA, " +
                    "@S3_BS_Wac, " +
                    "@S3_BS_PF, " +
                    "@S3_BS_ATHD, " +
                    "@S3_BS_Vdc, " +
                    "@S3_BS_IdcA, " +
                    "@S3_BS_Wdc, " +
                    "@S3_BS_Eff, " +
                    "@S3_BS_NoLoadV, " +
                    "@S3_BS_StartV, " +
                    "@S3_BS_Result, " +
                    "@S3_AS_Vac, " +
                    "@S3_AS_IacA, " +
                    "@S3_AS_Wac, " +
                    "@S3_AS_PF, " +
                    "@S3_AS_ATHD, " +
                    "@S3_AS_Vdc, " +
                    "@S3_AS_IdcA, " +
                    "@S3_AS_Wdc, " +
                    "@S3_AS_Eff, " +
                    "@S3_AS_NoLoadV, " +
                    "@S3_AS_StartV, " +
                    "@S3_AS_Result, " +
                    "@S4_BS_Vac, " +
                    "@S4_BS_IacA, " +
                    "@S4_BS_Wac, " +
                    "@S4_BS_PF, " +
                    "@S4_BS_ATHD, " +
                    "@S4_BS_Vdc, " +
                    "@S4_BS_IdcA, " +
                    "@S4_BS_Wdc, " +
                    "@S4_BS_Eff, " +
                    "@S4_BS_NoLoadV, " +
                    "@S4_BS_StartV, " +
                    "@S4_BS_Result, " +
                    "@S4_AS_Vac, " +
                    "@S4_AS_IacA, " +
                    "@S4_AS_Wac, " +
                    "@S4_AS_PF, " +
                    "@S4_AS_ATHD, " +
                    "@S4_AS_Vdc, " +
                    "@S4_AS_IdcA, " +
                    "@S4_AS_Wdc, " +
                    "@S4_AS_Eff, " +
                    "@S4_AS_NoLoadV, " +
                    "@S4_AS_StartV, " +
                    "@S4_AS_Result, " +
                    "@S5_BS_Vac, " +
                    "@S5_BS_IacA, " +
                    "@S5_BS_Wac, " +
                    "@S5_BS_PF, " +
                    "@S5_BS_ATHD, " +
                    "@S5_BS_Vdc, " +
                    "@S5_BS_IdcA, " +
                    "@S5_BS_Wdc, " +
                    "@S5_BS_Eff, " +
                    "@S5_BS_NoLoadV, " +
                    "@S5_BS_StartV, " +
                    "@S5_BS_Result, " +
                    "@S5_AS_Vac, " +
                    "@S5_AS_IacA, " +
                    "@S5_AS_Wac, " +
                    "@S5_AS_PF, " +
                    "@S5_AS_ATHD, " +
                    "@S5_AS_Vdc, " +
                    "@S5_AS_IdcA, " +
                    "@S5_AS_Wdc, " +
                    "@S5_AS_Eff, " +
                    "@S5_AS_NoLoadV, " +
                    "@S5_AS_StartV, " +
                    "@S5_AS_Result, " +
                    "@OverallResult, " +
                    "@TestedByName, " +
                    "@VerifiedByName, " +
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

    public async Task<OperationResult> DeleteElectricalPerformancesAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<ElectricalPerformance>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
