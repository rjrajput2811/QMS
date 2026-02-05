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
    public class GeneralObservationViewModel
    {
        public int Id { get; set; }
        public string? ProductCatRef { get; set; }
        public string? ProductDescription { get; set; }
        public string? ReportNo { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? CheckedBy { get; set; }
        public string? VerifiedBy { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? User { get; set; }
        public bool Deleted { get; set; }
        public List<GeneralObservationDetailViewModel>? Details { get; set; } = new();
    }

    public class GeneralObservationDetailViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Req_Spec { get; set; }
        public string? Actual_find { get; set; }
        public string? Open_Close { get; set; }
        public string? Closure_Respons { get; set; }
        public string? Attachment { get; set; }
        public int GenObs_Id { get; set; }
        [NotMapped] public IFormFile? AttachmentFile { get; set; }
    }

}
