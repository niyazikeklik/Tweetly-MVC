using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tweetly_MVC.Init
{
    public class UserSetMethods
    {
        static public async Task<string> getName(IWebDriver driverr)
        {

            var res = driverr.FindElement(By.XPath("//header[@role='banner']")).Text.Split('\r');
            string isim = res.Length == 1 ? null : res[0];
            return isim;
        }

        static public async Task<int> getTweetCount(IWebDriver driverr)
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
        static public async Task<string> getGunlukSiklik(int count, double date)
        {
            string result = "";
            double gunluktweet = (count / date);
            if (gunluktweet >= 1) result = "Günde: " + Math.Round(gunluktweet, 1);
            else result = Math.Round((1 / gunluktweet), 0) + " Günde bir";
            return result;
        }
        static public async Task<double> getDate(IWebDriver driverr)
        {
            var cocuklar = driverr.FindElement(By.CssSelector("[data-testid=UserProfileHeader_Items]")).FindElements(By.TagName("span"));
            return Yardimci.KayıtTarihi(cocuklar[cocuklar.Count - 1].Text);


        }
        static public async Task<string> getLocation(IWebDriver driverr)
        {
            string res = driverr.FindElement(By.CssSelector("[data-testid=UserProfileHeader_Items]")).FindElements(By.TagName("span"))[0].Text;
            if (!res.Contains("Doğum") && !res.Contains("Birthday") && !res.Contains("Born") && !res.Contains("Joined") && !res.Contains("tarih"))
                return res;
            else return null;
        }

        static public async Task<int> getFollowing(IWebDriver driverr, string ka)
        {
            string countText = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/following']")).Text;
            countText = countText.Replace(" Takip edilen", "").Replace(" Following", "");
            countText = Yardimci.Donustur(countText);
            return Convert.ToInt32(countText);

        }
        static public async Task<int> getFollowers(IWebDriver driverr, string ka)
        {

            string countText = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/followers']")).Text;
            countText = countText.Replace(" Takipçi", "").Replace(" Followers", "");
            countText = Yardimci.Donustur(countText);
            return Convert.ToInt32(countText);

        }

        static public async Task<double> getLastTweetsoOrLikesDateAVC(IWebDriver driverr, double date, int tweetorlikecount)
        {
            try
            {
                if (tweetorlikecount < 100) return Math.Round((date / tweetorlikecount), 2);
                else
                {
                    double toplamGunSayisi = 0;
                    List<IWebElement> sonTweetler = driverr.FindElements(By.CssSelector("[data-testid=tweet]")).ToList();
                    int count = 0;
                    while (sonTweetler.Count < 1 && count < 15)
                    {
                        sonTweetler = driverr.FindElements(By.CssSelector("[data-testid=tweet]")).ToList();
                        Thread.Sleep(100);
                        count++;
                    }

                    if (driverr.FindElements(By.CssSelector("[data-testid=socialContext]")).Count > 0)
                        sonTweetler.Remove(sonTweetler[0]);

                    foreach (var item in sonTweetler)
                    {
                        string datee = item.FindElement(By.TagName("time")).GetAttribute("datetime");
                        toplamGunSayisi += (DateTime.Today - Convert.ToDateTime(datee)).TotalDays;
                    }
                    return Math.Round((toplamGunSayisi / sonTweetler.Count), 2);

                }
            }
            catch (Exception)
            {
                ;
            }
            return 0;
        }

        static public async Task<string> getfollowStatus(IWebDriver driverr)
        {
            return driverr.FindElement(By.CssSelector("[data-testid=placementTracking]")).Text;
        }
        static public async Task<string> IsFollowers(IWebDriver driverr)
        {
            if (driverr.FindElements(By.CssSelector("div.css-901oao.css-bfa6kz.r-1awozwy.r-1sw30gj.r-z2wwpe.r-14j79pv.r-6koalj.r-1q142lx.r-1qd0xha.r-1enofrn.r-16dba41.r-fxxt2n.r-13hce6t.r-bcqeeo.r-s1qlax.r-qvutc0")).Count > 0)
                return "Seni\ntakip ediyor";
            else return "Takip\netmiyor";

        }

        static public async Task<string> getBio(IWebDriver driverr)
        {
            var x = driverr.FindElements(By.CssSelector("[data-testid=UserDescription]"));
            if (x.Count > 0)
                return x[0].Text;
            else return null;
        }

        static public async Task<int> getLikeCount(IWebDriver driverr)
        {
            string lc = driverr.FindElement(By.XPath("//header[@role='banner']")).Text;
            lc = lc.Split('\n').Length == 1 ? lc.Split('\n')[0] : lc.Split('\n')[1];
            lc = lc.Replace("Beğeniler", "").Replace("Beğeni", "").Replace("Likes", "").Replace("Like", "");
            int likecount = Convert.ToInt32(Yardimci.Donustur(lc));
            likecount = likecount > 1 ? likecount : likecount + 1;
            return likecount;
        }
        static public async Task<string> getPhotoURL(IWebDriver driverr, string ka)
        {
            string photoURL = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/photo']")).FindElement(By.TagName("img")).GetAttribute("src");
            return photoURL;
        }

    }
}

