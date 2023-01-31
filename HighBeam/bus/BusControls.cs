using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XInputDotNetPure;
using static HighBeam.Main;
using static HighBeam.bus.BusRoute;

namespace HighBeam.bus
{
    public static class BusControls
    {
        public static bool isOpeningDoor = false;
        public static Stopwatch doorOpeningStopWatch = new Stopwatch();
        public static float doorAngle = 0f;
        public static string doorDirection = "open";
        public static bool isAirSuspension = false;
        public static bool isAirSuspensionOperating = false;
        public static float airSuspensionForce = 0f;
        public static bool canGetOutside = false;
        public static bool isInVeh = false;
        public static Stopwatch busModeStopWatch = new Stopwatch();
        public static bool isBusMode = false;
        public static void RunBusControls()
        {
            if (Game.Player.Character.IsInVehicle(veh) && !isInVeh)
                canGetOutside = false;
            isInVeh = Game.Player.Character.IsInVehicle(veh);
            if (veh.DisplayName.ToLower().Contains("exped") || veh.DisplayName.ToLower().Contains("sprinte"))
            {
                Doors();
                BusModeControl();
                if (isBusMode)
                {
                    HighBeam.bus.BusRoute.RunRoute();
                }
            }
        }

        public static void BusModeControl()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == ButtonState.Pressed && !busModeStopWatch.IsRunning)
            {
                busModeStopWatch.Start();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == ButtonState.Pressed && busModeStopWatch.ElapsedMilliseconds > 100)
            {
                isBusMode = isBusMode ? false : true;
                SetBusRoute(exit: isBusMode ? false : true);
                busModeStopWatch = new Stopwatch();
            }
            if (busModeStopWatch.ElapsedMilliseconds > 500)
            {
                busModeStopWatch = new Stopwatch();
            }
        }
        public static void Doors()
        {
            if (Game.IsControlJustPressed(0, Control.ScriptPadDown) && !isOpeningDoor && veh.Speed * 3.6 < 20)
            {
                isOpeningDoor = true;
                if (!veh.DisplayName.ToLower().Contains("clio"))
                {
                    isAirSuspension = true;
                    isAirSuspensionOperating = true;
                }
                if (doorAngle <= 0f)
                    doorDirection = "open";
                else
                    doorDirection = "close";
            }
            if (isAirSuspensionOperating)
            {
                airSuspensionForce = (doorDirection == "open" ? airSuspensionForce - 0.0012f : airSuspensionForce + 0.0012f);
                if (airSuspensionForce <= -0.12f)
                {
                    isAirSuspensionOperating = false;
                }
                if (airSuspensionForce >= 0f)
                {
                    isAirSuspension = false;
                    isAirSuspensionOperating = false;
                }
            }
            if (isAirSuspension)
            {
                veh.ApplyForce(new Vector3(0, 0, airSuspensionForce), new Vector3());
            }
            if (isOpeningDoor)
            {
                var isSprinter = veh.DisplayName.ToLower().Contains("sprinter");
                if (Game.Player.Character.IsInVehicle(veh) && !canGetOutside)
                    Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Driver);
                doorAngle = (doorDirection == "open" ? doorAngle + 0.004f : doorAngle - 0.004f);
                if (isSprinter)
                {
                    Function.Call(Hash.SET_VEHICLE_DOOR_CONTROL, veh, 2, 0, doorAngle);
                }
                else
                {
                    Function.Call(Hash.SET_VEHICLE_DOOR_CONTROL, veh, veh.DisplayName.ToLower().Contains("clio") ? 1 : 0, 0, doorAngle);
                }

                if (doorAngle <= 0)
                {
                    doorAngle = 0f;
                    if (isSprinter)
                        veh.CloseDoor(VehicleDoor.BackLeftDoor, false);
                    else
                        veh.CloseDoor(veh.DisplayName.ToLower().Contains("clio") ? VehicleDoor.FrontRightDoor : VehicleDoor.FrontLeftDoor, false);
                    doorOpeningStopWatch = new Stopwatch();
                    isOpeningDoor = false;
                }
                else if (doorAngle >= 1)
                {
                    doorAngle = 1f;
                    isOpeningDoor = false;
                }
            }
            if (doorAngle >= 1f && !isOpeningDoor && !canGetOutside)
            {
                Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Driver);
            }
        }
    }
}
