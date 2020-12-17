using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
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
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OptimizationController : ControllerBase
    {
        private readonly ILogger<OptimizationController> _logger;
        private readonly IbsysDatabaseContext _db;
        private readonly OptimizationService _service;

        public OptimizationController(ILogger<OptimizationController> logger, IbsysDatabaseContext db,
            OptimizationService service)
        {
            _logger = logger;
            _db = db;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetOptimizedParts()
        {
            try
            {
                var optimizedProduction = await _service.OptimizeProductionOrder();
                
                return Ok(optimizedProduction
                    .Where(o => Convert.ToInt32(o.Quantity) > 0)
                    .Select(o => o));
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }

        [HttpPost("syncresult")]
        public async Task<ActionResult> PersistOptimizedParts([FromBody] List<OptimizedPart> optimizedPartHierarchie)
        {
            try
            {
                var optimizedParts = await _db.OptimizedParts
                    .Select(op => op)
                    .ToListAsync();

                var updatedOptimizedParts = new List<OptimizedPart>();

                if (optimizedParts.Any())
                {
                    _db.OptimizedParts.RemoveRange(optimizedParts);
                }

                await _db.AddRangeAsync(optimizedPartHierarchie);

                await _db.SaveChangesAsync();

                return Ok("Data sucessfully inserted!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}");
            }
        }

        [HttpGet("persisted")]
        public async Task<ActionResult> GetPersistedOptimizedParts()
        {
            var optimizedParts = await _db.OptimizedParts
                .AsNoTracking()
                .Select(op => op)
                .ToListAsync();

            if (optimizedParts.Any())
            {
                return Ok(optimizedParts.OrderBy(op => op.Optimized));
            }
            else
            {
                return BadRequest("No data found. Please calculate your optimized parts first!");
            }
        }
    }

    
}
