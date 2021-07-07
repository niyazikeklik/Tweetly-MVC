using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public class CreateUser
    {
        public User getList(IWebElement element)
        {
            User profil = new User();

            string Text = element.Text;

            profil.Name = Text.Split('\n')[0].StartsWith('@') ? null: Text.Split('\n')[0];

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




            var bios = element.FindElements(By.XPath("/div/div[2]/div[2]/span"));
            if (bios.Count > 0) profil.Bio = bios[0].Text;



            return profil;
        }

        public User getProfil(IWebDriver driver, string username, bool fast)
        {
            List<Task> tasks = new List<Task>();
            string link;
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + username;
                driver.Navigate().GoToUrl(link);
                Yardimci.WaitForLoad(driver);
            }
            catch { goto yeniden; }

            Yardimci.Control(driver, username, link);

            User profil = new User();

            profil.Username = username;

            Task g1 = UserSetMethods.getTweetCount(driver).ContinueWith(x => profil.TweetCount = x.Result);

            Task g2 = UserSetMethods.getName(driver).ContinueWith(x => profil.Name = x.Result);

            Task g3 = UserSetMethods.getDate(driver).ContinueWith(x => profil.Date = x.Result);

            Task g4 = UserSetMethods.getLocation(driver).ContinueWith(x => profil.Location = x.Result);

            Task g5 = UserSetMethods.getPhotoURL(driver, username).ContinueWith(x => profil.PhotoURL = x.Result);

            Task g6 = UserSetMethods.getFollowing(driver, username).ContinueWith(x => profil.Following = x.Result);

            Task g7 = UserSetMethods.getFollowers(driver, username).ContinueWith(x => profil.Followers = x.Result);

            Task g8 = UserSetMethods.IsFollowers(driver).ContinueWith(x => profil.FollowersStatus = x.Result);

            Task g9 = UserSetMethods.getfollowStatus(driver).ContinueWith(x => profil.FollowStatus = x.Result);

            Task g10 = UserSetMethods.getBio(driver).ContinueWith(x => profil.Bio = x.Result);

            Task g11 = UserSetMethods.isPrivate(driver).ContinueWith(x => profil.isPrivate = x.Result);

            Task.WaitAll(new Task[] { g3, g1 });

            Task g12 = UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date).ContinueWith(x => profil.TweetSikligi = x.Result);

            if (!fast)
            {
                profil.LastTweetsDate = UserSetMethods.getLastTweetsoOrLikesDateAVC(driver, profil.Date, profil.TweetCount).Result;

                Task.WaitAll(new Task[] { g2 });

                driver.FindElement(By.XPath("//a[@href='/" + username + "/likes']")).Click();

                profil.LikeCount = UserSetMethods.getLikeCount(driver).Result;

                profil.BegeniSikligi = UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date).Result;

                profil.LastLikesDate = UserSetMethods.getLastTweetsoOrLikesDateAVC(driver, profil.Date, profil.LikeCount).Result;
            }

            Task.WaitAll(new Task[] { g1, g2, g3, g4, g5, g6, g7, g8, g9, g10, g11, g12 });

            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }

        public User getEconomicProfil(User profil, IWebDriver driver)
        {
            string link = driver.Url;

            Yardimci.Control(driver, profil.Username, link);

            Task g1 = UserSetMethods.getTweetCount(driver).ContinueWith(x => profil.TweetCount = x.Result);

            Task g3 = UserSetMethods.getDate(driver).ContinueWith(x => profil.Date = x.Result);

            Task g4 = UserSetMethods.getLocation(driver).ContinueWith(x => profil.Location = x.Result);

            Task g6 = UserSetMethods.getFollowing(driver, profil.Username).ContinueWith(x => profil.Following = x.Result);

            Task g7 = UserSetMethods.getFollowers(driver, profil.Username).ContinueWith(x => profil.Followers = x.Result);

            Task.WaitAll(new Task[] { g3, g1 });

            Task g11 = UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date).ContinueWith(x => profil.TweetSikligi = x.Result);

            profil.LastTweetsDate = UserSetMethods.getLastTweetsoOrLikesDateAVC(driver, profil.Date, profil.TweetCount).Result;

            driver.FindElement(By.XPath("//a[@href='/" + profil.Username + "/likes']")).Click();

            profil.LikeCount = UserSetMethods.getLikeCount(driver).Result;

            profil.BegeniSikligi = UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date).Result;

            profil.LastLikesDate = UserSetMethods.getLastTweetsoOrLikesDateAVC(driver, profil.Date, profil.LikeCount).Result;

            Task.WaitAll(new Task[] { g4, g6, g7, g11 });

            Drivers.kullanıyorum.Remove(driver);

            return profil;
        }
    }
}
