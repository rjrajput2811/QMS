let filterStartDatePO = moment().startOf('month').format('YYYY-MM-DD');
let filterEndDatePO = moment().endOf('month').format('YYYY-MM-DD');
let tablePO = null;
let vendorOptions = {};
$(document).ready(function () {

    $('#POdateRangeText').text(
        moment(filterStartDatePO).format('MMMM D, YYYY') + ' - ' + moment(filterEndDatePO).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerPO'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartDatePO = start.format('YYYY-MM-DD');
                filterEndDatePO = end.format('YYYY-MM-DD');
                $('#POdateRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadDatapo();
            });
            picker.on('clear', () => {
                filterStartDatePO = "";
                filterEndDatePO = "";
                $('#POdateRangeText').text("Select Date Range");
                loadDatapo();
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

    $('#customDateTriggerPO').on('click', function () {
        picker.show();
    });

    document.getElementById('backButton').addEventListener('click', function () {
        window.history.back();
    });

    //$('#upload-button').on('click', async function () {
    //    var expectedColumns = [
    //        'Vendor', 'Material', 'Reference No', 'PO No', 'PO Date', 'PR No', 'Batch No', 'PO Qty', 'Balance Qty', 'Destination','Balance Value'
    //    ];

    //    var url = '/Service/UploadPoDumpExcel';
    //    handleImportExcelFile(url, expectedColumns);
    //});

    loadDatapo();
});

function loadDatapo() {
    Blockloadershow();

    $.ajax({
        url: '/Service/GetVendor',
        type: 'GET'
    }).done(function (vendorData) {
        if (Array.isArray(vendorData)) {
            vendorOptions = vendorData.reduce((acc, v) => {
                acc[v.value] = v.label;
                return acc;
            }, {});
        }

        $.ajax({
            url: '/Service/GetAllPO',
            type: 'GET',
            dataType: 'json',
            data: {
                startDate: filterStartDatePO,
                endDate: filterEndDatePO
            },
            success: function (data) {
                if (data.success && Array.isArray(data.data)) {
                    renderTable(data.data);
                } else {
                    showDangerAlert('No data available to load.');
                    renderTable([]); // avoid errors
                }
            },
            error: function (xhr, status, error) {
                showDangerAlert('Error retrieving data: ' + error);
                Blockloaderhide();
            }
        });

    }); // <- this was missing
}


function renderTable(response) {
    if (!Array.isArray(response)) {
        console.error("Invalid response, expected array:", response);
        showDangerAlert("Invalid data received. Expected array.");
        Blockloaderhide();
        return;
    }

    Blockloadershow();

    let tabledata = [];

    $.each(response, function (index, item) {
        tabledata.push({
            Sr_No: index + 1,
            Id: item.id,
            Key: item.key,
            Vendor: item.vendor,
            Material: item.material,
            ReferenceNo: item.referenceNo,
            PONo: item.poNo,
            PODate: item.poDate ? new Date(item.poDate).toLocaleDateString("en-GB") : "",
            PRNo: item.prNo,
            BatchNo: item.batchNo,
            POQty: item.poQty,
            BalanceQty: item.balanceQty,
            Destination: item.destination,
            BalanceValue: item.balanceValue,
            CreatedDate: item.createdDate ? new Date(item.createdDate).toLocaleDateString("en-GB") : "",
            CreatedBy: item.createdBy,
            UpdatedDate: item.updatedDate ? new Date(item.updatedDate).toLocaleDateString("en-GB") : "",
            UpdatedBy: item.updatedBy,
            IsDeleted: item.isDeleted
        });
    });
    console.log(tabledata);
    if (tabledata.length === 0 && tablePO) {
        tablePO.clearData();
        Blockloaderhide();
        return;
    }

    // Define your columns here or outside renderTable if reused
    const columns = [
        {
            title: "Action",
            field: "Action",
            hozAlign: "center",
            headerHozAlign: "center",
            frozen: true, headerMenu: headerMenu,
            formatter: function (cell) {
                const rowData = cell.getRow().getData();
                return `<i onclick="delConfirmPO(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`;
            }
        },
        { title: "SNo", field: "Sr_No", frozen: true, sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "center" },
        { title: "Key", field: "Key", frozen: true,headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "center" },
        editableColumn("PO Date", "PODate", "date", "center"),
        editableColumn("Vendor", "Vendor", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),
       // editableColumn("Vendor", "Vendor"),
        editableColumn("Material", "Material"),
        editableColumn("Reference No", "ReferenceNo"),
        editableColumn("PO No", "PONo"),
        editableColumn("PR No", "PRNo"),
        editableColumn("Batch No", "BatchNo"),
        editableColumn("PO Qty", "POQty", "input", "center"),
        editableColumn("Balance Qty", "BalanceQty", "input", "center"),
        editableColumn("Destination", "Destination"),
        editableColumn("Balance Value", "BalanceValue", "input", "center"),
        { title: "Created By", field: "CreatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated Date", field: "UpdatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" }
    ];

    if (tablePO) {
        tablePO.replaceData(tabledata);
    } else {
        tablePO = new Tabulator("#po-table", {
            data: tabledata,
            layout: "fitDataFill",
            movableColumns: true,
            pagination: "local",
            paginationSize: 10,
            paginationSizeSelector: [10, 50, 100, 500],
            paginationCounter: "rows",
            placeholder: "No data available",
            columns: columns
        });

        tablePO.on("cellEdited", function (cell) {
            saveEditedRowPO(cell.getRow().getData());
        });

        $("#addPOButton").on("click", function () {
            const newRow = {
                Id: 0,
                Sr_No: tablePO.getDataCount() + 1,
                Key: "",
                PODate: "",
                Vendor: "",
                Material: "",
                ReferenceNo: "",
                PONo: "",
                PRNo: "",
                BatchNo: "",
                POQty: 0,
                BalanceQty: 0,
                Destination: "",
                BalanceValue: 0,
                CreatedBy: "",
                CreatedDate: "",
                UpdatedBy: "",
                UpdatedDate: ""
            };
            tablePO.addRow(newRow, false);
        });

        // Export to Excel on button click
        document.getElementById("poExportButton").addEventListener("click", function () {
            // Get only visible data from Tabulator (respects filters, sorting, pagination)
            var visibleData = tablePO.getData("active"); // "active" gets only visible/filtered rows

            // Get visible columns only
            var visibleColumns = tablePO.getColumns().filter(col => col.isVisible() && col.getField() !== "Action");

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
            if (filterStartDatePO && filterEndDatePO) {
                dateRangeText = `Date Range: ${moment(filterStartDatePO).format('DD-MMM-YYYY')} to ${moment(filterEndDatePO).format('DD-MMM-YYYY')}`;
            } else if (filterStartDatePO) {
                dateRangeText = `Date From: ${moment(filterStartDatePO).format('DD-MMM-YYYY')}`;
            } else if (filterEndDatePO) {
                dateRangeText = `Date To: ${moment(filterEndDatePO).format('DD-MMM-YYYY')}`;
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
            XLSX.utils.book_append_sheet(wb, ws, "PendingPO");

            var fileName = `PendingPO_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
            XLSX.writeFile(wb, fileName);
        });

    }

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

function saveEditedRowPO(rowData) {
    

    function toIsoDate(value) {
        if (!value) return "";
        const parts = value.split('/');
        return parts.length === 3 ? `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}` : value;
    }
    console.log(rowData);

    const cleanedData = {
        Id: rowData.Id || 0,
        PODate: toIsoDate(rowData.PODate),
        Vendor: rowData.Vendor|| null,
        Material: rowData.Material || null,
        ReferenceNo: rowData.ReferenceNo || null,
        PONo: rowData.PONo || null,
        PRNo: rowData.PRNo || null,
        BatchNo: rowData.BatchNo || null,
        POQty: rowData.POQty || null,
        BalanceQty: rowData.BalanceQty || null,
        Destination: rowData.Destination || null,
        BalanceValue: rowData.BalanceValue || null
    };
    console.log(cleanedData);
    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/Service/CreatePO' : '/Service/UpdatePO';

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(cleanedData),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                //loadDatapo();
                if (isNew) {
                    showSuccessAlert("Saved successfully!.");
                    loadDatapo();
                }
            } else {
                showDangerAlert(data.message || (isNew ? "Create failed." : "Update failed."));
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving PO record.");
        }
    });
}

var headerMenu = function () {
    var menu = [];
    var columns = this.getColumns();

    columns.forEach(column => {
        let icon = document.createElement("i");
        icon.classList.add("fas", column.isVisible() ? "fa-check-square" : "fa-square");

        let label = document.createElement("span");
        let title = document.createElement("span");
        title.textContent = " " + column.getDefinition().title;

        label.appendChild(icon);
        label.appendChild(title);

        menu.push({
            label: label,
            action: function (e) {
                e.stopPropagation();
                column.toggle();
                icon.classList.toggle("fa-square", column.isVisible() === false);
                icon.classList.toggle("fa-check-square", column.isVisible() === true);
            }
        });
    });

    return menu;
};

function delConfirmPO(Id, element) {

    // Case 1: Unsaved row — delete directly without confirmation
    if (!Id || Id <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = tablePO.getRow(rowEl);
        if (row) {
            row.delete();
        }
        return;
    }

    PNotify.prototype.options.styling = "bootstrap3";
    new PNotify({
        title: 'Confirm Deletion',
        text: 'Are you sure you want to delete?',
        icon: 'fa fa-question-circle',
        hide: false,
        confirm: { confirm: true },
        buttons: { closer: false, sticker: false },
        history: { history: false }
    }).get().on('pnotify.confirm', function () {
        $.ajax({
            url: '/Service/PODelete',
            type: 'POST',
            data: { id: Id },
            success: function (data) {
                if (data.success) {
                    showSuccessAlert("Deleted successfully.");
                    setTimeout(() => loadDatapo(), 1000);
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
Tabulator.extendModule("edit", "editors", {
    select2: function (cell, onRendered, success, cancel, editorParams) {
        const values = editorParams.values || {};
        const select = document.createElement("select");
        select.style.width = "100%";

        for (let val in values) {
            let option = document.createElement("option");
            option.value = val;
            option.text = values[val];
            if (val === cell.getValue()) option.selected = true;
            select.appendChild(option);
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

function BlankPoDown() {
    Blockloadershow();

    var expectedColumns = [
        'Vendor', 'Material', 'Reference No', 'PO No', 'PO Date', 'Batch No', 'PO Qty', 'Balance Qty', 'Destination', 'Balance Value'
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
    XLSX.utils.book_append_sheet(wb, ws, "PO Dump Temaplate");

    XLSX.writeFile(wb, "PO_Dump_Temaplate.xlsx");

    Blockloaderhide();
};

//function openPoUpload() {
//    clearForm();
//    if (!$('#uploadModal').length) {
//        $('body').append(partialView);
//    }
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
