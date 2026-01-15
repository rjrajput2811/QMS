let filterStartDateFinalResult = moment().startOf('month').format('YYYY-MM-DD');
let filterEndDateFinalResult = moment().endOf('month').format('YYYY-MM-DD');
let tableFinalResult = null;

$(document).ready(function () {

    $('#FinaldateRangeText').text(
        moment(filterStartDateFinalResult).format('MMMM D, YYYY') + ' - ' + moment(filterEndDateFinalResult).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerFinal'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartDateFinalResult = start.format('YYYY-MM-DD');
                filterEndDateFinalResult = end.format('YYYY-MM-DD');
                $('#FinaldateRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadFinalResultData();
            });
            picker.on('clear', () => {
                filterStartDateFinalResult = "";
                filterEndDateFinalResult = "";
                $('#FinaldateRangeText').text("Select Date Range");
                loadFinalResultData();
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
        picker.show();
    });

    document.getElementById('backButton').addEventListener('click', function () {
        window.history.back();
    });

    //$('#f-tab2').on('shown.bs.tab', function () {
    //    loadFinalResultData();
    //});
});

function loadFinalResultData() {
    Blockloadershow();

    $.ajax({
        url: '/Service/GetFinalMergeData',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartDateFinalResult,
            endDate: filterEndDateFinalResult
        },
        success: function (data) {
            if (data.success && Array.isArray(data.data)) {
                renderFinalResultTable(data.data);
            } else {
                showDangerAlert('No data available to load.');
                renderFinalResultTable([]); // avoid errors
            }
        },
        error: function (xhr, status, error) {
            showDangerAlert('Error retrieving data: ' + error);
            Blockloaderhide();
        }
    });
}

function renderFinalResultTable(response) {
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

            CCN_No: item.ccN_No,
            CCCNDate: item.cccnDate ? new Date(item.cccnDate).toLocaleDateString("en-GB") : "",
            ReportedBy: item.reportedBy,
            Location: item.cLocation,
            CustName: item.custName,
            DealerName: item.dealerName,
            Description: item.cDescription,
            Status: item.cStatus,
            Completion: item.completion ? new Date(item.completion).toLocaleDateString("en-GB") : "",
            Remarks: item.remarks,
            TotalDays_Close: item.totalDays_Close,

            Final_Status: item.final_Status,
            Custome: item.custome ? new Date(item.custome).toLocaleDateString("en-GB") : "",
            Open_Lead_Time: item.open_Lead_Time,
            Final_Lead_Time: item.final_Lead_Time,
            Range: item.range,

            Indent_No: item.indent_No,
            Indent_Date: item.indent_Date ? new Date(item.indent_Date).toLocaleDateString("en-GB") : "",
            Ind_CCN_No: item.ind_CCN_No,
            Material_No: item.material_No,
            Item_Description: item.item_Description,
            WiproCatelog_No: item.wiproCatelog_No,
            Quantity: item.quantity,
            Key: item.key,

            Inv_Qty: item.inv_Qty,
            Bal_Qty: item.bal_Qty,
            Pc: item.pc,
            Fy: item.fy,

            Vendor: item.vendor,
            PONo: item.poNo,
            PODate: item.poDate ? new Date(item.poDate).toLocaleDateString("en-GB") : "",
            BalanceQty: item.balanceQty,
            BalanceValue: item.balanceValue,

            Closure_Range: item.closure_Range,
            Region: item.region,
            Closure_Type: item.closure_Type,
            Indent_Lead_Time: item.indent_Lead_Time,
            Inv_Lead_Time: item.inv_Lead_Time,
            Last_Inv_Date: item.last_Inv_Date ? new Date(item.last_Inv_Date).toLocaleDateString("en-GB") : "",
        });
    });

    console.log(tabledata);
    //if (tabledata.length === 0 && tableFinalResult) {
    //    tableFinalResult.clearData();
    //    Blockloaderhide();
    //    return;
    //}

    // Define your columns here or outside renderTable if reused
    const columns = [
        { title: "SNo", field: "Sr_No", frozen: true, sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "center" },
        { title: "CCN No", field: "CCN_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "CCCN Date", field: "CCCNDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "ReportedBy", field: "ReportedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Location", field: "Location", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Cust Name", field: "CustName", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Dealer Name", field: "DealerName", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Description", field: "Description", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Complaint Status", field: "Status", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Completion", field: "Completion", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Closure Remarks", field: "Remarks", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Time Taken for Closure (DAYS)", field: "TotalDays_Close", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },

        { title: "Final Status", field: "Final_Status", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Custome", field: "Custome", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Open Lead Time", field: "Open_Lead_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Final Lead Time", field: "Final_Lead_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Range", field: "Range", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },

        { title: "Indent No", field: "Indent_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Indent Date", field: "Indent_Date", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Indent CCN No.", field: "Ind_CCN_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Material No", field: "Material_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Item Description", field: "Item_Description", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
        { title: "Wipro CateLog No", field: "WiproCatelog_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "left" },
        { title: "Quantity", field: "Quantity", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Key", field: "Key", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },

        { title: "Invoice Quantity", field: "Inv_Qty", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Balance Quantity", field: "Bal_Qty", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "PC", field: "Pc", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "FY", field: "Fy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Vendor", field: "Vendor", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "PO No", field: "PONo", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "PO Date", field: "PODate", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Balance Qty", field: "BalanceQty", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Balance Value", field: "BalanceValue", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },

        { title: "Closure Range", field: "Closure_Range", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Region", field: "Region", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Closure Type", field: "Closure_Type", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "CCN to Indent Lead Time", field: "Indent_Lead_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Last Invoice Date", field: "Last_Inv_Date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Indent to Invoice Lead time", field: "Inv_Lead_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },

       
    ];

    if (tableFinalResult) {
        tableFinalResult.replaceData(tabledata);
    } else {
        tableFinalResult = new Tabulator("#final-table", {
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
    }

    // Export to Excel on button click
    document.getElementById("exlExtButton").addEventListener("click", function () {
        // Get only visible data from Tabulator (respects filters, sorting, pagination)
        var visibleData = tableFinalResult.getData("active"); // "active" gets only visible/filtered rows

        // Get visible columns only
        var visibleColumns = tableFinalResult.getColumns().filter(col => col.isVisible() && col.getField() !== "Action");

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
        if (filterStartDateFinalResult && filterEndDateFinalResult) {
            dateRangeText = `Date Range: ${moment(filterStartDateFinalResult).format('DD-MMM-YYYY')} to ${moment(filterEndDateFinalResult).format('DD-MMM-YYYY')}`;
        } else if (filterStartDateFinalResult) {
            dateRangeText = `Date From: ${moment(filterStartDateFinalResult).format('DD-MMM-YYYY')}`;
        } else if (filterEndDateFinalResult) {
            dateRangeText = `Date To: ${moment(filterEndDateFinalResult).format('DD-MMM-YYYY')}`;
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
        XLSX.utils.book_append_sheet(wb, ws, "FinalResult");

        var fileName = `FinalResult_${moment().format('YYYYMMDD_HHmmss')}.xlsx`;
        XLSX.writeFile(wb, fileName);
    });

    Blockloaderhide();
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



