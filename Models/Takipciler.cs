using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tweetly_MVC.Models
{
    public class Takipciler
    {
        public int Count { get; set; }
        public string PhotoURL { get; set; }
        public string Name { get; set; }
        [Key]
        public string Username { get; set; }
        public string Cinsiyet { get; set; }
        public string Bio { get; set; }
        public string FollowersStatus { get; set; }
        public string FollowStatus { get; set; }
    }
}
