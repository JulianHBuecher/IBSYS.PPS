using IBSYS.PPS.Models;
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
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInputController : ControllerBase
    {
        private readonly ILogger<UserInputController> _logger;
        private readonly IbsysDatabaseContext _db;

        public UserInputController(ILogger<UserInputController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpPost("productionorder")]
        public async Task<ActionResult> PostProductionOrders()
        {
            var productionOrders = new List<ProductionOrder>();

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body.Length != 0)
                {
                    JObject o = JObject.Parse(body);
                    JArray a = (JArray)o["ProductionOrders"];
                    productionOrders = a.ToObject<List<ProductionOrder>>();
                }
            }

            try
            {
                var existingOrders = await _db.ProductionOrders
                    .AsNoTracking()
                    .Select(o => o)
                    .ToListAsync();

                if (!existingOrders.Any())
                {
                    await _db.AddRangeAsync(productionOrders);
                }
                else
                {
                    productionOrders.ForEach(o =>
                    {
                        existingOrders.Where(p => p.Bicycle.Equals(o.Bicycle))
                            .Select(p => p).FirstOrDefault().Orders = o.Orders;
                    });

                    _db.UpdateRange(existingOrders);
                }
            
                await _db.SaveChangesAsync();
                
                return Ok("Production Orders successfully set!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }

        [HttpPost("selldirect")]
        public async Task<ActionResult> PostSellDirectOrders()
        {
            var sellDirectOrders = new List<SellDirectItem>();
            
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body.Length != 0)
                {
                    JObject o = JObject.Parse(body);
                    JArray a = (JArray)o["SelldirectOrders"];
                    sellDirectOrders = a.ToObject<List<SellDirectItem>>();
                }
            }
            
            try
            {
                var existingOrders = await _db.SellDirectItems
                    .AsNoTracking()
                    .Select(o => o)
                    .ToListAsync();

                var updatedOrders = new List<SellDirectItem>();

                if (!existingOrders.Any())
                {
                    await _db.AddRangeAsync(sellDirectOrders);
                }
                else
                {
                    sellDirectOrders.ForEach(o =>
                    {
                        var e = existingOrders.Where(p => p.Article.Equals(o.Article))
                            .Select(o => o).FirstOrDefault();
                        
                        e.Quantity = o.Quantity;
                        e.Penalty = o.Penalty;
                        e.Price = o.Price;

                        updatedOrders.Add(e);
                    });

                    _db.UpdateRange(updatedOrders);
                }

                await _db.SaveChangesAsync();

                return Ok("Selldirect Orders successfully placed!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }
        
        [HttpGet("productionorder")]
        public async Task<ActionResult> GetProductionOrders()
        {
            var productionOrders = await _db.ProductionOrders
                .AsNoTracking()
                .Select(o => o)
                .ToListAsync();

            if (productionOrders.Any())
            {
                return Ok(productionOrders);
            }
            else
            {
                return BadRequest("No data found. Please insert your program!");
            }
        }

        [HttpGet("selldirect")]
        public async Task<ActionResult> GetSellDirectOrders()
        {
            var sellDirectOrders = await _db.SellDirectItems
                .AsNoTracking()
                .Select(s => s)
                .ToListAsync();

            if (sellDirectOrders.Any())
            {
                return Ok(sellDirectOrders);
            }
            else
            {
                return BadRequest("No data found. Please insert your program!");
            }
        }
    }
}
