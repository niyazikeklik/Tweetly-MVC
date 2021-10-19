using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Cloud;
using Tweetly_MVC.Tweetly;
using Tweetly_MVC.Twitter;

namespace Tweetly_MVC.Init
{
    public static class CreateUser
    {
        public static bool Filter(this User profil)
        {
            if (profil.Cinsiyet == "Erkek" && !Repo.Ins.UserPrefs.CheckErkek)
                return false;
            if (profil.Cinsiyet == "Kadın" && !Repo.Ins.UserPrefs.CheckKadin)
                return false;
            if (profil.Cinsiyet == "Unisex" && !Repo.Ins.UserPrefs.CheckUnisex)
                return false;
            if (profil.Cinsiyet == "Belirsiz" && !Repo.Ins.UserPrefs.CheckBelirsiz)
                return false;

            if (profil.FollowStatus == "Takip et" && Repo.Ins.UserPrefs.CheckTakipEtmediklerim)
                return false;
            if (profil.FollowStatus == "Takip ediliyor" && Repo.Ins.UserPrefs.CheckTakipEttiklerim)
                return false;
            if (profil.FollowersStatus == "Takip etmiyor" && Repo.Ins.UserPrefs.CheckBeniTakipEtmeyenler)
                return false;
            if (profil.FollowersStatus == "Seni takip ediyor" && Repo.Ins.UserPrefs.CheckBeniTakipEdenler)
                return false;
            if (profil.IsPrivate && Repo.Ins.UserPrefs.CheckGizliHesap)
                return false;
            return true;
        }
        public static List<User> BegenenleriGetir(string username)
        {
            List<User> Begenenler = new();
            List<string> Tweets = GetUrlsOfTweet(username, Repo.Ins.UserPrefs.TextTweetControl);

            foreach (string item in Tweets)
            {
                List<User> TweetiBegenenler = ListeGezici(link: item + "/likes", detay: false);
                foreach (User profil in TweetiBegenenler)
                {
                    User x = Begenenler.FirstOrDefault(x => x.Username == profil.Username);
                    if (x != null) Begenenler.Remove(x);
                    else x = profil.DetayGetir(Drivers.MusaitOlanDriver());
                    x.BegeniSayisi++;
                    x.BegeniOrani = Math.Round((double)x.BegeniSayisi / (double)Tweets.Count, 2);
                    Begenenler.Add(x);
                }
                Repo.Ins.Iletisim.HataMetni = "Kontrol Edilen Tweet: " + Tweets.IndexOf(item) + 1 + " | ";
            }
            return Begenenler;

        }
        public static List<User> ListeGezici(string link, bool detay = true)
        {
            List<User> yerelList = new();
            IWebDriver driverr = Drivers.Driver.LinkeGit(link);
            List<string> kontrolEdildi = new();
            while (!driverr.IsSayfaSonu() && yerelList.Count < Repo.Ins.UserPrefs.TextBulunacakKisiSayisi)
            {
                var elementler = Drivers.Driver.GetListelenenler();
                foreach (string element in elementler)
                {
                    string html = element.Split(new string(Liste.sabit))[0];
                    string text = element.Split(new string(Liste.sabit))[1]; 
                    if (kontrolEdildi.Contains(text)) continue;
                    Repo.Ins.Iletisim.currentValue = yerelList.Count;
                    User profil = GetProfil(html, text);
                    if (profil != null)
                    {
                        if (detay) profil = profil.DetayGetir(Drivers.MusaitOlanDriver());
                        yerelList.Add(profil);
                    }
                    kontrolEdildi.Add(text);
                }
             
            }

            return yerelList;
        }
        public static User GetProfil(string innerHTML, string innerText)
        {
            User profil = new();

            profil.Count = Repo.Ins.Liste.Count + 1;
            profil.Name = Liste.GetName(innerText);
            profil.PhotoURL = innerHTML.GetPhotoURL();
            profil.Cinsiyet = DetectGender.CinsiyetBul(profil.Name, profil.PhotoURL);
            profil.Username = Liste.GetUserName(innerText);

            profil.IsPrivate = innerHTML.İsPrivate();
            profil.FollowersStatus = Liste.GetFollowersStatus(innerText);
            profil.FollowStatus = Liste.GetFollowStatus(innerText);
            profil.Bio = Liste.GetBio(innerHTML);
            profil.BegeniSayisi = 0;
            profil.BegeniOrani = 0;

            profil = Filter(profil) ? profil : null;
            return profil;
        }
        public static User DetayGetir(this User profil, IWebDriver musaitDriver)
        {
            if (profil == null)
            {
                Drivers.kullanıyorum.Remove(musaitDriver);
                return null;
            }
            if (Repo.Ins.UserPrefs.CheckDetayGetir)
                return musaitDriver.GetProfil(profil);
            else
            {
                Drivers.kullanıyorum.Remove(musaitDriver);
                return profil;
            }
        }
        public static User GetProfil(this IWebDriver driver, string username)
        {
            string link = $"https://mobile.twitter.com/{username}";
            driver.LinkeGit(link);
            User profil = new();
            if (Waiters.ProfilLoadControl(driver, link, 300000))
            {
                profil.Count = Repo.Ins.Liste.Count + 1;
                profil.Username = username;
                profil.TweetCount = driver.GetTweetCount();
                profil.Name = driver.GetName();
                profil.Date = driver.GetDate();
                profil.Location = driver.GetLocation();
                profil.PhotoURL = driver.GetPhotoURL(username);
                profil.Following = driver.GetFollowing();
                profil.Followers = driver.GetFollowers();
                profil.FollowersStatus = driver.IsFollowers();
                profil.FollowStatus = driver.GetfollowStatus();
                profil.Bio = driver.GetBio();
                profil.IsPrivate = driver.IsPrivate();
                profil.Cinsiyet = DetectGender.CinsiyetBul(profil.Name, profil.PhotoURL);
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
        private static User GetProfil(this IWebDriver driver, User profil)
        {

            User DBprofil = new DatabasesContext().Records.FirstOrDefault(x => x.Username == profil.Username);
            if (DBprofil != null && Repo.Ins.UserPrefs.CheckUseDB)
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
                Drivers.kullanıyorum.Remove(driver);
                return profil;
            }


            string link = "https://mobile.twitter.com/" + profil.Username;
            driver.LinkeGit(link);

            if (Waiters.ProfilLoadControl(driver, link, 300000))
            {
                profil.TweetCount = driver.GetTweetCount();
                profil.Date = driver.GetDate();
                profil.Location = driver.GetLocation();
                profil.Following = driver.GetFollowing();
                profil.Followers = driver.GetFollowers();
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
        private static List<string> GetUrlsOfTweet(string username, int tweetCount)
        {

            List<string> TweetIds = new();
            Drivers.Driver.LinkeGit($"https://mobile.twitter.com/{username}");
            while (!Drivers.Driver.IsSayfaSonu() && TweetIds.Count < tweetCount)
            {
                object result = Drivers.Driver.JSCodeRun("return document.querySelectorAll(\"a[id^='id__']\");");
                foreach (IWebElement item in (IReadOnlyCollection<IWebElement>)result)
                {
                    try
                    {
                        string url = item.GetAttribute("href");
                        if (url.Contains($"/{username}/status/") && !TweetIds.Contains(url))
                            TweetIds.Add(url);
                    }
                    catch (System.Exception) { continue; }

                }
            }
            return TweetIds;

        }
    }
}
