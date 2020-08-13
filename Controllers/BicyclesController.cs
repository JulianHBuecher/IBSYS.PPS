using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBSYS.PPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BicyclesController : ControllerBase
    {
        private readonly ILogger<BicyclesController> _logger;

        private readonly IbsysDatabaseContext _db;

        public BicyclesController(ILogger<BicyclesController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET - All bicycle by product name
        [HttpGet]
        public async Task<List<BillOfMaterial>> GetAllBicycles()
        {
            var listOfBicycles = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .ToListAsync();

            foreach (var b in listOfBicycles)
            {
                foreach (var material in b.RequiredMaterials)
                {
                    material.MaterialNeeded = await GetNestedMaterials(material);
                }
            }

            return listOfBicycles;
        }

        // GET - One bicycle with parts by product name
        [HttpGet("{id}")]
        public async Task<BillOfMaterial> GetOneBicycle(string id)
        {
            var bicycle = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .FirstOrDefaultAsync(b => b.ProductName == id);

            foreach (var material in bicycle.RequiredMaterials)
            {
                material.MaterialNeeded = await GetNestedMaterials(material);
            }

            return bicycle;
        }

        public async Task<List<Material>> GetNestedMaterials(Material m)
        {
            var nestedMaterials = await _db.Materials
                .AsNoTracking()
                .Include(nm => nm.ParentMaterial)
                .Where(nm => nm.ParentMaterial.ID.Equals(m.ID))
                .Select(nm => nm)
                .ToListAsync();


            m.MaterialNeeded = new List<Material>();

            if (nestedMaterials.Count != 0)
            {
                foreach (var nm in nestedMaterials)
                {
                    nm.MaterialNeeded = await GetNestedMaterials(nm);
                }
                m.MaterialNeeded = nestedMaterials;
            }

            return m.MaterialNeeded;
        }
    }
}
