using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetly_MVC.Init
{
    public class Yardimci
    {
        public static string Donustur(string metin)
        {
            metin = metin.Replace(" ", "");
            metin = metin.Replace("Mn", "000000");
            if (metin.IndexOf(',') == -1 && metin.IndexOf('.') == -1)
            {
                metin = metin.Replace("B", "000").Replace("K", "000");
            }
            else
            {
                metin = metin.Replace("B", "00").Replace("K", "00");
            }
            metin = metin.Replace(".", "").Replace(" ", "").Replace(",", "");
            return metin;
        }
        public static double KayıtTarihi(string kayittarihi)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            kayittarihi = kayittarihi.Replace("tarihinde katıldı", "").Replace("joined", "");
            string ayadi = "";
            foreach (var item in months)
                if (kayittarihi.Contains(item)) ayadi = item;
            kayittarihi = kayittarihi.Replace(" ", "").Replace(ayadi, "");
            int yil = Convert.ToInt32(kayittarihi);
            int kacinciay = 01;
            switch (ayadi)
            {
                case "January": kacinciay = 01; break;
                case "Ocak": kacinciay = 01; break;

                case "February": kacinciay = 02; break;
                case "Şubat": kacinciay = 02; break;

                case "March": kacinciay = 03; break;
                case "Mart": kacinciay = 03; break;

                case "April": kacinciay = 04; break;
                case "Nisan": kacinciay = 04; break;

                case "May": kacinciay = 05; break;
                case "Mayıs": kacinciay = 05; break;

                case "June": kacinciay = 06; break;
                case "Haziran": kacinciay = 06; break;

                case "July": kacinciay = 07; break;
                case "Temmuz": kacinciay = 07; break;

                case "August": kacinciay = 08; break;
                case "Ağustos": kacinciay = 08; break;

                case "September": kacinciay = 09; break;
                case "Eylül": kacinciay = 09; break;

                case "October": kacinciay = 10; break;
                case "Ekim": kacinciay = 10; break;

                case "November": kacinciay = 11; break;
                case "Kasım": kacinciay = 11; break;

                case "December": kacinciay = 12; break;
                case "Aralık": kacinciay = 12; break;

                default: break;
            }
            DateTime baslamaTarihi = new DateTime(yil, kacinciay, 01);
            DateTime bitisTarihi = DateTime.Today;
            var kalangun = bitisTarihi - baslamaTarihi;//Sonucu zaman olarak döndürür
            return kalangun.TotalDays;// kalanGun den TotalDays ile sadece toplam gun değerini çekiyoruz.
        }
    }
}
