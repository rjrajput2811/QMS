using QMS.Core.DatabaseContext;

namespace QMS.Core.Models;

public class HydraulicTestReportViewModel
{
    public int Id { get; set; }
    public string? ReportNo { get; set; }
    public string? CustomerProjectName { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? BatchCode { get; set; }
    public int Quantity { get; set; }
    public string? HydraulicTestPressure { get; set; }
    public string? OverallResult { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public string? User { get; set; }
    public bool Deleted { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public List<HydraulicTestObservationReportViewModel> Observations { get; set; } = new List<HydraulicTestObservationReportViewModel>();
}
