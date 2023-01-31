using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public class TollBoothsZones
    {
        public static List<HighwayZoneModel> TollBooths = new List<HighwayZoneModel>()
        {
            //new one
            new HighwayZoneModel()
            {
            Name = "Portland Toll",
             Index = 0, 
            HeadingDirection = 165.1f,
            isStartTollBooth = false,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 3063,
                    Y = -1612,
                    Z = 67
                },
                new Vector3()
                {
                    X = 3057,
                    Y = -1610,
                    Z = 67
                },
                new Vector3()
                {
                    X = 3052,
                    Y = -1609,
                    Z = 67
                },
                new Vector3()
                {
                    X = 3046,
                    Y = -1607,
                    Z = 67
                },
                new Vector3()
                {
                    X = 3041,
                    Y = -1606,
                    Z = 67
                },
                new Vector3()
                {
                    X = 3035, 
                    Y = -1604,
                    Z = 67
                },
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 3060,
            StartLeftY = -1632,
             StartRightX = 3028,
             StartRightY = -1624,
            FinishLeftX = 3059,
            FinishLeftY = -1636,
             FinishRightX = 3027,
             FinishRightY = -1628,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
               StartLeftX = 3108,
                StartLeftY = -1449,
                 StartRightX = 3074,
                 StartRightY = -1439,
                FinishLeftX = 3057,
                FinishLeftY = -1645,
                 FinishRightX = 3022,
                 FinishRightY = -1638,
            }
            },
             new HighwayZoneModel()
            {
            Name = "Portland Toll",
             Index = 0,
            HeadingDirection = 345.2f,
            isStartTollBooth = true,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 3061,
                    Y = -1644,
                    Z = 67
                },
                new Vector3()
                {
                    X = 3066,
                    Y = -1646,
                    Z = 67
                },
                new Vector3()
                { 
                    X = 3072,
                    Y = -1648,
                    Z = 67
                },
                new Vector3()
                {
                    X = 3075,
                    Y = -1655,
                    Z = 67
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 3062,
            StartLeftY = -1633,
             StartRightX = 3082,
             StartRightY = -1637,
            FinishLeftX = 3062,
            FinishLeftY = -1629,
             FinishRightX = 3083,
             FinishRightY = -1633,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
               StartLeftX = 3009,
            StartLeftY = -1844,
             StartRightX = 3029,
             StartRightY = -1850,
            FinishLeftX = 3067,
            FinishLeftY = -1613,
             FinishRightX = 3089,
             FinishRightY = -1619,
            }
            },
            new HighwayZoneModel()
            {
            Name = "Los Santos Toll",
             Index = 0,
            HeadingDirection = 38f,
            isStartTollBooth = true,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = -1356,
                    Y = -1653,
                    Z = 27
                },
                /*new Vector3()
                {
                    X = -1365,
                    Y = -1632,
                    Z = 27 
                },*/
                new Vector3()
                { 
                    X = -1348,
                    Y = -1646,
                    Z = 27
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = -1375,
            StartLeftY = -1632,
             StartRightX = -1363,
             StartRightY = -1623,
            FinishLeftX = -1377,
            FinishLeftY = -1630,
             FinishRightX = -1365,
             FinishRightY = -1620,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
               StartLeftX = -1276,
            StartLeftY = -1764,
             StartRightX = -1271,
             StartRightY = -1761,
            FinishLeftX = -1377,
            FinishLeftY = -1629,
             FinishRightX = -1365,
             FinishRightY = -1619,
            }
            },
            new HighwayZoneModel()
            {
            Name = "Los Santos Toll",
             Index = 0,
            HeadingDirection = 217.5f,
            isStartTollBooth = false,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = -1393,
                    Y = -1641,
                    Z = 27
                },
                new Vector3()
                {
                    X = -1389,
                    Y = -1638,
                    Z = 27
                },
                new Vector3()
                {
                    X = -1385,
                    Y = -1635,
                    Z = 27
                },
                new Vector3()
                {
                    X = -1381,
                    Y = -1631,
                    Z = 27
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = -1376,
            StartLeftY = -1634,
             StartRightX = -1391,
             StartRightY = -1647,
            FinishLeftX = -1373,
            FinishLeftY = -1637,
             FinishRightX = -1389,
             FinishRightY = -1649,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
               StartLeftX = -1546,
                StartLeftY = -1431,
                 StartRightX = -1555,
                 StartRightY = -1440,
                FinishLeftX = -1373,
                FinishLeftY = -1637,
                 FinishRightX = -1388,
                 FinishRightY = -1650,
            }
            },
            new HighwayZoneModel()
            {
            Name = "Nase Toll",
             Index = 0,
            HeadingDirection = 179.6f,
            isStartTollBooth = false,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 2589,
                    Y = -337,
                    Z = 93
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 2590,
            StartLeftY = -347,
             StartRightX = 2586,
             StartRightY = -347,
            FinishLeftX = 2590,
            FinishLeftY = -353,
             FinishRightX = 2586,
             FinishRightY = -353,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
           StartLeftX = 2589,
            StartLeftY = -217,
             StartRightX = 2583,
             StartRightY = -218,
            FinishLeftX = 2592,
            FinishLeftY = -354,
             FinishRightX = 2586,
             FinishRightY = -354,
            }
            },
            new HighwayZoneModel()
            {
            Name = "Nase Toll",
             Index = 0,
            HeadingDirection = 359.8f,
            isStartTollBooth = true,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 2594,
                    Y = -354,
                    Z = 93 
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 2593,
            StartLeftY = -344,
             StartRightX = 2597,
             StartRightY = -344,
            FinishLeftX = 2593,
            FinishLeftY = -340,
             FinishRightX = 2597,
             FinishRightY = -340,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 2592,
            StartLeftY = -425,
             StartRightX = 2598,
             StartRightY = -425,
            FinishLeftX = 2592,
            FinishLeftY = -338,
             FinishRightX = 2597,
             FinishRightY = -338,
            }
            },
             new HighwayZoneModel()
            {
            Name = "North Yankton Toll",
             Index = 0,
            HeadingDirection = 174.0f,
            isStartTollBooth = false,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 8015,
                    Y = -3350,
                    Z = 117
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 8016,
            StartLeftY = -3361,
             StartRightX = 8012,
             StartRightY = -3360,
            FinishLeftX = 8015,
            FinishLeftY = -3365,
             FinishRightX = 8012,
             FinishRightY = -3365,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 8023,
            StartLeftY = -3291,
             StartRightX = 8015,
             StartRightY = -3290,
            FinishLeftX = 8016,
            FinishLeftY = -3366,
             FinishRightX = 8011,
             FinishRightY = -3367,
            }
            },
             new HighwayZoneModel()
            {
            Name = "North Yankton Toll",
             Index = 0,
            HeadingDirection = 354.4f,
            isStartTollBooth = true,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 8019,
                    Y = -3367,
                    Z = 117
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 8018,
            StartLeftY = -3356,
             StartRightX = 8022,
             StartRightY = -3356,
            FinishLeftX = 8019,
            FinishLeftY = -3352,
             FinishRightX = 8023,
             FinishRightY = -3353,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 8009,
            StartLeftY = -3456,
             StartRightX = 8012,
             StartRightY = -3458,
            FinishLeftX = 8018,
            FinishLeftY = -3350,
             FinishRightX = 8023,
             FinishRightY = -3350,
            }
            },
             new HighwayZoneModel()
            {
            Name = "Lakewood Toll",
             Index = 0,
            HeadingDirection = 251.8f,
            isStartTollBooth = true,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 2818,
                    Y = -651,
                    Z = 96
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 2830,
            StartLeftY = -653,
             StartRightX = 2829,
             StartRightY = -657,
            FinishLeftX = 2834,
            FinishLeftY = -655,
             FinishRightX = 2833,
             FinishRightY = -658,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 2736,
            StartLeftY = -622,
             StartRightX = 2734,
             StartRightY = -628,
            FinishLeftX = 2839,
            FinishLeftY = -655,
             FinishRightX = 2837,
             FinishRightY = -661,
            }
            },
             new HighwayZoneModel()
            {
            Name = "Lakewood Toll",
             Index = 0,
            HeadingDirection = 72.1f,
            isStartTollBooth = false,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 2839,
                    Y = -652,
                    Z = 96
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 2827,
            StartLeftY = -650,
             StartRightX = 2828,
             StartRightY = -647,
            FinishLeftX = 2822,
            FinishLeftY = -649,
             FinishRightX = 2824,
             FinishRightY = -645,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 2978,
            StartLeftY = -700,
             StartRightX = 2981,
             StartRightY = -694,
            FinishLeftX = 2820,
            FinishLeftY = -649,
             FinishRightX = 2822,
             FinishRightY = -642,
            }
            },
              new HighwayZoneModel()
            {
            Name = "NY South Toll",
             Index = 0,
            HeadingDirection = 72.1f,
            isStartTollBooth = true,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
           StartLeftX = 1953,
            StartLeftY = -4294,
             StartRightX = 1954,
             StartRightY = -4288,
            FinishLeftX = 1949,
            FinishLeftY = -4293,
             FinishRightX = 1950,
             FinishRightY = -4287,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
           StartLeftX = 1953,
            StartLeftY = -4294,
             StartRightX = 1954,
             StartRightY = -4288,
            FinishLeftX = 1949,
            FinishLeftY = -4293,
             FinishRightX = 1950,
             FinishRightY = -4287,
            }
            },
               new HighwayZoneModel()
            {
            Name = "NY South Toll",
             Index = 0,
            HeadingDirection = 72.1f,
            isStartTollBooth = false,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
             StartLeftX = 1955,
            StartLeftY = -4295,
             StartRightX = 1954,
             StartRightY = -4301,
            FinishLeftX = 1965,
            FinishLeftY = -4296,
             FinishRightX = 1965,
             FinishRightY = -4303,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
          StartLeftX = 1955,
            StartLeftY = -4295,
             StartRightX = 1954,
             StartRightY = -4301,
            FinishLeftX = 1965,
            FinishLeftY = -4296,
             FinishRightX = 1965,
             FinishRightY = -4303,
            }
            }, 
             //in construction
            /*  new HighwayZoneModel()
            {
            Name = "Bonneville Toll",
             Index = 0,
            HeadingDirection = 46f,
            isStartTollBooth = false,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 4291,
                    Y = 3912,
                    Z = 36
                },
                new Vector3()
                {
                    X = 4296,
                    Y = 3916,
                    Z = 36
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 4282,
            StartLeftY = 3917,
             StartRightX = 4290,
             StartRightY = 3926,
            FinishLeftX = 4278,
            FinishLeftY = 3921,
             FinishRightX = 4287,
             FinishRightY = 3929,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 4442,
            StartLeftY = 3759,
             StartRightX = 4451,
             StartRightY = 3768,
            FinishLeftX = 4273,
            FinishLeftY = 3920,
             FinishRightX = 4283,
             FinishRightY = 3934,
            }
            },
               new HighwayZoneModel()
            {
            Name = "Bonneville Toll",
             Index = 0,
            HeadingDirection = 226f,
            isStartTollBooth = true,
            TrafficSpawnPoints = new List<Vector3>()
            {
                new Vector3()
                {
                    X = 4262,
                    Y = 3910,
                    Z = 36
                },
                new Vector3()
                {
                    X = 4257,
                    Y = 3906,
                    Z = 36
                }
            },
            TicketZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 4272,
            StartLeftY = 3904,
             StartRightX = 4264,
             StartRightY = 3896,
            FinishLeftX = 4275,
            FinishLeftY = 3900,
             FinishRightX = 4267,
             FinishRightY = 3892,
            },
            ZoneBoundary = new ZoneBoundaryModel()
            {
            StartLeftX = 4117,
            StartLeftY = 4059,
             StartRightX = 4108,
             StartRightY = 4049,
            FinishLeftX = 4282,
            FinishLeftY = 3900,
             FinishRightX = 4272,
             FinishRightY = 3887,
            }
            },*/
        };
    }
}
