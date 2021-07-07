$(function () {
    $('.js-basic-example').DataTable({
        responsive: true,
    });

    //Exportable table
    $('.js-exportable').DataTable({
        dom: 'Bfrtip',
        responsive: true,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
    });
    $('.js-exportablenosort').DataTable({
        dom: 'Bfrtip',
        responsive: true,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "order": [],
        columnDefs: [
            { orderable: true, className: 'reorder', targets: 0 },
            { orderable: true, className: 'reorder', targets: 4 },
            { orderable: true, className: 'reorder', targets: 5 },
            { orderable: false, targets: '_all' }
        ]
    });
});