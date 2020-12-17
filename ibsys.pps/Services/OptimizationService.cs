using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IBSYS.PPS.Services
{
    public class OptimizationService
    {
        private readonly ILogger<OptimizationService> _logger;
        private readonly IbsysDatabaseContext _db;

        public OptimizationService(ILogger<OptimizationService> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<OptimizedPart>> OptimizeProductionOrder()
        {
            var partsFromDisposition = await _db.DispositionEParts
                .AsNoTracking()
                .Select(e => e)
                .ToListAsync();

            var bicycleParts = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .ToListAsync();

            var initialStockEParts = await _db.SelfProductionItems
                .AsNoTracking()
                .Select(e => e)
                .ToListAsync();

            foreach (var bicycle in bicycleParts)
            {
                foreach (var material in bicycle.RequiredMaterials)
                {
                    material.MaterialNeeded = await GetNestedMaterials(material);
                }
            }

            var p1 = new BillOfMaterial();
            var p2 = new BillOfMaterial();
            var p3 = new BillOfMaterial();

            var extractedBicyclePOne = new List<Material>();
            var extractedBicyclePTwo = new List<Material>();
            var extractedBicyclePThree = new List<Material>();

            var counter = 0;

            foreach (var bicycle in new List<BillOfMaterial> { p1, p2, p3 })
            {
                bicycle.ProductName = bicycleParts[counter].ProductName;
                bicycle.RequiredMaterials = bicycleParts[counter].RequiredMaterials;
                counter++;
            }

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("E", material);
                extractedBicyclePOne.AddRange(filteredPart);
            }
            foreach (var material in p2.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("E", material);
                extractedBicyclePTwo.AddRange(filteredPart);
            }
            foreach (var material in p3.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("E", material);
                extractedBicyclePThree.AddRange(filteredPart);
            }

            var optimizedParts = new List<OptimizedPart>();

            var summedPartsPOne = SumFilteredMaterials(extractedBicyclePOne);
            var summedPartsPTwo = SumFilteredMaterials(extractedBicyclePTwo);
            var summedPartsPThree = SumFilteredMaterials(extractedBicyclePThree);

            var optimizedPartsP1 = initialStockEParts.Select(e => e).Where(p => extractedBicyclePOne.Select(pp => pp.MaterialName).Contains(p.ItemNumber) || p.ItemNumber == "P 1");
            var optimizedPartsP2 = initialStockEParts.Select(e => e).Where(p => extractedBicyclePTwo.Select(pp => pp.MaterialName).Contains(p.ItemNumber) || p.ItemNumber == "P 2");
            var optimizedPartsP3 = initialStockEParts.Select(e => e).Where(p => extractedBicyclePThree.Select(pp => pp.MaterialName).Contains(p.ItemNumber) || p.ItemNumber == "P 3");

            optimizedPartsP1 = optimizedPartsP1.Select(p => p).OrderByDescending(p => p.ProcessingTime.Split("-")[0]).ToList();
            optimizedPartsP2 = optimizedPartsP2.Select(p => p).OrderByDescending(p => p.ProcessingTime.Split("-")[0]).ToList();
            optimizedPartsP3 = optimizedPartsP3.Select(p => p).OrderByDescending(p => p.ProcessingTime.Split("-")[0]).ToList();

            foreach (var material in optimizedPartsP1)
            {
                var part = partsFromDisposition.Select(p => p)
                        .Where(p => Regex.Match(p.Name, @"\d+").Value.Equals(Regex.Match(material.ItemNumber, @"\d+").Value))
                        .FirstOrDefault();

                UpdateOptimizedList(part, optimizedParts, Convert.ToInt32(material.ProcessingTime.Split("-")[0]));

                partsFromDisposition.Remove(part);
            }

            foreach (var material in optimizedPartsP2)
            {
                var part = partsFromDisposition.Select(p => p)
                        .Where(p => Regex.Match(p.Name, @"\d+").Value.Equals(Regex.Match(material.ItemNumber, @"\d+").Value))
                        .FirstOrDefault();

                UpdateOptimizedList(part, optimizedParts, Convert.ToInt32(material.ProcessingTime.Split("-")[0]));

                partsFromDisposition.Remove(part);
            }

            foreach (var material in optimizedPartsP3)
            {
                var part = partsFromDisposition.Select(p => p)
                        .Where(p => Regex.Match(p.Name, @"\d+").Value.Equals(Regex.Match(material.ItemNumber, @"\d+").Value))
                        .FirstOrDefault();

                UpdateOptimizedList(part, optimizedParts, Convert.ToInt32(material.ProcessingTime.Split("-")[0]));

                partsFromDisposition.Remove(part);
            }
            return optimizedParts;
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

        public async Task<List<Material>> FilterNestedMaterialsByName(string parts, Material ml)
        {
            var partsForBicycle = new List<Material>();

            if (ml.MaterialName.StartsWith(parts))
            {
                partsForBicycle.Add(new Material { MaterialName = ml.MaterialName, QuantityNeeded = ml.QuantityNeeded, DirectAccess = ml.DirectAccess });
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

        public List<Material> SumFilteredMaterials(List<Material> materials)
        {
            var partsOrderedAndSummed = materials.GroupBy(p => p.MaterialName).OrderBy(p => p.Key)
                .Select(p => new Material
                {
                    MaterialName = p.Key,
                    QuantityNeeded = p.Select(pp => pp.QuantityNeeded).Sum(),
                    DirectAccess = p.Select(pp => pp.DirectAccess).First()
                })
                .ToList();

            return partsOrderedAndSummed;
        }

        public void UpdateOptimizedList(BicyclePart part, List<OptimizedPart> list, int processingTime)
        {
            var partExists = list.Select(p => p).Where(p => p.Name.Equals(part.Name)).FirstOrDefault();

            if (partExists != null)
            {
                var index = list.IndexOf(partExists);
                list[index].OrdersInQueueInherit = (Convert.ToInt32(list[index].OrdersInQueueInherit) + Convert.ToInt32(part.OrdersInQueueInherit)).ToString();
                list[index].OrdersInQueueOwn = (Convert.ToInt32(list[index].OrdersInQueueOwn) + Convert.ToInt32(part.OrdersInQueueOwn)).ToString();
                list[index].Quantity = (Convert.ToInt32(list[index].Quantity) + Convert.ToInt32(part.Quantity)).ToString();
                list[index].Wip = (Convert.ToInt32(list[index].Wip) + Convert.ToInt32(part.Wip)).ToString();
            }
            else
            {
                list.Add(new OptimizedPart
                {
                    ID = part.ID,
                    Name = part.Name,
                    OrdersInQueueInherit = part.OrdersInQueueInherit,
                    PlannedWarehouseFollowing = part.PlannedWarehouseFollowing,
                    WarehouseStockPassed = part.WarehouseStockPassed,
                    OrdersInQueueOwn = part.OrdersInQueueOwn,
                    Wip = part.Wip,
                    Quantity = part.Quantity,
                    Optimized = list.Count(),
                    ProcessingTime = Convert.ToInt32(part.Quantity) * processingTime
                });
            }
        }
    }
}
