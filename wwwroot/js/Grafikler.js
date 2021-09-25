function createCinsiyetGrafik(erkekSayisi,kadinSayisi,unisex,belirsiz) {
    const data = {
        labels: [
            'Kız',
            'Erkek',
            'Unisex',
            'Belirsiz',
        ],
        datasets: [
            {
                label: 'Cinsiyet Dağılımı',
                backgroundColor: [
                    'rgba(255, 50, 50, 0.6)',
                    'rgba(50, 50, 255, 0.6)',
                    'rgba(50, 255, 50, 0.6)',
                    'rgba(75, 75, 75, 0.6)'
                ],
                borderColor: [
                    'rgba(255, 100, 100, 0.6)'
                ],
                data: [erkekSayisi, kadinSayisi, unisex, belirsiz],
            }]
    };
    const config = {
        type: 'doughnut',
        data: data,
        options: {}
    };
    var myChart = new Chart(document.getElementById('myChart'), config);
    myChart.canvas.parentNode.style.width = '512px';
}

function createTakipciDurumuGrafik(begeniAktiflik, tweetAktiflik, OrtTweet, OrtBegeni, GTK) {
    const data2 = {
        labels: ["Beğeni Aktifliği", "Tweet Aktifliği", "Ortalama Tweet (Günde)", "Ortalama Beğeni (Günde)", "Genl Takipçi Kalitesi"],
        datasets: [{
            label: 'Takipçi Davranışları',
            data: [begeniAktiflik, tweetAktiflik, OrtTweet, OrtBegeni, GTK],
            backgroundColor: [
                'rgba(255, 50, 60, 0.6)',
                'rgba(50, 200, 50, 0.6)',
                'rgba(150, 100, 27, 0.6)',
                'rgba(35, 100, 255, 0.6)',
                'rgba(200, 50, 230, 0.6)'
            ],
            borderColor: [
                'rgba(255, 0, 0, 0.6)',
            ],
            borderWidth: 2
        }]
    };
    const config2 = {
        type: 'bar',
        data: data2,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        },
    };
    var myChart2 = new Chart(document.getElementById('myChart2'), config2);
    myChart2.canvas.parentNode.style.width = '256px';
}

function grafiklerToggle() {
    $("#grafikler").toggle();
}


createCinsiyetGrafik($("#kadin").val(), $("#erkek").val(), $("#unisex").val(), $("#belirsiz").val());
createTakipciDurumuGrafik($("#toplamSonBegeni").val(), $("#toplamSonTweet").val(), $("#toplamOrtTweet").val(), $("#toplamOrtBegeni").val(), $("#genelTakipciKalitesi").val());
//createTakipciDurumuGrafik(10, 20, 30, 40, 50, 55);