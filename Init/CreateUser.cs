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
        public static User getProfil(this IWebElement element, bool detay = false)
        {
            var profil = new TakipEdilen();

            string Text = element.Text.Replace("\r","");

            profil.Name = Text.Split('\n')[0].StartsWith('@') ? null : Text.Split('\n')[0];
            profil.Cinsiyet = UserGetMethods.CinsiyetBul(profil.Name);

            int basla = Text.IndexOf('@');
            profil.Username = Text.Substring(basla, Text.IndexOf('\n', basla) - basla);

            profil.PhotoURL = element.FindElement(By.TagName("img")).GetAttribute("src").Replace("x96", "200x200");
            profil.isPrivate = element.FindElements(By.CssSelector("[aria-label='Korumalı hesap']")).Count > 0;

            if (Text.Contains("Seni takip ediyor"))
                profil.FollowersStatus = "Seni takip ediyor";
            else profil.FollowersStatus = "Takip etmiyor";

            if (Text.Contains("Takip ediliyor"))
                profil.FollowStatus = "Takip ediliyor";
            else if (Text.Contains("Takip et")) profil.FollowStatus = "Takip et";

            var bios = element.FindElements(By.XPath("/div/div[2]/div[2]"));
            if (bios.Count > 0) profil.Bio = bios[0].Text;

            if (detay) profil = (TakipEdilen) Drivers.MusaitOlanDriver().getProfil(profil.Username, profil);
            

            return profil;
        }
        public static User getProfil(this IWebDriver driver, string username, bool faster = false)
        {
            string link;
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + username;
                driver.Navigate().GoToUrl(link);
            }
            catch { goto yeniden; }
            var profil = new TakipEdilen();
            if (Yardimci.Control(driver, username, link, 300000))
            {
                profil.Count = Hesap.Instance.TakipEdilenler.Count + 1;
                profil.Username = username;
                profil.TweetCount = driver.getTweetCount();
                profil.Name = driver.getName();
                profil.Date = driver.getDate();
                profil.Location = driver.getLocation();
                profil.PhotoURL = driver.getPhotoURL(username);
                profil.Following = driver.getFollowing(username);
                profil.Followers = driver.getFollowers(username);
                profil.FollowersStatus = driver.IsFollowers();
                profil.FollowStatus = driver.getfollowStatus();
                profil.Bio = driver.getBio();
                profil.isPrivate = driver.isPrivate();
                profil.Cinsiyet = UserGetMethods.CinsiyetBul(profil.Name);
                profil.TweetSikligi = UserGetMethods.getGunlukSiklik(profil.TweetCount, profil.Date);
                if(!faster) profil.LastTweetsDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);
                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");
                profil.LikeCount = driver.getLikeCount();
                profil.BegeniSikligi = UserGetMethods.getGunlukSiklik(profil.LikeCount, profil.Date);
                if (!faster) profil.LastLikesDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }



            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }
        public static User getProfil(this IWebDriver driver, string username, TakipEdilen profil, bool faster = false)
        {
            string link;
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + username;
                driver.Navigate().GoToUrl(link);
            }
            catch { goto yeniden; }
            if (Yardimci.Control(driver, username, link, 300000))
            {
                profil.TweetCount = driver.getTweetCount();
                profil.Date = driver.getDate();
                profil.Location = driver.getLocation();
                profil.Following = driver.getFollowing(username);
                profil.Followers = driver.getFollowers(username);
                profil.TweetSikligi = UserGetMethods.getGunlukSiklik(profil.TweetCount, profil.Date);
                if (!faster) profil.LastTweetsDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);

                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");

                profil.LikeCount = driver.getLikeCount();
                profil.BegeniSikligi = UserGetMethods.getGunlukSiklik(profil.LikeCount, profil.Date);
                if (!faster) profil.LastLikesDate = driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }
            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }

    }
}
