using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Tweetly_MVC.Init;
using Tweetly_MVC.Models;
using Tweetly_MVC.Tweetly;
using System.Net;
using System.IO;
using Tweetly_MVC.Cloud;

namespace Tweetly_MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public string TakipCik(string Usernames)
        {

            string butontext = "";
            string[] takiptenCikilicaklar = Usernames?.Split('@');
            if (takiptenCikilicaklar is null) return null;
            foreach (string item in takiptenCikilicaklar)
            {
                if (string.IsNullOrEmpty(item)) continue;
                IWebDriver driver = Drivers.MusaitOlanDriver();
                driver.LinkeGit("https://mobile.twitter.com/" + item, false);
                if (driver.WaitForProfilBilgileri())
                    butontext = driver.ProfilUserButonClick();
            }
            return butontext;
        }

        /* [HttpPost]
         public void GuncelleProgress()
         {
             Repo.Ins.Iletisim.BilgiMetni = Repo.Ins.Iletisim.HataMetni + "Bulunan Kullanıcı Sayısı: " + Repo.Ins.Iletisim.currentValue;
             ILetisim yedek = Repo.Ins.Iletisim;
             return Json(JsonConvert.SerializeObject(yedek));
         }*/

        [HttpGet]
        public JsonResult GuncelleProgress()
        {
            Repo.Ins.Iletisim.BilgiMetni = Repo.Ins.Iletisim.HataMetni + "Bulunan Kullanıcı Sayısı: " + Repo.Ins.Iletisim.currentValue;
            ILetisim yedek = Repo.Ins.Iletisim;
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
        public ViewResult ListGetir(string username, string listName)
        {
            DatabasesContext context = new();
            ViewBag.sutunGizle = !Repo.Ins.UserPrefs.CheckDetayGetir;
            //if (Repo.Ins.Liste.Count > 0) return View("Index", Repo.Ins.Liste);
            if (Repo.Ins.UserPrefs.CheckClearDB)
                context.RecordsAllDelete();

            Repo.Ins.Liste = CreateUser.ListeGezici($"https://mobile.twitter.com/{username}/{listName}");
            DetectGender.WriteGenderJson();
            if (Repo.Ins.UserPrefs.CheckDetayGetir)
                context.RecordsUpdateOrAdd(Repo.Ins.Liste);
            else context.UpdateTemelBilgiler(Repo.Ins.Liste);

            return View("Index", Repo.Ins.Liste);
        }

        [HttpGet]
        public ViewResult BegenenleriGetir(string username)
        {
            DatabasesContext context = new();
            ViewBag.sutunGizle = !Repo.Ins.UserPrefs.CheckDetayGetir;
            if (Repo.Ins.Begenenler.Count > 0) return View("Index", Repo.Ins.Begenenler);
            if (Repo.Ins.UserPrefs.CheckClearDB)
                context.RecordsAllDelete();

            Repo.Ins.Begenenler = CreateUser.BegenenleriGetir(username);
            DetectGender.WriteGenderJson();

            if (Repo.Ins.UserPrefs.CheckDetayGetir)
                context.RecordsUpdateOrAdd(Repo.Ins.Begenenler);

            return View("Index", Repo.Ins.Begenenler);
        }

        [HttpPost]
        public ActionResult SettingSave(string Model)
        {
            FinderSettings ayarlar = JsonConvert.DeserializeObject<FinderSettings>(Model);
            Repo.Ins.UserPrefs = ayarlar;
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