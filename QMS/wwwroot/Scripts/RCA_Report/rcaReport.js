var tabledata = [];
var table = '';
let filterStartDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndDate = moment().endOf('month').format('YYYY-MM-DD');
$(document).ready(function () {


    $('#dateRangeCustomerRCA').text(
        moment(filterStartDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndDate).format('MMMM D, YYYY')
    );

    // Initialize Litepicker and store reference
    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerCustomerRCA'),
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
                $('#dateRangeCustomerRCA').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartDate = "";
                filterEndDate = "";
                $('#dateRangeCustomerRCA').text("Select Date Range");
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

    // 🔑 Ensure calendar opens on click
    $('#customDateTriggerCustomerRCA').on('click', function () {
        picker.show();
    });

    document.addEventListener('DOMContentLoaded', function () {
        document.getElementById('backButton').addEventListener('click', function () {
            window.history.back();
        });
    });

    loadData();
});

function loadData() {
    Blockloadershow();
    $.ajax({
        url: '/CustomerRCAReport/GetRCAReport',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartDate,
            endDate: filterEndDate
        },
        success: function (data) {
            Blockloaderhide();
            if (data && Array.isArray(data)) {
                OnTabGridLoad(data);
            }
            else {
                showDangerAlert('No data available to load.');
            }
        },
        error: function (xhr, status, error) {
            showDangerAlert('Error retrieving data: ' + error);
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

function OnTabGridLoad(response) {
    debugger;
    Blockloadershow();

    tabledata = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            let formattedDate = "";
            let updatedDate = "";
            let reportDate = "";
            if (item.createdDate) {
                const dateObj = new Date(item.createdDate);
                formattedDate = dateObj.toLocaleDateString("en-GB");
            }
            if (item.updatedDate) {
                const updatedateObj = new Date(item.updatedDate);
                updatedDate = updatedateObj.toLocaleDateString("en-GB");
            }
            if (item.report_Date) {
                const reportDateObj = new Date(item.report_Date);
                reportDate = reportDateObj.toLocaleDateString("en-GB");
            }

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Complaint_No: item.complaint_No,
                Report_Date: reportDate,
                Cust_Name_Location: item.cust_Name_Location,
                Batch_Code: item.batch_Code,
                Supp_Qty: item.supp_Qty,
                Failure_Qty: item.failure_Qty,
                CreatedDate: formattedDate,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: updatedDate
            });
        });
    }

    columns.push(
        {
            title: "Action",
            field: "Action",
            headerMenu: headerMenu,
            hozAlign: "center",
            headerHozAlign: "center",
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                let actionButtons = "";

                actionButtons += `<i data-toggle="modal" onclick="delConfirm(${rowData.Id})" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>
                    <i onclick="exportToExcel(${rowData.Id})" 
               class="fas fa-file-excel fa-1x" 
               title="Export To Excel" 
               style="color:green;cursor:pointer;">
            </i>`

                return actionButtons;
            }
        },
        {
            title: "SNo", field: "Sr_No", sorter: "number", width: 35, headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left"
        },
        { title: "Complaint No", field: "Complaint_No", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Report Date", field: "Report_Date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Customer Name and Location ", field: "Cust_Name_Location", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Batch Code", field: "Batch_Code", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Supp Qty", field: "Supp_Qty", sorter: "number", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Failure Qty", field: "Failure_Qty", sorter: "number", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Create Date", field: "CreatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
    );

    // // Initialize Tabulator
    table = new Tabulator("#rca_Table", {
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

    table.on("cellClick", function (e, cell) {
        let columnField = cell.getColumn().getField();

        if (columnField !== "Action") {
            let rowData = cell.getRow().getData();
            showEditRCAReport(rowData.Id);
        }
    });



    // Hide loader
    Blockloaderhide();
}

function showEditRCAReport(id) {
    debugger
    var url = '/CustomerRCAReport/CustomerRCADetails?id=' + id;
    window.location.href = url;
}

function delConfirm(recid) {
    debugger;
    PNotify.prototype.options.styling = "bootstrap3";
    (new PNotify({
        title: 'Confirmation Needed',
        text: 'Are you sure to delete? It will not delete if this record is used in transactions.',
        icon: 'glyphicon glyphicon-question-sign',
        hide: false,
        confirm: {
            confirm: true
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        },
    })).get().on('pnotify.confirm', function () {
        $.ajax({
            url: '/CustomerRCAReport/Delete',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("Customer RCA Report Detail is Deleted successfully.");
                    setTimeout(function () {
                        window.location.reload();
                    }, 2500);
                }
                else if (data.success == false && data.message == "Not_Deleted") {
                    showDangerAlert("Record is used in PPS Log transactions.");
                }
                else {
                    showDangerAlert(data.message);
                }
            },
            error: function () {
                showDangerAlert('Error retrieving data.');
            }
        });
    }).on('pnotify.cancel', function () {
        loadData();
    });
}

function openRCADetail(id) {
    debugger
    var url = '/CustomerRCAReport/CustomerRCADetails';
    url = url + '?id=' + id
    window.location.href = url;
}

function exportToExcel(id) {

    const url = `/CustomerRCAReport/ExportCAReportToExcel?id=${id}`;

    // Open file download in new tab (best for Excel)
    window.open(url, "_blank");
}
