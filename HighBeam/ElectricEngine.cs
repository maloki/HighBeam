using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XInputDotNetPure;
using static HighBeam.Main;

namespace HighBeam
{
    public static class ElectricEngine
    {

        private static List<Exhaust> EvList = new List<Exhaust>()
        {
           new Exhaust(){VehName = "xc40", Original = "surge", Active = "surge"},
             new Exhaust(){VehName = "mini", Original = "surge", Active = "surge"},
              new Exhaust(){VehName = "camry", Original = "surge", Active = "surge"},
               new Exhaust(){VehName = "gcmtes", Original = "surge", Active = "surge"},
        };

        private static Vehicle prevVeh;
        private static Exhaust currentEv;
        public static bool isEvVehicle;

        public static void ElEngine() 
        {
            if (veh?.DisplayName != prevVeh?.DisplayName)
            {
                currentEv = EvList.Where(e => veh.DisplayName.ToLower().Contains(e.VehName))?.FirstOrDefault();
                batteryAmount = 100f;
                batteryCapacity = 100f;
                batteryRemainingPercentage = 100f;
                if (veh.DisplayName.ToLower().Contains("camry"))
                {
                    batteryCapacity = 38f;
                    batteryAmount = 38f;
                }
            }
            isEvVehicle = currentEv?.VehName != null;
            if (isEvVehicle)
            {
                isLowBattery = batteryRemainingPercentage < 15f;

                batteryRemainingPercentage = (batteryAmount / batteryCapacity) * 100;
                if (true)
                {
                    Throttle();
                    BatteryConsumption();
                    Regen();
                }

            }

            prevVeh = veh;
        }

        private static void Throttle()
        {
            var evName = veh.DisplayName.ToLower();
            if (evName.Contains("gcmtes"))
            {
                if (true)
                {
                    if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.9f && fakeSpeed > 2)
                    {
                        if (fakeSpeed < 109)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, 0.11f, 0f));
                        }
                        if (fakeSpeed > 109 && fakeSpeed < 201)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, 0.058f, 0f));
                        }
                        if (fakeSpeed > 200 && fakeSpeed < 252)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, 0.04f, 0f));
                        }
                        if (fakeSpeed > 252)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, -0.03f, 0f));
                        }
                    }
                }
            }
            if (evName.Contains("xc40"))
            {
                if (true)
                {
                    if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.9f && fakeSpeed > 2)
                    {
                        if (fakeSpeed < 49)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, 0.055f, 0f));
                        } else if (fakeSpeed < 102)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, 0.042f, 0f));
                        }
                        else if (fakeSpeed < 142)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, 0.031f, 0f));
                        }
                        else if (fakeSpeed < 202)
                        {
                            veh.ApplyForceRelative(new GTA.Math.Vector3(0, 0.020f, 0f));
                        }
                    }
                }
            }
        }

        private static void Regen()
        {
            float regen = 0;
            if (fakeSpeed > 20 && GamePad.GetState(PlayerIndex.One).Triggers.Right < 0.02f)
            {
                regen = 0.00001f;
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.1f)
                {
                    regen += 0.00009f;
                }
            }
            batteryAmount += regen;
            if (batteryAmount >= batteryCapacity)
                batteryAmount = batteryCapacity;

            //  UI.ShowSubtitle("amoun: " + batteryAmount + " fuel remain: " + batteryRemainingPercentage + " regen: " + regen);
        }

        public static float batteryAmount = 1f;
        public static float batteryCapacity = 10f;
        public static float batteryRemainingPercentage = 10f;
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
            //UI.ShowSubtitle("amoun: " + batteryAmount + " fuel remain: " + batteryRemainingPercentage + " consump: " + currentConsumption);

        }

        private static Stopwatch chargerLightsStopwatch = new Stopwatch();
        private static int chargerLightCount = 1;
        public static List<Prop> ChargerProps = new List<Prop>();
        public static bool isChargerCordEv;
        public static void ChargerCordEv()
        {
            if (isEvVehicle && !Game.Player.Character.IsInVehicle())
            {
                if (Game.IsControlJustPressed(0, GTA.Control.VehicleBrake) && Game.Player.Character.Position.DistanceTo(veh.Position) < 4)
                {
                    if (!isChargerCordEv)
                    {
                        var cord1 = World.CreateProp(new Model(1877113268), new GTA.Math.Vector3(), false, false);
                        //   var cord2 = World.CreateProp(new Model(1877113268), new GTA.Math.Vector3(), false, false);
                        //  cord1.AttachTo(cord1, 0, new GTA.Math.Vector3(-2f, 0f, 2f), new GTA.Math.Vector3(0, 0, 90f));
                        //   cord1.AttachTo(cord1, 0, new GTA.Math.Vector3(-2f, 0f, 2f), new GTA.Math.Vector3(0, 90f, 0f));
                        var bone = veh.GetBoneIndex("door_dside_f");
                        //  var plugPos = Function.Call<VFunction.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneRR.X, boneRR.Y, boneRR.Z)ector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneRR.X, boneRR.Y, boneRR.Z)
                        Vector3 pos = new Vector3(0, -0.14f, -0.41f);
                        if (veh.DisplayName.ToLower().Contains("crossik"))
                            pos = new Vector3(0, 0.2f, 0.23f);
                        if (veh.DisplayName.ToLower().Contains("camry"))
                            pos = new Vector3(-0.03f, -2.3f, 0.24f);
                        cord1.AttachTo(veh, bone, pos, new Vector3(0, 0, -90f));
                        ChargerProps.Add(cord1);
                        // ChargerProps.Add(cord2); 
                        isChargerCordEv = true;
                    }
                    else
                    {
                        ChargerProps.ForEach(p => p.Delete());
                        ChargerProps = new List<Prop>();
                        isChargerCordEv = false;
                    }
                }
            }



            if (isEvVehicle && isChargerCordEv)
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
                    batteryAmount += 0.0075f;
                    batteryRemainingPercentage = (batteryAmount / batteryCapacity) * 100;
                }

            }

        }
    }
}
