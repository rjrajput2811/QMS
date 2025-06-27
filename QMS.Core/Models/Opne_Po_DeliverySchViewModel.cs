using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class Opne_Po_DeliverySchViewModel
    {
        public  int Id { get; set; }
        public  bool Deleted { get; set; }
        public int Ven_PoId { get; set; }
        public string? Vendor { get; set; }
        public string? PO_No { get; set; }
        public DateTime? PO_Date { get; set; }
        public int? PO_Qty { get; set; }
        public int? Balance_Qty { get; set; }

        //public DateTime? Delivery_Date { get; set; }
        //public int? Delivery_Qty { get; set; }
        //public string? Delivery_Remark { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<DeliveryScheduleItem> DeliveryScheduleList { get; set; } = new();
    }

    public class DeliveryScheduleItem
    {
        public int SrNo { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public int? Delivery_Qty { get; set; }
        public string? Delivery_Remark { get; set; }
    }
}
