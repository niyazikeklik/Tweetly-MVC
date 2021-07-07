using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tweetly_MVC.Init
{
    public class Yardimci
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
            foreach (var item in months)
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
            var kalangun = bitisTarihi - baslamaTarihi;//Sonucu zaman olarak döndürür
            return kalangun.TotalDays;// kalanGun den TotalDays ile sadece toplam gun değerini çekiyoruz.
        }
        public static void WaitForLoad(IWebDriver driver, int timeoutSec = 15)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
        public static void Control(IWebDriver driver, string userName, string link)
        {
            //By.cssSelector("a[href='mysite.com']");
            //https://mobile.twitter.com/login
            ///login
            try
            {
                if (driver.FindElements(By.CssSelector("[data-testid=login]")).Count > 0)
                {
                    driver.Navigate().Refresh();
                    WaitForLoad(driver);
                    Control(driver, userName, link);
                }


                while (driver.FindElements(By.XPath("//a[@href='/" + userName + "/followers']")).Count == 0)
                {
                    if (driver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0) break;
                    if (driver.Url.Contains("limit"))
                    {
                        Thread.Sleep(300000);
                        driver.Navigate().GoToUrl(link);
                        Control(driver, userName,link);
                    }
                    Thread.Sleep(5);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(300000);
                driver.Navigate().GoToUrl(link);
                Control(driver, userName, link);
            }

        }
        public static bool isSayfaSonu(IWebDriver driverr)
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
        public static string CinsiyetBul(string isim)
        {
            return null;
        }
    }
}
