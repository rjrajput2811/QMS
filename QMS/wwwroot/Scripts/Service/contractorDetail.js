$(document).ready(function () {

});


function dateFormatter(cell) {
    const val = cell.getValue();
    if (!val) return "";
    const date = new Date(val);
    return `${String(date.getDate()).padStart(2, '0')}-${String(date.getMonth() + 1).padStart(2, '0')}-${date.getFullYear()}`;
}

function clearForm() {
    // Clear all input fields
    document.querySelectorAll('.form-control').forEach(function (input) {
        if (input.tagName === 'INPUT') {
            if (input.type === 'hidden' || input.readOnly) {
                // Skip hidden or readonly inputs
                return;
            }
            input.value = ''; // Clear input value
        } else if (input.tagName === 'SELECT') {
            input.selectedIndex = 0; // Reset dropdown to first option
        }
    });
    
    // Clear error messages if needed
    document.querySelectorAll('.text-danger').forEach(function (error) {
        error.textContent = '';
    });
}

function InsertUpdateContractorDetail() {
    debugger;
    Blockloadershow();
    var errorMsg = "";
    var fields = "";

    if ($("#firm_Name").val() == '' || $("#firm_Name").val() == null || $("#firm_Name").val() == undefined) {
        fields += " - Firm Name" + "<br>";
    }

    if ($("#vendor_Code").val() == '' || $("#vendor_Code").val() == null || $("#vendor_Code").val() == undefined) {
        fields += " - Vendor Code" + "<br>";
    }

    if (fields != "") {
        errorMsg = "Please fill following mandatory field(s):" + "<br><br>" + fields;
    }
    if (errorMsg != "") {
        Blockloaderhide();
        showDangerAlert(errorMsg);
        return false;
    }

    var ajaxUrl = "";
    if ($("#hdnId").val() != "0") {
        ajaxUrl = '/Service/UpdateContractor';
    }
    else {
        ajaxUrl = '/Service/CreateContractor';
    }

    var Model = {
        Id: $("#hdnId").val(),
        Cont_Firm_Name: $("#firm_Name").val(),
        Cont_Name: $("#name").val(),
        Cont_Ven_Code: $("#vendor_Code").val(),
        Pan_No: $("#pan").val(),
        Gst: $("#gst").val(),
        Cont_Valid_Date: $("#validity_Date").val(),
        Location: $("#location").val(),
        No_Tech: $("#no_Tech").val(),
        Moblie: $("#mobile").val(),
        Monthly_Salary: $("#monthly_Salary").val(),
        Conv_Bike_User: $("#conv_Bike_User").val(),
        Daily_Wages_Local: $("#daily_Wage_Local").val(),
        Conv_Fixed_Actual: $("#conv_Fixed_Actual").val(),
        Daily_Wages_Outstation: $("#daily_Outstation").val(),
        Dailywages_Ext_Manpower: $("#daily_Manpower").val(),
        OT_Charge_Full_Night: $("#ot_Night").val(),
        OT_Charge_Till_10: $("#ot_10pm").val(),
        OT_Outstation_night_Travel: $("#ot_Out_Travel").val(),
        Con_Cont_ESIC_Tech: $("#con_Cont_ESIC_Tech").val(),
        Con_Cont_PF_Tech: $("#con_Cont_PF_Tech").val(),
        Attchment: $("#attachment").val(),
    };

    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: Model,
        success: function (response) {
            Blockloaderhide();
            if (response.success) {
                if ($("#hdnId").val() != "0") {
                    showSuccessAlert("Contractor Detail is updated successfully!");
                }
                else {
                    showSuccessAlert("Contractor Detail is Saved Successfully!");

                }
                setTimeout(function () {
                    window.location.href = '/Service/ContractorList';
                }, 2500);
            }
            else if (response.message == "Exist") {
                // let duplicateFields = Array.isArray(response.playload) ? response.playload.join(", ") : "Unknown fields";
                let duplicateFields = Array.isArray(response.payload) ? response.payload.join("") : "Unknown fields";
                errorMsg = "Contractor Detail already exist:" + "<br><br>" + duplicateFields;
                showDangerAlert(errorMsg);
            }
            else {
                var errorMessg = "";
                for (var error in response.errors) {
                    errorMessg += error + "\n";
                }
                if (errorMessg != "") {
                    showDangerAlert(errorMessg);
                }
                else {
                    showDangerAlert(response.Message);
                }
            }
        },
        error: function (xhr, ststus, errors) {
            Blockloaderhide();
            showDangerAlert("An unexpected eror occured, please refresh the page and try again.");
        }
    });
}



