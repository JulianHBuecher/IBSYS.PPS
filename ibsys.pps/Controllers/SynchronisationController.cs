using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Serializer;
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
using System.Xml.Linq;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SynchronisationController : ControllerBase
    {
        private readonly ILogger<SynchronisationController> _logger;

        private readonly IbsysDatabaseContext _db;

        public SynchronisationController(ILogger<SynchronisationController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        // POST - Synchronization of results from last period
        [HttpPost("results")]
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

    }
}
