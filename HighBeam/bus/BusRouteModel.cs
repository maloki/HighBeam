using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam.bus
{
    public class BusRouteModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public List<BusStopModel> Stops { get; set; }
    }
}
