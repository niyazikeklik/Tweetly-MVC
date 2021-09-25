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
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async void Prog()
        {
            Hubs.ProgressBarHub hubs = new();
            for (int i = 0; i < 100; i++)
                await hubs.ProgressBar(i);
            
        }
        [HttpPost]
        public string TakipCik(string Usernames)
        {
            List<Task> tasks = new();
            string butontext = "";
            var takiptenCikilicaklar = Usernames?.Split('@');
            if (takiptenCikilicaklar is null) return null;
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
            List<User> list = Yardimci.BaseToSub<List<User>>(new DatabasesContext().TakipEdilenler);
            return View(list);
        }

        [HttpGet]
        public IActionResult ListGetir(string username, bool detay, bool useDB=true, bool clearDB=false, string listName = "followers")
        {

            ViewBag.sutunGizle = !detay;
            var Liste = Eylemler.ListeGezici(
              Func: Eylemler.ListeGetir,
              link: $"https://twitter.com/{username}/{listName}",
              progressMAX: Hesap.Instance.OturumBilgileri.Followers,
              detay: detay
              );
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
