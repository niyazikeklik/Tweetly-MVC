using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;
using Tweetly_MVC.Twitter;

namespace Tweetly_MVC.Init
{
    public static class CreateUser
    {
        public static bool Filter(this User profil)
        {
            if (profil.Cinsiyet == "Erkek" && !Hesap.Ins.UserPrefs.CheckErkek)
                return false;
            if (profil.Cinsiyet == "Kadın" && !Hesap.Ins.UserPrefs.CheckKadin)
                return false;
            if (profil.Cinsiyet == "Unisex" && !Hesap.Ins.UserPrefs.CheckUnisex)
                return false;
            if (profil.Cinsiyet == "Belirsiz" && !Hesap.Ins.UserPrefs.CheckBelirsiz)
                return false;

            if (profil.FollowStatus == "Takip et" && Hesap.Ins.UserPrefs.CheckTakipEtmediklerim)
                return false;
            if (profil.FollowStatus == "Takip ediliyor" && Hesap.Ins.UserPrefs.CheckTakipEttiklerim)
                return false;
            if (profil.FollowersStatus == "Takip etmiyor" && Hesap.Ins.UserPrefs.CheckBeniTakipEtmeyenler)
                return false;
            if (profil.FollowersStatus == "Seni takip ediyor" && Hesap.Ins.UserPrefs.CheckBeniTakipEdenler)
                return false;
            if (profil.IsPrivate && Hesap.Ins.UserPrefs.CheckGizliHesap)
                return false;
            return true;
        }
        public static List<User> BegenenleriGetir(string username, int tweetCount = 200)
        {
            List<User> Begenenler = new();
            List<string> Tweets = GetUrlsOfTweet(username, tweetCount);
            Hesap.Ins.Iletisim.currentValue = 0;

            foreach (string item in Tweets)
            {
                List<User> TweetiBegenenler = ListeGezici(link: item + "/likes", detay: false);
                foreach (User profil in TweetiBegenenler)
                {
                    User x = Begenenler.FirstOrDefault(x => x.Username == profil.Username);
                    if (x != null)
                    {
                        Begenenler.Remove(x);
                        x.BegeniSayisi++;
                        x.BegeniOrani = 100 * x.BegeniSayisi / Tweets.Count;
                        Begenenler.Add(x);
                    }
                    else
                        Begenenler.Add(profil.DetayGetir(Drivers.MusaitOlanDriver()));
                }
                Hesap.Ins.Iletisim.currentValue++;
            }
            return Begenenler;

        }
        public static List<User> ListeGezici(string link, bool detay = true, int sayfaLoadWait_MS = 1000)
        {
            List<User> yerelList = new();
            IWebDriver driverr = Drivers.Driver.LinkeGit(link, sayfaLoadWait_MS);
            List<IWebElement> kontrolEdildi = new();
            while (!driverr.IsSayfaSonu())
            {
                IReadOnlyCollection<IWebElement> elementler = Drivers.Driver.GetListelenenler();
                List<IWebElement> kontrolEdilecekler = elementler.Except(kontrolEdildi).ToList();
                foreach (IWebElement element in kontrolEdilecekler)
                {
                    Hesap.Ins.Iletisim.currentValue = yerelList.Count;
                    User profil = element.GetProfil();
                    if (profil == null) continue;
                    if (detay) profil = profil.DetayGetir(Drivers.MusaitOlanDriver());
                    yerelList.Add(profil);
                }
                kontrolEdildi.AddRange(kontrolEdilecekler);
            }
            return yerelList;
        }
        public static User GetProfil(this IWebElement element)
        {
            User profil = new();
            string Text = element.Text.Replace("\r", "");

            profil.Count = Hesap.Ins.Liste.Count + 1;
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
            if (Hesap.Ins.UserPrefs.CheckDetayGetir)
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
            driver.Navigate().GoToUrl(link);
            User profil = new();
            if (Yardimci.Control(driver, username, link, 300000))
            {
                profil.Count = Hesap.Ins.Liste.Count + 1;
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
            if (DBprofil != null && Hesap.Ins.UserPrefs.CheckUseDB)
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
            Drivers.Driver.LinkeGit($"https://mobile.twitter.com/{username}", 2000);
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
