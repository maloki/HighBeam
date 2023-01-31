using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam.bus
{
    public class BusStopModel
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public int MaxPassengersWaiting { get; set; }
        public int MaxPassengersExiting { get; set; }
    }
}
