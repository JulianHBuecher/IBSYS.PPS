using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Input;
using IBSYS.PPS.Models.Materialplanning;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Services
{
    public class ValidationService
    {
        private readonly ILogger<ValidationService> _logger;
        private readonly IbsysDatabaseContext _db;

        public ValidationService(ILogger<ValidationService> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
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

        public async Task<List<Material>> FilterNestedMaterialsByName(string parts, Material ml)
        {
            var partsForBicycle = new List<Material>();

            if (ml.MaterialName.StartsWith(parts))
            {
                partsForBicycle.Add(new Material { MaterialName = ml.MaterialName, QuantityNeeded = ml.QuantityNeeded, DirectAccess = ml.DirectAccess });
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

        public async Task<List<Material>> FilterKMaterialsByNameAndEPart(string parts, string ePart, Material ml)
        {
            var partsForBicycle = new List<Material>();
            if (!ePart.StartsWith("P"))
            {
                if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded != null && ml.MaterialNeeded.Count != 0)
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
            }
            else
            {
                if (ml.MaterialName.StartsWith("E") && ml.MaterialNeeded.Count != 0)
                {
                    foreach (var material in ml.MaterialNeeded)
                    {
                        partsForBicycle.AddRange(await FilterKMaterialsByNameAndEPart(parts, ePart, material));
                    }
                }
                else
                {
                    partsForBicycle.AddRange(await FilterNestedMaterialsByName(parts, ml));
                }
            }


            return partsForBicycle.Where(p => p.DirectAccess.Contains(ePart.Split(" ")[1])).Select(p => p).ToList();
        }

        public List<Material> SumFilteredMaterials(List<Material> materials)
        {
            var partsOrderedAndSummed = materials.GroupBy(p => p.MaterialName).OrderBy(p => p.Key)
                .Select(p => new Material
                {
                    MaterialName = p.Key,
                    QuantityNeeded = p.Select(pp => pp.QuantityNeeded).Sum(),
                    DirectAccess = p.Select(pp => pp.DirectAccess).First()
                })
                .ToList();

            return partsOrderedAndSummed;
        }

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
                        QuantityNeeded = 0,
                        DirectAccess = referenceListTwo[i].DirectAccess
                    });
                }
                if (listForInsert[i].MaterialName != referenceListThree[i].MaterialName &&
                    !listForInsert.Any(part => part.MaterialName == referenceListThree[i].MaterialName))
                {
                    listForInsert.Insert(i, new Material
                    {
                        MaterialName = referenceListThree[i].MaterialName,
                        QuantityNeeded = 0,
                        DirectAccess = referenceListThree[i].DirectAccess
                    });
                }
            }
            return listForInsert.OrderBy(p => Convert.ToInt32(p.MaterialName.Split(" ")[1])).ToList();
        }



        public async Task<List<OrderForK>> PlaceOrder(Matrix<Double> requiredParts, List<Material> partsForPlanning, BillOfMaterial bicycleOne, BillOfMaterial bicycleTwo, BillOfMaterial bicycleThree)
        {
            var orderPlacements = new List<OrderForK>();
            var position = 0;

            foreach (var part in partsForPlanning)
            {

                var partNumber = part.MaterialName.Split(" ")[1];

                var stockQuantity = await _db.StockValuesFromLastPeriod.AsNoTracking()
                    .Where(m => m.Id.Equals(partNumber))
                    .Select(m => m.Amount).FirstOrDefaultAsync();

                var warehouseStock = Convert.ToInt32(stockQuantity);

                var waitinglistWorkstations = await _db.WaitinglistWorkstations.AsNoTracking()
                    .Include(m => m.WaitingListForWorkplace)
                    .Select(w => w.WaitingListForWorkplace.Where(wl => part.DirectAccess.Contains(wl.Item)))
                    .SelectMany(wl => wl)
                    .ToListAsync();

                // If multiple Items in Waitinglist exists, but they are the same Waitinglist
                // Here they will be orderd and summed
                waitinglistWorkstations = waitinglistWorkstations
                    .GroupBy(wlw => new {
                        wlw.Item,
                        wlw.Batch
                    })
                    .OrderBy(wlw => wlw.Key.Item)
                    .Select(m => new WaitinglistForWorkplace
                    {
                        Amount = m.Select(p => p.Amount).First(),
                        Batch = m.Key.Batch,
                        Item = m.Key.Item,
                        TimeNeed = m.Select(p => p.TimeNeed).Sum()
                    }).ToList();

                waitinglistWorkstations = waitinglistWorkstations
                    .GroupBy(wlw => wlw.Item)
                    .OrderBy(wlw => wlw.Key)
                    .Select(m => new WaitinglistForWorkplace
                    {
                        Amount = m.Select(p => p.Amount).Sum(),
                        Batch = m.Select(p => p.Batch).First(),
                        Item = m.Key,
                        TimeNeed = m.Select(p => p.TimeNeed).Sum()
                    }).ToList();

                var waitinglistMissingParts = await _db.WaitinglistStock.AsNoTracking()
                    .Include(w => w.WaitinglistForStock).ThenInclude(w => w.WaitinglistForWorkplaceStock)
                    .SelectMany(w => w.WaitinglistForStock
                        .Select(ws => ws.WaitinglistForWorkplaceStock
                        .Where(wss => part.DirectAccess.Contains(wss.Item)).ToList()))
                    .ToListAsync();

                var missingParts = waitinglistMissingParts.SelectMany(p => p.Select(pp => pp)).ToList();

                var requiredPartsFromWaitingQueue = 0;

                // Get additional required K parts resulting from queue
                for (var i = 0; i < waitinglistWorkstations.Count(); i++)
                {
                    requiredPartsFromWaitingQueue += await ExtractAdditionalKParts(waitinglistWorkstations[i].Item, waitinglistWorkstations[i].Amount, part.MaterialName, bicycleOne, bicycleTwo, bicycleThree);
                }

                for (var i = 0; i < missingParts.Count(); i++)
                {
                    requiredPartsFromWaitingQueue += await ExtractAdditionalKParts(missingParts[i].Item, missingParts[i].Amount, part.MaterialName, bicycleOne, bicycleTwo, bicycleThree);
                }

                // Logik for Decision between E or N orders and how much
                var orderPlacement = await SetOrderTypeAndQuantity(part, stockQuantity, requiredParts.Row(position), requiredPartsFromWaitingQueue);
                orderPlacements.Add(orderPlacement);

                // Set Counter One Up
                position++;
            }
            return orderPlacements;
        }

        public async Task<int> ExtractAdditionalKParts(string ePart, int amount, string kPart, BillOfMaterial bicycleOne, BillOfMaterial bicycleTwo, BillOfMaterial bicycleThree)
        {
            if (Convert.ToInt32(ePart) <= 3)
            {
                ePart = $"P {ePart}";
            }
            else
            {
                ePart = $"E {ePart}";
            }

            var isPartOf = await _db.SelfProductionItems
                .AsNoTracking()
                .Where(sp => sp.ItemNumber.Equals(ePart))
                .Select(sp => sp)
                .FirstOrDefaultAsync();

            var bicycle = new BillOfMaterial();

            if (isPartOf.Usage != null)
            {
                bicycle = isPartOf.Usage switch
                {
                    "K" => bicycleOne,
                    "D" => bicycleTwo,
                    "H" => bicycleThree,
                    _ => bicycleOne
                };
            }
            else
            {
                bicycle = isPartOf.ItemNumber switch
                {
                    "P 1" => bicycleOne,
                    "P 2" => bicycleTwo,
                    "P 3" => bicycleThree,
                    _ => bicycleOne
                };
            }

            var partsOutOfQueue = new List<Material>();

            bicycle.RequiredMaterials.ForEach(async b =>
            {
                var parts = await FilterKMaterialsByNameAndEPart("K", ePart, b);
                partsOutOfQueue.AddRange(parts);
            });

            var summedParts = SumFilteredMaterials(partsOutOfQueue);

            var additionalAmount = summedParts.Where(s => s.MaterialName.Equals(kPart)).Select(s => s.QuantityNeeded).First();

            return additionalAmount *= amount;
        }

        public async Task<OrderForK> SetOrderTypeAndQuantity(Material material, string stockQuantity, Vector<Double> accordingRequirements, int partsFromQueue)
        {
            var kPart = await _db.PurchasedItems
                .AsNoTracking()
                .Where(sp => sp.ItemNumber.Equals(material.MaterialName))
                .Select(sp => sp)
                .FirstOrDefaultAsync();

            var materialNumber = material.MaterialName.Split(" ")[1];

            var futureInwardMovement = await _db.FutureInwardStockMovement
                .AsNoTracking()
                .Where(f => f.Article.Equals(materialNumber))
                .Select(f => f.Amount)
                .FirstOrDefaultAsync();

            var stockValue = await _db.StockValuesFromLastPeriod
                .AsNoTracking()
                .Select(s => Convert.ToDouble(s.Stockvalue.Replace(",", ".")))
                .SumAsync();

            var deliveryDuration = kPart.ProcureLeadTime;
            var deviation = kPart.Deviation;

            var discountQuantity = kPart.DiscountQuantity;

            var stock = Convert.ToInt32(stockQuantity) - partsFromQueue;

            var maxDeliveryDuration = deliveryDuration + deviation;
            var daysUntilNextDelivery = Math.Ceiling(maxDeliveryDuration * 5);

            var stockLasts = DaysStockWillLast(stock, Convert.ToInt32(futureInwardMovement), accordingRequirements);

            var orderAmount = 0;
            var orderType = 0;

            var orderQuotient = stockLasts / (maxDeliveryDuration * 5);

            if (orderQuotient is (> 0 and < 1.0))
            {
                // Fast Order
                var daysTillFastDeliveryAvailable = Math.Ceiling(deliveryDuration * 2.5);
                if (stockLasts < daysTillFastDeliveryAvailable + 5)
                {
                    orderType = 4;
                    orderAmount = discountQuantity;
                }
            }
            else
            {
                // Normal Order
                if (orderQuotient is (>= 1.0 and < 2.0) or < 2.5)
                {
                    orderType = 5;
                    orderAmount = discountQuantity;
                }
            }
            if (orderQuotient is 0)
            {
                // JustInTime Order
                orderType = 3;
                orderAmount = discountQuantity;
            }

            return new OrderForK
            {
                PartName = kPart.ItemNumber,
                OrderQuantity = orderAmount.ToString(),
                OrderModus = orderType,
                AdditionalParts = partsFromQueue,
                Stock = stock,
                Requirements = accordingRequirements.AsArray(),
                OrderQuotient = orderQuotient,
            };
        }

        public int DaysStockWillLast(int stockAmount, int futureInward, Vector<Double> requirements)
        {

            var lastForDays = 0;
            foreach (var req in requirements)
            {
                if (stockAmount + futureInward - req >= 0)
                {
                    stockAmount -= (int)req;
                    lastForDays += 5;
                }
                else
                {
                    //var averageRequirement = req / 5;
                    //lastForDays += Convert.ToInt32(Math.Floor(stockAmount / averageRequirement));
                    break;
                }
            }
            return lastForDays;
        }

        public double[,] CheckForChangedProductionOrders(List<ProductionOrder> productionOrders, List<(string Bicycle, string Amount)> changedRequirementsTupel)
        {
            double[,] productionMatrix = new double[3, 4];

            // If required parts in dispositions are changed
            // They will be corrected in the production matrix
            for (var i = 0; i < productionOrders.Count(); i++)
            {
                for (var j = 0; j < productionOrders[i].Orders.Count(); j++)
                {
                    if (j is 0)
                    {
                        var req = changedRequirementsTupel.Where(c => c.Bicycle.Contains("P" + (i + 1).ToString())).Select(c => c).FirstOrDefault();
                        if (Convert.ToDouble(req.Amount) != productionOrders[i].Orders[j]
                            && Convert.ToDouble(req.Amount) != 0)
                        {
                            productionMatrix[i, j] = Convert.ToInt32(req.Amount);
                        }
                        else
                        {
                            productionMatrix[i, j] = productionOrders[i].Orders[j];
                        }
                    }
                    else
                    {
                        productionMatrix[i, j] = productionOrders[i].Orders[j];
                    }
                }
            }
            return productionMatrix;
        }

        public async Task<(BillOfMaterial, BillOfMaterial, BillOfMaterial)> ExtractBicycles(List<BillOfMaterial> bicycleParts)
        {
            var p1 = new BillOfMaterial();
            var p2 = new BillOfMaterial();
            var p3 = new BillOfMaterial();

            foreach (var bicycle in bicycleParts)
            {
                foreach (var material in bicycle.RequiredMaterials)
                {
                    material.MaterialNeeded = await GetNestedMaterials(material);
                }
            }

            var counter = 0;

            foreach (var bicycle in new List<BillOfMaterial> { p1, p2, p3 })
            {
                bicycle.ProductName = bicycleParts[counter].ProductName;
                bicycle.RequiredMaterials = bicycleParts[counter].RequiredMaterials;
                counter++;
            }

            return (p1, p2, p3);
        }

        public async Task<Matrix<double>> CreateNeededMaterialMatrix(BillOfMaterial p1, BillOfMaterial p2, BillOfMaterial p3)
        {
            var extractedBicyclePOne = new List<Material>();
            var extractedBicyclePTwo = new List<Material>();
            var extractedBicyclePThree = new List<Material>();

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                extractedBicyclePOne.AddRange(filteredPart);
            }
            foreach (var material in p2.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                extractedBicyclePTwo.AddRange(filteredPart);
            }
            foreach (var material in p3.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                extractedBicyclePThree.AddRange(filteredPart);
            }

            var summedPartsPOne = SumFilteredMaterials(extractedBicyclePOne);
            var summedPartsPTwo = SumFilteredMaterials(extractedBicyclePTwo);
            var summedPartsPThree = SumFilteredMaterials(extractedBicyclePThree);

            var completedPartsForP1 = InsertNotNeededMaterials(new List<Material>(summedPartsPOne), new List<Material>(summedPartsPTwo), new List<Material>(summedPartsPThree));
            var completedPartsForP2 = InsertNotNeededMaterials(new List<Material>(summedPartsPTwo), new List<Material>(summedPartsPOne), new List<Material>(summedPartsPThree));
            var completedPartsForP3 = InsertNotNeededMaterials(new List<Material>(summedPartsPThree), new List<Material>(summedPartsPOne), new List<Material>(summedPartsPTwo));

            // Conversion of Summed Parts To Vectors
            var neededMaterialVecP1 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP1.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP2 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP2.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP3 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP3.Select(p => Convert.ToDouble(p.QuantityNeeded)));

            var neededMaterialMatrix = Matrix<Double>.Build.DenseOfColumnVectors(neededMaterialVecP1, neededMaterialVecP2, neededMaterialVecP3);

            return neededMaterialMatrix;
        }

        public async Task<List<Material>> CreateCompleteMaterialListForBicycle(BillOfMaterial p1, BillOfMaterial p2, BillOfMaterial p3)
        {
            var extractedBicyclePOne = new List<Material>();
            var extractedBicyclePTwo = new List<Material>();
            var extractedBicyclePThree = new List<Material>();

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                extractedBicyclePOne.AddRange(filteredPart);
            }
            foreach (var material in p2.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                extractedBicyclePTwo.AddRange(filteredPart);
            }
            foreach (var material in p3.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                extractedBicyclePThree.AddRange(filteredPart);
            }

            var summedPartsPOne = SumFilteredMaterials(extractedBicyclePOne);
            var summedPartsPTwo = SumFilteredMaterials(extractedBicyclePTwo);
            var summedPartsPThree = SumFilteredMaterials(extractedBicyclePThree);

            var completedParts = InsertNotNeededMaterials(new List<Material>(summedPartsPOne), new List<Material>(summedPartsPTwo), new List<Material>(summedPartsPThree));

            return completedParts;
        }

        public List<ProductionOrder> AddSelldirectItems(List<ProductionOrder> productionOrders, List<SellDirectItem> selldirectItems)
        {
            foreach (var po in productionOrders)
            {
                var s = selldirectItems.Where(sd => po.Bicycle.Equals($"P{sd.Article}")).Select(sd => sd).FirstOrDefault();
                po.Orders[0] += Convert.ToInt32(s.Quantity);
            }
            return productionOrders;
        }
    }
}
