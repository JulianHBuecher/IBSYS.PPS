using Microsoft.EntityFrameworkCore.Migrations;

namespace IBSYS.PPS.Migrations
{
    public partial class Production_Orders_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdersInWork",
                columns: table => new
                {
                    WaitinglistForOrdersInWorkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(nullable: true),
                    Period = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    TimeNeed = table.Column<int>(nullable: false),
                    Batch = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersInWork", x => x.WaitinglistForOrdersInWorkId);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bicycle = table.Column<string>(nullable: true),
                    Orders = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForStock",
                columns: table => new
                {
                    MissingPartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForStock", x => x.MissingPartId);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForWorkstations",
                columns: table => new
                {
                    WaitinglistForWorkstationsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkplaceId = table.Column<string>(nullable: true),
                    TimeNeed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForWorkstations", x => x.WaitinglistForWorkstationsId);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForStockWorkplaces",
                columns: table => new
                {
                    WaitinglistForStockId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkplaceId = table.Column<string>(nullable: true),
                    TimeNeed = table.Column<int>(nullable: false),
                    MissingPartInStockMissingPartId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForStockWorkplaces", x => x.WaitinglistForStockId);
                    table.ForeignKey(
                        name: "FK_WaitinglistForStockWorkplaces_WaitinglistForStock_MissingPartInStockMissingPartId",
                        column: x => x.MissingPartInStockMissingPartId,
                        principalTable: "WaitinglistForStock",
                        principalColumn: "MissingPartId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForWorkplace",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    TimeNeed = table.Column<int>(nullable: false),
                    Batch = table.Column<int>(nullable: false),
                    WaitinglistForWorkstationsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForWorkplace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitinglistForWorkplace_WaitinglistForWorkstations_WaitinglistForWorkstationsId",
                        column: x => x.WaitinglistForWorkstationsId,
                        principalTable: "WaitinglistForWorkstations",
                        principalColumn: "WaitinglistForWorkstationsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForWorkplaceStock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    TimeNeed = table.Column<int>(nullable: false),
                    Batch = table.Column<int>(nullable: false),
                    WaitinglistForStockId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForWorkplaceStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitinglistForWorkplaceStock_WaitinglistForStockWorkplaces_WaitinglistForStockId",
                        column: x => x.WaitinglistForStockId,
                        principalTable: "WaitinglistForStockWorkplaces",
                        principalColumn: "WaitinglistForStockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaitinglistForStockWorkplaces_MissingPartInStockMissingPartId",
                table: "WaitinglistForStockWorkplaces",
                column: "MissingPartInStockMissingPartId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitinglistForWorkplace_WaitinglistForWorkstationsId",
                table: "WaitinglistForWorkplace",
                column: "WaitinglistForWorkstationsId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitinglistForWorkplaceStock_WaitinglistForStockId",
                table: "WaitinglistForWorkplaceStock",
                column: "WaitinglistForStockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersInWork");

            migrationBuilder.DropTable(
                name: "ProductionOrders");

            migrationBuilder.DropTable(
                name: "WaitinglistForWorkplace");

            migrationBuilder.DropTable(
                name: "WaitinglistForWorkplaceStock");

            migrationBuilder.DropTable(
                name: "WaitinglistForWorkstations");

            migrationBuilder.DropTable(
                name: "WaitinglistForStockWorkplaces");

            migrationBuilder.DropTable(
                name: "WaitinglistForStock");
        }
    }
}
