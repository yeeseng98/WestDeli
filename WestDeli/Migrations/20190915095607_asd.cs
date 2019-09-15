using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WestDeli.Migrations
{
    public partial class asd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionID",
                table: "OrderObject",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactDate = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderObject_TransactionID",
                table: "OrderObject",
                column: "TransactionID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderObject_Transaction_TransactionID",
                table: "OrderObject",
                column: "TransactionID",
                principalTable: "Transaction",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderObject_Transaction_TransactionID",
                table: "OrderObject");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_OrderObject_TransactionID",
                table: "OrderObject");

            migrationBuilder.DropColumn(
                name: "TransactionID",
                table: "OrderObject");
        }
    }
}
