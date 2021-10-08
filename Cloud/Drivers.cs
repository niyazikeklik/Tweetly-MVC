using Microsoft.Extensions.Options;
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

        //ALGORİTMA ASENKRON PROGRAMLAMA İLE YAPILIRSA KULLANILABİLİR
        public static IWebDriver Driver3 { get; set; }
        public static IWebDriver Driver4 { get; set; }
        public static IWebDriver Driver5 { get; set; }

        private static int count = 20;

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
            //chromeOptions.AddArgument("--lang=en");
            chromeOptions.AddArgument("user-data-dir=C:/Users/niyazi/AppData/Local/Google/Chrome/User Data/Profile " + count++);
            chromeOptions.AddArgument("--headless");
            chromeOptions.EnableMobileEmulation("Pixel 2 XL");
            service.HideCommandPromptWindow = true;

            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            driver.Manage().Window.Size = new Size(400, 820);
            driver.LinkeGit("https://mobile.twitter.com/login", 1500);
            Login.Giris(Hesap.Instance.SettingsUser.Username, Hesap.Instance.SettingsUser.Pass, driver);

            return driver;
        }

        public static readonly List<IWebDriver> kullanıyorum = new();

        public static IWebDriver MusaitOlanDriver()
        {
            List<IWebDriver> driverss = 
                Hesap.Instance.SettingsFinder.CheckUseAllDriver ? 
                new() { Driver2, Driver3, Driver4, Driver5 } : 
                new() { Driver2 };

            foreach (IWebDriver item in driverss)
                if (!kullanıyorum.Contains(item))
                {
                    kullanıyorum.Add(item);
                    return item;
                }
            Thread.Sleep(1500);
            return MusaitOlanDriver();
        }

        public static void CreateDrivers()
        {
            Task g1 = Task.Run(() => OptionDriver()).ContinueWith(x =>
            {
                Drivers.Driver = x.Result;
                Hesap.Instance.OturumBilgileri = Drivers.Driver.GetProfil(Hesap.Instance.SettingsUser.Username);
            });
            Task.Run(() => Drivers.Driver2 = Drivers.OptionDriver());
            //ALGORİTMA ASENKRON PROGRAMLAMA İLE YAPILIRSA KULLANILABİLİR
            Task.Run(() => Drivers.Driver3 = Drivers.OptionDriver());
            Task.Run(() => Drivers.Driver4 = Drivers.OptionDriver());
            Task.Run(() => Drivers.Driver5 = Drivers.OptionDriver());
            Task.WaitAny(g1);
        }
    }
}