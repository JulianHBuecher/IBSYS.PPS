using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    ProductName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfMaterials", x => x.ProductName);
                });

            migrationBuilder.CreateTable(
                name: "DispositionEParts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    OrdersInQueueInherit = table.Column<string>(type: "text", nullable: true),
                    PlannedWarehouseFollowing = table.Column<string>(type: "text", nullable: true),
                    WarehouseStockPassed = table.Column<string>(type: "text", nullable: true),
                    OrdersInQueueOwn = table.Column<string>(type: "text", nullable: true),
                    Wip = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Optimized = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositionEParts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Forecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    P1 = table.Column<string>(type: "text", nullable: true),
                    P2 = table.Column<string>(type: "text", nullable: true),
                    P3 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FutureInwardStockMovement",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Orderperiod = table.Column<string>(type: "text", nullable: true),
                    Id = table.Column<string>(type: "text", nullable: true),
                    Mode = table.Column<string>(type: "text", nullable: true),
                    Article = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<string>(type: "text", nullable: true),
                    Time = table.Column<string>(type: "text", nullable: true),
                    Materialcosts = table.Column<string>(type: "text", nullable: true),
                    Ordercosts = table.Column<string>(type: "text", nullable: true),
                    Entirecosts = table.Column<string>(type: "text", nullable: true),
                    Piececosts = table.Column<string>(type: "text", nullable: true),
                    Period = table.Column<string>(type: "text", nullable: true),
                    Item = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<string>(type: "text", nullable: true),
                    Averageunitcosts = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FutureInwardStockMovement", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "InitialStock",
                columns: table => new
                {
                    ItemNumber = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Usage = table.Column<string>(type: "text", nullable: true),
                    ItemValue = table.Column<double>(type: "double precision", nullable: false),
                    QuantityInStock = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    DiscountQuantity = table.Column<int>(type: "integer", nullable: true),
                    OrderCosts = table.Column<double>(type: "double precision", nullable: true),
                    ProcureLeadTime = table.Column<double>(type: "double precision", nullable: true),
                    Deviation = table.Column<double>(type: "double precision", nullable: true),
                    ProcessingTime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialStock", x => x.ItemNumber);
                });

            migrationBuilder.CreateTable(
                name: "LaborAndMachineCosts",
                columns: table => new
                {
                    Workplace = table.Column<int>(type: "integer", nullable: false),
                    LaborCostsFirstShift = table.Column<double>(type: "double precision", nullable: false),
                    LaborCostsSecondShift = table.Column<double>(type: "double precision", nullable: false),
                    LaborCostsThirdShift = table.Column<double>(type: "double precision", nullable: false),
                    LaborCostsOvertime = table.Column<double>(type: "double precision", nullable: false),
                    ProductiveMachineCosts = table.Column<double>(type: "double precision", nullable: false),
                    IdleTimeMachineCosts = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborAndMachineCosts", x => x.Workplace);
                });

            migrationBuilder.CreateTable(
                name: "OrdersForK",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartName = table.Column<string>(type: "text", nullable: true),
                    OrderQuantity = table.Column<string>(type: "text", nullable: true),
                    OrderModus = table.Column<int>(type: "integer", nullable: false),
                    AdditionalParts = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersForK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdersInWork",
                columns: table => new
                {
                    WaitinglistForOrdersInWorkId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<string>(type: "text", nullable: true),
                    Period = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<string>(type: "text", nullable: true),
                    Item = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    TimeNeed = table.Column<int>(type: "integer", nullable: false),
                    Batch = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersInWork", x => x.WaitinglistForOrdersInWorkId);
                });

            migrationBuilder.CreateTable(
                name: "PlannedWarehouseStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Part = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    ReferenceToBicycle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedWarehouseStocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bicycle = table.Column<string>(type: "text", nullable: true),
                    Orders = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellDirectItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<string>(type: "text", nullable: true),
                    Penalty = table.Column<string>(type: "text", nullable: true),
                    Article = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellDirectItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetupEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumberOfSetupEvents = table.Column<int>(type: "integer", nullable: false),
                    WorkplaceId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetupEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockValuesFromLastPeriod",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<string>(type: "text", nullable: true),
                    Startamount = table.Column<string>(type: "text", nullable: true),
                    Pct = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<string>(type: "text", nullable: true),
                    Stockvalue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockValuesFromLastPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForStock",
                columns: table => new
                {
                    MissingPartId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForStock", x => x.MissingPartId);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForWorkstations",
                columns: table => new
                {
                    WaitinglistForWorkstationsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkplaceId = table.Column<string>(type: "text", nullable: true),
                    TimeNeed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForWorkstations", x => x.WaitinglistForWorkstationsId);
                });

            migrationBuilder.CreateTable(
                name: "Workingtimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Station = table.Column<string>(type: "text", nullable: true),
                    Shift = table.Column<string>(type: "text", nullable: true),
                    Overtime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workingtimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialName = table.Column<string>(type: "text", nullable: true),
                    QuantityNeeded = table.Column<int>(type: "integer", nullable: false),
                    DirectAccess = table.Column<string>(type: "text", nullable: true),
                    ParentMaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                    BillOfMaterialProductName = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<string>(type: "text", nullable: true),
                    Cycletime = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<string>(type: "text", nullable: true),
                    OrderID = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "WaitinglistForStockWorkplaces",
                columns: table => new
                {
                    WaitinglistForStockId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkplaceId = table.Column<string>(type: "text", nullable: true),
                    TimeNeed = table.Column<int>(type: "integer", nullable: false),
                    MissingPartInStockMissingPartId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForStockWorkplaces", x => x.WaitinglistForStockId);
                    table.ForeignKey(
                        name: "FK_WaitinglistForStockWorkplaces_WaitinglistForStock_MissingPa~",
                        column: x => x.MissingPartInStockMissingPartId,
                        principalTable: "WaitinglistForStock",
                        principalColumn: "MissingPartId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForWorkplace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WaitinglistForWorkstationsId = table.Column<int>(type: "integer", nullable: true),
                    Period = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<string>(type: "text", nullable: true),
                    Item = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    TimeNeed = table.Column<int>(type: "integer", nullable: false),
                    Batch = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForWorkplace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitinglistForWorkplace_WaitinglistForWorkstations_Waitingl~",
                        column: x => x.WaitinglistForWorkstationsId,
                        principalTable: "WaitinglistForWorkstations",
                        principalColumn: "WaitinglistForWorkstationsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WaitinglistForWorkplaceStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WaitinglistForStockId = table.Column<int>(type: "integer", nullable: true),
                    Period = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<string>(type: "text", nullable: true),
                    Item = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    TimeNeed = table.Column<int>(type: "integer", nullable: false),
                    Batch = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitinglistForWorkplaceStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitinglistForWorkplaceStock_WaitinglistForStockWorkplaces_~",
                        column: x => x.WaitinglistForStockId,
                        principalTable: "WaitinglistForStockWorkplaces",
                        principalColumn: "WaitinglistForStockId",
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

            migrationBuilder.CreateIndex(
                name: "IX_WaitinglistForStockWorkplaces_MissingPartInStockMissingPart~",
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
                name: "Batch");

            migrationBuilder.DropTable(
                name: "DispositionEParts");

            migrationBuilder.DropTable(
                name: "Forecasts");

            migrationBuilder.DropTable(
                name: "InitialStock");

            migrationBuilder.DropTable(
                name: "LaborAndMachineCosts");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "OrdersForK");

            migrationBuilder.DropTable(
                name: "OrdersInWork");

            migrationBuilder.DropTable(
                name: "PlannedWarehouseStocks");

            migrationBuilder.DropTable(
                name: "ProductionOrders");

            migrationBuilder.DropTable(
                name: "SellDirectItems");

            migrationBuilder.DropTable(
                name: "SetupEvents");

            migrationBuilder.DropTable(
                name: "StockValuesFromLastPeriod");

            migrationBuilder.DropTable(
                name: "WaitinglistForWorkplace");

            migrationBuilder.DropTable(
                name: "WaitinglistForWorkplaceStock");

            migrationBuilder.DropTable(
                name: "Workingtimes");

            migrationBuilder.DropTable(
                name: "FutureInwardStockMovement");

            migrationBuilder.DropTable(
                name: "BillOfMaterials");

            migrationBuilder.DropTable(
                name: "WaitinglistForWorkstations");

            migrationBuilder.DropTable(
                name: "WaitinglistForStockWorkplaces");

            migrationBuilder.DropTable(
                name: "WaitinglistForStock");
        }
    }
}
