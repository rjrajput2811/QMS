using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_SurgeTestReport")]
public class SurgeTestReport : SqlTable
{
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? DriverCode { get; set; }
    public string? SPDCode { get; set; }
    public string? LEDConfiguration { get; set; }
    public string? BatchCode { get; set; }
    public string? PKD { get; set; }
    public string? ReferenceStandard { get; set; }
    public string? AcceptanceNorm { get; set; }

    public string? WithoutSPD_R1_Voltage_KV { get; set; }
    public string? WithoutSPD_R1_Mode { get; set; }
    public string? WithoutSPD_R1_L_N_DM_90 { get; set; }
    public string? WithoutSPD_R1_L_N_DM_180 { get; set; }
    public string? WithoutSPD_R1_L_N_DM_270 { get; set; }
    public string? WithoutSPD_R1_L_N_DM_0 { get; set; }
    public string? WithoutSPD_R1_L_E_CM_90 { get; set; }
    public string? WithoutSPD_R1_L_E_CM_180 { get; set; }
    public string? WithoutSPD_R1_L_E_CM_270 { get; set; }
    public string? WithoutSPD_R1_L_E_CM_0 { get; set; }
    public string? WithoutSPD_R1_N_E_CM_90 { get; set; }
    public string? WithoutSPD_R1_N_E_CM_180 { get; set; }
    public string? WithoutSPD_R1_N_E_CM_270 { get; set; }
    public string? WithoutSPD_R1_N_E_CM_0 { get; set; }
    public string? WithoutSPD_R1_Observation { get; set; }

    public string? WithoutSPD_R2_Voltage_KV { get; set; }
    public string? WithoutSPD_R2_Mode { get; set; }
    public string? WithoutSPD_R2_L_N_DM_90 { get; set; }
    public string? WithoutSPD_R2_L_N_DM_180 { get; set; }
    public string? WithoutSPD_R2_L_N_DM_270 { get; set; }
    public string? WithoutSPD_R2_L_N_DM_0 { get; set; }
    public string? WithoutSPD_R2_L_E_CM_90 { get; set; }
    public string? WithoutSPD_R2_L_E_CM_180 { get; set; }
    public string? WithoutSPD_R2_L_E_CM_270 { get; set; }
    public string? WithoutSPD_R2_L_E_CM_0 { get; set; }
    public string? WithoutSPD_R2_N_E_CM_90 { get; set; }
    public string? WithoutSPD_R2_N_E_CM_180 { get; set; }
    public string? WithoutSPD_R2_N_E_CM_270 { get; set; }
    public string? WithoutSPD_R2_N_E_CM_0 { get; set; }
    public string? WithoutSPD_R2_Observation { get; set; }

    public string? WithoutSPD_R3_Voltage_KV { get; set; }
    public string? WithoutSPD_R3_Mode { get; set; }
    public string? WithoutSPD_R3_L_N_DM_90 { get; set; }
    public string? WithoutSPD_R3_L_N_DM_180 { get; set; }
    public string? WithoutSPD_R3_L_N_DM_270 { get; set; }
    public string? WithoutSPD_R3_L_N_DM_0 { get; set; }
    public string? WithoutSPD_R3_L_E_CM_90 { get; set; }
    public string? WithoutSPD_R3_L_E_CM_180 { get; set; }
    public string? WithoutSPD_R3_L_E_CM_270 { get; set; }
    public string? WithoutSPD_R3_L_E_CM_0 { get; set; }
    public string? WithoutSPD_R3_N_E_CM_90 { get; set; }
    public string? WithoutSPD_R3_N_E_CM_180 { get; set; }
    public string? WithoutSPD_R3_N_E_CM_270 { get; set; }
    public string? WithoutSPD_R3_N_E_CM_0 { get; set; }
    public string? WithoutSPD_R3_Observation { get; set; }

    public string? WithoutSPD_R4_Voltage_KV { get; set; }
    public string? WithoutSPD_R4_Mode { get; set; }
    public string? WithoutSPD_R4_L_N_DM_90 { get; set; }
    public string? WithoutSPD_R4_L_N_DM_180 { get; set; }
    public string? WithoutSPD_R4_L_N_DM_270 { get; set; }
    public string? WithoutSPD_R4_L_N_DM_0 { get; set; }
    public string? WithoutSPD_R4_L_E_CM_90 { get; set; }
    public string? WithoutSPD_R4_L_E_CM_180 { get; set; }
    public string? WithoutSPD_R4_L_E_CM_270 { get; set; }
    public string? WithoutSPD_R4_L_E_CM_0 { get; set; }
    public string? WithoutSPD_R4_N_E_CM_90 { get; set; }
    public string? WithoutSPD_R4_N_E_CM_180 { get; set; }
    public string? WithoutSPD_R4_N_E_CM_270 { get; set; }
    public string? WithoutSPD_R4_N_E_CM_0 { get; set; }
    public string? WithoutSPD_R4_Observation { get; set; }

    public string? WithoutSPD_R5_Voltage_KV { get; set; }
    public string? WithoutSPD_R5_Mode { get; set; }
    public string? WithoutSPD_R5_L_N_DM_90 { get; set; }
    public string? WithoutSPD_R5_L_N_DM_180 { get; set; }
    public string? WithoutSPD_R5_L_N_DM_270 { get; set; }
    public string? WithoutSPD_R5_L_N_DM_0 { get; set; }
    public string? WithoutSPD_R5_L_E_CM_90 { get; set; }
    public string? WithoutSPD_R5_L_E_CM_180 { get; set; }
    public string? WithoutSPD_R5_L_E_CM_270 { get; set; }
    public string? WithoutSPD_R5_L_E_CM_0 { get; set; }
    public string? WithoutSPD_R5_N_E_CM_90 { get; set; }
    public string? WithoutSPD_R5_N_E_CM_180 { get; set; }
    public string? WithoutSPD_R5_N_E_CM_270 { get; set; }
    public string? WithoutSPD_R5_N_E_CM_0 { get; set; }
    public string? WithoutSPD_R5_Observation { get; set; }

    public string? WithoutSPD_R6_Voltage_KV { get; set; }
    public string? WithoutSPD_R6_Mode { get; set; }
    public string? WithoutSPD_R6_L_N_DM_90 { get; set; }
    public string? WithoutSPD_R6_L_N_DM_180 { get; set; }
    public string? WithoutSPD_R6_L_N_DM_270 { get; set; }
    public string? WithoutSPD_R6_L_N_DM_0 { get; set; }
    public string? WithoutSPD_R6_L_E_CM_90 { get; set; }
    public string? WithoutSPD_R6_L_E_CM_180 { get; set; }
    public string? WithoutSPD_R6_L_E_CM_270 { get; set; }
    public string? WithoutSPD_R6_L_E_CM_0 { get; set; }
    public string? WithoutSPD_R6_N_E_CM_90 { get; set; }
    public string? WithoutSPD_R6_N_E_CM_180 { get; set; }
    public string? WithoutSPD_R6_N_E_CM_270 { get; set; }
    public string? WithoutSPD_R6_N_E_CM_0 { get; set; }
    public string? WithoutSPD_R6_Observation { get; set; }

    public string? WithoutSPD_Result_PassFail { get; set; }
    public string? WithoutSPD_Result_Driver_OK { get; set; }
    public string? WithoutSPD_Result_LED_PCB_OK { get; set; }

    public string? WithSPD_R1_Voltage_KV { get; set; }
    public string? WithSPD_R1_Mode { get; set; }
    public string? WithSPD_R1_L_N_DM_90 { get; set; }
    public string? WithSPD_R1_L_N_DM_180 { get; set; }
    public string? WithSPD_R1_L_N_DM_270 { get; set; }
    public string? WithSPD_R1_L_N_DM_0 { get; set; }
    public string? WithSPD_R1_L_E_CM_90 { get; set; }
    public string? WithSPD_R1_L_E_CM_180 { get; set; }
    public string? WithSPD_R1_L_E_CM_270 { get; set; }
    public string? WithSPD_R1_L_E_CM_0 { get; set; }
    public string? WithSPD_R1_N_E_CM_90 { get; set; }
    public string? WithSPD_R1_N_E_CM_180 { get; set; }
    public string? WithSPD_R1_N_E_CM_270 { get; set; }
    public string? WithSPD_R1_N_E_CM_0 { get; set; }
    public string? WithSPD_R1_Observation { get; set; }

    public string? WithSPD_R2_Voltage_KV { get; set; }
    public string? WithSPD_R2_Mode { get; set; }
    public string? WithSPD_R2_L_N_DM_90 { get; set; }
    public string? WithSPD_R2_L_N_DM_180 { get; set; }
    public string? WithSPD_R2_L_N_DM_270 { get; set; }
    public string? WithSPD_R2_L_N_DM_0 { get; set; }
    public string? WithSPD_R2_L_E_CM_90 { get; set; }
    public string? WithSPD_R2_L_E_CM_180 { get; set; }
    public string? WithSPD_R2_L_E_CM_270 { get; set; }
    public string? WithSPD_R2_L_E_CM_0 { get; set; }
    public string? WithSPD_R2_N_E_CM_90 { get; set; }
    public string? WithSPD_R2_N_E_CM_180 { get; set; }
    public string? WithSPD_R2_N_E_CM_270 { get; set; }
    public string? WithSPD_R2_N_E_CM_0 { get; set; }
    public string? WithSPD_R2_Observation { get; set; }

    public string? WithSPD_R3_Voltage_KV { get; set; }
    public string? WithSPD_R3_Mode { get; set; }
    public string? WithSPD_R3_L_N_DM_90 { get; set; }
    public string? WithSPD_R3_L_N_DM_180 { get; set; }
    public string? WithSPD_R3_L_N_DM_270 { get; set; }
    public string? WithSPD_R3_L_N_DM_0 { get; set; }
    public string? WithSPD_R3_L_E_CM_90 { get; set; }
    public string? WithSPD_R3_L_E_CM_180 { get; set; }
    public string? WithSPD_R3_L_E_CM_270 { get; set; }
    public string? WithSPD_R3_L_E_CM_0 { get; set; }
    public string? WithSPD_R3_N_E_CM_90 { get; set; }
    public string? WithSPD_R3_N_E_CM_180 { get; set; }
    public string? WithSPD_R3_N_E_CM_270 { get; set; }
    public string? WithSPD_R3_N_E_CM_0 { get; set; }
    public string? WithSPD_R3_Observation { get; set; }

    public string? WithSPD_R4_Voltage_KV { get; set; }
    public string? WithSPD_R4_Mode { get; set; }
    public string? WithSPD_R4_L_N_DM_90 { get; set; }
    public string? WithSPD_R4_L_N_DM_180 { get; set; }
    public string? WithSPD_R4_L_N_DM_270 { get; set; }
    public string? WithSPD_R4_L_N_DM_0 { get; set; }
    public string? WithSPD_R4_L_E_CM_90 { get; set; }
    public string? WithSPD_R4_L_E_CM_180 { get; set; }
    public string? WithSPD_R4_L_E_CM_270 { get; set; }
    public string? WithSPD_R4_L_E_CM_0 { get; set; }
    public string? WithSPD_R4_N_E_CM_90 { get; set; }
    public string? WithSPD_R4_N_E_CM_180 { get; set; }
    public string? WithSPD_R4_N_E_CM_270 { get; set; }
    public string? WithSPD_R4_N_E_CM_0 { get; set; }
    public string? WithSPD_R4_Observation { get; set; }

    public string? WithSPD_R5_Voltage_KV { get; set; }
    public string? WithSPD_R5_Mode { get; set; }
    public string? WithSPD_R5_L_N_DM_90 { get; set; }
    public string? WithSPD_R5_L_N_DM_180 { get; set; }
    public string? WithSPD_R5_L_N_DM_270 { get; set; }
    public string? WithSPD_R5_L_N_DM_0 { get; set; }
    public string? WithSPD_R5_L_E_CM_90 { get; set; }
    public string? WithSPD_R5_L_E_CM_180 { get; set; }
    public string? WithSPD_R5_L_E_CM_270 { get; set; }
    public string? WithSPD_R5_L_E_CM_0 { get; set; }
    public string? WithSPD_R5_N_E_CM_90 { get; set; }
    public string? WithSPD_R5_N_E_CM_180 { get; set; }
    public string? WithSPD_R5_N_E_CM_270 { get; set; }
    public string? WithSPD_R5_N_E_CM_0 { get; set; }
    public string? WithSPD_R5_Observation { get; set; }

    public string? WithSPD_R6_Voltage_KV { get; set; }
    public string? WithSPD_R6_Mode { get; set; }
    public string? WithSPD_R6_L_N_DM_90 { get; set; }
    public string? WithSPD_R6_L_N_DM_180 { get; set; }
    public string? WithSPD_R6_L_N_DM_270 { get; set; }
    public string? WithSPD_R6_L_N_DM_0 { get; set; }
    public string? WithSPD_R6_L_E_CM_90 { get; set; }
    public string? WithSPD_R6_L_E_CM_180 { get; set; }
    public string? WithSPD_R6_L_E_CM_270 { get; set; }
    public string? WithSPD_R6_L_E_CM_0 { get; set; }
    public string? WithSPD_R6_N_E_CM_90 { get; set; }
    public string? WithSPD_R6_N_E_CM_180 { get; set; }
    public string? WithSPD_R6_N_E_CM_270 { get; set; }
    public string? WithSPD_R6_N_E_CM_0 { get; set; }
    public string? WithSPD_R6_Observation { get; set; }

    public string? WithSPD_Result_PassFail { get; set; }
    public string? WithSPD_Result_Driver_OK { get; set; }
    public string? WithSPD_Result_LED_PCB_OK { get; set; }
    public string? WithSPD_Result_SPD_OK { get; set; }

    public string? Surge_Photo { get; set; }

    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
