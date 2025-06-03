﻿var tabledata = [];
var table = '';
let vendorOptions = {};
let filterStartDate = moment().startOf('week').format('YYYY-MM-DD');
let filterEndDate = moment().endOf('week').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangeText').text(
        moment(filterStartDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndDate).format('MMMM D, YYYY')
    );

    // Initialize Litepicker and store reference
    const picker = new Litepicker({
        element: document.getElementById('customDateTrigger'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: {
            minYear: 2020,
            maxYear: null,
            months: true,
            years: true
        },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartDate = start.format('YYYY-MM-DD');
                filterEndDate = end.format('YYYY-MM-DD');
                $('#dateRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartDate = "";
                filterEndDate = "";
                $('#dateRangeText').text("Select Date Range");
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


    $('#customDateTrigger').on('click', function () {
        picker.show();
    });
    document.getElementById('backButton').addEventListener('click', function () {
        window.history.back();
    });

    loadData();
});
function loadData() {
    Blockloadershow();

    $.ajax({
        url: '/DNTracker/GetVendor',
        type: 'GET'
    }).done(function (vendorData) {
        if (Array.isArray(vendorData)) {
            vendorOptions = vendorData.reduce((acc, v) => {
                acc[v.value] = v.label;
                return acc;
            }, {});
        }

        // Now load the actual Deviation Note data
        $.ajax({
            url: '/DNTracker/GetAll',
            type: 'GET',
            dataType: 'json',
            data: {
                startDate: filterStartDate,
                endDate: filterEndDate
            },
            success: function (data) {
                if (Array.isArray(data)) {
                    OnTabGridLoad(data); // load into Tabulator or grid
                } else {
                    showDangerAlert('No Deviation Note data available.');
                }
                Blockloaderhide();
            },
            error: function () {
                showDangerAlert('Error loading deviation note data.');
                Blockloaderhide();
            }
        });

    }).fail(function () {
        showDangerAlert('Failed to load vendor dropdown options.');
        Blockloaderhide();
    });
}

var headerMenu = function () {
    var menu = [];
    var columns = this.getColumns();

    for (let column of columns) {
        let icon = document.createElement("i");
        icon.classList.add("fas");
        icon.classList.add(column.isVisible() ? "fa-check-square" : "fa-square");

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


Tabulator.extendModule("edit", "editors", {
    autocomplete_ajax: function (cell, onRendered, success, cancel, editorParams) {
        const input = document.createElement("input");
        input.setAttribute("type", "text");
        input.style.width = "100%";
        input.value = cell.getValue() || "";

        let dropdown = null;

        function removeDropdown() {
            if (dropdown && dropdown.parentNode && document.body.contains(dropdown)) {
                dropdown.parentNode.removeChild(dropdown);
            }
            dropdown = null;
        }

        function fetchSuggestions(query) {
            $.ajax({
                url: '/DNTracker/GetCodeSearch',
                type: 'GET',
                data: { search: query },
                success: function (data) {
                    removeDropdown(); // clear old

                    dropdown = document.createElement("div");
                    dropdown.className = "autocomplete-dropdown";

                    data.forEach(item => {
                        const option = document.createElement("div");
                        option.textContent = item.oldPart_No;
                        option.className = "autocomplete-option";

                        option.addEventListener("mousedown", function (e) {
                            e.stopPropagation();
                            success(item.oldPart_No);

                            const row = cell.getRow();
                            row.update({ ProductDescription: item.description });
                            removeDropdown();
                        });

                        dropdown.appendChild(option);
                    });

                    document.body.appendChild(dropdown);
                    const rect = input.getBoundingClientRect();
                    dropdown.style.top = (window.scrollY + rect.bottom) + "px";
                    dropdown.style.left = (window.scrollX + rect.left) + "px";
                    dropdown.style.width = rect.width + "px";
                },
                error: function () {
                    console.error("Failed to fetch suggestions.");
                }
            });
        }

        input.addEventListener("input", function () {
            const val = input.value;
            if (val.length >= 4) {
                fetchSuggestions(val);
            } else {
                removeDropdown();
            }
        });

        input.addEventListener("blur", function () {
            setTimeout(() => {
                removeDropdown();
                success(input.value);
            }, 150); // delay to allow click
        });

        return input;
    }
});

function OnTabGridLoad(response) {
    Blockloadershow();
    let tabledata = [];

    if (response.length > 0) {
        $.each(response, function (index, item) {
            tabledata.push({
                Sr_No: index + 1,
                DNoteId: item.id,
                DNoteNumber: item.dNoteNumber || "",
                DNoteCategory: item.dNoteCategory || "",
                ProductCode: item.productCode || "",
                ProductDescription: item.productDescription || "",
                Wattage: item.wattage || "",
                DQty: item.dQty || null,
                DRequisitionBy: item.dRequisitionBy || "",
                Vendor: item.vendor || "",
                Remark: item.remark || "",
                UpdatedDate: item.updatedDate || "",
                CreatedDate: item.createdDate || "",
                CreatedBy: item.createdBy || "",
                UpdatedBy: item.updatedBy || ""
            });
        });
    }

    const columns = [
        {
            title: "Action", field: "Action", frozen: true, hozAlign: "center", headerSort: false,
            width: 90,
            headerMenu: headerMenu,
            formatter: function (cell) {
                const rowData = cell.getRow().getData();
                return `<i onclick="delConfirm(${rowData.DNoteId})" class="fas fa-trash-alt text-danger" title="Delete" style="cursor:pointer;"></i>`;
            }
        },
        { title: "S.No", field: "Sr_No", frozen: true, hozAlign: "center", headerSort: false, headerMenu: headerMenu, width:90 },

        editableColumn("DNote Number", "DNoteNumber", "input", "center", "input", {}, {}, 150),

        editableColumn("Category", "DNoteCategory", "input", "center", "input", {}, {}, 130),

        editableColumn("Product Code", "ProductCode", "autocomplete_ajax"),
        editableColumn("Product Description", "ProductDescription"),

        editableColumn("Wattage", "Wattage", "input", "center", "input", {}, {}, 100),


        editableColumn("Quantity", "DQty", "input", "center", "input", {}, {}, 130),

        editableColumn("Requisition By", "DRequisitionBy", "input", "center", "input", {}, {}, 130),

        editableColumn("Vendor", "Vendor", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),

        editableColumn("Remark", "Remark", "input", "center", null, {}, {}, 200),
                {
            title: "Created By", field: "CreatedBy",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        },
        {
            title: "Created Date", field: "CreatedDate",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        },
        {
            title: "Updated By", field: "UpdatedBy",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        },
        {
            title: "Updated Date", field: "UpdatedDate",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        }
    ];

    if (table) {
        table.replaceData(tabledata);
    } else {
        table = new Tabulator("#dn_table", {
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

        table.on("cellEdited", function (cell) {
            const rowData = cell.getRow().getData();
            saveEditedRow(rowData);
        });
    }

    $("#addButton").off("click").on("click", function () {
        const newRow = {
            DNoteId: 0,
            Sr_No: table.getDataCount() + 1,
            DNoteNumber: "",
            DNoteCategory: "",
            ProductCode: "",
            ProductDescription: "",
            Wattage: "",
            DQty: null,
            DRequisitionBy: "",
            Vendor: "",
            Remark: ""
        };
        table.addRow(newRow, false); // add to bottom
    });

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

    // Set custom width for specific fields
    if (field === "ProductCode") {
        columnDef.width = 220;
        columnDef.minWidth = 220;
    }
    else if (field === "ProductDescription") {
        columnDef.width = 290;
        columnDef.minWidth = 290;
        columnDef.hozAlign = "left";
    }

    return columnDef;
}

function delConfirm(DNoteId) {
    PNotify.prototype.options.styling = "bootstrap3";
    (new PNotify({
        title: 'Confirm Deletion',
        text: 'Are you sure you want to delete this Deviation Note?',
        icon: 'fa fa-question-circle',
        hide: false,
        confirm: { confirm: true },
        buttons: { closer: false, sticker: false },
        history: { history: false }
    })).get().on('pnotify.confirm', function () {
        $.ajax({
            url: '/DNTracker/Delete',
            type: 'POST',
            data: { id: DNoteId },
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

function saveEditedRow(rowData) {
    function emptyToNull(value) {
        return value === "" ? null : value;
    }

    const cleanedData = {
        Id: rowData.DNoteId || 0,
        DNoteNumber: rowData.DNoteNumber || "",
        DNoteCategory: rowData.DNoteCategory || null,
        ProductCode: rowData.ProductCode || null,
        ProductDescription: rowData.ProductDescription || null,
        Wattage: rowData.Wattage || null,
        DQty: emptyToNull(rowData.DQty),
        DRequisitionBy: rowData.DRequisitionBy || null,
        Vendor: rowData.Vendor || null,
        Remark: rowData.Remark || null
    };

    const isNew = cleanedData.Id == 0;
    const url = isNew ? '/DNTracker/CreateAsync' : '/DNTracker/UpdateAsync';

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(cleanedData),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                if (isNew) {
                    loadData();
                }
                if (isNew && data.id) {
                    rowData.DNoteId = data.id;
                }
                
            } else {
                showDangerAlert(data.message || (isNew ? "Create failed." : "Update failed."));
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving record.");
        }
    });
}
