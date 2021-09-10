function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}


var table;
async function tableAyarla() {
    console.log(1);
    table = await $('.fl-table').DataTable({
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
        buttons: [
            {
                extend: 'pageLength',
                text: 'Satır Sayısı',
                className: 'tablobutonlar'
            },
            {
            
                extend: 'searchBuilder',
                text: 'Gelişmiş Arama',
                className: 'tablobutonlar',
                enterSearch: true,

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
    table.buttons().container().appendTo($('div.column.is-one-half', table.table().container()).eq(0));
    $('.dt-buttons.field.is-grouped').append(`<button id="TakipCik" class ="button is-light buttons-collection tablobutonlar"">Seçilenleri Takipten Çık</button>`);
    $('.dt-buttons.field.is-grouped').append(`<button id="Yenile" asp-area="" asp-controller="Home" asp-action="Yenile" class ="button is-light buttons-collection tablobutonlar"">Yenile</button>`);
    $('#DataTables_Table_0_filter input').attr("placeholder", "Ara");
    $('#DataTables_Table_0_filter').css("text-align", "left");
   // $('#DataTables_Table_0_filter').html($('#DataTables_Table_0_filter input'));


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

function progresCalistir(element) {
    $(element).on("click", function () {
        $("#progressbox").css("display", "block");
        setInterval(() => {
            $.ajax({
                url: '/Home/GuncelleProgress/',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    data = JSON.parse(data)
                    $("#progress").css("width", data.veri + "%");
                    $("#progress").text(data.metin + " " + data.veri + "%");
                    $("#gecensure").text("Geçen süre: " + data.sure + " ")
                }
            });
        }, 5000);

    });
}
progresCalistir("#takipciler");
progresCalistir("#listGetir");


