using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.NyApartment;
using static HighBeam.AutobahnPropStreamer;
using static HighBeam.Main;

namespace HighBeam
{
    public static class Showroom
    {
        private static bool isShowroomLoaded;
        public static void RunShowroom()
        {
            // if (Game.IsControlJustPressed(0, Control.VehicleSelectNextWeapon))
            var pos = new Vector3(-1961.2f, 5664.4f, 10.9f);
           if (pos.DistanceTo(Game.Player.Character.Position) < 200 && !isShowroomLoaded)
            {
                isShowroomLoaded = true;
                LoadShowroom();
                LoadVehicles();
                LoadVehiclesShowroom();
                LoadPeds();
            }
           if(pos.DistanceTo(Game.Player.Character.Position) > 250 && isShowroomLoaded)
            {
                RemoveVehicles();
                RemoveShowroom();
                RemovePeds();
                isShowroomLoaded = false;
            }
            //  UI.ShowSubtitle(veh.Model.Hash.ToString());
        }

        public class PedModel
        {
            public Vector3 Position { get; set; }
            public float Heading { get; set; }
            public PedHash Model { get; set; }
            public string Task { get; set; }

        }

        private static List<PedModel> pedPos = new List<PedModel>()
        {
            new PedModel(){ Model = PedHash.Business01AMY, Task = "clip", Position = new Vector3(-2019.6f, 5676.5f, 10.8f)},
            new PedModel(){ Model = PedHash.AirworkerSMY, Task = "leaf", Position = new Vector3(-1961.2f, 5664.4f, 10.9f)},
             new PedModel(){ Model = PedHash.Bevhills01AMM, Task = "", Position = new Vector3(-1983.7f, 5687.4f, 10.8f), Heading = 166f},
              new PedModel(){ Model = PedHash.Business02AMY, Task = "tourist", Position = new Vector3(-1984.4f, 5686.2f, 10.8f)},
              new PedModel(){ Model = PedHash.Airhostess01SFY, Task = "", Position = new Vector3(-1996f, 5683.2f, 10.8f), Heading = 182f},
              new PedModel(){ Model = PedHash.Business03AMY, Task = "coffe", Position = new Vector3(-1996.2f, 5681.7f, 10.8f)},
              new PedModel(){ Model = PedHash.Business02AMY, Task = "wander", Position = new Vector3(-2019.6f, 5676.5f, 10.8f)},
               new PedModel(){ Model = PedHash.AirworkerSMY, Task = "wander", Position = new Vector3(-2019.6f, 5676.5f, 10.8f)},
                new PedModel(){ Model = PedHash.Business02AFY, Task = "wander", Position = new Vector3(-2019.6f, 5676.5f, 10.8f)}
        };

        private static List<PedHash> peds = new List<PedHash>() { PedHash.Business01AMY, PedHash.Business02AFY, PedHash.Business03AMY, PedHash.Business04AFY, PedHash.Business01AMM };
        private static List<Ped> showroomPeds = new List<Ped>();

        /*  if (a == "leaf blower")
              return "WORLD_HUMAN_GARDENER_LEAF_BLOWER";
          if (a == "drinking")
              return "world_human_drinking";
          if (a == "tourist")
              return "world_human_tourist_map";
          if (a == "smoke" || a == "smoke 2")
              return "world_human_aa_smoke";
          if (a == "tourist")
              return "world_human_tourist_map";
          if (a == "drug dealer")
              return "WORLD_HUMAN_DRUG_DEALER";
          if (a == "clipboard")
              return "WORLD_HUMAN_CLIPBOARD";
          if (a == "drug dealer hard")
              return "world_human_drug_dealer_hard";
          if (a == "hammering")
              return "WORLD_HUMAN_HAMMERING";*/
        private static void LoadPeds()
        {
            foreach (var p in pedPos)
            {
                var ped = World.CreatePed(new Model(p.Model), p.Position);
                ped.Heading = p.Heading;
                if (p.Task == "clip")
                {
                    ped.Task.StartScenario("WORLD_HUMAN_CLIPBOARD", ped.Position);
                }
                if (p.Task == "leaf")
                {
                    ped.Task.StartScenario("WORLD_HUMAN_GARDENER_LEAF_BLOWER", ped.Position);
                }
                if (p.Task == "tourist")
                {
                    ped.Task.StartScenario("world_human_tourist_map", ped.Position);
                }
                if (p.Task == "coffe")
                {
                    ped.Task.StartScenario("world_human_drinking", ped.Position);
                }
                if (p.Task == "wander")
                {
                    var seq1 = new TaskSequence();
                    var vehx = vehicles[vrnd.Next(0, 9)];
                    seq1.AddTask.WarpIntoVehicle(vehx, VehicleSeat.Driver);
                    seq1.AddTask.Wait(vrnd.Next(180000, 720000));
                    seq1.AddTask.LeaveVehicle(vehx, true);
                    seq1.AddTask.Wait(5000);
                    seq1.AddTask.GoTo(vehx.Position, true);
                    seq1.Close();
                    ped.AlwaysKeepTask = true;
                    ped.Task.PerformSequence(seq1);
                    seq1.Dispose();
                }
                showroomPeds.Add(ped);
            };
        }

        private static void RemovePeds()
        {
            foreach (var p in showroomPeds)
            {
                p.Delete();
            }
            showroomPeds = new List<Ped>();
        }

        private static List<ParkingModel> showroomCarSpaces = new List<ParkingModel>()
        {
            new ParkingModel() {
                Position = new Vector3(-2003.1f, 5685.8f, 10.5f),
                Heading = 102.4f,
                Model = 929136329,
                Color = VehicleColor.MetallicGracefulRed,
                Plate = "sk 8n291"
            },
              new ParkingModel() {
                Position = new Vector3(-1996.2f, 5687.2f, 10.5f),
                Heading = 102.4f,
                Model = 929136329,
                Color = VehicleColor.MetallicBlack,
                Plate = "sk 8n241"
            },
        };

        public static void LoadVehiclesShowroom()
        {
            for (var i = 0; i < showroomCarSpaces.Count; i++)
            {
                var num = vrnd.Next(0, vehicleHashes.Count - 1);

                var p = showroomCarSpaces[i];
                var mod = new Model(p.Model);
                var vehicl = World.CreateVehicle(mod, p.Position, p.Heading);
                vehicl.PrimaryColor = p.Color;
                vehicl.NumberPlate = p.Plate;
                vehicl.NumberPlateType = NumberPlateType.BlueOnWhite1;
                vehicl.InteriorLightOn = true;
                vehicles.Add(vehicl);
            }
        }

        private static List<ParkingModel> parkingSpaces = new List<ParkingModel>()
        {
            new ParkingModel() { Position = new Vector3(-1999.6f, 5667.7f, 10.5f), Heading = 1f},
             new ParkingModel() { Position = new Vector3(-1995.8f, 5667.7f, 10.5f), Heading = 1f},
              new ParkingModel() { Position = new Vector3(-1992.1f, 5667.1f, 10.5f), Heading = 1f},
               new ParkingModel() { Position = new Vector3(-1988.4f, 5667.3f, 10.5f), Heading = 1f},
                new ParkingModel() { Position = new Vector3(-1984.8f, 5667.1f, 10.5f), Heading = 1f},
                 new ParkingModel() { Position = new Vector3(-1981.1f, 5667.1f, 10.5f), Heading = 1f},
                  new ParkingModel() { Position = new Vector3(-1977.4f, 5667.2f, 10.5f), Heading = 1f},
                   new ParkingModel() { Position = new Vector3(-1973.6f, 5667.6f, 10.5f), Heading = 1f},
                    new ParkingModel() { Position = new Vector3(-1970f, 5667.7f, 10.5f), Heading = 1f},
                     new ParkingModel() { Position = new Vector3(-2022.5f, 5667.3f, 10.5f), Heading = 270f},
                      new ParkingModel() { Position = new Vector3(-2022.9f, 5671.2f, 10.5f), Heading = 270f},
                      new ParkingModel() { Position = new Vector3(-2022.2f, 5675f, 10.5f), Heading = 270f},
                      new ParkingModel() { Position = new Vector3(-2022.5f, 5678.7f, 10.5f), Heading = 270f},
                      new ParkingModel() { Position = new Vector3(-2022.2f, 5682.4f, 10.5f), Heading = 270f},
        };



        private static Random headingRnd = new Random();
        private static Random carsToRemoveRnd = new Random();
        private static Random toDelIndexRnd = new Random();
        private static List<Vehicle> vehicles = new List<Vehicle>();

        public static List<VehicleHash> vehicleHashesParking = new List<VehicleHash>() {
            VehicleHash.Asea, VehicleHash.Asterope,  VehicleHash.Primo, VehicleHash.Mesa, VehicleHash.Oracle, VehicleHash.Serrano, VehicleHash.Washington,
            VehicleHash.Baller2, VehicleHash.BJXL, VehicleHash.Dilettante, VehicleHash.Neon, VehicleHash.Khamelion, VehicleHash.Kuruma, VehicleHash.Oracle2, VehicleHash.Schwarzer,
            VehicleHash.Fugitive, VehicleHash.Bison,
            VehicleHash.Schafter2, VehicleHash.Ninef, VehicleHash.Elegy2, VehicleHash.Coquette, VehicleHash.RapidGT, VehicleHash.Carbonizzare, VehicleHash.Surano, VehicleHash.BestiaGTS
        };
        public static void LoadVehicles()
        {
            for (var i = 0; i < parkingSpaces.Count; i++)
            {
                var num = vrnd.Next(0, vehicleHashes.Count - 1);
                var mod = vehicleHashesParking[num];
                var p = parkingSpaces[i];
                var vehicl = World.CreateVehicle(mod, p.Position, headingRnd.Next(0, 10) < 6 ? p.Heading : (p.Heading + 180) % 360);
                vehicles.Add(vehicl);
            }
            var carsToRemoveCount = 0;
            var h = Main.fakeTimeHours;
            carsToRemoveCount = carsToRemoveRnd.Next(1, 3);

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
    }
}
