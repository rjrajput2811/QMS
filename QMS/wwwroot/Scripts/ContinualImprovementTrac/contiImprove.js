var table = null;
let vendorOptions = {};
let filterStartContiDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndContiDate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangeConti').text(
        moment(filterStartContiDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndContiDate).format('MMMM D, YYYY')
    );

    // Initialize Litepicker
    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerConti'),
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
                filterStartContiDate = start.format('YYYY-MM-DD');
                filterEndContiDate = end.format('YYYY-MM-DD');
                $('#dateRangeConti').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartContiDate = "";
                filterEndContiDate = "";
                $('#dateRangeConti').text("Select Date Range");
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

    $('#customDateTriggerConti').on('click', function () {
        picker.show();
    });

    $('#backButton').on('click', function () {
        window.history.back();
    });

    loadData();
});

function loadData() {
    Blockloadershow();

    $.ajax({
        url: '/ContiImprove/GetAll',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartContiDate,
            endDate: filterEndContiDate
        },
        success: function (data) {
            if (Array.isArray(data)) {
                OnTabGridLoad(data);
            } else {
                showDangerAlert('No Kaizen data available.');
            }
            Blockloaderhide();
        },
        error: function () {
            showDangerAlert('Error loading Kaizen data.');
            Blockloaderhide();
        }
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
    Blockloadershow();

    let tabledata = [];
    let columns = [];

    if (response.length > 0) {
        $.each(response, function (index, item) {

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Date: formatDate(item.date || ""),
                Conti_Improve_Plane: item.conti_Improve_Plane || "",
                Iso_9001: item.iso_9001 || "",
                Iso9001_Plane_Implement: item.iso9001_Plane_Implement || "",
                Iso_14001: item.iso_14001 || "",
                Iso14001_Plane_Implement: item.iso14001_Plane_Implement || "",
                Iso_45001: item.iso_45001 || "",
                Iso_45001_Plane_Implement: item.iso_45001_Plane_Implement || "",
                FY: item.fy || "",
                CreatedDate: formatDate(item.createdDate || ""),
                UpdatedDate: formatDate(item.updatedDate || ""),
                CreatedBy: item.createdBy || "",
                UpdatedBy: item.updatedBy || ""
            });
        });
    }
    console.log(tabledata);


    if (tabledata.length === 0 && table) {
        table.clearData();
        Blockloaderhide();
        return;
    }

    columns.push(
        {
            title: "Action",
            field: "Action",
            width: 40,
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
            title: "SNo", field: "Sr_No", sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left", width: 60
        },
        editableColumn("Date", "Date", "date", "center"),

        {
            title: "FY",
            field: "FY",
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

        editableColumn("Continual improvement done/Planned", "Conti_Improve_Plane", true),

        editableColumn("ISO 9001", "Iso_9001", "tickCross", "center", "input", {}, {}, function (cell) {
            return cell.getValue() ? "Yes" : "No";
        }, 130),

        /*editableColumn("Planned /Implemented", "Iso9001_Plane_Implement", true),*/
        editableColumn(
            "Planned /Implemented",
            "Iso9001_Plane_Implement",
            "list", // makes it dropdown
            "center",
            "input",
            {},
            {
                values: ["Planned", "Implemented"]   // direct values here
            },
            function (cell) {
                return cell.getValue() || "";
            }
        ),


        editableColumn("ISO 14001", "Iso_14001", "tickCross", "center", "input", {}, {}, function (cell) {
            return cell.getValue() ? "Yes" : "No";
        }, 130),

        //editableColumn("Planned /Implemented", "Iso14001_Plane_Implement", true),
        editableColumn(
            "Planned /Implemented",
            "Iso14001_Plane_Implement",
            "list", // makes it dropdown
            "center",
            "input",
            {},
            {
                values: ["Planned", "Implemented"]   // direct values here
            },
            function (cell) {
                return cell.getValue() || "";
            }
        ),


        editableColumn("ISO 45001", "Iso_45001", "tickCross", "center", "input", {}, {}, function (cell) {
            return cell.getValue() ? "Yes" : "No";
        }, 130),

        //editableColumn("Planned /Implemented", "Iso_45001_Plane_Implement", true),
        editableColumn(
            "Planned /Implemented",
            "Iso_45001_Plane_Implement",
            "list", // makes it dropdown
            "center",
            "input",
            {},
            {
                values: ["Planned", "Implemented"]   // direct values here
            },
            function (cell) {
                return cell.getValue() || "";
            }
        ),

        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false }
    );

    // // Initialize Tabulator
    table = new Tabulator("#contiTrac_Table", {
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
        InsertUpdateContiImprove(cell.getRow().getData());
    });

    (function bindAddButtonOnce() {
        var $btn = $("#addContiButton");
        $btn.attr("type", "button");
        $btn.off("click.addrow").on("click.addrow", function (e) {
            e.preventDefault(); e.stopPropagation();
            if ($btn.data("busy")) return;
            $btn.data("busy", true).prop("disabled", true);

            try {
                const fyOptions = getFinancialYears() || {};
                const currentFY = Object.keys(fyOptions)[0] || "";

                const newRow = {
                    Sr_No: 1,               // will renumber after insert
                    Id: 0,
                    FY: currentFY,
                    Date: "",
                    Conti_Improve_Plane: "",
                    Iso_9001: false,
                    Iso9001_Plane_Implement: "",
                    Iso_14001: false,
                    Iso14001_Plane_Implement: "",
                    Iso_45001: false,
                    Iso_45001_Plane_Implement: "",
                    CreatedBy: "",
                    UpdatedBy: "",
                    UpdatedDate: "",
                    CreatedDate: ""
                };

                table.addRow(newRow, true).then(function (row) {
                    table.scrollToRow(row, "top", false);
                    if (row.select) row.select();

                    const el = row.getElement();
                    el.classList.add("row-flash");
                    setTimeout(() => el.classList.remove("row-flash"), 1200);

                    renumberSrNo(); // keep S.No sequence
                }).catch(console.error).finally(function () {
                    $btn.data("busy", false).prop("disabled", false);
                });

            } catch (err) {
                console.error(err);
                $btn.data("busy", false).prop("disabled", false);
            }
        });
    })();

    // helper to renumber Sr_No after inserts/deletes/sorts (if needed)
    document.getElementById("exportContiButton").addEventListener("click", async function () {
        // ===== 0) OPTIONS =====
        const EXPORT_SCOPE = "active";   // "active" | "selected" | "all"
        const EXPORT_RAW = false;      // false = formatted values exactly as shown in Tabulator

        // ===== 1) COLUMNS from Tabulator (exact view) with EXCLUDES =====
        if (!window.table) { console.error("Tabulator 'table' not found."); return; }

        const EXCLUDE_FIELDS = new Set(["Action", "action", "Actions", "CreatedBy"]);
        const EXCLUDE_TITLES = new Set(["Action", "Actions", "User"]);

        const tabCols = table.getColumns(true)
            .filter(c => c.getField())
            .filter(c => c.isVisible())
            .filter(c => {
                const def = c.getDefinition();
                const field = def.field || "";
                const title = (def.title || "").trim();
                return !EXCLUDE_FIELDS.has(field) && !EXCLUDE_TITLES.has(title);
            });

        const excelCols = tabCols.map(col => {
            const def = col.getDefinition();
            const label = def.title || def.field;
            const px = (def.width || col.getWidth() || 120);
            const width = Math.max(8, Math.min(40, Math.round(px / 7))); // px->char heuristic
            return { label, key: def.field, width };
        });

        if (!excelCols.length) { alert("No visible columns to export."); return; }

        // ===== 2) DOC DETAILS (will be placed in second-last + last column) =====
        const docDetails = [
            ["Document No", "WCIB/LS/QA/R/005"],
            ["Effective Date", "01/10/2022"],
            ["Revision No", "0"],
            ["Revision Date", "01/10/2022"],
            ["Page No", "1 of 1"]
        ];

        // ===== 3) LAYOUT =====
        const TOTAL_COLS = excelCols.length;

        const HEADER_TOP = 1;
        const HEADER_BOTTOM = 5;
        const GRID_HEADER_ROW = HEADER_BOTTOM + 1;
        const TITLE_TEXT = "CONTINUAL IMPROVEMENT TRACKER";

        const LOGO_COL_START = 1;
        const LOGO_COL_END = 2;
        const LOGO_ROW_START = HEADER_TOP;
        const LOGO_ROW_END = HEADER_BOTTOM;

        // Title must not overlap the final 2 columns (reserved for details)
        const TITLE_COL_START = Math.min(3, TOTAL_COLS);
        const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - 2);

        // Document details strictly in second-last & last column
        const DETAILS_LABEL_COL = Math.max(1, TOTAL_COLS - 1);
        const DETAILS_VALUE_COL = TOTAL_COLS;

        // ===== 4) HELPERS =====
        async function fetchAsBase64(url) {
            const res = await fetch(url);
            const blob = await res.blob();
            return new Promise((resolve) => {
                const reader = new FileReader();
                reader.onloadend = () => resolve(reader.result.split(",")[1]);
                reader.readAsDataURL(blob);
            });
        }
        function setBorder(cell, style = "thin") {
            cell.border = { top: { style }, bottom: { style }, left: { style }, right: { style } };
        }
        function outlineRange(ws, r1, c1, r2, c2, style = "thin") {
            for (let c = c1; c <= c2; c++) {
                const top = ws.getCell(r1, c), bottom = ws.getCell(r2, c);
                top.border = { ...top.border, top: { style } };
                bottom.border = { ...bottom.border, bottom: { style } };
            }
            for (let r = r1; r <= r2; r++) {
                const left = ws.getCell(r, c1), right = ws.getCell(r, c2);
                left.border = { ...left.border, left: { style } };
                right.border = { ...right.border, right: { style } };
            }
        }

        // ===== 5) WORKBOOK / SHEET =====
        const wb = new ExcelJS.Workbook();
        const ws = wb.addWorksheet("Continual Improvement Tracker", {
            properties: { defaultRowHeight: 15 },
            views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }] // sticky header
        });

        // Set column widths (no header to avoid duplicates)
        ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

        // Give header band height so logo fits
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            ws.getRow(r).height = 18; // ~34px
        }

        ws.pageSetup = {
            orientation: "landscape",
            fitToPage: true,
            fitToWidth: 1,
            fitToHeight: 0,
            margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
            printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
        };

        // ===== 6) HEADER BAND FILL =====
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            for (let c = 1; c <= TOTAL_COLS; c++) {
                ws.getCell(r, c).fill = {
                    type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" }
                };
            }
        }

        // ===== 7) LOGO — centered in A2:B6 (no stretch) + outline =====
        // Desired logo size (px)
        const LOGO_WIDTH_PX = 100;  // adjust as you like
        const LOGO_HEIGHT_PX = 100;

        // Approx column/row pixel helpers
        const COL_PX = (c) => ((ws.getColumn(c).width || 8) * 7);       // ~7px per char width
        const ROW_PX = (r) => ((ws.getRow(r).height || 15) * (96 / 72));  // pt -> px

        // Size of A2:B6 rect in px
        let rectWpx = 0; for (let c = LOGO_COL_START; c <= LOGO_COL_END; c++) rectWpx += COL_PX(c);
        let rectHpx = 0; for (let r = LOGO_ROW_START; r <= LOGO_ROW_END; r++) rectHpx += ROW_PX(r);

        // Average px per column/row in that rectangle
        const avgColPx = rectWpx / (LOGO_COL_END - LOGO_COL_START + 1);
        const avgRowPx = rectHpx / (LOGO_ROW_END - LOGO_ROW_START + 1);

        // Convert desired pixel size → fractional col/row units
        const logoCols = LOGO_WIDTH_PX / avgColPx;
        const logoRows = LOGO_HEIGHT_PX / avgRowPx;

        // Centered TL anchor (fractional col/row)
        const tlCol = (LOGO_COL_START - 1) + ((LOGO_COL_END - LOGO_COL_START + 1) - logoCols) / 2;
        const tlRow = (LOGO_ROW_START - 1) + ((LOGO_ROW_END - LOGO_ROW_START + 1) - logoRows) / 2;

        const logoUrl = window.LOGO_URL || (window.APP_BASE && (window.APP_BASE + "images/wipro-logo.png"));
        if (logoUrl) {
            try {
                const base64 = await fetchAsBase64(logoUrl);
                const imgId = wb.addImage({ base64, extension: "png" });
                ws.addImage(imgId, {
                    tl: { col: tlCol, row: tlRow },
                    ext: { width: LOGO_WIDTH_PX, height: LOGO_HEIGHT_PX },
                    editAs: "oneCell"
                });
            } catch (e) { console.warn("Logo load failed:", e); }
        }
        outlineRange(ws, LOGO_ROW_START, LOGO_COL_START, LOGO_ROW_END, LOGO_COL_END, "thin");

        // ===== 8) TITLE (merge) + outline (from column C to third-last col) =====
        ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
        const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
        titleCell.value = TITLE_TEXT;
        titleCell.font = { bold: true, size: 18 };
        titleCell.alignment = { horizontal: "center", vertical: "middle" };

        outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

        // ===== 9) DOCUMENT DETAILS in (second-last, last) columns =====
        // Rows: HEADER_TOP..(HEADER_TOP + docDetails.length - 1)
        const detailsRowsEnd = HEADER_TOP + docDetails.length - 1;
        docDetails.forEach((pair, i) => {
            const r = HEADER_TOP + i;

            const labelCell = ws.getCell(r, DETAILS_LABEL_COL);
            const valueCell = ws.getCell(r, DETAILS_VALUE_COL);

            labelCell.value = pair[0];
            valueCell.value = pair[1];

            labelCell.font = { bold: true };
            [labelCell, valueCell].forEach(cell => {
                cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
                setBorder(cell, "thin");
            });
        });
        // Optional: outline the whole two-column details block
        outlineRange(ws, HEADER_TOP, DETAILS_LABEL_COL, detailsRowsEnd, DETAILS_VALUE_COL, "thin");

        // ===== 10) MANUAL TABLE HEADER (row 7) =====
        while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // up to row 6

        const headerTitles = excelCols.map(c => c.label);
        const headerRow = ws.addRow(headerTitles);
        headerRow.height = 22;
        headerRow.eachCell((cell) => {
            cell.font = { bold: true };
            cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
            cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
            setBorder(cell);
        });

        // ===== 11) DATA ROWS (exact Tabulator view, minus Action) =====
        let tabRows;
        switch (EXPORT_SCOPE) {
            case "selected": tabRows = table.getSelectedRows(); break;
            case "all": tabRows = table.getRows(); break;
            case "active":
            default: tabRows = table.getRows("active"); break;
        }

        tabRows.forEach(row => {
            const cells = row.getCells();
            const byField = {};
            cells.forEach(cell => {
                const f = cell.getField();
                if (!f) return;
                // Skip excluded
                const def = cell.getColumn().getDefinition();
                const title = (def.title || "").trim();
                if (EXCLUDE_FIELDS.has(f) || EXCLUDE_TITLES.has(title)) return;

                byField[f] = EXPORT_RAW ? row.getData()[f] : cell.getValue(); // exact display value by default
            });

            const values = excelCols.map(c => byField[c.key] ?? "");
            const xRow = ws.addRow(values);

            xRow.eachCell((cell, colNumber) => {
                cell.alignment = { vertical: "middle", horizontal: colNumber === 1 ? "center" : "left", wrapText: true };
                setBorder(cell);
            });
        });

        // ===== 12) DOWNLOAD =====
        const buffer = await wb.xlsx.writeBuffer();
        const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "Continual Improvement Tracker.xlsx";
        document.body.appendChild(link);
        link.click();
        link.remove();
    });;

    Blockloaderhide();
}

function renumberSrNo() {
    const rows = table.getRows("active");
    $.each(rows, function (i, r) {
        const d = r.getData();
        if (d.Sr_No !== i + 1) { r.update({ Sr_No: i + 1 }); }
    });
}

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
        //if (fieldName === "Nat_Project") {
        //    let addOption = document.createElement("option");
        //    addOption.value = "__add_new__";
        //    addOption.text = "➕ Add New Project Type";
        //    select.appendChild(addOption);
        //}

        //onRendered(function () {
        //    $(select).select2({
        //        dropdownParent: document.body,
        //        width: 'resolve',
        //        placeholder: "Select value",
        //        templateResult: function (data) {
        //            if (data.id === "__add_new__") {
        //                return $('<span style="color: blue;"><i class="fas fa-plus-circle"></i> ' + data.text + '</span>');
        //            }
        //            return data.text;
        //        },
        //        templateSelection: function (data) {
        //            return values[data.id] || data.text;
        //        }
        //    }).on("select2:select", function (e) {
        //        const selectedVal = select.value;

        //        if (selectedVal === "__add_new__") {
        //            $(select).select2('close');
        //            cancel(); // cancel cell edit
        //            selectedNatProjectCell = cell; // store the cell
        //            $('#natProjectModel').modal('show');
        //            loadNatProjectData();
        //        } else {
        //            success(selectedVal);
        //        }
        //    });
        //});

        return select;
    }
});


function delConfirm(recid, element) {
    if (!recid || recid <= 0) {
        // Unsaved row: just remove from UI
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = table.getRow(rowEl);
        if (row) row.delete();
        return;
    }

    PNotify.prototype.options.styling = "bootstrap3";

    const notice = new PNotify({
        title: false,
        text: 'Are you sure you want to delete?',
        icon: 'glyphicon glyphicon-question-sign',
        hide: false,
        width: '360px',
        confirm: {
            confirm: true,
            buttons: [
                {
                    text: 'Yes',
                    addClass: 'btn btn-sm btn-danger',
                    click: function (notice, value) {
                        // lock buttons while deleting
                        notice.get().find('.btn').prop('disabled', true);
                        $.ajax({
                            url: '/ContiImprove/Delete',
                            type: 'POST',
                            data: { id: recid }
                        }).done(function (data) {
                            if (data && data.success === true) {
                                showSuccessNewAlert("Continual Improvement Detail deleted successfully.");
                                // remove row immediately
                                const rowEl = $(element).closest(".tabulator-row")[0];
                                const row = table.getRow(rowEl);
                                if (row) row.delete();
                                // reload using SAME filter (globals preserved)
                                loadData();
                            } else if (data && data.success === false && data.message === "Not_Deleted") {
                                showDangerAlert("Record is used in QMS Log transactions.");
                            } else {
                                showDangerAlert((data && data.message) || "Delete failed.");
                            }
                        }).fail(function () {
                            showDangerAlert('Server error during delete.');
                        }).always(function () {
                            notice.remove(); // close the confirm
                        });
                    }
                },
                {
                    text: 'No',
                    addClass: 'btn btn-sm btn-default',
                    click: function (notice) {
                        notice.remove();      // just close
                        // no reload; your current date filter stays as-is
                    }
                }
            ]
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: { history: false },
        // Accessibility/keyboard
        animate_speed: 'fast',
        destroy: true
    });

    // Focus the "Yes" button by default
    notice.get().one('shown.bs.modal pnotify.afterOpen', function () {
        notice.get().find('.btn.btn-danger').focus();
    });

    // Also handle Enter/Esc
    notice.get().on('keydown', function (e) {
        if (e.key === 'Enter') {
            notice.get().find('.btn.btn-danger').click();
        } else if (e.key === 'Escape') {
            notice.remove();
        }
    });
}


function InsertUpdateContiImprove(rowData) {
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
    console.log(rowData);

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

    const cleanedData = {
        Id: rowData.Id || 0,
        Date: toIsoDate(rowData.Date || ""),
        Conti_Improve_Plane: rowData.Conti_Improve_Plane || "",
        Iso_9001: rowData.Iso_9001 || false,
        Iso9001_Plane_Implement: rowData.Iso9001_Plane_Implement || "",
        Iso_14001: rowData.Iso_14001 || false,
        Iso14001_Plane_Implement: rowData.Iso14001_Plane_Implement || "",
        Iso_45001: rowData.Iso_45001 || false,
        Iso_45001_Plane_Implement: rowData.Iso_45001_Plane_Implement || "",
        FY: rowData.FY || ""
    };

    console.log(cleanedData);
    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/ContiImprove/Create' : '/ContiImprove/Update';

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(cleanedData),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                if (isNew) {
                    showSuccessNewAlert("Saved successfully!.");
                    loadData();
                }
            }
            else if (response.message === "Exist") {
                showDangerAlert("Continual Improvement Tracker Detail already exists.");
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
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving record.");
        }
    });
}
