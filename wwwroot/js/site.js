$(document).ready(() => {

    const connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44308/progressHub").build();
    connection.start().catch(e => ("Bağlantı Başarısız!!!!! Hata Mesajı: " + e));
    $("#Yenile").on("click", function () {
        connection.invoke("ProgressBar", 11111111).catch(e => ("Gönderim Başarısız!!!!! Hata Mesajı: " + e));
    })
  
    connection.on("progressCalistir", value => { console.log(value); });
})
function modalGetir() {
    settingsGetir();
    $('#exampleModal').modal('toggle');
}
$(".gizli2").hide();
$(".images").on({
    mouseenter: function () {
        var element = $(this.getElementsByClassName("gizli"));
        element.attr('src', $(this.getElementsByClassName("photo")).attr('src').replace('x96', "normal"));
        element.show(500);
    },
    mouseleave: function () {
        var element = $(this.getElementsByClassName("gizli"));
        element.hide(500);
        element.attr('src', '#');
    }
});

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
                }
            });
        }, 5000);

    });
}
progresCalistir("#takipciler");
progresCalistir("#listGetir");


