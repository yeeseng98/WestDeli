using Microsoft.EntityFrameworkCore.Migrations;

namespace WestDeli.Migrations
{
    public partial class AddImgLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DishName",
                table: "Dish",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Dish",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgLink",
                table: "Dish",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgLink",
                table: "Dish");

            migrationBuilder.AlterColumn<string>(
                name: "DishName",
                table: "Dish",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Dish",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
