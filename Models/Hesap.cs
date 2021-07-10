using System.Collections.Generic;

namespace Tweetly_MVC.Models
{
    public static class Hesap
    {
        public static string loginUserName { get; set; }
        public static string loginPass { get; set; }
        public static User OturumBilgileri { get; set; }

        public static List<User> Takipciler = null;

        public static string progressText = "";

    }
}
