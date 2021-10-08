using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;
using Tweetly_MVC.Twitter;
using static System.Net.Mime.MediaTypeNames;

namespace Tweetly_MVC.Init
{
    public static class CreateUser
    {
        public static bool Filter(this User profil)
        {
            if (profil.Cinsiyet == "Erkek" && !Hesap.Instance.SettingsFinder.CheckErkek)
                return false;
            if (profil.Cinsiyet == "Kadın" && !Hesap.Instance.SettingsFinder.CheckKadin)
                return false;
            if (profil.Cinsiyet == "Unisex" && !Hesap.Instance.SettingsFinder.CheckUnisex)
                return false;
            if (profil.Cinsiyet == "Belirsiz" && !Hesap.Instance.SettingsFinder.CheckBelirsiz)
                return false;

            if (profil.FollowStatus == "Takip et" && Hesap.Instance.SettingsFinder.CheckTakipEtmediklerim)
                return false;
            if (profil.FollowStatus == "Takip ediliyor" && Hesap.Instance.SettingsFinder.CheckTakipEttiklerim)
                return false;
            if (profil.FollowersStatus == "Takip etmiyor" && Hesap.Instance.SettingsFinder.CheckBeniTakipEtmeyenler)
                return false;
            if (profil.FollowersStatus == "Seni takip ediyor" && Hesap.Instance.SettingsFinder.CheckBeniTakipEdenler)
                return false;
            if (profil.IsPrivate && Hesap.Instance.SettingsFinder.CheckGizliHesap)
                return false;
            return true;
        } 
        public static List<User> BegenenleriGetir(string username, int tweetCount)
        {
            List<User> Begenenler = new();
            var Tweets = GetUrlsOfTweet(username, tweetCount);
            Hesap.Instance.Iletisim.currentValue = 0;
            foreach (var item in Tweets)
            {
                Drivers.Driver.LinkeGit(item + "/likes");
                while (Drivers.Driver.IsSayfaSonu())
                {
                    var result = Drivers.Driver.GetListelenenler();
                    foreach (IWebElement element in result)
                    {
                        var x = Begenenler.FirstOrDefault(x => x.Username == element.GetUserName());
                        if (x != null)
                        {
                            Begenenler.Remove(x);
                            x.BegeniSayisi++;
                            x.BegeniOrani = 100 * x.BegeniSayisi / tweetCount;
                            Begenenler.Add(x);
                        }
                        else
                        {
                            User profil = element.GetProfil();
                            if (profil != null)
                                Begenenler.Add(profil);
                        }
                        
                    }
                }
                Hesap.Instance.Iletisim.currentValue++;
            }
            return Begenenler;

        }
        public static List<User> ListeGezici(string link, int sayfaLoadWait_MS = 1000)
        {
            List<User> yerelList = new();
            IWebDriver driverr = Drivers.Driver.LinkeGit(link, sayfaLoadWait_MS);
            List<IWebElement> kontrolEdildi = new();
            while (!driverr.IsSayfaSonu())
            {
                var elementler = Drivers.Driver.GetListelenenler();
                var kontrolEdilecekler = elementler.Except(kontrolEdildi).ToList();
                foreach (IWebElement element in kontrolEdilecekler)
                {
                 
                    User profil = element.GetProfil();
                    if (profil != null)
                        yerelList.Add(profil);
                    Hesap.Instance.Iletisim.currentValue = yerelList.Count;
                }
                kontrolEdildi.AddRange(kontrolEdilecekler);
            }
            return yerelList;
        }
        public static User GetProfil(this IWebElement element)
        {
            User profil = new();
            string Text = element.Text.Replace("\r", "");

            profil.Count = Hesap.Instance.Liste.Count + 1;
            profil.Name = Liste.GetName(Text);
            profil.Cinsiyet = Yardimci.CinsiyetBul(profil.Name);
            profil.Username = Liste.GetUserName(Text);
            profil.PhotoURL = element.GetPhotoURL();
            profil.IsPrivate = element.İsPrivate();
            profil.FollowersStatus = Liste.GetFollowersStatus(Text);
            profil.FollowStatus = Liste.GetFollowStatus(Text);
            profil.Bio = Liste.GetBio(element);
            profil.BegeniSayisi = 0;
            profil.BegeniOrani = 0;

            if (!Filter(profil)) return null;

            if (Hesap.Instance.SettingsFinder.CheckDetayGetir)
                profil = Drivers.MusaitOlanDriver().GetProfil(profil);

            return profil;
        }
        public static User GetProfil(this IWebDriver driver, string username)
        {
            string link = $"https://mobile.twitter.com/{username}";
            driver.Navigate().GoToUrl(link);
            User profil = new();
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
        private static User GetProfil(this IWebDriver driver, User profil)
        {

            User DBprofil = new DatabasesContext().Records.FirstOrDefault(x => x.Username == profil.Username);
            if (DBprofil != null && Hesap.Instance.SettingsFinder.CheckUseDB)
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
        private static List<string> GetUrlsOfTweet(string username, int tweetCount)
        {
            List<string> TweetIds = new();
            Drivers.Driver.LinkeGit($"www.twitter.com/{username}", 200);
            while (!Drivers.Driver.IsSayfaSonu() && TweetIds.Count < tweetCount)
            {
                var result = Drivers.Driver.JSCodeRun("return document.querySelectorAll(\"a[id^='id__']\");");
                foreach (IWebElement item in (IReadOnlyCollection<IWebElement>)result)
                {
                    string url = item.GetAttribute("href");
                    if (url.Contains($"/{username}/status/") && TweetIds.Contains(url))
                        TweetIds.Add(url);
                }
            }
            return TweetIds;
        }
    }
}
