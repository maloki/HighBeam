using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.NewHighwayTraffic.CarSpawner;
using static HighBeam.NewHighwayTraffic.Zone;
using static HighBeam.NewHighwayTraffic.Helpers;
using static HighBeam.NewHighwayTraffic.Index;
using GTA;
using GTA.Math;
using GTA.Native;

namespace HighBeam.NewHighwayTraffic
{
    public static class FakeTraffic
    {
        public static Stopwatch fakeLightStopWatch = new Stopwatch();
        public static Stopwatch fakeLightSpawnStopWatch = new Stopwatch();
        public  static List<GeneralCar> fakeLightList = new List<GeneralCar>();
        private static int maxNumOfSpawnedFakeCars = 30;
        public static bool spawnMoreFakeLightsOnZoneEnter = false;

        public static void RunFakeTraffic()
        {
            if (!fakeLightStopWatch.IsRunning)
            {
                fakeLightStopWatch.Start();
            }
            if (fakeLightStopWatch.IsRunning && fakeLightStopWatch.ElapsedMilliseconds >= 500)
            {
                MoveFakeLights();
                fakeLightStopWatch = new Stopwatch();
            } 
        }

        private static void MoveFakeLights()
        {
            if (CurrentZone.Name != null)
            {
                var toDelFakeList = false;
                if (CurrentZone.Name == null && fakeLightList.Count > 0)
                {
                    toDelFakeList = true;
                }
                if (!fakeLightSpawnStopWatch.IsRunning)
                {
                    fakeLightSpawnStopWatch.Start();
                }
                if ((fakeLightSpawnStopWatch.ElapsedMilliseconds > 5500 || spawnMoreFakeLightsOnZoneEnter) && !toDelFakeList && fakeLightList.Count < 4)
                {
                    spawnMoreFakeLightsOnZoneEnter = false;
                    var lightsToSpawn = GenerateRandomNumberBetween(minFake, maxFake);
                    var distAhead = 400;
                    for (var fls = 0; fls < lightsToSpawn; fls++)
                    {
                        distAhead += SpawnDummyCar(distAhead);
                    }
                    fakeLightSpawnStopWatch = new Stopwatch();
                }
                var endZone = new Vector3(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY, CurrentZone.ZoneBoundary.ZCoord);
                for (var fk = 0; fk < fakeLightList.Count; fk++)
                {
                    var light = fakeLightList[fk];
                    var isLightBehindPlayer = veh.Position.DistanceTo(endZone) < (light.Vehicle.Position.DistanceTo(endZone) - 50);
                    if ((isLightBehindPlayer || veh.Position.DistanceTo(endZone) < 200 || toDelFakeList) && !light.Stats.Deleted)
                    {
                        Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                        light.Vehicle.Speed = 0f;
                        light.Vehicle.MarkAsNoLongerNeeded();
                        light.Vehicle.Position = del;
                        light.Stats.Deleted = true;
                    }
                    else
                    {
                        light.Vehicle.Speed = light.Stats.Speed;
                        light.Vehicle.Heading = light.Stats.Heading;
                        var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
                        var m = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
                        var s = Function.Call<int>((Hash)0x494E97C2EF27C470);
                        if (isRaining || (h >= 19 || h < 7))
                        {
                            light.Vehicle.LightsOn = true;
                        }
                        if (!isRaining && (h >= 7 && h < 19))
                        {
                            light.Vehicle.LightsOn = false;
                        }
                    }
                }
                if (veh.Position.DistanceTo(endZone) < 200 || toDelFakeList)
                {
                    RemoveAllFakeLightsAndRef();
                }
                for (var po = 0; po < fakeLightList.Count; po++)
                {
                    if (fakeLightList.ElementAt(po).Stats.Deleted)
                    {
                        fakeLightList.RemoveAt(po);
                    };
                }
            }
        }

        private static int SpawnDummyCar(int metersAhead = 450)
        {
            int addMeters = 0;
            if (CurrentZone.Name != null && fakeLightList.Count < maxNumOfSpawnedFakeCars)
            {
                if (veh.Position.DistanceTo(new Vector3(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY, CurrentZone.ZoneBoundary.ZCoord)) > 400)
                {
                    var carStats = GetRandomCar();
                    carStats.Heading = (CurrentZone.HeadingDirection + 180) % 360;
                    float laneOffset = 0;
                    if (CurrentZone.TrafficPaths != null)
                    {
                        if (carStats.Class == "truck" || carStats.Class == "family")
                        {
                            addMeters = GenerateRandomNumberBetween(43, 54);
                            carStats.Speed = (float)(100 / 3.6);
                            laneOffset = CurrentZone.TrafficPaths.RightLaneOffset;
                        }
                        else
                        {
                            addMeters = GenerateRandomNumberBetween(28, 48);
                            carStats.Speed = (float)(155 / 3.6);
                            laneOffset = CurrentZone.TrafficPaths.LeftLaneOffset;
                        }
                    }

                    else
                    {
                        if (carStats.Class == "truck" || carStats.Class == "family")
                        {
                            addMeters = GenerateRandomNumberBetween(43, 54);
                            carStats.Speed = (float)(105 / 3.6);
                            laneOffset = 22;
                        }
                        else
                        {
                            carStats.Speed = (float)(155 / 3.6);
                            laneOffset = 16;
                            addMeters = GenerateRandomNumberBetween(28, 48);
                        }
                    }
                    metersAhead += addMeters;
                    var leftBorder = GetClosestPoint(
                                      new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                                      new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                                      new Vector2(veh.Position.X, veh.Position.Y)
                                  );
                    var rightBorder = GetClosestPoint(
                        new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                        new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                        new Vector2(veh.Position.X, veh.Position.Y)
                    );
                    Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = veh.Position.Z };
                    var referencePoint = World.CreateVehicle(new Model(VehicleHash.Panto), centerPoint, CurrentZone.HeadingDirection);
                    referencePoint.Position = referencePoint.GetOffsetInWorldCoords(new Vector3(0, -28, 0));
                    referencePoint.PlaceOnGround();
                    var car = World.CreateVehicle(carStats.Model, referencePoint.GetOffsetInWorldCoords(new Vector3(-(laneOffset), (metersAhead), 0)), carStats.Heading);
                    referencePoint.Position = new Vector3(0, 0, 0);
                    referencePoint.Delete();
                    car.Speed = carStats.Speed;
                    car.PlaceOnGround();
                    car.LightsMultiplier = 10f;
                    car.EngineRunning = true;
                    fakeLightList.Add(new GeneralCar() { Vehicle = car, Stats = carStats });

                }
            }
            return addMeters;
        }

        private static void SpawnFakeLight(int distAhead = 450)
        {
            SpawnDummyCar(distAhead);
        }

        private static void RemoveAllFakeLightsAndRef()
        {
            fakeLightList = new List<GeneralCar>();
        }
    }
}
