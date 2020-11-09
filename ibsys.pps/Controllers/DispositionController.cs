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

            try
            {
                if (plannedStocks != null)
                {
                    await _db.AddRangeAsync(plannedStocks);
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
    }
}
