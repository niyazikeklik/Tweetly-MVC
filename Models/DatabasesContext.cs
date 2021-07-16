using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Tweetly_MVC.Models
{
    public class DatabasesContext:DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=TweetlyDataBase;Integrated Security=True");
        }
        public DbSet<User> Users { get;set;}
    }
}
