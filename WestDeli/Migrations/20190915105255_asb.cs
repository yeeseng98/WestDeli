using Microsoft.EntityFrameworkCore.Migrations;

namespace WestDeli.Migrations
{
    public partial class asb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalTime",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TotalTime",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
