using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Tweetly_MVC.Models
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
        static private Hesap instance;
        public static Hesap Instance
        {
            get
            {
                if (instance == null)
                    instance = new Hesap();
                return instance;
            }
        }

        Hesap()
        {
            this.Liste = new List<User>();
            this.TakipEdilenler = new List<User>();
            this.GeriTakipEtmeyenler = new List<User>();
            this.Iletisim = new IILetisim();
            this.cins = JsonConvert.DeserializeObject<List<Cinsiyetler>>(File.ReadAllText("Cinsiyetler.json").ToLower());
        }
        public string loginUserName { get; set; }
        public string loginPass { get; set; }
        public User OturumBilgileri { get; set; }
        public List<User> TakipEdilenler { get; set; }
        public List<User> GeriTakipEtmeyenler { get; set; }

        public IILetisim Iletisim;
        public List<Cinsiyetler> cins { get; set; }
        public List<User>  Liste { get; set; }
    }
}
