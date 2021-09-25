function settingsGetir() {
    $("#modalicerik").html(`
<div class="container">
    <div class="my-4">
            <ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="contact-tab" data-toggle="tab" href="#contact" role="tab" aria-controls="contact" aria-selected="false">Notifications</a>
                </li>
            </ul>
            <strong class="mb-5">Performans ve Veri Kalitesi</strong>
            ${checkItemEkle("Veritabanını kullan", "Kullanıcılar kontrol edilirken daha önceden veritabanında kayıt varsa oradan çekilir. Bu işlem tutarsız verilere sebep olabilir. Hız açısından avantajlıdır.")}
            ${checkItemEkle("Veritabanını temizle", "Veritabanında eski kayıtlar tamamen silinir, güncel veri için önerilir, hız açısından dezavantajlıdır.")}
      ${checkItemEkle("Detay Getir", "Etkinleştirilirse detay bilgiler toplanır, hız önemli ölçüde azalır. Etkinleştirilmez ise temel bilgiler toplanır, hız önemli ölçüde artar.")}
  <strong class="mb-5">Cinsiyet Tercihi</strong>
   ${checkItemEkle("Erkek Kullanıcıları Getir", "Erkek kullanıcılar aramaya dahil edilir.")}
   ${checkItemEkle("Kadın Kullanıcıları Getir", "Kadın kullanıcılar aramaya dahil edilir.")}
   ${checkItemEkle("Unisex Kullanıcıları Getir", "Unisex kullanıcılar aramaya dahil edilir.")}
   ${checkItemEkle("Belirsiz Kullanıcıları Getir", "Belirsiz kullanıcılar aramaya dahil edilir.")}
  <strong class="mb-5">Takip Durumu</strong>
   ${checkItemEkle("Geri takip etmeyenleri getir", "")}
   ${checkItemEkle("Sadece takip etmediğim kayıtları getir", "")}
   ${checkItemEkle("Beni takip eden kayıtları getirme", "")}
   ${checkItemEkle("Gizli hesapları getirme", "")}

<strong class="mb-5">Detay Filtre</strong>
<p><i class="bi bi-exclamation-lg"></i> Bu filtreler kişi bulma süresini çok fazla uzatacağı için performansı düşürür. Limit'e takılma şansınız çok yüksektir.</p>
   ${inputItemEkle("Konumlara göre filtrele", "Sadece girilen konumlarda yaşayan kullanıcılar bulunur, aralarına  ';' koyarak birden fazla konum eklenebilir (örn: Türkiye;istanbul;pendik;kartal;izmir;ankara;34;35;06) ", "Konumlar")}
   ${inputItemEkle("Kayıt tarihine göre getir", "Girilen gün sayısından fazla olan kayıtlar getirilir (örn: 90 girilirse son 90 gün içinde kayıt olan kullanıcılar getirilmez.)", "Gün sayısı")}
   ${inputItemEkle("Tweet sayısına göre getir", "Girilen tweet sayısından az olan kullanıcılar getirilmez.", "Tweet sayısı")}
   ${inputItemEkle("Tweet sayısına göre getir", "Girilen tweet sayısından az olan kullanıcılar getirilmez.", "Tweet sayısı")}
   ${inputItemEkle("Son tweet atma süresine göre getir", "Son tweetleme zamanı girilen süreden az olan kullanıcılar getirilir", "Son tweet süresi")}
   ${inputItemEkle("Son beğeni atma süresine göre getir", "Son beğenme zamanı girilen süreden az olan kullanıcılar getirilir", "Son beğeni süresi")}
   ${inputItemEkle("Günde atılan ortalama tweet'e göre getir'", "Kullanıcının kayıt olduğu tarihten bu yana attığı tweetlerin ortalaması girilen değerden yüksek olan kayıtlar getirilir. Günde 1 tweet'ten az atan kullanıcılar negatif değerle gösterilir. Örn( 10 günde bir tweet atan biri -10 olarak değer alır.)", "Ortalama Tweet Sayısı")}
   ${inputItemEkle("Günde atılan ortalama beğeniye göre getir'", "Kullanıcının kayıt olduğu tarihten bu yana attığı beğenilerin ortalaması girilen değerden yüksek olan kayıtlar getirilir. Günde 1 beğeniden az beğenen kullanıcılar negatif değerle gösterilir. Örn( 10 günde bir beğeni atan biri -10 olarak değer alır.)", "Ortalama Tweet Sayısı")}
${arasındaItemEkle("Takipçi sayısına göre filtrele", "Takipçi")}
${arasındaItemEkle("Takip Edilen sayısına göre filtrele", "Takip Edilen")}

    </div>
</div>

`);
}


function checkItemEkle(baslik, aciklama) {
    return `<div class="list-group mb-2 shadow" style="margin: 10px">
                <div class="list-group-item">
                    <div class="row align-items-center">
                        <div class="col">
                            <strong class="mb-0">${baslik}</strong>
                            <p class="text-muted mb-0">${aciklama}</p>
                        </div>
                        <div class="col-auto">
                            <div class="form-check form-switch">
                              <input class="form-check-input" type="checkbox" id="flexSwitchCheckDefault">
                              <label class="form-check-label" for="flexSwitchCheckDefault"></label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`
}

function inputItemEkle(baslik, aciklama, placeholder) {
    return `<div class="list-group mb-2 shadow" style="margin: 10px">
                <div class="list-group-item">
                    <div class="row align-items-center">
                        <div class="col">
                            <strong class="mb-0">${baslik}</strong>
                            <p class="text-muted mb-0">${aciklama}</p>
                        </div>
                        <div class="col-auto">
                           <input type="text" class="form-control" placeholder="${placeholder}">
                        </div>
                    </div>
                </div>
            </div>`
}

function arasındaItemEkle(baslik, liste) {
    return `<div class="list-group mb-2 shadow" style="margin: 10px">
                <div class="list-group-item">
                    <div class="row align-items-center">
                        <div class="col">
                            <strong class="mb-0">${baslik}</strong> <br>
<p class="text-muted mb-0">
${liste} sayısı,
<input type="text" class="form-control" placeholder="En düşük">
<input type="text" class="form-control" placeholder="En yüksek">
arasında olan kullanıcılar bulunur.<p>
                        </div>
                    </div>
                </div>
            </div>`
}