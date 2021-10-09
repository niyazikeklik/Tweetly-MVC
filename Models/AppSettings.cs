using Microsoft.Extensions.Configuration;

namespace Tweetly_MVC.Models
{
    public static class AppSettings
    {
        public class UserSettings
        {
            public string Username { get; set; }
            public string Pass { get; set; }
            public string SQLServerName { get; set; }
        }
        public static IConfiguration Configuration { get; set; }
        public static UserSettings Get()
        {
            IConfiguration configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", true, true)
           .AddJsonFile("appsettings.Development.json", true, true)
           .Build();
            return configuration.GetSection("UserSettings")
               .Get<UserSettings>();
        }
    }
}