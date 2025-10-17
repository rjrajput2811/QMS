using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QMS.Core.Models
{
    public class ThirdPartyInspectionViewModel
    {
        public int InspectionID { get; set; }

        public DateTime? InspectionDate { get; set; }

        public string? ProjectName { get; set; }

        public string? InspName { get; set; }

        public string? ProductCode { get; set; }
        public string? ProdDesc { get; set; }

        public string? LOTQty { get; set; }

        public string? ProjectValue { get; set; }

        public string? Location { get; set; }
        public string? Mode { get; set; }

        public string? FirstAttempt { get; set; }

        public string? Remark { get; set; }

        public string? ActionPlan { get; set; }

        public DateTime? MOMDate { get; set; }

        public List<IFormFile>? AttachmentFiles { get; set; }

        public string? Attachment { get; set; }
        public string? Tpi_Duration { get; set; }
        public string? Pc { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}