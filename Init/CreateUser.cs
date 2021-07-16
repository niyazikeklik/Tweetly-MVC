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
            profil.Cinsiyet = UserSetMethods.CinsiyetBul(profil.Name);
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
        public static User getProfil(this IWebDriver driver, string username, bool fast)
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

            profil.Count = Hesap.Takipciler.Count + 1;
            profil.Username = username;

            profil.TweetCount = driver.getTweetCount();
            if (profil.TweetCount == 0) // Hesap kapatılmış ise
            {
                Drivers.kullanıyorum.Remove(driver);
                return profil;
            }

            Task g2 = Task.Run(() => profil.Name = driver.getName());

            Task g3 = Task.Run(() => profil.Date = driver.getDate());

            Task g4 = Task.Run(() => profil.Location = driver.getLocation());

            Task g5 = Task.Run(() => profil.PhotoURL = driver.getPhotoURL(username));

            Task g6 = Task.Run(() => profil.Following = driver.getFollowing(username));

            Task g7 = Task.Run(() => profil.Followers = driver.getFollowers(username));

            Task g8 = Task.Run(() => profil.FollowersStatus = driver.IsFollowers());

            Task g9 = Task.Run(() => profil.FollowStatus = driver.getfollowStatus());

            Task g10 = Task.Run(() => profil.Bio = driver.getBio());

            Task g11 = Task.Run(() => profil.isPrivate = driver.isPrivate());

            Task.WaitAll(new Task[] { g3, g2 });

            Task g13 = Task.Run(() => profil.Cinsiyet = UserSetMethods.CinsiyetBul(profil.Name));

            Task g12 = Task.Run(() => profil.TweetSikligi = UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date));

            if (!fast)
            {
                profil.LastTweetsDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);

                driver.FindElement(By.XPath("//a[@href='/" + username + "/likes']")).Click();

                profil.LikeCount = driver.getLikeCount();

                profil.BegeniSikligi = UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date);

                profil.LastLikesDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }

            Task.WaitAll(new Task[] { g2, g3, g4, g5, g6, g7, g8, g9, g10, g11, g12 });

            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }
        public static User getEconomicProfil(User profil, IWebDriver driver)
        {
            string link = driver.Url;

            driver.Control(profil.Username, link);

            Task g1 = Task.Run(() => driver.getTweetCount()).ContinueWith(x => profil.TweetCount = x.Result);

            Task g3 = Task.Run(() => driver.getDate()).ContinueWith(x => profil.Date = x.Result);

            Task g4 = Task.Run(() => driver.getLocation()).ContinueWith(x => profil.Location = x.Result);

            Task g6 = Task.Run(() => driver.getFollowing(profil.Username)).ContinueWith(x => profil.Following = x.Result);

            Task g7 = Task.Run(() => driver.getFollowers(profil.Username)).ContinueWith(x => profil.Followers = x.Result);

            Task.WaitAll(new Task[] { g3, g1 });

            Task g11 = Task.Run(() => UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date)).ContinueWith(x => profil.TweetSikligi = x.Result);

            profil.LastTweetsDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);

            driver.FindElement(By.XPath("//a[@href='/" + profil.Username + "/likes']")).Click();

            profil.LikeCount = Task.Run(() => driver.getLikeCount()).Result;

            profil.BegeniSikligi = UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date);

            profil.LastLikesDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);

            Task.WaitAll(new Task[] { g4, g6, g7, g11 });

            Drivers.kullanıyorum.Remove(driver);

            return profil;
        }
    }
}
