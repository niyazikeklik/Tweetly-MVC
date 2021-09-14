using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetly_MVC.Tweetly
{
    public class DatabasesContext:DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=TweetlyDataBase_"+Hesap.Instance.OturumBilgileri.Username+";Integrated Security=True");
            //Migrate komutu çalıştırırken cümleden usernameyi sil.
        }
        public DbSet<TakipEdilen> TakipEdilenler { get;set;}
        public DbSet<Takipci> Takipciler { get; set; }
        public DbSet<GeriTakipEtmeyen> GeriTakipEtmeyenler { get; set; }
    }
}
