var tabledata = [];
var table = null;
let filterStartJobDate = moment().startOf('week').format('YYYY-MM-DD');
let filterEndJobDate = moment().endOf('week').format('YYYY-MM-DD');

$(document).ready(function () {
    $('#jobRangeText').text(
        moment(filterStartJobDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndJobDate).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('jobDateTrigger'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartJobDate = start.format('YYYY-MM-DD');
                filterEndJobDate = end.format('YYYY-MM-DD');
                $('#jobRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartJobDate = "";
                filterEndJobDate = "";
                $('#jobRangeText').text("Select Date Range");
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
        startDate: moment().startOf('week').format('DD-MM-YYYY'),
        endDate: moment().endOf('week').format('DD-MM-YYYY')
    });

    $('#jobDateTrigger').on('click', function () {
        picker.show();
    });

    document.getElementById('backButton').addEventListener('click', function () {
        window.history.back();
    });

    //$('#upload-button').on('click', async function () {
    //    var expectedColumns = [
    //        'CCN No', 'CCCN Date', 'ReportedBy', 'Location', 'Customer Name', 'Dealer Name', 'Description', 'Status', 'Completion', 'ClosureRemarks', 'Time Taken for Closure (DAYS)'
    //    ];

    //    var url = '/Service/UploadComplaintDumpExcel';
    //    handleImportExcelFile(url, expectedColumns);
    //});

    //loadJobData();

    renderJobTable([]);
});

function loadJobData() {
    Blockloadershow();

    $.ajax({
        url: '/Service/GetAll',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartJobDate,
            endDate: filterEndJobDate
        },
        success: function (data) {
            if (Array.isArray(data)) {
                renderJobTable(data); // load into Tabulator or grid
            } else {
                showDangerAlert('No Data available.');
            }
            Blockloaderhide();
        },

        error: function (xhr, status, error) {
            showDangerAlert('Error retrieving data: ' + error);
            Blockloaderhide();
        }
    });
}
function renderJobTable(response) {
    //if (!Array.isArray(response)) {
    //    console.error("Invalid response, expected array:", response);
    //    showDangerAlert("Invalid data received.");
    //    Blockloaderhide();
    //    return;
    //}

    Blockloadershow();

    const tabledata = response.map((item, index) => ({
        Sr_No: index + 1,
        vendorName: item.vendorName,
        wiproDCNo: item.wiproDCNo,
        wiproDCDate: item.wiproDCDate,
        sapCode: item.sapCode,
        qtyAsPerWiproDC: item.qtyAsPerWiproDC,
        transporterWipro: item.transporterWipro,
        lrNoWipro: item.lrNoWipro,
        lrDateWipro: item.lrDateWipro,
        actualReceivedQty: item.actualReceivedQty,
        dispatchedThroughDC: item.dispatchedThroughDC,
        dispatchedThroughInvoice: item.dispatchedThroughInvoice,
        nonRepairable: item.nonRepairable,

        grandTotal: item.grandTotal,
        toBeProcessed: item.toBeProcessed,
        specialRemark: item.specialRemark,
        transporterVendor: item.transporterVendor,
        lrNoVendor: item.lrNoVendor,
        lrDateVendor: item.lrDateVendor,

        writeOffApproved: item.writeOffApproved,
        writeOffDate: item.writeOffDate,
        pendingWriteOff: item.pendingWriteOff,
    }));

    const columns = [
        { title: "SNo", field: "Sr_No", hozAlign: "center", headerHozAlign: "center", frozen: true },

        { title: "Vendor Name", field: "vendorName", frozen: true },

        {
            title: "Input for Wipro",
            headerHozAlign: "center", // Center group header
            columns: [
                {
                    title: "For Wipro",
                    headerHozAlign: "center", // Center group header
                    columns: [
                        { title: "Wipro DC No", field: "wiproDCNo" },
                        { title: "Wipro DC Date", field: "wiproDCDate" },
                        { title: "AS Per DC Sap code", field: "sapCode" },
                        { title: "Qty as per Wipro DC", field: "qtyAsPerWiproDC" },
                        { title: "Transporter", field: "transporterWipro" },
                        { title: "LR no", field: "lrNoWipro" },
                        { title: "LR date", field: "lrDateWipro" },
                        { title: "Actual Received Qty", field: "actualReceivedQty" },
                        { title: "Dispatched through DC (No Problem Drivers)", field: "dispatchedThroughDC" },
                        { title: "Dispatched through Invoice", field: "dispatchedThroughInvoice" },
                        { title: "Non Repairable", field: "nonRepairable" },
                    ],
                },
            ]
        },

        {
            title: "Input for Vendor",
            headerHozAlign: "center", // Center group header
            columns: [
                {
                    title: "For Vendor",
                    headerHozAlign: "center", // Center group header
                    columns: [
                        { title: "Grand Total", field: "grandTotal" },
                        { title: "To be processed", field: "toBeProcessed" },
                        { title: "Special Remark if any", field: "specialRemark" },
                        { title: "Transporter", field: "transporterVendor" },
                        { title: "LR no", field: "lrNoVendor" },
                        { title: "LR date", field: "lrDateVendor" },
                    ]
                }
            ]
        },

        {
            title: "Input For Wipro",
            headerHozAlign: "center", // Center group header
            columns: [
                {
                    title: "For Wipro",
                    headerHozAlign: "center", // Center group header
                    columns: [
                        { title: "Write OFF Approved", field: "writeOffApproved" },
                        { title: "Write oFF Date", field: "writeOffDate" },
                        { title: "Pending for Write off", field: "pendingWriteOff" },
                    ]
                }
            ]
        }
    ];

    if (typeof table !== 'undefined' && table instanceof Tabulator) {
        table.replaceData(tabledata);
    } else {
        table = new Tabulator("#job-table", {
            data: tabledata,
            layout: "fitDataFill",
            movableColumns: true,
            pagination: "local",
            paginationSize: 10,
            paginationSizeSelector: [10, 50, 100],
            paginationCounter: "rows",
            placeholder: "No data available",
            columns: columns
        });
    }

    Blockloaderhide();
}

// Utility for editable column config
function editableColumn(title, field, editor = "input", align = "left") {
    return {
        title: title,
        field: field,
        editor: editor,
        hozAlign: align,
        headerHozAlign: "center"
    };
}


function toIsoDateJob(value) {
    if (!value) return "";
    const parts = value.split('/');
    return parts.length === 3 ? `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}` : value;
}

function saveEditedRow(rowData) {
    const cleanedData = {
        Id: rowData.Id || 0,
        CCN_No: rowData.CCNNo || null,
        CCCNDate: toIsoDateJob(rowData.CCCNDate),
        ReportedBy: rowData.ReportedBy || null,
        CLocation: rowData.CLocation || null,
        CustName: rowData.CustName || null,
        DealerName: rowData.DealerName || null,
        CDescription: rowData.CDescription || null,
        CStatus: rowData.CStatus || null,
        Completion: toIsoDateJob(rowData.Completion),
        Remarks: rowData.Remarks || null

    };

    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/Service/CreateAsync' : '/Service/UpdateAsync';

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(cleanedData),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                loadData();
            } else {
                showDangerAlert(data.message || (isNew ? "Create failed." : "Update failed."));
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving record.");
        }
    });
}

var headerMenu = function () {
    const menu = [];
    const columns = this.getColumns();

    columns.forEach(column => {
        const icon = document.createElement("i");
        icon.classList.add("fas", column.isVisible() ? "fa-check-square" : "fa-square");

        const label = document.createElement("span");
        const title = document.createElement("span");
        title.textContent = " " + column.getDefinition().title;

        label.appendChild(icon);
        label.appendChild(title);

        menu.push({
            label: label,
            action: function (e) {
                e.stopPropagation();
                column.toggle();
                icon.classList.toggle("fa-check-square", column.isVisible());
                icon.classList.toggle("fa-square", !column.isVisible());
            }
        });
    });

    return menu;
};

function delConfirm(Id, element) {

    // Case 1: Unsaved row — delete directly without confirmation
    if (!Id || Id <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = table.getRow(rowEl);
        if (row) {
            row.delete();
        }
        return;
    }

    PNotify.prototype.options.styling = "bootstrap3";
    (new PNotify({
        title: 'Confirm Deletion',
        text: 'Are you sure you want to delete?',
        icon: 'fa fa-question-circle',
        hide: false,
        confirm: { confirm: true },
        buttons: { closer: false, sticker: false },
        history: { history: false }
    })).get().on('pnotify.confirm', function () {
        $.ajax({
            url: '/Service/Delete',
            type: 'POST',
            data: { id: Id },
            success: function (data) {
                if (data.success) {
                    showSuccessAlert("Deleted successfully.");
                    setTimeout(() => window.location.reload(), 1500);
                } else {
                    showDangerAlert(data.message || "Deletion failed.");
                }
            },
            error: function () {
                showDangerAlert('Error occurred during deletion.');
            }
        });
    });
}

function BlankComplaintDumpDown() {
    Blockloadershow();

    var expectedColumns = [
        'CCN No', 'CCCN Date', 'ReportedBy', 'Location', 'Customer Name', 'Dealer Name', 'Description', 'Status', 'Completion', 'ClosureRemarks', 'Time Taken for Closure (DAYS)'
    ];

    // Create worksheet with only the header row
    var data = [expectedColumns];
    var ws = XLSX.utils.aoa_to_sheet(data);

    // Apply bold style to header cells
    expectedColumns.forEach((col, index) => {
        const cellRef = XLSX.utils.encode_cell({ c: index, r: 0 }); // r: 0 => first row
        if (!ws[cellRef]) return;
        ws[cellRef].s = {
            font: {
                bold: true
            }
        };
    });

    // Auto-width calculation
    const columnWidths = expectedColumns.map(col => ({ wch: col.length + 2 }));
    ws['!cols'] = columnWidths;


    // Create workbook and export
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Complaint Dump Temaplate");

    XLSX.writeFile(wb, "Complaint_Dump_Temaplate.xlsx");

    Blockloaderhide();
};



//function openUpload() {
//    const columns = [
//        'CCN No', 'CCCN Date', 'ReportedBy', 'Location', 'Customer Name', 'Dealer Name',
//        'Description', 'Status', 'Completion', 'ClosureRemarks', 'Time Taken for Closure (DAYS)'
//    ];
//    $('#upload-url').val('/Service/UploadComplaintDumpExcel');
//    $('#expected-columns').val(JSON.stringify(columns));
//    $('#fileInput').val('');
//    $('#uploadModal').modal('show');
//}

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
