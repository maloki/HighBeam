using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.Main;

namespace HighBeam
{
    public static class Terminal
    {
        public static bool init = false;
        public static bool isTerminal = false;
        public static Vector3 terminalPosition = new Vector3(2031.2f, 4761.7f, 41f);
        public static Random rnd = new Random();
        public static Random rnd2 = new Random();
        // entrance barriers 
        public static Prop enterBarrier = null;
        public static float enterBarrierRot = 0f;
        public static Prop exitBarrier = null;
        public static Stopwatch barierStopwatch = new Stopwatch();
        public static bool canOperateBarrier = false;

        //palletes
        public static List<Prop> palletes = new List<Prop>();

        // vehicles
        public static List<Vehicle> vehicles = new List<Vehicle>();

        // train 
        public static List<Prop> locomotives = new List<Prop>();
        public static List<Prop> railBoxes = new List<Prop>();
        public static Vector3 initialLocomotivePos = new Vector3(1916.5f, 4723.89f, 40.6f);
        public static float trainHeading = 295.1f;
        public static string trainState = "entering";
        public static float trainSpeed = 0f;
        public static string trainType = "container";
        public static bool isTrainContentSpawned = false;
        public static List<Prop> railBoxContent = new List<Prop>();
        public static bool isTrainLoaded = false;
        public static Stopwatch nextTrainSpawnStopwatch = new Stopwatch();
        public static int timeToNextTrain = 0;
        public static List<string> trainTypes = new List<string>() { "container", "pallet" };
        public static int timeToTrainLeave = 0;
        public static Stopwatch trainLeaveStopwatch = new Stopwatch();
        public static Stopwatch trainIsReadyToLeave = new Stopwatch();

        // peds 
        public static List<PedHash> posPeds = new List<PedHash>()
        {
              PedHash.Dockwork01SMM,
              PedHash.Dockwork01SMY,
              PedHash.AirworkerSMY
        };
        public static List<Ped> peds = new List<Ped>();
        public static Stopwatch pedsTaskRefreshStopwatch = new Stopwatch();
        public static List<StaticPedModel> staticPeds = new List<StaticPedModel>() {
            new StaticPedModel(){ Animation = "WORLD_HUMAN_CLIPBOARD", Position = new Vector3(2020.36f, 4751.4f, 41.6f), Heading = 211, PedHash = posPeds[rnd.Next(0, posPeds.Count - 1)] },
            new StaticPedModel(){ Animation = "WORLD_HUMAN_AA_SMOKE", Position = new Vector3(2006.8f, 4744.3f, 41.6f), Heading = 315, PedHash = posPeds[rnd.Next(0, posPeds.Count - 1)] },
            new StaticPedModel(){ Animation = "WORLD_HUMAN_AA_COFFEE", Position = new Vector3(2008.6f, 4744.8f, 41.6f), Heading = 96, PedHash = posPeds[rnd.Next(0, posPeds.Count - 1)] },
            new StaticPedModel(){ Animation = "WORLD_HUMAN_DRUG_DEALER", Position = new Vector3(2007.665f, 4746.0f, 41.6f), Heading = 158, PedHash = posPeds[rnd.Next(0, posPeds.Count - 1)] },
            new StaticPedModel(){ Animation = "WORLD_HUMAN_CLIPBOARD", Position = new Vector3(2076.1f, 4764.0f, 41.6f), Heading = 292, PedHash = posPeds[rnd.Next(0, posPeds.Count - 1)] },
            new StaticPedModel(){ Animation = "WORLD_HUMAN_CLIPBOARD", Position = new Vector3(1963.5f, 4726.1f, 41.6f), Heading = 240, PedHash = posPeds[rnd.Next(0, posPeds.Count - 1)] },
            new StaticPedModel(){ Animation = "WORLD_HUMAN_CLIPBOARD", Position = new Vector3(1951.3f, 4710.5f, 41.6f), Heading = 292, PedHash = posPeds[rnd.Next(0, posPeds.Count - 1)] },
            new StaticPedModel(){ Animation = "WORLD_HUMAN_COP_IDLES", Position = new Vector3(2146.8f, 4809.4f, 41.6f), Heading = 290, PedHash = PedHash.Security01SMM },
        };
        public static List<Ped> staticPedsList = new List<Ped>();
        public static Stopwatch setTaskForPedsAfterLoadStopwatch = new Stopwatch();

        public static void RunTerminal()
        {
            if (!init && false)
            {
                init = true;
                LoadTerminalProps();
                LoadPeds();
                GenerateNextTrain();
            }
            if (Game.Player.Character.Position.DistanceTo(terminalPosition) < 330f && !isTerminal)
            {
                LoadTerminalProps();
                LoadPeds();
                GenerateNextTrain(true);
            }
            if (Game.Player.Character.Position.DistanceTo(terminalPosition) > 400f && isTerminal)
            {
                RemoveTerminalProps();
                RemoveTrain();
                RemovePeds();
            }
            if (isTerminal && Game.IsControlJustPressed(0, Control.Jump))
            {
                RemoveTerminalProps();
                RemoveTrain();
                RemovePeds();
            }
            if (isTerminal)
            {
                ManageEntranceBarriers();
                MoveTrain();
                if (!pedsTaskRefreshStopwatch.IsRunning)
                {
                    pedsTaskRefreshStopwatch.Start();
                }
                if (pedsTaskRefreshStopwatch.ElapsedMilliseconds > 5000)
                {
                    pedsTaskRefreshStopwatch = new Stopwatch();
                    SetPedsTask();
                }
                if (setTaskForPedsAfterLoadStopwatch.ElapsedMilliseconds > 4000)
                {
                    setTaskForPedsAfterLoadStopwatch = new Stopwatch();
                    SetPedsTask();
                }
                if (nextTrainSpawnStopwatch.ElapsedMilliseconds > timeToNextTrain && nextTrainSpawnStopwatch.IsRunning)
                {
                    LoadTrain();
                    nextTrainSpawnStopwatch = new Stopwatch();
                }
            }
        }

        public static void GenerateNextTrain(bool init = false)
        {
            timeToNextTrain = rnd.Next(60000, 180000);
            nextTrainSpawnStopwatch = new Stopwatch();
            nextTrainSpawnStopwatch.Start();
            var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
            trainType = trainTypes[h >= 13 && h <= 15 ? 0 : 1];
            if (trainType == "pallet")
            {
                timeToTrainLeave = 1800000;
            }
            else
            {
                timeToTrainLeave = rnd.Next(300000, 480000);
            }
            trainState = "entering";
            // timeToNextTrain = 5000;
            //   timeToTrainLeave = 5000;
        }

        public static void LoadPeds()
        {

            var ped = World.CreatePed(new Model(GTA.Native.PedHash.Dockwork01SMM), terminalPosition);
            ped.Heading = trainHeading;
            var p = -70f;
            for (var i = 0; i < 11; ++i)
            {
                if (i % 2 == 0)
                {
                    p += 30f;
                }
                var pedTer = World.CreatePed(new Model(posPeds[rnd.Next(0, posPeds.Count - 1)]), ped.GetOffsetInWorldCoords(new Vector3(0, p, 0)), trainHeading);
                pedTer.RandomizeOutfit();
                pedTer.AddBlip();
                pedTer.CurrentBlip.Position = pedTer.Position;
                pedTer.CurrentBlip.Scale = 0;
                pedTer.CurrentBlip.Alpha = 0;
                pedTer.Armor = 0;
                pedTer.IsInvincible = true;
                peds.Add(pedTer);
            }
            ped.Delete();
            for (var i = 0; i < staticPeds.Count; ++i)
            {
                var stat = staticPeds[i];
                var pedObj = World.CreatePed(new Model(stat.PedHash), stat.Position, stat.Heading);
                pedObj.IsInvincible = true;
                pedObj.Armor = 100;
                staticPedsList.Add(pedObj);
            }
            setTaskForPedsAfterLoadStopwatch.Start();
        }

        public static void SetPedsTask()
        {
            for (var i = 0; i < peds.Count; ++i)
            {
                var pedTer = peds[i];
                if (pedTer.Armor == 0)
                {
                    var seq1 = new TaskSequence();
                    seq1.AddTask.WanderAround(pedTer.CurrentBlip.Position, 20f);
                    seq1.Close();
                    pedTer.AlwaysKeepTask = true;
                    pedTer.Task.PerformSequence(seq1);
                    seq1.Dispose();
                }
            }
            for (var i = 0; i < staticPedsList.Count; ++i)
            {
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, staticPedsList[i], staticPeds[i].Animation.ToUpper(), -1, false);
            }
        }

        public static void RemovePeds()
        {
            for (var i = 0; i < peds.Count; ++i)
            {
                peds[i].Delete();
            }
            peds = new List<Ped>();
            for (var i = 0; i < staticPedsList.Count; ++i)
            {
                staticPedsList[i].Delete();
            }
            staticPedsList = new List<Ped>();
        }

        public static void LoadTrain()
        {
            for (var i = 0; i < 2; ++i)
            {
                var loc = World.CreateProp(new Model(1030400667), initialLocomotivePos, false, false);
                loc.Heading = trainHeading;
                if (i == 1)
                {
                    loc.Position = loc.GetOffsetInWorldCoords(new Vector3(0f, -18f, 0f));
                }
                locomotives.Add(loc);
            }
            var posBehindLoco = 0f;
            var posBehindRailBox = 0f;
            var railBoxHeight = 0f;
            var railBoxModel = 0;
            var railBoxHeading = 0f;
            var railBoxCount = 0;
            var railBoxHeightDiff = 0f;
            if (trainType == "pallet")
            {
                posBehindLoco = -17f;
                posBehindRailBox = 16.4f;
                railBoxHeight = -1.55f;
                railBoxModel = 2097329273;
                railBoxHeading = trainHeading - 90f;
                railBoxCount = 13;
            }
            if (trainType == "container")
            {
                posBehindLoco = -18.5f;
                posBehindRailBox = 19.2f;
                railBoxHeight = -1.44f;
                railBoxModel = -1282428048;
                railBoxHeading = trainHeading - 90f;
                railBoxCount = 11;
                railBoxHeightDiff = 0.04f;
            }
            var nextBoxPos = locomotives[locomotives.Count - 1].GetOffsetInWorldCoords(new Vector3(0, posBehindLoco, railBoxHeight));
            for (var i = 0; i < railBoxCount; ++i)
            {
                var box = World.CreateProp(new Model(railBoxModel), nextBoxPos, false, false);
                box.Heading = railBoxHeading;
                railBoxes.Add(box);
                var diffPos = box.GetOffsetInWorldCoords(new Vector3(posBehindRailBox, 0, 0));
                nextBoxPos = new Vector3(diffPos.X, diffPos.Y, nextBoxPos.Z);
            }
            if (trainType != "pallet")
            {
                LoadTrainContent();
                isTrainContentSpawned = true;
            }
        }

        public static void LoadTrainContent()
        {

            var railBoxContentModel = 0;
            Vector3 railBoxContentPos = new Vector3();
            var railBoxContentHeading = 0f;
            List<int> containers = new List<int>() { 772023703, -1617592469, 466911544, 2140719283, -1857328104, 1525186387, -380625884, 511018606 };
            if (trainType == "pallet")
            {
                railBoxContentModel = -1951226014;
                railBoxContentPos = new Vector3(0f, 0.5f, 1f);
                railBoxContentHeading = trainHeading - 90f;
            }
            if (trainType == "container")
            {
                railBoxContentModel = 772023703;
                railBoxContentPos = new Vector3(0f, 0.0f, 1f);
                railBoxContentHeading = trainHeading;
            }
            for (var i = 0; i < railBoxes.Count; ++i)
            {
                if (trainType == "container")
                {
                    railBoxContentModel = containers[rnd.Next(0, containers.Count - 1)];
                }
                var pos = railBoxes[i].GetOffsetInWorldCoords(railBoxContentPos);
                var content = World.CreateProp(new Model(railBoxContentModel), pos, true, false);
                content.Heading = railBoxContentHeading;
                railBoxContent.Add(content);
            }
        }

        public static void MoveTrain()
        {
            if (locomotives.Count > 0 && railBoxes.Count > 0)
            {
                var maxTrainSpeed = 0.12f;
                if (trainSpeed == 0f && trainState != "stop" && trainState != "exit")
                {
                    trainSpeed = maxTrainSpeed;
                }
                if (trainState == "entering")
                {
                    if (locomotives[0].Position.DistanceTo(initialLocomotivePos) > 180f)
                    {
                        trainSpeed -= 0.00017f;
                    }
                    if (trainSpeed <= 0)
                    {
                        trainSpeed = 0f;
                        trainState = "stop";
                    }
                }
                if (trainState == "exit")
                {
                    if (trainSpeed < maxTrainSpeed)
                    {
                        trainSpeed += 0.00004f;
                    }
                }
                if (trainLeaveStopwatch.ElapsedMilliseconds > timeToTrainLeave && trainLeaveStopwatch.IsRunning)
                {
                    trainState = "exit";
                    trainLeaveStopwatch = new Stopwatch();
                }
                if (trainState == "stop" && !isTrainContentSpawned)
                {
                    isTrainContentSpawned = true;
                    LoadTrainContent();
                }
                if (trainState == "stop" && !trainLeaveStopwatch.IsRunning)
                {
                    trainLeaveStopwatch = new Stopwatch();
                    trainLeaveStopwatch.Start();
                }
                World.DrawLightWithRange(locomotives[0].GetOffsetInWorldCoords(new Vector3(0, 15f, 0)), Color.White, 30f, 2f);
                if (trainState != "stop")
                {
                    for (var i = 0; i < locomotives.Count; ++i)
                    {
                        var locomotive = locomotives[i];
                        locomotive.Position = locomotive.GetOffsetInWorldCoords(new Vector3(0f, trainSpeed, 0.0f));
                        locomotive.FreezePosition = true;
                    }
                    for (var i = 0; i < railBoxes.Count; ++i)
                    {
                        var locomotive = railBoxes[i];
                        locomotive.Position = locomotive.GetOffsetInWorldCoords(new Vector3(-trainSpeed, 0f, 0.0f));
                        locomotive.FreezePosition = true;
                    }
                    if (railBoxContent.Count > 0)
                    {
                        for (var i = 0; i < railBoxContent.Count; ++i)
                        {
                            var locomotive = railBoxContent[i];
                            locomotive.Position = locomotive.GetOffsetInWorldCoords(new Vector3(0, trainSpeed, 0.0f));
                            locomotive.FreezePosition = true;
                        }
                    }
                }
                if (Game.Player.Character.Position.DistanceTo(new Vector3(2141.96f, 4821.18f, 41.5f)) < 1f && trainState == "stop" && !trainIsReadyToLeave.IsRunning)
                {
                    trainIsReadyToLeave = new Stopwatch();
                    trainIsReadyToLeave.Start();
                }
                if (trainIsReadyToLeave.ElapsedMilliseconds > 17000)
                {
                    trainIsReadyToLeave = new Stopwatch();
                    trainLeaveStopwatch = new Stopwatch();
                    trainState = "exit";
                    RemoveTrainContent();
                }
                if (railBoxes[railBoxes.Count - 1].Position.DistanceTo(initialLocomotivePos) > 300f && trainState == "exit")
                {
                    RemoveTrain();
                    GenerateNextTrain();
                }
            }
        }

        public static void RemoveTrain()
        {
            for (var i = 0; i < locomotives.Count; ++i)
            {
                locomotives[i].Delete();
            }
            locomotives = new List<Prop>();
            for (var i = 0; i < railBoxes.Count; ++i)
            {
                railBoxes[i].Delete();
            }
            railBoxes = new List<Prop>();
            RemoveTrainContent();


        }

        public static void RemoveTrainContent()
        {
            if (railBoxContent.Count > 0)
            {
                for (var i = 0; i < railBoxContent.Count; ++i)
                {
                    railBoxContent[i].Delete();
                }
                railBoxContent = new List<Prop>();
            }
        }

        public static void ManageEntranceBarriers()
        {
            if (!barierStopwatch.IsRunning)
            {
                barierStopwatch.Start();
            }
            if (veh.Heading < 110f && veh.Heading > 69f && enterBarrier.Rotation.Y > -50 && veh.Position.DistanceTo(enterBarrier.Position) < 7f)
            {
                barierStopwatch = new Stopwatch();
                enterBarrier.Rotation = new Vector3(enterBarrier.Rotation.X, enterBarrier.Rotation.Y - 1f, enterBarrier.Rotation.Z);
            }
            if (veh.Heading < 290f && veh.Heading > 240f && exitBarrier.Rotation.Y > -50 && veh.Position.DistanceTo(exitBarrier.Position) < 7f)
            {
                barierStopwatch = new Stopwatch();
                exitBarrier.Rotation = new Vector3(exitBarrier.Rotation.X, exitBarrier.Rotation.Y - 1f, exitBarrier.Rotation.Z);
            }
            if (true)
            {
                if (enterBarrier.Rotation.Y < 0 && veh.Position.DistanceTo(enterBarrier.Position) > 12f)
                {
                    enterBarrier.Rotation = new Vector3(enterBarrier.Rotation.X, enterBarrier.Rotation.Y + 1f, enterBarrier.Rotation.Z);
                    if (enterBarrier.Rotation.Y > 0)
                    {
                        RemoveEntranceBarriers();
                        LoadEntranceBarriers();
                    }
                }
                if (exitBarrier.Rotation.Y < 0 && veh.Position.DistanceTo(exitBarrier.Position) > 12f)
                {
                    exitBarrier.Rotation = new Vector3(exitBarrier.Rotation.X, exitBarrier.Rotation.Y + 1f, exitBarrier.Rotation.Z);
                    if (exitBarrier.Rotation.Y > 0)
                    {
                        RemoveEntranceBarriers();
                        LoadEntranceBarriers();
                    }
                }
            }
        }

        public static void LoadTerminalProps()
        {
            UI.Notify("Terminal Loaded");
            isTerminal = true;
            LoadEntranceBarriers();
            LoadPalletes();
            LoadVehicles();
        }

        public static void RemoveTerminalProps()
        {
            UI.Notify("Terminal Removed");
            isTerminal = false;
            RemoveEntranceBarriers();
            RemovePalletes();
            RemoveVehicles();
        }

        public static void LoadVehicles()
        {
            var model = VehicleHash.Forklift;
            var pos = new Vector3(1988.04f, 4724.16f, 41.3f);
            for (var i = 0; i < 8; ++i)
            {
                var v = World.CreateVehicle(new Model(model), pos, 205f);
                pos = v.GetOffsetInWorldCoords(new Vector3(2.5f, 0, 0));
                if (rnd2.Next(1, 9) % 2 == 0)
                {
                    v.Delete();
                }
                vehicles.Add(v);
            }
            model = VehicleHash.Handler;
            pos = new Vector3(2008.19f, 4733.65f, 41.3f);
            for (var i = 0; i < 3; ++i)
            {
                var v = World.CreateVehicle(new Model(model), pos, 25f);
                v.CustomPrimaryColor = Color.OrangeRed;
                pos = v.GetOffsetInWorldCoords(new Vector3(-8f, 0, 0));
                if (rnd2.Next(1, 9) % 5 == 0)
                {
                    v.Delete();
                }
                vehicles.Add(v);
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
        }

        public static void LoadPalletes()
        {
            var palletRef = World.CreateProp(new Model(-1951226014), new Vector3(2074.209f, 4759.893f, 40.4f), true, false);
            palletRef.Heading = 30f;
            palletes.Add(palletRef);
            var x = 0f;
            var y = 0f;
            for (var i = 0; i < 27; ++i)
            {
                x -= 2f;
                if (x < -18)
                {
                    x = 0;
                    y += 1.74f;
                }
                var pallete = World.CreateProp(new Model(-1951226014), palletRef.GetOffsetInWorldCoords(new Vector3(x, y, 0)), true, false);
                pallete.Heading = 30f;
                palletes.Add(pallete);

            }
            UI.ShowSubtitle(x.ToString());
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

        }

        public static void LoadEntranceBarriers()
        {
            enterBarrier = World.CreateProp(
                new Model(-1184516519),
                new Vector3(2144.86f, 4808.797f, 41.2666176f),
                new Vector3(91.00043f, 0.0004349f, 179.9997f),
                false,
                false
            );
            enterBarrier.Quaternion = new Quaternion(0f, 3.20387663E-08f, 0.715689957f, 0.69841814f);
            enterBarrier.FreezePosition = true;
            enterBarrier.HasCollision = false;
            exitBarrier = World.CreateProp(
                new Model(-1184516519),
                new Vector3(2149.21948f, 4809.641f, 41.3f),
                new Vector3(1.00179077E-05f, 0.00895658E-06f, -90.10021f),
                false,
                false
            );
            exitBarrier.FreezePosition = true;
            exitBarrier.HasCollision = false;
            exitBarrier.Quaternion = new Quaternion(-3.08274934E-08f, 9.27528347E-08f, 0.707724869f, -0.7064881f);
        }

        public static void RemoveEntranceBarriers()
        {
            enterBarrier.Delete();
            enterBarrier = null;
            exitBarrier.Delete();
            exitBarrier = null;
        }

        public class StaticPedModel
        {
            public Vector3 Position { get; set; }
            public string Animation { get; set; }
            public int Heading { get; set; }
            public PedHash PedHash { get; set; }
        }
    }
}
