using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC
{
    public class Program
    {
        static IWebDriver optionDriver(string un)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.EnableMobileEmulation("Pixel 2 XL");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            driver.Manage().Window.Size = new Size(400, 820);

            driver.Navigate().GoToUrl("https://mobile.twitter.com/login");
            Thread.Sleep(2500);
        tekrar:
            try
            {
                driver.FindElement(By.Name("session[username_or_email]")).Clear();
                driver.FindElement(By.Name("session[username_or_email]")).SendKeys(un);
                driver.FindElement(By.Name("session[password]")).Clear();
                driver.FindElement(By.Name("session[password]")).SendKeys("Galatasaray1453");
                driver.FindElement(By.CssSelector("[data-testid=LoginForm_Login_Button]")).Click();
            }
            catch (Exception)
            {

                goto tekrar;
            }

            return driver;
        }
        static void createDriver(string un)
        {
            IWebDriver driver = optionDriver(un);
            Drivers.Driver = driver;
        }
        static void createDriver2(string un)
        {
            IWebDriver driver = optionDriver(un);
            Drivers.Driver2 = driver;
        }
        static void createDriver3(string un)
        {
            IWebDriver driver = optionDriver(un);
            Drivers.Driver3 = driver;
        }
        static void createDriver4(string un)
        {
            IWebDriver driver = optionDriver(un);
            Drivers.Driver4 = driver;
        }
        static void createDriver5(string un)
        {
            IWebDriver driver = optionDriver(un);
            Drivers.Driver5 = driver;
        }
        public static void Main(string[] args)
        {
            string userName = "alienationxs";
            Process[] runingProcess = Process.GetProcesses();
            for (int i = 0; i < runingProcess.Length; i++)
            {
                if (runingProcess[i].ProcessName == "chrome" || runingProcess[i].ProcessName == "chromedriver" || runingProcess[i].ProcessName == "conhost")
                {
                    try
                    {
                        double s = (DateTime.Now - runingProcess[i].StartTime).TotalSeconds;
                        if (s >= 30) { runingProcess[i].Kill(); }
                    }
                    catch {; }
                }

            }
            Drivers.loginUserName = userName;
            Task g1 = Task.Run(() => createDriver(userName));
            Task g2 = Task.Run(() => createDriver2(userName));
            Task g3 = Task.Run(() => createDriver3(userName));
            Task g4 = Task.Run(() => createDriver4(userName));
            Task g5 = Task.Run(() => createDriver5(userName));
            Task.WaitAll(new Task[] { g1, g2, g3, g4, g5 });
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
