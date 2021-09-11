using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public static class Yardimci
    {
        private static readonly int ms = 75;

        public static object DistinctByUserName(this List<User> list)
        {
            return list.GroupBy(x => x.Username).Select(x => x.First()).ToList();
        }

        public static IWebDriver ProfileGit(string link, int waitForPageLoad_ms = 1000)
        {
            IWebDriver driverr = Drivers.Driver;
            driverr.Navigate().GoToUrl(link);
            driverr.WaitForLoad();
            Thread.Sleep(waitForPageLoad_ms);
            return driverr;
        }
        public static T BaseToSub<T>(object BaseObject)
        {
            string serialized = JsonConvert.SerializeObject(BaseObject);
            T child = JsonConvert.DeserializeObject<T>(serialized);
            return child;
        }
        public static IWebElement GetElement(this IWebElement driverr, By sorgu)
        {
            int count = 0;
            while (count < 10)
            {
                try
                {
                    return driverr.FindElement(sorgu);
                }
                catch (Exception)
                {
                    Thread.Sleep(ms);
                    count++;
                }
            }
            return null;
        }
        public static List<IWebElement> GetElements(this IWebElement driverr, By sorgu)
        {
            int count = 0;
            while (count < 10)
            {
                try
                {
                    return driverr.FindElements(sorgu).ToList();
                }
                catch (Exception)
                {
                    Thread.Sleep(ms);
                    count++;
                }
            }
            return null;
        }
        public static IWebElement GetElement(this IWebDriver driverr, By sorgu)
        {
            int count = 0;
            while (count < 10)
            {
                try
                {
                    return driverr.FindElement(sorgu);
                }
                catch (Exception)
                {
                    Thread.Sleep(ms);
                    count++;
                }
            }

            return null;
        }
        public static List<IWebElement> GetElements(this IWebDriver driverr, By sorgu)
        {
            int count = 0;
            while (count < 10)
            {
                try
                {
                    return driverr.FindElements(sorgu).ToList();
                }
                catch (Exception)
                {
                    Thread.Sleep(ms);
                    count++;
                }
            }
            return null;
        }
        public static bool İsPage(string url)
        {
            url = url.Replace("400x400", "x96").Replace("200x200", "x96").Replace("normal", "x96").Replace("mini", "x96");
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 1000;
                request.Method = "HEAD";
                using HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("404"))
                {
                    return false;
                }

                return true;
            }
        }
        public static string Donustur(string metin)
        {
            metin = metin
                .Replace("Tweets", "")
                .Replace("Tweet", "")
                .Replace("Tweetler", "")
                .Replace("Beğeniler", "")
                .Replace("Beğeni", "")
                .Replace("Likes", "")
                .Replace("Like", "");

            metin = metin.Replace("Mn", "000000");
            if (!metin.Contains(',') && !metin.Contains('.'))
            {
                metin = metin.Replace("B", "000").Replace("K", "000");
            }
            else
            {
                metin = metin.Replace("B", "00").Replace("K", "00");
            }

            string number = "";
            foreach (char item in metin)
            {
                if (char.IsNumber(item))
                {
                    number += item;
                }
            }

            return number;
        }
        public static double KayıtTarihi(string kayittarihi)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            kayittarihi = kayittarihi.Replace("tarihinde katıldı", "").Replace("joined", "");
            string ayadi = "";
            foreach (string item in months)
            {
                if (kayittarihi.Contains(item))
                {
                    ayadi = item;
                }
            }

            kayittarihi = kayittarihi.Replace(" ", "").Replace(ayadi, "");
            int yil = Convert.ToInt32(kayittarihi);
            int kacinciay = 01;
            switch (ayadi)
            {
                case "January": kacinciay = 01; break;
                case "Ocak": kacinciay = 01; break;

                case "February": kacinciay = 02; break;
                case "Şubat": kacinciay = 02; break;

                case "March": kacinciay = 03; break;
                case "Mart": kacinciay = 03; break;

                case "April": kacinciay = 04; break;
                case "Nisan": kacinciay = 04; break;

                case "May": kacinciay = 05; break;
                case "Mayıs": kacinciay = 05; break;

                case "June": kacinciay = 06; break;
                case "Haziran": kacinciay = 06; break;

                case "July": kacinciay = 07; break;
                case "Temmuz": kacinciay = 07; break;

                case "August": kacinciay = 08; break;
                case "Ağustos": kacinciay = 08; break;

                case "September": kacinciay = 09; break;
                case "Eylül": kacinciay = 09; break;

                case "October": kacinciay = 10; break;
                case "Ekim": kacinciay = 10; break;

                case "November": kacinciay = 11; break;
                case "Kasım": kacinciay = 11; break;

                case "December": kacinciay = 12; break;
                case "Aralık": kacinciay = 12; break;

                default: break;
            }
            DateTime baslamaTarihi = new(yil, kacinciay, 01);
            DateTime bitisTarihi = DateTime.Today;
            TimeSpan kalangun = bitisTarihi - baslamaTarihi;//Sonucu zaman olarak döndürür
            return kalangun.TotalDays;// kalanGun den TotalDays ile sadece toplam gun değerini çekiyoruz.
        }
        public static void WaitForLoad(this IWebDriver driver, int timeoutSec = 15)
       {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
        public static bool Control(this IWebDriver driver, string userName, string link, int ms = 300000)
        {
            userName = userName.Trim('@');
            Hesap.Instance.Iletisim.metin = "";
            int count = 0;
            driver.WaitForLoad();
            while (driver.FindElements(By.XPath("//a[@href='/" + userName + "/followers']")).Count == 0)
            {
                if (driver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0)
                {
                    return false; //Engellemişse
                }

                if (driver.Url.Contains("limit") || (bool)driver.JSCodeRun("return document.querySelectorAll('[data-testid=primaryColumn] > div > div > div > [role=button]').length > 0;"))
                {

                    Hesap.Instance.Iletisim.metin = "Limite takıldı. Bitiş: " + DateTime.Now.AddMilliseconds(ms) + " | ";
                    Thread.Sleep(ms);
                    Hesap.Instance.Iletisim.metin = "";
                    driver.Navigate().GoToUrl(link);
                    return driver.Control(userName, link, 60000);

                }
                if (count == 20)
                {
                    driver.Navigate().Refresh();
                    driver.WaitForLoad();
                    Thread.Sleep(2000);
                }
                Thread.Sleep(250);
                count++;


            }
            if (driver.FindElements(By.CssSelector("[data-testid=login]")).Count > 0)
            {
                driver.Navigate().Refresh();
                return driver.Control(userName, link, ms);
            }
            return true;

        }

        public static void KillProcces()
        {
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
        }
        /*private static string StringReplace(this string text)
        {
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ö", "O");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "U");
            text = text.Replace("ü", "u");
            text = text.Replace("Ş", "S");
            text = text.Replace("ş", "s");
            text = text.Replace("Ç", "C");
            text = text.Replace("ç", "c");
            return text;
        }*/


        public static bool IsSayfaSonu(this IWebDriver driverr)
        {
            try
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driverr;
                double oncekiY = Convert.ToDouble(jse.ExecuteScript("return window.scrollY;"));
                jse.ExecuteScript("window.scrollBy(0,1500);");
                Thread.Sleep(300);
                double sonrakiY = Convert.ToDouble(jse.ExecuteScript("return window.scrollY;"));
                if (oncekiY == sonrakiY)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return IsSayfaSonu(driverr);
            }

        }
    }
}
