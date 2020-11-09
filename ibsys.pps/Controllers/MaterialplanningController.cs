using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Materialplanning;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialplanningController : ControllerBase
    {
        private readonly ILogger<MaterialplanningController> _logger;
        private readonly IbsysDatabaseContext _db;

        public MaterialplanningController(ILogger<MaterialplanningController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpPost]
        public async void Materialplanning()
        {
            var productionOrders = await _db.ProductionOrders
                .AsNoTracking()
                .Select(po => po)
                .ToListAsync();

            double[,] productionMatrix = new double[3,4];

            for (var i = 0; i < productionOrders.Count(); i++)
            {
                for (var j = 0; j < productionOrders[i].Orders.Count(); j++)
                {
                    productionMatrix[i,j] = productionOrders[i].Orders[j];
                }
            }

            // Extract bicycles per number for filtering the needed materials
            var bicycleParts = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .ToListAsync();

            foreach (var bicycle in bicycleParts)
            {
                foreach (var material in bicycle.RequiredMaterials)
                {
                    material.MaterialNeeded = await GetNestedMaterials(material);
                }
            }

            var extractedBicyclePOne = new List<Material>();
            var extractedBicyclePTwo = new List<Material>();
            var extractedBicyclePThree = new List<Material>();

            foreach (var bicycle in new List<List<Material>> { extractedBicyclePOne, extractedBicyclePTwo, extractedBicyclePThree })
            {
                foreach (var b in bicycleParts)
                {
                    foreach (var material in b.RequiredMaterials)
                    {
                        var filteredPart = await FilterNestedMaterialsByName("K", material);
                        bicycle.AddRange(filteredPart);
                    }
                }
            }

            var summedPartsPOne = SumFilteredMaterials(extractedBicyclePOne);
            var summedPartsPTwo = SumFilteredMaterials(extractedBicyclePTwo);
            var summedPartsPThree = SumFilteredMaterials(extractedBicyclePThree);

            var completedPartsForP1 = InsertNotNeededMaterials(new List<Material>(summedPartsPOne), new List<Material>(summedPartsPTwo), new List<Material>(summedPartsPThree));
            var completedPartsForP2 = InsertNotNeededMaterials(new List<Material>(summedPartsPTwo), new List<Material>(summedPartsPOne), new List<Material>(summedPartsPThree));
            var completedPartsForP3 = InsertNotNeededMaterials(new List<Material>(summedPartsPThree), new List<Material>(summedPartsPOne), new List<Material>(summedPartsPTwo));

            // Conversion of Summed Parts To Vectors
            var neededMaterialVecP1 = Vector<Double>.Build
                .DenseOfEnumerable(summedPartsPOne.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP2 = Vector<Double>.Build
                .DenseOfEnumerable(summedPartsPTwo.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP3 = Vector<Double>.Build
                .DenseOfEnumerable(summedPartsPThree.Select(p => Convert.ToDouble(p.QuantityNeeded)));

            var neededMaterialMatrix = Matrix<Double>.Build.DenseOfColumnVectors(neededMaterialVecP1, neededMaterialVecP2, neededMaterialVecP3);

            // Matrix Multiplikation for Calculation of required parts
            var productionOrderMatrix = Matrix<Double>.Build.DenseOfArray(productionMatrix);

            var calculatedNewParts = neededMaterialMatrix.Multiply(productionOrderMatrix);


        }

        [HttpPost("syncresult")]
        public async Task<ActionResult> PostResultForPersistence()
        {
            // TODO: Create Class for JSON Serialization and Persistence in DB
            return Ok("Data sucessfully inserted");
        }

        [NonAction]
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
        [NonAction]
        public async Task<List<Material>> FilterNestedMaterialsByName(string parts, Material ml)
        {
            var partsForBicycle = new List<Material>();

            if (ml.MaterialName.StartsWith(parts))
            {
                partsForBicycle.Add(new Material { MaterialName = ml.MaterialName, QuantityNeeded = ml.QuantityNeeded });
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

        [NonAction]
        public List<Material> SumFilteredMaterials(List<Material> materials)
        {
            var partsOrderedAndSummed = materials.GroupBy(p => p.MaterialName).OrderBy(p => p.Key)
                .Select(p => new Material
                {
                    MaterialName = p.Key,
                    QuantityNeeded = p.Select(pp => pp.QuantityNeeded).Sum()
                })
                .ToList();

            return partsOrderedAndSummed;
        }

        [NonAction]
        public List<Material> InsertNotNeededMaterials(List<Material> listForInsert, List<Material> referenceListTwo, 
            List<Material> referenceListThree)
        {
            var initialCount = listForInsert.Count();

            for (var i = 0; i < initialCount; i++)
            {
                if (listForInsert[i].MaterialName != referenceListTwo[i].MaterialName &&
                    !listForInsert.Any(part => part.MaterialName == referenceListTwo[i].MaterialName))
                {
                    listForInsert.Insert(i, new Material
                    {
                        MaterialName = referenceListTwo[i].MaterialName,
                        QuantityNeeded = 0
                    });
                }
                if (listForInsert[i].MaterialName != referenceListThree[i].MaterialName &&
                    !listForInsert.Any(part => part.MaterialName == referenceListThree[i].MaterialName))
                {
                    listForInsert.Insert(i, new Material
                    {
                        MaterialName = referenceListThree[i].MaterialName,
                        QuantityNeeded = 0
                    });
                }
            }
            return listForInsert.OrderBy(p => Convert.ToInt32(p.MaterialName.Split(" ")[1])).ToList();
        }

        [NonAction]
        public async Task<List<Material>> FilterKMaterialsByNameAndEPart(string parts, string ePart, Material ml)
        {
            var partsForBicycle = new List<Material>();

            if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded.Count != 0)
            {
                if (ml.MaterialName.Equals(ePart))
                {
                    partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, ml));
                }
                else
                {
                    foreach (var material in ml.MaterialNeeded)
                    {
                        if (ml.MaterialName.Equals(ePart))
                        {
                            partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, ml));
                        }
                        partsForBicycle.AddRange(await FilterKMaterialsByNameAndEPart(parts, ePart, material));
                    }
                }
            }

            return partsForBicycle;
        }

        [NonAction]
        public async Task PlaceOrder(Matrix<Double> requiredParts, List<Material> partsForPlanning)
        {
            foreach (var part in partsForPlanning)
            {
                var position = 0;

                var partNumber = part.MaterialName.Split(" ")[1];

                var stockQuantity = await _db.StockValuesFromLastPeriod.AsNoTracking()
                    .Where(m => m.Id.Equals(partNumber))
                    .Select(m => m.Amount).FirstOrDefaultAsync();

                var warehouseStock = Convert.ToInt32(stockQuantity);

                var waitinglistWorkstations = await _db.WaitinglistWorkstations.AsNoTracking()
                    .Include(m => m.WaitingListForWorkplace)
                    .Select(w => w.WaitingListForWorkplace.Where(wl => wl.Item.Equals(partNumber)))
                    .SelectMany(wl => wl)
                    .ToListAsync();

                var waitinglistMissingParts = await _db.WaitinglistStock.AsNoTracking()
                    .Include(w => w.WaitinglistForStock).ThenInclude(w => w.WaitinglistForWorkplaceStock)
                    .Select(w => w.WaitinglistForStock
                        .Select(ws => ws.WaitinglistForWorkplaceStock
                        .Where(wss => wss.Item.Equals(partNumber)).ToList()))
                    .FirstOrDefaultAsync();

                var ordersInWaitingQueue = 0;

                // Filter list for the same batches
                int? wlwCounter = waitinglistWorkstations
                    .GroupBy(w => w.Batch)
                    .Select(wp => wp.OrderBy(wp => wp.Batch).First().Amount).Sum();

                int? wlmCounter = waitinglistMissingParts == null ? 0 :
                    waitinglistMissingParts.Select(mp => mp.GroupBy(w => w.Batch)
                        .Select(wmp => wmp.OrderBy(wp => wp.Batch).First().Amount).Sum()).Sum();

                // Check for no Items in lists
                wlwCounter ??= 0;
                wlmCounter ??= 0;

                ordersInWaitingQueue += wlwCounter.Value + wlmCounter.Value;

                // TODO: Get additional required K parts resulting from queue


                // TODO: Logik for Decision between E or N orders and how much


                // TODO: Return Results
            }
        }
    }
}
