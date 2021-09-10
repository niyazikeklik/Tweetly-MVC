using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return View(context.TakipEdilenler);
        }
        public IActionResult Index(List<User> model)
        {
            ViewBag.sutunGizle = false;
            if (Hesap.Instance.TakipEdilenler.Count != 0)
                return View(Hesap.Instance.TakipEdilenler);
            List<User> list = new List<User>();
            return View(list);
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
                    butontext = driver.profilUserButonClick();
                    if (butontext != null)
                    {
                        using (var context = new DatabasesContext())
                        {
                            var result = context.TakipEdilenler.SingleOrDefault(b => b.Username == item);
                            if (result != null) context.TakipEdilenler.Remove(result);
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
            Hesap.Instance.Iletisim.Max = Hesap.Instance.OturumBilgileri.Following;

            if (clearDB)
            {
                context.TakipEdilenler.RemoveRange(context.TakipEdilenler);
                context.SaveChanges();
            }
            if (Hesap.Instance.TakipEdilenler.Count != 0)
                return View("Index", Hesap.Instance.TakipEdilenler);
            else if (context.TakipEdilenler.Count() == Hesap.Instance.OturumBilgileri.Following)
            {
                Hesap.Instance.TakipEdilenler.AddRange(context.TakipEdilenler);
                return View("Index", Hesap.Instance.TakipEdilenler);
            }


            IWebDriver driverr = Drivers.Driver;
            driverr.Navigate().GoToUrl("https://mobile.twitter.com/" + username + "/" + liste);
            driverr.WaitForLoad();
            List<string> kontrolEdildi = new List<string>();
            while (!driverr.isSayfaSonu() || Hesap.Instance.TakipEdilenler.Count < Hesap.Instance.OturumBilgileri.Following - 10)
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
                    User takipci = context.TakipEdilenler.FirstOrDefault(x => x.Username == item);
                    if (takipci == null || !useDB)
                    {
                        var driver = Drivers.MusaitOlanDriver();
                        Thread baslat = new Thread(new ThreadStart(() =>
                        {
                            using (DatabasesContext context2 = new DatabasesContext())
                            {
                                takipci = driver.getProfil(item);
                                Hesap.Instance.TakipEdilenler.Add(takipci);
                                context2.TakipEdilenler.Add((TakipEdilen)takipci);
                                context2.SaveChanges();
                                Hesap.Instance.Iletisim.CurrentValue = Hesap.Instance.TakipEdilenler.Count;
                            }
                        }));
                        baslat.Start();
                    }
                    else Hesap.Instance.TakipEdilenler.Add(takipci);
                    kontrolEdildi.Add(item);
                }
            }
            context.TakipEdilenler.RemoveRange(context.TakipEdilenler);
            context.TakipEdilenler.AddRange(Yardimci.BaseToSubClassConverter<List<TakipEdilen>>(Hesap.Instance.TakipEdilenler));
            return View("Index", Hesap.Instance.TakipEdilenler);
        }
        public JsonResult GuncelleProgress()
        {
            Hesap.Instance.Iletisim.sure = Math.Round(((DateTime.Now) - Hesap.Instance.Iletisim.tarih).TotalMinutes, 0) + " Count: " + Hesap.Instance.TakipEdilenler.Count;
            Hesap.Instance.Iletisim.veri = 100 * (Hesap.Instance.Iletisim.CurrentValue + 1) / (Hesap.Instance.Iletisim.Max + 1);
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
                        if (otoTakipCik) ;
                        Hesap.Instance.GeriTakipEtmeyenler.Add(profil);
                    }
                    kontrolEdildi.Add(profil.Username);
                }
            }
            ViewBag.sutunGizle = true;
            // geri takip etmeyenleri veritabanına kaydet.
            return View("Index", Hesap.Instance.GeriTakipEtmeyenler);
        }
        public IActionResult ListGetir(string listName = "followers")
        {
            ViewBag.sutunGizle = true;
            DatabasesContext context = new DatabasesContext();
            if (context.Takipciler.Count() > 0)
            {
                var x = Yardimci.BaseToSubClassConverter<List<User>>(context.Takipciler);
                x.TakipEdenlerControl();
                return View("Index", x);
            }

            if (Hesap.Instance.Liste.Count != 0)
                return View(Hesap.Instance.Liste);

            Hesap.Instance.Iletisim.Max = Hesap.Instance.OturumBilgileri.Followers;
            IWebDriver driverr = Drivers.Driver;
            driverr.Navigate().GoToUrl("https://mobile.twitter.com/" + Hesap.Instance.OturumBilgileri.Username + "/" + listName);
            driverr.WaitForLoad();
            Thread.Sleep(1000);
            List<IWebElement> kontrolEdildi = new List<IWebElement>();
            while (!driverr.isSayfaSonu())
            {
                var oncekilist = driverr.FindElements(By.CssSelector("[data-testid=UserCell]"));
                var list = oncekilist.Except(kontrolEdildi);
                foreach (var x in list)
                {
                    var tak = x.getProfil();
                    Hesap.Instance.Liste.Add(tak);
                    kontrolEdildi.Add(x);
                    Hesap.Instance.Iletisim.CurrentValue = Hesap.Instance.Liste.Count;
                }
            }

            var listeee = Hesap.Instance.Liste.GroupBy(x => x.Username).Select(y => y.First()).ToList();
            context.Takipciler.AddRange(Yardimci.BaseToSubClassConverter<List<Takipci>>(listeee));

            context.SaveChanges();
            Hesap.Instance.Liste.TakipEdenlerControl();

            ViewBag.sutunGizle = true;
            return View(Hesap.Instance.Liste);
        }
    }
}
/*  || !Yardimci.isPage(takipci.PhotoURL)
             if (takipci != null)
             { context.Users.Remove(takipci); context.SaveChanges(); }*/


