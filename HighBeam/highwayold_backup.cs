using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using GTA.Math;
using static HighBeam.TollBoothsZones;
using static HighBeam.AutobahnZones;
using static HighBeam.NewHighwayTraffic.CarList;
using static HighBeam.Main;
/*
namespace HighBeam
{
    public static class HighwayTrafficOld
    {
        public static bool isOnHighway = false;
        private static List<GeneralCar> carList = new List<GeneralCar>();
        private static List<GeneralCar> tollBoothcarList = new List<GeneralCar>();
        private static List<GeneralCar> trafficJamCarList = new List<GeneralCar>();
        private static int x = 0;
        private static int y = 0;
        private static int z = 0;
        private static HighwayZoneModel CurrentZone = new HighwayZoneModel();
        private static int maxNumOfSpawnedCars = 22;
        private static int maxNumOfSpawnedFakeCars = 30;
        private static int minFake = 30;
        private static int maxFake = 30;
        private static Stopwatch SpawnCarStopWatch = new Stopwatch();
        private static Stopwatch MoveCarStopWatch = new Stopwatch();
        private static int timeToNextCarSpawn = 0;
        private static Vehicle veh;
        private static int test = 0;
        private static IEnumerable<GeneralCar> otherCars;
        private static bool wasSpawnMoreCarsWhenEnteringZone = false;
        private static Random r = new Random();
        private static Random rnd2 = new Random();
        private static HighwayZoneModel CurrentTollBooth = new HighwayZoneModel();
        private static bool isAtTollBooth = false;
        private static bool spawnedTollBoothCars = false;
        private static int spawnedCarsCount = 0;
        private static int maxNumOfSpawnedTrucks = 0;
        private static float minLeftLaneSpeed = (float)(100 / 3.6);
        private static DateTime TimeSinceEnteredHighway = new DateTime();
        private static bool isPlayerOnLeftLane = false;
        private static Stopwatch playerLaneCheckStopWatch = new Stopwatch();
        private static Stopwatch trafficRndStopWatch = new Stopwatch();
        private static bool canApplyTrafficJam = false;
        private static bool isTrafficJam = false;
        private static Vector3 trafficJamZoneEnd = new Vector3();
        private static int trafficRnd = 0;
        private static bool isZoneSet = false;
        private static Stopwatch RndStopWatch = new Stopwatch();
        private static Stopwatch HighBeamStopWatch = new Stopwatch();
        private static bool isNight = false;
        private static bool isHighBeam = false;
        private static Stopwatch tollBoothTicketGivenStopWatch = new Stopwatch();
        private static Vehicle fakeLightRefPoint = null;
        private static List<GeneralCar> fakeLightList = new List<GeneralCar>();
        private static Stopwatch fakeLightSpawnStopWatch = new Stopwatch();
        private static bool spawnMoreFakeLightsOnZoneEnter = false;
        private static bool isRaining = false;
        private static Stopwatch fakeLightStopWatch = new Stopwatch();
        private static Stopwatch zoneExitStopWatch = new Stopwatch();
        private static bool showProximityMeter = false;
        private static bool showFasterCarBehind = false;
        private static Stopwatch isOnHighwayStopwatch = new Stopwatch();
        private static List<Vehicle> StaticVehicleList = new List<Vehicle>();
        private static List<Ped> StaticPedList = new List<Ped>();
        private static bool StaticPropsSpawned = false;
        private static Stopwatch RadarStopWatch = new Stopwatch();

        public static void RunHighwayTraffic()
        {

            UpdateCoords();
            // GetCurrentTollBooth(); 
            veh = Game.Player.LastVehicle;
            //UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString()); 
            //  UI.ShowSubtitle(Game.Player.Character.Heading.ToString()); 
            //  UI.ShowSubtitle(Game.Player.Character.Heading.ToString() + " z:" + z); 
            if (!tollBoothTicketGivenStopWatch.IsRunning)
                tollBoothTicketGivenStopWatch.Start();
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadRight) && isOnHighwayStopwatch.ElapsedMilliseconds >= 600)
            {
                // isOnHighway = true; 
                //  UI.ShowSubtitle("On highway");
                //  isOnHighwayStopwatch = new Stopwatch();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadRight) && isOnHighwayStopwatch.ElapsedMilliseconds < 600)
            {
                isOnHighwayStopwatch = new Stopwatch();
            }
            if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadRight))
            {
                isOnHighwayStopwatch = new Stopwatch();
                isOnHighwayStopwatch.Start();
            }
            if (isOnHighway || false)
            {
                if (!RndStopWatch.IsRunning)
                {
                    RndStopWatch.Start();
                }
                if (RndStopWatch.ElapsedMilliseconds > 500)
                {
                    RndStopWatch = new Stopwatch();
                }
                GetCurrentZone();
                if (CurrentZone.Name != null)
                {
                    if (CurrentZone.EnableTrafficOnStreets)
                    {
                        LiveTraffic(true);
                    }
                    else
                    {
                        Function.Call((Hash)0xB3B3359379FE77D3, 0f);
                        Function.Call((Hash)0x245A6883D966D537, 0f);
                        Function.Call((Hash)0xEAE6DCC7EEE3DB1D, 0f);
                        Function.Call((Hash)0x95E3D6257B166CF2, 0f);
                    }
                }

                if (zoneExitStopWatch.IsRunning && zoneExitStopWatch.ElapsedMilliseconds > 10000)
                {
                    zoneExitStopWatch = new Stopwatch();
                    CurrentZone = new HighwayZoneModel();
                }
                if (CurrentZone.Name != null)
                {
                    if (showProximityMeter)
                        RenderProximityMeter();
                    if (showFasterCarBehind)
                        RenderFasterCarBehindIcon();
                    // UI.ShowSubtitle("x " + CurrentZone.ZoneBoundary.StartLeftX + " y" + CurrentZone.ZoneBoundary.StartLeftY);
                    if (!SpawnCarStopWatch.IsRunning && CurrentZone.Name != null)
                    {
                        SpawnCarStopWatch.Start();
                        var traffic = GetTraffic();
                        timeToNextCarSpawn = GenerateRandomNumberBetween(traffic.Min, traffic.Max);
                    }
                    if (SpawnCarStopWatch.IsRunning && SpawnCarStopWatch.ElapsedMilliseconds >= timeToNextCarSpawn && !isTrafficJam && carList.Count <= maxNumOfSpawnedCars)
                    {
                        var carAheadCount = 0;
                        Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
                        for (var ca = 0; ca < carList.Count; ca++)
                        {
                            var car = carList[ca];
                            if (car.Vehicle.Position.DistanceTo(zoneEnd) < veh.Position.DistanceTo(zoneEnd) && car.Stats.Class != "nightsuperfast")
                            {
                                carAheadCount++;
                            }
                        }
                        SpawnCarStopWatch = new Stopwatch();
                        if (carAheadCount < 3 || veh.Speed < minLeftLaneSpeed)
                        {
                            var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
                            var m = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
                            var s = Function.Call<int>((Hash)0x494E97C2EF27C470);
                            SpawnCar(isNightSuperFast: true);
                            SpawnMultipleCars();
                        }
                    }
                    if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
                    {
                        //  SpawnTrafficBlockers();  
                        //  SpawnCarTest();
                        // CreateFakeLightsRefPoint();
                        //    SpawnFakeLight();  
                        //  RemoveStaticPropsFromZone();
                    }
                    if (Game.IsControlJustReleased(0, GTA.Control.VehicleHandbrake))
                    {
                        // SpawnTrafficBlockers(); 
                        //SpawnCarTest();  
                        // RemoveAllFakeLightsAndRef();
                        // SpawnStaticPropsForZone();
                    }
                    // move all cars every 500 ms 
                    if (!MoveCarStopWatch.IsRunning)
                    {
                        MoveCarStopWatch.Start();
                    }
                    if (!RadarStopWatch.IsRunning)
                    {
                        RadarStopWatch.Start();
                    }
                    if (MoveCarStopWatch.IsRunning && MoveCarStopWatch.ElapsedMilliseconds >= 250)
                    {
                        MoveCar();
                        GetTraffic();
                    }
                    if (RadarStopWatch.IsRunning && RadarStopWatch.ElapsedMilliseconds >= 500)
                    {
                        Radar();
                        MoveCarStopWatch = new Stopwatch();
                        RadarStopWatch = new Stopwatch();
                    }
                    if (!HighBeamStopWatch.IsRunning)
                    {
                        HighBeamStopWatch.Start();
                    }
                    if (HighBeamStopWatch.IsRunning && HighBeamStopWatch.ElapsedMilliseconds >= 3000)
                    {
                        HighBeamStopWatch = new Stopwatch();
                        isHighBeam = false;
                    }
                    if (HighBeamStopWatch.IsRunning && Game.IsControlPressed(0, GTA.Control.VehicleSelectNextWeapon))
                    {
                        isHighBeam = true;
                    }
                    if (!playerLaneCheckStopWatch.IsRunning)
                    {
                        playerLaneCheckStopWatch.Start();
                    }
                    if (playerLaneCheckStopWatch.IsRunning && playerLaneCheckStopWatch.ElapsedMilliseconds >= 500)
                    {
                        PlayerLaneCheck();
                        playerLaneCheckStopWatch = new Stopwatch();
                    }
                    if (!trafficRndStopWatch.IsRunning)
                    {
                        trafficRndStopWatch.Start();
                    }
                    if (trafficRndStopWatch.IsRunning && trafficRndStopWatch.ElapsedMilliseconds >= 60000)
                    {
                        trafficRnd = GenerateRandomNumberBetween(0, 10);
                        trafficRndStopWatch = new Stopwatch();
                    }
                }
                // UI.ShowSubtitle(Game.Player.Character.Heading.ToString());
                // UI.ShowSubtitle("count cars " + spawnedCarsCount);
                // overtake function must be called on tick, since it controlls cars steering
                Overtake();
                AdjustCarSpeed();
                SpawnMoreCarsOnZoneEnter();
                if (!fakeLightStopWatch.IsRunning)
                {
                    fakeLightStopWatch.Start();
                }
                if (fakeLightStopWatch.IsRunning && fakeLightStopWatch.ElapsedMilliseconds >= 500)
                {
                    MoveFakeLights();
                    fakeLightStopWatch = new Stopwatch();
                }
                // UI.ShowSubtitle(carList.Count.ToString());
            }
            if (CurrentTollBooth.Name != null)
            {
                //MoveCarTollBooth();
            }
        }

        private static void RenderProximityMeter()
        {
            var cont = new UIContainer(new Point(5, UI.HEIGHT - 180), new Size(3, 15), Color.FromArgb(255, 251, 95, 21));
            cont.Enabled = true;
            cont.Draw();
        }

        private static void RenderFasterCarBehindIcon()
        {
            var cont = new UIContainer(new Point(5, UI.HEIGHT - 180), new Size(3, 15), Color.FromArgb(255, 0, 0, 247));
            var cont2 = new UIContainer(new Point(5, UI.HEIGHT - 180), new Size(11, 2), Color.FromArgb(255, 0, 0, 247));
            var cont3 = new UIContainer(new Point(5, (UI.HEIGHT - 180) + 7), new Size(11, 2), Color.FromArgb(255, 0, 0, 247));
            var cont4 = new UIContainer(new Point(5, (UI.HEIGHT - 180) + 15), new Size(11, 2), Color.FromArgb(255, 0, 0, 247));
            cont.Enabled = true;
            cont.Draw();
            cont2.Draw();
            cont3.Draw();
            cont4.Draw();
        }

        private static bool PlayerLaneCheck()
        {
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
            Vector3 leftLane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = veh.Position.Z };
            var dist = veh.Position.DistanceTo(leftLane);
            if (dist < 3.5f)
            {
                isPlayerOnLeftLane = true;
                return true;
            }
            else
            {
                isPlayerOnLeftLane = false;
                return false;
            }
        }

        private static void UpdateCoords()
        {
            x = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).X, 0).ToString());
            y = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Y, 0).ToString());
            z = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Z, 0).ToString());
            var hour = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
            isNight = (hour >= 21 || hour < 6);
            isRaining = Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Rain")
                      || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Thunder")
                      || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Clearing");
        }

        private static void SpawnCar(bool isSpawnMoreWHenEntering = false, int metersBefore = 0, bool isNightSuperFast = false)
        {

            if (CurrentZone.Name != null && carList.Count < maxNumOfSpawnedCars)
            {
                int metersBeforePlayer;
                if (isSpawnMoreWHenEntering || metersBefore > 0)
                    metersBeforePlayer = metersBefore;
                else
                    metersBeforePlayer = GenerateRandomNumberBetween(240, 270);
                if (veh.Speed <= minLeftLaneSpeed && !isSpawnMoreWHenEntering && !isPlayerOnLeftLane)
                {
                    metersBeforePlayer = isNight ? -140 : -250;
                }
                int oneLaneRnd = RndStopWatch.ElapsedMilliseconds >= 250 ? 1 : 0;
                if (((oneLaneRnd == 1 && CurrentZone.IsOneLaneRoad) && CurrentZone.IsStartingFromRightLane) || ((oneLaneRnd == 1 && CurrentZone.IsOneLaneRoad) && !CurrentZone.IsStartingFromRightLane))
                {
                    metersBeforePlayer = 430;
                }
                if (isNightSuperFast)
                    metersBeforePlayer = -180;
                var carStats = GetRandomCar(isNightSuperFast: isNightSuperFast);
                var playerCords = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, metersBeforePlayer, 0));
                var leftBorder = GetClosestPoint(
                    new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                    new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                    new Vector2(playerCords.X, playerCords.Y)
                    );
                var rightBorder = GetClosestPoint(
                    new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                    new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                    new Vector2(playerCords.X, playerCords.Y)
                    );
                Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = CurrentZone.ZoneBoundary.ZCoord };
                bool leftLane;
                Vector3 lane;
                leftLane = false;
                lane = new Vector3() { X = ((centerPoint.X + rightBorder.X) / 2), Y = ((centerPoint.Y + rightBorder.Y) / 2), Z = CurrentZone.ZoneBoundary.ZCoord };
                /* if (carStats.Speed > (isNight ? (float)(160 / 3.6) : (float)(110 / 3.6)))
                 {
                     carStats.SteerPercentage = 1f;
                     leftLane = true;
                     lane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = playerCords.Z };
                 }*/
                 /*
                if (isNightSuperFast)
                {
                    leftLane = true;
                    lane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = CurrentZone.ZoneBoundary.ZCoord };
                }
                carStats.noOvertake = CurrentZone.IsOneLaneRoad;
                carStats.isLeftLane = leftLane;
                if (carStats.noOvertake && carStats.isLeftLane && CurrentZone.IsStartingFromRightLane)
                {
                    carStats.Heading = CurrentZone.LeftLaneDirection;
                }
                if (carStats.noOvertake && carStats.isLeftLane && !CurrentZone.IsStartingFromRightLane)
                {
                    carStats.Heading = CurrentZone.LeftLaneDirection;
                }
                if (carStats.noOvertake && !carStats.isLeftLane && CurrentZone.IsStartingFromRightLane)
                {
                    carStats.Heading = CurrentZone.HeadingDirection;
                }
                if (carStats.noOvertake && !carStats.isLeftLane && !CurrentZone.IsStartingFromRightLane)
                {
                    carStats.Heading = CurrentZone.HeadingDirection;
                }
                var car = World.CreateVehicle(carStats.Model, lane, carStats.Heading);
                carStats.Speed = carStats.noOvertake ? (float)(96 / 3.6) : carStats.Speed;
                car.Speed = carStats.Speed;
                car.PlaceOnGround();
                car.LightsMultiplier = 6f;
                car.HighBeamsOn = false;
                car.EngineRunning = true;
                carList.Add(new GeneralCar() { Vehicle = car, Stats = carStats });
                spawnedCarsCount++;
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
                    car.HighBeamsOn = true;
                    car.EngineRunning = true;
                    fakeLightList.Add(new GeneralCar() { Vehicle = car, Stats = carStats });

                }
            }
            return addMeters;
        }

        private static void SpawnCarsTollBooth()
        {
            if (!spawnedTollBoothCars)
            {
                for (var sp = 0; sp < CurrentTollBooth.TrafficSpawnPoints.Count; sp++)
                {
                    var spawn = CurrentTollBooth.TrafficSpawnPoints[sp];
                    if (CurrentTollBooth.Name != null)
                    {
                        var hour = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
                        var str = CurrentTollBooth.isStartTollBooth;
                        var nm = CurrentTollBooth.Name;
                        var carsCount = GenerateRandomNumberBetween(
                            ((hour >= 19 || hour < 6) ? (str ? 2 : 1) : (str ? 5 : 4)),
                            ((hour >= 19 || hour < 6) ? (str ? 5 : 5) : (str ? 8 : 9)));
                        if (CurrentTollBooth.Name.Contains("ase"))
                        {
                            carsCount = GenerateRandomNumberBetween(1, 3);
                        }
                        for (var i = 0; i < carsCount; i++)
                        {
                            Vector3 offset;
                            if (i == 0)
                                offset = spawn;
                            else
                                offset = tollBoothcarList.ElementAt(tollBoothcarList.Count - 1).Vehicle.GetOffsetInWorldCoords(new Vector3(0, -16, 0));
                            var carStats = GetRandomCar();
                            carStats.Speed = (float)(10 / 3.6);
                            carStats.Heading = CurrentTollBooth.HeadingDirection;
                            var car = World.CreateVehicle(carStats.Model, offset, carStats.Heading);
                            car.Speed = carStats.Speed;
                            car.PlaceOnGround();
                            car.IsInvincible = true;
                            car.EngineRunning = true;
                            tollBoothcarList.Add(new GeneralCar() { Vehicle = car, Stats = carStats });
                        }
                    }
                }
                spawnedTollBoothCars = true;
            }
        }


        private static void SpawnCarTest()
        {
            if (CurrentZone.Name != null && spawnedCarsCount < maxNumOfSpawnedCars)
            {
                var metersBeforePlayer = GenerateRandomNumberBetween(50, 50);
                var playerCords = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, (test == 2 ? (-metersBeforePlayer) : (metersBeforePlayer)), 0));
                var leftBorder = GetClosestPoint(
                    new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                    new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                    new Vector2(playerCords.X, playerCords.Y)
                    );
                var rightBorder = GetClosestPoint(
                    new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                    new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                    new Vector2(playerCords.X, playerCords.Y)
                    );
                Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = playerCords.Z };
                Vector3 rightLane = test <= 1 ?
                    new Vector3() { X = ((centerPoint.X + rightBorder.X) / 2), Y = ((centerPoint.Y + rightBorder.Y) / 2), Z = playerCords.Z }
                    :
                    new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = playerCords.Z };
                // Vector3 leftLane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = playerCords.Z };
                var carStats = GetRandomCar();
                var car = World.CreateVehicle(VehicleHash.Baller, rightLane, carStats.Heading);
                switch (test)
                {
                    case 0:
                        carStats.Speed = (float)(70 / 3.6);
                        carStats.isLeftLane = false;
                        break;
                    case 1:
                        carStats.Speed = (float)(80 / 3.6);
                        carStats.isLeftLane = false;
                        break;
                    case 2:
                        carStats.Speed = (float)(140 / 3.6);
                        carStats.isLeftLane = true;
                        carStats.forcedLeftLane = false;
                        break;
                }
                car.Speed = carStats.Speed;
                carStats.DefaultSpeed = car.Speed;
                car.PlaceOnGround();
                carStats.Class = "family";
                carList.Add(new GeneralCar() { Vehicle = car, Stats = carStats });
                test++;
                if (test == 3)
                    test = 0;
            }

        }

        private static void SpawnTrafficBlockers()
        {
            if (CurrentZone.Name != null && spawnedCarsCount < maxNumOfSpawnedCars)
            {
                for (var cb = 0; cb < 2; ++cb)
                {
                    var metersBeforePlayer = GenerateRandomNumberBetween(250, 250);
                    var playerCords = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, metersBeforePlayer, 0));
                    var leftBorder = GetClosestPoint(
                        new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                        new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                        new Vector2(playerCords.X, playerCords.Y)
                        );
                    var rightBorder = GetClosestPoint(
                        new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                        new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                        new Vector2(playerCords.X, playerCords.Y)
                        );
                    Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = playerCords.Z };
                    Vector3 rightLane = test <= 0 ?
                        new Vector3() { X = ((centerPoint.X + rightBorder.X) / 2), Y = ((centerPoint.Y + rightBorder.Y) / 2), Z = playerCords.Z }
                        :
                        new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = playerCords.Z };
                    // Vector3 leftLane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = playerCords.Z };
                    var carStats = GetRandomCar();
                    var car = World.CreateVehicle(Cars[GenerateRandomNumberBetween(0, Cars.Count)].Model, rightLane, carStats.Heading);
                    carStats.Speed = (float)(30 / 3.6);
                    switch (test)
                    {
                        case 0:
                            carStats.isLeftLane = false;
                            break;
                        case 1:
                            carStats.isLeftLane = true;
                            break;
                    }
                    car.Speed = carStats.Speed;
                    carStats.DefaultSpeed = car.Speed;
                    car.PlaceOnGround();
                    carList.Add(new GeneralCar() { Vehicle = car, Stats = carStats });
                    test++;
                    if (test >= 2)
                        test = 0;
                }

            }

        }

        private static Vector2 GetClosestPoint(Vector2 v1, Vector2 v2, Vector2 p)
        {
            Vector2 AP = p - v1;
            Vector2 AB = v2 - v1;
            float ab2 = AB.X * AB.X + AB.Y * AB.Y;
            float ap_ab = AP.X * AB.X + AP.Y * AB.Y;
            float t = ap_ab / ab2;
            Vector2 closest = v1 + AB * t;
            return closest;
        }

        private static float dist2(Vector2 v1, Vector2 v2)
        {
            return (float)(Math.Sqrt(v1.X - v2.X) + Math.Sqrt(v1.Y - v2.Y));
        }

        private static void MoveCarTollBooth()
        {
            if (tollBoothcarList.Count > 0)
            {
                var toDelIndex = 0;
                List<int> toDelete = new List<int>();
                for (var cr = 0; cr < tollBoothcarList.Count; cr++)
                {
                    var car = tollBoothcarList[cr];
                    var end = new Vector3() { X = CurrentTollBooth.ZoneBoundary.FinishLeftX, Y = CurrentTollBooth.ZoneBoundary.FinishLeftY, Z = z };
                    var distToEnd = car.Vehicle.Position.DistanceTo(end);
                    if (distToEnd <= 4f)
                    {
                        car.Vehicle.MarkAsNoLongerNeeded();
                        Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                        car.Vehicle.Position = del;
                        toDelete.Add(toDelIndex);
                    }
                    car.Vehicle.Speed = car.Stats.Speed;
                    var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
                    var m = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
                    var s = Function.Call<int>((Hash)0x494E97C2EF27C470);
                    if (isRaining || (h >= 19 || h < 7))
                    {
                        car.Vehicle.LightsOn = true;
                    }
                    if (!isRaining && (h >= 7 && h < 19))
                    {
                        car.Vehicle.LightsOn = false;
                    }
                    toDelIndex++;
                }
                if (CurrentTollBooth.Name != null)
                {
                    var ls = toDelete.Distinct().ToList();
                    for (var td = 0; td < ls.Count; td++)
                    {
                        tollBoothcarList.RemoveAt(ls[td]);
                    }
                }
                else
                {
                    tollBoothcarList = new List<GeneralCar>();
                }
            }
        }

        private static void CreatePlayerCarModel(bool isTruck = false)
        {
            var carStats = GetRandomCar();
            carStats.isLeftLane = isPlayerOnLeftLane;
            var car = World.CreateVehicle(VehicleHash.Panto, veh.Position, veh.Heading);
            car.Position = veh.Position;
            if (isTruck)
            {
                car.Position = veh.GetOffsetInWorldCoords(new Vector3(0, -19, 0));
                carStats.isPlayerTruck = true;
            }
            else
            {
                carStats.isPlayerCar = true;
            }
            carStats.Speed = veh.Speed;
            car.Speed = carStats.Speed;
            carList.Add(new GeneralCar() { Vehicle = car, Stats = carStats });
        }

        private static int ProximityMeter(GeneralCar c)
        {
            var dist = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, -6f, 0)).DistanceTo(c.Vehicle.Position) < 70f;
            Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
            var distToEnd = c.Vehicle.Position.DistanceTo(zoneEnd) > Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, -6f, 0)).DistanceTo(zoneEnd);
            if (dist && distToEnd)
                return 1;
            else
                return 0;
        }

        private static int FasterCarBehind(GeneralCar c)
        {
            var dist = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, -6f, 0)).DistanceTo(c.Vehicle.Position) < 70f;
            Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
            var distToEnd = c.Vehicle.Position.DistanceTo(zoneEnd) > Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, -6f, 0)).DistanceTo(zoneEnd);
            if (dist && distToEnd && c.Stats.DefaultSpeed > veh.Speed)
                return 1;
            else
                return 0;
        }

        private static void Radar()
        {
            CreatePlayerCarModel();
            if (veh.DisplayName.Contains("vnl") || veh.DisplayName.Contains("ct66"))
            {
                CreatePlayerCarModel(true);
            }
            int plr = 0;
            int pIndex = 0;
            int plrT = 0;
            int carsNearPlayer = 0;
            int carsFasterThanPlayer = 0;
            for (var ik = 0; ik < carList.Count; ik++)
            {
                var c = carList.ElementAt(ik);
                if (c.Stats.isPlayerCar)
                {
                    plr = pIndex;
                }
                if (c.Stats.isPlayerTruck)
                {
                    plrT = pIndex;
                }
                if (!c.Stats.isPlayerCar && !c.Stats.isPlayerTruck && c.Stats.Class != "truck")
                {
                    if (!isPlayerOnLeftLane && c.Stats.isLeftLane)
                        carsNearPlayer += ProximityMeter(c);
                    if (isPlayerOnLeftLane && c.Stats.isLeftLane)
                        carsFasterThanPlayer += FasterCarBehind(c);
                    var nearCar = new List<GeneralCar>();
                    Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
                    var carAhead = new GeneralCar() { Stats = new CarStatsModel() { isEmpty = true }, Vehicle = new Vehicle(1) };
                    var carNextTo = new GeneralCar() { Stats = new CarStatsModel() { isEmpty = true }, Vehicle = new Vehicle(1) };
                    int truckSphereFactor = c.Stats.Class == "truck" ? 20 : 0;
                    for (var iu = 0; iu < carList.Count; iu++)
                    {
                        var carAround = carList.ElementAt(iu);
                        if (carAround.Vehicle.Position.DistanceTo(c.Vehicle.Position) < 40)
                        {
                            if (c.Stats.Id != carAround.Stats.Id)
                            {
                                if (c.Stats.isLeftLane)
                                {
                                    if (carAround.Vehicle.Position.DistanceTo(c.Vehicle.Position) < (37)
                                       && carAround.Stats.isLeftLane
                                       && !(carAround.Vehicle.Position.DistanceTo(zoneEnd) > c.Vehicle.Position.DistanceTo(zoneEnd)))
                                    {
                                        carAhead = carAround;
                                    }
                                    if (carAround.Vehicle.Position.DistanceTo(c.Vehicle.Position) < (35)
                                     && !carAround.Stats.isLeftLane
                                     && !(carAround.Vehicle.Position.DistanceTo(zoneEnd) - 10.0 > c.Vehicle.Position.DistanceTo(zoneEnd)))
                                    {
                                        carNextTo = carAround;
                                    }
                                }
                                else
                                {
                                    var isCarAroundBehind = carAround.Vehicle.Position.DistanceTo(zoneEnd) > c.Vehicle.Position.DistanceTo(zoneEnd);
                                    if (carAround.Vehicle.Position.DistanceTo(c.Vehicle.Position) < (37 + truckSphereFactor)
                                     && !carAround.Stats.isLeftLane
                                     && !isCarAroundBehind)
                                    {
                                        carAhead = carAround;
                                    }
                                    if (carAround.Vehicle.Position.DistanceTo(c.Vehicle.Position) < (35 + truckSphereFactor)
                                     && carAround.Stats.isLeftLane)
                                    {
                                        carNextTo = carAround;
                                    }
                                }
                            }
                        }
                    }
                    var carAheadKmh = Math.Round(carAhead.Vehicle.Speed * 3.6, 0);
                    var cKmh = Math.Round(c.Vehicle.Speed * 3.6, 0);
                    var isCarNextBehind = carNextTo.Vehicle.Position.DistanceTo(zoneEnd) - 10.0 > c.Vehicle.Position.DistanceTo(zoneEnd);
                    if (c.Stats.isLeftLane)
                    {
                        if (!carAhead.Stats.isEmpty)
                        {
                            if (carAheadKmh < cKmh)
                            {
                                c.Stats.deccelerate = true;
                                c.Stats.adjustSpeedTo = carAhead.Vehicle.Speed;
                            }
                            if ((carAheadKmh - cKmh > 10) && c.Stats.notDefaultSpeed && !c.Stats.accelerate)
                            {
                                c.Stats.accelerate = true;
                            }
                        }
                        else
                        {
                            if (c.Vehicle.Speed < c.Stats.DefaultSpeed)
                            {
                                c.Stats.accelerate = true;
                            }
                            if (carNextTo.Stats.isEmpty && !c.Stats.isEmergencyBraking && !c.Stats.IsOvertaking && (c.Stats.Class != "nightsuperfast"))
                            {

                                var leftBorder = GetClosestPoint(
                                 new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                                 new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                                 new Vector2(c.Vehicle.Position.X, c.Vehicle.Position.Y)
                                );
                                var rightBorder = GetClosestPoint(
                                    new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                                    new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                                    new Vector2(c.Vehicle.Position.X, c.Vehicle.Position.Y)
                                );
                                Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = veh.Position.Z };
                                Vector3 rightLane = new Vector3() { X = ((centerPoint.X + rightBorder.X) / 2), Y = ((centerPoint.Y + rightBorder.Y) / 2), Z = veh.Position.Z };
                                c.Stats.IsOvertaking = true;
                                c.Stats.SteerPercentage = 0.9f;
                                c.Vehicle.RightIndicatorLightOn = true;
                            }
                        }

                    }
                    if (!c.Stats.isLeftLane)
                    {
                        if (!carAhead.Stats.isEmpty)
                        {
                            if ((carAheadKmh - cKmh > 6) && c.Stats.notDefaultSpeed && !c.Stats.accelerate)
                            {
                                c.Stats.accelerate = true;
                            }
                            if (!carNextTo.Stats.isEmpty)
                            {
                                if (isCarNextBehind && c.Stats.IsOvertaking)
                                {

                                }
                                else if (carAheadKmh < cKmh && !carAhead.Stats.IsOvertaking)
                                {
                                    c.Stats.deccelerate = true;
                                    c.Stats.adjustSpeedTo = carAhead.Vehicle.Speed;
                                }
                            }
                            else if ((carAheadKmh < cKmh || (carAheadKmh < Math.Round(c.Stats.DefaultSpeed * 3.6, 0))) && (((float)Math.Round(c.Vehicle.Speed * 3.6, 0) - carAheadKmh) >= 25) && !c.Stats.IsOvertaking)
                            {
                                if (c.Stats.Class == "truck")
                                {
                                    c.Stats.deccelerate = true;
                                    c.Stats.adjustSpeedTo = carAhead.Vehicle.Speed;
                                }
                                else
                                {
                                    var leftBorder = GetClosestPoint(
                                                                    new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                                                                    new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                                                                    new Vector2(c.Vehicle.Position.X, c.Vehicle.Position.Y)
                                                                    );
                                    var rightBorder = GetClosestPoint(
                                        new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                                        new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                                        new Vector2(c.Vehicle.Position.X, c.Vehicle.Position.Y)
                                    );
                                    Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = c.Vehicle.Position.Z };
                                    Vector3 leftLane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = c.Vehicle.Position.Z };
                                    c.Stats.IsOvertaking = true;
                                    c.Stats.SteerPercentage = 0f;
                                    c.Vehicle.LeftIndicatorLightOn = true;
                                }
                            }
                        }
                    }
                }
                pIndex++;
            }
            if (carsNearPlayer > 0)
                showProximityMeter = true;
            else
                showProximityMeter = false;
            if (carsFasterThanPlayer > 0)
                showFasterCarBehind = true;
            else
                showFasterCarBehind = false;
            var playerCarToDel = carList.ElementAt(plr);
            Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
            playerCarToDel.Vehicle.Speed = 0f;
            playerCarToDel.Vehicle.MarkAsNoLongerNeeded();
            playerCarToDel.Vehicle.Position = del;
            playerCarToDel.Stats.Deleted = true;
            carList.RemoveAt(plr);
            if (veh.DisplayName.Contains("vnl") || veh.DisplayName.Contains("ct66"))
            {
                plrT = (plrT - 1) >= 0 ? plrT - 1 : 0;
                var playerTruckrToDel = carList.ElementAt(plrT);
                Vector3 delT = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                playerTruckrToDel.Vehicle.Speed = 0f;
                playerTruckrToDel.Vehicle.MarkAsNoLongerNeeded();
                playerTruckrToDel.Vehicle.Position = delT;
                playerTruckrToDel.Stats.Deleted = true;
                carList.RemoveAt(plrT);
            }
        }

        private static void AdjustCarSpeed()
        {
            for (var io = 0; io < carList.Count; io++)
            {
                var car = carList.ElementAt(io);
                if (car.Stats.deccelerate)
                {
                    car.Stats.accelerate = false;
                    if (car.Vehicle.Speed <= car.Stats.adjustSpeedTo)
                    {
                        car.Vehicle.BrakeLightsOn = false;
                        car.Stats.deccelerate = false;
                        car.Vehicle.Speed = car.Stats.adjustSpeedTo;
                    }
                    else
                    {
                        var isFast = car.Stats.Class == "nightsuperfast";
                        var bf = 0f;
                        var speedDif = (Math.Round(car.Stats.DefaultSpeed, 0) - Math.Round(car.Stats.adjustSpeedTo, 0));
                        bf = (float)(Math.Round(speedDif / 100, 2) + (isFast ? 0.30 : 0.20));
                        if (car.Stats.Speed - bf <= 0)
                        {
                            car.Stats.Speed = 0;
                            car.Vehicle.BrakeLightsOn = false;
                            car.Stats.deccelerate = false;
                        }
                        else
                        {
                            if (isFast)
                            {
                                //car.Stats.isFlashingHighBeam = true;
                            }
                            car.Stats.Speed -= bf;
                            car.Vehicle.BrakeLightsOn = true;
                            car.Stats.notDefaultSpeed = true;
                        }
                    }
                }
                if (car.Stats.accelerate)
                {
                    car.Vehicle.BrakeLightsOn = false;
                    car.Stats.deccelerate = false;
                    if (car.Stats.Speed >= car.Stats.DefaultSpeed)
                    {
                        car.Stats.accelerate = false;
                        car.Vehicle.Speed = car.Stats.DefaultSpeed;
                        car.Stats.notDefaultSpeed = false;
                    }
                    else
                    {
                        var speedDif = Math.Round(car.Stats.DefaultSpeed, 0) - Math.Round(car.Stats.adjustSpeedTo, 0) + 100;
                        car.Stats.Speed += (float)Math.Round(speedDif / 1000, 3);
                    }
                }
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

        private static void SpawnFakeLight(int distAhead = 450)
        {
            SpawnDummyCar(distAhead);
        }

        private static void RemoveAllFakeLightsAndRef()
        {
            fakeLightList = new List<GeneralCar>();
        }

        private static void MoveCar()
        {
            var i = 0;
            i++;
            for (var io = 0; io < carList.Count; io++)
            {
                var car = carList.ElementAt(io);
                bool toDelete = false;
                if (!car.Stats.Deleted)
                {
                    bool isInZone = false;
                    if (CurrentZone.Name != null)
                    {
                        var carX = (int)Math.Round(car.Vehicle.Position.X, 0);
                        var carY = (int)Math.Round(car.Vehicle.Position.Y, 0);
                        var t1 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY, CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY, CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY);
                        var t2 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY, CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY, CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY);
                        var t3 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY, CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY, CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY);
                        var t4 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY, CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY, CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY);
                        isInZone = t1 || t2 || t3 || t4;
                    }
                    if (isInZone)
                    {
                        if (car.Stats.isFlashingHighBeam)
                        {
                            Function.Call((Hash)0x8B7FD87F0DDB421E, car.Vehicle, (car.Vehicle.HighBeamsOn ? false : true));
                            car.Stats.highBeamTicks += 1;
                        }
                        if (car.Stats.highBeamTicks >= 6)
                        {
                            car.Stats.highBeamTicks = 0;
                            car.Stats.isFlashingHighBeam = false;
                        }
                        //fixing car
                        Function.Call((Hash)0x115722B1B9C14C1C, car.Vehicle);
                        Function.Call((Hash)0x953DA1E1B12C0491, car.Vehicle);
                    }
                    var distBetweenPlayer = Function.Call<float>((Hash)0xF1B760881820C952, veh.Position.X, veh.Position.Y, veh.Position.Z, car.Vehicle.Position.X, car.Vehicle.Position.Y, car.Vehicle.Position.Z);
                    // flying cars
                    /*  if (car.Vehicle.IsInAir)
                    {
                        toDelete = true; 
                    }*/
                    /*
                    Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
                    var isCarAroundBehind = car.Vehicle.Position.DistanceTo(zoneEnd) > veh.Position.DistanceTo(zoneEnd);
                    if (distBetweenPlayer > 100 && isCarAroundBehind && veh.Speed > car.Stats.DefaultSpeed)
                    {
                        toDelete = true;
                    }
                    if ((car.Stats.noOvertake ? distBetweenPlayer > 450f : distBetweenPlayer > 700f) || !isInZone || toDelete)
                    {
                        if (!car.Stats.Deleted)
                        {
                            Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                            car.Vehicle.Speed = 0f;
                            car.Vehicle.MarkAsNoLongerNeeded();
                            car.Vehicle.Position = del;
                            car.Stats.Deleted = true;
                            spawnedCarsCount--;
                        }
                    }
                    else
                    {
                        var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
                        var m = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
                        var s = Function.Call<int>((Hash)0x494E97C2EF27C470);
                        if (isRaining || (h >= 19 || h < 7))
                        {
                            car.Vehicle.LightsOn = true;
                        }
                        if (!isRaining && (h >= 7 && h < 19))
                        {
                            car.Vehicle.LightsOn = false;
                        }
                    }
                }
            }
            for (var po = 0; po < carList.Count; po++)
            {
                if (carList.ElementAt(po).Stats.Deleted)
                {
                    carList.RemoveAt(po);
                };
            }
        }

        private static void Overtake()
        {
            try
            {
                if (CurrentZone.Name == null && (carList.Count != 0 || trafficJamCarList.Count != 0))
                {
                    for (var cr = 0; cr < carList.Count; cr++)
                    {
                        var car = carList[cr];
                        car.Vehicle.MarkAsNoLongerNeeded();
                        Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                        car.Vehicle.Position = del;
                    }
                    carList = new List<GeneralCar>();
                    trafficJamCarList = new List<GeneralCar>();
                    spawnedCarsCount = 0;
                }
                for (var cra = 0; cra < carList.Count; cra++)
                {
                    var car = carList[cra];
                    if (!car.Stats.Deleted)
                    {
                        if (car.Stats.IsOvertaking)
                        {
                            var leftBorder = GetClosestPoint(
                                   new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                                   new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                                   new Vector2(car.Vehicle.Position.X, car.Vehicle.Position.Y)
                               );
                            var rightBorder = GetClosestPoint(
                                new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                                new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                                new Vector2(car.Vehicle.Position.X, car.Vehicle.Position.Y)
                            );
                            Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = car.Vehicle.Position.Z };
                            Vector3 leftLane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = car.Vehicle.Position.Z };
                            Vector3 rightLane = new Vector3() { X = ((centerPoint.X + rightBorder.X) / 2), Y = ((centerPoint.Y + rightBorder.Y) / 2), Z = car.Vehicle.Position.Z };
                            Vector3 dir = leftLane - rightLane;
                            // float distanceBetweenLanes = (float)Math.Sqrt(((leftLane.X - rightLane.X) * 2) + ((leftLane.Y - rightLane.Y) * 2));
                            float distanceBetweenLanes = leftLane.DistanceTo(rightLane);
                            float steerRatio = car.Stats.Class.ToLower() == "truck" ? 0.0045f : 0.007f;
                            car.Stats.SteerPercentage = car.Stats.isLeftLane ? (car.Stats.SteerPercentage - steerRatio) : (car.Stats.SteerPercentage + steerRatio);
                            Vector3 desiredPosition = (car.Stats.SteerPercentage * leftLane + (1 - car.Stats.SteerPercentage) * rightLane);
                            car.Vehicle.Position = desiredPosition;
                            if ((car.Stats.SteerPercentage >= 0f && car.Stats.SteerPercentage <= 0.3 && !car.Stats.isLeftLane))
                            {
                                car.Stats.HeadingPercentage -= 0.08f;
                            }
                            if ((car.Stats.SteerPercentage <= 1f && car.Stats.SteerPercentage >= 0.7 && car.Stats.isLeftLane))
                            {
                                car.Stats.HeadingPercentage -= 0.08f;
                            }
                            if ((car.Stats.SteerPercentage >= 0.7f && car.Stats.SteerPercentage < 1f && !car.Stats.isLeftLane && (car.Vehicle.Heading > car.Stats.Heading)))
                            {
                                car.Stats.HeadingPercentage += 0.08f;
                            }
                            if ((car.Stats.SteerPercentage <= 0.3f && car.Stats.SteerPercentage > 0 && car.Stats.isLeftLane && (car.Vehicle.Heading < car.Stats.Heading)))
                            {
                                car.Stats.HeadingPercentage += 0.08f;
                            }
                            if ((car.Stats.SteerPercentage >= 1f && !car.Stats.isLeftLane))
                            {
                                car.Stats.IsOvertaking = false;
                                car.Stats.isLeftLane = true;
                                car.Vehicle.LeftIndicatorLightOn = false;
                                car.Vehicle.Heading = car.Stats.Heading;
                                car.Stats.HeadingPercentage = 0;
                            }
                            if ((car.Stats.SteerPercentage <= 0f && car.Stats.isLeftLane))
                            {
                                car.Stats.IsOvertaking = false;
                                car.Stats.isLeftLane = false;
                                car.Vehicle.RightIndicatorLightOn = false;
                                car.Vehicle.Heading = car.Stats.Heading;
                                car.Stats.HeadingPercentage = 0;
                            }
                            car.Vehicle.Heading = car.Stats.Heading + (car.Stats.isLeftLane ? car.Stats.HeadingPercentage : -(car.Stats.HeadingPercentage));
                        }
                        else
                        {
                            car.Stats.HeadingPercentage = 0;
                            car.Vehicle.Heading = car.Stats.Heading;
                        }
                        if ((car.Vehicle.Speed * 3.6) < (car.Stats.Speed * 3.6))
                        {
                            car.Vehicle.ApplyForceRelative(new Vector3(0, 0.05f, 0));
                        }

                    }
                }
            }
            catch (Exception e)
            {
                UI.ShowSubtitle("error occured in overtake method", 5000);
            }
        }

        private static void RemoveStaticPropsFromZone()
        {
            if (StaticPropsSpawned)
            {
                StaticPropsSpawned = false;
                for (var i = 0; i < StaticVehicleList.Count; i++)
                {
                    var staticVehicle = StaticVehicleList[i];
                    staticVehicle.Position = new Vector3(0, 0, 0);
                    staticVehicle.MarkAsNoLongerNeeded();
                    staticVehicle.Delete();
                }
                for (var i = 0; i < StaticPedList.Count; i++)
                {
                    var staticPed = StaticPedList[i];
                    staticPed.Position = new Vector3(0, 0, 0);
                    staticPed.MarkAsNoLongerNeeded();
                    staticPed.Delete();
                }
            }
        }

        private static void SpawnStaticPropsForZone()
        {
            if (!StaticPropsSpawned)
            {
                var props = CurrentZone.StaticProps;
                if (props != null)
                {
                    StaticPropsSpawned = true;
                    for (var i = 0; i < props.Count; i++)
                    {
                        var prop = props[i];
                        if (prop.Type == "vehicle")
                        {
                            Vehicle staticVehicle;
                            if (prop.IsElectric)
                            {
                                staticVehicle = World.CreateVehicle(
                                  new Model(StaticPropsLists.ElectricCars[GenerateRandomNumberBetween(0, StaticPropsLists.ElectricCars.Count)]),
                                  new Vector3(prop.Postion.X, prop.Postion.Y, CurrentZone.ZoneBoundary.ZCoord));
                                if (true)
                                {
                                    staticVehicle.CreatePedOnSeat(VehicleSeat.Driver, new Model(StaticPropsLists.Peds[GenerateRandomNumberBetween(0, StaticPropsLists.Peds.Count)]));
                                }
                            }
                            else
                            {
                                staticVehicle = World.CreateVehicle(
                                  new Model(StaticPropsLists.PassengerCars[GenerateRandomNumberBetween(0, StaticPropsLists.PassengerCars.Count)]),
                                  new Vector3(prop.Postion.X, prop.Postion.Y, CurrentZone.ZoneBoundary.ZCoord));
                            }
                            staticVehicle.Heading = prop.Heading;
                            staticVehicle.PlaceOnGround();
                            StaticVehicleList.Add(staticVehicle);
                        }
                        if (prop.Type == "ped")
                        {
                            var staticPed = World.CreatePed(
                                new Model(StaticPropsLists.Peds[GenerateRandomNumberBetween(0, StaticPropsLists.Peds.Count)]),
                                new Vector3(prop.Postion.X, prop.Postion.Y, CurrentZone.ZoneBoundary.ZCoord),
                                prop.Heading);
                            var anim = "";
                            if (prop.ForcedAnimation != null)
                                anim = prop.ForcedAnimation;
                            else
                                anim = StaticPropsLists.PedAnimations[GenerateRandomNumberBetween(0, StaticPropsLists.PedAnimations.Count)];
                            Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, staticPed, anim.ToUpper(), -1, false);
                            StaticPedList.Add(staticPed);
                        }
                    }
                }
            }
        }

        private static void GetCurrentZone()
        {
            for (var zn = 0; zn < Zones.Count; zn++)
            {
                var zone = Zones[zn];
                var t1 = PointInTriangle(x, y, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY);
                var t2 = PointInTriangle(x, y, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY);
                var t3 = PointInTriangle(x, y, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY);
                var t4 = PointInTriangle(x, y, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY);
                if ((t1 || t2 || t3 || t4) && !isZoneSet)
                {
                    CurrentZone = zone;
                    //  UI.Notify("found zone" + zone.Name + zone.ZoneBoundary.StartLeftX);
                    Vector3 zoneStart = new Vector3() { X = CurrentZone.ZoneBoundary.StartLeftX, Y = CurrentZone.ZoneBoundary.StartLeftY, Z = z };
                    Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishLeftX, Y = CurrentZone.ZoneBoundary.FinishLeftY, Z = z };
                    if (Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneStart) <= 20f
                        || Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneEnd) <= 20f)
                    {
                        CurrentZone.IsStartingFromRightLane = !PlayerLaneCheck();
                    }
                    break;
                }
            }
            if (CurrentZone.Name != null)
            {
                Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishLeftX, Y = CurrentZone.ZoneBoundary.FinishLeftY, Z = z };
                Vector3 zoneStart = new Vector3() { X = CurrentZone.ZoneBoundary.StartLeftX, Y = CurrentZone.ZoneBoundary.StartLeftY, Z = z };
                var startDist = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneStart);
                if (startDist > 150 && !StaticPropsSpawned)
                {
                    // SpawnStaticPropsForZone();
                }
                var dist = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneEnd);
                if (dist < 40f)
                {
                    //  UI.Notify("end of zone");
                    // RemoveStaticPropsFromZone();
                    CurrentZone = new HighwayZoneModel();
                }
            }

        }

        private static void SpawnMoreCarsOnZoneEnter()
        {
            if (CurrentZone.Name != null && !isTrafficJam)
            {
                Vector3 zoneStart = new Vector3() { X = CurrentZone.ZoneBoundary.StartLeftX, Y = CurrentZone.ZoneBoundary.StartLeftY, Z = z };
                var dist = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).DistanceTo(zoneStart);
                if (dist <= 100f && !wasSpawnMoreCarsWhenEnteringZone)
                {
                    int metersBefore = (isNight ? 66 : 50);
                    int carCount = GenerateRandomNumberBetween((isNight ? 1 : 8), (isNight ? 1 : 11));
                    for (var i = 0; i < carCount; i++)
                    {
                        metersBefore += 54;
                        SpawnCar(true, metersBefore);
                    }
                    // CreateFakeLightsRefPoint();
                    wasSpawnMoreCarsWhenEnteringZone = true;
                    spawnMoreFakeLightsOnZoneEnter = true;
                }
            }
            else
            {
                wasSpawnMoreCarsWhenEnteringZone = false;
            }
        }

        private static void SpawnMultipleCars()
        {
            int metersBefore = 0;
            if (veh.Speed <= minLeftLaneSpeed)
                metersBefore = -260;
            else
                metersBefore = (isNight ? 250 : 200);
            int carCount = GenerateRandomNumberBetween((isNight ? 1 : 4), (isNight ? 4 : 10));
            if (veh.Speed <= minLeftLaneSpeed)
                carCount = GenerateRandomNumberBetween((isNight ? 1 : 2), (isNight ? 4 : 3));
            for (var i = 0; i < carCount; i++)
            {
                if (veh.Speed <= minLeftLaneSpeed)
                    metersBefore += 55;
                else
                    metersBefore += 79;
                SpawnCar(true, metersBefore);
            }
        }

        private static void GetCurrentTollBooth()
        {
            for (var zn = 0; zn < TollBooths.Count; zn++)
            {
                var zone = TollBooths[zn];
                var t1 = PointInTriangle(x, y, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY);
                var t2 = PointInTriangle(x, y, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY);
                var t3 = PointInTriangle(x, y, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY);
                var t4 = PointInTriangle(x, y, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY);
                if (t1 || t2 || t3 || t4)
                {
                    CurrentTollBooth = zone;
                    break;
                }
                else
                    CurrentTollBooth = new HighwayZoneModel();
            }
            if (CurrentTollBooth.Name != null && !isAtTollBooth)
            {
                isAtTollBooth = true;
                SpawnCarsTollBooth();
            }
            if (CurrentTollBooth.Name == null)
            {
                isAtTollBooth = false;
                spawnedTollBoothCars = false;
                for (var cru = 0; cru < tollBoothcarList.Count; cru++)
                {
                    var car = tollBoothcarList[cru];
                    car.Vehicle.MarkAsNoLongerNeeded();
                    Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                    car.Vehicle.Position = del;
                }
                tollBoothcarList = new List<GeneralCar>();
            }
            TollBoothTicket();
        }

        private static void TollBoothTicket()
        {
            if (isAtTollBooth && tollBoothTicketGivenStopWatch.ElapsedMilliseconds > 8000)
            {
                var ticketBoundary = CurrentTollBooth.TicketZoneBoundary;
                var t1 = PointInTriangle(x, y, ticketBoundary.StartLeftX, ticketBoundary.StartLeftY, ticketBoundary.FinishLeftX, ticketBoundary.FinishLeftY, ticketBoundary.StartRightX, ticketBoundary.StartRightY);
                var t2 = PointInTriangle(x, y, ticketBoundary.FinishLeftX, ticketBoundary.FinishLeftY, ticketBoundary.FinishRightX, ticketBoundary.FinishRightY, ticketBoundary.StartLeftX, ticketBoundary.StartLeftY);
                var t3 = PointInTriangle(x, y, ticketBoundary.FinishRightX, ticketBoundary.FinishRightY, ticketBoundary.StartRightX, ticketBoundary.StartRightY, ticketBoundary.FinishLeftX, ticketBoundary.FinishLeftY);
                var t4 = PointInTriangle(x, y, ticketBoundary.StartRightX, ticketBoundary.StartRightY, ticketBoundary.StartLeftX, ticketBoundary.StartLeftY, ticketBoundary.FinishRightX, ticketBoundary.FinishRightY);
                if (t1 || t2 || t3 || t4)
                {
                    tollBoothTicketGivenStopWatch = new Stopwatch();
                    tollBoothTicketGivenStopWatch.Start();
                    var hour = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
                    var minute = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
                    var second = Function.Call<int>((Hash)0x494E97C2EF27C470);
                    var day = Function.Call<int>((Hash)0xD972E4BD7AEB235F);
                    if (!CurrentTollBooth.isStartTollBooth)
                    {
                        isOnHighway = true;
                        if (!CurrentTollBooth.Name.Contains("ortland"))
                        {
                            isOnHighway = false;
                        }
                        isAtTollBooth = false;
                    }
                    else
                    {
                        TimeSinceEnteredHighway = new DateTime(1, 1, day, hour, minute, second);
                        isOnHighway = true;
                    }
                    int hDiff = (new DateTime(1, 1, day, hour, minute, second) - TimeSinceEnteredHighway).Hours;
                    int mDiff = (new DateTime(1, 1, day, hour, minute, second) - TimeSinceEnteredHighway).Minutes;
                    var hourString = hour.ToString().Length == 1 ? "0" + hour : hour.ToString();
                    var minuteString = minute.ToString().Length == 1 ? "0" + minute : minute.ToString();
                    string tollBoothText = CurrentTollBooth.isStartTollBooth ?
                    $@"A1 Highway - {CurrentTollBooth.Name}  Current time is: {hourString + ":" + minuteString}   Please keep your ticket"
                    :
                    $@"A1 Highway - {CurrentTollBooth.Name}  Current time is: {hourString + ":" + minuteString}   Time since: {hDiff + "h " + mDiff + "m "}   Thank you for traveling on our roads, safe journey!";
                    UI.Notify(tollBoothText);
                }
            }
        }

        private static double Sign(int p1x, int p1y, int p2x, int p2y, int p3x, int p3y)
        {
            return (p1x - p3x) * (p2y - p3y) - (p2x - p3x) * (p1y - p3y);
        }

        private static bool PointInTriangle(int pX, int pY, int v1x, int v1y, int v2x, int v2y, int v3x, int v3y)
        {
            bool b1;
            bool b2;
            bool b3;

            b1 = Sign(pX, pY, v1x, v1y, v2x, v2y) < 0.0;
            b2 = Sign(pX, pY, v2x, v2y, v3x, v3y) < 0.0;
            b3 = Sign(pX, pY, v3x, v3y, v1x, v1y) < 0.0;

            return ((b1 == b2) && (b2 == b3));
        }


        private static CarStatsModel GetRandomCar(bool isServiceCar = false, int serviceIndex = 0, bool isNightSuperFast = false)
        {
            CarModel model = Cars[GenerateRandomNumberBetween(0, Cars.Count)];
            if (isNightSuperFast)
            {
                model = SuperCars[GenerateRandomNumberBetween(0, SuperCars.Count)];
            }
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds + carList.Count;
            var car = new CarStatsModel()
            {
                Model = model.Model,
                Class = model.Class,
                Speed = GetSpeedForCar(model.Class),
                Heading = (float)CurrentZone.HeadingDirection,
                Id = secondsSinceEpoch.ToString(),
                OvertakenCarsIds = new List<string>(),
                DefaultSpeed = GetSpeedForCar(model.Class)
            };
            return car;
        }

        private static float GetSpeedForCar(string carClass)
        {
            /* if (carClass == "truck")
                 return (float)(88 / 3.6);
             else if (carClass == "nightsuperfast")
             {
                 return (float)(140 / 3.6);
             }
             else
             {
                 return (float)(110 / 3.6);
             }*/
             /*
            if ((veh.DisplayName.Contains("vnl") || veh.DisplayName.Contains("ct66")) && carClass != "nightsuperfast")
            {
                switch (carClass)
                {
                    case "truck":
                        return (float)(88 / 3.6);
                    default:
                        return (float)(130 / 3.6);
                }
            }
            if (isNight)
            {
                switch (carClass)
                {
                    case "family":
                        return (((veh.Speed > (float)(100 / 3.6) ? (float)(130 / 3.6) : (float)(130 / 3.6))));
                    case "truck":
                        return (((veh.Speed > (float)(105 / 3.6) ? (float)(95 / 3.6) : (float)(88 / 3.6))));
                    case "fast":
                        return (float)(170 / 3.6);
                    case "superfast":
                        return (float)(190 / 3.6);
                    case "pickup":
                        return (float)(140 / 3.6);
                    case "nightsuperfast":
                        return (float)(GenerateRandomNumberBetween(170, 195) / 3.6);
                    default:
                        return (((veh.Speed > (float)(100 / 3.6) ? (float)(120 / 3.6) : (float)(120 / 3.6)))); ;
                }
            }
            if (isRaining)
            {
                switch (carClass)
                {
                    case "family":
                        return (((veh.Speed > (float)(100 / 3.6) ? (float)(100 / 3.6) : (float)(100 / 3.6))));
                    case "truck":
                        return (((veh.Speed > (float)(105 / 3.6) ? (float)(95 / 3.6) : (float)(88 / 3.6))));
                    case "fast":
                        return (float)(140 / 3.6);
                    case "superfast":
                        return (float)(160 / 3.6);
                    case "pickup":
                        return (float)(110 / 3.6);
                    case "nightsuperfast":
                        return (float)(180 / 3.6);
                    default:
                        return (((veh.Speed > (float)(100 / 3.6) ? (float)(100 / 3.6) : (float)(100 / 3.6)))); ;
                }
            }
            if (!isRaining && !isNight)
            {
                switch (carClass)
                {
                    case "family":
                        return (((veh.Speed > (float)(100 / 3.6) ? (float)(110 / 3.6) : (float)(110 / 3.6))));
                    case "truck":
                        return (((veh.Speed > (float)(105 / 3.6) ? (float)(95 / 3.6) : (float)(88 / 3.6))));
                    case "fast":
                        return (float)(140 / 3.6);
                    case "superfast":
                        return (float)(170 / 3.6);
                    case "pickup":
                        return (float)(130 / 3.6);
                    case "nightsuperfast":
                        return (float)(GenerateRandomNumberBetween(150, 178) / 3.6);
                    default:
                        return (((veh.Speed > (float)(100 / 3.6) ? (float)(110 / 3.6) : (float)(110 / 3.6)))); ;
                }
            }
            return (float)(100 / 3.6);
        }

        private static TrafficHoursModel GetTraffic()
        {
            var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
            var m = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
            var s = Function.Call<int>((Hash)0x494E97C2EF27C470);
            TrafficHoursModel model = new TrafficHoursModel();
            bool isMinLeftLaneSpeed = (int)(Math.Round(veh.Speed * 3.6)) > 140 ? true : false;
            if (h > 6 && h <= 18)
            {
                if (isMinLeftLaneSpeed)
                {
                    model.Min = 5000;
                    model.Max = 8000;
                }
                else
                {
                    model.Min = 5000;
                    model.Max = 8000;
                }
            }
            else
            {
                if (isMinLeftLaneSpeed)
                {
                    model.Min = 6000;
                    model.Max = 10000;
                }
                else
                {
                    model.Min = 6000;
                    model.Max = 10000;
                }
            }
            if (h >= 0 && h <= 2)
            {
                maxNumOfSpawnedCars = 4;
                minFake = 3;
                maxFake = 9;
            }
            if (h >= 3 && h <= 5)
            {
                maxNumOfSpawnedCars = 3;
                minFake = 2;
                maxFake = 5;
            }
            if (h == 6)
            {
                maxNumOfSpawnedCars = 7;
                minFake = 8;
                maxFake = 18;
            }
            if (h >= 7 && h < 9)
            {
                maxNumOfSpawnedCars = 14;
                minFake = 27;
                maxFake = 30;
            }
            if (h >= 9 && h < 14)
            {
                maxNumOfSpawnedCars = 12;
                minFake = 13;
                maxFake = 21;
            }
            if (h == 14)
            {
                maxNumOfSpawnedCars = 14;
                minFake = 18;
                maxFake = 25;
            }
            if (h >= 15 && h < 18)
            {
                maxNumOfSpawnedCars = 18;
                minFake = 29;
                maxFake = 30;
            }
            if (h >= 18 && h < 20)
            {
                maxNumOfSpawnedCars = 12;
                minFake = 14;
                maxFake = 27;
            }
            if (h >= 20 && h < 23)
            {
                maxNumOfSpawnedCars = 8;
                minFake = 12;
                maxFake = 22;
            }
            if (h == 23)
            {
                maxNumOfSpawnedCars = 7;
                minFake = 7;
                maxFake = 15;
            }
            if (veh.DisplayName.Contains("vnl") || veh.DisplayName.Contains("ct66"))
            {
                //   if (maxNumOfSpawnedCars >= 10)
                //   maxNumOfSpawnedCars = 8;
                //  else
                ///    maxNumOfSpawnedCars = 4;
            }
            if (CurrentZone.IsOneLaneRoad)
            {
                model.Min = (int)Math.Round((model.Min / 2d), 0);
                model.Max = (int)Math.Round((model.Max / 2d), 0);
            }
            return model;
        }

        private class TrafficHoursModel
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }

        private static int GenerateRandomNumberBetween(int min, int max, bool isLookingRnd = false)
        {
            // using second random generator if function is called from the multiple threads at the same time, to prevent generating the same numbers
            return isLookingRnd ? rnd2.Next(min, max) : r.Next(min, max);
        }

        public class GeneralCar
        {
            public Vehicle Vehicle { get; set; }
            public CarStatsModel Stats { get; set; }
        }
    }
}


/*
   private static void MoveFakeLights()
        {
            if(fakeLightRefPoint != null)
            {
                if (!fakeLightSpawnStopWatch.IsRunning)
                {
                    fakeLightSpawnStopWatch.Start();
                }
                if (fakeLightSpawnStopWatch.ElapsedMilliseconds > 5500 || spawnMoreFakeLightsOnZoneEnter)
                {
                    spawnMoreFakeLightsOnZoneEnter = false;
                    var lightsToSpawn = GenerateRandomNumberBetween(1, 4);
                    var distAhead = 450;
                    for(var fls = 0; fls < lightsToSpawn; fls++)
                    {
                        SpawnFakeLight(distAhead);
                        distAhead -= 56;   
                    }
                    fakeLightSpawnStopWatch = new Stopwatch();
                }
                var distPlayerAndRefPoint = veh.Position.DistanceTo(fakeLightRefPoint.Position);
                if (distPlayerAndRefPoint < 200)
                { 
                    RemoveAllFakeLightsAndRef(); 
                }
                for (var fk = 0; fk < fakeLightList.Count; fk++)
                { 
                    var light = fakeLightList[fk];
                    light.Meters += light.Speed;
                    var pos = fakeLightRefPoint.GetOffsetInWorldCoords(new Vector3(-25, -(light.Meters), 0));
                    var isLightBehindPlayer = veh.Position.DistanceTo(fakeLightRefPoint.Position) < (pos.DistanceTo(fakeLightRefPoint.Position) - 50);
                    if (isLightBehindPlayer)
                    {
                        fakeLightList.RemoveAt(fk);
                    } 
                    else
                    {
                        World.DrawLightWithRange(pos, light.Color, 20f, 4.8f);
                    }
                }
            } 
        } 

        private static void CreateFakeLightsRefPoint()
        {
            var leftBorder = GetClosestPoint(
                               new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                               new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                               new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY)
                               );
            var rightBorder = GetClosestPoint(
                new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY)
            );
            var refPointPos = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = CurrentZone.ZoneBoundary.ZCoord };
            fakeLightRefPoint = World.CreateVehicle(VehicleHash.Panto, refPointPos, CurrentZone.HeadingDirection);
            fakeLightRefPoint.PlaceOnGround();
        }

        private static void SpawnFakeLight(int distAhead = 450)
        {
            if(fakeLightList.Count < maxNumOfSpawnedCars && (isNight || isRaining))
            {
                var distPlayerAndRefPoint = veh.Position.DistanceTo(fakeLightRefPoint.Position);
                var lightMeters = distPlayerAndRefPoint - distAhead;
                var lightColors = new List<Color>()
            {
                Color.FromArgb(255,255,255),
                Color.FromArgb(255, 255, 119),
                Color.FromArgb(86, 137, 252),
                Color.FromArgb(255,255,255),
                Color.FromArgb(255, 255, 119),
                Color.FromArgb(255,255,255),
                Color.FromArgb(255,255,255), 
                Color.FromArgb(255, 255, 119),
            }; 
                var light = new FakeLightModel()
                {
                    Meters = lightMeters,
                    Color = lightColors[GenerateRandomNumberBetween(0, lightColors.Count)],
                    Speed = 1f
                };
                fakeLightList.Add(light);
            }
        }

        private static void RemoveAllFakeLightsAndRef()
        {
            fakeLightList = new List<FakeLightModel>();
            fakeLightRefPoint.MarkAsNoLongerNeeded();
            fakeLightRefPoint.Position = new Vector3(0, 0, 0);
            fakeLightRefPoint = null;
        }*/


/*private static void SteerOnCorner()
    {
        for (var sc = 0; sc < carList.Count; sc++)
        {
            var car = carList[sc];
            if (car.Stats.isInCorner)
            {
                UI.ShowSubtitle("steering");
                Function.Call((Hash)0x42A8EC77D5150CBE, car.Vehicle, -1.0);
            } 
            if(CurrentZone.Name != null)
            {
                var leftBorder = GetClosestPoint(
                           new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                           new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                           new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY)
                          );
                var rightBorder = GetClosestPoint(
                    new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                    new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                    new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY)
                );
                Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = veh.Position.Z };
                var distToEnd = car.Vehicle.Position.DistanceTo(centerPoint);
                if(distToEnd < 10)
                {
                    UI.ShowSubtitle("is in corner");
                    car.Stats.isInCorner = true; 
                }
            }
        }
    }*/
    