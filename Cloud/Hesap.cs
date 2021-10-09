using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Tweetly_MVC.Models;
using static Tweetly_MVC.Models.AppSettings;

namespace Tweetly_MVC.Tweetly
{
    public class Cinsiyetler
    {
        public string Ad { get; set; }
        public string Cinsiyet { get; set; }
    }

    public struct IILetisim
    {
        public string HataMetni;
        public int currentValue;
        public string BilgiMetni;
    }

    public class Hesap
    {
        private static Hesap instance;
        public IILetisim Iletisim;

        public static Hesap Ins {
            get {
                if (instance == null)
                    instance = new();
                return instance;
            }
        }

        private Hesap()
        {
            UserSettings = Models.AppSettings.Get();
            OturumBilgileri = new();
            Liste = new();
            Takipciler = new();
            TakipEdilenler = new();
            GeriTakipYapmayanlar = new();
            UserPrefs = new();
            Begenenler = new();
            Iletisim = new();
            Cins = JsonConvert.DeserializeObject<List<Cinsiyetler>>(File.ReadAllText("Cinsiyetler.json").ToLower());
        }

        public User OturumBilgileri;
        public List<Cinsiyetler> Cins;
        public List<User> Liste;
        public List<User> Begenenler;
        public List<User> Takipciler;
        public List<User> TakipEdilenler;
        public List<User> GeriTakipYapmayanlar;
        public FinderSettings UserPrefs;
        public UserSettings UserSettings;
    }
}