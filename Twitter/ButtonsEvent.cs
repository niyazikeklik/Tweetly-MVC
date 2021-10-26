using OpenQA.Selenium;
using System.Threading;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class ButtonsEvent
    {
        public static string ProfilUserButonClick(this IWebDriver driverr)
        {

            driverr.JsRun("document.querySelector('[data-testid=placementTracking] [role=button]').click();");
            OnayButonClick(driverr);
            Drivers.kullanıyorum.Remove(driverr);
            return driverr.GetfollowStatus();
        }
        public static void ProfilUserActionsButonClick(this IWebDriver driverr)
        {
            driverr.JsRun("document.querySelector('[data-testid=userActions]').click();");
            Thread.Sleep(200);
        }
        public static void ProfilEngelle(this IWebDriver driverr)
        {

            driverr.ProfilUserActionsButonClick();
            driverr.JsRun("document.querySelector('[data-testid=block]').click();");
            driverr.OnayButonClick();

        }
        public static void OnayButonClick(this IWebDriver driverr)
        {
            Thread.Sleep(200);
            if ((bool)driverr.JsRun("return document.querySelectorAll('[data-testid=confirmationSheetConfirm]').length > 0"))
                driverr.JsRun("document.querySelector('[data-testid=confirmationSheetConfirm]').click();");
        }
        public static void TakipcilerdenCikar(this IWebDriver driver)
        {
            if (driver.IsFollowers().StartsWith("Seni"))
            {
                driver.ProfilUserActionsButonClick();
                driver.JsRun("document.querySelectorAll('[role=\"menuitem\"]')[8].click();");
                driver.OnayButonClick();
            }
                
        }
    }
}