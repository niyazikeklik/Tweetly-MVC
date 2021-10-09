using Microsoft.EntityFrameworkCore.Migrations;

namespace Tweetly_MVC.Migrations
{
    public partial class TabloChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeriTakipEtmeyenler");

            migrationBuilder.DropTable(
                name: "Takipciler");

            migrationBuilder.DropTable(
                name: "TakipEdilenler");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Followers = table.Column<int>(type: "int", nullable: false),
                    Following = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<double>(type: "float", nullable: false),
                    TweetCount = table.Column<int>(type: "int", nullable: false),
                    LastTweetsDate = table.Column<double>(type: "float", nullable: false),
                    TweetSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    LastLikesDate = table.Column<double>(type: "float", nullable: false),
                    BegeniSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowersStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_User", x => x.Username));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.CreateTable(
                name: "GeriTakipEtmeyenler",
                columns: table => new {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BegeniSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<double>(type: "float", nullable: false),
                    FollowStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Followers = table.Column<int>(type: "int", nullable: false),
                    FollowersStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Following = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    LastLikesDate = table.Column<double>(type: "float", nullable: false),
                    LastTweetsDate = table.Column<double>(type: "float", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TweetCount = table.Column<int>(type: "int", nullable: false),
                    TweetSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_GeriTakipEtmeyenler", x => x.Username));

            migrationBuilder.CreateTable(
                name: "Takipciler",
                columns: table => new {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BegeniSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<double>(type: "float", nullable: false),
                    FollowStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Followers = table.Column<int>(type: "int", nullable: false),
                    FollowersStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Following = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    LastLikesDate = table.Column<double>(type: "float", nullable: false),
                    LastTweetsDate = table.Column<double>(type: "float", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TweetCount = table.Column<int>(type: "int", nullable: false),
                    TweetSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_Takipciler", x => x.Username));

            migrationBuilder.CreateTable(
                name: "TakipEdilenler",
                columns: table => new {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BegeniSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<double>(type: "float", nullable: false),
                    FollowStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Followers = table.Column<int>(type: "int", nullable: false),
                    FollowersStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Following = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    LastLikesDate = table.Column<double>(type: "float", nullable: false),
                    LastTweetsDate = table.Column<double>(type: "float", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TweetCount = table.Column<int>(type: "int", nullable: false),
                    TweetSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_TakipEdilenler", x => x.Username));
        }
    }
}