using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IBSYS.PPS.Services
{
    public class DispositionService
    {
        private readonly ILogger<DispositionService> _logger;
        private readonly IbsysDatabaseContext _db;

        public DispositionService(ILogger<DispositionService> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

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

    }
}
