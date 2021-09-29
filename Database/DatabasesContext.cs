using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tweetly_MVC.Init;

namespace Tweetly_MVC.Tweetly
{
    public class DatabasesContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@$"Data Source={Hesap.Instance.SettingsUser.SQLServerName};Initial Catalog=TweetlyDataBase_{Hesap.Instance.OturumBilgileri.Username};Integrated Security=True");
            //Migrate komutu çalıştırırken cümleden usernameyi sil.
        }//212 331 02 00

        public DbSet<User> Records { get; set; }
    }

    public static class DataBase
    {
        public static void RecordsAllDelete(this DatabasesContext context)
        {
            context.Records.RemoveRange(context.Records);
            context.SaveChanges();
        }

        public static void RecordsUpdateOrAdd(this DatabasesContext context, List<User> GuncellenecekKayitlar)
        {
            context.Records.RecordsDeleteExist(GuncellenecekKayitlar);
            context.RecordsAdd(GuncellenecekKayitlar);
        }

        private static void RecordsDeleteExist(this DbSet<User> DBtablo, List<User> guncelTablo)
        {
            foreach (var item in guncelTablo)
            {
                var silinecek = DBtablo.FirstOrDefault(x => x.Username == item.Username);
                if (silinecek != null)
                    DBtablo.Remove(silinecek);
            }
        }

        private static void RecordsAdd(this DatabasesContext context, List<User> EklenecekKayitlar)
        {
            context.Records.AddRange(EklenecekKayitlar.DistinctByUserName());
            context.SaveChanges();
        }

        public static void KayitlarıListelereBol()
        {
            DatabasesContext context = new();
            Hesap.Instance.Takipciler = context.Records.Where(x => x.FollowersStatus == "Seni takip ediyor").ToList();
            Hesap.Instance.TakipEdilenler = context.Records.Where(x => x.FollowersStatus == "Takip ediliyor").ToList();
            Hesap.Instance.GeriTakipYapmayanlar = context.Records.Where(x => x.FollowersStatus == "Takip ediliyor" && x.FollowersStatus != "Seni takip ediyor").ToList();
        }
    }
}