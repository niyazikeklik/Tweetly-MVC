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
        [HttpPost]
        public string TakipCik(string Usernames)
        {
            List<Task> tasks = new();
            string butontext = "";
            var takiptenCikilicaklar = Usernames?.Split('@');
            foreach (var item in takiptenCikilicaklar)
            {
                if (string.IsNullOrEmpty(item)) continue;
                IWebDriver driver = Drivers.MusaitOlanDriver();
                driver.Navigate().GoToUrl("https://mobile.twitter.com/" + item);
                Task t1 = Task.Run(() =>
                {
                    butontext = driver.ProfilUserButonClick();
                    if (butontext != null)
                    {
                        using var context = new DatabasesContext();
                        var result = context.TakipEdilenler.SingleOrDefault(b => b.Username == item);
                        if (result != null) context.TakipEdilenler.Remove(result);
                        context.SaveChanges();
                        Drivers.kullanıyorum.Remove(driver);
                    }
                });
                tasks.Add(t1);
            }
            Task.WaitAll(tasks.ToArray());
            return butontext;
        }
        [HttpGet]
        public JsonResult GuncelleProgress()
        {
            Hesap.Instance.Iletisim.sure = Math.Round(((DateTime.Now) - Hesap.Instance.Iletisim.tarih).TotalMinutes, 0) + " Count: " + Hesap.Instance.TakipEdilenler.Count;
            Hesap.Instance.Iletisim.veri = 100 * (Hesap.Instance.Iletisim.CurrentValue + 1) / (Hesap.Instance.Iletisim.Max + 1);
            var yedek = Hesap.Instance.Iletisim;
            return Json(JsonConvert.SerializeObject(yedek));
        }
        [HttpGet]
        public IActionResult Yenile()
        {
            DatabasesContext context = new();
            return View(context.TakipEdilenler);
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.sutunGizle = false;
            if (Hesap.Instance.TakipEdilenler.Count != 0)
                return View(Hesap.Instance.TakipEdilenler);
            List<User> list = new();
            return View(list);
        }
        [HttpGet]
        public IActionResult TakipciList(string username, string liste = "following", bool clearDB = false, bool useDB = true)
        {
            DatabasesContext context = new();
            SettingsMethod.TakipEdilenleriGetir.UseDB = useDB;
            SettingsMethod.TakipEdilenleriGetir.Context = context;
            ViewBag.sutunGizle = false;

            if (clearDB)
                context.TakipEdilenler.RemoveRange(context.TakipEdilenler);
            context.SaveChanges();
            if (Hesap.Instance.TakipEdilenler.Any())
                return View("Index", Hesap.Instance.TakipEdilenler);
            if (context.TakipEdilenler.Count() == Hesap.Instance.OturumBilgileri.Following)
            {
                Hesap.Instance.TakipEdilenler.AddRange(context.TakipEdilenler);
                return View("Index", Hesap.Instance.TakipEdilenler);
            }

            Eylemler.ListeGezici(Eylemler.TakipEdilenleriGetir, $"https://mobile.twitter.com/{username}/{liste}",Hesap.Instance.OturumBilgileri.Following);

            context.TakipEdilenler.RemoveRange(context.TakipEdilenler);
            context.TakipEdilenler.AddRange(Yardimci.BaseToSub<List<TakipEdilen>>(Hesap.Instance.TakipEdilenler.DistinctByUserName()));

            return View("Index", Hesap.Instance.TakipEdilenler);
        }
        [HttpGet]
        public IActionResult UnfollowEt(bool otoTakipCik = false)
        {
            Eylemler.ListeGezici(Eylemler.UnfollowEt, $"https://mobile.twitter.com/{Hesap.Instance.OturumBilgileri.Username}/following", Hesap.Instance.OturumBilgileri.Following);
            ViewBag.sutunGizle = true;
            DatabasesContext context = new();
            Hesap.Instance.GeriTakipEtmeyenler.AddRange(context.GeriTakipEtmeyenler);
            context.GeriTakipEtmeyenler.RemoveRange(context.GeriTakipEtmeyenler);
            context.GeriTakipEtmeyenler.AddRange(Yardimci.BaseToSub<List<GeriTakipEtmeyen>>(Hesap.Instance.GeriTakipEtmeyenler.DistinctByUserName()));
            context.SaveChanges();
            return View("Index", Hesap.Instance.GeriTakipEtmeyenler);
        }
        [HttpGet]
        public IActionResult ListGetir(string listName = "followers")
        {
            ViewBag.sutunGizle = true;
            DatabasesContext context = new();
            Eylemler.ListeGezici(Eylemler.DetaysızListeGetir, $"https://twitter.com/{Hesap.Instance.OturumBilgileri.Username}/{listName}", Hesap.Instance.OturumBilgileri.Followers);
            context.Takipciler.AddRange(Yardimci.BaseToSub<List<Takipci>>(Hesap.Instance.Liste.DistinctByUserName()));
            context.SaveChanges();

            return View("Index", Hesap.Instance.Liste);
        }
    }
}
/*  || !Yardimci.isPage(takipci.PhotoURL)
             if (takipci != null)
             { context.Users.Remove(takipci); context.SaveChanges(); }*/



/*   if (context.Takipciler.Any())
   {
       var x = Yardimci.BaseToSubClassConverter<List<User>>(context.Takipciler);
       x.TakipEdenlerControl();
       return View("Index", x);
   }
   if (Hesap.Instance.Liste.Count != 0)
       return View(Hesap.Instance.Liste);*/
