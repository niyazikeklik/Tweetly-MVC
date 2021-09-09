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
        public IActionResult Yenile()
        {
            DatabasesContext context = new DatabasesContext();
            return View(context.Users);
        }
        public IActionResult Index()
        {
            ViewBag.sutunGizle = false;
            if (Hesap.Instance.Takipciler.Count != 0)
                return View(Hesap.Instance.Takipciler);
            List<User> user = new List<User>();
            return View(user);
        }
        public string TakipCik(string Usernames)
        {
            List<Task> tasks = new List<Task>();
            string butontext = "";
            var takiptenCikilicaklar = Usernames?.Split('@');
            foreach (var item in takiptenCikilicaklar)
            {
                if (string.IsNullOrEmpty(item)) continue;
                IWebDriver driver = Drivers.MusaitOlanDriver();
                driver.Navigate().GoToUrl("https://mobile.twitter.com/" + item);
                Task t1 = Task.Run(() =>
                {
                    butontext = driver.profilUserButtonClick();
                    if (butontext != null)
                    {
                        using (var context = new DatabasesContext())
                        {
                            var result = context.Users.SingleOrDefault(b => b.Username == item);
                            if (result != null) context.Users.Remove(result);
                            context.SaveChanges();
                            Drivers.kullanıyorum.Remove(driver);
                        }
                    }
                });
                tasks.Add(t1);
            }
            Task.WaitAll(tasks.ToArray());
            return butontext;

        }
        public IActionResult TakipciList(string username, string liste = "following", bool clearDB = false, bool useDB = true)
        {
            ViewBag.sutunGizle = false;
            Hesap.Instance.Iletisim.tarih = DateTime.Now;
            DatabasesContext context = new DatabasesContext();

            if (clearDB)
            {
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();
            }
            if (Hesap.Instance.Takipciler.Count != 0)
                return View("Index", Hesap.Instance.Takipciler);
            else if (context.Users.Count() == Hesap.Instance.OturumBilgileri.Following)
            {
                Hesap.Instance.Takipciler.AddRange(context.Users);
                return View("Index", Hesap.Instance.Takipciler);
            }


            IWebDriver driverr = Drivers.Driver;
            driverr.Navigate().GoToUrl("https://mobile.twitter.com/" + username + "/" + liste);
            driverr.WaitForLoad();
            List<string> kontrolEdildi = new List<string>();
            while (!driverr.isSayfaSonu() || Hesap.Instance.Takipciler.Count < Hesap.Instance.OturumBilgileri.Following - 10)
            {
                var listelenenKullanicilar = (List<string>)driverr.FindElements(By.CssSelector("[data-testid=UserCell]")).Select(x =>
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
                foreach (string item in kontrolEdilecekler)
                {
                    User takipci = context.Users.FirstOrDefault(x => x.Username == item);
                    if (takipci == null || !useDB)
                    {
                        var driver = Drivers.MusaitOlanDriver();
                        Thread baslat = new Thread(new ThreadStart(() =>
                        {
                            using (DatabasesContext context2 = new DatabasesContext())
                            {
                                takipci = driver.getProfil(item);
                                Hesap.Instance.Takipciler.Add(takipci);
                                context2.Users.Add(takipci);
                                context2.SaveChanges();
                            }
                        }));
                        baslat.Start();
                    }
                    else Hesap.Instance.Takipciler.Add(takipci);
                    kontrolEdildi.Add(item);
                }
            }
            context.Users.RemoveRange(context.Users);
            context.Users.AddRange(Hesap.Instance.Takipciler);
            return View("Index", Hesap.Instance.Takipciler);
        }
        public JsonResult GuncelleProgress()
        {
            Hesap.Instance.Iletisim.sure = Math.Round(((DateTime.Now) - Hesap.Instance.Iletisim.tarih).TotalMinutes, 0) + " Count: " + Hesap.Instance.Takipciler.Count;
            Hesap.Instance.Iletisim.veri = 100 * Hesap.Instance.Takipciler.Count / Hesap.Instance.OturumBilgileri.Following;
            var yedek = Hesap.Instance.Iletisim;
            return Json(JsonConvert.SerializeObject(yedek));
        }
        public IActionResult UnfollowEt(bool otoTakipCik = false)
        {
            Hesap.Instance.Iletisim.tarih = DateTime.Now;
            IWebDriver driverr = Drivers.Driver;
            driverr.Navigate().GoToUrl("https://mobile.twitter.com/" + Hesap.Instance.OturumBilgileri.Username + "/following");
            driverr.WaitForLoad();
            List<string> kontrolEdildi = new List<string>();
            while (!driverr.isSayfaSonu())
            {
                var kontrolEdilecekler = driverr.FindElements(By.CssSelector("[data-testid=UserCell]")).Select(x =>
                {
                    foreach (var item in kontrolEdildi)
                        if (x.Text.Contains(item))
                            return null;
                    return x;
                }).ToList();
                foreach (var item in kontrolEdilecekler)
                {
                    User profil = item.getProfil();
                    if (profil.FollowersStatus.StartsWith("Seni") && profil.FollowStatus == "Takip ediliyor")
                    {
                        if (otoTakipCik);
                        Hesap.Instance.GeriTakipEtmeyenler.Add(profil);
                    }
                    kontrolEdildi.Add(profil.Username);
                }
            }
            ViewBag.sutunGizle = true;
            // geri takip etmeyenleri veritabanına kaydet.
            return View("Index", Hesap.Instance.GeriTakipEtmeyenler);
        }
        public IActionResult ListGetir(string listName = "following")
        {
            List<User> liste = new List<User>();
            IWebDriver driverr = Drivers.Driver;
            driverr.Navigate().GoToUrl("https://mobile.twitter.com/" + Hesap.Instance.OturumBilgileri.Username + "/" + listName);
            driverr.WaitForLoad();
            List<string> kontrolEdildi = new List<string>();
            while (!driverr.isSayfaSonu())
            {
                var kontrolEdilecekler = driverr.FindElements(By.CssSelector("[data-testid=UserCell]")).Select(x =>
                {
                    foreach (var item in kontrolEdildi)
                        if (x.Text.Contains(item))
                            return null;
                    return x;
                }).ToList();
                foreach (var item in kontrolEdilecekler)
                    liste.Add(item.getProfil());
            }
            ViewBag.sutunGizle = true;
            return View("Index", liste);
        }
    }
}
/*  || !Yardimci.isPage(takipci.PhotoURL)
             if (takipci != null)
             { context.Users.Remove(takipci); context.SaveChanges(); }*/


