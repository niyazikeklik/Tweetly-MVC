using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public class Login
    {
        private static void Giris(string ka, string ps, IWebDriver driver)
        {
            Yardimci.WaitForLoad(driver);
            Thread.Sleep(1000);
            if (driver.Url.Contains("flow"))
            {
                driver.FindElement(By.Name("username")).SendKeys(ka + Keys.Enter);
                Thread.Sleep(2500);
                driver.FindElement(By.Name("password")).SendKeys(ps + Keys.Enter);
                while (driver.Url.Contains("login")) Thread.Sleep(500);
                

            }
            else
            {
            tekrar:
                try
                {
                    driver.FindElement(By.Name("session[username_or_email]")).Clear();
                    driver.FindElement(By.Name("session[username_or_email]")).SendKeys(ka);
                    driver.FindElement(By.Name("session[password]")).Clear();
                    driver.FindElement(By.Name("session[password]")).SendKeys(ps + Keys.Enter);
                    Thread.Sleep(1000);
                    if (driver.Url.Contains("redirect_after_login"))
                        Giris("niyazikeklik@gmail.com", "Galatasaray1453", driver);
                }
                catch (Exception)
                {

                    goto tekrar;
                }
            }

        }

        private static IWebDriver optionDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //   chromeOptions.AddArgument("--headless");
            chromeOptions.EnableMobileEmulation("Pixel 2 XL");
            service.HideCommandPromptWindow = true;


            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            driver.Manage().Window.Size = new Size(400, 820);
            driver.Navigate().GoToUrl("https://mobile.twitter.com/login");

            Giris(Hesap.loginUserName, Hesap.loginPass, driver);

            return driver;
        }

        public static Task CreateDrivers()
        {
            List<Task> gorevler = new List<Task>();
            for (int i = 0; i < Drivers.drivers.Length; i++)
            {
                Task gorev = Task.Run(() =>
                   Drivers.drivers[i] = optionDriver()
                );
                gorevler.Add(gorev);
            }
            Task.WaitAll(gorevler.ToArray());
            return Task.CompletedTask;
        }

    }
}
