using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_GeneralObservationReport")]
public class GeneralObservationReport : SqlTable
{
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }

    public string? Row1_Requirement { get; set; }
    public string? Row1_ActualFindings { get; set; }
    public string? Row1_OpenClosed { get; set; }
    public string? Row1_ClosureResponsibility { get; set; }
    public string? Row1_UploadFile { get; set; }

    public string? Row2_Requirement { get; set; }
    public string? Row2_ActualFindings { get; set; }
    public string? Row2_OpenClosed { get; set; }
    public string? Row2_ClosureResponsibility { get; set; }
    public string? Row2_UploadFile { get; set; }

    public string? Row3_Requirement { get; set; }
    public string? Row3_ActualFindings { get; set; }
    public string? Row3_OpenClosed { get; set; }
    public string? Row3_ClosureResponsibility { get; set; }
    public string? Row3_UploadFile { get; set; }

    public string? Row4_Requirement { get; set; }
    public string? Row4_ActualFindings { get; set; }
    public string? Row4_OpenClosed { get; set; }
    public string? Row4_ClosureResponsibility { get; set; }
    public string? Row4_UploadFile { get; set; }

    public string? Row5_Requirement { get; set; }
    public string? Row5_ActualFindings { get; set; }
    public string? Row5_OpenClosed { get; set; }
    public string? Row5_ClosureResponsibility { get; set; }
    public string? Row5_UploadFile { get; set; }

    public string? Row6_Requirement { get; set; }
    public string? Row6_ActualFindings { get; set; }
    public string? Row6_OpenClosed { get; set; }
    public string? Row6_ClosureResponsibility { get; set; }
    public string? Row6_UploadFile { get; set; }

    public string? Row7_Requirement { get; set; }
    public string? Row7_ActualFindings { get; set; }
    public string? Row7_OpenClosed { get; set; }
    public string? Row7_ClosureResponsibility { get; set; }
    public string? Row7_UploadFile { get; set; }

    public string? Row8_Requirement { get; set; }
    public string? Row8_ActualFindings { get; set; }
    public string? Row8_OpenClosed { get; set; }
    public string? Row8_ClosureResponsibility { get; set; }
    public string? Row8_UploadFile { get; set; }

    public string? Row9_Requirement { get; set; }
    public string? Row9_ActualFindings { get; set; }
    public string? Row9_OpenClosed { get; set; }
    public string? Row9_ClosureResponsibility { get; set; }
    public string? Row9_UploadFile { get; set; }

    public string? Row10_Requirement { get; set; }
    public string? Row10_ActualFindings { get; set; }
    public string? Row10_OpenClosed { get; set; }
    public string? Row10_ClosureResponsibility { get; set; }
    public string? Row10_UploadFile { get; set; }

    public string? Row11_Requirement { get; set; }
    public string? Row11_ActualFindings { get; set; }
    public string? Row11_OpenClosed { get; set; }
    public string? Row11_ClosureResponsibility { get; set; }
    public string? Row11_UploadFile { get; set; }

    public string? Row12_Requirement { get; set; }
    public string? Row12_ActualFindings { get; set; }
    public string? Row12_OpenClosed { get; set; }
    public string? Row12_ClosureResponsibility { get; set; }
    public string? Row12_UploadFile { get; set; }

    public string? Row13_Requirement { get; set; }
    public string? Row13_ActualFindings { get; set; }
    public string? Row13_OpenClosed { get; set; }
    public string? Row13_ClosureResponsibility { get; set; }
    public string? Row13_UploadFile { get; set; }

    public string? Row14_Requirement { get; set; }
    public string? Row14_ActualFindings { get; set; }
    public string? Row14_OpenClosed { get; set; }
    public string? Row14_ClosureResponsibility { get; set; }
    public string? Row14_UploadFile { get; set; }

    public string? Row15_Requirement { get; set; }
    public string? Row15_ActualFindings { get; set; }
    public string? Row15_OpenClosed { get; set; }
    public string? Row15_ClosureResponsibility { get; set; }
    public string? Row15_UploadFile { get; set; }

    public string? Row16_Requirement { get; set; }
    public string? Row16_ActualFindings { get; set; }
    public string? Row16_OpenClosed { get; set; }
    public string? Row16_ClosureResponsibility { get; set; }
    public string? Row16_UploadFile { get; set; }

    public string? Row17_Requirement { get; set; }
    public string? Row17_ActualFindings { get; set; }
    public string? Row17_OpenClosed { get; set; }
    public string? Row17_ClosureResponsibility { get; set; }
    public string? Row17_UploadFile { get; set; }

    public string? Row18_Requirement { get; set; }
    public string? Row18_ActualFindings { get; set; }
    public string? Row18_OpenClosed { get; set; }
    public string? Row18_ClosureResponsibility { get; set; }
    public string? Row18_UploadFile { get; set; }

    public string? Row19_Requirement { get; set; }
    public string? Row19_ActualFindings { get; set; }
    public string? Row19_OpenClosed { get; set; }
    public string? Row19_ClosureResponsibility { get; set; }
    public string? Row19_UploadFile { get; set; }

    public string? Row20_Requirement { get; set; }
    public string? Row20_ActualFindings { get; set; }
    public string? Row20_OpenClosed { get; set; }
    public string? Row20_ClosureResponsibility { get; set; }
    public string? Row20_UploadFile { get; set; }

    public string? Row21_Requirement { get; set; }
    public string? Row21_ActualFindings { get; set; }
    public string? Row21_OpenClosed { get; set; }
    public string? Row21_ClosureResponsibility { get; set; }
    public string? Row21_UploadFile { get; set; }

    public string? Row22_Requirement { get; set; }
    public string? Row22_ActualFindings { get; set; }
    public string? Row22_OpenClosed { get; set; }
    public string? Row22_ClosureResponsibility { get; set; }
    public string? Row22_UploadFile { get; set; }

    public string? Row23_Requirement { get; set; }
    public string? Row23_ActualFindings { get; set; }
    public string? Row23_OpenClosed { get; set; }
    public string? Row23_ClosureResponsibility { get; set; }
    public string? Row23_UploadFile { get; set; }

    public string? Row24_Requirement { get; set; }
    public string? Row24_ActualFindings { get; set; }
    public string? Row24_OpenClosed { get; set; }
    public string? Row24_ClosureResponsibility { get; set; }
    public string? Row24_UploadFile { get; set; }

    public string? Row25_Requirement { get; set; }
    public string? Row25_ActualFindings { get; set; }
    public string? Row25_OpenClosed { get; set; }
    public string? Row25_ClosureResponsibility { get; set; }
    public string? Row25_UploadFile { get; set; }

    public string? Row26_Requirement { get; set; }
    public string? Row26_ActualFindings { get; set; }
    public string? Row26_OpenClosed { get; set; }
    public string? Row26_ClosureResponsibility { get; set; }
    public string? Row26_UploadFile { get; set; }

    public string? Row27_Requirement { get; set; }
    public string? Row27_ActualFindings { get; set; }
    public string? Row27_OpenClosed { get; set; }
    public string? Row27_ClosureResponsibility { get; set; }
    public string? Row27_UploadFile { get; set; }

    public string? Row28_Requirement { get; set; }
    public string? Row28_ActualFindings { get; set; }
    public string? Row28_OpenClosed { get; set; }
    public string? Row28_ClosureResponsibility { get; set; }
    public string? Row28_UploadFile { get; set; }

    public string? Row29_Requirement { get; set; }
    public string? Row29_ActualFindings { get; set; }
    public string? Row29_OpenClosed { get; set; }
    public string? Row29_ClosureResponsibility { get; set; }
    public string? Row29_UploadFile { get; set; }

    public string? Row30_Requirement { get; set; }
    public string? Row30_ActualFindings { get; set; }
    public string? Row30_OpenClosed { get; set; }
    public string? Row30_ClosureResponsibility { get; set; }
    public string? Row30_UploadFile { get; set; }

    public string? CheckedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
