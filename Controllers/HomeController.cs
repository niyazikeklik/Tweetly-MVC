using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Init;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            if (Hesap.Takipciler.Count == 0)
                return View(Hesap.Takipciler);
       

            List<User> user = new List<User>();
            return View(user);
        }

        public IActionResult TakipciList()
        {
            DatabasesContext context = new DatabasesContext();
            if (Hesap.Takipciler.Count == 0) return View("Index", Hesap.Takipciler);


            IWebDriver driverr = Drivers.Driver;

            string link = "https://mobile.twitter.com/" + Hesap.OturumBilgileri.Username + "/following";
            driverr.Navigate().GoToUrl(link);
            driverr.WaitForLoad();
            var kontrolEdildi = new List<IWebElement>();

            while (!driverr.isSayfaSonu() || Hesap.Takipciler.Count < Hesap.OturumBilgileri.Following - 10)
            {
                var listelenenKullanicilar = driverr.FindElements(By.CssSelector("[data-testid=UserCell]"));
                var kontrolEdilecekler = listelenenKullanicilar.Except(kontrolEdildi);
                foreach (var item in kontrolEdilecekler)
                {
                    try
                    {

                        string followingUserName = item.FindElements(By.TagName("a"))[1].GetAttribute("href");
                        followingUserName = followingUserName.Substring(followingUserName.LastIndexOf('/') + 1);

                        User takipci = context.Users.FirstOrDefault(x => x.Username == followingUserName);

                        if (takipci == null)
                        {
                            var driver = Drivers.MusaitOlanDriver();
                            Task.Run(() =>
                            {
                                takipci = driver.getProfil(followingUserName, false);
                                context.Users.Add(takipci);

                                takipci.Count = Hesap.Takipciler.Count + 1;
                                Hesap.Takipciler.Add(takipci);

                            });
                        }
                        else Hesap.Takipciler.Add(takipci);
                    }
                    catch (Exception) { continue; }
                }
                kontrolEdildi.AddRange(kontrolEdilecekler);
                context.SaveChanges();
            }
            return View("Index", Hesap.Takipciler);
        }


        [HttpGet]
        public JsonResult GuncelleProgress()
        {
            Hesap.Iletisim.veri = (100 * Hesap.Takipciler.Count) / Hesap.OturumBilgileri.Following;
            return Json(Hesap.Iletisim);
        }
    }
}
