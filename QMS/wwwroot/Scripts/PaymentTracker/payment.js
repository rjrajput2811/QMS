var tabledata = [];
var table = null;
const searchTerms = {};
let vendorOptions = {};


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

    // Step 1: Load vendor data
    $.ajax({
        url: '/Service/GetVendor',
        type: 'GET'
    }).done(function (vendorData) {
        //let vendorOptions = {};

        if (Array.isArray(vendorData)) {
            vendorOptions = vendorData.reduce((acc, v) => {
                acc[v.value] = v.label;
                return acc;
            }, {});
        }

        $.ajax({
            url: '/PaymentTracker/GetAll',
            type: 'GET',
            success: function (data) {
                Blockloaderhide();
                if (data && Array.isArray(data)) {
                    OnTabGridLoad(data);
                } else {
                    showDangerAlert('No data available to load.');
                }
            },
            error: function (xhr, status, error) {
                Blockloaderhide();
                showDangerAlert('Error retrieving data: ' + error);
            }
        });


    }).fail(function () {
        Blockloaderhide();
        showDangerAlert('Failed to load vendor data.');
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

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Fin_Year: item.fin_Year,
                Month: item.month,
                Lab: item.lab,
                Vendor: item.vendor,
                Type_Test: item.type_Test,
                Description: item.description,
                Bis_Id: item.bis_Id,
                Invoice_No: item.invoice_No,
                Invoice_Date: formatDate(item.invoice_Date),
                Testing_Fee: item.testing_Fee,
                Approval_By: item.approval_By,
                Remark: item.remark,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
            });
        });
    }

    if (tabledata.length === 0 && table) {
        table.clearData();
        Blockloaderhide();
        return;
    }

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

                actionButtons += `<i onclick="delConfirm(${rowData.Id}, this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                return actionButtons;
            }
        },
        {
            title: "SNo", field: "Sr_No", sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left"
        },
        { title: "Date", field: "CreatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        editableColumn("Financial Year", "Fin_Year", true),
        editableColumn("Month", "Month", "select2", "center", "input", {}, {
            values: [
                { label: "January", value: "January" },
                { label: "February", value: "February" },
                { label: "March", value: "March" },
                { label: "April", value: "April" },
                { label: "May", value: "May" },
                { label: "June", value: "June" },
                { label: "July", value: "July" },
                { label: "August", value: "August" },
                { label: "September", value: "September" },
                { label: "October", value: "October" },
                { label: "November", value: "November" },
                { label: "December", value: "December" }
            ]
        }),
        editableColumn("Lab", "Lab", true),
        editableColumn("Vendor", "Vendor", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),
        editableColumn("Type of Test/ Standard", "Type_Test", true),

        editableColumn("Description", "Description", true),
        editableColumn("Test report no./ BIS ID", "Bis_Id", true),
        editableColumn("Invoice No.", "Invoice_No", true),
        editableColumn("Invoice Date", "Invoice_Date", "date", "center"),
        //editableColumn("Testing fees (Rs)", "Testing_Fee", true),
        editableColumn("Testing fees (Rs)", "Testing_Fee", "input", "right", "input", {}, {
            elementAttributes: {
                type: "number",
                min: 0,
                step: "0.01" // Optional: allows decimal
            }
        }),

        editableColumn("Approval by", "Approval_By", true),
        editableColumn("Remark", "Remark", true),
        
        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
    );

    // // Initialize Tabulator
    table = new Tabulator("#payment_Table", {
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

    table.on("cellEdited", function (cell) {

        InsertUpdatePayment(cell.getRow().getData());
    });

    $("#addPaymentButton").on("click", function () {
        const currentMonth = new Date().toLocaleString('default', { month: 'long' });

        const newRow = {
            Sr_No: table.getDataCount() + 1,
            Id: 0,
            CreatedDate: "",
            Fin_Year: "", 
            Month: "",
            Lab: "",
            Vendor: "",
            Type_Test: "",
            Description: "",
            Bis_Id: "",
            Invoice_No: "",
            Invoice_Date: "",
            Testing_Fee: "",
            Approval_By: "",
            Remark: "",
            CreatedBy: "",
            UpdatedBy: "",
            UpdatedDate: ""
        };
        table.addRow(newRow, false);
    });

    // Export to Excel on button click
    // document.getElementById("exportExcel").addEventListener("click", function () {
    //     table.download("xlsx", "ProductCode_Data.xlsx", { sheetName: "Product Code Data" });
    // });

    Blockloaderhide();
}


function editableColumn(title, field, editorType = true, align = "center", headerFilterType = "input", headerFilterParams = {}, editorParams = {}, formatter = null) {
    let columnDef = {
        title: title,
        field: field,
        editor: editorType,
        editorParams: editorParams,
        formatter: formatter,
        headerFilter: headerFilterType,
        headerFilterParams: headerFilterParams,
        headerMenu: headerMenu,
        hozAlign: align,
        headerHozAlign: "left"
    };

    return columnDef;
}

Tabulator.extendModule("edit", "editors", {
    select2: function (cell, onRendered, success, cancel, editorParams) {
        const values = editorParams.values || {};
        const select = document.createElement("select");
        select.style.width = "100%";

        if (Array.isArray(values)) 
        {
            values.forEach(function (item) {
                let option = document.createElement("option");
                option.value = item.value;
                option.text = item.label;
                if (item.value === cell.getValue()) option.selected = true;
                select.appendChild(option);
            });
        }
        else
        {

            for (let val in values)
            {
                let option = document.createElement("option");
                option.value = val;
                option.text = values[val];
                if (val === cell.getValue()) option.selected = true;
                select.appendChild(option);
            }
        }
        onRendered(function () {
            $(select).select2({
                dropdownParent: document.body,
                width: 'resolve',
                placeholder: "Select value"
            }).on("change", function () {
                success(select.value);
            });
        });

        return select;
    }
});


function delConfirm(recid, element) {
    debugger;

    if (!recid || recid <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = table.getRow(rowEl);
        if (row) {
            row.delete();
        }
        return;
    }

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
            url: '/PaymentTracker/Delete',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("Payment Detail Deleted successfully.");
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



function InsertUpdatePayment(rowData) {
    debugger
    if (!rowData) {
        showDangerAlert("Invalid data provided.");
        return;
    }

    Blockloadershow();
    var errorMsg = "";

    if (errorMsg !== "") {
        Blockloaderhide();
        showDangerAlert(errorMsg);
        return false;
    }

    function toIsoDate(value) {
        if (!value || value === "" || value === "Invalid Date") return null;

        if (typeof value === "string" && value.includes("/")) {
            const parts = value.split("/");
            if (parts.length === 3) {
                const [day, month, year] = parts;
                return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
            }
        }

        const parsed = new Date(value);
        return isNaN(parsed.getTime()) ? null : parsed.toISOString().substring(0, 10);
    }

    var Model = {
        Id: rowData.Id || 0,
        Fin_Year: rowData.Fin_Year || null,
        Month: rowData.Month || null,
        Lab: rowData.Lab || null,
        Vendor: rowData.Vendor || null,
        Type_Test: rowData.Type_Test || null,
        Description: rowData.Description || null,
        Bis_Id: rowData.Bis_Id || null,
        Invoice_No: rowData.Invoice_No || null,
        Invoice_Date: toIsoDate(rowData.Invoice_Date),
        Testing_Fee: rowData.Testing_Fee || 0,
        Approval_By: rowData.Approval_By || null,
        Remark: rowData.Remark || null,
    };

    var ajaxUrl = Model.Id === 0 ? '/PaymentTracker/Create' : '/PaymentTracker/Update';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            Blockloaderhide();
            if (response.success) {
                const msg = Model.Id != 0
                    ? "Payment Tracker updated successfully!"
                    : "Payment Tracker saved successfully!";
                showSuccessAlert(msg);
                loadData();
            }
            else if (response.message === "Exist") {
                showDangerAlert("Payment Tracker Detail already exists.");
            }
            else {
                var errorMessg = "";
                if (response.errors) {
                    for (var error in response.errors) {
                        if (response.errors.hasOwnProperty(error)) {
                            errorMessg += `${response.errors[error]}\n`;
                        }
                    }
                }
                showDangerAlert(errorMessg || response.message || "An error occurred while saving.");
            }
        },
        error: function (xhr, status, error) {
            Blockloaderhide();
            showDangerAlert("An unexpected error occurred. Please refresh the page and try again.");
        }
    });
}




