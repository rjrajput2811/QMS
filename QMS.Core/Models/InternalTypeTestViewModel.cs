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
        public int Internal_TypeId { get; set; }

        [JsonPropertyName("Report_No")]
        public string? Report_No { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now;

        [JsonPropertyName("Cust_Name")]
        public string? Cust_Name { get; set; }

        [JsonPropertyName("Samp_Identi_Lab")]
        public string? Samp_Identi_Lab { get; set; }

        [JsonPropertyName("Samp_Desc")]
        public string? Samp_Desc { get; set; }

        [JsonPropertyName("Prod_Cat_Code")]
        public string? Prod_Cat_Code { get; set; }

        [JsonPropertyName("Input_Voltage")]
        public string? Input_Voltage { get; set; }

        [JsonPropertyName("Ref_Standard")]
        public string? Ref_Standard { get; set; }

        [JsonPropertyName("TestedBy")]
        public string? TestedBy { get; set; }

        // SP expects CreatedBy NVARCHAR(500)
        [JsonPropertyName("CreatedBy")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        // make audit fields nullable so new models are easy to create
        public int? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Child detail rows (initialized to avoid null ref)
        [JsonPropertyName("Details")]
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

        // audit fields - nullable
        public int? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
