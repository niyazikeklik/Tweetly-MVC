using Microsoft.EntityFrameworkCore.Migrations;

namespace Tweetly_MVC.Migrations
{
    public partial class TweetlyMVC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isPrivate = table.Column<bool>(type: "bit", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Followers = table.Column<int>(type: "int", nullable: true),
                    Following = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<double>(type: "float", nullable: true),
                    TweetCount = table.Column<int>(type: "int", nullable: true),
                    LastTweetsDate = table.Column<double>(type: "float", nullable: true),
                    TweetSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: true),
                    LastLikesDate = table.Column<double>(type: "float", nullable: true),
                    BegeniSikligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowersStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
