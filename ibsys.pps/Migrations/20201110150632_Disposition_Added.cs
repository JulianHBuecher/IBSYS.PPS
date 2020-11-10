using Microsoft.EntityFrameworkCore.Migrations;

namespace IBSYS.PPS.Migrations
{
    public partial class Disposition_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "SellDirectItems",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "SellDirectItems",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Penalty",
                table: "SellDirectItems",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "DispositionEParts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    OrdersInQueueInherit = table.Column<string>(nullable: true),
                    PlannedWarehouseFollowing = table.Column<string>(nullable: true),
                    WarehouseStockPassed = table.Column<string>(nullable: true),
                    OrdersInQueueOwn = table.Column<string>(nullable: true),
                    Wip = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositionEParts", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispositionEParts");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "SellDirectItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "SellDirectItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Penalty",
                table: "SellDirectItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
