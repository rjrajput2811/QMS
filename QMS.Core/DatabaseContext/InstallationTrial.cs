using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_InstallationTrial")]
public class InstallationTrial : SqlTable
{
    public string? ReportNo { get; set; }
    public string? ProductCatRef { get; set; }
    public string? BatchCode { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductDescription { get; set; }
    public string? PKD { get; set; }
    public int SampleQty { get; set; }
    public string? ProductCategory_SampleDetails { get; set; }
    public string? ProductCategory_Result { get; set; }
    public string? InstallationSheet_SampleDetails { get; set; }
    public string? InstallationSheet_Result { get; set; }
    public string? MountingMechanism_SampleDetails { get; set; }
    public string? MountingMechanism_Result { get; set; }
    public string? DurationOfTest_SampleDetails { get; set; }
    public string? DurationOfTest_Result { get; set; }
    public string? InstallationWith4xLoad_SampleDetails { get; set; }
    public string? InstallationWith4xLoad_Result { get; set; }
    public string? InstallationWithSandBagLoad_SampleDetails { get; set; }
    public string? InstallationWithSandBagLoad_Result { get; set; }
    public string? InstallationWithSandBagLoad2_SampleDetails { get; set; }
    public string? InstallationWithSandBagLoad2_Result { get; set; }
    public string? Photo_WithLoad { get; set; }
    public string? Photo_WithoutLoad { get; set; }
    public string? OverallResult { get; set; }
    public string? CheckedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
