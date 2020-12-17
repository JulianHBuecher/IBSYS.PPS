using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Capacity;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Models.Input;
using IBSYS.PPS.Services;
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
        private readonly DispositionService _dispositionService;
        private readonly CapacityService _capacityService;

        public DispositionController(ILogger<DispositionController> logger, IbsysDatabaseContext db, 
            DispositionService dispositionService, CapacityService capacityService)
        {
            _logger = logger;
            _db = db;
            _dispositionService = dispositionService;
            _capacityService = capacityService;
        }

        // POST - Calculate Disposition of a bicycle
        [HttpPost("{bicycle}")]
        public async Task<ActionResult> CalculateDisposition([FromRoute] string bicycle) 
            //[FromBody] List<PlannedWarehouseStock> plannedStocks)
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

            var disposition = await _dispositionService.ExecuteDisposition(bicycle,
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
                
                    var capRequirements = await _capacityService.ExecuteCapacityRequirements(disposition);
                
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
                            _capacityService.CheckForConsistency(w);
                        });

                        await _db.AddRangeAsync(workingTimes);
                    }
                    else
                    {
                        workingTimes.ForEach(w =>
                        {
                            _capacityService.CheckForConsistency(w);
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
    }
}
