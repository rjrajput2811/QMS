var tabledata = [];
var table = null;
var tabledata1 = [];
var finalTable = null;
let filterStartJobDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndJobDate = moment().endOf('month').format('YYYY-MM-DD');
var userType = document.getElementById("hdnUserType").value;
var vendorName = document.getElementById("hdnVendorName").value;

$(document).ready(function () {

    if (userType != "Vendor") {
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
                    loadJobData();
                });

                picker.on('clear', () => {
                    filterStartJobDate = "";
                    filterEndJobDate = "";
                    $('#jobRangeText').text("Select Date Range");
                    loadJobData();
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

        $('#jobDateTrigger').on('click', function () {
            picker.show();
        });

        $('#FinaldateRangeText').text(
            moment(filterStartJobDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndJobDate).format('MMMM D, YYYY')
        );

        const picker1 = new Litepicker({
            element: document.getElementById('customDateTriggerFinal'),
            singleMode: false,
            format: 'DD-MM-YYYY',
            numberOfMonths: 2,
            numberOfColumns: 2,
            dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
            plugins: ['ranges'],
            setup: (picker1) => {
                picker1.on('selected', (start, end) => {
                    filterStartJobDate = start.format('YYYY-MM-DD');
                    filterEndJobDate = end.format('YYYY-MM-DD');
                    $('#FinaldateRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                    loadFinalJobData();
                });

                picker1.on('clear', () => {
                    filterStartJobDate = "";
                    filterEndJobDate = "";
                    $('#FinaldateRangeText').text("Select Date Range");
                    loadFinalJobData();
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

        $('#customDateTriggerFinal').on('click', function () {
            picker1.show();
        });

        document.getElementById('backButton').addEventListener('click', function () {
            window.history.back();
        });

    }

    if (userType != "Vendor") {

        $('#upload-button').on('click', async function () {
            var expectedColumns = [
                'Vendor Name', 'Wipro DC No', 'Wipro DC Date', 'AS Per DC Sap code', 'Qty as per Wipro DC', 'Transporter', 'LR No', 'LR Date',
                'Write Off Approved', 'Write Off Date', 'Pending for Write off'
            ];

            var url = '/Service/UploadJobWorkExcel';
            handleImportExcelFile(url, expectedColumns);
        });
    }
    else {
        $('#upload-button').on('click', async function () {
            var expectedColumns = [
                'SNo',
                'Unique Id',
                'Vendor Name',
                'Wipro DC No',
                'Wipro DC Date',
                'AS Per DC Sap code',
                'Qty as per Wipro DC',
                'Transporter',
                'LR no',
                'LR date',
                'Actual Received Qty',
                'Dispatched through DC (No Problem Drivers)',
                'Dispatched through Invoice',
                'Non Repairable',
                'Grand Total',
                'To be processed',
                'Special Remark if any',
                'Transporter',
                'LR no',
                'LR date'
            ];

            var url = '/Service/UploadVendorInputExcel';
            handleImportExcelFile(url, expectedColumns);
        });
    }

    if (userType != "Vendor")
    {
        loadJobData();
    }
    else
    {
        loadVendorJobData();
    }
});

function openUploadJob() {

    clearForm();
    if (!$('#uploadModal').length) {
        $('body').append(partialView);
    }
    $('#uploadModal').modal('show');
}

function loadJobData() {
    Blockloadershow();
    $('#ftab2').hide();
    $('#ftab1').show();
    $.ajax({
        url: '/Service/GetJobWorkAll',
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

function loadVendorJobData() {
    Blockloadershow();
    $('#ftab2').hide();
    $('#ftab1').show();
    $.ajax({
        url: '/Service/GetJobWorkById',
        type: 'GET',
        dataType: 'json',
        data: {
            vendor: vendorName
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

function renderJobTable(response) {

    Blockloadershow();

    const tabledata = response.map((item, index) => ({
        Sr_No: index + 1,
        Vendor: item.vendor,
        Wipro_Dc_No: item.wipro_Dc_No,
        Wipro_Dc_Date: item.wipro_Dc_Date ? new Date(item.wipro_Dc_Date).toLocaleDateString("en-GB") : "",
        Dc_Sap_Code: item.dc_Sap_Code,
        Qty_Wipro_Dc: item.qty_Wipro_Dc,
        Wipro_Transporter: item.wipro_Transporter,
        Wipro_LR_No: item.wipro_LR_No,
        Wipro_LR_Date: item.wipro_LR_Date ? new Date(item.wipro_LR_Date).toLocaleDateString("en-GB") : "",

        Actu_Rece_Qty: item.actu_Rece_Qty,
        Dispatch_Dc: item.dispatch_Dc,
        Dispatch_Invoice: item.dispatch_Invoice,
        Non_Repairable: item.non_Repairable,
        Grand_Total: item.grand_Total,
        To_Process: item.to_Process,
        Remark: item.remark,
        Vendor_Transporter: item.vendor_Transporter,
        Vendor_LR_No: item.vendor_LR_No,
        Vendor_LR_Date: item.vendor_LR_Date ? new Date(item.vendor_LR_Date).toLocaleDateString("en-GB") : "",

        Write_Off_Approved: item.write_Off_Approved,
        Write_Off_Date: item.write_Off_Date ? new Date(item.write_Off_Date).toLocaleDateString("en-GB") : "",
        Pending_Write_Off: item.pending_Write_Off,
        UniqueId: item.uniqueId,

        CreatedDate: item.createdDate ? new Date(item.createdDate).toLocaleDateString("en-GB") : "",
        CreatedBy: item.createdBy,
        UpdatedDate: item.updatedDate ? new Date(item.updatedDate).toLocaleDateString("en-GB") : "",
        UpdatedBy: item.updatedBy,
    }));

    const columns = [
        { title: "SNo", field: "Sr_No", hozAlign: "center", headerHozAlign: "center", frozen: true },
        { title: "Unique Id", field: "UniqueId", hozAlign: "left", headerHozAlign: "center", frozen: true },

        { title: "Vendor Name", field: "Vendor", frozen: true,headerMenu: headerMenu, headerFilter: "input" },

        {
            title: "Input for Wipro",
            headerHozAlign: "center", // Center group header
            columns: [
                {
                    title: "For Wipro",
                    headerHozAlign: "center", // Center group header
                    columns: [
                        { title: "Wipro DC No", field: "Wipro_Dc_No", headerMenu: headerMenu, headerFilter: "input" },
                        { title: "Wipro DC Date", field: "Wipro_Dc_Date", headerMenu: headerMenu, headerFilter: "input" },
                        { title: "AS Per DC Sap code", field: "Dc_Sap_Code", headerMenu: headerMenu, headerFilter: "input" },
                        { title: "Qty as per Wipro DC", field: "Qty_Wipro_Dc", headerMenu: headerMenu, headerFilter: "input" },
                        { title: "Transporter", field: "Wipro_Transporter", headerMenu: headerMenu, headerFilter: "input" },
                        { title: "LR no", field: "Wipro_LR_No", headerMenu: headerMenu, headerFilter: "input" },
                        { title: "LR date", field: "Wipro_LR_Date", headerMenu: headerMenu, headerFilter: "input" },

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
                        { title: "Actual Received Qty", field: "Actu_Rece_Qty", headerMenu: headerMenu, headerFilter: "input" },
                        { title: "Dispatched through DC (No Problem Drivers)", field: "Dispatch_Dc", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "Dispatched through Invoice", field: "Dispatch_Invoice", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "Non Repairable", field: "Non_Repairable", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "Grand Total", field: "Grand_Total", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "To be processed", field: "To_Process", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "Special Remark if any", field: "Remark", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "Transporter", field: "Vendor_Transporter", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "LR no", field: "Vendor_LR_No", headerMenu: headerMenu,headerFilter: "input" },
                        { title: "LR date", field: "Vendor_LR_Date", headerMenu: headerMenu,headerFilter: "input" },
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
                        { title: "Write OFF Approved", field: "Write_Off_Approved", dowload: false, headerMenu: headerMenu,headerFilter: "input" },
                        { title: "Write oFF Date", field: "Write_Off_Date", dowload: false, headerMenu: headerMenu,headerFilter: "input" },
                        { title: "Pending for Write off", field: "Pending_Write_Off", dowload: false, headerMenu: headerMenu,headerFilter: "input" },
                    ]
                }
            ]
        },

        { title: "Created By", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", dowload: false, hozAlign: "center", headerHozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", headerMenu: headerMenu, headerFilter: "input", dowload: false, hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", dowload: false, hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated Date", field: "UpdatedDate", headerMenu: headerMenu, headerFilter: "input", dowload: false, hozAlign: "center", headerHozAlign: "center" }
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

    if (userType != "Vendor") {
        document.getElementById("exclExpButton").addEventListener("click", function () {
            // Get only visible data from Tabulator (respects filters, sorting, pagination)
            var visibleData = table.getData("active"); // "active" gets only visible/filtered rows

            // Get visible columns only
            var visibleColumns = table.getColumns().filter(col => col.isVisible() && col.getField() !== "Action"
                && col.getField() !== "Write_Off_Approved" && col.getField() !== "Pending_Write_Off" && col.getField() !== "Write_Off_Date");

            // Prepare headers
            var headers = visibleColumns.map(col => col.getDefinition().title);

            // Prepare data rows
            var rows = visibleData.map(row => {
                return visibleColumns.map(col => {
                    var field = col.getField();
                    return row[field] !== undefined ? row[field] : "";
                });
            });

            // Create date range text
            var dateRangeText = "";
            if (filterStartJobDate && filterEndJobDate) {
                dateRangeText = `Date Range: ${moment(filterStartJobDate).format('DD-MMM-YYYY')} to ${moment(filterEndJobDate).format('DD-MMM-YYYY')}`;
            } else if (filterStartJobDate) {
                dateRangeText = `Date From: ${moment(filterStartJobDate).format('DD-MMM-YYYY')}`;
            } else if (filterEndJobDate) {
                dateRangeText = `Date To: ${moment(filterEndJobDate).format('DD-MMM-YYYY')}`;
            } else {
                dateRangeText = "Date Range: All Dates";
            }

            // Combine: date range (row 1), empty row (row 2), headers (row 3), data (row 4+)
            var exportData = [
                [dateRangeText], // Row 1: Date range
                [],              // Row 2: Empty row
                headers,         // Row 3: Headers
                ...rows          // Row 4+: Data
            ];


            // Create worksheet
            var ws = XLSX.utils.aoa_to_sheet(exportData);

            // Style header row (bold)
            headers.forEach((header, index) => {
                const cellRef = XLSX.utils.encode_cell({ c: index, r: 0 });
                if (!ws[cellRef]) return;
                ws[cellRef].s = {
                    font: { bold: true },
                    fill: { fgColor: { rgb: "D3D3D3" } },
                    alignment: { horizontal: "center" }
                };
            });

            // Auto-width calculation
            const columnWidths = headers.map(header => ({ wch: Math.max(header.length + 2, 10) }));
            ws['!cols'] = columnWidths;

            // Freeze first row
            ws['!freeze'] = { xSplit: 0, ySplit: 1 };

            // Set row heights
            if (!ws['!rows']) ws['!rows'] = [];
            ws['!rows'][0] = { hpt: 25 }; // Date range row height
            ws['!rows'][1] = { hpt: 10 };  // Empty row height
            ws['!rows'][2] = { hpt: 20 };  // Header row height

            // Create workbook and download
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "JobWrkSpareIN");

            var fileName = `JobWrkSpareIN_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
            XLSX.writeFile(wb, fileName);
        });
    }
    else {
        document.getElementById("exclVendorExpButton").addEventListener("click", async function () {
            var visibleData = table.getData("active");
            var visibleColumns = table.getColumns().filter(col => col.isVisible()
                && col.getField() !== "Action"
                && col.getField() !== "Write_Off_Approved"
                && col.getField() !== "Pending_Write_Off"
                && col.getField() !== "Write_Off_Date");

            var headers = visibleColumns.map(col => col.getDefinition().title);

            var rows = visibleData.map(row => {
                return visibleColumns.map(col => {
                    var field = col.getField();
                    return row[field] !== undefined ? row[field] : "";
                });
            });

            // ── ExcelJS workbook ──────────────────────────────────────────────
            const workbook = new ExcelJS.Workbook();
            const worksheet = workbook.addWorksheet("JobWrkSpareIN");

            // Auto column widths based on header length
            worksheet.columns = headers.map(h => ({
                //header: "",          // we'll set header manually below
                width: Math.max(h.length + 4, 12)
            }));

            // ── Header row ────────────────────────────────────────────────────
            const headerRow = worksheet.addRow(headers);
            headerRow.height = 25;

            headerRow.eachCell((cell) => {
                cell.value = cell.value;          // keep value
                cell.font = { bold: true, size: 14, color: { argb: "FF000000" } };
                cell.fill = {
                    type: "pattern",
                    pattern: "solid",
                    fgColor: { argb: "FFD6E4F0" }   // same blue-grey you use in Tabulator header
                };
                cell.alignment = { horizontal: "center", vertical: "middle" };
                cell.border = {
                    top: { style: "thin" },
                    left: { style: "thin" },
                    bottom: { style: "thin" },
                    right: { style: "thin" }
                };
            });

            // Freeze header row
            worksheet.views = [{ state: "frozen", ySplit: 1 }];

            // ── Data rows ─────────────────────────────────────────────────────
            rows.forEach((rowData, idx) => {
                const dataRow = worksheet.addRow(rowData);
                dataRow.height = 20;

                dataRow.eachCell({ includeEmpty: true }, (cell) => {
                    cell.font = { size: 12 };
                    cell.alignment = { vertical: "middle" };
                    // Alternate row shading (optional)
                    if (idx % 2 === 0) {
                        cell.fill = {
                            type: "pattern",
                            pattern: "solid",
                            fgColor: { argb: "FFF5F5F5" }
                        };
                    }
                    cell.border = {
                        top: { style: "thin", color: { argb: "FFCCCCCC" } },
                        left: { style: "thin", color: { argb: "FFCCCCCC" } },
                        bottom: { style: "thin", color: { argb: "FFCCCCCC" } },
                        right: { style: "thin", color: { argb: "FFCCCCCC" } }
                    };
                });
            });

            // ── Download ──────────────────────────────────────────────────────
            const buffer = await workbook.xlsx.writeBuffer();
            const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            const url = URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = `JobWrkSpareIN_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
            a.click();
            URL.revokeObjectURL(url);
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
            url: '/Service/JobWorkDelete',
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

//function BlankJobWorkTrackDown() {
//    Blockloadershow();

//    var expectedColumns = [
//        'Vendor Name', 'Wipro DC No', 'Wipro DC Date', 'AS Per DC Sap code', 'Qty as per Wipro DC', 'Transporter', 'LR No', 'LR Date',
//        'Write Off Approved', 'Write Off Date', 'Pending for Write off'
//    ];

//    // Create worksheet with only the header row
//    var data = [expectedColumns];
//    var ws = XLSX.utils.aoa_to_sheet(data);

//    // Apply bold style to header cells
//    expectedColumns.forEach((col, index) => {
//        const cellRef = XLSX.utils.encode_cell({ c: index, r: 0 }); // r: 0 => first row
//        if (!ws[cellRef]) return;
//        ws[cellRef].s = {
//            font: {
//                bold: true
//            }
//        };
//    });

//    // Auto-width calculation
//    const columnWidths = expectedColumns.map(col => ({ wch: col.length + 2 }));
//    ws['!cols'] = columnWidths;


//    // Create workbook and export
//    var wb = XLSX.utils.book_new();
//    XLSX.utils.book_append_sheet(wb, ws, "Jobwork Spares Tracking");

//    XLSX.writeFile(wb, "Jobwork_Spares_Tracking.xlsx");

//    Blockloaderhide();
//};


async function BlankJobWorkTrackDown() {
    Blockloadershow();

    const expectedColumns = [
        'Vendor Name', 'Wipro DC No', 'Wipro DC Date', 'AS Per DC Sap code',
        'Qty as per Wipro DC', 'Transporter', 'LR No', 'LR Date',
        'Write Off Approved', 'Write Off Date', 'Pending for Write off'
    ];

    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Jobwork Spares Tracking');

    // Set column widths FIRST (before adding any rows)
    worksheet.columns = expectedColumns.map((col) => ({
        width: col.length + 4
    }));

    // Now add header row
    const headerRow = worksheet.addRow(expectedColumns);

    // Apply styles to each header cell
    headerRow.eachCell((cell) => {
        cell.font = {
            bold: true,
            size: 14,
            color: { argb: 'FF000000' }  // White text
        };
        cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFD6E4F0' }  // Blue background
        };
        cell.alignment = {
            vertical: 'middle',
            horizontal: 'center'
        };
        cell.border = {
            top: { style: 'thin', color: { argb: 'FF000000' } },
            left: { style: 'thin', color: { argb: 'FF000000' } },
            bottom: { style: 'thin', color: { argb: 'FF000000' } },
            right: { style: 'thin', color: { argb: 'FF000000' } }
        };
    });

    // Set row height for header
    headerRow.height = 22;

    // Export as blob and trigger download
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'Jobwork_Spares_Tracking.xlsx';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);

    Blockloaderhide();
}

//function BlankVendorJobWorkTrackDown() {
//    Blockloadershow();

//    var expectedColumns = [
//        'Actual\n Received\n Qty', 'Dispatched through\nDC (No Problem\n Drivers)', 'Dispatched\n through\n Invoice', 'Non\n Repairable ', 'Grand\n Total', 'To be\n processed', 'Special\n Remark if\n any ', 'Transporter',
//        'LR no', 'LR date'
//    ];

//    // Create worksheet with only the header row
//    var data = [expectedColumns];
//    var ws = XLSX.utils.aoa_to_sheet(data);

//    // Apply bold style to header cells
//    expectedColumns.forEach((col, index) => {
//        const cellRef = XLSX.utils.encode_cell({ c: index, r: 0 }); // r: 0 => first row
//        if (!ws[cellRef]) return;
//        ws[cellRef].s = {
//            font: {
//                bold: true
//            }
//        };
//    });

//    // Auto-width calculation
//    const columnWidths = expectedColumns.map(col => ({ wch: col.length + 2 }));
//    ws['!cols'] = columnWidths;


//    // Create workbook and export
//    var wb = XLSX.utils.book_new();
//    XLSX.utils.book_append_sheet(wb, ws, "Vendor Jobwork Spares Tracking");

//    XLSX.writeFile(wb, "Vendor_Spares_Tracking.xlsx");

//    Blockloaderhide();
//};

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

function loadFinalJobData() {
    Blockloadershow();
    $('#ftab1').hide();
    $('#ftab2').show();
    $.ajax({
        url: '/Service/GetJobListFinalResult',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartJobDate,
            endDate: filterEndJobDate
        },
        success: function (data) {
            if (Array.isArray(data)) {
                renderFinalJobTable(data); // load into Tabulator or grid
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


function renderFinalJobTable(response) {
    debugger;

    Blockloadershow();

    const tabledata1 = response.map((item, index) => ({
        Sr_No: index + 1,
        Vendor: item.vendor,
        Dc_Sap_Code: item.dc_Sap_Code,
        Qty_Wipro_Dc: item.qty_Wipro_Dc,
        Wipro_Transporter: item.Wipro_Transporter,
        Actu_Rece_Qty: item.actu_Rece_Qty,
        Dispatch_Dc: item.dispatch_Dc,
        Dispatch_Invoice: item.dispatch_Invoice,
        Non_Repairable: item.non_Repairable,
        To_Process: item.to_Process,
        Pending_Write_Off: item.pending_Write_Off,
    }));

    const columns = [
        { title: "SNo", field: "Sr_No", hozAlign: "center", headerHozAlign: "center", },
        { title: "Vendor Name", field: "Vendor", hozAlign: "left", headerHozAlign: "left", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Code", field: "Dc_Sap_Code", hozAlign: "left", headerHozAlign: "left", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Qty as per Wipro DC", field: "Qty_Wipro_Dc", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "QTY Received by Vendor", field: "Actu_Rece_Qty", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Reworked QTY Dispatched on DC", field: "Dispatch_Dc", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Reworked QTY Dispatched on Invoice ", field: "Dispatch_Invoice", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Non Repairable", field: "Non_Repairable", hozAlign: "left", headerHozAlign: "left", headerMenu: headerMenu, headerFilter: "input" },
        { title: "To be processed", field: "To_Process", hozAlign: "left", headerHozAlign: "left", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Lead time for Pending To be Processed QTY", field: "Pending_Write_Off", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
    ];

    if (typeof finalTable !== 'undefined' && finalTable instanceof Tabulator) {
        finalTable.replaceData(tabledata1);
    } else {
        finalTable = new Tabulator("#finalJob-table", {
            data: tabledata1,
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

    // Export to Excel on button click
    document.getElementById("exlExtButton").addEventListener("click", function () {
        // Get only visible data from Tabulator (respects filters, sorting, pagination)
        var visibleData = finalTable.getData("active"); // "active" gets only visible/filtered rows

        // Get visible columns only
        var visibleColumns = finalTable.getColumns().filter(col => col.isVisible() && col.getField() !== "Action");

        // Prepare headers
        var headers = visibleColumns.map(col => col.getDefinition().title);

        // Prepare data rows
        var rows = visibleData.map(row => {
            return visibleColumns.map(col => {
                var field = col.getField();
                return row[field] !== undefined ? row[field] : "";
            });
        });

        // Create date range text
        var dateRangeText = "";
        if (filterStartJobDate && filterEndJobDate) {
            dateRangeText = `Date Range: ${moment(filterStartJobDate).format('DD-MMM-YYYY')} to ${moment(filterEndJobDate).format('DD-MMM-YYYY')}`;
        } else if (filterStartJobDate) {
            dateRangeText = `Date From: ${moment(filterStartJobDate).format('DD-MMM-YYYY')}`;
        } else if (filterEndJobDate) {
            dateRangeText = `Date To: ${moment(filterEndJobDate).format('DD-MMM-YYYY')}`;
        } else {
            dateRangeText = "Date Range: All Dates";
        }

        // Combine: date range (row 1), empty row (row 2), headers (row 3), data (row 4+)
        var exportData = [
            [dateRangeText], // Row 1: Date range
            [],              // Row 2: Empty row
            headers,         // Row 3: Headers
            ...rows          // Row 4+: Data
        ];


        // Create worksheet
        var ws = XLSX.utils.aoa_to_sheet(exportData);

        // Style header row (bold)
        headers.forEach((header, index) => {
            const cellRef = XLSX.utils.encode_cell({ c: index, r: 0 });
            if (!ws[cellRef]) return;
            ws[cellRef].s = {
                font: { bold: true },
                fill: { fgColor: { rgb: "D3D3D3" } },
                alignment: { horizontal: "center" }
            };
        });

        // Auto-width calculation
        const columnWidths = headers.map(header => ({ wch: Math.max(header.length + 2, 10) }));
        ws['!cols'] = columnWidths;

        // Freeze first row
        ws['!freeze'] = { xSplit: 0, ySplit: 1 };

        // Set row heights
        if (!ws['!rows']) ws['!rows'] = [];
        ws['!rows'][0] = { hpt: 25 }; // Date range row height
        ws['!rows'][1] = { hpt: 10 };  // Empty row height
        ws['!rows'][2] = { hpt: 20 };  // Header row height

        // Create workbook and download
        var wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "JobWrkSpareOUT");

        var fileName = `JobWrkSpareOUT_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
        XLSX.writeFile(wb, fileName);
    });


    Blockloaderhide();
}
