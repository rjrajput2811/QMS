using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class JobWork_TracViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Vendor { get; set; }
        public string? Wipro_Dc_No { get; set; }
        public DateTime? Wipro_Dc_Date { get; set; }
        public string? Dc_Sap_Code { get; set; }
        public int? Qty_Wipro_Dc { get; set; }
        public string? Wipro_Transporter { get; set; }
        public string? Wipro_LR_No { get; set; }
        public DateTime? Wipro_LR_Date { get; set; }
        public int? Actu_Rece_Qty { get; set; }
        public string? Dispatch_Dc { get; set; }
        public string? Dispatch_Invoice { get; set; }
        public string? Non_Repairable { get; set; }
        public string? Grand_Total { get; set; }
        public string? To_Process { get; set; }
        public string? Remark { get; set; }
        public string? Vendor_Transporter { get; set; }
        public string? Vendor_LR_No { get; set; }
        public DateTime? Vendor_LR_Date { get; set; }
        public string? Write_Off_Approved { get; set; }
        public DateTime? Write_Off_Date { get; set; }
        public string? Pending_Write_Off { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
