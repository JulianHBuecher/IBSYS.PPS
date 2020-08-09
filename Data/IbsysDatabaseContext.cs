using Microsoft.EntityFrameworkCore;

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
        public DbSet<Stock> Stock { get; set; }
        public DbSet<SelfProductionItems> SelfProductionItems{ get; set; }
        public DbSet<PurchasedItems> PurchasedItems{ get; set; }
        public DbSet<LaborAndMachineCosts> LaborAndMachineCosts{ get; set; }
    }
}
