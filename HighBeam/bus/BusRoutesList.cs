using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam.bus
{
    public class BusRoutesList
    {
        public static List<BusRouteModel> Routes = new List<BusRouteModel>()
        {
            new BusRouteModel()
            {
                Name = "Airport Express",
                Number = "431",
                From = "LS International Airport",
                To = "Paleto Bay",
                Stops = new List<BusStopModel>()
                {
                    new BusStopModel()
                    {
                        Name = "Airport terminal 2 Arivals",
                        Position = new Vector3(-1022.88f, -2494.95f, 14.5f),
                        MaxPassengersExiting = 0,
                        MaxPassengersWaiting = 55,
                    },
                     new BusStopModel()
                    {
                        Name = "Los Santos Central Metro Station",
                        Position = new Vector3(258.18f, -1188.71f, 29.4f),
                        MaxPassengersExiting = 3,
                        MaxPassengersWaiting = 20,
                    },
                     new BusStopModel()
                    {
                        Name = "Los Santos Bus Station",
                        Position = new Vector3(456.26f, -644.68f, 28.3f),
                        MaxPassengersExiting = 4,
                        MaxPassengersWaiting = 7,
                    },
                     new BusStopModel()
                    {
                        Name = "Sandy Shores",
                        Position = new Vector3(1925.87f, 3692.1f, 32.5f),
                        MaxPassengersExiting = 16,
                        MaxPassengersWaiting = 7,
                    },
                     new BusStopModel()
                    {
                        Name = "Paleto Bay",
                        Position = new Vector3(-332.55f, 6190.5f, 31.2f),
                        MaxPassengersExiting = 16,
                        MaxPassengersWaiting = 7,
                    },
                }
            },
             new BusRouteModel()
            {
                Name = "Airport Express",
                Number = "431",
                To = "LS International Airport",
                From = "Paleto Bay",
                Stops = new List<BusStopModel>()
                {
                      new BusStopModel()
                    {
                        Name = "Paleto Bay",
                        Position = new Vector3(-156.72f, 6205.46f, 32.1f),
                        MaxPassengersExiting = 0,
                        MaxPassengersWaiting = 13,
                    },
                           new BusStopModel()
                    {
                        Name = "Sandy Shores",
                        Position = new Vector3(1925.87f, 3692.1f, 32.5f),
                        MaxPassengersExiting = 16,
                        MaxPassengersWaiting = 7,
                    },
                    new BusStopModel()
                    {
                        Name = "Los Santos Bus Station",
                        Position = new Vector3(456.26f, -644.68f, 28.3f),
                        MaxPassengersExiting = 4,
                        MaxPassengersWaiting = 7,
                    },
                     new BusStopModel()
                    {
                        Name = "Los Santos Central Metro Station",
                        Position = new Vector3(258.18f, -1188.71f, 29.4f),
                        MaxPassengersExiting = 3,
                        MaxPassengersWaiting = 20,
                    },
                      new BusStopModel()
                    {
                        Name = "Airport terminal 2 Departures",
                        Position = new Vector3(-1022.88f, -2494.95f, 30.5f),
                        MaxPassengersExiting = 55,
                        MaxPassengersWaiting = 0,
                    },
                }
            }
        };
    }
}
