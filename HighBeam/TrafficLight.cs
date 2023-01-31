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
using static HighBeam.AutobahnPropStreamer;
using static HighBeam.Main;
namespace HighBeam
{

    public static class TrafficLight
    {
        public static bool init = false;
        public static List<TrafficLightModel> traffLights = new List<TrafficLightModel>();
        public static List<IntersectionModel> intersections = new List<IntersectionModel>();
        private static int redBulbHash = 754816039;
        private static int yellowBulbHash = -1384835816;
        private static int greenBulbHash = 140790497;
        private static Stopwatch test = new Stopwatch();
        private static int tests = 0;
        private static Stopwatch trafficLightTickStopwatch = new Stopwatch();
        private static int id = 0;
        private static IntersectionModel currentIntersection = new IntersectionModel() { Name = "defaultintrer", isFake = true };
        private static List<IntersectionModel> intersectionList = new List<IntersectionModel>() {
            new IntersectionModel(){ Name = "sandy1", LoadPoint = new Vector3(2649.3418f, 5111.686f, 43.779f), LOD = 400f },
            new IntersectionModel(){ Name = "sandy2",    LoadPoint = new Vector3(2767.77515f, 4406.957f, 47.8402023f), LOD = 400f },
            new IntersectionModel(){ Name = "s7_north", LoadPoint = new Vector3(-773.0f, 5488.6f, 34.5f), LOD = 400f },
            new IntersectionModel(){ Name = "a4_0_wro_exit_traffic_lights", LoadPoint = new Vector3(253.3f, -2663.2f, 17.9f), LOD = 150f } 
        };
        public static bool isInCurrentIntersection = false;
        private static int currentHour = 0;
        private static bool isNightMode = false;
        private static Stopwatch nightModeTickStopwatch = new Stopwatch();
        private static int nightModeLightState = 0;
        private static bool justEnterIntersection = false;
        public static Random switchPatternRnd = new Random();
        public static Random switchPatternMoreRnd = new Random();
        public static Random alreadyElapsedTimeRnd = new Random();
        public static int currentLightStatus = 0;
        public static bool isIntersectionRendered = false;
        public static int alreadyElapsedTime = 0;
        private static readonly object syncLock = new object();
        public static void RunTrafficLightSystem()
        {
            try
            {
                currentHour = fakeTimeHours;
                var wasNightMode = isNightMode;
                if (currentHour > 22 || currentHour < 7)
                    isNightMode = true;
                else  
                    isNightMode = false;
                var toLoadInter = intersectionList.Where(inter => inter.LoadPoint.DistanceTo(veh.Position) < inter.LOD).FirstOrDefault();
                if (toLoadInter != null
                    && !isIntersectionRendered)
                {
                    isIntersectionRendered = true;
                    RenderAllContent(toLoadInter);
                }
                if (!currentIntersection.isFake && veh.Position.DistanceTo(currentIntersection.LoadPoint) > (currentIntersection.LOD + 10f)
                    && isIntersectionRendered)
                {
                    isIntersectionRendered = false;
                    RemoveAllContent();
                }
                if (isIntersectionRendered)
                {
                    if (!wasNightMode && isNightMode)
                    {
                        RemoveAllSpeedZones(currentIntersection);
                    }
                    if (wasNightMode && !isNightMode)
                    {
                        SwitchLight("main", "straight", BlipColor.Red, currentIntersection.Name);
                        SwitchLight("main", "left", BlipColor.Red, currentIntersection.Name);
                        SwitchLight("side", "straight", BlipColor.Red, currentIntersection.Name);
                    }
                    if (isNightMode)
                        NightMode();
                    TrafficLightRefresh();
                    if (Game.IsControlJustReleased(0, GTA.Control.Jump) && false)
                    {
                        RemoveAllContent();
                    }
                    if (Game.IsControlJustReleased(0, GTA.Control.Sprint) && false)
                    {
                        if (id > 0)
                        {
                            Function.Call((Hash)0x1033371FC8E842A7, id);
                            id = 0;
                            UI.ShowSubtitle("remove");
                        }
                        else
                        {
                            var p = veh.GetOffsetInWorldCoords(new Vector3(0, 15f, 0));
                            id = Function.Call<int>((Hash)0x2CE544C68FB812A0, p.X, p.Y, p.Z, 15f, 0f, false);
                            UI.ShowSubtitle("active");
                        }

                        /*  if (tests == 0)
                          {
                              SwitchLight("main", "straight", BlipColor.Red);
                          }
                          if (tests == 1)
                          {
                              SwitchLight("main", "straight", BlipColor.Yellow);
                          }
                          if (tests == 2)
                          {
                              SwitchLight("main", "straight", BlipColor.Green);
                          }*/
                        tests++;
                    }
                    if (traffLights.Count > 0 && false)
                    {
                        intersections.ForEach(inter => inter.TrafficLights.ForEach(tl => tl.BulbsSet.ForEach(bs => bs.SlowDownZones.ForEach(sdz =>
                        {
                            var color = sdz.Id == 0 ? Color.FromArgb(0, 255, 0) : Color.FromArgb(255, 5, 5);
                            Function.Call((Hash)0x6B7256074AE34680, sdz.Offset.X, sdz.Offset.Y, sdz.Offset.Z - 0.5f, sdz.Offset.X, sdz.Offset.Y, sdz.Offset.Z, color.R, color.G, color.B, 255);
                        }))));
                    }
                }
            }
            catch (Exception e)
            {
                UI.Notify(e.Message);
                UI.ShowSubtitle(e.Message);
            }
        }

        public static void NightMode()
        {
            if (nightModeTickStopwatch.ElapsedMilliseconds > 550)
            {
                nightModeTickStopwatch = new Stopwatch();
                nightModeTickStopwatch.Start();
                for (var i = 0; i < currentIntersection.TrafficLights.Count; ++i)
                {
                    SwitchLight(currentIntersection.TrafficLights[i].RoadType, "straight", nightModeLightState == 0 ? BlipColor.White : BlipColor.Yellow, currentIntersection.Name);
                    SwitchLight(currentIntersection.TrafficLights[i].RoadType, "left", nightModeLightState == 0 ? BlipColor.White : BlipColor.Yellow, currentIntersection.Name);
                }
                nightModeLightState = nightModeLightState > 0 ? 0 : 1;
            }
        }

        public static void RenderAllContent(IntersectionModel toLoad)
        {
            test.Start();
            init = true;

            var objects = ReadXml($"traff_light_intersection_{toLoad.Name}.xml");
            var lights = objects.GetRange(0, 4);
            var otherProps = objects.Skip(4).ToList();
            var count = 0;
            traffLights = new List<TrafficLightModel>();
            lights.ForEach(l =>
            {
                var lightPost = RenderSingleObject(l);
                var mod = new TrafficLightModel()
                {
                    LightPost = lightPost,
                    BulbsSet = RenderBulbsSet(lightPost, (count > 1 ? "double" : "triple")),
                    RoadType = (count > 1 ? "side" : "main")
                };
                traffLights.Add(mod);
                count++;
            });
            List<Prop> objs = new List<Prop>();
            otherProps.ForEach(p =>
            {
                var objec = RenderSingleObject(p);
                objs.Add(objec);
            });
            var patterns = new List<LightSwitchPatternModel>()
                        {
                            new LightSwitchPatternModel(){ DirType = "straight", Duration = 32000, RoadType = "main", Timer = new Stopwatch() },
                            new LightSwitchPatternModel(){ DirType = "left", Duration = 15000, RoadType = "main", Timer = new Stopwatch() },
                            new LightSwitchPatternModel(){ DirType = "straight", Duration = 15000, RoadType = "side", Timer = new Stopwatch() }
                        };
            var inn = new IntersectionModel()
            {
                LoadPoint = toLoad.LoadPoint,
                OtherProps = objs,
                Name = toLoad.Name,
                TrafficLights = traffLights,
                CurrentSwitchPattern = 0,
                SwitchPatterns = patterns,
                LOD = toLoad.LOD,
                IsJustLoaded = true,
            };
            intersections.Add(inn);

            currentIntersection = inn;

            SwitchLight("main", "straight", BlipColor.Red, inn.Name);
            SwitchLight("main", "left", BlipColor.Red, inn.Name);
            SwitchLight("side", "straight", BlipColor.Red, inn.Name);
            trafficLightTickStopwatch.Start();
            nightModeTickStopwatch.Start();
        }

        public static void TrafficLightRefresh()
        {
            //UI.ShowSubtitle(alreadyElapsedTime.ToString());
            if (trafficLightTickStopwatch.ElapsedMilliseconds > 1000)
            {
                if (currentIntersection.IsJustLoaded)
                {
                    currentIntersection.IsJustLoaded = false;
                    isInCurrentIntersection = true;
                    currentIntersection = intersections[0];
                    currentLightStatus = 0;

                    var patt = switchPatternRnd.Next(0, currentIntersection.SwitchPatterns.Count - 1);
                    if (patt > 0)
                        patt = switchPatternMoreRnd.Next(0, 10) > 5 ? patt : 0;
                    currentIntersection.CurrentSwitchPattern = patt;

                    var dur = currentIntersection.SwitchPatterns[currentIntersection.CurrentSwitchPattern].Duration;

                    alreadyElapsedTime = alreadyElapsedTimeRnd.Next((dur > 30000 ? 20000 : 7000), dur - 4000);
                    if (isNightMode)
                    {
                        RemoveAllSpeedZones(currentIntersection);
                    }

                }
                trafficLightTickStopwatch = new Stopwatch();
                trafficLightTickStopwatch.Start();
                var inter = currentIntersection;
                if (!isNightMode)
                {
                    var curr = inter.SwitchPatterns[inter.CurrentSwitchPattern];
                    if (!isInCurrentIntersection)
                    {
                        curr.Timer = new Stopwatch();
                    }
                    else
                    {
                        //  UI.ShowSubtitle(curr.RoadType + " " + curr.DirType);
                        if (!curr.Timer.IsRunning)
                        {
                            curr.Timer.Start();
                        }
                        if (curr.Timer.ElapsedMilliseconds > 2000 && currentLightStatus == 0)
                        {
                            SwitchLight(curr.RoadType, curr.DirType, BlipColor.Yellow, currentIntersection.Name);
                            currentLightStatus++;
                        }
                        if (curr.Timer.ElapsedMilliseconds > 3000 && currentLightStatus == 1)
                        {
                            SwitchLight(curr.RoadType, curr.DirType, BlipColor.Green, currentIntersection.Name);
                            currentLightStatus++;
                        }
                        if ((curr.Timer.ElapsedMilliseconds + alreadyElapsedTime) > (justEnterIntersection && false ? 3300 : (curr.Duration + 3000)) && currentLightStatus == 2)
                        {
                            SwitchLight(curr.RoadType, curr.DirType, BlipColor.Yellow, currentIntersection.Name);
                            currentLightStatus++;
                        }
                        if ((curr.Timer.ElapsedMilliseconds + alreadyElapsedTime) > (justEnterIntersection && false ? 6300 : (curr.Duration + 6000)) && currentLightStatus == 3)
                        {
                            currentLightStatus = 0;
                            justEnterIntersection = false;
                            SwitchLight(curr.RoadType, curr.DirType, BlipColor.Red, currentIntersection.Name);
                            inter.SwitchPatterns[inter.CurrentSwitchPattern].Timer = new Stopwatch();
                            var nextPattern = inter.CurrentSwitchPattern + 1;
                            inter.CurrentSwitchPattern = nextPattern > (inter.SwitchPatterns.Count - 1) ? 0 : nextPattern;
                            alreadyElapsedTime = 0;
                        }
                    }
                }
            }
        }

        public static void SwitchLight(string roadType, string dirType, BlipColor bulbColor, string interName)
        {
            currentIntersection.TrafficLights.Where(tl => tl.RoadType == roadType).ToList().ForEach(tl =>
            {
                tl.BulbsSet.Where(bs => bs.Type == dirType).ToList().ForEach(bs =>
                {
                    bs.Bulbs.ForEach(b =>
                    {
                        b.Alpha = b.MaxHealth == convertColor(bulbColor) ? 255 : 0;
                        if (bs.CurrentColor == BlipColor.Red && bulbColor == BlipColor.Yellow && b.MaxHealth == convertColor(BlipColor.Red))
                        {
                            b.Alpha = 255;
                        }
                        // special condition for flashing yellow night mode 
                        if (bulbColor == BlipColor.White)
                        {
                            b.Alpha = 0;
                        }
                    });
                    if (bulbColor == BlipColor.Red)
                    {
                        bs.SlowDownZones.ForEach(sdz =>
                        {
                            var speed = sdz.SpeedType == "stop" ? 0f : (28 / 3.6);
                            var id = Function.Call<int>((Hash)0x2CE544C68FB812A0, sdz.Offset.X, sdz.Offset.Y, sdz.Offset.Z, 0.8f, speed, false);
                            sdz.Id = id;
                            // UI.Notify("add " + sdz.Id.ToString());
                            if (sdz.Id == 0)
                            {
                                //  UI.Notify("fail " + roadType + " " + sdz.Offset.X.ToString() + " " + sdz.Offset.Y.ToString() + " " + sdz.Offset.Z.ToString() + " " + speed);
                            }
                        });
                    }
                    if (bulbColor == BlipColor.Yellow && bs.CurrentColor == BlipColor.Red)
                    {
                        bs.SlowDownZones.ForEach(sdz =>
                        {
                            Function.Call((Hash)0x1033371FC8E842A7, sdz.Id);
                            sdz.Id = 0;
                        });
                    }
                    bs.CurrentColor = bulbColor;
                });
            });
        }

        private static int convertColor(BlipColor c)
        {
            if (c == BlipColor.Red)
            {
                return 3;
            }
            if (c == BlipColor.Yellow)
            {
                return 2;
            }
            if (c == BlipColor.Green)
            {
                return 1;
            }
            return 1;
        }

        public static Prop RenderSingleObject(PropModel el)
        {
            Prop prop = new Prop(Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, el.Hash, el.Position.X, el.Position.Y, el.Position.Z, true, true, false));
            prop.FreezePosition = (el.Hash == -984871726 ? true : (el.Dynamic ? false : (el.Door ? false : true)));
            Function.Call(Hash.SET_ENTITY_QUATERNION, prop.Handle, el.Quaternion.X, el.Quaternion.Y, el.Quaternion.Z, el.Quaternion.W);
            prop.LodDistance = 2000;
            return prop;
        }

        public static List<BulbsModel> RenderBulbsSet(Prop light, string type)
        {
            List<BulbsModel> bulbsSet = new List<BulbsModel>();
            List<string> tripleSet = new List<string>() { "lower-straight", "upper-straight", "upper-straight-2", "upper-left" };
            List<string> doubleSet = new List<string>() { "lower-straight", "upper-straight" };
            List<string> typesSet;
            if (type == "triple")
                typesSet = tripleSet;
            else
                typesSet = doubleSet;
            for (var i = 0; i < typesSet.Count; ++i)
            {
                var bulbType = typesSet[i].Split('-')[1];
                var bulbs = RenderBulbs(light, typesSet[i]);
                var bulbSet = new BulbsModel() { Bulbs = bulbs, Type = bulbType, SlowDownZones = GetSlowDownZones(typesSet, typesSet[i], bulbs[0]), CurrentColor = BlipColor.Red };
                bulbsSet.Add(bulbSet);
            }
            return bulbsSet;
        }

        private static List<SlowDownZoneModel> GetSlowDownZones(List<string> types, string type, Prop offRef)
        {

            List<SlowDownZoneModel> zones = new List<SlowDownZoneModel>();
            List<SlowDownZoneModel> realZones = new List<SlowDownZoneModel>();
            List<SlowDownZoneModel> offsets = new List<SlowDownZoneModel>();
            var z = 5.9f;
            if (type == "upper-straight" || type == "upper-straight-2")
            {
                offsets = new List<SlowDownZoneModel>()
                {
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 6f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 23f), SpeedType = "medium" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 7f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 8f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 10f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 12f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 18f), SpeedType = "medium" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 26f), SpeedType = "medium" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 29f), SpeedType = "medium" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 32f), SpeedType = "medium" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, 0f, 36f), SpeedType = "medium" }
                };
            }
            if (type == "upper-left")
            {
                offsets = new List<SlowDownZoneModel>()
                {
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.5f, 7f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -5f, 22f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 8f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 9f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 10f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 11f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 12f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 13f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 14f), SpeedType = "stop" },
                    new SlowDownZoneModel() { Offset = new Vector3(z, -4.8f, 15f), SpeedType = "stop" },
                };
            }
            offsets.ForEach(sdz =>
            {
                sdz.Offset = offRef.GetOffsetInWorldCoords(sdz.Offset);
            });
            zones.AddRange(offsets);
            zones.ForEach(zon =>
            {
                realZones.Add(new SlowDownZoneModel() { Offset = new Vector3(zon.Offset.X, zon.Offset.Y, zon.Offset.Z + 1f), SpeedType = zon.SpeedType });
                realZones.Add(new SlowDownZoneModel() { Offset = new Vector3(zon.Offset.X, zon.Offset.Y, zon.Offset.Z - 1f), SpeedType = zon.SpeedType });
            });
            realZones.AddRange(zones);
            return realZones;
        }

        public static List<Prop> RenderBulbs(Prop light, string type)
        {
            List<Prop> bulbs = new List<Prop>();

            Vector3 lowerStraightOffset = light.GetOffsetInWorldCoords(new Vector3(0.19f, -0.47f, 4.8f));
            Vector3 upperStraightOffset = light.GetOffsetInWorldCoords(new Vector3(-4.78f, -0.47f, 6.5f));
            Vector3 upperStraightOffset2 = light.GetOffsetInWorldCoords(new Vector3(-9.5f, -0.47f, 6.75f));
            Vector3 upperLeftOffset = light.GetOffsetInWorldCoords(new Vector3(-10f, -0.47f, 6.75f));

            Vector3 bulbPos = lowerStraightOffset;
            if (type == "upper-straight")
                bulbPos = upperStraightOffset;
            if (type == "upper-straight-2")
                bulbPos = upperStraightOffset2;
            if (type == "upper-left")
                bulbPos = upperLeftOffset;
            for (var i = 0; i < 3; ++i)
            {
                int hash = redBulbHash;
                int bulbColor = 3;
                if (i == 1)
                {
                    bulbColor = 2;
                    hash = yellowBulbHash;
                }
                if (i == 2)
                {
                    bulbColor = 1;
                    hash = greenBulbHash;
                }
                Prop bulb = new Prop(
                 Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, hash, bulbPos.X, bulbPos.Y, bulbPos.Z, true, true, false)
                );
                bulb.MaxHealth = bulbColor;
                bulb.Rotation = new Vector3(0, 90f, light.Rotation.Z - 90f);
                bulb.LodDistance = 2000;
                bulbs.Add(bulb);
                bulbPos = bulbs[i].GetOffsetInWorldCoords(new Vector3(0.41f, 0, 0));
            }
            return bulbs;
        }

        public static void RemoveAllSpeedZones(IntersectionModel inter)
        {
            inter.TrafficLights.ForEach(tl => tl.BulbsSet.ForEach(bs => bs.SlowDownZones.ForEach(sdz =>
            {
                if (sdz.Id > 0)
                {
                    Function.Call((Hash)0x1033371FC8E842A7, sdz.Id);
                    sdz.Id = 0;
                }
            })));
        }

        public static void RemoveAllContent()
        {
            for (var ix = 0; ix < intersections[0].TrafficLights.Count; ++ix)
            {
                var traffModel = intersections[0].TrafficLights[ix];
                for (var ii = 0; ii < traffModel.BulbsSet.Count; ++ii)
                {
                    traffModel.BulbsSet[ii].SlowDownZones.ForEach(sdz =>
                    {
                        Function.Call((Hash)0x1033371FC8E842A7, sdz.Id);
                        sdz.Id = 0;
                    });
                    for (var iii = 0; iii < traffModel.BulbsSet[ii].Bulbs.Count; ++iii)
                    {
                        traffModel.BulbsSet[ii].Bulbs[iii].Delete();
                    }
                }
                traffModel.LightPost.Delete();
            }
            if (intersections[0].OtherProps.Count > 0)
            {
                for (var ir = 0; ir < intersections[0].OtherProps.Count; ++ir)
                {
                    intersections[0].OtherProps[ir].Delete();
                }
            }
            traffLights = new List<TrafficLightModel>();
            intersections = new List<IntersectionModel>();
            currentIntersection = new IntersectionModel() { Name = "defaultintrer", isFake = true };
            id = 0;
            isInCurrentIntersection = false;
            isNightMode = false;
            nightModeLightState = 0;
            justEnterIntersection = false;
            currentLightStatus = 0;
            alreadyElapsedTime = 0;
        }

        public class LightSwitchPatternModel
        {
            public string RoadType { get; set; }
            public string DirType { get; set; }
            public Stopwatch Timer { get; set; }
            public int Duration { get; set; }
        }

        public class IntersectionModel
        {
            public string Name { get; set; }
            public List<TrafficLightModel> TrafficLights { get; set; }
            public int CurrentSwitchPattern { get; set; }
            public List<LightSwitchPatternModel> SwitchPatterns { get; set; }
            public List<Prop> OtherProps { get; set; }
            public bool isFake { get; set; }
            public Vector3 LoadPoint { get; set; }
            public float LOD { get; set; }
            public bool IsJustLoaded { get; set; }
        }

        public class TrafficLightModel
        {
            public Prop LightPost { get; set; }
            public List<BulbsModel> BulbsSet { get; set; }
            public string RoadType { get; set; }
        }

        public class BulbsModel
        {
            public string Type { get; set; }
            public List<Prop> Bulbs { get; set; }
            public BlipColor CurrentColor { get; set; }
            public bool isSwitching { get; set; }
            public List<SlowDownZoneModel> SlowDownZones { get; set; }

        }

        public class SlowDownZoneModel
        {
            public int Id { get; set; }
            public Vector3 Offset { get; set; }
            public string SpeedType { get; set; }

        }
    }
}
