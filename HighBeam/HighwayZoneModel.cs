using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public class HighwayZoneModel
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int CompassDirection { get; set; }
        public float HeadingDirection { get; set; }
        public ZoneBoundaryModel ZoneBoundary { get; set; }
        public ZoneBoundaryModel TicketZoneBoundary { get; set; }
        public List<Vector3> TrafficSpawnPoints { get; set; }
        public bool isStartTollBooth { get; set; }
        public float LeftLaneDirection { get; set; }
        public bool IsStartingFromRightLane { get; set; }
        public bool IsOneLaneRoad { get; set; }
        public FakeTrafficModel TrafficPaths { get; set; }
        public List<StaticPropsModel> StaticProps { get; set; }
        public bool EnableTrafficOnStreets { get; set; }
    }

    public class ZoneBoundaryModel
    { 
        public int StartLeftX { get; set; }
        public int StartLeftY { get; set; }
        public int StartRightX { get; set; }
        public int StartRightY { get; set; }
        public int FinishLeftX { get; set; }
        public int FinishLeftY { get; set; }
        public int FinishRightX { get; set; }
        public int FinishRightY { get; set; }
        public int ZCoord { get; set; }
    }
}
