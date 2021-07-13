$(document).ready(function () {
    $(".stats").width($(".stats .box").length * ($(".stats .box").width() + 50));
    var table = $('.fl-table').DataTable({
        paging: false,
        info: false,
        ordering: true,
        searching: true,
        fixedHeader: true,
        responsive: true,
        colReorder: true,
        select: true,
        select: {
            style: 'multi'

        },
        lengthChange: false,
        buttons: [
            {
                extend: 'searchBuilder',
                text: 'Gelişmiş Arama',
                className: 'btn btn-danger'
            },
            {
                extend: 'colvis',
                text: 'Sütun Gizle'
            },
            {
                extend: 'selectAll',
                text: 'Tümünü Seç',
                className: 'btn btn-danger'
            },
            {
                extend: 'selected',
                text: 'Count selected rows',
                action: function (e, dt, button, config) {
                    alert(dt.rows({ selected: true }) + ' row(s) selected');
                }
            }
        ]

    });

    table.buttons().container()
        .appendTo($('div.column.is-one-half', table.table().container()).eq(0));
    $('.dt-buttons.field.is-grouped').append(`<button id="TakipCik" class ="btn btn-danger"">Seçilenleri Takipten Çık</button>`);

    $("#TakipCik").on("click", function () {
        var kullaniciAdi = "";
        var secilenlerArray = $('.fl-table').DataTable().rows({ selected: true }).data().toArray();
        for (var i = 0; i < secilenlerArray.length; i++) {
            var satir = secilenlerArray[i];
            var sutunText = satir[1];
            var basla = sutunText.indexOf('@');
            var kullaniciadi = sutunText.substring(basla, sutunText.indexOf('<', basla));
            kullaniciAdi += kullaniciadi+';';
        }
        setTimeout(function () {
            
            $.ajax({
                type: "POST",
                url: "/Home/TakipCik",
                data: { Usernames: kullaniciAdi },
                dataType: "text",
                success: function (msg) {
                    // Replace the div's content with the page method's return.
                    $("#Result").text(msg.d);
                }
            });

           
        }, 2000);

    });

});