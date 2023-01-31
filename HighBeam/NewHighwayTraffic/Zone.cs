using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.NewHighwayTraffic.Helpers;
using static HighBeam.NewHighwayTraffic.CarSpawner;
using static HighBeam.AutobahnZones;
using GTA.Math;
using GTA;
using GTA.Native;

namespace HighBeam.NewHighwayTraffic
{
    public static class Zone
    {
        public static HighwayZoneModel CurrentZone = new HighwayZoneModel();

        public static void GetCurrentZone() 
        {
            for (var zn = 0; zn < Zones.Count; zn++)
            {
                var zone = Zones[zn];
                var t1 = PointInTriangle(x, y, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY);
                var t2 = PointInTriangle(x, y, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY);
                var t3 = PointInTriangle(x, y, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY);
                var t4 = PointInTriangle(x, y, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY);
                if ((t1 || t2 || t3 || t4)) 
                { 
                    CurrentZone = zone;
                    Vector3 zoneStart = new Vector3() { X = CurrentZone.ZoneBoundary.StartLeftX, Y = CurrentZone.ZoneBoundary.StartLeftY, Z = z };
                    Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishLeftX, Y = CurrentZone.ZoneBoundary.FinishLeftY, Z = z };
                    if (Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneStart) <= 20f
                        || Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneEnd) <= 20f)
                    {
                        CurrentZone.IsStartingFromRightLane = !PlayerLaneCheck();
                    }
                    if (CurrentZone.EnableTrafficOnStreets)
                    {
                       // LiveTraffic(true); 
                    }
                    else
                    {
                        Function.Call((Hash)0xB3B3359379FE77D3, 0f);
                        Function.Call((Hash)0x245A6883D966D537, 0f);
                        Function.Call((Hash)0xEAE6DCC7EEE3DB1D, 0f);
                        Function.Call((Hash)0x95E3D6257B166CF2, 0f);
                    }
                    break;
                }
            } 
            if (CurrentZone.Name != null)
            {
                Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishLeftX, Y = CurrentZone.ZoneBoundary.FinishLeftY, Z = z };
                Vector3 zoneStart = new Vector3() { X = CurrentZone.ZoneBoundary.StartLeftX, Y = CurrentZone.ZoneBoundary.StartLeftY, Z = z };
                var dist = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneEnd);
                if (dist < 40f)
                {
                    CurrentZone = new HighwayZoneModel();
                }
            }
            else
            {
                if (false)
                {
                    for (var cr = 0; cr < carListOnRightLane.Count; cr++)
                    {
                        var car = carListOnRightLane[cr];
                        car.Vehicle.MarkAsNoLongerNeeded();
                        Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                        car.Vehicle.Position = del;
                    }
                    for (var cr = 0; cr < carList.Count; cr++)
                    {
                        var car = carList[cr];
                        car.Vehicle.MarkAsNoLongerNeeded();
                        Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                        car.Vehicle.Position = del;
                    }
                    carListOnRightLane = new List<GeneralCar>();
                    carList = new List<GeneralCar>();
                }
            }

        }
    }
}
