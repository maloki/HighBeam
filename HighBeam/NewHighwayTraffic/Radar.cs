using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.NewHighwayTraffic.CarSpawner;
using static HighBeam.NewHighwayTraffic.Zone;
using static HighBeam.NewHighwayTraffic.Index;
using static HighBeam.Main;
using System.Security.Policy;
using GTA.Native;

namespace HighBeam.NewHighwayTraffic
{
    public static class Radar
    {

        public static void NewRadar(GeneralCar car)
        {
            Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
            var isCarAroundBehind = car.Vehicle.Position.DistanceTo(zoneEnd) > Index.veh.Position.DistanceTo(zoneEnd);
            double speed = Index.veh.Speed * 3.6;
            int meter =  speed <= 100 ? 34 : 70;
            if (speed > 100 && speed < 155)
                meter = 13;
            if (car.Stats.DefaultSpeed * 3.6 > 120 && !isCarAroundBehind && car.Vehicle.Position.DistanceTo(Index.veh.Position) < meter)
            {
                var v = car.Vehicle;
                bool isLeftLane = car.Stats.isLeftLane;
                float sidePointOffset = isLeftLane ? 5f : -5f;
                Vector3 side1 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 22f, 0));
                Vector3 side2 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 20f, 0));
                Vector3 side3 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 18f, 0));
                Vector3 side4 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 16f, 0));
                Vector3 side5 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 14f, 0));
                Vector3 side6 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 12f, 0));
                Vector3 side7 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 10f, 0));
                Vector3 side8 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 8f, 0));
                Vector3 side9 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 6f, 0));
                Vector3 side10 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 4f, 0));
                Vector3 side11 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 2f, 0));
                Vector3 side12 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 0f, 0));
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, side1.X, side1.Y, side1.Z - 5f, side1.X, side1.Y, side1.Z + 20f, 255, 5, 5, 255);
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, side2.X, side2.Y, side2.Z - 5f, side2.X, side2.Y, side2.Z + 20f, 255, 5, 5, 255);
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, side3.X, side3.Y, side3.Z - 5f, side3.X, side3.Y, side3.Z + 20f, 255, 5, 5, 255);
                Vehicle isSideCar = null;
                Vehicle side1Car = World.GetClosestVehicle(side1, 4f);
                Vehicle side2Car = World.GetClosestVehicle(side2, 4f);
                Vehicle side3Car = World.GetClosestVehicle(side3, 4f);
                Vehicle side4Car = World.GetClosestVehicle(side4, 4f);
                Vehicle side5Car = World.GetClosestVehicle(side5, 4f);
                Vehicle side6Car = World.GetClosestVehicle(side6, 4f);
                Vehicle side7Car = World.GetClosestVehicle(side7, 4f);
                Vehicle side8Car = World.GetClosestVehicle(side8, 4f);
                Vehicle side9Car = World.GetClosestVehicle(side9, 4f);
                Vehicle side10Car = World.GetClosestVehicle(side10, 4f);
                Vehicle side11Car = World.GetClosestVehicle(side11, 4f);
                Vehicle side12Car = World.GetClosestVehicle(side12, 4f);
                if (side1Car != null)
                    isSideCar = side1Car;
                if (side2Car != null)
                    isSideCar = side2Car;
                if (side3Car != null)
                    isSideCar = side3Car;
                if (side4Car != null)
                    isSideCar = side4Car;
                if (side5Car != null)
                    isSideCar = side5Car;
                if (side6Car != null)
                    isSideCar = side6Car;
                if (side7Car != null)
                    isSideCar = side7Car;
                if (side8Car != null)
                    isSideCar = side8Car;
                if (side9Car != null)
                    isSideCar = side9Car;
                if (side10Car != null)
                    isSideCar = side10Car;
                if (side11Car != null)
                    isSideCar = side11Car;
                if (side12Car != null)
                    isSideCar = side12Car;
                Vector3 front1 = v.GetOffsetInWorldCoords(new Vector3(0f, 43f, 0));
                Vector3 front2 = v.GetOffsetInWorldCoords(new Vector3(0f, 41f, 0));
                Vector3 front3 = v.GetOffsetInWorldCoords(new Vector3(0f, 39f, 0));
                Vector3 front4 = v.GetOffsetInWorldCoords(new Vector3(0f, 37f, 0));
                Vector3 front5 = v.GetOffsetInWorldCoords(new Vector3(0f, 35f, 0));
                Vector3 front6 = v.GetOffsetInWorldCoords(new Vector3(0f, 33f, 0));
                Vector3 front7 = v.GetOffsetInWorldCoords(new Vector3(0f, 31f, 0));
                Vector3 front8 = v.GetOffsetInWorldCoords(new Vector3(0f, 29f, 0));
                Vector3 front9 = v.GetOffsetInWorldCoords(new Vector3(0f, 27f, 0));
                Vector3 front10 = v.GetOffsetInWorldCoords(new Vector3(0f, 25f, 0));
                Vector3 front11 = v.GetOffsetInWorldCoords(new Vector3(0f, 23f, 0));
                Vector3 front12 = v.GetOffsetInWorldCoords(new Vector3(0f, 21f, 0));
                Vector3 front13 = v.GetOffsetInWorldCoords(new Vector3(0f, 19f, 0));
                Vector3 front14 = v.GetOffsetInWorldCoords(new Vector3(0f, 17f, 0));
                Vector3 front15 = v.GetOffsetInWorldCoords(new Vector3(0f, 15f, 0));
                Vector3 front16 = v.GetOffsetInWorldCoords(new Vector3(0f, 13f, 0));
                Vector3 front17 = v.GetOffsetInWorldCoords(new Vector3(0f, 11f, 0));
                Vector3 front18 = v.GetOffsetInWorldCoords(new Vector3(0f, 9f, 0));
                Vector3 front19 = v.GetOffsetInWorldCoords(new Vector3(0f, 7f, 0));
                Vector3 front20 = v.GetOffsetInWorldCoords(new Vector3(0f, 5f, 0));
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, front1.X, front1.Y, front1.Z - 5f, front1.X, front1.Y, front1.Z + 20f, 255, 5, 5, 255);
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, front2.X, front2.Y, front2.Z - 5f, front2.X, front2.Y, front2.Z + 20f, 255, 5, 5, 255);
                Vehicle isFrontCar = null;
                Vehicle front1Car = World.GetClosestVehicle(front1, 4f);
                Vehicle front2Car = World.GetClosestVehicle(front2, 4f);
                Vehicle front3Car = World.GetClosestVehicle(front3, 4f);
                Vehicle front4Car = World.GetClosestVehicle(front4, 4f);
                Vehicle front5Car = World.GetClosestVehicle(front5, 4f);
                Vehicle front6Car = World.GetClosestVehicle(front6, 4f);
                Vehicle front7Car = World.GetClosestVehicle(front7, 4f);
                Vehicle front8Car = World.GetClosestVehicle(front8, 4f);
                Vehicle front9Car = World.GetClosestVehicle(front9, 4f);
                Vehicle front10Car = World.GetClosestVehicle(front10, 4f);
                Vehicle front11Car = World.GetClosestVehicle(front11, 4f);
                Vehicle front12Car = World.GetClosestVehicle(front12, 4f);
                Vehicle front13Car = World.GetClosestVehicle(front13, 4f);
                Vehicle front14Car = World.GetClosestVehicle(front14, 4f);
                Vehicle front15Car = World.GetClosestVehicle(front15, 4f);
                Vehicle front16Car = World.GetClosestVehicle(front16, 4f);
                Vehicle front17Car = World.GetClosestVehicle(front17, 4f);
                Vehicle front18Car = World.GetClosestVehicle(front18, 4f);
                Vehicle front19Car = World.GetClosestVehicle(front19, 4f);
                Vehicle front20Car = World.GetClosestVehicle(front20, 4f);

                if (front1Car != null)
                    isFrontCar = front1Car;
                if (front2Car != null)
                    isFrontCar = front2Car;
                if (front3Car != null)
                    isFrontCar = front3Car;
                if (front4Car != null)
                    isFrontCar = front4Car;
                if (front5Car != null)
                    isFrontCar = front5Car;
                if (front6Car != null)
                    isFrontCar = front6Car;
                if (front7Car != null)
                    isFrontCar = front7Car;
                if (front8Car != null)
                    isFrontCar = front8Car;
                if (front9Car != null)
                    isFrontCar = front9Car;
                if (front10Car != null)
                    isFrontCar = front10Car;
                if (front11Car != null)
                    isFrontCar = front11Car;
                if (front12Car != null)
                    isFrontCar = front12Car;
                if (front13Car != null)
                    isFrontCar = front13Car;
                if (front14Car != null)
                    isFrontCar = front14Car;
                if (front15Car != null)
                    isFrontCar = front15Car;
                if (front16Car != null)
                    isFrontCar = front16Car;
                if (front17Car != null)
                    isFrontCar = front17Car;
                if (front18Car != null)
                    isFrontCar = front18Car;
                if (front19Car != null)
                    isFrontCar = front19Car;
                if (front20Car != null)
                    isFrontCar = front20Car;
                Vehicle isBehindCar = null;
                if (!isLeftLane)
                {
                    Vector3 behind1 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -6f, 0));
                    Vector3 behind2 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -9f, 0));
                    Vector3 behind3 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -12f, 0));
                    Vector3 behind4 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -15f, 0));
                    Vector3 behind5 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -18f, 0));
                    Vector3 behind6 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -21f, 0));
                    Vector3 behind7 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -24f, 0));
                    Vector3 behind8 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -27f, 0));
                    Vector3 behind9 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -30f, 0));
                    Vector3 behind10 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -33f, 0));
                    Vector3 behind11 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -36f, 0));
                    Vector3 behind12 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -39f, 0));
                    Vector3 behind13 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -42f, 0));
                    Vector3 behind14 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -45f, 0));
                    Vector3 behind15 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -47f, 0));
                    Vehicle behind1Car = World.GetClosestVehicle(behind1, 4f);
                    Vehicle behind2Car = World.GetClosestVehicle(behind2, 4f);
                    Vehicle behind3Car = World.GetClosestVehicle(behind3, 4f);
                    Vehicle behind4Car = World.GetClosestVehicle(behind4, 4f);
                    Vehicle behind5Car = World.GetClosestVehicle(behind5, 4f);
                    Vehicle behind6Car = World.GetClosestVehicle(behind6, 4f);
                    Vehicle behind7Car = World.GetClosestVehicle(behind7, 4f);
                    Vehicle behind8Car = World.GetClosestVehicle(behind8, 4f);
                    Vehicle behind9Car = World.GetClosestVehicle(behind9, 4f);
                    Vehicle behind10Car = World.GetClosestVehicle(behind10, 4f);
                    Vehicle behind11Car = World.GetClosestVehicle(behind11, 4f);
                    Vehicle behind12Car = World.GetClosestVehicle(behind12, 4f);
                    Vehicle behind13Car = World.GetClosestVehicle(behind13, 4f);
                    Vehicle behind14Car = World.GetClosestVehicle(behind14, 4f);
                    Vehicle behind15Car = World.GetClosestVehicle(behind15, 4f);
                    if (behind1Car != null)
                        isBehindCar = behind1Car;
                    if (behind2Car != null)
                        isBehindCar = behind2Car;
                    if (behind3Car != null)
                        isBehindCar = behind3Car;
                    if (behind4Car != null)
                        isBehindCar = behind4Car;
                    if (behind5Car != null)
                        isBehindCar = behind5Car;
                    if (behind6Car != null)
                        isBehindCar = behind6Car;
                    if (behind7Car != null)
                        isBehindCar = behind7Car;
                    if (behind8Car != null)
                        isBehindCar = behind8Car;
                    if (behind9Car != null)
                        isBehindCar = behind9Car;
                    if (behind10Car != null)
                        isBehindCar = behind10Car;
                    if (behind11Car != null)
                        isBehindCar = behind11Car;
                    if (behind12Car != null)
                        isBehindCar = behind12Car;
                    if (behind13Car != null)
                        isBehindCar = behind13Car;
                    if (behind14Car != null)
                        isBehindCar = behind14Car;
                    if (behind15Car != null)
                        isBehindCar = behind15Car;
                }
                if (isLeftLane)
                {
                    if (isFrontCar != null)
                    {
                        if (((isFrontCar.Speed * 3.6) + 4) < (v.Speed * 3.6))
                        {
                            car.Stats.deccelerate = true;
                            car.Stats.adjustSpeedTo = isFrontCar.Speed;
                        }

                    }
                    if (isFrontCar == null && ((car.Vehicle.Speed * 3.6) + 12) < (car.Stats.DefaultSpeed * 3.6))
                    {
                        car.Stats.accelerate = true;
                        car.Stats.adjustSpeedTo = car.Stats.DefaultSpeed;
                    }
                    if (isFrontCar == null && isSideCar == null && ((car.Vehicle.Speed * 3.6)) >= ((car.Stats.DefaultSpeed * 3.6) - 15))
                    {
                        turnRight(car);
                    }

                }
                else
                {
                    if (isFrontCar != null)
                    {
                        if (((isFrontCar.Speed * 3.6) + 4) < (car.Vehicle.Speed * 3.6))
                        {
                            if (isSideCar != null)
                            {
                                car.Stats.deccelerate = true;
                                car.Stats.adjustSpeedTo = isFrontCar.Speed;
                            }
                            else if (isBehindCar != null)
                            {
                                if (isBehindCar.Speed < car.Vehicle.Speed || ((isBehindCar.Speed * 3.6) - 3) < car.Vehicle.Speed * 3.6)
                                {
                                    turnLeft(car);
                                }
                            }
                            else
                            {
                                turnLeft(car);
                            }
                        }

                    }
                    if (isFrontCar != null && ((car.Vehicle.Speed * 3.6) + 12) < (car.Stats.DefaultSpeed * 3.6))
                    {
                        if (isSideCar == null && isBehindCar == null)
                        {
                            turnLeft(car);
                        }
                    }
                }
            }

            /*
             if (car.Stats.DefaultSpeed * 3.6 > 120 && !isCarAroundBehind && car.Vehicle.Position.DistanceTo(Index.veh.Position) < 80)
            {
                var v = car.Vehicle;
                bool isLeftLane = car.Stats.isLeftLane;
                float sidePointOffset = isLeftLane ? 5f : -5f;
                Vector3 side1 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 22f, 0));
                Vector3 side2 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 20f, 0));
                Vector3 side3 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 18f, 0));
                Vector3 side4 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 16f, 0));
                Vector3 side5 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 14f, 0));
                Vector3 side6 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 12f, 0));
                Vector3 side7 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 10f, 0));
                Vector3 side8 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 8f, 0));
                Vector3 side9 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 6f, 0));
                Vector3 side10 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 4f, 0));
                Vector3 side11 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 2f, 0));
                Vector3 side12 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, 0f, 0));
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, side1.X, side1.Y, side1.Z - 5f, side1.X, side1.Y, side1.Z + 20f, 255, 5, 5, 255);
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, side2.X, side2.Y, side2.Z - 5f, side2.X, side2.Y, side2.Z + 20f, 255, 5, 5, 255);
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, side3.X, side3.Y, side3.Z - 5f, side3.X, side3.Y, side3.Z + 20f, 255, 5, 5, 255);
                Vehicle isSideCar = null;
                Vehicle side1Car = World.GetClosestVehicle(side1, 4f);
                Vehicle side2Car = World.GetClosestVehicle(side2, 4f);
                Vehicle side3Car = World.GetClosestVehicle(side3, 4f);
                Vehicle side4Car = World.GetClosestVehicle(side4, 4f);
                Vehicle side5Car = World.GetClosestVehicle(side5, 4f);
                Vehicle side6Car = World.GetClosestVehicle(side6, 4f);
                Vehicle side7Car = World.GetClosestVehicle(side7, 4f);
                Vehicle side8Car = World.GetClosestVehicle(side8, 4f);
                Vehicle side9Car = World.GetClosestVehicle(side9, 4f);
                Vehicle side10Car = World.GetClosestVehicle(side10, 4f);
                Vehicle side11Car = World.GetClosestVehicle(side11, 4f);
                Vehicle side12Car = World.GetClosestVehicle(side12, 4f);
                if (side1Car != null)
                    isSideCar = side1Car;
                if (side2Car != null)
                    isSideCar = side2Car;
                if (side3Car != null)
                    isSideCar = side3Car;
                if (side4Car != null)
                    isSideCar = side4Car;
                if (side5Car != null)
                    isSideCar = side5Car;
                if (side6Car != null)
                    isSideCar = side6Car;
                if (side7Car != null)
                    isSideCar = side7Car;
                if (side8Car != null)
                    isSideCar = side8Car;
                if (side9Car != null)
                    isSideCar = side9Car;
                if (side10Car != null)
                    isSideCar = side10Car;
                if (side11Car != null)
                    isSideCar = side11Car;
                if (side12Car != null)
                    isSideCar = side12Car;
                Vector3 front1 = v.GetOffsetInWorldCoords(new Vector3(0f, 43f, 0));
                Vector3 front2 = v.GetOffsetInWorldCoords(new Vector3(0f, 41f, 0));
                Vector3 front3 = v.GetOffsetInWorldCoords(new Vector3(0f, 39f, 0));
                Vector3 front4 = v.GetOffsetInWorldCoords(new Vector3(0f, 37f, 0));
                Vector3 front5 = v.GetOffsetInWorldCoords(new Vector3(0f, 35f, 0));
                Vector3 front6 = v.GetOffsetInWorldCoords(new Vector3(0f, 33f, 0));
                Vector3 front7 = v.GetOffsetInWorldCoords(new Vector3(0f, 31f, 0));
                Vector3 front8 = v.GetOffsetInWorldCoords(new Vector3(0f, 29f, 0));
                Vector3 front9 = v.GetOffsetInWorldCoords(new Vector3(0f, 27f, 0));
                Vector3 front10 = v.GetOffsetInWorldCoords(new Vector3(0f, 25f, 0));
                Vector3 front11 = v.GetOffsetInWorldCoords(new Vector3(0f, 23f, 0));
                Vector3 front12 = v.GetOffsetInWorldCoords(new Vector3(0f, 21f, 0));
                Vector3 front13 = v.GetOffsetInWorldCoords(new Vector3(0f, 19f, 0));
                Vector3 front14 = v.GetOffsetInWorldCoords(new Vector3(0f, 17f, 0));
                Vector3 front15 = v.GetOffsetInWorldCoords(new Vector3(0f, 15f, 0));
                Vector3 front16 = v.GetOffsetInWorldCoords(new Vector3(0f, 13f, 0));
                Vector3 front17 = v.GetOffsetInWorldCoords(new Vector3(0f, 11f, 0));
                Vector3 front18 = v.GetOffsetInWorldCoords(new Vector3(0f, 9f, 0));
                Vector3 front19 = v.GetOffsetInWorldCoords(new Vector3(0f, 7f, 0));
                Vector3 front20 = v.GetOffsetInWorldCoords(new Vector3(0f, 5f, 0));
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, front1.X, front1.Y, front1.Z - 5f, front1.X, front1.Y, front1.Z + 20f, 255, 5, 5, 255);
                //  Function.Call((GTA.Native.Hash)0x6B7256074AE34680, front2.X, front2.Y, front2.Z - 5f, front2.X, front2.Y, front2.Z + 20f, 255, 5, 5, 255);
                Vehicle isFrontCar = null;
                Vehicle front1Car = World.GetClosestVehicle(front1, 4f);
                Vehicle front2Car = World.GetClosestVehicle(front2, 4f);
                Vehicle front3Car = World.GetClosestVehicle(front3, 4f);
                Vehicle front4Car = World.GetClosestVehicle(front4, 4f);
                Vehicle front5Car = World.GetClosestVehicle(front5, 4f);
                Vehicle front6Car = World.GetClosestVehicle(front6, 4f);
                Vehicle front7Car = World.GetClosestVehicle(front7, 4f);
                Vehicle front8Car = World.GetClosestVehicle(front8, 4f);
                Vehicle front9Car = World.GetClosestVehicle(front9, 4f);
                Vehicle front10Car = World.GetClosestVehicle(front10, 4f);
                Vehicle front11Car = World.GetClosestVehicle(front11, 4f);
                Vehicle front12Car = World.GetClosestVehicle(front12, 4f);
                Vehicle front13Car = World.GetClosestVehicle(front13, 4f);
                Vehicle front14Car = World.GetClosestVehicle(front14, 4f);
                Vehicle front15Car = World.GetClosestVehicle(front15, 4f);
                Vehicle front16Car = World.GetClosestVehicle(front16, 4f);
                Vehicle front17Car = World.GetClosestVehicle(front17, 4f);
                Vehicle front18Car = World.GetClosestVehicle(front18, 4f);
                Vehicle front19Car = World.GetClosestVehicle(front19, 4f);
                Vehicle front20Car = World.GetClosestVehicle(front20, 4f);

                if (front1Car != null)
                    isFrontCar = front1Car;
                if (front2Car != null)
                    isFrontCar = front2Car;
                if (front3Car != null)
                    isFrontCar = front3Car;
                if (front4Car != null)
                    isFrontCar = front4Car;
                if (front5Car != null)
                    isFrontCar = front5Car;
                if (front6Car != null)
                    isFrontCar = front6Car;
                if (front7Car != null)
                    isFrontCar = front7Car;
                if (front8Car != null)
                    isFrontCar = front8Car;
                if (front9Car != null)
                    isFrontCar = front9Car;
                if (front10Car != null)
                    isFrontCar = front10Car;
                if (front11Car != null)
                    isFrontCar = front11Car;
                if (front12Car != null)
                    isFrontCar = front12Car;
                if (front13Car != null)
                    isFrontCar = front13Car;
                if (front14Car != null)
                    isFrontCar = front14Car;
                if (front15Car != null)
                    isFrontCar = front15Car;
                if (front16Car != null)
                    isFrontCar = front16Car;
                if (front17Car != null)
                    isFrontCar = front17Car;
                if (front18Car != null)
                    isFrontCar = front18Car;
                if (front19Car != null)
                    isFrontCar = front19Car;
                if (front20Car != null)
                    isFrontCar = front20Car;
                Vehicle isBehindCar = null;
                if (!isLeftLane)
                {
                    Vector3 behind1 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -6f, 0));
                    Vector3 behind2 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -9f, 0));
                    Vector3 behind3 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -12f, 0));
                    Vector3 behind4 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -15f, 0));
                    Vector3 behind5 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -18f, 0));
                    Vector3 behind6 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -21f, 0));
                    Vector3 behind7 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -24f, 0));
                    Vector3 behind8 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -27f, 0));
                    Vector3 behind9 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -30f, 0));
                    Vector3 behind10 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -33f, 0));
                    Vector3 behind11 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -36f, 0));
                    Vector3 behind12 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -39f, 0));
                    Vector3 behind13 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -42f, 0));
                    Vector3 behind14 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -45f, 0));
                    Vector3 behind15 = v.GetOffsetInWorldCoords(new Vector3(sidePointOffset, -47f, 0));
                    Vehicle behind1Car = World.GetClosestVehicle(behind1, 4f);
                    Vehicle behind2Car = World.GetClosestVehicle(behind2, 4f);
                    Vehicle behind3Car = World.GetClosestVehicle(behind3, 4f);
                    Vehicle behind4Car = World.GetClosestVehicle(behind4, 4f);
                    Vehicle behind5Car = World.GetClosestVehicle(behind5, 4f);
                    Vehicle behind6Car = World.GetClosestVehicle(behind6, 4f);
                    Vehicle behind7Car = World.GetClosestVehicle(behind7, 4f);
                    Vehicle behind8Car = World.GetClosestVehicle(behind8, 4f);
                    Vehicle behind9Car = World.GetClosestVehicle(behind9, 4f);
                    Vehicle behind10Car = World.GetClosestVehicle(behind10, 4f);
                    Vehicle behind11Car = World.GetClosestVehicle(behind11, 4f);
                    Vehicle behind12Car = World.GetClosestVehicle(behind12, 4f);
                    Vehicle behind13Car = World.GetClosestVehicle(behind13, 4f);
                    Vehicle behind14Car = World.GetClosestVehicle(behind14, 4f);
                    Vehicle behind15Car = World.GetClosestVehicle(behind15, 4f);
                    if (behind1Car != null)
                        isBehindCar = behind1Car;
                    if (behind2Car != null)
                        isBehindCar = behind2Car;
                    if (behind3Car != null)
                        isBehindCar = behind3Car;
                    if (behind4Car != null)
                        isBehindCar = behind4Car;
                    if (behind5Car != null)
                        isBehindCar = behind5Car;
                    if (behind6Car != null)
                        isBehindCar = behind6Car;
                    if (behind7Car != null)
                        isBehindCar = behind7Car;
                    if (behind8Car != null)
                        isBehindCar = behind8Car;
                    if (behind9Car != null)
                        isBehindCar = behind9Car;
                    if (behind10Car != null)
                        isBehindCar = behind10Car;
                    if (behind11Car != null)
                        isBehindCar = behind11Car;
                    if (behind12Car != null)
                        isBehindCar = behind12Car;
                    if (behind13Car != null)
                        isBehindCar = behind13Car;
                    if (behind14Car != null)
                        isBehindCar = behind14Car;
                    if (behind15Car != null)
                        isBehindCar = behind15Car;
                }
                if (isLeftLane)
                {
                    if (isFrontCar != null)
                    {
                        if (((isFrontCar.Speed * 3.6) + 4) < (v.Speed * 3.6))
                        {
                            car.Stats.deccelerate = true;
                            car.Stats.adjustSpeedTo = isFrontCar.Speed;
                        }

                    }
                    if (isFrontCar == null && ((car.Vehicle.Speed * 3.6) + 12) < (car.Stats.DefaultSpeed * 3.6))
                    {
                        car.Stats.accelerate = true;
                        car.Stats.adjustSpeedTo = car.Stats.DefaultSpeed;
                    }
                    if (isFrontCar == null && isSideCar == null && ((car.Vehicle.Speed * 3.6)) >= ((car.Stats.DefaultSpeed * 3.6) - 15))
                    {
                        turnRight(car);
                    }

                }
                else
                {
                    if (isFrontCar != null)
                    {
                        if (((isFrontCar.Speed * 3.6) + 4) < (car.Vehicle.Speed * 3.6))
                        {
                            if (isSideCar != null)
                            {
                                car.Stats.deccelerate = true;
                                car.Stats.adjustSpeedTo = isFrontCar.Speed;
                            }
                            else if (isBehindCar != null)
                            {
                                if (isBehindCar.Speed < car.Vehicle.Speed || ((isBehindCar.Speed * 3.6) - 3) < car.Vehicle.Speed * 3.6)
                                {
                                    turnLeft(car);
                                }
                            }
                            else
                            {
                                turnLeft(car);
                            }
                        }

                    }
                    if (isFrontCar != null && ((car.Vehicle.Speed * 3.6) + 12) < (car.Stats.DefaultSpeed * 3.6))
                    {
                        if (isSideCar == null && isBehindCar == null)
                        {
                            turnLeft(car);
                        }
                    }
                }
            }
             */
            /*  if (isFrontCar != null)
                  UI.ShowSubtitle("car is in front left: " + isFrontCar.NumberPlate + " " + isFrontCar.DisplayName);
              else
                  UI.ShowSubtitle("no car in front " + car.Vehicle.NumberPlate);*/
            // var testveh = World.GetClosestVehicle(pos, 5f);
            //  if (testveh != null)
            //  {
            //     UI.ShowSubtitle(testveh.DisplayName);
            // }
        }
        public static void CheckMirrors(GeneralCar currentCar)
        {
            if (Index.veh.Speed * 3.6 >= ((currentCar.Vehicle.Speed * 3.6) + 65) && currentCar.Stats.isLeftLane && Index.veh.Position.DistanceTo(currentCar.Vehicle.Position) < 80 && false)
            {
                var carOnRight = World.GetClosestVehicle(currentCar.Vehicle.GetOffsetInWorldCoords(new Vector3(10, 5, 0)), 7f);
                if (carOnRight != null)
                {
                    if (carOnRight.Speed == currentCar.Vehicle.Speed)
                    {
                        turnRight(currentCar);
                    }
                    else
                    {
                        // nothing, car is on the right
                    }
                }
                else
                {
                    turnRight(currentCar);
                }
            }
        }
        public static void RunRadar(GeneralCar currentCar)
        {
            GeneralCar carInFront = new GeneralCar();
            GeneralCar carOnSide = new GeneralCar();
            float lastDistToCarInFront = 99999;
            float lastDistToCarOnSide = 99999;
            float driverFrontSeight = 50f;
            float driverSideSeight = 40f;
            int plr = 0;
            float onSideCarLengthFactor = 20f;
            Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
            if (currentCar.Stats.isLeftLane)
            {
                Vector3 lastPostion = currentCar.Vehicle.Position;
                float lastSpeed = currentCar.Vehicle.Speed;
                if (Index.veh.Position.DistanceTo(currentCar.Vehicle.Position) <= (isTruckMode ? 55f : 40f))
                {
                    if (((Index.veh.Position.DistanceTo(zoneEnd)) < currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && isPlayerOnLeftLane)
                    {
                        carInFront = new GeneralCar() { Stats = new CarStatsModel() { Id = "super new id", isLeftLane = isPlayerOnLeftLane, isPlayerCar = true }, Vehicle = Index.veh };
                    }
                    if ((Index.veh.Position.DistanceTo(zoneEnd) - (isTruckMode ? 18f : 10f) < currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && !isPlayerOnLeftLane)
                    {
                        carOnSide = new GeneralCar() { Stats = new CarStatsModel() { Id = "super new id", isLeftLane = isPlayerOnLeftLane, isPlayerCar = true }, Vehicle = Index.veh };
                    }
                }
                if (carOnSide.Vehicle == null)
                {
                    currentCar.Vehicle.Position = new Vector3(0, 0, 0);
                    GeneralCar carAround = new GeneralCar() { Vehicle = Function.Call<Vehicle>((GTA.Native.Hash)0xF73EB622C4F1689B, lastPostion.X, lastPostion.Y, lastPostion.Z, 40f, 0, 70), Stats = new CarStatsModel() };
                    currentCar.Vehicle.Position = lastPostion;
                    currentCar.Vehicle.Speed = lastSpeed;
                    if (carAround.Vehicle.DisplayName != "CARNOTFOUND")
                    {
                        float dist = currentCar.Vehicle.Position.DistanceTo(carAround.Vehicle.Position);
                        if ((carAround.Vehicle.Position.DistanceTo(zoneEnd) - 10f < currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && carAround.Vehicle.FuelLevel == 0)
                        {
                            carOnSide = carAround;
                        }
                    }
                }
                if (carOnSide.Vehicle != null)
                {
                    if (carOnSide.Vehicle.DisplayName != "CARNOTFOUND")
                    {
                        Vector3 lastPositon2;
                        lastPositon2 = carOnSide.Vehicle.Position;
                        float lastSpeed2 = carOnSide.Vehicle.Speed;
                        if (!carOnSide.Stats.isPlayerCar)
                        {
                            carOnSide.Vehicle.Position = new Vector3(0, 0, 0);
                        }
                        currentCar.Vehicle.Position = new Vector3(0, 0, 0);
                        Vehicle maybeCarInFront = Function.Call<Vehicle>((GTA.Native.Hash)0xF73EB622C4F1689B, lastPostion.X, lastPostion.Y, lastPostion.Z, 40f, 0, 70);
                        currentCar.Vehicle.Position = lastPostion;
                        currentCar.Vehicle.Speed = lastSpeed;
                        if (!carOnSide.Stats.isPlayerCar)
                        {
                            carOnSide.Vehicle.Position = lastPositon2;
                            carOnSide.Vehicle.Speed = lastSpeed2;
                        }
                        if (maybeCarInFront.DisplayName != "CARNOTFOUND")
                        {
                            if ((maybeCarInFront.Position.DistanceTo(zoneEnd) < currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && maybeCarInFront.FuelLevel == 1)
                            {
                                carInFront = new GeneralCar() { Vehicle = maybeCarInFront, Stats = new CarStatsModel() };
                            }
                        }
                    }
                }
            }
            else
            {
                Vector3 lastPostion = currentCar.Vehicle.Position;
                float lastSpeed = currentCar.Vehicle.Speed;
                if (Index.veh.Position.DistanceTo(currentCar.Vehicle.Position) <= (isTruckMode ? 55f : 40f))
                {
                    if (((Index.veh.Position.DistanceTo(zoneEnd)) < currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && !isPlayerOnLeftLane)
                    {
                        carInFront = new GeneralCar() { Stats = new CarStatsModel() { Id = "super new id", isLeftLane = isPlayerOnLeftLane, isPlayerCar = true }, Vehicle = Index.veh };
                    }
                    if ((Index.veh.Position.DistanceTo(zoneEnd) + (isTruckMode ? 18f : 10f) > currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && isPlayerOnLeftLane)
                    {
                        carOnSide = new GeneralCar() { Stats = new CarStatsModel() { Id = "super new id", isLeftLane = isPlayerOnLeftLane, isPlayerCar = true }, Vehicle = Index.veh };
                    }
                }
                if (carInFront.Vehicle == null)
                {
                    currentCar.Vehicle.Position = new Vector3(0, 0, 0);
                    GeneralCar carAround = new GeneralCar() { Vehicle = Function.Call<Vehicle>((GTA.Native.Hash)0xF73EB622C4F1689B, lastPostion.X, lastPostion.Y, lastPostion.Z, 40f, 0, 70), Stats = new CarStatsModel() };
                    currentCar.Vehicle.Position = lastPostion;
                    currentCar.Vehicle.Speed = lastSpeed;
                    if (carAround.Vehicle.DisplayName != "CARNOTFOUND")
                    {
                        float dist = currentCar.Vehicle.Position.DistanceTo(carAround.Vehicle.Position);
                        if ((carAround.Vehicle.Position.DistanceTo(zoneEnd) < currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && carAround.Vehicle.FuelLevel == 0)
                        {
                            carInFront = carAround;
                        }
                    }
                }
                if (carInFront.Vehicle != null)
                {
                    if (carInFront.Vehicle.DisplayName != "CARNOTFOUND")
                    {
                        Vector3 lastPositon2;
                        lastPositon2 = carInFront.Vehicle.Position;
                        float lastSpeed2 = carInFront.Vehicle.Speed;
                        if (!carInFront.Stats.isPlayerCar)
                        {
                            carInFront.Vehicle.Position = new Vector3(0, 0, 0);
                        }
                        currentCar.Vehicle.Position = new Vector3(0, 0, 0);
                        Vehicle maybeCarOnSide = Function.Call<Vehicle>((GTA.Native.Hash)0xF73EB622C4F1689B, lastPostion.X, lastPostion.Y, lastPostion.Z, 40f, 0, 70);
                        currentCar.Vehicle.Position = lastPostion;
                        currentCar.Vehicle.Speed = lastSpeed;
                        if (!carInFront.Stats.isPlayerCar)
                        {
                            carInFront.Vehicle.Position = lastPositon2;
                            carInFront.Vehicle.Speed = lastSpeed2;
                        }
                        if (maybeCarOnSide.DisplayName != "CARNOTFOUND")
                        {
                            if ((maybeCarOnSide.Position.DistanceTo(zoneEnd) + 10f > currentCar.Vehicle.Position.DistanceTo(zoneEnd)) && maybeCarOnSide.FuelLevel == 1)
                            {
                                carOnSide = new GeneralCar() { Vehicle = maybeCarOnSide, Stats = new CarStatsModel() };
                            }
                        }
                    }
                }
            }
            if (currentCar.Stats.isLeftLane)
            {
                if (carInFront.Stats != null)
                {
                    var carAheadKmh = Math.Round(carInFront.Vehicle.Speed * 3.6, 0);
                    var cKmh = Math.Round(currentCar.Vehicle.Speed * 3.6, 0);
                    if (carAheadKmh < cKmh)
                    {
                        currentCar.Stats.deccelerate = true;
                        currentCar.Stats.adjustSpeedTo = carInFront.Vehicle.Speed;
                    }
                    if ((carAheadKmh - cKmh > 10) && currentCar.Stats.notDefaultSpeed && !currentCar.Stats.accelerate)
                    {
                        currentCar.Stats.accelerate = true;
                    }
                }
                else
                {
                    if (currentCar.Vehicle.Speed < currentCar.Stats.DefaultSpeed)
                    {
                        currentCar.Stats.accelerate = true;
                    }
                    if (!isTruckMode && carOnSide.Stats == null && !currentCar.Stats.isEmergencyBraking && !currentCar.Stats.IsOvertaking)
                    {
                        turnRight(currentCar);
                    }
                }

            }
            if (!currentCar.Stats.isLeftLane)
            {
                if (carInFront.Stats != null)
                {
                    var carAheadKmh = Math.Round(carInFront.Vehicle.Speed * 3.6, 0);
                    var cKmh = Math.Round(currentCar.Vehicle.Speed * 3.6, 0);
                    if ((carAheadKmh - cKmh > 4) && currentCar.Stats.notDefaultSpeed && !currentCar.Stats.accelerate)
                    {
                        currentCar.Stats.accelerate = true;
                    }
                    if (carOnSide.Stats != null)
                    {
                        if (carAheadKmh < cKmh && !carInFront.Stats.IsOvertaking)
                        {
                            currentCar.Stats.deccelerate = true;
                            currentCar.Stats.adjustSpeedTo = carInFront.Vehicle.Speed;
                        }
                    }
                    else if (carAheadKmh < cKmh && !currentCar.Stats.IsOvertaking)
                    {
                        if (currentCar.Stats.Class == "truck" || isTruckMode)
                        {
                            currentCar.Stats.deccelerate = true;
                            currentCar.Stats.adjustSpeedTo = carInFront.Vehicle.Speed;
                        }
                        else
                        {
                            turnLeft(currentCar);
                        }
                    }
                }
            }
            if (carInFront.Stats == null && currentCar.Stats.notDefaultSpeed)
            {
                currentCar.Stats.accelerate = true;
            }
            /*  for (var i = 0; i < (carList.Count +1); ++i)
              {
                  if (i == 0)
                  {
                      plr = i;
                  }  
                  else
                  {
                      GeneralCar fromCarList;
                      if (i >= carList.Count)  
                      {    
                        //  fromCarList = carListOnRightLane[i - (carList.Count - 2)];
                      }    
                      else
                      { 
                          //fromCarList = carList[i - 1];
                      }
                      fromCarList = carList[i - 1];
                      carsAround.Add(new GeneralCar() { Stats = fromCarList.Stats, Vehicle = fromCarList.Vehicle });
                  }
                  var carAround = carsAround[i];
                  if (currentCar.Stats.Id != carAround.Stats.Id)
                  {
                      Vector3 zoneEnd = new Vector3() { X = CurrentZone.ZoneBoundary.FinishRightX, Y = CurrentZone.ZoneBoundary.FinishRightY, Z = CurrentZone.ZoneBoundary.ZCoord };
                      float dist = currentCar.Vehicle.Position.DistanceTo(carAround.Vehicle.Position);
                      // car in front
                      if ((carAround.Vehicle.Position.DistanceTo(zoneEnd) < currentCar.Vehicle.Position.DistanceTo(zoneEnd)))
                      {
                          if (dist < lastDistToCarInFront && currentCar.Stats.isLeftLane == carAround.Stats.isLeftLane)
                          { 
                              lastDistToCarInFront = dist;
                              carInFront = carAround;
                          }
                      }
                      // car on side
                      if (currentCar.Stats.isLeftLane)
                      {
                          if (((carAround.Vehicle.Position.DistanceTo(zoneEnd) - onSideCarLengthFactor) < currentCar.Vehicle.Position.DistanceTo(zoneEnd)))
                          {
                              if (dist < lastDistToCarOnSide)
                              {
                                  lastDistToCarOnSide = dist;
                                  carOnSide = carAround;
                              }
                          }
                      }
                      else
                      {
                          if ((carAround.Vehicle.Position.DistanceTo(zoneEnd) > currentCar.Vehicle.Position.DistanceTo(zoneEnd)))
                          {
                              if (dist < lastDistToCarOnSide)
                              {
                                  lastDistToCarOnSide = dist;
                                  carOnSide = carAround;
                              }
                          }
                      }
                  }
              }
              */
            /* if (!currentCar.Stats.isLeftLane)
             {
                 if (carInFront.Stats != null)
                 {
                     var carAheadKmh = Math.Round(carInFront.Vehicle.Speed * 3.6, 0);
                     var cKmh = Math.Round(currentCar.Vehicle.Speed * 3.6, 0);
                     if ((carAheadKmh - cKmh > 6) && currentCar.Stats.notDefaultSpeed && !currentCar.Stats.accelerate)
                     {
                         currentCar.Stats.accelerate = true;
                     }
                     if (!carOnSide.Stats.isEmpty)
                     {
                         var isCarNextBehind = false;
                         if (isCarNextBehind && currentCar.Stats.IsOvertaking)
                         {

                         }
                         else if (carAheadKmh < cKmh && !carInFront.Stats.IsOvertaking)
                         {
                             currentCar.Stats.deccelerate = true;
                             currentCar.Stats.adjustSpeedTo = carInFront.Vehicle.Speed;
                         }
                     }
                     else if ((carAheadKmh < cKmh || (carAheadKmh < Math.Round(currentCar.Stats.DefaultSpeed * 3.6, 0))) && (((float)Math.Round(currentCar.Vehicle.Speed * 3.6, 0) - carAheadKmh) >= 25) && !currentCar.Stats.IsOvertaking)
                     {
                         if (currentCar.Stats.Class == "truck")
                         {
                             currentCar.Stats.deccelerate = true;
                             currentCar.Stats.adjustSpeedTo = carInFront.Vehicle.Speed;
                         }
                         else
                         {
                             turnLeft(currentCar);
                         }
                     }
                 }
             }
             else
             {
                 if (carInFront.Stats != null)
                 {
                     var carAheadKmh = Math.Round(carInFront.Vehicle.Speed * 3.6, 0);
                     var cKmh = Math.Round(currentCar.Vehicle.Speed * 3.6, 0);
                     if (carAheadKmh < cKmh)
                     {
                         currentCar.Stats.deccelerate = true;
                         currentCar.Stats.adjustSpeedTo = carInFront.Vehicle.Speed;
                     }
                     if ((carAheadKmh - cKmh > 10) && currentCar.Stats.notDefaultSpeed && !currentCar.Stats.accelerate)
                     {
                         currentCar.Stats.accelerate = true;
                     }
                 }
                 else
                 {
                     if (currentCar.Vehicle.Speed < currentCar.Stats.DefaultSpeed)
                     {
                         currentCar.Stats.accelerate = true;
                     }
                     if (carOnSide.Stats == null)
                     {
                         turnRight(currentCar);
                     }
                 }
             }*/
        }

        private static void turnRight(GeneralCar c)
        {
            if (!c.Stats.IsOvertaking && !c.Stats.forcedLeftLane)
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
    }
}
