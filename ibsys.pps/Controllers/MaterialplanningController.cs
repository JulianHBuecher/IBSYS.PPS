using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Materialplanning;
using IBSYS.PPS.Services;
using MathNet.Numerics.LinearAlgebra;
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
    public class MaterialplanningController : ControllerBase
    {
        private readonly ILogger<MaterialplanningController> _logger;
        private readonly IbsysDatabaseContext _db;
        private readonly MaterialplanningService _service;
        public MaterialplanningController(ILogger<MaterialplanningController> logger, IbsysDatabaseContext db, 
            MaterialplanningService service)
        {
            _logger = logger;
            _db = db;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Materialplanning()
        {
            var productionOrders = await _db.ProductionOrders
                .AsNoTracking()
                .Select(po => po)
                .ToListAsync();

            var changedRequirements = await _db.DispositionEParts
                .AsNoTracking()
                .Where(d => d.Name.Equals("P1") || d.Name.Equals("P2") || d.Name.Equals("P3"))
                .Select(d => new { d.Name, d.Quantity })
                .ToListAsync();

            var selldirectItems = await _db.SellDirectItems
                .AsNoTracking()
                .Select(s => s)
                .ToListAsync();

            var placedOrders = await _db.OrdersForK
                .Include(o => o.OptimalOrderQuantity)
                .Select(o => o)
                .ToListAsync();

            productionOrders = _service.AddSelldirectItems(productionOrders, selldirectItems);

            var changedRequirementsTupel = changedRequirements.Select(d => (Bicycle: d.Name, Amount: d.Quantity)).ToList();

            var productionMatrix = _service.CheckForChangedProductionOrders(productionOrders, changedRequirementsTupel);

            // Extract bicycles per number for filtering the needed materials
            var bicycleParts = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .ToListAsync();

            var (p1, p2, p3) = await _service.ExtractBicycles(bicycleParts);

            var completedPartList = await _service.CreateCompleteMaterialListForBicycle(p1, p2, p3);

            var neededMaterialMatrix = await _service.CreateNeededMaterialMatrix(p1, p2, p3);
            
            // Matrix Multiplikation for Calculation of required parts
            var productionOrderMatrix = Matrix<Double>.Build.DenseOfArray(productionMatrix);

            var calculatedNewParts = neededMaterialMatrix.Multiply(productionOrderMatrix);

            try
            {
                var orders = await _service.PlaceOrder(calculatedNewParts, completedPartList, p1, p2, p3);

                if (placedOrders.Any())
                {
                    return Ok(placedOrders
                        .Select(o => o)
                        .OrderBy(o => Regex.Match(o.PartName, @"\d+").Value)
                        .ToList());
                }
                else
                {
                    return Ok(orders
                        .Where(o => (o.OrderModus != 0 && Convert.ToInt32(o.OrderQuantity) != 0))
                        .Select(o => o).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Something bad happens, {ex.Message}");
            }
        }

        [HttpPost("syncresult")]
        public async Task<ActionResult> PostResultForPersistence([FromBody] List<OrderForK> orderPlacements)
        {
            try
            {
                if (orderPlacements != null)
                {
                    var placedOrders = await _db.OrdersForK
                        .Include(o => o.OptimalOrderQuantity)
                        .Select(o => o)
                        .ToListAsync();

                    if (!placedOrders.Any())
                    {
                        await _db.AddRangeAsync(orderPlacements);
                    }
                    else
                    {
                        _db.RemoveRange(placedOrders);
                        await _db.AddRangeAsync(orderPlacements);
                    }
                }

                await _db.SaveChangesAsync();

                return Ok("Data sucessfully inserted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong, {ex.Message}. Inner Exception, {ex.InnerException}");
            }
        }

        [HttpGet("placedorders")]
        public async Task<ActionResult> GetPlacedOrders()
        {
            var placedOrders = await _db.OrdersForK
                .AsNoTracking()
                .Include(o => o.OptimalOrderQuantity)
                .Select(o => o)
                .ToListAsync();

            if (placedOrders.Any())
            {
                return Ok(placedOrders);
            }
            else
            {
                return BadRequest("No data found. Please calculate and persist your results!");
            }
        }

        [HttpGet("kpart/{partnumber}")]
        public async Task<ActionResult> GetKPartByNumber([FromRoute] string partnumber)
        {
            var kPart = await _db.PurchasedItems
                .AsNoTracking()
                .Where(p => p.ItemNumber.Equals($"K {partnumber}"))
                .Select(p => p)
                .FirstOrDefaultAsync();

            var material = await _db.Materials
                .AsNoTracking()
                .Where(m => m.MaterialName.Equals($"K {partnumber}"))
                .Select(m => m)
                .FirstOrDefaultAsync();

            var productionOrders = await _db.ProductionOrders
                .AsNoTracking()
                .Select(po => po)
                .ToListAsync();

            var changedRequirements = await _db.DispositionEParts
                .AsNoTracking()
                .Where(d => d.Name.Equals("P1") || d.Name.Equals("P2") || d.Name.Equals("P3"))
                .Select(d => new { d.Name, d.Quantity })
                .ToListAsync();

            var changedRequirementsTupel = changedRequirements.Select(d => (Bicycle: d.Name, Amount: d.Quantity)).ToList();

            var productionMatrix = _service.CheckForChangedProductionOrders(productionOrders, changedRequirementsTupel);

            // Extract bicycles per number for filtering the needed materials
            var bicycleParts = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .ToListAsync();

            var (p1, p2, p3) = await _service.ExtractBicycles(bicycleParts);

            var completedPartList = await _service.CreateCompleteMaterialListForBicycle(p1, p2, p3);

            var neededMaterialMatrix = await _service.CreateNeededMaterialMatrix(p1, p2, p3);

            // Matrix Multiplikation for Calculation of required parts
            var productionOrderMatrix = Matrix<Double>.Build.DenseOfArray(productionMatrix);

            var calculatedNewParts = neededMaterialMatrix.Multiply(productionOrderMatrix);

            var orders = await _service.PlaceOrder(calculatedNewParts, completedPartList, p1, p2, p3);

            var order = orders.Where(o => o.PartName.Equals($"K {partnumber}")).Select(o => o).FirstOrDefault();

            if (order != null)
            {
                return Ok(new ExtendedKPart 
                {
                    ItemNumber = order.PartName,
                    Description = kPart.Description,
                    DiscountQuantity = kPart.DiscountQuantity,
                    OrderCosts = kPart.OrderCosts,
                    AdditionalParts = order.AdditionalParts,
                    Stock = order.Stock,
                    Requirements = order.Requirements,
                    OrderQuotient = order.OrderQuotient,
                    OptimalOrderQuantity = order.OptimalOrderQuantity
                });
            }
            else
            {
                return NotFound("No purchasable item with this number found!");
            }
        }
    }
}
