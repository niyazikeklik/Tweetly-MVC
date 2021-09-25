var table;
async function tableAyarla() {
    table = await $('.fl-table').DataTable({
        "language": {
            "search": ""
        },
        paging: true,
        lengthMenu: [
            [10, 25, 50, 100, 200, 500, -1],
            ['10 Satır', '25 Satır', '50 Satır', '100 Satır', '200 Satır', '500 Satır', 'Hepsini Göster']
        ],
        info: false,
        ordering: true,
        searching: true,
        fixedHeader: true,
        responsive: true,
        colReorder: true,
        select: true,
        //columnDefs: [{
        //    orderable: false,
        //    className: 'select-checkbox',
        //    targets: 0
        //}],
        select: {
            style: 'multi',
            blurable: true
            //  selector: 'td:first-child'
        },
        lengthChange: false,
        dom: 'Bfrtip',
        buttons: [
            {

                extend: 'searchBuilder',
                text: 'Gelişmiş Arama',
                className: 'tablobutonlar',
                enterSearch: true,

            },
            {
                extend: 'pageLength',
                text: 'Satır Sayısı',
                className: 'tablobutonlar'
            },
            {
                extend: 'colvis',
                text: 'Sütun Gizle',
                collectionLayout: 'two-column',
                className: 'tablobutonlar'
            },
            {
                extend: 'selectAll',
                text: 'Tümünü seç',
                className: 'tablobutonlar'
            },
            /*,
            {
                extend: 'collection',
                text: 'Diğerleri',
                className: 'tablobutonlar',
                buttons: [
                    {
                        collectionTitle: 'Visibility control',
                        extend: 'colvis',
                        collectionLayout: 'two-column',
                        className: 'tablobutonlar'
                    }
                ]
            }*/

        ]

    });
    $('#DataTables_Table_0_filter label').toggleClass("row");

    table.buttons().container().appendTo($('#DataTables_Table_0_filter label', table.table().container()).eq(0));

    $('.dt-buttons.field.is-grouped').append(`<button id="TakipCik" class ="button is-light buttons-collection tablobutonlar"">Seçilenleri Takipten Çık</button>`);
    $('.dt-buttons.field.is-grouped').append(`<button id="Yenile" asp-area="" asp-controller="Home" asp-action="Yenile" class ="button is-light buttons-collection tablobutonlar"">Yenile</button>`);

    $('#DataTables_Table_0_filter label').css("margin", "20px 0px 0px 0px");
    $('#DataTables_Table_0_filter label input').css("margin", "5px 20px 0px 0px");
    $('.dt-buttons.field.is-grouped').toggleClass("row");
    $('#DataTables_Table_0_filter input').attr("placeholder", "Ara");

    $("#TakipCik").on("click", function () {

        $.ajax({
            type: "POST",
            url: "/Home/TakipCik",
            data: { Usernames: $("tr.selected #username").text() },
            dataType: "text",
            success: function (msg) {
                // $("tr.selected").css("background-color", "#dc3545");
                //$("tr.selected").css("color", "white");
                $('tr.selected button[name="Usernames"]').text("Takip et");
                $('tr.selected').remove();
            }
        });

    });

}

tableAyarla();