using IBSYS.PPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using System.Threading.Tasks;
using Xunit;

namespace IBSYS.PPS.Logic.Tests
{
    public class Materialplanning
    {
        [Fact]
        public async Task Materialplanning_For_K_Parts_of_P1()
        {
            var p1 = new BillOfMaterial()
            {
                ProductName = "P1",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 21", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 51",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 50",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 4",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 10",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 49",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 7",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 13",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 18",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 3 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p1Parts = new List<Material>();

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p1Parts.AddRange(filteredPart);
            }

            var summedParts = SumFilteredMaterials(p1Parts);

            var neededMaterialVec = Vector<Double>.Build
                .DenseOfEnumerable(summedParts.Select(p => Convert.ToDouble(p.QuantityNeeded)));

            var calculatedNewParts = neededMaterialVec.Multiply(150.0);

            Assert.Equal(23, summedParts.Count);
        }

        [Fact]
        public async Task Materialplanning_For_K_Parts_of_All_Bicycles()
        {
            var p1 = new BillOfMaterial()
            {
                ProductName = "P1",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 21", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 51",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 50",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 4",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 10",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 49",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 7",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 13",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 18",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 3 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p2 = new BillOfMaterial()
            {
                ProductName = "P2",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 22", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 56",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 55",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 5",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 57", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 58", QuantityNeeded = 36 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 11",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 54",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 8",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 57", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 58", QuantityNeeded = 36 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 14",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 19",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 4 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p3 = new BillOfMaterial()
            {
                ProductName = "P3",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 23", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 31",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 30",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 6",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 33", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 34", QuantityNeeded = 36 },
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 12",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 29",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 9",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 33", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 34", QuantityNeeded = 36 },
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 15",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 20",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 5 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p1Parts = new List<Material>();
            var p2Parts = new List<Material>();
            var p3Parts = new List<Material>();

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p1Parts.AddRange(filteredPart);
            }
            foreach (var material in p2.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p2Parts.AddRange(filteredPart);
            }
            foreach (var material in p3.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p3Parts.AddRange(filteredPart);
            }
            
            var summedPartsForP1 = SumFilteredMaterials(p1Parts);
            var summedPartsForP2 = SumFilteredMaterials(p2Parts);
            var summedPartsForP3 = SumFilteredMaterials(p3Parts);

            var completedPartsForP1 = InsertNotNeededMaterials(new List<Material>(summedPartsForP1), new List<Material>(summedPartsForP2), new List<Material>(summedPartsForP3));
            var completedPartsForP2 = InsertNotNeededMaterials(new List<Material>(summedPartsForP2), new List<Material>(summedPartsForP1), new List<Material>(summedPartsForP3));
            var completedPartsForP3 = InsertNotNeededMaterials(new List<Material>(summedPartsForP3), new List<Material>(summedPartsForP1), new List<Material>(summedPartsForP2));

            var neededMaterialVecP1 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP1.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP2 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP2.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP3 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP3.Select(p => Convert.ToDouble(p.QuantityNeeded)));

            var neededMaterialMatrix = Matrix<Double>.Build.DenseOfColumnVectors(neededMaterialVecP1, neededMaterialVecP2, neededMaterialVecP3);

            double[,] productionOrders = { 
                { 100.0, 140.0, 100.0, 150.0 }, 
                { 200.0, 180.0, 210.0, 150.0 }, 
                { 100.0, 160.0, 140.0, 200.0 } 
            };

            var productionOrderMatrix = Matrix<Double>.Build.DenseOfArray(productionOrders);

            var calculatedNewParts = neededMaterialMatrix.Multiply(productionOrderMatrix);

            var n = calculatedNewParts.ToMatrixString();

            Assert.Equal(29, calculatedNewParts.RowCount);
        }

        [Fact]
        public void Materialplanning_Extract_Queue()
        {
            var p1 = new BillOfMaterial()
            {
                ProductName = "P1",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 21", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 51",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 50",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 4",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 10",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 49",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 7",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 13",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 18",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 3 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p1PartsQueue = new List<Material>();

            p1.RequiredMaterials.ForEach(async p =>
            {
                var parts = await FilterKMaterialsByNameAndEPart("K", "E 49", p);
                p1PartsQueue.AddRange(parts);
            });

            var summedParts = SumFilteredMaterials(p1PartsQueue);

            var ePartsVec = Vector<Double>.Build.DenseOfEnumerable(summedParts.Select(e => Convert.ToDouble(e.QuantityNeeded)));

            var additionalPartsNeeded = ePartsVec.Multiply(60.0);

            var n = additionalPartsNeeded.ToString();

            Assert.Equal(11, summedParts.Count());
        }

        [Fact]
        public async Task Materialplanning_For_K_Parts_and_Orders()
        {
            var p1 = new BillOfMaterial()
            {
                ProductName = "P1",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 21", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 51",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 50",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 4",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 10",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 49",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 7",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 52", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 53", QuantityNeeded = 36 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 13",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 18",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 3 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p2 = new BillOfMaterial()
            {
                ProductName = "P2",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 22", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 56",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 55",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 5",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 57", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 58", QuantityNeeded = 36 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 11",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 54",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 8",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 57", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 58", QuantityNeeded = 36 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 14",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 19",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 4 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p3 = new BillOfMaterial()
            {
                ProductName = "P3",
                RequiredMaterials = new List<Material>()
                    {
                        new Material() { MaterialName = "K 23", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2 },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2 }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 31",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1 },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1 },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1 }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 30",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                        new Material()
                                        {
                                            MaterialName = "E 6",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 33", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 34", QuantityNeeded = 36 },
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 12",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 29",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2 },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2 },
                                                new Material()
                                                {
                                                    MaterialName = "E 9",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 33", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 34", QuantityNeeded = 36 },
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2 },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 15",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1 }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 20",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 5 },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1 },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2 }
                                                    }
                                                },
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            var p1Parts = new List<Material>();
            var p2Parts = new List<Material>();
            var p3Parts = new List<Material>();

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p1Parts.AddRange(filteredPart);
            }
            foreach (var material in p2.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p2Parts.AddRange(filteredPart);
            }
            foreach (var material in p3.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p3Parts.AddRange(filteredPart);
            }

            var summedPartsForP1 = SumFilteredMaterials(p1Parts);
            var summedPartsForP2 = SumFilteredMaterials(p2Parts);
            var summedPartsForP3 = SumFilteredMaterials(p3Parts);

            var completedPartsForP1 = InsertNotNeededMaterials(new List<Material>(summedPartsForP1), new List<Material>(summedPartsForP2), new List<Material>(summedPartsForP3));
            var completedPartsForP2 = InsertNotNeededMaterials(new List<Material>(summedPartsForP2), new List<Material>(summedPartsForP1), new List<Material>(summedPartsForP3));
            var completedPartsForP3 = InsertNotNeededMaterials(new List<Material>(summedPartsForP3), new List<Material>(summedPartsForP1), new List<Material>(summedPartsForP2));

            var neededMaterialVecP1 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP1.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP2 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP2.Select(p => Convert.ToDouble(p.QuantityNeeded)));
            var neededMaterialVecP3 = Vector<Double>.Build
                .DenseOfEnumerable(completedPartsForP3.Select(p => Convert.ToDouble(p.QuantityNeeded)));

            var neededMaterialMatrix = Matrix<Double>.Build.DenseOfColumnVectors(neededMaterialVecP1, neededMaterialVecP2, neededMaterialVecP3);

            double[,] productionOrders = {
                { 100.0, 140.0, 100.0, 150.0 },
                { 200.0, 180.0, 210.0, 150.0 },
                { 100.0, 160.0, 140.0, 200.0 }
            };

            var productionOrderMatrix = Matrix<Double>.Build.DenseOfArray(productionOrders);

            var calculatedNewParts = neededMaterialMatrix.Multiply(productionOrderMatrix);

            var n = calculatedNewParts.ToMatrixString();

            Assert.Equal(29, calculatedNewParts.RowCount);
        }

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
    }
}
