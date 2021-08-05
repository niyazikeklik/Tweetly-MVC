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
        /* public static User getList(this IWebElement element)
          {
            /*  User profil = new User();

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
          }*/

        public static User getProfilSenkron(this IWebDriver driver, string username)
        {
            List<Task> tasks = new List<Task>();
            string link;
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + username;
                driver.Navigate().GoToUrl(link);
            }
            catch { goto yeniden; }
            User profil = new User();
            if (Yardimci.Control(driver, username, link, 300000))
            {
                profil.Count = Hesap.Instance.Takipciler.Count + 1;
                profil.Username = username;
                profil.TweetCount =  driver.getTweetCount();
                profil.Name =  driver.getName();
                profil.Date =  driver.getDate();
                profil.Location =  driver.getLocation();
                profil.PhotoURL =  driver.getPhotoURL(username);
                profil.Following =  driver.getFollowing(username);
                profil.Followers =  driver.getFollowers(username);
                profil.FollowersStatus =  driver.IsFollowers();
                profil.FollowStatus =  driver.getfollowStatus();
                profil.Bio =  driver.getBio();
                profil.isPrivate =  driver.isPrivate();
                profil.Cinsiyet =  UserSetMethods.CinsiyetBul(profil.Name);
                profil.TweetSikligi =  UserSetMethods.getGunlukSiklik(profil.TweetCount, profil.Date);
                profil.LastTweetsDate =  driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);

                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");

                profil.LikeCount =  driver.getLikeCount();
                profil.BegeniSikligi =  UserSetMethods.getGunlukSiklik(profil.LikeCount, profil.Date);
                profil.LastLikesDate =  driver.getLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }



            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }
    }
}
