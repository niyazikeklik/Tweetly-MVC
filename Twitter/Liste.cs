using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetly_MVC.Twitter
{
    public static class Liste
    {
        public static string getUserName(string elementText)
        {
            int basla = elementText.IndexOf('@');
            return elementText[basla..elementText.IndexOf('\n', basla)];
        }
        public static string getName(string elementText)
        {
            return elementText.Split('\n')[0].StartsWith('@') ? null : elementText.Split('\n')[0];
        }

        public static string getPhotoURL(IWebElement element)
        {
            return element.FindElement(By.TagName("img")).GetAttribute("src").Replace("x96", "200x200");
        }
        public static bool isPrivate(IWebElement element)
        {
            return element.FindElements(By.CssSelector("[aria-label='Korumalı hesap']")).Count > 0;
        }
        public static string getFollowersStatus(string elementText)
        {
            if (elementText.Contains("Seni takip ediyor"))
                return "Seni takip ediyor";
            return "Takip etmiyor";
        }
        public static string getFollowStatus(string elementText)
        {
            if (elementText.Contains("Takip ediliyor"))
                return "Takip ediliyor";
            else if (elementText.Contains("Takip et")) 
                return "Takip et";
            return "Engellendi";
        }
        public static string getBio(IWebElement element)
        {
            var bios = element.FindElements(By.XPath("/div/div[2]/div[2]/span"));
            if (bios.Count > 0) return bios[0].Text;
            return null;
        }
    }
}
