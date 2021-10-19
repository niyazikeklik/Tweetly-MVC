using OpenQA.Selenium;
using System.Threading;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class Login
    {
        public static void Giris(string ka, string ps, string mail, IWebDriver driver)
        {
         
            Thread.Sleep(2500);
            if (driver.Url.Contains("flow"))
            {
                ((IWebElement)driver.JSCodeRun("return document.querySelector('[name=username]');")).SendKeys(ka + Keys.Enter);
                Thread.Sleep(2500);
                ((IWebElement)driver.JSCodeRun("return document.querySelector('[name=password]');")).SendKeys(ps + Keys.Enter);
                Thread.Sleep(2500);
                if (driver.Url.Contains("redirect_after_login"))
                {
                    driver.LinkeGit("https://mobile.twitter.com/login");
                    Giris(ka, Repo.Ins.UserSettings.Pass,mail, driver);
                }
                    

            }
            // driver.LinkeGit("https://mobile.twitter.com/settings/language", 1500);
            //   driver.JSCodeRun("document.querySelector('select [value=\"tr\"]').selected = true;");
            //driver.JSCodeRun("document.querySelector('[data-testid=settingsDetailSave]').click();");
        }
    }
}