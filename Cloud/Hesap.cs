using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Tweetly
{

    public class Cinsiyetler
    {
        public string Ad { get; set; }
        public string Cinsiyet { get; set; }

    }
    public struct IILetisim
    {
        public string metin;
        public string veri;
    }
    public class Hesap
    {
        private static Hesap instance;
        public IILetisim Iletisim;
        public static Hesap Instance
        {
            get
            {
                if (instance == null)
                    instance = new();
                return instance;
            }
        }

        private Hesap()
        {
            SettingsUser = AppSettings.Get();
            OturumBilgileri = new();
            Liste = new();
            Takipciler = new();
            TakipEdilenler = new();
            GeriTakipYapmayanlar = new();
            SettingsFinder = new();
            Iletisim = new();
            Cins = JsonConvert.DeserializeObject<List<Cinsiyetler>>(File.ReadAllText("Cinsiyetler.json").ToLower());
        }

        public User OturumBilgileri;
        public List<Cinsiyetler> Cins;
        public List<User> Liste;
        public List<User> Takipciler;
        public List<User> TakipEdilenler;
        public List<User> GeriTakipYapmayanlar;
        public FinderSettings SettingsFinder;
        public UserSettings SettingsUser;
    }
}
