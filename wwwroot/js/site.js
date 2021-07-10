$(document).ready(function () {
    var table = $('.fl-table').DataTable({
        "paging": false,
        "ordering": true,
        "info": false,
        searching: false,
        fixedHeader: true,
        responsive: true,
        colReorder: true,
        select: true,
        select: {
            style: 'multi'
        }
    });

});