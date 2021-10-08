using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Collections.Generic;
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
            string[] takiptenCikilicaklar = Usernames?.Split('@');
            if (takiptenCikilicaklar is null) return null;
            foreach (string item in takiptenCikilicaklar)
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
            Hesap.Instance.Iletisim.BilgiMetni = "Bulunan Kullanıcı Sayısı: " + Hesap.Instance.Iletisim.currentValue;
            IILetisim yedek = Hesap.Instance.Iletisim;
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
        public IActionResult BegenenleriGetir(string username, int kontrolEdilecekTweetSayisi)
        {
            var r = CreateUser.BegenenleriGetir(username, kontrolEdilecekTweetSayisi);
            return View("Index", r);
        }

        [HttpGet]
        public IActionResult ListGetir(string username, string listName)
        {
            DatabasesContext context = new();
            ViewBag.sutunGizle = !Hesap.Instance.SettingsFinder.CheckDetayGetir;
            if (Hesap.Instance.Liste.Count > 0) return View("Index", Hesap.Instance.Liste);
            if (Hesap.Instance.SettingsFinder.CheckClearDB)
                context.RecordsAllDelete();

            Hesap.Instance.Liste = CreateUser.ListeGezici($"https://mobile.twitter.com/{username}/{listName}");

            if (Hesap.Instance.SettingsFinder.CheckDetayGetir)
                context.RecordsUpdateOrAdd(Hesap.Instance.Liste);

            return View("Index", Hesap.Instance.Liste);
        }

        [HttpPost]
        public ActionResult SettingSave(string Model)
        {
            FinderSettings ayarlar = JsonConvert.DeserializeObject<FinderSettings>(Model);
            Hesap.Instance.SettingsFinder = ayarlar;
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