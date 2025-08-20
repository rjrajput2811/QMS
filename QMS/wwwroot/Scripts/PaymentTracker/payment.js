var tabledata = [];
var table = null;
const searchTerms = {};
let vendorOptions = {};
var tabledataLab = [];
var tableLab = '';
let labOptions = {};
let selectedLabCell = null;

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

        // Step 2: Load Nat Project data
        $.ajax({
            url: '/PaymentTracker/GetLabDropdown',
            type: 'GET'
        }).done(function (lab) {
            //let natProjectOptions = {};

            if (Array.isArray(lab)) {
                labOptions = lab.reduce((acc, v) => {
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
            showDangerAlert('Failed to load Nat Project data.');
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

function getFinancialYears() {
    let today = new Date();
    let year = today.getFullYear();
    let month = today.getMonth() + 1; // Jan=0, so +1

    let startYear;
    if (month < 4) {
        // Before April → FY belongs to previous year
        startYear = year - 1;
    } else {
        startYear = year;
    }

    // Build last 5 financial years
    let years = {};
    for (let i = 0; i < 5; i++) {
        let sYear = startYear - i;
        let eYear = sYear + 1;
        let fy = sYear + "-" + eYear.toString().slice(-2);
        years[fy] = fy;
    }

    return years;
}

var financialYears = getFinancialYears();

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
                Attachment: item.attachment,
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
        {
            title: "Financial Year",
            field: "Fin_Year",
            editor: "list",
            editorParams: {
                values: financialYears,
                clearable: true
            },
            headerFilter: "list",
            headerFilterParams: { values: financialYears },
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },
        //editableColumn("Financial Year", "Fin_Year", true),
        editableColumn("Month", "Month", "select2Test", "center", "input", {}, {
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
        //editableColumn("Lab", "Lab", true),
        editableColumn("Lab", "Lab", "select2", "center", "input", {}, {
            values: labOptions
        }, function (cell) {
            const val = cell.getValue();
            return labOptions[val] || val;
        }, 170),
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
        {
            title: "Attachment",
            field: "Attachment",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="/PaymentTrac_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file payment-upload" data-id="${cell.getRow().getData().Id}" style="width:160px;" />`;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

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
        const fyOptions = getFinancialYears(); // returns object
        const currentFY = Object.keys(fyOptions)[0]; // take the first key (e.g. "2025-26")
        const currentMonth = new Date().toLocaleString('default', { month: 'long' });

        const newRow = {
            Sr_No: table.getDataCount() + 1,
            Id: 0,
            CreatedDate: "",
            Fin_Year: currentFY,
            Month: currentMonth,
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

$('#payment_Table').on('change', '.payment-upload', function () {
    const input = this;
    const file = input.files[0];

    if (!file) return;

    const allowedTypes = [
        "application/pdf",
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/bmp",
        "image/webp"
    ];

    if (!allowedTypes.includes(file.type)) {
        showDangerAlert("Only PDF and image files (PDF, JPG, PNG, GIF, BMP, WEBP) are allowed.");
        $(this).val(""); // reset the input
        return;
    }

    const formData = new FormData();
    formData.append("file", file);
    formData.append("id", $(this).data("id"));

    Blockloadershow();

    $.ajax({
        url: "/PaymentTracker/UploadPaymentAttachment",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            showSuccessAlert("File uploaded successfully.");
            table.updateData([{ Id: response.id, BIS_Attachment: response.fileName }]);
        } else {
            showDangerAlert(response.message || "Upload failed.");
        }
    }).fail(function () {
        showDangerAlert("Upload failed due to server error.");
    }).always(function () {
        Blockloaderhide();
    });
});

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
    select2Test: function (cell, onRendered, success, cancel, editorParams) {
        const values = editorParams.values || {};
        const select = document.createElement("select");
        select.style.width = "100%";

        if (Array.isArray(values)) {
            values.forEach(function (item) {
                let option = document.createElement("option");
                option.value = item.value;
                option.text = item.label;
                if (item.value === cell.getValue()) option.selected = true;
                select.appendChild(option);
            });
        }
        else {

            for (let val in values) {
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

Tabulator.extendModule("edit", "editors", {
    select2: function (cell, onRendered, success, cancel, editorParams) {
        const fieldName = cell.getField(); // column field
        const values = editorParams.values || {};
        const select = document.createElement("select");
        select.style.width = "100%";

        // Add regular options
        for (let val in values) {
            let option = document.createElement("option");
            option.value = val;
            option.text = values[val];
            if (val === cell.getValue()) option.selected = true;
            select.appendChild(option);
        }

        // Add "Add New" option only for Nat_Project
        if (fieldName === "Lab") {
            let addOption = document.createElement("option");
            addOption.value = "__add_new__";
            addOption.text = "➕ Add New Lab";
            select.appendChild(addOption);
        }

        onRendered(function () {
            $(select).select2({
                dropdownParent: document.body,
                width: 'resolve',
                placeholder: "Select value",
                templateResult: function (data) {
                    if (data.id === "__add_new__") {
                        return $('<span style="color: blue;"><i class="fas fa-plus-circle"></i> ' + data.text + '</span>');
                    }
                    return data.text;
                },
                templateSelection: function (data) {
                    return values[data.id] || data.text;
                }
            }).on("select2:select", function (e) {
                const selectedVal = select.value;

                if (selectedVal === "__add_new__") {
                    $(select).select2('close');
                    cancel(); // cancel cell edit
                    selectedLabCell = cell; // store the cell
                    $('#labPaymentModel').modal('show');
                    loadLabData();
                } else {
                    success(selectedVal);
                }
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

    //Blockloadershow();
    var errorMsg = "";

    if (errorMsg !== "") {
        //Blockloaderhide();
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

    const isNew = Model.Id === 0;
    var ajaxUrl = isNew ? '/PaymentTracker/Create' : '/PaymentTracker/Update';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            //Blockloaderhide();
            if (response.success) {
                //const msg = Model.Id != 0
                //    ? "Payment Tracker updated successfully!"
                //    : "Payment Tracker saved successfully!";
                //showSuccessAlert(msg);
                if (isNew) {
                    showSuccessAlert("Saved successfully!.");
                    loadData();
                }
                //loadData();
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
            //Blockloaderhide();
            showDangerAlert("An unexpected error occurred. Please refresh the page and try again.");
        }
    });
}

function loadLabData() {
    Blockloadershow();
    $.ajax({
        url: '/PaymentTracker/GetLabPayment',
        type: 'GET',
        success: function (data) {
            Blockloaderhide();
            if (data && Array.isArray(data)) {
                OnNatLabGridLoad(data);
            } else {
                showDangerAlert('No data available to load.');
            }
        },
        error: function (xhr, status, error) {
            Blockloaderhide();
            showDangerAlert('Error retrieving data: ' + error);
        }
    });
}

function OnNatLabGridLoad(response) {
    debugger;
    Blockloadershow();

    tabledataLab = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledataLab.push({
                Sr_No: index + 1,
                Id: item.id,
                Lab: item.lab,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
            });
        });
    }

    if (tabledataLab.length === 0 && tableLab) {
        tableLab.clearData();
        Blockloaderhide();
        return;
    }

    columns.push(
        {
            title: "Action",
            field: "Action",
            width: 46,
            hozAlign: "center",
            headerHozAlign: "center",
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                let actionButtons = "";

                actionButtons += `<i onclick="delLabConfirm(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                return actionButtons;
            }
        },
        {
            title: "SNo", field: "Sr_No", width: 48, sorter: "number", hozAlign: "center", headerHozAlign: "left"
        },
        editableColumn("Lab", "Lab", true),
        { title: "CreatedBy", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", width: 129, sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
    );

    // // Initialize Tabulator
    tableLab = new Tabulator("#lab_Table", {
        data: tabledataLab,
        renderHorizontal: "virtual",
        movableColumns: true,
        pagination: "local",
        paginationSize: 10,
        paginationSizeSelector: [50, 100, 500, 1500, 2000],
        paginationCounter: "rows",
        dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
        columns: columns
    });

    tableLab.on("cellEdited", function (cell) {
        InsertUpdateLab(cell.getRow().getData());
    });

    $("#addLabBtn").on("click", function () {
        const newRow1 = {
            Sr_No: tableLab.getDataCount() + 1,
            Id: 0,
            Lab: "",
            CreatedBy: "",
            UpdatedBy: "",
            UpdatedDate: "",
            CreatedDate: ""
        };
        tableLab.addRow(newRow1, false);
    });


    Blockloaderhide();
}

function InsertUpdateLab(rowData) {
    debugger
    if (!rowData) {
        showDangerAlert("Invalid data provided.");
        return;
    }

    //Blockloadershow();
    var errorMsg = "";

    if (errorMsg !== "") {
        Blockloaderhide();
        showDangerAlert(errorMsg);
        return false;
    }

    var Model = {
        Id: rowData.Id || 0,
        Lab: rowData.Lab || null,
    };

    var ajaxUrl = Model.Id === 0 ? '/PaymentTracker/CreateLabPayment' : '/PaymentTracker/UpdateLabPayment';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            //Blockloaderhide();
            if (response.success) {
                const msg = Model.Id != 0
                    ? "Lab updated successfully!"
                    : "Lab saved successfully!";
                showSuccessAlert(msg);
                loadLabData();
            }
            else if (response.message === "Exist") {
                showDangerAlert("Lab already exists.");
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
            //Blockloaderhide();
            showDangerAlert("An unexpected error occurred. Please refresh the page and try again.");
        }
    });
}

$('#labPaymentModel').on('hidden.bs.modal', function () {
    loadLabData(); // uncomment if you want full reload
});

function delLabConfirm(recid, element) {
    debugger;

    if (!recid || recid <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = tableLab.getRow(rowEl);
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
            url: '/PaymentTracker/DeleteLabPayment',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("Nature of Project Deleted successfully.");
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
        loadLabData();
    });
}




