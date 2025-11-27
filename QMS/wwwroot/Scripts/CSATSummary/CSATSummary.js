var tabledata = [];
var table = '';
let filterStartDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndDate = moment().endOf('month').format('YYYY-MM-DD');
const searchTerms = {};
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
        startDate: moment().startOf('month').format('DD-MM-YYYY'),
        endDate: moment().endOf('month').format('DD-MM-YYYY')
    });

    // 🔑 Ensure calendar opens on click
    $('#customDateTrigger').on('click', function () {
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
        url: '/CSATSummary/GetAll',
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
            if (item.createdDate) {
                const dateObj = new Date(item.createdDate);
                formattedDate = dateObj.toLocaleDateString("en-GB");
            }
            if (item.updatedDate) {
                const updatedateObj = new Date(item.updatedDate);
                updatedDate = updatedateObj.toLocaleDateString("en-GB");
            }

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Ytd_ReqSent11: item.ytd_ReqSent11,
                Ytd_ResRece11: item.ytd_ResRece11,
                Ytd_Promoter11: item.ytd_Promoter11,
                Ytd_Collection11: item.ytd_Collection11,
                Ytd_Detractor11: item.ytd_Detractor11,
                Ytd_Nps11: item.ytd_Nps11,
                CreatedDate: formattedDate,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: updatedDate
            });
        });

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

                    actionButtons += `<i data-toggle="modal" onclick="delConfirm(${rowData.Id})" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                    return actionButtons;
                }
            },
            {
                title: "SNo", field: "Sr_No", sorter: "number", width: 35, headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left"
            },
            { title: "Request Sent", field: "Ytd_ReqSent11", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Response Received", field: "Ytd_ResRece11", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Promoters ", field: "Ytd_Promoter11", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "% Collection", field: "Ytd_Collection11", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Detractors", field: "Ytd_Detractor11", sorter: "number", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "NPS", field: "Ytd_Nps11", sorter: "number", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Create Date", field: "CreatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
            { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        );

        // // Initialize Tabulator
        table = new Tabulator("#sum_Table", {
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
                showEditSummary(rowData.Id);
            }
        });

    }
    else {
        showDangerAlert('No data available.');
    }

    // Hide loader
    Blockloaderhide();
}

function showEditSummary(id) {
    debugger
    var url = '/CSATSummary/CSATSummaryDetail?id=' + id;
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
            url: '/CSATSummary/Delete',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("CSAT Summary Detail is Deleted successfully.");
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

function openSummaryDetail(id) {
    debugger
    var url = '/CSATSummary/CSATSummaryDetail';
    url = url + '?id=' + id
    window.location.href = url;
}
