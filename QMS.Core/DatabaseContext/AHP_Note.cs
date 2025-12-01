using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_AHP_Note")]
public class AHP_Note : SqlTable
{
    #region SECTION 1: CIB LIGHTING (Lit)

    // 1.1 SEE Metrics
    public string? Lit_SEE_Engagement_Target { get; set; }
    public string? Lit_SEE_Engagement_FY23_24_Q1 { get; set; }
    public string? Lit_SEE_Engagement_FY23_24_Q2 { get; set; }
    public string? Lit_SEE_Engagement_FY23_24_Q3 { get; set; }
    public string? Lit_SEE_Engagement_FY23_24_Q4 { get; set; }
    public string? Lit_SEE_Engagement_FY24_25_Q1 { get; set; }
    public string? Lit_SEE_Engagement_FY24_25_Q2 { get; set; }

    public string? Lit_SEE_Effectiveness_Target { get; set; }
    public string? Lit_SEE_Effectiveness_FY23_24_Q1 { get; set; }
    public string? Lit_SEE_Effectiveness_FY23_24_Q2 { get; set; }
    public string? Lit_SEE_Effectiveness_FY23_24_Q3 { get; set; }
    public string? Lit_SEE_Effectiveness_FY23_24_Q4 { get; set; }
    public string? Lit_SEE_Effectiveness_FY24_25_Q1 { get; set; }
    public string? Lit_SEE_Effectiveness_FY24_25_Q2 { get; set; }

    // 1.2 Six Sigma Projects
    public string? Lit_SS_ServiceComplaints_Baseline { get; set; }
    public string? Lit_SS_ServiceComplaints_Target { get; set; }
    public string? Lit_SS_ServiceComplaints_Q1 { get; set; }
    public string? Lit_SS_ServiceComplaints_Q2 { get; set; }

    public string? Lit_SS_DesignLSG_Baseline { get; set; }
    public string? Lit_SS_DesignLSG_Target { get; set; }
    public string? Lit_SS_DesignLSG_Q1 { get; set; }
    public string? Lit_SS_DesignLSG_Q2 { get; set; }

    public string? Lit_SS_CostReduction_Baseline { get; set; }
    public string? Lit_SS_CostReduction_Target { get; set; }
    public string? Lit_SS_CostReduction_Q1 { get; set; }
    public string? Lit_SS_CostReduction_Q2 { get; set; }

    public string? Lit_SS_OTIF_Baseline { get; set; }
    public string? Lit_SS_OTIF_Target { get; set; }
    public string? Lit_SS_OTIF_Q1 { get; set; }
    public string? Lit_SS_OTIF_Q2 { get; set; }

    public string? Lit_SS_SPMScore_Baseline { get; set; }
    public string? Lit_SS_SPMScore_Target { get; set; }
    public string? Lit_SS_SPMScore_Q1 { get; set; }
    public string? Lit_SS_SPMScore_Q2 { get; set; }

    // 1.3 CSAT
    public string? Lit_CSAT_ReqSent_YTD23_24 { get; set; }
    public string? Lit_CSAT_ReqSent_Baseline { get; set; }
    public string? Lit_CSAT_ReqSent_Target { get; set; }
    public string? Lit_CSAT_ReqSent_Q1 { get; set; }
    public string? Lit_CSAT_ReqSent_Q2 { get; set; }
    public string? Lit_CSAT_ReqSent_YTD { get; set; }

    public string? Lit_CSAT_RespRecvd_YTD23_24 { get; set; }
    public string? Lit_CSAT_RespRecvd_Baseline { get; set; }
    public string? Lit_CSAT_RespRecvd_Target { get; set; }
    public string? Lit_CSAT_RespRecvd_Q1 { get; set; }
    public string? Lit_CSAT_RespRecvd_Q2 { get; set; }
    public string? Lit_CSAT_RespRecvd_YTD { get; set; }

    public string? Lit_CSAT_Promoter_YTD23_24 { get; set; }
    public string? Lit_CSAT_Promoter_Baseline { get; set; }
    public string? Lit_CSAT_Promoter_Target { get; set; }
    public string? Lit_CSAT_Promoter_Q1 { get; set; }
    public string? Lit_CSAT_Promoter_Q2 { get; set; }
    public string? Lit_CSAT_Promoter_YTD { get; set; }

    public string? Lit_CSAT_Detractor_YTD23_24 { get; set; }
    public string? Lit_CSAT_Detractor_Baseline { get; set; }
    public string? Lit_CSAT_Detractor_Target { get; set; }
    public string? Lit_CSAT_Detractor_Q1 { get; set; }
    public string? Lit_CSAT_Detractor_Q2 { get; set; }
    public string? Lit_CSAT_Detractor_YTD { get; set; }

    public string? Lit_CSAT_NPS_YTD23_24 { get; set; }
    public string? Lit_CSAT_NPS_Baseline { get; set; }
    public string? Lit_CSAT_NPS_Target { get; set; }
    public string? Lit_CSAT_NPS_Q1 { get; set; }
    public string? Lit_CSAT_NPS_Q2 { get; set; }
    public string? Lit_CSAT_NPS_YTD { get; set; }

    // 1.4 SPM Score
    public string? Lit_SPM_Supp1_Q1 { get; set; }
    public string? Lit_SPM_Supp1_Q2 { get; set; }
    public string? Lit_SPM_Supp2_Q1 { get; set; }
    public string? Lit_SPM_Supp2_Q2 { get; set; }
    public string? Lit_SPM_Supp3_Q1 { get; set; }
    public string? Lit_SPM_Supp3_Q2 { get; set; }
    public string? Lit_SPM_Supp4_Q1 { get; set; }
    public string? Lit_SPM_Supp4_Q2 { get; set; }
    public string? Lit_SPM_Supp5_Q1 { get; set; }
    public string? Lit_SPM_Supp5_Q2 { get; set; }
    public string? Lit_SPM_Supp6_Q1 { get; set; }
    public string? Lit_SPM_Supp6_Q2 { get; set; }
    public string? Lit_SPM_Supp7_Q1 { get; set; }
    public string? Lit_SPM_Supp7_Q2 { get; set; }
    public string? Lit_SPM_Supp8_Q1 { get; set; }
    public string? Lit_SPM_Supp8_Q2 { get; set; }
    public string? Lit_SPM_Supp9_Q1 { get; set; }
    public string? Lit_SPM_Supp9_Q2 { get; set; }
    public string? Lit_SPM_Supp10_Q1 { get; set; }
    public string? Lit_SPM_Supp10_Q2 { get; set; }

    // 1.5 OTIF
    public string? Lit_OTIF_YTD23_24 { get; set; }
    public string? Lit_OTIF_Target { get; set; }
    public string? Lit_OTIF_Q1 { get; set; }
    public string? Lit_OTIF_Q2 { get; set; }
    public string? Lit_OTIF_YTD { get; set; }

    // 1.6 Service Complaints Closure
    public string? Lit_SC_Closure_YTD23_24 { get; set; }
    public string? Lit_SC_Closure_Baseline { get; set; }
    public string? Lit_SC_Closure_Target { get; set; }
    public string? Lit_SC_Closure_Q1 { get; set; }
    public string? Lit_SC_Closure_Q2 { get; set; }
    public string? Lit_SC_Closure_YTD { get; set; }

    // 1.7 CSO
    public string? Lit_CSO_TotalLogged_YTD23_24 { get; set; }
    public string? Lit_CSO_TotalLogged_Q1 { get; set; }
    public string? Lit_CSO_TotalLogged_Q2 { get; set; }
    public string? Lit_CSO_TotalLogged_YTD { get; set; }
    public string? Lit_CSO_TotalAClass_YTD23_24 { get; set; }
    public string? Lit_CSO_TotalAClass_Q1 { get; set; }
    public string? Lit_CSO_TotalAClass_Q2 { get; set; }
    public string? Lit_CSO_TotalAClass_YTD { get; set; }
    public string? Lit_CSO_AClassClosed_YTD23_24 { get; set; }
    public string? Lit_CSO_AClassClosed_Q1 { get; set; }
    public string? Lit_CSO_AClassClosed_Q2 { get; set; }
    public string? Lit_CSO_AClassClosed_YTD { get; set; }
    public string? Lit_CSO_AClassClosedLess45_YTD23_24 { get; set; }
    public string? Lit_CSO_AClassClosedLess45_Q1 { get; set; }
    public string? Lit_CSO_AClassClosedLess45_Q2 { get; set; }
    public string? Lit_CSO_AClassClosedLess45_YTD { get; set; }
    public string? Lit_CSO_PercentageClosure_YTD23_24 { get; set; }
    public string? Lit_CSO_PercentageClosure_Q1 { get; set; }
    public string? Lit_CSO_PercentageClosure_Q2 { get; set; }
    public string? Lit_CSO_PercentageClosure_YTD { get; set; }

    // 1.8 Cost Savings
    public string? Lit_CostSavings_YTD23_24 { get; set; }
    public string? Lit_CostSavings_Target { get; set; }
    public string? Lit_CostSavings_Q1 { get; set; }
    public string? Lit_CostSavings_Q2 { get; set; }
    public string? Lit_CostSavings_YTD { get; set; }

    // 1.9 Outgoing Quality Level
    public string? Lit_OQL_ArtLuminaires_YTD23_24 { get; set; }
    public string? Lit_OQL_ArtLuminaires_Target { get; set; }
    public string? Lit_OQL_ArtLuminaires_PC01 { get; set; }
    public string? Lit_OQL_ArtLuminaires_PC02 { get; set; }
    public string? Lit_OQL_ArtLuminaires_PC03 { get; set; }
    public string? Lit_OQL_ArtLuminaires_PC04 { get; set; }
    public string? Lit_OQL_ArtLuminaires_PC05 { get; set; }
    public string? Lit_OQL_ArtLuminaires_PC06 { get; set; }

    public string? Lit_OQL_IdealLighting_YTD23_24 { get; set; }
    public string? Lit_OQL_IdealLighting_Target { get; set; }
    public string? Lit_OQL_IdealLighting_PC01 { get; set; }
    public string? Lit_OQL_IdealLighting_PC02 { get; set; }
    public string? Lit_OQL_IdealLighting_PC03 { get; set; }
    public string? Lit_OQL_IdealLighting_PC04 { get; set; }
    public string? Lit_OQL_IdealLighting_PC05 { get; set; }
    public string? Lit_OQL_IdealLighting_PC06 { get; set; }

    public string? Lit_OQL_Everlite_YTD23_24 { get; set; }
    public string? Lit_OQL_Everlite_Target { get; set; }
    public string? Lit_OQL_Everlite_PC01 { get; set; }
    public string? Lit_OQL_Everlite_PC02 { get; set; }
    public string? Lit_OQL_Everlite_PC03 { get; set; }
    public string? Lit_OQL_Everlite_PC04 { get; set; }
    public string? Lit_OQL_Everlite_PC05 { get; set; }
    public string? Lit_OQL_Everlite_PC06 { get; set; }

    public string? Lit_OQL_Rama_YTD23_24 { get; set; }
    public string? Lit_OQL_Rama_Target { get; set; }
    public string? Lit_OQL_Rama_PC01 { get; set; }
    public string? Lit_OQL_Rama_PC02 { get; set; }
    public string? Lit_OQL_Rama_PC03 { get; set; }
    public string? Lit_OQL_Rama_PC04 { get; set; }
    public string? Lit_OQL_Rama_PC05 { get; set; }
    public string? Lit_OQL_Rama_PC06 { get; set; }

    public string? Lit_OQL_Shantinath_YTD23_24 { get; set; }
    public string? Lit_OQL_Shantinath_Target { get; set; }
    public string? Lit_OQL_Shantinath_PC01 { get; set; }
    public string? Lit_OQL_Shantinath_PC02 { get; set; }
    public string? Lit_OQL_Shantinath_PC03 { get; set; }
    public string? Lit_OQL_Shantinath_PC04 { get; set; }
    public string? Lit_OQL_Shantinath_PC05 { get; set; }
    public string? Lit_OQL_Shantinath_PC06 { get; set; }

    public string? Lit_OQL_Varun_YTD23_24 { get; set; }
    public string? Lit_OQL_Varun_Target { get; set; }
    public string? Lit_OQL_Varun_PC01 { get; set; }
    public string? Lit_OQL_Varun_PC02 { get; set; }
    public string? Lit_OQL_Varun_PC03 { get; set; }
    public string? Lit_OQL_Varun_PC04 { get; set; }
    public string? Lit_OQL_Varun_PC05 { get; set; }
    public string? Lit_OQL_Varun_PC06 { get; set; }

    public string? Lit_OQL_Ujas_YTD23_24 { get; set; }
    public string? Lit_OQL_Ujas_Target { get; set; }
    public string? Lit_OQL_Ujas_PC01 { get; set; }
    public string? Lit_OQL_Ujas_PC02 { get; set; }
    public string? Lit_OQL_Ujas_PC03 { get; set; }
    public string? Lit_OQL_Ujas_PC04 { get; set; }
    public string? Lit_OQL_Ujas_PC05 { get; set; }
    public string? Lit_OQL_Ujas_PC06 { get; set; }

    public string? Lit_OQL_NAK_YTD23_24 { get; set; }
    public string? Lit_OQL_NAK_Target { get; set; }
    public string? Lit_OQL_NAK_PC01 { get; set; }
    public string? Lit_OQL_NAK_PC02 { get; set; }
    public string? Lit_OQL_NAK_PC03 { get; set; }
    public string? Lit_OQL_NAK_PC04 { get; set; }
    public string? Lit_OQL_NAK_PC05 { get; set; }
    public string? Lit_OQL_NAK_PC06 { get; set; }

    public string? Lit_OQL_CumulativeAvg1_YTD23_24 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_Target { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC01 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC02 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC03 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC04 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC05 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC06 { get; set; }

    public string? Lit_OQL_CumulativeAvg2_YTD23_24 { get; set; }
    public string? Lit_OQL_CumulativeAvg2_Target { get; set; }
    public string? Lit_OQL_CumulativeAvg2_PC01 { get; set; }
    public string? Lit_OQL_CumulativeAvg2_PC02 { get; set; }
    public string? Lit_OQL_CumulativeAvg2_PC03 { get; set; }
    public string? Lit_OQL_CumulativeAvg2_PC04 { get; set; }
    public string? Lit_OQL_CumulativeAvg2_PC05 { get; set; }
    public string? Lit_OQL_CumulativeAvg2_PC06 { get; set; }
    #endregion

    #region SECTION 2: CIB SEATING (Seat)

    // 2.1 Six Sigma Projects
    public string? Seat_SS_CNClosure_Baseline { get; set; }
    public string? Seat_SS_CNClosure_Target { get; set; }
    public string? Seat_SS_CNClosure_Q1 { get; set; }
    public string? Seat_SS_CNClosure_Q2 { get; set; }
    public string? Seat_SS_CNClosure_YTD { get; set; }

    public string? Seat_SS_OTIF_Baseline { get; set; }
    public string? Seat_SS_OTIF_Target { get; set; }
    public string? Seat_SS_OTIF_Q1 { get; set; }
    public string? Seat_SS_OTIF_Q2 { get; set; }
    public string? Seat_SS_OTIF_YTD { get; set; }

    public string? Seat_SS_SPMScore_Baseline { get; set; }
    public string? Seat_SS_SPMScore_Target { get; set; }
    public string? Seat_SS_SPMScore_Q1 { get; set; }
    public string? Seat_SS_SPMScore_Q2 { get; set; }
    public string? Seat_SS_SPMScore_YTD { get; set; }

    // 2.2 OTIF
    public string? Seat_OTIF_Performance_YTD23_24 { get; set; }
    public string? Seat_OTIF_Performance_Target { get; set; }
    public string? Seat_OTIF_Performance_Q1 { get; set; }
    public string? Seat_OTIF_Performance_Q2 { get; set; }
    public string? Seat_OTIF_Performance_YTD { get; set; }

    // 2.3 CSAT
    public string? Seat_CSAT_ReqSent_YTD23_24 { get; set; }
    public string? Seat_CSAT_ReqSent_Baseline { get; set; }
    public string? Seat_CSAT_ReqSent_Target { get; set; }
    public string? Seat_CSAT_ReqSent_Q1 { get; set; }
    public string? Seat_CSAT_ReqSent_Q2 { get; set; }
    public string? Seat_CSAT_ReqSent_YTD { get; set; }

    public string? Seat_CSAT_RespRecvd_YTD23_24 { get; set; }
    public string? Seat_CSAT_RespRecvd_Baseline { get; set; }
    public string? Seat_CSAT_RespRecvd_Target { get; set; }
    public string? Seat_CSAT_RespRecvd_Q1 { get; set; }
    public string? Seat_CSAT_RespRecvd_Q2 { get; set; }
    public string? Seat_CSAT_RespRecvd_YTD { get; set; }

    public string? Seat_CSAT_Promoter_YTD23_24 { get; set; }
    public string? Seat_CSAT_Promoter_Baseline { get; set; }
    public string? Seat_CSAT_Promoter_Target { get; set; }
    public string? Seat_CSAT_Promoter_Q1 { get; set; }
    public string? Seat_CSAT_Promoter_Q2 { get; set; }
    public string? Seat_CSAT_Promoter_YTD { get; set; }

    public string? Seat_CSAT_Detractor_YTD23_24 { get; set; }
    public string? Seat_CSAT_Detractor_Baseline { get; set; }
    public string? Seat_CSAT_Detractor_Target { get; set; }
    public string? Seat_CSAT_Detractor_Q1 { get; set; }
    public string? Seat_CSAT_Detractor_Q2 { get; set; }
    public string? Seat_CSAT_Detractor_YTD { get; set; }

    public string? Seat_CSAT_NPS_YTD23_24 { get; set; }
    public string? Seat_CSAT_NPS_Baseline { get; set; }
    public string? Seat_CSAT_NPS_Target { get; set; }
    public string? Seat_CSAT_NPS_Q1 { get; set; }
    public string? Seat_CSAT_NPS_Q2 { get; set; }
    public string? Seat_CSAT_NPS_YTD { get; set; }

    // 2.4 CSO
    public string? Seat_CSO_TotalLogged_YTD23_24 { get; set; }
    public string? Seat_CSO_TotalLogged_Q1 { get; set; }
    public string? Seat_CSO_TotalLogged_Q2 { get; set; }
    public string? Seat_CSO_TotalLogged_YTD { get; set; }

    public string? Seat_CSO_TotalAClass_YTD23_24 { get; set; }
    public string? Seat_CSO_TotalAClass_Q1 { get; set; }
    public string? Seat_CSO_TotalAClass_Q2 { get; set; }
    public string? Seat_CSO_TotalAClass_YTD { get; set; }

    public string? Seat_CSO_AClassClosed_YTD23_24 { get; set; }
    public string? Seat_CSO_AClassClosed_Q1 { get; set; }
    public string? Seat_CSO_AClassClosed_Q2 { get; set; }
    public string? Seat_CSO_AClassClosed_YTD { get; set; }

    public string? Seat_CSO_AClassClosedLess48_YTD23_24 { get; set; }
    public string? Seat_CSO_AClassClosedLess48_Q1 { get; set; }
    public string? Seat_CSO_AClassClosedLess48_Q2 { get; set; }
    public string? Seat_CSO_AClassClosedLess48_YTD { get; set; }

    public string? Seat_CSO_AClassClosedUnder48_YTD23_24 { get; set; }
    public string? Seat_CSO_AClassClosedUnder48_Q1 { get; set; }
    public string? Seat_CSO_AClassClosedUnder48_Q2 { get; set; }
    public string? Seat_CSO_AClassClosedUnder48_YTD { get; set; }

    // 2.5 SPM Score
    public string? Seat_SPM_MPPL_Q4_23_24 { get; set; }
    public string? Seat_SPM_MPPL_Q1_24_25 { get; set; }
    public string? Seat_SPM_MPPL_Q2_24_25 { get; set; }
    public string? Seat_SPM_EXCLUSIFF_Q4_23_24 { get; set; }
    public string? Seat_SPM_EXCLUSIFF_Q1_24_25 { get; set; }
    public string? Seat_SPM_EXCLUSIFF_Q2_24_25 { get; set; }
    public string? Seat_SPM_CVG_Q4_23_24 { get; set; }
    public string? Seat_SPM_CVG_Q1_24_25 { get; set; }
    public string? Seat_SPM_CVG_Q2_24_25 { get; set; }
    public string? Seat_SPM_STARSHINE_Q4_23_24 { get; set; }
    public string? Seat_SPM_STARSHINE_Q1_24_25 { get; set; }
    public string? Seat_SPM_STARSHINE_Q2_24_25 { get; set; }
    public string? Seat_SPM_SAVITON_Q4_23_24 { get; set; }
    public string? Seat_SPM_SAVITON_Q1_24_25 { get; set; }
    public string? Seat_SPM_SAVITON_Q2_24_25 { get; set; }

    // 2.6 IQA
    public string? Seat_IQA_TotalSites_YTD23_24 { get; set; }
    public string? Seat_IQA_TotalSites_Target { get; set; }
    public string? Seat_IQA_TotalSites_Q1 { get; set; }
    public string? Seat_IQA_TotalSites_Q2 { get; set; }
    public string? Seat_IQA_TotalSites_YTD { get; set; }

    public string? Seat_IQA_SitesCompleted_YTD23_24 { get; set; }
    public string? Seat_IQA_SitesCompleted_Target { get; set; }
    public string? Seat_IQA_SitesCompleted_Q1 { get; set; }
    public string? Seat_IQA_SitesCompleted_Q2 { get; set; }
    public string? Seat_IQA_SitesCompleted_YTD { get; set; }

    public string? Seat_IQA_AuditsCompleted_YTD23_24 { get; set; }
    public string? Seat_IQA_AuditsCompleted_Target { get; set; }
    public string? Seat_IQA_AuditsCompleted_Q1 { get; set; }
    public string? Seat_IQA_AuditsCompleted_Q2 { get; set; }
    public string? Seat_IQA_AuditsCompleted_YTD { get; set; }

    public string? Seat_IQA_PercCompleted_YTD23_24 { get; set; }
    public string? Seat_IQA_PercCompleted_Target { get; set; }
    public string? Seat_IQA_PercCompleted_Q1 { get; set; }
    public string? Seat_IQA_PercCompleted_Q2 { get; set; }
    public string? Seat_IQA_PercCompleted_YTD { get; set; }

    public string? Seat_IQA_AvgSigma_YTD23_24 { get; set; }
    public string? Seat_IQA_AvgSigma_Target { get; set; }
    public string? Seat_IQA_AvgSigma_Q1 { get; set; }
    public string? Seat_IQA_AvgSigma_Q2 { get; set; }
    public string? Seat_IQA_AvgSigma_YTD { get; set; }
    #endregion

    #region Audit Fields
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    #endregion
}
