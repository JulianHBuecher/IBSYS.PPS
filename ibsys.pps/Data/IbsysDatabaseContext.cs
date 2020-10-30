﻿using IBSYS.PPS.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DbSet<Order> FutureInwardStockMovement { get; set; }
        public DbSet<WaitinglistForWorkstations> WaitinglistWorkstations { get; set; }
        public DbSet<MissingPartInStock> WaitinglistStock { get; set; }
        public DbSet<WaitinglistForOrdersInWork> OrdersInWork { get; set; }
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
}
