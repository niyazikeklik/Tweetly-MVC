
using Tweetly_MVC.Models;

namespace Tweetly_MVC
{
    public class SettingsMethod
    {
        public static class TakipEdilenleriGetir
        {
            public static bool UseDB { get; set; }
            public static DatabasesContext Context { get; set; }

        }
    }
}
