using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext;

[Table("tbl_DropTestReport")]
public class DropTestReport : SqlTable
{
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? CaseLot { get; set; }
    public string? PackingBox_MasterCarton_Dimension { get; set; }
    public string? PackingBox_InnerCarton_Dimension { get; set; }
    public string? InnerPaddingDimension { get; set; }
    public string? GrossWeight_Kg { get; set; }
    public string? HeightForTest_IS9000 { get; set; }
    public string? GlowTest_Criteria { get; set; }
    public string? DropTest_Observations { get; set; }
    public string? RollingTest_Observations { get; set; }
    public string? Photographs_Before1 { get; set; }
    public string? Photographs_Before2 { get; set; }
    public string? Photographs_After1 { get; set; }
    public string? Photographs_After2 { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
