using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models;

public class RippleTestReportViewModel
{
    public int Id { get; set; }
    public string? ReportNo { get; set; }
    public DateTime? TestingDate { get; set; }
    public string? MeasuringInstrument { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? BatchCode { get; set; }
    public string? PKD { get; set; }
    public string? LEDDetails { get; set; }
    public string? LEDDriver { get; set; }
    public string? LEDCombination { get; set; }
    public decimal? DeltaValue { get; set; }
    public decimal? RMSValue { get; set; }
    public string? Calculation { get; set; }
    public decimal? RipplePercentage { get; set; }
    public List<IFormFile>? RippleTestFileAttachedFile { get; set; }
    public string? RippleTestFileAttachedPath { get; set; }
    public string? RemainingImages { get; set; }
    public string? Result { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string? User { get; set; }
}
