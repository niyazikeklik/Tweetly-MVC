$(".gizli2").hide();
$(".images").on({
    mouseenter: function () {
        var element = $(this.getElementsByClassName("gizli"));
        element.attr('src', $(this.getElementsByClassName("photo")).attr('src').replace('x96', "400x400"));
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
                    $("#gecensure").text("Geçen süre: " + data.sure + " ")
                }
            });
        }, 5000);

    });
}
progresCalistir("#takipciler");
progresCalistir("#listGetir");


