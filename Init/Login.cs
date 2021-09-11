using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public static class Login
    {
        public static void Giris(string ka, string ps, IWebDriver driver)
        {
            driver.WaitForLoad();
            Thread.Sleep(1000);
            if (driver.Url.Contains("flow"))
            {
                driver.GetElement(By.Name("username")).SendKeys(ka + Keys.Enter);
                driver.GetElement(By.Name("password")).SendKeys(ps + Keys.Enter);
            }
            else if (driver.Url.Contains("login"))
            {
            tekrar:
                try
                {
                    driver.GetElement(By.Name("session[username_or_email]"))?.Clear();
                    driver.GetElement(By.Name("session[username_or_email]"))?.SendKeys(ka);
                    driver.GetElement(By.Name("session[password]"))?.Clear();
                    driver.GetElement(By.Name("session[password]"))?.SendKeys(ps + Keys.Enter);
                }
                catch (Exception)
                {
                    goto tekrar;
                }

                Thread.Sleep(1000);
                if (driver.Url.Contains("redirect_after_login"))
                    Giris("niyazikeklik@gmail.com", Hesap.Instance.LoginPass, driver);
            }
            else if (driver.Url.Contains("home"))
            {
                ;
            }

        }
    

     

    }
}
