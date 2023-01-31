using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public class TollBoothZoneTrafficModel
    {
        public List<Vector3> TrafficSpawnPoints { get; set; }
        public float Heading { get; set; }  
        public ZoneBoundaryModel ZoneBoundary { get; set; }
    }
}
