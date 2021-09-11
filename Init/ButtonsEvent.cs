using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tweetly_MVC.Init
{
    public static class ButtonsEvent
    {
        public static string ProfilUserButonClick(this IWebDriver driverr)
        {
            driverr.JSCodeRun("document.querySelector('[data-testid=placementTracking] [role=button]').click();");
            OnayButonClick(driverr);
            return driverr.GetfollowStatus();
        }
        public static void OnayButonClick(this IWebDriver driverr)
        {
            Thread.Sleep(200);
            if ((bool)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=confirmationSheetConfirm]').length > 0"))
                driverr.JSCodeRun("document.querySelector('[data-testid=confirmationSheetConfirm]').click();");
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

    }
}
