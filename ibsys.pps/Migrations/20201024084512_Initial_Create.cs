using Microsoft.EntityFrameworkCore.Migrations;

namespace IBSYS.PPS.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillOfMaterials",
                columns: table => new
                {
                    ProductName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfMaterials", x => x.ProductName);
                });

            migrationBuilder.CreateTable(
                name: "FutureInwardStockMovement",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Orderperiod = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: true),
                    Mode = table.Column<string>(nullable: true),
                    Article = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Materialcosts = table.Column<string>(nullable: true),
                    Ordercosts = table.Column<string>(nullable: true),
                    Entirecosts = table.Column<string>(nullable: true),
                    Piececosts = table.Column<string>(nullable: true),
                    Period = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Cost = table.Column<string>(nullable: true),
                    Averageunitcosts = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FutureInwardStockMovement", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "InitialStock",
                columns: table => new
                {
                    ItemNumber = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Usage = table.Column<string>(nullable: true),
                    ItemValue = table.Column<double>(nullable: false),
                    QuantityInStock = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    DiscountQuantity = table.Column<int>(nullable: true),
                    OrderCosts = table.Column<double>(nullable: true),
                    ProcureLeadTime = table.Column<double>(nullable: true),
                    Deviation = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialStock", x => x.ItemNumber);
                });

            migrationBuilder.CreateTable(
                name: "LaborAndMachineCosts",
                columns: table => new
                {
                    Workplace = table.Column<int>(nullable: false),
                    LaborCostsFirstShift = table.Column<double>(nullable: false),
                    LaborCostsSecondShift = table.Column<double>(nullable: false),
                    LaborCostsThirdShift = table.Column<double>(nullable: false),
                    LaborCostsOvertime = table.Column<double>(nullable: false),
                    ProductiveMachineCosts = table.Column<double>(nullable: false),
                    IdleTimeMachineCosts = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborAndMachineCosts", x => x.Workplace);
                });

            migrationBuilder.CreateTable(
                name: "StockValuesFromLastPeriod",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<string>(nullable: true),
                    Startamount = table.Column<string>(nullable: true),
                    Pct = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    Stockvalue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockValuesFromLastPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialName = table.Column<string>(nullable: true),
                    QuantityNeeded = table.Column<int>(nullable: false),
                    ParentMaterialId = table.Column<int>(nullable: true),
                    BillOfMaterialProductName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Materials_BillOfMaterials_BillOfMaterialProductName",
                        column: x => x.BillOfMaterialProductName,
                        principalTable: "BillOfMaterials",
                        principalColumn: "ProductName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_Materials_ParentMaterialId",
                        column: x => x.ParentMaterialId,
                        principalTable: "Materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<string>(nullable: true),
                    Cycletime = table.Column<string>(nullable: true),
                    Cost = table.Column<string>(nullable: true),
                    OrderID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batch_FutureInwardStockMovement_OrderID",
                        column: x => x.OrderID,
                        principalTable: "FutureInwardStockMovement",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batch_OrderID",
                table: "Batch",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_BillOfMaterialProductName",
                table: "Materials",
                column: "BillOfMaterialProductName");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ParentMaterialId",
                table: "Materials",
                column: "ParentMaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.DropTable(
                name: "InitialStock");

            migrationBuilder.DropTable(
                name: "LaborAndMachineCosts");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "StockValuesFromLastPeriod");

            migrationBuilder.DropTable(
                name: "FutureInwardStockMovement");

            migrationBuilder.DropTable(
                name: "BillOfMaterials");
        }
    }
}
