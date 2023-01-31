using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.NewHighwayTraffic.CarSpawner;
using static HighBeam.NewHighwayTraffic.Zone;
using static HighBeam.NewHighwayTraffic.Helpers;
using static HighBeam.NewHighwayTraffic.Index;
using static HighBeam.NewHighwayTraffic.CarSpawner;
using GTA.Native;
using GTA;

namespace HighBeam.NewHighwayTraffic
{
    public static class MoveCar
    {
        public static void RunMoveCar(GeneralCar currentCar)
        {
            currentCar.Vehicle.EngineRunning = true;
            currentCar.Vehicle.Heading = currentCar.Stats.Heading;
            currentCar.Vehicle.Speed = currentCar.Stats.Speed;
        }

        public static bool CheckIfCarIsToDelete(GeneralCar car)
        {
            bool toDelete = false;
            var distBetweenPlayer = Function.Call<float>((Hash)0xF1B760881820C952, veh.Position.X, veh.Position.Y, veh.Position.Z, car.Vehicle.Position.X, car.Vehicle.Position.Y, car.Vehicle.Position.Z);
            Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
            var isCarAroundBehind = car.Vehicle.Position.DistanceTo(zoneEnd) > veh.Position.DistanceTo(zoneEnd);
            if (distBetweenPlayer > 100 && isCarAroundBehind && veh.Speed > car.Stats.DefaultSpeed)
            {
                toDelete = true;
            }
            bool isInZone = false;
            if (CurrentZone.Name != null)
            {
                var carX = (int)Math.Round(car.Vehicle.Position.X, 0);
                var carY = (int)Math.Round(car.Vehicle.Position.Y, 0);
                var t1 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY, CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY, CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY);
                var t2 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY, CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY, CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY);
                var t3 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY, CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY, CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY);
                var t4 = PointInTriangle(carX, carY, CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY, CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY, CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY);
                isInZone = t1 || t2 || t3 || t4;
                if (car.Vehicle.Position.Z - CurrentZone.ZoneBoundary.ZCoord > 2f)
                    toDelete = true;
            }
            if ((distBetweenPlayer > 1200f) || zoneEnd.DistanceTo(car.Vehicle.Position) < 50 || toDelete || !isInZone || car.Vehicle.HasCollidedWithAnything)
            {
                if (!car.Stats.Deleted)
                {
                    Vector3 del = new Vector3() { X = 0f, Y = 0f, Z = 0 };
                    car.Vehicle.Speed = 0f;
                    car.Vehicle.MarkAsNoLongerNeeded();
                    car.Vehicle.Position = del;
                    car.Stats.Deleted = true;
                    carList.Remove(car);
                }
                return true;
            }
            return false;
        }

        public static void ForceCarInFrontToChangeLane(GeneralCar c)
        {
            turnRight(c);
        }

        public static void AdjustCarSpeed(GeneralCar car)
        {
            if (car.Stats.deccelerate)
            {
                car.Stats.accelerate = false;
                if (car.Vehicle.Speed <= car.Stats.adjustSpeedTo)
                {
                    car.Vehicle.BrakeLightsOn = false;
                    car.Stats.deccelerate = false;
                    car.Vehicle.Speed = car.Stats.adjustSpeedTo;
                    car.Stats.Speed = car.Stats.adjustSpeedTo;
                    //UI.ShowSubtitle("speed is lower than to adjust");
                }
                else
                {
                    var isFast = car.Stats.Class == "nightsuperfast";
                    var bf = 0f;
                    var speedDif = (Math.Round(car.Stats.DefaultSpeed, 0) - Math.Round(car.Stats.adjustSpeedTo, 0));
                    bf = (float)(Math.Round(speedDif / 90, 2));
                    if (car.Stats.Speed - bf <= 0)
                    {
                        car.Stats.Speed = 0;
                        car.Vehicle.BrakeLightsOn = false;
                        car.Stats.deccelerate = false;
                    }
                    else
                    {
                        car.Stats.Speed -= bf;
                        car.Vehicle.BrakeLightsOn = true;
                        car.Stats.notDefaultSpeed = true;
                        // UI.ShowSubtitle("braking: " + bf + " speed: " + car.Stats.Speed * 3.6);
                    }
                }
            }
            if (car.Stats.accelerate)
            {
                car.Vehicle.BrakeLightsOn = false;
                car.Stats.deccelerate = false;
                if (car.Stats.Speed >= car.Stats.DefaultSpeed)
                {
                    car.Stats.accelerate = false;
                    car.Vehicle.Speed = car.Stats.DefaultSpeed;
                    car.Stats.notDefaultSpeed = false;
                }
                else
                {
                    var speedDif = Math.Round(car.Stats.DefaultSpeed, 0) - Math.Round(car.Stats.adjustSpeedTo, 0) + 100;
                    car.Stats.Speed += (float)Math.Round(speedDif / 2500, 3);
                }
            }
        }

        public static void Overtake(GeneralCar car)
        {
            if (!car.Stats.Deleted)
            {
                if (car.Stats.IsOvertaking)
                {
                    var leftBorder = GetClosestPoint(
                           new Vector2(CurrentZone.ZoneBoundary.StartLeftX, CurrentZone.ZoneBoundary.StartLeftY),
                           new Vector2(CurrentZone.ZoneBoundary.FinishLeftX, CurrentZone.ZoneBoundary.FinishLeftY),
                           new Vector2(car.Vehicle.Position.X, car.Vehicle.Position.Y)
                       );
                    var rightBorder = GetClosestPoint(
                        new Vector2(CurrentZone.ZoneBoundary.StartRightX, CurrentZone.ZoneBoundary.StartRightY),
                        new Vector2(CurrentZone.ZoneBoundary.FinishRightX, CurrentZone.ZoneBoundary.FinishRightY),
                        new Vector2(car.Vehicle.Position.X, car.Vehicle.Position.Y)
                    );
                    Vector3 centerPoint = new Vector3() { X = ((leftBorder.X + rightBorder.X) / 2), Y = ((leftBorder.Y + rightBorder.Y) / 2), Z = car.Vehicle.Position.Z };
                    Vector3 leftLane = new Vector3() { X = ((leftBorder.X + centerPoint.X) / 2), Y = ((leftBorder.Y + centerPoint.Y) / 2), Z = car.Vehicle.Position.Z };
                    Vector3 rightLane = new Vector3() { X = ((centerPoint.X + rightBorder.X) / 2), Y = ((centerPoint.Y + rightBorder.Y) / 2), Z = car.Vehicle.Position.Z };
                    Vector3 dir = leftLane - rightLane;
                    // float distanceBetweenLanes = (float)Math.Sqrt(((leftLane.X - rightLane.X) * 2) + ((leftLane.Y - rightLane.Y) * 2));
                    float distanceBetweenLanes = leftLane.DistanceTo(rightLane);
                    float steerRatio = car.Stats.Class.ToLower() == "truck" ? 0.0045f : 0.007f;
                    car.Stats.SteerPercentage = car.Stats.isLeftLane ? (car.Stats.SteerPercentage - steerRatio) : (car.Stats.SteerPercentage + steerRatio);
                    Vector3 desiredPosition = (car.Stats.SteerPercentage * leftLane + (1 - car.Stats.SteerPercentage) * rightLane);
                    car.Vehicle.Position = desiredPosition;
                    if ((car.Stats.SteerPercentage >= 0f && car.Stats.SteerPercentage <= 0.3 && !car.Stats.isLeftLane))
                    {
                        car.Stats.HeadingPercentage -= 0.08f;
                    }
                    if ((car.Stats.SteerPercentage <= 1f && car.Stats.SteerPercentage >= 0.7 && car.Stats.isLeftLane))
                    {
                        car.Stats.HeadingPercentage -= 0.08f;
                    }
                    if ((car.Stats.SteerPercentage >= 0.7f && car.Stats.SteerPercentage < 1f && !car.Stats.isLeftLane && (car.Vehicle.Heading > car.Stats.Heading)))
                    {
                        car.Stats.HeadingPercentage += 0.08f;
                    }
                    if ((car.Stats.SteerPercentage <= 0.3f && car.Stats.SteerPercentage > 0 && car.Stats.isLeftLane && (car.Vehicle.Heading < car.Stats.Heading)))
                    {
                        car.Stats.HeadingPercentage += 0.08f;
                    }
                    if ((car.Stats.SteerPercentage >= 1f && !car.Stats.isLeftLane))
                    {
                        car.Stats.IsOvertaking = false;
                        car.Stats.isLeftLane = true;
                        car.Vehicle.TaxiLightOn = true;
                        car.Vehicle.LeftIndicatorLightOn = false;
                        car.Vehicle.Heading = car.Stats.Heading;
                        car.Stats.HeadingPercentage = 0;
                        //  car.Stats.accelerate = true;
                        // car.Stats.adjustSpeedTo = GetSpeedForCar(rightLane: false);
                    }
                    if ((car.Stats.SteerPercentage <= 0f && car.Stats.isLeftLane))
                    {
                        car.Stats.IsOvertaking = false;
                        car.Stats.isLeftLane = false;
                        car.Vehicle.TaxiLightOn = false;
                        car.Vehicle.RightIndicatorLightOn = false;
                        car.Vehicle.Heading = car.Stats.Heading;
                        car.Stats.HeadingPercentage = 0;
                        //  car.Stats.deccelerate = true;
                        //   car.Stats.adjustSpeedTo = GetSpeedForCar(rightLane: true);
                    }
                    car.Vehicle.Heading = car.Stats.Heading + (car.Stats.isLeftLane ? car.Stats.HeadingPercentage : -(car.Stats.HeadingPercentage));
                    car.Vehicle.Speed = car.Stats.Speed;
                }
                else
                {
                    //     car.Stats.HeadingPercentage = 0;
                    // car.Vehicle.Heading = car.Stats.Heading;
                }
            }
        }

        private static void turnRight(GeneralCar c)
        {
            if (!c.Stats.IsOvertaking)
            {
                c.Stats.IsOvertaking = true;
                c.Stats.SteerPercentage = 0.9f;
                c.Vehicle.RightIndicatorLightOn = true;
            }
        }

        private static void turnLeft(GeneralCar c)
        {
            if (!c.Stats.IsOvertaking)
            {
                c.Stats.IsOvertaking = true;
                c.Stats.SteerPercentage = 0f;
                c.Vehicle.LeftIndicatorLightOn = true;
            }
        }
        public static void GetTraffic()
        {
            var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
            var m = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
            var s = Function.Call<int>((Hash)0x494E97C2EF27C470);
            bool isMinLeftLaneSpeed = (int)(Math.Round(veh.Speed * 3.6)) > 140 ? true : false;
            if (h >= 0 && h <= 2)
            {
                maxNumOfSpawnedCars = 6;
                minFake = 3;
                maxFake = 9;
            }
            if (h >= 3 && h <= 5)
            {
                maxNumOfSpawnedCars = 3;
                minFake = 2;
                maxFake = 5;
            }
            if (h == 6)
            {
                maxNumOfSpawnedCars = 12;
                minFake = 8;
                maxFake = 18;
            }
            if (h >= 7 && h < 9)
            {
                maxNumOfSpawnedCars = 36;
                minFake = 27;
                maxFake = 30;
            }
            if (h >= 9 && h < 14)
            {
                maxNumOfSpawnedCars = 10;
                minFake = 13;
                maxFake = 21;
            }
            if (h == 14)
            {
                maxNumOfSpawnedCars = 26;
                minFake = 18;
                maxFake = 25;
            }
            if (h >= 15 && h < 18)
            {
                maxNumOfSpawnedCars = 50;
                minFake = 29;
                maxFake = 30;
            }
            if (h >= 18 && h < 20)
            {
                maxNumOfSpawnedCars = 14;
                minFake = 14;
                maxFake = 27;
            }
            if (h >= 20 && h < 23)
            {
                maxNumOfSpawnedCars = 10;
                minFake = 12;
                maxFake = 22;
            }
            if (h == 23)
            {
                maxNumOfSpawnedCars = 8;
                minFake = 7;
                maxFake = 15;
            }
        }
    }
}
