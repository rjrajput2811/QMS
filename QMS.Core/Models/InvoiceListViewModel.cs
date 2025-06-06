using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class InvoiceListViewModel
    {
        public int Id { get; set; }

        public bool Deleted { get; set; }
        
        public string? Key { get; set; }
        public string? Inv_No { get; set; }
        public DateTime? Inv_Date { get; set; }
        public string? Inv_Type { get; set; }
        public string? Sales_Order { get; set; }
        public string? Plant_Code { get; set; }
        public string? Material_No { get; set; }
        public string? Description { get; set; }
        public string? Batch { get; set; }
        public string? Customer { get; set; }
        public string? Customer_Name { get; set; }
        public string? Name { get; set; }
        public string? Collective_No { get; set; }
        public string? Reference { get; set; }
        public string? Quantity { get; set; }
        public string? Cost { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
