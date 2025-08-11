let filterStartDateInvoice = moment().startOf('month').format('YYYY-MM-DD');
let filterEndDateInvoice = moment().endOf('month').format('YYYY-MM-DD');
let tableInvoice = null;

$(document).ready(function () {

    $('#InvdateRangeText').text(
        moment(filterStartDateInvoice).format('MMMM D, YYYY') + ' - ' + moment(filterEndDateInvoice).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerInv'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartDateInvoice = start.format('YYYY-MM-DD');
                filterEndDateInvoice = end.format('YYYY-MM-DD');
                $('#InvdateRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadInvoiceData();
            });
            picker.on('clear', () => {
                filterStartDateInvoice = "";
                filterEndDateInvoice = "";
                $('#InvdateRangeText').text("Select Date Range");
                loadInvoiceData();
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

    $('#customDateTriggerInv').on('click', function () {
        picker.show();
    });

    document.getElementById('backButton').addEventListener('click', function () {
        window.history.back();
    });

    //$('#upload-button').on('click', async function () {
    //    var expectedColumns = [
    //        'Key', 'Invoice No.', 'Invoice Type', 'Sales Order', 'Plant Code', 'Plant Name', 'Material No.', 'Description', 'Batch', 'Customer', 'Customer Name',
    //        'Name', 'Collective No', 'Your Reference', 'Invoice Date', 'Quantity', 'Cost'
    //    ];

    //    var url = '/Service/UploadInvoiceExcel';
    //    handleImportExcelFile(url, expectedColumns);
    //});

    loadInvoiceData();
});

function loadInvoiceData() {
    Blockloadershow();

    $.ajax({
        url: '/Service/GetAllInvoice',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartDateInvoice,
            endDate: filterEndDateInvoice
        },
        success: function (data) {
            if (data.success && Array.isArray(data.data)) {
                renderInvoiceTable(data.data);
            } else {
                showDangerAlert('No data available to load.');
                renderInvoiceTable([]); // avoid errors
            }
        },
        error: function (xhr, status, error) {
            showDangerAlert('Error retrieving data: ' + error);
            Blockloaderhide();
        }
    });

}

function renderInvoiceTable(response) {
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
            Inv_No: item.inv_No,
            Inv_Date: item.inv_Date ? new Date(item.inv_Date).toLocaleDateString("en-GB") : "",
            Inv_Type: item.inv_Type,
            Sales_Order: item.sales_Order,
            Plant_Code: item.plant_Code,
            Plant_Name: item.plant_Name,
            Material_No: item.material_No,
            Dealer_Name: item.dealer_Name,
            End_Customer: item.end_Customer,
            Collective_No: item.collective_No,
            Indent_No: item.indent_No,
            Quantity: item.quantity,
            Cost: item.cost,
            CreatedDate: item.createdDate ? new Date(item.createdDate).toLocaleDateString("en-GB") : "",
            CreatedBy: item.createdBy,
            UpdatedDate: item.updatedDate ? new Date(item.updatedDate).toLocaleDateString("en-GB") : "",
            UpdatedBy: item.updatedBy,
            IsDeleted: item.isDeleted
        });
    });

    console.log(tabledata);
    if (tabledata.length === 0 && tableInvoice) {
        tableInvoice.clearData();
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
                return `<i onclick="delConfirmInvoice(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`;
            }
        },
        { title: "SNo", field: "Sr_No", frozen: true, sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "center" },
        { title: "Key", field: "Key", frozen: true, headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "center" },
        //editableColumn("Key", "Key"),
        editableColumn("Invoice Date", "Inv_Date", "date", "center"),
        editableColumn("Invoice No.", "Inv_No"),
        editableColumn("Invoice Type", "Inv_Type"),
        editableColumn("Sales Order", "Sales_Order"),
        editableColumn("Plant Code", "Plant_Code"),
        editableColumn("Plant Name", "Plant_Name"),
        editableColumn("Material No.", "Material_No"),
        editableColumn("Dealer Name", "Dealer_Name"),
        editableColumn("End Customer", "End_Customer"),
        editableColumn("Collective_No", "Collective_No"),
        editableColumn("Indent No", "Indent_No"),
        editableColumn("Quantity", "Quantity"),
        editableColumn("Cost", "Cost"),
        { title: "Created By", field: "CreatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated Date", field: "UpdatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" }
    ];

    if (tableInvoice) {
        tableInvoice.replaceData(tabledata);
    } else {
        tableInvoice = new Tabulator("#inv-table", {
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

        tableInvoice.on("cellEdited", function (cell) {
            InsertUpdateInvoice(cell.getRow().getData());
        });

        $("#addInvButton").on("click", function () {
            const newRow = {
                Id: 0,
                Sr_No: tableInvoice.getDataCount() + 1,
                Key: "",
                Inv_No: "",
                Inv_Date: "",
                Inv_Type: "",
                Sales_Order: "",
                Plant_Code: "",
                Plant_Name: "",
                Material_No: "",
                Dealer_Name: "",
                End_Customer: "",
                Collective_No: "",
                Indent_No: "",
                Quantity: "",
                Cost: "",
                CreatedBy: "",
                CreatedDate: "",
                UpdatedBy: "",
                UpdatedDate: ""
            };
            tableInvoice.addRow(newRow, false);
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

function InsertUpdateInvoice(rowData) {

    function toIsoDate(value) {
        if (!value) return "";
        const parts = value.split('/');
        return parts.length === 3 ? `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}` : value;
    }

    const cleanedData = {
        Id: rowData.Id || 0,
        Inv_Date: toIsoDate(rowData.Inv_Date),
        Key: rowData.Key || null,
        Inv_No: rowData.Inv_No || null,
        Inv_Type: rowData.Inv_Type || null,
        Sales_Order: rowData.Sales_Order || null,
        Plant_Code: rowData.Plant_Code || null,
        Material_No: rowData.Material_No || null,
        Description: rowData.Description || null,
        Batch: rowData.Batch || null,
        Customer: rowData.Customer || null,
        Customer_Name: rowData.Customer_Name || null,
        Name: rowData.Name || null,
        Collective_No: rowData.Collective_No || null,
        Created_By: rowData.Vendor || null,
        Reference: rowData.Reference || null,
        Quantity: rowData.Quantity || null,
        Cost: rowData.Cost || null
        
    };
    console.log(cleanedData);
    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/Service/CreateInvoice' : '/Service/UpdateInvoice';

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(cleanedData),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                //loadInvoiceData();
                if (isNew) {
                    showSuccessAlert("Saved successfully!.");
                    loadInvoiceData();
                }
            } else {
                showDangerAlert(data.message || (isNew ? "Create failed." : "Update failed."));
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving Invoice record.");
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

function delConfirmInvoice(Id, element) {

    // Case 1: Unsaved row — delete directly without confirmation
    if (!Id || Id <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = tableInvoice.getRow(rowEl);
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
            url: '/Service/InvoiceDelete',
            type: 'POST',
            data: { id: Id },
            success: function (data) {
                if (data.success) {
                    showSuccessAlert("Deleted successfully.");
                    setTimeout(() => loadInvoiceData(), 1000);
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

function BlankInvDown() {
    Blockloadershow();

    var expectedColumns = [
        'Invoice No.', 'Invoice Type', 'Sales Order', 'Plant Code', 'Plant Name', 'Material No.', 'Dealer Name', 'End Customer ',
        'Collective No', 'Indent No', 'Invoice Date', 'Quantity', 'Cost'
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
    XLSX.utils.book_append_sheet(wb, ws, "Invoice List Temaplate");

    XLSX.writeFile(wb, "InvoiceList_Temaplate.xlsx");

    Blockloaderhide();
};

//function openInvUpload() {
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