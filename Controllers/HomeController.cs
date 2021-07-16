using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        [HttpGet]
        public IActionResult Index()
        {
            if (Hesap.Takipciler.Count != 0)
                return View(Hesap.Takipciler);
            List<User> user = new List<User>();
            return View(user);
        }
        [HttpPost]
        public string TakipCik(string Usernames)
        {


            List<Task> tasks = new List<Task>();
            string butontext = "";
            var takiptenCikilicaklar = Usernames?.Split('@');
            foreach (var item in takiptenCikilicaklar)
            {
                IWebDriver driver = Drivers.MusaitOlanDriver();
                Task t1 = Task.Run(() =>
                {
                    driver.Navigate().GoToUrl("https://mobile.twitter.com/" + item);
                    butontext = driver.profilUserButtonClick();
                    if (butontext != null)
                    {
                        using (var context = new DatabasesContext())
                        {
                            var result = context.Users.SingleOrDefault(b => b.Username == item);
                            if (result != null) context.Users.Remove(result);
                            context.SaveChanges();
                        }

                    }
                });
                tasks.Add(t1);
                Drivers.kullanıyorum.Remove(driver);
            }

            Task.WaitAll(tasks.ToArray());
            return null;

        }
        [HttpGet]

        public IActionResult TakipciList()
        {
            DateTime baslangic = DateTime.Now;
            ViewBag.gecensure = 0;

            if (Hesap.Takipciler.Count != 0)
                return View("Index", Hesap.Takipciler);
            DatabasesContext context = new DatabasesContext();

            if (context.Users.Count() == Hesap.OturumBilgileri.Following)
            {
                Hesap.Takipciler.AddRange(context.Users);
                return View("Index", Hesap.Takipciler);
            }




            IWebDriver driverr = Drivers.Driver;
            driverr.Navigate().GoToUrl("https://mobile.twitter.com/" + Hesap.OturumBilgileri.Username + "/following");
            driverr.WaitForLoad();

            List<string> kontrolEdildi = new List<string>();
            while (!driverr.isSayfaSonu() || Hesap.Takipciler.Count < Hesap.OturumBilgileri.Following - 10)
            {
                var listelenenKullanicilar = driverr.FindElements(By.CssSelector("[data-testid=UserCell]")).Select(x =>
                {
                    try
                    {
                        var y = x.FindElement(By.TagName("a")).GetAttribute("href");
                        y = y.Substring(y.LastIndexOf('/') + 1);
                        return y;
                    }
                    catch (Exception) { return null; }
                }).ToList();
                var kontrolEdilecekler = listelenenKullanicilar.Except(kontrolEdildi).ToList();
                kontrolEdilecekler.RemoveAll(x => x == null);
                foreach (string username in kontrolEdilecekler)
                {
                    
                    User takipci = context.Users.FirstOrDefault(x => x.Username == username);
                    if (takipci == null)
                    {
                        var driver = Drivers.MusaitOlanDriver();
                        Task.Run(() =>
                        {
                            using (DatabasesContext context2 = new DatabasesContext())
                            {
                                takipci = driver.getProfil(username, false);
                                context2.Users.Add(takipci);
                                Hesap.Takipciler.Add(takipci);
                                context2.SaveChanges();
                            }
                        });
                    }
                    else Hesap.Takipciler.Add(takipci);
                    kontrolEdildi.Add(username);
                }
            }
            ViewBag.gecensure = (DateTime.Now - baslangic).TotalMinutes;
            return View("Index", Hesap.Takipciler);
        }

        [HttpGet]
        public JsonResult GuncelleProgress()
        {
            Hesap.Iletisim.veri = 100 * Hesap.Takipciler.Count / Hesap.OturumBilgileri.Following;
            var yedek = Hesap.Iletisim;
            return Json(JsonConvert.SerializeObject(yedek));
        }
    }
}
/*  || !Yardimci.isPage(takipci.PhotoURL)
             if (takipci != null)
             { context.Users.Remove(takipci); context.SaveChanges(); }*/