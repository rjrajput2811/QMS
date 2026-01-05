using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class RM_TCViewModel
    {
        public  int Id { get; set; }
        public  bool Deleted { get; set; }
        public string? Vendor { get; set; }
        public string? Product_No { get; set; }
        public string? ProdDesc { get; set; }
        public DateTime? Date { get; set; }
        public string? Housing_Body { get; set; }
        public string? Wires_Cable { get; set; }
        public string? Diffuser_Lens { get; set; }
        public string? Pcb { get; set; }
        public string? Connectors { get; set; }
        public string? Powder_Coat { get; set; }
        public string? Led_LM80 { get; set; }
        public string? Led_Purchase_Proof { get; set; }
        public string? Driver { get; set; }
        public string? Pre_Treatment { get; set; }
        public string? Hardware { get; set; }
        public string? Other_Critical_Items { get; set; }
        public string? Attchment { get; set; }
        public string? Remarks { get; set; }

        public string? Housing_Body_Attch { get; set; }
        public string? Wires_Cable_Attch { get; set; }
        public string? Diffuser_Lens_Attch { get; set; }
        public string? Pcb_Attch { get; set; }
        public string? Connectors_Attch { get; set; }
        public string? Powder_Coat_Attch { get; set; }
        public string? Led_LM80_Attch { get; set; }
        public string? Led_Purchase_Proof_Attch { get; set; }
        public string? Driver_Attch { get; set; }
        public string? Pre_Treatment_Attch { get; set; }
        public string? Hardware_Attch { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class UploadAttachmentResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public int id { get; set; }
        public string? fileName { get; set; }
        public string? field { get; set; }
    }
}
