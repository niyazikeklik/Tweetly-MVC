using Microsoft.EntityFrameworkCore.Migrations;

namespace Tweetly_MVC.Migrations
{
    public partial class GeriTakip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPrivate",
                table: "TakipEdilenler",
                newName: "IsPrivate");

            migrationBuilder.RenameColumn(
                name: "isPrivate",
                table: "Takipciler",
                newName: "IsPrivate");

            migrationBuilder.CreateTable(
                name: "GeriTakipEtmeyenler",
                columns: table => new
                {
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
                constraints: table => table.PrimaryKey("PK_GeriTakipEtmeyenler", x => x.Username));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeriTakipEtmeyenler");

            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "TakipEdilenler",
                newName: "isPrivate");

            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "Takipciler",
                newName: "isPrivate");
        }
    }
}