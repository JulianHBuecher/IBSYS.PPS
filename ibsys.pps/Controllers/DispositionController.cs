using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Capacity;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        // POST - Calculate Disposition of a bicycle
        [HttpPost("{bicycle}")]
        public async Task<ActionResult> CalculateDisposition(string bicycle)
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

            var productionOrders = await _db.ProductionOrders
                .AsNoTracking()
                .Where(po => po.Bicycle.Equals(bicycle))
                .Select(po => po.Orders)
                .SingleOrDefaultAsync();

            var salesOrders = await _db.Forecasts
                .AsNoTracking()
                .Select(f => f)
                .FirstOrDefaultAsync();

            var salesOrder = bicycle switch
            {
                "P1" => Convert.ToInt32(salesOrders.P1),
                "P2" => Convert.ToInt32(salesOrders.P2),
                "P3" => Convert.ToInt32(salesOrders.P3),
                _ => Convert.ToInt32(salesOrders.P1)
            };

            var directSalesOrder = await _db.SellDirectItems
                .AsNoTracking()
                .Where(ds => ds.Article.Equals(Regex.Match(bicycle, @"\d+").Value))
                .Select(ds => ds.Quantity)
                .SingleOrDefaultAsync();

            directSalesOrder ??= "0";

            var disposition = await ExecuteDisposition(bicycle,
                productionOrders,
                (salesOrder + Convert.ToInt32(directSalesOrder)), 
                plannedStocks);

            return Ok(disposition);
        }

        [HttpPost("syncresult/disposition/{bicycle}")]
        public async Task<ActionResult> PostResultForPersistence(string bicycle)
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
                    var existingOrders = await _db.DispositionEParts
                        .Where(o => o.ReferenceToBicycle.Equals(bicycle.ToUpper()))
                        .Select(o => o)
                        .ToListAsync();

                    var plannedWarehouseStocks = await _db.PlannedWarehouseStocks
                        .Where(p => p.ReferenceToBicycle.Equals(bicycle.ToUpper()))
                        .Select(p => p)
                        .ToListAsync();

                    var updatedOrders = new List<BicyclePart>();
                    var updatedWarehouseStocks = new List<PlannedWarehouseStock>();

                    if (!existingOrders.Any() && !plannedWarehouseStocks.Any())
                    {
                        await _db.AddRangeAsync(productionOrders.Select(po => 
                            new BicyclePart
                            {
                                Name = po.Name,
                                OrdersInQueueInherit = po.OrdersInQueueInherit,
                                PlannedWarehouseFollowing = po.PlannedWarehouseFollowing,
                                WarehouseStockPassed = po.WarehouseStockPassed,
                                OrdersInQueueOwn = po.OrdersInQueueOwn,
                                Wip = po.Wip,
                                Quantity = po.Quantity,
                                ReferenceToBicycle = productionOrders.Where(po => po.Name.Contains("P")).Select(po => po.Name).FirstOrDefault()
                            }).ToList());

                        await _db.PlannedWarehouseStocks
                        .AddRangeAsync(productionOrders.Select(po =>
                            new PlannedWarehouseStock
                            {
                                Part = po.Name,
                                Amount = Convert.ToInt32(po.PlannedWarehouseFollowing),
                                ReferenceToBicycle = productionOrders.Where(po => po.Name.Contains("P")).Select(po => po.Name).FirstOrDefault()
                            }
                        ).ToList());
                    }
                    else
                    {
                        productionOrders.ForEach(o =>
                        {
                            var e = existingOrders.Where(p => p.Name.Equals(o.Name)).Select(p => p).FirstOrDefault();
                            e.PlannedWarehouseFollowing = o.PlannedWarehouseFollowing;
                            e.Quantity = o.Quantity;
                            updatedOrders.Add(e);

                            var p = plannedWarehouseStocks.Where(w => w.Part.Equals(o.Name)).Select(w => w).FirstOrDefault();
                            p.Amount = Convert.ToInt32(o.PlannedWarehouseFollowing);
                            updatedWarehouseStocks.Add(p);
                        });

                        _db.UpdateRange(updatedOrders);
                        _db.UpdateRange(updatedWarehouseStocks);
                    }
                }

                await _db.SaveChangesAsync();

                return Ok("Data sucessfully inserted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }

        [HttpGet("capacity")]
        public async Task<ActionResult> GetCapacityRequirements()
        {
            try
            {
                var disposition = await _db.DispositionEParts
                    .AsNoTracking()
                    .Where(d => d.ReferenceToBicycle != null)
                    .Select(d => d)
                    .ToListAsync();
                
                    var capRequirements = await ExecuteCapacityRequirements(disposition);
                
                return Ok(capRequirements.Select(c => c.Value).OrderBy(c => c.Workstation));
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}!");
            }
        }

        [HttpPost("syncresult/capacityplanning")]
        public async Task<ActionResult> PostCapacityResultForPersistence()
        {
            var workingTimes = new List<Workingtime>();

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body.Length != 0)
                {
                    JObject o = JObject.Parse(body);
                    JArray a = (JArray)o["CapacityPlanningResult"];
                    workingTimes = a.ToObject<List<Workingtime>>();
                }
            }

            try
            {
                if (workingTimes != null)
                {
                    var existingWorkingTimes = await _db.Workingtimes
                        .Select(w => w)
                        .ToListAsync();

                    var updatedWorkingTimes = new List<Workingtime>();

                    if (!existingWorkingTimes.Any())
                    {
                        workingTimes.ForEach(w =>
                        {
                            CheckForConsistency(w);
                        });

                        await _db.AddRangeAsync(workingTimes);
                    }
                    else
                    {
                        workingTimes.ForEach(w =>
                        {
                            CheckForConsistency(w);
                            var wt = existingWorkingTimes.Where(t => t.Station.Equals(w.Station)).Select(t => t).FirstOrDefault();
                            wt.Shift = w.Shift;
                            wt.Overtime = w.Overtime;
                            updatedWorkingTimes.Add(wt);
                        });

                        _db.UpdateRange(updatedWorkingTimes);
                    }
                }

                await _db.SaveChangesAsync();

                return Ok("Data sucessfully inserted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }

        [HttpGet("{bicycle}")]
        public async Task<ActionResult> GetDispositionForBicycle(string bicycle)
        {
            var disposition = await _db.DispositionEParts
                .AsNoTracking()
                .Where(d => d.ReferenceToBicycle.Equals(bicycle.ToUpper()))
                .Select(d => d)
                .ToListAsync();

            if (disposition != null)
            {
                return Ok(disposition);
            }
            else
            {
                return BadRequest("No data found. Please calculate and persist your results!");
            }
        }

        [NonAction]
        public async Task<Bicycle> ExecuteDisposition(string id, double[] forecasts, int salesOrders, List<PlannedWarehouseStock> plannedWarehouseStock)
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

            partsForDisposition = (id.ToLower()) switch
            {
                "p1" => p1,
                "p2" => p2,
                _ => p3,
            };
            var disposition = new List<BicyclePart>();

            for (var i = 0; i < partsForDisposition.Length; i++)
            {
                var parts = new List<BicyclePart>();
                if (!disposition.Any())
                {
                    parts = await CalculateParts(partsForDisposition[i].Select(a => a.ToString()).ToList(), salesOrders, forecasts, 0, plannedWarehouseStock);
                    disposition.AddRange(parts);
                }
                else
                {
                    var queueFromPrevious = disposition.Last();
                    parts = await CalculateParts(
                        partsForDisposition[i].Select(a => a.ToString()).ToList(),
                        Convert.ToInt32(queueFromPrevious.Quantity),
                        forecasts,
                        Convert.ToInt32(queueFromPrevious.OrdersInQueueOwn),
                        plannedWarehouseStock);
                    disposition.AddRange(parts);
                }
            }

            return new Bicycle { Parts = disposition };
        }

        [NonAction]
        public async Task<List<BicyclePart>> CalculateParts(List<string> partsForDisposition,
            int salesOrders, double[] forecasts, int ordersInQueueFromPrevious, List<PlannedWarehouseStock> plannedWarehouseStock)
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
                        .SelectMany(ws => ws.WaitinglistForWorkplaceStock
                            .Where(wss => wss.Item.Equals(partNumber))
                            .Select(wss => wss))).ToListAsync();

                waitinglistMissingParts = waitinglistMissingParts.Where(ws => ws.Any()).Select(ws => ws).ToList();

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
                    plannedStock = (int)Math.Round(Math.Ceiling((forecasts[0] - (double)salesOrders + (double)warehouseStock + (double)ordersInWaitingQueue + (double)wip) / 10) * 10);

                    if (plannedStock < 0)
                    {
                        plannedStock = Convert.ToInt32(stockQuantity);
                    }
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

        [NonAction]
        public async Task<Dictionary<int, CapacityRequirementExtended>> ExecuteCapacityRequirements(List<BicyclePart> dispositionData)
        {
            #region Capacity Data
            var capData = new Dictionary<String, List<CapacityRequirement>>();
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

            // Includes every workstation with the regarding capacity requirement (new)
            var capRequirements = new Dictionary<int, int>();

            var setupTimes = new Dictionary<int, int>();
            
            foreach (var part in dispositionData)
            {
                var capacityData = capData[part.Name];

                foreach (var requirement in capacityData)
                {
                    // Amount of parts for production
                    var quantity = Convert.ToInt32(part.Quantity);
                    // Time required for the production of this amount of parts
                    int time;
                    // If the workstation exists in the list, give back the existing capacity requirement
                    if (capRequirements.TryGetValue(requirement.workStation, out time))
                    {
                        time += requirement.processTime * quantity;
                        capRequirements[requirement.workStation] = time;
                    }
                    // If not, create a new item in the workstation item in the list
                    // with the regarding capacity requirement for the specific part
                    else
                    {
                        capRequirements.Add(requirement.workStation, requirement.processTime * quantity);
                    }

                    int setupTime;
                    // If the workstation exists with their Setup-Events and Setup-Times, give them back
                    if (setupTimes.TryGetValue(requirement.workStation, out setupTime))
                    {
                        setupTime = (setupTime + requirement.setupTime) / 2;
                        setupTimes[requirement.workStation] = setupTime;
                    }
                    // If not, add the workstation with the regarding values
                    else
                    {
                        setupTimes.Add(requirement.workStation, requirement.setupTime);
                    }
                }
            }

            List<int> workstations = new List<int>(capRequirements.Keys);

            var requirements = new Dictionary<int, CapacityRequirementExtended>();

            foreach (var workstation in workstations)
            {
                // Aquire Setup-Events from last period for this workplace
                // Serves as prognosis for the next period
                var setupEvents = await _db.SetupEvents
                    .AsNoTracking()
                    .Where(s => s.WorkplaceId.Equals(workstation.ToString()))
                    .Select(s => s.NumberOfSetupEvents)
                    .FirstOrDefaultAsync();

                capRequirements[workstation] += setupEvents * setupTimes[workstation];

                // Aquire the time needed from orders out of the queue (not enough time)
                var waitinglistWorkstations = await _db.WaitinglistWorkstations
                    .AsNoTracking()
                    .Where(ws => ws.WorkplaceId.Equals(workstation.ToString()) && ws.TimeNeed > 0)
                    .Select(ws => ws.TimeNeed)
                    .FirstOrDefaultAsync();

                // Aquire the time needed from orders out of the queue (not enough parts)
                var waitinglistMissingParts = await _db.WaitinglistStock
                    .AsNoTracking()
                    .Include(wls => wls.WaitinglistForStock).ThenInclude(wls => wls.WaitinglistForWorkplaceStock)
                    .Select(wls => wls.WaitinglistForStock
                        .Where(wls => wls.WorkplaceId.Equals(workstation.ToString()))
                        .Select(wlfs => wlfs.TimeNeed).Sum())
                    .ToListAsync();

                // Aquire the time needed from orders out of the queue (not enough time)
                var workInProgress = await _db.OrdersInWork.AsNoTracking()
                   .Where(wip => wip.Id.Equals(workstation.ToString()) && wip.TimeNeed > 0)
                   .Select(oiw => oiw.TimeNeed).SumAsync();

                foreach (var value in new List<int> { waitinglistWorkstations, waitinglistMissingParts.Sum(), workInProgress })
                {
                    capRequirements[workstation] += value;
                }

                requirements.Add(workstation, new CapacityRequirementExtended
                {
                    Workstation = workstation,
                    TimeFromWaitinglist = waitinglistWorkstations,
                    TimeFromWiP = workInProgress,
                    SetupEvents = setupEvents,
                    SetupTime = setupTimes[workstation],
                    RequiredCapacity = capRequirements[workstation]
                });
            }
            return requirements;
        }

        /// <summary>
        /// Method to check, if the shifts and overtimes don't break their limits
        /// </summary>
        /// <param name="w">A single workingtime object</param>
        [NonAction]
        public void CheckForConsistency(Workingtime w)
        {
            if ((w.Shift.Equals("1") || w.Shift.Equals("2")) && Convert.ToInt32(w.Overtime) > 240)
            {
                throw new Exception("Overtime have to be lesser than 240 minutes a day!");
            }
            if (w.Shift.Equals("3") && Convert.ToInt32(w.Overtime) > 0)
            {
                throw new Exception("In third shift you could not set overtime!");
            }
            if (Convert.ToInt32(w.Overtime) < 0)
            {
                throw new Exception("Overtime could not be negative!");
            }
        }
    }
}
