var tabledata = [];
var table = ''; let vendorOptions = {};
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

    // 🔑 Ensure calendar opens on click
    $('#customDateTrigger').on('click', function () {
        picker.show();
    });

    document.getElementById('backButton').addEventListener('click', function () {
        window.history.back();
    });

    loadData();
});
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

function loadData() {
    Blockloadershow();
    $.ajax({
        url: '/PDITracker/GetVendor',
        type: 'GET'
    }).done(function (vendorData) {
        if (Array.isArray(vendorData)) {
            vendorOptions = vendorData.reduce((acc, v) => {
                acc[v.value] = v.label;
                return acc;
            }, {});
        }

        // Nested AJAX call to get actual data after vendorOptions is populated
        $.ajax({
            url: '/PDITracker/GetAll',
            type: 'GET',
            dataType: 'json',
            data: {
                startDate: filterStartDate,
                endDate: filterEndDate
            },
        })
            .done(function (data) {
                const safeData = Array.isArray(data) ? data : [];
                if (safeData.length) {
                    console.log("PDI data:", safeData);
                    OnTabGridLoad(safeData);
                } else {
                    showDangerAlert('No data available to load.');
                    OnTabGridLoad([]);
                }
            })
            .fail(function (xhr, status, error) {
                console.error('Error retrieving data:', status, error, xhr.responseText);
                showDangerAlert('Error retrieving data: ' + (error || status));
                OnTabGridLoad([]);
            })
            .always(function () {
                Blockloaderhide();
            });

    });
}


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
                url: '/PDITracker/GetCodeSearch',
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
                            const data = row.getData();
                            //row.update({ ProductDescription: item.description });
                            row.update({ ProductDescription: item.description }).then(() => {
                                // Save full row (with updated description)
                                saveEditedRow({ ...data, ProductCode: item.oldPart_No, ProductDescription: item.description });
                            });

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

    // Utility function to format date to dd/MM/yyyy
    function formatDate(value) {
        return value ? new Date(value).toLocaleDateString("en-GB") : "";
    }

    // Prepare data
    if (response.length > 0) {
        $.each(response, function (index, item) {
            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                PC: item.pc || "",
                DispatchDate: formatDate(item.dispatchDate),
                ProductCode: item.productCode || "",
                ProductDescription: item.productDescription || "",
                BatchCodeVendor: item.batchCodeVendor || "",
                PONo: item.poNo || "",
                PDIDate: formatDate(item.pdiDate),
                PDIRefNo: item.pdiRefNo || "",
                OfferedQty: item.offeredQty,
                ClearedQty: item.clearedQty,
                BISCompliance: item.bisCompliance,
                InspectedBy: item.inspectedBy || "",
                Remark: item.remark || "",
                Attahcment: item.attahcment || "",
                Document_No: item.document_No || "",
                Revision_No: item.revision_No || "",
                Effective_Date: formatDate(item.effective_Date),
                Revision_Date: formatDate(item.revision_Date),
                UpdatedDate: item.updatedDate || "",
                CreatedDate: item.createdDate || "",
                CreatedBy: item.createdBy || "",
                UpdatedBy: item.updatedBy || ""
            });
        });
    }

    // Define columns
    const columns = [
        {
            title: "Action", field: "Action", frozen: true, hozAlign: "center", headerSort: false,
            width: 90,
            headerMenu: headerMenu,
            formatter: function (cell) {
                const rowData = cell.getRow().getData();
                return `<i onclick="delConfirm(${rowData.PDIId},this)" class="fas fa-trash-alt text-danger" title="Delete" style="cursor:pointer;"></i>`;
            }
        },
        { title: "S.No", field: "Sr_No", frozen: true, hozAlign: "center", headerSort: false, headerMenu: headerMenu, width: 80 },

        editableColumn("Dispatch Date", "DispatchDate", "date", "center", "input", {}, {}, 120),
        editableColumn("PC", "PC", "input", "center", "input", {}, {}, 160),
        editableColumn("Product Code", "ProductCode", "autocomplete_ajax"),
        editableColumn("Product Description", "ProductDescription", "input"),
        // editableColumn("Batch Code (Vendor)", "BatchCodeVendor", "input", "center", null, {}, {}, 140),
        editableColumn("Batch Code(Vendor)", "BatchCodeVendor", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),
        editableColumn("PDI Date", "PDIDate", "date", "center", "input", {}, {}, 120),
        editableColumn("PDI Ref No", "PDIRefNo", "input", "center", "input", {}, {}, 130),
        editableColumn("Offered Qty", "OfferedQty", "input", "center", "input", {}, {}, 110),
        editableColumn("Cleared Qty", "ClearedQty", "input", "center", "input", {}, {}, 110),
        editableColumn("BIS Compliance", "BISCompliance", "tickCross", "center", "input", {}, {}, function (cell) {
            return cell.getValue() ? "Yes" : "No";
        }, 130),
        editableColumn("Inspected By", "InspectedBy", "input", "center", "input", {}, {}, 130),
        editableColumn("Remark", "Remark", "input", "center", null, {}, {}, 180),
        {
            title: "Attachment",
            field: "Attahcment",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="/PDITrac_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file pdi-upload" data-id="${cell.getRow().getData().Id}" style="width:160px;" />
        `;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

        editableColumn("Document No", "Document_No", "input", "left", "input", {}, {}),
        editableColumn("Revision No", "Revision_No", "input", "center", "input", {}, {}),
        editableColumn("Effective Date", "Effective_Date", "date", "center", "input", {}, {}, 120),
        editableColumn("Revision Date", "Revision_Date", "date", "center", "input", {}, {}, 120),
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

    // Create or replace Tabulator table
    if (table) {
        table.replaceData(tabledata);
    } else {
        table = new Tabulator("#pdi_table", {
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

        // Handle cell edit event
        table.on("cellEdited", function (cell) {
            debugger
            saveEditedRow(cell.getRow().getData());
        });
    }

    $("#addPDIButton").on("click", function () {
        const newRow = {
            Id: 0,
            Sr_No: table.getDataCount() + 1,
            PC: "",
            DispatchDate: "",
            ProductDescription: "",
            BatchCodeVendor: "",
            PONo: "",
            PDIDate: "",
            PDIRefNo: "",
            OfferedQty: "",
            ClearedQty: "",
            BISCompliance: false,
            InspectedBy: "",
            Remark: "",
            Attahcment: "",
            Document_No: "",
            Revision_No: "",
            Effective_Date: "",
            Revision_Date: "",
        };
        table.addRow(newRow, false); // false = add to bottom
    });

    Blockloaderhide();
}

$('#pdi_table').on('change', '.pdi-upload', function () {
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
        url: "/PDITracker/UploadPDIAttachment",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            showSuccessAlert("File uploaded successfully.");
            table.updateData([{ Id: response.id, Attachment: response.fileName }]);
        } else {
            showDangerAlert(response.message || "Upload failed.");
        }
    }).fail(function () {
        showDangerAlert("Upload failed due to server error.");
    }).always(function () {
        Blockloaderhide();
    });
});


// Helper functions below — place these outside of OnTabGridLoad

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

function delConfirm(PDIId, element) {


    if (!PDIId || PDIId <= 0) {
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
        text: 'Are you sure to delete? It will not delete if this record is used in transactions.',
        icon: 'fa fa-question-circle',
        hide: false,
        confirm: { confirm: true },
        buttons: { closer: false, sticker: false },
        history: { history: false }
    })).get().on('pnotify.confirm', function () {
        $.ajax({
            url: '/PDITracker/Delete',
            type: 'POST',
            data: { id: PDIId },
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

    // Converts "dd/MM/yyyy" to "yyyy-MM-dd"
    function toIsoDate(value) {
        if (!value) return null;
        const parts = value.split('/');
        if (parts.length === 3) {
            return `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}`;
        }
        return null;
    }

    const cleanedData = {
        Id: rowData.Id || 0,
        PC: rowData.PC || "",
        DispatchDate: toIsoDate(rowData.DispatchDate) || null,
        ProductCode: rowData.ProductCode || "",
        ProductDescription: rowData.ProductDescription || "",
        BatchCodeVendor: rowData.BatchCodeVendor || "",
        PONo: rowData.PONo || "",
        PDIDate: toIsoDate(rowData.PDIDate) || null,
        PDIRefNo: rowData.PDIRefNo || "",
        OfferedQty: emptyToNull(rowData.OfferedQty),
        ClearedQty: emptyToNull(rowData.ClearedQty),
        BISCompliance: rowData.BISCompliance || false,
        InspectedBy: rowData.InspectedBy || "",
        Remark: rowData.Remark || "",
        Attahcment: rowData.Attahcment || null,
        Document_No: rowData.Document_No || "",
        Revision_No: rowData.Revision_No || "",
        Effective_Date: toIsoDate(rowData.Effective_Date),
        Revision_Date: toIsoDate(rowData.Revision_Date),
    };

    console.log("Cleaned data:", cleanedData);

    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/PDITracker/Create' : '/PDITracker/Update';

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
                    rowData.PDIId = data.id;
                }
            } else {
                var errorMessg = "";
                if (data.errors) {
                    for (var error in data.errors) {
                        if (data.errors.hasOwnProperty(error)) {
                            errorMessg += `${data.errors[error]}\n`;
                        }
                    }
                }
                showDangerAlert(errorMessg || data.message || "An error occurred while saving.");
            }
        },
        error: function (xhr, status, error) {
            showDangerAlert(xhr.responseText || "Error saving record.");
        }
    });
}
