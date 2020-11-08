using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Models.Capacity;
using IBSYS.PPS.Controllers;
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
    public class CapacityController : ControllerBase
    {
        private readonly ILogger<BicyclesController> _logger;

        private readonly IbsysDatabaseContext _db;

        private readonly BicyclesController _bicyclesController;

        public CapacityController(ILogger<BicyclesController> logger, IbsysDatabaseContext db, BicyclesController bicyclesController)
        {
            _logger = logger;
            _db = db;
            _bicyclesController = bicyclesController;
        }

        [HttpPost("capacity")]
        public async Task<ActionResult> GetCapacityRequirements()
        {
            var plannedStocks = new Dictionary<String, List<PlannedWarehouseStock>>();

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body.Length != 0)
                {
                    JObject o = JObject.Parse(body);
                    foreach (string id in new string[] { "p1", "p2", "p3" })
                    {
                        JArray a = (JArray)o[id]["PlannedStocks"];
                        var plannedStock = a.ToObject<List<PlannedWarehouseStock>>();
                        plannedStocks.Add(id, plannedStock);
                    }
                }
            }
            var capRequirements = await ExecuteCapacityRequirements(100, plannedStocks);
            return Ok(capRequirements);
        }

        public async Task<Dictionary<int, int>> ExecuteCapacityRequirements(int salesOrders, Dictionary<String, List<PlannedWarehouseStock>> plannedWarehouseStocks)
        {
            #region Capacity Data
            Dictionary<String, List<CapacityRequirement>> capData = new Dictionary<String, List<CapacityRequirement>>();
            capData.Add("P1", new List<CapacityRequirement>() { new CapacityRequirement(6, 30, 4) });
            capData.Add("P2", new List<CapacityRequirement>() { new CapacityRequirement(7, 20, 4) });
            capData.Add("P3", new List<CapacityRequirement>() { new CapacityRequirement(7, 30, 4) });
            capData.Add("E4", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E5", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E6", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 20, 11) });
            capData.Add("E7", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E8", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 10, 11) });
            capData.Add("E9", new List<CapacityRequirement>() { new CapacityRequirement(4, 20, 10), new CapacityRequirement(3, 20, 11) });
            capData.Add("E10", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(1, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E11", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E12", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E13", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(1, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E14", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E15", new List<CapacityRequirement>() { new CapacityRequirement(2, 20, 7), new CapacityRequirement(2, 15, 8), new CapacityRequirement(3, 15, 9), new CapacityRequirement(3, 0, 12), new CapacityRequirement(2, 0, 13) });
            capData.Add("E16*", new List<CapacityRequirement>() { new CapacityRequirement(2, 15, 6), new CapacityRequirement(3, 0, 14) });
            capData.Add("E17*", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 15) });
            capData.Add("E18", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 6), new CapacityRequirement(2, 20, 7), new CapacityRequirement(3, 20, 8), new CapacityRequirement(2, 15, 9) });
            capData.Add("E19", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 6), new CapacityRequirement(2, 20, 7), new CapacityRequirement(3, 25, 8), new CapacityRequirement(2, 20, 9) });
            capData.Add("E20", new List<CapacityRequirement>() { new CapacityRequirement(3, 15, 6), new CapacityRequirement(2, 20, 7), new CapacityRequirement(3, 20, 8), new CapacityRequirement(2, 15, 9) });
            capData.Add("E26*", new List<CapacityRequirement>() { new CapacityRequirement(2, 30, 7), new CapacityRequirement(3, 15, 15) });
            capData.Add("E29", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 1) });
            capData.Add("E30", new List<CapacityRequirement>() { new CapacityRequirement(5, 20, 2) });
            capData.Add("E31", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 3) });
            capData.Add("E49", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 1) });
            capData.Add("E50", new List<CapacityRequirement>() { new CapacityRequirement(5, 30, 2) });
            capData.Add("E51", new List<CapacityRequirement>() { new CapacityRequirement(5, 20, 3) });
            capData.Add("E54", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 1) });
            capData.Add("E55", new List<CapacityRequirement>() { new CapacityRequirement(5, 30, 2) });
            capData.Add("E56", new List<CapacityRequirement>() { new CapacityRequirement(6, 20, 3) });
            #endregion

            var capRequirements = new Dictionary<int, int>();
            var setupTimes = new Dictionary<int, int>();
            foreach (KeyValuePair<String, List<PlannedWarehouseStock>> pair in plannedWarehouseStocks)
            {
                var dispositionData = _bicyclesController.ExecuteDisposition(pair.Key, 100, pair.Value);
                foreach (BicyclePart part in dispositionData.Result.parts)
                {
                    var capacityData = capData[part.name];
                    foreach (CapacityRequirement requirement in capacityData)
                    {
                        int time;
                        int quantity = Convert.ToInt32(part.quantity);
                        if (capRequirements.TryGetValue(requirement.workStation, out time))
                        {
                            time += requirement.processTime * quantity;
                        }
                        else
                        {
                            capRequirements.Add(requirement.workStation, requirement.processTime * quantity);
                        }
                        int setupTime;
                        if (setupTimes.TryGetValue(requirement.workStation, out setupTime))
                        {
                            setupTime = (setupTime + requirement.setupTime) / 2;
                        }
                        else
                        {
                            setupTimes.Add(requirement.workStation, requirement.setupTime);
                        }
                    }
                }
            }

            List<int> workstations = new List<int>(capRequirements.Keys);
            foreach (var workstation in workstations)
            {
                var setupEvents = await _db.SetupEvents.AsNoTracking()
                    .Where(setupEvent => setupEvent.WorkplaceId.Equals(workstation))
                    .Select(setupEvent => setupEvent.NumberOfSetupEvents)
                    .FirstOrDefaultAsync();

                capRequirements[workstation] += setupEvents * setupTimes[workstation];

                var waitinglistWorkstations = await _db.WaitinglistWorkstations.AsNoTracking()
                    .Where(ws => Convert.ToInt32(ws.WorkplaceId).Equals(workstation) && ws.TimeNeed > 0)
                    .Select(ws => ws.TimeNeed).FirstOrDefaultAsync();

                var waitinglistMissingParts = await _db.WaitinglistStock.AsNoTracking()
                     .Include(w => w.WaitinglistForStock).ThenInclude(w => w.WaitinglistForWorkplaceStock)
                     .Select(w => w.WaitinglistForStock
                         .Where(wws => Convert.ToInt32(wws.WorkplaceId).Equals(workstation))
                         .Select(ws => ws.WaitinglistForWorkplaceStock
                         .Where(wss => wss.TimeNeed > 0).Select(wss => wss.TimeNeed).Sum())).FirstOrDefaultAsync();

                var workInProgress = await _db.OrdersInWork.AsNoTracking()
                   .Where(wip => wip.Id.Equals(workstation) && wip.TimeNeed > 0)
                   .Select(oiw => oiw.TimeNeed).SumAsync();

                foreach (var value in new List<int> { waitinglistWorkstations, workInProgress, workInProgress })
                {
                    capRequirements[workstation] += value;
                }
            }

            return capRequirements;
        }
    }
}
