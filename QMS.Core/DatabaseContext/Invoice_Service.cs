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
    [Table("tbl_InvoiceList_Service")]
    public class Invoice_Service: SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Inv_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Key { get; set; }
        public string? Inv_No { get; set; }
        public DateTime? Inv_Date { get; set; }
        public string? Inv_Type { get; set; }
        public string? Sales_Order { get; set; }
        public string? Plant_Code { get; set; }
        public string? Plant_Name { get; set; }
        public string? Material_No { get; set; }
        public string? Dealer_Name { get; set; }
        public string? End_Customer { get; set; }
        public string? Collective_No { get; set; }
        public string? Indent_No { get; set; }
        public string? Quantity { get; set; }
        public string? Cost { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
