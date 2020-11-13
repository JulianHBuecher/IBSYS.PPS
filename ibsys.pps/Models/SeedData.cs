using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBSYS.PPS.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IbsysDatabaseContext context)
        {
            // Is the data initially seeded?
            if (context.BillOfMaterials.Any() || context.LaborAndMachineCosts.Any() || context.SelfProductionItems.Any()
                || context.PurchasedItems.Any())
            {
                return; // DB has been seeded
            }

            await context.AddAsync(new BillOfMaterial()
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
                                                new Material() { MaterialName = "K 36", QuantityNeeded = 1, DirectAccess = new string[] { "4", "5", "6" }},
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
                });

            await context.AddAsync(new BillOfMaterial()
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
            });

            await context.AddAsync(new BillOfMaterial()
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
            });

            var laborAndMachineCosts = new List<LaborAndMachineCosts>()
                {
                    new LaborAndMachineCosts() { Workplace=1,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.05,IdleTimeMachineCosts=0.01},
                    new LaborAndMachineCosts() { Workplace=2,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.05,IdleTimeMachineCosts=0.01},
                    new LaborAndMachineCosts() { Workplace=3,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.05,IdleTimeMachineCosts=0.01},
                    new LaborAndMachineCosts() { Workplace=4,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.05,IdleTimeMachineCosts=0.01},
                    new LaborAndMachineCosts() { Workplace=6,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.30,IdleTimeMachineCosts=0.10},
                    new LaborAndMachineCosts() { Workplace=7,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.30,IdleTimeMachineCosts=0.10},
                    new LaborAndMachineCosts() { Workplace=8,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.30,IdleTimeMachineCosts=0.10},
                    new LaborAndMachineCosts() { Workplace=9,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.80,IdleTimeMachineCosts=0.25},
                    new LaborAndMachineCosts() { Workplace=10,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.30,IdleTimeMachineCosts=0.10},
                    new LaborAndMachineCosts() { Workplace=11,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.30,IdleTimeMachineCosts=0.10},
                    new LaborAndMachineCosts() { Workplace=12,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.30,IdleTimeMachineCosts=0.10},
                    new LaborAndMachineCosts() { Workplace=13,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.50,IdleTimeMachineCosts=0.15},
                    new LaborAndMachineCosts() { Workplace=14,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.05,IdleTimeMachineCosts=0.01},
                    new LaborAndMachineCosts() { Workplace=15,LaborCostsFirstShift=0.45,LaborCostsSecondShift=0.55,LaborCostsThirdShift=0.70,LaborCostsOvertime=0.90,ProductiveMachineCosts=0.05,IdleTimeMachineCosts=0.01},
                };

            // Rüstzeit und Durchlaufzeiten ergänzen
            var initialStockEParts = new List<SelfProductionItems>()
                {
                    new SelfProductionItems(){ ItemNumber="P 1", Description="Children's bicycle", ItemValue=156.13, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="P 2", Description="Ladies bicycle", ItemValue=163.33, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="P 3", Description="Men's bicycle", ItemValue=165.08, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 4", Description="Rear wheel group", Usage="K", ItemValue=40.85, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 5", Description="Rear wheel group", Usage="D", ItemValue=39.85, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 6", Description="Rear wheel group", Usage="H", ItemValue=40.85, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 7", Description="Front wheel group", Usage="K", ItemValue=35.85, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 8", Description="Front wheel group", Usage="D", ItemValue=35.85, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 9", Description="Front wheel group", Usage="H", ItemValue=35.85, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 10", Description="Mudguard rear", Usage="K", ItemValue=12.40, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 11", Description="Mudguard rear", Usage="D", ItemValue=14.65, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 12", Description="Mudguard rear", Usage="H", ItemValue=14.65, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 13", Description="Mudguard front", Usage="K", ItemValue=12.40, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 14", Description="Mudguard front", Usage="D", ItemValue=14.65, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 15", Description="Mudguard front", Usage="H", ItemValue=14.65, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 16", Description="Handle complete", Usage="KDH", ItemValue=7.02, QuantityInStock=300 },
                    new SelfProductionItems(){ ItemNumber="E 17", Description="Saddle complete", Usage="KDH", ItemValue=7.16, QuantityInStock=300 },
                    new SelfProductionItems(){ ItemNumber="E 18", Description="Frame", Usage="K", ItemValue=13.15, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 19", Description="Frame", Usage="D", ItemValue=14.35, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 20", Description="Frame", Usage="H", ItemValue=15.55, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 26", Description="Pedal complete", Usage="KDH", ItemValue=10.50, QuantityInStock=300 },
                    new SelfProductionItems(){ ItemNumber="E 29", Description="Front wheel complete", Usage="H", ItemValue=69.29, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 30", Description="Frame and wheels", Usage="H", ItemValue=127.53, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 31", Description="Bicycle w/o pedals", Usage="H", ItemValue=144.42, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 49", Description="Front wheel complete", Usage="K", ItemValue=64.64, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 50", Description="Frame and wheels", Usage="K", ItemValue=120.63, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 51", Description="Bicycle w/o pedals", Usage="K", ItemValue=137.47, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 54", Description="Front wheel complete", Usage="D", ItemValue=68.09, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 55", Description="Frame and wheels", Usage="D", ItemValue=125.33, QuantityInStock=100 },
                    new SelfProductionItems(){ ItemNumber="E 56", Description="Bicycle w/o pedals", Usage="D", ItemValue=142.67, QuantityInStock=100 }
                };

            var initialStockKParts = new List<PurchasedItems>()
                {
                    new PurchasedItems() { ItemNumber="K 21", Description="Chain", Usage="K", ItemValue=5.00, QuantityInStock=300, DiscountQuantity=300, OrderCosts=50.00, ProcureLeadTime=1.8, Deviation=0.4 },
                    new PurchasedItems() { ItemNumber="K 22", Description="Chain", Usage="D", ItemValue=6.50, QuantityInStock=300, DiscountQuantity=300, OrderCosts=50.00, ProcureLeadTime=1.7, Deviation=0.4 },
                    new PurchasedItems() { ItemNumber="K 23", Description="Chain", Usage="H", ItemValue=6.50, QuantityInStock=300, DiscountQuantity=300, OrderCosts=50.00, ProcureLeadTime=1.2, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 24", Description="Nut 3/8\"", Usage="KDH", ItemValue=0.06, QuantityInStock=6100, DiscountQuantity=6100, OrderCosts=100.00, ProcureLeadTime=3.2, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 25", Description="Washer 3/8\"", Usage="KDH", ItemValue=0.06, QuantityInStock=3600, DiscountQuantity=3600, OrderCosts=50.00, ProcureLeadTime=0.9, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 27", Description="Screw 3/8\"", Usage="KDH", ItemValue=0.10, QuantityInStock=1800, DiscountQuantity=1800, OrderCosts=75.00, ProcureLeadTime=0.9, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 28", Description="Tube 3/4\"", Usage="KDH", ItemValue=1.20, QuantityInStock=4500, DiscountQuantity=4500, OrderCosts=50.00, ProcureLeadTime=1.7, Deviation=0.4 },
                    new PurchasedItems() { ItemNumber="K 32", Description="Paint", Usage="KDH", ItemValue=0.75, QuantityInStock=2700, DiscountQuantity=2700, OrderCosts=50.00, ProcureLeadTime=2.1, Deviation=0.5 },
                    new PurchasedItems() { ItemNumber="K 33", Description="Rim complete", Usage="H", ItemValue=22.00, QuantityInStock=900, DiscountQuantity=900, OrderCosts=75.00, ProcureLeadTime=1.9, Deviation=0.5 },
                    new PurchasedItems() { ItemNumber="K 34", Description="Spoke", Usage="H", ItemValue=0.10, QuantityInStock=22000, DiscountQuantity=22000, OrderCosts=50.00, ProcureLeadTime=1.6, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 35", Description="Taper sleeve", Usage="KDH", ItemValue=1.00, QuantityInStock=3600, DiscountQuantity=3600, OrderCosts=75.00, ProcureLeadTime=2.2, Deviation=0.4 },
                    new PurchasedItems() { ItemNumber="K 36", Description="Free wheel", Usage="KDH", ItemValue=8.00, QuantityInStock=900, DiscountQuantity=900, OrderCosts=100.00, ProcureLeadTime=1.2, Deviation=0.1 },
                    new PurchasedItems() { ItemNumber="K 37", Description="Fork", Usage="KDH", ItemValue=1.50, QuantityInStock=900, DiscountQuantity=900, OrderCosts=50.00, ProcureLeadTime=1.5, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 38", Description="Axle", Usage="KDH", ItemValue=1.50, QuantityInStock=300, DiscountQuantity=300, OrderCosts=50.00, ProcureLeadTime=1.7, Deviation=0.4 },
                    new PurchasedItems() { ItemNumber="K 39", Description="Sheet", Usage="KDH", ItemValue=1.50, QuantityInStock=900, DiscountQuantity=1800, OrderCosts=75.00, ProcureLeadTime=1.5, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 40", Description="Handle bar", Usage="KDH", ItemValue=2.50, QuantityInStock=900, DiscountQuantity=900, OrderCosts=50.00, ProcureLeadTime=1.7, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 41", Description="Nut 3/4\"", Usage="KDH", ItemValue=0.06, QuantityInStock=900, DiscountQuantity=900, OrderCosts=50.00, ProcureLeadTime=0.9, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 42", Description="Handle grip", Usage="KDH", ItemValue=0.10, QuantityInStock=1800, DiscountQuantity=1800, OrderCosts=50.00, ProcureLeadTime=1.2, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 43", Description="Saddle", Usage="KDH", ItemValue=5.00, QuantityInStock=1900, DiscountQuantity=2700, OrderCosts=75.00, ProcureLeadTime=2.0, Deviation=0.5 },
                    new PurchasedItems() { ItemNumber="K 44", Description="Bar 1/2\"", Usage="KDH", ItemValue=0.50, QuantityInStock=2700, DiscountQuantity=900, OrderCosts=50.00, ProcureLeadTime=1.0, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 45", Description="Nut 1/4\"", Usage="KDH", ItemValue=0.06, QuantityInStock=900, DiscountQuantity=900, OrderCosts=50.00, ProcureLeadTime=1.7, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 46", Description="Screw 1/4\"", Usage="KDH", ItemValue=0.10, QuantityInStock=900, DiscountQuantity=900, OrderCosts=50.00, ProcureLeadTime=0.9, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 47", Description="Sprocket", Usage="KDH", ItemValue=3.50, QuantityInStock=900, DiscountQuantity=900, OrderCosts=50.00, ProcureLeadTime=1.1, Deviation=0.1 },
                    new PurchasedItems() { ItemNumber="K 48", Description="Pedal", Usage="KDH", ItemValue=1.50, QuantityInStock=1800, DiscountQuantity=1800, OrderCosts=75.00, ProcureLeadTime=1.0, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 52", Description="Rim complete", Usage="K", ItemValue=22.00, QuantityInStock=600, DiscountQuantity=600, OrderCosts=50.00, ProcureLeadTime=1.6, Deviation=0.4 },
                    new PurchasedItems() { ItemNumber="K 53", Description="Spoke", Usage="K", ItemValue=0.10, QuantityInStock=22000, DiscountQuantity=22000, OrderCosts=50.00, ProcureLeadTime=1.6, Deviation=0.2 },
                    new PurchasedItems() { ItemNumber="K 57", Description="Rim complete", Usage="D", ItemValue=22.00, QuantityInStock=600, DiscountQuantity=600, OrderCosts=50.00, ProcureLeadTime=1.7, Deviation=0.3 },
                    new PurchasedItems() { ItemNumber="K 58", Description="Spoke", Usage="D", ItemValue=0.10, QuantityInStock=22000, DiscountQuantity=22000, OrderCosts=50.00, ProcureLeadTime=1.6, Deviation=0.5 },
                    new PurchasedItems() { ItemNumber="K 59", Description="Welding wires", Usage="KDH", ItemValue=0.15, QuantityInStock=1800, DiscountQuantity=1800, OrderCosts=50.00, ProcureLeadTime=0.7, Deviation=0.2 },
                };

            //await context.AddRangeAsync(new List<BillOfMaterial> { p1, p2, p3 });
            await context.AddRangeAsync(laborAndMachineCosts);
            await context.AddRangeAsync(initialStockEParts);
            await context.AddRangeAsync(initialStockKParts);

            await context.SaveChangesAsync();
        }
    }
}