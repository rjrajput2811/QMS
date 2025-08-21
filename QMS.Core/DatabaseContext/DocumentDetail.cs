using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_Document_Config")]
    public class DocumentDetail :SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Doc_Id")]
        public override int Id { get; set; }
        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Document_No {get; set;}
        public string? Revision_No {get; set;}
        public DateTime? Effective_Date {get; set;}
        public string? Type {get; set;}
        public DateTime? Revision_Date {get; set;}
        public string? CreatedBy {get; set;}
        public DateTime? CreatedDate {get; set;}
        public string? UpdatedBy {get; set;}
        public DateTime? UpdatedDate {get; set;}
    }
}
