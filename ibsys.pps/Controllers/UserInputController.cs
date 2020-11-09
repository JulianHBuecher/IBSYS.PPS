using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Input;
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
                await _db.AddRangeAsync(productionOrders);
            
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
                await _db.AddRangeAsync(sellDirectOrders);

                await _db.SaveChangesAsync();

                return Ok("Selldirect Orders successfully placed!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }
    }
}
