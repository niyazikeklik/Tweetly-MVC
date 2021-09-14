using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public static class Profil
    {
        public static string GetUserName(this IWebElement element)
        {
            string text = element.Text;
            int basla = text.IndexOf('@');
            string Username = text[basla..text.IndexOf('\n', basla)];
            return Username;
        }
        public static bool IsPrivate(this IWebDriver driverr)
        {
            return (bool)driverr.JSCodeRun("return document.querySelectorAll('[aria-label=\"Korumalı hesap\"]').length > 0");
        }
        public static int GetFollowing(this IWebDriver driverr, string ka)
        {
            string secilenElement = (string)driverr.JSCodeRun("return document.querySelector('a[href=\"/" + ka + "/following\"] > span ').textContent;");
            secilenElement = Yardimci.Donustur(secilenElement);
            return Convert.ToInt32(secilenElement);
        }
        public static int GetFollowers(this IWebDriver driverr, string ka)
        {

            string secilenElement = (string)driverr.JSCodeRun("return document.querySelector('a[href=\"/" + ka + "/followers\"] > span ').textContent;");
            secilenElement = Yardimci.Donustur(secilenElement);
            return Convert.ToInt32(secilenElement);

        }
        public static int GetLikeCount(this IWebDriver driverr)
        {
            string lc = ((string)driverr.JSCodeRun("return document.querySelector('header').innerText;"));
            var dizi = lc.Split("\n");
            lc = dizi[^2];
            int likecount = Convert.ToInt32(Yardimci.Donustur(lc));
            likecount = likecount > 1 ? likecount : likecount + 1;
            return likecount;
        }
        public static int GetTweetCount(this IWebDriver driverr)
        {
            string TweetCount = driverr.JSCodeRun("return document.querySelector('header').innerText;").ToString().Replace("\r", "");
            var dizi = TweetCount.Split("\n");
            TweetCount = dizi[^2];
            TweetCount = Yardimci.Donustur(TweetCount);
            if (int.TryParse(TweetCount, out int countt))
            {
                var r = countt > 1 ? countt : countt + 1;
                return r;
            }
            else
                return countt;
        }
        public static double GetDate(this IWebDriver driverr)
        {
            var secilenElement = driverr.JSCodeRun("return document.querySelector('[data-testid=UserProfileHeader_Items] > span:last-child').textContent;");
            return Yardimci.KayıtTarihi(secilenElement.ToString());


        }
        public static double GetLastTweetsoOrLikesDateAVC(this IWebDriver driverr, double date, int tweetorlikecount)
        {

            if (tweetorlikecount < 21) return (Math.Round((date / tweetorlikecount), 2));
            else
            {
                int count = 0;
                while (driverr.FindElements(By.CssSelector("[data-testid=tweet]")).Count == 0 && count < 20)
                {
                    Thread.Sleep(150);
                    count++;
                    if (count == 20) return 0;
                }

                double toplamGunSayisi = 0;
                dynamic result = driverr.JSCodeRun(
                    "var tarihler= []; var x = document.querySelectorAll('[data-testid=tweet] time'); x.forEach(x => tarihler.push(x.getAttribute('datetime'))); return tarihler;");
                //      bool sabitTweetVarmi = (bool)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=socialContext]').length > 0");
                int kuralaUygunTarihler = 0;
                for (int i = 0; i < result.Count; i++)
                {
                    if (!DateTime.TryParse(result[i], out DateTime tarih)) continue;
                    DateTime tarih2 = new();
                    for (int j = i + 1; j < result.Count; j++)
                    {
                        if (!DateTime.TryParse(result[j], out tarih2)) continue;
                        if ((tarih - tarih2).TotalDays < -7) break;
                    }
                    if ((tarih - tarih2).TotalDays > -7)
                    {
                        toplamGunSayisi += (DateTime.Now - tarih).TotalDays;
                        kuralaUygunTarihler++;
                    }
                }

                return Math.Round((toplamGunSayisi / kuralaUygunTarihler), 1);
            }

        }
        public static string GetfollowStatus(this IWebDriver driverr)
        {
            if ((Int64)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=placementTracking]').length;") > 0)
                return (string)driverr.JSCodeRun("return document.querySelector('[data-testid=placementTracking]').textContent;");
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

            var x = (Int64)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=UserDescription]').length;") > 0;
            if (x) return (string)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=UserDescription]')[0].textContent;");
            else return null;
        }
        public static string GetPhotoURL(this IWebDriver driverr, string ka)
        {
            return (string)driverr.JSCodeRun("return document.querySelector('a[href=\"/" + ka + "/photo\"] img').getAttribute(\"src\");");
        }
        public static string CinsiyetBul(string name)
        {

            if (String.IsNullOrEmpty(name)) return "Belirsiz";
            string[] isimler = name?.Split(' ');
            string isim = isimler[0].Replace("'", "").Replace(".", "").Replace(",", "").ToLower();
            string cinsiyet = "Belirsi...z";
            var result = Hesap.Instance.Cins.FirstOrDefault(x => x.Ad == isim);
            if (result != null)
            {
                cinsiyet = result.Cinsiyet == "e" ? "Erkek" :
                           result.Cinsiyet == "k" ? "Kadın" :
                           result.Cinsiyet == "u" ? "Unisex" : "Belirsi...z";
                return cinsiyet;
            }
            else
            {
                /*
                isim = isim.StringReplace();
                var msg =  await client.GetStringAsync("https://api.genderize.io?name=" + isim);
                cinsiyet = msg.Contains("male") ? "Erke-k" : 
                           msg.Contains("female") ? "Kadı-n" : 
                           "Belirsiz";
                return cinsiyet;*/
                return "Belirsiz";
            }
        }
        public static string GetName(this IWebDriver driverr)
        {
            return driverr.JSCodeRun("return document.querySelector('header h2').textContent;").ToString();
        }
        public static string GetGunlukSiklik(int count, double date)
        {
            double gunluktweet = (count / date);
            string result;
            if (gunluktweet >= 1) result = Math.Round(gunluktweet, 0).ToString();
            else result = (-1 * Math.Round((1 / gunluktweet), 0)).ToString();
            return result;
        }
        public static string GetLocation(this IWebDriver driverr)
        {
            var res = driverr.JSCodeRun("return document.querySelector('[data-testid=UserProfileHeader_Items] > span').textContent;")?.ToString().ToLower();
            if (res != null && !res.Contains("doğum") && !res.Contains("birthday") && !res.Contains("born") && !res.Contains("joined") && !res.Contains("tarih"))
                return res;
            else return null;
        }

    }
}

