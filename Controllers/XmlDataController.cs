using IBSYS.PPS.Models;
using IBSYS.PPS.Serializer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XmlDataController : ControllerBase
    {
        private readonly ILogger<XmlDataController> _logger;

        private readonly IbsysDatabaseContext _db;

        public XmlDataController(ILogger<XmlDataController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET
        [HttpGet]
        public ActionResult<Input> GetAll()
        {
            DataSerializer serializer = new DataSerializer();

            var input = serializer.ReadDataAndDeserialize(@"./Assets/input.xml");

            if (input == null)
            {
                return NotFound();
            }

            return input;
        }

        // GET all bicycle parts
        [HttpGet("P1")]
        public async Task<List<BillOfMaterial>> GetBicycleP1()
        {
            var listOfBicycles = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Where(b => b.ProductName == "P1")
                .Select(b => b)
                .ToListAsync();

            foreach (var b in listOfBicycles)
            {
                foreach (var rm in b.RequiredMaterials)
                {
                    var nestedMaterials = await _db.Materials
                        .AsNoTracking()
                        .Include(m => m.ParentMaterial)
                        .Where(m => m.ParentMaterial.ID == rm.ID)
                        .Select(m => m)
                        .ToListAsync();


                    rm.MaterialNeeded = new List<Material>();

                    if (nestedMaterials != null)
                    {
                        foreach (var nm in nestedMaterials)
                        {
                            var secondnestedMaterials = await _db.Materials
                                .AsNoTracking()
                                .Include(m => m.ParentMaterial)
                                .Where(m => m.ParentMaterial.ID == nm.ID)
                                .Select(m => m)
                                .ToListAsync();

                            nm.MaterialNeeded = new List<Material>();

                            if (secondnestedMaterials != null)
                            {
                                foreach (var nnm in secondnestedMaterials)
                                {
                                    var thirdnestedMaterials = await _db.Materials
                                        .AsNoTracking()
                                        .Include(m => m.ParentMaterial)
                                        .Where(m => m.ParentMaterial.ID == nnm.ID)
                                        .Select(m => m)
                                        .ToListAsync();

                                    nnm.MaterialNeeded = new List<Material>();

                                    if (thirdnestedMaterials != null)
                                    {
                                        foreach (var nnnm in thirdnestedMaterials)
                                        {
                                            var fourthnestedMaterials = await _db.Materials
                                                .AsNoTracking()
                                                .Include(m => m.ParentMaterial)
                                                .Where(m => m.ParentMaterial.ID == nnnm.ID)
                                                .Select(m => m)
                                                .ToListAsync();

                                            nnnm.MaterialNeeded = new List<Material>();

                                            if (fourthnestedMaterials != null)
                                            {
                                                nnnm.MaterialNeeded.AddRange(fourthnestedMaterials.OrderBy(m => m.ID));
                                                nnm.MaterialNeeded = nnm.MaterialNeeded.Concat(thirdnestedMaterials.OrderBy(m => m.ID)).ToList();
                                                nm.MaterialNeeded = nm.MaterialNeeded.Concat(secondnestedMaterials.OrderBy(m => m.ID)).ToList();
                                                rm.MaterialNeeded = rm.MaterialNeeded.Concat(nestedMaterials.OrderBy(m => m.ID)).ToList();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return listOfBicycles;
        }
    }
}