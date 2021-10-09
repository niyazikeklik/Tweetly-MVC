using OpenQA.Selenium;
using System.Threading;
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
                Thread.Sleep(2500);
                IWebElement inputPass = (IWebElement)driver.JSCodeRun("return document.querySelector('[name=password]');");
                inputPass.SendKeys(ps + Keys.Enter);
                Thread.Sleep(2500);

                if (driver.Url.Contains("redirect_after_login"))
                    Giris("niyazikeklik@gmail.com", Hesap.Ins.UserSettings.Pass, driver);

            }
            // driver.LinkeGit("https://mobile.twitter.com/settings/language", 1500);
            //   driver.JSCodeRun("document.querySelector('select [value=\"tr\"]').selected = true;");
            //driver.JSCodeRun("document.querySelector('[data-testid=settingsDetailSave]').click();");
        }
    }
}