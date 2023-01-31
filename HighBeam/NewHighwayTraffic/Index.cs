using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.NewHighwayTraffic.Radar;
using static HighBeam.NewHighwayTraffic.CarSpawner;
using static HighBeam.NewHighwayTraffic.Zone;
using static HighBeam.NewHighwayTraffic.MoveCar;
using static HighBeam.NewHighwayTraffic.Helpers;
using static HighBeam.NewHighwayTraffic.FakeTraffic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace HighBeam.NewHighwayTraffic
{
    public static class Index
    {
        public static bool isOnHighway = false;
        public static Vehicle veh;
        public static bool isPlayerOnLeftLane;
        public static bool isNight;
        public static bool isRaining;
        private static Stopwatch RadarStopWatch = new Stopwatch();
        public static bool isForcingCarInFrontToChangeLane = false;
        private static Stopwatch ForcingCarInFrontToChangeLaneReaction = new Stopwatch();
        public static int forceReaction = 0;
        public static void RunNewHighwayTraffic()
        {
            try
            {

                if (isOnHighway)
                {
                    UpdateCoords();
                    Index.veh = Game.Player.LastVehicle;
                    GetCurrentZone();
                    if (CurrentZone.Name != null)
                    {
                        if (!CurrentZone.EnableTrafficOnStreets)
                        {
                            Function.Call((Hash)0xB3B3359379FE77D3, 0f);
                            Function.Call((Hash)0x245A6883D966D537, 0f);
                        }
                        PlayerLaneCheck();
                        GetTraffic();
                        ManageTrafficForZone();
                        if (RadarStopWatch.ElapsedMilliseconds > 400)
                        {
                            for (var i = 0; i < carList.Count; ++i)
                            {
                                GeneralCar currentCar = carList[i];
                              /*  if (ForcingCarInFrontToChangeLaneReaction.ElapsedMilliseconds > forceReaction && currentCar.Vehicle.Position.DistanceTo(Index.veh.Position) < 30 && currentCar.Stats.isLeftLane)
                                {
                                    ForceCarInFrontToChangeLane(currentCar);
                                    ForcingCarInFrontToChangeLaneReaction = new Stopwatch();
                                    isForcingCarInFrontToChangeLane = false;
                                }*/
                                NewRadar(currentCar);
                            //    CheckMirrors(currentCar); 
                                SetCarLight(currentCar);
                                CheckIfCarIsToDelete(currentCar);
                                RunMoveCar(currentCar);
                                // AdjustCarSpeed(currentCar);

                            }
                            RadarStopWatch = new Stopwatch();
                        }
                        for (var i = 0; i < carList.Count; ++i)
                        {
                            GeneralCar currentCar = carList[i];
                            Overtake(currentCar);
                            
                            AdjustCarSpeed(currentCar);
                        }
                        if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
                        {
                          //  SpawnCarTest();
                           /* isForcingCarInFrontToChangeLane = true;
                            ForcingCarInFrontToChangeLaneReaction = new Stopwatch();
                            ForcingCarInFrontToChangeLaneReaction.Start();
                            forceReaction = GenerateRandomNumberBetween(1300, 4000);*/
                        }
                        if (!RadarStopWatch.IsRunning)
                        {
                            RadarStopWatch = new Stopwatch();
                            RadarStopWatch.Start();
                        }
                        RunFakeTraffic();
                    }
                }
            }
            catch (Exception e)
            {
                UI.Notify(e.Message);
            }
        }
    }
}
