using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Init;

namespace Tweetly_MVC.Tweetly
{
    public class Drivers
    {
        public static IWebDriver Driver { get; set; }
        public static IWebDriver Driver2 { get; set; }
        public static IWebDriver Driver3 { get; set; }
        public static IWebDriver Driver4 { get; set; }
        public static IWebDriver Driver5 { get; set; }

        private static int count = 15;
        public static IWebDriver OptionDriver()
        {
            ChromeOptions chromeOptions = new();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
            chromeOptions.AddArgument("test-type");
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("no-sandbox");
            chromeOptions.AddArgument("disable-infobars");
            chromeOptions.AddArgument("--window-size=400,820");
            chromeOptions.AddArgument("user-data-dir=C:/Users/niyazi/AppData/Local/Google/Chrome/User Data/Profile " + count++);
         //  chromeOptions.AddArgument("--headless");
            chromeOptions.EnableMobileEmulation("Pixel 2 XL");
            service.HideCommandPromptWindow = true;


            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            driver.Manage().Window.Size = new Size(400, 820);
            driver.Navigate().GoToUrl("https://mobile.twitter.com/login");
            Login.Giris(Hesap.Instance.LoginUserName, Hesap.Instance.LoginPass, driver);

            return driver;
        }

        public readonly static List<IWebDriver> kullanıyorum = new();
        public static IWebDriver MusaitOlanDriver()
        {
            IWebDriver[] driverss = { Driver2, Driver3, Driver4, Driver5 };
            foreach (IWebDriver item in driverss)
            {
                if (!kullanıyorum.Contains(item))
                {
                    kullanıyorum.Add(item);
                    return item;
                }
            }
            Thread.Sleep(500);
            return MusaitOlanDriver();
        }

        public static void CreateDrivers()
        {
            Task g1 = Task.Run(() => OptionDriver()).ContinueWith(x =>
            {
                Drivers.Driver = x.Result;
                Hesap.Instance.OturumBilgileri = Drivers.Driver.GetProfil(Hesap.Instance.LoginUserName);
            });
            Task.Run(() => Drivers.Driver2 = Drivers.OptionDriver());
            Task.Run(() => Drivers.Driver3 = Drivers.OptionDriver());
            Task.Run(() => Drivers.Driver4 = Drivers.OptionDriver());
            Task.Run(() => Drivers.Driver5 = Drivers.OptionDriver());
            Task.WaitAny(g1);
        }

    }
}
