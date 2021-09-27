using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;
using Tweetly_MVC.Twitter;

namespace Tweetly_MVC.Init
{
    public static class CreateUser
    {
        public static object ListeGezici(string link, int sayfaLoadWait_MS = 1000)
        {
            Hesap.Instance.Liste.Clear();
            IWebDriver driverr = Drivers.Driver.LinkeGit(link, sayfaLoadWait_MS);
            List<IWebElement> kontrolEdildi = new();
            while (!driverr.IsSayfaSonu())
            {
                var elementler = driverr.FindElements(By.CssSelector("[data-testid=UserCell]"));
                var kontrolEdilecekler = elementler.Except(kontrolEdildi);
                foreach (var element in kontrolEdilecekler)
                {
                    User profil = element.GetProfil();
                    if (profil != null)
                        Hesap.Instance.Liste.Add(profil);

                    kontrolEdildi.Add(element);
                }
            }
            return Hesap.Instance.Liste;
        }
        public static bool Filter(this User profil)
        {
            if (profil.Cinsiyet == "Erkek" && !Hesap.Instance.Settings.CheckErkek)
                return false;
            if (profil.Cinsiyet == "Kadın" && !Hesap.Instance.Settings.CheckKadin)
                return false;
            if (profil.Cinsiyet == "Unisex" && !Hesap.Instance.Settings.CheckUnisex)
                return false;
            if (profil.Cinsiyet == "Belirsiz" && !Hesap.Instance.Settings.CheckBelirsiz)
                return false;


            if (profil.FollowStatus == "Takip et" && Hesap.Instance.Settings.CheckTakipEtmediklerim)
                return false;
            if (profil.FollowStatus == "Takip ediliyor" && Hesap.Instance.Settings.CheckTakipEttiklerim)
                return false;
            if (profil.FollowersStatus == "Takip etmiyor" && Hesap.Instance.Settings.CheckBeniTakipEtmeyenler)
                return false;
            if (profil.FollowersStatus == "Seni takip ediyor" && Hesap.Instance.Settings.CheckBeniTakipEdenler)
                return false;
            if (profil.IsPrivate && Hesap.Instance.Settings.CheckGizliHesap)
                return false;
            return true;
        }
        public static User GetProfil(this IWebElement element)
        {
            var profil = new User();
            string Text = element.Text.Replace("\r", "");
            profil.Count = Hesap.Instance.Liste.Count + 1;
            profil.Name = Liste.GetName(Text);
            profil.Cinsiyet = Yardimci.CinsiyetBul(profil.Name);
            profil.Username = Liste.GetUserName(Text);
            profil.PhotoURL = Liste.GetPhotoURL(element);
            profil.IsPrivate = Liste.İsPrivate(element);
            profil.FollowersStatus = Liste.GetFollowersStatus(Text);
            profil.FollowStatus = Liste.GetFollowStatus(Text);
            profil.Bio = Liste.GetBio(element);

            if (!profil.Filter()) return null;

            if (Hesap.Instance.Settings.CheckDetayGetir)
            {
                if (Hesap.Instance.Settings.CheckUseDB)
                {
                    User DBprofil = new DatabasesContext().Records.FirstOrDefault(x => x.Username == profil.Username);
                    if (DBprofil != null)
                    {
                        profil.TweetCount = DBprofil.TweetCount;
                        profil.Date = DBprofil.Date;
                        profil.Location = DBprofil.Location;
                        profil.Following = DBprofil.Following;
                        profil.Followers = DBprofil.Followers;
                        profil.TweetSikligi = DBprofil.TweetSikligi;
                        profil.LastTweetsDate = DBprofil.LastTweetsDate;
                        profil.LikeCount = DBprofil.LikeCount;
                        profil.BegeniSikligi = DBprofil.BegeniSikligi;
                        profil.LastLikesDate = DBprofil.LastLikesDate;
                        return profil;
                    }
                }
                profil = Drivers.MusaitOlanDriver().GetProfil(profil);
            }
            return profil;


        }
        public static User GetProfil(this IWebDriver driver, string username)
        {
            string link = $"https://mobile.twitter.com/{username}";
            driver.Navigate().GoToUrl(link);
            var profil = new User();
            if (Yardimci.Control(driver, username, link, 300000))
            {
                profil.Count = Hesap.Instance.Liste.Count + 1;
                profil.Username = username;
                profil.TweetCount = driver.GetTweetCount();
                profil.Name = driver.GetName();
                profil.Date = driver.GetDate();
                profil.Location = driver.GetLocation();
                profil.PhotoURL = driver.GetPhotoURL(username);
                profil.Following = driver.GetFollowing(username);
                profil.Followers = driver.GetFollowers(username);
                profil.FollowersStatus = driver.IsFollowers();
                profil.FollowStatus = driver.GetfollowStatus();
                profil.Bio = driver.GetBio();
                profil.IsPrivate = driver.IsPrivate();
                profil.Cinsiyet = Yardimci.CinsiyetBul(profil.Name);
                profil.TweetSikligi = Profil.GetGunlukSiklik(profil.TweetCount, profil.Date);
                profil.LastTweetsDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);
                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");
                profil.LikeCount = driver.GetLikeCount();
                profil.BegeniSikligi = Profil.GetGunlukSiklik(profil.LikeCount, profil.Date);
                profil.LastLikesDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }

            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }
        private static User GetProfil(this IWebDriver driver , User profil)
        {
            string link = "https://mobile.twitter.com/" + profil.Username; 
            driver.Navigate().GoToUrl(link);

            if (Yardimci.Control(driver, profil.Username, link, 300000))
            {
                profil.TweetCount = driver.GetTweetCount();
                profil.Date = driver.GetDate();
                profil.Location = driver.GetLocation();
                profil.Following = driver.GetFollowing(profil.Username);
                profil.Followers = driver.GetFollowers(profil.Username);
                profil.TweetSikligi = Profil.GetGunlukSiklik(profil.TweetCount, profil.Date);
                profil.LastTweetsDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);

                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");

                profil.LikeCount = driver.GetLikeCount();
                profil.BegeniSikligi = Profil.GetGunlukSiklik(profil.LikeCount, profil.Date);
                profil.LastLikesDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }
            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }

    }
}
