using IBSYS.PPS.Models;
using IBSYS.PPS.Services;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ValidationController : ControllerBase
    {
        private readonly ILogger<ValidationController> _logger;
        private readonly IbsysDatabaseContext _db;
        private readonly ValidationService _service;

        public ValidationController(ILogger<ValidationController> logger, IbsysDatabaseContext db,
            ValidationService service)
        {
            _logger = logger;
            _db = db;
            _service = service;
        }


        [HttpGet("required/capacity")]
        public async Task<ActionResult> CapacityValidation()
        {
            var productionOrders = await _db.ProductionOrders
                .AsNoTracking()
                .Select(po => po)
                .ToListAsync();

            var selldirectItems = await _db.SellDirectItems
                .AsNoTracking()
                .Select(s => s)
                .ToListAsync();

            var changedRequirements = await _db.DispositionEParts
                .AsNoTracking()
                .Where(d => d.Name.Equals("P1") || d.Name.Equals("P2") || d.Name.Equals("P3"))
                .Select(d => new { d.Name, d.Quantity })
                .ToListAsync();

            productionOrders = _service.AddSelldirectItems(productionOrders, selldirectItems);

            double[,] productionMatrix;

            var changedRequirementsTupel = changedRequirements.Select(d => (Bicycle: d.Name, Amount: d.Quantity)).ToList();

            productionMatrix = _service.CheckForChangedProductionOrders(productionOrders, changedRequirementsTupel);
            
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

                return Ok(orders
                    .Where(o => (o.OrderModus != 0 && Convert.ToInt32(o.OrderQuantity) != 0 && o.OrderQuotient == 0))
                    .Select(o => o).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest($"Something bad happens, {ex.Message}");
            }
        }

        [HttpGet("required/time")]
        public async Task<ActionResult> RequiredTimeValidation()
        {
            try
            {
                var productionOrders = await _db.ProductionOrders
                    .AsNoTracking()
                    .Select(po => po)
                    .ToListAsync();

                var selldirectItems = await _db.SellDirectItems
                    .AsNoTracking()
                    .Select(s => s)
                    .ToListAsync();

                var changedRequirements = await _db.DispositionEParts
                    .AsNoTracking()
                    .Where(d => d.Name.Equals("P1") || d.Name.Equals("P2") || d.Name.Equals("P3"))
                    .Select(d => new { d.Name, d.Quantity })
                    .ToListAsync();

                productionOrders = _service.AddSelldirectItems(productionOrders, selldirectItems);

                double[,] productionMatrix;

                var changedRequirementsTupel = changedRequirements.Select(d => (Bicycle: d.Name, Amount: d.Quantity)).ToList();

                productionMatrix = _service.CheckForChangedProductionOrders(productionOrders, changedRequirementsTupel);

                var requiredTime = productionMatrix[0, 0] * 6 + productionMatrix[1, 0] * 7 + productionMatrix[2, 0] * 7;

                if (requiredTime > 7200)
                {
                    return BadRequest("Not enough time to produce all parts");
                }
                else
                {
                    return Ok("Required Time is ok!");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong, {e.Message}");
            }
        }

    }
}
