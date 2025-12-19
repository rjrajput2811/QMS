using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_AHP_Note")]
public class AHP_Note : SqlTable
{
    public int FinancialYear { get; set; }

    #region SECTION 1: CIB LIGHTING (Lit)

    // 1.1 SEE Engagement
    public string? Lit_SEE_Engagement_Target { get; set; }
    public string? Lit_SEE_Engagement_PreviousYear_Q1 { get; set; }
    public string? Lit_SEE_Engagement_PreviousYear_Q2 { get; set; }
    public string? Lit_SEE_Engagement_PreviousYear_Q3 { get; set; }
    public string? Lit_SEE_Engagement_PreviousYear_Q4 { get; set; }
    public string? Lit_SEE_Engagement_CurrentYear_Q1 { get; set; }
    public string? Lit_SEE_Engagement_CurrentYear_Q2 { get; set; }
    public string? Lit_SEE_Engagement_CurrentYear_Q3 { get; set; }
    public string? Lit_SEE_Engagement_CurrentYear_Q4 { get; set; }

    // 1.1 SEE Effectiveness
    public string? Lit_SEE_Effectiveness_Target { get; set; }
    public string? Lit_SEE_Effectiveness_PreviousYear_Q1 { get; set; }
    public string? Lit_SEE_Effectiveness_PreviousYear_Q2 { get; set; }
    public string? Lit_SEE_Effectiveness_PreviousYear_Q3 { get; set; }
    public string? Lit_SEE_Effectiveness_PreviousYear_Q4 { get; set; }
    public string? Lit_SEE_Effectiveness_CurrentYear_Q1 { get; set; }
    public string? Lit_SEE_Effectiveness_CurrentYear_Q2 { get; set; }
    public string? Lit_SEE_Effectiveness_CurrentYear_Q3 { get; set; }
    public string? Lit_SEE_Effectiveness_CurrentYear_Q4 { get; set; }

    // 1.2 Six Sigma Projects
    public string? Lit_SS_ServiceComplaints_Baseline { get; set; }
    public string? Lit_SS_ServiceComplaints_Target { get; set; }
    public string? Lit_SS_ServiceComplaints_CurrentYear_Q1 { get; set; }
    public string? Lit_SS_ServiceComplaints_CurrentYear_Q2 { get; set; }
    public string? Lit_SS_ServiceComplaints_CurrentYear_Q3 { get; set; }
    public string? Lit_SS_ServiceComplaints_CurrentYear_Q4 { get; set; }

    public string? Lit_SS_DesignLSG_Baseline { get; set; }
    public string? Lit_SS_DesignLSG_Target { get; set; }
    public string? Lit_SS_DesignLSG_CurrentYear_Q1 { get; set; }
    public string? Lit_SS_DesignLSG_CurrentYear_Q2 { get; set; }
    public string? Lit_SS_DesignLSG_CurrentYear_Q3 { get; set; }
    public string? Lit_SS_DesignLSG_CurrentYear_Q4 { get; set; }

    public string? Lit_SS_CostReduction_Baseline { get; set; }
    public string? Lit_SS_CostReduction_Target { get; set; }
    public string? Lit_SS_CostReduction_CurrentYear_Q1 { get; set; }
    public string? Lit_SS_CostReduction_CurrentYear_Q2 { get; set; }
    public string? Lit_SS_CostReduction_CurrentYear_Q3 { get; set; }
    public string? Lit_SS_CostReduction_CurrentYear_Q4 { get; set; }

    public string? Lit_SS_OTIF_Baseline { get; set; }
    public string? Lit_SS_OTIF_Target { get; set; }
    public string? Lit_SS_OTIF_CurrentYear_Q1 { get; set; }
    public string? Lit_SS_OTIF_CurrentYear_Q2 { get; set; }
    public string? Lit_SS_OTIF_CurrentYear_Q3 { get; set; }
    public string? Lit_SS_OTIF_CurrentYear_Q4 { get; set; }

    public string? Lit_SS_SPMScore_Baseline { get; set; }
    public string? Lit_SS_SPMScore_Target { get; set; }
    public string? Lit_SS_SPMScore_CurrentYear_Q1 { get; set; }
    public string? Lit_SS_SPMScore_CurrentYear_Q2 { get; set; }
    public string? Lit_SS_SPMScore_CurrentYear_Q3 { get; set; }
    public string? Lit_SS_SPMScore_CurrentYear_Q4 { get; set; }

    // 1.3 CSAT
    public string? Lit_CSAT_ReqSent_YTD_PreviousYear { get; set; }
    public string? Lit_CSAT_ReqSent_Baseline { get; set; }
    public string? Lit_CSAT_ReqSent_Target { get; set; }
    public string? Lit_CSAT_ReqSent_CurrentYear_Q1 { get; set; }
    public string? Lit_CSAT_ReqSent_CurrentYear_Q2 { get; set; }
    public string? Lit_CSAT_ReqSent_CurrentYear_Q3 { get; set; }
    public string? Lit_CSAT_ReqSent_CurrentYear_Q4 { get; set; }
    public string? Lit_CSAT_ReqSent_YTD_CurrentYear { get; set; }

    public string? Lit_CSAT_RespRecvd_YTD_PreviousYear { get; set; }
    public string? Lit_CSAT_RespRecvd_Baseline { get; set; }
    public string? Lit_CSAT_RespRecvd_Target { get; set; }
    public string? Lit_CSAT_RespRecvd_CurrentYear_Q1 { get; set; }
    public string? Lit_CSAT_RespRecvd_CurrentYear_Q2 { get; set; }
    public string? Lit_CSAT_RespRecvd_CurrentYear_Q3 { get; set; }
    public string? Lit_CSAT_RespRecvd_CurrentYear_Q4 { get; set; }
    public string? Lit_CSAT_RespRecvd_YTD_CurrentYear { get; set; }

    public string? Lit_CSAT_Promoter_YTD_PreviousYear { get; set; }
    public string? Lit_CSAT_Promoter_Baseline { get; set; }
    public string? Lit_CSAT_Promoter_Target { get; set; }
    public string? Lit_CSAT_Promoter_CurrentYear_Q1 { get; set; }
    public string? Lit_CSAT_Promoter_CurrentYear_Q2 { get; set; }
    public string? Lit_CSAT_Promoter_CurrentYear_Q3 { get; set; }
    public string? Lit_CSAT_Promoter_CurrentYear_Q4 { get; set; }
    public string? Lit_CSAT_Promoter_YTD_CurrentYear { get; set; }

    public string? Lit_CSAT_Detractor_YTD_PreviousYear { get; set; }
    public string? Lit_CSAT_Detractor_Baseline { get; set; }
    public string? Lit_CSAT_Detractor_Target { get; set; }
    public string? Lit_CSAT_Detractor_CurrentYear_Q1 { get; set; }
    public string? Lit_CSAT_Detractor_CurrentYear_Q2 { get; set; }
    public string? Lit_CSAT_Detractor_CurrentYear_Q3 { get; set; }
    public string? Lit_CSAT_Detractor_CurrentYear_Q4 { get; set; }
    public string? Lit_CSAT_Detractor_YTD_CurrentYear { get; set; }

    public string? Lit_CSAT_NPS_YTD_PreviousYear { get; set; }
    public string? Lit_CSAT_NPS_Baseline { get; set; }
    public string? Lit_CSAT_NPS_Target { get; set; }
    public string? Lit_CSAT_NPS_CurrentYear_Q1 { get; set; }
    public string? Lit_CSAT_NPS_CurrentYear_Q2 { get; set; }
    public string? Lit_CSAT_NPS_CurrentYear_Q3 { get; set; }
    public string? Lit_CSAT_NPS_CurrentYear_Q4 { get; set; }
    public string? Lit_CSAT_NPS_YTD_CurrentYear { get; set; }

    // 1.4 SPM Score (Suppliers 1-10)
    public int? Lit_SPM_Supp1 { get; set; }
    public string? Lit_SPM_Supp1_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp1_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp1_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp1_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp2 { get; set; }
    public string? Lit_SPM_Supp2_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp2_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp2_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp2_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp3 { get; set; }
    public string? Lit_SPM_Supp3_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp3_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp3_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp3_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp4 { get; set; }
    public string? Lit_SPM_Supp4_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp4_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp4_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp4_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp5 { get; set; }
    public string? Lit_SPM_Supp5_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp5_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp5_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp5_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp6 { get; set; }
    public string? Lit_SPM_Supp6_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp6_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp6_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp6_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp7 { get; set; }
    public string? Lit_SPM_Supp7_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp7_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp7_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp7_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp8 { get; set; }
    public string? Lit_SPM_Supp8_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp8_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp8_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp8_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp9 { get; set; }
    public string? Lit_SPM_Supp9_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp9_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp9_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp9_CurrentYear_Q4 { get; set; }

    public int? Lit_SPM_Supp10 { get; set; }
    public string? Lit_SPM_Supp10_CurrentYear_Q1 { get; set; }
    public string? Lit_SPM_Supp10_CurrentYear_Q2 { get; set; }
    public string? Lit_SPM_Supp10_CurrentYear_Q3 { get; set; }
    public string? Lit_SPM_Supp10_CurrentYear_Q4 { get; set; }

    // 1.5 OTIF
    public string? Lit_OTIF_YTD_PreviousYear { get; set; }
    public string? Lit_OTIF_Target { get; set; }
    public string? Lit_OTIF_CurrentYear_Q1 { get; set; }
    public string? Lit_OTIF_CurrentYear_Q2 { get; set; }
    public string? Lit_OTIF_CurrentYear_Q3 { get; set; }
    public string? Lit_OTIF_CurrentYear_Q4 { get; set; }
    public string? Lit_OTIF_YTD_CurrentYear { get; set; }

    // 1.6 Service Complaints Closure
    public string? Lit_SC_Closure_YTD_PreviousYear { get; set; }
    public string? Lit_SC_Closure_Baseline { get; set; }
    public string? Lit_SC_Closure_Target { get; set; }
    public string? Lit_SC_Closure_CurrentYear_Q1 { get; set; }
    public string? Lit_SC_Closure_CurrentYear_Q2 { get; set; }
    public string? Lit_SC_Closure_CurrentYear_Q3 { get; set; }
    public string? Lit_SC_Closure_CurrentYear_Q4 { get; set; }
    public string? Lit_SC_Closure_YTD_CurrentYear { get; set; }

    // 1.7 CSO
    public string? Lit_CSO_TotalLogged_YTD_PreviousYear { get; set; }
    public string? Lit_CSO_TotalLogged_CurrentYear_Q1 { get; set; }
    public string? Lit_CSO_TotalLogged_CurrentYear_Q2 { get; set; }
    public string? Lit_CSO_TotalLogged_CurrentYear_Q3 { get; set; }
    public string? Lit_CSO_TotalLogged_CurrentYear_Q4 { get; set; }
    public string? Lit_CSO_TotalLogged_YTD_CurrentYear { get; set; }

    public string? Lit_CSO_TotalAClass_YTD_PreviousYear { get; set; }
    public string? Lit_CSO_TotalAClass_CurrentYear_Q1 { get; set; }
    public string? Lit_CSO_TotalAClass_CurrentYear_Q2 { get; set; }
    public string? Lit_CSO_TotalAClass_CurrentYear_Q3 { get; set; }
    public string? Lit_CSO_TotalAClass_CurrentYear_Q4 { get; set; }
    public string? Lit_CSO_TotalAClass_YTD_CurrentYear { get; set; }

    public string? Lit_CSO_AClassClosed_YTD_PreviousYear { get; set; }
    public string? Lit_CSO_AClassClosed_CurrentYear_Q1 { get; set; }
    public string? Lit_CSO_AClassClosed_CurrentYear_Q2 { get; set; }
    public string? Lit_CSO_AClassClosed_CurrentYear_Q3 { get; set; }
    public string? Lit_CSO_AClassClosed_CurrentYear_Q4 { get; set; }
    public string? Lit_CSO_AClassClosed_YTD_CurrentYear { get; set; }

    public string? Lit_CSO_AClassClosedLess45_YTD_PreviousYear { get; set; }
    public string? Lit_CSO_AClassClosedLess45_CurrentYear_Q1 { get; set; }
    public string? Lit_CSO_AClassClosedLess45_CurrentYear_Q2 { get; set; }
    public string? Lit_CSO_AClassClosedLess45_CurrentYear_Q3 { get; set; }
    public string? Lit_CSO_AClassClosedLess45_CurrentYear_Q4 { get; set; }
    public string? Lit_CSO_AClassClosedLess45_YTD_CurrentYear { get; set; }

    public string? Lit_CSO_PercentageClosure_YTD_PreviousYear { get; set; }
    public string? Lit_CSO_PercentageClosure_CurrentYear_Q1 { get; set; }
    public string? Lit_CSO_PercentageClosure_CurrentYear_Q2 { get; set; }
    public string? Lit_CSO_PercentageClosure_CurrentYear_Q3 { get; set; }
    public string? Lit_CSO_PercentageClosure_CurrentYear_Q4 { get; set; }
    public string? Lit_CSO_PercentageClosure_YTD_CurrentYear { get; set; }

    // 1.8 Cost Savings
    public string? Lit_CostSavings_YTD_PreviousYear { get; set; }
    public string? Lit_CostSavings_Target { get; set; }
    public string? Lit_CostSavings_CurrentYear_Q1 { get; set; }
    public string? Lit_CostSavings_CurrentYear_Q2 { get; set; }
    public string? Lit_CostSavings_CurrentYear_Q3 { get; set; }
    public string? Lit_CostSavings_CurrentYear_Q4 { get; set; }
    public string? Lit_CostSavings_YTD_CurrentYear { get; set; }

    // 1.9 Outgoing Quality Level (Vendors 1-8)
    public int? Lit_OQL_Vendor1 { get; set; }
    public string? Lit_OQL_Vendor1_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor1_Target { get; set; }
    public string? Lit_OQL_Vendor1_PC01 { get; set; }
    public string? Lit_OQL_Vendor1_PC02 { get; set; }
    public string? Lit_OQL_Vendor1_PC03 { get; set; }
    public string? Lit_OQL_Vendor1_PC04 { get; set; }
    public string? Lit_OQL_Vendor1_PC05 { get; set; }
    public string? Lit_OQL_Vendor1_PC06 { get; set; }

    public int? Lit_OQL_Vendor2 { get; set; }
    public string? Lit_OQL_Vendor2_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor2_Target { get; set; }
    public string? Lit_OQL_Vendor2_PC01 { get; set; }
    public string? Lit_OQL_Vendor2_PC02 { get; set; }
    public string? Lit_OQL_Vendor2_PC03 { get; set; }
    public string? Lit_OQL_Vendor2_PC04 { get; set; }
    public string? Lit_OQL_Vendor2_PC05 { get; set; }
    public string? Lit_OQL_Vendor2_PC06 { get; set; }

    public int? Lit_OQL_Vendor3 { get; set; }
    public string? Lit_OQL_Vendor3_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor3_Target { get; set; }
    public string? Lit_OQL_Vendor3_PC01 { get; set; }
    public string? Lit_OQL_Vendor3_PC02 { get; set; }
    public string? Lit_OQL_Vendor3_PC03 { get; set; }
    public string? Lit_OQL_Vendor3_PC04 { get; set; }
    public string? Lit_OQL_Vendor3_PC05 { get; set; }
    public string? Lit_OQL_Vendor3_PC06 { get; set; }

    public int? Lit_OQL_Vendor4 { get; set; }
    public string? Lit_OQL_Vendor4_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor4_Target { get; set; }
    public string? Lit_OQL_Vendor4_PC01 { get; set; }
    public string? Lit_OQL_Vendor4_PC02 { get; set; }
    public string? Lit_OQL_Vendor4_PC03 { get; set; }
    public string? Lit_OQL_Vendor4_PC04 { get; set; }
    public string? Lit_OQL_Vendor4_PC05 { get; set; }
    public string? Lit_OQL_Vendor4_PC06 { get; set; }

    public int? Lit_OQL_Vendor5 { get; set; }
    public string? Lit_OQL_Vendor5_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor5_Target { get; set; }
    public string? Lit_OQL_Vendor5_PC01 { get; set; }
    public string? Lit_OQL_Vendor5_PC02 { get; set; }
    public string? Lit_OQL_Vendor5_PC03 { get; set; }
    public string? Lit_OQL_Vendor5_PC04 { get; set; }
    public string? Lit_OQL_Vendor5_PC05 { get; set; }
    public string? Lit_OQL_Vendor5_PC06 { get; set; }

    public int? Lit_OQL_Vendor6 { get; set; }
    public string? Lit_OQL_Vendor6_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor6_Target { get; set; }
    public string? Lit_OQL_Vendor6_PC01 { get; set; }
    public string? Lit_OQL_Vendor6_PC02 { get; set; }
    public string? Lit_OQL_Vendor6_PC03 { get; set; }
    public string? Lit_OQL_Vendor6_PC04 { get; set; }
    public string? Lit_OQL_Vendor6_PC05 { get; set; }
    public string? Lit_OQL_Vendor6_PC06 { get; set; }

    public int? Lit_OQL_Vendor7 { get; set; }
    public string? Lit_OQL_Vendor7_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor7_Target { get; set; }
    public string? Lit_OQL_Vendor7_PC01 { get; set; }
    public string? Lit_OQL_Vendor7_PC02 { get; set; }
    public string? Lit_OQL_Vendor7_PC03 { get; set; }
    public string? Lit_OQL_Vendor7_PC04 { get; set; }
    public string? Lit_OQL_Vendor7_PC05 { get; set; }
    public string? Lit_OQL_Vendor7_PC06 { get; set; }

    public int? Lit_OQL_Vendor8 { get; set; }
    public string? Lit_OQL_Vendor8_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_Vendor8_Target { get; set; }
    public string? Lit_OQL_Vendor8_PC01 { get; set; }
    public string? Lit_OQL_Vendor8_PC02 { get; set; }
    public string? Lit_OQL_Vendor8_PC03 { get; set; }
    public string? Lit_OQL_Vendor8_PC04 { get; set; }
    public string? Lit_OQL_Vendor8_PC05 { get; set; }
    public string? Lit_OQL_Vendor8_PC06 { get; set; }

    // OQL Cumulative
    public string? Lit_OQL_CumulativeAvg1_YTD_PreviousYear { get; set; }
    public string? Lit_OQL_CumulativeAvg1_Target { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC01 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC02 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC03 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC04 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC05 { get; set; }
    public string? Lit_OQL_CumulativeAvg1_PC06 { get; set; }

    public string? Lit_OQL_CumulativeAvg2_YTD_PreviousYear { get; set; }
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
    public string? Seat_SS_CNClosure_CurrentYear_Q1 { get; set; }
    public string? Seat_SS_CNClosure_CurrentYear_Q2 { get; set; }
    public string? Seat_SS_CNClosure_CurrentYear_Q3 { get; set; }
    public string? Seat_SS_CNClosure_CurrentYear_Q4 { get; set; }
    public string? Seat_SS_CNClosure_YTD_CurrentYear { get; set; }

    public string? Seat_SS_OTIF_Baseline { get; set; }
    public string? Seat_SS_OTIF_Target { get; set; }
    public string? Seat_SS_OTIF_CurrentYear_Q1 { get; set; }
    public string? Seat_SS_OTIF_CurrentYear_Q2 { get; set; }
    public string? Seat_SS_OTIF_CurrentYear_Q3 { get; set; }
    public string? Seat_SS_OTIF_CurrentYear_Q4 { get; set; }
    public string? Seat_SS_OTIF_YTD_CurrentYear { get; set; }

    public string? Seat_SS_SPMScore_Baseline { get; set; }
    public string? Seat_SS_SPMScore_Target { get; set; }
    public string? Seat_SS_SPMScore_CurrentYear_Q1 { get; set; }
    public string? Seat_SS_SPMScore_CurrentYear_Q2 { get; set; }
    public string? Seat_SS_SPMScore_CurrentYear_Q3 { get; set; }
    public string? Seat_SS_SPMScore_CurrentYear_Q4 { get; set; }
    public string? Seat_SS_SPMScore_YTD_CurrentYear { get; set; }

    // 2.2 OTIF
    public string? Seat_OTIF_Performance_YTD_PreviousYear { get; set; }
    public string? Seat_OTIF_Performance_Target { get; set; }
    public string? Seat_OTIF_Performance_CurrentYear_Q1 { get; set; }
    public string? Seat_OTIF_Performance_CurrentYear_Q2 { get; set; }
    public string? Seat_OTIF_Performance_CurrentYear_Q3 { get; set; }
    public string? Seat_OTIF_Performance_CurrentYear_Q4 { get; set; }
    public string? Seat_OTIF_Performance_YTD_CurrentYear { get; set; }

    // 2.3 CSAT
    public string? Seat_CSAT_ReqSent_YTD_PreviousYear { get; set; }
    public string? Seat_CSAT_ReqSent_Baseline { get; set; }
    public string? Seat_CSAT_ReqSent_Target { get; set; }
    public string? Seat_CSAT_ReqSent_CurrentYear_Q1 { get; set; }
    public string? Seat_CSAT_ReqSent_CurrentYear_Q2 { get; set; }
    public string? Seat_CSAT_ReqSent_CurrentYear_Q3 { get; set; }
    public string? Seat_CSAT_ReqSent_CurrentYear_Q4 { get; set; }
    public string? Seat_CSAT_ReqSent_YTD_CurrentYear { get; set; }

    public string? Seat_CSAT_RespRecvd_YTD_PreviousYear { get; set; }
    public string? Seat_CSAT_RespRecvd_Baseline { get; set; }
    public string? Seat_CSAT_RespRecvd_Target { get; set; }
    public string? Seat_CSAT_RespRecvd_CurrentYear_Q1 { get; set; }
    public string? Seat_CSAT_RespRecvd_CurrentYear_Q2 { get; set; }
    public string? Seat_CSAT_RespRecvd_CurrentYear_Q3 { get; set; }
    public string? Seat_CSAT_RespRecvd_CurrentYear_Q4 { get; set; }
    public string? Seat_CSAT_RespRecvd_YTD_CurrentYear { get; set; }

    public string? Seat_CSAT_Promoter_YTD_PreviousYear { get; set; }
    public string? Seat_CSAT_Promoter_Baseline { get; set; }
    public string? Seat_CSAT_Promoter_Target { get; set; }
    public string? Seat_CSAT_Promoter_CurrentYear_Q1 { get; set; }
    public string? Seat_CSAT_Promoter_CurrentYear_Q2 { get; set; }
    public string? Seat_CSAT_Promoter_CurrentYear_Q3 { get; set; }
    public string? Seat_CSAT_Promoter_CurrentYear_Q4 { get; set; }
    public string? Seat_CSAT_Promoter_YTD_CurrentYear { get; set; }

    public string? Seat_CSAT_Detractor_YTD_PreviousYear { get; set; }
    public string? Seat_CSAT_Detractor_Baseline { get; set; }
    public string? Seat_CSAT_Detractor_Target { get; set; }
    public string? Seat_CSAT_Detractor_CurrentYear_Q1 { get; set; }
    public string? Seat_CSAT_Detractor_CurrentYear_Q2 { get; set; }
    public string? Seat_CSAT_Detractor_CurrentYear_Q3 { get; set; }
    public string? Seat_CSAT_Detractor_CurrentYear_Q4 { get; set; }
    public string? Seat_CSAT_Detractor_YTD_CurrentYear { get; set; }

    public string? Seat_CSAT_NPS_YTD_PreviousYear { get; set; }
    public string? Seat_CSAT_NPS_Baseline { get; set; }
    public string? Seat_CSAT_NPS_Target { get; set; }
    public string? Seat_CSAT_NPS_CurrentYear_Q1 { get; set; }
    public string? Seat_CSAT_NPS_CurrentYear_Q2 { get; set; }
    public string? Seat_CSAT_NPS_CurrentYear_Q3 { get; set; }
    public string? Seat_CSAT_NPS_CurrentYear_Q4 { get; set; }
    public string? Seat_CSAT_NPS_YTD_CurrentYear { get; set; }

    // 2.4 CSO
    public string? Seat_CSO_TotalLogged_YTD_PreviousYear { get; set; }
    public string? Seat_CSO_TotalLogged_CurrentYear_Q1 { get; set; }
    public string? Seat_CSO_TotalLogged_CurrentYear_Q2 { get; set; }
    public string? Seat_CSO_TotalLogged_CurrentYear_Q3 { get; set; }
    public string? Seat_CSO_TotalLogged_CurrentYear_Q4 { get; set; }
    public string? Seat_CSO_TotalLogged_YTD_CurrentYear { get; set; }

    public string? Seat_CSO_TotalAClass_YTD_PreviousYear { get; set; }
    public string? Seat_CSO_TotalAClass_CurrentYear_Q1 { get; set; }
    public string? Seat_CSO_TotalAClass_CurrentYear_Q2 { get; set; }
    public string? Seat_CSO_TotalAClass_CurrentYear_Q3 { get; set; }
    public string? Seat_CSO_TotalAClass_CurrentYear_Q4 { get; set; }
    public string? Seat_CSO_TotalAClass_YTD_CurrentYear { get; set; }

    public string? Seat_CSO_AClassClosed_YTD_PreviousYear { get; set; }
    public string? Seat_CSO_AClassClosed_CurrentYear_Q1 { get; set; }
    public string? Seat_CSO_AClassClosed_CurrentYear_Q2 { get; set; }
    public string? Seat_CSO_AClassClosed_CurrentYear_Q3 { get; set; }
    public string? Seat_CSO_AClassClosed_CurrentYear_Q4 { get; set; }
    public string? Seat_CSO_AClassClosed_YTD_CurrentYear { get; set; }

    public string? Seat_CSO_AClassClosedLess45_YTD_PreviousYear { get; set; }
    public string? Seat_CSO_AClassClosedLess45_CurrentYear_Q1 { get; set; }
    public string? Seat_CSO_AClassClosedLess45_CurrentYear_Q2 { get; set; }
    public string? Seat_CSO_AClassClosedLess45_CurrentYear_Q3 { get; set; }
    public string? Seat_CSO_AClassClosedLess45_CurrentYear_Q4 { get; set; }
    public string? Seat_CSO_AClassClosedLess45_YTD_CurrentYear { get; set; }

    public string? Seat_CSO_AClassClosedUnder45_YTD_PreviousYear { get; set; }
    public string? Seat_CSO_AClassClosedUnder45_CurrentYear_Q1 { get; set; }
    public string? Seat_CSO_AClassClosedUnder45_CurrentYear_Q2 { get; set; }
    public string? Seat_CSO_AClassClosedUnder45_CurrentYear_Q3 { get; set; }
    public string? Seat_CSO_AClassClosedUnder45_CurrentYear_Q4 { get; set; }
    public string? Seat_CSO_AClassClosedUnder45_YTD_CurrentYear { get; set; }

    // 2.5 SPM Score (Suppliers 1-5)
    public int? Seat_SPM_Supp1 { get; set; }
    public string? Seat_SPM_Supp1_PreviousYear_Q4 { get; set; }
    public string? Seat_SPM_Supp1_CurrentYear_Q1 { get; set; }
    public string? Seat_SPM_Supp1_CurrentYear_Q2 { get; set; }
    public string? Seat_SPM_Supp1_CurrentYear_Q3 { get; set; }
    public string? Seat_SPM_Supp1_CurrentYear_Q4 { get; set; }

    public int? Seat_SPM_Supp2 { get; set; }
    public string? Seat_SPM_Supp2_PreviousYear_Q4 { get; set; }
    public string? Seat_SPM_Supp2_CurrentYear_Q1 { get; set; }
    public string? Seat_SPM_Supp2_CurrentYear_Q2 { get; set; }
    public string? Seat_SPM_Supp2_CurrentYear_Q3 { get; set; }
    public string? Seat_SPM_Supp2_CurrentYear_Q4 { get; set; }

    public int? Seat_SPM_Supp3 { get; set; }
    public string? Seat_SPM_Supp3_PreviousYear_Q4 { get; set; }
    public string? Seat_SPM_Supp3_CurrentYear_Q1 { get; set; }
    public string? Seat_SPM_Supp3_CurrentYear_Q2 { get; set; }
    public string? Seat_SPM_Supp3_CurrentYear_Q3 { get; set; }
    public string? Seat_SPM_Supp3_CurrentYear_Q4 { get; set; }

    public int? Seat_SPM_Supp4 { get; set; }
    public string? Seat_SPM_Supp4_PreviousYear_Q4 { get; set; }
    public string? Seat_SPM_Supp4_CurrentYear_Q1 { get; set; }
    public string? Seat_SPM_Supp4_CurrentYear_Q2 { get; set; }
    public string? Seat_SPM_Supp4_CurrentYear_Q3 { get; set; }
    public string? Seat_SPM_Supp4_CurrentYear_Q4 { get; set; }

    public int? Seat_SPM_Supp5 { get; set; }
    public string? Seat_SPM_Supp5_PreviousYear_Q4 { get; set; }
    public string? Seat_SPM_Supp5_CurrentYear_Q1 { get; set; }
    public string? Seat_SPM_Supp5_CurrentYear_Q2 { get; set; }
    public string? Seat_SPM_Supp5_CurrentYear_Q3 { get; set; }
    public string? Seat_SPM_Supp5_CurrentYear_Q4 { get; set; }

    // 2.6 IQA
    public string? Seat_IQA_TotalSites_YTD_PreviousYear { get; set; }
    public string? Seat_IQA_TotalSites_Target { get; set; }
    public string? Seat_IQA_TotalSites_CurrentYear_Q1 { get; set; }
    public string? Seat_IQA_TotalSites_CurrentYear_Q2 { get; set; }
    public string? Seat_IQA_TotalSites_CurrentYear_Q3 { get; set; }
    public string? Seat_IQA_TotalSites_CurrentYear_Q4 { get; set; }
    public string? Seat_IQA_TotalSites_YTD_CurrentYear { get; set; }

    public string? Seat_IQA_SitesCompleted_YTD_PreviousYear { get; set; }
    public string? Seat_IQA_SitesCompleted_Target { get; set; }
    public string? Seat_IQA_SitesCompleted_CurrentYear_Q1 { get; set; }
    public string? Seat_IQA_SitesCompleted_CurrentYear_Q2 { get; set; }
    public string? Seat_IQA_SitesCompleted_CurrentYear_Q3 { get; set; }
    public string? Seat_IQA_SitesCompleted_CurrentYear_Q4 { get; set; }
    public string? Seat_IQA_SitesCompleted_YTD_CurrentYear { get; set; }

    public string? Seat_IQA_AuditsCompleted_YTD_PreviousYear { get; set; }
    public string? Seat_IQA_AuditsCompleted_Target { get; set; }
    public string? Seat_IQA_AuditsCompleted_CurrentYear_Q1 { get; set; }
    public string? Seat_IQA_AuditsCompleted_CurrentYear_Q2 { get; set; }
    public string? Seat_IQA_AuditsCompleted_CurrentYear_Q3 { get; set; }
    public string? Seat_IQA_AuditsCompleted_CurrentYear_Q4 { get; set; }
    public string? Seat_IQA_AuditsCompleted_YTD_CurrentYear { get; set; }

    public string? Seat_IQA_PercCompleted_YTD_PreviousYear { get; set; }
    public string? Seat_IQA_PercCompleted_Target { get; set; }
    public string? Seat_IQA_PercCompleted_CurrentYear_Q1 { get; set; }
    public string? Seat_IQA_PercCompleted_CurrentYear_Q2 { get; set; }
    public string? Seat_IQA_PercCompleted_CurrentYear_Q3 { get; set; }
    public string? Seat_IQA_PercCompleted_CurrentYear_Q4 { get; set; }
    public string? Seat_IQA_PercCompleted_YTD_CurrentYear { get; set; }

    public string? Seat_IQA_AvgSigma_YTD_PreviousYear { get; set; }
    public string? Seat_IQA_AvgSigma_Target { get; set; }
    public string? Seat_IQA_AvgSigma_CurrentYear_Q1 { get; set; }
    public string? Seat_IQA_AvgSigma_CurrentYear_Q2 { get; set; }
    public string? Seat_IQA_AvgSigma_CurrentYear_Q3 { get; set; }
    public string? Seat_IQA_AvgSigma_CurrentYear_Q4 { get; set; }
    public string? Seat_IQA_AvgSigma_YTD_CurrentYear { get; set; }
    #endregion

    #region Audit Fields
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    #endregion
}