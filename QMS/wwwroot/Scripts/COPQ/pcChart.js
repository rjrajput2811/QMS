let filterStartDatePc= moment().startOf('month').format('YYYY-MM-DD');
let filterEndDatePc = moment().endOf('month').format('YYYY-MM-DD');
let tablePc = null;

$(document).ready(function () {

    $('#PcdateRangeText').text(
        moment(filterStartDatePc).format('MMMM D, YYYY') + ' - ' + moment(filterEndDatePc).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerPc'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartDatePc = start.format('YYYY-MM-DD');
                filterEndDatePc = end.format('YYYY-MM-DD');
                $('#PcdateRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadPcData();
            });
            picker.on('clear', () => {
                filterStartDatePc = "";
                filterEndDatePc = "";
                $('#PcdateRangeText').text("Select Date Range");
                loadPcData();
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

    $('#customDateTriggerPc').on('click', function () {
        picker.show();
    });

    document.getElementById('backButton').addEventListener('click', function () {
        window.history.back();
    });

    $('#upload-button').on('click', async function () {
        var expectedColumns = [
            'Date', 'PC', 'FY', 'Qtr'
        ];

        var url = '/Service/UploadPcChartExcel';
        handleImportExcelFile(url, expectedColumns);
    });

    loadPcData();
});

function loadPcData() {
    Blockloadershow();

    $.ajax({
        url: '/Service/GetAllPc',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartDatePc,
            endDate: filterEndDatePc
        },
        success: function (data) {
            if (data.success && Array.isArray(data.data)) {
                renderPcTable(data.data);
            } else {
                showDangerAlert('No data available to load.');
                renderPcTable([]); // avoid errors
            }
        },
        error: function (xhr, status, error) {
            showDangerAlert('Error retrieving data: ' + error);
            Blockloaderhide();
        }
    });

}

function renderPcTable(response) {
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
            Date: item.date ? new Date(item.date).toLocaleDateString("en-GB") : "",
            PC: item.pC,
            FY: item.fY,
            Qtr: item.qtr,
            CreatedDate: item.createdDate ? new Date(item.createdDate).toLocaleDateString("en-GB") : "",
            CreatedBy: item.createdBy,
            UpdatedDate: item.updatedDate ? new Date(item.updatedDate).toLocaleDateString("en-GB") : "",
            UpdatedBy: item.updatedBy,
            IsDeleted: item.isDeleted
        });
    });

    console.log(tabledata);
    if (tabledata.length === 0 && tablePc) {
        tablePc.clearData();
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
                return `<i onclick="delConfirmPc(${rowData.Id}, this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`;
            }
        },
        { title: "SNo", field: "Sr_No", frozen: true, sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "center" },
        editableColumn("Date", "Date", "date", "center"),
        editableColumn("PC", "PC"),
        editableColumn("Financial Year", "FY"),
        editableColumn("Quater", "Qtr"),
        { title: "Created By", field: "CreatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated Date", field: "UpdatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" }
    ];

    if (tablePc) {
        tablePc.replaceData(tabledata);
    } else {
        tablePc = new Tabulator("#pc-table", {
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

        tablePc.on("cellEdited", function (cell) {
            InsertUpdatePc(cell.getRow().getData());
        });

        $("#addPcButton").on("click", function () {
            const newRow = {
                Id: 0,
                Sr_No: tablePc.getDataCount() + 1,
                Date: "",
                PC: "",
                FY: "",
                Qtr: "",
                CreatedBy: "",
                CreatedDate: "",
                UpdatedBy: "",
                UpdatedDate: ""
            };
            tablePc.addRow(newRow, false);
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

function InsertUpdatePc(rowData) {

    function toIsoDate(value) {
        if (!value) return "";
        const parts = value.split('/');
        return parts.length === 3 ? `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}` : value;
    }

    const cleanedData = {
        Id: rowData.Id || 0,
        Date: toIsoDate(rowData.Date),
        PC: rowData.PC || null,
        FY: rowData.FY || null,
        Qtr: rowData.Qtr || null

    };
    console.log(cleanedData);
    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/Service/CreatePc' : '/Service/UpdatePc';

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(cleanedData),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                loadPcData();
            } else {
                showDangerAlert(data.message || (isNew ? "Create failed." : "Update failed."));
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving Pc record.");
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

function delConfirmPc(Id, element) {

    // Case 1: Unsaved row — delete directly without confirmation
    if (!Id || Id <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = tablePc.getRow(rowEl);
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
            url: '/Service/PcDelete',
            type: 'POST',
            data: { id: Id },
            success: function (data) {
                if (data.success) {
                    showSuccessAlert("Deleted successfully.");
                    setTimeout(() => loadPcData(), 1000);
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

function BlankPcDown() {
    Blockloadershow();

    var expectedColumns = [
        'Date', 'PC', 'FY', 'Qtr'
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
    XLSX.utils.book_append_sheet(wb, ws, "Pc List Temaplate");

    XLSX.writeFile(wb, "PcList_Temaplate.xlsx");

    Blockloaderhide();
};

function openPcUpload() {
    clearForm();
    if (!$('#uploadModal').length) {
        $('body').append(partialView);
    }
    $('#uploadModal').modal('show');
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