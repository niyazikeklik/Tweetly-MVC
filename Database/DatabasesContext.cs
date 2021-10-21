using Microsoft.EntityFrameworkCore;
using System;
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
        public static void UpdateTemelBilgiler(this DatabasesContext context, List<User> takipEdilenler)
        {
            int count = 0;
            context.Records.ForEachAsync(DbUser => {
                var guncel = takipEdilenler.FirstOrDefault(x => x.Username == DbUser.Username);
                if(guncel != null)
                {
                    DbUser.Count = ++count;
                    DbUser.PhotoURL = guncel.PhotoURL;
                    DbUser.FollowStatus = guncel.FollowStatus;
                    DbUser.FollowersStatus = guncel.FollowersStatus;
                    DbUser.Name = guncel.Name;
                    DbUser.IsPrivate = guncel.IsPrivate;
                    DbUser.Bio = guncel.Bio;
                    DbUser.Cinsiyet = 
                    DbUser.Cinsiyet.Equals("Belirsiz") ? guncel.Cinsiyet : DbUser.Cinsiyet;
                }
                else
                {
                    DbUser.FollowStatus = takipEdilenler.First().FollowStatus.Equals("Takip ediliyor") ? "Takip et" : "Takip ediliyor";
                }
            
            }).Wait();
            context.SaveChanges();
            /*     context.Records.ForEachAsync(y => y.FollowStatus = "Takip et").Wait();

                 takipEdilenler.ForEach(x => {

                     var DBuser = context.Records.FirstOrDefault(y => x.Username == y.Username);
                     if (DBuser != null)
                     {
                         DBuser.Count = ++count;
                         DBuser.PhotoURL = x.PhotoURL;
                         DBuser.FollowStatus = x.FollowStatus;
                         DBuser.FollowersStatus = x.FollowersStatus;
                         DBuser.Name = x.Name;
                         DBuser.IsPrivate = x.IsPrivate;
                         DBuser.Bio = x.Bio;
                         DBuser.Cinsiyet = DBuser.Cinsiyet.Equals("Belirsiz") ? x.Cinsiyet : DBuser.Cinsiyet;
                     }

                 });*/


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