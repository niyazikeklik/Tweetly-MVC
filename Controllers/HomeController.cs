using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Tweetly_MVC.Init;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {

            if (Hesap.Takipciler != null) return View(Hesap.Takipciler);

            Hesap.OturumBilgileri = CreateUser.getProfil(Drivers.Driver, Hesap.loginUserName, false);
            List<User> user = new List<User>();
            return View(user);
        }

        public IActionResult TakipciList()
        {
            if (Hesap.Takipciler != null) return View("Index", Hesap.Takipciler);
            Yardimci.count = 0;
            IWebDriver driverr = Drivers.Driver;

            string link = "https://mobile.twitter.com/" + Hesap.OturumBilgileri.Username + "/following";
            driverr.Navigate().GoToUrl(link);
            Thread.Sleep(1500);
            List<User> takipciler = new List<User>();
            List<IWebElement> kontrolEdildi = new List<IWebElement>();
            DatabasesContext context = new DatabasesContext();
            while (!Yardimci.isSayfaSonu(driverr) || takipciler.Count < Hesap.OturumBilgileri.Following - 30)
            {
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> listelenenKullanicilar = driverr.FindElements(By.CssSelector("[data-testid=UserCell]"));
                IEnumerable<IWebElement> kontrolEdilecekler = listelenenKullanicilar.Except(kontrolEdildi);

                foreach (IWebElement item in kontrolEdilecekler)
                {
                    try
                    {

                        string followingUserName = item.FindElements(By.TagName("a"))[1].GetAttribute("href");
                        followingUserName = followingUserName.Substring(followingUserName.LastIndexOf('/') + 1);

                        User DBtakipci = context.Users.FirstOrDefault(x => x.Username == followingUserName);

                        if (DBtakipci == null)
                        {
                            IWebDriver calismadriver = Drivers.MusaitOlanDriver();
                            Thread baslat = new Thread(new ThreadStart(() =>
                            {
                                User takipci = CreateUser.getProfil(calismadriver, followingUserName, false);
                                takipciler.Add(takipci);
                                context.Users.Add(takipci);
                            }));
                            baslat.Start();
                        }
                        else
                        {
                            DBtakipci.Cinsiyet = Yardimci.CinsiyetBul(DBtakipci.Name).Result;
                            takipciler.Add(DBtakipci);
                        };
                        Yardimci.count++;
                    }
                    catch (Exception) { continue; }
                }
                kontrolEdildi.AddRange(kontrolEdilecekler);
                context.SaveChanges();
            }
            int countt = 1;
            takipciler.All(x => { x.Count = countt++; return true; });
            // var json = JsonConvert.SerializeObject(takipciler);
            Hesap.Takipciler = takipciler;



            return View("Index", takipciler);
        }


        [HttpGet]
        public JsonResult GuncelleProgress()
        {
            Hesap.Iletisim.veri = (100 * Yardimci.count) / Hesap.OturumBilgileri.Following;
            return Json(Hesap.Iletisim);
        }
    }
}
