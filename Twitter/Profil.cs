using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Tweetly_MVC.Init
{
    public static class Profil
    {
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
            return (string)driverr.JSCodeRun("return document.querySelector('[data-testid=\"UserName\"]').textContent.split('@')[1];");
        }

        public static bool IsPrivate(this IWebDriver driverr) => (bool)driverr.JSCodeRun("return document.querySelectorAll('[aria-label=\"Korumalı hesap\"]').length > 0");

        public static int GetFollowing(this IWebDriver driverr)
        {
            string secilenElement = (string)driverr.JSCodeRun("return document.querySelector('a[href*=\"following\"] > span ').textContent;");
            secilenElement = secilenElement.Donustur();
            return Convert.ToInt32(secilenElement);
        }

        public static int GetFollowers(this IWebDriver driverr)
        {
            string secilenElement = (string)driverr.JSCodeRun("return document.querySelector('a[href*=\"followers\"] > span ').textContent;");
            secilenElement = secilenElement.Donustur();
            return Convert.ToInt32(secilenElement);
        }

        public static int GetLikeCount(this IWebDriver driverr)
        {
            string lc = (string)driverr.JSCodeRun("return document.querySelector('header').innerText;");
            string[] dizi = lc.Split("\n");
            lc = dizi[^2];
            int likecount = Convert.ToInt32(lc.Donustur());
            likecount = likecount > 1 ? likecount : likecount + 1;
            return likecount;
        }

        public static int GetTweetCount(this IWebDriver driverr)
        {
            string TweetCount = driverr.JSCodeRun("return document.querySelector('header').innerText;").ToString().Replace("\r", "");
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
            var secilenElement = (string)driverr.JSCodeRun("return document.querySelector('[data-testid=UserProfileHeader_Items] > span:last-child').textContent;");
            return secilenElement.UyelikSuresi();
        }

        public static double GetLastTweetsoOrLikesDateAVC(this IWebDriver driverr, double date, int tweetorlikecount)
        {
            if (tweetorlikecount < 20)
                return Math.Round(date / tweetorlikecount, 2);
            else
            {
                /*      int count = 0;
                      while (driverr.FindElements(By.CssSelector("[data-testid=tweet]")).Count == 0 && count < 20)
                      {
                          Thread.Sleep(150);
                          count++;
                          if (count == 20) return 0;
                      }*/
                driverr.WaitForPageLoad();
                dynamic result = driverr.JSCodeRun(
                    "var tarihler= []; var x = document.querySelectorAll('[data-testid=tweet] time'); x.forEach(x => tarihler.push(x.getAttribute('datetime'))); return tarihler;");
                //      bool sabitTweetVarmi = (bool)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=socialContext]').length > 0");
                double toplamGunSayisi = 0; int kuralaUygunTarihler = 0;
                for (int i = 0; i < result.Count; i++)
                {
                    if (!DateTime.TryParse(result[i], out DateTime YakinTarih)) continue;
                    bool kuralaUygun = true;
                    for (int j = i + 1; j < result.Count; j++)
                    {
                        if (!DateTime.TryParse(result[j], out DateTime UzakTarih)) continue;
                        double AradakiGunSayisi = (YakinTarih - UzakTarih).TotalDays;
                        if (AradakiGunSayisi < -7) { kuralaUygun = false; break; }
                    }
                    if (kuralaUygun) { toplamGunSayisi += (DateTime.Now - YakinTarih).TotalDays; kuralaUygunTarihler++; }
                }
                return Math.Round(toplamGunSayisi / kuralaUygunTarihler, 1);
            }
        }

        public static string GetfollowStatus(this IWebDriver driverr)
        {
            string command = "return document.querySelectorAll('[data-testid=placementTracking]')";
            if ((long)driverr.JSCodeRun(command + ".length;") > 0)
                return (string)driverr.JSCodeRun(command + ".textContent;");
            else return null;
        }

        public static string IsFollowers(this IWebDriver driverr)
        {
            if ((bool)driverr.JSCodeRun("var x = false; document.querySelectorAll(\"div[data-testid='primaryColumn'] div[dir='auto'] > span\").forEach(element => {if (element.textContent.indexOf('Seni takip ediyor') != -1) x = true;}); return x; "))
                return "Seni takip ediyor";
            else return "Takip etmiyor";
        }

        public static string GetBio(this IWebDriver driverr)
        {
            bool x = (long)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=UserDescription]').length;") > 0;
            if (x) return (string)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=UserDescription]')[0].textContent;");
            else return null;
        }

        public static string GetPhotoURL(this IWebDriver driverr, string ka) => (string)driverr.JSCodeRun("return document.querySelector('a[href=\"/" + ka + "/photo\"] img').getAttribute(\"src\");");

        public static string GetName(this IWebDriver driverr) => driverr.JSCodeRun("return document.querySelector('header h2').textContent;").ToString();

        public static string GetGunlukSiklik(int count, double date)
        {
            double gunluktweet = count / date;
            string result = gunluktweet >= 1 ? Math.Round(gunluktweet, 0).ToString() : (-1 * Math.Round(1 / gunluktweet, 0)).ToString();
            return result;
        }

        public static string GetLocation(this IWebDriver driverr)
        {
            if ((long)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=\"UserLocation\"]').length") > 0)
                return (string)driverr.JSCodeRun("return document.querySelector('[data-testid=\"UserLocation\"]').textContent;");
            else return null;
        }
    }
}