using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading;

namespace Tweetly_MVC.Models
{
    public class Drivers
    {
        public static IWebDriver Driver { get; set; }
        public static IWebDriver Driver2 { get; set; }
        public static IWebDriver Driver3 { get; set; }
        public static IWebDriver Driver4 { get; set; }
        public static IWebDriver Driver5 { get; set; }



        public static List<IWebDriver> kullanıyorum = new List<IWebDriver>();


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
            Thread.Sleep(50);
            return MusaitOlanDriver();
        }

    }
}
