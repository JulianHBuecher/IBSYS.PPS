using Microsoft.EntityFrameworkCore.Migrations;

namespace IBSYS.PPS.Migrations
{
    public partial class InitialCreate : Migration
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
                name: "Stock",
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
                    table.PrimaryKey("PK_Stock", x => x.ItemNumber);
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
                name: "LaborAndMachineCosts");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "BillOfMaterials");
        }
    }
}
