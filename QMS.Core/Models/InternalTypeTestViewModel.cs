using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QMS.Core.ViewModels
{
    // =======================
    // PARENT VIEW MODEL
    // =======================
    public class InternalTypeTestViewModel
    {
        // optional DB identity returned after insert
        public int Id { get; set; }
        public bool Deleted { get; set; } = false;

        public string? Report_No { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now;

        public string? Cust_Name { get; set; }

        public string? Samp_Identi_Lab { get; set; }

        public string? Samp_Desc { get; set; }

        public string? Prod_Cat_Code { get; set; }

        public string? Input_Voltage { get; set; }

        public string? Ref_Standard { get; set; }

        public string? TestedBy { get; set; }

        // SP expects CreatedBy NVARCHAR(500)
        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


        // Child detail rows (initialized to avoid null ref)
        public List<InternalTypeTestDetailViewModel> Details { get; set; } = new();

        // if you still post a RowsJson string from the view, keep this optional property
        public string? RowsJson { get; set; }
    }

    // =======================
    // CHILD VIEW MODEL
    // =======================
    public class InternalTypeTestDetailViewModel
    {
        public int InternalType_DetId { get; set; }

        // optional FK if you maintain it client-side
        public int? Internal_TypeId { get; set; }

        // name used by TVP is SeqNo
        [JsonPropertyName("SrNo")]                 // map client's SrNo -> SeqNo
        public int? SeqNo { get; set; }

        // map JSON property ParticularOfTest -> Perticular_Test (DB column)
        [JsonPropertyName("ParticularOfTest")]
        public string? Perticular_Test { get; set; }

        [JsonPropertyName("TestMethod")]
        public string? Test_Method { get; set; }

        [JsonPropertyName("TestRequirement")]
        public string? Test_Requirement { get; set; } // keep HTML if you want

        [JsonPropertyName("TestResult")]
        public string? Test_Result { get; set; }

        // TVP expects IsDeleted column; default false
        [JsonPropertyName("IsDeleted")]
        public bool IsDeleted { get; set; } = false;

        [JsonPropertyName("CreatedBy")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
