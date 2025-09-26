using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Core.Models
{
    public class KaizenTracViewModel
    {
        public int Id { get; set; }

        public bool? Deleted { get; set; }

        public string? Vendor { get; set; }

        public string? Kaizen_Theme { get; set; }

        public string? Month { get; set; }

        public string? Team { get; set; }

        public string? Kaizen_Attch { get; set; }

        public string? Remark { get; set; }

        public string? FY { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

    }
}
