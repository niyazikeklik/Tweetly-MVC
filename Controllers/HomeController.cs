using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
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
        private readonly ILogger<HomeController> _logger;
        int toplam = 861;
        int count = 0;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<User> bos = new List<User>();
            return View(bos);
        }

        public IActionResult Indexx()
        {
            IWebDriver driverr = Drivers.Driver;

            string link = "https://mobile.twitter.com/alienationxs/following";
            driverr.Navigate().GoToUrl(link);
            Thread.Sleep(1500);
            List<User> takipciler = new List<User>();
            List<IWebElement> kontrolEdildi = new List<IWebElement>();

            while (!Yardimci.isSayfaSonu(driverr))
            {
                var listelenenKullanicilar = driverr.FindElements(By.CssSelector("[data-testid=UserCell]"));
                var kontrolEdilecekler = listelenenKullanicilar.Except(kontrolEdildi);

                foreach (var item in kontrolEdilecekler)
                {
                    try
                    {
                        string followingUserName = item.FindElements(By.TagName("a"))[1].GetAttribute("href");
                        followingUserName = followingUserName.Substring(followingUserName.LastIndexOf('/') + 1);
                        IWebDriver calismadriver = Drivers.MusaitOlanDriver();
                        Thread baslat = new Thread(new ThreadStart(() =>
                        {
                           
                            CreateUser profil = new CreateUser();
                            takipciler.Add(profil.getProfil(calismadriver, followingUserName, false));
                        }));
                        baslat.Start();
                    }
                    catch (Exception) { continue; }
                    count++;
                }
                kontrolEdildi.AddRange(kontrolEdilecekler);
            }
            /*
             800 125
             100 x
             */
            int countt = 1;
            takipciler.All(x => { x.Count = countt++; return true; });
            // var json = JsonConvert.SerializeObject(takipciler);
            return View("Index", takipciler);
        }
        [HttpPost]
        public int GuncelleProgress()
        {
            return (100 * count) / toplam;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
