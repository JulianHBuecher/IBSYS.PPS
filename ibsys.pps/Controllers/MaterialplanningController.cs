using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Materialplanning;
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
using System.Threading.Tasks;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialplanningController : ControllerBase
    {
        private readonly ILogger<MaterialplanningController> _logger;
        private readonly IbsysDatabaseContext _db;

        public MaterialplanningController(ILogger<MaterialplanningController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpPost]
        public async void Materialplanning()
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

            double[,] productionMatrix = new double[3,4];

            for (var i = 0; i < productionOrders.Count(); i++)
            {
                for (var j = 0; j < productionOrders[i].Orders.Count(); j++)
                {
                    productionMatrix[i,j] = productionOrders[i].Orders[j];
                }
            }

            // Extract bicycles per number for filtering the needed materials
            var bicycleParts = await _db.BillOfMaterials
                .AsNoTracking()
                .Include(b => b.RequiredMaterials)
                .Select(b => b)
                .ToListAsync();

            foreach (var bicycle in bicycleParts)
            {
                foreach (var material in bicycle.RequiredMaterials)
                {
                    material.MaterialNeeded = await GetNestedMaterials(material);
                }
            }

            var extractedBicyclePOne = new List<Material>();
            var extractedBicyclePTwo = new List<Material>();
            var extractedBicyclePThree = new List<Material>();

            foreach (var bicycle in new List<List<Material>> { extractedBicyclePOne, extractedBicyclePTwo, extractedBicyclePThree })
            {
                foreach (var b in bicycleParts)
                {
                    foreach (var material in b.RequiredMaterials)
                    {
                        var filteredPart = await FilterNestedMaterialsByName("K", material);
                        bicycle.AddRange(filteredPart);
                    }
                }
            }

            var summedPartsPOne = SumFilteredMaterials(extractedBicyclePOne);
            var summedPartsPTwo = SumFilteredMaterials(extractedBicyclePTwo);
            var summedPartsPThree = SumFilteredMaterials(extractedBicyclePThree);

            var completedPartsForP1 = InsertNotNeededMaterials(new List<Material>(summedPartsPOne), new List<Material>(summedPartsPTwo), new List<Material>(summedPartsPThree));
            var completedPartsForP2 = InsertNotNeededMaterials(new List<Material>(summedPartsPTwo), new List<Material>(summedPartsPOne), new List<Material>(summedPartsPThree));
            var completedPartsForP3 = InsertNotNeededMaterials(new List<Material>(summedPartsPThree), new List<Material>(summedPartsPOne), new List<Material>(summedPartsPTwo));


            var neededMaterialVecP1 = Vector<Double>.Build
                .DenseOfEnumerable(summedPartsPOne.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP2 = Vector<Double>.Build
                .DenseOfEnumerable(summedPartsPTwo.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP3 = Vector<Double>.Build
                .DenseOfEnumerable(summedPartsPThree.Select(p => Convert.ToDouble(p.QuantityNeeded)));

            var neededMaterialMatrix = Matrix<Double>.Build.DenseOfColumnVectors(neededMaterialVecP1, neededMaterialVecP2, neededMaterialVecP3);

            // Matrix Multiplikation for Calculation of required parts
            var productionOrderMatrix = Matrix<Double>.Build.DenseOfArray(productionMatrix);

            var calculatedNewParts = neededMaterialMatrix.Multiply(productionOrderMatrix);


        }

        [NonAction]
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
        [NonAction]
        public async Task<List<Material>> FilterNestedMaterialsByName(string parts, Material ml)
        {
            var partsForBicycle = new List<Material>();

            if (ml.MaterialName.StartsWith(parts))
            {
                partsForBicycle.Add(new Material { MaterialName = ml.MaterialName, QuantityNeeded = ml.QuantityNeeded });
                if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded.Count != 0)
                {
                    foreach (var material in ml.MaterialNeeded)
                    {
                        partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, material));
                    }
                }
            }
            else
            {
                if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded.Count != 0)
                {
                    foreach (var material in ml.MaterialNeeded)
                    {
                        partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, material));
                    }
                }
            }

            return partsForBicycle;
        }

        [NonAction]
        public List<Material> SumFilteredMaterials(List<Material> materials)
        {
            var partsOrderedAndSummed = materials.GroupBy(p => p.MaterialName).OrderBy(p => p.Key)
                .Select(p => new Material
                {
                    MaterialName = p.Key,
                    QuantityNeeded = p.Select(pp => pp.QuantityNeeded).Sum()
                })
                .ToList();

            return partsOrderedAndSummed;
        }

        [NonAction]
        public List<Material> InsertNotNeededMaterials(List<Material> listForInsert, List<Material> referenceListTwo, 
            List<Material> referenceListThree)
        {
            var initialCount = listForInsert.Count();

            for (var i = 0; i < initialCount; i++)
            {
                if (listForInsert[i].MaterialName != referenceListTwo[i].MaterialName &&
                    !listForInsert.Any(part => part.MaterialName == referenceListTwo[i].MaterialName))
                {
                    listForInsert.Insert(i, new Material
                    {
                        MaterialName = referenceListTwo[i].MaterialName,
                        QuantityNeeded = 0
                    });
                }
                if (listForInsert[i].MaterialName != referenceListThree[i].MaterialName &&
                    !listForInsert.Any(part => part.MaterialName == referenceListThree[i].MaterialName))
                {
                    listForInsert.Insert(i, new Material
                    {
                        MaterialName = referenceListThree[i].MaterialName,
                        QuantityNeeded = 0
                    });
                }
            }
            return listForInsert.OrderBy(p => Convert.ToInt32(p.MaterialName.Split(" ")[1])).ToList();
        }

        [NonAction]
        public async Task<List<Material>> FilterKMaterialsByNameAndEPart(string parts, string ePart, Material ml)
        {
            var partsForBicycle = new List<Material>();

            if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded.Count != 0)
            {
                if (ml.MaterialName.Equals(ePart))
                {
                    partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, ml));
                }
                else
                {
                    foreach (var material in ml.MaterialNeeded)
                    {
                        if (ml.MaterialName.Equals(ePart))
                        {
                            partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, ml));
                        }
                        partsForBicycle.AddRange(await FilterKMaterialsByNameAndEPart(parts, ePart, material));
                    }
                }
            }

            return partsForBicycle;
        }

    }
}
