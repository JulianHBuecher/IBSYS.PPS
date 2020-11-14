using IBSYS.PPS.Models;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IBSYS.PPS.Logic.Tests
{
    public class Queue
    {
        [Fact]
        public async Task Materialplanning_Extract_Queue()
        {
            var p1 = new BillOfMaterial()
            {
                ProductName = "P1",
                RequiredMaterials = new List<Material>()
                {
                    new Material() { MaterialName = "K 21", QuantityNeeded = 1, DirectAccess = new string[] { "1" } },
                    new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                    new Material() { MaterialName = "K 27", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "31", "51", "56" } },
                    new Material()
                    {
                        MaterialName = "E 26",
                        QuantityNeeded = 1,
                        MaterialNeeded = new List<Material>()
                        {
                            new Material() { MaterialName = "K 44", QuantityNeeded = 2, DirectAccess = new string[] { "17" } },
                            new Material() { MaterialName = "K 47", QuantityNeeded = 1, DirectAccess = new string[] { "26" } },
                            new Material() { MaterialName = "K 48", QuantityNeeded = 2, DirectAccess = new string[] { } }
                        }
                    },
                    new Material()
                    {
                        MaterialName = "E 51",
                        QuantityNeeded = 1,
                        MaterialNeeded = new List<Material>()
                        {
                            new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                            new Material() { MaterialName = "K 27", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "31", "51", "56" } },
                            new Material()
                            {
                                MaterialName = "E 16",
                                QuantityNeeded = 1,
                                MaterialNeeded = new List<Material>()
                                {
                                    new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                    new Material() { MaterialName = "K 28", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                    new Material() { MaterialName = "K 40", QuantityNeeded = 1, DirectAccess = new string[] { "16" } },
                                    new Material() { MaterialName = "K 41", QuantityNeeded = 1, DirectAccess = new string[] { "16" } },
                                    new Material() { MaterialName = "K 42", QuantityNeeded = 2, DirectAccess = new string[] { "16" } }
                                }
                            },
                            new Material()
                            {
                                MaterialName = "E 17",
                                QuantityNeeded = 1,
                                MaterialNeeded = new List<Material>()
                                {
                                    new Material() { MaterialName = "K 43", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                    new Material() { MaterialName = "K 44", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                    new Material() { MaterialName = "K 45", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                    new Material() { MaterialName = "K 46", QuantityNeeded = 1, DirectAccess = new string[] { "17" } }
                                }
                            },
                            new Material()
                            {
                                MaterialName = "E 50",
                                QuantityNeeded = 1,
                                MaterialNeeded = new List<Material>()
                                {
                                    new Material() { MaterialName = "K 24", QuantityNeeded = 2, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                    new Material() { MaterialName = "K 25", QuantityNeeded = 2, DirectAccess = new string[] { "29", "30", "49", "50", "54", "55" } },
                                    new Material()
                                    {
                                        MaterialName = "E 4",
                                        QuantityNeeded = 1,
                                        MaterialNeeded = new List<Material>()
                                        {
                                            new Material() { MaterialName = "K 35", QuantityNeeded = 2, DirectAccess = new string[] { "4", "5", "6", "7", "8", "9" } },
                                            new Material() { MaterialName = "K 36", QuantityNeeded = 1, DirectAccess = new string[] { "4", "5", "6" } },
                                            new Material() { MaterialName = "K 52", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                            new Material() { MaterialName = "K 53", QuantityNeeded = 36, DirectAccess = new string[] { } }
                                        }
                                    },
                                    new Material()
                                    {
                                        MaterialName = "E 10",
                                        QuantityNeeded = 1,
                                        MaterialNeeded = new List<Material>()
                                        {
                                            new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                            new Material() { MaterialName = "K 39", QuantityNeeded = 1, DirectAccess = new string[] { } }
                                        }
                                    },
                                    new Material()
                                    {
                                        MaterialName = "E 49",
                                        QuantityNeeded = 1,
                                        MaterialNeeded = new List<Material>()
                                        {
                                            new Material() { MaterialName = "K 24", QuantityNeeded = 2, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                            new Material() { MaterialName = "K 25", QuantityNeeded = 2, DirectAccess = new string[] { "29", "30", "49", "50", "54", "55" } },
                                            new Material()
                                            {
                                                MaterialName = "E 7",
                                                QuantityNeeded = 1,
                                                MaterialNeeded = new List<Material>()
                                                {
                                                    new Material() { MaterialName = "K 35", QuantityNeeded = 2, DirectAccess = new string[] { "4", "5", "6", "7", "8", "9" } },
                                                    new Material() { MaterialName = "K 37", QuantityNeeded = 1, DirectAccess = new string[] { "7", "8", "9" } },
                                                    new Material() { MaterialName = "K 38", QuantityNeeded = 1, DirectAccess = new string[] { "7", "8", "9" } },
                                                    new Material() { MaterialName = "K 52", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                                    new Material() { MaterialName = "K 53", QuantityNeeded = 36, DirectAccess = new string[] { } }
                                                }
                                            },
                                            new Material()
                                            {
                                                MaterialName = "E 13",
                                                QuantityNeeded = 1,
                                                MaterialNeeded = new List<Material>()
                                                {
                                                    new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                    new Material() { MaterialName = "K 39", QuantityNeeded = 1, DirectAccess = new string[] { } }
                                                }
                                            },
                                            new Material()
                                            {
                                                MaterialName = "E 18",
                                                QuantityNeeded = 1,
                                                MaterialNeeded = new List<Material>()
                                                {
                                                    new Material() { MaterialName = "K 28", QuantityNeeded = 3, DirectAccess = new string[] { } },
                                                    new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                    new Material() { MaterialName = "K 59", QuantityNeeded = 2, DirectAccess = new string[] { } }
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
                        new Material() { MaterialName = "K 22", QuantityNeeded = 1, DirectAccess = new string[] { "2" } },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "31", "51", "56" } },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2, DirectAccess = new string[] { "17" } },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1, DirectAccess = new string[] { "26" } },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2, DirectAccess = new string[] { } }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 56",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "31", "51", "56" }  },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1, DirectAccess = new string[] { "16" } },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1, DirectAccess = new string[] { "16" } },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2, DirectAccess = new string[] { "16" } }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1, DirectAccess = new string[] { "17" } }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 55",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2, DirectAccess = new string[] { "29", "30", "49", "50", "54", "55" } },
                                        new Material()
                                        {
                                            MaterialName = "E 5",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2, DirectAccess = new string[] { "4", "5", "6", "7", "8", "9" } },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1, DirectAccess = new string[] { "4", "5", "6" } },
                                                new Material() { MaterialName = "K 57", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                                new Material() { MaterialName = "K 58", QuantityNeeded = 36, DirectAccess = new string[] { } }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 11",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1, DirectAccess = new string[] { } }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 54",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2, DirectAccess = new string[] { "29", "30", "49", "50", "54", "55" } },
                                                new Material()
                                                {
                                                    MaterialName = "E 8",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2, DirectAccess = new string[] { "4", "5", "6", "7", "8", "9" } },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1, DirectAccess = new string[] { "7", "8", "9" } },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1, DirectAccess = new string[] { "7", "8", "9" } },
                                                        new Material() { MaterialName = "K 57", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                                        new Material() { MaterialName = "K 58", QuantityNeeded = 36, DirectAccess = new string[] { } }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 14",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1, DirectAccess = new string[] { } }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 19",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 4, DirectAccess = new string[] { } },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2, DirectAccess = new string[] { } }
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
                        new Material() { MaterialName = "K 23", QuantityNeeded = 1, DirectAccess = new string[] { "3" } },
                        new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                        new Material() { MaterialName = "K 27", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "31", "51", "56" }  },
                        new Material()
                        {
                            MaterialName = "E 26",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 44", QuantityNeeded = 2, DirectAccess = new string[] { "17" } },
                                new Material() { MaterialName = "K 47", QuantityNeeded = 1, DirectAccess = new string[] { "26" } },
                                new Material() { MaterialName = "K 48", QuantityNeeded = 2, DirectAccess = new string[] { } }
                            }
                        },
                        new Material()
                        {
                            MaterialName = "E 31",
                            QuantityNeeded = 1,
                            MaterialNeeded = new List<Material>()
                            {
                                new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                new Material() { MaterialName = "K 27", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "31", "51", "56" }  },
                                new Material()
                                {
                                    MaterialName = "E 16",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 1, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                        new Material() { MaterialName = "K 28", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                        new Material() { MaterialName = "K 40", QuantityNeeded = 1, DirectAccess = new string[] { "16" } },
                                        new Material() { MaterialName = "K 41", QuantityNeeded = 1, DirectAccess = new string[] { "16" } },
                                        new Material() { MaterialName = "K 42", QuantityNeeded = 2, DirectAccess = new string[] { "16" } }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 17",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 43", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                        new Material() { MaterialName = "K 44", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                        new Material() { MaterialName = "K 45", QuantityNeeded = 1, DirectAccess = new string[] { "17" } },
                                        new Material() { MaterialName = "K 46", QuantityNeeded = 1, DirectAccess = new string[] { "17" } }
                                    }
                                },
                                new Material()
                                {
                                    MaterialName = "E 30",
                                    QuantityNeeded = 1,
                                    MaterialNeeded = new List<Material>()
                                    {
                                        new Material() { MaterialName = "K 24", QuantityNeeded = 2, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                        new Material() { MaterialName = "K 25", QuantityNeeded = 2, DirectAccess = new string[] { "29", "30", "49", "50", "54", "55" } },
                                        new Material()
                                        {
                                            MaterialName = "E 6",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 33", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                                new Material() { MaterialName = "K 34", QuantityNeeded = 36, DirectAccess = new string[] { } },
                                                new Material() { MaterialName = "K 35", QuantityNeeded = 2, DirectAccess = new string[] { "4", "5", "6", "7", "8", "9" } },
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1, DirectAccess = new string[] { "4", "5", "6" } }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 12",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                new Material() { MaterialName = "K 39", QuantityNeeded = 1, DirectAccess = new string[] { } }
                                            }
                                        },
                                        new Material()
                                        {
                                            MaterialName = "E 29",
                                            QuantityNeeded = 1,
                                            MaterialNeeded = new List<Material>()
                                            {
                                                new Material() { MaterialName = "K 24", QuantityNeeded = 2, DirectAccess = new string[] { "1", "2", "3", "16", "29", "30", "31", "49", "50", "51", "54", "55", "56" } },
                                                new Material() { MaterialName = "K 25", QuantityNeeded = 2, DirectAccess = new string[] { "29", "30", "49", "50", "54", "55" } },
                                                new Material()
                                                {
                                                    MaterialName = "E 9",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 33", QuantityNeeded = 1, DirectAccess = new string[] { } },
                                                        new Material() { MaterialName = "K 34", QuantityNeeded = 36, DirectAccess = new string[] { } },
                                                        new Material() { MaterialName = "K 35", QuantityNeeded = 2, DirectAccess = new string[] { "4", "5", "6", "7", "8", "9" } },
                                                        new Material() { MaterialName = "K 37", QuantityNeeded = 1, DirectAccess = new string[] { "7", "8", "9" } },
                                                        new Material() { MaterialName = "K 38", QuantityNeeded = 1, DirectAccess = new string[] { "7", "8", "9" } }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 15",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                        new Material() { MaterialName = "K 39", QuantityNeeded = 1, DirectAccess = new string[] { } }
                                                    }
                                                },
                                                new Material()
                                                {
                                                    MaterialName = "E 20",
                                                    QuantityNeeded = 1,
                                                    MaterialNeeded = new List<Material>()
                                                    {
                                                        new Material() { MaterialName = "K 28", QuantityNeeded = 5, DirectAccess = new string[] { } },
                                                        new Material() { MaterialName = "K 32", QuantityNeeded = 1, DirectAccess = new string[] { "10", "11", "12", "13", "14", "15", "18", "19", "20" } },
                                                        new Material() { MaterialName = "K 59", QuantityNeeded = 2, DirectAccess = new string[] { } }
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

            var pList = new List<BillOfMaterial> { p1, p2, p3 };

            var p1PartsQueue = new List<Material>();

            p1.RequiredMaterials.ForEach(async p =>
            {
                var parts = await FilterKMaterialsByNameAndEPart("K", "E 16", p);
                p1PartsQueue.AddRange(parts);
            });

            var summedParts = SumFilteredMaterials(new List<Material>(p1PartsQueue));

            var ePartsVec = Vector<Double>.Build.DenseOfEnumerable(summedParts.Select(e => Convert.ToDouble(e.QuantityNeeded)));

            var additionalPartsNeeded = ePartsVec.Multiply(60.0);

            var n = additionalPartsNeeded.ToString();

            Assert.Equal(4, summedParts.Count());
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
            if (!ePart.StartsWith("P")) {
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
            //return partsForBicycle;
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

    }
}
