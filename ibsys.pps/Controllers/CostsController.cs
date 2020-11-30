using IBSYS.PPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostsController : ControllerBase
    {
        private readonly ILogger<CostsController> _logger;
        private readonly IbsysDatabaseContext _db;

        public CostsController(ILogger<CostsController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Return the labor and machine costs for all machines and shifts
        [HttpGet("laborandmachine")]
        public async Task<ActionResult> GetLaborAndMachineCosts()
        {
            try
            {
                var costs = await _db.LaborAndMachineCosts
                    .AsNoTracking().Select(c => c).ToListAsync();

                if (costs.Any())
                {
                    return Ok(costs.OrderBy(c => c.Workplace));
                }
                else
                {
                    throw new Exception("No data found!");
                }
            } 
            catch (Exception e)
            {
                return BadRequest($"Something went wrong, {e.Message}");
            }

        }
    }
}
