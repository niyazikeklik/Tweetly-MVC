
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class Login
    {
        public static void Giris(string ka, string ps, IWebDriver driver)
        {
            Thread.Sleep(1000);
            if (driver.Url.Contains("flow"))
            {
                IWebElement inputUserName = (IWebElement)driver.JSCodeRun("return document.querySelector('[name=username]');");
                inputUserName.SendKeys(ka + Keys.Enter);
                IWebElement inputPass = (IWebElement)driver.JSCodeRun("return document.querySelector('[name=password]');");
                inputPass.SendKeys(ps + Keys.Enter);
                if (driver.Url.Contains("redirect_after_login"))
                    Giris("niyazikeklik@gmail.com", Hesap.Instance.SettingsUser.Pass, driver);
            }

        }
    }
}
