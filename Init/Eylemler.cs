using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;
using Tweetly_MVC.Init;
using OpenQA.Selenium;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Tweetly_MVC.Init
{
    public static class Eylemler
    {
        public static void TakipcilerdenCikar(this IWebDriver driver, User profil)
        {
            string link = "https://twitter.com/" + profil.Username;
            driver.Navigate().GoToUrl(link);
            driver.Control(profil.Username, link);


            driver.ProfilEngelle();
            if (driver.GetfollowStatus().StartsWith("Engel"))
                driver.ProfilUserButonClick();
            // Engeli kaldır.

            Drivers.kullanıyorum.Remove(driver);
        }
        public static void TakipEdenlerControl(this List<User> Liste)
        {
            Liste.Reverse();
            DatabasesContext cx = new();
            Hesap.Instance.Iletisim.Max = Liste.Count;
            int count = 0;
            foreach (var item in Liste)
            {
                if (item.FollowStatus == "Takip et")
                {
                    IWebDriver driver = Drivers.MusaitOlanDriver();
                    Task.Run(() => driver.TakipcilerdenCikar(item));
                    cx.Takipciler.Remove(cx.Takipciler.FirstOrDefault(x => x.Username == item.Username));
                    cx.SaveChanges();
                    Hesap.Instance.Iletisim.CurrentValue = ++count;
                }

            }

        }
        public static IWebElement DetaysızListeGetir(IWebElement element)
        {
            var tak = element.GetProfil();
            Hesap.Instance.Liste.Add(tak);
            Hesap.Instance.Iletisim.CurrentValue = Hesap.Instance.Liste.Count;
            return element;
        }
        public static IWebElement UnfollowEt(IWebElement element)
        {
            User profil = element.GetProfil();
            if (profil.FollowersStatus.StartsWith("Seni") && profil.FollowStatus == "Takip ediliyor")
            {
                //if (otoTakipCik) ;
                Hesap.Instance.GeriTakipEtmeyenler.Add(profil);
            }
            return element;
        }
        public static IWebElement TakipEdilenleriGetir(IWebElement element)
        {

            DatabasesContext context = SettingsMethod.TakipEdilenleriGetir.Context;
            string username = element.GetUserName();
            User takipci = context.TakipEdilenler.FirstOrDefault(x => x.Username == username);
            if (takipci == null || !SettingsMethod.TakipEdilenleriGetir.UseDB)
            {
                var driver = Drivers.MusaitOlanDriver();
                Thread baslat = new(new ThreadStart(() =>
                {
                    using DatabasesContext context2 = new();
                    takipci = driver.GetProfil(username);
                    Hesap.Instance.TakipEdilenler.Add(takipci);
                    context2.TakipEdilenler.Add((TakipEdilen)takipci);
                    context2.SaveChanges();
                    Hesap.Instance.Iletisim.CurrentValue = Hesap.Instance.TakipEdilenler.Count;
                }));
                baslat.Start();
            }
            else Hesap.Instance.TakipEdilenler.Add(takipci);
            return element;
        }
        public static void ListeGezici(Func<IWebElement, IWebElement> func, string link, int progressMAX = 1,int sayfaLoadWait_MS = 1000)
        {
            Hesap.Instance.Iletisim.Max = progressMAX;
            IWebDriver driverr = Yardimci.ProfileGitWithMainDriver(link, sayfaLoadWait_MS);
            List<IWebElement> kontrolEdildi = new();
            while (!driverr.IsSayfaSonu())
            {
                var oncekilist = driverr.FindElements(By.CssSelector("[data-testid=UserCell]"));
                var list = oncekilist.Except(kontrolEdildi);
                foreach (var x in list)
                {
                    kontrolEdildi.Add(func(x));
                }
            }
        }

    }
}
