var tabledata = [];
var table = null;
var outtbl = null;
var tabledata1 = [];
var finalTable = null;
let filterStartRLTDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndRLTDate = moment().endOf('month').format('YYYY-MM-DD');
var userType = document.getElementById("hdnUserType").value;
var vendorName = document.getElementById("hdnVendorName").value;

$(document).ready(function () {
    if (userType != "Vendor") {
        $('#rLTRangeText').text(
            moment(filterStartRLTDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndRLTDate).format('MMMM D, YYYY')
        );

        const picker = new Litepicker({
            element: document.getElementById('rLTDateTrigger'),
            singleMode: false,
            format: 'DD-MM-YYYY',
            numberOfMonths: 2,
            numberOfColumns: 2,
            dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
            plugins: ['ranges'],
            setup: (picker) => {
                picker.on('selected', (start, end) => {
                    filterStartRLTDate = start.format('YYYY-MM-DD');
                    filterEndRLTDate = end.format('YYYY-MM-DD');
                    $('#rLTRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                    loadRLTData();
                });

                picker.on('clear', () => {
                    filterStartRLTDate = "";
                    filterEndRLTDate = "";
                    $('#rLTRangeText').text("Select Date Range");
                    loadRLTData();
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

        $('#rLTDateTrigger').on('click', function () {
            picker.show();
        });

        document.getElementById('backButton').addEventListener('click', function () {
            window.history.back();
        });
    }

    if (userType != "Vendor") {
        $('#upload-button').on('click', async function () {
            var expectedColumns = [
                'Vendor', 'Material', 'Reference No', 'PO No', 'PO Date', 'PR No', 'Batch No', 'PO Qty',
                'Balance Qty', 'Destination', 'Balance Value', 'Lead time', 'Lead time Range', 'Wipro Remark'
            ];

            var url = '/Service/UploadRLTExcel';
            handleImportExcelFileBak(url, expectedColumns);
        });
    }
    else {
        $('#upload-button').on('click', async function () {
            var expectedColumns = [
                'SNo','Unique Id','Vendor', 'Material', 'Reference No', 'PO No', 'PO Date', 'PR No', 'Batch No', 'PO Qty',
                'Balance Qty', 'Destination', 'Balance Value', 'Lead time', 'Lead time Range', 'Dispatch Date','Remark'
            ];

            var url = '/Service/UploadRLTVendorInputExcel';
            handleImportExcelFile(url, expectedColumns);
        });
    }

    if (userType != "Vendor") {
        loadRLTData();
    }
    else {
        loadVendorRLTData();
    }
});

function openUploadRLT() {

    clearForm();
    if (!$('#uploadModal').length) {
        $('body').append(partialView);
    }
    $('#uploadModal').modal('show');
}

function loadRLTData() {
    Blockloadershow();
    $('#ftab2').hide();
    $('#ftab1').show();
    $.ajax({
        url: '/Service/GetRLTAll',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartRLTDate,
            endDate: filterEndRLTDate
        },
        success: function (data) {
            if (Array.isArray(data)) {
                renderRLTTable(data); // load into Tabulator or grid
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

function loadVendorRLTData() {
    Blockloadershow();
    $('#ftab2').hide();
    $('#ftab1').show();
    $.ajax({
        url: '/Service/GetRLTById',
        type: 'GET',
        dataType: 'json',
        data: {
            vendor: vendorName
        },
        success: function (data) {
            if (Array.isArray(data)) {
                renderRLTTable(data); // load into Tabulator or grid
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
function renderRLTTable(response) {

    Blockloadershow();

    const tabledata = response.map((item, index) => ({
        Sr_No: index + 1,
        Vendor: item.vendor,
        Material: item.material,
        Ref_No: item.ref_No,
        Po_No: item.po_No,
        Po_Date: item.po_Date ? new Date(item.po_Date).toLocaleDateString("en-GB") : "",
        PR_No: item.pR_No,
        Batch_No: item.batch_No,
        Po_Qty: item.po_Qty,

        Balance_Qty: item.balance_Qty,
        Destination: item.destination,
        Balance_Value: item.balance_Value,
        Lead_Time: item.lead_Time,
        Lead_Time_Range: item.lead_Time_Range,
        Dispatch_Date: item.dispatch_Date ? new Date(item.dispatch_Date).toLocaleDateString("en-GB") : "",

        Remark: item.Remark,
        Wipro_Remark: item.wipro_Remark,
        UniqueId: item.uniqueId,

        CreatedDate: item.createdDate ? new Date(item.createdDate).toLocaleDateString("en-GB") : "",
        CreatedBy: item.createdBy,
        UpdatedDate: item.updatedDate ? new Date(item.updatedDate).toLocaleDateString("en-GB") : "",
        UpdatedBy: item.updatedBy,
    }));

    const columns = [


        {
            title: "Input for Wipro",
            headerHozAlign: "center", // Center group header
            columns: [

                { title: "SNo", field: "Sr_No", hozAlign: "center", headerHozAlign: "center", frozen: true },
                { title: "Unique Id", field: "UniqueId", hozAlign: "left", headerHozAlign: "center", frozen: true },
                { title: "Vendor", field: "Vendor", frozen: true, headerMenu: headerMenu, headerFilter: "input" },
                { title: "Material", field: "Material", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
                { title: "Reference No", field: "Ref_No", headerMenu: headerMenu,headerFilter: "input"},
                { title: "PO No", field: "Po_No", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "PO Date", field: "Po_Date", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "PR No", field: "PR_No", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "Batch No", field: "Batch_No", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "PO Qty", field: "Po_Qty", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "Balance Qty", field: "Balance_Qty", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "Destination", field: "Destination", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "Balance Value", field: "Balance_Value", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
            ]
        },

        {
            title: "Calculate from Input ",
            headerHozAlign: "center", // Center group header
            columns: [
                { title: "Lead time", field: "Lead_Time", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
                { title: "Lead time Range", field: "Lead_Time_Range", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu,headerFilter: "input" },
            ]
        },

        {
            title: "Vendor to Update ",
            headerHozAlign: "center", // Center group header
            columns: [
                { title: "Dispatch Date", field: "Dispatch_Date", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
                { title: "Remark", field: "Remark", headerMenu: headerMenu, headerFilter: "input" },
            ]
        },

        {
            title: "Wipro Remark",
            headerHozAlign: "center", // Center group header
            columns: [
                { title: "Wipro Remark", field: "Wipro_Remark", dowload: false, headerMenu: headerMenu, headerFilter: "input" },
            ]
        },

        { title: "Created By", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", dowload: false, hozAlign: "center", headerHozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", headerMenu: headerMenu, headerFilter: "input", dowload: false , hozAlign: "center", headerHozAlign: "center"},
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", dowload: false , hozAlign: "center", headerHozAlign: "center"},
        { title: "Updated Date", field: "UpdatedDate", headerMenu: headerMenu, headerFilter: "input", dowload: false, hozAlign: "center", headerHozAlign: "center" }
    ];

    if (typeof table !== 'undefined' && table instanceof Tabulator) {
        table.replaceData(tabledata);
    } else {
        table = new Tabulator("#rLT-table", {
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
            var visibleColumns = table.getColumns().filter(col => col.isVisible() && col.getField() !== "Action");

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
            if (filterStartRLTDate && filterEndRLTDate) {
                dateRangeText = `Date Range: ${moment(filterStartRLTDate).format('DD-MMM-YYYY')} to ${moment(filterEndRLTDate).format('DD-MMM-YYYY')}`;
            } else if (filterStartRLTDate) {
                dateRangeText = `Date From: ${moment(filterStartRLTDate).format('DD-MMM-YYYY')}`;
            } else if (filterEndRLTDate) {
                dateRangeText = `Date To: ${moment(filterEndRLTDate).format('DD-MMM-YYYY')}`;
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
            XLSX.utils.book_append_sheet(wb, ws, "RLTTrackIN");

            var fileName = `RLTTrackIN_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
            XLSX.writeFile(wb, fileName);
        });
    }
    else {
        //document.getElementById("exclVendorExpButton").addEventListener("click", function () {
        //    // Get only visible data from Tabulator (respects filters, sorting, pagination)
        //    var visibleData = table.getData("active"); // "active" gets only visible/filtered rows

        //    // Get visible columns only
        //    var visibleColumns = table.getColumns().filter(col => col.isVisible() && col.getField() !== "Action"
        //        && col.getField() !== "Wipro_Remark");

        //    // Prepare headers
        //    var headers = visibleColumns.map(col => col.getDefinition().title);

        //    // Prepare data rows
        //    var rows = visibleData.map(row => {
        //        return visibleColumns.map(col => {
        //            var field = col.getField();
        //            return row[field] !== undefined ? row[field] : "";
        //        });
        //    });

        //    // Combine: date range (row 1), empty row (row 2), headers (row 3), data (row 4+)
        //    var exportData = [
        //        headers,         // Row 3: Headers
        //        ...rows          // Row 4+: Data
        //    ];


        //    // Create worksheet
        //    var ws = XLSX.utils.aoa_to_sheet(exportData);

        //    // Style header row (bold)
        //    headers.forEach((header, index) => {
        //        const cellRef = XLSX.utils.encode_cell({ c: index, r: 0 });
        //        if (!ws[cellRef]) return;
        //        ws[cellRef].s = {
        //            font: { bold: true },
        //            fill: { fgColor: { rgb: "D3D3D3" } },
        //            alignment: { horizontal: "center" }
        //        };
        //    });

        //    // Auto-width calculation
        //    const columnWidths = headers.map(header => ({ wch: Math.max(header.length + 2, 10) }));
        //    ws['!cols'] = columnWidths;

        //    // Freeze first row
        //    ws['!freeze'] = { xSplit: 0, ySplit: 1 };

        //    // Set row heights
        //    if (!ws['!rows']) ws['!rows'] = [];
        //    ws['!rows'][0] = { hpt: 25 }; // Date range row height

        //    // Create workbook and download
        //    var wb = XLSX.utils.book_new();
        //    XLSX.utils.book_append_sheet(wb, ws, "RLTTrackIN");

        //    var fileName = `RLTTrackIN_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
        //    XLSX.writeFile(wb, fileName);
        //});

        document.getElementById("exclVendorExpButton").addEventListener("click", async function () {
            var visibleData = table.getData("active");
            var visibleColumns = table.getColumns().filter(col => col.isVisible() && col.getField() !== "Action"
                 && col.getField() !== "Wipro_Remark");

            var headers = visibleColumns.map(col => col.getDefinition().title);

            var rows = visibleData.map(row => {
                return visibleColumns.map(col => {
                    var field = col.getField();
                    return row[field] !== undefined ? row[field] : "";
                });
            });

            // ── ExcelJS workbook ──────────────────────────────────────────────
            const workbook = new ExcelJS.Workbook();
            const worksheet = workbook.addWorksheet("RLTTrackIN");

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
            a.download = `RLTTrackIN_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
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


function toIsoDateRLT(value) {
    if (!value) return "";
    const parts = value.split('/');
    return parts.length === 3 ? `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}` : value;
}

function saveEditedRow(rowData) {
    const cleanedData = {
        Id: rowData.Id || 0,
        CCN_No: rowData.CCNNo || null,
        CCCNDate: toIsoDateRLT(rowData.CCCNDate),
        ReportedBy: rowData.ReportedBy || null,
        CLocation: rowData.CLocation || null,
        CustName: rowData.CustName || null,
        DealerName: rowData.DealerName || null,
        CDescription: rowData.CDescription || null,
        CStatus: rowData.CStatus || null,
        Completion: toIsoDateRLT(rowData.Completion),
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
            url: '/Service/RLTDelete',
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

//function BlankRLTTrackDown() {
//    Blockloadershow();

//    var expectedColumns = [
//        'Vendor', 'Material', 'Reference No', 'PO No', 'PO Date', 'PR No', 'Batch No', 'PO Qty',
//        'Balance Qty', 'Destination', 'Balance Value', 'Lead time', 'Lead time Range', 'Wipro Remark'
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
//    XLSX.utils.book_append_sheet(wb, ws, "RLT Tracking for Vendors ");

//    XLSX.writeFile(wb, "RLT_Tracking.xlsx");

//    Blockloaderhide();
//};

async function BlankRLTTrackDown() {
    Blockloadershow();

    var expectedColumns = [
        'Vendor', 'Material', 'Reference No', 'PO No', 'PO Date', 'PR No', 'Batch No', 'PO Qty',
        'Balance Qty', 'Destination', 'Balance Value', 'Lead time', 'Lead time Range', 'Wipro Remark'
    ];

    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('RLT Tracking for Vendors');

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
    a.download = 'RLT_Tracking.xlsx.xlsx';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);

    Blockloaderhide();
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

function loadFinalRLTData() {
    Blockloadershow();
    $('#ftab1').hide();
    $('#ftab2').show();
    $.ajax({
        url: '/Service/GetFinalRLT',
        type: 'GET',
        success: function (data) {
            if (Array.isArray(data)) {
                renderFinalRLTTable(data); // load into Tabulator or grid
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


function renderFinalRLTTable(response) {
    debugger;

    Blockloadershow();

    const tabledata1 = response.map((item, index) => ({
        Sr_No: index + 1,
        Vendor: item.vendor,
        Day0To5: item.day0To5,
        Day6To10: item.day6To10,
        Day11To15: item.day11To15,
        Gt15Days: item.gt15Days,
        Total: item.total
    }));

    const columns = [
        { title: "SNo", field: "Sr_No", width: 80, hozAlign: "center", headerHozAlign: "center", },
        { title: "Vendor", field: "Vendor", width: 304, hozAlign: "left", headerHozAlign: "left", headerMenu: headerMenu, headerFilter: "input" },
        { title: "0-5 Days", field: "Day0To5", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "6-10 Days", field: "Day6To10", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "11-15 Days", field: "Day11To15", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Gt-15 Days", field: "Gt15Days", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
        { title: "Total", field: "Total", hozAlign: "center", headerHozAlign: "center", headerMenu: headerMenu, headerFilter: "input" },
    ];

    if (typeof finalTable !== 'undefined' && finalTable instanceof Tabulator) {
        finalTable.replaceData(tabledata1);
    } else {
        finalTable = new Tabulator("#finalRLT-table", {
            data: tabledata1,
            layout: "fitColumns",
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
        if (filterStartRLTDate && filterEndRLTDate) {
            dateRangeText = `Date Range: ${moment(filterStartRLTDate).format('DD-MMM-YYYY')} to ${moment(filterEndRLTDate).format('DD-MMM-YYYY')}`;
        } else if (filterStartRLTDate) {
            dateRangeText = `Date From: ${moment(filterStartRLTDate).format('DD-MMM-YYYY')}`;
        } else if (filterEndRLTDate) {
            dateRangeText = `Date To: ${moment(filterEndRLTDate).format('DD-MMM-YYYY')}`;
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
        XLSX.utils.book_append_sheet(wb, ws, "RLTTrackOUT");

        var fileName = `RLTTrackOUT_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
        XLSX.writeFile(wb, fileName);
    });

    Blockloaderhide();
}
