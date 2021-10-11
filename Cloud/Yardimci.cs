using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Init
{
    public static class Yardimci
    {
        public static object JSCodeRun(this IWebDriver driver, string command)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            int count = 0;
            string exMg = "";
            while (count < 10)
            {
                try
                {
                    object result = jse.ExecuteScript(command);
                    if (result != null || !command.Contains("return")) return result;
                }
                catch (StaleElementReferenceException ex)
                {
                    exMg = ex.Message;
                    count++;
                }
                catch (WebDriverException ex)
                {
                    Thread.Sleep(150);
                    exMg = ex.Message;
                    count++;
                }
            }

            if (command.Contains("return"))
                throw new ApplicationException(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name + " Hata,\nparametre: " + command + "\nCount: " + count + "\nHata Mesajı: " + exMg);
            else return null;
        }

        public static void KillProcces()
        {
            foreach (Process item in Process.GetProcesses())
            {
                if (item.ProcessName is "chrome" or "chromedriver" or "conhost")
                {
                    try
                    {
                        double s = (DateTime.Now - item.StartTime).TotalSeconds;
                        if (s >= 30) { item.Kill(); }
                    }
                    catch {; }
                }
            }
        }

        public static string Donustur(string metin)
        {
            metin = metin
                .Replace("Tweets", "")
                .Replace("Tweet", "")
                .Replace("Tweetler", "")
                .Replace("Beğeniler", "")
                .Replace("Beğeni", "")
                .Replace("Likes", "")
                .Replace("Like", "")
                .Replace("Tepki", "");

            metin = metin.Replace(" Mn ", "000000");
            metin = !metin.Contains(',') && !metin.Contains('.')
                ? metin.Replace(" B ", "000").Replace(" K ", "000")
                : metin.Replace(" B ", "00").Replace(" K ", "00");

            string number = "";
            foreach (char item in metin)
            {
                if (char.IsNumber(item))
                    number += item;
            }

            return number;
        }

        public static double UyelikSuresi(string kayittarihi)
        {
            List<string> Months = new() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            string AyAdi = kayittarihi.Split(' ')[0];
            int Yil = Convert.ToInt32(kayittarihi.Split(' ')[1]);

            int Index = Months.IndexOf(AyAdi) + 1;
            int Ay = Index > 12 ? Index - 12 : Index;
            return (DateTime.Today - new DateTime(Yil, Ay, 01)).TotalDays;
        }
             private static string GetGenderFromPhoto(string photoURL)
        {
            photoURL = photoURL.Replace("200x200", "x96");
            Stopwatch watch = new();
            watch.Start();
            string Gender = "Belirsiz";
            string fileName = @"python C:\Users\niyazi\Desktop\Tweetly\Tweetly-MVC\YapayZeka\detect.py";
            ProcessStartInfo ProcessInfo = new("cmd.exe", "/c " + string.Format(fileName + " --image " + photoURL));
            ProcessInfo.CreateNoWindow = false;
            ProcessInfo.UseShellExecute = false;
            ProcessInfo.RedirectStandardOutput = true;
            ProcessInfo.Verb = "runas";
            Process Process = Process.Start(ProcessInfo);
            using StreamReader reader = Process.StandardOutput;
            string result = reader.ReadToEnd();
            if (result.Contains("Age") || result.Contains("Gender"))
            {

                int basla = result.IndexOf("Gender: *") + 9;
                Gender = result[basla..result.IndexOf("*", basla)].Replace("Female", "Kadın").Replace("Male", "Erkek");

                /* int basla = result.IndexOf("Age: *") + 6;
                 int bitir = result.IndexOf("*", basla);
                 string Age = result.Substring(basla, bitir - basla);
                Console.Write(Age + " - " + Gender);*/

            }
            else Gender = "Belirsiz";
            Process.WaitForExit();
            Process.Close();
            watch.Stop();
            Debug.WriteLine("Yapay Zeka Cinsiyet Tespit Süresi: " + watch.Elapsed.TotalSeconds + " saniye");
            return Gender;
        }
        private static string GetGenderFromAPI(string name)
        {
            string cinsiyet = "Belirsiz";
            var msg = new WebClient().DownloadString("https://api.genderize.io?name=" + name.StringReplace());
            if (msg.Contains("female"))
            {
                Hesap.Ins.Cins.Add(new Cinsiyetler(name, "k"));
                cinsiyet = "Kadın";
            }
            else if (msg.Contains("male"))
            {
                Hesap.Ins.Cins.Add(new Cinsiyetler(name, "e"));
                cinsiyet = "Erkek";
            }
            else return cinsiyet;

            Task.Run(() => {
                string stringJSON = JsonConvert.SerializeObject(Hesap.Ins.Cins);
                File.WriteAllText("Cinsiyetler.json", stringJSON);
            });
            return cinsiyet;
        }
        public static string CinsiyetBul(string name, string link)
        {
            string cinsiyet = "Belirsiz";
            if (!String.IsNullOrEmpty(name))
            {
                string isim = name.Split(' ')[0].Replace("'", "").Replace(".", "").Replace(",", "").ToLower();
                Cinsiyetler result = Hesap.Ins.Cins.FirstOrDefault(x => x.Ad == isim);
                if (result != null)
                {
                    cinsiyet = result.Cins == "e" ? "Erkek" :
                               result.Cins == "k" ? "Kadın" :
                               GetGenderFromPhoto(link).Replace("Belirsiz", "Unisex");
                    return cinsiyet;
                }
            }
            cinsiyet = GetGenderFromPhoto(link);
            if (!cinsiyet.Contains("Belirsiz")) return cinsiyet;
            if (!String.IsNullOrEmpty(name)) cinsiyet = GetGenderFromAPI(name);
            return cinsiyet;
        }

        public static List<User> DistinctByUserName(this List<User> list) => list.GroupBy(x => x.Username).Select(x => x.First()).ToList();

        public static IWebDriver LinkeGit(this IWebDriver driverr, string link, int waitForPageLoad_ms = 1000)
        {
            driverr.Navigate().GoToUrl(link);
            Thread.Sleep(waitForPageLoad_ms);
            return driverr;
        }

        public static bool Control(this IWebDriver driver, string userName, string link, int ms = 300000)
        {
            Hesap.Ins.Iletisim.HataMetni = "";
            int count = 0;
            while (driver.FindElements(By.XPath("//a[@href='/" + userName + "/followers']")).Count == 0)
            {
                if (driver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0)
                    return false; //Engellemişse

                if (driver.Url.Contains("limit") || (bool)driver.JSCodeRun("return document.querySelectorAll('[data-testid=primaryColumn] > div > div > div > [role=button]').length > 0;"))
                {
                    Hesap.Ins.Iletisim.HataMetni = "Limite takıldı. Bitiş: " + DateTime.Now.AddMilliseconds(ms) + " | ";
                    Thread.Sleep(ms);
                    Hesap.Ins.Iletisim.HataMetni = "";
                    driver.Navigate().GoToUrl(link);
                    return driver.Control(userName, link, 60000);
                }
                if (count is 20)
                {
                    driver.Navigate().Refresh();
                    Thread.Sleep(2000);
                }
                Thread.Sleep(250);
                count++;
            }
            if (driver.FindElements(By.CssSelector("[data-testid=login]")).Count > 0)
            {
                driver.Navigate().Refresh();
                return driver.Control(userName, link, ms);
            }
            return true;
        }

        public static bool IsSayfaSonu(this IWebDriver driverr)
        {
            long sonrakiY, count = 0;
            long oncekiY = (long)driverr.JSCodeRun("return window.scrollY;");
            driverr.JSCodeRun("window.scrollBy(0, 2000);");
            do
            {
                Thread.Sleep(200);
                sonrakiY = (long)driverr.JSCodeRun("return window.scrollY;");
            } while (oncekiY == sonrakiY && count++ < 20);
            if (oncekiY == sonrakiY)
                return true;
            else return false;
        }
    }
}

/*private static bool İsPage(string url)
       {
           url = url.Replace("400x400", "normal").Replace("200x200", "normal").Replace("x96", "normal");
           try
           {
               HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
               request.Timeout = 1500;
               request.Method = "HEAD";
               using HttpWebResponse response = request.GetResponse() as HttpWebResponse;
               return true;
           }
           catch (WebException ex)
           {
               if (ex.Message.Contains("404"))
                   return false;
               return true;
           }
       }*/
/*private static string StringReplace(this string text)
{
    text = text.Replace("İ", "I");
    text = text.Replace("ı", "i");
    text = text.Replace("Ğ", "G");
    text = text.Replace("ğ", "g");
    text = text.Replace("Ö", "O");
    text = text.Replace("ö", "o");
    text = text.Replace("Ü", "U");
    text = text.Replace("ü", "u");
    text = text.Replace("Ş", "S");
    text = text.Replace("ş", "s");
    text = text.Replace("Ç", "C");
    text = text.Replace("ç", "c");
    return text;
}*/
/*  public static T BaseToSub<T>(object BaseObject)
     {
         string serialized = JsonConvert.SerializeObject(BaseObject);
         T child = JsonConvert.DeserializeObject<T>(serialized);
         return child;
     }*/