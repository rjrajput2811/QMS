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
        public string? Key { get; set; }
        public string? Key1 { get; set; }

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
        public string? Date_PC_Week { get; set; }
    }

    public class OpenPoDeliveryExcelRow
    {
        public int ExcelRowNo { get; set; }
        public string Key { get; set; } = "";
        public string? Vendor { get; set; }
        public string? PO_No { get; set; }
        public DateTime? PO_Date { get; set; }

        public int? PO_Qty { get; set; }
        public int? BalanceQty { get; set; }

        public DateTime? Delivery_Date { get; set; }
        public string? Date_PC_Week { get; set; }
        public int? Qty { get; set; }
        public string? Remark { get; set; }
    }

}
