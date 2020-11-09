using IBSYS.PPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionOrdersController : ControllerBase
    {
        private readonly ILogger<ProductionOrdersController> _logger;
        private readonly IbsysDatabaseContext _db;

        public ProductionOrdersController(ILogger<ProductionOrdersController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpPost]
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
                await _db.AddRangeAsync(productionOrders);
            
                await _db.SaveChangesAsync();
                
                return Ok("Production Orders successfully set!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }
    }
}
