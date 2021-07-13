using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public static class CreateUser
    {
        public static User getList(IWebElement element)
        {
            User profil = new User();

            string Text = element.Text;

            profil.Name = Text.Split('\n')[0].StartsWith('@') ? null : Text.Split('\n')[0];
            profil.Cinsiyet = UserSetMethods.CinsiyetBul(profil.Name).Result;
            int basla = Text.IndexOf('@');
            profil.Username = Text.Substring(basla, Text.IndexOf('\n', basla) - basla);

            profil.PhotoURL = element.FindElement(By.TagName("img")).GetAttribute("src").Replace("x96", "200x200");
            profil.isPrivate = element.FindElements(By.CssSelector("[aria-label=Korumalı hesap]")).Count > 0;

            if (Text.Contains("Seni takip ediyor"))
                profil.FollowersStatus = "Seni takip ediyor";
            else profil.FollowersStatus = "Takip etmiyor";

            if (Text.Contains("Takip ediliyor"))
                profil.FollowStatus = "Takip ediliyor";
            else if (Text.Contains("Takip et")) profil.FollowStatus = "Takip et";




            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> bios = element.FindElements(By.XPath("/div/div[2]/div[2]/span"));
            if (bios.Count > 0) profil.Bio = bios[0].Text;



            return profil;
        }
        public static User getProfil(this IWebDriver driver , string username, bool fast)
        {
            List<Task> tasks = new List<Task>();
            string link;
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + username;
                driver.Navigate().GoToUrl(link);
                driver.WaitForLoad();
            }
            catch { goto yeniden; }

            Yardimci.Control(driver, username, link);

            User profil = new User();
            try
            {
                profil.Username = username;

                Task g1 = driver.getTweetCount().ContinueWith(x => profil.TweetCount = x.Result);

                Task g2 = driver.getName().ContinueWith(x => profil.Name = x.Result);

                Task g3 = driver.getDate().ContinueWith(x => profil.Date = x.Result);

                Task g4 = driver.getLocation().ContinueWith(x => profil.Location = x.Result);

                Task g5 = driver.getPhotoURL(username).ContinueWith(x => profil.PhotoURL = x.Result);

                Task g6 = driver.getFollowing(username).ContinueWith(x => profil.Following = x.Result);

                Task g7 = driver.getFollowers(username).ContinueWith(x => profil.Followers = x.Result);

                Task g8 = driver.IsFollowers().ContinueWith(x => profil.FollowersStatus = x.Result);

                Task g9 = driver.getfollowStatus().ContinueWith(x => profil.FollowStatus = x.Result);

                Task g10 = driver.getBio().ContinueWith(x => profil.Bio = x.Result);

                Task g11 = driver.isPrivate().ContinueWith(x => profil.isPrivate = x.Result);

                Task.WaitAll(new Task[] { g3, g1, g2 });

                Task g13 = UserSetMethods.CinsiyetBul(profil.Name).ContinueWith(x => profil.Cinsiyet = x.Result) ;

            Task g12 = UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date).ContinueWith(x => profil.TweetSikligi = x.Result);

                if (!fast)
                {
                    profil.LastTweetsDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount).Result;

                    driver.FindElement(By.XPath("//a[@href='/" + username + "/likes']")).Click();

                    profil.LikeCount = driver.getLikeCount().Result;

                    profil.BegeniSikligi = UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date).Result;

                    profil.LastLikesDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount).Result;
                }

                Task.WaitAll(new Task[] { g1, g2, g3, g4, g5, g6, g7, g8, g9, g10, g11, g12 });

                Drivers.kullanıyorum.Remove(driver);
            }
            catch (System.Exception)
            {
                ;
            }
            return profil;
        }
        public static User getEconomicProfil(User profil, IWebDriver driver )
        {
            string link = driver.Url;

            driver.Control(profil.Username, link);

            Task g1 = driver.getTweetCount().ContinueWith(x => profil.TweetCount = x.Result);

            Task g3 = driver.getDate().ContinueWith(x => profil.Date = x.Result);

            Task g4 = driver.getLocation().ContinueWith(x => profil.Location = x.Result);

            Task g6 = driver.getFollowing(profil.Username).ContinueWith(x => profil.Following = x.Result);

            Task g7 = driver.getFollowers(profil.Username).ContinueWith(x => profil.Followers = x.Result);

            Task.WaitAll(new Task[] { g3, g1 });

            Task g11 = UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date).ContinueWith(x => profil.TweetSikligi = x.Result);

            profil.LastTweetsDate = driver.getLastTweetsoOrLikesDateAVC( profil.Date, profil.TweetCount).Result;

            driver.FindElement(By.XPath("//a[@href='/" + profil.Username + "/likes']")).Click();

            profil.LikeCount = driver.getLikeCount().Result;

            profil.BegeniSikligi = UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date).Result;

            profil.LastLikesDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount).Result;

            Task.WaitAll(new Task[] { g4, g6, g7, g11 });

            Drivers.kullanıyorum.Remove(driver);

            return profil;
        }
    }
}
