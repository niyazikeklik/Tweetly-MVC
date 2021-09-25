function settingsCreate() {
    $("#modalicerik").html(`
<div class="container">
    <div class="my-4">
        <ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" id="contact-tab" data-toggle="tab" href="#contact" role="tab" aria-controls="contact" aria-selected="false">Notifications</a>
            </li>
        </ul>

        <strong class="mb-5">Performans ve Veri Kalitesi</strong>
              ${checkItemEkle("Veritabanını kullan", "Kullanıcılar kontrol edilirken daha önceden veritabanında kayıt varsa oradan çekilir. Bu işlem tutarsız verilere sebep olabilir. Hız açısından avantajlıdır.", "checkUseDB")}
              ${checkItemEkle("Veritabanını temizle", "Veritabanında eski kayıtlar tamamen silinir, güncel veri için önerilir, hız açısından dezavantajlıdır.", "checkClearDB")}
              ${checkItemEkle("Detay Getir", "Etkinleştirilirse detay bilgiler toplanır, hız önemli ölçüde azalır. Etkinleştirilmez ise temel bilgiler toplanır, hız önemli ölçüde artar.", "checkDetayGetir")}
        <strong class="mb-5">Cinsiyet Tercihi</strong>
               ${checkItemEkle("Erkek Kullanıcıları Getir", "Erkek kullanıcılar aramaya dahil edilir.", "checkErkek")}
               ${checkItemEkle("Kadın Kullanıcıları Getir", "Kadın kullanıcılar aramaya dahil edilir.", "checkKadin")}
               ${checkItemEkle("Unisex Kullanıcıları Getir", "Unisex kullanıcılar aramaya dahil edilir.", "checkUnisex")}
               ${checkItemEkle("Belirsiz Kullanıcıları Getir", "Belirsiz kullanıcılar aramaya dahil edilir.", "checkBelirsiz")}
        <strong class="mb-5">Takip Durumu</strong>
               ${checkItemEkle("Takip etmediğim kayıtları getirme", "", "checkTakipEtmediklerim")}
               ${checkItemEkle("Takip ettiğim kayıtları getirme", "", "checkTakipEttiklerim")}
               ${checkItemEkle("Beni takip eden kayıtları getirme", "","checkBeniTakipEdenler")}
               ${checkItemEkle("Beni takip etmeyen kayıtları getirme", "", "checkBeniTakipEtmeyenler")}
               ${checkItemEkle("Gizli hesapları getirme", "", "checkGizliHesap")}
    </div>
</div>


`);
    $("#checkKadin").trigger("click");
    $("#checkUnisex").trigger("click");
    $("#checkBelirsiz").trigger("click");
    $("#checkErkek").trigger("click");
    $("#checkTakipEttiklerim").trigger("click");
    $("#checkBeniTakipEtmeyenler").trigger("click");
    $("#checkUseDB").trigger("click");

    $("#checkErkek").change(function () {
        if (!this.checked && !$("#checkKadin").is(":checked") && !$("#checkUnisex").is(":checked") && !$("#checkBelirsiz").is(":checked") ) {
            $('#checkKadin').trigger('click');
        }
    });
    $("#checkKadin").change(function () {
        if (!this.checked && !$("#checkErkek").is(":checked") &&!$("#checkUnisex").is(":checked") && !$("#checkBelirsiz").is(":checked")) {
            $('#checkErkek').trigger('click');
        }
    });
    $("#checkUnisex").change(function () {
        if (!this.checked && !$("#checkErkek").is(":checked") && !$("#checkKadin").is(":checked") && !$("#checkBelirsiz").is(":checked")) {
            $('#checkErkek').trigger('click');
        }
    });
    $("#checkBelirsiz").change(function () {
        if (!this.checked && !$("#checkErkek").is(":checked") && !$("#checkKadin").is(":checked") && !$("#checkUnisex").is(":checked")) {
            $('#checkErkek').trigger('click');
        }
    });
    $("#checkTakipEtmediklerim").change(function () {
        if (this.checked && $("#checkTakipEttiklerim").is(":checked")) {
            $('#checkTakipEttiklerim').trigger('click');
        }
    });
    $("#checkTakipEttiklerim").change(function () {
        if (this.checked && $("#checkTakipEtmediklerim").is(":checked")) {
            $('#checkTakipEtmediklerim').trigger('click');
        }
    });
    $("#checkBeniTakipEdenler").change(function () {
        if (this.checked && $("#checkBeniTakipEtmeyenler").is(":checked")) {
            $('#checkBeniTakipEtmeyenler').trigger('click');
        }
    });
    $("#checkBeniTakipEtmeyenler").change(function () {
        if (this.checked && $("#checkBeniTakipEdenler").is(":checked")) {
            $('#checkBeniTakipEdenler').trigger('click');
        }
    });
    $("#checkClearDB").change(function () {
        if (this.checked && $("#checkUseDB").is(":checked")) {
            $('#checkUseDB').trigger('click');
        }
    });
    $("#checkUseDB").change(function () {
        if (this.checked && $("#checkClearDB").is(":checked")) {
            $('#checkClearDB').trigger('click');
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
<div class="form-check form-switch">
  <input class="form-check-input" type="checkbox" style="width: 3.5em; height: 1.75em;" id="${id}">
</div>
                        </div>
                    </div>
                </div>
            </div>`
}
