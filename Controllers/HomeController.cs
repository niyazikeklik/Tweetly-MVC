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
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Controllers
{
    public class HomeController : Controller
    {

        [HttpPost]
        public string TakipCik(string Usernames)
        {
            string butontext = "";
            var takiptenCikilicaklar = Usernames?.Split('@');
            if (takiptenCikilicaklar is null) return null;
            foreach (var item in takiptenCikilicaklar)
            {
                if (string.IsNullOrEmpty(item)) continue;
                IWebDriver driver = Drivers.MusaitOlanDriver();
                driver.Navigate().GoToUrl("https://mobile.twitter.com/" + item);
                Task.Run(() =>
                {
                    butontext = driver.ProfilUserButonClick();
                    Drivers.kullanıyorum.Remove(driver);
                });
            }
            return butontext;
        }
        [HttpGet]
        public JsonResult GuncelleProgress()
        {
            Hesap.Instance.Iletisim.veri = "Bulunan Kullanıcı Sayısı: " + Hesap.Instance.Liste.Count;
            var yedek = Hesap.Instance.Iletisim;
            return Json(JsonConvert.SerializeObject(yedek));
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.sutunGizle = false;
            List<User> x = new();
            x.AddRange(new DatabasesContext().Records);
            return View(x);
        }
        [HttpGet]
        public IActionResult ListGetir(string username, string listName)
        {
            DatabasesContext context = new();
            ViewBag.sutunGizle = !Hesap.Instance.Settings.CheckDetayGetir;

            if (Hesap.Instance.Settings.CheckClearDB)
                context.RecordsAllDelete();

            Eylemler.ListeGezici($"https://twitter.com/{username}/{listName}");

            if (Hesap.Instance.Settings.CheckDetayGetir)
                context.RecordsUpdate(Hesap.Instance.Liste);


            return View("Index", Hesap.Instance.Liste);
        }
        [HttpPost]
        public ActionResult SettingSave(string Model)
        {
            Setting ayarlar = JsonConvert.DeserializeObject<Setting>(Model);
            Hesap.Instance.Settings = ayarlar;
            return Content("Başarılı!");
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
