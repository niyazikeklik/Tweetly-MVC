using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Tweetly_MVC.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Tweetly_MVC.Init
{
    public static class Yardimci
    {
        public static string Donustur(string metin)
        {
            metin = metin.Replace(" ", "");
            metin = metin.Replace("Mn", "000000");
            if (metin.IndexOf(',') == -1 && metin.IndexOf('.') == -1)
            {
                metin = metin.Replace("B", "000").Replace("K", "000");
            }
            else
            {
                metin = metin.Replace("B", "00").Replace("K", "00");
            }
            metin = metin.Replace(".", "").Replace(" ", "").Replace(",", "");
            return metin;
        }
        public static double KayıtTarihi(string kayittarihi)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            kayittarihi = kayittarihi.Replace("tarihinde katıldı", "").Replace("joined", "");
            string ayadi = "";
            foreach (string item in months)
                if (kayittarihi.Contains(item)) ayadi = item;
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
            DateTime baslamaTarihi = new DateTime(yil, kacinciay, 01);
            DateTime bitisTarihi = DateTime.Today;
            TimeSpan kalangun = bitisTarihi - baslamaTarihi;//Sonucu zaman olarak döndürür
            return kalangun.TotalDays;// kalanGun den TotalDays ile sadece toplam gun değerini çekiyoruz.
        }
        public static void WaitForLoad(this IWebDriver driver, int timeoutSec = 15)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
        public static void Control(this IWebDriver driver, string userName, string link)
        {
            //By.cssSelector("a[href='mysite.com']");
            //https://mobile.twitter.com/login
            ///login
            try
            {
                while (driver.FindElements(By.XPath("//a[@href='/" + userName + "/followers']")).Count == 0)
                {
                    if (driver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0) break;
                    if (driver.Url.Contains("limit"))
                    {
                        Hesap.Iletisim.metin = "Limite takıldı. Bitiş: " + DateTime.Now.AddMilliseconds(360000) + " | ";
                        Thread.Sleep(360000);
                        Hesap.Iletisim.metin = "";
                        driver.Navigate().GoToUrl(link);
                        driver.Control(userName, link);

                    }
                    Thread.Sleep(5);
                }
                if (driver.FindElements(By.CssSelector("[data-testid=login]")).Count > 0)
                {
                    driver.Navigate().Refresh();
                    driver.WaitForLoad();
                    driver.Control(userName, link);
                }
            }
            catch (Exception)
            {
                Hesap.Iletisim.metin = "Limite takıldı. Bitiş: " + DateTime.Now.AddMilliseconds(360000) + " | ";

                Thread.Sleep(360000);
                Hesap.Iletisim.metin = "";
                driver.Navigate().GoToUrl(link);
                driver.Control(userName, link);
            }

        }
        public static void killProcces()
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
        private static string StringReplace(this string text)
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
        }

        private static readonly HttpClient client = new HttpClient();
    
        public static bool isSayfaSonu(this IWebDriver driverr)
        {
            try
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driverr;
                double oncekiY = Convert.ToDouble(jse.ExecuteScript("return window.scrollY;"));
                double sonrakiY = Convert.ToDouble(jse.ExecuteScript("window.scrollBy(0,1500); return window.scrollY;"));

                if (oncekiY == sonrakiY) return true;
                else return false;
            }
            catch (Exception)
            {
                return isSayfaSonu(driverr);
            }

        }
    }
}
