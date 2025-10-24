using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class Sales_Order_ViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? SO_No { get; set; }
        public string? SaleOrder_Type { get; set; }
        public DateTime? SO_Date { get; set; }
        public int? Line_Item { get; set; }
        public string? Indent_No { get; set; }
        public DateTime? Indent_Date { get; set; }
        public string? Order_Type { get; set; }
        public string? Vertical { get; set; }
        public string? Region { get; set; }
        public string? Sales_Group { get; set; }
        public string? Sales_Group_desc { get; set; }
        public string? Sales_Office { get; set; }
        public string? Sales_Office_Desc { get; set; }
        public string? Sale_Person { get; set; }
        public string? Project_Name { get; set; }
        public string? Project_Name_Tag { get; set; }
        public string? Priority_Tag { get; set; }
        public string? Customer_Code { get; set; }
        public string? Customer_Name { get; set; }
        public string? Dealer_Direct { get; set; }
        public string? Inspection { get; set; }
        public string? Material { get; set; }
        public string? Old_Material_No { get; set; }
        public string? Description { get; set; }
        public int? SO_Qty { get; set; }
        public double? SO_Value { get; set; }
        public double? Rate { get; set; }
        public int? Del_Qty { get; set; }
        public int? Open_Sale_Qty { get; set; }
        public double? Opne_Sale_Value { get; set; }
        public string? Plant { get; set; }
        public string? Item_Category { get; set; }
        public string? Item_Category_Latest { get; set; }
        public string? Procurement_Type { get; set; }
        public string? Vendor_Po_No { get; set; }
        public DateTime? Vendor_Po_Date { get; set; }
        public int? CPR_Number { get; set; }
        public string? Vendor { get; set; }
        public string? Planner { get; set; }
        public int? Po_Release_Qty { get; set; }
        public int? Allocated_Stock_Qty { get; set; }
        public double? Allocated_Stock_Value { get; set; }
        public int? Net_Qty { get; set; }
        public double? Net_Value { get; set; }
        public int? Qty_In_Week { get; set; }
        public double? Value_In_Week { get; set; }
        public int? Qty_After_Week { get; set; }
        public double? Value_After_Week { get; set; }
        public bool? Check5 { get; set; }
        public string? Indent_Status { get; set; }
        public string? Sales_Call_Point { get; set; }
        public int? Free_Stock { get; set; }
        public int? Grn_Qty { get; set; }
        public DateTime? Last_Grn_Date { get; set; }
        public string? Check1 { get; set; }
        public string? Delivery_Schedule { get; set; }
        public string? Readiness_Vendor_Released_Fr_Date { get; set; }
        public string? Readiness_Vendor_Released_To_Date { get; set; }
        public string? Readiness_Schedule_Vendor_Released { get; set; }
        public string? Delivery_Schedule_PC_Breakup { get; set; }
        public string? Check2 { get; set; }
        public string? Line_Item_Schedule { get; set; }
        public string? R_B { get; set; }
        public string? Schedule_Repeat { get; set; }
        public string? Internal_Pending_Issue { get; set; }
        public string? Pending_With { get; set; }
        public string? Remark { get; set; }
        public string? CRD_OverDue { get; set; }
        public DateTime? Delivert_Date { get; set; }
        public string? Process_Plan_On_Crd { get; set; }
        public string? Last_Week_PC { get; set; }
        public int? Schedule_Line_Qty1 { get; set; }
        public DateTime? Schedule_Line_Date1 { get; set; }
        public int? Schedule_Line_Qty2 { get; set; }
        public DateTime? Schedule_Line_Date2 { get; set; }
        public int? Schedule_Line_Qty3 { get; set; }
        public DateTime? Schedule_Line_Date3 { get; set; }
        public string? To_Consider { get; set; }
        public string? Person_Name { get; set; }
        public string? Visibility { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Key { get; set; }
        public string? Key1 { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }


    public class MatchedRecordViewModel
    {
        public string? SO_Key { get; set; }
        public string? PO_Key { get; set; }
        public string? SO_Key1 { get; set; }
        public string? PO_Key1 { get; set; }
        public string? PO_No { get; set; }
        public DateTime? PO_Date { get; set; }
        public decimal? PO_Qty { get; set; }

        //public DateTime? Delivery_Date { get; set; }
        public string? Item_Category { get; set; }
        public string? SO_No { get; set; }
        public string? SO_SaleOrder_Type { get; set; }
        public DateTime? SO_Date { get; set; }
        public int? Line_Item { get; set; }
        public string? Indent_No { get; set; }
        public DateTime? Indent_Date { get; set; }
        public int? SO_Qty { get; set; }
        public string? PO_Material { get; set; }
        public string? SO_Material { get; set; }
        public string? Sales_Group { get; set; }
        public string? Sales_Group_desc { get; set; }
        public string? Sales_Office { get; set; }
        public string? Sales_Office_Desc { get; set; }
        public string? Sale_Person { get; set; }
        public string? Project_Name { get; set; }
        public string? Customer_Code { get; set; }
        public string? Customer_Name { get; set; }
        public string? Material { get; set; }
        public string? Old_Material_No { get; set; }
        public string? Description { get; set; }
        public double? SO_Value { get; set; }
        public int? Del_Qty { get; set; }
        public int? Open_Sale_Qty { get; set; }
        public double? Opne_Sale_Value { get; set; }
        public string? Plant { get; set; }
        public string? Procurement_Type { get; set; }
        public string? Vendor_Po_No { get; set; }
        public DateTime? Vendor_Po_Date { get; set; }
        public int? Po_Release_Qty { get; set; }
        public int? Allocated_Stock_Qty { get; set; }
        public double? Allocated_Stock_Value { get; set; }
        public int? Net_Qty { get; set; }
        public double? Net_Value { get; set; }
        public string? Indent_Status { get; set; }
        public string? Delivery_Schedule { get; set; }
        public string? Readiness_Schedule_Vendor_Released { get; set; }
        public DateTime? Delivert_Date { get; set; }
        public int? Schedule_Line_Qty1 { get; set; }
        public DateTime? Schedule_Line_Date1 { get; set; }
        public int? Schedule_Line_Qty2 { get; set; }
        public DateTime? Schedule_Line_Date2 { get; set; }
        public int? Schedule_Line_Qty3 { get; set; }
        public DateTime? Schedule_Line_Date3 { get; set; }
        public int? Total_Scheduled_Qty { get; set; }
        public DateTime? Last_Schedule_Updated { get; set; }
        public string DS_Delivery_Date { get; set; }
        public string DS_Delivery_Qty { get; set; }
        public string DS_Date_PC_Week { get; set; }
        public string DS_Comit_Qty { get; set; }
        public string DS_Comit_Qty1 { get; set; }
        public string DS_Comit_Final_Qty { get; set; }
        public string DS_Comit_Date { get; set; }
        public string DS_Comit_Date1 { get; set; }
        public string DS_Comit_Final_Date { get; set; }
        public string DS_Week_Qty { get; set; }

        public string? Delivery_Date { get; set; }
        public int Delivery_Qty { get; set; }
        public string? Date_PC_Week { get; set; }

        public string? Comit_Date { get; set; }
        public int Comit_Qty { get; set; }
        public string? Comit_Date_PC_Week { get; set; }
        public string? Comit_Date1 { get; set; }
        public int Comit_Qty1 { get; set; }
        public string? Comit_Date_PC_Week1 { get; set; }

        public string? Final_Comit_Date { get; set; }
        public int Final_Comit_Qty { get; set; }
        public string? Final_Comit_Date_PC_Week { get; set; }
    }

    public class MatchSummaryViewModel
    {
        public int Total_PO_Count { get; set; }
        public int Total_SO_Count { get; set; }
        public int Matched_Count { get; set; }
        public int Not_Matched_PO_Count { get; set; }
        public int Not_Matched_SO_Count { get; set; }
    }

    public class MatchDeliverySchViewModel
    {
        public string? DS_PO_No { get; set; }
        public DateTime? DS_PO_Date { get; set; }
        public int? DS_PO_Qty { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public int? Delivery_Qty { get; set; }
        public string? Date_PC_Week { get; set; }
        public string? DS_Key { get; set; }
        public string? DS_Key1 { get; set; }
    }

    public class MatchSODeliverySchViewModel
    {
        public int? SaleOrder_Id { get; set; }
        public string? Material { get; set; }
        public string? Old_Material_No { get; set; }
        public DateTime? SO_Date { get; set; }
        public int? Net_Qty { get; set; }
        public int? Delivery_Qty { get; set; }
        public string? Date_PC_Week { get; set; }
        public string? Delivery_Date { get; set; }
        public string? Readiness_Schedule_Vendor_Released { get; set; }
    }

    public class OpenPODetailViewModel
    {
        public string? Vendor { get; set; }
        public string? PO_No { get; set; }
        public DateTime? PO_Date { get; set; }
        public int? PO_Qty { get; set; }
        public int? Balance_Qty { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public string? Date_PC_Week { get; set; }
        public int? Delivery_Qty { get; set; }
        public string? Delivery_Remark { get; set; }
        public string? Reference_No { get; set; }
        public string? PO_Material { get; set; }
        public int? Remaining_Qty { get; set; }

    }
}
