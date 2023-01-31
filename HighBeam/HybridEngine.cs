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

namespace HighBeam
{
    public static class HybridEngine
    {
        public static bool isSoundSet = true;
        public static bool isEv = false;
        public static Stopwatch chillAccelerationSpeedCheck = new Stopwatch();
        public static float chillAccelerationLastSpeed = 0f;
        public static bool disableAcceleration = false;
        public static bool isChillMode = false;
        public static float lastSpeed;
        public static int lastFakeSpeed = 0;
        public static int canTurnOnBoostStages = 0;
        public static Stopwatch canTurnOnBoostStopwatch = new Stopwatch();
        public static Stopwatch canTurnOffBoostStopwatch = new Stopwatch();
        public static bool isHybridMode = false;
        public static bool isElectricMode = false;
        public static Stopwatch tailgateStopWatch = new Stopwatch();
        public static bool toggleChargerCord = false;
        public static bool isChargerCord;
        public static List<Prop> ChargerProps = new List<Prop>();
        public static bool isTesla = false;
        public static bool isPhevVehicle = false;
        public static bool isPhevOnElectricEngine = false;
        private static Vehicle prevVeh;
        private static Exhaust currentPhev;
        private static string currentSound;
        private static Stopwatch isStoppedStopwatch = new Stopwatch();
        public static bool isRangeExtenderRunning = false;

        public static bool willChangePhevSound;
        public static void Hybrid()
        {
            /* var sensorPos = veh.GetOffsetInWorldCoords(new Vector3(0, 2.3f, -0.1f));
             var sensorDir = veh.GetOffsetInWorldCoords(new Vector3(0, 3f, -0.1f));
             Function.Call((Hash)0x6B7256074AE34680, sensorPos.X, sensorPos.Y, sensorPos.Z, sensorPos.X, sensorPos.Y, (sensorPos.Z + 1f), 255, 5, 5, 255); 

             var raycast = World.Raycast(sensorPos, sensorDir, IntersectOptions.Everything); 
             Function.Call((Hash)0x6B7256074AE34680, sensorDir.X, sensorDir.Y, sensorDir.Z, sensorDir.X, sensorDir.Y, (sensorDir.Z + 1f), 255, 5, 5, 255);
             UI.ShowSubtitle(raycast.DitHitAnything.ToString());*/
            if (veh?.DisplayName != prevVeh?.DisplayName)
            {
                currentPhev = PhevList.Where(e => veh.DisplayName.ToLower().Contains(e.VehName))?.FirstOrDefault();
                currentSound = "surge";
                batteryAmount = 10f;
                batteryCapacity = 10f;
                batteryRemainingPercentage = 100f;
                isRangeExtenderRunning = false;
            }
            isPhevVehicle = currentPhev?.VehName != null;
            if (isPhevVehicle)
            {
                isLowBattery = batteryRemainingPercentage < 5f;
                if (drivingMode == "ev" && !isLowBattery)
                {
                    isPhevOnElectricEngine = true;
                }
                else
                {
                    PhevAutoMode();
                }
                if ((isLowBattery || isRangeExtenderRunning) && drivingMode == "ev")
                {
                    drivingMode = null;
                }

                if (isCruiseControlActive)
                    isPhevOnElectricEngine = false;

                batteryRemainingPercentage = (batteryAmount / batteryCapacity) * 100;
                var isBatteryCritical = batteryRemainingPercentage < 2f;
                if (isBatteryCritical)
                    isPhevOnElectricEngine = false;

                if (isPhevOnElectricEngine)
                    ElectricEngine();
                else
                    CombustionEngine();
                if (!isCruiseControlActive)
                    Regen();
            }

            prevVeh = veh;
        }

        private static void Regen()
        {
            float regen = 0;
            if (fakeSpeed > 20 && GamePad.GetState(PlayerIndex.One).Triggers.Right < 0.02f)
            {
                regen = 0.00001f;
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.1f)
                {
                    regen += 0.00006f;
                }
            }
            batteryAmount += regen;
            if (batteryAmount >= batteryCapacity)
                batteryAmount = batteryCapacity;

            if (isLowBattery && !isRangeExtenderRunning)
            {
                isPhevOnElectricEngine = false;
                isRangeExtenderRunning = true;
            }
            if (isRangeExtenderRunning && !isPhevOnElectricEngine)
            {
                batteryAmount += 0.00099f;
            }
            if (batteryRemainingPercentage > 97 && isRangeExtenderRunning)
            {
                isRangeExtenderRunning = false;
            }

            //  UI.ShowSubtitle("amoun: " + batteryAmount + " fuel remain: " + batteryRemainingPercentage + " regen: " + regen);
        }

        private static Stopwatch isSailingStopwatch = new Stopwatch();
        private static void PhevAutoMode()
        {
            if (fakeSpeed < 1)
            {
                isStoppedStopwatch.Start();
            }
            else
            {
                isStoppedStopwatch = new Stopwatch();
                isPhevOnElectricEngine = false;
            }

            if (isStoppedStopwatch.ElapsedMilliseconds > 1000)
            {
                isPhevOnElectricEngine = true;
            }

            if (fakeSpeed > 0 && GamePad.GetState(PlayerIndex.One).Triggers.Right < 0.1f)
            {
                isSailingStopwatch.Start();
            }
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.99f && fakeSpeed > 43)
            {
                isSailingStopwatch = new Stopwatch();
                isPhevOnElectricEngine = false;
            }
            if (isSailingStopwatch.ElapsedMilliseconds > 1000)
            {
                isPhevOnElectricEngine = true;
            }
        }

        private static void ElectricEngine()
        {
            if (fakeSpeed > 43)
                veh.EngineTorqueMultiplier = 0.7f;
            else
                veh.EngineTorqueMultiplier = 1.5f;

            if (fakeSpeed > 118 && drivingMode == "ev")
            {
                veh.EngineTorqueMultiplier = 0.25f;
            }

            if (currentSound != currentPhev.Active)
            {
                if (!willChangePhevSound)
                    willChangePhevSound = true;
                else
                {
                    Function.Call((Hash)0x4F0C413926060B38, veh, currentPhev.Active);
                    currentSound = currentPhev.Active;
                    willChangePhevSound = false;
                }
            }
            BatteryConsumption();
        }

        public static void CombustionEngine()
        {
            veh.EnginePowerMultiplier = 1f;
            if (currentSound != currentPhev.Original)
            {
                if (!willChangePhevSound)
                    willChangePhevSound = true;
                else
                {
                    Function.Call((Hash)0x4F0C413926060B38, veh, currentPhev.Original);
                    willChangePhevSound = false;
                    currentSound = currentPhev.Original;
                }
            }
        }

        private static List<Exhaust> PhevList = new List<Exhaust>()
        {
          // new Exhaust(){VehName = "sahara", Original = "sandking", Active = "surge"},
         //  new Exhaust(){VehName = "63", Original = "tailgater", Active = "surge"},
      //     new Exhaust(){VehName = "bmw", Original = "tailgater", Active = "surge"},
           new Exhaust(){VehName = "x5", Original = "baller", Active = "surge"},
             new Exhaust(){VehName = "s60", Original = "tailgater", Active = "surge"},
             new Exhaust(){VehName = "toycamr", Original = "asea", Active = "surge"},
            // new Exhaust(){VehName = "f pac", Original = "blista", Active = "surge"}, 
        };

        public static float batteryAmount = 2f;
        public static float batteryCapacity = 10f;
        public static float batteryRemainingPercentage = 20f;
        public static bool isLowBattery = false;

        public static double instantBatteryConsumption = 0.0;
        public static Stopwatch instantFuelConsumptionStopwatch = new Stopwatch();


        public static void BatteryConsumption()
        {
            float throttle = isCruiseControlActive ? 0.5f : GamePad.GetState(PlayerIndex.One).Triggers.Right * 1f;
            if (throttle <= 0 && fakeSpeed < 10)
            {
                throttle = 0.01f;
            }

            var pitch = (float)(Math.Round((veh.ForwardVector.Z * 100), 0)) / 100;


            float engineSize = Function.Call<float>((Hash)0x5DD35C8D074E57AE, veh);
            var currentConsumption = ((((((engineSize * 10) * (engineSize * 10)) / 2f) + ((float)((float)(fakeSpeed * fakeSpeed) / 10000))) * throttle) + (throttle > 0.05f ? pitch : 0)) / 13f;
            if (currentConsumption <= 0)
            {
                currentConsumption = 0;
            }

            if (instantFuelConsumptionStopwatch.ElapsedMilliseconds > 1000)
            {
                instantBatteryConsumption = Math.Round(currentConsumption * 100, 1);
                if (instantBatteryConsumption == 0)
                {
                    instantBatteryConsumption = 0.0;
                }
                instantFuelConsumptionStopwatch = new Stopwatch();
            }
            if (!instantFuelConsumptionStopwatch.IsRunning)
            {
                instantFuelConsumptionStopwatch.Start();
            }
            currentConsumption = (currentConsumption / 280);
            batteryAmount -= currentConsumption;
            if (batteryAmount <= 0)
            {
                batteryAmount = 0;
            }
            batteryRemainingPercentage = (batteryAmount / batteryCapacity) * 100;
            if (batteryRemainingPercentage < 3f)
            {
                veh.EngineTorqueMultiplier = 0.3f;
            }
            if (batteryRemainingPercentage < 0.1f)
            {
                veh.EngineTorqueMultiplier = 0.001f;
            }
            //  UI.ShowSubtitle("amoun: " + batteryAmount + " fuel remain: " + batteryRemainingPercentage + " consump: " + currentConsumption);

        }

        private static Stopwatch chargerLightsStopwatch = new Stopwatch();
        private static int chargerLightCount = 1;

        public static void ChargerCord()
        {
            if (isPhevVehicle && !Game.Player.Character.IsInVehicle())
            {
                if (Game.IsControlJustPressed(0, GTA.Control.VehicleBrake) && Game.Player.Character.Position.DistanceTo(veh.Position) < 4)
                {
                    if (!isChargerCord)
                    {
                        var cord1 = World.CreateProp(new Model(1877113268), new GTA.Math.Vector3(), false, false);
                        //   var cord2 = World.CreateProp(new Model(1877113268), new GTA.Math.Vector3(), false, false);
                        //  cord1.AttachTo(cord1, 0, new GTA.Math.Vector3(-2f, 0f, 2f), new GTA.Math.Vector3(0, 0, 90f));
                        //   cord1.AttachTo(cord1, 0, new GTA.Math.Vector3(-2f, 0f, 2f), new GTA.Math.Vector3(0, 90f, 0f));
                        var bone = veh.GetBoneIndex("door_dside_f");
                        //  var plugPos = Function.Call<VFunction.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneRR.X, boneRR.Y, boneRR.Z)ector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneRR.X, boneRR.Y, boneRR.Z)
                        Vector3 pos = new Vector3(0, -0.14f, -0.41f);
                        if (veh.DisplayName.ToLower().Contains("x5"))
                            pos = new Vector3(-0.03f, 0.16f, 0.04f);
                        if (veh.DisplayName.ToLower().Contains("s60"))
                            pos = new Vector3(-0.03f, 0.16f, 0.04f);
                        cord1.AttachTo(veh, bone, pos, new Vector3(0, 0, -90f));
                        ChargerProps.Add(cord1);
                        // ChargerProps.Add(cord2); 
                        isChargerCord = true;
                    }
                    else
                    {
                        ChargerProps.ForEach(p => p.Delete());
                        ChargerProps = new List<Prop>();
                        isChargerCord = false;
                    }
                }
            }



            if (isPhevVehicle && isChargerCord)
            {

                var pos = ChargerProps.First().GetOffsetInWorldCoords(new Vector3(0.0f, -0.13f, -0.35f));
                var z = 0.05f;
                for (var i = 0; i < chargerLightCount; i++)
                {
                    Function.Call((Hash)0x6B7256074AE34680, pos.X, pos.Y, pos.Z + z, pos.X, pos.Y, (pos.Z + z + 0.02), 63, 224, 0, 255);
                    z -= 0.06f;
                }
                if (!chargerLightsStopwatch.IsRunning)
                    chargerLightsStopwatch.Start();
                if (chargerLightsStopwatch.ElapsedMilliseconds > 600)
                {
                    chargerLightCount++;
                    chargerLightsStopwatch = new Stopwatch();
                }
                if (chargerLightCount > 4)
                {
                    chargerLightCount = 1;
                }
                if (batteryRemainingPercentage < 100f)
                {
                    batteryAmount += 0.003f;
                    batteryRemainingPercentage = (batteryAmount / batteryCapacity) * 100;
                }

            }


        }
        public static void BoostMode()
        {
            if (isBoostMode)
            {
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right <= 0 && !canTurnOffBoostStopwatch.IsRunning)
                {
                    canTurnOffBoostStopwatch = new Stopwatch();
                    canTurnOffBoostStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.1)
                {
                    canTurnOffBoostStopwatch = new Stopwatch();
                }
                if (canTurnOffBoostStopwatch.ElapsedMilliseconds > 3000)
                {
                    isBoostMode = false;
                    if (fakeSpeed < 90)
                    {
                        isEv = true;
                        isSoundSet = false;
                    }
                    canTurnOffBoostStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1 && fakeSpeed < 90 && !isLaunchControl)
                {
                    isBoostMode = false;
                    isEv = true;
                    isSoundSet = false;
                }
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 1 && canTurnOnBoostStages == 0)
                {
                    canTurnOnBoostStopwatch = new Stopwatch();
                    canTurnOnBoostStopwatch.Start();
                    canTurnOnBoostStages += 1;
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right <= 0 && canTurnOnBoostStages == 1)
                    canTurnOnBoostStages += 1;
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 1 && canTurnOnBoostStages == 2)
                    canTurnOnBoostStages += 1;
                if (canTurnOnBoostStages == 3)
                {
                    canTurnOnBoostStopwatch = new Stopwatch();
                    isBoostMode = true;
                    isEv = false;
                    isSoundSet = false;
                    canTurnOnBoostStages = 0;
                }
                if (canTurnOnBoostStopwatch.ElapsedMilliseconds > 500)
                {
                    canTurnOnBoostStages = 0;
                    canTurnOnBoostStopwatch = new Stopwatch();
                }
            }

        }
    }
}
