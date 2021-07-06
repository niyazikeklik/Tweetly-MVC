using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public class CreateUser
    {
        string userName;
        string link;
        bool fast;
        public static void WaitForLoad(IWebDriver driver, int timeoutSec = 15)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
        void Control(IWebDriver driver)
        {
            //By.cssSelector("a[href='mysite.com']");
            //https://mobile.twitter.com/login
            ///login
            try
            {
                if (driver.FindElements(By.CssSelector("[data-testid=login]")).Count > 0)
                {
                    driver.Navigate().Refresh();
                    WaitForLoad(driver);
                    Control(driver);
                }


                while (driver.FindElements(By.XPath("//a[@href='/" + this.userName + "/followers']")).Count == 0)
                {
                    if (driver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0) break;
                    if (driver.Url.Contains("limit"))
                    {
                        Thread.Sleep(300000);
                        driver.Navigate().GoToUrl(this.link);
                        Control(driver);
                    }
                    Thread.Sleep(5);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(300000);
                driver.Navigate().GoToUrl(link);
                Control(driver);
            }

        }

        public User getProfil(IWebDriver driver, string username, bool fast)
        {
            List<Task> tasks = new List<Task>();
            this.userName = username;
            this.fast = fast;
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + userName;
                driver.Navigate().GoToUrl(link);
                WaitForLoad(driver);
            }
            catch { goto yeniden; }
            Control(driver);
            User profil = new User();
            profil.Username = userName;

            Task g5 = UserSetMethods.getDate(driver).ContinueWith(x => profil.Date = x.Result);
            tasks.Add(g5);

            Task g6 = UserSetMethods.getLocation(driver).ContinueWith(x => profil.Location = x.Result);
            tasks.Add(g6);

            Task g13 = UserSetMethods.getPhotoURL(driver, userName).ContinueWith(x => {
                profil.PhotoURL = x.Result;
            });
            tasks.Add(g13);

            Task g7 = UserSetMethods.getFollowing(driver, userName).ContinueWith(x => profil.Following = x.Result);
            tasks.Add(g7);

            Task g8 = UserSetMethods.getFollowers(driver, userName).ContinueWith(x => profil.Followers = x.Result);
            tasks.Add(g8);

            Task g1 = UserSetMethods.getName(driver).ContinueWith(x => profil.Name = x.Result);
            tasks.Add(g1);

            Task g2 = UserSetMethods.IsFollowers(driver).ContinueWith(x => profil.FollowersStatus = x.Result);
            tasks.Add(g2);

            Task g3 = UserSetMethods.getfollowStatus(driver).ContinueWith(x => profil.FollowStatus = x.Result);
            tasks.Add(g3);

            Task g4 = UserSetMethods.getBio(driver).ContinueWith(x => profil.Bio = x.Result);
            tasks.Add(g4);

            Task g11 = UserSetMethods.getTweetCount(driver).ContinueWith(x => profil.TweetCount = x.Result);
            tasks.Add(g11);

            Task.WaitAll(new Task[] { g5, g11 });
            Task g15 = UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date).ContinueWith(x => profil.TweetSikligi = x.Result);
            tasks.Add(g15);

            if (!fast)
            {

                Task.WaitAny(g11);
                Task g12 = UserSetMethods.getLastTweetsoOrLikesDateAVC(driver, profil.Date, profil.TweetCount).ContinueWith(x => profil.LastTweetsDate = x.Result);


                Task.WaitAll(new Task[] { g1, g12 });

                try
                {
                    driver.FindElement(By.XPath("//a[@href='/" + userName + "/likes']")).Click();

                    Task g9 = UserSetMethods.getLikeCount(driver).ContinueWith(x => profil.LikeCount = x.Result);
                    tasks.Add(g9);
                    Task.WaitAny(g9);

                    Task g16 = UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date).ContinueWith(x => profil.BegeniSikligi = x.Result);
                    tasks.Add(g16);

                    Task g10 = UserSetMethods.getLastTweetsoOrLikesDateAVC(driver, profil.Date, profil.LikeCount).ContinueWith(x => profil.LastLikesDate = x.Result);
                    tasks.Add(g10);
                }
                catch {; }
            }

            Task.WaitAll(tasks.ToArray());
            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }
    }
}
