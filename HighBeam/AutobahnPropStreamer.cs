using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static HighBeam.Main;
using static HighBeam.HighwayTrafficOld;
using static HighBeam.NewHighwayTraffic.Index;
using static HighBeam.Plane;
using static HighBeam.Warehouse;
using GTA.Native;
using System.Drawing;

namespace HighBeam
{
    public static class AutobahnPropStreamer
    {
        public static List<PropModel> AutobahnPropsToRender = new List<PropModel>();
        public static PropFamilyModel AutobahnRenderedProps = new PropFamilyModel();
        public static PropFamilyModel BaseRenderedProps = new PropFamilyModel();
        public static List<PropModel> NyAirportPropsToRender = new List<PropModel>();
        public static PropFamilyModel NyAirportRenderedProps = new PropFamilyModel();
        public static List<PropModel> WarePropsToRender = new List<PropModel>();
        public static PropFamilyModel WareRenderedProps = new PropFamilyModel();
        public static List<PropModel> PaletoOfficeLiftToRender = new List<PropModel>();
        public static PropFamilyModel PaletoOfficeRenderedProps = new PropFamilyModel();
        public static List<PropModel> ContainerWarePropsToRender = new List<PropModel>();
        public static PropFamilyModel ContainerWareRenderedProps = new PropFamilyModel();

        public static List<PropModel> S7PropsToRender = new List<PropModel>();
        public static PropFamilyModel S7RenderedProps = new PropFamilyModel();

        public static List<PropModel> HousePropsToRender = new List<PropModel>();
        public static PropFamilyModel HouseRenderedProps = new PropFamilyModel();

        public static List<PropModel> RoadConsPropsToRender = new List<PropModel>();
        public static PropFamilyModel RoadConsRenderedProps = new PropFamilyModel();

        public static List<PropModel> NyApartsPropsToRender = new List<PropModel>();
        public static PropFamilyModel NyApartsRenderedProps = new PropFamilyModel();

        public static List<PropModel> NyShopPropsToRender = new List<PropModel>();
        public static PropFamilyModel NyShopRenderedProps = new PropFamilyModel();

        public static List<PropModel> ShowroomPropsToRender = new List<PropModel>();
        public static PropFamilyModel ShowroomRenderedProps = new PropFamilyModel();

        public static List<PropModel> TrainStationsPropsToRender = new List<PropModel>();
        public static PropFamilyModel TrainStationsRenderedProps = new PropFamilyModel();

        public static bool isXmlLoaded = false;
        public static bool isAutobahnRendered = false;
        public static bool isPaletoOfficeRenderd = false;
        public static Stopwatch RenderAutobahnStopWatch = new Stopwatch();
        public static bool isTruckBaseRendered = false;
        public static bool isNyAirportRenedered = false;
        public static Stopwatch isInLiftStopWatch = new Stopwatch();
        public static Stopwatch afterLiftExitStopwatch = new Stopwatch();
        public static bool isLiftRunning = false;
        public static StepModel currentStep = null;
        public static float currentLiftZ = 0;
        public static Prop currentLiftProp;
        public static Prop beachBridgeLightRef = null;
        public static Prop beachSideWallLightRef = null;
        public static bool init = false;
        public static bool isWarehouseLoaded = false;
        public static List<Color> colors = new List<Color>() {
            Color.FromArgb(255, 95, 1),
            Color.FromArgb(0, 32, 127),
            Color.FromArgb(233, 39, 91)
        };
        public static int colorRnd = new Random().Next(0, colors.Count - 1);
        public static bool isS7Loaded = false;
        public class StepModel
        {
            public int step;
            public float fromZ;
            public float toZ;

        }
        public static void RunAutobahnPropStreamer()
        {

            if (!isXmlLoaded)
            {
                isXmlLoaded = true;
                // LoadCons();
            }
        }

        public static void LoadWare()
        {
            List<string> chunks = new List<string>() {
                    "new_warehouse_paleto_old.xml",
                    "ware_ambient_lights.xml",
                    "ware_content.xml",
                  //  "ware_truck_helpers.xml",
                    "ware_office_may2.xml",
                    "ware_parking_outside.xml",
                    "ware_bollards_palletes.xml",
                    "ware_wash.xml",
                     "ware_truck_ramp.xml",
                     "ware_factory.xml"
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            WarePropsToRender = l;
            Render(WarePropsToRender, WareRenderedProps);
        }

        public static void RemoveWare()
        {
            Delete(WareRenderedProps);
            WarePropsToRender = new List<PropModel>();
        }

        public static void LoadNyWare()
        {
            List<string> chunks = new List<string>() {
                    "ny_ware.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            WarePropsToRender = l;
            Render(WarePropsToRender, WareRenderedProps);
        }

        public static void RemoveNyWare()
        {
            Delete(WareRenderedProps);
            WarePropsToRender = new List<PropModel>();
        }

        public static void LoadContainerWare()
        {
            List<string> chunks = new List<string>() {
                    "container_ware.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            ContainerWarePropsToRender = l;
            Render(ContainerWarePropsToRender, ContainerWareRenderedProps);
        }

        public static void RemoveContainerWare()
        {
            Delete(ContainerWareRenderedProps);
            ContainerWarePropsToRender = new List<PropModel>();
        }

        public static void LoadNyShop()
        {
            List<string> chunks = new List<string>() {
                    "ny_shop.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            NyShopPropsToRender = l;
            Render(NyShopPropsToRender, NyShopRenderedProps);
        }

        public static void RemoveNyShop()
        {
            Delete(NyShopRenderedProps);
            NyShopPropsToRender = new List<PropModel>();
        }

        public static void LoadTrainStations()
        {
            List<string> chunks = new List<string>() {
                    "train_stations.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            TrainStationsPropsToRender = l;
            Render(TrainStationsPropsToRender, TrainStationsRenderedProps);
        }

        public static void RemoveTrainStations()
        {
            Delete(TrainStationsRenderedProps);
            TrainStationsPropsToRender = new List<PropModel>();
        }

        public static void LoadShowroom()
        {
            List<string> chunks = new List<string>() {
                    "showroom.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            ShowroomPropsToRender = l;
            Render(ShowroomPropsToRender, ShowroomRenderedProps);
        }

        public static void RemoveShowroom()
        { 
            Delete(ShowroomRenderedProps);
            ShowroomPropsToRender = new List<PropModel>();
        }

        public static void LoadNyApartment()
        {
            List<string> chunks = new List<string>() {
                    "vb.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            NyApartsPropsToRender = l;
            Render(NyApartsPropsToRender, NyApartsRenderedProps);
        }

        public static void RemoveNyApartment()
        {
            Delete(NyApartsRenderedProps);
            NyApartsPropsToRender = new List<PropModel>();
        }

        public static void LoadS7()
        {
            List<string> chunks = new List<string>() {
                    "s7_addons.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            S7PropsToRender = l;
            UI.Notify("S7 loaded");
            Render(S7PropsToRender, S7RenderedProps);
        }

        public static void RemoveS7()
        {
            Delete(S7RenderedProps);
            UI.Notify("S7 removed");
            S7PropsToRender = new List<PropModel>();
        }



        public static void LoadHouse()
        {
            List<string> chunks = new List<string>() {
                    "new_house_big.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            HousePropsToRender = l;
            Render(HousePropsToRender, HouseRenderedProps);
        }

        public static void LoadCons()
        {
            List<string> chunks = new List<string>() {
                    "s1_cons.xml",
                    "s1_cons2.xml",
                };
            List<PropModel> l = new List<PropModel>();
            for (var i = 0; i < chunks.Count; ++i)
            {
                l.AddRange(ReadXml(chunks[i]));
            }
            //   autobahn.AddRange(trees);
            RoadConsPropsToRender = l;
            Render(RoadConsPropsToRender, RoadConsRenderedProps);
        }

        public static void RemoveHouse()
        {
            Delete(HouseRenderedProps);
            HousePropsToRender = new List<PropModel>();
        }

        private static void RenderRefPointsLight(bool delete = false)
        {
            if (delete)
            {
                beachBridgeLightRef.Delete();
                beachSideWallLightRef.Delete();
                beachBridgeLightRef = null;
                beachSideWallLightRef = null;
            }
            else
            {
                beachBridgeLightRef = World.CreateProp(new Model(-1038739674), new Vector3(-1970.310f, -469.86f, 19.4f), new Vector3(0, 0, 50f), true, true);
                beachBridgeLightRef.Alpha = 0;
                beachSideWallLightRef = World.CreateProp(new Model(-1038739674), new Vector3(-1903.240f, -509.3f, 11.8f), new Vector3(0, 0, 50.7f), true, true);
                beachSideWallLightRef.Alpha = 0;
            }

        }

        private static void RenderAutobahmLights()
        {
            if (beachSideWallLightRef != null && beachBridgeLightRef != null)
            {
                if (beachBridgeLightRef.Position.DistanceTo(Main.veh.Position) < 600)
                {
                    // ped bridge 
                    var color = colors[colorRnd];
                    int off = -35;
                    for (var i = 0; i < 13; i++)
                    {
                        var v = beachBridgeLightRef.GetOffsetInWorldCoords(new Vector3(off, 2, -2f));
                        var v2 = beachBridgeLightRef.GetOffsetInWorldCoords(new Vector3(off, -2, -2f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, v.X, v.Y, v.Z, color.R, color.G, color.B, 5f, 10f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, v2.X, v2.Y, v2.Z, color.R, color.G, color.B, 5f, 10f);
                        off += 5;
                    }
                    //  wall 
                    int off1 = 0;
                    for (var i = 0; i < 17; i++)
                    {
                        var v = beachSideWallLightRef.GetOffsetInWorldCoords(new Vector3(0f, off1, 0f));
                        var v2 = beachSideWallLightRef.GetOffsetInWorldCoords(new Vector3(-24.5f, off1, 0f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, v.X, v.Y, v.Z, color.R, color.G, color.B, 10f, 10f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, v2.X, v2.Y, v2.Z, color.R, color.G, color.B, 10f, 10f);
                        off1 += 10;
                    }
                }
            }
        }

        private static void CheckIfPlayerIsCloseToAutobahn()
        {
            //  var t1 = PointInTriangle(x, y, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY);
            //  var t2 = PointInTriangle(x, y, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY);
            //  var t3 = PointInTriangle(x, y, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.FinishLeftX, zone.ZoneBoundary.FinishLeftY);
            //  var t4 = PointInTriangle(x, y, zone.ZoneBoundary.StartRightX, zone.ZoneBoundary.StartRightY, zone.ZoneBoundary.StartLeftX, zone.ZoneBoundary.StartLeftY, zone.ZoneBoundary.FinishRightX, zone.ZoneBoundary.FinishRightY);
            //   if (t1 || t2 || t3 || t4)
            //   {

            // }
        }

        public static List<PropModel> ReadXml(string dir, int maxIndex = 0)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Directory.GetCurrentDirectory().ToString() + "/" + dir);
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("//Map/Objects/MapObject");
                var elements = new List<PropModel>();
                for (var i = 0; i < (maxIndex > 0 ? maxIndex : nodes.Count); ++i)
                {
                    XmlNode node = nodes[i];
                    if (node.Name == "MapObject")
                    {
                        PropModel el;
                        var dynamic = node["Dynamic"].InnerText.ToString();
                        var door = node["Door"].InnerText.ToString();

                        el = new PropModel()
                        {
                            Type = node["Type"].InnerText,
                            Position = new Vector3(float.Parse(node["Position"].ChildNodes[0].InnerText.ToString(), CultureInfo.InvariantCulture), float.Parse(node["Position"].ChildNodes[1].InnerText.ToString(), CultureInfo.InvariantCulture), float.Parse(node["Position"].ChildNodes[2].InnerText.ToString(), CultureInfo.InvariantCulture)),
                            Rotation = new Vector3(float.Parse(node["Rotation"].ChildNodes[0].InnerText.ToString(), CultureInfo.InvariantCulture), float.Parse(node["Rotation"].ChildNodes[1].InnerText.ToString(), CultureInfo.InvariantCulture), float.Parse(node["Rotation"].ChildNodes[2].InnerText.ToString(), CultureInfo.InvariantCulture)),
                            Hash = int.Parse(node["Hash"].InnerText.ToString()),
                            Dynamic = dynamic == "true" ? true : false,
                            Door = door == "true" ? true : false,
                            PedAction = node["Action"]?.ChildNodes[0]?.InnerText.ToString(),
                            Quaternion = new Quaternion(float.Parse(node["Quaternion"].ChildNodes[0].InnerText.ToString(), CultureInfo.InvariantCulture), float.Parse(node["Quaternion"].ChildNodes[1].InnerText.ToString(), CultureInfo.InvariantCulture), float.Parse(node["Quaternion"].ChildNodes[2].InnerText.ToString(), CultureInfo.InvariantCulture), float.Parse(node["Quaternion"].ChildNodes[3].InnerText.ToString(), CultureInfo.InvariantCulture)),
                        };
                        elements.Add(el);
                    }
                }
                return elements;
            }
            catch (Exception e)
            {
                UI.Notify(e.ToString());
                return new List<PropModel>();
            }

        }

        public static void RenderTruckBase()
        {
            var dist = HighBeam.Main.veh.Position.DistanceTo(new Vector3(-1091.893f, -2214.289f, 13.2f));
            if (dist < 30 && !isTruckBaseRendered)
            {
                var vehs = World.GetAllVehicles();
                Vector3 refpoint = new Vector3(-1102.828f, -2140.918f, 13.9f);
                foreach (var v in vehs)
                {
                    if (refpoint.DistanceTo(v.Position) <= 30)
                    {
                        v.MarkAsNoLongerNeeded();
                        v.Position = new Vector3(0, 0, 0);
                        v.Delete();
                    }
                }
                isTruckBaseRendered = true;
                Render(ReadXml("new_warehouse_addons_active.xml"), BaseRenderedProps);
                UI.Notify("rendered base");
            }
            if (dist > 150 && isTruckBaseRendered)
            {
                isTruckBaseRendered = false;
                Delete(BaseRenderedProps);
                UI.Notify("deleted base");
            }
        }

        public static void RenderWarehouse()
        {
            Vector3 warehousePos = new Vector3(150.0f, 6430.44f, 31.2f);
            var dist = Main.veh.Position.DistanceTo(warehousePos);
            if (dist < 150 && !isWarehouseLoaded)
            {
                isWarehouseLoaded = true;
                var propList = new List<PropModel>();
                var l1 = ReadXml("ware_bollards_palletes.xml");
                var l2 = ReadXml("ware_content.xml");
                var l3 = ReadXml("ware_lines.xml");
                var l4 = ReadXml("ware_office.xml");
                var l5 = ReadXml("ware_shower.xml");
                var l6 = ReadXml("new_warehouse_paleto.xml");
                // var trees = ReadXml("trees.xml"); 
                propList.AddRange(l1);
                propList.AddRange(l2);
                propList.AddRange(l3);
                propList.AddRange(l4);
                propList.AddRange(l5);
                propList.AddRange(l6);
                Render(propList, BaseRenderedProps);
            }
            if (dist > 200 && isWarehouseLoaded)
            {
                isWarehouseLoaded = false;
                Delete(BaseRenderedProps);
            }
        }

        public static void RenderS7()
        {
            //   UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
            Vector3 loadPoint = new Vector3(-787.8f, 5624.4f, 26.2f);
            var dis = Game.Player.Character.Position.DistanceTo(loadPoint);
            if (dis < 20)
            {
                var heading = Game.Player.Character.Heading;
                if (heading < 200f && heading > 110f && !isS7Loaded)
                {
                    isS7Loaded = true;
                    LoadS7();
                }
                if (heading > 310f && heading < 360f && isS7Loaded)
                {
                    isS7Loaded = false;
                    RemoveS7();
                }
            }

        }

        public static void RenderPaletoOffice()
        {
            // UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
            var distToOff = HighBeam.Main.veh.Position.DistanceTo(new Vector3(79.12f, 6538.77f, 13.2f));
            if (!isPaletoOfficeRenderd && distToOff < 300)
            {
                isPaletoOfficeRenderd = true;
                Render(ReadXml("lift.xml"), PaletoOfficeRenderedProps, isLift: true);
                UI.Notify("rendered office");
            }
            if (isPaletoOfficeRenderd && distToOff > 300)
            {
                isPaletoOfficeRenderd = false;
                Delete(PaletoOfficeRenderedProps);
                UI.Notify("deleted office");
            }
            if (isPaletoOfficeRenderd)
            {
                var dist = Game.Player.Character.Position.DistanceTo(new Vector3(79.12f, 6538.77f, Game.Player.Character.Position.Z));
                if (dist < 1f && !isInLiftStopWatch.IsRunning && !isLiftRunning)
                {
                    isInLiftStopWatch = new Stopwatch();
                    isInLiftStopWatch.Start();
                }
                if (afterLiftExitStopwatch.IsRunning)
                {
                    isInLiftStopWatch = new Stopwatch();
                }
                if (afterLiftExitStopwatch.IsRunning && afterLiftExitStopwatch.ElapsedMilliseconds > 9999)
                {
                    afterLiftExitStopwatch = new Stopwatch();
                }
                if (dist > 1f)
                {
                    isInLiftStopWatch = new Stopwatch();
                }
                if (isInLiftStopWatch.IsRunning && isInLiftStopWatch.ElapsedMilliseconds > 500)
                {
                    isLiftRunning = true;
                }
                if (isLiftRunning && currentStep == null)
                {
                    posToStep();
                    isInLiftStopWatch = new Stopwatch();
                }
                if (isLiftRunning && currentStep != null)
                {
                    runLift();
                }
            }
            // isTruckBaseRendered = false;
            // Delete(BaseRenderedProps);
            //  UI.Notify("deleted base");

            // UI.ShowSubtitle(dist.ToString());
        }

        private static void posToStep()
        {
            var z = Game.Player.Character.Position.Z;
            if (z > 32.1f && z < 32.6f)
            {
                currentStep = new StepModel() { step = -1, fromZ = 32.2f, toZ = 65.0f };
            }
            else if (z > 64.4f && z < 65.9f)
            {
                currentStep = new StepModel() { step = 8, fromZ = 65.0f, toZ = 32.2f };
            }
            else
            {
                currentStep = new StepModel() { step = 0, fromZ = 0f, toZ = 0f };
            }
        }

        private static void runLift()
        {
            var liftSpeed = 0.03f;
            if (currentLiftZ == 0f)
            {
                currentLiftZ = currentStep.fromZ - 1.8f;
            }
            currentLiftZ = (currentStep.fromZ < currentStep.toZ ? (currentLiftZ + liftSpeed) : (currentLiftZ - liftSpeed));
            if (currentStep.fromZ < currentStep.toZ ? (currentLiftZ >= currentStep.toZ) : (currentLiftZ <= (currentStep.toZ + 1.9f)) || currentLiftZ <= 30.0f)
            {
                isLiftRunning = false;
                afterLiftExitStopwatch = new Stopwatch();
                afterLiftExitStopwatch.Start();
                currentStep = null;
            }
            currentLiftProp.Position = new Vector3(currentLiftProp.Position.X, currentLiftProp.Position.Y, currentLiftZ);
        }

        private static void Delete(PropFamilyModel propFamily)
        {
            for (var i = 0; i < propFamily.Vehicles.Count; ++i)
            {
                var toDel = propFamily.Vehicles[i];
                toDel.Delete();
                toDel.MarkAsNoLongerNeeded();
                toDel.Position = new Vector3(0, 0, 0);
            }
            for (var i = 0; i < propFamily.Peds.Count; ++i)
            {
                var toDel = propFamily.Peds[i];
                toDel.Delete();
                toDel.MarkAsNoLongerNeeded();
                toDel.Position = new Vector3(0, 0, 0);
            }
            for (var i = 0; i < propFamily.Props.Count; ++i)
            {
                var toDel = propFamily.Props[i];
                toDel.Delete();
                toDel.MarkAsNoLongerNeeded();
                toDel.Position = new Vector3(0, 0, 0);
            }
            propFamily = new PropFamilyModel();
        }

        public static double RadianToDegree(double angle) { return 180.0 * angle / Math.PI; }

        private static void Render(List<PropModel> toRender, PropFamilyModel propFamily, bool isLift = false)
        {
            try
            {
                propFamily.Props = new List<Prop>();
                propFamily.Vehicles = new List<Vehicle>();
                propFamily.Peds = new List<Ped>();
                for (var i = 0; i < toRender.Count; ++i)
                {

                    PropModel el = toRender[i];
                    if (el.Type.ToLower() == "vehicle")
                    {
                        var vehicle = World.CreateVehicle(new Model(el.Hash), el.Position, el.Rotation.Z);
                        vehicle.PlaceOnGround();
                        propFamily.Vehicles.Add(vehicle);
                    }
                    if (el.Type.ToLower() == "ped")
                    {
                        var ped = World.CreatePed(new Model(el.Hash), el.Position, el.Rotation.Z);
                        Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, convertPedAction(el.PedAction.ToLower()), -1, false);
                        propFamily.Peds.Add(ped);
                    }
                    if (el.Type.ToLower() == "prop")
                    {
                        Prop prop = new Prop(Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, el.Hash, el.Position.X, el.Position.Y, el.Position.Z, true, true, false));
                        prop.FreezePosition = (el.Hash == -984871726 ? true : (el.Dynamic ? false : (el.Door ? false : true)));
                        Function.Call(Hash.SET_ENTITY_QUATERNION, prop.Handle, el.Quaternion.X, el.Quaternion.Y, el.Quaternion.Z, el.Quaternion.W);
                        prop.LodDistance = 2000;
                        propFamily.Props.Add(prop);
                        if (isLift)
                            currentLiftProp = prop;
                    }

                }
            }
            catch (Exception e)
            {
                UI.ShowSubtitle(e.Message);
            }

        }

        public static string convertPedAction(string a)
        {
            if (a == "leaf blower")
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
                return "WORLD_HUMAN_HAMMERING";
            return "";
        }

        /*   "world_human_aa_coffee",
             "world_human_aa_smoke",
             "world_human_car_park_attendant",
             "world_human_drinking",
             "world_human_drug_dealer_hard",
             "world_human_guard_patrol",
             "world_human_hang_out_street",
             "world_human_hiker_standing",
             "world_human_jog_standing",
             "world_human_mobile_film_shocking",
             "world_human_stand_guard",
             "world_human_stand_impatient",
             "world_human_stand_mobile_fat", 
             "world_human_stand_mobile",
             "world_human_tourist_map",
             "world_human_tourist_map",
             "world_human_tourist_mobile",*/

        public class PropModel
        {
            public Vector3 Position { get; set; }
            public Vector3 Rotation { get; set; }
            public int Hash { get; set; }
            public bool Dynamic { get; set; }
            public bool Door { get; set; }
            public Quaternion Quaternion { get; set; }
            public string Type { get; set; }
            public string PedAction { get; set; }
            public float Heading { get; set; }
        }

        public class PropFamilyModel
        {
            public List<Vehicle> Vehicles { get; set; }
            public List<Ped> Peds { get; set; }
            public List<Prop> Props { get; set; }
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
    }
}
