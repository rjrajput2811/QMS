using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_HydraulicTestObservationReport")]
public class HydraulicTestObservationReport : SqlTable
{
    public int HydraulicTestReport_Id { get; set; }
    public string? PhotoBeforeTest { get; set; }
    public string? PhotoAfterTest { get; set; }
    public string? Observation { get; set; }
    public string? Result { get; set; }
}
