var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblBookings').DataTable({
        "ajax": {
            url: '/booking/getall',
            dataSrc: function (json) {
                console.log(json);
                return json.data || json; 
            },
            error: function (xhr, error, code) {
                console.log("Error loading data: " + error);
            }
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phone', "width": "10%" },
            { data: 'email', "width": "15%" },
            { data: 'status', "width": "10%" },
            { data: 'checkInDate', "width": "10%" },
            { data: 'nights', "width": "10%" },
            { data: 'totalCost', render: $.fn.dataTable.render.number(',', '.', 2), "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a href="/booking/bookingDetails?bookingId=${data}" class="btn btn-outline-warning mx-2">
                            <i class="bi bi-pencil-square"></i> Details
                        </a>
                    </div>`
                },
                "width": "15%"
            }
        ],
        "responsive": true,
        "paging": true,
        "searching": true,
        "ordering": true
    });
}
