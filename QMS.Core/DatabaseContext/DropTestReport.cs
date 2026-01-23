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
    public string? Glow_Test { get; set; }
    public string? OverallResult { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public List<DropTestReportDetail>? Details { get; set; }
    public List<DropTestReportImgDetail>? ImgDetails { get; set; }
}

[Table("tbl_DropTest_Details")]
public class DropTestReportDetail : SqlTable
{
    public string? Test { get; set; }
    public string? Parameter { get; set; }
    public string? Acceptance_Criteria { get; set; }
    public string? Observations { get; set; }
    public int DropTest_Id { get; set; }
}

[Table("tbl_DropTest_ImgDetails")]
public class DropTestReportImgDetail : SqlTable
{
    public string? Before_Img { get; set; }
    public string? After_Img { get; set; }
    public int DropTest_Id { get; set; }
}


