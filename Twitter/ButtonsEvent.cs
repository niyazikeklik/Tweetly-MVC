using OpenQA.Selenium;
using System.Threading;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class ButtonsEvent
    {
        public static string ProfilUserButonClick(this IWebDriver driverr)
        {

            driverr.JSCodeRun("document.querySelector('[data-testid=placementTracking] [role=button]').click();");
            OnayButonClick(driverr);
            Drivers.kullanıyorum.Remove(driverr);
            return driverr.GetfollowStatus();
        }

        public static void ProfilUserActionsButonClick(this IWebDriver driverr)
        {
            driverr.JSCodeRun("document.querySelector('[data-testid=userActions]').click();");
            Thread.Sleep(200);
        }

        public static void ProfilEngelle(this IWebDriver driverr)
        {

            driverr.ProfilUserActionsButonClick();
            driverr.JSCodeRun("document.querySelector('[data-testid=block]').click();");
            driverr.OnayButonClick();

        }

        public static void OnayButonClick(this IWebDriver driverr)
        {
            Thread.Sleep(200);
            if ((bool)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=confirmationSheetConfirm]').length > 0"))
                driverr.JSCodeRun("document.querySelector('[data-testid=confirmationSheetConfirm]').click();");
        }

        public static void TakipcilerdenCikar(this IWebDriver driver, User profil)
        {
            string link = "https://twitter.com/" + profil.Username;
            driver.LinkeGit(link);
            driver.ProfilEngelle();
            if (driver.GetfollowStatus().StartsWith("Engel"))
                driver.ProfilUserButonClick();
            // Engeli kaldır.

            Drivers.kullanıyorum.Remove(driver);
        }
    }
}