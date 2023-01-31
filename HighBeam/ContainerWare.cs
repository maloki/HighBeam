using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.Main;
using static HighBeam.AutobahnPropStreamer;
using System.Diagnostics;

namespace HighBeam
{
    public class ContainerWare
    {
        public static bool isContainerWarehouse = false;
        private static int currentHour = 0;
        private static Vector3 containerWarePos = new Vector3(-277.401367f, 6120.839f, 30.32772f);

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
        public static Random r = new Random();
        public static bool isCollisionsRemoved = false;


        public static List<Prop> ContainerList = new List<Prop>();
        private static List<Vehicle> VehiclesList = new List<Vehicle>();
        private static List<Vector3> ContainerPostions = new List<Vector3>()
        {
            new Vector3(-309.8846f, 6111.471f, 32.48f),
            new Vector3(-305.545532f, 6107.241f, 32.48f),
            new Vector3(-300.680878f, 6102.873f, 32.48f)
        };

        public static int GenerateRandomNumberBetween(int min, int max, bool isLookingRnd = false)
        {
            // using second random generator if function is called from the multiple threads at the same time, to prevent generating the same numbers
            return r.Next(min, max);
        }

        public static void RunContainerWarehouse()
        {

            // UI.ShowSubtitle($"{VehiclesList.Count}");
            try
            {
                currentHour = fakeTimeHours;
                //UI.ShowSubtitle(veh.Model.Hash.ToString());

                //  UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
                if (veh.Position.DistanceTo(containerWarePos) < 70f && !isContainerWarehouse)
                {
                    UI.Notify("Container loaded");
                    LoadContainerWare();
                    LoadAllProps();
                    ToggleGate();
                }
                if (veh.Position.DistanceTo(containerWarePos) > 90f && isContainerWarehouse)
                {
                    UI.Notify("Container removed");
                    RemoveContainerWare();
                    RemoveAllProps();
                    isPropsDeleted = false;
                }
                if (isContainerWarehouse)
                {
                    if (!isCollisionsRemoved && truckTrailer != null)
                        RemoveCollisions();
                    if (Game.IsControlJustPressed(0, Control.Phone) && false)
                    {
                        ToggleGate();
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
                    ManageContainerOnTrailer();
                }
            }
            catch (Exception e)
            {
                UI.ShowSubtitle(e.Message);
            }
        }

        public static void LoadAllProps()
        {
            isContainerWarehouse = true;
            LoadGate();
            LoadContainers();
            LoadVehicles();
        }

        public static void RemoveAllProps()
        {
            isContainerWarehouse = false;
            RemoveContainers();
            RemoveGate();
            RemoveVehicles();
            isCollisionsRemoved = false;
        }

        private static void LoadVehicles()
        {
            // trailer
            var pos = new Vector3(-314f, 6117.26f, 31.8f);
            var v = World.CreateVehicle(new Model(-877478386), pos, 309f);
            v.ToggleExtra(2, false);
            v.ToggleExtra(3, false);
            v.ToggleExtra(4, false);
            v.ToggleExtra(1, false);
            v.ToggleExtra(5, false);
            v.PlaceOnGround();
            if (truckTrailer != null)
                v.Delete();
            else
                VehiclesList.Add(v);
        }

        private static void RemoveVehicles()
        {
            for (var i = 0; i < VehiclesList.Count; ++i)
            {
                if (truckTrailer != null)
                {
                    if (!VehiclesList[i].IsAttached())
                    {
                        VehiclesList[i].Delete();
                    }
                }
                else
                {
                    VehiclesList[i].Delete();
                }
            }
            VehiclesList = new List<Vehicle>();
        }

        public static void ManageContainerOnTrailer()
        {
            if (truckTrailer != null)
            {
                if (!Game.Player.Character.IsInVehicle(veh)
                                  && Game.Player.Character.Position.DistanceTo(truckTrailer.Position) < 4
                                  && Game.IsControlJustPressed(0, GTA.Control.ScriptPadRight) && truckTrailer != null)
                {
                    Vector3 closestContPos = new Vector3();
                    for (var i = 0; i < ContainerPostions.Count; ++i)
                    {
                        if (truckTrailer.Position.DistanceTo(ContainerPostions[i]) < truckTrailer.Position.DistanceTo(closestContPos))
                            closestContPos = ContainerPostions[i];
                    }
                    if (isContainerAttached)
                    {
                        truckTrailerContainer.Detach();
                        ContainerList.Add(truckTrailerContainer);
                        truckTrailerContainer.Position = closestContPos;
                        truckTrailerContainer.FreezePosition = true;
                        truckTrailerContainer = null;
                        isContainerAttached = false;
                    }
                    else 
                    {
                        containerPos = new Vector3(0, -1.15f, -1.41f);
                        truckTrailerContainer = World.GetNearbyProps(closestContPos, 3f).Where(c => containerList.Any(cl => cl == c.Model.Hash)).FirstOrDefault();
                        truckTrailerContainer.AttachTo(truckTrailer, 0, containerPos, new Vector3(0, 0, 0));
                        truckTrailerContainer.FreezePosition = false;
                        ContainerList.Remove(truckTrailerContainer);
                        isContainerAttached = true;
                    }
                }
            }
        }

        private static List<int> NoCollisionList = new List<int>()
        {
            -475521732
        };


        public static void RemoveCollisions()
        {
            if (truckTrailer != null)
            {
                for (var i = 0; i < ContainerWareRenderedProps.Props.Count; ++i)
                {
                    var prop = ContainerWareRenderedProps.Props[i];

                    if (NoCollisionList.Any(c => c == prop.Model.Hash))
                    {
                        prop.SetNoCollision(truckTrailer, true);
                    }
                }
                for (var i = 0; i < ContainerList.Count; ++i)
                {
                    var prop = ContainerList[i];

                    if (containerList.Any(c => c == prop.Model.Hash))
                    {
                        prop.SetNoCollision(truckTrailer, true);
                    }
                }
            }
            isCollisionsRemoved = true;
        }

        public static void LoadContainers()
        {
            for (var i = 0; i < ContainerPostions.Count; ++i)
            {
                if (GenerateRandomNumberBetween(0, 9) % 3 == 0)
                {
                    var pos = ContainerPostions[i];
                    var container = World.CreateProp(new Model(containerList[GenerateRandomNumberBetween(0, containerList.Count - 1)]), pos, new Vector3(0f, 0f, -45.09992f), false, false);
                    container.FreezePosition = true;
                    ContainerList.Add(container);
                }
            }
        }

        public static void RemoveContainers()
        {
            for (var i = 0; i < ContainerList.Count; ++i)
            {
                ContainerList[i].Delete();
            }
            ContainerList = new List<Prop>();
        }

        public static void LoadGate()
        {
            gate = World.CreateProp(new Model(1286535678), new Vector3(-277.401367f, 6120.839f, 30.32772f), new Vector3(1.00179086E-05f, -5.00895339E-06f, 44.5248833f), false, false);
            gate.FreezePosition = true;
        }

        public static void ToggleGate()
        {
            isGateOpen = !isGateOpen;
            gateMovingStopwatch = new Stopwatch();
            gateMovingStopwatch.Start();
            isGateMoving = true;
        }
        public static void RemoveGate()
        {
            gate.Delete();
            gate = null;
            gateX = 0;
            isGateMoving = false;
            isGateOpen = false;
        }
    }
}
