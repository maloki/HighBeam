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
using static HighBeam.AutobahnPropStreamer;

namespace HighBeam
{
    public static class Train
    {

        public static bool isInTrain = false;
        public static float speed = 22f;
        private static int speedInKmh = 0;
        private static Stopwatch afterExitStopwatch = new Stopwatch();
        private static Vehicle train;
        private static Vector3 mainCam = new GTA.Math.Vector3(0, 13f, 0.5f);
        private static Vector3 playerPos = new Vector3();
        private static float playerHead = 0;
        private static int currentCam = 0;
        private static Stopwatch checkIfAtStationStopwatch = new Stopwatch();
        public static bool isTrainStationsRendered = false;
        private static StationModel currentStation;

        public class StationModel
        {
            public Vector3 Position { get; set; }
            public bool LoadingCloseDistance { get; set; }

        }

        private static List<StationModel> stations = new List<StationModel>()
        {
            new StationModel(){ Position = new Vector3(30.184f, 6275.7f, 32.4f), LoadingCloseDistance = true},
            new StationModel(){ Position = new Vector3(393.0f, -1720.8f, 30.3f)}
        };

        public static void RunTrain()
        {
            Function.Call((Hash)0xD4B8E3D1917BC86B, true);
            if (!isTrainStationsRendered && false)
            {
                Function.Call((Hash)0xD4B8E3D1917BC86B, true);
                if (!checkIfAtStationStopwatch.IsRunning) 
                    checkIfAtStationStopwatch.Start();
                 //UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
                if (checkIfAtStationStopwatch.ElapsedMilliseconds > 2000)
                {
                    for (var i = 0; i < stations.Count; ++i)
                    {
                        isTrainStationsRendered = stations[i].Position.DistanceTo(Game.Player.Character.Position) < (stations[i].LoadingCloseDistance ? 80f : 200f);
                        if (isTrainStationsRendered)
                        {
                             LoadTrainStations();
                            UI.Notify("Loaded Train Stations");
                            currentStation = stations[i];
                            break;
                        }
                    }
                    checkIfAtStationStopwatch = new Stopwatch();
                }
            }
            else if(false)
            {
                Function.Call((Hash)0xD4B8E3D1917BC86B, false);
                TrainMode();
                if ((afterExitStopwatch.IsRunning && afterExitStopwatch.ElapsedMilliseconds > 32000) 
                    || (isTrainStationsRendered && currentStation.Position.DistanceTo(Game.Player.Character.Position) > 250 && currentStation.Position.DistanceTo(Game.Player.Character.Position) < 1000 && !isInTrain))
                {
                    RemoveTrainStations(); 
                    UI.Notify("Removed Train Stations"); 
                    isTrainStationsRendered = false;
                    speed = 22f;
                    currentStation = null;
                    afterExitStopwatch = new Stopwatch();
                }
            }
        }

        public static void TrainMode()
        {
            var player = Game.Player.Character;
            if (train?.DisplayName == null || train?.Position.DistanceTo(player.Position) > 600)
                train = World.GetNearbyVehicles(player.Position, 10000, new Model(VehicleHash.Freight)).FirstOrDefault();
            var isTrainSpawned = train?.DisplayName != null;
            speedInKmh = (int)(speed * 3.6);

            if (!isInTrain && isTrainSpawned)
            {
                UI.ShowSubtitle("Train is coming");
                if (currentStation.Position.DistanceTo(train.Position) < 60)
                {
                    speed -= 0.06f;
                    if (speed <= 0)
                    {
                        speed = 0f;
                    }
                }
            }

            if (isTrainSpawned)
            {
                if (Game.IsControlJustPressed(0, Control.VehicleExit) && (player.Position.DistanceTo(train.Position) < 15 || isInTrain))
                {
                    isInTrain = !isInTrain;
                    if (!isInTrain)
                    {
                        player.IsVisible = true;
                        player.FreezePosition = false;
                        speed = 0f;
                        afterExitStopwatch.Start();
                        player.Position = train.GetOffsetInWorldCoords(new GTA.Math.Vector3(((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.5 ? -1 : 1) * 2.5f), -8f, 0.7f));
                    }
                    else
                    {
                        currentCam = 0;
                    }
                }

                if (afterExitStopwatch.IsRunning)
                {
                    if (speedInKmh < 40)
                    {
                        speed += 0.01f;
                    }
                }
                if (afterExitStopwatch.ElapsedMilliseconds > 35000)
                {
                    afterExitStopwatch = new Stopwatch();
                }

                if (isTrainSpawned)
                {
                    Function.Call((Hash)0xAA0BC91BE0B796E3, train, speed);
                }

                if (isInTrain && isTrainSpawned)
                {
                    if (Game.IsControlJustPressed(0, Control.VehicleSelectNextWeapon))
                    {
                        train.HighBeamsOn = !train.HighBeamsOn;
                    }
                    if (train.HighBeamsOn)
                    {
                        HighBeams();
                    }

                    if (speedInKmh <= 350)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.1)
                        {
                            speed += GamePad.GetState(PlayerIndex.One).Triggers.Right / 21;
                        }
                    }
                    if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.1)
                    {
                        speed -= GamePad.GetState(PlayerIndex.One).Triggers.Left / 9;
                    }

                    if (speed <= 0f)
                    {
                        speed = 0f;
                        train.FreezePosition = true;
                    }
                    else
                    {
                        train.FreezePosition = false;
                    }
                    if (Game.IsControlJustPressed(0, Control.VehicleCinCam))
                    {
                        if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.8f)
                        {
                            currentCam = 1;
                        }
                        else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.8f)
                        {
                            currentCam = 2;
                        }
                        else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.8f)
                        {
                            currentCam = 3;
                        }
                        else
                        {
                            currentCam = 0;
                        }
                    }

                    if (currentCam == 0)
                    {
                        player.Position = train.GetOffsetInWorldCoords(mainCam);
                        player.Heading = train.Heading;
                    }
                    if (currentCam == 1)
                    {
                        player.Position = train.GetOffsetInWorldCoords(new Vector3(-2f, -2f, 0.0f));
                    }
                    if (currentCam == 2)
                    {
                        player.Position = train.GetOffsetInWorldCoords(new Vector3(2f, -2f, 0.0f));
                    }

                    if (currentCam == 3)
                    {
                        player.Position = train.GetOffsetInWorldCoords(new Vector3(0f, -14f, 5f));
                    }

                    player.FreezePosition = true;
                    player.IsVisible = false;

                    UI.ShowSubtitle($"Speed: {speedInKmh} kmh");
                }
            }
        }

        public static void HighBeams()
        {
            var debug = false;
            var lBone = train.GetBoneCoord("headlight_l");
            var rBone = train.GetBoneCoord("headlight_r");

            var lampInitHeight = train.GetOffsetFromWorldCoords(lBone).Z;
            var defaultLampHeight = 0.092f;
            var lampDiff = (defaultLampHeight - lampInitHeight);

            var leftLightBone = new Vector3(lBone.X, lBone.Y, lBone.Z + lampDiff);
            var rightLightBone = new Vector3(rBone.X, rBone.Y, rBone.Z + lampDiff);
            if (debug && false)
            {
                Function.Call((Hash)0x6B7256074AE34680, rightLightBone.X, rightLightBone.Y, rightLightBone.Z, rightLightBone.X, rightLightBone.Y, (rightLightBone.Z + 4f), 255, 5, 5, 255);
                Function.Call((Hash)0x6B7256074AE34680, leftLightBone.X, leftLightBone.Y, leftLightBone.Z, leftLightBone.X, leftLightBone.Y, (leftLightBone.Z + 4f), 255, 5, 5, 255);
            }

            /* old config */
            /*
            var offset = -3f;
            var index = 0;
            var genericSensorOffset = 6f;
            var sensorAmount = 16f; 
            var leftLightLedAmount = 16;
            var rightLightLedAmount = 24; 
             */

            var offset = -2f;
            var index = 0;
            var genericSensorOffset = 6f;
            var leftSensorOffset = 6f;
            var sensorAmount = 32;
            var leftLightLedAmount = 18;
            var rightLightLedAmount = 10;
            var fadeout = 10f;
            var matrixCalculationFrequency = 120;
            var matrixDistance = 200f;
            var matrixColor = System.Drawing.Color.FromArgb(255, 99, 137, 198);
            var matrixBrightness = 0.7f;
            var ledDistBetween = 0.15f;



            for (var i = 0; i < leftLightLedAmount; ++i)
            {

                var frontModelSize = train.Model.GetDimensions();
                frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, 0.854f);
                var matrixPosition = GetEntityOffset(train, new Vector3(offset, (frontModelSize.Y) - 6f, (-frontModelSize.Z * 0)));
                matrixPosition = new Vector3(matrixPosition.X, matrixPosition.Y, matrixPosition.Z);
                if (debug)
                    Function.Call((Hash)0x6B7256074AE34680, matrixPosition.X, matrixPosition.Y, matrixPosition.Z, matrixPosition.X, matrixPosition.Y, (matrixPosition.Z + 4f), 255, 5, 5, 255);

                var matrixDir = matrixPosition - leftLightBone;
                matrixDir.Normalize();


                World.DrawSpotLight(
                                   leftLightBone,
                                   -matrixDir,
                                   matrixColor,
                                   matrixDistance, matrixBrightness, 3f, 4f, fadeout
                              );
                offset += ledDistBetween;
                index++;
            }
        }

        private static Vector3 GetEntityOffset(Entity ent, Vector3 offset)
        {
            return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, ent.Handle, offset.X, offset.Y, offset.Z);
        }
    }
}
