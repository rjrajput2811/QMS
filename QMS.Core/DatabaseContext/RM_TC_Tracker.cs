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
    [Table("tbl_RM_TC_Tracker")]
    public class RM_TC_Tracker :SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Rm_Tc_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Vendor { get; set; }
        public string? Product_No { get; set; }
        public string? ProdDesc { get; set; }
        public DateTime? Date { get; set; }
        public string? Housing_Body { get; set; }
        public string? Wires_Cable { get; set; }
        public string? Diffuser_Lens { get; set; }
        public string? Pcb { get; set; }
        public string? Connectors { get; set; }
        public string? Powder_Coat { get; set; }
        public string? Led_LM80 { get; set; }
        public string? Led_Purchase_Proof { get; set; }
        public string? Driver { get; set; }
        public string? Pre_Treatment { get; set; }
        public string? Hardware { get; set; }
        public string? Other_Critical_Items { get; set; }
        public string? Attchment { get; set; }     
        public string? Remarks { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
