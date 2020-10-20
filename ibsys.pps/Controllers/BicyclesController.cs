using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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

        // GET - Disposition of all bicycles
        [HttpGet("disposition/{id}")]
        public async Task<ActionResult> GetDisposition(string id)
        {
            var disposition = await ExecuteDisposition(id, 110, 0, 20, 0);

            return Ok(disposition);
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

        public async Task<List<string>> FilterNestedMaterialsByName(string parts, Material ml)
        {
            var partsForBicycle = new List<string>();

            if (ml.MaterialName.StartsWith(parts))
            {
                partsForBicycle.Add(ml.MaterialName);
                if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded.Count != 0)
                {
                    foreach (var material in ml.MaterialNeeded)
                    {
                        partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, material));
                    }
                }
            }
            else
            {
                if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded.Count != 0)
                {
                    foreach (var material in ml.MaterialNeeded)
                    {
                        partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, material));
                    }
                }
            }

            return partsForBicycle;
        }

        public async Task<Bicycle> ExecuteDisposition(string id, int salesOrders, int ordersInQueue, 
            int wareHouseStockAfter, int wip)
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

            var partsForDisposition = new List<string>();
            partsForDisposition.Add(id);

            foreach (var material in bicycle.RequiredMaterials)
            {
                partsForDisposition.AddRange(await FilterNestedMaterialsByName("E", material));
            }

            var disposition = await CalculateParts(partsForDisposition, salesOrders, ordersInQueue, wareHouseStockAfter, wip);

            return new Bicycle { parts = disposition };
        }

        public async Task<List<BicyclePart>> CalculateParts(List<string> partsForDisposition,
            int salesOrders, int ordersInQueue, int wareHouseStockAfter, int wip)
        {
            var requiredMaterials = new List<BicyclePart>();

            var storedParts = new List<int>();

            foreach (var material in partsForDisposition)
            {
                var stockQuantity = await _db.Stock.AsNoTracking()
                    .Where(m => m.ItemNumber.Contains(material))
                    .Select(m => m.QuantityInStock).FirstOrDefaultAsync();

                storedParts.Add(stockQuantity);
            }

            var bicycle = Math.Max(0,salesOrders + wareHouseStockAfter - storedParts[0] - ordersInQueue - wip);
            var firstPart = Math.Max(0,bicycle + ordersInQueue + wareHouseStockAfter - storedParts[1] - ordersInQueue - wip);
            var secondpart = Math.Max(0,bicycle + ordersInQueue + wareHouseStockAfter - storedParts[2] - ordersInQueue - wip);
            var thirdPart = Math.Max(0,secondpart + ordersInQueue + wareHouseStockAfter - storedParts[3] - ordersInQueue - wip);
            var fourthPart = Math.Max(0,secondpart + ordersInQueue + wareHouseStockAfter - storedParts[4] - ordersInQueue - wip);
            var fifthPart = Math.Max(0,secondpart + ordersInQueue + wareHouseStockAfter - storedParts[5] - ordersInQueue - wip);
            var sixthPart = Math.Max(0, fifthPart + ordersInQueue + wareHouseStockAfter - storedParts[6] - ordersInQueue - wip);
            var seventhPart = Math.Max(0, fifthPart + ordersInQueue + wareHouseStockAfter - storedParts[7] - ordersInQueue - wip);
            var eigthPart = Math.Max(0, fifthPart + ordersInQueue + wareHouseStockAfter - storedParts[8] - ordersInQueue - wip);
            var ninethPart = Math.Max(0, eigthPart + ordersInQueue + wareHouseStockAfter - storedParts[9] - ordersInQueue - wip);
            var tenthPart = Math.Max(0, eigthPart + ordersInQueue + wareHouseStockAfter - storedParts[10] - ordersInQueue - wip);
            var eleventhPart = Math.Max(0, eigthPart + ordersInQueue + wareHouseStockAfter - storedParts[11] - ordersInQueue - wip);


            requiredMaterials.AddRange(new BicyclePart[]
            {
                new BicyclePart(partsForDisposition[0],bicycle.ToString()),
                new BicyclePart(partsForDisposition[1],firstPart.ToString()),
                new BicyclePart(partsForDisposition[2],secondpart.ToString()),
                new BicyclePart(partsForDisposition[3],thirdPart.ToString()),
                new BicyclePart(partsForDisposition[4],fourthPart.ToString()),
                new BicyclePart(partsForDisposition[5],fifthPart.ToString()),
                new BicyclePart(partsForDisposition[6],sixthPart.ToString()),
                new BicyclePart(partsForDisposition[7],seventhPart.ToString()),
                new BicyclePart(partsForDisposition[8],eigthPart.ToString()),
                new BicyclePart(partsForDisposition[9],ninethPart.ToString()),
                new BicyclePart(partsForDisposition[10],tenthPart.ToString()),
                new BicyclePart(partsForDisposition[11],eleventhPart.ToString())
            });


            return requiredMaterials;
        }
    }
}
