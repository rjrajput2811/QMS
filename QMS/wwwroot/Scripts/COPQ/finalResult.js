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
            Id: item.id,
            Indent_No: item.indent_No,
            Indent_Date: item.indent_Date ? new Date(item.indent_Date).toLocaleDateString("en-GB") : "",
            Business_Unit: item.business_Unit,
            Vertical: item.vertical,
            Branch: item.branch,
            Indent_Status: item.indent_Status,
            End_Cust_Name: item.end_Cust_Name,
            Complaint_Id: item.complaint_Id,
            Customer_Code: item.customer_Code,
            Customer_Name: item.customer_Name,
            Bill_Req_Date: item.bill_Req_Date ? new Date(item.bill_Req_Date).toLocaleDateString("en-GB") : "",
            Created_By: item.created_By,
            Wipro_Commit_Date: item.wipro_Commit_Date ? new Date(item.wipro_Commit_Date).toLocaleDateString("en-GB") : "",
            Material_No: item.material_No,
            Item_Description: item.item_Description,
            Quantity: item.quantity,
            Price: item.price,
            Final_Price: item.final_Price,
            SapSoNo: item.sapSoNo,
            CreateSoQty: item.createSoQty,
            Inv_Qty: item.inv_Qty,
            Inv_Value: item.inv_Value,
            WiproCatelog_No: item.wiproCatelog_No,
            Batch_Code: item.batch_Code,
            Batch_Date: item.batch_Date ? new Date(item.batch_Date).toLocaleDateString("en-GB") : "",
            Main_Prodcode: item.main_Prodcode,
            User_Name: item.user_Name,
            CreatedDate: item.createdDate ? new Date(item.createdDate).toLocaleDateString("en-GB") : "",
            CreatedBy: item.createdBy,
            UpdatedDate: item.updatedDate ? new Date(item.updatedDate).toLocaleDateString("en-GB") : "",
            UpdatedBy: item.updatedBy,
            IsDeleted: item.isDeleted
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
        {
            title: "Action",
            field: "Action",
            hozAlign: "center",
            headerHozAlign: "center",
            frozen: true, headerMenu: headerMenu,
            formatter: function (cell) {
                const rowData = cell.getRow().getData();
                return `<i onclick="delConfirmIndent(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`;
            }
        },
        { title: "SNo", field: "Sr_No", frozen: true, sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "center" },
        { title: "CCN No", field: "CCN_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "CCCN Date", field: "CCCN_Date", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "ReportedBy", field: "ReportedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Location", field: "Location", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Cust Name", field: "Cust_name", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Dealer Name", field: "Dealer_name", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Complaint Status", field: "Complaint_Status", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Completion Cate", field: "Completion_Cate", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Closure Remarks", field: "ClosureRemarks", headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
        { title: "Closure Lead time", field: "Closure_Lead_time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Final Status", field: "Final_Status", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Custom", field: "Custom", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Open Lead Time", field: "Open_Lead_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Final Lead Time", field: "Final_Lead_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Range", field: "Range", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.Indent No", field: "Indent_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.Indent Date", field: "Indent_Date", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.Customer PO Number", field: "Cust_PO_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.ItemNo", field: "ItemNo", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.Description", field: "Description", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.Qty", field: "Qty", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.PO Number", field: "PO_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.PO Date", field: "PO_Date", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "UEOB.Indent Key", field: "Indent_Key", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Invoice Summary.Sum of Quantity", field: "Invoice_Quantity", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "PC Chart.PC", field: "PC_Chart", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Pending PO.Vendor", field: "PO_Vendor", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Pending PO.PO No", field: "PO_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Pending PO.PO Date", field: "PO_Date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Pending PO. Balance Qty", field: "PO_Balance_Qty", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Pending PO. Balance Value", field: "PO_Balance_Value", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Closure Range", field: "Closure_Range", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Pending With", field: "Pending With", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Region", field: "Region", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Current Status", field: "Current_Status", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Closure Type", field: "Closure_Type", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "CCN to Indent Lead Time", field: "Indent_Lead_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Last Invoice Date", field: "Last_Inv_Date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Indent to Invoice Lead time", field: "Indent_Invoice_Time", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },

        { title: "Created By", field: "CreatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", visible: false, headerMenu: headerMenu, hozAlign: "center" },
        { title: "Updated Date", field: "UpdatedDate", visible: false, headerMenu: headerMenu, hozAlign: "center" }
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



