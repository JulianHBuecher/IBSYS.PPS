using IBSYS.PPS.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Xunit;

namespace IBSYS.PPS.Logic.Tests
{
    public class Disposition
    {
        [Fact]
        public async Task Filter_For_Nested_E_Parts()
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

            var p1Parts = new List<string>();
            p1Parts.Add("P1");

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("E", material);
                p1Parts.AddRange(filteredPart);
            }

            Assert.Equal(12, p1Parts.Count);
        }

        [Fact]
        public async Task Filter_For_Nested_K_Parts()
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

            var p1Parts = new List<string>();
            p1Parts.Add("P1");

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("K", material);
                p1Parts.AddRange(filteredPart);
            }

            Assert.Equal(38, p1Parts.Count);
        }

        public async Task<List<string>> FilterNestedMaterialsByName(string parts, Material ml)
        {
            var partsForBicycle = new List<string>();

            if (ml.MaterialName.StartsWith(parts))
            {
                partsForBicycle.Add(ml.MaterialName);
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
    }
}
