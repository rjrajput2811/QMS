using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System.Data;

namespace QMS.Core.Repositories.AHPNoteReposotory;

public class AHPNoteReposotory : SqlTableRepository, IAHPNoteReposotory
{
    private new readonly QMSDbContext _dbContext;
    private readonly ISystemLogService _systemLogService;

    public AHPNoteReposotory(QMSDbContext dbContext, ISystemLogService systemLogService) : base(dbContext)
    {
        _dbContext = dbContext;
        _systemLogService = systemLogService;
    }

    public async Task<List<AHPNoteViewModel>> GetAHPNotesAsync()
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

            var sql = @"EXEC sp_Get_AHPNote";

            var result = await Task.Run(() => _dbContext.AHPNote.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new AHPNoteViewModel
                {
                    Id = x.Id,
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

    public async Task<AHPNoteViewModel> GetAHPNotesByIdAsync(int Id)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", Id),
            };

            var sql = @"EXEC sp_Get_AHPNote";

            var result = await Task.Run(() => _dbContext.AHPNote.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new AHPNoteViewModel
                {
                    Id = x.Id,
                    Lit_SEE_Engagement_Target = x.Lit_SEE_Engagement_Target,
                    Lit_SEE_Engagement_FY23_24_Q1 = x.Lit_SEE_Engagement_FY23_24_Q1,
                    Lit_SEE_Engagement_FY23_24_Q2 = x.Lit_SEE_Engagement_FY23_24_Q2,
                    Lit_SEE_Engagement_FY23_24_Q3 = x.Lit_SEE_Engagement_FY23_24_Q3,
                    Lit_SEE_Engagement_FY23_24_Q4 = x.Lit_SEE_Engagement_FY23_24_Q4,
                    Lit_SEE_Engagement_FY24_25_Q1 = x.Lit_SEE_Engagement_FY24_25_Q1,
                    Lit_SEE_Engagement_FY24_25_Q2 = x.Lit_SEE_Engagement_FY24_25_Q2,
                    Lit_SEE_Effectiveness_Target = x.Lit_SEE_Effectiveness_Target,
                    Lit_SEE_Effectiveness_FY23_24_Q1 = x.Lit_SEE_Effectiveness_FY23_24_Q1,
                    Lit_SEE_Effectiveness_FY23_24_Q2 = x.Lit_SEE_Effectiveness_FY23_24_Q2,
                    Lit_SEE_Effectiveness_FY23_24_Q3 = x.Lit_SEE_Effectiveness_FY23_24_Q3,
                    Lit_SEE_Effectiveness_FY23_24_Q4 = x.Lit_SEE_Effectiveness_FY23_24_Q4,
                    Lit_SEE_Effectiveness_FY24_25_Q1 = x.Lit_SEE_Effectiveness_FY24_25_Q1,
                    Lit_SEE_Effectiveness_FY24_25_Q2 = x.Lit_SEE_Effectiveness_FY24_25_Q2,
                    Lit_SS_ServiceComplaints_Baseline = x.Lit_SS_ServiceComplaints_Baseline,
                    Lit_SS_ServiceComplaints_Target = x.Lit_SS_ServiceComplaints_Target,
                    Lit_SS_ServiceComplaints_Q1 = x.Lit_SS_ServiceComplaints_Q1,
                    Lit_SS_ServiceComplaints_Q2 = x.Lit_SS_ServiceComplaints_Q2,
                    Lit_SS_DesignLSG_Baseline = x.Lit_SS_DesignLSG_Baseline,
                    Lit_SS_DesignLSG_Target = x.Lit_SS_DesignLSG_Target,
                    Lit_SS_DesignLSG_Q1 = x.Lit_SS_DesignLSG_Q1,
                    Lit_SS_DesignLSG_Q2 = x.Lit_SS_DesignLSG_Q2,
                    Lit_SS_CostReduction_Baseline = x.Lit_SS_CostReduction_Baseline,
                    Lit_SS_CostReduction_Target = x.Lit_SS_CostReduction_Target,
                    Lit_SS_CostReduction_Q1 = x.Lit_SS_CostReduction_Q1,
                    Lit_SS_CostReduction_Q2 = x.Lit_SS_CostReduction_Q2,
                    Lit_SS_OTIF_Baseline = x.Lit_SS_OTIF_Baseline,
                    Lit_SS_OTIF_Target = x.Lit_SS_OTIF_Target,
                    Lit_SS_OTIF_Q1 = x.Lit_SS_OTIF_Q1,
                    Lit_SS_OTIF_Q2 = x.Lit_SS_OTIF_Q2,
                    Lit_SS_SPMScore_Baseline = x.Lit_SS_SPMScore_Baseline,
                    Lit_SS_SPMScore_Target = x.Lit_SS_SPMScore_Target,
                    Lit_SS_SPMScore_Q1 = x.Lit_SS_SPMScore_Q1,
                    Lit_SS_SPMScore_Q2 = x.Lit_SS_SPMScore_Q2,
                    Lit_CSAT_ReqSent_YTD23_24 = x.Lit_CSAT_ReqSent_YTD23_24,
                    Lit_CSAT_ReqSent_Baseline = x.Lit_CSAT_ReqSent_Baseline,
                    Lit_CSAT_ReqSent_Target = x.Lit_CSAT_ReqSent_Target,
                    Lit_CSAT_ReqSent_Q1 = x.Lit_CSAT_ReqSent_Q1,
                    Lit_CSAT_ReqSent_Q2 = x.Lit_CSAT_ReqSent_Q2,
                    Lit_CSAT_ReqSent_YTD = x.Lit_CSAT_ReqSent_YTD,
                    Lit_CSAT_RespRecvd_YTD23_24 = x.Lit_CSAT_RespRecvd_YTD23_24,
                    Lit_CSAT_RespRecvd_Baseline = x.Lit_CSAT_RespRecvd_Baseline,
                    Lit_CSAT_RespRecvd_Target = x.Lit_CSAT_RespRecvd_Target,
                    Lit_CSAT_RespRecvd_Q1 = x.Lit_CSAT_RespRecvd_Q1,
                    Lit_CSAT_RespRecvd_Q2 = x.Lit_CSAT_RespRecvd_Q2,
                    Lit_CSAT_RespRecvd_YTD = x.Lit_CSAT_RespRecvd_YTD,
                    Lit_CSAT_Promoter_YTD23_24 = x.Lit_CSAT_Promoter_YTD23_24,
                    Lit_CSAT_Promoter_Baseline = x.Lit_CSAT_Promoter_Baseline,
                    Lit_CSAT_Promoter_Target = x.Lit_CSAT_Promoter_Target,
                    Lit_CSAT_Promoter_Q1 = x.Lit_CSAT_Promoter_Q1,
                    Lit_CSAT_Promoter_Q2 = x.Lit_CSAT_Promoter_Q2,
                    Lit_CSAT_Promoter_YTD = x.Lit_CSAT_Promoter_YTD,
                    Lit_CSAT_Detractor_YTD23_24 = x.Lit_CSAT_Detractor_YTD23_24,
                    Lit_CSAT_Detractor_Baseline = x.Lit_CSAT_Detractor_Baseline,
                    Lit_CSAT_Detractor_Target = x.Lit_CSAT_Detractor_Target,
                    Lit_CSAT_Detractor_Q1 = x.Lit_CSAT_Detractor_Q1,
                    Lit_CSAT_Detractor_Q2 = x.Lit_CSAT_Detractor_Q2,
                    Lit_CSAT_Detractor_YTD = x.Lit_CSAT_Detractor_YTD,
                    Lit_CSAT_NPS_YTD23_24 = x.Lit_CSAT_NPS_YTD23_24,
                    Lit_CSAT_NPS_Baseline = x.Lit_CSAT_NPS_Baseline,
                    Lit_CSAT_NPS_Target = x.Lit_CSAT_NPS_Target,
                    Lit_CSAT_NPS_Q1 = x.Lit_CSAT_NPS_Q1,
                    Lit_CSAT_NPS_Q2 = x.Lit_CSAT_NPS_Q2,
                    Lit_CSAT_NPS_YTD = x.Lit_CSAT_NPS_YTD,
                    Lit_SPM_Supp1_Q1 = x.Lit_SPM_Supp1_Q1,
                    Lit_SPM_Supp1_Q2 = x.Lit_SPM_Supp1_Q2,
                    Lit_SPM_Supp2_Q1 = x.Lit_SPM_Supp2_Q1,
                    Lit_SPM_Supp2_Q2 = x.Lit_SPM_Supp2_Q2,
                    Lit_SPM_Supp3_Q1 = x.Lit_SPM_Supp3_Q1,
                    Lit_SPM_Supp3_Q2 = x.Lit_SPM_Supp3_Q2,
                    Lit_SPM_Supp4_Q1 = x.Lit_SPM_Supp4_Q1,
                    Lit_SPM_Supp4_Q2 = x.Lit_SPM_Supp4_Q2,
                    Lit_SPM_Supp5_Q1 = x.Lit_SPM_Supp5_Q1,
                    Lit_SPM_Supp5_Q2 = x.Lit_SPM_Supp5_Q2,
                    Lit_SPM_Supp6_Q1 = x.Lit_SPM_Supp6_Q1,
                    Lit_SPM_Supp6_Q2 = x.Lit_SPM_Supp6_Q2,
                    Lit_SPM_Supp7_Q1 = x.Lit_SPM_Supp7_Q1,
                    Lit_SPM_Supp7_Q2 = x.Lit_SPM_Supp7_Q2,
                    Lit_SPM_Supp8_Q1 = x.Lit_SPM_Supp8_Q1,
                    Lit_SPM_Supp8_Q2 = x.Lit_SPM_Supp8_Q2,
                    Lit_SPM_Supp9_Q1 = x.Lit_SPM_Supp9_Q1,
                    Lit_SPM_Supp9_Q2 = x.Lit_SPM_Supp9_Q2,
                    Lit_SPM_Supp10_Q1 = x.Lit_SPM_Supp10_Q1,
                    Lit_SPM_Supp10_Q2 = x.Lit_SPM_Supp10_Q2,
                    Lit_OTIF_YTD23_24 = x.Lit_OTIF_YTD23_24,
                    Lit_OTIF_Target = x.Lit_OTIF_Target,
                    Lit_OTIF_Q1 = x.Lit_OTIF_Q1,
                    Lit_OTIF_Q2 = x.Lit_OTIF_Q2,
                    Lit_OTIF_YTD = x.Lit_OTIF_YTD,
                    Lit_SC_Closure_YTD23_24 = x.Lit_SC_Closure_YTD23_24,
                    Lit_SC_Closure_Baseline = x.Lit_SC_Closure_Baseline,
                    Lit_SC_Closure_Target = x.Lit_SC_Closure_Target,
                    Lit_SC_Closure_Q1 = x.Lit_SC_Closure_Q1,
                    Lit_SC_Closure_Q2 = x.Lit_SC_Closure_Q2,
                    Lit_SC_Closure_YTD = x.Lit_SC_Closure_YTD,
                    Lit_CSO_TotalLogged_YTD23_24 = x.Lit_CSO_TotalLogged_YTD23_24,
                    Lit_CSO_TotalLogged_Q1 = x.Lit_CSO_TotalLogged_Q1,
                    Lit_CSO_TotalLogged_Q2 = x.Lit_CSO_TotalLogged_Q2,
                    Lit_CSO_TotalLogged_YTD = x.Lit_CSO_TotalLogged_YTD,
                    Lit_CSO_TotalAClass_YTD23_24 = x.Lit_CSO_TotalAClass_YTD23_24,
                    Lit_CSO_TotalAClass_Q1 = x.Lit_CSO_TotalAClass_Q1,
                    Lit_CSO_TotalAClass_Q2 = x.Lit_CSO_TotalAClass_Q2,
                    Lit_CSO_TotalAClass_YTD = x.Lit_CSO_TotalAClass_YTD,
                    Lit_CSO_AClassClosed_YTD23_24 = x.Lit_CSO_AClassClosed_YTD23_24,
                    Lit_CSO_AClassClosed_Q1 = x.Lit_CSO_AClassClosed_Q1,
                    Lit_CSO_AClassClosed_Q2 = x.Lit_CSO_AClassClosed_Q2,
                    Lit_CSO_AClassClosed_YTD = x.Lit_CSO_AClassClosed_YTD,
                    Lit_CSO_AClassClosedLess45_YTD23_24 = x.Lit_CSO_AClassClosedLess45_YTD23_24,
                    Lit_CSO_AClassClosedLess45_Q1 = x.Lit_CSO_AClassClosedLess45_Q1,
                    Lit_CSO_AClassClosedLess45_Q2 = x.Lit_CSO_AClassClosedLess45_Q2,
                    Lit_CSO_AClassClosedLess45_YTD = x.Lit_CSO_AClassClosedLess45_YTD,
                    Lit_CSO_PercentageClosure_YTD23_24 = x.Lit_CSO_PercentageClosure_YTD23_24,
                    Lit_CSO_PercentageClosure_Q1 = x.Lit_CSO_PercentageClosure_Q1,
                    Lit_CSO_PercentageClosure_Q2 = x.Lit_CSO_PercentageClosure_Q2,
                    Lit_CSO_PercentageClosure_YTD = x.Lit_CSO_PercentageClosure_YTD,
                    Lit_CostSavings_YTD23_24 = x.Lit_CostSavings_YTD23_24,
                    Lit_CostSavings_Target = x.Lit_CostSavings_Target,
                    Lit_CostSavings_Q1 = x.Lit_CostSavings_Q1,
                    Lit_CostSavings_Q2 = x.Lit_CostSavings_Q2,
                    Lit_CostSavings_YTD = x.Lit_CostSavings_YTD,
                    Lit_OQL_ArtLuminaires_YTD23_24 = x.Lit_OQL_ArtLuminaires_YTD23_24,
                    Lit_OQL_ArtLuminaires_Target = x.Lit_OQL_ArtLuminaires_Target,
                    Lit_OQL_ArtLuminaires_PC01 = x.Lit_OQL_ArtLuminaires_PC01,
                    Lit_OQL_ArtLuminaires_PC02 = x.Lit_OQL_ArtLuminaires_PC02,
                    Lit_OQL_ArtLuminaires_PC03 = x.Lit_OQL_ArtLuminaires_PC03,
                    Lit_OQL_ArtLuminaires_PC04 = x.Lit_OQL_ArtLuminaires_PC04,
                    Lit_OQL_ArtLuminaires_PC05 = x.Lit_OQL_ArtLuminaires_PC05,
                    Lit_OQL_ArtLuminaires_PC06 = x.Lit_OQL_ArtLuminaires_PC06,
                    Lit_OQL_IdealLighting_YTD23_24 = x.Lit_OQL_IdealLighting_YTD23_24,
                    Lit_OQL_IdealLighting_Target = x.Lit_OQL_IdealLighting_Target,
                    Lit_OQL_IdealLighting_PC01 = x.Lit_OQL_IdealLighting_PC01,
                    Lit_OQL_IdealLighting_PC02 = x.Lit_OQL_IdealLighting_PC02,
                    Lit_OQL_IdealLighting_PC03 = x.Lit_OQL_IdealLighting_PC03,
                    Lit_OQL_IdealLighting_PC04 = x.Lit_OQL_IdealLighting_PC04,
                    Lit_OQL_IdealLighting_PC05 = x.Lit_OQL_IdealLighting_PC05,
                    Lit_OQL_IdealLighting_PC06 = x.Lit_OQL_IdealLighting_PC06,
                    Lit_OQL_Everlite_YTD23_24 = x.Lit_OQL_Everlite_YTD23_24,
                    Lit_OQL_Everlite_Target = x.Lit_OQL_Everlite_Target,
                    Lit_OQL_Everlite_PC01 = x.Lit_OQL_Everlite_PC01,
                    Lit_OQL_Everlite_PC02 = x.Lit_OQL_Everlite_PC02,
                    Lit_OQL_Everlite_PC03 = x.Lit_OQL_Everlite_PC03,
                    Lit_OQL_Everlite_PC04 = x.Lit_OQL_Everlite_PC04,
                    Lit_OQL_Everlite_PC05 = x.Lit_OQL_Everlite_PC05,
                    Lit_OQL_Everlite_PC06 = x.Lit_OQL_Everlite_PC06,
                    Lit_OQL_Rama_YTD23_24 = x.Lit_OQL_Rama_YTD23_24,
                    Lit_OQL_Rama_Target = x.Lit_OQL_Rama_Target,
                    Lit_OQL_Rama_PC01 = x.Lit_OQL_Rama_PC01,
                    Lit_OQL_Rama_PC02 = x.Lit_OQL_Rama_PC02,
                    Lit_OQL_Rama_PC03 = x.Lit_OQL_Rama_PC03,
                    Lit_OQL_Rama_PC04 = x.Lit_OQL_Rama_PC04,
                    Lit_OQL_Rama_PC05 = x.Lit_OQL_Rama_PC05,
                    Lit_OQL_Rama_PC06 = x.Lit_OQL_Rama_PC06,
                    Lit_OQL_Shantinath_YTD23_24 = x.Lit_OQL_Shantinath_YTD23_24,
                    Lit_OQL_Shantinath_Target = x.Lit_OQL_Shantinath_Target,
                    Lit_OQL_Shantinath_PC01 = x.Lit_OQL_Shantinath_PC01,
                    Lit_OQL_Shantinath_PC02 = x.Lit_OQL_Shantinath_PC02,
                    Lit_OQL_Shantinath_PC03 = x.Lit_OQL_Shantinath_PC03,
                    Lit_OQL_Shantinath_PC04 = x.Lit_OQL_Shantinath_PC04,
                    Lit_OQL_Shantinath_PC05 = x.Lit_OQL_Shantinath_PC05,
                    Lit_OQL_Shantinath_PC06 = x.Lit_OQL_Shantinath_PC06,
                    Lit_OQL_Varun_YTD23_24 = x.Lit_OQL_Varun_YTD23_24,
                    Lit_OQL_Varun_Target = x.Lit_OQL_Varun_Target,
                    Lit_OQL_Varun_PC01 = x.Lit_OQL_Varun_PC01,
                    Lit_OQL_Varun_PC02 = x.Lit_OQL_Varun_PC02,
                    Lit_OQL_Varun_PC03 = x.Lit_OQL_Varun_PC03,
                    Lit_OQL_Varun_PC04 = x.Lit_OQL_Varun_PC04,
                    Lit_OQL_Varun_PC05 = x.Lit_OQL_Varun_PC05,
                    Lit_OQL_Varun_PC06 = x.Lit_OQL_Varun_PC06,
                    Lit_OQL_Ujas_YTD23_24 = x.Lit_OQL_Ujas_YTD23_24,
                    Lit_OQL_Ujas_Target = x.Lit_OQL_Ujas_Target,
                    Lit_OQL_Ujas_PC01 = x.Lit_OQL_Ujas_PC01,
                    Lit_OQL_Ujas_PC02 = x.Lit_OQL_Ujas_PC02,
                    Lit_OQL_Ujas_PC03 = x.Lit_OQL_Ujas_PC03,
                    Lit_OQL_Ujas_PC04 = x.Lit_OQL_Ujas_PC04,
                    Lit_OQL_Ujas_PC05 = x.Lit_OQL_Ujas_PC05,
                    Lit_OQL_Ujas_PC06 = x.Lit_OQL_Ujas_PC06,
                    Lit_OQL_NAK_YTD23_24 = x.Lit_OQL_NAK_YTD23_24,
                    Lit_OQL_NAK_Target = x.Lit_OQL_NAK_Target,
                    Lit_OQL_NAK_PC01 = x.Lit_OQL_NAK_PC01,
                    Lit_OQL_NAK_PC02 = x.Lit_OQL_NAK_PC02,
                    Lit_OQL_NAK_PC03 = x.Lit_OQL_NAK_PC03,
                    Lit_OQL_NAK_PC04 = x.Lit_OQL_NAK_PC04,
                    Lit_OQL_NAK_PC05 = x.Lit_OQL_NAK_PC05,
                    Lit_OQL_NAK_PC06 = x.Lit_OQL_NAK_PC06,
                    Lit_OQL_CumulativeAvg1_YTD23_24 = x.Lit_OQL_CumulativeAvg1_YTD23_24,
                    Lit_OQL_CumulativeAvg1_Target = x.Lit_OQL_CumulativeAvg1_Target,
                    Lit_OQL_CumulativeAvg1_PC01 = x.Lit_OQL_CumulativeAvg1_PC01,
                    Lit_OQL_CumulativeAvg1_PC02 = x.Lit_OQL_CumulativeAvg1_PC02,
                    Lit_OQL_CumulativeAvg1_PC03 = x.Lit_OQL_CumulativeAvg1_PC03,
                    Lit_OQL_CumulativeAvg1_PC04 = x.Lit_OQL_CumulativeAvg1_PC04,
                    Lit_OQL_CumulativeAvg1_PC05 = x.Lit_OQL_CumulativeAvg1_PC05,
                    Lit_OQL_CumulativeAvg1_PC06 = x.Lit_OQL_CumulativeAvg1_PC06,
                    Lit_OQL_CumulativeAvg2_YTD23_24 = x.Lit_OQL_CumulativeAvg2_YTD23_24,
                    Lit_OQL_CumulativeAvg2_Target = x.Lit_OQL_CumulativeAvg2_Target,
                    Lit_OQL_CumulativeAvg2_PC01 = x.Lit_OQL_CumulativeAvg2_PC01,
                    Lit_OQL_CumulativeAvg2_PC02 = x.Lit_OQL_CumulativeAvg2_PC02,
                    Lit_OQL_CumulativeAvg2_PC03 = x.Lit_OQL_CumulativeAvg2_PC03,
                    Lit_OQL_CumulativeAvg2_PC04 = x.Lit_OQL_CumulativeAvg2_PC04,
                    Lit_OQL_CumulativeAvg2_PC05 = x.Lit_OQL_CumulativeAvg2_PC05,
                    Lit_OQL_CumulativeAvg2_PC06 = x.Lit_OQL_CumulativeAvg2_PC06,
                    Seat_SS_CNClosure_Baseline = x.Seat_SS_CNClosure_Baseline,
                    Seat_SS_CNClosure_Target = x.Seat_SS_CNClosure_Target,
                    Seat_SS_CNClosure_Q1 = x.Seat_SS_CNClosure_Q1,
                    Seat_SS_CNClosure_Q2 = x.Seat_SS_CNClosure_Q2,
                    Seat_SS_CNClosure_YTD = x.Seat_SS_CNClosure_YTD,
                    Seat_SS_OTIF_Baseline = x.Seat_SS_OTIF_Baseline,
                    Seat_SS_OTIF_Target = x.Seat_SS_OTIF_Target,
                    Seat_SS_OTIF_Q1 = x.Seat_SS_OTIF_Q1,
                    Seat_SS_OTIF_Q2 = x.Seat_SS_OTIF_Q2,
                    Seat_SS_OTIF_YTD = x.Seat_SS_OTIF_YTD,
                    Seat_SS_SPMScore_Baseline = x.Seat_SS_SPMScore_Baseline,
                    Seat_SS_SPMScore_Target = x.Seat_SS_SPMScore_Target,
                    Seat_SS_SPMScore_Q1 = x.Seat_SS_SPMScore_Q1,
                    Seat_SS_SPMScore_Q2 = x.Seat_SS_SPMScore_Q2,
                    Seat_SS_SPMScore_YTD = x.Seat_SS_SPMScore_YTD,
                    Seat_OTIF_Performance_YTD23_24 = x.Seat_OTIF_Performance_YTD23_24,
                    Seat_OTIF_Performance_Target = x.Seat_OTIF_Performance_Target,
                    Seat_OTIF_Performance_Q1 = x.Seat_OTIF_Performance_Q1,
                    Seat_OTIF_Performance_Q2 = x.Seat_OTIF_Performance_Q2,
                    Seat_OTIF_Performance_YTD = x.Seat_OTIF_Performance_YTD,
                    Seat_CSAT_ReqSent_YTD23_24 = x.Seat_CSAT_ReqSent_YTD23_24,
                    Seat_CSAT_ReqSent_Baseline = x.Seat_CSAT_ReqSent_Baseline,
                    Seat_CSAT_ReqSent_Target = x.Seat_CSAT_ReqSent_Target,
                    Seat_CSAT_ReqSent_Q1 = x.Seat_CSAT_ReqSent_Q1,
                    Seat_CSAT_ReqSent_Q2 = x.Seat_CSAT_ReqSent_Q2,
                    Seat_CSAT_ReqSent_YTD = x.Seat_CSAT_ReqSent_YTD,
                    Seat_CSAT_RespRecvd_YTD23_24 = x.Seat_CSAT_RespRecvd_YTD23_24,
                    Seat_CSAT_RespRecvd_Baseline = x.Seat_CSAT_RespRecvd_Baseline,
                    Seat_CSAT_RespRecvd_Target = x.Seat_CSAT_RespRecvd_Target,
                    Seat_CSAT_RespRecvd_Q1 = x.Seat_CSAT_RespRecvd_Q1,
                    Seat_CSAT_RespRecvd_Q2 = x.Seat_CSAT_RespRecvd_Q2,
                    Seat_CSAT_RespRecvd_YTD = x.Seat_CSAT_RespRecvd_YTD,
                    Seat_CSAT_Promoter_YTD23_24 = x.Seat_CSAT_Promoter_YTD23_24,
                    Seat_CSAT_Promoter_Baseline = x.Seat_CSAT_Promoter_Baseline,
                    Seat_CSAT_Promoter_Target = x.Seat_CSAT_Promoter_Target,
                    Seat_CSAT_Promoter_Q1 = x.Seat_CSAT_Promoter_Q1,
                    Seat_CSAT_Promoter_Q2 = x.Seat_CSAT_Promoter_Q2,
                    Seat_CSAT_Promoter_YTD = x.Seat_CSAT_Promoter_YTD,
                    Seat_CSAT_Detractor_YTD23_24 = x.Seat_CSAT_Detractor_YTD23_24,
                    Seat_CSAT_Detractor_Baseline = x.Seat_CSAT_Detractor_Baseline,
                    Seat_CSAT_Detractor_Target = x.Seat_CSAT_Detractor_Target,
                    Seat_CSAT_Detractor_Q1 = x.Seat_CSAT_Detractor_Q1,
                    Seat_CSAT_Detractor_Q2 = x.Seat_CSAT_Detractor_Q2,
                    Seat_CSAT_Detractor_YTD = x.Seat_CSAT_Detractor_YTD,
                    Seat_CSAT_NPS_YTD23_24 = x.Seat_CSAT_NPS_YTD23_24,
                    Seat_CSAT_NPS_Baseline = x.Seat_CSAT_NPS_Baseline,
                    Seat_CSAT_NPS_Target = x.Seat_CSAT_NPS_Target,
                    Seat_CSAT_NPS_Q1 = x.Seat_CSAT_NPS_Q1,
                    Seat_CSAT_NPS_Q2 = x.Seat_CSAT_NPS_Q2,
                    Seat_CSAT_NPS_YTD = x.Seat_CSAT_NPS_YTD,
                    Seat_CSO_TotalLogged_YTD23_24 = x.Seat_CSO_TotalLogged_YTD23_24,
                    Seat_CSO_TotalLogged_Q1 = x.Seat_CSO_TotalLogged_Q1,
                    Seat_CSO_TotalLogged_Q2 = x.Seat_CSO_TotalLogged_Q2,
                    Seat_CSO_TotalLogged_YTD = x.Seat_CSO_TotalLogged_YTD,
                    Seat_CSO_TotalAClass_YTD23_24 = x.Seat_CSO_TotalAClass_YTD23_24,
                    Seat_CSO_TotalAClass_Q1 = x.Seat_CSO_TotalAClass_Q1,
                    Seat_CSO_TotalAClass_Q2 = x.Seat_CSO_TotalAClass_Q2,
                    Seat_CSO_TotalAClass_YTD = x.Seat_CSO_TotalAClass_YTD,
                    Seat_CSO_AClassClosed_YTD23_24 = x.Seat_CSO_AClassClosed_YTD23_24,
                    Seat_CSO_AClassClosed_Q1 = x.Seat_CSO_AClassClosed_Q1,
                    Seat_CSO_AClassClosed_Q2 = x.Seat_CSO_AClassClosed_Q2,
                    Seat_CSO_AClassClosed_YTD = x.Seat_CSO_AClassClosed_YTD,
                    Seat_CSO_AClassClosedLess48_YTD23_24 = x.Seat_CSO_AClassClosedLess48_YTD23_24,
                    Seat_CSO_AClassClosedLess48_Q1 = x.Seat_CSO_AClassClosedLess48_Q1,
                    Seat_CSO_AClassClosedLess48_Q2 = x.Seat_CSO_AClassClosedLess48_Q2,
                    Seat_CSO_AClassClosedLess48_YTD = x.Seat_CSO_AClassClosedLess48_YTD,
                    Seat_CSO_AClassClosedUnder48_YTD23_24 = x.Seat_CSO_AClassClosedUnder48_YTD23_24,
                    Seat_CSO_AClassClosedUnder48_Q1 = x.Seat_CSO_AClassClosedUnder48_Q1,
                    Seat_CSO_AClassClosedUnder48_Q2 = x.Seat_CSO_AClassClosedUnder48_Q2,
                    Seat_CSO_AClassClosedUnder48_YTD = x.Seat_CSO_AClassClosedUnder48_YTD,
                    Seat_SPM_MPPL_Q4_23_24 = x.Seat_SPM_MPPL_Q4_23_24,
                    Seat_SPM_MPPL_Q1_24_25 = x.Seat_SPM_MPPL_Q1_24_25,
                    Seat_SPM_MPPL_Q2_24_25 = x.Seat_SPM_MPPL_Q2_24_25,
                    Seat_SPM_EXCLUSIFF_Q4_23_24 = x.Seat_SPM_EXCLUSIFF_Q4_23_24,
                    Seat_SPM_EXCLUSIFF_Q1_24_25 = x.Seat_SPM_EXCLUSIFF_Q1_24_25,
                    Seat_SPM_EXCLUSIFF_Q2_24_25 = x.Seat_SPM_EXCLUSIFF_Q2_24_25,
                    Seat_SPM_CVG_Q4_23_24 = x.Seat_SPM_CVG_Q4_23_24,
                    Seat_SPM_CVG_Q1_24_25 = x.Seat_SPM_CVG_Q1_24_25,
                    Seat_SPM_CVG_Q2_24_25 = x.Seat_SPM_CVG_Q2_24_25,
                    Seat_SPM_STARSHINE_Q4_23_24 = x.Seat_SPM_STARSHINE_Q4_23_24,
                    Seat_SPM_STARSHINE_Q1_24_25 = x.Seat_SPM_STARSHINE_Q1_24_25,
                    Seat_SPM_STARSHINE_Q2_24_25 = x.Seat_SPM_STARSHINE_Q2_24_25,
                    Seat_SPM_SAVITON_Q4_23_24 = x.Seat_SPM_SAVITON_Q4_23_24,
                    Seat_SPM_SAVITON_Q1_24_25 = x.Seat_SPM_SAVITON_Q1_24_25,
                    Seat_SPM_SAVITON_Q2_24_25 = x.Seat_SPM_SAVITON_Q2_24_25,
                    Seat_IQA_TotalSites_YTD23_24 = x.Seat_IQA_TotalSites_YTD23_24,
                    Seat_IQA_TotalSites_Target = x.Seat_IQA_TotalSites_Target,
                    Seat_IQA_TotalSites_Q1 = x.Seat_IQA_TotalSites_Q1,
                    Seat_IQA_TotalSites_Q2 = x.Seat_IQA_TotalSites_Q2,
                    Seat_IQA_TotalSites_YTD = x.Seat_IQA_TotalSites_YTD,
                    Seat_IQA_SitesCompleted_YTD23_24 = x.Seat_IQA_SitesCompleted_YTD23_24,
                    Seat_IQA_SitesCompleted_Target = x.Seat_IQA_SitesCompleted_Target,
                    Seat_IQA_SitesCompleted_Q1 = x.Seat_IQA_SitesCompleted_Q1,
                    Seat_IQA_SitesCompleted_Q2 = x.Seat_IQA_SitesCompleted_Q2,
                    Seat_IQA_SitesCompleted_YTD = x.Seat_IQA_SitesCompleted_YTD,
                    Seat_IQA_AuditsCompleted_YTD23_24 = x.Seat_IQA_AuditsCompleted_YTD23_24,
                    Seat_IQA_AuditsCompleted_Target = x.Seat_IQA_AuditsCompleted_Target,
                    Seat_IQA_AuditsCompleted_Q1 = x.Seat_IQA_AuditsCompleted_Q1,
                    Seat_IQA_AuditsCompleted_Q2 = x.Seat_IQA_AuditsCompleted_Q2,
                    Seat_IQA_AuditsCompleted_YTD = x.Seat_IQA_AuditsCompleted_YTD,
                    Seat_IQA_PercCompleted_YTD23_24 = x.Seat_IQA_PercCompleted_YTD23_24,
                    Seat_IQA_PercCompleted_Target = x.Seat_IQA_PercCompleted_Target,
                    Seat_IQA_PercCompleted_Q1 = x.Seat_IQA_PercCompleted_Q1,
                    Seat_IQA_PercCompleted_Q2 = x.Seat_IQA_PercCompleted_Q2,
                    Seat_IQA_PercCompleted_YTD = x.Seat_IQA_PercCompleted_YTD,
                    Seat_IQA_AvgSigma_YTD23_24 = x.Seat_IQA_AvgSigma_YTD23_24,
                    Seat_IQA_AvgSigma_Target = x.Seat_IQA_AvgSigma_Target,
                    Seat_IQA_AvgSigma_Q1 = x.Seat_IQA_AvgSigma_Q1,
                    Seat_IQA_AvgSigma_Q2 = x.Seat_IQA_AvgSigma_Q2,
                    Seat_IQA_AvgSigma_YTD = x.Seat_IQA_AvgSigma_YTD
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

    public async Task<OperationResult> InsertAHPNotesAsync(AHPNoteViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Lit_SEE_Engagement_Target", model.Lit_SEE_Engagement_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q1", model.Lit_SEE_Engagement_FY23_24_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q2", model.Lit_SEE_Engagement_FY23_24_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q3", model.Lit_SEE_Engagement_FY23_24_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q4", model.Lit_SEE_Engagement_FY23_24_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY24_25_Q1", model.Lit_SEE_Engagement_FY24_25_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY24_25_Q2", model.Lit_SEE_Engagement_FY24_25_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_Target", model.Lit_SEE_Effectiveness_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q1", model.Lit_SEE_Effectiveness_FY23_24_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q2", model.Lit_SEE_Effectiveness_FY23_24_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q3", model.Lit_SEE_Effectiveness_FY23_24_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q4", model.Lit_SEE_Effectiveness_FY23_24_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY24_25_Q1", model.Lit_SEE_Effectiveness_FY24_25_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY24_25_Q2", model.Lit_SEE_Effectiveness_FY24_25_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Baseline", model.Lit_SS_ServiceComplaints_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Target", model.Lit_SS_ServiceComplaints_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Q1", model.Lit_SS_ServiceComplaints_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Q2", model.Lit_SS_ServiceComplaints_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Baseline", model.Lit_SS_DesignLSG_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Target", model.Lit_SS_DesignLSG_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Q1", model.Lit_SS_DesignLSG_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Q2", model.Lit_SS_DesignLSG_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Baseline", model.Lit_SS_CostReduction_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Target", model.Lit_SS_CostReduction_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Q1", model.Lit_SS_CostReduction_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Q2", model.Lit_SS_CostReduction_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Baseline", model.Lit_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Target", model.Lit_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Q1", model.Lit_SS_OTIF_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Q2", model.Lit_SS_OTIF_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Baseline", model.Lit_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Target", model.Lit_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Q1", model.Lit_SS_SPMScore_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Q2", model.Lit_SS_SPMScore_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD23_24", model.Lit_CSAT_ReqSent_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Baseline", model.Lit_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Target", model.Lit_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Q1", model.Lit_CSAT_ReqSent_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Q2", model.Lit_CSAT_ReqSent_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD", model.Lit_CSAT_ReqSent_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD23_24", model.Lit_CSAT_RespRecvd_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Baseline", model.Lit_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Target", model.Lit_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Q1", model.Lit_CSAT_RespRecvd_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Q2", model.Lit_CSAT_RespRecvd_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD", model.Lit_CSAT_RespRecvd_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD23_24", model.Lit_CSAT_Promoter_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Baseline", model.Lit_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Target", model.Lit_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Q1", model.Lit_CSAT_Promoter_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Q2", model.Lit_CSAT_Promoter_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD", model.Lit_CSAT_Promoter_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD23_24", model.Lit_CSAT_Detractor_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Baseline", model.Lit_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Target", model.Lit_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Q1", model.Lit_CSAT_Detractor_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Q2", model.Lit_CSAT_Detractor_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD", model.Lit_CSAT_Detractor_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD23_24", model.Lit_CSAT_NPS_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Baseline", model.Lit_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Target", model.Lit_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Q1", model.Lit_CSAT_NPS_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Q2", model.Lit_CSAT_NPS_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD", model.Lit_CSAT_NPS_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_Q1", model.Lit_SPM_Supp1_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_Q2", model.Lit_SPM_Supp1_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_Q1", model.Lit_SPM_Supp2_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_Q2", model.Lit_SPM_Supp2_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_Q1", model.Lit_SPM_Supp3_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_Q2", model.Lit_SPM_Supp3_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_Q1", model.Lit_SPM_Supp4_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_Q2", model.Lit_SPM_Supp4_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_Q1", model.Lit_SPM_Supp5_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_Q2", model.Lit_SPM_Supp5_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_Q1", model.Lit_SPM_Supp6_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_Q2", model.Lit_SPM_Supp6_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_Q1", model.Lit_SPM_Supp7_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_Q2", model.Lit_SPM_Supp7_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_Q1", model.Lit_SPM_Supp8_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_Q2", model.Lit_SPM_Supp8_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_Q1", model.Lit_SPM_Supp9_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_Q2", model.Lit_SPM_Supp9_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_Q1", model.Lit_SPM_Supp10_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_Q2", model.Lit_SPM_Supp10_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD23_24", model.Lit_OTIF_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Target", model.Lit_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Q1", model.Lit_OTIF_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Q2", model.Lit_OTIF_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD", model.Lit_OTIF_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD23_24", model.Lit_SC_Closure_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Baseline", model.Lit_SC_Closure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Target", model.Lit_SC_Closure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Q1", model.Lit_SC_Closure_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Q2", model.Lit_SC_Closure_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD", model.Lit_SC_Closure_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD23_24", model.Lit_CSO_TotalLogged_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_Q1", model.Lit_CSO_TotalLogged_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_Q2", model.Lit_CSO_TotalLogged_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD", model.Lit_CSO_TotalLogged_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD23_24", model.Lit_CSO_TotalAClass_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_Q1", model.Lit_CSO_TotalAClass_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_Q2", model.Lit_CSO_TotalAClass_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD", model.Lit_CSO_TotalAClass_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD23_24", model.Lit_CSO_AClassClosed_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_Q1", model.Lit_CSO_AClassClosed_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_Q2", model.Lit_CSO_AClassClosed_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD", model.Lit_CSO_AClassClosed_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD23_24", model.Lit_CSO_AClassClosedLess45_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_Q1", model.Lit_CSO_AClassClosedLess45_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_Q2", model.Lit_CSO_AClassClosedLess45_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD", model.Lit_CSO_AClassClosedLess45_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD23_24", model.Lit_CSO_PercentageClosure_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_Q1", model.Lit_CSO_PercentageClosure_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_Q2", model.Lit_CSO_PercentageClosure_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD", model.Lit_CSO_PercentageClosure_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD23_24", model.Lit_CostSavings_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Target", model.Lit_CostSavings_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Q1", model.Lit_CostSavings_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Q2", model.Lit_CostSavings_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD", model.Lit_CostSavings_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_YTD23_24", model.Lit_OQL_ArtLuminaires_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_Target", model.Lit_OQL_ArtLuminaires_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC01", model.Lit_OQL_ArtLuminaires_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC02", model.Lit_OQL_ArtLuminaires_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC03", model.Lit_OQL_ArtLuminaires_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC04", model.Lit_OQL_ArtLuminaires_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC05", model.Lit_OQL_ArtLuminaires_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC06", model.Lit_OQL_ArtLuminaires_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_YTD23_24", model.Lit_OQL_IdealLighting_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_Target", model.Lit_OQL_IdealLighting_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC01", model.Lit_OQL_IdealLighting_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC02", model.Lit_OQL_IdealLighting_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC03", model.Lit_OQL_IdealLighting_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC04", model.Lit_OQL_IdealLighting_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC05", model.Lit_OQL_IdealLighting_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC06", model.Lit_OQL_IdealLighting_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_YTD23_24", model.Lit_OQL_Everlite_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_Target", model.Lit_OQL_Everlite_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC01", model.Lit_OQL_Everlite_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC02", model.Lit_OQL_Everlite_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC03", model.Lit_OQL_Everlite_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC04", model.Lit_OQL_Everlite_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC05", model.Lit_OQL_Everlite_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC06", model.Lit_OQL_Everlite_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_YTD23_24", model.Lit_OQL_Rama_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_Target", model.Lit_OQL_Rama_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC01", model.Lit_OQL_Rama_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC02", model.Lit_OQL_Rama_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC03", model.Lit_OQL_Rama_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC04", model.Lit_OQL_Rama_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC05", model.Lit_OQL_Rama_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC06", model.Lit_OQL_Rama_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_YTD23_24", model.Lit_OQL_Shantinath_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_Target", model.Lit_OQL_Shantinath_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC01", model.Lit_OQL_Shantinath_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC02", model.Lit_OQL_Shantinath_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC03", model.Lit_OQL_Shantinath_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC04", model.Lit_OQL_Shantinath_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC05", model.Lit_OQL_Shantinath_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC06", model.Lit_OQL_Shantinath_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_YTD23_24", model.Lit_OQL_Varun_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_Target", model.Lit_OQL_Varun_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC01", model.Lit_OQL_Varun_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC02", model.Lit_OQL_Varun_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC03", model.Lit_OQL_Varun_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC04", model.Lit_OQL_Varun_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC05", model.Lit_OQL_Varun_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC06", model.Lit_OQL_Varun_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_YTD23_24", model.Lit_OQL_Ujas_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_Target", model.Lit_OQL_Ujas_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC01", model.Lit_OQL_Ujas_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC02", model.Lit_OQL_Ujas_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC03", model.Lit_OQL_Ujas_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC04", model.Lit_OQL_Ujas_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC05", model.Lit_OQL_Ujas_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC06", model.Lit_OQL_Ujas_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_YTD23_24", model.Lit_OQL_NAK_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_Target", model.Lit_OQL_NAK_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC01", model.Lit_OQL_NAK_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC02", model.Lit_OQL_NAK_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC03", model.Lit_OQL_NAK_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC04", model.Lit_OQL_NAK_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC05", model.Lit_OQL_NAK_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC06", model.Lit_OQL_NAK_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_YTD23_24", model.Lit_OQL_CumulativeAvg1_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_Target", model.Lit_OQL_CumulativeAvg1_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC01", model.Lit_OQL_CumulativeAvg1_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC02", model.Lit_OQL_CumulativeAvg1_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC03", model.Lit_OQL_CumulativeAvg1_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC04", model.Lit_OQL_CumulativeAvg1_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC05", model.Lit_OQL_CumulativeAvg1_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC06", model.Lit_OQL_CumulativeAvg1_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_YTD23_24", model.Lit_OQL_CumulativeAvg2_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_Target", model.Lit_OQL_CumulativeAvg2_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC01", model.Lit_OQL_CumulativeAvg2_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC02", model.Lit_OQL_CumulativeAvg2_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC03", model.Lit_OQL_CumulativeAvg2_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC04", model.Lit_OQL_CumulativeAvg2_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC05", model.Lit_OQL_CumulativeAvg2_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC06", model.Lit_OQL_CumulativeAvg2_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Baseline", model.Seat_SS_CNClosure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Target", model.Seat_SS_CNClosure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Q1", model.Seat_SS_CNClosure_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Q2", model.Seat_SS_CNClosure_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_YTD", model.Seat_SS_CNClosure_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Baseline", model.Seat_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Target", model.Seat_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Q1", model.Seat_SS_OTIF_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Q2", model.Seat_SS_OTIF_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_YTD", model.Seat_SS_OTIF_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Baseline", model.Seat_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Target", model.Seat_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Q1", model.Seat_SS_SPMScore_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Q2", model.Seat_SS_SPMScore_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_YTD", model.Seat_SS_SPMScore_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD23_24", model.Seat_OTIF_Performance_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Target", model.Seat_OTIF_Performance_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Q1", model.Seat_OTIF_Performance_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Q2", model.Seat_OTIF_Performance_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD", model.Seat_OTIF_Performance_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD23_24", model.Seat_CSAT_ReqSent_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Baseline", model.Seat_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Target", model.Seat_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Q1", model.Seat_CSAT_ReqSent_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Q2", model.Seat_CSAT_ReqSent_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD", model.Seat_CSAT_ReqSent_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD23_24", model.Seat_CSAT_RespRecvd_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Baseline", model.Seat_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Target", model.Seat_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Q1", model.Seat_CSAT_RespRecvd_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Q2", model.Seat_CSAT_RespRecvd_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD", model.Seat_CSAT_RespRecvd_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD23_24", model.Seat_CSAT_Promoter_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Baseline", model.Seat_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Target", model.Seat_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Q1", model.Seat_CSAT_Promoter_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Q2", model.Seat_CSAT_Promoter_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD", model.Seat_CSAT_Promoter_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD23_24", model.Seat_CSAT_Detractor_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Baseline", model.Seat_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Target", model.Seat_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Q1", model.Seat_CSAT_Detractor_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Q2", model.Seat_CSAT_Detractor_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD", model.Seat_CSAT_Detractor_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD23_24", model.Seat_CSAT_NPS_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Baseline", model.Seat_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Target", model.Seat_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Q1", model.Seat_CSAT_NPS_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Q2", model.Seat_CSAT_NPS_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD", model.Seat_CSAT_NPS_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD23_24", model.Seat_CSO_TotalLogged_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_Q1", model.Seat_CSO_TotalLogged_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_Q2", model.Seat_CSO_TotalLogged_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD", model.Seat_CSO_TotalLogged_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD23_24", model.Seat_CSO_TotalAClass_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_Q1", model.Seat_CSO_TotalAClass_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_Q2", model.Seat_CSO_TotalAClass_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD", model.Seat_CSO_TotalAClass_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD23_24", model.Seat_CSO_AClassClosed_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_Q1", model.Seat_CSO_AClassClosed_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_Q2", model.Seat_CSO_AClassClosed_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD", model.Seat_CSO_AClassClosed_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_YTD23_24", model.Seat_CSO_AClassClosedLess48_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_Q1", model.Seat_CSO_AClassClosedLess48_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_Q2", model.Seat_CSO_AClassClosedLess48_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_YTD", model.Seat_CSO_AClassClosedLess48_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_YTD23_24", model.Seat_CSO_AClassClosedUnder48_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_Q1", model.Seat_CSO_AClassClosedUnder48_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_Q2", model.Seat_CSO_AClassClosedUnder48_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_YTD", model.Seat_CSO_AClassClosedUnder48_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_MPPL_Q4_23_24", model.Seat_SPM_MPPL_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_MPPL_Q1_24_25", model.Seat_SPM_MPPL_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_MPPL_Q2_24_25", model.Seat_SPM_MPPL_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_EXCLUSIFF_Q4_23_24", model.Seat_SPM_EXCLUSIFF_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_EXCLUSIFF_Q1_24_25", model.Seat_SPM_EXCLUSIFF_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_EXCLUSIFF_Q2_24_25", model.Seat_SPM_EXCLUSIFF_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_CVG_Q4_23_24", model.Seat_SPM_CVG_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_CVG_Q1_24_25", model.Seat_SPM_CVG_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_CVG_Q2_24_25", model.Seat_SPM_CVG_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_STARSHINE_Q4_23_24", model.Seat_SPM_STARSHINE_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_STARSHINE_Q1_24_25", model.Seat_SPM_STARSHINE_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_STARSHINE_Q2_24_25", model.Seat_SPM_STARSHINE_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_SAVITON_Q4_23_24", model.Seat_SPM_SAVITON_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_SAVITON_Q1_24_25", model.Seat_SPM_SAVITON_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_SAVITON_Q2_24_25", model.Seat_SPM_SAVITON_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD23_24", model.Seat_IQA_TotalSites_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Target", model.Seat_IQA_TotalSites_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Q1", model.Seat_IQA_TotalSites_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Q2", model.Seat_IQA_TotalSites_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD", model.Seat_IQA_TotalSites_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD23_24", model.Seat_IQA_SitesCompleted_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Target", model.Seat_IQA_SitesCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Q1", model.Seat_IQA_SitesCompleted_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Q2", model.Seat_IQA_SitesCompleted_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD", model.Seat_IQA_SitesCompleted_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD23_24", model.Seat_IQA_AuditsCompleted_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Target", model.Seat_IQA_AuditsCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Q1", model.Seat_IQA_AuditsCompleted_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Q2", model.Seat_IQA_AuditsCompleted_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD", model.Seat_IQA_AuditsCompleted_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD23_24", model.Seat_IQA_PercCompleted_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Target", model.Seat_IQA_PercCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Q1", model.Seat_IQA_PercCompleted_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Q2", model.Seat_IQA_PercCompleted_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD", model.Seat_IQA_PercCompleted_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD23_24", model.Seat_IQA_AvgSigma_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Target", model.Seat_IQA_AvgSigma_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Q1", model.Seat_IQA_AvgSigma_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Q2", model.Seat_IQA_AvgSigma_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD", model.Seat_IQA_AvgSigma_YTD ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_AHPNote " +
                    "@Lit_SEE_Engagement_Target, " +
                    "@Lit_SEE_Engagement_FY23_24_Q1, " +
                    "@Lit_SEE_Engagement_FY23_24_Q2, " +
                    "@Lit_SEE_Engagement_FY23_24_Q3, " +
                    "@Lit_SEE_Engagement_FY23_24_Q4, " +
                    "@Lit_SEE_Engagement_FY24_25_Q1, " +
                    "@Lit_SEE_Engagement_FY24_25_Q2, " +
                    "@Lit_SEE_Effectiveness_Target, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q1, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q2, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q3, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q4, " +
                    "@Lit_SEE_Effectiveness_FY24_25_Q1, " +
                    "@Lit_SEE_Effectiveness_FY24_25_Q2, " +
                    "@Lit_SS_ServiceComplaints_Baseline, " +
                    "@Lit_SS_ServiceComplaints_Target, " +
                    "@Lit_SS_ServiceComplaints_Q1, " +
                    "@Lit_SS_ServiceComplaints_Q2, " +
                    "@Lit_SS_DesignLSG_Baseline, " +
                    "@Lit_SS_DesignLSG_Target, " +
                    "@Lit_SS_DesignLSG_Q1, " +
                    "@Lit_SS_DesignLSG_Q2, " +
                    "@Lit_SS_CostReduction_Baseline, " +
                    "@Lit_SS_CostReduction_Target, " +
                    "@Lit_SS_CostReduction_Q1, " +
                    "@Lit_SS_CostReduction_Q2, " +
                    "@Lit_SS_OTIF_Baseline, " +
                    "@Lit_SS_OTIF_Target, " +
                    "@Lit_SS_OTIF_Q1, " +
                    "@Lit_SS_OTIF_Q2, " +
                    "@Lit_SS_SPMScore_Baseline, " +
                    "@Lit_SS_SPMScore_Target, " +
                    "@Lit_SS_SPMScore_Q1, " +
                    "@Lit_SS_SPMScore_Q2, " +
                    "@Lit_CSAT_ReqSent_YTD23_24, " +
                    "@Lit_CSAT_ReqSent_Baseline, " +
                    "@Lit_CSAT_ReqSent_Target, " +
                    "@Lit_CSAT_ReqSent_Q1, " +
                    "@Lit_CSAT_ReqSent_Q2, " +
                    "@Lit_CSAT_ReqSent_YTD, " +
                    "@Lit_CSAT_RespRecvd_YTD23_24, " +
                    "@Lit_CSAT_RespRecvd_Baseline, " +
                    "@Lit_CSAT_RespRecvd_Target, " +
                    "@Lit_CSAT_RespRecvd_Q1, " +
                    "@Lit_CSAT_RespRecvd_Q2, " +
                    "@Lit_CSAT_RespRecvd_YTD, " +
                    "@Lit_CSAT_Promoter_YTD23_24, " +
                    "@Lit_CSAT_Promoter_Baseline, " +
                    "@Lit_CSAT_Promoter_Target, " +
                    "@Lit_CSAT_Promoter_Q1, " +
                    "@Lit_CSAT_Promoter_Q2, " +
                    "@Lit_CSAT_Promoter_YTD, " +
                    "@Lit_CSAT_Detractor_YTD23_24, " +
                    "@Lit_CSAT_Detractor_Baseline, " +
                    "@Lit_CSAT_Detractor_Target, " +
                    "@Lit_CSAT_Detractor_Q1, " +
                    "@Lit_CSAT_Detractor_Q2, " +
                    "@Lit_CSAT_Detractor_YTD, " +
                    "@Lit_CSAT_NPS_YTD23_24, " +
                    "@Lit_CSAT_NPS_Baseline, " +
                    "@Lit_CSAT_NPS_Target, " +
                    "@Lit_CSAT_NPS_Q1, " +
                    "@Lit_CSAT_NPS_Q2, " +
                    "@Lit_CSAT_NPS_YTD, " +
                    "@Lit_SPM_Supp1_Q1, " +
                    "@Lit_SPM_Supp1_Q2, " +
                    "@Lit_SPM_Supp2_Q1, " +
                    "@Lit_SPM_Supp2_Q2, " +
                    "@Lit_SPM_Supp3_Q1, " +
                    "@Lit_SPM_Supp3_Q2, " +
                    "@Lit_SPM_Supp4_Q1, " +
                    "@Lit_SPM_Supp4_Q2, " +
                    "@Lit_SPM_Supp5_Q1, " +
                    "@Lit_SPM_Supp5_Q2, " +
                    "@Lit_SPM_Supp6_Q1, " +
                    "@Lit_SPM_Supp6_Q2, " +
                    "@Lit_SPM_Supp7_Q1, " +
                    "@Lit_SPM_Supp7_Q2, " +
                    "@Lit_SPM_Supp8_Q1, " +
                    "@Lit_SPM_Supp8_Q2, " +
                    "@Lit_SPM_Supp9_Q1, " +
                    "@Lit_SPM_Supp9_Q2, " +
                    "@Lit_SPM_Supp10_Q1, " +
                    "@Lit_SPM_Supp10_Q2, " +
                    "@Lit_OTIF_YTD23_24, " +
                    "@Lit_OTIF_Target, " +
                    "@Lit_OTIF_Q1, " +
                    "@Lit_OTIF_Q2, " +
                    "@Lit_OTIF_YTD, " +
                    "@Lit_SC_Closure_YTD23_24, " +
                    "@Lit_SC_Closure_Baseline, " +
                    "@Lit_SC_Closure_Target, " +
                    "@Lit_SC_Closure_Q1, " +
                    "@Lit_SC_Closure_Q2, " +
                    "@Lit_SC_Closure_YTD, " +
                    "@Lit_CSO_TotalLogged_YTD23_24, " +
                    "@Lit_CSO_TotalLogged_Q1, " +
                    "@Lit_CSO_TotalLogged_Q2, " +
                    "@Lit_CSO_TotalLogged_YTD, " +
                    "@Lit_CSO_TotalAClass_YTD23_24, " +
                    "@Lit_CSO_TotalAClass_Q1, " +
                    "@Lit_CSO_TotalAClass_Q2, " +
                    "@Lit_CSO_TotalAClass_YTD, " +
                    "@Lit_CSO_AClassClosed_YTD23_24, " +
                    "@Lit_CSO_AClassClosed_Q1, " +
                    "@Lit_CSO_AClassClosed_Q2, " +
                    "@Lit_CSO_AClassClosed_YTD, " +
                    "@Lit_CSO_AClassClosedLess45_YTD23_24, " +
                    "@Lit_CSO_AClassClosedLess45_Q1, " +
                    "@Lit_CSO_AClassClosedLess45_Q2, " +
                    "@Lit_CSO_AClassClosedLess45_YTD, " +
                    "@Lit_CSO_PercentageClosure_YTD23_24, " +
                    "@Lit_CSO_PercentageClosure_Q1, " +
                    "@Lit_CSO_PercentageClosure_Q2, " +
                    "@Lit_CSO_PercentageClosure_YTD, " +
                    "@Lit_CostSavings_YTD23_24, " +
                    "@Lit_CostSavings_Target, " +
                    "@Lit_CostSavings_Q1, " +
                    "@Lit_CostSavings_Q2, " +
                    "@Lit_CostSavings_YTD, " +
                    "@Lit_OQL_ArtLuminaires_YTD23_24, " +
                    "@Lit_OQL_ArtLuminaires_Target, " +
                    "@Lit_OQL_ArtLuminaires_PC01, " +
                    "@Lit_OQL_ArtLuminaires_PC02, " +
                    "@Lit_OQL_ArtLuminaires_PC03, " +
                    "@Lit_OQL_ArtLuminaires_PC04, " +
                    "@Lit_OQL_ArtLuminaires_PC05, " +
                    "@Lit_OQL_ArtLuminaires_PC06, " +
                    "@Lit_OQL_IdealLighting_YTD23_24, " +
                    "@Lit_OQL_IdealLighting_Target, " +
                    "@Lit_OQL_IdealLighting_PC01, " +
                    "@Lit_OQL_IdealLighting_PC02, " +
                    "@Lit_OQL_IdealLighting_PC03, " +
                    "@Lit_OQL_IdealLighting_PC04, " +
                    "@Lit_OQL_IdealLighting_PC05, " +
                    "@Lit_OQL_IdealLighting_PC06, " +
                    "@Lit_OQL_Everlite_YTD23_24, " +
                    "@Lit_OQL_Everlite_Target, " +
                    "@Lit_OQL_Everlite_PC01, " +
                    "@Lit_OQL_Everlite_PC02, " +
                    "@Lit_OQL_Everlite_PC03, " +
                    "@Lit_OQL_Everlite_PC04, " +
                    "@Lit_OQL_Everlite_PC05, " +
                    "@Lit_OQL_Everlite_PC06, " +
                    "@Lit_OQL_Rama_YTD23_24, " +
                    "@Lit_OQL_Rama_Target, " +
                    "@Lit_OQL_Rama_PC01, " +
                    "@Lit_OQL_Rama_PC02, " +
                    "@Lit_OQL_Rama_PC03, " +
                    "@Lit_OQL_Rama_PC04, " +
                    "@Lit_OQL_Rama_PC05, " +
                    "@Lit_OQL_Rama_PC06, " +
                    "@Lit_OQL_Shantinath_YTD23_24, " +
                    "@Lit_OQL_Shantinath_Target, " +
                    "@Lit_OQL_Shantinath_PC01, " +
                    "@Lit_OQL_Shantinath_PC02, " +
                    "@Lit_OQL_Shantinath_PC03, " +
                    "@Lit_OQL_Shantinath_PC04, " +
                    "@Lit_OQL_Shantinath_PC05, " +
                    "@Lit_OQL_Shantinath_PC06, " +
                    "@Lit_OQL_Varun_YTD23_24, " +
                    "@Lit_OQL_Varun_Target, " +
                    "@Lit_OQL_Varun_PC01, " +
                    "@Lit_OQL_Varun_PC02, " +
                    "@Lit_OQL_Varun_PC03, " +
                    "@Lit_OQL_Varun_PC04, " +
                    "@Lit_OQL_Varun_PC05, " +
                    "@Lit_OQL_Varun_PC06, " +
                    "@Lit_OQL_Ujas_YTD23_24, " +
                    "@Lit_OQL_Ujas_Target, " +
                    "@Lit_OQL_Ujas_PC01, " +
                    "@Lit_OQL_Ujas_PC02, " +
                    "@Lit_OQL_Ujas_PC03, " +
                    "@Lit_OQL_Ujas_PC04, " +
                    "@Lit_OQL_Ujas_PC05, " +
                    "@Lit_OQL_Ujas_PC06, " +
                    "@Lit_OQL_NAK_YTD23_24, " +
                    "@Lit_OQL_NAK_Target, " +
                    "@Lit_OQL_NAK_PC01, " +
                    "@Lit_OQL_NAK_PC02, " +
                    "@Lit_OQL_NAK_PC03, " +
                    "@Lit_OQL_NAK_PC04, " +
                    "@Lit_OQL_NAK_PC05, " +
                    "@Lit_OQL_NAK_PC06, " +
                    "@Lit_OQL_CumulativeAvg1_YTD23_24, " +
                    "@Lit_OQL_CumulativeAvg1_Target, " +
                    "@Lit_OQL_CumulativeAvg1_PC01, " +
                    "@Lit_OQL_CumulativeAvg1_PC02, " +
                    "@Lit_OQL_CumulativeAvg1_PC03, " +
                    "@Lit_OQL_CumulativeAvg1_PC04, " +
                    "@Lit_OQL_CumulativeAvg1_PC05, " +
                    "@Lit_OQL_CumulativeAvg1_PC06, " +
                    "@Lit_OQL_CumulativeAvg2_YTD23_24, " +
                    "@Lit_OQL_CumulativeAvg2_Target, " +
                    "@Lit_OQL_CumulativeAvg2_PC01, " +
                    "@Lit_OQL_CumulativeAvg2_PC02, " +
                    "@Lit_OQL_CumulativeAvg2_PC03, " +
                    "@Lit_OQL_CumulativeAvg2_PC04, " +
                    "@Lit_OQL_CumulativeAvg2_PC05, " +
                    "@Lit_OQL_CumulativeAvg2_PC06, " +
                    "@Seat_SS_CNClosure_Baseline, " +
                    "@Seat_SS_CNClosure_Target, " +
                    "@Seat_SS_CNClosure_Q1, " +
                    "@Seat_SS_CNClosure_Q2, " +
                    "@Seat_SS_CNClosure_YTD, " +
                    "@Seat_SS_OTIF_Baseline, " +
                    "@Seat_SS_OTIF_Target, " +
                    "@Seat_SS_OTIF_Q1, " +
                    "@Seat_SS_OTIF_Q2, " +
                    "@Seat_SS_OTIF_YTD, " +
                    "@Seat_SS_SPMScore_Baseline, " +
                    "@Seat_SS_SPMScore_Target, " +
                    "@Seat_SS_SPMScore_Q1, " +
                    "@Seat_SS_SPMScore_Q2, " +
                    "@Seat_SS_SPMScore_YTD, " +
                    "@Seat_OTIF_Performance_YTD23_24, " +
                    "@Seat_OTIF_Performance_Target, " +
                    "@Seat_OTIF_Performance_Q1, " +
                    "@Seat_OTIF_Performance_Q2, " +
                    "@Seat_OTIF_Performance_YTD, " +
                    "@Seat_CSAT_ReqSent_YTD23_24, " +
                    "@Seat_CSAT_ReqSent_Baseline, " +
                    "@Seat_CSAT_ReqSent_Target, " +
                    "@Seat_CSAT_ReqSent_Q1, " +
                    "@Seat_CSAT_ReqSent_Q2, " +
                    "@Seat_CSAT_ReqSent_YTD, " +
                    "@Seat_CSAT_RespRecvd_YTD23_24, " +
                    "@Seat_CSAT_RespRecvd_Baseline, " +
                    "@Seat_CSAT_RespRecvd_Target, " +
                    "@Seat_CSAT_RespRecvd_Q1, " +
                    "@Seat_CSAT_RespRecvd_Q2, " +
                    "@Seat_CSAT_RespRecvd_YTD, " +
                    "@Seat_CSAT_Promoter_YTD23_24, " +
                    "@Seat_CSAT_Promoter_Baseline, " +
                    "@Seat_CSAT_Promoter_Target, " +
                    "@Seat_CSAT_Promoter_Q1, " +
                    "@Seat_CSAT_Promoter_Q2, " +
                    "@Seat_CSAT_Promoter_YTD, " +
                    "@Seat_CSAT_Detractor_YTD23_24, " +
                    "@Seat_CSAT_Detractor_Baseline, " +
                    "@Seat_CSAT_Detractor_Target, " +
                    "@Seat_CSAT_Detractor_Q1, " +
                    "@Seat_CSAT_Detractor_Q2, " +
                    "@Seat_CSAT_Detractor_YTD, " +
                    "@Seat_CSAT_NPS_YTD23_24, " +
                    "@Seat_CSAT_NPS_Baseline, " +
                    "@Seat_CSAT_NPS_Target, " +
                    "@Seat_CSAT_NPS_Q1, " +
                    "@Seat_CSAT_NPS_Q2, " +
                    "@Seat_CSAT_NPS_YTD, " +
                    "@Seat_CSO_TotalLogged_YTD23_24, " +
                    "@Seat_CSO_TotalLogged_Q1, " +
                    "@Seat_CSO_TotalLogged_Q2, " +
                    "@Seat_CSO_TotalLogged_YTD, " +
                    "@Seat_CSO_TotalAClass_YTD23_24, " +
                    "@Seat_CSO_TotalAClass_Q1, " +
                    "@Seat_CSO_TotalAClass_Q2, " +
                    "@Seat_CSO_TotalAClass_YTD, " +
                    "@Seat_CSO_AClassClosed_YTD23_24, " +
                    "@Seat_CSO_AClassClosed_Q1, " +
                    "@Seat_CSO_AClassClosed_Q2, " +
                    "@Seat_CSO_AClassClosed_YTD, " +
                    "@Seat_CSO_AClassClosedLess48_YTD23_24, " +
                    "@Seat_CSO_AClassClosedLess48_Q1, " +
                    "@Seat_CSO_AClassClosedLess48_Q2, " +
                    "@Seat_CSO_AClassClosedLess48_YTD, " +
                    "@Seat_CSO_AClassClosedUnder48_YTD23_24, " +
                    "@Seat_CSO_AClassClosedUnder48_Q1, " +
                    "@Seat_CSO_AClassClosedUnder48_Q2, " +
                    "@Seat_CSO_AClassClosedUnder48_YTD, " +
                    "@Seat_SPM_MPPL_Q4_23_24, " +
                    "@Seat_SPM_MPPL_Q1_24_25, " +
                    "@Seat_SPM_MPPL_Q2_24_25, " +
                    "@Seat_SPM_EXCLUSIFF_Q4_23_24, " +
                    "@Seat_SPM_EXCLUSIFF_Q1_24_25, " +
                    "@Seat_SPM_EXCLUSIFF_Q2_24_25, " +
                    "@Seat_SPM_CVG_Q4_23_24, " +
                    "@Seat_SPM_CVG_Q1_24_25, " +
                    "@Seat_SPM_CVG_Q2_24_25, " +
                    "@Seat_SPM_STARSHINE_Q4_23_24, " +
                    "@Seat_SPM_STARSHINE_Q1_24_25, " +
                    "@Seat_SPM_STARSHINE_Q2_24_25, " +
                    "@Seat_SPM_SAVITON_Q4_23_24, " +
                    "@Seat_SPM_SAVITON_Q1_24_25, " +
                    "@Seat_SPM_SAVITON_Q2_24_25, " +
                    "@Seat_IQA_TotalSites_YTD23_24, " +
                    "@Seat_IQA_TotalSites_Target, " +
                    "@Seat_IQA_TotalSites_Q1, " +
                    "@Seat_IQA_TotalSites_Q2, " +
                    "@Seat_IQA_TotalSites_YTD, " +
                    "@Seat_IQA_SitesCompleted_YTD23_24, " +
                    "@Seat_IQA_SitesCompleted_Target, " +
                    "@Seat_IQA_SitesCompleted_Q1, " +
                    "@Seat_IQA_SitesCompleted_Q2, " +
                    "@Seat_IQA_SitesCompleted_YTD, " +
                    "@Seat_IQA_AuditsCompleted_YTD23_24, " +
                    "@Seat_IQA_AuditsCompleted_Target, " +
                    "@Seat_IQA_AuditsCompleted_Q1, " +
                    "@Seat_IQA_AuditsCompleted_Q2, " +
                    "@Seat_IQA_AuditsCompleted_YTD, " +
                    "@Seat_IQA_PercCompleted_YTD23_24, " +
                    "@Seat_IQA_PercCompleted_Target, " +
                    "@Seat_IQA_PercCompleted_Q1, " +
                    "@Seat_IQA_PercCompleted_Q2, " +
                    "@Seat_IQA_PercCompleted_YTD, " +
                    "@Seat_IQA_AvgSigma_YTD23_24, " +
                    "@Seat_IQA_AvgSigma_Target, " +
                    "@Seat_IQA_AvgSigma_Q1, " +
                    "@Seat_IQA_AvgSigma_Q2, " +
                    "@Seat_IQA_AvgSigma_YTD, " +
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

    public async Task<OperationResult> UpdateAHPNotesAsync(AHPNoteViewModel model)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@Lit_SEE_Engagement_Target", model.Lit_SEE_Engagement_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q1", model.Lit_SEE_Engagement_FY23_24_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q2", model.Lit_SEE_Engagement_FY23_24_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q3", model.Lit_SEE_Engagement_FY23_24_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY23_24_Q4", model.Lit_SEE_Engagement_FY23_24_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY24_25_Q1", model.Lit_SEE_Engagement_FY24_25_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_FY24_25_Q2", model.Lit_SEE_Engagement_FY24_25_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_Target", model.Lit_SEE_Effectiveness_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q1", model.Lit_SEE_Effectiveness_FY23_24_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q2", model.Lit_SEE_Effectiveness_FY23_24_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q3", model.Lit_SEE_Effectiveness_FY23_24_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY23_24_Q4", model.Lit_SEE_Effectiveness_FY23_24_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY24_25_Q1", model.Lit_SEE_Effectiveness_FY24_25_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_FY24_25_Q2", model.Lit_SEE_Effectiveness_FY24_25_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Baseline", model.Lit_SS_ServiceComplaints_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Target", model.Lit_SS_ServiceComplaints_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Q1", model.Lit_SS_ServiceComplaints_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Q2", model.Lit_SS_ServiceComplaints_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Baseline", model.Lit_SS_DesignLSG_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Target", model.Lit_SS_DesignLSG_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Q1", model.Lit_SS_DesignLSG_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Q2", model.Lit_SS_DesignLSG_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Baseline", model.Lit_SS_CostReduction_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Target", model.Lit_SS_CostReduction_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Q1", model.Lit_SS_CostReduction_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Q2", model.Lit_SS_CostReduction_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Baseline", model.Lit_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Target", model.Lit_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Q1", model.Lit_SS_OTIF_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Q2", model.Lit_SS_OTIF_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Baseline", model.Lit_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Target", model.Lit_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Q1", model.Lit_SS_SPMScore_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Q2", model.Lit_SS_SPMScore_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD23_24", model.Lit_CSAT_ReqSent_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Baseline", model.Lit_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Target", model.Lit_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Q1", model.Lit_CSAT_ReqSent_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Q2", model.Lit_CSAT_ReqSent_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD", model.Lit_CSAT_ReqSent_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD23_24", model.Lit_CSAT_RespRecvd_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Baseline", model.Lit_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Target", model.Lit_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Q1", model.Lit_CSAT_RespRecvd_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Q2", model.Lit_CSAT_RespRecvd_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD", model.Lit_CSAT_RespRecvd_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD23_24", model.Lit_CSAT_Promoter_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Baseline", model.Lit_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Target", model.Lit_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Q1", model.Lit_CSAT_Promoter_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Q2", model.Lit_CSAT_Promoter_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD", model.Lit_CSAT_Promoter_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD23_24", model.Lit_CSAT_Detractor_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Baseline", model.Lit_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Target", model.Lit_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Q1", model.Lit_CSAT_Detractor_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Q2", model.Lit_CSAT_Detractor_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD", model.Lit_CSAT_Detractor_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD23_24", model.Lit_CSAT_NPS_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Baseline", model.Lit_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Target", model.Lit_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Q1", model.Lit_CSAT_NPS_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Q2", model.Lit_CSAT_NPS_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD", model.Lit_CSAT_NPS_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_Q1", model.Lit_SPM_Supp1_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_Q2", model.Lit_SPM_Supp1_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_Q1", model.Lit_SPM_Supp2_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_Q2", model.Lit_SPM_Supp2_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_Q1", model.Lit_SPM_Supp3_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_Q2", model.Lit_SPM_Supp3_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_Q1", model.Lit_SPM_Supp4_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_Q2", model.Lit_SPM_Supp4_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_Q1", model.Lit_SPM_Supp5_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_Q2", model.Lit_SPM_Supp5_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_Q1", model.Lit_SPM_Supp6_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_Q2", model.Lit_SPM_Supp6_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_Q1", model.Lit_SPM_Supp7_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_Q2", model.Lit_SPM_Supp7_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_Q1", model.Lit_SPM_Supp8_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_Q2", model.Lit_SPM_Supp8_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_Q1", model.Lit_SPM_Supp9_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_Q2", model.Lit_SPM_Supp9_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_Q1", model.Lit_SPM_Supp10_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_Q2", model.Lit_SPM_Supp10_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD23_24", model.Lit_OTIF_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Target", model.Lit_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Q1", model.Lit_OTIF_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Q2", model.Lit_OTIF_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD", model.Lit_OTIF_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD23_24", model.Lit_SC_Closure_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Baseline", model.Lit_SC_Closure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Target", model.Lit_SC_Closure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Q1", model.Lit_SC_Closure_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Q2", model.Lit_SC_Closure_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD", model.Lit_SC_Closure_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD23_24", model.Lit_CSO_TotalLogged_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_Q1", model.Lit_CSO_TotalLogged_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_Q2", model.Lit_CSO_TotalLogged_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD", model.Lit_CSO_TotalLogged_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD23_24", model.Lit_CSO_TotalAClass_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_Q1", model.Lit_CSO_TotalAClass_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_Q2", model.Lit_CSO_TotalAClass_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD", model.Lit_CSO_TotalAClass_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD23_24", model.Lit_CSO_AClassClosed_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_Q1", model.Lit_CSO_AClassClosed_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_Q2", model.Lit_CSO_AClassClosed_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD", model.Lit_CSO_AClassClosed_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD23_24", model.Lit_CSO_AClassClosedLess45_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_Q1", model.Lit_CSO_AClassClosedLess45_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_Q2", model.Lit_CSO_AClassClosedLess45_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD", model.Lit_CSO_AClassClosedLess45_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD23_24", model.Lit_CSO_PercentageClosure_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_Q1", model.Lit_CSO_PercentageClosure_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_Q2", model.Lit_CSO_PercentageClosure_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD", model.Lit_CSO_PercentageClosure_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD23_24", model.Lit_CostSavings_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Target", model.Lit_CostSavings_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Q1", model.Lit_CostSavings_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Q2", model.Lit_CostSavings_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD", model.Lit_CostSavings_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_YTD23_24", model.Lit_OQL_ArtLuminaires_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_Target", model.Lit_OQL_ArtLuminaires_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC01", model.Lit_OQL_ArtLuminaires_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC02", model.Lit_OQL_ArtLuminaires_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC03", model.Lit_OQL_ArtLuminaires_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC04", model.Lit_OQL_ArtLuminaires_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC05", model.Lit_OQL_ArtLuminaires_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_ArtLuminaires_PC06", model.Lit_OQL_ArtLuminaires_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_YTD23_24", model.Lit_OQL_IdealLighting_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_Target", model.Lit_OQL_IdealLighting_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC01", model.Lit_OQL_IdealLighting_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC02", model.Lit_OQL_IdealLighting_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC03", model.Lit_OQL_IdealLighting_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC04", model.Lit_OQL_IdealLighting_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC05", model.Lit_OQL_IdealLighting_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_IdealLighting_PC06", model.Lit_OQL_IdealLighting_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_YTD23_24", model.Lit_OQL_Everlite_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_Target", model.Lit_OQL_Everlite_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC01", model.Lit_OQL_Everlite_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC02", model.Lit_OQL_Everlite_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC03", model.Lit_OQL_Everlite_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC04", model.Lit_OQL_Everlite_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC05", model.Lit_OQL_Everlite_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Everlite_PC06", model.Lit_OQL_Everlite_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_YTD23_24", model.Lit_OQL_Rama_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_Target", model.Lit_OQL_Rama_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC01", model.Lit_OQL_Rama_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC02", model.Lit_OQL_Rama_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC03", model.Lit_OQL_Rama_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC04", model.Lit_OQL_Rama_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC05", model.Lit_OQL_Rama_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Rama_PC06", model.Lit_OQL_Rama_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_YTD23_24", model.Lit_OQL_Shantinath_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_Target", model.Lit_OQL_Shantinath_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC01", model.Lit_OQL_Shantinath_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC02", model.Lit_OQL_Shantinath_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC03", model.Lit_OQL_Shantinath_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC04", model.Lit_OQL_Shantinath_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC05", model.Lit_OQL_Shantinath_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Shantinath_PC06", model.Lit_OQL_Shantinath_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_YTD23_24", model.Lit_OQL_Varun_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_Target", model.Lit_OQL_Varun_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC01", model.Lit_OQL_Varun_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC02", model.Lit_OQL_Varun_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC03", model.Lit_OQL_Varun_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC04", model.Lit_OQL_Varun_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC05", model.Lit_OQL_Varun_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Varun_PC06", model.Lit_OQL_Varun_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_YTD23_24", model.Lit_OQL_Ujas_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_Target", model.Lit_OQL_Ujas_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC01", model.Lit_OQL_Ujas_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC02", model.Lit_OQL_Ujas_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC03", model.Lit_OQL_Ujas_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC04", model.Lit_OQL_Ujas_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC05", model.Lit_OQL_Ujas_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Ujas_PC06", model.Lit_OQL_Ujas_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_YTD23_24", model.Lit_OQL_NAK_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_Target", model.Lit_OQL_NAK_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC01", model.Lit_OQL_NAK_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC02", model.Lit_OQL_NAK_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC03", model.Lit_OQL_NAK_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC04", model.Lit_OQL_NAK_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC05", model.Lit_OQL_NAK_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_NAK_PC06", model.Lit_OQL_NAK_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_YTD23_24", model.Lit_OQL_CumulativeAvg1_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_Target", model.Lit_OQL_CumulativeAvg1_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC01", model.Lit_OQL_CumulativeAvg1_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC02", model.Lit_OQL_CumulativeAvg1_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC03", model.Lit_OQL_CumulativeAvg1_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC04", model.Lit_OQL_CumulativeAvg1_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC05", model.Lit_OQL_CumulativeAvg1_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC06", model.Lit_OQL_CumulativeAvg1_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_YTD23_24", model.Lit_OQL_CumulativeAvg2_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_Target", model.Lit_OQL_CumulativeAvg2_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC01", model.Lit_OQL_CumulativeAvg2_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC02", model.Lit_OQL_CumulativeAvg2_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC03", model.Lit_OQL_CumulativeAvg2_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC04", model.Lit_OQL_CumulativeAvg2_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC05", model.Lit_OQL_CumulativeAvg2_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC06", model.Lit_OQL_CumulativeAvg2_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Baseline", model.Seat_SS_CNClosure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Target", model.Seat_SS_CNClosure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Q1", model.Seat_SS_CNClosure_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Q2", model.Seat_SS_CNClosure_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_YTD", model.Seat_SS_CNClosure_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Baseline", model.Seat_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Target", model.Seat_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Q1", model.Seat_SS_OTIF_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Q2", model.Seat_SS_OTIF_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_YTD", model.Seat_SS_OTIF_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Baseline", model.Seat_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Target", model.Seat_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Q1", model.Seat_SS_SPMScore_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Q2", model.Seat_SS_SPMScore_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_YTD", model.Seat_SS_SPMScore_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD23_24", model.Seat_OTIF_Performance_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Target", model.Seat_OTIF_Performance_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Q1", model.Seat_OTIF_Performance_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Q2", model.Seat_OTIF_Performance_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD", model.Seat_OTIF_Performance_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD23_24", model.Seat_CSAT_ReqSent_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Baseline", model.Seat_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Target", model.Seat_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Q1", model.Seat_CSAT_ReqSent_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Q2", model.Seat_CSAT_ReqSent_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD", model.Seat_CSAT_ReqSent_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD23_24", model.Seat_CSAT_RespRecvd_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Baseline", model.Seat_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Target", model.Seat_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Q1", model.Seat_CSAT_RespRecvd_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Q2", model.Seat_CSAT_RespRecvd_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD", model.Seat_CSAT_RespRecvd_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD23_24", model.Seat_CSAT_Promoter_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Baseline", model.Seat_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Target", model.Seat_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Q1", model.Seat_CSAT_Promoter_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Q2", model.Seat_CSAT_Promoter_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD", model.Seat_CSAT_Promoter_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD23_24", model.Seat_CSAT_Detractor_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Baseline", model.Seat_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Target", model.Seat_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Q1", model.Seat_CSAT_Detractor_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Q2", model.Seat_CSAT_Detractor_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD", model.Seat_CSAT_Detractor_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD23_24", model.Seat_CSAT_NPS_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Baseline", model.Seat_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Target", model.Seat_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Q1", model.Seat_CSAT_NPS_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Q2", model.Seat_CSAT_NPS_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD", model.Seat_CSAT_NPS_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD23_24", model.Seat_CSO_TotalLogged_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_Q1", model.Seat_CSO_TotalLogged_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_Q2", model.Seat_CSO_TotalLogged_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD", model.Seat_CSO_TotalLogged_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD23_24", model.Seat_CSO_TotalAClass_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_Q1", model.Seat_CSO_TotalAClass_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_Q2", model.Seat_CSO_TotalAClass_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD", model.Seat_CSO_TotalAClass_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD23_24", model.Seat_CSO_AClassClosed_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_Q1", model.Seat_CSO_AClassClosed_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_Q2", model.Seat_CSO_AClassClosed_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD", model.Seat_CSO_AClassClosed_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_YTD23_24", model.Seat_CSO_AClassClosedLess48_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_Q1", model.Seat_CSO_AClassClosedLess48_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_Q2", model.Seat_CSO_AClassClosedLess48_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess48_YTD", model.Seat_CSO_AClassClosedLess48_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_YTD23_24", model.Seat_CSO_AClassClosedUnder48_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_Q1", model.Seat_CSO_AClassClosedUnder48_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_Q2", model.Seat_CSO_AClassClosedUnder48_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder48_YTD", model.Seat_CSO_AClassClosedUnder48_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_MPPL_Q4_23_24", model.Seat_SPM_MPPL_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_MPPL_Q1_24_25", model.Seat_SPM_MPPL_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_MPPL_Q2_24_25", model.Seat_SPM_MPPL_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_EXCLUSIFF_Q4_23_24", model.Seat_SPM_EXCLUSIFF_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_EXCLUSIFF_Q1_24_25", model.Seat_SPM_EXCLUSIFF_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_EXCLUSIFF_Q2_24_25", model.Seat_SPM_EXCLUSIFF_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_CVG_Q4_23_24", model.Seat_SPM_CVG_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_CVG_Q1_24_25", model.Seat_SPM_CVG_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_CVG_Q2_24_25", model.Seat_SPM_CVG_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_STARSHINE_Q4_23_24", model.Seat_SPM_STARSHINE_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_STARSHINE_Q1_24_25", model.Seat_SPM_STARSHINE_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_STARSHINE_Q2_24_25", model.Seat_SPM_STARSHINE_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_SAVITON_Q4_23_24", model.Seat_SPM_SAVITON_Q4_23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_SAVITON_Q1_24_25", model.Seat_SPM_SAVITON_Q1_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_SAVITON_Q2_24_25", model.Seat_SPM_SAVITON_Q2_24_25 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD23_24", model.Seat_IQA_TotalSites_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Target", model.Seat_IQA_TotalSites_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Q1", model.Seat_IQA_TotalSites_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Q2", model.Seat_IQA_TotalSites_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD", model.Seat_IQA_TotalSites_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD23_24", model.Seat_IQA_SitesCompleted_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Target", model.Seat_IQA_SitesCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Q1", model.Seat_IQA_SitesCompleted_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Q2", model.Seat_IQA_SitesCompleted_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD", model.Seat_IQA_SitesCompleted_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD23_24", model.Seat_IQA_AuditsCompleted_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Target", model.Seat_IQA_AuditsCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Q1", model.Seat_IQA_AuditsCompleted_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Q2", model.Seat_IQA_AuditsCompleted_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD", model.Seat_IQA_AuditsCompleted_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD23_24", model.Seat_IQA_PercCompleted_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Target", model.Seat_IQA_PercCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Q1", model.Seat_IQA_PercCompleted_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Q2", model.Seat_IQA_PercCompleted_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD", model.Seat_IQA_PercCompleted_YTD ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD23_24", model.Seat_IQA_AvgSigma_YTD23_24 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Target", model.Seat_IQA_AvgSigma_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Q1", model.Seat_IQA_AvgSigma_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Q2", model.Seat_IQA_AvgSigma_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD", model.Seat_IQA_AvgSigma_YTD ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_AHPNote " +
                    "@Id, " +
                    "@Lit_SEE_Engagement_Target, " +
                    "@Lit_SEE_Engagement_FY23_24_Q1, " +
                    "@Lit_SEE_Engagement_FY23_24_Q2, " +
                    "@Lit_SEE_Engagement_FY23_24_Q3, " +
                    "@Lit_SEE_Engagement_FY23_24_Q4, " +
                    "@Lit_SEE_Engagement_FY24_25_Q1, " +
                    "@Lit_SEE_Engagement_FY24_25_Q2, " +
                    "@Lit_SEE_Effectiveness_Target, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q1, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q2, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q3, " +
                    "@Lit_SEE_Effectiveness_FY23_24_Q4, " +
                    "@Lit_SEE_Effectiveness_FY24_25_Q1, " +
                    "@Lit_SEE_Effectiveness_FY24_25_Q2, " +
                    "@Lit_SS_ServiceComplaints_Baseline, " +
                    "@Lit_SS_ServiceComplaints_Target, " +
                    "@Lit_SS_ServiceComplaints_Q1, " +
                    "@Lit_SS_ServiceComplaints_Q2, " +
                    "@Lit_SS_DesignLSG_Baseline, " +
                    "@Lit_SS_DesignLSG_Target, " +
                    "@Lit_SS_DesignLSG_Q1, " +
                    "@Lit_SS_DesignLSG_Q2, " +
                    "@Lit_SS_CostReduction_Baseline, " +
                    "@Lit_SS_CostReduction_Target, " +
                    "@Lit_SS_CostReduction_Q1, " +
                    "@Lit_SS_CostReduction_Q2, " +
                    "@Lit_SS_OTIF_Baseline, " +
                    "@Lit_SS_OTIF_Target, " +
                    "@Lit_SS_OTIF_Q1, " +
                    "@Lit_SS_OTIF_Q2, " +
                    "@Lit_SS_SPMScore_Baseline, " +
                    "@Lit_SS_SPMScore_Target, " +
                    "@Lit_SS_SPMScore_Q1, " +
                    "@Lit_SS_SPMScore_Q2, " +
                    "@Lit_CSAT_ReqSent_YTD23_24, " +
                    "@Lit_CSAT_ReqSent_Baseline, " +
                    "@Lit_CSAT_ReqSent_Target, " +
                    "@Lit_CSAT_ReqSent_Q1, " +
                    "@Lit_CSAT_ReqSent_Q2, " +
                    "@Lit_CSAT_ReqSent_YTD, " +
                    "@Lit_CSAT_RespRecvd_YTD23_24, " +
                    "@Lit_CSAT_RespRecvd_Baseline, " +
                    "@Lit_CSAT_RespRecvd_Target, " +
                    "@Lit_CSAT_RespRecvd_Q1, " +
                    "@Lit_CSAT_RespRecvd_Q2, " +
                    "@Lit_CSAT_RespRecvd_YTD, " +
                    "@Lit_CSAT_Promoter_YTD23_24, " +
                    "@Lit_CSAT_Promoter_Baseline, " +
                    "@Lit_CSAT_Promoter_Target, " +
                    "@Lit_CSAT_Promoter_Q1, " +
                    "@Lit_CSAT_Promoter_Q2, " +
                    "@Lit_CSAT_Promoter_YTD, " +
                    "@Lit_CSAT_Detractor_YTD23_24, " +
                    "@Lit_CSAT_Detractor_Baseline, " +
                    "@Lit_CSAT_Detractor_Target, " +
                    "@Lit_CSAT_Detractor_Q1, " +
                    "@Lit_CSAT_Detractor_Q2, " +
                    "@Lit_CSAT_Detractor_YTD, " +
                    "@Lit_CSAT_NPS_YTD23_24, " +
                    "@Lit_CSAT_NPS_Baseline, " +
                    "@Lit_CSAT_NPS_Target, " +
                    "@Lit_CSAT_NPS_Q1, " +
                    "@Lit_CSAT_NPS_Q2, " +
                    "@Lit_CSAT_NPS_YTD, " +
                    "@Lit_SPM_Supp1_Q1, " +
                    "@Lit_SPM_Supp1_Q2, " +
                    "@Lit_SPM_Supp2_Q1, " +
                    "@Lit_SPM_Supp2_Q2, " +
                    "@Lit_SPM_Supp3_Q1, " +
                    "@Lit_SPM_Supp3_Q2, " +
                    "@Lit_SPM_Supp4_Q1, " +
                    "@Lit_SPM_Supp4_Q2, " +
                    "@Lit_SPM_Supp5_Q1, " +
                    "@Lit_SPM_Supp5_Q2, " +
                    "@Lit_SPM_Supp6_Q1, " +
                    "@Lit_SPM_Supp6_Q2, " +
                    "@Lit_SPM_Supp7_Q1, " +
                    "@Lit_SPM_Supp7_Q2, " +
                    "@Lit_SPM_Supp8_Q1, " +
                    "@Lit_SPM_Supp8_Q2, " +
                    "@Lit_SPM_Supp9_Q1, " +
                    "@Lit_SPM_Supp9_Q2, " +
                    "@Lit_SPM_Supp10_Q1, " +
                    "@Lit_SPM_Supp10_Q2, " +
                    "@Lit_OTIF_YTD23_24, " +
                    "@Lit_OTIF_Target, " +
                    "@Lit_OTIF_Q1, " +
                    "@Lit_OTIF_Q2, " +
                    "@Lit_OTIF_YTD, " +
                    "@Lit_SC_Closure_YTD23_24, " +
                    "@Lit_SC_Closure_Baseline, " +
                    "@Lit_SC_Closure_Target, " +
                    "@Lit_SC_Closure_Q1, " +
                    "@Lit_SC_Closure_Q2, " +
                    "@Lit_SC_Closure_YTD, " +
                    "@Lit_CSO_TotalLogged_YTD23_24, " +
                    "@Lit_CSO_TotalLogged_Q1, " +
                    "@Lit_CSO_TotalLogged_Q2, " +
                    "@Lit_CSO_TotalLogged_YTD, " +
                    "@Lit_CSO_TotalAClass_YTD23_24, " +
                    "@Lit_CSO_TotalAClass_Q1, " +
                    "@Lit_CSO_TotalAClass_Q2, " +
                    "@Lit_CSO_TotalAClass_YTD, " +
                    "@Lit_CSO_AClassClosed_YTD23_24, " +
                    "@Lit_CSO_AClassClosed_Q1, " +
                    "@Lit_CSO_AClassClosed_Q2, " +
                    "@Lit_CSO_AClassClosed_YTD, " +
                    "@Lit_CSO_AClassClosedLess45_YTD23_24, " +
                    "@Lit_CSO_AClassClosedLess45_Q1, " +
                    "@Lit_CSO_AClassClosedLess45_Q2, " +
                    "@Lit_CSO_AClassClosedLess45_YTD, " +
                    "@Lit_CSO_PercentageClosure_YTD23_24, " +
                    "@Lit_CSO_PercentageClosure_Q1, " +
                    "@Lit_CSO_PercentageClosure_Q2, " +
                    "@Lit_CSO_PercentageClosure_YTD, " +
                    "@Lit_CostSavings_YTD23_24, " +
                    "@Lit_CostSavings_Target, " +
                    "@Lit_CostSavings_Q1, " +
                    "@Lit_CostSavings_Q2, " +
                    "@Lit_CostSavings_YTD, " +
                    "@Lit_OQL_ArtLuminaires_YTD23_24, " +
                    "@Lit_OQL_ArtLuminaires_Target, " +
                    "@Lit_OQL_ArtLuminaires_PC01, " +
                    "@Lit_OQL_ArtLuminaires_PC02, " +
                    "@Lit_OQL_ArtLuminaires_PC03, " +
                    "@Lit_OQL_ArtLuminaires_PC04, " +
                    "@Lit_OQL_ArtLuminaires_PC05, " +
                    "@Lit_OQL_ArtLuminaires_PC06, " +
                    "@Lit_OQL_IdealLighting_YTD23_24, " +
                    "@Lit_OQL_IdealLighting_Target, " +
                    "@Lit_OQL_IdealLighting_PC01, " +
                    "@Lit_OQL_IdealLighting_PC02, " +
                    "@Lit_OQL_IdealLighting_PC03, " +
                    "@Lit_OQL_IdealLighting_PC04, " +
                    "@Lit_OQL_IdealLighting_PC05, " +
                    "@Lit_OQL_IdealLighting_PC06, " +
                    "@Lit_OQL_Everlite_YTD23_24, " +
                    "@Lit_OQL_Everlite_Target, " +
                    "@Lit_OQL_Everlite_PC01, " +
                    "@Lit_OQL_Everlite_PC02, " +
                    "@Lit_OQL_Everlite_PC03, " +
                    "@Lit_OQL_Everlite_PC04, " +
                    "@Lit_OQL_Everlite_PC05, " +
                    "@Lit_OQL_Everlite_PC06, " +
                    "@Lit_OQL_Rama_YTD23_24, " +
                    "@Lit_OQL_Rama_Target, " +
                    "@Lit_OQL_Rama_PC01, " +
                    "@Lit_OQL_Rama_PC02, " +
                    "@Lit_OQL_Rama_PC03, " +
                    "@Lit_OQL_Rama_PC04, " +
                    "@Lit_OQL_Rama_PC05, " +
                    "@Lit_OQL_Rama_PC06, " +
                    "@Lit_OQL_Shantinath_YTD23_24, " +
                    "@Lit_OQL_Shantinath_Target, " +
                    "@Lit_OQL_Shantinath_PC01, " +
                    "@Lit_OQL_Shantinath_PC02, " +
                    "@Lit_OQL_Shantinath_PC03, " +
                    "@Lit_OQL_Shantinath_PC04, " +
                    "@Lit_OQL_Shantinath_PC05, " +
                    "@Lit_OQL_Shantinath_PC06, " +
                    "@Lit_OQL_Varun_YTD23_24, " +
                    "@Lit_OQL_Varun_Target, " +
                    "@Lit_OQL_Varun_PC01, " +
                    "@Lit_OQL_Varun_PC02, " +
                    "@Lit_OQL_Varun_PC03, " +
                    "@Lit_OQL_Varun_PC04, " +
                    "@Lit_OQL_Varun_PC05, " +
                    "@Lit_OQL_Varun_PC06, " +
                    "@Lit_OQL_Ujas_YTD23_24, " +
                    "@Lit_OQL_Ujas_Target, " +
                    "@Lit_OQL_Ujas_PC01, " +
                    "@Lit_OQL_Ujas_PC02, " +
                    "@Lit_OQL_Ujas_PC03, " +
                    "@Lit_OQL_Ujas_PC04, " +
                    "@Lit_OQL_Ujas_PC05, " +
                    "@Lit_OQL_Ujas_PC06, " +
                    "@Lit_OQL_NAK_YTD23_24, " +
                    "@Lit_OQL_NAK_Target, " +
                    "@Lit_OQL_NAK_PC01, " +
                    "@Lit_OQL_NAK_PC02, " +
                    "@Lit_OQL_NAK_PC03, " +
                    "@Lit_OQL_NAK_PC04, " +
                    "@Lit_OQL_NAK_PC05, " +
                    "@Lit_OQL_NAK_PC06, " +
                    "@Lit_OQL_CumulativeAvg1_YTD23_24, " +
                    "@Lit_OQL_CumulativeAvg1_Target, " +
                    "@Lit_OQL_CumulativeAvg1_PC01, " +
                    "@Lit_OQL_CumulativeAvg1_PC02, " +
                    "@Lit_OQL_CumulativeAvg1_PC03, " +
                    "@Lit_OQL_CumulativeAvg1_PC04, " +
                    "@Lit_OQL_CumulativeAvg1_PC05, " +
                    "@Lit_OQL_CumulativeAvg1_PC06, " +
                    "@Lit_OQL_CumulativeAvg2_YTD23_24, " +
                    "@Lit_OQL_CumulativeAvg2_Target, " +
                    "@Lit_OQL_CumulativeAvg2_PC01, " +
                    "@Lit_OQL_CumulativeAvg2_PC02, " +
                    "@Lit_OQL_CumulativeAvg2_PC03, " +
                    "@Lit_OQL_CumulativeAvg2_PC04, " +
                    "@Lit_OQL_CumulativeAvg2_PC05, " +
                    "@Lit_OQL_CumulativeAvg2_PC06, " +
                    "@Seat_SS_CNClosure_Baseline, " +
                    "@Seat_SS_CNClosure_Target, " +
                    "@Seat_SS_CNClosure_Q1, " +
                    "@Seat_SS_CNClosure_Q2, " +
                    "@Seat_SS_CNClosure_YTD, " +
                    "@Seat_SS_OTIF_Baseline, " +
                    "@Seat_SS_OTIF_Target, " +
                    "@Seat_SS_OTIF_Q1, " +
                    "@Seat_SS_OTIF_Q2, " +
                    "@Seat_SS_OTIF_YTD, " +
                    "@Seat_SS_SPMScore_Baseline, " +
                    "@Seat_SS_SPMScore_Target, " +
                    "@Seat_SS_SPMScore_Q1, " +
                    "@Seat_SS_SPMScore_Q2, " +
                    "@Seat_SS_SPMScore_YTD, " +
                    "@Seat_OTIF_Performance_YTD23_24, " +
                    "@Seat_OTIF_Performance_Target, " +
                    "@Seat_OTIF_Performance_Q1, " +
                    "@Seat_OTIF_Performance_Q2, " +
                    "@Seat_OTIF_Performance_YTD, " +
                    "@Seat_CSAT_ReqSent_YTD23_24, " +
                    "@Seat_CSAT_ReqSent_Baseline, " +
                    "@Seat_CSAT_ReqSent_Target, " +
                    "@Seat_CSAT_ReqSent_Q1, " +
                    "@Seat_CSAT_ReqSent_Q2, " +
                    "@Seat_CSAT_ReqSent_YTD, " +
                    "@Seat_CSAT_RespRecvd_YTD23_24, " +
                    "@Seat_CSAT_RespRecvd_Baseline, " +
                    "@Seat_CSAT_RespRecvd_Target, " +
                    "@Seat_CSAT_RespRecvd_Q1, " +
                    "@Seat_CSAT_RespRecvd_Q2, " +
                    "@Seat_CSAT_RespRecvd_YTD, " +
                    "@Seat_CSAT_Promoter_YTD23_24, " +
                    "@Seat_CSAT_Promoter_Baseline, " +
                    "@Seat_CSAT_Promoter_Target, " +
                    "@Seat_CSAT_Promoter_Q1, " +
                    "@Seat_CSAT_Promoter_Q2, " +
                    "@Seat_CSAT_Promoter_YTD, " +
                    "@Seat_CSAT_Detractor_YTD23_24, " +
                    "@Seat_CSAT_Detractor_Baseline, " +
                    "@Seat_CSAT_Detractor_Target, " +
                    "@Seat_CSAT_Detractor_Q1, " +
                    "@Seat_CSAT_Detractor_Q2, " +
                    "@Seat_CSAT_Detractor_YTD, " +
                    "@Seat_CSAT_NPS_YTD23_24, " +
                    "@Seat_CSAT_NPS_Baseline, " +
                    "@Seat_CSAT_NPS_Target, " +
                    "@Seat_CSAT_NPS_Q1, " +
                    "@Seat_CSAT_NPS_Q2, " +
                    "@Seat_CSAT_NPS_YTD, " +
                    "@Seat_CSO_TotalLogged_YTD23_24, " +
                    "@Seat_CSO_TotalLogged_Q1, " +
                    "@Seat_CSO_TotalLogged_Q2, " +
                    "@Seat_CSO_TotalLogged_YTD, " +
                    "@Seat_CSO_TotalAClass_YTD23_24, " +
                    "@Seat_CSO_TotalAClass_Q1, " +
                    "@Seat_CSO_TotalAClass_Q2, " +
                    "@Seat_CSO_TotalAClass_YTD, " +
                    "@Seat_CSO_AClassClosed_YTD23_24, " +
                    "@Seat_CSO_AClassClosed_Q1, " +
                    "@Seat_CSO_AClassClosed_Q2, " +
                    "@Seat_CSO_AClassClosed_YTD, " +
                    "@Seat_CSO_AClassClosedLess48_YTD23_24, " +
                    "@Seat_CSO_AClassClosedLess48_Q1, " +
                    "@Seat_CSO_AClassClosedLess48_Q2, " +
                    "@Seat_CSO_AClassClosedLess48_YTD, " +
                    "@Seat_CSO_AClassClosedUnder48_YTD23_24, " +
                    "@Seat_CSO_AClassClosedUnder48_Q1, " +
                    "@Seat_CSO_AClassClosedUnder48_Q2, " +
                    "@Seat_CSO_AClassClosedUnder48_YTD, " +
                    "@Seat_SPM_MPPL_Q4_23_24, " +
                    "@Seat_SPM_MPPL_Q1_24_25, " +
                    "@Seat_SPM_MPPL_Q2_24_25, " +
                    "@Seat_SPM_EXCLUSIFF_Q4_23_24, " +
                    "@Seat_SPM_EXCLUSIFF_Q1_24_25, " +
                    "@Seat_SPM_EXCLUSIFF_Q2_24_25, " +
                    "@Seat_SPM_CVG_Q4_23_24, " +
                    "@Seat_SPM_CVG_Q1_24_25, " +
                    "@Seat_SPM_CVG_Q2_24_25, " +
                    "@Seat_SPM_STARSHINE_Q4_23_24, " +
                    "@Seat_SPM_STARSHINE_Q1_24_25, " +
                    "@Seat_SPM_STARSHINE_Q2_24_25, " +
                    "@Seat_SPM_SAVITON_Q4_23_24, " +
                    "@Seat_SPM_SAVITON_Q1_24_25, " +
                    "@Seat_SPM_SAVITON_Q2_24_25, " +
                    "@Seat_IQA_TotalSites_YTD23_24, " +
                    "@Seat_IQA_TotalSites_Target, " +
                    "@Seat_IQA_TotalSites_Q1, " +
                    "@Seat_IQA_TotalSites_Q2, " +
                    "@Seat_IQA_TotalSites_YTD, " +
                    "@Seat_IQA_SitesCompleted_YTD23_24, " +
                    "@Seat_IQA_SitesCompleted_Target, " +
                    "@Seat_IQA_SitesCompleted_Q1, " +
                    "@Seat_IQA_SitesCompleted_Q2, " +
                    "@Seat_IQA_SitesCompleted_YTD, " +
                    "@Seat_IQA_AuditsCompleted_YTD23_24, " +
                    "@Seat_IQA_AuditsCompleted_Target, " +
                    "@Seat_IQA_AuditsCompleted_Q1, " +
                    "@Seat_IQA_AuditsCompleted_Q2, " +
                    "@Seat_IQA_AuditsCompleted_YTD, " +
                    "@Seat_IQA_PercCompleted_YTD23_24, " +
                    "@Seat_IQA_PercCompleted_Target, " +
                    "@Seat_IQA_PercCompleted_Q1, " +
                    "@Seat_IQA_PercCompleted_Q2, " +
                    "@Seat_IQA_PercCompleted_YTD, " +
                    "@Seat_IQA_AvgSigma_YTD23_24, " +
                    "@Seat_IQA_AvgSigma_Target, " +
                    "@Seat_IQA_AvgSigma_Q1, " +
                    "@Seat_IQA_AvgSigma_Q2, " +
                    "@Seat_IQA_AvgSigma_YTD, " +
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

    public async Task<OperationResult> DeleteAHPNotesAsync(int Id)
    {
        try
        {
            var result = await base.DeleteAsync<AHP_Note>(Id);
            return result;
        }
        catch (Exception ex)
        {
            _systemLogService.WriteLog(ex.Message);
            throw;
        }
    }
}
