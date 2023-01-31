using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;
using XInputDotNetPure;
using System.Diagnostics;
using GTA.Native;
using System.Drawing;
using static HighBeam.AutobahnPropStreamer;

namespace HighBeam
{
    public static class NyApartment
    {
        private static Prop garageDoor = null;
        private static Stopwatch garageControlerKeyStopwatch = new Stopwatch();
        private static bool isGarageOpen = false;
        private static bool isGarageToggleInProgress = false;
        private static float garageRot = 0f;
        private static Vector3 garagePos;
        private static Vector3 defaultGaragePos;
        private static float garageForwardPos = 0f;
        private static float garageZ = 0f;
        private static bool init;
        private static Stopwatch rotDelay = new Stopwatch();
        private static Stopwatch zDelay = new Stopwatch();
        private static Stopwatch forwDelay = new Stopwatch();
        private static Vector3 liftFirstFloor = new Vector3(-1181.6f, -1722.3f, 5.5f);
        private static Vector3 liftLastFloor = new Vector3(-1180.2f, -1721.4f, 20.2f);
        private static Stopwatch afterLiftExitStopwatch = new Stopwatch();
        private static bool isLoaded = false;
        public static List<VehicleHash> vehicleHashes = new List<VehicleHash>() {
            VehicleHash.Minivan, VehicleHash.Asea, VehicleHash.Asterope,  VehicleHash.Primo, VehicleHash.Mesa, VehicleHash.Oracle, VehicleHash.Serrano, VehicleHash.Washington,
            VehicleHash.Baller2, VehicleHash.BJXL, VehicleHash.Dilettante, VehicleHash.Neon, VehicleHash.Khamelion, VehicleHash.Kuruma, VehicleHash.Oracle2, VehicleHash.Schwarzer,
            VehicleHash.Fugitive, VehicleHash.Bison,
            VehicleHash.Schafter2, VehicleHash.Ninef, VehicleHash.Elegy2, VehicleHash.Coquette, VehicleHash.RapidGT, VehicleHash.Carbonizzare, VehicleHash.Surano, VehicleHash.BestiaGTS
        };

        public static Random vrnd = new Random();
        public static List<Vehicle> vehicles = new List<Vehicle>();
        public static void RunNyAparment()
        {
            if (!isLoaded && Game.Player.Character.Position.DistanceTo(liftFirstFloor) < 160)
            {
                isLoaded = true;
                LoadNyApartment();
                LoadVehicles();
            }
            if (isLoaded && Game.Player.Character.Position.DistanceTo(liftFirstFloor) > 220)
            {
                isLoaded = false;
                RemoveNyApartment();
                RemoveVehicles();
            }
            Lift();
        }

        public class ParkingModel
        {
            public Vector3 Position { get; set; }
            public float Heading { get; set; }
            public int Model { get; set; } 
            public VehicleColor Color { get; set; }
            public string Plate { get; set; }

        }

        private static void AmbientLight()
        {
            var poss = new Vector3(1005.88f, -757.8f, 58.32f);
            if (Game.Player.Character.Position.DistanceTo(poss) < 2.5f)
                Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, poss.X, poss.Y, poss.Z, 197, 36, 126, 7f, 1.6f);
        }

        private static void Lift()
        {
            if (!afterLiftExitStopwatch.IsRunning)
            {
                var player = Game.Player.Character;
                //  UI.ShowSubtitle(player.Position.DistanceTo(liftFirstFloor).ToString());
                if (liftFirstFloor.DistanceTo(player.Position) < 0.5f && !afterLiftExitStopwatch.IsRunning)
                {
                    player.Position = liftLastFloor;
                    afterLiftExitStopwatch.Start();
                }
                if (liftLastFloor.DistanceTo(player.Position) < 0.5f && !afterLiftExitStopwatch.IsRunning)
                {
                    player.Position = liftFirstFloor;
                    afterLiftExitStopwatch.Start();
                }
            }
            else
            {
                if (afterLiftExitStopwatch.ElapsedMilliseconds > 10000)
                {
                    afterLiftExitStopwatch = new Stopwatch();
                }
            }
        }

        private static List<ParkingModel> parkingSpaces = new List<ParkingModel>()
        {
            new ParkingModel() { Position = new Vector3(-1178.8f, -1744.4f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1176.0f, -1742.1f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1173.3f, -1740.3f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1166.8f, -1735.7f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1164.0f, -1733.7f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1161.1f, -1731.6f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1158.2f, -1729.5f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1149.3f, -1742.9f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1152.3f, -1744.8f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1155.0f, -1746.8f, 3.5f), Heading = 35.8f},
            new ParkingModel() { Position = new Vector3(-1157.8f, -1748.9f, 3.5f), Heading = 35.8f},
        };

        private static Random headingRnd = new Random();
        private static Random carsToRemoveRnd = new Random();
        private static Random toDelIndexRnd = new Random();
        public static void LoadVehicles()
        {
            for (var i = 0; i < parkingSpaces.Count; i++)
            {
                var num = vrnd.Next(0, vehicleHashes.Count - 1);
                var mod = vehicleHashes[num];
                var p = parkingSpaces[i];
                var vehicl = World.CreateVehicle(mod, p.Position, headingRnd.Next(0, 10) > 5 ? p.Heading : (p.Heading + 180) % 360);
                vehicles.Add(vehicl);
            }
            var carsToRemoveCount = 0;
            var h = Main.fakeTimeHours;
            if (h > 20 || h < 7)
                carsToRemoveCount = carsToRemoveRnd.Next(0, 2);
            if (h > 6 && h < 21)
                carsToRemoveCount = carsToRemoveRnd.Next(8, 10);

            for (var i = 0; i < carsToRemoveCount; i++)
            {
                try
                {
                    var indx = toDelIndexRnd.Next(0, parkingSpaces.Count);
                    if (vehicles?[indx]?.Position.DistanceTo(Game.Player.Character.Position) < 1000)
                    {
                        vehicles[indx]?.Delete();
                        vehicles?.RemoveAt(indx);
                    }
                }
                catch
                {

                }

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

        private static void OperateGarageDoor()
        {
            if (isGarageToggleInProgress && garageDoor != null)
            {
                if (!init)
                {
                    forwDelay = new Stopwatch();
                    zDelay = new Stopwatch();
                    forwDelay.Start();
                    zDelay.Start();
                    init = true;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 81, "dryer", garageDoor, "CARWASH_SOUNDS", 0, 0);
                }
                bool isForward = isGarageOpen ? false : true;
                float zStep = 0.0020f;
                float forwardStep = 0.0021f;
                float tempZ = 0f;
                float tempForward = 0f;
                if (garageRot <= 90)
                {
                    garageRot = isForward ? garageRot + 0.08f : garageRot - 0.08f;
                }
                if (garageRot > 35f && garageZ <= 1.1f && (isForward || zDelay.ElapsedMilliseconds >= 2284))
                {
                    garageZ = isForward ? garageZ + zStep : garageZ - zStep;
                    tempZ = zStep;
                }
                if (garageRot > 30f && garageForwardPos <= 1.5f && (isForward || forwDelay.ElapsedMilliseconds >= 600))
                {
                    garageDoor.Rotation = new Vector3(0, garageDoor.Rotation.Y, garageDoor.Rotation.Z);
                    garageForwardPos = isForward ? garageForwardPos + forwardStep : garageForwardPos - forwardStep;
                    tempForward = forwardStep;
                }
                if (!isForward)
                {
                    if (garageZ < 0 || garageRot <= 35f)
                        garageZ = 0;
                    if (garageForwardPos < 0 || garageRot <= 30f)
                        garageForwardPos = 0;
                    if (garageRot < 0)
                        garageRot = 0;
                }
                garagePos = garageDoor.GetOffsetInWorldCoords(new Vector3(0f, isForward ? -tempForward : tempForward, isForward ? tempZ : -tempZ));
                if (garageRot >= 90f && garageForwardPos >= 1.5f && garageZ >= 1.1f && isForward)
                {
                    isGarageOpen = true;
                    isGarageToggleInProgress = false;
                    garageRot = 90f;
                    garageForwardPos = 1.5f;
                    garageZ = 1.1f;
                    init = false;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", garageDoor, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                }
                if (garageRot <= 0 && garageForwardPos <= 0 && garageZ <= 0 && !isForward)
                {
                    garageForwardPos = 0f;
                    garageZ = 0f;
                    garageRot = 0f;
                    garagePos = defaultGaragePos;
                    isGarageOpen = false;
                    isGarageToggleInProgress = false;
                    init = false;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", garageDoor, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                }
                garageDoor.Rotation = new Vector3(garageRot, garageDoor.Rotation.Y, garageDoor.Rotation.Z);
                garageDoor.Position = garagePos;
            }
        }

        public static void GarageDoor()
        {
            var garPos = new Vector3(6209.67236f, -2515.94629f, 31.64f);
            var dist = Game.Player.Character.Position.DistanceTo(garPos);
            if (dist < 120f)
            {
                if (garageDoor == null)
                {
                    garageDoor = World.CreateProp(new Model(30769481), new Vector3(0f, 0f, 0f), false, false);
                    garageDoor.FreezePosition = true;
                    garageDoor.Position = garPos;
                    garagePos = garageDoor.Position;
                    defaultGaragePos = garagePos;
                    garageDoor.Rotation = new Vector3(0f, 0f, -89.89989f);
                    garageRot = garageDoor.Rotation.X;
                    garageDoor.Quaternion = new Quaternion(0f, 0f, -0.7064888f, 0.7077242f);
                    LoadNyApartment();
                    LoadVehicles();
                }
            }
            else if (dist > 140f)
            {
                if (garageDoor != null)
                {
                    garageDoor.Delete();
                    garageDoor = null;
                    garageForwardPos = 0f;
                    RemoveVehicles();
                    RemoveNyApartment();

                }
            }
        }
    }
}

/*
 
  var dist = Game.Player.Character.Position.DistanceTo(new Vector3(1012.94f, -751.08f, 58.79f));
            if (dist < 60f)
            {
                if (garageDoor == null)
                {
                    garageDoor = World.CreateProp(new Model(30769481), new Vector3(0f, 0f, 0f), false, false);
                    garageDoor.FreezePosition = true;
                    garageDoor.Position = new Vector3(1012.94f, -751.08f, 58.79f);
                    garagePos = garageDoor.Position;
                    defaultGaragePos = garagePos;
                    garageDoor.Rotation = new Vector3(-0.04058862f, 0.000104278784f, -49.59975f);
                    garageRot = garageDoor.Rotation.X;
                    garageDoor.Quaternion = new Quaternion(-0.000321155676f, 0.000149396335f, -0.4194501f, 0.9077783f);
                    LoadHouse();

                }
            }*/
