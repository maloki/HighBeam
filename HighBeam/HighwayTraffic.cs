using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using GTA.Math;
using static HighBeam.Paths;

namespace HighBeam
{
    public static class HighwayTraffic
    {
        private static int x = 0;
        private static int y = 0;
        private static int z = 0;
        private static Stopwatch MoveCarStopWatch = new Stopwatch();
        private static List<GeneralCar> carList = new List<GeneralCar>();
        private static Random r = new Random();

        public static void RunTestTraffic()
        {
            UpdateCoords();
            if (!MoveCarStopWatch.IsRunning)
            {
                MoveCarStopWatch.Start();
            }
            if (MoveCarStopWatch.IsRunning && MoveCarStopWatch.ElapsedMilliseconds >= 150)
            {
                MoveCar();
                MoveCarStopWatch = new Stopwatch();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
            {
                SpawnCar();
            }
        }

        private static void SpawnCar()
        {
            var pos = PathList.ElementAt(0).PathList.ElementAt(0);
            var carStats = GetRandomCar();
            var car = new GeneralCar()
            {
                Position = 0,
                Stats = carStats,
                Vehicle = World.CreateVehicle(carStats.Model, pos.Position, pos.Direction)
            };
            car.Vehicle.PlaceOnGround();
            car.Vehicle.EngineRunning = true;
            carList.Add(car);
        }

        private static void MoveCar()
        {
            foreach (var car in carList)
            {
                car.Position += 1;
                var nextPos = PathList.ElementAt(0).PathList.ElementAt(car.Position);
                car.Vehicle.Speed = nextPos.Speed; 
                Function.Call<float>((Hash)0x42A8EC77D5150CBE, car.Vehicle, nextPos.Steer);
            }
        }

        private static void UpdateCoords()
        {
            x = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).X, 0).ToString());
            y = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Y, 0).ToString());
            z = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Z, 0).ToString());
        }

        private static Vector2 GetClosestPoint(Vector2 v1, Vector2 v2, Vector2 p)
        {
            Vector2 AP = p - v1;
            Vector2 AB = v2 - v1;
            float ab2 = AB.X * AB.X + AB.Y * AB.Y;
            float ap_ab = AP.X * AB.X + AP.Y * AB.Y;
            float t = ap_ab / ab2;
            Vector2 closest = v1 + AB * t;
            return closest;
        }

        private static float dist2(Vector2 v1, Vector2 v2)
        {
            return (float)(Math.Sqrt(v1.X - v2.X) + Math.Sqrt(v1.Y - v2.Y));
        }

        private static CarStatsModel GetRandomCar(bool isServiceCar = false, int serviceIndex = 0)
        {
            return new CarStatsModel();
        }

        private static int GenerateRandomNumberBetween(int min, int max, bool isLookingRnd = false)
        {
            return r.Next(min, max);
        }

        public class GeneralCar
        {
            public Vehicle Vehicle { get; set; }
            public CarStatsModel Stats { get; set; }
            public int Position { get; set; }
        }
    }
}
