using System;

namespace QMS.Core.Models
{
    public class SixSigmaIndicesViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Fy { get; set; }
        public string? Pc { get; set; }
        public string? Location { get; set; }
        public double? Lean_Data_Pc_No_People { get; set; }
        public double? Lean_Data_Pc_Total_People { get; set; }
        public double? Lean_Target { get; set; }
        public double? Lean_Weightage { get; set; }
        public string? Lean_Formula { get; set; }
        public double? Lean_Weighted_Score { get; set; }

        public double? Six_Sigma_Data_Pc_Proj { get; set; }
        public double? Six_Sigma_Data_Pc_Ann { get; set; }
        public double? Six_Sigma_Target { get; set; }
        public double? Six_Sigma__Weightage { get; set; }
        public string? Six_Sigma_Formula { get; set; }
        public double? Six_Sigma_Weighted_Score { get; set; }

        public double? Review_Data_Pc_Proj_Review { get; set; }
        public double? Review_Data_Pc_Proj_Running { get; set; }
        public double? Review_Target { get; set; }
        public double? Review_Weightage { get; set; }
        public string? Review_Formula { get; set; }
        public double? Review_Weighted_Score { get; set; }

        public double? Market_Data_Pc_Cap_Cso { get; set; }
        public double? Market_Data_Pc_Tar_Cso { get; set; }
        public double? Market_Target { get; set; }
        public double? Market_Weightage { get; set; }
        public string? Market_Formula { get; set; }
        public double? Market_Weighted_Score { get; set; }

        public double? Bkpt_Data_Pc_Ytd { get; set; }
        public double? Bkpt_Data_Pc_Ann_Ytd { get; set; }
        public double? Bkpt_Data_Pc_Mfg { get; set; }
        public double? Bkpt_Data_Pc_Ann_Mfg { get; set; }
        public double? Bkpt_Target_Ytd { get; set; }
        public double? Bkpt_Weightage_Ytd { get; set; }
        public string? Bkpt_Formula_Ytd { get; set; }
        public double? Bkpt_Weighted_Score_Ytd { get; set; }
        public double? Bkpt_Target_Mfg { get; set; }
        public double? Bkpt_Weightage_Mfg { get; set; }
        public string? Bkpt_Formula_Mfg { get; set; }
        public double? Bkpt_Weighted_Score_Mfg { get; set; }
        public double? Eng_Total_Weighted_Score { get; set; }
        public double? Eng_Total_Sum { get; set; }
        public double? Cust_Data_Pc_Ot { get; set; }
        public double? Cust_Data_Pc_As { get; set; }
        public double? Cust_Data_Pc_Close { get; set; }
        public double? Cust_Data_Pc_Total { get; set; }
        public double? Cust_Target_Tgt { get; set; }
        public double? Cust_Weightage_Tgt { get; set; }
        public string? Cust_Formula_Tgt { get; set; }
        public double? Cust_Weighted_Score_Tgt { get; set; }
        //busines factro-1
        public double? Cust_Target_Ytd { get; set; }
        public double? Cust_Weightage_Ytd { get; set; }
        public string? Cust_Formula_Ytd { get; set; }
        public double? Cust_Weighted_Score_Ytd { get; set; }

        public double? Proc_Data_Pc_Npd { get; set; }
        public double? Proc_Data_Pc_Ann { get; set; }
        public double? Proc_Target { get; set; }
        public double? Proc_Weightage { get; set; }
        public string? Proc_Formula { get; set; }
        public double? Proc_Weighted_Score { get; set; }
        public double? Key_Data_Pc_Cust { get; set; }
        public double? Key_Data_Pc_Ann { get; set; }
        public double? Key_Target { get; set; }
        public double? Key_Weightage { get; set; }
        public string? Key_Formula { get; set; }
        public double? Key_Weighted_Score { get; set; }
        public double? Save_Data_Pc_Actu { get; set; }
        public double? Save_Data_Pc_Ann { get; set; }
        public double? Save_Target { get; set; }
        public double? Save_Weightage { get; set; }
        public string? Save_Formula { get; set; }
        public double? Save_Weighted_Score { get; set; }

        public double? Out_Data_Pc_Sigma { get; set; }
        public double? Out_Data_Pc_As { get; set; }
        public double? Out_Target { get; set; }
        public double? Out_Weightage { get; set; }
        public string? Out_Formula { get; set; }
        public double? Out_Weighted_Score { get; set; }

        public double? Spm_Data_Pc_Spm { get; set; }
        public double? Spm_Data_Pc_As { get; set; }
        public double? Spm_Target { get; set; }
        public double? Spm_Weightage { get; set; }
        public string? Spm_Formula { get; set; }
        public double? Spm_Weighted_Score { get; set; }

        public double? Proj_Data_Pc_Close { get; set; }
        public double? Proj_Data_Pc_Dmaic { get; set; }
        public double? Proj_Data_Pc_Turbo { get; set; }
        public double? Proj_Data_Pc_Planned { get; set; }

        public double? Proj_Num_Target { get; set; }
        public double? Proj_Num_Weightage { get; set; }
        public string? Proj_Num_Formula { get; set; }
        public double? Proj_Num_Weighted_Score { get; set; }
        public double? Proj_No_Weighted_Score { get; set; }
        public string? Proj_No_Formula { get; set; }
        public double? Proj_No_Weightage { get; set; }
        public double? Proj_No_Target { get; set; }


        public double? Eff_Total_Weighted_Score { get; set; }
        public double? Eff_Total_Sum { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
