using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Models;
using Tweetly_MVC.Init;
using OpenQA.Selenium;
using System.Threading;

namespace Tweetly_MVC.Init
{
    public static class Eylemler
    {
        public static void TakipcilerdenCikar(this IWebDriver driver, User profil)
        {
            string link = "https://twitter.com/" + profil.Username;
            driver.Navigate().GoToUrl(link);
            driver.Control(profil.Username, link);

            if (profil.FollowStatus == "Takip et")
            {
                driver.profilEngelle();
                if (driver.getfollowStatus().StartsWith("Engel"))
                    driver.profilUserButonClick();
                // Engeli kaldır.
            }
            Drivers.kullanıyorum.Remove(driver);
        }
        public static void TakipEdenlerControl(this List<User> Liste)
        {
            Liste.Reverse();
            DatabasesContext cx = new DatabasesContext();
            Hesap.Instance.Iletisim.Max = Liste.Count;
            int count = 0;
            foreach (var item in Liste)
            {
                IWebDriver driver = Drivers.MusaitOlanDriver();
                Task.Run(() => driver.TakipcilerdenCikar(item));
                cx.Takipciler.Remove(cx.Takipciler.FirstOrDefault(x => x.Username == item.Username));
                cx.SaveChanges();
                Hesap.Instance.Iletisim.CurrentValue = ++count;
            }


        }
    }
}
