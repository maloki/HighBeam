using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XInputDotNetPure;
using static HighBeam.Main;
using static HighBeam.HybridEngine;
using static HighBeam.ElectricEngine;

namespace HighBeam
{
    public static class Fuel
    {
        public static Stopwatch savingFuelToFileStopwatch = new Stopwatch();
        private static bool init = false;
        public static bool isWRC = false;
        public static void RunFuel()
        {
            if (!init)
            {
                FuelMap();
                init = true;
            }

            if (veh.EngineRunning)
            {
                if (!isPhevOnElectricEngine && !isWRC && !isEvVehicle)
                {
                    FuelConsumption();
                }
                if (isPhevOnElectricEngine)
                    instantFuelConsumption = 0;
                DistanceTraveled();
            }

            if (!savingFuelToFileStopwatch.IsRunning)
            {
                savingFuelToFileStopwatch.Start();
            }
            if (savingFuelToFileStopwatch.ElapsedMilliseconds > 10000)
            {
                savingFuelToFileStopwatch = new Stopwatch();
                WriteFuelFile(veh.Model.Hash, fuelAmount);
            }
        }

        public static void FuelMap()
        {
            fuelStationList.ForEach(v =>
            {
                var blip = World.CreateBlip(v);
                blip.Sprite = BlipSprite.ArmsTraffickingGround;
                blip.IsShortRange = true;
            });
        }

        public static List<Vector3> fuelStationList = new List<Vector3>()
        {
            new Vector3(2581.3f, 361.8f, 108.4f),
            new Vector3(1208.8f, -1402.4f, 34f),
            new Vector3(818.2f, -1028.6f, 26.4f),
            new Vector3(1181f, -330.8f, 69.3f),
            new Vector3(264.8f, -1262.9f, 29.3f),
            new Vector3(174.5f, -1561.6f, 29.3f),
            new Vector3(-69.6f, -1761.5f, 29.5f),
            new Vector3(-319.4f, -1471.5f, 30.5f),
            new Vector3(-524.5f, -1212.7f, 18.3f),
            new Vector3(-723.5f, -936f, 19.2f),
            new Vector3(-1437.1f, -275.4f, 46.2f),
            new Vector3(-2097.3f, -319.8f, 13.2f),
            new Vector3(-1799.1f, 803.2f, 138.5f),
            new Vector3(620.8f, 268.8f, 103f),
             new Vector3(-2555.1f, 2334.7f, 33f),
              new Vector3(1038.2f, 2671.3f, 39f),
              new Vector3(2005.5f, 3773.3f, 32.4f),
              new Vector3(1701.4f, 6415.7f, 32.7f),
                new Vector3(180.0f, 6603.9f, 32.0f),
                // LC
                  new Vector3(4707.47f, -3465.3f, 3.1f),
                  new Vector3(3797.1f, -3225.6f, 6.9f),
                new Vector3(3873.8f, -1515.9f, 27.8f),
             new Vector3(5297.0f, -2119.3f, 14.5f),
           new Vector3(4753.3f, -3277.2f, 9.8f),
              new Vector3(6641.8f, -1544.6f, 16.6f),
              new Vector3(6313f, -2927f, 29.7f),
                new Vector3(6959f, -2419.1f, 16.5f),
                // autobahn 1
                new Vector3(8156.4f, 8510.3f, 6.4f),
                // autobahn to ny
                new Vector3(2803.7f, 6932.7f, 6.4f),
                new Vector3(2838.2f, 7077.1f, 6.4f),
                // s1 chiliad mountain
                new Vector3(-4521.1f, 2377.1f, 36.5f),
                //las vegas 
                new Vector3(-2381.4f, 5801.2f, 10.7f),
                new Vector3(-1861.6f, 5985.6f, 10.7f),
                  new Vector3(-2294.5f, 7353.8f, 10.7f),
                  // -5 a4
                    new Vector3(20671.3f, 16403.6f, 3.4f),
                    // dk94 bida
                    new Vector3(-5803.4f, -668.9f, 2.5f),
                    // dk94 dg
                    new Vector3(-9703.5f, -5618.2f, 4.1f),
                    // zkpane dk7
                    new Vector3(6198.1f, -5595.3f, 76.4f),
                    //dk61 
                     new Vector3(-4426.6f, 6099.2f, 18.8f),
                     // a4 1st german autohof
                        new Vector3(17929.3f, -5219f, 8.3f),
                        // pyrzowice airport
                          new Vector3(20926.8f, -6943.5f, 3.2f),
        };

        public static bool isFuelPumpActive = false;
        public static double fuelPrice = 5.48;
        public static double litersAdded = 0;
        public static double fuelPumpRate = 0.05;

        public static void FuelStation()
        {
            if (isFuelPumpActive)
            {
                var cluster = new UIContainer(new Point(40, UI.HEIGHT - 200), new Size(140, 60), Color.FromArgb(80, 0, 0, 0));
                if (isEngineRunning)
                {
                    cluster.Items.Add(new UIText("Turn Off Engine", new Point(70, 13), 0.3f, Color.OrangeRed, GTA.Font.ChaletLondon, true));
                }
                else
                {
                    Game.DisableControlThisFrame(0, Control.Attack);
                    if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.4f)
                    {
                        if (fuelAmount >= fuelTankCapacity)
                        {
                            fuelAmount = fuelTankCapacity;
                        }
                        else
                        {
                            litersAdded += fuelPumpRate;
                            fuelAmount += (float)fuelPumpRate;
                        }
                    }
                    cluster.Items.Add(new UIText(Math.Round(litersAdded * fuelPrice, 2).ToString(), new Point(70, 13), 0.46f, Color.White, GTA.Font.ChaletLondon, true));
                    cluster.Items.Add(new UIText(Math.Round(litersAdded, 2).ToString() + " L", new Point(76, 35), 0.22f, Color.White, GTA.Font.ChaletLondon, true));
                }
                cluster.Draw();
            }
            if (Game.IsControlJustPressed(0, Control.Sprint))
            {
                if (isFuelPumpActive)
                {
                    isFuelPumpActive = false;
                }
                else
                {
                    for (var i = 0; i < fuelStationList.Count; ++i)
                    {
                        if (fuelStationList[i].DistanceTo(Game.Player.Character.Position) < 30)
                        {
                            isFuelPumpActive = true;
                            litersAdded = 0;
                            break;
                        }
                    }
                }
            }
        }


        public static List<string> toSort = new List<string>();

        public static void ReadFuelFile()
        {
            try
            {
                var file = System.IO.File.ReadAllText(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\fuel_config.txt");
                toSort = new List<string>();
                bool wasAnyFuel = false;
                file.Split(',').ToList().ForEach(l =>
                {
                    if (l.Length > 2)
                    {
                        var model = l.Split(':')[0];
                        var amount = l.Split(':')[1];
                        var curOdo = double.Parse(l.Split(':')[2]);
                        if (int.Parse(model) == veh.Model.Hash)
                        {
                            fuelAmount = float.Parse(amount.Replace(",", string.Empty));
                            odo = curOdo;
                            wasAnyFuel = true;
                            if (fuelAmount < 8f)
                            {
                                fuelAmount = 30f;
                            }
                        }
                        else
                        {
                            toSort.Add($"{l.Replace(",", string.Empty)},");
                        }
                    }
                });
                if (!wasAnyFuel)
                {
                    fuelAmount = 30f;
                    odo = 0;
                }
                toSort.Insert(0, $"{veh.Model.Hash}:{(wasAnyFuel ? (int)fuelAmount : 35f)}:{(int)odo},");
                using (StreamWriter writer = new StreamWriter(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\fuel_config.txt", false))
                {
                    writer.Write(string.Join("", toSort));
                }
                CalculateFuelCapacity();
            }
            catch (Exception e)
            {
                UI.ShowSubtitle(e.Message);
            }
        }

        public static void CalculateFuelCapacity()
        {
            var dim = veh.Model.GetDimensions();
            var height = dim.Z * 10;
            var length = dim.Y * 10;

            var defaultSize = 40f;
            float engineSize = Function.Call<float>((Hash)0x5DD35C8D074E57AE, veh);
            var size = ((int)Math.Round((defaultSize + (height * height / 100) + (length * length / 100)) / 10.0)) * 10;
            if (engineSize >= 0.15f)
            {
                size += 10;
            }
            if (engineSize >= 0.2)
            {
                size += 20;
            }
            if (size > 100)
            {
                size = 100;
            }
            fuelTankCapacity = size;
        }

        public static void WriteFuelFile(int vehHash, float fuelAmount)
        {
            var i = 0;
            List<string> sorted = new List<string>();
            toSort.ForEach(l =>
           {
               if (i == 0)
               {
                   sorted.Add($"{vehHash}:{(int)fuelAmount}:{(int)odo},");
               }
               else if (l.Length > 2)
               {
                   sorted.Add($"{l.Replace(",", string.Empty)},");
               }
               i++;
           });
            using (StreamWriter writer = new StreamWriter(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\fuel_config.txt", false))
            {
                writer.Write(string.Join("", sorted));
            }
        }

        public static float fuelAmount = 50f;
        public static float fuelTankCapacity = 50f;
        public static float fuelRemainingPercentage = 0f;
        public static bool isLowFuel = false;

        public static double odo = 0;

        public static double instantFuelConsumption = 0.0;
        public static Stopwatch instantFuelConsumptionStopwatch = new Stopwatch();


        public static void FuelConsumption()
        {
            float throttle = isCruiseControlActive ? 0.5f : GamePad.GetState(PlayerIndex.One).Triggers.Right * 1f;
            if (throttle <= 0 && fakeSpeed < 10)
            {
                throttle = 0.05f;
            }

            var pitch = (float)(Math.Round((veh.ForwardVector.Z * 100), 0)) / 50;


            float engineSize = Function.Call<float>((Hash)0x5DD35C8D074E57AE, veh);
            var currentConsumption = ((((((engineSize * 10) * (engineSize * 10)) / 2f) + ((float)((float)(fakeSpeed * fakeSpeed) / 10000))) * throttle) + (throttle > 0.05f ? pitch : 0)) / 13f;
            if (currentConsumption <= 0)
            {
                currentConsumption = 0;
            }
            if (isRangeExtenderRunning)
                currentConsumption = currentConsumption * 1.5f;

            if (instantFuelConsumptionStopwatch.ElapsedMilliseconds > 1000)
            {
                instantFuelConsumption = Math.Round(currentConsumption * 100, 1);
                if (instantFuelConsumption == 0)
                {
                    instantFuelConsumption = 0.0;
                }
                if (instantFuelConsumption > 70)
                    instantFuelConsumption = 70;
                instantFuelConsumptionStopwatch = new Stopwatch();
            }
            if (!instantFuelConsumptionStopwatch.IsRunning)
            {
                instantFuelConsumptionStopwatch.Start();
            }
            currentConsumption = (currentConsumption / ((isPhevVehicle && fakeSpeed < 51) ? 440 : 280));

            if (currentConsumption > 0.0025)
                currentConsumption = 0.0025f;
            fuelAmount -= (currentConsumption / 2);
            if (fuelAmount <= 0)
            {
                fuelAmount = 0;
            }
            fuelRemainingPercentage = (fuelAmount / fuelTankCapacity) * 100;
            if (fuelRemainingPercentage < 14f)
            {
                isLowFuel = true;
            }
            else
            {
                isLowFuel = false;
            }
            if (fuelRemainingPercentage < 3f)
            {
                veh.EngineTorqueMultiplier = 0.3f;
            }
            if (fuelRemainingPercentage < 0.1f)
            {
                veh.EngineTorqueMultiplier = 0.001f;
            }
            //  UI.ShowSubtitle("amoun: " + fuelAmount + " fuel remain: " + fuelRemainingPercentage + " consump: " + instantFuelConsumption);

        }
        public static Stopwatch distanceTraveledStopwatch = new Stopwatch();
        public static void DistanceTraveled()
        {
            if (!distanceTraveledStopwatch.IsRunning)
            {
                distanceTraveledStopwatch.Start();
            }
            odo += ((fakeSpeed * (TimeSpan.FromMilliseconds(11).TotalHours)) * 10);
            tempOdo += (fakeSpeed * (TimeSpan.FromMilliseconds(1).TotalHours) * 30); 
            // tempOdo += (fakeSpeed * (TimeSpan.FromMilliseconds(1).TotalHours) * 20); real km

        }
    }
}

