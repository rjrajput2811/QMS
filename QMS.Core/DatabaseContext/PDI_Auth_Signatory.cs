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
    [Table("tbl_PDI_Auth_Signatory")]
    public class PDI_Auth_Signatory : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Pdi_Auth_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

      public string? Vendor {get; set;}
      public string? Address {get; set;}
      public string? Pdi_Inspector {get; set;}
      public string? Designation {get; set;}
      public string? Photo_Inspector {get; set;}
      public string? Specimen_Sign {get; set;}
      public string? Remark {get; set;}
      public DateTime? CreatedDate {get; set;}
      public string? CreatedBy {get; set;}
      public DateTime? UpdatedDate {get; set;}
      public string? UpdatedBy {get; set;}
    }
}
