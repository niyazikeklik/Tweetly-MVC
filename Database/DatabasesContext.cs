using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tweetly_MVC.Init;

namespace Tweetly_MVC.Tweetly
{
    public class DatabasesContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@$"Data Source={Repo.Ins.UserSettings.SQLServerName};Initial Catalog=TweetlyDataBase_{Repo.Ins.OturumBilgileri.Username};Integrated Security=True");//Migrate komutu çalıştırırken cümleden usernameyi sil.//212 331 02 00

        public DbSet<User> Records { get; set; }
    }

    public static class DataBase
    {
        private static List<User> DistinctByUserName(this List<User> list) => list.GroupBy(x => x.Username).Select(x => x.First()).ToList();
        public static void RecordsAllDelete(this DatabasesContext context)
        {
            context.Records.RemoveRange(context.Records);
            context.SaveChanges();
        }

        public static void RecordsUpdateOrAdd(this DatabasesContext context, List<User> GuncellenecekKayitlar)
        {
            GuncellenecekKayitlar.ForEach(x => {
                var y = context.Records.FirstOrDefault(x => x.Username == x.Username); y = x;
            });
            context.SaveChanges();
            /* context.RecordsDeleteExist(GuncellenecekKayitlar);
             context.RecordsAdd(GuncellenecekKayitlar);*/
        }

        private static void RecordsDeleteExist(this DatabasesContext context, List<User> guncelTablo)
        {
            foreach (User item in guncelTablo)
            {
                User silinecek = context.Records.FirstOrDefault(x => x.Username == item.Username);
                if (silinecek != null)
                    context.Records.Remove(silinecek);
            }
        }

        private static void RecordsAdd(this DatabasesContext context, List<User> EklenecekKayitlar)
        {
            context.Records.AddRange(EklenecekKayitlar.DistinctByUserName());
            context.SaveChanges();
        }
        public static void UpdateFollowStatus(this DatabasesContext context, List<User> takipEdilenler)
        {
            var DBTakipEdilenler = context.Records.Where(x => x.FollowStatus == "Takip ediliyor");
            foreach (var item in DBTakipEdilenler)
            {
                if (takipEdilenler.FirstOrDefault(y => y.Username == item.Username) == null)
                    item.FollowStatus = "Takip et";
            }
            context.SaveChanges();
        }

        /* public static void KayitlarıListelereBol()
         {
             DatabasesContext context = new();
             Hesap.Ins.Takipciler = context.Records.Where(x => x.FollowersStatus == "Seni takip ediyor").ToList();
             Hesap.Ins.TakipEdilenler = context.Records.Where(x => x.FollowersStatus == "Takip ediliyor").ToList();
             Hesap.Ins.GeriTakipYapmayanlar = context.Records.Where(x => x.FollowersStatus == "Takip ediliyor" && x.FollowersStatus != "Seni takip ediyor").ToList();
         }*/
    }
}