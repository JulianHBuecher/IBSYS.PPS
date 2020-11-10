using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Models.Generated;
using IBSYS.PPS.Models.Input;
using IBSYS.PPS.Models.Materialplanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace IBSYS.PPS.Models
{
    public class IbsysDatabaseContext : DbContext
    {
        public IbsysDatabaseContext (DbContextOptions<IbsysDatabaseContext> options) 
            : base(options)
        {

        }
        public DbSet<BillOfMaterial> BillOfMaterials { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Stock> InitialStock { get; set; }
        public DbSet<SelfProductionItems> SelfProductionItems { get; set; }
        public DbSet<PurchasedItems> PurchasedItems { get; set; }
        public DbSet<LaborAndMachineCosts> LaborAndMachineCosts { get; set; }
        public DbSet<Generated.Article> StockValuesFromLastPeriod { get; set; }
        public DbSet<Generated.Order> FutureInwardStockMovement { get; set; }
        public DbSet<WaitinglistForWorkstations> WaitinglistWorkstations { get; set; }
        public DbSet<MissingPartInStock> WaitinglistStock { get; set; }
        public DbSet<WaitinglistForOrdersInWork> OrdersInWork { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<PlannedWarehouseStock> PlannedWarehouseStocks { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<SellDirectItem> SellDirectItems { get; set; }
        public DbSet<OrderForK> OrdersForK { get; set; }
        public DbSet<BicyclePart> DispositionEParts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<double[], string>(
                v => string.Join(";",v),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(val => double.Parse(val)).ToArray());

            modelBuilder.Entity<ProductionOrder>()
                .Property(e => e.Orders)
                .HasConversion(converter);
        }
    }

    [Table("WaitinglistForWorkstations")]
    public class WaitinglistForWorkstations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WaitinglistForWorkstationsId { get; set; }
        public string WorkplaceId { get; set; }
        public int TimeNeed { get; set; }
        public List<WaitinglistForWorkplace> WaitingListForWorkplace { get; set; }
    }

    [Table("WaitinglistForStock")]
    public class MissingPartInStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MissingPartId { get; set; }
        public string Id { get; set; }
        public List<WaitinglistForStock> WaitinglistForStock { get; set; }
    }

    [Table("WaitinglistForStockWorkplaces")]
    public class WaitinglistForStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WaitinglistForStockId { get; set; }
        public string WorkplaceId { get; set; }
        public int TimeNeed { get; set; }
        public List<WaitinglistForWorkplaceStock> WaitinglistForWorkplaceStock { get; set; }

        // Foreign Key
        public MissingPartInStock MissingPartInStock { get; set; }
    }

    [Table("OrdersInWork")]
    public class WaitinglistForOrdersInWork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WaitinglistForOrdersInWorkId { get; set; }
        public string Id { get; set; }
        public string Period { get; set; }
        public string Order { get; set; }
        public string Item { get; set; }
        public int Amount { get; set; }
        public int TimeNeed { get; set; }
        public int Batch { get; set; }
    }

    public class WaitinglistBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Period { get; set; }
        public string Order { get; set; }
        public string Item { get; set; }
        public int Amount { get; set; }
        public int TimeNeed { get; set; }
        // To differ between the waitinglist amounts
        public int Batch { get; set; }
    }

    [Table("WaitinglistForWorkplace")]
    public class WaitinglistForWorkplace : WaitinglistBase
    {
        public WaitinglistForWorkstations WaitinglistForWorkstations { get; set; }
    }
    
    [Table("WaitinglistForWorkplaceStock")]
    public class WaitinglistForWorkplaceStock : WaitinglistBase
    {
        public WaitinglistForStock WaitinglistForStock { get; set; }
    }

    [Table("ProductionOrders")]
    public class ProductionOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string Bicycle { get; set; }
        public double[] Orders { get; set; }
    }

    [Table("PlannedWarehouseStocks")]
    public class PlannedWarehouseStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string Part { get; set; }
        public int Amount { get; set; }
    }
}
