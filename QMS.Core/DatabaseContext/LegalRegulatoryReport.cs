using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext;

[Table("tbl_LegalRegulatoryReport")]
public class LegalRegulatoryReport : SqlTable
{
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }

    public string? DriverBIS_Result1 { get; set; }
    public string? DriverBIS_Result2 { get; set; }
    public string? DriverBIS_Result3 { get; set; }
    public string? DriverBIS_Result4 { get; set; }
    public string? DriverBIS_UploadFile { get; set; }
    public string? LuminairesBIS_Result1 { get; set; }
    public string? LuminairesBIS_Result2 { get; set; }
    public string? LuminairesBIS_Result3 { get; set; }
    public string? LuminairesBIS_Result4 { get; set; }
    public string? LuminairesBIS_UploadFile { get; set; }
    public string? CCL_Result1 { get; set; }
    public string? CCL_Result2 { get; set; }
    public string? CCL_Result3 { get; set; }
    public string? CCL_Result4 { get; set; }
    public string? CCL_UploadFile { get; set; }
    public string? NPIBuySheet_Result1 { get; set; }
    public string? NPIBuySheet_Result2 { get; set; }
    public string? NPIBuySheet_Result3 { get; set; }
    public string? NPIBuySheet_Result4 { get; set; }
    public string? NPIBuySheet_UploadFile { get; set; }
    public string? CPS_Result1 { get; set; }
    public string? CPS_Result2 { get; set; }
    public string? CPS_Result3 { get; set; }
    public string? CPS_Result4 { get; set; }
    public string? CPS_UploadFile { get; set; }
    public string? PPS_Result1 { get; set; }
    public string? PPS_Result2 { get; set; }
    public string? PPS_Result3 { get; set; }
    public string? PPS_Result4 { get; set; }
    public string? PPS_UploadFile { get; set; }
    public string? TDS_Result1 { get; set; }
    public string? TDS_Result2 { get; set; }
    public string? TDS_Result3 { get; set; }
    public string? TDS_Result4 { get; set; }
    public string? TDS_UploadFile { get; set; }
    public string? DesignDocket_Result1 { get; set; }
    public string? DesignDocket_Result2 { get; set; }
    public string? DesignDocket_Result3 { get; set; }
    public string? DesignDocket_Result4 { get; set; }
    public string? DesignDocket_UploadFile { get; set; }
    public string? InstallationSheet_Result1 { get; set; }
    public string? InstallationSheet_Result2 { get; set; }
    public string? InstallationSheet_Result3 { get; set; }
    public string? InstallationSheet_Result4 { get; set; }
    public string? InstallationSheet_UploadFile { get; set; }
    public string? ROHSCompliance_Result1 { get; set; }
    public string? ROHSCompliance_Result2 { get; set; }
    public string? ROHSCompliance_Result3 { get; set; }
    public string? ROHSCompliance_Result4 { get; set; }
    public string? ROHSCompliance_UploadFile { get; set; }
    public string? CIMFR_Result1 { get; set; }
    public string? CIMFR_Result2 { get; set; }
    public string? CIMFR_Result3 { get; set; }
    public string? CIMFR_Result4 { get; set; }
    public string? CIMFR_UploadFile { get; set; }
    public string? PESO_Result1 { get; set; }
    public string? PESO_Result2 { get; set; }
    public string? PESO_Result3 { get; set; }
    public string? PESO_Result4 { get; set; }
    public string? PESO_UploadFile { get; set; }
    public string? BOM_Result1 { get; set; }
    public string? BOM_Result2 { get; set; }
    public string? BOM_Result3 { get; set; }
    public string? BOM_Result4 { get; set; }
    public string? BOM_UploadFile { get; set; }
    public string? SpareCodeSAP_Result1 { get; set; }
    public string? SpareCodeSAP_Result2 { get; set; }
    public string? SpareCodeSAP_Result3 { get; set; }
    public string? SpareCodeSAP_Result4 { get; set; }
    public string? SpareCodeSAP_UploadFile { get; set; }
    public string? CERegistration_Result1 { get; set; }
    public string? CERegistration_Result2 { get; set; }
    public string? CERegistration_Result3 { get; set; }
    public string? CERegistration_Result4 { get; set; }
    public string? CERegistration_UploadFile { get; set; }
    public string? OverallResult { get; set; }
    public string? CheckedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
