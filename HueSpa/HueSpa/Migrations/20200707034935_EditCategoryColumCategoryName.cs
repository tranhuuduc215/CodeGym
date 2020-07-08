using Microsoft.EntityFrameworkCore.Migrations;

namespace HueSpa.Migrations
{
    public partial class EditCategoryColumCategoryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategotyName",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CategotyName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
