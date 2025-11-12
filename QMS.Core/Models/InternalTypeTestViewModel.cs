using System;
using System.Collections.Generic;

namespace QMS.Core.ViewModels
{
    // =======================
    // PARENT VIEW MODEL
    // =======================
    public class InternalTypeTestViewModel
    {
        public int Internal_TypeId { get; set; }
        public string? Report_No { get; set; }
        public DateTime? Date { get; set; }
        public string? Cust_Name { get; set; }
        public string? Samp_Identi_Lab { get; set; }
        public string? Samp_Desc { get; set; }
        public string? Prod_Cat_Code { get; set; }
        public string? Input_Voltage { get; set; }
        public string? Ref_Standard { get; set; }
        public string? TestedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // List of child test details from the HTML form
        public List<InternalTypeTestDetailViewModel>? Details { get; set; }
    }

    // =======================
    // CHILD VIEW MODEL
    // =======================
    public class InternalTypeTestDetailViewModel
    {
        public int InternalType_DetId { get; set; }
        public int? Internal_TypeId { get; set; }  // For linking to parent (optional)
        public int? SeqNo { get; set; }
        public string? Perticular_Test { get; set; }
        public string? Test_Method { get; set; }
        public string? Test_Requirement { get; set; }
        public string? Test_Result { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
