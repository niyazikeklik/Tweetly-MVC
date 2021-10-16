using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class ExtensionMethod
    {

        public static object JSCodeRun(this IWebDriver driver, string command)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            int count = 0;
            string exMg = "";
            while (count < 10)
            {
                try
                {
                    object result = jse.ExecuteScript(command);
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
            }

            if (command.Contains("return"))
                throw new ApplicationException(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name + " Hata,\nparametre: " + command + "\nCount: " + count + "\nHata Mesajı: " + exMg);
            else return null;
        }
        public static IWebDriver LinkeGit(this IWebDriver driverr, string link, int waitForPageLoad_ms = 1000)
        {
            driverr.Navigate().GoToUrl(link);
            Thread.Sleep(waitForPageLoad_ms);
            return driverr;
        }
        public static bool ProfilLoadControl(this IWebDriver driver, string userName, string link, int ms = 300000)
        {
            Repo.Ins.Iletisim.HataMetni = "";
            int count = 0;
            while (driver.FindElements(By.XPath("//a[@href='/" + userName + "/followers']")).Count == 0)
            {
                if (driver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0)
                    return false; //Engellemişse

                if (driver.Url.Contains("limit") || (bool)driver.JSCodeRun("return document.querySelectorAll('[data-testid=primaryColumn] > div > div > div > [role=button]').length > 0;"))
                {
                    //Yenile butonu görürse veya url'de limit varsa
                    Repo.Ins.Iletisim.HataMetni = "Limite takıldı. Bitiş: " + DateTime.Now.AddMilliseconds(ms) + " | ";
                    Thread.Sleep(ms);
                    Repo.Ins.Iletisim.HataMetni = "";
                    driver.Navigate().GoToUrl(link);
                    return driver.ProfilLoadControl(userName, link, 60000);
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
                return driver.ProfilLoadControl(userName, link, ms);
            }
            return true;
        }
        public static bool IsSayfaSonu(this IWebDriver driverr)
        {
            long sonrakiY, count = 0;
            long oncekiY = (long)driverr.JSCodeRun("return window.scrollY;");
            driverr.JSCodeRun("window.scrollBy(0, 500);");
            do
            {
                Thread.Sleep(300);
                sonrakiY = (long)driverr.JSCodeRun("return window.scrollY;");
            } while (oncekiY == sonrakiY && count++ < 3);
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