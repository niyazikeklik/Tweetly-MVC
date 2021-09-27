using Microsoft.EntityFrameworkCore;
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
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class Yardimci
    {
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
        public static string Donustur(string metin)
        {
            metin = metin
                .Replace("Tweets", "")
                .Replace("Tweet", "")
                .Replace("Tweetler", "")
                .Replace("Beğeniler", "")
                .Replace("Beğeni", "")
                .Replace("Likes", "")
                .Replace("Like", "")
                .Replace("Tepki", "");

            metin = metin.Replace(" Mn ", "000000");
            if (!metin.Contains(',') && !metin.Contains('.'))
                metin = metin.Replace(" B ", "000").Replace(" K ", "000");
            else
                metin = metin.Replace(" B ", "00").Replace(" K ", "00");

            string number = "";
            foreach (char item in metin)
                if (char.IsNumber(item))
                    number += item;
            return number;
        }
        public static double UyelikSuresi(string kayittarihi)
        {
            List<string> Months = new (){"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            string AyAdi = kayittarihi.Split(' ')[0];
            int Yil = Convert.ToInt32(kayittarihi.Split(' ')[1]);

            int Index = Months.IndexOf(AyAdi) + 1;
            int Ay = Index > 12 ? Index - 12  : Index;
            return (DateTime.Today - new DateTime(Yil, Ay, 01)).TotalDays;
        }
        public static string CinsiyetBul(string name)
        {

            if (String.IsNullOrEmpty(name)) return "Belirsiz";
            string[] isimler = name?.Split(' ');
            string isim = isimler[0].Replace("'", "").Replace(".", "").Replace(",", "").ToLower();
            string cinsiyet = "Belirsi...z";
            var result = Hesap.Instance.Cins.FirstOrDefault(x => x.Ad == isim);
            if (result != null)
            {
                cinsiyet = result.Cinsiyet == "e" ? "Erkek" :
                           result.Cinsiyet == "k" ? "Kadın" :
                           result.Cinsiyet == "u" ? "Unisex" : "Belirsi...z";
                return cinsiyet;
            }
            else
            {
                /*
                isim = isim.StringReplace();
                var msg =  await client.GetStringAsync("https://api.genderize.io?name=" + isim);
                cinsiyet = msg.Contains("male") ? "Erke-k" : 
                           msg.Contains("female") ? "Kadı-n" : 
                           "Belirsiz";
                return cinsiyet;*/


                /*Yapay Zeka ile Tespit*/

                return "Belirsiz";
            }
        }
        public static object JSCodeRun(this IWebDriver driver, string command)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            int count = 0;
            string exMg = "";
            while (count < 10)
                try
                {
                    var result = jse.ExecuteScript(command);
                    if (result != null || !command.Contains("return")) return result;
                }
                catch (StaleElementReferenceException ex)
                {
                    exMg = ex.Message;
                    count++;
                }
                catch (WebDriverException ex)
                {
                    Thread.Sleep(150);
                    exMg = ex.Message;
                    count++;
                }
            if (command.Contains("return"))
                throw new ApplicationException((new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name + " Hata,\nparametre: " + command + "\nCount: " + count + "\nHata Mesajı: " + exMg);
            else return null;
        }
        public static List<User> DistinctByUserName(this List<User> list)
        {
            return list.GroupBy(x => x.Username).Select(x => x.First()).ToList();
        }
        public static IWebDriver LinkeGit(this IWebDriver driverr, string link, int waitForPageLoad_ms = 1000)
        {
            driverr.Navigate().GoToUrl(link);
            Thread.Sleep(waitForPageLoad_ms);
            return driverr;
        }
        public static bool Control(this IWebDriver driver, string userName, string link, int ms = 300000)
        {
            Hesap.Instance.Iletisim.metin = "";
            int count = 0;
            while (driver.FindElements(By.XPath("//a[@href='/" + userName + "/followers']")).Count == 0)
            {
                if (driver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0)
                    return false; //Engellemişse


                if (driver.Url.Contains("limit") || (bool)driver.JSCodeRun("return document.querySelectorAll('[data-testid=primaryColumn] > div > div > div > [role=button]').length > 0;"))
                {

                    Hesap.Instance.Iletisim.metin = "Limite takıldı. Bitiş: " + DateTime.Now.AddMilliseconds(ms) + " | ";
                    Thread.Sleep(ms);
                    Hesap.Instance.Iletisim.metin = "";
                    driver.Navigate().GoToUrl(link);
                    return driver.Control(userName, link, 60000);

                }
                if (count is 20)
                {
                    driver.Navigate().Refresh();
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
        public static bool IsSayfaSonu(this IWebDriver driverr)
        {
            Int64 sonrakiY, count = 0;
            Int64 oncekiY = (Int64)driverr.JSCodeRun("return window.scrollY;");
            driverr.JSCodeRun("window.scrollBy(0, 2000);");
            do
            {
                Thread.Sleep(200);
                sonrakiY = (Int64)driverr.JSCodeRun("return window.scrollY;");
            } while (oncekiY == sonrakiY && count++ < 20);
            if (oncekiY == sonrakiY)
                return true;
            else return false;
        }
    }
}
/*private static bool İsPage(string url)
       {
           url = url.Replace("400x400", "normal").Replace("200x200", "normal").Replace("x96", "normal");
           try
           {
               HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
               request.Timeout = 1500;
               request.Method = "HEAD";
               using HttpWebResponse response = request.GetResponse() as HttpWebResponse;
               return true;
           }
           catch (WebException ex)
           {
               if (ex.Message.Contains("404"))
                   return false;
               return true;
           }
       }*/
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
/*  public static T BaseToSub<T>(object BaseObject)
     {
         string serialized = JsonConvert.SerializeObject(BaseObject);
         T child = JsonConvert.DeserializeObject<T>(serialized);
         return child;
     }*/

