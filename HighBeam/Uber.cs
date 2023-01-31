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
    public static class Uber
    {
        public static bool init = false;
        public static Vector2 LosSantosTL = new Vector2(-3034.59f, 173.9f);
        public static Vector2 LosSantosTR = new Vector2(2583.7f, 406.8f);
        public static Vector2 LosSantosBR = new Vector2(1550.6f, -2574.0f);
        public static Vector2 LosSantosBL = new Vector2(-1528.6f, -2675.4f);
        public static List<Vector3> PossibleCustomersPositions = new List<Vector3>();
        public static Vector3 CustomerStartPosition = new Vector3(0, 0, 0);
        public static Vector2 CustomerDestination = new Vector2(0, 0);
        public static Ped CustomerPed = null;
        public static bool isCustomerInCar = false;
        private static Random rnd = new Random();
        public static Stopwatch deleteCustomerStopwatch = new Stopwatch();
        public static Stopwatch customerLeaveCarStopwatch = new Stopwatch();
        public static UIContainer AppDisplay;
        public static bool isUberMode;
        public static Stopwatch uberModeStopwatch = new Stopwatch();
        public static bool showApp = false;
        public static Stopwatch searchingForCustomersStopwatch = new Stopwatch();
        public static bool canPressButton = true;
        public static int appCurrentHover = 0;
        public static Stopwatch radioMenuScrollDelay = new Stopwatch();
        public static ButtonState lastRadioMenuButtonState = ButtonState.Released;
        public static bool isTripActive = false;
        public static Stopwatch tripCompleteStopwatch = new Stopwatch();
        public static bool hasCustomerStartedEnteringVehicle = false;
        public static List<PedHash> peds = new List<PedHash>()
        {
            PedHash.Bevhills01AMY,
            PedHash.Bevhills01AFY,
            PedHash.Bevhills02AFY,
            PedHash.Bevhills04AFY,
            PedHash.Business01AFY,
            PedHash.Business02AFM,
            PedHash.Business03AFY,
            PedHash.Downtown01AFM,
            PedHash.Downtown01AMY,
            PedHash.Eastsa01AFM,
            PedHash.Eastsa01AMM,
            PedHash.Eastsa02AFY,
            PedHash.Eastsa02AMY,
            PedHash.Eastsa03AFY,
            PedHash.Lifeinvad01,
            PedHash.Runner01AMY,
            PedHash.Soucent01AMY,
            PedHash.Sweatshop01SFM,
            PedHash.Tennis01AFY
        };
        public static void RunUber()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == ButtonState.Pressed && !uberModeStopwatch.IsRunning)
            {
                uberModeStopwatch.Start();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == ButtonState.Pressed && uberModeStopwatch.ElapsedMilliseconds > 500 && canPressButton)
            {
                canPressButton = false;
                isUberMode = isUberMode ? false : true;
                showApp = isUberMode ? true : false;
                if (PossibleCustomersPositions.Count == 0 && !isTripActive)
                    SearchForCustomer();
            }
            if (uberModeStopwatch.ElapsedMilliseconds > 1000)
            {
                uberModeStopwatch = new Stopwatch();
                canPressButton = true;
            }
            if (isUberMode)
            {
                var main = new UIContainer(new Point(45, UI.HEIGHT - 220), new Size(50, 24), System.Drawing.Color.FromArgb(80, 0, 0, 0));
                main.Items.Add(new UIText("UBER", new Point(8, 3), 0.30f, Color.White));
                main.Draw();
                Game.DisableControlThisFrame(0, Control.Phone);
                Game.DisableControlThisFrame(0, Control.VehicleCinCam);
                //   SelectCustomer();
                // GenerateDestination();
                AppControls();
                AppUi();
                GoToCustomer();
                CustomerHandleDoor();
                ArriveAtDestination();
                if (deleteCustomerStopwatch.ElapsedMilliseconds > 10000)
                {
                    CustomerPed.Delete();
                    CustomerPed = null;
                    deleteCustomerStopwatch = new Stopwatch();
                }
                if (customerLeaveCarStopwatch.ElapsedMilliseconds > 1700)
                {
                    veh.CloseDoor(VehicleDoor.BackRightDoor, false);
                    veh.CloseDoor(VehicleDoor.BackLeftDoor, false);
                    customerLeaveCarStopwatch = new Stopwatch();
                }
                if (tripCompleteStopwatch.ElapsedMilliseconds > 7000)
                {
                    tripCompleteStopwatch = new Stopwatch();
                    SearchForCustomer();
                }
            }

            //  UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z);
        }
        public static void ArriveAtDestination()
        {
            if (isCustomerInCar && veh.Speed <= 5 && Game.IsControlJustPressed(0, Control.ScriptPadDown))
            {
                customerLeaveCarStopwatch.Start();
                var seq1 = new TaskSequence();
                seq1.AddTask.LeaveVehicle(veh, true);
                seq1.AddTask.Wait(700);
                seq1.AddTask.WanderAround(CustomerPed.Position, 100f);
                seq1.Close();
                CustomerPed.AlwaysKeepTask = true;
                CustomerPed.Task.PerformSequence(seq1);
                seq1.Dispose();
                deleteCustomerStopwatch.Start();
                isTripActive = false;
                tripCompleteStopwatch.Start();
                isCustomerInCar = false;
                showApp = true;
                appCurrentHover = 0;
            }
        }

        public static void GenerateDestination()
        {
            CustomerDestination = new Vector2((float)rnd.Next(-3034, 2583), (float)rnd.Next(-2675, 406));
        }

        public static void CustomerHandleDoor()
        {
            if (veh.Speed <= 0)
            {
                if (!isCustomerInCar && veh.Position.DistanceTo(CustomerStartPosition) < 40f)
                {
                    CustomerPed.Task.EnterVehicle(veh, rnd.Next(1, 6) % 2 == 0 ? VehicleSeat.RightRear : VehicleSeat.LeftRear);
                    Function.Call(Hash.SET_NEW_WAYPOINT, CustomerDestination.X, CustomerDestination.Y);
                    isCustomerInCar = true;
                }
            }
        }

        public static void SelectCustomer()
        {
            CustomerStartPosition = PossibleCustomersPositions[appCurrentHover];
            appCurrentHover = 0;
            Function.Call(Hash.SET_NEW_WAYPOINT, CustomerStartPosition.X, CustomerStartPosition.Y);
            showApp = false;
            isTripActive = true;
            PossibleCustomersPositions = new List<Vector3>();
            GenerateDestination();
        }

        public static void GoToCustomer()
        {
            if (Game.IsControlJustPressed(0, Control.VehicleHorn) && CustomerPed != null)
            {
                if (!CustomerPed.IsInVehicle(veh))
                {
                    CustomerPed.Task.EnterVehicle(veh, rnd.Next(1, 6) % 2 == 0 ? VehicleSeat.RightRear : VehicleSeat.LeftRear);
                }
            }
            if (CustomerStartPosition.X != 0 && CustomerStartPosition.Y != 0 && CustomerPed == null)
            {
                if (CustomerStartPosition.DistanceTo(veh.Position) < 300f)
                {
                    var pedPos = World.GetNextPositionOnSidewalk(World.GetNextPositionOnStreet(CustomerStartPosition));
                    if (pedPos.X != 0 && pedPos.Y != 0)
                    {
                        CustomerStartPosition = pedPos;
                        Function.Call(Hash.SET_NEW_WAYPOINT, pedPos.X, pedPos.Y);
                    }
                }
                if (CustomerStartPosition.DistanceTo(veh.Position) < 50f)
                {
                    CustomerPed = World.CreatePed(new Model(peds[rnd.Next(0, peds.Count - 1)]), new Vector3(CustomerStartPosition.X, CustomerStartPosition.Y, veh.Position.Z));
                    CustomerPed.AddBlip();
                    CustomerPed.CurrentBlip.Color = BlipColor.Blue;
                }
            }
        }

        public static void SearchForCustomer()
        {
            if (PossibleCustomersPositions.Count == 0)
            {
                searchingForCustomersStopwatch = new Stopwatch();
                searchingForCustomersStopwatch.Start();
                int possibleCustomersCount = 3;
                for (var i = 0; i < possibleCustomersCount; ++i)
                {
                    var pos = Game.Player.Character.Position.Around((float)rnd.Next(100, 1000));
                    PossibleCustomersPositions.Add(pos);
                }
                PossibleCustomersPositions.OrderBy(v => v.DistanceTo(veh.Position));
            }
        }

        public static void AppControls()
        {
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && !showApp)
            {
                showApp = true;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed && showApp)
            {
                showApp = false;
            }
            if (showApp)
            {
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && lastRadioMenuButtonState == ButtonState.Released)
                {
                    lastRadioMenuButtonState = ButtonState.Pressed;
                    appCurrentHover -= 1;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && lastRadioMenuButtonState == ButtonState.Released)
                {
                    lastRadioMenuButtonState = ButtonState.Pressed;
                    appCurrentHover += 1;
                }
                if (appCurrentHover > (PossibleCustomersPositions.Count - 1))
                    appCurrentHover = 0;
                if (appCurrentHover < 0)
                    appCurrentHover = PossibleCustomersPositions.Count - 1;
                if (lastRadioMenuButtonState == ButtonState.Pressed && !radioMenuScrollDelay.IsRunning)
                    radioMenuScrollDelay.Start();
                if (radioMenuScrollDelay.ElapsedMilliseconds > 200)
                {
                    lastRadioMenuButtonState = ButtonState.Released;
                    radioMenuScrollDelay = new Stopwatch();
                }
                if (Game.IsControlJustPressed(0, Control.VehicleHandbrake))
                {
                    if (isTripActive)
                    {
                        isTripActive = false;
                        SearchForCustomer();
                        if (CustomerPed != null)
                        {
                            CustomerPed.Delete();
                            CustomerPed = null;
                            isCustomerInCar = false;
                        }
                    }
                    else
                    {
                        SelectCustomer();
                    }
                }
            }
        }

        public static void AppUi()
        {
            if (showApp)
            {
                var radioBackground = new UIContainer(new Point(UI.WIDTH - 200, UI.HEIGHT - 210), new Size(140, 210), Color.FromArgb(255, 5, 5, 5));
                // antena
                // display background  
                AppDisplay = new UIContainer(new Point(UI.WIDTH - 190, UI.HEIGHT - 170), new Size(120, 160), Color.White);
                // motorola logo
                if (searchingForCustomersStopwatch.IsRunning && searchingForCustomersStopwatch.ElapsedMilliseconds < 4500)
                {
                    AppDisplay.Items.Add(new UIText("Searching for trips...", new Point(6, 70), 0.30f, Color.FromArgb(190, 0, 0, 0), GTA.Font.ChaletLondon, false));
                }
                else if (PossibleCustomersPositions.Count > 0 && !isTripActive)
                {
                    int fakeIndex = 0;
                    for (var i = 0; i < PossibleCustomersPositions.Count; ++i)
                    {
                        if ((appCurrentHover - i >= 0 && appCurrentHover - i <= 3) || (i - appCurrentHover >= 0 && i - appCurrentHover <= 3))
                        {
                            AppPossibleCustomersMenu(PossibleCustomersPositions[i], fakeIndex, i);
                            fakeIndex++;
                        }
                    }
                }
                else if (isTripActive)
                {
                    var item = new UIContainer(new Point(15, 60), new Size(90, 25), Color.PaleVioletRed);
                    item.Items.Add(new UIText("Cancel Trip", new Point(44, 4), 0.28f, Color.White, GTA.Font.ChaletLondon, true));
                    AppDisplay.Items.Add(item);
                }
                else if (tripCompleteStopwatch.ElapsedMilliseconds > 0)
                {
                    AppDisplay.Items.Add(new UIText("Trip completed", new Point(39, 70), 0.30f, Color.FromArgb(190, 0, 0, 0), GTA.Font.ChaletLondon, false));
                }
                radioBackground.Items.Add(new UIText("UBER", new Point(72, 14), 0.46f, Color.White, GTA.Font.Monospace, true));
                radioBackground.Enabled = true;
                radioBackground.Draw();
                AppDisplay.Draw();
            }

        }

        private static void AppPossibleCustomersMenu(Vector3 position, int index, int realIndex)
        {
            Color textColor = realIndex == appCurrentHover ? Color.White : Color.Black;
            float distance = veh.Position.DistanceTo(position);
            string distanceText = "";
            if (distance >= 1000)
            {
                distanceText = Math.Round(distance / 1000, 1) + " km";
            }
            else
                distanceText = Math.Round(distance, 0) + " m";
            var y = (index * 46) + 8;
            var item = new UIContainer(new Point(5, y), new Size(110, 40), Color.FromArgb(realIndex == appCurrentHover ? 255 : 50, 0, 0, 0));
            item.Items.Add(new UIText(World.GetStreetName(position), new Point(55, 1), 0.25f, textColor, GTA.Font.ChaletLondon, true));
            item.Items.Add(new UIText(distanceText, new Point(55, 20), 0.22f, textColor, GTA.Font.ChaletLondon, true));
            AppDisplay.Items.Add(item);
        }

    }
}
