var tabledata = [];
var table = '';
const searchTerms = {};
$(document).ready(function () {

    document.addEventListener('DOMContentLoaded', function () {
        document.getElementById('backButton').addEventListener('click', function () {
            window.history.back();
        });
    });

    loadData();
});

function loadData() {
    Blockloadershow();
    $.ajax({
        url: '/Service/GetAllContractor',
        type: 'GET',
        success: function (data) {
            Blockloaderhide();
            if (data && Array.isArray(data)) {
                OnTabGridLoad(data);
            }
            else {
                showDangerAlert('No data available to load.');
            }
        },
        error: function (xhr, status, error) {
            showDangerAlert('Error retrieving data: ' + error);
            Blockloaderhide();
        }
    });
}

//define column header menu as column visibility toggle
var headerMenu = function () {
    var menu = [];
    var columns = this.getColumns();

    for (let column of columns) {

        //create checkbox element using font awesome icons
        let icon = document.createElement("i");
        icon.classList.add("fas");
        icon.classList.add(column.isVisible() ? "fa-check-square" : "fa-square");

        //build label
        let label = document.createElement("span");
        let title = document.createElement("span");

        title.textContent = " " + column.getDefinition().title;

        label.appendChild(icon);
        label.appendChild(title);

        //create menu item
        menu.push({
            label: label,
            action: function (e) {
                //prevent menu closing
                e.stopPropagation();

                //toggle current column visibility
                column.toggle();

                //change menu item icon
                if (column.isVisible()) {
                    icon.classList.remove("fa-square");
                    icon.classList.add("fa-check-square");
                } else {
                    icon.classList.remove("fa-check-square");
                    icon.classList.add("fa-square");
                }
            }
        });
    }

    return menu;
};

function OnTabGridLoad(response) {
    debugger;
    Blockloadershow();

    tabledata = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            let formattedDate = "";
            let updatedDate = "";
            let validate = "";
            if (item.cont_Valid_Date) {
                const validateObj = new Date(item.cont_Valid_Date);
                validate = validateObj.toLocaleDateString("en-GB");
            }
            if (item.createdDate) {
                const dateObj = new Date(item.createdDate);
                formattedDate = dateObj.toLocaleDateString("en-GB");
            }
            if (item.updatedDate) {
                const updatedateObj = new Date(item.updatedDate);
                updatedDate = updatedateObj.toLocaleDateString("en-GB");
            }

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Cont_Firm_Name: item.cont_Firm_Name,
                Cont_Name: item.cont_Name,
                Cont_Ven_Code: item.cont_Ven_Code,
                Pan_No: item.pan_No,
                Gst: item.gst,
                Cont_Valid_Date: validate,
                Location: item.location,
                No_Tech: item.no_Tech,
                Moblie: item.moblie,
                Monthly_Salary: item.monthly_Salary,
                Conv_Bike_User: item.conv_Bike_User,
                Daily_Wages_Local: item.daily_Wages_Local,
                Conv_Fixed_Actual: item.conv_Fixed_Actual,
                Daily_Wages_Outstation: item.daily_Wages_Outstation,
                Dailywages_Ext_Manpower: item.dailywages_Ext_Manpower,
                OT_Charge_Full_Night: item.oT_Charge_Full_Night,
                OT_Charge_Till_10: item.oT_Charge_Till_10,
                OT_Outstation_night_Travel: item.oT_Outstation_night_Travel,
                Dailywages_Ext_Manpower: item.dailywages_Ext_Manpower,
                Con_Cont_ESIC_Tech: item.con_Cont_ESIC_Tech,
                Con_Cont_PF_Tech: item.con_Cont_PF_Tech,
                CreatedDate: formattedDate,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: updatedDate
            });
        });

        columns.push(
            {
                title: "Action",
                field: "Action",
                // width: 130,
                headerMenu: headerMenu,
                hozAlign: "center",
                headerHozAlign: "center",
                formatter: function (cell, formatterParams) {
                    const rowData = cell.getRow().getData();
                    let actionButtons = "";

                    actionButtons += `<i data-toggle="modal" onclick="delConfirm(${rowData.Id})" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                    return actionButtons;
                }
            },
            {
                title: "SNo", field: "Sr_No", sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left"
            },
            { title: "Contractor Name", field: "Cont_Name", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Location", field: "Location", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Contractor Vendor Code", field: "Cont_Ven_Code", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "No Of Technicians ", field: "No_Tech", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "GST", field: "Gst", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Contract Validity", field: "Cont_Valid_Date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Daily wages Monthly Fixed Salary ", field: "Monthly_Salary", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Daily Wages Local", field: "Daily_Wages_Local", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "Daily Wages Outstation", field: "Daily_Wages_Outstation", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "OT Charge Full night", field: "OT_Charge_Full_Night", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "OT Charge Till 10.Pm", field: "OT_Charge_Till_10", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "OT for Outstation night Travel", field: "OT_Outstation_night_Travel", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "Mobile", field: "Moblie", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "Local Conveyance For Bike User", field: "Conv_Bike_User", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "Local Conveyance -Fixed or at Actual", field: "Conv_Fixed_Actual", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
            { title: "Dailywages for Extra Manpower", field: "Dailywages_Ext_Manpower", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Confirm Contractor Providing ESIC to Technicians", field: "Con_Cont_ESIC_Tech", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Confirm Contractor Providing PF to Technicians", field: "Con_Cont_PF_Tech", sorter: "number", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Created By", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
            { title: "Create Date", field: "CreatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
            { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
            { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        );

        // // Initialize Tabulator
        table = new Tabulator("#cont_Table", {
            data: tabledata,
            renderHorizontal: "virtual",
            movableColumns: true,
            pagination: "local",
            paginationSize: 10,
            paginationSizeSelector: [50, 100, 500, 1500, 2000],
            paginationCounter: "rows",
            dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
            columns: columns
        });

        table.on("cellClick", function (e, cell) {
            let columnField = cell.getColumn().getField();

            if (columnField !== "Action") {
                let rowData = cell.getRow().getData();
                showEditContractor(rowData.Id);
            }
        });

        // Export to Excel on button click
        // document.getElementById("exportExcel").addEventListener("click", function () {
        //     table.download("xlsx", "ProductCode_Data.xlsx", { sheetName: "Product Code Data" });
        // });
    }
    else {
        showDangerAlert('No data available.');
    }

    // Hide loader
    Blockloaderhide();
}

function showEditContractor(id) {
    debugger
    var url = '/Service/ContractorDetails?id=' + id;
    window.location.href = url;
}

function delConfirm(recid) {
    debugger;
    PNotify.prototype.options.styling = "bootstrap3";
    (new PNotify({
        title: 'Confirmation Needed',
        text: 'Are you sure to delete? It will not delete if this record is used in transactions.',
        icon: 'glyphicon glyphicon-question-sign',
        hide: false,
        confirm: {
            confirm: true
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        },
    })).get().on('pnotify.confirm', function () {
        $.ajax({
            url: '/Service/Delete',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("Contractor Detail Deleted successfully.");
                    setTimeout(function () {
                        window.location.reload();
                    }, 2500);
                }
                else if (data.success == false && data.message == "Not_Deleted") {
                    showDangerAlert("Record is used in QMS Log transactions.");
                }
                else {
                    showDangerAlert(data.message);
                }
            },
            error: function () {
                showDangerAlert('Error retrieving data.');
            }
        });
    }).on('pnotify.cancel', function () {
        loadData();
    });
}

function openContractorDetail(id) {
    debugger
    var url = '/Service/ContractorDetails';
    url = url + '?id=' + id
    window.location.href = url;
}
