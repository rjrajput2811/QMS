using Microsoft.AspNetCore.Http;
using QMS.Core.DatabaseContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class DropTestViewModel
    {
        public int Id { get; set; }
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
        public string? User { get; set; }
        public bool Deleted { get; set; }
        public List<DropTestReportDetailViewModel>? Details { get; set; } = new();
        public List<DropTestReportImgDetailViewModel>? ImgDetails { get; set; } = new();
    }

    public class DropTestReportDetailViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Test { get; set; }
        public string? Parameter { get; set; }
        public string? Acceptance_Criteria { get; set; }
        public string? Observations { get; set; }
        public int DropTest_Id { get; set; }

    }

    public class DropTestReportImgDetailViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Before_Img { get; set; }
        public string? After_Img { get; set; }
        public int DropTest_Id { get; set; }

        public IFormFile? Before_ImgFile { get; set; }
         public IFormFile? After_ImgFile { get; set; }

    }
}
