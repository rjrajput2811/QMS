using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_MainValidationReport")]
public class MainValidationReport : SqlTable
{
    public string? VendorNameLocation { get; set; }
    public string? ReportNo { get; set; }
    public string? VendorQAPerson { get; set; }
    public string? ValidationDoneBy { get; set; }
    public string? ProductCode { get; set; }
    public int QuantityOffered { get; set; }
    public string? SampleType { get; set; }
    public string? ProductDescription { get; set; }
    public string? ProductSpec { get; set; }
    public string? DriverMakeRating { get; set; }
    public string? LEDMakeRating { get; set; }
    public string? LensDiffuserMake { get; set; }
    public string? SPDMakeRating { get; set; }
    public string? GasketDetails { get; set; }
    public string? ProductCategory { get; set; }
    public string? DriverIPRating { get; set; }
    public string? ProductIPRating { get; set; }

    public string? RefDoc_NPISheet { get; set; }
    public string? RefDoc_PPS { get; set; }
    public string? RefDoc_TDS { get; set; }
    public string? RefDoc_DOCKET { get; set; }
    public string? RefDoc_CPS { get; set; }
    public string? RefDoc_CPSRefNo { get; set; }

    public string? Photo_FrontSide { get; set; }
    public string? Photo_BackSide { get; set; }
    public string? Photo_InternalAssy { get; set; }

    public string? PhysicalCheck_Obs_S1 { get; set; }
    public string? PhysicalCheck_Obs_S2 { get; set; }
    public string? PhysicalCheck_Obs_S3 { get; set; }
    public string? PhysicalCheck_Obs_S4 { get; set; }
    public string? PhysicalCheck_Obs_S5 { get; set; }
    public string? PhysicalCheck_Result { get; set; }

    public string? ElectricalPerformance_Obs_S1 { get; set; }
    public string? ElectricalPerformance_Obs_S2 { get; set; }
    public string? ElectricalPerformance_Obs_S3 { get; set; }
    public string? ElectricalPerformance_Obs_S4 { get; set; }
    public string? ElectricalPerformance_Obs_S5 { get; set; }
    public string? ElectricalPerformance_Result { get; set; }

    public string? ElectricalProtectionSafety_Obs_S1 { get; set; }
    public string? ElectricalProtectionSafety_Obs_S2 { get; set; }
    public string? ElectricalProtectionSafety_Obs_S3 { get; set; }
    public string? ElectricalProtectionSafety_Obs_S4 { get; set; }
    public string? ElectricalProtectionSafety_Obs_S5 { get; set; }
    public string? ElectricalProtectionSafety_Result { get; set; }

    public string? RippleTest_Obs_S1 { get; set; }
    public string? RippleTest_Obs_S2 { get; set; }
    public string? RippleTest_Obs_S3 { get; set; }
    public string? RippleTest_Obs_S4 { get; set; }
    public string? RippleTest_Obs_S5 { get; set; }
    public string? RippleTest_Result { get; set; }

    public string? SurgeWithoutSPD_Obs_S1 { get; set; }
    public string? SurgeWithoutSPD_Obs_S2 { get; set; }
    public string? SurgeWithoutSPD_Obs_S3 { get; set; }
    public string? SurgeWithoutSPD_Obs_S4 { get; set; }
    public string? SurgeWithoutSPD_Obs_S5 { get; set; }
    public string? SurgeWithoutSPD_Result { get; set; }

    public string? SurgeWithSPD_Obs_S1 { get; set; }
    public string? SurgeWithSPD_Obs_S2 { get; set; }
    public string? SurgeWithSPD_Obs_S3 { get; set; }
    public string? SurgeWithSPD_Obs_S4 { get; set; }
    public string? SurgeWithSPD_Obs_S5 { get; set; }
    public string? SurgeWithSPD_Result { get; set; }

    public string? TRT_Obs_S1 { get; set; }
    public string? TRT_Obs_S2 { get; set; }
    public string? TRT_Obs_S3 { get; set; }
    public string? TRT_Obs_S4 { get; set; }
    public string? TRT_Obs_S5 { get; set; }
    public string? TRT_Result { get; set; }

    public string? Photometry_Obs_S1 { get; set; }
    public string? Photometry_Obs_S2 { get; set; }
    public string? Photometry_Obs_S3 { get; set; }
    public string? Photometry_Obs_S4 { get; set; }
    public string? Photometry_Obs_S5 { get; set; }
    public string? Photometry_Result { get; set; }

    public string? InstallationTrial_Obs_S1 { get; set; }
    public string? InstallationTrial_Obs_S2 { get; set; }
    public string? InstallationTrial_Obs_S3 { get; set; }
    public string? InstallationTrial_Obs_S4 { get; set; }
    public string? InstallationTrial_Obs_S5 { get; set; }
    public string? InstallationTrial_Result { get; set; }

    public string? DropTest_Obs_S1 { get; set; }
    public string? DropTest_Obs_S2 { get; set; }
    public string? DropTest_Obs_S3 { get; set; }
    public string? DropTest_Obs_S4 { get; set; }
    public string? DropTest_Obs_S5 { get; set; }
    public string? DropTest_Result { get; set; }

    public string? IKTest_Obs_S1 { get; set; }
    public string? IKTest_Obs_S2 { get; set; }
    public string? IKTest_Obs_S3 { get; set; }
    public string? IKTest_Obs_S4 { get; set; }
    public string? IKTest_Obs_S5 { get; set; }
    public string? IKTest_Result { get; set; }

    public string? IPTest_Obs_S1 { get; set; }
    public string? IPTest_Obs_S2 { get; set; }
    public string? IPTest_Obs_S3 { get; set; }
    public string? IPTest_Obs_S4 { get; set; }
    public string? IPTest_Obs_S5 { get; set; }
    public string? IPTest_Result { get; set; }

    public string? LegalRegulatoryDocs_Obs_S1 { get; set; }
    public string? LegalRegulatoryDocs_Obs_S2 { get; set; }
    public string? LegalRegulatoryDocs_Obs_S3 { get; set; }
    public string? LegalRegulatoryDocs_Obs_S4 { get; set; }
    public string? LegalRegulatoryDocs_Obs_S5 { get; set; }
    public string? LegalRegulatoryDocs_Result { get; set; }

    public string? GlowWireTest_Obs_S1 { get; set; }
    public string? GlowWireTest_Obs_S2 { get; set; }
    public string? GlowWireTest_Obs_S3 { get; set; }
    public string? GlowWireTest_Obs_S4 { get; set; }
    public string? GlowWireTest_Obs_S5 { get; set; }
    public string? GlowWireTest_Result { get; set; }

    public string? HydraulicTest_Obs_S1 { get; set; }
    public string? HydraulicTest_Obs_S2 { get; set; }
    public string? HydraulicTest_Obs_S3 { get; set; }
    public string? HydraulicTest_Obs_S4 { get; set; }
    public string? HydraulicTest_Obs_S5 { get; set; }
    public string? HydraulicTest_Result { get; set; }

    public string? WindLoadTest_Obs_S1 { get; set; }
    public string? WindLoadTest_Obs_S2 { get; set; }
    public string? WindLoadTest_Obs_S3 { get; set; }
    public string? WindLoadTest_Obs_S4 { get; set; }
    public string? WindLoadTest_Obs_S5 { get; set; }
    public string? WindLoadTest_Result { get; set; }

    public string? GeneralObservationOpenPoints_Obs_S1 { get; set; }
    public string? GeneralObservationOpenPoints_Obs_S2 { get; set; }
    public string? GeneralObservationOpenPoints_Obs_S3 { get; set; }
    public string? GeneralObservationOpenPoints_Obs_S4 { get; set; }
    public string? GeneralObservationOpenPoints_Obs_S5 { get; set; }
    public string? GeneralObservationOpenPoints_Result { get; set; }

    public string? Test17_Obs_S1 { get; set; }
    public string? Test17_Obs_S2 { get; set; }
    public string? Test17_Obs_S3 { get; set; }
    public string? Test17_Obs_S4 { get; set; }
    public string? Test17_Obs_S5 { get; set; }
    public string? Test17_Result { get; set; }

    public string? Test18_Obs_S1 { get; set; }
    public string? Test18_Obs_S2 { get; set; }
    public string? Test18_Obs_S3 { get; set; }
    public string? Test18_Obs_S4 { get; set; }
    public string? Test18_Obs_S5 { get; set; }
    public string? Test18_Result { get; set; }

    public string? Test19_Obs_S1 { get; set; }
    public string? Test19_Obs_S2 { get; set; }
    public string? Test19_Obs_S3 { get; set; }
    public string? Test19_Obs_S4 { get; set; }
    public string? Test19_Obs_S5 { get; set; }
    public string? Test19_Result { get; set; }

    public string? Test20_Obs_S1 { get; set; }
    public string? Test20_Obs_S2 { get; set; }
    public string? Test20_Obs_S3 { get; set; }
    public string? Test20_Obs_S4 { get; set; }
    public string? Test20_Obs_S5 { get; set; }
    public string? Test20_Result { get; set; }

    public string? Test21_Obs_S1 { get; set; }
    public string? Test21_Obs_S2 { get; set; }
    public string? Test21_Obs_S3 { get; set; }
    public string? Test21_Obs_S4 { get; set; }
    public string? Test21_Obs_S5 { get; set; }
    public string? Test21_Result { get; set; }

    public string? Test22_Obs_S1 { get; set; }
    public string? Test22_Obs_S2 { get; set; }
    public string? Test22_Obs_S3 { get; set; }
    public string? Test22_Obs_S4 { get; set; }
    public string? Test22_Obs_S5 { get; set; }
    public string? Test22_Result { get; set; }

    public string? VendorQA_FinalComments { get; set; }
    public string? WiproQA_FinalComments { get; set; }
    public string? VendorQA_Signature { get; set; }
    public string? WiproQA_Signature { get; set; }
    public string? ReportAttached { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
