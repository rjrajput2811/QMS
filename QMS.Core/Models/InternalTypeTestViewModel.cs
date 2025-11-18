using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QMS.Core.ViewModels
{
    public class InternalTypeTestViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }

        public string? Report_No { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now;

        public string? Cust_Name { get; set; }

        public string? Samp_Identi_Lab { get; set; }

        public string? Samp_Desc { get; set; }

        public string? Prod_Cat_Code { get; set; }

        public string? Input_Voltage { get; set; }

        public string? Ref_Standard { get; set; }

        public string? TestedBy { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public List<InternalTypeTestDetailViewModel> Details { get; set; } = new();

        public string? RowsJson { get; set; }
    }

    public class InternalTypeTestDetailViewModel
    {
        public int Id { get; set; }

        public int? Internal_TypeId { get; set; }

        public int? SeqNo { get; set; }

        public string? Perticular_Test { get; set; }

        public string? Test_Method { get; set; }

        public string? Test_Requirement { get; set; } 

        public string? Test_Result { get; set; }

        public bool Deleted { get; set; } = false;

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
