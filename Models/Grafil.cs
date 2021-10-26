using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetly_MVC.Models
{
    public class Grafil
    {
        public double BegeniAktifligi { get; set; }
        public double TweetAktifligi { get; set; }
        public double OrtalamaBegeni { get; set; }
        public double OrtalamaTweet { get; set; }
        public int KadinSayisi { get; set; }
        public int ErkekSayisi { get; set; }
        public int UnisexSayisi { get; set; }
        public int BelirsizSayisi { get; set; }
        public int BegenenlerSayisi { get; set; }
        public int BegenmeyenlerSayisi { get; set; }

    }
}
