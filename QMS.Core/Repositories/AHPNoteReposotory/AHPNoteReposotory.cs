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

    public async Task<List<AHPNoteViewModel>> GetAHPNotesAsync(int financialYear)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", SqlDbType.Int)
                {
                    Value = 0
                },
                new SqlParameter("@FinancialYear", financialYear)
            };

            var sql = @"EXEC sp_Get_AHPNote @Id, @FinancialYear";

            var result = await Task.Run(() => _dbContext.AHPNote.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new AHPNoteViewModel
                {
                    Id = x.Id,
                    AddedBy = x.AddedBy,
                    FinancialYear = x.FinancialYear,
                    Lit_SEE_Engagement_Target = x.Lit_SEE_Engagement_Target,
                    Lit_SEE_Engagement_PreviousYear_Q1 = x.Lit_SEE_Engagement_PreviousYear_Q1,
                    Lit_SEE_Engagement_PreviousYear_Q2 = x.Lit_SEE_Engagement_PreviousYear_Q2,
                    Lit_SEE_Engagement_PreviousYear_Q3 = x.Lit_SEE_Engagement_PreviousYear_Q3,
                    Lit_SEE_Engagement_PreviousYear_Q4 = x.Lit_SEE_Engagement_PreviousYear_Q4,
                    Lit_SEE_Engagement_CurrentYear_Q1 = x.Lit_SEE_Engagement_CurrentYear_Q1,
                    Lit_SEE_Engagement_CurrentYear_Q2 = x.Lit_SEE_Engagement_CurrentYear_Q2,
                    Lit_SEE_Engagement_CurrentYear_Q3 = x.Lit_SEE_Engagement_CurrentYear_Q3,
                    Lit_SEE_Engagement_CurrentYear_Q4 = x.Lit_SEE_Engagement_CurrentYear_Q4,
                    Lit_SEE_Effectiveness_Target = x.Lit_SEE_Effectiveness_Target,
                    Lit_SEE_Effectiveness_PreviousYear_Q1 = x.Lit_SEE_Effectiveness_PreviousYear_Q1,
                    Lit_SEE_Effectiveness_PreviousYear_Q2 = x.Lit_SEE_Effectiveness_PreviousYear_Q2,
                    Lit_SEE_Effectiveness_PreviousYear_Q3 = x.Lit_SEE_Effectiveness_PreviousYear_Q3,
                    Lit_SEE_Effectiveness_PreviousYear_Q4 = x.Lit_SEE_Effectiveness_PreviousYear_Q4,
                    Lit_SEE_Effectiveness_CurrentYear_Q1 = x.Lit_SEE_Effectiveness_CurrentYear_Q1,
                    Lit_SEE_Effectiveness_CurrentYear_Q2 = x.Lit_SEE_Effectiveness_CurrentYear_Q2,
                    Lit_SEE_Effectiveness_CurrentYear_Q3 = x.Lit_SEE_Effectiveness_CurrentYear_Q3,
                    Lit_SEE_Effectiveness_CurrentYear_Q4 = x.Lit_SEE_Effectiveness_CurrentYear_Q4,
                    Lit_SS_ServiceComplaints_Baseline = x.Lit_SS_ServiceComplaints_Baseline,
                    Lit_SS_ServiceComplaints_Target = x.Lit_SS_ServiceComplaints_Target,
                    Lit_SS_ServiceComplaints_CurrentYear_Q1 = x.Lit_SS_ServiceComplaints_CurrentYear_Q1,
                    Lit_SS_ServiceComplaints_CurrentYear_Q2 = x.Lit_SS_ServiceComplaints_CurrentYear_Q2,
                    Lit_SS_ServiceComplaints_CurrentYear_Q3 = x.Lit_SS_ServiceComplaints_CurrentYear_Q3,
                    Lit_SS_ServiceComplaints_CurrentYear_Q4 = x.Lit_SS_ServiceComplaints_CurrentYear_Q4,
                    Lit_SS_DesignLSG_Baseline = x.Lit_SS_DesignLSG_Baseline,
                    Lit_SS_DesignLSG_Target = x.Lit_SS_DesignLSG_Target,
                    Lit_SS_DesignLSG_CurrentYear_Q1 = x.Lit_SS_DesignLSG_CurrentYear_Q1,
                    Lit_SS_DesignLSG_CurrentYear_Q2 = x.Lit_SS_DesignLSG_CurrentYear_Q2,
                    Lit_SS_DesignLSG_CurrentYear_Q3 = x.Lit_SS_DesignLSG_CurrentYear_Q3,
                    Lit_SS_DesignLSG_CurrentYear_Q4 = x.Lit_SS_DesignLSG_CurrentYear_Q4,
                    Lit_SS_CostReduction_Baseline = x.Lit_SS_CostReduction_Baseline,
                    Lit_SS_CostReduction_Target = x.Lit_SS_CostReduction_Target,
                    Lit_SS_CostReduction_CurrentYear_Q1 = x.Lit_SS_CostReduction_CurrentYear_Q1,
                    Lit_SS_CostReduction_CurrentYear_Q2 = x.Lit_SS_CostReduction_CurrentYear_Q2,
                    Lit_SS_CostReduction_CurrentYear_Q3 = x.Lit_SS_CostReduction_CurrentYear_Q3,
                    Lit_SS_CostReduction_CurrentYear_Q4 = x.Lit_SS_CostReduction_CurrentYear_Q4,
                    Lit_SS_OTIF_Baseline = x.Lit_SS_OTIF_Baseline,
                    Lit_SS_OTIF_Target = x.Lit_SS_OTIF_Target,
                    Lit_SS_OTIF_CurrentYear_Q1 = x.Lit_SS_OTIF_CurrentYear_Q1,
                    Lit_SS_OTIF_CurrentYear_Q2 = x.Lit_SS_OTIF_CurrentYear_Q2,
                    Lit_SS_OTIF_CurrentYear_Q3 = x.Lit_SS_OTIF_CurrentYear_Q3,
                    Lit_SS_OTIF_CurrentYear_Q4 = x.Lit_SS_OTIF_CurrentYear_Q4,
                    Lit_SS_SPMScore_Baseline = x.Lit_SS_SPMScore_Baseline,
                    Lit_SS_SPMScore_Target = x.Lit_SS_SPMScore_Target,
                    Lit_SS_SPMScore_CurrentYear_Q1 = x.Lit_SS_SPMScore_CurrentYear_Q1,
                    Lit_SS_SPMScore_CurrentYear_Q2 = x.Lit_SS_SPMScore_CurrentYear_Q2,
                    Lit_SS_SPMScore_CurrentYear_Q3 = x.Lit_SS_SPMScore_CurrentYear_Q3,
                    Lit_SS_SPMScore_CurrentYear_Q4 = x.Lit_SS_SPMScore_CurrentYear_Q4,
                    Lit_CSAT_ReqSent_YTD_PreviousYear = x.Lit_CSAT_ReqSent_YTD_PreviousYear,
                    Lit_CSAT_ReqSent_Baseline = x.Lit_CSAT_ReqSent_Baseline,
                    Lit_CSAT_ReqSent_Target = x.Lit_CSAT_ReqSent_Target,
                    Lit_CSAT_ReqSent_CurrentYear_Q1 = x.Lit_CSAT_ReqSent_CurrentYear_Q1,
                    Lit_CSAT_ReqSent_CurrentYear_Q2 = x.Lit_CSAT_ReqSent_CurrentYear_Q2,
                    Lit_CSAT_ReqSent_CurrentYear_Q3 = x.Lit_CSAT_ReqSent_CurrentYear_Q3,
                    Lit_CSAT_ReqSent_CurrentYear_Q4 = x.Lit_CSAT_ReqSent_CurrentYear_Q4,
                    Lit_CSAT_ReqSent_YTD_CurrentYear = x.Lit_CSAT_ReqSent_YTD_CurrentYear,
                    Lit_CSAT_RespRecvd_YTD_PreviousYear = x.Lit_CSAT_RespRecvd_YTD_PreviousYear,
                    Lit_CSAT_RespRecvd_Baseline = x.Lit_CSAT_RespRecvd_Baseline,
                    Lit_CSAT_RespRecvd_Target = x.Lit_CSAT_RespRecvd_Target,
                    Lit_CSAT_RespRecvd_CurrentYear_Q1 = x.Lit_CSAT_RespRecvd_CurrentYear_Q1,
                    Lit_CSAT_RespRecvd_CurrentYear_Q2 = x.Lit_CSAT_RespRecvd_CurrentYear_Q2,
                    Lit_CSAT_RespRecvd_CurrentYear_Q3 = x.Lit_CSAT_RespRecvd_CurrentYear_Q3,
                    Lit_CSAT_RespRecvd_CurrentYear_Q4 = x.Lit_CSAT_RespRecvd_CurrentYear_Q4,
                    Lit_CSAT_RespRecvd_YTD_CurrentYear = x.Lit_CSAT_RespRecvd_YTD_CurrentYear,
                    Lit_CSAT_Promoter_YTD_PreviousYear = x.Lit_CSAT_Promoter_YTD_PreviousYear,
                    Lit_CSAT_Promoter_Baseline = x.Lit_CSAT_Promoter_Baseline,
                    Lit_CSAT_Promoter_Target = x.Lit_CSAT_Promoter_Target,
                    Lit_CSAT_Promoter_CurrentYear_Q1 = x.Lit_CSAT_Promoter_CurrentYear_Q1,
                    Lit_CSAT_Promoter_CurrentYear_Q2 = x.Lit_CSAT_Promoter_CurrentYear_Q2,
                    Lit_CSAT_Promoter_CurrentYear_Q3 = x.Lit_CSAT_Promoter_CurrentYear_Q3,
                    Lit_CSAT_Promoter_CurrentYear_Q4 = x.Lit_CSAT_Promoter_CurrentYear_Q4,
                    Lit_CSAT_Promoter_YTD_CurrentYear = x.Lit_CSAT_Promoter_YTD_CurrentYear,
                    Lit_CSAT_Detractor_YTD_PreviousYear = x.Lit_CSAT_Detractor_YTD_PreviousYear,
                    Lit_CSAT_Detractor_Baseline = x.Lit_CSAT_Detractor_Baseline,
                    Lit_CSAT_Detractor_Target = x.Lit_CSAT_Detractor_Target,
                    Lit_CSAT_Detractor_CurrentYear_Q1 = x.Lit_CSAT_Detractor_CurrentYear_Q1,
                    Lit_CSAT_Detractor_CurrentYear_Q2 = x.Lit_CSAT_Detractor_CurrentYear_Q2,
                    Lit_CSAT_Detractor_CurrentYear_Q3 = x.Lit_CSAT_Detractor_CurrentYear_Q3,
                    Lit_CSAT_Detractor_CurrentYear_Q4 = x.Lit_CSAT_Detractor_CurrentYear_Q4,
                    Lit_CSAT_Detractor_YTD_CurrentYear = x.Lit_CSAT_Detractor_YTD_CurrentYear,
                    Lit_CSAT_NPS_YTD_PreviousYear = x.Lit_CSAT_NPS_YTD_PreviousYear,
                    Lit_CSAT_NPS_Baseline = x.Lit_CSAT_NPS_Baseline,
                    Lit_CSAT_NPS_Target = x.Lit_CSAT_NPS_Target,
                    Lit_CSAT_NPS_CurrentYear_Q1 = x.Lit_CSAT_NPS_CurrentYear_Q1,
                    Lit_CSAT_NPS_CurrentYear_Q2 = x.Lit_CSAT_NPS_CurrentYear_Q2,
                    Lit_CSAT_NPS_CurrentYear_Q3 = x.Lit_CSAT_NPS_CurrentYear_Q3,
                    Lit_CSAT_NPS_CurrentYear_Q4 = x.Lit_CSAT_NPS_CurrentYear_Q4,
                    Lit_CSAT_NPS_YTD_CurrentYear = x.Lit_CSAT_NPS_YTD_CurrentYear,
                    Lit_SPM_Supp1 = x.Lit_SPM_Supp1,
                    Lit_SPM_Supp1_CurrentYear_Q1 = x.Lit_SPM_Supp1_CurrentYear_Q1,
                    Lit_SPM_Supp1_CurrentYear_Q2 = x.Lit_SPM_Supp1_CurrentYear_Q2,
                    Lit_SPM_Supp1_CurrentYear_Q3 = x.Lit_SPM_Supp1_CurrentYear_Q3,
                    Lit_SPM_Supp1_CurrentYear_Q4 = x.Lit_SPM_Supp1_CurrentYear_Q4,
                    Lit_SPM_Supp2 = x.Lit_SPM_Supp2,
                    Lit_SPM_Supp2_CurrentYear_Q1 = x.Lit_SPM_Supp2_CurrentYear_Q1,
                    Lit_SPM_Supp2_CurrentYear_Q2 = x.Lit_SPM_Supp2_CurrentYear_Q2,
                    Lit_SPM_Supp2_CurrentYear_Q3 = x.Lit_SPM_Supp2_CurrentYear_Q3,
                    Lit_SPM_Supp2_CurrentYear_Q4 = x.Lit_SPM_Supp2_CurrentYear_Q4,
                    Lit_SPM_Supp3 = x.Lit_SPM_Supp3,
                    Lit_SPM_Supp3_CurrentYear_Q1 = x.Lit_SPM_Supp3_CurrentYear_Q1,
                    Lit_SPM_Supp3_CurrentYear_Q2 = x.Lit_SPM_Supp3_CurrentYear_Q2,
                    Lit_SPM_Supp3_CurrentYear_Q3 = x.Lit_SPM_Supp3_CurrentYear_Q3,
                    Lit_SPM_Supp3_CurrentYear_Q4 = x.Lit_SPM_Supp3_CurrentYear_Q4,
                    Lit_SPM_Supp4 = x.Lit_SPM_Supp4,
                    Lit_SPM_Supp4_CurrentYear_Q1 = x.Lit_SPM_Supp4_CurrentYear_Q1,
                    Lit_SPM_Supp4_CurrentYear_Q2 = x.Lit_SPM_Supp4_CurrentYear_Q2,
                    Lit_SPM_Supp4_CurrentYear_Q3 = x.Lit_SPM_Supp4_CurrentYear_Q3,
                    Lit_SPM_Supp4_CurrentYear_Q4 = x.Lit_SPM_Supp4_CurrentYear_Q4,
                    Lit_SPM_Supp5 = x.Lit_SPM_Supp5,
                    Lit_SPM_Supp5_CurrentYear_Q1 = x.Lit_SPM_Supp5_CurrentYear_Q1,
                    Lit_SPM_Supp5_CurrentYear_Q2 = x.Lit_SPM_Supp5_CurrentYear_Q2,
                    Lit_SPM_Supp5_CurrentYear_Q3 = x.Lit_SPM_Supp5_CurrentYear_Q3,
                    Lit_SPM_Supp5_CurrentYear_Q4 = x.Lit_SPM_Supp5_CurrentYear_Q4,
                    Lit_SPM_Supp6 = x.Lit_SPM_Supp6,
                    Lit_SPM_Supp6_CurrentYear_Q1 = x.Lit_SPM_Supp6_CurrentYear_Q1,
                    Lit_SPM_Supp6_CurrentYear_Q2 = x.Lit_SPM_Supp6_CurrentYear_Q2,
                    Lit_SPM_Supp6_CurrentYear_Q3 = x.Lit_SPM_Supp6_CurrentYear_Q3,
                    Lit_SPM_Supp6_CurrentYear_Q4 = x.Lit_SPM_Supp6_CurrentYear_Q4,
                    Lit_SPM_Supp7 = x.Lit_SPM_Supp7,
                    Lit_SPM_Supp7_CurrentYear_Q1 = x.Lit_SPM_Supp7_CurrentYear_Q1,
                    Lit_SPM_Supp7_CurrentYear_Q2 = x.Lit_SPM_Supp7_CurrentYear_Q2,
                    Lit_SPM_Supp7_CurrentYear_Q3 = x.Lit_SPM_Supp7_CurrentYear_Q3,
                    Lit_SPM_Supp7_CurrentYear_Q4 = x.Lit_SPM_Supp7_CurrentYear_Q4,
                    Lit_SPM_Supp8 = x.Lit_SPM_Supp8,
                    Lit_SPM_Supp8_CurrentYear_Q1 = x.Lit_SPM_Supp8_CurrentYear_Q1,
                    Lit_SPM_Supp8_CurrentYear_Q2 = x.Lit_SPM_Supp8_CurrentYear_Q2,
                    Lit_SPM_Supp8_CurrentYear_Q3 = x.Lit_SPM_Supp8_CurrentYear_Q3,
                    Lit_SPM_Supp8_CurrentYear_Q4 = x.Lit_SPM_Supp8_CurrentYear_Q4,
                    Lit_SPM_Supp9 = x.Lit_SPM_Supp9,
                    Lit_SPM_Supp9_CurrentYear_Q1 = x.Lit_SPM_Supp9_CurrentYear_Q1,
                    Lit_SPM_Supp9_CurrentYear_Q2 = x.Lit_SPM_Supp9_CurrentYear_Q2,
                    Lit_SPM_Supp9_CurrentYear_Q3 = x.Lit_SPM_Supp9_CurrentYear_Q3,
                    Lit_SPM_Supp9_CurrentYear_Q4 = x.Lit_SPM_Supp9_CurrentYear_Q4,
                    Lit_SPM_Supp10 = x.Lit_SPM_Supp10,
                    Lit_SPM_Supp10_CurrentYear_Q1 = x.Lit_SPM_Supp10_CurrentYear_Q1,
                    Lit_SPM_Supp10_CurrentYear_Q2 = x.Lit_SPM_Supp10_CurrentYear_Q2,
                    Lit_SPM_Supp10_CurrentYear_Q3 = x.Lit_SPM_Supp10_CurrentYear_Q3,
                    Lit_SPM_Supp10_CurrentYear_Q4 = x.Lit_SPM_Supp10_CurrentYear_Q4,
                    Lit_OTIF_YTD_PreviousYear = x.Lit_OTIF_YTD_PreviousYear,
                    Lit_OTIF_Target = x.Lit_OTIF_Target,
                    Lit_OTIF_CurrentYear_Q1 = x.Lit_OTIF_CurrentYear_Q1,
                    Lit_OTIF_CurrentYear_Q2 = x.Lit_OTIF_CurrentYear_Q2,
                    Lit_OTIF_CurrentYear_Q3 = x.Lit_OTIF_CurrentYear_Q3,
                    Lit_OTIF_CurrentYear_Q4 = x.Lit_OTIF_CurrentYear_Q4,
                    Lit_OTIF_YTD_CurrentYear = x.Lit_OTIF_YTD_CurrentYear,
                    Lit_SC_Closure_YTD_PreviousYear = x.Lit_SC_Closure_YTD_PreviousYear,
                    Lit_SC_Closure_Baseline = x.Lit_SC_Closure_Baseline,
                    Lit_SC_Closure_Target = x.Lit_SC_Closure_Target,
                    Lit_SC_Closure_CurrentYear_Q1 = x.Lit_SC_Closure_CurrentYear_Q1,
                    Lit_SC_Closure_CurrentYear_Q2 = x.Lit_SC_Closure_CurrentYear_Q2,
                    Lit_SC_Closure_CurrentYear_Q3 = x.Lit_SC_Closure_CurrentYear_Q3,
                    Lit_SC_Closure_CurrentYear_Q4 = x.Lit_SC_Closure_CurrentYear_Q4,
                    Lit_SC_Closure_YTD_CurrentYear = x.Lit_SC_Closure_YTD_CurrentYear,
                    Lit_CSO_TotalLogged_YTD_PreviousYear = x.Lit_CSO_TotalLogged_YTD_PreviousYear,
                    Lit_CSO_TotalLogged_CurrentYear_Q1 = x.Lit_CSO_TotalLogged_CurrentYear_Q1,
                    Lit_CSO_TotalLogged_CurrentYear_Q2 = x.Lit_CSO_TotalLogged_CurrentYear_Q2,
                    Lit_CSO_TotalLogged_CurrentYear_Q3 = x.Lit_CSO_TotalLogged_CurrentYear_Q3,
                    Lit_CSO_TotalLogged_CurrentYear_Q4 = x.Lit_CSO_TotalLogged_CurrentYear_Q4,
                    Lit_CSO_TotalLogged_YTD_CurrentYear = x.Lit_CSO_TotalLogged_YTD_CurrentYear,
                    Lit_CSO_TotalAClass_YTD_PreviousYear = x.Lit_CSO_TotalAClass_YTD_PreviousYear,
                    Lit_CSO_TotalAClass_CurrentYear_Q1 = x.Lit_CSO_TotalAClass_CurrentYear_Q1,
                    Lit_CSO_TotalAClass_CurrentYear_Q2 = x.Lit_CSO_TotalAClass_CurrentYear_Q2,
                    Lit_CSO_TotalAClass_CurrentYear_Q3 = x.Lit_CSO_TotalAClass_CurrentYear_Q3,
                    Lit_CSO_TotalAClass_CurrentYear_Q4 = x.Lit_CSO_TotalAClass_CurrentYear_Q4,
                    Lit_CSO_TotalAClass_YTD_CurrentYear = x.Lit_CSO_TotalAClass_YTD_CurrentYear,
                    Lit_CSO_AClassClosed_YTD_PreviousYear = x.Lit_CSO_AClassClosed_YTD_PreviousYear,
                    Lit_CSO_AClassClosed_CurrentYear_Q1 = x.Lit_CSO_AClassClosed_CurrentYear_Q1,
                    Lit_CSO_AClassClosed_CurrentYear_Q2 = x.Lit_CSO_AClassClosed_CurrentYear_Q2,
                    Lit_CSO_AClassClosed_CurrentYear_Q3 = x.Lit_CSO_AClassClosed_CurrentYear_Q3,
                    Lit_CSO_AClassClosed_CurrentYear_Q4 = x.Lit_CSO_AClassClosed_CurrentYear_Q4,
                    Lit_CSO_AClassClosed_YTD_CurrentYear = x.Lit_CSO_AClassClosed_YTD_CurrentYear,
                    Lit_CSO_AClassClosedLess45_YTD_PreviousYear = x.Lit_CSO_AClassClosedLess45_YTD_PreviousYear,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q1 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q1,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q2 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q2,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q3 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q3,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q4 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q4,
                    Lit_CSO_AClassClosedLess45_YTD_CurrentYear = x.Lit_CSO_AClassClosedLess45_YTD_CurrentYear,
                    Lit_CSO_PercentageClosure_YTD_PreviousYear = x.Lit_CSO_PercentageClosure_YTD_PreviousYear,
                    Lit_CSO_PercentageClosure_CurrentYear_Q1 = x.Lit_CSO_PercentageClosure_CurrentYear_Q1,
                    Lit_CSO_PercentageClosure_CurrentYear_Q2 = x.Lit_CSO_PercentageClosure_CurrentYear_Q2,
                    Lit_CSO_PercentageClosure_CurrentYear_Q3 = x.Lit_CSO_PercentageClosure_CurrentYear_Q3,
                    Lit_CSO_PercentageClosure_CurrentYear_Q4 = x.Lit_CSO_PercentageClosure_CurrentYear_Q4,
                    Lit_CSO_PercentageClosure_YTD_CurrentYear = x.Lit_CSO_PercentageClosure_YTD_CurrentYear,
                    Lit_CostSavings_YTD_PreviousYear = x.Lit_CostSavings_YTD_PreviousYear,
                    Lit_CostSavings_Target = x.Lit_CostSavings_Target,
                    Lit_CostSavings_CurrentYear_Q1 = x.Lit_CostSavings_CurrentYear_Q1,
                    Lit_CostSavings_CurrentYear_Q2 = x.Lit_CostSavings_CurrentYear_Q2,
                    Lit_CostSavings_CurrentYear_Q3 = x.Lit_CostSavings_CurrentYear_Q3,
                    Lit_CostSavings_CurrentYear_Q4 = x.Lit_CostSavings_CurrentYear_Q4,
                    Lit_CostSavings_YTD_CurrentYear = x.Lit_CostSavings_YTD_CurrentYear,
                    Lit_OQL_Vendor1 = x.Lit_OQL_Vendor1,
                    Lit_OQL_Vendor1_YTD_PreviousYear = x.Lit_OQL_Vendor1_YTD_PreviousYear,
                    Lit_OQL_Vendor1_Target = x.Lit_OQL_Vendor1_Target,
                    Lit_OQL_Vendor1_PC01 = x.Lit_OQL_Vendor1_PC01,
                    Lit_OQL_Vendor1_PC02 = x.Lit_OQL_Vendor1_PC02,
                    Lit_OQL_Vendor1_PC03 = x.Lit_OQL_Vendor1_PC03,
                    Lit_OQL_Vendor1_PC04 = x.Lit_OQL_Vendor1_PC04,
                    Lit_OQL_Vendor1_PC05 = x.Lit_OQL_Vendor1_PC05,
                    Lit_OQL_Vendor1_PC06 = x.Lit_OQL_Vendor1_PC06,
                    Lit_OQL_Vendor2 = x.Lit_OQL_Vendor2,
                    Lit_OQL_Vendor2_YTD_PreviousYear = x.Lit_OQL_Vendor2_YTD_PreviousYear,
                    Lit_OQL_Vendor2_Target = x.Lit_OQL_Vendor2_Target,
                    Lit_OQL_Vendor2_PC01 = x.Lit_OQL_Vendor2_PC01,
                    Lit_OQL_Vendor2_PC02 = x.Lit_OQL_Vendor2_PC02,
                    Lit_OQL_Vendor2_PC03 = x.Lit_OQL_Vendor2_PC03,
                    Lit_OQL_Vendor2_PC04 = x.Lit_OQL_Vendor2_PC04,
                    Lit_OQL_Vendor2_PC05 = x.Lit_OQL_Vendor2_PC05,
                    Lit_OQL_Vendor2_PC06 = x.Lit_OQL_Vendor2_PC06,
                    Lit_OQL_Vendor3 = x.Lit_OQL_Vendor3,
                    Lit_OQL_Vendor3_YTD_PreviousYear = x.Lit_OQL_Vendor3_YTD_PreviousYear,
                    Lit_OQL_Vendor3_Target = x.Lit_OQL_Vendor3_Target,
                    Lit_OQL_Vendor3_PC01 = x.Lit_OQL_Vendor3_PC01,
                    Lit_OQL_Vendor3_PC02 = x.Lit_OQL_Vendor3_PC02,
                    Lit_OQL_Vendor3_PC03 = x.Lit_OQL_Vendor3_PC03,
                    Lit_OQL_Vendor3_PC04 = x.Lit_OQL_Vendor3_PC04,
                    Lit_OQL_Vendor3_PC05 = x.Lit_OQL_Vendor3_PC05,
                    Lit_OQL_Vendor3_PC06 = x.Lit_OQL_Vendor3_PC06,
                    Lit_OQL_Vendor4 = x.Lit_OQL_Vendor4,
                    Lit_OQL_Vendor4_YTD_PreviousYear = x.Lit_OQL_Vendor4_YTD_PreviousYear,
                    Lit_OQL_Vendor4_Target = x.Lit_OQL_Vendor4_Target,
                    Lit_OQL_Vendor4_PC01 = x.Lit_OQL_Vendor4_PC01,
                    Lit_OQL_Vendor4_PC02 = x.Lit_OQL_Vendor4_PC02,
                    Lit_OQL_Vendor4_PC03 = x.Lit_OQL_Vendor4_PC03,
                    Lit_OQL_Vendor4_PC04 = x.Lit_OQL_Vendor4_PC04,
                    Lit_OQL_Vendor4_PC05 = x.Lit_OQL_Vendor4_PC05,
                    Lit_OQL_Vendor4_PC06 = x.Lit_OQL_Vendor4_PC06,
                    Lit_OQL_Vendor5 = x.Lit_OQL_Vendor5,
                    Lit_OQL_Vendor5_YTD_PreviousYear = x.Lit_OQL_Vendor5_YTD_PreviousYear,
                    Lit_OQL_Vendor5_Target = x.Lit_OQL_Vendor5_Target,
                    Lit_OQL_Vendor5_PC01 = x.Lit_OQL_Vendor5_PC01,
                    Lit_OQL_Vendor5_PC02 = x.Lit_OQL_Vendor5_PC02,
                    Lit_OQL_Vendor5_PC03 = x.Lit_OQL_Vendor5_PC03,
                    Lit_OQL_Vendor5_PC04 = x.Lit_OQL_Vendor5_PC04,
                    Lit_OQL_Vendor5_PC05 = x.Lit_OQL_Vendor5_PC05,
                    Lit_OQL_Vendor5_PC06 = x.Lit_OQL_Vendor5_PC06,
                    Lit_OQL_Vendor6 = x.Lit_OQL_Vendor6,
                    Lit_OQL_Vendor6_YTD_PreviousYear = x.Lit_OQL_Vendor6_YTD_PreviousYear,
                    Lit_OQL_Vendor6_Target = x.Lit_OQL_Vendor6_Target,
                    Lit_OQL_Vendor6_PC01 = x.Lit_OQL_Vendor6_PC01,
                    Lit_OQL_Vendor6_PC02 = x.Lit_OQL_Vendor6_PC02,
                    Lit_OQL_Vendor6_PC03 = x.Lit_OQL_Vendor6_PC03,
                    Lit_OQL_Vendor6_PC04 = x.Lit_OQL_Vendor6_PC04,
                    Lit_OQL_Vendor6_PC05 = x.Lit_OQL_Vendor6_PC05,
                    Lit_OQL_Vendor6_PC06 = x.Lit_OQL_Vendor6_PC06,
                    Lit_OQL_Vendor7 = x.Lit_OQL_Vendor7,
                    Lit_OQL_Vendor7_YTD_PreviousYear = x.Lit_OQL_Vendor7_YTD_PreviousYear,
                    Lit_OQL_Vendor7_Target = x.Lit_OQL_Vendor7_Target,
                    Lit_OQL_Vendor7_PC01 = x.Lit_OQL_Vendor7_PC01,
                    Lit_OQL_Vendor7_PC02 = x.Lit_OQL_Vendor7_PC02,
                    Lit_OQL_Vendor7_PC03 = x.Lit_OQL_Vendor7_PC03,
                    Lit_OQL_Vendor7_PC04 = x.Lit_OQL_Vendor7_PC04,
                    Lit_OQL_Vendor7_PC05 = x.Lit_OQL_Vendor7_PC05,
                    Lit_OQL_Vendor7_PC06 = x.Lit_OQL_Vendor7_PC06,
                    Lit_OQL_Vendor8 = x.Lit_OQL_Vendor8,
                    Lit_OQL_Vendor8_YTD_PreviousYear = x.Lit_OQL_Vendor8_YTD_PreviousYear,
                    Lit_OQL_Vendor8_Target = x.Lit_OQL_Vendor8_Target,
                    Lit_OQL_Vendor8_PC01 = x.Lit_OQL_Vendor8_PC01,
                    Lit_OQL_Vendor8_PC02 = x.Lit_OQL_Vendor8_PC02,
                    Lit_OQL_Vendor8_PC03 = x.Lit_OQL_Vendor8_PC03,
                    Lit_OQL_Vendor8_PC04 = x.Lit_OQL_Vendor8_PC04,
                    Lit_OQL_Vendor8_PC05 = x.Lit_OQL_Vendor8_PC05,
                    Lit_OQL_Vendor8_PC06 = x.Lit_OQL_Vendor8_PC06,
                    Lit_OQL_CumulativeAvg1_YTD_PreviousYear = x.Lit_OQL_CumulativeAvg1_YTD_PreviousYear,
                    Lit_OQL_CumulativeAvg1_Target = x.Lit_OQL_CumulativeAvg1_Target,
                    Lit_OQL_CumulativeAvg1_PC01 = x.Lit_OQL_CumulativeAvg1_PC01,
                    Lit_OQL_CumulativeAvg1_PC02 = x.Lit_OQL_CumulativeAvg1_PC02,
                    Lit_OQL_CumulativeAvg1_PC03 = x.Lit_OQL_CumulativeAvg1_PC03,
                    Lit_OQL_CumulativeAvg1_PC04 = x.Lit_OQL_CumulativeAvg1_PC04,
                    Lit_OQL_CumulativeAvg1_PC05 = x.Lit_OQL_CumulativeAvg1_PC05,
                    Lit_OQL_CumulativeAvg1_PC06 = x.Lit_OQL_CumulativeAvg1_PC06,
                    Lit_OQL_CumulativeAvg2_YTD_PreviousYear = x.Lit_OQL_CumulativeAvg2_YTD_PreviousYear,
                    Lit_OQL_CumulativeAvg2_Target = x.Lit_OQL_CumulativeAvg2_Target,
                    Lit_OQL_CumulativeAvg2_PC01 = x.Lit_OQL_CumulativeAvg2_PC01,
                    Lit_OQL_CumulativeAvg2_PC02 = x.Lit_OQL_CumulativeAvg2_PC02,
                    Lit_OQL_CumulativeAvg2_PC03 = x.Lit_OQL_CumulativeAvg2_PC03,
                    Lit_OQL_CumulativeAvg2_PC04 = x.Lit_OQL_CumulativeAvg2_PC04,
                    Lit_OQL_CumulativeAvg2_PC05 = x.Lit_OQL_CumulativeAvg2_PC05,
                    Lit_OQL_CumulativeAvg2_PC06 = x.Lit_OQL_CumulativeAvg2_PC06,
                    Seat_SS_CNClosure_Baseline = x.Seat_SS_CNClosure_Baseline,
                    Seat_SS_CNClosure_Target = x.Seat_SS_CNClosure_Target,
                    Seat_SS_CNClosure_CurrentYear_Q1 = x.Seat_SS_CNClosure_CurrentYear_Q1,
                    Seat_SS_CNClosure_CurrentYear_Q2 = x.Seat_SS_CNClosure_CurrentYear_Q2,
                    Seat_SS_CNClosure_CurrentYear_Q3 = x.Seat_SS_CNClosure_CurrentYear_Q3,
                    Seat_SS_CNClosure_CurrentYear_Q4 = x.Seat_SS_CNClosure_CurrentYear_Q4,
                    Seat_SS_CNClosure_YTD_CurrentYear = x.Seat_SS_CNClosure_YTD_CurrentYear,
                    Seat_SS_OTIF_Baseline = x.Seat_SS_OTIF_Baseline,
                    Seat_SS_OTIF_Target = x.Seat_SS_OTIF_Target,
                    Seat_SS_OTIF_CurrentYear_Q1 = x.Seat_SS_OTIF_CurrentYear_Q1,
                    Seat_SS_OTIF_CurrentYear_Q2 = x.Seat_SS_OTIF_CurrentYear_Q2,
                    Seat_SS_OTIF_CurrentYear_Q3 = x.Seat_SS_OTIF_CurrentYear_Q3,
                    Seat_SS_OTIF_CurrentYear_Q4 = x.Seat_SS_OTIF_CurrentYear_Q4,
                    Seat_SS_OTIF_YTD_CurrentYear = x.Seat_SS_OTIF_YTD_CurrentYear,
                    Seat_SS_SPMScore_Baseline = x.Seat_SS_SPMScore_Baseline,
                    Seat_SS_SPMScore_Target = x.Seat_SS_SPMScore_Target,
                    Seat_SS_SPMScore_CurrentYear_Q1 = x.Seat_SS_SPMScore_CurrentYear_Q1,
                    Seat_SS_SPMScore_CurrentYear_Q2 = x.Seat_SS_SPMScore_CurrentYear_Q2,
                    Seat_SS_SPMScore_CurrentYear_Q3 = x.Seat_SS_SPMScore_CurrentYear_Q3,
                    Seat_SS_SPMScore_CurrentYear_Q4 = x.Seat_SS_SPMScore_CurrentYear_Q4,
                    Seat_SS_SPMScore_YTD_CurrentYear = x.Seat_SS_SPMScore_YTD_CurrentYear,
                    Seat_OTIF_Performance_YTD_PreviousYear = x.Seat_OTIF_Performance_YTD_PreviousYear,
                    Seat_OTIF_Performance_Target = x.Seat_OTIF_Performance_Target,
                    Seat_OTIF_Performance_CurrentYear_Q1 = x.Seat_OTIF_Performance_CurrentYear_Q1,
                    Seat_OTIF_Performance_CurrentYear_Q2 = x.Seat_OTIF_Performance_CurrentYear_Q2,
                    Seat_OTIF_Performance_CurrentYear_Q3 = x.Seat_OTIF_Performance_CurrentYear_Q3,
                    Seat_OTIF_Performance_CurrentYear_Q4 = x.Seat_OTIF_Performance_CurrentYear_Q4,
                    Seat_OTIF_Performance_YTD_CurrentYear = x.Seat_OTIF_Performance_YTD_CurrentYear,
                    Seat_CSAT_ReqSent_YTD_PreviousYear = x.Seat_CSAT_ReqSent_YTD_PreviousYear,
                    Seat_CSAT_ReqSent_Baseline = x.Seat_CSAT_ReqSent_Baseline,
                    Seat_CSAT_ReqSent_Target = x.Seat_CSAT_ReqSent_Target,
                    Seat_CSAT_ReqSent_CurrentYear_Q1 = x.Seat_CSAT_ReqSent_CurrentYear_Q1,
                    Seat_CSAT_ReqSent_CurrentYear_Q2 = x.Seat_CSAT_ReqSent_CurrentYear_Q2,
                    Seat_CSAT_ReqSent_CurrentYear_Q3 = x.Seat_CSAT_ReqSent_CurrentYear_Q3,
                    Seat_CSAT_ReqSent_CurrentYear_Q4 = x.Seat_CSAT_ReqSent_CurrentYear_Q4,
                    Seat_CSAT_ReqSent_YTD_CurrentYear = x.Seat_CSAT_ReqSent_YTD_CurrentYear,
                    Seat_CSAT_RespRecvd_YTD_PreviousYear = x.Seat_CSAT_RespRecvd_YTD_PreviousYear,
                    Seat_CSAT_RespRecvd_Baseline = x.Seat_CSAT_RespRecvd_Baseline,
                    Seat_CSAT_RespRecvd_Target = x.Seat_CSAT_RespRecvd_Target,
                    Seat_CSAT_RespRecvd_CurrentYear_Q1 = x.Seat_CSAT_RespRecvd_CurrentYear_Q1,
                    Seat_CSAT_RespRecvd_CurrentYear_Q2 = x.Seat_CSAT_RespRecvd_CurrentYear_Q2,
                    Seat_CSAT_RespRecvd_CurrentYear_Q3 = x.Seat_CSAT_RespRecvd_CurrentYear_Q3,
                    Seat_CSAT_RespRecvd_CurrentYear_Q4 = x.Seat_CSAT_RespRecvd_CurrentYear_Q4,
                    Seat_CSAT_RespRecvd_YTD_CurrentYear = x.Seat_CSAT_RespRecvd_YTD_CurrentYear,
                    Seat_CSAT_Promoter_YTD_PreviousYear = x.Seat_CSAT_Promoter_YTD_PreviousYear,
                    Seat_CSAT_Promoter_Baseline = x.Seat_CSAT_Promoter_Baseline,
                    Seat_CSAT_Promoter_Target = x.Seat_CSAT_Promoter_Target,
                    Seat_CSAT_Promoter_CurrentYear_Q1 = x.Seat_CSAT_Promoter_CurrentYear_Q1,
                    Seat_CSAT_Promoter_CurrentYear_Q2 = x.Seat_CSAT_Promoter_CurrentYear_Q2,
                    Seat_CSAT_Promoter_CurrentYear_Q3 = x.Seat_CSAT_Promoter_CurrentYear_Q3,
                    Seat_CSAT_Promoter_CurrentYear_Q4 = x.Seat_CSAT_Promoter_CurrentYear_Q4,
                    Seat_CSAT_Promoter_YTD_CurrentYear = x.Seat_CSAT_Promoter_YTD_CurrentYear,
                    Seat_CSAT_Detractor_YTD_PreviousYear = x.Seat_CSAT_Detractor_YTD_PreviousYear,
                    Seat_CSAT_Detractor_Baseline = x.Seat_CSAT_Detractor_Baseline,
                    Seat_CSAT_Detractor_Target = x.Seat_CSAT_Detractor_Target,
                    Seat_CSAT_Detractor_CurrentYear_Q1 = x.Seat_CSAT_Detractor_CurrentYear_Q1,
                    Seat_CSAT_Detractor_CurrentYear_Q2 = x.Seat_CSAT_Detractor_CurrentYear_Q2,
                    Seat_CSAT_Detractor_CurrentYear_Q3 = x.Seat_CSAT_Detractor_CurrentYear_Q3,
                    Seat_CSAT_Detractor_CurrentYear_Q4 = x.Seat_CSAT_Detractor_CurrentYear_Q4,
                    Seat_CSAT_Detractor_YTD_CurrentYear = x.Seat_CSAT_Detractor_YTD_CurrentYear,
                    Seat_CSAT_NPS_YTD_PreviousYear = x.Seat_CSAT_NPS_YTD_PreviousYear,
                    Seat_CSAT_NPS_Baseline = x.Seat_CSAT_NPS_Baseline,
                    Seat_CSAT_NPS_Target = x.Seat_CSAT_NPS_Target,
                    Seat_CSAT_NPS_CurrentYear_Q1 = x.Seat_CSAT_NPS_CurrentYear_Q1,
                    Seat_CSAT_NPS_CurrentYear_Q2 = x.Seat_CSAT_NPS_CurrentYear_Q2,
                    Seat_CSAT_NPS_CurrentYear_Q3 = x.Seat_CSAT_NPS_CurrentYear_Q3,
                    Seat_CSAT_NPS_CurrentYear_Q4 = x.Seat_CSAT_NPS_CurrentYear_Q4,
                    Seat_CSAT_NPS_YTD_CurrentYear = x.Seat_CSAT_NPS_YTD_CurrentYear,
                    Seat_CSO_TotalLogged_YTD_PreviousYear = x.Seat_CSO_TotalLogged_YTD_PreviousYear,
                    Seat_CSO_TotalLogged_CurrentYear_Q1 = x.Seat_CSO_TotalLogged_CurrentYear_Q1,
                    Seat_CSO_TotalLogged_CurrentYear_Q2 = x.Seat_CSO_TotalLogged_CurrentYear_Q2,
                    Seat_CSO_TotalLogged_CurrentYear_Q3 = x.Seat_CSO_TotalLogged_CurrentYear_Q3,
                    Seat_CSO_TotalLogged_CurrentYear_Q4 = x.Seat_CSO_TotalLogged_CurrentYear_Q4,
                    Seat_CSO_TotalLogged_YTD_CurrentYear = x.Seat_CSO_TotalLogged_YTD_CurrentYear,
                    Seat_CSO_TotalAClass_YTD_PreviousYear = x.Seat_CSO_TotalAClass_YTD_PreviousYear,
                    Seat_CSO_TotalAClass_CurrentYear_Q1 = x.Seat_CSO_TotalAClass_CurrentYear_Q1,
                    Seat_CSO_TotalAClass_CurrentYear_Q2 = x.Seat_CSO_TotalAClass_CurrentYear_Q2,
                    Seat_CSO_TotalAClass_CurrentYear_Q3 = x.Seat_CSO_TotalAClass_CurrentYear_Q3,
                    Seat_CSO_TotalAClass_CurrentYear_Q4 = x.Seat_CSO_TotalAClass_CurrentYear_Q4,
                    Seat_CSO_TotalAClass_YTD_CurrentYear = x.Seat_CSO_TotalAClass_YTD_CurrentYear,
                    Seat_CSO_AClassClosed_YTD_PreviousYear = x.Seat_CSO_AClassClosed_YTD_PreviousYear,
                    Seat_CSO_AClassClosed_CurrentYear_Q1 = x.Seat_CSO_AClassClosed_CurrentYear_Q1,
                    Seat_CSO_AClassClosed_CurrentYear_Q2 = x.Seat_CSO_AClassClosed_CurrentYear_Q2,
                    Seat_CSO_AClassClosed_CurrentYear_Q3 = x.Seat_CSO_AClassClosed_CurrentYear_Q3,
                    Seat_CSO_AClassClosed_CurrentYear_Q4 = x.Seat_CSO_AClassClosed_CurrentYear_Q4,
                    Seat_CSO_AClassClosed_YTD_CurrentYear = x.Seat_CSO_AClassClosed_YTD_CurrentYear,
                    Seat_CSO_AClassClosedLess45_YTD_PreviousYear = x.Seat_CSO_AClassClosedLess45_YTD_PreviousYear,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q1 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q1,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q2 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q2,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q3 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q3,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q4 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q4,
                    Seat_CSO_AClassClosedLess45_YTD_CurrentYear = x.Seat_CSO_AClassClosedLess45_YTD_CurrentYear,
                    Seat_CSO_AClassClosedUnder45_YTD_PreviousYear = x.Seat_CSO_AClassClosedUnder45_YTD_PreviousYear,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q1 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q1,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q2 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q2,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q3 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q3,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q4 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q4,
                    Seat_CSO_AClassClosedUnder45_YTD_CurrentYear = x.Seat_CSO_AClassClosedUnder45_YTD_CurrentYear,
                    Seat_SPM_Supp1 = x.Seat_SPM_Supp1,
                    Seat_SPM_Supp1_PreviousYear_Q4 = x.Seat_SPM_Supp1_PreviousYear_Q4,
                    Seat_SPM_Supp1_CurrentYear_Q1 = x.Seat_SPM_Supp1_CurrentYear_Q1,
                    Seat_SPM_Supp1_CurrentYear_Q2 = x.Seat_SPM_Supp1_CurrentYear_Q2,
                    Seat_SPM_Supp1_CurrentYear_Q3 = x.Seat_SPM_Supp1_CurrentYear_Q3,
                    Seat_SPM_Supp1_CurrentYear_Q4 = x.Seat_SPM_Supp1_CurrentYear_Q4,
                    Seat_SPM_Supp2 = x.Seat_SPM_Supp2,
                    Seat_SPM_Supp2_PreviousYear_Q4 = x.Seat_SPM_Supp2_PreviousYear_Q4,
                    Seat_SPM_Supp2_CurrentYear_Q1 = x.Seat_SPM_Supp2_CurrentYear_Q1,
                    Seat_SPM_Supp2_CurrentYear_Q2 = x.Seat_SPM_Supp2_CurrentYear_Q2,
                    Seat_SPM_Supp2_CurrentYear_Q3 = x.Seat_SPM_Supp2_CurrentYear_Q3,
                    Seat_SPM_Supp2_CurrentYear_Q4 = x.Seat_SPM_Supp2_CurrentYear_Q4,
                    Seat_SPM_Supp3 = x.Seat_SPM_Supp3,
                    Seat_SPM_Supp3_PreviousYear_Q4 = x.Seat_SPM_Supp3_PreviousYear_Q4,
                    Seat_SPM_Supp3_CurrentYear_Q1 = x.Seat_SPM_Supp3_CurrentYear_Q1,
                    Seat_SPM_Supp3_CurrentYear_Q2 = x.Seat_SPM_Supp3_CurrentYear_Q2,
                    Seat_SPM_Supp3_CurrentYear_Q3 = x.Seat_SPM_Supp3_CurrentYear_Q3,
                    Seat_SPM_Supp3_CurrentYear_Q4 = x.Seat_SPM_Supp3_CurrentYear_Q4,
                    Seat_SPM_Supp4 = x.Seat_SPM_Supp4,
                    Seat_SPM_Supp4_PreviousYear_Q4 = x.Seat_SPM_Supp4_PreviousYear_Q4,
                    Seat_SPM_Supp4_CurrentYear_Q1 = x.Seat_SPM_Supp4_CurrentYear_Q1,
                    Seat_SPM_Supp4_CurrentYear_Q2 = x.Seat_SPM_Supp4_CurrentYear_Q2,
                    Seat_SPM_Supp4_CurrentYear_Q3 = x.Seat_SPM_Supp4_CurrentYear_Q3,
                    Seat_SPM_Supp4_CurrentYear_Q4 = x.Seat_SPM_Supp4_CurrentYear_Q4,
                    Seat_SPM_Supp5 = x.Seat_SPM_Supp5,
                    Seat_SPM_Supp5_PreviousYear_Q4 = x.Seat_SPM_Supp5_PreviousYear_Q4,
                    Seat_SPM_Supp5_CurrentYear_Q1 = x.Seat_SPM_Supp5_CurrentYear_Q1,
                    Seat_SPM_Supp5_CurrentYear_Q2 = x.Seat_SPM_Supp5_CurrentYear_Q2,
                    Seat_SPM_Supp5_CurrentYear_Q3 = x.Seat_SPM_Supp5_CurrentYear_Q3,
                    Seat_SPM_Supp5_CurrentYear_Q4 = x.Seat_SPM_Supp5_CurrentYear_Q4,
                    Seat_IQA_TotalSites_YTD_PreviousYear = x.Seat_IQA_TotalSites_YTD_PreviousYear,
                    Seat_IQA_TotalSites_Target = x.Seat_IQA_TotalSites_Target,
                    Seat_IQA_TotalSites_CurrentYear_Q1 = x.Seat_IQA_TotalSites_CurrentYear_Q1,
                    Seat_IQA_TotalSites_CurrentYear_Q2 = x.Seat_IQA_TotalSites_CurrentYear_Q2,
                    Seat_IQA_TotalSites_CurrentYear_Q3 = x.Seat_IQA_TotalSites_CurrentYear_Q3,
                    Seat_IQA_TotalSites_CurrentYear_Q4 = x.Seat_IQA_TotalSites_CurrentYear_Q4,
                    Seat_IQA_TotalSites_YTD_CurrentYear = x.Seat_IQA_TotalSites_YTD_CurrentYear,
                    Seat_IQA_SitesCompleted_YTD_PreviousYear = x.Seat_IQA_SitesCompleted_YTD_PreviousYear,
                    Seat_IQA_SitesCompleted_Target = x.Seat_IQA_SitesCompleted_Target,
                    Seat_IQA_SitesCompleted_CurrentYear_Q1 = x.Seat_IQA_SitesCompleted_CurrentYear_Q1,
                    Seat_IQA_SitesCompleted_CurrentYear_Q2 = x.Seat_IQA_SitesCompleted_CurrentYear_Q2,
                    Seat_IQA_SitesCompleted_CurrentYear_Q3 = x.Seat_IQA_SitesCompleted_CurrentYear_Q3,
                    Seat_IQA_SitesCompleted_CurrentYear_Q4 = x.Seat_IQA_SitesCompleted_CurrentYear_Q4,
                    Seat_IQA_SitesCompleted_YTD_CurrentYear = x.Seat_IQA_SitesCompleted_YTD_CurrentYear,
                    Seat_IQA_AuditsCompleted_YTD_PreviousYear = x.Seat_IQA_AuditsCompleted_YTD_PreviousYear,
                    Seat_IQA_AuditsCompleted_Target = x.Seat_IQA_AuditsCompleted_Target,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q1 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q1,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q2 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q2,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q3 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q3,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q4 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q4,
                    Seat_IQA_AuditsCompleted_YTD_CurrentYear = x.Seat_IQA_AuditsCompleted_YTD_CurrentYear,
                    Seat_IQA_PercCompleted_YTD_PreviousYear = x.Seat_IQA_PercCompleted_YTD_PreviousYear,
                    Seat_IQA_PercCompleted_Target = x.Seat_IQA_PercCompleted_Target,
                    Seat_IQA_PercCompleted_CurrentYear_Q1 = x.Seat_IQA_PercCompleted_CurrentYear_Q1,
                    Seat_IQA_PercCompleted_CurrentYear_Q2 = x.Seat_IQA_PercCompleted_CurrentYear_Q2,
                    Seat_IQA_PercCompleted_CurrentYear_Q3 = x.Seat_IQA_PercCompleted_CurrentYear_Q3,
                    Seat_IQA_PercCompleted_CurrentYear_Q4 = x.Seat_IQA_PercCompleted_CurrentYear_Q4,
                    Seat_IQA_PercCompleted_YTD_CurrentYear = x.Seat_IQA_PercCompleted_YTD_CurrentYear,
                    Seat_IQA_AvgSigma_YTD_PreviousYear = x.Seat_IQA_AvgSigma_YTD_PreviousYear,
                    Seat_IQA_AvgSigma_Target = x.Seat_IQA_AvgSigma_Target,
                    Seat_IQA_AvgSigma_CurrentYear_Q1 = x.Seat_IQA_AvgSigma_CurrentYear_Q1,
                    Seat_IQA_AvgSigma_CurrentYear_Q2 = x.Seat_IQA_AvgSigma_CurrentYear_Q2,
                    Seat_IQA_AvgSigma_CurrentYear_Q3 = x.Seat_IQA_AvgSigma_CurrentYear_Q3,
                    Seat_IQA_AvgSigma_CurrentYear_Q4 = x.Seat_IQA_AvgSigma_CurrentYear_Q4,
                    Seat_IQA_AvgSigma_YTD_CurrentYear = x.Seat_IQA_AvgSigma_YTD_CurrentYear

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
                new SqlParameter("@FinancialYear", SqlDbType.Int)
                {
                    Value = 0
                }
            };

            var sql = @"EXEC sp_Get_AHPNote @Id, @FinancialYear";

            var result = await Task.Run(() => _dbContext.AHPNote.FromSqlRaw(sql, parameters)
                .AsEnumerable()
                .Select(x => new AHPNoteViewModel
                {
                    Id = x.Id,
                    FinancialYear = x.FinancialYear,
                    Lit_SEE_Engagement_Target = x.Lit_SEE_Engagement_Target,
                    Lit_SEE_Engagement_PreviousYear_Q1 = x.Lit_SEE_Engagement_PreviousYear_Q1,
                    Lit_SEE_Engagement_PreviousYear_Q2 = x.Lit_SEE_Engagement_PreviousYear_Q2,
                    Lit_SEE_Engagement_PreviousYear_Q3 = x.Lit_SEE_Engagement_PreviousYear_Q3,
                    Lit_SEE_Engagement_PreviousYear_Q4 = x.Lit_SEE_Engagement_PreviousYear_Q4,
                    Lit_SEE_Engagement_CurrentYear_Q1 = x.Lit_SEE_Engagement_CurrentYear_Q1,
                    Lit_SEE_Engagement_CurrentYear_Q2 = x.Lit_SEE_Engagement_CurrentYear_Q2,
                    Lit_SEE_Engagement_CurrentYear_Q3 = x.Lit_SEE_Engagement_CurrentYear_Q3,
                    Lit_SEE_Engagement_CurrentYear_Q4 = x.Lit_SEE_Engagement_CurrentYear_Q4,
                    Lit_SEE_Effectiveness_Target = x.Lit_SEE_Effectiveness_Target,
                    Lit_SEE_Effectiveness_PreviousYear_Q1 = x.Lit_SEE_Effectiveness_PreviousYear_Q1,
                    Lit_SEE_Effectiveness_PreviousYear_Q2 = x.Lit_SEE_Effectiveness_PreviousYear_Q2,
                    Lit_SEE_Effectiveness_PreviousYear_Q3 = x.Lit_SEE_Effectiveness_PreviousYear_Q3,
                    Lit_SEE_Effectiveness_PreviousYear_Q4 = x.Lit_SEE_Effectiveness_PreviousYear_Q4,
                    Lit_SEE_Effectiveness_CurrentYear_Q1 = x.Lit_SEE_Effectiveness_CurrentYear_Q1,
                    Lit_SEE_Effectiveness_CurrentYear_Q2 = x.Lit_SEE_Effectiveness_CurrentYear_Q2,
                    Lit_SEE_Effectiveness_CurrentYear_Q3 = x.Lit_SEE_Effectiveness_CurrentYear_Q3,
                    Lit_SEE_Effectiveness_CurrentYear_Q4 = x.Lit_SEE_Effectiveness_CurrentYear_Q4,
                    Lit_SS_ServiceComplaints_Baseline = x.Lit_SS_ServiceComplaints_Baseline,
                    Lit_SS_ServiceComplaints_Target = x.Lit_SS_ServiceComplaints_Target,
                    Lit_SS_ServiceComplaints_CurrentYear_Q1 = x.Lit_SS_ServiceComplaints_CurrentYear_Q1,
                    Lit_SS_ServiceComplaints_CurrentYear_Q2 = x.Lit_SS_ServiceComplaints_CurrentYear_Q2,
                    Lit_SS_ServiceComplaints_CurrentYear_Q3 = x.Lit_SS_ServiceComplaints_CurrentYear_Q3,
                    Lit_SS_ServiceComplaints_CurrentYear_Q4 = x.Lit_SS_ServiceComplaints_CurrentYear_Q4,
                    Lit_SS_DesignLSG_Baseline = x.Lit_SS_DesignLSG_Baseline,
                    Lit_SS_DesignLSG_Target = x.Lit_SS_DesignLSG_Target,
                    Lit_SS_DesignLSG_CurrentYear_Q1 = x.Lit_SS_DesignLSG_CurrentYear_Q1,
                    Lit_SS_DesignLSG_CurrentYear_Q2 = x.Lit_SS_DesignLSG_CurrentYear_Q2,
                    Lit_SS_DesignLSG_CurrentYear_Q3 = x.Lit_SS_DesignLSG_CurrentYear_Q3,
                    Lit_SS_DesignLSG_CurrentYear_Q4 = x.Lit_SS_DesignLSG_CurrentYear_Q4,
                    Lit_SS_CostReduction_Baseline = x.Lit_SS_CostReduction_Baseline,
                    Lit_SS_CostReduction_Target = x.Lit_SS_CostReduction_Target,
                    Lit_SS_CostReduction_CurrentYear_Q1 = x.Lit_SS_CostReduction_CurrentYear_Q1,
                    Lit_SS_CostReduction_CurrentYear_Q2 = x.Lit_SS_CostReduction_CurrentYear_Q2,
                    Lit_SS_CostReduction_CurrentYear_Q3 = x.Lit_SS_CostReduction_CurrentYear_Q3,
                    Lit_SS_CostReduction_CurrentYear_Q4 = x.Lit_SS_CostReduction_CurrentYear_Q4,
                    Lit_SS_OTIF_Baseline = x.Lit_SS_OTIF_Baseline,
                    Lit_SS_OTIF_Target = x.Lit_SS_OTIF_Target,
                    Lit_SS_OTIF_CurrentYear_Q1 = x.Lit_SS_OTIF_CurrentYear_Q1,
                    Lit_SS_OTIF_CurrentYear_Q2 = x.Lit_SS_OTIF_CurrentYear_Q2,
                    Lit_SS_OTIF_CurrentYear_Q3 = x.Lit_SS_OTIF_CurrentYear_Q3,
                    Lit_SS_OTIF_CurrentYear_Q4 = x.Lit_SS_OTIF_CurrentYear_Q4,
                    Lit_SS_SPMScore_Baseline = x.Lit_SS_SPMScore_Baseline,
                    Lit_SS_SPMScore_Target = x.Lit_SS_SPMScore_Target,
                    Lit_SS_SPMScore_CurrentYear_Q1 = x.Lit_SS_SPMScore_CurrentYear_Q1,
                    Lit_SS_SPMScore_CurrentYear_Q2 = x.Lit_SS_SPMScore_CurrentYear_Q2,
                    Lit_SS_SPMScore_CurrentYear_Q3 = x.Lit_SS_SPMScore_CurrentYear_Q3,
                    Lit_SS_SPMScore_CurrentYear_Q4 = x.Lit_SS_SPMScore_CurrentYear_Q4,
                    Lit_CSAT_ReqSent_YTD_PreviousYear = x.Lit_CSAT_ReqSent_YTD_PreviousYear,
                    Lit_CSAT_ReqSent_Baseline = x.Lit_CSAT_ReqSent_Baseline,
                    Lit_CSAT_ReqSent_Target = x.Lit_CSAT_ReqSent_Target,
                    Lit_CSAT_ReqSent_CurrentYear_Q1 = x.Lit_CSAT_ReqSent_CurrentYear_Q1,
                    Lit_CSAT_ReqSent_CurrentYear_Q2 = x.Lit_CSAT_ReqSent_CurrentYear_Q2,
                    Lit_CSAT_ReqSent_CurrentYear_Q3 = x.Lit_CSAT_ReqSent_CurrentYear_Q3,
                    Lit_CSAT_ReqSent_CurrentYear_Q4 = x.Lit_CSAT_ReqSent_CurrentYear_Q4,
                    Lit_CSAT_ReqSent_YTD_CurrentYear = x.Lit_CSAT_ReqSent_YTD_CurrentYear,
                    Lit_CSAT_RespRecvd_YTD_PreviousYear = x.Lit_CSAT_RespRecvd_YTD_PreviousYear,
                    Lit_CSAT_RespRecvd_Baseline = x.Lit_CSAT_RespRecvd_Baseline,
                    Lit_CSAT_RespRecvd_Target = x.Lit_CSAT_RespRecvd_Target,
                    Lit_CSAT_RespRecvd_CurrentYear_Q1 = x.Lit_CSAT_RespRecvd_CurrentYear_Q1,
                    Lit_CSAT_RespRecvd_CurrentYear_Q2 = x.Lit_CSAT_RespRecvd_CurrentYear_Q2,
                    Lit_CSAT_RespRecvd_CurrentYear_Q3 = x.Lit_CSAT_RespRecvd_CurrentYear_Q3,
                    Lit_CSAT_RespRecvd_CurrentYear_Q4 = x.Lit_CSAT_RespRecvd_CurrentYear_Q4,
                    Lit_CSAT_RespRecvd_YTD_CurrentYear = x.Lit_CSAT_RespRecvd_YTD_CurrentYear,
                    Lit_CSAT_Promoter_YTD_PreviousYear = x.Lit_CSAT_Promoter_YTD_PreviousYear,
                    Lit_CSAT_Promoter_Baseline = x.Lit_CSAT_Promoter_Baseline,
                    Lit_CSAT_Promoter_Target = x.Lit_CSAT_Promoter_Target,
                    Lit_CSAT_Promoter_CurrentYear_Q1 = x.Lit_CSAT_Promoter_CurrentYear_Q1,
                    Lit_CSAT_Promoter_CurrentYear_Q2 = x.Lit_CSAT_Promoter_CurrentYear_Q2,
                    Lit_CSAT_Promoter_CurrentYear_Q3 = x.Lit_CSAT_Promoter_CurrentYear_Q3,
                    Lit_CSAT_Promoter_CurrentYear_Q4 = x.Lit_CSAT_Promoter_CurrentYear_Q4,
                    Lit_CSAT_Promoter_YTD_CurrentYear = x.Lit_CSAT_Promoter_YTD_CurrentYear,
                    Lit_CSAT_Detractor_YTD_PreviousYear = x.Lit_CSAT_Detractor_YTD_PreviousYear,
                    Lit_CSAT_Detractor_Baseline = x.Lit_CSAT_Detractor_Baseline,
                    Lit_CSAT_Detractor_Target = x.Lit_CSAT_Detractor_Target,
                    Lit_CSAT_Detractor_CurrentYear_Q1 = x.Lit_CSAT_Detractor_CurrentYear_Q1,
                    Lit_CSAT_Detractor_CurrentYear_Q2 = x.Lit_CSAT_Detractor_CurrentYear_Q2,
                    Lit_CSAT_Detractor_CurrentYear_Q3 = x.Lit_CSAT_Detractor_CurrentYear_Q3,
                    Lit_CSAT_Detractor_CurrentYear_Q4 = x.Lit_CSAT_Detractor_CurrentYear_Q4,
                    Lit_CSAT_Detractor_YTD_CurrentYear = x.Lit_CSAT_Detractor_YTD_CurrentYear,
                    Lit_CSAT_NPS_YTD_PreviousYear = x.Lit_CSAT_NPS_YTD_PreviousYear,
                    Lit_CSAT_NPS_Baseline = x.Lit_CSAT_NPS_Baseline,
                    Lit_CSAT_NPS_Target = x.Lit_CSAT_NPS_Target,
                    Lit_CSAT_NPS_CurrentYear_Q1 = x.Lit_CSAT_NPS_CurrentYear_Q1,
                    Lit_CSAT_NPS_CurrentYear_Q2 = x.Lit_CSAT_NPS_CurrentYear_Q2,
                    Lit_CSAT_NPS_CurrentYear_Q3 = x.Lit_CSAT_NPS_CurrentYear_Q3,
                    Lit_CSAT_NPS_CurrentYear_Q4 = x.Lit_CSAT_NPS_CurrentYear_Q4,
                    Lit_CSAT_NPS_YTD_CurrentYear = x.Lit_CSAT_NPS_YTD_CurrentYear,
                    Lit_SPM_Supp1 = x.Lit_SPM_Supp1,
                    Lit_SPM_Supp1_CurrentYear_Q1 = x.Lit_SPM_Supp1_CurrentYear_Q1,
                    Lit_SPM_Supp1_CurrentYear_Q2 = x.Lit_SPM_Supp1_CurrentYear_Q2,
                    Lit_SPM_Supp1_CurrentYear_Q3 = x.Lit_SPM_Supp1_CurrentYear_Q3,
                    Lit_SPM_Supp1_CurrentYear_Q4 = x.Lit_SPM_Supp1_CurrentYear_Q4,
                    Lit_SPM_Supp2 = x.Lit_SPM_Supp2,
                    Lit_SPM_Supp2_CurrentYear_Q1 = x.Lit_SPM_Supp2_CurrentYear_Q1,
                    Lit_SPM_Supp2_CurrentYear_Q2 = x.Lit_SPM_Supp2_CurrentYear_Q2,
                    Lit_SPM_Supp2_CurrentYear_Q3 = x.Lit_SPM_Supp2_CurrentYear_Q3,
                    Lit_SPM_Supp2_CurrentYear_Q4 = x.Lit_SPM_Supp2_CurrentYear_Q4,
                    Lit_SPM_Supp3 = x.Lit_SPM_Supp3,
                    Lit_SPM_Supp3_CurrentYear_Q1 = x.Lit_SPM_Supp3_CurrentYear_Q1,
                    Lit_SPM_Supp3_CurrentYear_Q2 = x.Lit_SPM_Supp3_CurrentYear_Q2,
                    Lit_SPM_Supp3_CurrentYear_Q3 = x.Lit_SPM_Supp3_CurrentYear_Q3,
                    Lit_SPM_Supp3_CurrentYear_Q4 = x.Lit_SPM_Supp3_CurrentYear_Q4,
                    Lit_SPM_Supp4 = x.Lit_SPM_Supp4,
                    Lit_SPM_Supp4_CurrentYear_Q1 = x.Lit_SPM_Supp4_CurrentYear_Q1,
                    Lit_SPM_Supp4_CurrentYear_Q2 = x.Lit_SPM_Supp4_CurrentYear_Q2,
                    Lit_SPM_Supp4_CurrentYear_Q3 = x.Lit_SPM_Supp4_CurrentYear_Q3,
                    Lit_SPM_Supp4_CurrentYear_Q4 = x.Lit_SPM_Supp4_CurrentYear_Q4,
                    Lit_SPM_Supp5 = x.Lit_SPM_Supp5,
                    Lit_SPM_Supp5_CurrentYear_Q1 = x.Lit_SPM_Supp5_CurrentYear_Q1,
                    Lit_SPM_Supp5_CurrentYear_Q2 = x.Lit_SPM_Supp5_CurrentYear_Q2,
                    Lit_SPM_Supp5_CurrentYear_Q3 = x.Lit_SPM_Supp5_CurrentYear_Q3,
                    Lit_SPM_Supp5_CurrentYear_Q4 = x.Lit_SPM_Supp5_CurrentYear_Q4,
                    Lit_SPM_Supp6 = x.Lit_SPM_Supp6,
                    Lit_SPM_Supp6_CurrentYear_Q1 = x.Lit_SPM_Supp6_CurrentYear_Q1,
                    Lit_SPM_Supp6_CurrentYear_Q2 = x.Lit_SPM_Supp6_CurrentYear_Q2,
                    Lit_SPM_Supp6_CurrentYear_Q3 = x.Lit_SPM_Supp6_CurrentYear_Q3,
                    Lit_SPM_Supp6_CurrentYear_Q4 = x.Lit_SPM_Supp6_CurrentYear_Q4,
                    Lit_SPM_Supp7 = x.Lit_SPM_Supp7,
                    Lit_SPM_Supp7_CurrentYear_Q1 = x.Lit_SPM_Supp7_CurrentYear_Q1,
                    Lit_SPM_Supp7_CurrentYear_Q2 = x.Lit_SPM_Supp7_CurrentYear_Q2,
                    Lit_SPM_Supp7_CurrentYear_Q3 = x.Lit_SPM_Supp7_CurrentYear_Q3,
                    Lit_SPM_Supp7_CurrentYear_Q4 = x.Lit_SPM_Supp7_CurrentYear_Q4,
                    Lit_SPM_Supp8 = x.Lit_SPM_Supp8,
                    Lit_SPM_Supp8_CurrentYear_Q1 = x.Lit_SPM_Supp8_CurrentYear_Q1,
                    Lit_SPM_Supp8_CurrentYear_Q2 = x.Lit_SPM_Supp8_CurrentYear_Q2,
                    Lit_SPM_Supp8_CurrentYear_Q3 = x.Lit_SPM_Supp8_CurrentYear_Q3,
                    Lit_SPM_Supp8_CurrentYear_Q4 = x.Lit_SPM_Supp8_CurrentYear_Q4,
                    Lit_SPM_Supp9 = x.Lit_SPM_Supp9,
                    Lit_SPM_Supp9_CurrentYear_Q1 = x.Lit_SPM_Supp9_CurrentYear_Q1,
                    Lit_SPM_Supp9_CurrentYear_Q2 = x.Lit_SPM_Supp9_CurrentYear_Q2,
                    Lit_SPM_Supp9_CurrentYear_Q3 = x.Lit_SPM_Supp9_CurrentYear_Q3,
                    Lit_SPM_Supp9_CurrentYear_Q4 = x.Lit_SPM_Supp9_CurrentYear_Q4,
                    Lit_SPM_Supp10 = x.Lit_SPM_Supp10,
                    Lit_SPM_Supp10_CurrentYear_Q1 = x.Lit_SPM_Supp10_CurrentYear_Q1,
                    Lit_SPM_Supp10_CurrentYear_Q2 = x.Lit_SPM_Supp10_CurrentYear_Q2,
                    Lit_SPM_Supp10_CurrentYear_Q3 = x.Lit_SPM_Supp10_CurrentYear_Q3,
                    Lit_SPM_Supp10_CurrentYear_Q4 = x.Lit_SPM_Supp10_CurrentYear_Q4,
                    Lit_OTIF_YTD_PreviousYear = x.Lit_OTIF_YTD_PreviousYear,
                    Lit_OTIF_Target = x.Lit_OTIF_Target,
                    Lit_OTIF_CurrentYear_Q1 = x.Lit_OTIF_CurrentYear_Q1,
                    Lit_OTIF_CurrentYear_Q2 = x.Lit_OTIF_CurrentYear_Q2,
                    Lit_OTIF_CurrentYear_Q3 = x.Lit_OTIF_CurrentYear_Q3,
                    Lit_OTIF_CurrentYear_Q4 = x.Lit_OTIF_CurrentYear_Q4,
                    Lit_OTIF_YTD_CurrentYear = x.Lit_OTIF_YTD_CurrentYear,
                    Lit_SC_Closure_YTD_PreviousYear = x.Lit_SC_Closure_YTD_PreviousYear,
                    Lit_SC_Closure_Baseline = x.Lit_SC_Closure_Baseline,
                    Lit_SC_Closure_Target = x.Lit_SC_Closure_Target,
                    Lit_SC_Closure_CurrentYear_Q1 = x.Lit_SC_Closure_CurrentYear_Q1,
                    Lit_SC_Closure_CurrentYear_Q2 = x.Lit_SC_Closure_CurrentYear_Q2,
                    Lit_SC_Closure_CurrentYear_Q3 = x.Lit_SC_Closure_CurrentYear_Q3,
                    Lit_SC_Closure_CurrentYear_Q4 = x.Lit_SC_Closure_CurrentYear_Q4,
                    Lit_SC_Closure_YTD_CurrentYear = x.Lit_SC_Closure_YTD_CurrentYear,
                    Lit_CSO_TotalLogged_YTD_PreviousYear = x.Lit_CSO_TotalLogged_YTD_PreviousYear,
                    Lit_CSO_TotalLogged_CurrentYear_Q1 = x.Lit_CSO_TotalLogged_CurrentYear_Q1,
                    Lit_CSO_TotalLogged_CurrentYear_Q2 = x.Lit_CSO_TotalLogged_CurrentYear_Q2,
                    Lit_CSO_TotalLogged_CurrentYear_Q3 = x.Lit_CSO_TotalLogged_CurrentYear_Q3,
                    Lit_CSO_TotalLogged_CurrentYear_Q4 = x.Lit_CSO_TotalLogged_CurrentYear_Q4,
                    Lit_CSO_TotalLogged_YTD_CurrentYear = x.Lit_CSO_TotalLogged_YTD_CurrentYear,
                    Lit_CSO_TotalAClass_YTD_PreviousYear = x.Lit_CSO_TotalAClass_YTD_PreviousYear,
                    Lit_CSO_TotalAClass_CurrentYear_Q1 = x.Lit_CSO_TotalAClass_CurrentYear_Q1,
                    Lit_CSO_TotalAClass_CurrentYear_Q2 = x.Lit_CSO_TotalAClass_CurrentYear_Q2,
                    Lit_CSO_TotalAClass_CurrentYear_Q3 = x.Lit_CSO_TotalAClass_CurrentYear_Q3,
                    Lit_CSO_TotalAClass_CurrentYear_Q4 = x.Lit_CSO_TotalAClass_CurrentYear_Q4,
                    Lit_CSO_TotalAClass_YTD_CurrentYear = x.Lit_CSO_TotalAClass_YTD_CurrentYear,
                    Lit_CSO_AClassClosed_YTD_PreviousYear = x.Lit_CSO_AClassClosed_YTD_PreviousYear,
                    Lit_CSO_AClassClosed_CurrentYear_Q1 = x.Lit_CSO_AClassClosed_CurrentYear_Q1,
                    Lit_CSO_AClassClosed_CurrentYear_Q2 = x.Lit_CSO_AClassClosed_CurrentYear_Q2,
                    Lit_CSO_AClassClosed_CurrentYear_Q3 = x.Lit_CSO_AClassClosed_CurrentYear_Q3,
                    Lit_CSO_AClassClosed_CurrentYear_Q4 = x.Lit_CSO_AClassClosed_CurrentYear_Q4,
                    Lit_CSO_AClassClosed_YTD_CurrentYear = x.Lit_CSO_AClassClosed_YTD_CurrentYear,
                    Lit_CSO_AClassClosedLess45_YTD_PreviousYear = x.Lit_CSO_AClassClosedLess45_YTD_PreviousYear,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q1 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q1,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q2 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q2,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q3 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q3,
                    Lit_CSO_AClassClosedLess45_CurrentYear_Q4 = x.Lit_CSO_AClassClosedLess45_CurrentYear_Q4,
                    Lit_CSO_AClassClosedLess45_YTD_CurrentYear = x.Lit_CSO_AClassClosedLess45_YTD_CurrentYear,
                    Lit_CSO_PercentageClosure_YTD_PreviousYear = x.Lit_CSO_PercentageClosure_YTD_PreviousYear,
                    Lit_CSO_PercentageClosure_CurrentYear_Q1 = x.Lit_CSO_PercentageClosure_CurrentYear_Q1,
                    Lit_CSO_PercentageClosure_CurrentYear_Q2 = x.Lit_CSO_PercentageClosure_CurrentYear_Q2,
                    Lit_CSO_PercentageClosure_CurrentYear_Q3 = x.Lit_CSO_PercentageClosure_CurrentYear_Q3,
                    Lit_CSO_PercentageClosure_CurrentYear_Q4 = x.Lit_CSO_PercentageClosure_CurrentYear_Q4,
                    Lit_CSO_PercentageClosure_YTD_CurrentYear = x.Lit_CSO_PercentageClosure_YTD_CurrentYear,
                    Lit_CostSavings_YTD_PreviousYear = x.Lit_CostSavings_YTD_PreviousYear,
                    Lit_CostSavings_Target = x.Lit_CostSavings_Target,
                    Lit_CostSavings_CurrentYear_Q1 = x.Lit_CostSavings_CurrentYear_Q1,
                    Lit_CostSavings_CurrentYear_Q2 = x.Lit_CostSavings_CurrentYear_Q2,
                    Lit_CostSavings_CurrentYear_Q3 = x.Lit_CostSavings_CurrentYear_Q3,
                    Lit_CostSavings_CurrentYear_Q4 = x.Lit_CostSavings_CurrentYear_Q4,
                    Lit_CostSavings_YTD_CurrentYear = x.Lit_CostSavings_YTD_CurrentYear,
                    Lit_OQL_Vendor1 = x.Lit_OQL_Vendor1,
                    Lit_OQL_Vendor1_YTD_PreviousYear = x.Lit_OQL_Vendor1_YTD_PreviousYear,
                    Lit_OQL_Vendor1_Target = x.Lit_OQL_Vendor1_Target,
                    Lit_OQL_Vendor1_PC01 = x.Lit_OQL_Vendor1_PC01,
                    Lit_OQL_Vendor1_PC02 = x.Lit_OQL_Vendor1_PC02,
                    Lit_OQL_Vendor1_PC03 = x.Lit_OQL_Vendor1_PC03,
                    Lit_OQL_Vendor1_PC04 = x.Lit_OQL_Vendor1_PC04,
                    Lit_OQL_Vendor1_PC05 = x.Lit_OQL_Vendor1_PC05,
                    Lit_OQL_Vendor1_PC06 = x.Lit_OQL_Vendor1_PC06,
                    Lit_OQL_Vendor2 = x.Lit_OQL_Vendor2,
                    Lit_OQL_Vendor2_YTD_PreviousYear = x.Lit_OQL_Vendor2_YTD_PreviousYear,
                    Lit_OQL_Vendor2_Target = x.Lit_OQL_Vendor2_Target,
                    Lit_OQL_Vendor2_PC01 = x.Lit_OQL_Vendor2_PC01,
                    Lit_OQL_Vendor2_PC02 = x.Lit_OQL_Vendor2_PC02,
                    Lit_OQL_Vendor2_PC03 = x.Lit_OQL_Vendor2_PC03,
                    Lit_OQL_Vendor2_PC04 = x.Lit_OQL_Vendor2_PC04,
                    Lit_OQL_Vendor2_PC05 = x.Lit_OQL_Vendor2_PC05,
                    Lit_OQL_Vendor2_PC06 = x.Lit_OQL_Vendor2_PC06,
                    Lit_OQL_Vendor3 = x.Lit_OQL_Vendor3,
                    Lit_OQL_Vendor3_YTD_PreviousYear = x.Lit_OQL_Vendor3_YTD_PreviousYear,
                    Lit_OQL_Vendor3_Target = x.Lit_OQL_Vendor3_Target,
                    Lit_OQL_Vendor3_PC01 = x.Lit_OQL_Vendor3_PC01,
                    Lit_OQL_Vendor3_PC02 = x.Lit_OQL_Vendor3_PC02,
                    Lit_OQL_Vendor3_PC03 = x.Lit_OQL_Vendor3_PC03,
                    Lit_OQL_Vendor3_PC04 = x.Lit_OQL_Vendor3_PC04,
                    Lit_OQL_Vendor3_PC05 = x.Lit_OQL_Vendor3_PC05,
                    Lit_OQL_Vendor3_PC06 = x.Lit_OQL_Vendor3_PC06,
                    Lit_OQL_Vendor4 = x.Lit_OQL_Vendor4,
                    Lit_OQL_Vendor4_YTD_PreviousYear = x.Lit_OQL_Vendor4_YTD_PreviousYear,
                    Lit_OQL_Vendor4_Target = x.Lit_OQL_Vendor4_Target,
                    Lit_OQL_Vendor4_PC01 = x.Lit_OQL_Vendor4_PC01,
                    Lit_OQL_Vendor4_PC02 = x.Lit_OQL_Vendor4_PC02,
                    Lit_OQL_Vendor4_PC03 = x.Lit_OQL_Vendor4_PC03,
                    Lit_OQL_Vendor4_PC04 = x.Lit_OQL_Vendor4_PC04,
                    Lit_OQL_Vendor4_PC05 = x.Lit_OQL_Vendor4_PC05,
                    Lit_OQL_Vendor4_PC06 = x.Lit_OQL_Vendor4_PC06,
                    Lit_OQL_Vendor5 = x.Lit_OQL_Vendor5,
                    Lit_OQL_Vendor5_YTD_PreviousYear = x.Lit_OQL_Vendor5_YTD_PreviousYear,
                    Lit_OQL_Vendor5_Target = x.Lit_OQL_Vendor5_Target,
                    Lit_OQL_Vendor5_PC01 = x.Lit_OQL_Vendor5_PC01,
                    Lit_OQL_Vendor5_PC02 = x.Lit_OQL_Vendor5_PC02,
                    Lit_OQL_Vendor5_PC03 = x.Lit_OQL_Vendor5_PC03,
                    Lit_OQL_Vendor5_PC04 = x.Lit_OQL_Vendor5_PC04,
                    Lit_OQL_Vendor5_PC05 = x.Lit_OQL_Vendor5_PC05,
                    Lit_OQL_Vendor5_PC06 = x.Lit_OQL_Vendor5_PC06,
                    Lit_OQL_Vendor6 = x.Lit_OQL_Vendor6,
                    Lit_OQL_Vendor6_YTD_PreviousYear = x.Lit_OQL_Vendor6_YTD_PreviousYear,
                    Lit_OQL_Vendor6_Target = x.Lit_OQL_Vendor6_Target,
                    Lit_OQL_Vendor6_PC01 = x.Lit_OQL_Vendor6_PC01,
                    Lit_OQL_Vendor6_PC02 = x.Lit_OQL_Vendor6_PC02,
                    Lit_OQL_Vendor6_PC03 = x.Lit_OQL_Vendor6_PC03,
                    Lit_OQL_Vendor6_PC04 = x.Lit_OQL_Vendor6_PC04,
                    Lit_OQL_Vendor6_PC05 = x.Lit_OQL_Vendor6_PC05,
                    Lit_OQL_Vendor6_PC06 = x.Lit_OQL_Vendor6_PC06,
                    Lit_OQL_Vendor7 = x.Lit_OQL_Vendor7,
                    Lit_OQL_Vendor7_YTD_PreviousYear = x.Lit_OQL_Vendor7_YTD_PreviousYear,
                    Lit_OQL_Vendor7_Target = x.Lit_OQL_Vendor7_Target,
                    Lit_OQL_Vendor7_PC01 = x.Lit_OQL_Vendor7_PC01,
                    Lit_OQL_Vendor7_PC02 = x.Lit_OQL_Vendor7_PC02,
                    Lit_OQL_Vendor7_PC03 = x.Lit_OQL_Vendor7_PC03,
                    Lit_OQL_Vendor7_PC04 = x.Lit_OQL_Vendor7_PC04,
                    Lit_OQL_Vendor7_PC05 = x.Lit_OQL_Vendor7_PC05,
                    Lit_OQL_Vendor7_PC06 = x.Lit_OQL_Vendor7_PC06,
                    Lit_OQL_Vendor8 = x.Lit_OQL_Vendor8,
                    Lit_OQL_Vendor8_YTD_PreviousYear = x.Lit_OQL_Vendor8_YTD_PreviousYear,
                    Lit_OQL_Vendor8_Target = x.Lit_OQL_Vendor8_Target,
                    Lit_OQL_Vendor8_PC01 = x.Lit_OQL_Vendor8_PC01,
                    Lit_OQL_Vendor8_PC02 = x.Lit_OQL_Vendor8_PC02,
                    Lit_OQL_Vendor8_PC03 = x.Lit_OQL_Vendor8_PC03,
                    Lit_OQL_Vendor8_PC04 = x.Lit_OQL_Vendor8_PC04,
                    Lit_OQL_Vendor8_PC05 = x.Lit_OQL_Vendor8_PC05,
                    Lit_OQL_Vendor8_PC06 = x.Lit_OQL_Vendor8_PC06,
                    Lit_OQL_CumulativeAvg1_YTD_PreviousYear = x.Lit_OQL_CumulativeAvg1_YTD_PreviousYear,
                    Lit_OQL_CumulativeAvg1_Target = x.Lit_OQL_CumulativeAvg1_Target,
                    Lit_OQL_CumulativeAvg1_PC01 = x.Lit_OQL_CumulativeAvg1_PC01,
                    Lit_OQL_CumulativeAvg1_PC02 = x.Lit_OQL_CumulativeAvg1_PC02,
                    Lit_OQL_CumulativeAvg1_PC03 = x.Lit_OQL_CumulativeAvg1_PC03,
                    Lit_OQL_CumulativeAvg1_PC04 = x.Lit_OQL_CumulativeAvg1_PC04,
                    Lit_OQL_CumulativeAvg1_PC05 = x.Lit_OQL_CumulativeAvg1_PC05,
                    Lit_OQL_CumulativeAvg1_PC06 = x.Lit_OQL_CumulativeAvg1_PC06,
                    Lit_OQL_CumulativeAvg2_YTD_PreviousYear = x.Lit_OQL_CumulativeAvg2_YTD_PreviousYear,
                    Lit_OQL_CumulativeAvg2_Target = x.Lit_OQL_CumulativeAvg2_Target,
                    Lit_OQL_CumulativeAvg2_PC01 = x.Lit_OQL_CumulativeAvg2_PC01,
                    Lit_OQL_CumulativeAvg2_PC02 = x.Lit_OQL_CumulativeAvg2_PC02,
                    Lit_OQL_CumulativeAvg2_PC03 = x.Lit_OQL_CumulativeAvg2_PC03,
                    Lit_OQL_CumulativeAvg2_PC04 = x.Lit_OQL_CumulativeAvg2_PC04,
                    Lit_OQL_CumulativeAvg2_PC05 = x.Lit_OQL_CumulativeAvg2_PC05,
                    Lit_OQL_CumulativeAvg2_PC06 = x.Lit_OQL_CumulativeAvg2_PC06,
                    Seat_SS_CNClosure_Baseline = x.Seat_SS_CNClosure_Baseline,
                    Seat_SS_CNClosure_Target = x.Seat_SS_CNClosure_Target,
                    Seat_SS_CNClosure_CurrentYear_Q1 = x.Seat_SS_CNClosure_CurrentYear_Q1,
                    Seat_SS_CNClosure_CurrentYear_Q2 = x.Seat_SS_CNClosure_CurrentYear_Q2,
                    Seat_SS_CNClosure_CurrentYear_Q3 = x.Seat_SS_CNClosure_CurrentYear_Q3,
                    Seat_SS_CNClosure_CurrentYear_Q4 = x.Seat_SS_CNClosure_CurrentYear_Q4,
                    Seat_SS_CNClosure_YTD_CurrentYear = x.Seat_SS_CNClosure_YTD_CurrentYear,
                    Seat_SS_OTIF_Baseline = x.Seat_SS_OTIF_Baseline,
                    Seat_SS_OTIF_Target = x.Seat_SS_OTIF_Target,
                    Seat_SS_OTIF_CurrentYear_Q1 = x.Seat_SS_OTIF_CurrentYear_Q1,
                    Seat_SS_OTIF_CurrentYear_Q2 = x.Seat_SS_OTIF_CurrentYear_Q2,
                    Seat_SS_OTIF_CurrentYear_Q3 = x.Seat_SS_OTIF_CurrentYear_Q3,
                    Seat_SS_OTIF_CurrentYear_Q4 = x.Seat_SS_OTIF_CurrentYear_Q4,
                    Seat_SS_OTIF_YTD_CurrentYear = x.Seat_SS_OTIF_YTD_CurrentYear,
                    Seat_SS_SPMScore_Baseline = x.Seat_SS_SPMScore_Baseline,
                    Seat_SS_SPMScore_Target = x.Seat_SS_SPMScore_Target,
                    Seat_SS_SPMScore_CurrentYear_Q1 = x.Seat_SS_SPMScore_CurrentYear_Q1,
                    Seat_SS_SPMScore_CurrentYear_Q2 = x.Seat_SS_SPMScore_CurrentYear_Q2,
                    Seat_SS_SPMScore_CurrentYear_Q3 = x.Seat_SS_SPMScore_CurrentYear_Q3,
                    Seat_SS_SPMScore_CurrentYear_Q4 = x.Seat_SS_SPMScore_CurrentYear_Q4,
                    Seat_SS_SPMScore_YTD_CurrentYear = x.Seat_SS_SPMScore_YTD_CurrentYear,
                    Seat_OTIF_Performance_YTD_PreviousYear = x.Seat_OTIF_Performance_YTD_PreviousYear,
                    Seat_OTIF_Performance_Target = x.Seat_OTIF_Performance_Target,
                    Seat_OTIF_Performance_CurrentYear_Q1 = x.Seat_OTIF_Performance_CurrentYear_Q1,
                    Seat_OTIF_Performance_CurrentYear_Q2 = x.Seat_OTIF_Performance_CurrentYear_Q2,
                    Seat_OTIF_Performance_CurrentYear_Q3 = x.Seat_OTIF_Performance_CurrentYear_Q3,
                    Seat_OTIF_Performance_CurrentYear_Q4 = x.Seat_OTIF_Performance_CurrentYear_Q4,
                    Seat_OTIF_Performance_YTD_CurrentYear = x.Seat_OTIF_Performance_YTD_CurrentYear,
                    Seat_CSAT_ReqSent_YTD_PreviousYear = x.Seat_CSAT_ReqSent_YTD_PreviousYear,
                    Seat_CSAT_ReqSent_Baseline = x.Seat_CSAT_ReqSent_Baseline,
                    Seat_CSAT_ReqSent_Target = x.Seat_CSAT_ReqSent_Target,
                    Seat_CSAT_ReqSent_CurrentYear_Q1 = x.Seat_CSAT_ReqSent_CurrentYear_Q1,
                    Seat_CSAT_ReqSent_CurrentYear_Q2 = x.Seat_CSAT_ReqSent_CurrentYear_Q2,
                    Seat_CSAT_ReqSent_CurrentYear_Q3 = x.Seat_CSAT_ReqSent_CurrentYear_Q3,
                    Seat_CSAT_ReqSent_CurrentYear_Q4 = x.Seat_CSAT_ReqSent_CurrentYear_Q4,
                    Seat_CSAT_ReqSent_YTD_CurrentYear = x.Seat_CSAT_ReqSent_YTD_CurrentYear,
                    Seat_CSAT_RespRecvd_YTD_PreviousYear = x.Seat_CSAT_RespRecvd_YTD_PreviousYear,
                    Seat_CSAT_RespRecvd_Baseline = x.Seat_CSAT_RespRecvd_Baseline,
                    Seat_CSAT_RespRecvd_Target = x.Seat_CSAT_RespRecvd_Target,
                    Seat_CSAT_RespRecvd_CurrentYear_Q1 = x.Seat_CSAT_RespRecvd_CurrentYear_Q1,
                    Seat_CSAT_RespRecvd_CurrentYear_Q2 = x.Seat_CSAT_RespRecvd_CurrentYear_Q2,
                    Seat_CSAT_RespRecvd_CurrentYear_Q3 = x.Seat_CSAT_RespRecvd_CurrentYear_Q3,
                    Seat_CSAT_RespRecvd_CurrentYear_Q4 = x.Seat_CSAT_RespRecvd_CurrentYear_Q4,
                    Seat_CSAT_RespRecvd_YTD_CurrentYear = x.Seat_CSAT_RespRecvd_YTD_CurrentYear,
                    Seat_CSAT_Promoter_YTD_PreviousYear = x.Seat_CSAT_Promoter_YTD_PreviousYear,
                    Seat_CSAT_Promoter_Baseline = x.Seat_CSAT_Promoter_Baseline,
                    Seat_CSAT_Promoter_Target = x.Seat_CSAT_Promoter_Target,
                    Seat_CSAT_Promoter_CurrentYear_Q1 = x.Seat_CSAT_Promoter_CurrentYear_Q1,
                    Seat_CSAT_Promoter_CurrentYear_Q2 = x.Seat_CSAT_Promoter_CurrentYear_Q2,
                    Seat_CSAT_Promoter_CurrentYear_Q3 = x.Seat_CSAT_Promoter_CurrentYear_Q3,
                    Seat_CSAT_Promoter_CurrentYear_Q4 = x.Seat_CSAT_Promoter_CurrentYear_Q4,
                    Seat_CSAT_Promoter_YTD_CurrentYear = x.Seat_CSAT_Promoter_YTD_CurrentYear,
                    Seat_CSAT_Detractor_YTD_PreviousYear = x.Seat_CSAT_Detractor_YTD_PreviousYear,
                    Seat_CSAT_Detractor_Baseline = x.Seat_CSAT_Detractor_Baseline,
                    Seat_CSAT_Detractor_Target = x.Seat_CSAT_Detractor_Target,
                    Seat_CSAT_Detractor_CurrentYear_Q1 = x.Seat_CSAT_Detractor_CurrentYear_Q1,
                    Seat_CSAT_Detractor_CurrentYear_Q2 = x.Seat_CSAT_Detractor_CurrentYear_Q2,
                    Seat_CSAT_Detractor_CurrentYear_Q3 = x.Seat_CSAT_Detractor_CurrentYear_Q3,
                    Seat_CSAT_Detractor_CurrentYear_Q4 = x.Seat_CSAT_Detractor_CurrentYear_Q4,
                    Seat_CSAT_Detractor_YTD_CurrentYear = x.Seat_CSAT_Detractor_YTD_CurrentYear,
                    Seat_CSAT_NPS_YTD_PreviousYear = x.Seat_CSAT_NPS_YTD_PreviousYear,
                    Seat_CSAT_NPS_Baseline = x.Seat_CSAT_NPS_Baseline,
                    Seat_CSAT_NPS_Target = x.Seat_CSAT_NPS_Target,
                    Seat_CSAT_NPS_CurrentYear_Q1 = x.Seat_CSAT_NPS_CurrentYear_Q1,
                    Seat_CSAT_NPS_CurrentYear_Q2 = x.Seat_CSAT_NPS_CurrentYear_Q2,
                    Seat_CSAT_NPS_CurrentYear_Q3 = x.Seat_CSAT_NPS_CurrentYear_Q3,
                    Seat_CSAT_NPS_CurrentYear_Q4 = x.Seat_CSAT_NPS_CurrentYear_Q4,
                    Seat_CSAT_NPS_YTD_CurrentYear = x.Seat_CSAT_NPS_YTD_CurrentYear,
                    Seat_CSO_TotalLogged_YTD_PreviousYear = x.Seat_CSO_TotalLogged_YTD_PreviousYear,
                    Seat_CSO_TotalLogged_CurrentYear_Q1 = x.Seat_CSO_TotalLogged_CurrentYear_Q1,
                    Seat_CSO_TotalLogged_CurrentYear_Q2 = x.Seat_CSO_TotalLogged_CurrentYear_Q2,
                    Seat_CSO_TotalLogged_CurrentYear_Q3 = x.Seat_CSO_TotalLogged_CurrentYear_Q3,
                    Seat_CSO_TotalLogged_CurrentYear_Q4 = x.Seat_CSO_TotalLogged_CurrentYear_Q4,
                    Seat_CSO_TotalLogged_YTD_CurrentYear = x.Seat_CSO_TotalLogged_YTD_CurrentYear,
                    Seat_CSO_TotalAClass_YTD_PreviousYear = x.Seat_CSO_TotalAClass_YTD_PreviousYear,
                    Seat_CSO_TotalAClass_CurrentYear_Q1 = x.Seat_CSO_TotalAClass_CurrentYear_Q1,
                    Seat_CSO_TotalAClass_CurrentYear_Q2 = x.Seat_CSO_TotalAClass_CurrentYear_Q2,
                    Seat_CSO_TotalAClass_CurrentYear_Q3 = x.Seat_CSO_TotalAClass_CurrentYear_Q3,
                    Seat_CSO_TotalAClass_CurrentYear_Q4 = x.Seat_CSO_TotalAClass_CurrentYear_Q4,
                    Seat_CSO_TotalAClass_YTD_CurrentYear = x.Seat_CSO_TotalAClass_YTD_CurrentYear,
                    Seat_CSO_AClassClosed_YTD_PreviousYear = x.Seat_CSO_AClassClosed_YTD_PreviousYear,
                    Seat_CSO_AClassClosed_CurrentYear_Q1 = x.Seat_CSO_AClassClosed_CurrentYear_Q1,
                    Seat_CSO_AClassClosed_CurrentYear_Q2 = x.Seat_CSO_AClassClosed_CurrentYear_Q2,
                    Seat_CSO_AClassClosed_CurrentYear_Q3 = x.Seat_CSO_AClassClosed_CurrentYear_Q3,
                    Seat_CSO_AClassClosed_CurrentYear_Q4 = x.Seat_CSO_AClassClosed_CurrentYear_Q4,
                    Seat_CSO_AClassClosed_YTD_CurrentYear = x.Seat_CSO_AClassClosed_YTD_CurrentYear,
                    Seat_CSO_AClassClosedLess45_YTD_PreviousYear = x.Seat_CSO_AClassClosedLess45_YTD_PreviousYear,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q1 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q1,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q2 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q2,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q3 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q3,
                    Seat_CSO_AClassClosedLess45_CurrentYear_Q4 = x.Seat_CSO_AClassClosedLess45_CurrentYear_Q4,
                    Seat_CSO_AClassClosedLess45_YTD_CurrentYear = x.Seat_CSO_AClassClosedLess45_YTD_CurrentYear,
                    Seat_CSO_AClassClosedUnder45_YTD_PreviousYear = x.Seat_CSO_AClassClosedUnder45_YTD_PreviousYear,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q1 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q1,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q2 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q2,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q3 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q3,
                    Seat_CSO_AClassClosedUnder45_CurrentYear_Q4 = x.Seat_CSO_AClassClosedUnder45_CurrentYear_Q4,
                    Seat_CSO_AClassClosedUnder45_YTD_CurrentYear = x.Seat_CSO_AClassClosedUnder45_YTD_CurrentYear,
                    Seat_SPM_Supp1 = x.Seat_SPM_Supp1,
                    Seat_SPM_Supp1_PreviousYear_Q4 = x.Seat_SPM_Supp1_PreviousYear_Q4,
                    Seat_SPM_Supp1_CurrentYear_Q1 = x.Seat_SPM_Supp1_CurrentYear_Q1,
                    Seat_SPM_Supp1_CurrentYear_Q2 = x.Seat_SPM_Supp1_CurrentYear_Q2,
                    Seat_SPM_Supp1_CurrentYear_Q3 = x.Seat_SPM_Supp1_CurrentYear_Q3,
                    Seat_SPM_Supp1_CurrentYear_Q4 = x.Seat_SPM_Supp1_CurrentYear_Q4,
                    Seat_SPM_Supp2 = x.Seat_SPM_Supp2,
                    Seat_SPM_Supp2_PreviousYear_Q4 = x.Seat_SPM_Supp2_PreviousYear_Q4,
                    Seat_SPM_Supp2_CurrentYear_Q1 = x.Seat_SPM_Supp2_CurrentYear_Q1,
                    Seat_SPM_Supp2_CurrentYear_Q2 = x.Seat_SPM_Supp2_CurrentYear_Q2,
                    Seat_SPM_Supp2_CurrentYear_Q3 = x.Seat_SPM_Supp2_CurrentYear_Q3,
                    Seat_SPM_Supp2_CurrentYear_Q4 = x.Seat_SPM_Supp2_CurrentYear_Q4,
                    Seat_SPM_Supp3 = x.Seat_SPM_Supp3,
                    Seat_SPM_Supp3_PreviousYear_Q4 = x.Seat_SPM_Supp3_PreviousYear_Q4,
                    Seat_SPM_Supp3_CurrentYear_Q1 = x.Seat_SPM_Supp3_CurrentYear_Q1,
                    Seat_SPM_Supp3_CurrentYear_Q2 = x.Seat_SPM_Supp3_CurrentYear_Q2,
                    Seat_SPM_Supp3_CurrentYear_Q3 = x.Seat_SPM_Supp3_CurrentYear_Q3,
                    Seat_SPM_Supp3_CurrentYear_Q4 = x.Seat_SPM_Supp3_CurrentYear_Q4,
                    Seat_SPM_Supp4 = x.Seat_SPM_Supp4,
                    Seat_SPM_Supp4_PreviousYear_Q4 = x.Seat_SPM_Supp4_PreviousYear_Q4,
                    Seat_SPM_Supp4_CurrentYear_Q1 = x.Seat_SPM_Supp4_CurrentYear_Q1,
                    Seat_SPM_Supp4_CurrentYear_Q2 = x.Seat_SPM_Supp4_CurrentYear_Q2,
                    Seat_SPM_Supp4_CurrentYear_Q3 = x.Seat_SPM_Supp4_CurrentYear_Q3,
                    Seat_SPM_Supp4_CurrentYear_Q4 = x.Seat_SPM_Supp4_CurrentYear_Q4,
                    Seat_SPM_Supp5 = x.Seat_SPM_Supp5,
                    Seat_SPM_Supp5_PreviousYear_Q4 = x.Seat_SPM_Supp5_PreviousYear_Q4,
                    Seat_SPM_Supp5_CurrentYear_Q1 = x.Seat_SPM_Supp5_CurrentYear_Q1,
                    Seat_SPM_Supp5_CurrentYear_Q2 = x.Seat_SPM_Supp5_CurrentYear_Q2,
                    Seat_SPM_Supp5_CurrentYear_Q3 = x.Seat_SPM_Supp5_CurrentYear_Q3,
                    Seat_SPM_Supp5_CurrentYear_Q4 = x.Seat_SPM_Supp5_CurrentYear_Q4,
                    Seat_IQA_TotalSites_YTD_PreviousYear = x.Seat_IQA_TotalSites_YTD_PreviousYear,
                    Seat_IQA_TotalSites_Target = x.Seat_IQA_TotalSites_Target,
                    Seat_IQA_TotalSites_CurrentYear_Q1 = x.Seat_IQA_TotalSites_CurrentYear_Q1,
                    Seat_IQA_TotalSites_CurrentYear_Q2 = x.Seat_IQA_TotalSites_CurrentYear_Q2,
                    Seat_IQA_TotalSites_CurrentYear_Q3 = x.Seat_IQA_TotalSites_CurrentYear_Q3,
                    Seat_IQA_TotalSites_CurrentYear_Q4 = x.Seat_IQA_TotalSites_CurrentYear_Q4,
                    Seat_IQA_TotalSites_YTD_CurrentYear = x.Seat_IQA_TotalSites_YTD_CurrentYear,
                    Seat_IQA_SitesCompleted_YTD_PreviousYear = x.Seat_IQA_SitesCompleted_YTD_PreviousYear,
                    Seat_IQA_SitesCompleted_Target = x.Seat_IQA_SitesCompleted_Target,
                    Seat_IQA_SitesCompleted_CurrentYear_Q1 = x.Seat_IQA_SitesCompleted_CurrentYear_Q1,
                    Seat_IQA_SitesCompleted_CurrentYear_Q2 = x.Seat_IQA_SitesCompleted_CurrentYear_Q2,
                    Seat_IQA_SitesCompleted_CurrentYear_Q3 = x.Seat_IQA_SitesCompleted_CurrentYear_Q3,
                    Seat_IQA_SitesCompleted_CurrentYear_Q4 = x.Seat_IQA_SitesCompleted_CurrentYear_Q4,
                    Seat_IQA_SitesCompleted_YTD_CurrentYear = x.Seat_IQA_SitesCompleted_YTD_CurrentYear,
                    Seat_IQA_AuditsCompleted_YTD_PreviousYear = x.Seat_IQA_AuditsCompleted_YTD_PreviousYear,
                    Seat_IQA_AuditsCompleted_Target = x.Seat_IQA_AuditsCompleted_Target,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q1 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q1,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q2 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q2,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q3 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q3,
                    Seat_IQA_AuditsCompleted_CurrentYear_Q4 = x.Seat_IQA_AuditsCompleted_CurrentYear_Q4,
                    Seat_IQA_AuditsCompleted_YTD_CurrentYear = x.Seat_IQA_AuditsCompleted_YTD_CurrentYear,
                    Seat_IQA_PercCompleted_YTD_PreviousYear = x.Seat_IQA_PercCompleted_YTD_PreviousYear,
                    Seat_IQA_PercCompleted_Target = x.Seat_IQA_PercCompleted_Target,
                    Seat_IQA_PercCompleted_CurrentYear_Q1 = x.Seat_IQA_PercCompleted_CurrentYear_Q1,
                    Seat_IQA_PercCompleted_CurrentYear_Q2 = x.Seat_IQA_PercCompleted_CurrentYear_Q2,
                    Seat_IQA_PercCompleted_CurrentYear_Q3 = x.Seat_IQA_PercCompleted_CurrentYear_Q3,
                    Seat_IQA_PercCompleted_CurrentYear_Q4 = x.Seat_IQA_PercCompleted_CurrentYear_Q4,
                    Seat_IQA_PercCompleted_YTD_CurrentYear = x.Seat_IQA_PercCompleted_YTD_CurrentYear,
                    Seat_IQA_AvgSigma_YTD_PreviousYear = x.Seat_IQA_AvgSigma_YTD_PreviousYear,
                    Seat_IQA_AvgSigma_Target = x.Seat_IQA_AvgSigma_Target,
                    Seat_IQA_AvgSigma_CurrentYear_Q1 = x.Seat_IQA_AvgSigma_CurrentYear_Q1,
                    Seat_IQA_AvgSigma_CurrentYear_Q2 = x.Seat_IQA_AvgSigma_CurrentYear_Q2,
                    Seat_IQA_AvgSigma_CurrentYear_Q3 = x.Seat_IQA_AvgSigma_CurrentYear_Q3,
                    Seat_IQA_AvgSigma_CurrentYear_Q4 = x.Seat_IQA_AvgSigma_CurrentYear_Q4,
                    Seat_IQA_AvgSigma_YTD_CurrentYear = x.Seat_IQA_AvgSigma_YTD_CurrentYear
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
                new SqlParameter("@FinancialYear", model.FinancialYear),
                new SqlParameter("@Lit_SEE_Engagement_Target", model.Lit_SEE_Engagement_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q1", model.Lit_SEE_Engagement_PreviousYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q2", model.Lit_SEE_Engagement_PreviousYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q3", model.Lit_SEE_Engagement_PreviousYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q4", model.Lit_SEE_Engagement_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q1", model.Lit_SEE_Engagement_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q2", model.Lit_SEE_Engagement_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q3", model.Lit_SEE_Engagement_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q4", model.Lit_SEE_Engagement_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_Target", model.Lit_SEE_Effectiveness_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q1", model.Lit_SEE_Effectiveness_PreviousYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q2", model.Lit_SEE_Effectiveness_PreviousYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q3", model.Lit_SEE_Effectiveness_PreviousYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q4", model.Lit_SEE_Effectiveness_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q1", model.Lit_SEE_Effectiveness_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q2", model.Lit_SEE_Effectiveness_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q3", model.Lit_SEE_Effectiveness_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q4", model.Lit_SEE_Effectiveness_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Baseline", model.Lit_SS_ServiceComplaints_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Target", model.Lit_SS_ServiceComplaints_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q1", model.Lit_SS_ServiceComplaints_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q2", model.Lit_SS_ServiceComplaints_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q3", model.Lit_SS_ServiceComplaints_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q4", model.Lit_SS_ServiceComplaints_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Baseline", model.Lit_SS_DesignLSG_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Target", model.Lit_SS_DesignLSG_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q1", model.Lit_SS_DesignLSG_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q2", model.Lit_SS_DesignLSG_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q3", model.Lit_SS_DesignLSG_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q4", model.Lit_SS_DesignLSG_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Baseline", model.Lit_SS_CostReduction_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Target", model.Lit_SS_CostReduction_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q1", model.Lit_SS_CostReduction_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q2", model.Lit_SS_CostReduction_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q3", model.Lit_SS_CostReduction_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q4", model.Lit_SS_CostReduction_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Baseline", model.Lit_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Target", model.Lit_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q1", model.Lit_SS_OTIF_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q2", model.Lit_SS_OTIF_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q3", model.Lit_SS_OTIF_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q4", model.Lit_SS_OTIF_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Baseline", model.Lit_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Target", model.Lit_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q1", model.Lit_SS_SPMScore_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q2", model.Lit_SS_SPMScore_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q3", model.Lit_SS_SPMScore_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q4", model.Lit_SS_SPMScore_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD_PreviousYear", model.Lit_CSAT_ReqSent_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Baseline", model.Lit_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Target", model.Lit_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q1", model.Lit_CSAT_ReqSent_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q2", model.Lit_CSAT_ReqSent_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q3", model.Lit_CSAT_ReqSent_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q4", model.Lit_CSAT_ReqSent_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD_CurrentYear", model.Lit_CSAT_ReqSent_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD_PreviousYear", model.Lit_CSAT_RespRecvd_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Baseline", model.Lit_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Target", model.Lit_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q1", model.Lit_CSAT_RespRecvd_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q2", model.Lit_CSAT_RespRecvd_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q3", model.Lit_CSAT_RespRecvd_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q4", model.Lit_CSAT_RespRecvd_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD_CurrentYear", model.Lit_CSAT_RespRecvd_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD_PreviousYear", model.Lit_CSAT_Promoter_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Baseline", model.Lit_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Target", model.Lit_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q1", model.Lit_CSAT_Promoter_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q2", model.Lit_CSAT_Promoter_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q3", model.Lit_CSAT_Promoter_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q4", model.Lit_CSAT_Promoter_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD_CurrentYear", model.Lit_CSAT_Promoter_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD_PreviousYear", model.Lit_CSAT_Detractor_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Baseline", model.Lit_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Target", model.Lit_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q1", model.Lit_CSAT_Detractor_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q2", model.Lit_CSAT_Detractor_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q3", model.Lit_CSAT_Detractor_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q4", model.Lit_CSAT_Detractor_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD_CurrentYear", model.Lit_CSAT_Detractor_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD_PreviousYear", model.Lit_CSAT_NPS_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Baseline", model.Lit_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Target", model.Lit_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q1", model.Lit_CSAT_NPS_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q2", model.Lit_CSAT_NPS_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q3", model.Lit_CSAT_NPS_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q4", model.Lit_CSAT_NPS_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD_CurrentYear", model.Lit_CSAT_NPS_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1", model.Lit_SPM_Supp1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q1", model.Lit_SPM_Supp1_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q2", model.Lit_SPM_Supp1_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q3", model.Lit_SPM_Supp1_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q4", model.Lit_SPM_Supp1_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2", model.Lit_SPM_Supp2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q1", model.Lit_SPM_Supp2_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q2", model.Lit_SPM_Supp2_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q3", model.Lit_SPM_Supp2_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q4", model.Lit_SPM_Supp2_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3", model.Lit_SPM_Supp3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q1", model.Lit_SPM_Supp3_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q2", model.Lit_SPM_Supp3_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q3", model.Lit_SPM_Supp3_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q4", model.Lit_SPM_Supp3_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4", model.Lit_SPM_Supp4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q1", model.Lit_SPM_Supp4_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q2", model.Lit_SPM_Supp4_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q3", model.Lit_SPM_Supp4_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q4", model.Lit_SPM_Supp4_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5", model.Lit_SPM_Supp5 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q1", model.Lit_SPM_Supp5_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q2", model.Lit_SPM_Supp5_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q3", model.Lit_SPM_Supp5_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q4", model.Lit_SPM_Supp5_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6", model.Lit_SPM_Supp6 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q1", model.Lit_SPM_Supp6_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q2", model.Lit_SPM_Supp6_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q3", model.Lit_SPM_Supp6_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q4", model.Lit_SPM_Supp6_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7", model.Lit_SPM_Supp7 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q1", model.Lit_SPM_Supp7_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q2", model.Lit_SPM_Supp7_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q3", model.Lit_SPM_Supp7_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q4", model.Lit_SPM_Supp7_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8", model.Lit_SPM_Supp8 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q1", model.Lit_SPM_Supp8_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q2", model.Lit_SPM_Supp8_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q3", model.Lit_SPM_Supp8_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q4", model.Lit_SPM_Supp8_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9", model.Lit_SPM_Supp9 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q1", model.Lit_SPM_Supp9_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q2", model.Lit_SPM_Supp9_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q3", model.Lit_SPM_Supp9_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q4", model.Lit_SPM_Supp9_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10", model.Lit_SPM_Supp10 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q1", model.Lit_SPM_Supp10_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q2", model.Lit_SPM_Supp10_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q3", model.Lit_SPM_Supp10_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q4", model.Lit_SPM_Supp10_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD_PreviousYear", model.Lit_OTIF_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Target", model.Lit_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q1", model.Lit_OTIF_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q2", model.Lit_OTIF_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q3", model.Lit_OTIF_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q4", model.Lit_OTIF_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD_CurrentYear", model.Lit_OTIF_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD_PreviousYear", model.Lit_SC_Closure_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Baseline", model.Lit_SC_Closure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Target", model.Lit_SC_Closure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q1", model.Lit_SC_Closure_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q2", model.Lit_SC_Closure_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q3", model.Lit_SC_Closure_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q4", model.Lit_SC_Closure_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD_CurrentYear", model.Lit_SC_Closure_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD_PreviousYear", model.Lit_CSO_TotalLogged_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q1", model.Lit_CSO_TotalLogged_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q2", model.Lit_CSO_TotalLogged_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q3", model.Lit_CSO_TotalLogged_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q4", model.Lit_CSO_TotalLogged_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD_CurrentYear", model.Lit_CSO_TotalLogged_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD_PreviousYear", model.Lit_CSO_TotalAClass_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q1", model.Lit_CSO_TotalAClass_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q2", model.Lit_CSO_TotalAClass_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q3", model.Lit_CSO_TotalAClass_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q4", model.Lit_CSO_TotalAClass_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD_CurrentYear", model.Lit_CSO_TotalAClass_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD_PreviousYear", model.Lit_CSO_AClassClosed_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q1", model.Lit_CSO_AClassClosed_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q2", model.Lit_CSO_AClassClosed_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q3", model.Lit_CSO_AClassClosed_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q4", model.Lit_CSO_AClassClosed_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD_CurrentYear", model.Lit_CSO_AClassClosed_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD_PreviousYear", model.Lit_CSO_AClassClosedLess45_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q1", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q2", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q3", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q4", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD_CurrentYear", model.Lit_CSO_AClassClosedLess45_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD_PreviousYear", model.Lit_CSO_PercentageClosure_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q1", model.Lit_CSO_PercentageClosure_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q2", model.Lit_CSO_PercentageClosure_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q3", model.Lit_CSO_PercentageClosure_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q4", model.Lit_CSO_PercentageClosure_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD_CurrentYear", model.Lit_CSO_PercentageClosure_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD_PreviousYear", model.Lit_CostSavings_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Target", model.Lit_CostSavings_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q1", model.Lit_CostSavings_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q2", model.Lit_CostSavings_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q3", model.Lit_CostSavings_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q4", model.Lit_CostSavings_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD_CurrentYear", model.Lit_CostSavings_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1", model.Lit_OQL_Vendor1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_YTD_PreviousYear", model.Lit_OQL_Vendor1_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_Target", model.Lit_OQL_Vendor1_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC01", model.Lit_OQL_Vendor1_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC02", model.Lit_OQL_Vendor1_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC03", model.Lit_OQL_Vendor1_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC04", model.Lit_OQL_Vendor1_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC05", model.Lit_OQL_Vendor1_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC06", model.Lit_OQL_Vendor1_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2", model.Lit_OQL_Vendor2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_YTD_PreviousYear", model.Lit_OQL_Vendor2_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_Target", model.Lit_OQL_Vendor2_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC01", model.Lit_OQL_Vendor2_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC02", model.Lit_OQL_Vendor2_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC03", model.Lit_OQL_Vendor2_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC04", model.Lit_OQL_Vendor2_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC05", model.Lit_OQL_Vendor2_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC06", model.Lit_OQL_Vendor2_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3", model.Lit_OQL_Vendor3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_YTD_PreviousYear", model.Lit_OQL_Vendor3_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_Target", model.Lit_OQL_Vendor3_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC01", model.Lit_OQL_Vendor3_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC02", model.Lit_OQL_Vendor3_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC03", model.Lit_OQL_Vendor3_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC04", model.Lit_OQL_Vendor3_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC05", model.Lit_OQL_Vendor3_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC06", model.Lit_OQL_Vendor3_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4", model.Lit_OQL_Vendor4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_YTD_PreviousYear", model.Lit_OQL_Vendor4_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_Target", model.Lit_OQL_Vendor4_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC01", model.Lit_OQL_Vendor4_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC02", model.Lit_OQL_Vendor4_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC03", model.Lit_OQL_Vendor4_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC04", model.Lit_OQL_Vendor4_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC05", model.Lit_OQL_Vendor4_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC06", model.Lit_OQL_Vendor4_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5", model.Lit_OQL_Vendor5 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_YTD_PreviousYear", model.Lit_OQL_Vendor5_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_Target", model.Lit_OQL_Vendor5_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC01", model.Lit_OQL_Vendor5_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC02", model.Lit_OQL_Vendor5_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC03", model.Lit_OQL_Vendor5_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC04", model.Lit_OQL_Vendor5_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC05", model.Lit_OQL_Vendor5_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC06", model.Lit_OQL_Vendor5_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6", model.Lit_OQL_Vendor6 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_YTD_PreviousYear", model.Lit_OQL_Vendor6_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_Target", model.Lit_OQL_Vendor6_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC01", model.Lit_OQL_Vendor6_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC02", model.Lit_OQL_Vendor6_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC03", model.Lit_OQL_Vendor6_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC04", model.Lit_OQL_Vendor6_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC05", model.Lit_OQL_Vendor6_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC06", model.Lit_OQL_Vendor6_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7", model.Lit_OQL_Vendor7 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_YTD_PreviousYear", model.Lit_OQL_Vendor7_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_Target", model.Lit_OQL_Vendor7_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC01", model.Lit_OQL_Vendor7_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC02", model.Lit_OQL_Vendor7_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC03", model.Lit_OQL_Vendor7_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC04", model.Lit_OQL_Vendor7_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC05", model.Lit_OQL_Vendor7_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC06", model.Lit_OQL_Vendor7_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8", model.Lit_OQL_Vendor8 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_YTD_PreviousYear", model.Lit_OQL_Vendor8_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_Target", model.Lit_OQL_Vendor8_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC01", model.Lit_OQL_Vendor8_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC02", model.Lit_OQL_Vendor8_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC03", model.Lit_OQL_Vendor8_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC04", model.Lit_OQL_Vendor8_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC05", model.Lit_OQL_Vendor8_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC06", model.Lit_OQL_Vendor8_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_YTD_PreviousYear", model.Lit_OQL_CumulativeAvg1_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_Target", model.Lit_OQL_CumulativeAvg1_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC01", model.Lit_OQL_CumulativeAvg1_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC02", model.Lit_OQL_CumulativeAvg1_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC03", model.Lit_OQL_CumulativeAvg1_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC04", model.Lit_OQL_CumulativeAvg1_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC05", model.Lit_OQL_CumulativeAvg1_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC06", model.Lit_OQL_CumulativeAvg1_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_YTD_PreviousYear", model.Lit_OQL_CumulativeAvg2_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_Target", model.Lit_OQL_CumulativeAvg2_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC01", model.Lit_OQL_CumulativeAvg2_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC02", model.Lit_OQL_CumulativeAvg2_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC03", model.Lit_OQL_CumulativeAvg2_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC04", model.Lit_OQL_CumulativeAvg2_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC05", model.Lit_OQL_CumulativeAvg2_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC06", model.Lit_OQL_CumulativeAvg2_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Baseline", model.Seat_SS_CNClosure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Target", model.Seat_SS_CNClosure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q1", model.Seat_SS_CNClosure_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q2", model.Seat_SS_CNClosure_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q3", model.Seat_SS_CNClosure_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q4", model.Seat_SS_CNClosure_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_YTD_CurrentYear", model.Seat_SS_CNClosure_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Baseline", model.Seat_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Target", model.Seat_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q1", model.Seat_SS_OTIF_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q2", model.Seat_SS_OTIF_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q3", model.Seat_SS_OTIF_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q4", model.Seat_SS_OTIF_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_YTD_CurrentYear", model.Seat_SS_OTIF_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Baseline", model.Seat_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Target", model.Seat_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q1", model.Seat_SS_SPMScore_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q2", model.Seat_SS_SPMScore_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q3", model.Seat_SS_SPMScore_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q4", model.Seat_SS_SPMScore_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_YTD_CurrentYear", model.Seat_SS_SPMScore_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD_PreviousYear", model.Seat_OTIF_Performance_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Target", model.Seat_OTIF_Performance_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q1", model.Seat_OTIF_Performance_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q2", model.Seat_OTIF_Performance_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q3", model.Seat_OTIF_Performance_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q4", model.Seat_OTIF_Performance_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD_CurrentYear", model.Seat_OTIF_Performance_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD_PreviousYear", model.Seat_CSAT_ReqSent_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Baseline", model.Seat_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Target", model.Seat_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q1", model.Seat_CSAT_ReqSent_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q2", model.Seat_CSAT_ReqSent_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q3", model.Seat_CSAT_ReqSent_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q4", model.Seat_CSAT_ReqSent_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD_CurrentYear", model.Seat_CSAT_ReqSent_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD_PreviousYear", model.Seat_CSAT_RespRecvd_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Baseline", model.Seat_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Target", model.Seat_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q1", model.Seat_CSAT_RespRecvd_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q2", model.Seat_CSAT_RespRecvd_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q3", model.Seat_CSAT_RespRecvd_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q4", model.Seat_CSAT_RespRecvd_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD_CurrentYear", model.Seat_CSAT_RespRecvd_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD_PreviousYear", model.Seat_CSAT_Promoter_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Baseline", model.Seat_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Target", model.Seat_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q1", model.Seat_CSAT_Promoter_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q2", model.Seat_CSAT_Promoter_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q3", model.Seat_CSAT_Promoter_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q4", model.Seat_CSAT_Promoter_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD_CurrentYear", model.Seat_CSAT_Promoter_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD_PreviousYear", model.Seat_CSAT_Detractor_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Baseline", model.Seat_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Target", model.Seat_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q1", model.Seat_CSAT_Detractor_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q2", model.Seat_CSAT_Detractor_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q3", model.Seat_CSAT_Detractor_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q4", model.Seat_CSAT_Detractor_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD_CurrentYear", model.Seat_CSAT_Detractor_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD_PreviousYear", model.Seat_CSAT_NPS_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Baseline", model.Seat_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Target", model.Seat_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q1", model.Seat_CSAT_NPS_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q2", model.Seat_CSAT_NPS_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q3", model.Seat_CSAT_NPS_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q4", model.Seat_CSAT_NPS_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD_CurrentYear", model.Seat_CSAT_NPS_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD_PreviousYear", model.Seat_CSO_TotalLogged_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q1", model.Seat_CSO_TotalLogged_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q2", model.Seat_CSO_TotalLogged_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q3", model.Seat_CSO_TotalLogged_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q4", model.Seat_CSO_TotalLogged_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD_CurrentYear", model.Seat_CSO_TotalLogged_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD_PreviousYear", model.Seat_CSO_TotalAClass_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q1", model.Seat_CSO_TotalAClass_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q2", model.Seat_CSO_TotalAClass_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q3", model.Seat_CSO_TotalAClass_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q4", model.Seat_CSO_TotalAClass_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD_CurrentYear", model.Seat_CSO_TotalAClass_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD_PreviousYear", model.Seat_CSO_AClassClosed_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q1", model.Seat_CSO_AClassClosed_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q2", model.Seat_CSO_AClassClosed_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q3", model.Seat_CSO_AClassClosed_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q4", model.Seat_CSO_AClassClosed_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD_CurrentYear", model.Seat_CSO_AClassClosed_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_YTD_PreviousYear", model.Seat_CSO_AClassClosedLess45_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q1", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q2", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q3", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q4", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_YTD_CurrentYear", model.Seat_CSO_AClassClosedLess45_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_YTD_PreviousYear", model.Seat_CSO_AClassClosedUnder45_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q1", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q2", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q3", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q4", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_YTD_CurrentYear", model.Seat_CSO_AClassClosedUnder45_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1", model.Seat_SPM_Supp1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_PreviousYear_Q4", model.Seat_SPM_Supp1_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q1", model.Seat_SPM_Supp1_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q2", model.Seat_SPM_Supp1_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q3", model.Seat_SPM_Supp1_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q4", model.Seat_SPM_Supp1_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2", model.Seat_SPM_Supp2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_PreviousYear_Q4", model.Seat_SPM_Supp2_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q1", model.Seat_SPM_Supp2_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q2", model.Seat_SPM_Supp2_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q3", model.Seat_SPM_Supp2_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q4", model.Seat_SPM_Supp2_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3", model.Seat_SPM_Supp3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_PreviousYear_Q4", model.Seat_SPM_Supp3_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q1", model.Seat_SPM_Supp3_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q2", model.Seat_SPM_Supp3_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q3", model.Seat_SPM_Supp3_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q4", model.Seat_SPM_Supp3_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4", model.Seat_SPM_Supp4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_PreviousYear_Q4", model.Seat_SPM_Supp4_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q1", model.Seat_SPM_Supp4_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q2", model.Seat_SPM_Supp4_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q3", model.Seat_SPM_Supp4_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q4", model.Seat_SPM_Supp4_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5", model.Seat_SPM_Supp5 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_PreviousYear_Q4", model.Seat_SPM_Supp5_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q1", model.Seat_SPM_Supp5_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q2", model.Seat_SPM_Supp5_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q3", model.Seat_SPM_Supp5_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q4", model.Seat_SPM_Supp5_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD_PreviousYear", model.Seat_IQA_TotalSites_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Target", model.Seat_IQA_TotalSites_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q1", model.Seat_IQA_TotalSites_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q2", model.Seat_IQA_TotalSites_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q3", model.Seat_IQA_TotalSites_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q4", model.Seat_IQA_TotalSites_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD_CurrentYear", model.Seat_IQA_TotalSites_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD_PreviousYear", model.Seat_IQA_SitesCompleted_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Target", model.Seat_IQA_SitesCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q1", model.Seat_IQA_SitesCompleted_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q2", model.Seat_IQA_SitesCompleted_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q3", model.Seat_IQA_SitesCompleted_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q4", model.Seat_IQA_SitesCompleted_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD_CurrentYear", model.Seat_IQA_SitesCompleted_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD_PreviousYear", model.Seat_IQA_AuditsCompleted_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Target", model.Seat_IQA_AuditsCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q1", model.Seat_IQA_AuditsCompleted_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q2", model.Seat_IQA_AuditsCompleted_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q3", model.Seat_IQA_AuditsCompleted_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q4", model.Seat_IQA_AuditsCompleted_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD_CurrentYear", model.Seat_IQA_AuditsCompleted_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD_PreviousYear", model.Seat_IQA_PercCompleted_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Target", model.Seat_IQA_PercCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q1", model.Seat_IQA_PercCompleted_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q2", model.Seat_IQA_PercCompleted_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q3", model.Seat_IQA_PercCompleted_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q4", model.Seat_IQA_PercCompleted_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD_CurrentYear", model.Seat_IQA_PercCompleted_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD_PreviousYear", model.Seat_IQA_AvgSigma_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Target", model.Seat_IQA_AvgSigma_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q1", model.Seat_IQA_AvgSigma_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q2", model.Seat_IQA_AvgSigma_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q3", model.Seat_IQA_AvgSigma_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q4", model.Seat_IQA_AvgSigma_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD_CurrentYear", model.Seat_IQA_AvgSigma_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@AddedBy", model.AddedBy),
                new SqlParameter("@AddedOn", model.AddedOn)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Insert_AHPNote " +
                    "@FinancialYear, " +
                    "@Lit_SEE_Engagement_Target, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q1, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q2, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q3, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q4, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q1, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q2, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q3, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q4, " +
                    "@Lit_SEE_Effectiveness_Target, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q1, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q2, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q3, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q4, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q1, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q2, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q3, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q4, " +
                    "@Lit_SS_ServiceComplaints_Baseline, " +
                    "@Lit_SS_ServiceComplaints_Target, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q1, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q2, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q3, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q4, " +
                    "@Lit_SS_DesignLSG_Baseline, " +
                    "@Lit_SS_DesignLSG_Target, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q1, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q2, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q3, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q4, " +
                    "@Lit_SS_CostReduction_Baseline, " +
                    "@Lit_SS_CostReduction_Target, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q1, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q2, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q3, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q4, " +
                    "@Lit_SS_OTIF_Baseline, " +
                    "@Lit_SS_OTIF_Target, " +
                    "@Lit_SS_OTIF_CurrentYear_Q1, " +
                    "@Lit_SS_OTIF_CurrentYear_Q2, " +
                    "@Lit_SS_OTIF_CurrentYear_Q3, " +
                    "@Lit_SS_OTIF_CurrentYear_Q4, " +
                    "@Lit_SS_SPMScore_Baseline, " +
                    "@Lit_SS_SPMScore_Target, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q1, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q2, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q3, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q4, " +
                    "@Lit_CSAT_ReqSent_YTD_PreviousYear, " +
                    "@Lit_CSAT_ReqSent_Baseline, " +
                    "@Lit_CSAT_ReqSent_Target, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q1, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q2, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q3, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q4, " +
                    "@Lit_CSAT_ReqSent_YTD_CurrentYear, " +
                    "@Lit_CSAT_RespRecvd_YTD_PreviousYear, " +
                    "@Lit_CSAT_RespRecvd_Baseline, " +
                    "@Lit_CSAT_RespRecvd_Target, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q1, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q2, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q3, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q4, " +
                    "@Lit_CSAT_RespRecvd_YTD_CurrentYear, " +
                    "@Lit_CSAT_Promoter_YTD_PreviousYear, " +
                    "@Lit_CSAT_Promoter_Baseline, " +
                    "@Lit_CSAT_Promoter_Target, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q1, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q2, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q3, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q4, " +
                    "@Lit_CSAT_Promoter_YTD_CurrentYear, " +
                    "@Lit_CSAT_Detractor_YTD_PreviousYear, " +
                    "@Lit_CSAT_Detractor_Baseline, " +
                    "@Lit_CSAT_Detractor_Target, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q1, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q2, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q3, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q4, " +
                    "@Lit_CSAT_Detractor_YTD_CurrentYear, " +
                    "@Lit_CSAT_NPS_YTD_PreviousYear, " +
                    "@Lit_CSAT_NPS_Baseline, " +
                    "@Lit_CSAT_NPS_Target, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q1, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q2, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q3, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q4, " +
                    "@Lit_CSAT_NPS_YTD_CurrentYear, " +
                    "@Lit_SPM_Supp1, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp2, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp3, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp4, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp5, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp6, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp7, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp8, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp9, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp10, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q4, " +
                    "@Lit_OTIF_YTD_PreviousYear, " +
                    "@Lit_OTIF_Target, " +
                    "@Lit_OTIF_CurrentYear_Q1, " +
                    "@Lit_OTIF_CurrentYear_Q2, " +
                    "@Lit_OTIF_CurrentYear_Q3, " +
                    "@Lit_OTIF_CurrentYear_Q4, " +
                    "@Lit_OTIF_YTD_CurrentYear, " +
                    "@Lit_SC_Closure_YTD_PreviousYear, " +
                    "@Lit_SC_Closure_Baseline, " +
                    "@Lit_SC_Closure_Target, " +
                    "@Lit_SC_Closure_CurrentYear_Q1, " +
                    "@Lit_SC_Closure_CurrentYear_Q2, " +
                    "@Lit_SC_Closure_CurrentYear_Q3, " +
                    "@Lit_SC_Closure_CurrentYear_Q4, " +
                    "@Lit_SC_Closure_YTD_CurrentYear, " +
                    "@Lit_CSO_TotalLogged_YTD_PreviousYear, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q1, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q2, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q3, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q4, " +
                    "@Lit_CSO_TotalLogged_YTD_CurrentYear, " +
                    "@Lit_CSO_TotalAClass_YTD_PreviousYear, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q1, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q2, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q3, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q4, " +
                    "@Lit_CSO_TotalAClass_YTD_CurrentYear, " +
                    "@Lit_CSO_AClassClosed_YTD_PreviousYear, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q1, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q2, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q3, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q4, " +
                    "@Lit_CSO_AClassClosed_YTD_CurrentYear, " +
                    "@Lit_CSO_AClassClosedLess45_YTD_PreviousYear, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q1, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q2, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q3, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q4, " +
                    "@Lit_CSO_AClassClosedLess45_YTD_CurrentYear, " +
                    "@Lit_CSO_PercentageClosure_YTD_PreviousYear, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q1, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q2, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q3, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q4, " +
                    "@Lit_CSO_PercentageClosure_YTD_CurrentYear, " +
                    "@Lit_CostSavings_YTD_PreviousYear, " +
                    "@Lit_CostSavings_Target, " +
                    "@Lit_CostSavings_CurrentYear_Q1, " +
                    "@Lit_CostSavings_CurrentYear_Q2, " +
                    "@Lit_CostSavings_CurrentYear_Q3, " +
                    "@Lit_CostSavings_CurrentYear_Q4, " +
                    "@Lit_CostSavings_YTD_CurrentYear, " +
                    "@Lit_OQL_Vendor1, " +
                    "@Lit_OQL_Vendor1_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor1_Target, " +
                    "@Lit_OQL_Vendor1_PC01, " +
                    "@Lit_OQL_Vendor1_PC02, " +
                    "@Lit_OQL_Vendor1_PC03, " +
                    "@Lit_OQL_Vendor1_PC04, " +
                    "@Lit_OQL_Vendor1_PC05, " +
                    "@Lit_OQL_Vendor1_PC06, " +
                    "@Lit_OQL_Vendor2, " +
                    "@Lit_OQL_Vendor2_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor2_Target, " +
                    "@Lit_OQL_Vendor2_PC01, " +
                    "@Lit_OQL_Vendor2_PC02, " +
                    "@Lit_OQL_Vendor2_PC03, " +
                    "@Lit_OQL_Vendor2_PC04, " +
                    "@Lit_OQL_Vendor2_PC05, " +
                    "@Lit_OQL_Vendor2_PC06, " +
                    "@Lit_OQL_Vendor3, " +
                    "@Lit_OQL_Vendor3_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor3_Target, " +
                    "@Lit_OQL_Vendor3_PC01, " +
                    "@Lit_OQL_Vendor3_PC02, " +
                    "@Lit_OQL_Vendor3_PC03, " +
                    "@Lit_OQL_Vendor3_PC04, " +
                    "@Lit_OQL_Vendor3_PC05, " +
                    "@Lit_OQL_Vendor3_PC06, " +
                    "@Lit_OQL_Vendor4, " +
                    "@Lit_OQL_Vendor4_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor4_Target, " +
                    "@Lit_OQL_Vendor4_PC01, " +
                    "@Lit_OQL_Vendor4_PC02, " +
                    "@Lit_OQL_Vendor4_PC03, " +
                    "@Lit_OQL_Vendor4_PC04, " +
                    "@Lit_OQL_Vendor4_PC05, " +
                    "@Lit_OQL_Vendor4_PC06, " +
                    "@Lit_OQL_Vendor5, " +
                    "@Lit_OQL_Vendor5_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor5_Target, " +
                    "@Lit_OQL_Vendor5_PC01, " +
                    "@Lit_OQL_Vendor5_PC02, " +
                    "@Lit_OQL_Vendor5_PC03, " +
                    "@Lit_OQL_Vendor5_PC04, " +
                    "@Lit_OQL_Vendor5_PC05, " +
                    "@Lit_OQL_Vendor5_PC06, " +
                    "@Lit_OQL_Vendor6, " +
                    "@Lit_OQL_Vendor6_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor6_Target, " +
                    "@Lit_OQL_Vendor6_PC01, " +
                    "@Lit_OQL_Vendor6_PC02, " +
                    "@Lit_OQL_Vendor6_PC03, " +
                    "@Lit_OQL_Vendor6_PC04, " +
                    "@Lit_OQL_Vendor6_PC05, " +
                    "@Lit_OQL_Vendor6_PC06, " +
                    "@Lit_OQL_Vendor7, " +
                    "@Lit_OQL_Vendor7_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor7_Target, " +
                    "@Lit_OQL_Vendor7_PC01, " +
                    "@Lit_OQL_Vendor7_PC02, " +
                    "@Lit_OQL_Vendor7_PC03, " +
                    "@Lit_OQL_Vendor7_PC04, " +
                    "@Lit_OQL_Vendor7_PC05, " +
                    "@Lit_OQL_Vendor7_PC06, " +
                    "@Lit_OQL_Vendor8, " +
                    "@Lit_OQL_Vendor8_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor8_Target, " +
                    "@Lit_OQL_Vendor8_PC01, " +
                    "@Lit_OQL_Vendor8_PC02, " +
                    "@Lit_OQL_Vendor8_PC03, " +
                    "@Lit_OQL_Vendor8_PC04, " +
                    "@Lit_OQL_Vendor8_PC05, " +
                    "@Lit_OQL_Vendor8_PC06, " +
                    "@Lit_OQL_CumulativeAvg1_YTD_PreviousYear, " +
                    "@Lit_OQL_CumulativeAvg1_Target, " +
                    "@Lit_OQL_CumulativeAvg1_PC01, " +
                    "@Lit_OQL_CumulativeAvg1_PC02, " +
                    "@Lit_OQL_CumulativeAvg1_PC03, " +
                    "@Lit_OQL_CumulativeAvg1_PC04, " +
                    "@Lit_OQL_CumulativeAvg1_PC05, " +
                    "@Lit_OQL_CumulativeAvg1_PC06, " +
                    "@Lit_OQL_CumulativeAvg2_YTD_PreviousYear, " +
                    "@Lit_OQL_CumulativeAvg2_Target, " +
                    "@Lit_OQL_CumulativeAvg2_PC01, " +
                    "@Lit_OQL_CumulativeAvg2_PC02, " +
                    "@Lit_OQL_CumulativeAvg2_PC03, " +
                    "@Lit_OQL_CumulativeAvg2_PC04, " +
                    "@Lit_OQL_CumulativeAvg2_PC05, " +
                    "@Lit_OQL_CumulativeAvg2_PC06, " +
                    "@Seat_SS_CNClosure_Baseline, " +
                    "@Seat_SS_CNClosure_Target, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q1, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q2, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q3, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q4, " +
                    "@Seat_SS_CNClosure_YTD_CurrentYear, " +
                    "@Seat_SS_OTIF_Baseline, " +
                    "@Seat_SS_OTIF_Target, " +
                    "@Seat_SS_OTIF_CurrentYear_Q1, " +
                    "@Seat_SS_OTIF_CurrentYear_Q2, " +
                    "@Seat_SS_OTIF_CurrentYear_Q3, " +
                    "@Seat_SS_OTIF_CurrentYear_Q4, " +
                    "@Seat_SS_OTIF_YTD_CurrentYear, " +
                    "@Seat_SS_SPMScore_Baseline, " +
                    "@Seat_SS_SPMScore_Target, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q1, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q2, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q3, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q4, " +
                    "@Seat_SS_SPMScore_YTD_CurrentYear, " +
                    "@Seat_OTIF_Performance_YTD_PreviousYear, " +
                    "@Seat_OTIF_Performance_Target, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q1, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q2, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q3, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q4, " +
                    "@Seat_OTIF_Performance_YTD_CurrentYear, " +
                    "@Seat_CSAT_ReqSent_YTD_PreviousYear, " +
                    "@Seat_CSAT_ReqSent_Baseline, " +
                    "@Seat_CSAT_ReqSent_Target, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q1, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q2, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q3, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q4, " +
                    "@Seat_CSAT_ReqSent_YTD_CurrentYear, " +
                    "@Seat_CSAT_RespRecvd_YTD_PreviousYear, " +
                    "@Seat_CSAT_RespRecvd_Baseline, " +
                    "@Seat_CSAT_RespRecvd_Target, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q1, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q2, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q3, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q4, " +
                    "@Seat_CSAT_RespRecvd_YTD_CurrentYear, " +
                    "@Seat_CSAT_Promoter_YTD_PreviousYear, " +
                    "@Seat_CSAT_Promoter_Baseline, " +
                    "@Seat_CSAT_Promoter_Target, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q1, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q2, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q3, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q4, " +
                    "@Seat_CSAT_Promoter_YTD_CurrentYear, " +
                    "@Seat_CSAT_Detractor_YTD_PreviousYear, " +
                    "@Seat_CSAT_Detractor_Baseline, " +
                    "@Seat_CSAT_Detractor_Target, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q1, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q2, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q3, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q4, " +
                    "@Seat_CSAT_Detractor_YTD_CurrentYear, " +
                    "@Seat_CSAT_NPS_YTD_PreviousYear, " +
                    "@Seat_CSAT_NPS_Baseline, " +
                    "@Seat_CSAT_NPS_Target, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q1, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q2, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q3, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q4, " +
                    "@Seat_CSAT_NPS_YTD_CurrentYear, " +
                    "@Seat_CSO_TotalLogged_YTD_PreviousYear, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q1, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q2, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q3, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q4, " +
                    "@Seat_CSO_TotalLogged_YTD_CurrentYear, " +
                    "@Seat_CSO_TotalAClass_YTD_PreviousYear, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q1, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q2, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q3, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q4, " +
                    "@Seat_CSO_TotalAClass_YTD_CurrentYear, " +
                    "@Seat_CSO_AClassClosed_YTD_PreviousYear, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q1, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q2, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q3, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q4, " +
                    "@Seat_CSO_AClassClosed_YTD_CurrentYear, " +
                    "@Seat_CSO_AClassClosedLess45_YTD_PreviousYear, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q1, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q2, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q3, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q4, " +
                    "@Seat_CSO_AClassClosedLess45_YTD_CurrentYear, " +
                    "@Seat_CSO_AClassClosedUnder45_YTD_PreviousYear, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q1, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q2, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q3, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q4, " +
                    "@Seat_CSO_AClassClosedUnder45_YTD_CurrentYear, " +
                    "@Seat_SPM_Supp1, " +
                    "@Seat_SPM_Supp1_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp2, " +
                    "@Seat_SPM_Supp2_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp3, " +
                    "@Seat_SPM_Supp3_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp4, " +
                    "@Seat_SPM_Supp4_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp5, " +
                    "@Seat_SPM_Supp5_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q4, " +
                    "@Seat_IQA_TotalSites_YTD_PreviousYear, " +
                    "@Seat_IQA_TotalSites_Target, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q1, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q2, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q3, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q4, " +
                    "@Seat_IQA_TotalSites_YTD_CurrentYear, " +
                    "@Seat_IQA_SitesCompleted_YTD_PreviousYear, " +
                    "@Seat_IQA_SitesCompleted_Target, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q1, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q2, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q3, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q4, " +
                    "@Seat_IQA_SitesCompleted_YTD_CurrentYear, " +
                    "@Seat_IQA_AuditsCompleted_YTD_PreviousYear, " +
                    "@Seat_IQA_AuditsCompleted_Target, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q1, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q2, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q3, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q4, " +
                    "@Seat_IQA_AuditsCompleted_YTD_CurrentYear, " +
                    "@Seat_IQA_PercCompleted_YTD_PreviousYear, " +
                    "@Seat_IQA_PercCompleted_Target, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q1, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q2, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q3, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q4, " +
                    "@Seat_IQA_PercCompleted_YTD_CurrentYear, " +
                    "@Seat_IQA_AvgSigma_YTD_PreviousYear, " +
                    "@Seat_IQA_AvgSigma_Target, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q1, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q2, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q3, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q4, " +
                    "@Seat_IQA_AvgSigma_YTD_CurrentYear, " +
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
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q1", model.Lit_SEE_Engagement_PreviousYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q2", model.Lit_SEE_Engagement_PreviousYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q3", model.Lit_SEE_Engagement_PreviousYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_PreviousYear_Q4", model.Lit_SEE_Engagement_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q1", model.Lit_SEE_Engagement_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q2", model.Lit_SEE_Engagement_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q3", model.Lit_SEE_Engagement_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Engagement_CurrentYear_Q4", model.Lit_SEE_Engagement_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_Target", model.Lit_SEE_Effectiveness_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q1", model.Lit_SEE_Effectiveness_PreviousYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q2", model.Lit_SEE_Effectiveness_PreviousYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q3", model.Lit_SEE_Effectiveness_PreviousYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_PreviousYear_Q4", model.Lit_SEE_Effectiveness_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q1", model.Lit_SEE_Effectiveness_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q2", model.Lit_SEE_Effectiveness_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q3", model.Lit_SEE_Effectiveness_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SEE_Effectiveness_CurrentYear_Q4", model.Lit_SEE_Effectiveness_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Baseline", model.Lit_SS_ServiceComplaints_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_Target", model.Lit_SS_ServiceComplaints_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q1", model.Lit_SS_ServiceComplaints_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q2", model.Lit_SS_ServiceComplaints_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q3", model.Lit_SS_ServiceComplaints_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_ServiceComplaints_CurrentYear_Q4", model.Lit_SS_ServiceComplaints_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Baseline", model.Lit_SS_DesignLSG_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_Target", model.Lit_SS_DesignLSG_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q1", model.Lit_SS_DesignLSG_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q2", model.Lit_SS_DesignLSG_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q3", model.Lit_SS_DesignLSG_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_DesignLSG_CurrentYear_Q4", model.Lit_SS_DesignLSG_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Baseline", model.Lit_SS_CostReduction_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_Target", model.Lit_SS_CostReduction_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q1", model.Lit_SS_CostReduction_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q2", model.Lit_SS_CostReduction_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q3", model.Lit_SS_CostReduction_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_CostReduction_CurrentYear_Q4", model.Lit_SS_CostReduction_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Baseline", model.Lit_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_Target", model.Lit_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q1", model.Lit_SS_OTIF_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q2", model.Lit_SS_OTIF_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q3", model.Lit_SS_OTIF_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_OTIF_CurrentYear_Q4", model.Lit_SS_OTIF_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Baseline", model.Lit_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_Target", model.Lit_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q1", model.Lit_SS_SPMScore_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q2", model.Lit_SS_SPMScore_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q3", model.Lit_SS_SPMScore_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SS_SPMScore_CurrentYear_Q4", model.Lit_SS_SPMScore_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD_PreviousYear", model.Lit_CSAT_ReqSent_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Baseline", model.Lit_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_Target", model.Lit_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q1", model.Lit_CSAT_ReqSent_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q2", model.Lit_CSAT_ReqSent_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q3", model.Lit_CSAT_ReqSent_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_CurrentYear_Q4", model.Lit_CSAT_ReqSent_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_ReqSent_YTD_CurrentYear", model.Lit_CSAT_ReqSent_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD_PreviousYear", model.Lit_CSAT_RespRecvd_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Baseline", model.Lit_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_Target", model.Lit_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q1", model.Lit_CSAT_RespRecvd_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q2", model.Lit_CSAT_RespRecvd_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q3", model.Lit_CSAT_RespRecvd_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_CurrentYear_Q4", model.Lit_CSAT_RespRecvd_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_RespRecvd_YTD_CurrentYear", model.Lit_CSAT_RespRecvd_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD_PreviousYear", model.Lit_CSAT_Promoter_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Baseline", model.Lit_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_Target", model.Lit_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q1", model.Lit_CSAT_Promoter_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q2", model.Lit_CSAT_Promoter_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q3", model.Lit_CSAT_Promoter_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_CurrentYear_Q4", model.Lit_CSAT_Promoter_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Promoter_YTD_CurrentYear", model.Lit_CSAT_Promoter_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD_PreviousYear", model.Lit_CSAT_Detractor_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Baseline", model.Lit_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_Target", model.Lit_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q1", model.Lit_CSAT_Detractor_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q2", model.Lit_CSAT_Detractor_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q3", model.Lit_CSAT_Detractor_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_CurrentYear_Q4", model.Lit_CSAT_Detractor_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_Detractor_YTD_CurrentYear", model.Lit_CSAT_Detractor_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD_PreviousYear", model.Lit_CSAT_NPS_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Baseline", model.Lit_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_Target", model.Lit_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q1", model.Lit_CSAT_NPS_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q2", model.Lit_CSAT_NPS_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q3", model.Lit_CSAT_NPS_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_CurrentYear_Q4", model.Lit_CSAT_NPS_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSAT_NPS_YTD_CurrentYear", model.Lit_CSAT_NPS_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1", model.Lit_SPM_Supp1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q1", model.Lit_SPM_Supp1_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q2", model.Lit_SPM_Supp1_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q3", model.Lit_SPM_Supp1_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp1_CurrentYear_Q4", model.Lit_SPM_Supp1_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2", model.Lit_SPM_Supp2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q1", model.Lit_SPM_Supp2_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q2", model.Lit_SPM_Supp2_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q3", model.Lit_SPM_Supp2_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp2_CurrentYear_Q4", model.Lit_SPM_Supp2_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3", model.Lit_SPM_Supp3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q1", model.Lit_SPM_Supp3_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q2", model.Lit_SPM_Supp3_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q3", model.Lit_SPM_Supp3_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp3_CurrentYear_Q4", model.Lit_SPM_Supp3_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4", model.Lit_SPM_Supp4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q1", model.Lit_SPM_Supp4_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q2", model.Lit_SPM_Supp4_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q3", model.Lit_SPM_Supp4_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp4_CurrentYear_Q4", model.Lit_SPM_Supp4_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5", model.Lit_SPM_Supp5 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q1", model.Lit_SPM_Supp5_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q2", model.Lit_SPM_Supp5_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q3", model.Lit_SPM_Supp5_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp5_CurrentYear_Q4", model.Lit_SPM_Supp5_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6", model.Lit_SPM_Supp6 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q1", model.Lit_SPM_Supp6_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q2", model.Lit_SPM_Supp6_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q3", model.Lit_SPM_Supp6_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp6_CurrentYear_Q4", model.Lit_SPM_Supp6_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7", model.Lit_SPM_Supp7 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q1", model.Lit_SPM_Supp7_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q2", model.Lit_SPM_Supp7_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q3", model.Lit_SPM_Supp7_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp7_CurrentYear_Q4", model.Lit_SPM_Supp7_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8", model.Lit_SPM_Supp8 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q1", model.Lit_SPM_Supp8_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q2", model.Lit_SPM_Supp8_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q3", model.Lit_SPM_Supp8_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp8_CurrentYear_Q4", model.Lit_SPM_Supp8_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9", model.Lit_SPM_Supp9 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q1", model.Lit_SPM_Supp9_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q2", model.Lit_SPM_Supp9_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q3", model.Lit_SPM_Supp9_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp9_CurrentYear_Q4", model.Lit_SPM_Supp9_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10", model.Lit_SPM_Supp10 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q1", model.Lit_SPM_Supp10_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q2", model.Lit_SPM_Supp10_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q3", model.Lit_SPM_Supp10_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SPM_Supp10_CurrentYear_Q4", model.Lit_SPM_Supp10_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD_PreviousYear", model.Lit_OTIF_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_Target", model.Lit_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q1", model.Lit_OTIF_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q2", model.Lit_OTIF_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q3", model.Lit_OTIF_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_CurrentYear_Q4", model.Lit_OTIF_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OTIF_YTD_CurrentYear", model.Lit_OTIF_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD_PreviousYear", model.Lit_SC_Closure_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Baseline", model.Lit_SC_Closure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_Target", model.Lit_SC_Closure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q1", model.Lit_SC_Closure_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q2", model.Lit_SC_Closure_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q3", model.Lit_SC_Closure_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_CurrentYear_Q4", model.Lit_SC_Closure_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_SC_Closure_YTD_CurrentYear", model.Lit_SC_Closure_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD_PreviousYear", model.Lit_CSO_TotalLogged_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q1", model.Lit_CSO_TotalLogged_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q2", model.Lit_CSO_TotalLogged_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q3", model.Lit_CSO_TotalLogged_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_CurrentYear_Q4", model.Lit_CSO_TotalLogged_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalLogged_YTD_CurrentYear", model.Lit_CSO_TotalLogged_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD_PreviousYear", model.Lit_CSO_TotalAClass_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q1", model.Lit_CSO_TotalAClass_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q2", model.Lit_CSO_TotalAClass_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q3", model.Lit_CSO_TotalAClass_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_CurrentYear_Q4", model.Lit_CSO_TotalAClass_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_TotalAClass_YTD_CurrentYear", model.Lit_CSO_TotalAClass_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD_PreviousYear", model.Lit_CSO_AClassClosed_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q1", model.Lit_CSO_AClassClosed_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q2", model.Lit_CSO_AClassClosed_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q3", model.Lit_CSO_AClassClosed_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_CurrentYear_Q4", model.Lit_CSO_AClassClosed_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosed_YTD_CurrentYear", model.Lit_CSO_AClassClosed_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD_PreviousYear", model.Lit_CSO_AClassClosedLess45_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q1", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q2", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q3", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_CurrentYear_Q4", model.Lit_CSO_AClassClosedLess45_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_AClassClosedLess45_YTD_CurrentYear", model.Lit_CSO_AClassClosedLess45_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD_PreviousYear", model.Lit_CSO_PercentageClosure_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q1", model.Lit_CSO_PercentageClosure_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q2", model.Lit_CSO_PercentageClosure_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q3", model.Lit_CSO_PercentageClosure_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_CurrentYear_Q4", model.Lit_CSO_PercentageClosure_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CSO_PercentageClosure_YTD_CurrentYear", model.Lit_CSO_PercentageClosure_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD_PreviousYear", model.Lit_CostSavings_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_Target", model.Lit_CostSavings_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q1", model.Lit_CostSavings_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q2", model.Lit_CostSavings_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q3", model.Lit_CostSavings_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_CurrentYear_Q4", model.Lit_CostSavings_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_CostSavings_YTD_CurrentYear", model.Lit_CostSavings_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1", model.Lit_OQL_Vendor1 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_YTD_PreviousYear", model.Lit_OQL_Vendor1_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_Target", model.Lit_OQL_Vendor1_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC01", model.Lit_OQL_Vendor1_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC02", model.Lit_OQL_Vendor1_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC03", model.Lit_OQL_Vendor1_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC04", model.Lit_OQL_Vendor1_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC05", model.Lit_OQL_Vendor1_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor1_PC06", model.Lit_OQL_Vendor1_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2", model.Lit_OQL_Vendor2 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_YTD_PreviousYear", model.Lit_OQL_Vendor2_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_Target", model.Lit_OQL_Vendor2_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC01", model.Lit_OQL_Vendor2_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC02", model.Lit_OQL_Vendor2_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC03", model.Lit_OQL_Vendor2_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC04", model.Lit_OQL_Vendor2_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC05", model.Lit_OQL_Vendor2_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor2_PC06", model.Lit_OQL_Vendor2_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3", model.Lit_OQL_Vendor3 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_YTD_PreviousYear", model.Lit_OQL_Vendor3_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_Target", model.Lit_OQL_Vendor3_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC01", model.Lit_OQL_Vendor3_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC02", model.Lit_OQL_Vendor3_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC03", model.Lit_OQL_Vendor3_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC04", model.Lit_OQL_Vendor3_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC05", model.Lit_OQL_Vendor3_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor3_PC06", model.Lit_OQL_Vendor3_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4", model.Lit_OQL_Vendor4 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_YTD_PreviousYear", model.Lit_OQL_Vendor4_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_Target", model.Lit_OQL_Vendor4_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC01", model.Lit_OQL_Vendor4_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC02", model.Lit_OQL_Vendor4_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC03", model.Lit_OQL_Vendor4_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC04", model.Lit_OQL_Vendor4_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC05", model.Lit_OQL_Vendor4_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor4_PC06", model.Lit_OQL_Vendor4_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5", model.Lit_OQL_Vendor5 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_YTD_PreviousYear", model.Lit_OQL_Vendor5_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_Target", model.Lit_OQL_Vendor5_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC01", model.Lit_OQL_Vendor5_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC02", model.Lit_OQL_Vendor5_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC03", model.Lit_OQL_Vendor5_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC04", model.Lit_OQL_Vendor5_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC05", model.Lit_OQL_Vendor5_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor5_PC06", model.Lit_OQL_Vendor5_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6", model.Lit_OQL_Vendor6 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_YTD_PreviousYear", model.Lit_OQL_Vendor6_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_Target", model.Lit_OQL_Vendor6_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC01", model.Lit_OQL_Vendor6_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC02", model.Lit_OQL_Vendor6_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC03", model.Lit_OQL_Vendor6_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC04", model.Lit_OQL_Vendor6_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC05", model.Lit_OQL_Vendor6_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor6_PC06", model.Lit_OQL_Vendor6_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7", model.Lit_OQL_Vendor7 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_YTD_PreviousYear", model.Lit_OQL_Vendor7_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_Target", model.Lit_OQL_Vendor7_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC01", model.Lit_OQL_Vendor7_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC02", model.Lit_OQL_Vendor7_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC03", model.Lit_OQL_Vendor7_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC04", model.Lit_OQL_Vendor7_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC05", model.Lit_OQL_Vendor7_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor7_PC06", model.Lit_OQL_Vendor7_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8", model.Lit_OQL_Vendor8 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_YTD_PreviousYear", model.Lit_OQL_Vendor8_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_Target", model.Lit_OQL_Vendor8_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC01", model.Lit_OQL_Vendor8_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC02", model.Lit_OQL_Vendor8_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC03", model.Lit_OQL_Vendor8_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC04", model.Lit_OQL_Vendor8_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC05", model.Lit_OQL_Vendor8_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_Vendor8_PC06", model.Lit_OQL_Vendor8_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_YTD_PreviousYear", model.Lit_OQL_CumulativeAvg1_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_Target", model.Lit_OQL_CumulativeAvg1_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC01", model.Lit_OQL_CumulativeAvg1_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC02", model.Lit_OQL_CumulativeAvg1_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC03", model.Lit_OQL_CumulativeAvg1_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC04", model.Lit_OQL_CumulativeAvg1_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC05", model.Lit_OQL_CumulativeAvg1_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg1_PC06", model.Lit_OQL_CumulativeAvg1_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_YTD_PreviousYear", model.Lit_OQL_CumulativeAvg2_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_Target", model.Lit_OQL_CumulativeAvg2_Target ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC01", model.Lit_OQL_CumulativeAvg2_PC01 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC02", model.Lit_OQL_CumulativeAvg2_PC02 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC03", model.Lit_OQL_CumulativeAvg2_PC03 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC04", model.Lit_OQL_CumulativeAvg2_PC04 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC05", model.Lit_OQL_CumulativeAvg2_PC05 ?? (object)DBNull.Value),
                new SqlParameter("@Lit_OQL_CumulativeAvg2_PC06", model.Lit_OQL_CumulativeAvg2_PC06 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Baseline", model.Seat_SS_CNClosure_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_Target", model.Seat_SS_CNClosure_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q1", model.Seat_SS_CNClosure_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q2", model.Seat_SS_CNClosure_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q3", model.Seat_SS_CNClosure_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_CurrentYear_Q4", model.Seat_SS_CNClosure_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_CNClosure_YTD_CurrentYear", model.Seat_SS_CNClosure_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Baseline", model.Seat_SS_OTIF_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_Target", model.Seat_SS_OTIF_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q1", model.Seat_SS_OTIF_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q2", model.Seat_SS_OTIF_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q3", model.Seat_SS_OTIF_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_CurrentYear_Q4", model.Seat_SS_OTIF_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_OTIF_YTD_CurrentYear", model.Seat_SS_OTIF_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Baseline", model.Seat_SS_SPMScore_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_Target", model.Seat_SS_SPMScore_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q1", model.Seat_SS_SPMScore_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q2", model.Seat_SS_SPMScore_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q3", model.Seat_SS_SPMScore_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_CurrentYear_Q4", model.Seat_SS_SPMScore_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SS_SPMScore_YTD_CurrentYear", model.Seat_SS_SPMScore_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD_PreviousYear", model.Seat_OTIF_Performance_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_Target", model.Seat_OTIF_Performance_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q1", model.Seat_OTIF_Performance_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q2", model.Seat_OTIF_Performance_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q3", model.Seat_OTIF_Performance_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_CurrentYear_Q4", model.Seat_OTIF_Performance_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_OTIF_Performance_YTD_CurrentYear", model.Seat_OTIF_Performance_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD_PreviousYear", model.Seat_CSAT_ReqSent_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Baseline", model.Seat_CSAT_ReqSent_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_Target", model.Seat_CSAT_ReqSent_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q1", model.Seat_CSAT_ReqSent_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q2", model.Seat_CSAT_ReqSent_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q3", model.Seat_CSAT_ReqSent_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_CurrentYear_Q4", model.Seat_CSAT_ReqSent_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_ReqSent_YTD_CurrentYear", model.Seat_CSAT_ReqSent_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD_PreviousYear", model.Seat_CSAT_RespRecvd_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Baseline", model.Seat_CSAT_RespRecvd_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_Target", model.Seat_CSAT_RespRecvd_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q1", model.Seat_CSAT_RespRecvd_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q2", model.Seat_CSAT_RespRecvd_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q3", model.Seat_CSAT_RespRecvd_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_CurrentYear_Q4", model.Seat_CSAT_RespRecvd_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_RespRecvd_YTD_CurrentYear", model.Seat_CSAT_RespRecvd_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD_PreviousYear", model.Seat_CSAT_Promoter_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Baseline", model.Seat_CSAT_Promoter_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_Target", model.Seat_CSAT_Promoter_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q1", model.Seat_CSAT_Promoter_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q2", model.Seat_CSAT_Promoter_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q3", model.Seat_CSAT_Promoter_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_CurrentYear_Q4", model.Seat_CSAT_Promoter_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Promoter_YTD_CurrentYear", model.Seat_CSAT_Promoter_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD_PreviousYear", model.Seat_CSAT_Detractor_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Baseline", model.Seat_CSAT_Detractor_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_Target", model.Seat_CSAT_Detractor_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q1", model.Seat_CSAT_Detractor_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q2", model.Seat_CSAT_Detractor_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q3", model.Seat_CSAT_Detractor_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_CurrentYear_Q4", model.Seat_CSAT_Detractor_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_Detractor_YTD_CurrentYear", model.Seat_CSAT_Detractor_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD_PreviousYear", model.Seat_CSAT_NPS_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Baseline", model.Seat_CSAT_NPS_Baseline ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_Target", model.Seat_CSAT_NPS_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q1", model.Seat_CSAT_NPS_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q2", model.Seat_CSAT_NPS_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q3", model.Seat_CSAT_NPS_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_CurrentYear_Q4", model.Seat_CSAT_NPS_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSAT_NPS_YTD_CurrentYear", model.Seat_CSAT_NPS_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD_PreviousYear", model.Seat_CSO_TotalLogged_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q1", model.Seat_CSO_TotalLogged_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q2", model.Seat_CSO_TotalLogged_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q3", model.Seat_CSO_TotalLogged_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_CurrentYear_Q4", model.Seat_CSO_TotalLogged_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalLogged_YTD_CurrentYear", model.Seat_CSO_TotalLogged_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD_PreviousYear", model.Seat_CSO_TotalAClass_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q1", model.Seat_CSO_TotalAClass_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q2", model.Seat_CSO_TotalAClass_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q3", model.Seat_CSO_TotalAClass_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_CurrentYear_Q4", model.Seat_CSO_TotalAClass_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_TotalAClass_YTD_CurrentYear", model.Seat_CSO_TotalAClass_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD_PreviousYear", model.Seat_CSO_AClassClosed_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q1", model.Seat_CSO_AClassClosed_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q2", model.Seat_CSO_AClassClosed_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q3", model.Seat_CSO_AClassClosed_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_CurrentYear_Q4", model.Seat_CSO_AClassClosed_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosed_YTD_CurrentYear", model.Seat_CSO_AClassClosed_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_YTD_PreviousYear", model.Seat_CSO_AClassClosedLess45_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q1", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q2", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q3", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_CurrentYear_Q4", model.Seat_CSO_AClassClosedLess45_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedLess45_YTD_CurrentYear", model.Seat_CSO_AClassClosedLess45_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_YTD_PreviousYear", model.Seat_CSO_AClassClosedUnder45_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q1", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q2", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q3", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_CurrentYear_Q4", model.Seat_CSO_AClassClosedUnder45_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_CSO_AClassClosedUnder45_YTD_CurrentYear", model.Seat_CSO_AClassClosedUnder45_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1", model.Seat_SPM_Supp1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_PreviousYear_Q4", model.Seat_SPM_Supp1_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q1", model.Seat_SPM_Supp1_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q2", model.Seat_SPM_Supp1_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q3", model.Seat_SPM_Supp1_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp1_CurrentYear_Q4", model.Seat_SPM_Supp1_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2", model.Seat_SPM_Supp2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_PreviousYear_Q4", model.Seat_SPM_Supp2_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q1", model.Seat_SPM_Supp2_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q2", model.Seat_SPM_Supp2_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q3", model.Seat_SPM_Supp2_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp2_CurrentYear_Q4", model.Seat_SPM_Supp2_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3", model.Seat_SPM_Supp3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_PreviousYear_Q4", model.Seat_SPM_Supp3_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q1", model.Seat_SPM_Supp3_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q2", model.Seat_SPM_Supp3_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q3", model.Seat_SPM_Supp3_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp3_CurrentYear_Q4", model.Seat_SPM_Supp3_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4", model.Seat_SPM_Supp4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_PreviousYear_Q4", model.Seat_SPM_Supp4_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q1", model.Seat_SPM_Supp4_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q2", model.Seat_SPM_Supp4_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q3", model.Seat_SPM_Supp4_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp4_CurrentYear_Q4", model.Seat_SPM_Supp4_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5", model.Seat_SPM_Supp5 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_PreviousYear_Q4", model.Seat_SPM_Supp5_PreviousYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q1", model.Seat_SPM_Supp5_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q2", model.Seat_SPM_Supp5_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q3", model.Seat_SPM_Supp5_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_SPM_Supp5_CurrentYear_Q4", model.Seat_SPM_Supp5_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD_PreviousYear", model.Seat_IQA_TotalSites_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_Target", model.Seat_IQA_TotalSites_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q1", model.Seat_IQA_TotalSites_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q2", model.Seat_IQA_TotalSites_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q3", model.Seat_IQA_TotalSites_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_CurrentYear_Q4", model.Seat_IQA_TotalSites_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_TotalSites_YTD_CurrentYear", model.Seat_IQA_TotalSites_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD_PreviousYear", model.Seat_IQA_SitesCompleted_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_Target", model.Seat_IQA_SitesCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q1", model.Seat_IQA_SitesCompleted_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q2", model.Seat_IQA_SitesCompleted_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q3", model.Seat_IQA_SitesCompleted_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_CurrentYear_Q4", model.Seat_IQA_SitesCompleted_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_SitesCompleted_YTD_CurrentYear", model.Seat_IQA_SitesCompleted_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD_PreviousYear", model.Seat_IQA_AuditsCompleted_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_Target", model.Seat_IQA_AuditsCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q1", model.Seat_IQA_AuditsCompleted_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q2", model.Seat_IQA_AuditsCompleted_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q3", model.Seat_IQA_AuditsCompleted_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_CurrentYear_Q4", model.Seat_IQA_AuditsCompleted_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AuditsCompleted_YTD_CurrentYear", model.Seat_IQA_AuditsCompleted_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD_PreviousYear", model.Seat_IQA_PercCompleted_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_Target", model.Seat_IQA_PercCompleted_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q1", model.Seat_IQA_PercCompleted_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q2", model.Seat_IQA_PercCompleted_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q3", model.Seat_IQA_PercCompleted_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_CurrentYear_Q4", model.Seat_IQA_PercCompleted_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_PercCompleted_YTD_CurrentYear", model.Seat_IQA_PercCompleted_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD_PreviousYear", model.Seat_IQA_AvgSigma_YTD_PreviousYear ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_Target", model.Seat_IQA_AvgSigma_Target ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q1", model.Seat_IQA_AvgSigma_CurrentYear_Q1 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q2", model.Seat_IQA_AvgSigma_CurrentYear_Q2 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q3", model.Seat_IQA_AvgSigma_CurrentYear_Q3 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_CurrentYear_Q4", model.Seat_IQA_AvgSigma_CurrentYear_Q4 ?? (object)DBNull.Value),
                new SqlParameter("@Seat_IQA_AvgSigma_YTD_CurrentYear", model.Seat_IQA_AvgSigma_YTD_CurrentYear ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", model.UpdatedBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedOn", model.UpdatedOn ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_Update_AHPNote " +
                    "@Id, " +
                    "@Lit_SEE_Engagement_Target, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q1, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q2, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q3, " +
                    "@Lit_SEE_Engagement_PreviousYear_Q4, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q1, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q2, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q3, " +
                    "@Lit_SEE_Engagement_CurrentYear_Q4, " +
                    "@Lit_SEE_Effectiveness_Target, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q1, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q2, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q3, " +
                    "@Lit_SEE_Effectiveness_PreviousYear_Q4, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q1, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q2, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q3, " +
                    "@Lit_SEE_Effectiveness_CurrentYear_Q4, " +
                    "@Lit_SS_ServiceComplaints_Baseline, " +
                    "@Lit_SS_ServiceComplaints_Target, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q1, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q2, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q3, " +
                    "@Lit_SS_ServiceComplaints_CurrentYear_Q4, " +
                    "@Lit_SS_DesignLSG_Baseline, " +
                    "@Lit_SS_DesignLSG_Target, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q1, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q2, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q3, " +
                    "@Lit_SS_DesignLSG_CurrentYear_Q4, " +
                    "@Lit_SS_CostReduction_Baseline, " +
                    "@Lit_SS_CostReduction_Target, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q1, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q2, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q3, " +
                    "@Lit_SS_CostReduction_CurrentYear_Q4, " +
                    "@Lit_SS_OTIF_Baseline, " +
                    "@Lit_SS_OTIF_Target, " +
                    "@Lit_SS_OTIF_CurrentYear_Q1, " +
                    "@Lit_SS_OTIF_CurrentYear_Q2, " +
                    "@Lit_SS_OTIF_CurrentYear_Q3, " +
                    "@Lit_SS_OTIF_CurrentYear_Q4, " +
                    "@Lit_SS_SPMScore_Baseline, " +
                    "@Lit_SS_SPMScore_Target, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q1, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q2, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q3, " +
                    "@Lit_SS_SPMScore_CurrentYear_Q4, " +
                    "@Lit_CSAT_ReqSent_YTD_PreviousYear, " +
                    "@Lit_CSAT_ReqSent_Baseline, " +
                    "@Lit_CSAT_ReqSent_Target, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q1, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q2, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q3, " +
                    "@Lit_CSAT_ReqSent_CurrentYear_Q4, " +
                    "@Lit_CSAT_ReqSent_YTD_CurrentYear, " +
                    "@Lit_CSAT_RespRecvd_YTD_PreviousYear, " +
                    "@Lit_CSAT_RespRecvd_Baseline, " +
                    "@Lit_CSAT_RespRecvd_Target, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q1, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q2, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q3, " +
                    "@Lit_CSAT_RespRecvd_CurrentYear_Q4, " +
                    "@Lit_CSAT_RespRecvd_YTD_CurrentYear, " +
                    "@Lit_CSAT_Promoter_YTD_PreviousYear, " +
                    "@Lit_CSAT_Promoter_Baseline, " +
                    "@Lit_CSAT_Promoter_Target, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q1, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q2, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q3, " +
                    "@Lit_CSAT_Promoter_CurrentYear_Q4, " +
                    "@Lit_CSAT_Promoter_YTD_CurrentYear, " +
                    "@Lit_CSAT_Detractor_YTD_PreviousYear, " +
                    "@Lit_CSAT_Detractor_Baseline, " +
                    "@Lit_CSAT_Detractor_Target, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q1, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q2, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q3, " +
                    "@Lit_CSAT_Detractor_CurrentYear_Q4, " +
                    "@Lit_CSAT_Detractor_YTD_CurrentYear, " +
                    "@Lit_CSAT_NPS_YTD_PreviousYear, " +
                    "@Lit_CSAT_NPS_Baseline, " +
                    "@Lit_CSAT_NPS_Target, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q1, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q2, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q3, " +
                    "@Lit_CSAT_NPS_CurrentYear_Q4, " +
                    "@Lit_CSAT_NPS_YTD_CurrentYear, " +
                    "@Lit_SPM_Supp1, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp1_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp2, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp2_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp3, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp3_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp4, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp4_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp5, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp5_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp6, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp6_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp7, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp7_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp8, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp8_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp9, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp9_CurrentYear_Q4, " +
                    "@Lit_SPM_Supp10, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q1, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q2, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q3, " +
                    "@Lit_SPM_Supp10_CurrentYear_Q4, " +
                    "@Lit_OTIF_YTD_PreviousYear, " +
                    "@Lit_OTIF_Target, " +
                    "@Lit_OTIF_CurrentYear_Q1, " +
                    "@Lit_OTIF_CurrentYear_Q2, " +
                    "@Lit_OTIF_CurrentYear_Q3, " +
                    "@Lit_OTIF_CurrentYear_Q4, " +
                    "@Lit_OTIF_YTD_CurrentYear, " +
                    "@Lit_SC_Closure_YTD_PreviousYear, " +
                    "@Lit_SC_Closure_Baseline, " +
                    "@Lit_SC_Closure_Target, " +
                    "@Lit_SC_Closure_CurrentYear_Q1, " +
                    "@Lit_SC_Closure_CurrentYear_Q2, " +
                    "@Lit_SC_Closure_CurrentYear_Q3, " +
                    "@Lit_SC_Closure_CurrentYear_Q4, " +
                    "@Lit_SC_Closure_YTD_CurrentYear, " +
                    "@Lit_CSO_TotalLogged_YTD_PreviousYear, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q1, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q2, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q3, " +
                    "@Lit_CSO_TotalLogged_CurrentYear_Q4, " +
                    "@Lit_CSO_TotalLogged_YTD_CurrentYear, " +
                    "@Lit_CSO_TotalAClass_YTD_PreviousYear, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q1, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q2, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q3, " +
                    "@Lit_CSO_TotalAClass_CurrentYear_Q4, " +
                    "@Lit_CSO_TotalAClass_YTD_CurrentYear, " +
                    "@Lit_CSO_AClassClosed_YTD_PreviousYear, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q1, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q2, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q3, " +
                    "@Lit_CSO_AClassClosed_CurrentYear_Q4, " +
                    "@Lit_CSO_AClassClosed_YTD_CurrentYear, " +
                    "@Lit_CSO_AClassClosedLess45_YTD_PreviousYear, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q1, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q2, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q3, " +
                    "@Lit_CSO_AClassClosedLess45_CurrentYear_Q4, " +
                    "@Lit_CSO_AClassClosedLess45_YTD_CurrentYear, " +
                    "@Lit_CSO_PercentageClosure_YTD_PreviousYear, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q1, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q2, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q3, " +
                    "@Lit_CSO_PercentageClosure_CurrentYear_Q4, " +
                    "@Lit_CSO_PercentageClosure_YTD_CurrentYear, " +
                    "@Lit_CostSavings_YTD_PreviousYear, " +
                    "@Lit_CostSavings_Target, " +
                    "@Lit_CostSavings_CurrentYear_Q1, " +
                    "@Lit_CostSavings_CurrentYear_Q2, " +
                    "@Lit_CostSavings_CurrentYear_Q3, " +
                    "@Lit_CostSavings_CurrentYear_Q4, " +
                    "@Lit_CostSavings_YTD_CurrentYear, " +
                    "@Lit_OQL_Vendor1, " +
                    "@Lit_OQL_Vendor1_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor1_Target, " +
                    "@Lit_OQL_Vendor1_PC01, " +
                    "@Lit_OQL_Vendor1_PC02, " +
                    "@Lit_OQL_Vendor1_PC03, " +
                    "@Lit_OQL_Vendor1_PC04, " +
                    "@Lit_OQL_Vendor1_PC05, " +
                    "@Lit_OQL_Vendor1_PC06, " +
                    "@Lit_OQL_Vendor2, " +
                    "@Lit_OQL_Vendor2_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor2_Target, " +
                    "@Lit_OQL_Vendor2_PC01, " +
                    "@Lit_OQL_Vendor2_PC02, " +
                    "@Lit_OQL_Vendor2_PC03, " +
                    "@Lit_OQL_Vendor2_PC04, " +
                    "@Lit_OQL_Vendor2_PC05, " +
                    "@Lit_OQL_Vendor2_PC06, " +
                    "@Lit_OQL_Vendor3, " +
                    "@Lit_OQL_Vendor3_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor3_Target, " +
                    "@Lit_OQL_Vendor3_PC01, " +
                    "@Lit_OQL_Vendor3_PC02, " +
                    "@Lit_OQL_Vendor3_PC03, " +
                    "@Lit_OQL_Vendor3_PC04, " +
                    "@Lit_OQL_Vendor3_PC05, " +
                    "@Lit_OQL_Vendor3_PC06, " +
                    "@Lit_OQL_Vendor4, " +
                    "@Lit_OQL_Vendor4_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor4_Target, " +
                    "@Lit_OQL_Vendor4_PC01, " +
                    "@Lit_OQL_Vendor4_PC02, " +
                    "@Lit_OQL_Vendor4_PC03, " +
                    "@Lit_OQL_Vendor4_PC04, " +
                    "@Lit_OQL_Vendor4_PC05, " +
                    "@Lit_OQL_Vendor4_PC06, " +
                    "@Lit_OQL_Vendor5, " +
                    "@Lit_OQL_Vendor5_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor5_Target, " +
                    "@Lit_OQL_Vendor5_PC01, " +
                    "@Lit_OQL_Vendor5_PC02, " +
                    "@Lit_OQL_Vendor5_PC03, " +
                    "@Lit_OQL_Vendor5_PC04, " +
                    "@Lit_OQL_Vendor5_PC05, " +
                    "@Lit_OQL_Vendor5_PC06, " +
                    "@Lit_OQL_Vendor6, " +
                    "@Lit_OQL_Vendor6_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor6_Target, " +
                    "@Lit_OQL_Vendor6_PC01, " +
                    "@Lit_OQL_Vendor6_PC02, " +
                    "@Lit_OQL_Vendor6_PC03, " +
                    "@Lit_OQL_Vendor6_PC04, " +
                    "@Lit_OQL_Vendor6_PC05, " +
                    "@Lit_OQL_Vendor6_PC06, " +
                    "@Lit_OQL_Vendor7, " +
                    "@Lit_OQL_Vendor7_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor7_Target, " +
                    "@Lit_OQL_Vendor7_PC01, " +
                    "@Lit_OQL_Vendor7_PC02, " +
                    "@Lit_OQL_Vendor7_PC03, " +
                    "@Lit_OQL_Vendor7_PC04, " +
                    "@Lit_OQL_Vendor7_PC05, " +
                    "@Lit_OQL_Vendor7_PC06, " +
                    "@Lit_OQL_Vendor8, " +
                    "@Lit_OQL_Vendor8_YTD_PreviousYear, " +
                    "@Lit_OQL_Vendor8_Target, " +
                    "@Lit_OQL_Vendor8_PC01, " +
                    "@Lit_OQL_Vendor8_PC02, " +
                    "@Lit_OQL_Vendor8_PC03, " +
                    "@Lit_OQL_Vendor8_PC04, " +
                    "@Lit_OQL_Vendor8_PC05, " +
                    "@Lit_OQL_Vendor8_PC06, " +
                    "@Lit_OQL_CumulativeAvg1_YTD_PreviousYear, " +
                    "@Lit_OQL_CumulativeAvg1_Target, " +
                    "@Lit_OQL_CumulativeAvg1_PC01, " +
                    "@Lit_OQL_CumulativeAvg1_PC02, " +
                    "@Lit_OQL_CumulativeAvg1_PC03, " +
                    "@Lit_OQL_CumulativeAvg1_PC04, " +
                    "@Lit_OQL_CumulativeAvg1_PC05, " +
                    "@Lit_OQL_CumulativeAvg1_PC06, " +
                    "@Lit_OQL_CumulativeAvg2_YTD_PreviousYear, " +
                    "@Lit_OQL_CumulativeAvg2_Target, " +
                    "@Lit_OQL_CumulativeAvg2_PC01, " +
                    "@Lit_OQL_CumulativeAvg2_PC02, " +
                    "@Lit_OQL_CumulativeAvg2_PC03, " +
                    "@Lit_OQL_CumulativeAvg2_PC04, " +
                    "@Lit_OQL_CumulativeAvg2_PC05, " +
                    "@Lit_OQL_CumulativeAvg2_PC06, " +
                    "@Seat_SS_CNClosure_Baseline, " +
                    "@Seat_SS_CNClosure_Target, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q1, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q2, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q3, " +
                    "@Seat_SS_CNClosure_CurrentYear_Q4, " +
                    "@Seat_SS_CNClosure_YTD_CurrentYear, " +
                    "@Seat_SS_OTIF_Baseline, " +
                    "@Seat_SS_OTIF_Target, " +
                    "@Seat_SS_OTIF_CurrentYear_Q1, " +
                    "@Seat_SS_OTIF_CurrentYear_Q2, " +
                    "@Seat_SS_OTIF_CurrentYear_Q3, " +
                    "@Seat_SS_OTIF_CurrentYear_Q4, " +
                    "@Seat_SS_OTIF_YTD_CurrentYear, " +
                    "@Seat_SS_SPMScore_Baseline, " +
                    "@Seat_SS_SPMScore_Target, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q1, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q2, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q3, " +
                    "@Seat_SS_SPMScore_CurrentYear_Q4, " +
                    "@Seat_SS_SPMScore_YTD_CurrentYear, " +
                    "@Seat_OTIF_Performance_YTD_PreviousYear, " +
                    "@Seat_OTIF_Performance_Target, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q1, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q2, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q3, " +
                    "@Seat_OTIF_Performance_CurrentYear_Q4, " +
                    "@Seat_OTIF_Performance_YTD_CurrentYear, " +
                    "@Seat_CSAT_ReqSent_YTD_PreviousYear, " +
                    "@Seat_CSAT_ReqSent_Baseline, " +
                    "@Seat_CSAT_ReqSent_Target, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q1, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q2, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q3, " +
                    "@Seat_CSAT_ReqSent_CurrentYear_Q4, " +
                    "@Seat_CSAT_ReqSent_YTD_CurrentYear, " +
                    "@Seat_CSAT_RespRecvd_YTD_PreviousYear, " +
                    "@Seat_CSAT_RespRecvd_Baseline, " +
                    "@Seat_CSAT_RespRecvd_Target, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q1, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q2, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q3, " +
                    "@Seat_CSAT_RespRecvd_CurrentYear_Q4, " +
                    "@Seat_CSAT_RespRecvd_YTD_CurrentYear, " +
                    "@Seat_CSAT_Promoter_YTD_PreviousYear, " +
                    "@Seat_CSAT_Promoter_Baseline, " +
                    "@Seat_CSAT_Promoter_Target, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q1, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q2, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q3, " +
                    "@Seat_CSAT_Promoter_CurrentYear_Q4, " +
                    "@Seat_CSAT_Promoter_YTD_CurrentYear, " +
                    "@Seat_CSAT_Detractor_YTD_PreviousYear, " +
                    "@Seat_CSAT_Detractor_Baseline, " +
                    "@Seat_CSAT_Detractor_Target, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q1, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q2, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q3, " +
                    "@Seat_CSAT_Detractor_CurrentYear_Q4, " +
                    "@Seat_CSAT_Detractor_YTD_CurrentYear, " +
                    "@Seat_CSAT_NPS_YTD_PreviousYear, " +
                    "@Seat_CSAT_NPS_Baseline, " +
                    "@Seat_CSAT_NPS_Target, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q1, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q2, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q3, " +
                    "@Seat_CSAT_NPS_CurrentYear_Q4, " +
                    "@Seat_CSAT_NPS_YTD_CurrentYear, " +
                    "@Seat_CSO_TotalLogged_YTD_PreviousYear, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q1, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q2, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q3, " +
                    "@Seat_CSO_TotalLogged_CurrentYear_Q4, " +
                    "@Seat_CSO_TotalLogged_YTD_CurrentYear, " +
                    "@Seat_CSO_TotalAClass_YTD_PreviousYear, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q1, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q2, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q3, " +
                    "@Seat_CSO_TotalAClass_CurrentYear_Q4, " +
                    "@Seat_CSO_TotalAClass_YTD_CurrentYear, " +
                    "@Seat_CSO_AClassClosed_YTD_PreviousYear, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q1, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q2, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q3, " +
                    "@Seat_CSO_AClassClosed_CurrentYear_Q4, " +
                    "@Seat_CSO_AClassClosed_YTD_CurrentYear, " +
                    "@Seat_CSO_AClassClosedLess45_YTD_PreviousYear, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q1, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q2, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q3, " +
                    "@Seat_CSO_AClassClosedLess45_CurrentYear_Q4, " +
                    "@Seat_CSO_AClassClosedLess45_YTD_CurrentYear, " +
                    "@Seat_CSO_AClassClosedUnder45_YTD_PreviousYear, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q1, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q2, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q3, " +
                    "@Seat_CSO_AClassClosedUnder45_CurrentYear_Q4, " +
                    "@Seat_CSO_AClassClosedUnder45_YTD_CurrentYear, " +
                    "@Seat_SPM_Supp1, " +
                    "@Seat_SPM_Supp1_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp1_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp2, " +
                    "@Seat_SPM_Supp2_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp2_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp3, " +
                    "@Seat_SPM_Supp3_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp3_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp4, " +
                    "@Seat_SPM_Supp4_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp4_CurrentYear_Q4, " +
                    "@Seat_SPM_Supp5, " +
                    "@Seat_SPM_Supp5_PreviousYear_Q4, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q1, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q2, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q3, " +
                    "@Seat_SPM_Supp5_CurrentYear_Q4, " +
                    "@Seat_IQA_TotalSites_YTD_PreviousYear, " +
                    "@Seat_IQA_TotalSites_Target, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q1, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q2, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q3, " +
                    "@Seat_IQA_TotalSites_CurrentYear_Q4, " +
                    "@Seat_IQA_TotalSites_YTD_CurrentYear, " +
                    "@Seat_IQA_SitesCompleted_YTD_PreviousYear, " +
                    "@Seat_IQA_SitesCompleted_Target, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q1, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q2, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q3, " +
                    "@Seat_IQA_SitesCompleted_CurrentYear_Q4, " +
                    "@Seat_IQA_SitesCompleted_YTD_CurrentYear, " +
                    "@Seat_IQA_AuditsCompleted_YTD_PreviousYear, " +
                    "@Seat_IQA_AuditsCompleted_Target, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q1, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q2, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q3, " +
                    "@Seat_IQA_AuditsCompleted_CurrentYear_Q4, " +
                    "@Seat_IQA_AuditsCompleted_YTD_CurrentYear, " +
                    "@Seat_IQA_PercCompleted_YTD_PreviousYear, " +
                    "@Seat_IQA_PercCompleted_Target, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q1, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q2, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q3, " +
                    "@Seat_IQA_PercCompleted_CurrentYear_Q4, " +
                    "@Seat_IQA_PercCompleted_YTD_CurrentYear, " +
                    "@Seat_IQA_AvgSigma_YTD_PreviousYear, " +
                    "@Seat_IQA_AvgSigma_Target, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q1, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q2, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q3, " +
                    "@Seat_IQA_AvgSigma_CurrentYear_Q4, " +
                    "@Seat_IQA_AvgSigma_YTD_CurrentYear, " +
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
