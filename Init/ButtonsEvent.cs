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
        public static string profilUserButonClick(this IWebDriver driverr)
        {
            driverr.JSCodeRun("document.querySelector('[data-testid=placementTracking] [role=button]').click();");
            onayButonClick(driverr);
            return driverr.getfollowStatus();
        }
        public static void onayButonClick(this IWebDriver driverr)
        {
            Thread.Sleep(200);
            if ((bool)driverr.JSCodeRun("return document.querySelectorAll('[data-testid=confirmationSheetConfirm]').length > 0"))
                driverr.JSCodeRun("document.querySelector('[data-testid=confirmationSheetConfirm]').click();");
        }
        public static void UserActionsButonClick(this IWebDriver driverr)
        {
            driverr.JSCodeRun("document.querySelector('[data-testid=userActions]').click();");
            Thread.Sleep(200);
        }
        public static void profilEngelle(this IWebDriver driverr)
        {
            driverr.UserActionsButonClick();
            driverr.JSCodeRun("document.querySelector('[data-testid=block]').click();");
            driverr.onayButonClick();
        }

    }
}
