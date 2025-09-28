using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class Csat_CommentViewModel
    {
        public  int Id { get; set; }
        public  bool Deleted { get; set; }
        public string? Quarter { get; set; }
        public string? Organisation { get; set; }
        public string? Region { get; set; }
        public string? Q1 { get; set; }
        public string? Q2 { get; set; }
        public string? Q3 { get; set; }
        public string? Q4 { get; set; }
        public string? Q5 { get; set; }
        public string? Q6 { get; set; }
        public string? Q7 { get; set; }
        public string? Q8 { get; set; }
        public string? Q9 { get; set; }
        public string? Q10 { get; set; }
        public string? Q11 { get; set; }
        public string? Q12 { get; set; }
        public string? Q13 { get; set; }
        public string? Cust_Critical_Aspect { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
