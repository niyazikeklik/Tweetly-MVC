using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class Waiters
    {
     
        public static object JsRun(this IWebDriver driver, string command)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            int count = 0;
            string exMg;
            while (count < 10)
            {
                
                count++;
                try
                {
                    dynamic result = jse.ExecuteScript(command);
                    var tp = typeof(ReadOnlyCollection<object>);
                    //result?.GetType() == typeof(ReadOnlyCollection<object>)
                    if (result?.GetType() == tp && result.Count == 0)
                    {
                        driver.WaitForPageLoad();
                        continue;
                    }
                   
                    
                    if (result != null || !command.Contains("return")) return result;
                }
                catch (Exception ex)
                {
                    driver.WaitForPageLoad();
                    exMg = ex.Message;
                    continue;
                  
                }
            }
 /*           if (command.Contains("return"))
                throw new ApplicationException(new StackTrace().GetFrame(1).GetMethod().Name + " Hata,\nparametre: " + command + "\nCount: " + count + "\nHata Mesajı: " + exMg);*/
            return null;
        }
        public static IWebDriver LinkeGit(this IWebDriver driverr, string link, bool WaitForLoad = true)
        {
            driverr.Navigate().GoToUrl(link);
            Thread.Sleep(250);
            if (WaitForLoad) driverr.WaitForPageLoad();
            return driverr;
        }
        public static bool ProfilLoadControl(this IWebDriver driver, string link, int ms = 300000)
        {
            Repo.Ins.Iletisim.HataMetni = "";

            if (driver.FindElements(By.CssSelector("[data-testid=login]")).Count > 0)
            {//Giriş butonu gözüküyorsa..
                driver.Navigate().Refresh();
                return driver.ProfilLoadControl(link, ms);
            }
            if (!driver.WaitForProfilBilgileri())
            {
                if (driver.IsBlocker())
                    return false; //Engellemişse


                if (driver.Url.Contains("limit") || driver.YenileButonuContains())
                {
                    //Yenile butonu görürse veya url'de limit varsa
                    Repo.Ins.Iletisim.HataMetni = "Limite takıldı. Bitiş: " + DateTime.Now.AddMilliseconds(ms) + " | ";
                    Thread.Sleep(ms);
                    Repo.Ins.Iletisim.HataMetni = "";
                    driver.LinkeGit(link);
                    return driver.ProfilLoadControl(link, 60000);
                }

            }
            return true;
        }
        public static void WaitForPageLoad(this IWebDriver driverr)
        {
            while (driverr.IsSayfaLoading()) Thread.Sleep(300);
        }
        public static bool WaitForProfilBilgileri(this IWebDriver driverr)
        {
            int count = 0;
            while (!driverr.IsProfilBilgileriLoad() && count < 10)
            {
                Thread.Sleep(500);
                count++;
            }
            if (count is 10) return false;
            else return true;
        }
        private static bool IsSayfaLoading(this IWebDriver driverr)
        {
            return (bool)driverr.JsRun("return document.querySelectorAll('[role=\"progressbar\"]').length > 0");
        }
        private static bool IsProfilBilgileriLoad(this IWebDriver driverr)
        {
            return (bool)
                driverr.JsRun("return document.querySelectorAll('a[href*=\"followers\"]').length > 0");
        }
        public static bool IsSayfaSonu(this IWebDriver driverr)
        {
            long sonrakiY;
            long oncekiY = (long)driverr.JsRun("return window.scrollY;");
            driverr.JsRun("window.scrollBy(0, 500);");
            driverr.WaitForPageLoad();
            sonrakiY = (long)driverr.JsRun("return window.scrollY;");
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

/*  public static T BaseToSub<T>(object BaseObject)
     {
         string serialized = JsonConvert.SerializeObject(BaseObject);
         T child = JsonConvert.DeserializeObject<T>(serialized);
         return child;
     }*/