using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.Main;
using static HighBeam.AutobahnPropStreamer;
using GTA;
using GTA.Math;
using System.Diagnostics;
using GTA.Native;

namespace HighBeam
{
    public class NyWarehouse
    {
        private static int currentHour;
        private static bool isWarehouse = false;
        private static Vector3 warehousePos = new Vector3(5911.462f, -3215.70337f, 5.796047f);

        public static void RunNyWarehouse()
        {
            //  UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
            try
            {
                currentHour = fakeTimeHours;

                if (Game.Player.Character.Position.DistanceTo(warehousePos) < 100f && !isWarehouse)
                {
                    UI.Notify("Ny Warehouse loaded");
                    isWarehouse = true;
                    LoadAllProps();
                }
                if (Game.Player.Character.Position.DistanceTo(warehousePos) > 120f && isWarehouse)
                {
                    UI.Notify("Ny Warehouse removed");
                    isWarehouse = false;
                    RemoveAllProps();
                }
                if (isWarehouse)
                {
                    if (currentHour >= 17 || currentHour < 6)
                    {

                    }
                    CheckBarriers();
                }
            }
            catch (Exception e)
            {
                UI.Notify(e.Message);
            }
        }

        private static void LoadAllProps()
        {
            LoadNyWare();
            LoadBarriers();
            LoadVehicles();
            LoadPeds();
        }

        private static void RemoveAllProps()
        {
            RemoveNyWare();
            RemoveBarriers();
            RemoveVehicles();
            RemovePeds();
        }

        private static List<VehicleHash> vehicleHashes = new List<VehicleHash>() {
            VehicleHash.Minivan, VehicleHash.Asea, VehicleHash.Asterope, VehicleHash.Panto, VehicleHash.Primo, VehicleHash.Mesa, VehicleHash.Oracle, VehicleHash.Serrano, VehicleHash.Washington,
            VehicleHash.Baller2, VehicleHash.BJXL, VehicleHash.Dilettante, VehicleHash.Futo
        };
        private static List<Vehicle> vehicles = new List<Vehicle>();
        private static Random rnd = new Random();
        private static Random rnd2 = new Random();
        private static Random rnd4 = new Random();

        private static List<Ped> warehousePeds = new List<Ped>();

        public static List<PedHash> pedHashesPossible = new List<PedHash>() { PedHash.AirworkerSMY, PedHash.Dockwork01SMY, PedHash.Dockwork01SMM, PedHash.Ups01SMM, PedHash.Ups02SMM };

        public static void LoadPeds()
        {
            if (currentHour > 5 && currentHour < 17)
            {
                for (var i = 0; i < 0; ++i)
                {
                    var ped = World.CreatePed(pedHashesPossible[rnd4.Next(0, pedHashesPossible.Count - 1)], exitBarrier.GetOffsetInWorldCoords(new Vector3(0, 10f, 0)));
                    ped.Task.WanderAround(warehousePos, 20f);
                    ped.IsInvincible = true;
                    ped.Health = 9999;
                    ped.AlwaysKeepTask = true;
                    warehousePeds.Add(ped);
                }
                for (var i = 0; i < 3; ++i)
                {
                    var ped = World.CreatePed(pedHashesPossible[rnd4.Next(0, pedHashesPossible.Count - 1)], exitBarrier.GetOffsetInWorldCoords(new Vector3(0, 40f, 0)));
                    ped.Task.WanderAround(exitBarrier.GetOffsetInWorldCoords(new Vector3(3f, 37f, 0)), 40f);
                    ped.IsInvincible = true;
                    ped.Health = 9999;
                    ped.AlwaysKeepTask = true;
                    warehousePeds.Add(ped);
                }
            }

        }

        public static void RemovePeds()
        {
            for (var i = 0; i < warehousePeds.Count; ++i)
            {
                warehousePeds[i].Delete();
            }
            warehousePeds = new List<Ped>();

        }

        private static void LoadVehicles()
        {
            if (currentHour >= 6 && currentHour < 17)
            {
                if (vehicles.Count <= 0)
                {
                    var pos = new Vector3(5898.7f, -3212.9f, 4.9f);
                    for (var i = 0; i < 6; ++i)
                    {
                        var mod = vehicleHashes[rnd.Next(0, vehicleHashes.Count - 1)];
                        var v = World.CreateVehicle(new Model(mod), pos, 14.1f);
                        pos = v.GetOffsetInWorldCoords(new Vector3(3.2f, 0, 0));
                        if (rnd2.Next(0, 10) % 4 == 0)
                            v.Delete();
                        else
                            vehicles.Add(v);
                    }
                    vehicles.Shuffle();
                }
            }
        }

        private static void RemoveVehicles()
        {
            if (vehicles.Count > 0)
            {
                for (var i = 0; i < vehicles.Count; ++i)
                {
                    vehicles[i].Delete();
                }
                vehicles = new List<Vehicle>();
            }
        }

        private static Prop enterBarrier = null;
        private static Prop exitBarrier = null;
        private static Stopwatch barrierCheckStopwatch = new Stopwatch();
        private static Stopwatch enterBarrierMoveStopwatch = new Stopwatch();
        private static Stopwatch exitBarrierMoveStopwatch = new Stopwatch();
        private static Stopwatch enterBarrierCloseStopwatch = new Stopwatch();
        private static Stopwatch exitBarrierCloseStopwatch = new Stopwatch();
        private static bool isEnterBarrierOpen = false;
        private static bool isExitBarrierOpen = false;

        private static float enterY = -5.0089543E-06f;
        private static float exitY = -5.0089543E-06f;

        public static void LoadBarriers()
        {
            enterBarrier = World.CreateProp(
                new Model(1230099731),
                new Vector3(5911.462f, -3215.70337f, 5.796047f),
                new Vector3(1.00179113E-05f, -5.008955E-06f, -75.29986f),
                false, false);
            exitBarrier = World.CreateProp(
                new Model(1230099731),
                new Vector3(5913.86f, -3225.194f, 5.79f),
                new Vector3(1.00178959E-05f, 5.008939E-06f, 104.699867f),
                false, false);
            enterBarrier.FreezePosition = true;
            exitBarrier.FreezePosition = true;
            enterBarrier.HasCollision = false;
            exitBarrier.HasCollision = false;
        }

        public static void RemoveBarriers()
        {
            enterY = -5.0089543E-06f;
            exitY = -5.0089543E-06f;
            enterBarrier.Delete();
            exitBarrier.Delete();
            enterBarrier = null;
            exitBarrier = null;
        }

        public static void CheckBarriers()
        {
            if (!barrierCheckStopwatch.IsRunning)
                barrierCheckStopwatch.Start();
            if (barrierCheckStopwatch.ElapsedMilliseconds > 700)
            {
                barrierCheckStopwatch = new Stopwatch();
                var enterCheckPos = enterBarrier.GetOffsetInWorldCoords(new Vector3(2f, 7f, 0));
                var vehAtEnter = World.GetNearbyVehicles(enterCheckPos, 8f).FirstOrDefault();

                var exitCheckPos = exitBarrier.GetOffsetInWorldCoords(new Vector3(2f, 6f, 0));
                var vehAtExit = World.GetNearbyVehicles(exitCheckPos, 6f).FirstOrDefault();
                if (vehAtEnter?.DisplayName != null && isEnterBarrierOpen && !enterBarrierMoveStopwatch.IsRunning)
                {
                    enterBarrierCloseStopwatch = new Stopwatch();
                    enterBarrierCloseStopwatch.Start();
                }

                if (vehAtExit?.DisplayName != null && isExitBarrierOpen && !exitBarrierMoveStopwatch.IsRunning)
                {
                    exitBarrierCloseStopwatch = new Stopwatch();
                    exitBarrierCloseStopwatch.Start();
                }

                if (vehAtEnter?.DisplayName != null && !isEnterBarrierOpen && !enterBarrierMoveStopwatch.IsRunning && !enterBarrierCloseStopwatch.IsRunning
                    && vehAtEnter.Heading < 133f && vehAtEnter.Heading > 78f)
                {
                    enterBarrierMoveStopwatch.Start();
                    if (vehAtEnter.ClassType == VehicleClass.Commercial || vehAtEnter.MaxBraking < 0.25)
                    {
                        exitBarrierMoveStopwatch.Start();
                    }
                }
                if (vehAtExit?.DisplayName != null && !isExitBarrierOpen && !exitBarrierMoveStopwatch.IsRunning && !exitBarrierCloseStopwatch.IsRunning
                    && vehAtExit.Heading > 255f && vehAtExit.Heading < 304f)
                {
                    exitBarrierMoveStopwatch.Start();
                    if (vehAtExit.ClassType == VehicleClass.Commercial || vehAtExit.MaxBraking < 0.25)
                    {
                        enterBarrierMoveStopwatch.Start();
                    }
                }
            }

            if (enterBarrierMoveStopwatch.IsRunning)
            {
                if (!isEnterBarrierOpen)
                {
                    enterY -= 0.25f;
                    if (enterY <= -15f)
                    {
                        enterY = -15f;
                        enterBarrierMoveStopwatch = new Stopwatch();
                        isEnterBarrierOpen = true;
                        enterBarrierCloseStopwatch.Start();
                    }
                }
                else
                {
                    enterY += 0.25f;
                    if (enterY >= 0f)
                    {
                        enterY = -5.0089543E-06f;
                        enterBarrierMoveStopwatch = new Stopwatch();
                        isEnterBarrierOpen = false;
                    }
                }
            }
            enterBarrier.Rotation = new Vector3(enterBarrier.Rotation.X, enterY, enterBarrier.Rotation.Z);
            if (isEnterBarrierOpen && enterBarrierCloseStopwatch.ElapsedMilliseconds > 10000)
            {
                enterBarrierMoveStopwatch.Start();
                enterBarrierCloseStopwatch = new Stopwatch();
            }


            if (exitBarrierMoveStopwatch.IsRunning)
            {
                if (!isExitBarrierOpen)
                {
                    exitY -= 0.25f;
                    if (exitY <= -10f)
                    {
                        exitY = -10f;
                        exitBarrierMoveStopwatch = new Stopwatch();
                        isExitBarrierOpen = true;
                        exitBarrierCloseStopwatch.Start();
                    }
                }
                else
                {
                    exitY += 0.25f;
                    if (exitY >= 0f)
                    {
                        exitY = -5.0089543E-06f;
                        exitBarrierMoveStopwatch = new Stopwatch();
                        isExitBarrierOpen = false;
                    }
                }
            }
            exitBarrier.Rotation = new Vector3(exitBarrier.Rotation.X, exitY, exitBarrier.Rotation.Z);
            if (isExitBarrierOpen && exitBarrierCloseStopwatch.ElapsedMilliseconds > 10000)
            {
                exitBarrierMoveStopwatch.Start();
                exitBarrierCloseStopwatch = new Stopwatch();
            }
        }
    }
}
