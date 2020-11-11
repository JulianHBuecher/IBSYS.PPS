using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using IBSYS.PPS.Models.Capacity;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispositionController : ControllerBase
    {
        private readonly ILogger<DispositionController> _logger;
        private readonly IbsysDatabaseContext _db;

        public DispositionController(ILogger<DispositionController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }


        // GET - Disposition of a bicycle
        [HttpPost("{bicycle}")]
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

            var productionOrder = await _db.ProductionOrders
                .AsNoTracking()
                .Where(po => po.Bicycle.Equals(bicycle))
                .Select(po => po.Orders)
                .SingleOrDefaultAsync();

            var disposition = await ExecuteDisposition(bicycle, Convert.ToInt32(productionOrder[0]), plannedStocks);

            return Ok(disposition);
        }

        [HttpPost("syncresult")]
        public async Task<ActionResult> PostResultForPersistence()
        {
            var productionOrders = new List<BicyclePart>();

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body.Length != 0)
                {
                    JObject o = JObject.Parse(body);
                    JArray a = (JArray)o["DispositionResult"];
                    productionOrders = a.ToObject<List<BicyclePart>>();
                }
            }

            try
            {
                if (productionOrders != null)
                {
                    await _db.AddRangeAsync(productionOrders);
                }

                await _db.SaveChangesAsync();

                return Ok("Data sucessfully inserted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
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
                        Convert.ToInt32(queueFromPrevious.Quantity),
                        Convert.ToInt32(queueFromPrevious.OrdersInQueueOwn),
                        plannedWarehouseStock);
                    disposition.AddRange(parts);
                }
            }

            return new Bicycle { Parts = disposition };
        }

        [NonAction]
        public async Task<List<BicyclePart>> CalculateParts(List<string> partsForDisposition,
            int salesOrders, int ordersInQueueFromPrevious, List<PlannedWarehouseStock> plannedWarehouseStock)
        {
            var requiredMaterials = new List<BicyclePart>();

            foreach (var material in partsForDisposition)
            {
                var partNumber = Regex.Match(material, @"\d+").Value;

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
                        .Where(wss => wss.Item.Equals(partNumber))
                        .ToList())).FirstOrDefaultAsync();

                var workInProgress = await _db.OrdersInWork.AsNoTracking()
                    .Where(oiw => oiw.Item.Equals(partNumber))
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
                    ordersInWaitingQueue = ordersInWaitingQueue == 0 ? 0 : Convert.ToInt32(Math.Ceiling((decimal)ordersInWaitingQueue / 3));
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

                var requiredParts = (int)Math.Round((decimal)Math.Max(0, salesOrders + ordersInQueueFromPrevious + plannedStock - warehouseStock - ordersInWaitingQueue - wip) / 10) * 10;
                
                requiredMaterials.Add(new BicyclePart
                {
                    Name = material.ToString(),
                    OrdersInQueueInherit = ordersInQueueFromPrevious.ToString(),
                    PlannedWarehouseFollowing = plannedStock.ToString(),
                    WarehouseStockPassed = warehouseStock.ToString(),
                    OrdersInQueueOwn = ordersInWaitingQueue.ToString(),
                    Wip = wip.ToString(),
                    Quantity = requiredParts.ToString()
                });
            }

            return requiredMaterials;
        }

        [HttpPost("capacity")]
        public async Task<ActionResult> GetCapacityRequirements()
        {
            var plannedStocks = new Dictionary<String, List<PlannedWarehouseStock>>();

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body.Length != 0)
                {
                    JObject o = JObject.Parse(body);
                    foreach (string id in new string[] { "p1", "p2", "p3" })
                    {
                        JArray a = (JArray)o[id]["PlannedStocks"];
                        var plannedStock = a.ToObject<List<PlannedWarehouseStock>>();
                        plannedStocks.Add(id, plannedStock);
                    }
                }
            }
            var capRequirements = await ExecuteCapacityRequirements(100, plannedStocks);
            return Ok(capRequirements);
        }

        public async Task<Dictionary<int, int>> ExecuteCapacityRequirements(int salesOrders, Dictionary<String, List<PlannedWarehouseStock>> plannedWarehouseStocks)
        {
            #region Capacity Data
            Dictionary<String, List<CapacityRequirement>> capData = new Dictionary<String, List<CapacityRequirement>>();
            capData.Add("P1", new List<CapacityRequirement>() { new CapacityRequirement(6, 30, 4) });
            capData.Add("P2", new List<CapacityRequirement>() { new CapacityRequirement(7, 20, 4) });
            capData.Add("P3", new List<CapacityRequirement>() { new CapacityRequirement(7, 30, 4) });
            capData.Add("E4", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E5", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E6", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 20, 11) });
            capData.Add("E7", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E8", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E9", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 20, 11) });
            capData.Add("E10", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(1, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E11", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E12", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E13", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(1, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E14", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E15", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E16*", new List<CapacityRequirement>() { new CapacityRequirement(2, 15, 6), new CapacityRequirement(3, 0, 14) });
            capData.Add("E17*", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 15) });
            capData.Add("E18", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 6), new CapacityRequirement(2, 20, 7), new CapacityRequirement(3, 20, 8), new CapacityRequirement(2, 15, 9) });
            capData.Add("E19", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 6), new CapacityRequirement(2, 20, 7), new CapacityRequirement(3, 25, 8), new CapacityRequirement(2, 20, 9) });
            capData.Add("E20", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 6), new CapacityRequirement(2, 20, 7), new CapacityRequirement(3, 20, 8), new CapacityRequirement(2, 15, 9) });
            capData.Add("E26*", new List<CapacityRequirement>() { new CapacityRequirement(2, 30, 7), new CapacityRequirement(3, 15, 15) });
            capData.Add("E29", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 1) });
            capData.Add("E30", new List<CapacityRequirement>() { new CapacityRequirement(5, 20, 2) });
            capData.Add("E31", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 3) });
            capData.Add("E49", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 1) });
            capData.Add("E50", new List<CapacityRequirement>() { new CapacityRequirement(5, 30, 2) });
            capData.Add("E51", new List<CapacityRequirement>() { new CapacityRequirement(5, 20, 3) });
            capData.Add("E54", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 1) });
            capData.Add("E55", new List<CapacityRequirement>() { new CapacityRequirement(5, 30, 2) });
            capData.Add("E56", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 3) });
            #endregion

            var capRequirements = new Dictionary<int, int>();
            var setupTimes = new Dictionary<int, int>();
            foreach (KeyValuePair<String, List<PlannedWarehouseStock>> pair in plannedWarehouseStocks)
            {
                var dispositionData = ExecuteDisposition(pair.Key, 100, pair.Value);
                foreach (BicyclePart part in dispositionData.Result.Parts)
                {
                    var capacityData = capData[part.Name];
                    foreach (CapacityRequirement requirement in capacityData)
                    {
                        int time;
                        int quantity = Convert.ToInt32(part.Quantity);
                        if (capRequirements.TryGetValue(requirement.workStation, out time))
                        {
                            time += requirement.processTime * quantity;
                        }
                        else
                        {
                            capRequirements.Add(requirement.workStation, requirement.processTime * quantity);
                        }
                        int setupTime;
                        if (setupTimes.TryGetValue(requirement.workStation, out setupTime))
                        {
                            setupTime = (setupTime + requirement.setupTime) / 2;
                        }
                        else
                        {
                            setupTimes.Add(requirement.workStation, requirement.setupTime);
                        }
                    }
                }
            }

            List<int> workstations = new List<int>(capRequirements.Keys);
            foreach (var workstation in workstations)
            {
                var setupEvents = await _db.SetupEvents.AsNoTracking()
                    .Where(setupEvent => setupEvent.WorkplaceId.Equals(workstation))
                    .Select(setupEvent => setupEvent.NumberOfSetupEvents)
                    .FirstOrDefaultAsync();

                capRequirements[workstation] += setupEvents * setupTimes[workstation];

                var waitinglistWorkstations = await _db.WaitinglistWorkstations.AsNoTracking()
                    .Where(ws => Convert.ToInt32(ws.WorkplaceId).Equals(workstation) && ws.TimeNeed > 0)
                    .Select(ws => ws.TimeNeed).FirstOrDefaultAsync();

                var waitinglistMissingParts = await _db.WaitinglistStock.AsNoTracking()
                     .Include(w => w.WaitinglistForStock).ThenInclude(w => w.WaitinglistForWorkplaceStock)
                     .Select(w => w.WaitinglistForStock
                         .Where(wws => Convert.ToInt32(wws.WorkplaceId).Equals(workstation))
                         .Select(ws => ws.WaitinglistForWorkplaceStock
                         .Where(wss => wss.TimeNeed > 0).Select(wss => wss.TimeNeed).Sum())).FirstOrDefaultAsync();

                var workInProgress = await _db.OrdersInWork.AsNoTracking()
                   .Where(wip => wip.Id.Equals(workstation) && wip.TimeNeed > 0)
                   .Select(oiw => oiw.TimeNeed).SumAsync();

                foreach (var value in new List<int> { waitinglistWorkstations, workInProgress, workInProgress })
                {
                    capRequirements[workstation] += value;
                }
            }

            return capRequirements;
        }
    }
}
