using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Serializer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        [HttpPost("disposition/{bicycle}")]
        public async Task<ActionResult> GetDisposition(string bicycle)
        {
            var plannedStocks = new List<PlannedWarehouseStock>();

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body.Length != 0)
                {
                    JObject o = JObject.Parse(body);
                    JArray a = (JArray)o["PlannedStocks"];
                    plannedStocks = a.ToObject<List<PlannedWarehouseStock>>();
                }
            }

            var disposition = await ExecuteDisposition(bicycle, 100, plannedStocks);
            

            return Ok(disposition);
        }

        [HttpPost("syncresult")]
        public async Task<ActionResult> SyncResultsOfLastPeriod([FromBody] XElement input)
        {
            DataSerializer serializer = new DataSerializer();

            var resultFromLastPeriod = serializer.DeserializePeriodResults(input.ToString());

            if (resultFromLastPeriod == null)
            {
                return BadRequest();
            }

            try
            {
                await _db.FutureInwardStockMovement
                    .AddRangeAsync(resultFromLastPeriod.Futureinwardstockmovement.Order);

                await _db.StockValuesFromLastPeriod
                    .AddRangeAsync(resultFromLastPeriod.Warehousestock.Article);

                await _db.WaitinglistWorkstations
                    .AddRangeAsync(
                        resultFromLastPeriod.Waitinglistworkstations.Workplace
                            .Where(w => Convert.ToInt32(w.Timeneed) > 0)
                            .Select(w => new WaitinglistForWorkstations
                            {
                                WorkplaceId = w.Id,
                                TimeNeed = Convert.ToInt32(w.Timeneed),
                                WaitingListForWorkplace = w.Waitinglist.Select(wl => new WaitinglistForWorkplace
                                {
                                    Period = wl.Period,
                                    Order = wl.Order,
                                    Item = wl.Item,
                                    Amount = Convert.ToInt32(wl.Amount),
                                    TimeNeed = Convert.ToInt32(wl.Timeneed),
                                    Batch = Convert.ToInt32(wl.Firstbatch)
                                }).ToList()

                            }).ToList());

                await _db.WaitinglistStock
                    .AddRangeAsync(
                    resultFromLastPeriod.Waitingliststock.Missingpart
                        .Where(mp => mp.Workplace.Any())
                        .Select(mp => new MissingPartInStock
                        {
                            Id = mp.Id,
                            WaitinglistForStock = mp.Workplace.Select(wp => new WaitinglistForStock
                            {
                                WorkplaceId = wp.Id,
                                TimeNeed = Convert.ToInt32(wp.Timeneed),
                                WaitinglistForWorkplaceStock = wp.Waitinglist.Select(wl => new WaitinglistForWorkplaceStock
                                {
                                    Period = wl.Period,
                                    Order = wl.Order,
                                    Item = wl.Item,
                                    Amount = Convert.ToInt32(wl.Amount),
                                    TimeNeed = Convert.ToInt32(wl.Timeneed),
                                    Batch = Convert.ToInt32(wl.Firstbatch)
                                }).ToList()
                            }).ToList()
                        }).ToList());

                await _db.OrdersInWork
                    .AddRangeAsync(
                    resultFromLastPeriod.Ordersinwork.Workplace.Any() ?
                        resultFromLastPeriod.Ordersinwork.Workplace
                            .Select(wp => new WaitinglistForOrdersInWork
                            {
                                Id = wp.Id,
                                Period = wp.Period,
                                Order = wp.Order,
                                Item = wp.Item,
                                Amount = Convert.ToInt32(wp.Amount),
                                TimeNeed = Convert.ToInt32(wp.Timeneed),
                                Batch = Convert.ToInt32(wp.Batch)
                            }).ToList() : new List<WaitinglistForOrdersInWork>());

                await _db.SaveChangesAsync();

                return Ok("Data sucessfully inserted!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Data could not be written to DB: {ex.Message}");
            }
        }

        [HttpPost("materialplanning/{bicycleNumber}/{part}")]
        public async void Materialplanning(string bicycleNumber, string part)
        {
            // Extract bicycles per number for filtering the needed materials
            var bicycle = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .FirstOrDefaultAsync(b => b.ProductName == bicycleNumber);

            foreach (var material in bicycle.RequiredMaterials)
            {
                material.MaterialNeeded = await GetNestedMaterials(material);
            }

            var bicycleParts = new List<Material>();

            foreach (var material in bicycle.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                bicycleParts.AddRange(filteredPart);
            }

            var summedParts = SumFilteredMaterials(bicycleParts);
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
        public async Task<Bicycle> ExecuteDisposition(string id, int salesOrders, List<PlannedWarehouseStock> plannedWarehouseStock)
        {
            #region Bicycle Data
            string[][] p1 = new string[][] 
            { 
                new string[] { "P1" },
                new string[] { "E26*", "E51" },
                new string[] { "E16*", "E17*", "E50" },
                new string[] { "E4", "E10", "E49" },
                new string[] { "E7", "E13", "E18" } 
            };

            string[][] p2 = new string[][] 
            {
                new string[] { "P2" },
                new string[] { "E26*", "E56" },
                new string[] { "E16*", "E17*", "E55" },
                new string[] { "E5", "E11", "E54" },
                new string[] { "E8", "E14", "E19" }
            };

            string[][] p3 = new string[][] 
            {
                new string[] { "P3" },
                new string[] { "E26*", "E31" },
                new string[] { "E16*", "E17*", "E30" },
                new string[] { "E6", "E12", "E29" },
                new string[] { "E9", "E15", "E20" }
            };
            #endregion

            string[][] partsForDisposition = new string[][] { };

            switch (id.ToLower())
            {
                case "p1": 
                    partsForDisposition = p1;
                    break;
                case "p2":
                    partsForDisposition = p2;
                    break;
                default:
                    partsForDisposition = p3;
                    break;
            }

            var disposition = new List<BicyclePart>();

            for (var i = 0; i < partsForDisposition.Length; i++)
            {
                var parts = new List<BicyclePart>();
                if (!disposition.Any())
                {
                    parts = await CalculateParts(partsForDisposition[i].Select(a => a.ToString()).ToList(), salesOrders, 0, plannedWarehouseStock);
                    disposition.AddRange(parts);
                }
                else
                {
                    var queueFromPrevious = disposition.Last();
                    parts = await CalculateParts(
                        partsForDisposition[i].Select(a => a.ToString()).ToList(), 
                        Convert.ToInt32(queueFromPrevious.quantity), 
                        Convert.ToInt32(queueFromPrevious.ordersInQueueOwn),
                        plannedWarehouseStock);
                    disposition.AddRange(parts);
                }
            }

            return new Bicycle { parts = disposition };
        }

        [NonAction]
        public async Task<List<BicyclePart>> CalculateParts(List<string> partsForDisposition,
            int salesOrders, int ordersInQueueFromPrevious, List<PlannedWarehouseStock> plannedWarehouseStock)
        {
            var requiredMaterials = new List<BicyclePart>();

            foreach (var material in partsForDisposition)
            {
                var extractedNumber = Regex.Match(material, @"\d+");

                var stockQuantity = await _db.StockValuesFromLastPeriod.AsNoTracking()
                    .Where(m => m.Id.Equals(extractedNumber.Value))
                    .Select(m => m.Amount).FirstOrDefaultAsync();

                var warehouseStock = Convert.ToInt32(stockQuantity);

                var waitinglistWorkstations = await _db.WaitinglistWorkstations.AsNoTracking()
                    .Include(m => m.WaitingListForWorkplace)
                    .Select(w => w.WaitingListForWorkplace.Where(wl => wl.Item.Equals(extractedNumber.Value)))
                    .SelectMany(wl => wl)
                    .ToListAsync();

                var waitinglistMissingParts = await _db.WaitinglistStock.AsNoTracking()
                    .Include(w => w.WaitinglistForStock).ThenInclude(w => w.WaitinglistForWorkplaceStock)
                    .Select(w => w.WaitinglistForStock
                        .Select(ws => ws.WaitinglistForWorkplaceStock
                        .Where(wss => wss.Item.Equals(extractedNumber.Value))
                        .ToList())).FirstOrDefaultAsync();

                var workInProgress = await _db.OrdersInWork.AsNoTracking()
                    .Where(oiw => oiw.Item.Equals(extractedNumber.Value))
                    .Select(oiw => oiw).ToListAsync();

                var ordersInWaitingQueue = 0;

                // Filter list for the same batches
                int? wlwCounter = waitinglistWorkstations
                    .GroupBy(w => w.Batch)
                    .Select(wp => wp.OrderBy(wp => wp.Batch).First().Amount).Sum();

                int? wlmCounter = waitinglistMissingParts == null ? 0 : 
                    waitinglistMissingParts.Select(mp => mp.GroupBy(w => w.Batch)
                        .Select(wmp => wmp.OrderBy(wp => wp.Batch).First().Amount).Sum()).Sum();

                int? wipCounter = workInProgress.GroupBy(oiw => oiw.Batch)
                    .Select(oiwp => oiwp.OrderBy(o => o.Batch).First().Amount).Sum();

                // Check for no Items in lists
                wlwCounter ??= 0;
                wlmCounter ??= 0;
                wipCounter ??= 0;

                ordersInWaitingQueue += wlwCounter.Value + wlmCounter.Value;

                var wip = wipCounter.Value;

                if (material.Contains("*"))
                {
                    warehouseStock = warehouseStock == 0 ? 0 : Convert.ToInt32(Math.Floor((decimal)warehouseStock / 3));
                    ordersInWaitingQueue = ordersInWaitingQueue == 0 ? 0 : Convert.ToInt32(Math.Ceiling((decimal)ordersInWaitingQueue/ 3));
                    wip = wip == 0 ? 0 : Convert.ToInt32(Math.Ceiling((decimal)wip / 3));
                }

                var plannedStock = 0;

                if (!plannedWarehouseStock.Any())
                {
                    plannedStock = Convert.ToInt32(stockQuantity);
                }
                else
                {
                    plannedStock = plannedWarehouseStock.Where(p => p.Part.Contains(material)).Select(p => p.Amount).FirstOrDefault();
                }

                var requiredParts = Math.Max(0, salesOrders + ordersInQueueFromPrevious + plannedStock - warehouseStock - ordersInWaitingQueue - wip);

                requiredMaterials.Add(new BicyclePart
                {
                    name = material.ToString(),
                    ordersInQueueInherit = ordersInQueueFromPrevious.ToString(),
                    plannedWarehouseFollowing = plannedStock.ToString(),
                    warehouseStockPassed = warehouseStock.ToString(),
                    ordersInQueueOwn = ordersInWaitingQueue.ToString(),
                    wip = wip.ToString(),
                    quantity = requiredParts.ToString()
                });
            }

            return requiredMaterials;
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
    }
}
