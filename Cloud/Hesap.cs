using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Tweetly_MVC.Tweetly
{
    public class Cinsiyetler
    {
        public string Ad { get; set; }
        public string Cinsiyet { get; set; }

    }
    public struct IILetisim
    {
        public System.DateTime tarih;
        public string sure;
        public string metin;
        public int veri;
        public int Max;
        public int CurrentValue;
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
                    instance = new Hesap();
                return instance;
            }
        }

        private Hesap()
        {
            Liste = new List<User>();
            TakipEdilenler = new List<User>();
            GeriTakipEtmeyenler = new List<User>();
            Iletisim = new IILetisim();
            Cins = JsonConvert.DeserializeObject<List<Cinsiyetler>>(File.ReadAllText("Cinsiyetler.json").ToLower());
        }
        public string LoginUserName { get; set; }
        public string LoginPass { get; set; }
        public User OturumBilgileri { get; set; }
        public List<Cinsiyetler> Cins { get; set; }
        public List<User> Liste { get; set; }
        public List<User> TakipEdilenler { get; set; }
        public List<User> GeriTakipEtmeyenler { get; set; }

    }
}
