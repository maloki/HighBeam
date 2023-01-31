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
using XInputDotNetPure;
using static HighBeam.Main;

namespace HighBeam
{
    public static class Plane
    {
        public static List<string> nyList = new List<string>() { "plg_01", "prologue01",
"prologue01_lod",
"prologue01c",
"prologue01c_lod",
"prologue01d",
"prologue01d_lod",
"prologue01e",
"prologue01e_lod",
"prologue01f",
"prologue01f_lod",
"prologue01g",
"prologue01h",
"prologue01h_lod",
"prologue01i",
"prologue01i_lod",
"prologue01j",
"prologue01j_lod",
"prologue01k",
"prologue01k_lod",
"prologue01z",
"prologue01z_lod",
"plg_02",
"prologue02",
"rologue02_lod",
"plg_03",
"prologue03",
"prologue03_lod",
"prologue03b",
"prologue03b_lod",
"prologue03_grv_dug",
"prologue03_grv_dug_lod",
"prologue_grv_torch",
"plg_04",
"prologue04",
"prologue04_lod",
"prologue04b",
"prologue04b_lod",
"prologue04_cover",
"des_protree_end",
"des_protree_start",
"des_protree_start_lod",
"plg_05",
"prologue05",
"prologue05_lod",
"prologue05b",
"prologue05b_lod",
"plg_06",
"prologue06",
"prologue06_lod",
"prologue06b",
"prologue06b_lod",
"prologue06_int",
"prologue06_int_lod",
"prologue06_pannel",
"prologue06_pannel_lod",
"prologue_m2_door",
"prologue_m2_door_lod",
"plg_occl_00",
"prologue_occl",
"plg_rd",
"prologuerd",
"prologuerdb",
"prologuerd_lod",};
        public static Airport CurrentAirport = null;
        public static bool isEngineRunning = false;
        public static bool isNyAirportLoaded = false;
        public static Stopwatch EngineToggleStopwatch = new Stopwatch();
        public static bool isAutopilotOn = false;
        public static float autopilotSpeed = 0;
        public static bool canPressCruiseButton = true;
        public static Stopwatch cruiseControlButtonsStopwatch = new Stopwatch();
        public static Vector3 wp = new Vector3();
        public static Vector3 lastWp = new Vector3();
        public static int autopilotHeight = 101;
        public static Stopwatch autopilotEngageStopwatch = new Stopwatch();
        public static void RunPlane()
        {
            NyAirport();
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleExit))
            {
                isEngineRunning = false;
            }
            if (veh.ClassType == VehicleClass.Planes)
            {
                Game.DisableControlThisFrame(0, GTA.Control.Phone);
                Game.DisableControlThisFrame(0, GTA.Control.ScriptPadDown);
                Game.DisableControlThisFrame(0, GTA.Control.VehicleRadioWheel);
                Game.DisableControlThisFrame(0, GTA.Control.VehicleNextRadio);
                Game.DisableControlThisFrame(0, GTA.Control.VehiclePrevRadio);
                if (veh.HighBeamsOn)
                {
                    Vector3 lt3 = (veh.GetOffsetInWorldCoords(new Vector3(0, 40f, -2f)) - veh.Position);
                    lt3.Normalize();
                    var _pos = veh.GetOffsetInWorldCoords(new Vector3(3f, 0, -0.9f));
                    var _pos2 = veh.GetOffsetInWorldCoords(new Vector3(-3f, 0, -0.9f));
                    World.DrawSpotLight(_pos, lt3, Color.White, 100, 3f, 50f, 15f, 0f);
                    World.DrawSpotLight(_pos2, lt3, Color.White, 100, 3f, 50f, 15f, 0f);
                }
                var q = veh.Quaternion;
                var pitch = Math.Atan2(2 * q.X * q.W + 2 * q.Y * q.Z, 1 - 2 * q.X * q.X - 2 * q.Z * q.Z);
                lastWp = World.GetWaypointPosition();
                var groundZ = World.GetGroundHeight(wp);
                var pos = new Vector3(wp.X, wp.Y, groundZ);
                var cont = new UIContainer(new Point(300, UI.HEIGHT - 40), new Size(540, 36), Color.FromArgb(160, 0, 0, 0));

                var height = World.GetDistance(veh.Position, new Vector3(veh.Position.X, veh.Position.Y, 0)) - (World.GetGroundHeight(veh.Position) >= 0 ? World.GetGroundHeight(veh.Position) : 0);
                cont.Items.Add(new UIText(
                    "Ground height: " + Math.Round(veh.Position.DistanceTo(new Vector3(veh.Position.X, veh.Position.Y, 0)), 0) + "m"
                    , new Point(110, 10), 0.4f, Color.White, GTA.Font.ChaletLondon, true));
                cont.Items.Add(new UIText(
                    "Height: " + ((Math.Round(height - 1.8, 0)) < 4 ? (Math.Round(height - 1.8, 1)) : (Math.Round(height - 1.8, 0))) + "m"
                    , new Point(280, 10), 0.4f, Color.White, GTA.Font.ChaletLondon, true));
                cont.Items.Add(new UIText(
                    "Heading: " + Math.Round(veh.Heading, 0)
                    , new Point(420, 10), 0.4f, Color.White, GTA.Font.ChaletLondon, true));
                if (Function.Call<bool>((Hash)0x1DD1F58F493F1DA5))
                {
                    if (Object.Equals(CurrentAirport, null) || lastWp != wp)
                    {
                        wp = lastWp;
                        Airport closest = new Airport() { Location = new Vector3(0, 0, 0) };
                        for (var ia = 0; ia < Airports.Count; ++ia)
                        {
                            Airport a = Airports[ia];
                            var dist = a.Location.DistanceTo(wp);
                            var distToClosest = closest.Location.DistanceTo(wp);
                            if (dist < distToClosest)
                                closest = a;
                        }
                        var refPoint = World.CreateVehicle(new Model(VehicleHash.PCJ), closest.Location);
                        refPoint.Heading = closest.Heading;
                        List<Vector3> ilsPoints = new List<Vector3>();
                        int ilsOffset = 0;
                        int ilsHeightOffset = 24;
                        for (var il = 0; il < 10; ++il)
                        {
                            ilsPoints.Add(refPoint.GetOffsetInWorldCoords(new Vector3(0, -(ilsOffset), -(ilsHeightOffset))));
                            ilsOffset += 50;
                            ilsHeightOffset -= 3;
                            if (ilsHeightOffset <= 0)
                                ilsHeightOffset = 0;
                        }
                        closest.IlsPoints = ilsPoints;
                        refPoint.Delete();
                        refPoint.MarkAsNoLongerNeeded();
                        CurrentAirport = closest;
                    }
                    var etaDist = Math.Round(veh.Position.DistanceTo(CurrentAirport.Location));
                    var destContainer = new UIContainer(new Point(300, UI.HEIGHT - 74), new Size(540, 36), Color.FromArgb(160, 0, 0, 0));
                    destContainer.Items.Add(new UIText(CurrentAirport.Name, new Point(80, 10), 0.4f, Color.White, GTA.Font.ChaletLondon, true));
                    destContainer.Items.Add(new UIText(
                    "Dist: " + (etaDist > 999 ? Math.Round(etaDist / 1000, 1) + "km" : etaDist + "m ")
                    , new Point(250, 10), 0.4f, Color.White, GTA.Font.ChaletLondon, true));
                    var bearing = Bearing();
                    destContainer.Items.Add(new UIText("Bearing: " + Math.Round((veh.Position - CurrentAirport.Location).ToHeading(), 0).ToString(), new Point(380, 10), 0.4f, Color.White, GTA.Font.ChaletLondon, true));
                    for (var il = 0; il < CurrentAirport.IlsPoints.Count; ++il)
                    {
                        var ils = CurrentAirport.IlsPoints[il];
                        Function.Call((Hash)0x6B7256074AE34680, ils.X, ils.Y, ils.Z, ils.X, ils.Y, (ils.Z + 29f), 255, 5, 5, 255);
                    }
                    if (etaDist <= 10000)
                    {
                        var ilsHeight = (Math.Round(veh.Position.DistanceTo(new Vector3(veh.Position.X, veh.Position.Y, 0)), 0) - CurrentAirport.Location.Z) * 10;
                        var ih = ilsHeight - etaDist;
                        var text = "";
                        if ((ih >= 0 && ih < 80) || (ih <= 0 && ih > -80))
                            text = "-";
                        else
                        {
                            text = (int)ilsHeight > etaDist ? "DOWN" : "UP";
                        }
                        destContainer.Items.Add(new UIText("ILS: " + text, new Point(480, 10), 0.4f, Color.White, GTA.Font.ChaletLondon, true));

                    }
                    //  destContainer.Enabled = true;
                    //  destContainer.Draw();
                }
                else
                {
                    CurrentAirport = null;
                }
                // cont.Enabled = true;
                //  cont.Draw();
                // RenderAttitudeIndicator();
                Engine();
                NyAirport();
                Autopilot();
                FlightInstruments();
                Function.Call((Hash)0xCB7CC0D58405AD41, 200f);
                Function.Call((Hash)0xAD2D28A1AFDFF131, veh, 0.0f);

                // var s = new XInputDotNetPure.GamePadThumbSticks();
                //  GamePadState state = GamePad.GetState(PlayerIndex.One);
                //   UI.ShowSubtitle(s.Left.X.ToString());
                // XInput.Wrapper.X.Gamepad_1.FFB_LeftMotor(1,1);
            }

        }

        public static void FlightInstruments()
        {
            RenderLeftDisplay();
            RenderRightDisplay();
        }

        public static void RenderRightDisplay()
        {
            var mainDisplay = new UIContainer(new Point((UI.WIDTH / 2) - 280, UI.HEIGHT - 130), new Size(160, 130), Color.FromArgb(160, 0, 0, 0));
            // clock 
            var h = fakeTimeHours;
            var m = fakeTimeMinutes;
            var colorGreen = Color.FromArgb(255, 0, 255, 65);
            mainDisplay.Items.Add(new UIText($"{(h < 10 ? ("0" + h.ToString()) : h.ToString())}:{(m < 10 ? ("0" + m.ToString()) : m.ToString())}", new Point(80, 4), 0.3f, colorGreen, GTA.Font.ChaletLondon, true));
            if (CurrentAirport != null)
            {
                var etaDist = Math.Round(veh.Position.DistanceTo(CurrentAirport.Location));
                mainDisplay.Items.Add(new UIText("Dest Airport: " + CurrentAirport.Name, new Point(80, 20), 0.26f, colorGreen, GTA.Font.ChaletLondon, true));
                mainDisplay.Items.Add(new UIText("Dist: " + (etaDist > 999 ? Math.Round(etaDist / 1000, 1) + "km" : etaDist + "m "), new Point(80, 35), 0.26f, colorGreen, GTA.Font.ChaletLondon, true));
            }
            mainDisplay.Draw();

        }

        public static void RenderLeftDisplay()
        {
            var height = World.GetDistance(veh.Position, new Vector3(veh.Position.X, veh.Position.Y, 0)) - (World.GetGroundHeight(veh.Position) >= 0 ? World.GetGroundHeight(veh.Position) : 0);
            var mainDisplay = new UIContainer(new Point((UI.WIDTH / 2) - 100, UI.HEIGHT - 130), new Size(200, 130), Color.FromArgb(160, 0, 0, 0));
            //  var mainDisplay = new UIContainer(new Point((UI.WIDTH / 2) + 300, UI.HEIGHT - 130), new Size(200, 130), Color.FromArgb(160, 0, 0, 0));
            // SPEED
            var speedDisplay = new UIContainer(new Point(10, 5), new Size(30, 120), Color.FromArgb(255, 87, 80, 104));
            if (isAutopilotOn)
                speedDisplay.Items.Add(new UIText(autopilotSpeed.ToString(), new Point(14, 5), 0.36f, Color.FromArgb(255, 148, 73, 191), GTA.Font.ChaletLondon, true));
            var currentSpeed = new UIContainer(new Point(0, 50), new Size(30, 20), Color.FromArgb(255, 0, 0, 0));
            if (height < 2f && fakeSpeed >= 150)
                currentSpeed.Items.Add(new UIText("ROT", new Point(14, -20), 0.36f, Color.Yellow, GTA.Font.ChaletLondon, true));
            currentSpeed.Items.Add(new UIText(fakeSpeed.ToString(), new Point(14, 0), 0.36f, Color.White, GTA.Font.ChaletLondon, true));
            currentSpeed.Items.Add(new UIText("KM/H", new Point(14, 20), 0.2f, Color.White, GTA.Font.ChaletLondon, true));
            speedDisplay.Items.Add(currentSpeed);
            mainDisplay.Items.Add(speedDisplay);
            //HEADING
            var headingDisplay = new UIContainer(new Point(50, 5), new Size(100, 15), Color.FromArgb(255, 87, 80, 104));
            headingDisplay.Items.Add(new UIText(Math.Round(veh.Heading, 0).ToString(), new Point(24, -2), 0.35f, Color.White, GTA.Font.ChaletLondon, true));
            if (CurrentAirport != null)
                headingDisplay.Items.Add(new UIText(Math.Round(CurrentAirport.Heading, 0).ToString(), new Point(60, -2), 0.35f, Color.FromArgb(255, 148, 73, 191), GTA.Font.ChaletLondon, true));
            // HORIZONT
            mainDisplay.Items.Add(RenderAttitudeIndicator());
            // HEIGHT

            var heightDisplay = new UIContainer(new Point(160, 5), new Size(30, 120), Color.FromArgb(255, 87, 80, 104));
            if (CurrentAirport != null)
            {
                var etaDist = Math.Round(veh.Position.DistanceTo(CurrentAirport.Location));
                if (etaDist < 10000)
                {
                    var ilsHeight = (Math.Round(veh.Position.DistanceTo(new Vector3(veh.Position.X, veh.Position.Y, 0)), 0) - CurrentAirport.Location.Z) * 10;
                    var ih = ilsHeight - etaDist;
                    var ilsState = "center";
                    if ((ih >= 0 && ih < 80) || (ih <= 0 && ih > -80))
                    {

                    }
                    else
                    {
                        ilsState = (int)ilsHeight > etaDist ? "down" : "up";
                    }
                    heightDisplay.Items.Add(new UIText("ILS", new Point(14, 95), 0.30f, Color.White, GTA.Font.ChaletLondon, true));
                    if (ilsState == "up")
                        heightDisplay.Items.Add(new UIText(@"^", new Point(14, 82), 0.50f, Color.OrangeRed, GTA.Font.ChaletLondon, true));
                    if (ilsState == "down")
                        heightDisplay.Items.Add(new UIText(@"v", new Point(14, 100), 0.50f, Color.OrangeRed, GTA.Font.ChaletLondon, true));

                }
            }
            if (isAutopilotOn)
            {
                var ilsHeightDis = new UIContainer(new Point(0, 30), new Size(30, 20), Color.FromArgb(255, 0, 0, 0));
                ilsHeightDis.Items.Add(new UIText((autopilotHeight.ToString()), new Point(14, 5), 0.30f, Color.FromArgb(255, 148, 73, 191), GTA.Font.ChaletLondon, true));
                heightDisplay.Items.Add(ilsHeightDis);
            }
            var currentHeight = new UIContainer(new Point(0, 50), new Size(30, 20), Color.FromArgb(255, 0, 0, 0));
            currentHeight.Items.Add(
                new UIText((Math.Round(veh.Position.DistanceTo(new Vector3(veh.Position.X, veh.Position.Y, 0)), 0)).ToString(),
                new Point(14, 0), 0.30f, Color.White, GTA.Font.ChaletLondon, true));
            currentHeight.Items.Add(new UIText("M", new Point(14, 20), 0.2f, Color.White, GTA.Font.ChaletLondon, true));
            heightDisplay.Items.Add(currentHeight);
            mainDisplay.Items.Add(heightDisplay);
            mainDisplay.Items.Add(headingDisplay);
            mainDisplay.Draw();
        }

        private static UIContainer RenderAttitudeIndicator()
        {

            var q = veh.Quaternion;
            //  var eX = atan2(-2 * (qy * qz - qw * qx), qw * qw - qx * qx - qy * qy + qz * qz);
            var pitch = Math.Round(veh.ForwardVector.Z * 100, 0);
            //  var eZ = atan2(-2 * (qx * qy - qw * qz), qw * qw + qx * qx - qy * qy - qz * qz);

            var pitchPercentage = Math.Round(((pitch - (-30)) * 10) / (30 - (-30)) * 10, 0);
            if (pitchPercentage < 0)
                pitchPercentage = 0;
            if (pitchPercentage > 100)
                pitchPercentage = 100;
            //   UI.ShowSubtitle("     sssssssssssssssssssssssssssssssss             " + (pitchPercentage).ToString());
            var attitudeContainer = new UIContainer(new Point(50, 30), new Size(100, 100), Color.FromArgb(160, 0, 0, 0));
            attitudeContainer.Items.Add(new UIContainer(new Point(0, 0), new Size(100, 100), Color.FromArgb(255, 137, 71, 0)));
            attitudeContainer.Items.Add(new UIContainer(new Point(0, 0), new Size(100, (int)pitchPercentage), Color.FromArgb(255, 41, 119, 239)));
            attitudeContainer.Items.Add(new UIContainer(new Point(0, 50), new Size(100, 2), Color.FromArgb(255, 229, 231, 0)));
            attitudeContainer.Items.Add(new UIText(pitch.ToString(), new Point(50, 30), 0.36f, Color.White, GTA.Font.ChaletLondon, true));
            // var horizonTop = new UIContainer(new Point(900, UI.HEIGHT - 200), new Size(100, 50), Color.FromArgb(255, 41, 119, 239));
            //  var horizonBottom = new UIContainer(new Point(900, UI.HEIGHT - 150), new Size(100, 50), Color.FromArgb(255, 137, 71, 0));
            return attitudeContainer;
            /*horizonTop.Enabled = true;
            horizonTop.Draw();
            horizonBottom.Enabled = true;
            horizonBottom.Draw();*/
        }

        public static void Autopilot()
        {
            var curHei = (int)Math.Round(veh.Position.DistanceTo(new Vector3(veh.Position.X, veh.Position.Y, 0)), 0);
            if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadLeft) && !autopilotEngageStopwatch.IsRunning)
            {
                autopilotEngageStopwatch = new Stopwatch();
                autopilotEngageStopwatch.Start();
                UI.ShowSubtitle("start");
            }
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadLeft))
            {
                autopilotEngageStopwatch = new Stopwatch();
                UI.ShowSubtitle("releas");
            }
            if (Game.IsControlPressed(0, GTA.Control.ScriptPadLeft) && fakeSpeed > 100 && fakeSpeed < 360 && autopilotEngageStopwatch.ElapsedMilliseconds > 1000)
            {
                isAutopilotOn = true;
                autopilotSpeed = fakeSpeed < 50 ? 50 : fakeSpeed;
                autopilotHeight = curHei;
                UI.ShowSubtitle("engaged");
            }
            if (isAutopilotOn)
            {
                double currentSpeed = fakeSpeed;
                double cruiseSpeed = autopilotSpeed + 0.4;


                if (fakeSpeed >= autopilotSpeed && autopilotSpeed - fakeSpeed < 2f && autopilotSpeed - fakeSpeed > -2f)
                {
                    fakeSpeed = (int)autopilotSpeed;
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 0.05)
                {
                    isAutopilotOn = false;
                }
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0.3 && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > -0.3)
                {
                    var pitch = Math.Round(veh.ForwardVector.Z * 100, 0);
                    var force = 0.02f;
                    var yforce = 0f;
                    var zforce = 0f;
                    var maxPitch = 6;
                    var diff = curHei - autopilotHeight;
                    if (diff > 200 || diff < -200)
                    {
                        force = 0.025f;
                        maxPitch = 14;
                    }
                    if (diff < 50 && diff > -50)
                    {
                        force = 0.017f;
                        maxPitch = 2;
                    }
                    if (diff < 8 && diff > -8 && (pitch < 0 || pitch > 0))
                    {
                        force = 0.2f;
                        maxPitch = 1;
                    }
                    maxPitch = -(int)(diff / 10) + (diff > 14 ? 4 : 0);
                    if (diff < 14 && diff > -14)
                        maxPitch = 0;
                    if (maxPitch > 20)
                    {
                        maxPitch = 20;
                    }
                    if (maxPitch < -20)
                        maxPitch = -20;

                    force = 0.2f;
                    if (diff > 1 || diff < -1)
                    {
                        if ((curHei < autopilotHeight && pitch < maxPitch) || pitch < -maxPitch)
                        {
                            zforce = -force;
                        }
                        if ((curHei > autopilotHeight && pitch > -maxPitch) || pitch > maxPitch)
                        {
                            zforce = force;
                        }
                        if (currentSpeed < cruiseSpeed)
                        {
                            yforce = 0.1f;
                            yforce = 1f;
                        }
                        if (currentSpeed > cruiseSpeed)
                        {
                            yforce = -0.04f;
                        }
                        veh.ApplyForceRelative(new Vector3(0.0f, yforce, zforce));
                    }
                }
                var maxSpeed = 1000;
                var maxHeight = 100000;
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == XInputDotNetPure.ButtonState.Pressed && canPressCruiseButton && autopilotSpeed < maxSpeed)
                {
                    canPressCruiseButton = false;
                    cruiseControlButtonsStopwatch.Start();
                    autopilotSpeed = (fakeSpeed % 5 == 0 ? autopilotSpeed : (RoundUp((int)(autopilotSpeed)))) + 5;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == XInputDotNetPure.ButtonState.Pressed && canPressCruiseButton && autopilotSpeed > 100)
                {
                    canPressCruiseButton = false;
                    cruiseControlButtonsStopwatch.Start();
                    autopilotSpeed = (fakeSpeed % 5 == 0 ? autopilotSpeed : (RoundDown((int)(autopilotSpeed)))) - 5;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Right == XInputDotNetPure.ButtonState.Pressed && canPressCruiseButton && autopilotHeight < maxHeight)
                {
                    canPressCruiseButton = false;
                    cruiseControlButtonsStopwatch.Start();
                    autopilotHeight = (curHei % 50 == 0 ? autopilotHeight : (RoundUp((int)(autopilotHeight), 50))) + 50;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Left == XInputDotNetPure.ButtonState.Pressed && canPressCruiseButton && autopilotHeight > 100)
                {
                    canPressCruiseButton = false;
                    cruiseControlButtonsStopwatch.Start();
                    autopilotHeight = (curHei % 50 == 0 ? autopilotHeight : (RoundDown((int)(autopilotHeight), 50))) - 50;
                }
                if (cruiseControlButtonsStopwatch.ElapsedMilliseconds > 100)
                {
                    cruiseControlButtonsStopwatch = new Stopwatch();
                    canPressCruiseButton = true;
                }
                if (CurrentAirport != null)
                {
                    if (CurrentAirport.Location.DistanceTo(veh.Position) < 10000)
                    {
                        autopilotHeight = (int)Math.Round((double)(CurrentAirport.Location.DistanceTo(veh.Position) / 10), 0) + 30;
                    }
                }
            }
        }

        private static int RoundUp(int toRound, int roundBy = 5)
        {
            if (toRound % roundBy == 0) return toRound;
            return (roundBy - toRound % roundBy) + toRound;
        }
        private static int RoundDown(int toRound, int roundBy = 5)
        {
            return toRound - toRound % roundBy;
        }

        public static void NyAirport()
        {
           // Function.Call((Hash)0x6B7256074AE34680, 9993.4f, -5332.011f, 168.7, 9993.4f, -5332.011f, 290f, 255, 5, 5, 255);
         //   Function.Call((Hash)0x6B7256074AE34680, 9994.0f, -5319.7f, 168.7, 9994.0f, -5319.7f, 290f, 255, 5, 5, 255);
            if (!isNyAirportLoaded)
            {
                for (var i = 0; i < nyList.Count; ++i)
                {
                   // Function.Call((Hash)0x41B4893843BBDB74, nyList[i]);
                }
                isNyAirportLoaded = true;
            }
            if (CurrentAirport != null)
            {
                if (CurrentAirport.Name == "NY 26 W" && !isNyAirportLoaded)
                {

                }
            }
            else if (isNyAirportLoaded && false)
            {
                for (var i = 0; i < nyList.Count; ++i)
                {
                    Function.Call((Hash)0xEE6C5AD3ECE0A82D, nyList[i]);
                }
                isNyAirportLoaded = false;
            }

        }

        public static void Engine()
        {
            veh.EngineRunning = isEngineRunning;
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleHandbrake))
            {
                EngineToggleStopwatch = new Stopwatch();
                EngineToggleStopwatch.Start();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleHandbrake) && EngineToggleStopwatch.ElapsedMilliseconds < 3000)
            {
                EngineToggleStopwatch = new Stopwatch();
            }
            if (EngineToggleStopwatch.ElapsedMilliseconds > 5000)
            {
                isEngineRunning = isEngineRunning ? false : true;
                EngineToggleStopwatch = new Stopwatch();
            }
            var cont = new UIContainer(new Point(220, UI.HEIGHT - 74), new Size(60, 60), Color.FromArgb(160, 0, 0, 0));
            var colorGreen = Color.FromArgb(255, 0, 206, 0);
            var colorGray = Color.DarkGray;
            var colorOrange = Color.FromArgb(255, 255, 204, 0);
            var colorRed = Color.FromArgb(255, 204, 0, 0);
            Color flapsStatusColor = colorOrange;
            if (veh.LandingGear.ToString().ToLower() == "4")
                flapsStatusColor = colorGray;
            else if (veh.LandingGear.ToString().ToLower() == "deployed")
                flapsStatusColor = colorGreen;
            Color engineStatusColor = colorGray;
            if (isEngineRunning)
                engineStatusColor = colorGreen;
            else
                engineStatusColor = colorGray;
            if (EngineToggleStopwatch.IsRunning)
                engineStatusColor = colorOrange;
            Color autopilotStatusColor = colorOrange;
            if (isAutopilotOn)
                autopilotStatusColor = colorGreen;
            else
                autopilotStatusColor = colorGray;
            cont.Items.Add(new UIContainer(new Point(0, 0), new Size(60, 20), engineStatusColor));
            cont.Items.Add(new UIText("ENGINE", new Point(30, 0), 0.3f, Color.White, GTA.Font.ChaletLondon, true));
            cont.Items.Add(new UIContainer(new Point(0, 20), new Size(60, 20), flapsStatusColor));
            cont.Items.Add(new UIText("FLAPS", new Point(31, 20), 0.3f, Color.White, GTA.Font.ChaletLondon, true));
            cont.Items.Add(new UIContainer(new Point(0, 40), new Size(60, 20), autopilotStatusColor));
            cont.Items.Add(new UIText("AUTOPILOT", new Point(31, 44), 0.25f, Color.White, GTA.Font.ChaletLondon, true));
            cont.Enabled = true;
            cont.Draw();
        }

        public static double DegreeToRadian(double angle) { return Math.PI * angle / 180.0; }

        public static double RadianToDegree(double angle) { return 180.0 * angle / Math.PI; }

        public static double Bearing()
        {
            var lat1 = DegreeToRadian(veh.Position.X);
            var lon1 = DegreeToRadian(veh.Position.Y);
            var lat2 = DegreeToRadian(CurrentAirport.Location.X);
            var lon2 = DegreeToRadian(CurrentAirport.Location.Y);


            var dLon = lon2 - lon1;

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            var radiansBearing = Math.Atan2(y, x);

            return RadianToDegree(radiansBearing);
        }





        public class Airport
        {
            public Vector3 Location { get; set; }
            public float Heading { get; set; }
            public string Name { get; set; }
            public List<Vector3> IlsPoints { get; set; }
        }

        public static List<Airport> Airports = new List<Airport>()
        {
            new Airport()
            {
                Location = new Vector3(-1603.616f, -2794.649f, 13.74f),
                Heading = 239.75f,
                Name = "LAX 23 W",
            },
            new Airport()
            {
                Location = new Vector3(-1678.717f, -2805.49f, 13.71f),
                Heading = 330.1f,
                Name = "LAX 33 S",
            },
            new Airport()
            {
                Location = new Vector3(-883.157f, -3210.683f, 13.71f),
                Heading = 60.39f,
                Name = "LAX 6 E",
            },
            new Airport()
            {
                Location = new Vector3(-1344.2f, -2223.9f, 13.71f),
                Heading = 149.8f,
                Name = "LAX 6 N",
            },
            new Airport()
            {
                Location = new Vector3(-1969.634f, 2836.288f, 32.58f),
                Heading = 60.19f,
                Name = "Airbase 6 E",
            },
            new Airport()
            {
                Location = new Vector3(-2738.573f, 3280.57f, 32.58f),
                Heading = 240.46f,
                Name = "Airbase 24 W",
            },
            new Airport()
            {
                Location = new Vector3(5468.44f, -6418.549f, 108.6f),
                Heading = 266.5f,
                Name = "NY 26 W",
            }
        };
    }
}
