using Microsoft.EntityFrameworkCore.Migrations;

namespace Tweetly_MVC.Migrations
{
    public partial class Userupate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BegeniOrani",
                table: "Records",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BegeniSayisi",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BegeniOrani",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "BegeniSayisi",
                table: "Records");
        }
    }
}
