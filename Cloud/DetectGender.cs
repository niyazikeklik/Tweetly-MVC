using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Cloud
{
    public static class DetectGender
    {
        public static void WriteGenderJson()
        {
            var x = DateTime.Now;
            string stringJSON = JsonConvert.SerializeObject(Repo.Ins.Cins);
            File.WriteAllText("Cinsiyetler.json", stringJSON);
            Debug.WriteLine("Dosya Yazma Süresi:" + (DateTime.Now - x).TotalSeconds);
        }
        private static string StringReplace(this string text)
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
        }
        private static string GetGenderFromPhoto(string photoURL)
        {
            //   photoURL = photoURL.Replace("200x200", "x96");
            string Gender = "Belirsiz";
            string fileName = @"python C:\Users\niyazi\Desktop\son\Tweetly-MVC\YapayZeka\detect.py";
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
                Gender = result[basla..result.IndexOf("*", basla)]
                    .Replace("Female", "Kadın")
                    .Replace("Male", "Erkek");
            }
            else Gender = "Belirsiz";
            Process.WaitForExit();
            Process.Close();
            return Gender;
        }
        private static string GetGenderFromAPI(string name)
        {
            string cinsiyet = "Belirsiz";
            var msg = new WebClient().DownloadString("https://api.genderize.io?name=" + name.StringReplace());
            if (msg.Contains("female"))
            {
                Repo.Ins.Cins.Add(new Cinsiyetler(name, "k"));
                cinsiyet = "Kadın";
            }
            else if (msg.Contains("male"))
            {
                Repo.Ins.Cins.Add(new Cinsiyetler(name, "e"));
                cinsiyet = "Erkek";
            }
            return cinsiyet;
        }
        public static string CinsiyetBul(string name, string link)
        {
            string cinsiyet = "Belirsiz";
            if (!String.IsNullOrEmpty(name))
            {
                string isim = name.Split(' ')[0].Replace("'", "").Replace(".", "").Replace(",", "").ToLower();
                Cinsiyetler result = Repo.Ins.Cins.FirstOrDefault(x => x.Ad == isim);
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
  
    }
}
