using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public static class UserSetMethods
    {
        public static async Task<string> getName(this IWebDriver driverr)
        {

            string[] res = driverr.FindElement(By.XPath("//header[@role='banner']")).Text.Split('\r');
            string isim = res.Length == 1 ? null : res[0];
            return isim;
        }

        public static async Task<int> getTweetCount(this IWebDriver driverr)
        {
            string TweetCount = driverr.FindElement(By.XPath("//header[@role='banner']")).Text;
            TweetCount = TweetCount.Split('\n').Length == 1 ? TweetCount.Split('\n')[0] : TweetCount.Split('\n')[1];
            TweetCount = TweetCount.Replace("Tweets", "")
                                   .Replace("Tweet", "")
                                   .Replace("Tweetler", "");
            TweetCount = Yardimci.Donustur(TweetCount);
            int count = 0;
            count = Convert.ToInt32(TweetCount);
            count = count > 1 ? count : count + 1;
            return count;
        }
        public static async Task<string> getGunlukSiklik(int count, double date)
        {
            string result = "";
            double gunluktweet = (count / date);
            if (gunluktweet >= 1) result = "1/" + Math.Round(gunluktweet, 1);
            else result = Math.Round((1 / gunluktweet), 0) + "/1";
            return result;
        }
        public static async Task<double> getDate(this IWebDriver driverr)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> cocuklar = driverr.FindElement(By.CssSelector("[data-testid=UserProfileHeader_Items]")).FindElements(By.TagName("span"));
            return Yardimci.KayıtTarihi(cocuklar[cocuklar.Count - 1].Text);


        }
        public static async Task<string> getLocation(this IWebDriver driverr)
        {
            string res = driverr.FindElement(By.CssSelector("[data-testid=UserProfileHeader_Items]")).FindElements(By.TagName("span"))[0].Text;
            if (!res.Contains("Doğum") && !res.Contains("Birthday") && !res.Contains("Born") && !res.Contains("Joined") && !res.Contains("tarih"))
                return res;
            else return null;
        }

        public static async Task<int> getFollowing(this IWebDriver driverr, string ka)
        {
            string countText = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/following']")).Text;
            countText = countText.Replace(" Takip edilen", "").Replace(" Following", "");
            countText = Yardimci.Donustur(countText);
            return Convert.ToInt32(countText);

        }
        public static async Task<int> getFollowers(this IWebDriver driverr, string ka)
        {

            string countText = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/followers']")).Text;
            countText = countText.Replace(" Takipçi", "").Replace(" Followers", "");
            countText = Yardimci.Donustur(countText);
            return Convert.ToInt32(countText);

        }

        public static async Task<double> getLastTweetsoOrLikesDateAVC(this IWebDriver driverr, double date, int tweetorlikecount)
        {
            try
            {
                if (tweetorlikecount < 21) return (Math.Round((date / tweetorlikecount), 2));
                else
                {
                    double toplamGunSayisi = 0;
                    List<IWebElement> sonTweetler = driverr.FindElements(By.CssSelector("[data-testid=tweet]")).ToList();
                    int count = 0;
                    while (sonTweetler.Count < 1 && count < 20)
                    {
                        sonTweetler = driverr.FindElements(By.CssSelector("[data-testid=tweet]")).ToList();
                        Thread.Sleep(100);
                        count++;
                    }
                    if (sonTweetler.Count == 0) return 0;
                    if (driverr.FindElements(By.CssSelector("[data-testid=socialContext]")).Count > 0)
                        sonTweetler.Remove(sonTweetler[0]);

                    foreach (IWebElement item in sonTweetler)
                    {
                        string datee = item.FindElement(By.TagName("time")).GetAttribute("datetime");
                        toplamGunSayisi += (DateTime.Today - Convert.ToDateTime(datee)).TotalDays;
                    }
                    var i = Math.Round((toplamGunSayisi / sonTweetler.Count), 2);
                    return i;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static async Task<bool> isPrivate(this IWebDriver driverr)
        {
            return driverr.FindElements(By.CssSelector("[aria-label='Korumalı hesap']")).Count > 0;

        }
        public static async Task<string> getfollowStatus(this IWebDriver driverr)
        {
            if (driverr.FindElements(By.CssSelector("[data-testid=placementTracking]")).Count > 0)
                return driverr.FindElement(By.CssSelector("[data-testid=placementTracking]")).Text;
            return null;
        }
        public static async Task<string> IsFollowers(this IWebDriver driverr)
        {
            if (driverr.FindElements(By.CssSelector("div.css-901oao.css-bfa6kz.r-1awozwy.r-1sw30gj.r-z2wwpe.r-14j79pv.r-6koalj.r-1q142lx.r-1qd0xha.r-1enofrn.r-16dba41.r-fxxt2n.r-13hce6t.r-bcqeeo.r-s1qlax.r-qvutc0")).Count > 0)
                return "Seni\ntakip ediyor";
            else return "Takip\netmiyor";

        }

        public static async Task<string> getBio(this IWebDriver driverr)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> x = driverr.FindElements(By.CssSelector("[data-testid=UserDescription]"));
            if (x.Count > 0)
                return x[0].Text;
            else return null;
        }

        public static async Task<int> getLikeCount(this IWebDriver driverr)
        {
            string lc = driverr.FindElement(By.XPath("//header[@role='banner']")).Text;
            lc = lc.Split('\n').Length == 1 ? lc.Split('\n')[0] : lc.Split('\n')[1];
            lc = lc.Replace("Beğeniler", "").Replace("Beğeni", "").Replace("Likes", "").Replace("Like", "");
            int likecount = Convert.ToInt32(Yardimci.Donustur(lc));
            likecount = likecount > 1 ? likecount : likecount + 1;
            return likecount;
        }
        public static async Task<string> getPhotoURL(this IWebDriver driverr, string ka)
        {
            string photoURL = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/photo']")).FindElement(By.TagName("img")).GetAttribute("src");
            return photoURL;
        }

        public async static Task<string> CinsiyetBul(string name)
        {
            if (String.IsNullOrEmpty(name)) return "Belirsiz";
            string[] isimler = name?.Split(' ');
            string isim = isimler[0].Replace("'", "").Replace(".", "").Replace(",", "").ToLower();
            string cinsiyet = "";
            var result = Hesap.cins.FirstOrDefault(x => x.Ad == isim);
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

    }
}

