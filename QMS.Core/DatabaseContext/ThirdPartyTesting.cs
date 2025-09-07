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
    [Table("tbl_ThirdParty_Testing")]
    public class ThirdPartyTesting : SqlTable
    {
        [Key]
        [Column("ThirdPartyTest_ID")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }

        public string? Purpose { get; set; }
        public string? Project_Det { get; set; }
        public string? Product_Det { get; set; }
        public string? Wipro_Product_Code { get; set; }
        public int? Sample_Qty { get; set; }
        public string? Test_Detail { get; set; }
        public string? Project_Initiator { get; set; }
        public string? Vendor { get; set; }
        public string? Lab { get; set; }
        public string? Sample_Status { get; set; }
        public string? Testing_Status { get; set; }
        public string? Lab_Contact_Person { get; set; }
        public string? Contact_Number { get; set; }
        public string? Email_Id { get; set; }
        public string? Testing_Charge_offer { get; set; }
        public string? Final_Testing_Charge { get; set; }
        public string? Report { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
