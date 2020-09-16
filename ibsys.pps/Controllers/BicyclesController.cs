using IBSYS.PPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BicyclesController : ControllerBase
    {
        private readonly ILogger<BicyclesController> _logger;

        private readonly IbsysDatabaseContext _db;

        public BicyclesController(ILogger<BicyclesController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET - All bicycle by product name
        [HttpGet]
        public async Task<List<BillOfMaterial>> GetAllBicycles()
        {
            var listOfBicycles = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .ToListAsync();

            foreach (var b in listOfBicycles)
            {
                foreach (var material in b.RequiredMaterials)
                {
                    material.MaterialNeeded = await GetNestedMaterials(material);
                }
            }

            return listOfBicycles;
        }

        // GET - One bicycle with parts by product name
        [HttpGet("{id}")]
        public async Task<BillOfMaterial> GetOneBicycle(string id)
        {
            var bicycle = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .FirstOrDefaultAsync(b => b.ProductName == id);

            foreach (var material in bicycle.RequiredMaterials)
            {
                material.MaterialNeeded = await GetNestedMaterials(material);
            }

            return bicycle;
        }

        // GET - All labor and machine costs
        [HttpGet("laborandmachinecosts")]
        public async Task<List<LaborAndMachineCosts>> GetLaborAndMachineCosts()
        {
            var laborAndMachineCosts = await _db.LaborAndMachineCosts
                .AsNoTracking()
                .Select(l => l)
                .ToListAsync();

            
            return laborAndMachineCosts;
        }
        
        // GET - All self production items
        [HttpGet("selfproductionitems")]
        public async Task<List<SelfProductionItems>> GetSelfProductionItems()
        {
            var selfProductionItems = await _db.SelfProductionItems
                .AsNoTracking()
                .Select(s => s)
                .ToListAsync();


            return selfProductionItems;
        }

        // GET - All purchased items
        [HttpGet("purchaseditems")]
        public async Task<List<PurchasedItems>> GetPurchasedItems()
        {
            var purchasedItems = await _db.PurchasedItems
                .AsNoTracking()
                .Select(p => p)
                .ToListAsync();


            return purchasedItems;
        }


        public async Task<List<Material>> GetNestedMaterials(Material m)
        {
            var nestedMaterials = await _db.Materials
                .AsNoTracking()
                .Include(nm => nm.ParentMaterial)
                .Where(nm => nm.ParentMaterial.ID.Equals(m.ID))
                .Select(nm => nm)
                .ToListAsync();


            m.MaterialNeeded = new List<Material>();

            if (nestedMaterials.Count != 0)
            {
                foreach (var nm in nestedMaterials)
                {
                    nm.MaterialNeeded = await GetNestedMaterials(nm);
                }
                m.MaterialNeeded = nestedMaterials;
            }

            return m.MaterialNeeded;
        }
    }
}
