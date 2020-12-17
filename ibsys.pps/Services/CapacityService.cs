using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Capacity;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Models.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Services
{
    public class CapacityService
    {
        private readonly ILogger<CapacityService> _logger;
        private readonly IbsysDatabaseContext _db;

        public CapacityService(ILogger<CapacityService> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<Dictionary<int, CapacityRequirementExtended>> ExecuteCapacityRequirements(List<BicyclePart> dispositionData)
        {
            #region Capacity Data
            var capData = new Dictionary<String, List<CapacityRequirement>>();
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

            // Includes every workstation with the regarding capacity requirement (new)
            var capRequirements = new Dictionary<int, int>();

            var setupTimes = new Dictionary<int, int>();

            foreach (var part in dispositionData)
            {
                var capacityData = capData[part.Name];

                foreach (var requirement in capacityData)
                {
                    // Amount of parts for production
                    var quantity = Convert.ToInt32(part.Quantity);
                    // Time required for the production of this amount of parts
                    int time;
                    // If the workstation exists in the list, give back the existing capacity requirement
                    if (capRequirements.TryGetValue(requirement.workStation, out time))
                    {
                        time += requirement.processTime * quantity;
                        capRequirements[requirement.workStation] = time;
                    }
                    // If not, create a new item in the workstation item in the list
                    // with the regarding capacity requirement for the specific part
                    else
                    {
                        capRequirements.Add(requirement.workStation, requirement.processTime * quantity);
                    }

                    int setupTime;
                    // If the workstation exists with their Setup-Events and Setup-Times, give them back
                    if (setupTimes.TryGetValue(requirement.workStation, out setupTime))
                    {
                        setupTime = (setupTime + requirement.setupTime) / 2;
                        setupTimes[requirement.workStation] = setupTime;
                    }
                    // If not, add the workstation with the regarding values
                    else
                    {
                        setupTimes.Add(requirement.workStation, requirement.setupTime);
                    }
                }
            }

            List<int> workstations = new List<int>(capRequirements.Keys);

            var requirements = new Dictionary<int, CapacityRequirementExtended>();

            foreach (var workstation in workstations)
            {
                // Aquire Setup-Events from last period for this workplace
                // Serves as prognosis for the next period
                var setupEvents = await _db.SetupEvents
                    .AsNoTracking()
                    .Where(s => s.WorkplaceId.Equals(workstation.ToString()))
                    .Select(s => s.NumberOfSetupEvents)
                    .FirstOrDefaultAsync();

                capRequirements[workstation] += setupEvents * setupTimes[workstation];

                // Aquire the time needed from orders out of the queue (not enough time)
                var waitinglistWorkstations = await _db.WaitinglistWorkstations
                    .AsNoTracking()
                    .Where(ws => ws.WorkplaceId.Equals(workstation.ToString()) && ws.TimeNeed > 0)
                    .Select(ws => ws.TimeNeed)
                    .FirstOrDefaultAsync();

                // Aquire the time needed from orders out of the queue (not enough parts)
                var waitinglistMissingParts = await _db.WaitinglistStock
                    .AsNoTracking()
                    .Include(wls => wls.WaitinglistForStock).ThenInclude(wls => wls.WaitinglistForWorkplaceStock)
                    .Select(wls => wls.WaitinglistForStock
                        .Where(wls => wls.WorkplaceId.Equals(workstation.ToString()))
                        .Select(wlfs => wlfs.TimeNeed).Sum())
                    .ToListAsync();

                // Aquire the time needed from orders out of the queue (not enough time)
                var workInProgress = await _db.OrdersInWork.AsNoTracking()
                   .Where(wip => wip.Id.Equals(workstation.ToString()) && wip.TimeNeed > 0)
                   .Select(oiw => oiw.TimeNeed).SumAsync();

                foreach (var value in new List<int> { waitinglistWorkstations, waitinglistMissingParts.Sum(), workInProgress })
                {
                    capRequirements[workstation] += value;
                }

                requirements.Add(workstation, new CapacityRequirementExtended
                {
                    Workstation = workstation,
                    TimeFromWaitinglist = waitinglistWorkstations,
                    TimeFromWiP = workInProgress,
                    SetupEvents = setupEvents,
                    SetupTime = setupTimes[workstation],
                    RequiredCapacity = capRequirements[workstation]
                });
            }
            return requirements;
        }

        /// <summary>
        /// Method to check, if the shifts and overtimes don't break their limits
        /// </summary>
        /// <param name="w">A single workingtime object</param>
        public void CheckForConsistency(Workingtime w)
        {
            if ((w.Shift.Equals("1") || w.Shift.Equals("2")) && Convert.ToInt32(w.Overtime) > 240)
            {
                throw new Exception("Overtime have to be lesser than 240 minutes a day!");
            }
            if (w.Shift.Equals("3") && Convert.ToInt32(w.Overtime) > 0)
            {
                throw new Exception("In third shift you could not set overtime!");
            }
            if (Convert.ToInt32(w.Overtime) < 0)
            {
                throw new Exception("Overtime could not be negative!");
            }
        }
    }
}
