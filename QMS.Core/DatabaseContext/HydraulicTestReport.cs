using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext;

[Table("tbl_HydraulicTestReport")]
public class HydraulicTestReport : SqlTable
{
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
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }


}
