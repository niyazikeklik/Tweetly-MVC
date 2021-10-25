using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Tweetly_MVC.Init;

namespace Tweetly_MVC.Twitter
{
    public static class Liste
    {
        public const string sabit = "**__*-*__**";
        public static ReadOnlyCollection<object> GetListelenenler(this IWebDriver driverr)
        {
            var result = (ReadOnlyCollection<object>)driverr.JsRun("var list =[]; var x = document.querySelectorAll('[data-testid=\"UserCell\"]'); x.forEach(elem =>{list.push(elem.innerHTML + \"" + sabit + "\" + elem.innerText);}); return list;");
            return result;
        }
        public static string GetUserName(string elementText)
        {
            int basla = elementText.IndexOf('@') + 1;
            return elementText[basla..elementText.IndexOf('\n', basla)];
        }

        public static string GetName(string elementText) => elementText.Split('\n')[0].StartsWith('@') ? null : elementText.Split('\n')[0];

        public static string GetPhotoURL(this string element)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(element);
            var result = htmlDoc.QuerySelector("img").Attributes["src"].Value.Replace("x96", "200x200");
            return result;
        }

        public static bool İsPrivate(this string element)
        {
            return element.Contains("Korumalı hesap");
        }

        public static string GetFollowersStatus(string elementText)
        {
            string r = elementText.Contains("ediyor") ? "Seni takip ediyor" : "Takip etmiyor";
            return r;
        }

        public static string GetFollowStatus(string elementText)
        {
            if (elementText.Contains("Takip ediliyor"))
                return "Takip ediliyor";
            else if (elementText.Contains("Takip et"))
                return "Takip et";
            return "Engellendi";
        }

        public static string GetBio(this string element)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(element);
            HtmlNode node = htmlDoc.QuerySelector("div:nth-child(1) > div:nth-child(2) > div:nth-child(2)");
            var result = node?.InnerText;
            return result;
            /*   string name = htmlDoc.QuerySelector()
                   .SelectSingleNode("")
                   .Attributes["value"].Value;*/

            /*    System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> bios = element.FindElements(By.CssSelector(""));
                if (bios.Count > 0) return bios[0].Text;
                return null;*/
        }
    }
}