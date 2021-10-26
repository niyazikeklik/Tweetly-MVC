using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class Profil
    {
        const string RDQS = "return document.querySelector";
        private static string Donustur(this string metin)
        {
            metin = metin
                .Replace("Tweets", "")
                .Replace("Tweet", "")
                .Replace("Tweetler", "")
                .Replace("Beğeniler", "")
                .Replace("Beğeni", "")
                .Replace("Likes", "")
                .Replace("Like", "");

            metin = metin.Replace(" Mn ", "000000");
            metin = !metin.Contains(',') && !metin.Contains('.')
                ? metin.Replace(" B ", "000").Replace(" K ", "000")
                : metin.Replace(" B ", "00").Replace(" K ", "00");

            string number = "";
            foreach (char item in metin)
            {
                if (char.IsNumber(item))
                    number += item;
            }

            return number;
        }
        private static double UyelikSuresi(this string kayittarihi)
        {
            List<string> Months = new() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            string AyAdi = kayittarihi.Split(' ')[0];
            int Yil = Convert.ToInt32(kayittarihi.Split(' ')[1]);

            int Index = Months.IndexOf(AyAdi) + 1;
            int Ay = Index > 12 ? Index - 12 : Index;
            return (DateTime.Today - new DateTime(Yil, Ay, 01)).TotalDays;
        }
        public static string GetUserName(this IWebDriver driverr)
        {
            return (string)driverr.JsRun($"{RDQS}('[data-testid=\"UserName\"]').textContent.split('@')[1];");
        }
        public static bool IsPrivate(this IWebDriver driverr) { 
            return (bool)driverr.JsRun("return document.querySelectorAll('[aria-label=\"Korumalı hesap\"]').length > 0");
        }
        public static int GetFollowing(this IWebDriver driverr)
        {
            string secilenElement = (string)driverr.JsRun($"{RDQS}('a[href*=\"following\"] > span ').textContent;");
            secilenElement = secilenElement.Donustur();
            return Convert.ToInt32(secilenElement);
        }
        public static int GetFollowers(this IWebDriver driverr)
        {
            string secilenElement = (string)driverr.JsRun($"{RDQS}('a[href*=\"followers\"] > span ').textContent;");
            secilenElement = secilenElement.Donustur();
            return Convert.ToInt32(secilenElement);
        }
        public static int GetLikeCount(this IWebDriver driverr)
        {
            string lc = (string)driverr.JsRun($"{RDQS}('header').innerText;");
            string[] dizi = lc.Split("\n");
            lc = dizi[^2];
            int likecount = Convert.ToInt32(lc.Donustur());
            likecount = likecount > 1 ? likecount : likecount + 1;
            return likecount;
        }
        public static int GetTweetCount(this IWebDriver driverr)
        {
            string TweetCount = driverr.JsRun($"{RDQS}('header').innerText;").ToString().Replace("\r", "");
            string[] dizi = TweetCount.Split("\n");
            TweetCount = dizi[^2].Donustur();
            if (int.TryParse(TweetCount, out int countt))
            {
                int r = countt > 1 ? countt : countt + 1;
                return r;
            }
            else
                return countt;
        }
        public static double GetDate(this IWebDriver driverr)
        {
            var secilenElement = (string)driverr.JsRun($"{RDQS}('[data-testid=UserProfileHeader_Items] > span:last-child').textContent;");
            return secilenElement.UyelikSuresi();
        }
        public static double GetSonEtkilesimOrtalama(this IWebDriver driverr, double date, int count)
        {
            if (count < 20)
                return Math.Round(date / count, 2);
            else
            {
                driverr.WaitForPageLoad();
                dynamic Tarihler = driverr.JsRun("var tarihler= []; var x = document.querySelectorAll('[data-testid=tweet] time'); x.forEach(x => tarihler.push(x.getAttribute('datetime'))); return tarihler;");
                double toplamGunSayisi = 0; int kuralaUygunTarihler = 0;
                if (Tarihler == null) return Math.Round(date / count, 2);
                for (int i = 0; i < Tarihler.Count; i++)
                {
                    if (!DateTime.TryParse(Tarihler[i], out DateTime YakinTarih)) continue;
                    bool kuralaUygun = true;
                    for (int j = i + 1; j < Tarihler.Count; j++)
                    {
                        if (!DateTime.TryParse(Tarihler[j], out DateTime UzakTarih)) continue;
                        double AradakiGunSayisi = (YakinTarih - UzakTarih).TotalDays;
                        if (AradakiGunSayisi < -7) { kuralaUygun = false; break; }
                    }
                    if (kuralaUygun)
                    {
                        toplamGunSayisi += (DateTime.Now - YakinTarih).TotalDays;
                        kuralaUygunTarihler++;
                    }  
                }
                return Math.Round(toplamGunSayisi / kuralaUygunTarihler, 1);
            }
        }
        public static string GetfollowStatus(this IWebDriver driverr)
        {
            string command = $"{RDQS}All('[data-testid=placementTracking]')";
            if ((long)driverr.JsRun(command + ".length;") > 0)
                return (string)driverr.JsRun(command + "[0].textContent;");
            else return null;
        }
        public static string IsFollowers(this IWebDriver driverr)
        {
            if ((bool)driverr.JsRun("var x = false; document.querySelectorAll(\"div[data-testid='primaryColumn'] div[dir='auto'] > span\").forEach(element => {if (element.textContent.indexOf('Seni takip ediyor') != -1) x = true;}); return x; "))
                return "Seni takip ediyor";
            else return "Takip etmiyor";
        }
        public static bool YenileButonuContains(this IWebDriver driverr)
        {
            return (bool)driverr.JsRun($"{RDQS}All('[data-testid=primaryColumn] > div > div > div > [role=button]').length > 0;");
        }
        public static bool IsBlocker(this IWebDriver driverr)
        {
           return (bool)driverr.JsRun($"{RDQS}All('[data-testid=\"emptyState\"]').length > 0");
        }
        public static string GetBio(this IWebDriver driverr)
        {
            bool x = (long)driverr.JsRun($"{RDQS}All('[data-testid=UserDescription]').length;") > 0;
            if (x) return (string)driverr.JsRun($"{RDQS}All('[data-testid=UserDescription]')[0].textContent;");
            else return null;
        }
        public static string GetPhotoURL(this IWebDriver driverr, string ka) { 
            return (string)driverr.JsRun("return document.querySelector('a[href=\"/" + ka + "/photo\"] img').getAttribute(\"src\");"); 
        }
        public static string GetName(this IWebDriver driverr) { 
            return driverr.JsRun($"{RDQS}('header h2').textContent;").ToString(); 
        }
        public static string GetGunlukSiklik(int count, double date)
        {
            double gunluktweet = count / date;
            string result = gunluktweet >= 1 ? Math.Round(gunluktweet, 0).ToString() : (-1 * Math.Round(1 / gunluktweet, 0)).ToString();
            return result;
        }
        public static string GetLocation(this IWebDriver driverr)
        {
            if ((long)driverr.JsRun($"{RDQS}All('[data-testid=\"UserLocation\"]').length") > 0)
                return (string)driverr.JsRun($"{RDQS}('[data-testid=\"UserLocation\"]').textContent;");
            else return null;
        }
    }
}