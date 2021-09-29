using OpenQA.Selenium;

namespace Tweetly_MVC.Twitter
{
    public static class Liste
    {
        public static string GetUserName(string elementText)
        {
            int basla = elementText.IndexOf('@') + 1;
            return elementText[basla..elementText.IndexOf('\n', basla)];
        }

        public static string GetName(string elementText) => elementText.Split('\n')[0].StartsWith('@') ? null : elementText.Split('\n')[0];

        public static string GetPhotoURL(this IWebElement element) => element.FindElement(By.TagName("img")).GetAttribute("src").Replace("x96", "200x200");

        public static bool İsPrivate(this IWebElement element) => element.FindElements(By.CssSelector("[aria-label='Korumalı hesap']")).Count > 0;

        public static string GetFollowersStatus(string elementText)
        {
            if (elementText.Contains("Seni takip ediyor"))
                return "Seni takip ediyor";
            return "Takip etmiyor";
        }

        public static string GetFollowStatus(string elementText)
        {
            if (elementText.Contains("Takip ediliyor"))
                return "Takip ediliyor";
            else if (elementText.Contains("Takip et"))
                return "Takip et";
            return "Engellendi";
        }

        public static string GetBio(this IWebElement element)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> bios = element.FindElements(By.CssSelector("div > div:nth-child(1) > div:nth-child(2) > div:nth-child(2)"));
            if (bios.Count > 0) return bios[0].Text;
            return null;
        }
    }
}