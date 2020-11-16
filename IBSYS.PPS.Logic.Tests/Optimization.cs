using IBSYS.PPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IBSYS.PPS.Logic.Tests
{
    public class Optimization
    {
        public record SelfProductionItems()
        {
            public string ItemNumber { get; init; }
            public string Description { get; init; }
            public string? Usage { get; init; }
            public double ItemValue { get; init; }
            public int QuantityInStock { get; init; }
            public string ProcessingTime { get; init; }
        }

        public record OptimizedPart()
        {
            public int Id { get; set; }
            public string ItemNumber { get; init; }
            public string Description { get; init; }
            public string ProcessingTime { get; init; }
        }

        [Fact]
        public async Task Optimize_Production_Order()
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

            // Self-Production Items for Optimization
            var initialStockEParts = new List<SelfProductionItems>()
                {
                    new SelfProductionItems() { ItemNumber="P 1", Description="Children's bicycle", ItemValue=156.13, QuantityInStock=100, ProcessingTime="6-30" },
                    new SelfProductionItems(){ ItemNumber="P 2", Description="Ladies bicycle", ItemValue=163.33, QuantityInStock=100, ProcessingTime="7-20" },
                    new SelfProductionItems(){ ItemNumber="P 3", Description="Men's bicycle", ItemValue=165.08, QuantityInStock=100, ProcessingTime="7-30" },
                    new SelfProductionItems(){ ItemNumber="E 4", Description="Rear wheel group", Usage="K", ItemValue=40.85, QuantityInStock=100, ProcessingTime="7-30" },
                    new SelfProductionItems(){ ItemNumber="E 5", Description="Rear wheel group", Usage="D", ItemValue=39.85, QuantityInStock=100, ProcessingTime="7-30" },
                    new SelfProductionItems(){ ItemNumber="E 6", Description="Rear wheel group", Usage="H", ItemValue=40.85, QuantityInStock=100, ProcessingTime="7-40" },
                    new SelfProductionItems(){ ItemNumber="E 7", Description="Front wheel group", Usage="K", ItemValue=35.85, QuantityInStock=100, ProcessingTime="7-40" },
                    new SelfProductionItems(){ ItemNumber="E 8", Description="Front wheel group", Usage="D", ItemValue=35.85, QuantityInStock=100, ProcessingTime="7-40" },
                    new SelfProductionItems(){ ItemNumber="E 9", Description="Front wheel group", Usage="H", ItemValue=35.85, QuantityInStock=100, ProcessingTime="7-40" },
                    new SelfProductionItems(){ ItemNumber="E 10", Description="Mudguard rear", Usage="K", ItemValue=12.40, QuantityInStock=100, ProcessingTime="11-50" },
                    new SelfProductionItems(){ ItemNumber="E 11", Description="Mudguard rear", Usage="D", ItemValue=14.65, QuantityInStock=100, ProcessingTime="12-50" },
                    new SelfProductionItems(){ ItemNumber="E 12", Description="Mudguard rear", Usage="H", ItemValue=14.65, QuantityInStock=100, ProcessingTime="12-50" },
                    new SelfProductionItems(){ ItemNumber="E 13", Description="Mudguard front", Usage="K", ItemValue=12.40, QuantityInStock=100, ProcessingTime="11-50" },
                    new SelfProductionItems(){ ItemNumber="E 14", Description="Mudguard front", Usage="D", ItemValue=14.65, QuantityInStock=100, ProcessingTime="12-50" },
                    new SelfProductionItems(){ ItemNumber="E 15", Description="Mudguard front", Usage="H", ItemValue=14.65, QuantityInStock=100, ProcessingTime="12-50" },
                    new SelfProductionItems(){ ItemNumber="E 16", Description="Handle complete", Usage="KDH", ItemValue=7.02, QuantityInStock=300, ProcessingTime="5-15" },
                    new SelfProductionItems(){ ItemNumber="E 17", Description="Saddle complete", Usage="KDH", ItemValue=7.16, QuantityInStock=300, ProcessingTime="3-15" },
                    new SelfProductionItems(){ ItemNumber="E 18", Description="Frame", Usage="K", ItemValue=13.15, QuantityInStock=100, ProcessingTime="10-70" },
                    new SelfProductionItems(){ ItemNumber="E 19", Description="Frame", Usage="D", ItemValue=14.35, QuantityInStock=100, ProcessingTime="10-80" },
                    new SelfProductionItems(){ ItemNumber="E 20", Description="Frame", Usage="H", ItemValue=15.55, QuantityInStock=100, ProcessingTime="10-70" },
                    new SelfProductionItems(){ ItemNumber="E 26", Description="Pedal complete", Usage="KDH", ItemValue=10.50, QuantityInStock=300, ProcessingTime="5-45" },
                    new SelfProductionItems(){ ItemNumber="E 29", Description="Front wheel complete", Usage="H", ItemValue=69.29, QuantityInStock=100, ProcessingTime="6-20" },
                    new SelfProductionItems(){ ItemNumber="E 30", Description="Frame and wheels", Usage="H", ItemValue=127.53, QuantityInStock=100, ProcessingTime="5-20" },
                    new SelfProductionItems(){ ItemNumber="E 31", Description="Bicycle w/o pedals", Usage="H", ItemValue=144.42, QuantityInStock=100, ProcessingTime="6-20" },
                    new SelfProductionItems(){ ItemNumber="E 49", Description="Front wheel complete", Usage="K", ItemValue=64.64, QuantityInStock=100, ProcessingTime="6-20" },
                    new SelfProductionItems(){ ItemNumber="E 50", Description="Frame and wheels", Usage="K", ItemValue=120.63, QuantityInStock=100, ProcessingTime="5-30" },
                    new SelfProductionItems(){ ItemNumber="E 51", Description="Bicycle w/o pedals", Usage="K", ItemValue=137.47, QuantityInStock=100, ProcessingTime="5-20" },
                    new SelfProductionItems(){ ItemNumber="E 54", Description="Front wheel complete", Usage="D", ItemValue=68.09, QuantityInStock=100, ProcessingTime="6-20" },
                    new SelfProductionItems(){ ItemNumber="E 55", Description="Frame and wheels", Usage="D", ItemValue=125.33, QuantityInStock=100, ProcessingTime="5-30" },
                    new SelfProductionItems(){ ItemNumber="E 56", Description="Bicycle w/o pedals", Usage="D", ItemValue=142.67, QuantityInStock=100, ProcessingTime="6-20" }
                };

            var p1Parts = new List<Material>();
            var p2Parts = new List<Material>();
            var p3Parts = new List<Material>();

            foreach (var material in p1.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("E", material);
                p1Parts.AddRange(filteredPart);
            }

            foreach (var material in p2.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("E", material);
                p2Parts.AddRange(filteredPart);
            }

            foreach (var material in p3.RequiredMaterials)
            {
                var filteredPart = await FilterNestedMaterialsByName("E", material);
                p3Parts.AddRange(filteredPart);
            }

            var optimizedPartsP1 = initialStockEParts.Select(e => e).Where(p => p1Parts.Select(pp => pp.MaterialName).Contains(p.ItemNumber) || p.ItemNumber == "P 1");
            var optimizedPartsP2 = initialStockEParts.Select(e => e).Where(p => p2Parts.Select(pp => pp.MaterialName).Contains(p.ItemNumber) || p.ItemNumber == "P 2");
            var optimizedPartsP3 = initialStockEParts.Select(e => e).Where(p => p3Parts.Select(pp => pp.MaterialName).Contains(p.ItemNumber) || p.ItemNumber == "P 3");

            optimizedPartsP1 = optimizedPartsP1.Select(p => p).OrderByDescending(p => p.ProcessingTime.Split("-")[1]).ToList();
            optimizedPartsP2 = optimizedPartsP2.Select(p => p).OrderByDescending(p => p.ProcessingTime.Split("-")[1]).ToList();
            optimizedPartsP3 = optimizedPartsP3.Select(p => p).OrderByDescending(p => p.ProcessingTime.Split("-")[1]).ToList();

            var optimizedParts = new List<Material>();

            foreach (var material in optimizedPartsP1)
            {
                optimizedParts.Add(p1Parts.Select(p => p).Where(p => p.MaterialName.Equals(material.ItemNumber)).FirstOrDefault());
            }

            //var optimizedParts = initialStockEParts.Select(p => p).OrderBy(p => p.ProcessingTime.Split("-")[1]).ToList();

            Assert.Equal(30, optimizedParts.Count());
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
