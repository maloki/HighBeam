using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public class StaticPropsModel
    {
        public String Type { get; set; }
        public Vector3 Postion { get; set; }
        public List<VehicleHash> PossibleVehicleList { get; set; }
        public List<PedHash> PossiblePedList { get; set; } 
        public float Heading { get; set; } 
        public bool IsElectric { get; set; }
        public string ForcedAnimation { get; set; }
    }
}
