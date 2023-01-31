﻿using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.Main;
using static HighBeam.AutobahnPropStreamer;
using static HighBeam.Train;

namespace HighBeam
{
    public static class Warehouse
    {
        public static bool init = false;
        public static bool isWarehouse = false;
        //gate 
        public static Prop gate = null;
        public static Prop garage = null;
        public static Stopwatch gateMovingStopwatch = new Stopwatch();
        public static bool isGateOpen = false;
        public static bool isGateMoving = false;
        public static float gateX = 0;
        public static Stopwatch garageMovingStopwatch = new Stopwatch();
        public static bool isGarageOpen = false;
        public static bool isGarageMoving = false;
        public static float garageZ = 0;
        public static bool isPropsDeleted = false;
        //paletes
        public static List<Prop> palletes = new List<Prop>();
        public static int palletesSupplyCount = 0;
        public static bool supplyMode;
        //truck
        public static Vehicle truck = null;
        public static Vehicle trailer = null;
        public static Ped truckDriver = null;
        public static Stopwatch t = new Stopwatch();
        public static Stopwatch readyToLeaveStopwatch = new Stopwatch();
        public static Vector3 truckSpawnPoint = new Vector3();
        public static Vector3 truckDriveToPos = new Vector3();
        public static string truckState = "enter";
        public static Stopwatch timeToNextTruckStopwatch = new Stopwatch();
        public static Random rnd = new Random();
        public static Random rnd2 = new Random();
        public static Random rnd3 = new Random();
        public static Random rnd4 = new Random();
        public static Random rnd5 = new Random();
        public static int timeToNextTruck = rnd.Next(67000, 130000);
        public static List<Prop> trailerContent = new List<Prop>();
        public static Stopwatch lastSetTruckDriverTaskStopwatch = new Stopwatch();
        public static Stopwatch zeroSpeedTruckStopwatch = new Stopwatch();
        public static int activeCompanyCar = 1002;
        public static bool isPrivateCar = false;
        public static Stopwatch repeatRemovingPropsStopwatch = new Stopwatch();


        public static int currentHour = 0;
        public static Stopwatch setPedTaskStopwatch = new Stopwatch();
        //1152297372
        public static List<int> propsToRemoveHashes = new List<int>()
        {
            -143315610, -741944541, -1098506160, 682074297, -1775229459, 1723816705, 2111998691, -57215983,
            -1901227524, 161075092, 533342826, 136645433, -310198185, 63237339, -1036807324, -296249014,
            386059801,772023703, -2003545603
        };
        public static Vector3 warehousePos = new Vector3(150.0f, 6430.44f, 31.2f);
        public static List<Ped> warehousePeds = new List<Ped>();
        public static List<Ped> warehousePedsFactory = new List<Ped>();

        public static Stopwatch pedsDriveToHomeStopwatch = new Stopwatch();
        public static List<Ped> pedsDrivingToHome = new List<Ped>();
        public static List<PedHash> pedHashesPossible = new List<PedHash>() { PedHash.AirworkerSMY, PedHash.Dockwork01SMY, PedHash.Dockwork01SMM, PedHash.Ups01SMM, PedHash.Ups02SMM };
        public static List<VehicleHash> vehicleHashes = new List<VehicleHash>() {
            VehicleHash.Minivan, VehicleHash.Asea, VehicleHash.Asterope, VehicleHash.Panto, VehicleHash.Primo, VehicleHash.Mesa, VehicleHash.Oracle, VehicleHash.Serrano, VehicleHash.Washington,
            VehicleHash.Baller2, VehicleHash.BJXL, VehicleHash.Dilettante, VehicleHash.Futo
        };
        public static List<Vehicle> vehicles = new List<Vehicle>();
        public static List<Vehicle> companyVehicles = new List<Vehicle>();
        public static int gatesState = 0;

        private static Prop locomotive = null;
        private static List<Prop> trainFreightList = new List<Prop>();
        private static int currentFreight = 0;
        private static int currentStartPalleteIndex = 0;
        private static bool isTrainMoving = false;
        private static float trainSpeed = 0f;
        private static Vector3 rampPos = new Vector3(212.81f, 6374.2f, 31.67f);
        private static List<Prop> trainPalletes = new List<Prop>();
        private static Stopwatch removeTrainStopwatch = new Stopwatch();


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
        private static int timeToNext = rnd.Next(15000, 31000);


        private static List<Prop> shelfList = new List<Prop>();
        private static void LoadShelf()
        {
            var shelfRef = World.CreateProp(new Model(-1902543747), new Vector3(143.1985f, 6474.382f, 30.7633667f), new Vector3(0, 0, -44.99991f), false, false);
            shelfRef.FreezePosition = true;
            shelfRef.HasCollision = false;
            var shelfOffset = new Vector3(4f, 0, 0);
            shelfList.Add(shelfRef);
            for (var i = 0; i < 18; ++i)
            {
                if (i != 2)
                {
                    var shelf = World.CreateProp(new Model(-1902543747), shelfRef.GetOffsetInWorldCoords(shelfOffset), new Vector3(0, 0, -44.99991f), false, false);
                    shelf.HasCollision = false;
                    shelf.FreezePosition = true;
                    var count = i + 1;
                    if (count % 3 == 0)
                    {
                        if (count == 15)
                        {
                            shelfOffset = new Vector3(0, shelfOffset.Y - 7f, 0);
                        }
                        else
                        {
                            shelfOffset = new Vector3(0, shelfOffset.Y - 11f, 0);
                        }
                    }
                    else
                    {
                        shelfOffset = new Vector3(shelfOffset.X + 4f, shelfOffset.Y, 0);
                    }
                    shelfList.Add(shelf);

                    var palleteRef = new Vector3(-1.2f, 1f, 0);
                    var realPosCount = 0;
                    for (var ii = 0; ii < 24; ++ii)
                    {
                        var pallete = GetPallete(shelf, palleteRef);
                        if (palletes.Count > 0)
                        {
                            for (var cc = 0; i < cc; ++cc)
                            {
                                palletes[cc].SetNoCollision(pallete, true);
                            }
                        }
                        palletes.Add(pallete);
                        var ccount = ii + 1;
                        realPosCount++;
                        if (realPosCount == 1 || realPosCount == 3)
                        {
                            palleteRef = new Vector3(palleteRef.X + 2.1f, palleteRef.Y, palleteRef.Z);
                        }
                        if (realPosCount == 2)
                        {
                            palleteRef = new Vector3(-1.2f, palleteRef.Y - 2f, palleteRef.Z);
                        }
                        if (ccount % 4 == 0)
                        {
                            palleteRef = new Vector3(-1.2f, 1f, palleteRef.Z + 1.77f);
                            realPosCount = 0;
                        }
                    }
                }
                else
                {
                    shelfOffset = new Vector3(0, shelfOffset.Y - 11f, 0);
                }

            }
        }

        private static Random palleteTypeRnd = new Random();

        private class PalleteModel
        {
            public int Hash { get; set; }
            public float Heading { get; set; }
            public bool HasCollision { get; set; }
        }

        private static List<PalleteModel> palleteTypes = new List<PalleteModel>()
        {
            new PalleteModel(){ Hash = 153748523, Heading = 45f, HasCollision = true},
            new PalleteModel(){ Hash = 1524671283, Heading = 45f, HasCollision = false},
            new PalleteModel(){ Hash = 1576342596, Heading = 45f, HasCollision = false},
            new PalleteModel(){ Hash = 300547451, Heading = 45f, HasCollision = false}
        };
        private static Prop GetPallete(Prop shelf, Vector3 palleteRef)
        {
            var model = palleteTypes[palleteTypeRnd.Next(0, palleteTypes.Count - 1)];
            var pallete = World.CreateProp(new Model(model.Hash), shelf.GetOffsetInWorldCoords(palleteRef), true, false);
            pallete.Heading = model.Heading;
            pallete.HasCollision = model.HasCollision;
            return pallete;
        }

        private static void RemoveShelf()
        {
            shelfList.ForEach(s => s.Delete());
            shelfList = new List<Prop>();
        }



        public class GarageDoorModel
        {
            public Prop Door { get; set; }
            public Model DoorModel { get; set; }
            public Vector3 DoorPos { get; set; }
            public Vector3 DoorRot { get; set; }
            public float DoorZ { get; set; }
            public Vector3 StateLightOffset { get; set; }
            public Vector3 StateLightPos { get; set; }
            public bool IsOpen { get; set; }
            public List<Prop> StateLigts { get; set; }
        }

        private static List<GarageDoorModel> garageDoors = new List<GarageDoorModel>()
        {
            new GarageDoorModel(){
                DoorModel = new Model(-190780785),
                DoorPos = new Vector3(125.306358f, 6425.105f, 31.87f),
                DoorRot = new Vector3(0, 0, 44.99984f),
                DoorZ = 0,
                StateLightOffset = new Vector3(0, 0.56f, 1.8f),
                StateLigts = new List<Prop>()
            },
            new GarageDoorModel(){
                DoorModel = new Model(-190780785),
                DoorPos = new Vector3(122.189171f, 6421.985f, 31.87f),
                DoorRot = new Vector3(0, 0, 44.99984f),
                DoorZ = 0,
                  StateLightOffset = new Vector3(0, 0.56f, 1.8f),
                StateLigts = new List<Prop>()
            },
            new GarageDoorModel(){
                DoorModel = new Model(-190780785),
                DoorPos = new Vector3(128.420914f, 6428.29443f, 31.87f),
                DoorRot = new Vector3(0, 0, 44.99984f),
                DoorZ = 0,
               StateLightOffset = new Vector3(0, 0.56f, 1.8f),
                StateLigts = new List<Prop>()
            },
               new GarageDoorModel(){
                DoorModel = new Model(-1212944997),
                DoorPos = new Vector3(139.887527f, 6439.69971f, 30.5689468f),
                DoorRot = new Vector3(1.00179086E-05f, -5.008955E-06f, 45.899807f),
                DoorZ = 0,
                StateLightOffset = new Vector3(6f, 0.3f, 2.4f),
                StateLigts = new List<Prop>()
            }
        };

        private static Stopwatch manageGarageDoorsStopwatch = new Stopwatch();
        private static GarageDoorModel currentGarageDoor = null;
        private static int redBulbHash = 754816039;
        private static int yellowBulbHash = -1384835816;
        private static int greenBulbHash = 140790497;

        public static void ManageGarageDoors()
        {
            if (manageGarageDoorsStopwatch.IsRunning && currentGarageDoor != null)
            {
                var speed = 0.005f;
                currentGarageDoor.DoorZ = currentGarageDoor.IsOpen ? currentGarageDoor.DoorZ + speed : currentGarageDoor.DoorZ - speed;
                currentGarageDoor.Door.Position = currentGarageDoor.Door.GetOffsetInWorldCoords(new Vector3(0, 0, currentGarageDoor.IsOpen ? speed : -speed));
                var toOpen = currentGarageDoor.DoorZ >= 3.3f;
                var toClose = currentGarageDoor.DoorZ <= 0;
                if ((currentGarageDoor.IsOpen && toOpen) || (!currentGarageDoor.IsOpen && toClose))
                {
                    if (toOpen)
                    {
                        currentGarageDoor.StateLigts[1].Position = new Vector3(0, 0, 0);
                        currentGarageDoor.StateLigts[2].Position = currentGarageDoor.StateLightPos;
                    }

                    if (toClose)
                    {
                        currentGarageDoor.StateLigts[1].Position = new Vector3(0, 0, 0);
                        currentGarageDoor.StateLigts[0].Position = currentGarageDoor.StateLightPos;
                    }

                    manageGarageDoorsStopwatch = new Stopwatch();
                    currentGarageDoor = null;
                }
            }
            if (Game.IsControlJustPressed(0, Control.Sprint))
            {
                for (var i = 0; i < garageDoors.Count; ++i)
                {
                    if (garageDoors[i].DoorPos.DistanceTo(Game.Player.Character.Position) < 2f)
                    {
                        garageDoors[i].IsOpen = !garageDoors[i].IsOpen;
                        garageDoors[i].StateLigts[0].Position = new Vector3(0, 0, 0);
                        garageDoors[i].StateLigts[2].Position = new Vector3(0, 0, 0);
                        garageDoors[i].StateLigts[1].Position = garageDoors[i].StateLightPos;
                        currentGarageDoor = garageDoors[i];
                        manageGarageDoorsStopwatch.Start();
                        break;
                    }
                }
            }
        }

        public static void LoadGarageDoors()
        {
            garageDoors.ForEach(gd =>
            {
                gd.Door = World.CreateProp(gd.DoorModel, gd.DoorPos, gd.DoorRot, false, false);
                gd.Door.FreezePosition = true;
                for (var i = 0; i < 3; ++i)
                {
                    gd.StateLightPos = gd.Door.GetOffsetInWorldCoords(gd.StateLightOffset);
                    var hash = redBulbHash;
                    if (i == 1)
                        hash = yellowBulbHash;
                    if (i == 2)
                        hash = greenBulbHash;
                    var bulb = World.CreateProp(new Model(hash), gd.StateLightPos, false, false);
                    bulb.Rotation = new Vector3(0, 90f, gd.Door.Rotation.Z - 270f);
                    bulb.FreezePosition = true;
                    bulb.Position = new Vector3(0, 0, 0);
                    if (i == 0)
                    {
                        bulb.Position = gd.StateLightPos;
                    }
                    gd.StateLigts.Add(bulb);
                }
            });
        }

        public static void RemoveGarageDoors()
        {
            garageDoors.ForEach(gd =>
            {
                gd.Door.Delete();
                gd.DoorZ = 0f;
                gd.StateLigts.ForEach(sl => sl.Delete());
                gd.StateLigts = new List<Prop>();
            });
            currentGarageDoor = null;
        }

        public static void LoadBarriers()
        {
            enterBarrier = World.CreateProp(
                new Model(1230099731),
                new Vector3(130.706009f, 6386.843f, 31.26314f),
                new Vector3(1.00179022E-05f, -5.0089543E-06f, 133.7498f),
                false, false);
            exitBarrier = World.CreateProp(
                new Model(1230099731),
                new Vector3(124.01f, 6394.02246f, 31.25f),
                new Vector3(1.00178877E-05f, 5.00895567E-06f, -46.7747154f),
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
                var enterCheckPos = enterBarrier.GetOffsetInWorldCoords(new Vector3(10f, 12f, 0));
                var vehAtEnter = World.GetNearbyVehicles(enterCheckPos, 8f).FirstOrDefault();

                var exitCheckPos = exitBarrier.GetOffsetInWorldCoords(new Vector3(2f, 14f, 0));
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
                    && vehAtEnter.Heading < 316f && vehAtEnter.Heading > 194f)
                {
                    enterBarrierMoveStopwatch.Start();
                    if (vehAtEnter.ClassType == VehicleClass.Commercial || vehAtEnter.MaxBraking < 0.25)
                    {
                        exitBarrierMoveStopwatch.Start();
                    }
                }
                if (vehAtExit?.DisplayName != null && !isExitBarrierOpen && !exitBarrierMoveStopwatch.IsRunning && !exitBarrierCloseStopwatch.IsRunning
                    && vehAtExit.Heading > 100f && vehAtExit.Heading < 160f)
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
                    if (enterY <= -39f)
                    {
                        enterY = -39f;
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
                    if (exitY <= -39f)
                    {
                        exitY = -39f;
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

        public static void LoadSupplyTrain()
        {
            locomotive = World.CreateProp(
                new Model(1030400667),
                new Vector3(190.6646f, 6361.853f, 30.74f),
                new Vector3(1.00179113E-05f, 5.00895567E-06f, -60.7497635f),
                false,
                false);
            var boxOff = 18.4f;
            locomotive.Position = locomotive.GetOffsetInWorldCoords(new Vector3(0, -30f, 0));
            for (var i = 0; i < 6; ++i)
            {
                var box = World.CreateProp(new Model(184361638), new Vector3(), false, false);
                box.AttachTo(locomotive, 0, new Vector3(0, -boxOff, -0.8f), new Vector3());
                var cont = World.CreateProp(new Model(1525186387), new Vector3(), false, false);
                cont.AttachTo(box, 0, new Vector3(0, 0, -1.62f), new Vector3());
                cont.HasCollision = true;
                box.HasCollision = true;
                trainFreightList.Add(box);
                trainFreightList.Add(cont);


                var palOff = 5f;
                for (var ii = 0; ii < 6; ++ii)
                {
                    var pallet = World.CreateProp(new Model(-1951226014), new Vector3(), true, false);
                    pallet.AttachTo(box, 0, new Vector3(0, palOff, 1.2f), new Vector3(0, 0, 90f));
                    pallet.HasCollision = true;
                    trainPalletes.Add(pallet);
                    palOff -= 1.94f;
                }

                boxOff += 18.4f;
            }
            isTrainMoving = true;
            string[] l = { "71" };
            System.IO.File.WriteAllLines(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\warehouse_config.txt", l);
            UI.Notify("Supply train is coming");

            var rampPos = new Vector3(199.83f, 6377.2f, 33.63f);
            var ped = World.CreatePed(pedHashesPossible[rnd4.Next(0, pedHashesPossible.Count - 1)], rampPos);
            ped.Heading = 204f;
            ped.Task.StartScenario("WORLD_HUMAN_CLIPBOARD", rampPos);
            ped.IsInvincible = true;
            ped.Health = 9999;
            warehousePeds.Add(ped);
        }

        private static void MoveFreightToRamp()
        {
            var maxTrainSpeed = 0.016f;
            var currentFreightProp = trainFreightList[currentFreight];
            if (currentFreightProp.Position.DistanceTo(rampPos) > 5.3f)
            {
                if (trainSpeed < maxTrainSpeed)
                {
                    trainSpeed += 0.00008f;
                }
            }
            else
            {
                if (trainSpeed > 0)
                {
                    trainSpeed -= 0.0001f;
                }
                else
                {
                    trainSpeed = 0;
                    trainPalletes.GetRange(currentStartPalleteIndex, 6).ForEach(p => p.Detach());
                    currentStartPalleteIndex += 6;
                    isTrainMoving = false;
                }
            }
            locomotive.Position = locomotive.GetOffsetInWorldCoords(new Vector3(0, trainSpeed, 0));
        }

        public static void MonitorPalletes()
        {
            var currentFreightProp = trainFreightList[currentFreight];
            var num = World.GetNearbyProps(currentFreightProp.Position, 6f, new Model(-1951226014)).Length;
            if (num <= 0)
            {
                currentFreight += 2;
                if (currentFreight >= (trainFreightList.Count - 1))
                {
                    currentFreight = 0;
                    removeTrainStopwatch.Start();
                }
                isTrainMoving = true;
            }
        }

        public static void ManageSupplyTrain()
        {
            if (trainFreightList.Count > 0)
            {
                if (isTrainMoving)
                {
                    MoveFreightToRamp();
                }

                if (!isTrainMoving)
                {
                    MonitorPalletes();
                }
                if (removeTrainStopwatch.ElapsedMilliseconds > 22000)
                {
                    RemoveSupplyTrain();
                    removeTrainStopwatch = new Stopwatch();
                }
            }
        }


        public static void RemoveSupplyTrain()
        {
            if (locomotive != null)
            {
                for (var i = 0; i < trainFreightList.Count; ++i)
                {
                    trainFreightList[i].Delete();
                }
                trainFreightList = new List<Prop>();
                locomotive.Delete();
                locomotive = null;
                currentFreight = 0;
                currentStartPalleteIndex = 0;

                warehousePeds.Last().Task.WanderAround(warehousePos, 45f);
            }
        }


        public static void RunWarehouse()
        {
            // UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
            try
            {
                //  Function.Call((Hash)0xD4B8E3D1917BC86B, true);
                currentHour = fakeTimeHours;
                //   UI.ShowSubtitle(veh.Model.Hash.ToString());
                if (!init && false)
                {
                    init = true;
                    isWarehouse = true;
                    LoadAllProps();
                }
                if (Game.IsControlJustPressed(0, Control.Jump) && false)
                {
                    RemoveWare();
                    RemoveAllProps();
                    gatesState = 0;
                    isPropsDeleted = false;
                    RemoveSupplyTrain();
                }

                //  if (Game.Player.Character.Position.DistanceTo(warehousePos) < 250f && !isWarehouse && !isInTrain && !isOnAutobahn)
                if (veh.Position.DistanceTo(warehousePos) < 250f && !isWarehouse && !isInTrain && !isOnAutobahn)
                {
                    UI.Notify("Warehouse loaded");
                    LoadWare();
                    isWarehouse = true;
                    var vehs = World.GetNearbyVehicles(warehousePos, 69f);
                    for (var i = 0; i < vehs.Length; ++i)
                    {
                        if (vehs[i].DashboardColor != VehicleColor.EpsilonBlue && !vehs[i].DisplayName.ToLower().Contains("porcaygt"))
                            vehs[i].Delete();
                    }
                    LoadAllProps();
                    if (currentHour >= 6 && currentHour <= 16)
                    {
                        garageDoors[3].IsOpen = !garageDoors[3].IsOpen;
                        currentGarageDoor = garageDoors[3];
                        manageGarageDoorsStopwatch.Start();
                    }
                }
                if (Game.Player.Character.Position.DistanceTo(warehousePos) > 280f && isWarehouse)
                {
                    UI.Notify("Warehouse removed");
                    RemoveWare();
                    RemoveAllProps();

                    gatesState = 0;
                    isPropsDeleted = false;
                }
                if (isWarehouse)
                {
                    // Function.Call((Hash)0xD4B8E3D1917BC86B, true);
                    ManageSupplyTrain();
                    ManageGarageDoors();
                    if (!isPropsDeleted && Game.Player.Character.Position.DistanceTo(warehousePos) < 54f)
                    {
                        for (var ii = 0; ii < propsToRemoveHashes.Count; ++ii)
                        {
                            var props = World.GetNearbyProps(warehousePos, 320f, new Model(propsToRemoveHashes[ii]));
                            for (var i = 0; i < props.Length; ++i)
                            {
                                props[i].Position = new Vector3(0, 0, 0);
                                props[i].Delete();
                            }
                        }
                        repeatRemovingPropsStopwatch.Start();
                        var tr = World.GetAllVehicles(new Model(VehicleHash.Trailers2));
                        for (var i = 0; i < tr.Length; ++i)
                        {
                            tr[i].Delete();
                        }
                        isPropsDeleted = true;
                    }
                    if (isPropsDeleted && repeatRemovingPropsStopwatch.ElapsedMilliseconds > 2000)
                    {
                        repeatRemovingPropsStopwatch = new Stopwatch();
                        for (var ii = 0; ii < propsToRemoveHashes.Count; ++ii)
                        {
                            var props = World.GetNearbyProps(warehousePos, 120f, new Model(propsToRemoveHashes[ii]));
                            for (var i = 0; i < props.Length; ++i)
                            {
                                props[i].Position = new Vector3(0, 0, 0);
                                props[i].Delete();
                            }
                        }
                    }
                    LoadVehicles();
                    if (Game.Player.Character.Position.DistanceTo(new Vector3(125.99f, 6396.54f, 31.53f)) <= 1f)
                    {
                        // SupplyMode();
                        if (locomotive == null)
                            LoadSupplyTrain();
                    }
                    if (currentHour >= 17 || currentHour < 6)
                    {
                        // UI.ShowSubtitle(vehicles.Count + " " + pedsDriveToHomeStopwatch.IsRunning.ToString());

                        if (warehousePeds.Count > 0)
                            RemovePeds();
                        if (vehicles.Count > 0 && !pedsDriveToHomeStopwatch.IsRunning)
                        {
                            pedsDriveToHomeStopwatch.Start();
                        }

                        if (pedsDriveToHomeStopwatch.ElapsedMilliseconds > timeToNext && pedsDriveToHomeStopwatch.IsRunning)
                        {
                            timeToNext = rnd.Next(10000, 15000);
                            pedsDriveToHomeStopwatch = new Stopwatch();
                            var anys = false;
                            for (var i = 0; i < vehicles.Count; ++i)
                            {
                                var v = vehicles[i];
                                if (v.IsSeatFree(VehicleSeat.Driver))
                                {
                                    v.Heading = (v.Heading + 180) % 360;
                                    v.LightsOn = true;
                                    anys = true;
                                    var ped = v.CreatePedOnSeat(VehicleSeat.Driver, new Model(pedHashesPossible[0]));
                                    ped.SetIntoVehicle(v, VehicleSeat.Driver);
                                    var seq1 = new TaskSequence();
                                    if (v.Position.DistanceTo(exitBarrier.Position) > 50)
                                    {
                                        seq1.AddTask.DriveTo(v, v.GetOffsetInWorldCoords(new Vector3(0, 4f, 0)), 0.7f, 1f);
                                        seq1.AddTask.DriveTo(v, exitBarrier.GetOffsetInWorldCoords(new Vector3(2f, 30f, 0)), 4f, 4f);
                                        seq1.AddTask.DriveTo(v, exitBarrier.GetOffsetInWorldCoords(new Vector3(2f, 23f, 0)), 4f, 4f);
                                        seq1.AddTask.DriveTo(v, exitBarrier.GetOffsetInWorldCoords(new Vector3(2f, 5f, 0)), 4f, 4f);
                                        seq1.AddTask.DriveTo(v, exitBarrier.GetOffsetInWorldCoords(new Vector3(2f, -5f, 0)), 4f, 4f);
                                    }
                                    seq1.AddTask.DriveTo(v, truckSpawnPoint, 4f, 4f);
                                    seq1.AddTask.DriveTo(v, new Vector3(29.2f, 6647.8f, 31.3f), 2f, 9f);
                                    seq1.Close();
                                    ped.AlwaysKeepTask = true;
                                    ped.Task.PerformSequence(seq1);
                                    seq1.Dispose();
                                    pedsDrivingToHome.Add(ped);
                                    break;
                                }
                            }
                            if (!anys)
                            {
                                pedsDriveToHomeStopwatch = new Stopwatch();
                            }
                        }
                    }
                    if (isGateMoving)
                    {
                        var speed = 0.01f;
                        gateX = isGateOpen ? gateX + speed : gateX - speed;
                        gate.Position = gate.GetOffsetInWorldCoords(new Vector3(isGateOpen ? speed : -speed, 0, 0));
                        if ((isGateOpen && gateX >= 7.6) || (!isGateOpen && gateX <= 0))
                        {
                            isGateMoving = false;
                        }
                    }
                    if (isGarageMoving)
                    {
                        var speed = 0.003f;
                        garageZ = isGarageOpen ? garageZ + speed : garageZ - speed;
                        garage.Position = garage.GetOffsetInWorldCoords(new Vector3(0, 0, isGarageOpen ? speed : -speed));
                        if ((isGarageOpen && garageZ >= 5.7f) || (!isGarageOpen && garageZ <= 0))
                        {
                            isGarageMoving = false;
                        }
                    }
                    if (t.ElapsedMilliseconds > 34000 && false)
                    {
                        var pos = new Vector3(95.33f, 6403.54f, 31.0f);
                        truckDriver.Task.ClearAllImmediately();
                        truckDriver.SetIntoVehicle(truck, VehicleSeat.Driver);
                        Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, truckDriver, truck, pos.X, pos.Y, 31.3f, 5f, 1f, truck.GetHashCode(), 6, 1f, true);
                        t = new Stopwatch();
                    }
                    if (!setPedTaskStopwatch.IsRunning)
                    {
                        setPedTaskStopwatch.Start();
                    }
                    if (setPedTaskStopwatch.ElapsedMilliseconds > 30000)
                    {
                        setPedTaskStopwatch = new Stopwatch();
                        SetPedTasks();
                    }
                    MoveTruck();
                    CarWash();
                    CheckBarriers();
                }
            }
            catch (Exception e)
            {
                UI.Notify(e.Message);
            }
        }

        private static Stopwatch carWashMoveStopwatch = new Stopwatch();
        private static Stopwatch toggleWashStopwatch = new Stopwatch();
        public static bool isWash = false;
        public static void CarWash()
        {
            if (!toggleWashStopwatch.IsRunning)
                toggleWashStopwatch.Start();
            var player = Game.Player.Character;

            if (player.Position.DistanceTo(new Vector3(201.11f, 6418.75f, 31.1f)) < 0.7f && toggleWashStopwatch.ElapsedMilliseconds > 6000
                && !Game.IsControlPressed(0, Control.Attack)
                && Game.IsControlPressed(0, Control.Sprint))
            {
                isWash = !isWash;
                toggleWashStopwatch = new Stopwatch();
                if (isWash)
                {
                    player.Weapons.Give(WeaponHash.FireExtinguisher, 99999, true, true);
                }
                else
                {
                    player.Weapons.RemoveAll();
                    UI.Notify("Exit wash");
                }
            }
            if (isWash && !player.IsInVehicle())
            {
                var currentModelSize = veh.Model.GetDimensions();
                currentModelSize = new Vector3(currentModelSize.X / 2, currentModelSize.Y / 2, currentModelSize.Z / 2);
                var z = currentModelSize.Z + 0.2f;
                z = z < 2.3f ? 2.3f : z;
                var cablePos = veh.GetOffsetInWorldCoords(new Vector3(0, 0, z));
                var cablePos2 = new Vector3(player.Position.X, player.Position.Y, player.Position.Z + z);
                var hose = player.Weapons.CurrentWeaponObject.Position;
                var off = 0f;
                for (var i = 0; i < 5; ++i)
                {
                    off += 0.007f;
                    Function.Call((Hash)0x6B7256074AE34680, cablePos.X, cablePos.Y, cablePos.Z + off, cablePos2.X, cablePos2.Y, (cablePos2.Z + off), 0, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, cablePos2.X, cablePos2.Y, cablePos2.Z + off, hose.X, hose.Y, (hose.Z + off), 0, 0, 0, 255);
                }
                if (!carWashMoveStopwatch.IsRunning)
                {
                    carWashMoveStopwatch.Start();
                }

                if (Game.IsControlPressed(0, Control.Attack))
                {
                    if (carWashMoveStopwatch.ElapsedMilliseconds % 2000 > 490)
                    {
                        Game.DisableControlThisFrame(0, Control.MoveLeftRight);
                        Game.DisableControlThisFrame(0, Control.MoveUpDown);
                    }
                    veh.DirtLevel -= 0.0028f;
                    if (veh.DirtLevel < 0.2)
                    {
                        veh.DirtLevel = 0.0f;
                        UI.Notify("Clean");
                    }
                }
            }
        }


        public static void SupplyMode()
        {
            supplyMode = true;
            RemovePalletes();
            string[] l = { "71" };
            System.IO.File.WriteAllLines(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\warehouse_config.txt", l);
            LoadPalletes(true);
            UI.ShowSubtitle("supply mode");
        }

        public static void LoadVehicles()
        {
            /*var model = VehicleHash.Forklift;
            
            for (var i = 0; i < 2; ++i)
            {
                var v = World.CreateVehicle(new Model(model), pos, 205f);
                pos = v.GetOffsetInWorldCoords(new Vector3(2.5f, 0, 0));
                if (rnd2.Next(1, 9) % 2 == 0)
                {
                    v.Delete();
                }
                vehicles.Add(v);
            }
            model = VehicleHash.Handler;*/

            if (currentHour >= 6 && currentHour < 17)
            {
                if (vehicles.Count <= 0)
                {
                    pedsDriveToHomeStopwatch = new Stopwatch();
                    var pos = new Vector3(1988.04f, 4724.16f, 41.3f);
                    pos = new Vector3(156.9f, 6436.1f, 31.6f);
                    for (var i = 0; i < 8; ++i)
                    {
                        var mod = i == 0 ? VehicleHash.Neon : vehicleHashes[rnd.Next(0, vehicleHashes.Count - 1)];
                        var v = World.CreateVehicle(new Model(mod), pos, 226f);
                        if (i == 0)
                        {
                            v.PrimaryColor = VehicleColor.MetallicSilver;
                            v.SecondaryColor = VehicleColor.MetallicSilver;
                            v.PearlescentColor = VehicleColor.MetallicSilver;
                        }
                        pos = v.GetOffsetInWorldCoords(new Vector3(i == 0 ? -3f : -2.6f, 0, 0));
                        if (i == 1)
                            v.Delete();
                        else
                            vehicles.Add(v);
                    }
                    pos = new Vector3(133.0f, 6376.8f, 31.6f);
                    if (true)
                    {
                        for (var i = 0; i < 8; ++i)
                        {
                            var mod = vehicleHashes[rnd.Next(0, vehicleHashes.Count - 1)];
                            var v = World.CreateVehicle(new Model(mod), pos, 298f);
                            pos = v.GetOffsetInWorldCoords(new Vector3(3f, 0, 0));
                            vehicles.Add(v);
                        }
                    }
                    vehicles.Shuffle();
                }
            }
            if (companyVehicles.Count <= 0)
            {

                // var pos = new Vector3(179.8f, 6436.5f, 31.1f); //40
                var pos = new Vector3(163.7f, 6415.66f, 30.8f);
                List<int> vh = new List<int>()
                {
                      1201526847,
                    //  1733978250,
                    //  1733978250,
                    //  1733978250,
                    361399297,
                     361399297,
                      361399297,
                      -1356133282,
                     // 1441840295,
                     // 1441840295,
                     1201526847,
                     1201526847,
                      996052471,
                      996052471,
                      996052471
                };
                var heading = 69.82f;
                for (var i = 0; i < vh.Count; ++i)
                {
                    if ((veh.LodDistance == activeCompanyCar && activeCompanyCar == (i + 2000)))
                    {
                        companyVehicles.Add(veh);
                    }
                    var mod = vehicleHashes[rnd.Next(0, vehicleHashes.Count - 1)];
                    var v = World.CreateVehicle(new Model(vh[i]), pos, heading);
                    pos = v.GetOffsetInWorldCoords(new Vector3(3f, 0.0f, 0));
                    if (i == 0)
                    {
                        //  pos = v.GetOffsetInWorldCoords(new Vector3(-3.4f, 2f, 0));
                    }
                    v.LodDistance = i + 2000;
                    if (i == 5)
                    {
                        pos = new Vector3(171.9f, 6432.3f, 30.9f);
                        v.Position = pos;
                        heading = 40f;
                        v.Heading = heading;
                        pos = v.GetOffsetInWorldCoords(new Vector3(4f, 0.0f, 0));
                    }
                    if (i == 7)
                    {
                        pos = new Vector3(199.6f, 6384.0f, 31.5f);
                        v.Position = pos;
                        heading = 28.0f;
                        v.Heading = heading;
                        pos = v.GetOffsetInWorldCoords(new Vector3(4.4f, 0.0f, 0));
                    }
                    if (i > 7)
                    {
                        pos = v.GetOffsetInWorldCoords(new Vector3(4.4f, 0.0f, 0));
                    }
                    if (activeCompanyCar == (i + 2000))
                    {
                        v.Position = new Vector3(0, 0, 0);
                        v.Delete();
                    }
                    else
                    {
                        if (vh[i] == 361399297)
                        {
                            v.PrimaryColor = VehicleColor.MetallicDarkBlue;
                            v.SecondaryColor = VehicleColor.MatteBlack;
                            v.SetMod(VehicleMod.Engine, 1, false);
                            v.WheelType = VehicleWheelType.Tuner;
                            Function.Call(Hash.SET_VEHICLE_MOD_KIT, v, 0);
                            Function.Call(Hash.SET_VEHICLE_MOD, v, 23, 89, false);
                            v.NumberPlate = $"SK 83N2{i}";

                            v.NumberPlateType = NumberPlateType.BlueOnWhite1;
                            // Function.Call(Hash.SET_VEHICLE_MOD, v, 23, 84, false);
                            v.ToggleExtra(3, false);
                            v.ToggleExtra(1, false);
                            v.ToggleExtra(11, true);
                        }
                        else if (vh[i] == 1441840295)
                        {
                            v.NumberPlate = $"SK 9MV2{i}";
                            v.WindowTint = VehicleWindowTint.Green;
                            v.NumberPlateType = NumberPlateType.BlueOnWhite1;
                            v.PrimaryColor = VehicleColor.MetallicBlack;
                            v.SecondaryColor = VehicleColor.MetallicBlack;
                            v.PearlescentColor = VehicleColor.MetallicBlack;
                        }
                        else if (vh[i] == 1201526847)
                        {
                            v.NumberPlate = "SK 9KJ24";
                            v.NumberPlateType = NumberPlateType.BlueOnWhite1;
                            for (var ikk = 0; ikk < 10; ++ikk)
                            {
                                v.ToggleExtra(ikk, true);
                            }
                            v.PrimaryColor = VehicleColor.WornWhite;
                            v.SecondaryColor = VehicleColor.WornWhite;
                        }
                        else if (vh[i] == -131701523)
                        {
                            v.PrimaryColor = VehicleColor.WornGraphite;
                            v.NumberPlate = "SG 87N32";
                            v.NumberPlateType = NumberPlateType.BlueOnWhite1;
                            v.SetMod(VehicleMod.Engine, 1, false);
                            v.ToggleExtra(0, false);
                            v.ToggleExtra(1, false);
                        }
                        else if (vh[i] == -1356133282)
                        {
                            // v.PrimaryColor = VehicleColor.MetallicSteelGray;
                            v.PrimaryColor = VehicleColor.UtilSeaFoamBlue;
                            v.NumberPlate = "SG 87N39";
                            v.NumberPlateType = NumberPlateType.BlueOnWhite1;
                            v.SetMod(VehicleMod.Engine, 1, false);
                            v.WheelType = VehicleWheelType.Sport;
                            v.ToggleExtra(0, false);
                            v.ToggleExtra(1, false);
                            v.ToggleExtra(2, false);
                            v.ToggleExtra(3, false);
                            v.WindowTint = VehicleWindowTint.Green;
                            Function.Call(Hash.SET_VEHICLE_MOD_KIT, v, 0);
                            Function.Call(Hash.SET_VEHICLE_MOD, v, 23, 61, false);
                        }
                        else if (vh[i] == 996052471)
                        {
                            v.WindowTint = VehicleWindowTint.Green;
                            v.ToggleExtra(0, true);
                            v.ToggleExtra(1, true);

                        }
                        else
                        {
                            v.SecondaryColor = VehicleColor.MetallicBlack;
                            v.PrimaryColor = VehicleColor.MetallicBlack;
                            v.PearlescentColor = VehicleColor.MatteBlack;
                        }
                        companyVehicles.Add(v);
                    }
                }
                if (false) {
                    pos = new Vector3(169.2f, 6456.5f, 31.1f);

                    for (var i = 0; i < 2; ++i)
                    {
                        if (veh.LodDistance == activeCompanyCar && activeCompanyCar == ((2000 + vh.Count) + i))
                        {
                            companyVehicles.Add(veh);
                        }
                        var v = World.CreateVehicle(new Model(-887116351), pos, 135.7f);
                        pos = v.GetOffsetInWorldCoords(new Vector3(3.4f, 0f, 0));
                        if (activeCompanyCar == ((i + vh.Count) + 2000))
                        {
                            v.Position = new Vector3(0, 0, 0);
                            v.Delete();
                        }
                        else
                        {
                            v.PrimaryColor = VehicleColor.PureWhite;
                            v.CustomSecondaryColor = Color.FromArgb(0, 0, 0);
                            v.PearlescentColor = VehicleColor.MetallicFrostWhite;
                            //   v.WheelType = VehicleWheelType.SUV; 
                            v.NumberPlate = "SK " + (i + 4) + "HN34";
                            v.NumberPlateType = NumberPlateType.BlueOnWhite1;
                            v.LodDistance = ((2000 + vh.Count) + i);
                            for (var ikk = 0; ikk < 55; ++ikk)
                            {
                                v.ToggleExtra(ikk, false);
                            }
                            //   Function.Call(Hash.SET_VEHICLE_MOD_KIT, v, 0);
                            //  Function.Call(Hash.SET_VEHICLE_MOD, v, 23, 57, false);
                            // v.WindowTint = VehicleWindowTint.Green;
                            Function.Call(Hash.SET_VEHICLE_EXTRA_COLOURS, v, 0, 0);
                            v.PlaceOnGround();
                            companyVehicles.Add(v);
                        }
                    }
                }
               
                pos = new Vector3(151.2f, 6467.1f, 31.3f);

                for (var i = 0; i < 2 && false; ++i)
                {
                    var v = World.CreateVehicle(new Model(true ? -854789358 : 1491375716), pos, 134.9f);
                    pos = v.GetOffsetInWorldCoords(new Vector3(-2.4f, 0f, 0));
                    v.RimColor = VehicleColor.MatteBlack;
                    v.Livery = 0;
                    companyVehicles.Add(v);
                }

                if (false)
                {
                    pos = new Vector3(169.3f, 6441.8f, 32.6f);
                    var v = World.CreateVehicle(new Model(-877478386), pos, 133f);
                    v.ToggleExtra(2, false);
                    v.ToggleExtra(3, false);
                    v.ToggleExtra(4, false);
                    v.ToggleExtra(1, false);
                    v.ToggleExtra(5, false);
                    v.PlaceOnGround();
                    if (truckTrailer != null)
                        v.Delete();
                    else
                        companyVehicles.Add(v);
                }
                activeCompanyCar = 1002;
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void RemoveVehicles()
        {
            if (vehicles.Count > 0)
            {
                for (var i = 0; i < vehicles.Count; ++i)
                {
                    vehicles[i].Delete();
                }
                vehicles = new List<Vehicle>();
            }
            if (companyVehicles.Count > 0)
            {
                for (var i = 0; i < companyVehicles.Count; ++i)
                {
                    if (veh.LodDistance != (i + 2000))
                    {
                        if (truckTrailer?.DisplayName != companyVehicles[i]?.DisplayName)
                        {
                            companyVehicles[i].Delete();
                        }
                    }
                    else
                        activeCompanyCar = veh.LodDistance;
                }
                companyVehicles = new List<Vehicle>();
            }
        }

        private static List<Prop> rampProps = new List<Prop>();

        public static void MoveTruck()
        {
            if (timeToNextTruckStopwatch.ElapsedMilliseconds > timeToNextTruck)
            {
                timeToNextTruckStopwatch = new Stopwatch();
                if (currentHour < 17 && currentHour > 5)
                    LoadTruck();
            }
            if (trailer != null)
            {
                // if (trailer.Speed < 1f && truck.Speed >= 2f)
                // {
                //     Function.Call((Hash)0x3C7D42D58F770B54, truck, trailer, 5f);
                //  }
            }
            if (truckDriver != null)
            {
                if (truck.Speed < 1 && truckState == "enter")
                {
                    zeroSpeedTruckStopwatch.Start();
                }
                else if (zeroSpeedTruckStopwatch.IsRunning)
                {
                    zeroSpeedTruckStopwatch = new Stopwatch();
                }
                if ((truckState == "enter" && truck.Position.DistanceTo(truckDriveToPos) < 5f) && zeroSpeedTruckStopwatch.ElapsedMilliseconds > 5000)
                {
                    var scenarios = new List<string>() { "WORLD_HUMAN_AA_COFFEE", "WORLD_HUMAN_DRUG_DEALER", "WORLD_HUMAN_COP_IDLES", "WORLD_HUMAN_AA_SMOKE" };
                    truckState = "stop";
                    truck.FreezePosition = true;
                    trailer.FreezePosition = true;
                    var offsetT = new Vector3();
                    for (var i = 0; i < 3; ++i)
                    {
                        if (garageDoors[i].DoorZ <= 0)
                        {
                            offsetT = garageDoors[i].Door.GetOffsetInWorldCoords(new Vector3(0, -14.5f, 0));
                            break;
                        }
                    }

                    truck.Position = new Vector3(offsetT.X, offsetT.Y, truck.Position.Z);
                    truck.Heading = (garageDoors[0].Door.Heading + 180) % 360;
                    trailer.Alpha = 80;
                    truck.EngineRunning = false;
                    var seq1 = new TaskSequence();
                    seq1.AddTask.Wait(4000);
                    seq1.AddTask.LeaveVehicle();
                    var officePos = new Vector3(150.1f, 6449.5f, 31.8f);
                    seq1.AddTask.GoTo(truck.GetOffsetInWorldCoords(new Vector3(-10f, 2f, 0f)));
                    seq1.AddTask.StartScenario(scenarios[rnd.Next(0, scenarios.Count - 1)], officePos);
                    seq1.Close();
                    truckDriver.AlwaysKeepTask = true;
                    truckDriver.Task.PerformSequence(seq1);
                    seq1.Dispose();
                    zeroSpeedTruckStopwatch = new Stopwatch();

                    var containerPos = new Vector3(0, -1f, -1.41f);
                    var fakeContainer = World.CreateProp(new Model(1022953480), containerPos, false, false);
                    Function.Call((Hash)0x9EBC85ED0FFFE51C, fakeContainer, true, true);
                    fakeContainer.AttachTo(trailer, 0, containerPos, new Vector3(0, 0, 0));
                    rampProps.Add(fakeContainer);
                    var fakeContFloor1 = World.CreateProp(new Model(2067252279), new Vector3(0, 0, 0), false, false);
                    fakeContFloor1.AttachTo(trailer, 0, new Vector3(0, 1.4f, -1.28f), new Vector3(0, 0, 0));
                    fakeContFloor1.HasCollision = true;
                    rampProps.Add(fakeContFloor1);
                    var fakeContFloor2 = World.CreateProp(new Model(2067252279), new Vector3(0, 0, 0), false, false);
                    fakeContFloor2.AttachTo(trailer, 0, new Vector3(0, -3.9f, -1.28f), new Vector3(0, 0, 0));
                    fakeContFloor2.HasCollision = true;
                    rampProps.Add(fakeContFloor2);

                    var fakeContRamp = World.CreateProp(new Model(2067252279), new Vector3(0, 0, 0), false, false);
                    fakeContRamp.AttachTo(trailer, 0, new Vector3(0, -10.2f, -2.27f), new Vector3(18, 0, 0));
                    fakeContRamp.HasCollision = true;
                    rampProps.Add(fakeContRamp);
                    trailer.HasCollision = false;
                    trailer.FreezePosition = true;
                }
                if (truckState == "stop")
                {
                    // trailer.ApplyForceRelative(new Vector3(0, 0, -0.35f));
                }
                if (truckState == "stop" && truckDriver.Position.DistanceTo(Game.Player.Character.Position) <= 2f)
                {
                    truck.FreezePosition = false;
                    trailer.HasCollision = true;
                    trailer.FreezePosition = false;
                    truck.EngineRunning = true;
                    truckState = "exit";
                    trailer.Alpha = 255;
                    var y = -5f;

                    for (var i = 0; i < rampProps.Count; ++i)
                    {
                        if (i == 0)
                            rampProps[0].Position = new Vector3(0, 0, 0);
                        rampProps[i].Delete();
                    }
                    rampProps = new List<Prop>();
                    trailerContent = World.GetNearbyProps(trailer.Position, 15f, new Model(153748523)).ToList();
                    for (var i = 0; i < trailerContent.Count; ++i)
                    {
                        trailerContent[i].Delete();
                    }
                    SetTruckDriverTask();
                }
                if (truckState == "exit")
                {
                    if (lastSetTruckDriverTaskStopwatch.ElapsedMilliseconds > 10000 && !truckDriver.IsInVehicle())
                        SetTruckDriverTask();
                    if (truck.Position.DistanceTo(warehousePos) > 100f)
                    {
                        RemoveTruck();
                        if (currentHour >= 6 && currentHour < 17)
                        {
                            timeToNextTruckStopwatch.Start();
                            timeToNextTruck = rnd.Next(60000, 180000);
                        }
                    }
                }
            }
        }

        public static void SetTruckDriverTask()
        {
            truckDriver.Position = truck.GetOffsetInWorldCoords(new Vector3(2f, 2f, 0f));
            var seq1 = new TaskSequence();
            seq1.AddTask.EnterVehicle(truck, VehicleSeat.Driver);
            seq1.AddTask.DriveTo(truck, truckSpawnPoint, 4f, 4f);
            seq1.AddTask.DriveTo(truck, new Vector3(367.34f, 6564.71f, 27.6f), 2f, 9f);
            seq1.Close();
            truckDriver.AlwaysKeepTask = true;
            truckDriver.Task.PerformSequence(seq1);
            seq1.Dispose();
            lastSetTruckDriverTaskStopwatch = new Stopwatch();
            lastSetTruckDriverTaskStopwatch.Start();
        }

        private static Random truckSelectRnd = new Random();

        public static void LoadTruck()
        {
            if (currentHour > 5 && currentHour < 17 && !supplyMode)
            {
                List<VehicleHash> vehHashes = new List<VehicleHash>() { VehicleHash.Packer, VehicleHash.Phantom, VehicleHash.Hauler };
                List<PedHash> pedhashesTruck = new List<PedHash>() { PedHash.Trucker01SMM, PedHash.Dockwork01SMM, PedHash.AirworkerSMY, PedHash.GentransportSMM, PedHash.Salton02AMM };
                truckSpawnPoint = new Vector3(95.33f, 6403.54f, 31.0f);
                truck = World.CreateVehicle(new Model(vehHashes[truckSelectRnd.Next(0, vehHashes.Count - 1)]), truckSpawnPoint);
                trailer = World.CreateVehicle(new Model(VehicleHash.Trailers4), truckSpawnPoint);
                truck.IsInvincible = true;
                trailer.IsInvincible = true;
                truck.Heading = 220f;
                trailer.Heading = truck.Heading;
                truck.TowVehicle(trailer, true);
                truck.PlaceOnGround();
                trailer.PlaceOnGround();
                truckDriver = truck.CreatePedOnSeat(VehicleSeat.Driver, new Model(pedhashesTruck[rnd.Next(0, pedhashesTruck.Count - 1)]));
                //  truckDriveToPos = new Vector3(155.19f, 6449.68f, 31.4f);
                truckDriveToPos = new Vector3(154.79f, 6404.9f, 31.1f);
                Function.Call((Hash)0x3C7D42D58F770B54, truck, trailer, 5f);
                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, truckDriver, truck, truckDriveToPos.X, truckDriveToPos.Y, truckDriveToPos.Z, 5f, 1f, truck.GetHashCode(), 6, 1f, true);
                truckDriver.AlwaysKeepTask = true;
                truckState = "enter";
                t.Start();
            }

        }

        public static void RemoveTruck()
        {
            if (truck != null)
            {
                truck.Delete();
                truck = null;
                trailer.Delete();
                trailer = null;
                truckDriver.Delete();
                truckDriver = null;
                if (trailerContent.Count > 0)
                {
                    palletesSupplyCount -= trailerContent.Count;
                    string[] l = { palletesSupplyCount.ToString() };
                    System.IO.File.WriteAllLines(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\warehouse_config.txt", l);
                    for (var i = 0; i < trailerContent.Count; ++i)
                    {
                        trailerContent[i].Delete();
                    }
                }
                rampProps.ForEach(rp => rp.Delete());
                rampProps = new List<Prop>();
            }

        }

        public static void LoadAllProps()
        {
            isWarehouse = true;
            // LoadPalletes();
            LoadPeds();
            LoadBarriers();
            LoadGarageDoors();
            //   LoadShelf();
            timeToNextTruckStopwatch.Start();
        }

        public static void RemoveAllProps()
        {
            isWarehouse = false;
            RemovePalletes();
            RemoveTruck();
            RemovePeds();
            RemoveVehicles();
            RemoveBarriers();
            RemoveSupplyTrain();
            RemoveGarageDoors();
            //  RemoveShelf();
        }

        public static void SetPedTasks()
        {
            for (var i = 0; i < warehousePeds.Count; ++i)
            {
                warehousePeds[i].Task.WanderAround(warehousePos, 50f);
            }
        }

        public static List<PropModel> pedTaskList = new List<PropModel>()
        {
            new PropModel()
            {
                Position = new Vector3(136.9f, 6451.7f, 31.7f),
                Heading = 137.7f,
                PedAction = "clipboard"
            },
             new PropModel()
            {
                Position = new Vector3(141.1f, 6457.4f, 31.7f),
                Heading = 223.4f,
                PedAction = "clipboard"
            },
              new PropModel()
            {
                Position = new Vector3(148.3f, 6464.2f, 31.7f),
                Heading = 309.8f,
                PedAction = "clipboard"
            },
                 new PropModel()
            {
                Position = new Vector3(129.8f, 6459.42f, 31.7f),
                Heading = 15.1f,
                PedAction = "clipboard"
            },
                    new PropModel()
            {
                Position = new Vector3(118.7f, 6446.1f, 31.7f),
                Heading = 47.1f,
                PedAction = "leaf blower"
            }
        };


        public static void LoadPeds()
        {
            var rampAreaPos = new Vector3(195.822f, 6394.24f, 31.3f);
            if (currentHour > 5 && currentHour < 17)
            {
                for (var i = 0; i < 6; ++i)
                {
                    var ped = World.CreatePed(pedHashesPossible[rnd4.Next(0, pedHashesPossible.Count - 1)], warehousePos);
                    ped.Task.WanderAround(ped.Position, 50f);
                    ped.IsInvincible = true;
                    ped.Health = 9999;
                    warehousePeds.Add(ped);
                }
                for (var i = 0; i < 2; ++i)
                {
                    var ped = World.CreatePed(pedHashesPossible[rnd4.Next(0, pedHashesPossible.Count - 1)], rampAreaPos);
                    ped.Task.WanderAround(ped.Position, 35f);
                    ped.IsInvincible = true;
                    ped.Health = 9999;
                    warehousePeds.Add(ped);
                }
                for (var i = 0; i < pedTaskList.Count; ++i)
                {
                    var p = pedTaskList[i];
                    var ped = World.CreatePed(pedHashesPossible[rnd4.Next(0, pedHashesPossible.Count - 1)], p.Position);
                    var seq = new TaskSequence();
                    seq.AddTask.AchieveHeading(p.Heading);

                    seq.AddTask.PlayAnimation("amb@world_human_push_ups@male@base", "base", 1, 9999999, false, 1f);
                    ped.Task.PerformSequence(seq);
                    //  Function.Call(Hash.TASK_PLAY_ANIM, ped, "amb@world_human_push_ups@male@base", "base", 1, -1, 0, 0, false, false, false);
                    ped.AlwaysKeepTask = true;
                    warehousePedsFactory.Add(ped);
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

            for (var i = 0; i < warehousePedsFactory.Count; ++i)
            {
                warehousePedsFactory[i].Delete();
            }
            warehousePedsFactory = new List<Ped>();

            if (pedsDrivingToHome.Count > 0)
            {
                for (var i = 0; i < pedsDrivingToHome.Count; ++i)
                {
                    pedsDrivingToHome[i].Delete();
                }
                pedsDrivingToHome = new List<Ped>();
            }

        }


        public static void LoadPalletes(bool supplyMode = false)
        {
            var posz = new Vector3(104.24f, 6435.07f, 30.75f);
            if (supplyMode)
                posz = new Vector3(136.2f, 6410.67f, 30.24f);
            var palletRef = World.CreateProp(new Model(-1951226014), posz, true, false);
            palletRef.Heading = 45f;
            palletes.Add(palletRef);
            var x = 4f;
            var y = 1.74f;
            var z = 0f;
            palletesSupplyCount = int.Parse(System.IO.File.ReadAllText(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\warehouse_config.txt"));
            for (var i = 0; i < palletesSupplyCount; ++i)
            {
                x += 2f;
                if (i % 24 == 0 && i > 1)
                {
                    if (z <= 0)
                    {
                        // z = 1.38f;
                        x = 4;
                    }
                    else if (z > 0)
                    {
                        z = 0;
                    }
                    if (z == 0)
                    {
                        x = 4;
                        y -= 1.74f;
                    }
                }
                var pallete = World.CreateProp(new Model(-1951226014), palletRef.GetOffsetInWorldCoords(new Vector3(x, y, z)), true, false);
                pallete.Heading = 45f;
                pallete.Alpha = 50;
                var palleteFake = World.CreateProp(new Model(153748523), palletRef.GetOffsetInWorldCoords(new Vector3(x, y, z)), false, false);
                palleteFake.AttachTo(pallete, 0, new Vector3(0, 0, 0f), new Vector3());
                palleteFake.Alpha = 255;
                palleteFake.FreezePosition = true;
                palletes.Add(palleteFake);
                palletes.Add(pallete);
            }
            posz = new Vector3(128.3f, 6407.6f, 30.24f);
            palletRef = World.CreateProp(new Model(1711856655), posz, true, false);
            palletRef.Heading = 135f;
            x = 0f;
            y = 1.74f * 3;
            z = 0f;
            var row = 0;
            var column = 0;
            for (var i = 0; i < 110; ++i)
            {
                if (i != 0)
                    x += 1.8f;
                if (column > 8)
                {
                    column = -1;
                    row += 1;
                    y += 1.74f;
                    x = 0f;
                    if (row > 2)
                    {
                        row = 0;
                        y = 1.74f * 3;
                        z += 1.35f;
                    }
                }
                if (column != 6 || true)
                {
                    var pallete = World.CreateProp(new Model(1711856655), palletRef.GetOffsetInWorldCoords(new Vector3(x, y, z)), true, false);
                    pallete.FreezePosition = true;
                    pallete.Heading = 135f;
                    palletes.Add(pallete);
                }
                column += 1;
            }
            palletRef.Delete();
        }

        public static void RemovePalletes()
        {
            if (palletes.Count > 0)
            {
                for (var i = 0; i < palletes.Count; ++i)
                {
                    palletes[i].Delete();
                }
                palletes = new List<Prop>();
            }
            if (trainPalletes.Count > 0)
            {
                for (var i = 0; i < trainPalletes.Count; ++i)
                {
                    trainPalletes[i].Delete();
                }
                trainPalletes = new List<Prop>();
            }
        }

        public static void LoadGate()
        {
            gate = World.CreateProp(new Model(1286535678), new Vector3(136.066666f, 6388.05859f, 30.1773453f), new Vector3(0, 0, -61.79959f), false, false);
            gate.FreezePosition = true;
            //garage 
            garage = World.CreateProp(new Model(-1212944997), new Vector3(134.49f, 6445.1f, 31.4f), new Vector3(0, 0, -44.8999329f), false, false);
            garage.Quaternion = new Quaternion(0f, 0f, 0.3818765f, 0.92421335f);
            garage.FreezePosition = true;
        }

        public static void ToggleGate()
        {
            bool dub = false;
            if (gatesState >= 1)
            {
                isGarageOpen = !isGarageOpen;
                garageMovingStopwatch = new Stopwatch();
                garageMovingStopwatch.Start();
                isGarageMoving = true;
                gatesState = isGarageOpen ? gatesState + 1 : gatesState - 1;
                dub = true;
            }
            if (gatesState == 0 && !dub)
            {
                isGateOpen = !isGateOpen;
                gateMovingStopwatch = new Stopwatch();
                gateMovingStopwatch.Start();
                isGateMoving = true;
                gatesState = isGateOpen ? gatesState + 1 : gatesState - 1;
            }
        }
        public static void RemoveGate()
        {
            gate.Delete();
            gate = null;
            gateX = 0;
            isGateMoving = false;
            isGateOpen = false;
            garage.Delete();
            garage = null;
            garageZ = 0;
            isGarageMoving = false;
            isGarageOpen = false;
        }
    }
}
