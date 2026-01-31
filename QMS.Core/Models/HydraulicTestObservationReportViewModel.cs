using Microsoft.AspNetCore.Http;

namespace QMS.Core.Models;

public class HydraulicTestObservationReportViewModel
{
    public int Id { get; set; }
    public int HydraulicTestReport_Id { get; set; }
    public string? PhotoBeforeTest { get; set; }
    public string? PhotoAfterTest { get; set; }
    public IFormFile? PhotoBeforeTestAttachedFile { get; set; }
    public IFormFile? PhotoAfterTestAttachedFile { get; set; }
    public string? Observation { get; set; }
    public string? Result { get; set; }
}
