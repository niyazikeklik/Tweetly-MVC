function settingsCreate() {
    $("#modalicerik").html(`
<div class="container">
    <div class="my-4">
        <div style ="margin-top: 30px; margin-top: 15px;"><strong class="mb-5" >Performans ve Veri Kalitesi</strong></div>
              ${checkItemEkle("Veritabanını kullan", "Kullanıcılar kontrol edilirken daha önceden veritabanında kayıt varsa oradan çekilir. Bu işlem tutarsız verilere sebep olabilir. Hız açısından avantajlıdır.", "CheckUseDB")}
              ${checkItemEkle("Veritabanını temizle", "Veritabanında eski kayıtlar tamamen silinir, güncel veri için önerilir, hız açısından dezavantajlıdır.", "CheckClearDB")}
              ${checkItemEkle("Detay Getir", "Etkinleştirilirse detay bilgiler toplanır, hız önemli ölçüde azalır. Etkinleştirilmez ise temel bilgiler toplanır, hız önemli ölçüde artar.", "CheckDetayGetir")}
           

            ${textItemEkle("Beğenisi Kontrol Edilecek Tweet Sayısı", "Kronolojik sıraya göre girilen sayı kadar atılan tweetler'in beğenisi kontrol edilir.", "TextTweetControl", "Tweet Sayısı", 200)}
            ${textItemEkle("Bulunacak Kişi Sayısı", "Liste aramalarında kaç kişi bulunacağını ayarlar. Liste sonuna kadar arama devam etsin, tüm liste getirilsin istiyorsanız büyük bir sayı girin.", "TextBulunacakKisiSayisi", "Kişi Sayısı", 99999)}

       <div style ="margin-top: 50px; margin-bottom: 15px;"><strong class="mb-5" >Cinsiyet Tercihi</strong></div>
               ${checkItemEkle("Erkek Kullanıcıları Getir", "Erkek kullanıcılar aramaya dahil edilir.", "CheckErkek")}
               ${checkItemEkle("Kadın Kullanıcıları Getir", "Kadın kullanıcılar aramaya dahil edilir.", "CheckKadin")}
               ${checkItemEkle("Unisex Kullanıcıları Getir", "Unisex kullanıcılar aramaya dahil edilir.", "CheckUnisex")}
               ${checkItemEkle("Belirsiz Kullanıcıları Getir", "Belirsiz kullanıcılar aramaya dahil edilir.", "CheckBelirsiz")}
      <div style ="margin-top: 50px; margin-bottom: 15px;"><strong class="mb-5" >Takip Durumu</strong></div>
               ${checkItemEkle("Takip etmediğim kayıtları getirme", "", "CheckTakipEtmediklerim")}
               ${checkItemEkle("Takip ettiğim kayıtları getirme", "", "CheckTakipEttiklerim")}
               ${checkItemEkle("Beni takip eden kayıtları getirme", "", "CheckBeniTakipEdenler")}
               ${checkItemEkle("Beni takip etmeyen kayıtları getirme", "", "CheckBeniTakipEtmeyenler")}
               ${checkItemEkle("Gizli hesapları getirme", "", "CheckGizliHesap")}
    </div>
</div>
`);
    VarsayılanAyarlar();
    Kurallar();
    saveSettings(false);
    $("#AyarlarıKaydet").on("click", function () {
        saveSettings(true);
    });
}
function saveSettings(msgShow) {
    $.ajax({
        type: "POST",
        url: '/Home/SettingSave',
        data: { Model: JSON.stringify(getValues()) },
        dataType: "text",
        success: function (msg) {
            if (msgShow) alert("Başarıyla kaydedildi: " + msg)
        }
    });
}
function checkItemEkle(baslik, aciklama, id) {
    return `<div class="list-group mb-2 shadow" style="margin: 10px">
                <div class="list-group-item">
                    <div class="row align-items-center">
                        <div class="col">
                            <strong class="mb-0">${baslik}</strong>
                            <p class="text-muted mb-0">${aciklama}</p>
                        </div>
                        <div class="col-auto">
<div class="checkboxDiv form-check form-switch">
  <input class="form-check-input" type="checkbox" style="width: 3.5em; height: 1.75em;" id="${id}">
</div>
                        </div>
                    </div>
                </div>
            </div>`
}

function textItemEkle(baslik, aciklama, id, placeholder, defaultValue) {
    return `<div class="list-group mb-2 shadow" style="margin: 10px">
                <div class="list-group-item">
                    <div class="row align-items-center">
                        <div class="col">
                            <strong class="mb-0">${baslik}</strong>
                            <p class="text-muted mb-0">${aciklama}</p>
                        </div>
                        <div class="col-auto">
                            <input id=${id} class="form-control" value="${defaultValue}" type="text" placeholder="${placeholder}" aria-label="${placeholder}">
                        </div>
                    </div>
                </div>
            </div>`
}
function VarsayılanAyarlar() {
    $("#CheckUseDB").trigger("click");
    $("#CheckKadin").trigger("click");
    $("#CheckUnisex").trigger("click");
    $("#CheckBelirsiz").trigger("click");
    $("#CheckErkek").trigger("click");
}
function Kurallar() {
    $("#CheckErkek").change(function () {
        if (!this.checked && !$("#CheckKadin").is(":checked") && !$("#CheckUnisex").is(":checked") && !$("#CheckBelirsiz").is(":checked")) {
            $('#CheckKadin').trigger('click');
        }
    });
    $("#CheckKadin").change(function () {
        if (!this.checked && !$("#CheckErkek").is(":checked") && !$("#CheckUnisex").is(":checked") && !$("#CheckBelirsiz").is(":checked")) {
            $('#CheckErkek').trigger('click');
        }
    });
    $("#CheckUnisex").change(function () {
        if (!this.checked && !$("#CheckErkek").is(":checked") && !$("#CheckKadin").is(":checked") && !$("#CheckBelirsiz").is(":checked")) {
            $('#CheckErkek').trigger('click');
        }
    });
    $("#CheckBelirsiz").change(function () {
        if (!this.checked && !$("#CheckErkek").is(":checked") && !$("#CheckKadin").is(":checked") && !$("#CheckUnisex").is(":checked")) {
            $('#CheckErkek').trigger('click');
        }
    });
    $("#CheckTakipEtmediklerim").change(function () {
        if (this.checked && $("#CheckTakipEttiklerim").is(":checked")) {
            $('#CheckTakipEttiklerim').trigger('click');
        }
    });
    $("#CheckTakipEttiklerim").change(function () {
        if (this.checked && $("#CheckTakipEtmediklerim").is(":checked")) {
            $('#CheckTakipEtmediklerim').trigger('click');
        }
    });
    $("#CheckBeniTakipEdenler").change(function () {
        if (this.checked && $("#CheckBeniTakipEtmeyenler").is(":checked")) {
            $('#CheckBeniTakipEtmeyenler').trigger('click');
        }
    });
    $("#CheckBeniTakipEtmeyenler").change(function () {
        if (this.checked && $("#CheckBeniTakipEdenler").is(":checked")) {
            $('#CheckBeniTakipEdenler').trigger('click');
        }
    });
    $("#CheckClearDB").change(function () {
        if (this.checked && $("#CheckUseDB").is(":checked")) {
            $('#CheckUseDB').trigger('click');
        }
    });
    $("#CheckUseDB").change(function () {
        if (this.checked && $("#CheckClearDB").is(":checked")) {
            $('#CheckClearDB').trigger('click');
        }
    });
    $("#CheckDetayGetir").change(function () {
        if (this.checked) {
            $("#CheckUseDB").removeAttr("disabled");
        } else {
            $("#CheckUseDB").attr("disabled", true);
        }
    });
    $(".checkboxDiv input").change();
}
function getValues() {
    var obj = new Object();
    obj.checkGizliHesap = $("#CheckGizliHesap").is(":checked");;
    obj.checkBeniTakipEtmeyenler = $("#CheckBeniTakipEtmeyenler").is(":checked");;
    obj.checkBeniTakipEdenler = $("#CheckBeniTakipEdenler").is(":checked");
    obj.checkTakipEtmediklerim = $("#CheckTakipEtmediklerim").is(":checked");;
    obj.checkTakipEttiklerim = $("#CheckTakipEttiklerim").is(":checked");;
    obj.checkBelirsiz = $("#CheckBelirsiz").is(":checked");;
    obj.checkUnisex = $("#CheckUnisex").is(":checked");;
    obj.checkKadin = $("#CheckKadin").is(":checked");;
    obj.checkErkek = $("#CheckErkek").is(":checked");;
    obj.checkDetayGetir = $("#CheckDetayGetir").is(":checked");;
    obj.checkClearDB = $("#CheckClearDB").is(":checked");;
    obj.checkUseDB = $("#CheckUseDB").is(":checked");
    obj.CheckUseAllDriver = $("#CheckUseAllDriver").is(":checked");
    obj.TextTweetControl = $("#TextTweetControl").val();
    obj.TextBulunacakKisiSayisi = $("#TextBulunacakKisiSayisi").val();
    return obj;
}



/*checkItemEkle("Tüm driverları kullan", "Kazıma yapılırken tam güç kullanılır, bu sizin daha kısa sürede daha çok profile ulaşmanızı sağlarken Twitter tarafından limit yeme şansınızı arttıracağı için performansı düşürebilir, az sayıda kullanıcı kontrolü için önerilir. ", "CheckUseAllDriver")*/
