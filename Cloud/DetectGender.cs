using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC.Cloud
{
    public static class DetectGender
    {
        private const string cinsiyetlerJsonYolu = "Cinsiyetler.json";
        private const string yapayZekaYolu = @" C:\Users\niyazi\Desktop\son\Tweetly-MVC\YapayZeka\detect.py";
        public static void WriteGenderJson()
        {
            string stringJSON = JsonConvert.SerializeObject(Repo.Ins.Cins);
            File.WriteAllText(cinsiyetlerJsonYolu, stringJSON);
        }
        public static List<Cinsiyetler> ReadGenderJson()
        {
            string dosyaText = File.ReadAllText(cinsiyetlerJsonYolu).ToLower();
            var x = JsonConvert.DeserializeObject<List<Cinsiyetler>>(dosyaText);
            return x;
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
               photoURL = photoURL.Replace("200x200", "400x400");
  
            ProcessStartInfo ProcessInfo = new("cmd.exe", "/c" + $" python {yapayZekaYolu} --image {photoURL}");
            ProcessInfo.CreateNoWindow = false;
            ProcessInfo.UseShellExecute = false;
            ProcessInfo.RedirectStandardOutput = true;
            ProcessInfo.Verb = "runas";
            Process Process = Process.Start(ProcessInfo);
            string result = Process.StandardOutput.ReadToEnd().Replace("Kadin","Kadın");
            Process.WaitForExit();
            Process.Close();
            return result;
        }
        private static string GetGenderFromAPI(string name)
        {
            string cinsiyet = "Belirsiz";
            try
            {

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
            }
            catch (Exception)
            {

                ;
            }

            return cinsiyet;
        }
        private static string GetGenderFromName(string name)
        {
            string cinsiyet = "Belirsiz";
            string isim = name.Split(' ')[0].Replace("'", "").Replace(".", "").Replace(",", "").ToLower();
            Cinsiyetler result = Repo.Ins.Cins.FirstOrDefault(x => x.Ad == isim);
            if (result == null) return null;
            cinsiyet = result.Cins == "e" ? "Erkek" :
                       result.Cins == "k" ? "Kadın" : "Unisex";
                return cinsiyet;
            

        }
        public static string CinsiyetBul(string name, string link)
        {
            string cinsiyet = "Belirsiz";
            if (!String.IsNullOrEmpty(name))
            {
                cinsiyet = GetGenderFromName(name);
                if (cinsiyet == "Erkek" || cinsiyet == "Kadın") return cinsiyet;
                else if(cinsiyet == "Unisex") return GetGenderFromPhoto(link).Replace("Belirsiz", "Unisex");
            }
            cinsiyet = GetGenderFromPhoto(link);
            if (!cinsiyet.Equals("Belirsiz")) return cinsiyet;
            if (!String.IsNullOrEmpty(name)) cinsiyet = GetGenderFromAPI(name);
            return cinsiyet;
        }

    }
}
