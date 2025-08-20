var tabledata = [];
var table = null;
var tabledataNatProject = [];
var tableNatProject = '';
const searchTerms = {};
let vendorOptions = {};
let natProjectOptions = {};
let selectedNatProjectCell = null;
let filterStartBISDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndBISDate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangeBIS').text(
        moment(filterStartBISDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndBISDate).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerBIS'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartBISDate = start.format('YYYY-MM-DD');
                filterEndBISDate = end.format('YYYY-MM-DD');
                $('#dateRangeBIS').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartBISDate = "";
                filterEndBISDate = "";
                $('#dateRangeBIS').text("Select Date Range");
                loadData();
            });
        },
        ranges: {
            Today: [moment(), moment()],
            Yesterday: [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        startDate: moment().startOf('month').format('DD-MM-YYYY'),
        endDate: moment().endOf('month').format('DD-MM-YYYY')
    });

    $('#customDateTriggerBIS').on('click', function () {
        picker.show();
    });

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
            url: '/BisProjectTrac/GetNatProjectDropdown',
            type: 'GET'
        }).done(function (natProject) {
            //let natProjectOptions = {};

            if (Array.isArray(natProject)) {
                natProjectOptions = natProject.reduce((acc, v) => {
                    acc[v.value] = v.label;
                    return acc;
                }, {});
            }

            // Add "Others" as special static option with style/icon

            // Step 3: Load grid data
            $.ajax({
                url: '/BisProjectTrac/GetAll',
                type: 'GET',
                dataType: 'json',
                data: {
                    startDate: filterStartBISDate,
                    endDate: filterEndBISDate
                },
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

    let endYear = startYear + 1;
    return startYear + "-" + endYear.toString().slice(-2);
}

var financialYears = getFinancialYears();

function getMonthOptions() {
    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    const yearSuffix = new Date().getFullYear().toString().slice(-2);
    return months.map(m => `${m} - ${yearSuffix}`); // array of strings
}

function getMonthString() {
    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    const today = new Date();
    const monthName = months[today.getMonth()];
    const yearSuffix = today.getFullYear().toString().slice(-2);
    return monthName + " - " + yearSuffix;
}


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
                Financial_Year: item.financial_Year,
                Mon_PC: item.mon_Pc,
                Nat_Project: item.nat_Project,
                Lea_Model_No: item.lea_Model_No,
                No_Seri_Add: item.no_Seri_Add,
                Cat_Ref_Lea_Model: item.cat_Ref_Lea_Model,
                Section: item.section,
                Manuf_Location: item.manuf_Location,
                BIS_Project_Id: item.biS_Project_Id,
                Lab: item.lab,
                Report_Owner: item.report_Owner,
                Start_Date: formatDate(item.start_Date),
                Comp_Date: formatDate(item.comp_Date),
                Test_Duration: item.test_Duration,
                Submitted_Date: formatDate(item.submitted_Date),
                Received_Date: formatDate(item.received_Date),
                Bis_Duration: item.bis_Duration,
                Ven_Sample_Sub_Date: formatDate(item.ven_Sample_Sub_Date),
                Current_Status: item.current_Status,
                BIS_Attachment: item.biS_Attachment,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate)
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
        
        {
            title: "Financial Year",
            field: "Financial_Year",
            editor: "list",
            editorParams: {
                values: financialYears,   // array or object
                autocomplete: true,
                clearable: true
            },
            headerFilter: "list",
            headerFilterParams: { values: financialYears },
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },

        {
            title: "Month/PCr",
            field: "Mon_PC",
            editor: "list",
            editorParams: {
                values: getMonthOptions(),   // array or object
                clearable: true
            },
            headerFilter: "list",
            headerFilterParams: { values: getMonthOptions() },
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },
    
        //editableColumn("Month/PC", "Mon_PC", true),
        //editableColumn("Nature of Project", "Nat_Project", true),
        editableColumn("Nature of Project", "Nat_Project", "select2", "center", "input", {}, {
            values: natProjectOptions
        }, function (cell) {
            const val = cell.getValue();
            return natProjectOptions[val] || val;
        }, 170),

        editableColumn("Lead Model Number", "Lea_Model_No", true),
        editableColumn("No. of Series Added", "No_Seri_Add", true),
        editableColumn("Cat Ref of Lead Model", "Cat_Ref_Lea_Model", true),
        editableColumn("Section", "Section", true),
        //editableColumn("Manufacturing Location", "Manuf_Location", true),
        editableColumn("Manufacturing Location", "Manuf_Location", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),

        editableColumn("BIS Project ID", "BIS_Project_Id", true),
        editableColumn("Lab", "Lab", true),
        editableColumn("Report Owner", "Report_Owner", true),

        editableColumn("Vendor Sample Submission Date", "Ven_Sample_Sub_Date", "date", "center"),
        editableColumn("Test Start Date", "Start_Date", "date", "center"),
        editableColumn("Test Complete Date", "Comp_Date", "date", "center"),
        /*editableColumn("Test Duration", "Test_Duration", true),*/
        {
            title: "Test Duration",
            field: "Test_Duration",
            mutator: function (value, data) {
                const start = parseDate(data.Start_Date);
                const end = parseDate(data.Comp_Date);
                if (start && end) return Math.floor((end - start) / (1000 * 60 * 60 * 24));
                return "";
            },
            hozAlign: "center",
            headerFilter: "input",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },
        editableColumn("BIS Submitted Date", "Submitted_Date", "date", "center"),
        editableColumn("BIS Received Date", "Received_Date", "date", "center"),
        //editableColumn("BIS Duration", "Bis_Duration", true),
        {
            title: "BIS Duration",
            field: "Bis_Duration",
            mutator: function (value, data) {
                const sub = parseDate(data.Ven_Sample_Sub_Date);
                const recv = parseDate(data.Received_Date);
                if (sub && recv) return Math.floor((recv - sub) / (1000 * 60 * 60 * 24));
                return "";
            },
            hozAlign: "center",
            headerFilter: "input",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },

        editableColumn("Current Status", "Current_Status", true),
        //editableColumn("BIS Attachment", "BIS_Attachment", true),
        {
            title: "BIS Attachment",
            field: "BIS_Attachment",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="/BISTrac_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file bis-upload" data-id="${cell.getRow().getData().Id}" style="width:160px;" />`;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false }
    );

    // // Initialize Tabulator
    table = new Tabulator("#bisProject_Table", {
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
        const field = cell.getField();
        const row = cell.getRow();
        const data = row.getData();

        if (["Test_Duration", "Bis_Duration"].includes(field)) {
            return;
        }

        if (["Start_Date", "Comp_Date"].includes(field)) {
            const start = parseDate(data.Start_Date);
            const end = parseDate(data.Comp_Date);
            const diff = start && end ? Math.floor((end - start) / (1000 * 60 * 60 * 24)) : "";
            row.update({ Test_Duration: diff.toString() });
        }

        if (["Ven_Sample_Sub_Date", "Received_Date"].includes(field)) {
            const sub = parseDate(data.Ven_Sample_Sub_Date);
            const rec = parseDate(data.Received_Date);
            const diff = sub && rec ? Math.floor((rec - sub) / (1000 * 60 * 60 * 24)) : "";
            row.update({ Bis_Duration: diff.toString() });
        }

        InsertUpdateBisProject(cell.getRow().getData());
    });

    $("#addBISButton").on("click", function () {
        const currentFY = getFinancialYears(); 
        var month = getMonthString();

        const newRow = {
            Sr_No: table.getDataCount() + 1,
            Id: 0,
            Financial_Year: currentFY,
            Mon_PC: month,
            Nat_Project: "",
            Lea_Model_No: "",
            No_Seri_Add: "",
            Cat_Ref_Lea_Model: "",
            Section: "",
            Manuf_Location: "",
            BIS_Project_Id: "",
            Lab: "",
            Report_Owner: "",
            Start_Date: "",
            Comp_Date: "",
            Test_Duration: "",
            Submitted_Date: "",
            Received_Date: "",
            Bis_Duration: "",
            Ven_Sample_Sub_Date: "",
            Current_Status: "",
            BIS_Attachment: "",
            Effective_Date: "",
            Document_No: "",
            Revision_No: "",
            Revision_Date: "",
            CreatedBy: "",
            UpdatedBy: "",
            UpdatedDate: "",
            CreatedDate: ""
        };
        table.addRow(newRow, false);
    });

    // Export to Excel on button click
    // document.getElementById("exportExcel").addEventListener("click", function () {
    //     table.download("xlsx", "ProductCode_Data.xlsx", { sheetName: "Product Code Data" });
    // });


    Blockloaderhide();
}

$('#bisProject_Table').on('change', '.bis-upload', function () {
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
        url: "/BisProjectTrac/UploadBISAttachment",
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

function parseDate(value) {
    if (!value) return null;
    // If value is a string in "dd/mm/yyyy", convert to Date
    if (typeof value === "string" && value.includes("/")) {
        const parts = value.split("/");
        if (parts.length === 3) {
            const [day, month, year] = parts;
            return new Date(`${year}-${month}-${day}`);
        }
    }
    return new Date(value);
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

//Tabulator.extendModule("edit", "editors", {
//    select2: function (cell, onRendered, success, cancel, editorParams) {
//        const fieldName = cell.getField(); // Get the column field name
//        const values = editorParams.values || {};
//        const select = document.createElement("select");
//        select.style.width = "100%";

//        // Append normal options
//        for (let val in values) {
//            let option = document.createElement("option");
//            option.value = val;
//            option.text = values[val];
//            if (val === cell.getValue()) option.selected = true;
//            select.appendChild(option);
//        }

//        // 👉 Only add the "Add New" option for Nat_Project column
//        if (fieldName === "Nat_Project") {
//            let addOption = document.createElement("option");
//            addOption.value = "__add_new__";
//            addOption.text = "➕ Add New Project Type";
//            select.appendChild(addOption);
//        }

//        onRendered(function () {
//            $(select).select2({
//                dropdownParent: document.body,
//                width: 'resolve',
//                placeholder: "Select value",
//                templateResult: function (data) {
//                    if (data.id === "__add_new__") {
//                        return $('<span style="color: blue;"><i class="fas fa-plus-circle"></i> ' + data.text + '</span>');
//                    }
//                    return data.text;
//                },
//                templateSelection: function (data) {
//                    return values[data.id] || data.text;
//                }
//            }).on("select2:select", function (e) {
//                const selectedVal = select.value;
//                if (selectedVal === "__add_new__") {
//                    $(select).select2('close');
//                    cancel(); // Cancel edit
//                    $('#yourModalId').modal('show'); // Show modal
//                } else {
//                    success(selectedVal);
//                }
//            });
//        });

//        return select;
//    }
//});

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
        if (fieldName === "Nat_Project") {
            let addOption = document.createElement("option");
            addOption.value = "__add_new__";
            addOption.text = "➕ Add New Project Type";
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
                    selectedNatProjectCell = cell; // store the cell
                    $('#natProjectModel').modal('show');
                    loadNatProjectData();
                } else {
                    success(selectedVal);
                }
            });
        });

        return select;
    }
});



//function showEditBisProject(id) {
//    debugger
//    var url = '/BisProjectTrac/BisProjectTracker?id=' + id;
//    window.location.href = url;
//}

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
            url: '/BisProjectTrac/Delete',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("Bis Projecet Deleted successfully.");
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

//function openBisProject(id) {
//    debugger
//    var url = '/BisProjectTrac/BisProjectTrackerDetail';
//    url = url + '?id=' + id
//    window.location.href = url;
//}

function InsertUpdateBisProject(rowData) {
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
        Financial_Year: rowData.Financial_Year || null,
        Mon_Pc: rowData.Mon_PC || null,
        Nat_Project: rowData.Nat_Project || null,
        Lea_Model_No: rowData.Lea_Model_No || null,
        No_Seri_Add: rowData.No_Seri_Add || null,
        Cat_Ref_Lea_Model: rowData.Cat_Ref_Lea_Model || null,
        Section: rowData.Section || null,
        Manuf_Location: rowData.Manuf_Location || null,
        BIS_Project_Id: rowData.BIS_Project_Id || null,
        Lab: rowData.Lab || null,
        Report_Owner: rowData.Report_Owner || null,
        Ven_Sample_Sub_Date: toIsoDate(rowData.Ven_Sample_Sub_Date),
        Start_Date: toIsoDate(rowData.Start_Date),
        Comp_Date: toIsoDate(rowData.Comp_Date),
        Test_Duration: rowData.Test_Duration.toString() || null,
        Submitted_Date: toIsoDate(rowData.Submitted_Date),
        Received_Date: toIsoDate(rowData.Received_Date),
        Bis_Duration: rowData.Bis_Duration.toString() || null,
        Current_Status: rowData.Current_Status || null,
        BIS_Attachment: rowData.BIS_Attachment || null,
        Effective_Date: toIsoDate(rowData.Effective_Date),
        Document_No: rowData.Document_No || null,
        Revision_No: rowData.Revision_No || null,
        Revision_Date: toIsoDate(rowData.Revision_Date)
    };

    const isNew = Model.Id === 0;
    var ajaxUrl = isNew ? '/BisProjectTrac/Create' : '/BisProjectTrac/Update';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            //Blockloaderhide();
            if (response.success) {
                //const msg = Model.Id != 0
                //    ? "BIS Project Tracker updated successfully!"
                //    : "BIS Project Tracker saved successfully!";
                //showSuccessAlert(msg);
                //loadData();
                if (isNew) {
                    showSuccessAlert("Saved successfully!.");
                    loadData();
                }
            }
            else if (response.message === "Exist") {
                showDangerAlert("BIS Project Tracker Detail already exists.");
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


function loadNatProjectData() {
    Blockloadershow();
    $.ajax({
        url: '/BisProjectTrac/GetNatProject',
        type: 'GET',
        success: function (data) {
            Blockloaderhide();
            if (data && Array.isArray(data)) {
                OnNatProjectTabGridLoad(data);
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

function OnNatProjectTabGridLoad(response) {
    debugger;
    Blockloadershow();

    tabledataNatProject = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledataNatProject.push({
                Sr_No: index + 1,
                Id: item.id,
                Nat_Project: item.nat_Project,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
            });
        });
    }

    if (tabledataNatProject.length === 0 && tableNatProject) {
        tableNatProject.clearData();
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

                actionButtons += `<i onclick="delNatProjectConfirm(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                return actionButtons;
            }
        },
        {
            title: "SNo", field: "Sr_No", width: 48, sorter: "number", hozAlign: "center", headerHozAlign: "left"
        },
        editableColumn("Nature of Project", "Nat_Project", true),
        { title: "CreatedBy", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", width: 129, sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
    );

    // // Initialize Tabulator
    tableNatProject = new Tabulator("#natProject_Table", {
        data: tabledataNatProject,
        renderHorizontal: "virtual",
        movableColumns: true,
        pagination: "local",
        paginationSize: 10,
        paginationSizeSelector: [50, 100, 500, 1500, 2000],
        paginationCounter: "rows",
        dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
        columns: columns
    });

    tableNatProject.on("cellEdited", function (cell) {
        InsertUpdateNatProject(cell.getRow().getData());
    });

    $("#addNatProjectBtn").on("click", function () {
        const newRow1 = {
            Sr_No: tableNatProject.getDataCount() + 1,
            Id: 0,
            Nat_Project: "",
            CreatedBy: "",
            UpdatedBy: "",
            UpdatedDate: "",
            CreatedDate: ""
        };
        tableNatProject.addRow(newRow1, false);
    });


    Blockloaderhide();
}

function InsertUpdateNatProject(rowData) {
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

    var Model = {
        Id: rowData.Id || 0,
        Nat_Project: rowData.Nat_Project || null,
    };

    var ajaxUrl = Model.Id === 0 ? '/BisProjectTrac/CreateNatProject' : '/BisProjectTrac/UpdateNatProject';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            Blockloaderhide();
            if (response.success) {
                const msg = Model.Id != 0
                    ? "Nature of Project updated successfully!"
                    : "Nature of Project saved successfully!";
                showSuccessAlert(msg);
                loadNatProjectData();
            }
            else if (response.message === "Exist") {
                showDangerAlert("Nature of Project already exists.");
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

$('#natProjectModel').on('hidden.bs.modal', function () {
    loadData(); // uncomment if you want full reload
});

function delNatProjectConfirm(recid, element) {
    debugger;

    if (!recid || recid <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = tableNatProject.getRow(rowEl);
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
            url: '/BisProjectTrac/DeleteNatProjectAsync',
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
        loadNatProjectData();
    });
}



