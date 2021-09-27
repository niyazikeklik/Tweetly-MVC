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

        public static object ListeGezici(string link,  int sayfaLoadWait_MS = 1000)
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


    }
}
