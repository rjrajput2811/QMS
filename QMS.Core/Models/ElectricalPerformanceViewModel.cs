using QMS.Core.DatabaseContext;
using System.ComponentModel.DataAnnotations;

namespace QMS.Core.Models;

public class ElectricalPerformanceViewModel
{
    public int Id { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? ReportNo { get; set; }
    public string? LightSourceDetails { get; set; }
    public string? DriverDetails { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? ReportDate { get; set; }
    public string? PCBDetails { get; set; }
    public string? LEDCombinations { get; set; }
    public string? BatchCode { get; set; }
    public string? SensorDetails { get; set; }
    public string? LampDetails { get; set; }
    public string? PKD { get; set; }

    public string? OverallResult { get; set; }
    public string? TestedByName { get; set; }
    public string? VerifiedByName { get; set; }
    public int AddedBy { get; set; }
    public string? User { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public int Success { get; set; }
    public string? ElectricalDetails { get; set; }
    public List<ElectricalPerDetailsViewModal> Details { get; set; } = new();

}
