$(document).ready(function () {
    $("#datatbl").DataTable({
        dom: 'Bfrti',
        buttons: [{
            extend: 'excel',
            filename: 'TrackingInfo',
            text: '<strong>EXPORT TO EXCEL</strong>',
            header: 'false'
        }
        ],
        "paging": false,
    });
});