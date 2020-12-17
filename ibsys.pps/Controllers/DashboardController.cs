using IBSYS.PPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IbsysDatabaseContext _db;

        public DashboardController(ILogger<DashboardController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("storagevalue")]
        public async Task<ActionResult> GetStorageValue([FromQuery] int p1 = 0, 
            [FromQuery] int p2 = 0, [FromQuery] int p3 = 0)
        {
            try
            {
                var stockValue = await _db.StockValuesFromLastPeriod
                    .AsNoTracking()
                    .Select(s => Convert.ToDouble(s.Stockvalue.Replace(",",".")))
                    .SumAsync();

                var stockValueP1 = await _db.StockValuesFromLastPeriod
                    .AsNoTracking()
                    .Where(s => s.Id.Equals("1"))
                    .Select(s => Convert.ToDouble(s.Price.Replace(",", ".")))
                    .FirstOrDefaultAsync();

                var stockValueP2 = await _db.StockValuesFromLastPeriod
                    .AsNoTracking()
                    .Where(s => s.Id.Equals("2"))
                    .Select(s => Convert.ToDouble(s.Price.Replace(",", ".")))
                    .FirstOrDefaultAsync();

                var stockValueP3 = await _db.StockValuesFromLastPeriod
                    .AsNoTracking()
                    .Where(s => s.Id.Equals("3"))
                    .Select(s => Convert.ToDouble(s.Price.Replace(",", ".")))
                    .FirstOrDefaultAsync();

                var salesOrders = await _db.Forecasts
                    .AsNoTracking()
                    .Select(f => f)
                    .ToListAsync();

                // Adding additional stock value for p1
                var salesOrdersP1 = salesOrders.Select(s => Convert.ToInt32(s.P1)).First();
                if ((p1 - salesOrdersP1) > 0)
                {
                    stockValue += (p1 - salesOrdersP1) * stockValueP1;
                }
                // Adding additional stock value for p2
                var salesOrdersP2 = salesOrders.Select(s => Convert.ToInt32(s.P2)).First();
                if ((p2 - salesOrdersP2) > 0)
                {
                    stockValue += (p2 - salesOrdersP2) * stockValueP2;
                }
                // Adding additional stock value for p3
                var salesOrdersP3 = salesOrders.Select(s => Convert.ToInt32(s.P3)).First();
                if ((p3 - salesOrdersP3) > 0)
                {
                    stockValue += (p3 - salesOrdersP3) * stockValueP3;
                }

                return Ok(stockValue);
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }

        [HttpGet("salesorders")]
        public async Task<ActionResult> GetSalesOrders()
        {
            var salesOrders = await _db.Forecasts
                .AsNoTracking()
                .Select(f => f)
                .ToListAsync();

            if (salesOrders.Any())
            {
                return Ok(salesOrders);
            }
            else
            {
                return NotFound("Data not found in the database.");
            }
        }
    }
}
