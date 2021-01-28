using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Models.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IBSYS.PPS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/xml")]
    public class ResultsController : ControllerBase
    {
        private readonly ILogger<ResultsController> _logger;
        private readonly IbsysDatabaseContext _db;

        public ResultsController(ILogger<ResultsController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult> GetInputFile()
        {
            var forecast = await _db.Forecasts
                .AsNoTracking()
                .SingleOrDefaultAsync();

            var selldirect = await _db.SellDirectItems
                .AsNoTracking()
                .Select(s => s)
                .ToListAsync();

            var orders = await _db.OrdersForK
                .AsNoTracking()
                .Select(o => o)
                .ToListAsync();

            var productionOrdersEParts = await _db.OptimizedParts
                .AsNoTracking()
                .Select(d => d)
                .OrderBy(d => d.Optimized)
                .ToListAsync();

            var productionList = new List<Production>();

            productionList.AddRange(
                productionOrdersEParts.Select(pe => new Production
                {
                    Article = Regex.Match(pe.Name, @"\d+").Value,
                    Quantity = pe.Quantity
                }));

            var workingtimeList = await _db.Workingtimes
                .AsNoTracking()
                .Select(w => w)
                .ToListAsync();


            var inputFile = new Input
            {
                Qualitycontrol = new Qualitycontrol
                {
                    Type = "no",
                    LoseQuantity = "0",
                    Delay = "0"
                },
                PrognosedItems = new List<SellWishItem>
                {
                    new SellWishItem
                    {
                        Article = "1",
                        Quantity = forecast.P1
                    },
                    new SellWishItem
                    {
                        Article = "2",
                        Quantity = forecast.P2
                    },
                    new SellWishItem
                    {
                        Article = "3",
                        Quantity = forecast.P3
                    }
                }.ToArray(),
                DirectSellItems = selldirect.Where(s => !s.Quantity.Equals("0"))
                .Select(s => new SellDirectItem
                {
                    Article = s.Article,
                    Quantity = s.Quantity,
                    Price = s.Price,
                    Penalty = s.Penalty
                }).ToArray(),
                Orders = orders.Select(o => new Order
                {
                    Article = Regex.Match(o.PartName, @"\d+").Value,
                    Quantity = o.OrderQuantity,
                    Modus = o.OrderModus.ToString()
                }).ToArray(),
                Productions = productionList.ToArray(),
                Workingtimes = workingtimeList.ToArray()
            };

            return Ok(inputFile);
        }
    }
}
