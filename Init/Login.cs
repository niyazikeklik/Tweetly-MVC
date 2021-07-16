using Microsoft.Edge.SeleniumTools;
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
            driver.WaitForLoad();
            Thread.Sleep(1000);
            if (driver.Url.Contains("flow"))
            {
                driver.getElement(By.Name("username")).SendKeys(ka + Keys.Enter);
                driver.getElement(By.Name("password")).SendKeys(ps + Keys.Enter);
            }
            else if(driver.Url.Contains("login"))
            {
                tekrar:
                try
                {
                    driver.getElement(By.Name("session[username_or_email]"))?.Clear();
                    driver.getElement(By.Name("session[username_or_email]"))?.SendKeys(ka);
                    driver.getElement(By.Name("session[password]"))?.Clear();
                    driver.getElement(By.Name("session[password]"))?.SendKeys(ps + Keys.Enter);
                }
                catch (Exception)
                {
                    goto tekrar;
                } 

                Thread.Sleep(1000);
                if (driver.Url.Contains("redirect_after_login"))
                    Giris("niyazikeklik@gmail.com", Hesap.loginPass, driver);
            }
            else if (driver.Url.Contains("home"))
            {
                ;
            }

        }
        static int count = 10;
        private static IWebDriver optionDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
            chromeOptions.AddArgument("test-type");
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("no-sandbox");
            chromeOptions.AddArgument("disable-infobars");
            chromeOptions.AddArgument("--window-size=400,820");
            chromeOptions.AddArgument("user-data-dir=C:/Users/niyazi/AppData/Local/Google/Chrome/User Data/Profile " + count++);
            chromeOptions.AddArgument("--headless");
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
            Task g1 = Task.Run(() => optionDriver()).ContinueWith(x =>
            {
                Drivers.Driver = x.Result;
                Hesap.OturumBilgileri = Drivers.Driver.getProfil(Hesap.loginUserName, false);
            });
            Task g2 = Task.Run(() => optionDriver()).ContinueWith(x => Drivers.Driver2 = x.Result);
            Task g3 = Task.Run(() => optionDriver()).ContinueWith(x => Drivers.Driver3 = x.Result);
            Task g4 = Task.Run(() => optionDriver()).ContinueWith(x => Drivers.Driver4 = x.Result);
            Task g5 = Task.Run(() => optionDriver()).ContinueWith(x => Drivers.Driver5 = x.Result);

            Task.WaitAll(new Task[] { g1, g2, g3, g4, g5 });
            return Task.CompletedTask;
        }

    }
}
