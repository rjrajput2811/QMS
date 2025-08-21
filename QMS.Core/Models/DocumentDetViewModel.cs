using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class DocumentDetViewModel
    {
        public int Id { get; set; }
        public  bool Deleted { get; set; }
        public string? Document_No { get; set; }
        public string? Revision_No { get; set; }
        public DateTime? Effective_Date { get; set; }
        public string? Type { get; set; }
        public DateTime? Revision_Date { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
