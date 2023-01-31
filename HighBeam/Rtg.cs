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
using System.Windows.Media;
using XInputDotNetPure;
using static HighBeam.NyApartment;

namespace HighBeam
{
    public static class Rtg
    {
        private static Prop mainFrame;
        private static Prop cabin;
        private static Prop hook;
        private static Prop cables;
        private static bool isRtgSpawned;
        public static bool isControlingRtg;
        private static bool firstInit;

        private static Vector3 rtgEntryPos = new Vector3(-1384.8f, -2461.9f, 29.3f);

        private static Stopwatch setPedTaskStopwatch = new Stopwatch();
        private static Stopwatch playAudioStopwatch = new Stopwatch();

        public static void RunRtg()
        {
           // UI.ShowSubtitle(Game.Player.Character.Position.X + " " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading);
            if (Game.IsControlJustPressed(0, Control.ScriptPadRight) && Game.Player.Character.Position.DistanceTo(new Vector3(-12728.4f, -13143.4f, 6.3f)) < 300f)
            {
                if (!isRtgSpawned)
                { 
                    SpawnRtg(new Vector3(-12496.8f, -13818f, 5.3f));
                    SpawnContainerRow(cabin.GetOffsetInWorldCoords(new Vector3(-10f, 9.5f, -18f)));
                    LoadParkingLot();
                    LoadPeds();
                }

                else
                {
                    DeleteRtg();
                    DeleteParkingLot();
                    DeletePeds();
                    spawnedContainerList.ForEach(c => c.Delete());
                }

                isRtgSpawned = !isRtgSpawned;
            }

            if (isRtgSpawned)
            {
                if (!setPedTaskStopwatch.IsRunning)
                {
                    setPedTaskStopwatch.Start();
                }
                if (setPedTaskStopwatch.ElapsedMilliseconds > 30000)
                {
                    for (var i = 0; i < walkingWorkingPeds.Count; i++)
                    {
                        walkingWorkingPeds[i].Task.GoTo(CalculatePoint(workerSpawnPos), true);
                    }
                    setPedTaskStopwatch = new Stopwatch();
                }

                if (!playAudioStopwatch.IsRunning)
                    playAudioStopwatch.Start();

                if (playAudioStopwatch.ElapsedMilliseconds > 1000)
                {
                    PlayPedAudio();
                    playAudioStopwatch = new Stopwatch();
                }
            }

            //     UI.ShowSubtitle(mainFrame.Position.X + " " + mainFrame.Position.Y + " " + mainFrame.Position.Z);

            if (Game.IsControlJustPressed(0, Control.VehicleExit) && isRtgSpawned && (rtgEntryPos.DistanceTo(Game.Player.Character.Position) < 5f || isControlingRtg))
            {
                if (!isControlingRtg)
                {
                    Game.Player.Character.Position = cabin.GetOffsetInWorldCoords(new Vector3(0f, 3.5f, -6f));

                    Game.Player.Character.Rotation = new Vector3(-50f, 0f, 0f);
                    Game.Player.Character.Heading = cabin.Heading;
                    Game.Player.Character.FreezePosition = true;

                }

                else
                {
                    rtgEntryPos = cabin.GetOffsetInWorldCoords(new Vector3(-1.4f, -2.5f, -1.8f));
                    Game.Player.Character.Position = rtgEntryPos;

                    Game.Player.Character.Rotation = new Vector3(0f, 0f, 0f);
                    Game.Player.Character.Heading = cabin.Heading;
                    Game.Player.Character.FreezePosition = false;
                    Game.Player.Character.IsVisible = true;
                }


                isControlingRtg = !isControlingRtg;
            }

            if (isControlingRtg)
            {
                Game.Player.Character.IsVisible = false;
                Game.Player.Character.FreezePosition = true;
                Game.DisableControlThisFrame(0, Control.SelectWeapon);
                RtgControl();
                Audio();
                ManageTrucks();
                WalkieTalkie();
            }
            else
            {
                Game.Player.Character.IsVisible = true;
            }
        }


        private static List<Ped> peds = new List<Ped>();
        private static List<Ped> walkingWorkingPeds = new List<Ped>();
        private class StaticPedModel
        {
            public Vector3 Pos { get; set; }
            public float Heading { get; set; }
            public string Action { get; set; }
        }

        private static List<StaticPedModel> securityPeds = new List<StaticPedModel>() {
            new StaticPedModel() { Pos = new Vector3(-12607.8f, -13549f, 6.5f), Heading = 71, Action = "guard" },
            new StaticPedModel() { Pos = new Vector3(-12574.6f, -13530.3f, 6.5f), Heading = 230, Action = "guard" },
            new StaticPedModel() { Pos = new Vector3(-12324.7f, -14069.2f, 6.5f), Heading = 230, Action = "guard" },
        };

        private static void LoadPeds()
        {
            LoadSecurity();
            LoadWorkers();
        }

        private static string ConvertAction(string action)
        {
            if (action == "clipboard")
                return "WORLD_HUMAN_TOURIST_MAP";
            if (action == "guard")
                return "WORLD_HUMAN_GUARD_PATROL";
            if (action == "smoke")
                return "WORLD_HUMAN_SMOKING";
            if (action == "smoke2")
                return "WORLD_HUMAN_DRUG_DEALER_HARD";
            if (action == "drink")
                return "WORLD_HUMAN_DRINKING";
            return "WORLD_HUMAN_CLIPBOARD";
        }

        private static List<PedHash> workersPedModels = new List<PedHash>() { PedHash.Dockwork01SMM, PedHash.Dockwork01SMY };
        private static Random workerRnd = new Random();
        private static Random workerHeadingRnd = new Random();
        private static Vector3 workerSpawnPos = new Vector3(-12380.4f, -13993.9f, 6.5f);


        private static List<StaticPedModel> workerStaticPeds = new List<StaticPedModel>() {
            new StaticPedModel() { Pos = new Vector3(-12468.7f, -13946.6f, 6.5f), Heading = 341, Action = "clipboard" },

            new StaticPedModel() { Pos = new Vector3(-12367.4f, -13944.8f, 6.5f), Heading = 117, Action = "smoke" },
            new StaticPedModel() { Pos = new Vector3(-12367.6f, -13946.3f, 6.5f), Heading = 22, Action = "drink" },
            new StaticPedModel() { Pos = new Vector3(-12368.5f, -13943.7f, 6.5f), Heading = 195, Action = "smoke2" },
        };

        private static List<string> audioIds = new List<string>()
        {
            "CHAT_STATE",
            "CHAT_RESP",
            "GENERIC_THANKS",
            "GENERIC_HI",
            "GENERIC_HOWS_IT_GOING",
        };
        private static Random rndPedAudio = new Random();
        private static Random rndPedAudioId = new Random();

        private static void PlayPedAudio()
        {
            if (walkingWorkingPeds.Count > 0)
            {
                var ped = walkingWorkingPeds[rndPedAudio.Next(0, walkingWorkingPeds.Count - 1)];
                Function.Call(Hash._PLAY_AMBIENT_SPEECH1, ped, audioIds[rndPedAudioId.Next(0, audioIds.Count - 1)], "SPEECH_PARAMS_STANDARD");
            }
        }

        private static void LoadWorkers()
        {

            workerStaticPeds.ForEach(model =>
            {
                var pedModel = workersPedModels[workerRnd.Next(0, workersPedModels.Count)];
                var ped = World.CreatePed(pedModel, model.Pos, model.Heading);
                Function.Call(Hash.TASK_START_SCENARIO_AT_POSITION, ped, ConvertAction(model.Action), ped.Position.X, ped.Position.Y, ped.Position.Z, ped.Heading, 99999, false, false);
                Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, pedModel == PedHash.Dockwork01SMY ? 1 : 0, 0, true);
                peds.Add(ped);
            });

            for (var i = 0; i < 30; i++)
            {
                var pedModel = workersPedModels[workerRnd.Next(0, workersPedModels.Count)];
                var ped = World.CreatePed(pedModel, CalculatePoint(workerSpawnPos), workerHeadingRnd.Next(0, 360));
                Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, pedModel == PedHash.Dockwork01SMY ? 1 : 0, 0, true);
                ped.Task.GoTo(CalculatePoint(workerSpawnPos), true);
                ped.AlwaysKeepTask = true;
                ped.LodDistance = 400;
                walkingWorkingPeds.Add(ped);
            }
        }

        private static Random randomPointRnd = new Random();

        private static Vector3 CalculatePoint(Vector3 pos)
        {
            var angle = randomPointRnd.NextDouble() * Math.PI * 2;
            var radius = Math.Sqrt(randomPointRnd.NextDouble()) * 180;
            var x = pos.X + radius * Math.Cos(angle);
            var y = pos.Y + radius * Math.Sin(angle);
            return new Vector3((int)x, (int)y, pos.Z);
        }

        private static List<PedHash> securityPedModels = new List<PedHash>() { PedHash.Security01SMM };
        private static void LoadSecurity()
        {
            var pedModel = securityPedModels[0];
            securityPeds.ForEach(model =>
            {
                var ped = World.CreatePed(pedModel, model.Pos, model.Heading);
                Function.Call(Hash.TASK_START_SCENARIO_AT_POSITION, ped, ConvertAction(model.Action), ped.Position.X, ped.Position.Y, ped.Position.Z, ped.Heading, 99999, false, false);
                peds.Add(ped);
            });
        }

        private static void DeletePeds()
        {
            peds.ForEach(p => p.Delete());
            peds = new List<Ped>();

            walkingWorkingPeds.ForEach(p => p.Delete());
            walkingWorkingPeds = new List<Ped>();
        }

        private static List<Vector3> parkingLotSpawnPoints = new List<Vector3>()
        {
            new Vector3(-12315.88f, -14082f, 6.3f),
            new Vector3(-12302.6f, -14123.6f, 6.3f),
            new Vector3(-12336.9f, -14134.3f, 6.3f),
            new Vector3(-12350.4f, -14093.1f, 6.3f),
        };

        private static Random headingRnd = new Random();
        private static Random carsToRemoveRnd = new Random();
        private static Random toDelIndexRnd = new Random();
        private static List<Vehicle> parkingLotVehicles = new List<Vehicle>();
        public static void LoadParkingLot()
        {
            for (var i = 0; i < parkingLotSpawnPoints.Count; i++)
            {

                var pos = parkingLotSpawnPoints[i];
                for (var ii = 0; ii < 20; ii++)
                {
                    var num = vrnd.Next(0, vehicleHashes.Count - 1);
                    var mod = vehicleHashes[num];
                    var heading = 287f;
                    var vehicl = World.CreateVehicle(mod, pos, heading);
                    pos = vehicl.GetOffsetInWorldCoords(ii > 8 ? new Vector3(ii == 9 ? 0 : -3f, ii == 9 ? 7f : 0, 0) : new Vector3(3f, 0, 0));
                    vehicl.Heading = headingRnd.Next(0, 10) > 5 || true ? heading : (heading + 180) % 360;
                    vehicl.LodDistance = 100;
                    parkingLotVehicles.Add(vehicl);
                }


            }
            var carsToRemoveCount = 0;
            var h = Main.fakeTimeHours;
            if (h > 20 || h < 7)
                carsToRemoveCount = carsToRemoveRnd.Next(20, 28);
            if (h > 6 && h < 21)
                carsToRemoveCount = carsToRemoveRnd.Next(10, 16);

            for (var i = 0; i < carsToRemoveCount; i++)
            {
                try
                {
                    var indx = toDelIndexRnd.Next(0, parkingLotVehicles.Count);
                    if (parkingLotVehicles?[indx]?.Position.DistanceTo(Game.Player.Character.Position) < 1000)
                    {
                        parkingLotVehicles[indx]?.Delete();
                        parkingLotVehicles?.RemoveAt(indx);
                    }
                }
                catch
                {
                }
            }
        }

        private static void DeleteParkingLot()
        {
            foreach (var car in parkingLotVehicles)
            {
                car.Delete();
            }
            parkingLotVehicles = new List<Vehicle>();
        }

        private static Random containerRowRnd = new Random();
        private static Random containerSpawnRnd = new Random();
        private static List<Prop> spawnedContainerList = new List<Prop>();

        private static void SpawnContainerRow(Vector3 startPoint)
        {
            var head = cabin.Heading + 90f;
            var maxZ = 4;
            var maxX = 3;
            var maxY = 10;

            Prop contOffset = null;

            for (var y = 0; y < maxY; y++)
            {
                for (var x = 0; x < maxX; x++)
                {
                    for (var z = 0; z < maxZ; z++)
                    {
                        var pos = y == 0 && x == 0 && z == 0 ? startPoint : contOffset.GetOffsetInWorldCoords(new Vector3((x) * 3f, (y) * 15f, (z) * 2.81f));
                        Prop tempCont = null;
                        if (contOffset == null)
                        {
                            contOffset = World.CreateProp(new Model(containerList[containerSpawnRnd.Next(0, containerList.Count - 1)]), pos, false, false);
                            contOffset.Heading = head;
                            tempCont = contOffset;
                        }
                        else
                        {
                            if (z > 0)
                            {
                                if (containerRowRnd.Next(0, 10) > 4)
                                {
                                    break;
                                }
                                else
                                {
                                    tempCont = World.CreateProp(new Model(containerList[containerSpawnRnd.Next(0, containerList.Count - 1)]), pos, false, false);
                                }
                            }
                            else
                            {
                                tempCont = World.CreateProp(new Model(containerList[containerSpawnRnd.Next(0, containerList.Count - 1)]), pos, false, false);
                            }
                        }
                        if (tempCont != null)
                        {
                            tempCont.Heading = head;
                            spawnedContainerList.Add(tempCont);
                        }

                    }
                }
            }

        }

        public static int radioMenuCurrentHover = 0;
        public static UIContainer radioDisplay;
        public static ButtonState lastRadioMenuButtonState = ButtonState.Released;
        public static Stopwatch radioMenuScrollDelay = new Stopwatch();
        public static bool showRadio = false;
        public static string selectedMenuItem = "";
        public static List<string> groundCrewMenu = new List<string>() { "DRV IN", "DRV IN (EMPTY)", "DRV OUT", "Random chat" };
        public static List<string> productsMenu = new List<string>() { };
        public static List<string> radioMenu = groundCrewMenu;
        public static List<Ped> groundCrew;
        public static bool init;
        public static Stopwatch monitorGroundCrewTaskStopwatch = new Stopwatch();
        private static Random rnd = new Random();
        public static Stopwatch radioChatterResponseMessageDelayStopwatch = new Stopwatch();
        public static int radioChatterResponseMessageDelay = 0;
        public static string radioChatterResponseMessageType = "";
        public static string radioChatterResponseMessageCallbackAction = "";
        public static Stopwatch radioToggleStopwatch = new Stopwatch();
        public static string radioChatterResponseMessageAction = "";
        public static string currentRadioMenu = "";
        public static List<Prop> spawnedPropsList = new List<Prop>();
        public static bool isGroundCrewFollowingArmHook = false;
        public static MediaPlayer chatterVoiceSound = new MediaPlayer();

        private static void WalkieTalkie()
        {
            Game.DisableControlThisFrame(0, Control.Phone);
            if (showRadio)
            {
                WalkieTalkieUI();
                WalkieTalkieControls();
            }
            if (radioChatterResponseMessageDelayStopwatch.ElapsedMilliseconds > radioChatterResponseMessageDelay)
            {
                radioChatterResponseMessageDelayStopwatch = new Stopwatch();
                WalkieTalkiePlaySound(radioChatterResponseMessageAction, radioChatterResponseMessageType);
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && !showRadio)
            {
                lastRadioMenuButtonState = ButtonState.Pressed;
                showRadio = true;
                radioToggleStopwatch = new Stopwatch();
                radioToggleStopwatch.Start();
            }
        }


        private static void SetRadioChatterDelayedMessage(string type, int minTime, int maxTime, string callbackAction = "", string action = "")
        {
            radioChatterResponseMessageDelayStopwatch.Start();
            radioChatterResponseMessageType = type;
            radioChatterResponseMessageDelay = rnd.Next(minTime, maxTime);
            radioChatterResponseMessageCallbackAction = callbackAction;
            radioChatterResponseMessageAction = action;
        }

        private static void WalkieTalkieUI()
        {
            var radioBackground = new UIContainer(new Point(UI.WIDTH - 200, UI.HEIGHT - 210), new Size(140, 210), System.Drawing.Color.FromArgb(255, 5, 5, 5));
            // antena
            radioBackground.Items.Add(new UIContainer(new Point(20, -80), new Size(22, 80), System.Drawing.Color.FromArgb(255, 5, 5, 5)));
            // display background  
            radioDisplay = new UIContainer(new Point(UI.WIDTH - 190, UI.HEIGHT - 160), new Size(120, 95), System.Drawing.Color.FromArgb(255, 202, 116, 35));
            // motorola logo
            int fakeIndex = 0;
            for (var i = 0; i < radioMenu.Count; ++i)
            {
                if ((radioMenuCurrentHover - i >= 0 && radioMenuCurrentHover - i <= 3) || (i - radioMenuCurrentHover >= 0 && i - radioMenuCurrentHover <= 3))
                {
                    WalkieTalkieMenu(radioMenu[i], fakeIndex, i);
                    fakeIndex++;
                }
            }
            radioBackground.Items.Add(new UIText("MOTOROLA", new Point(72, 14), 0.46f, System.Drawing.Color.White, GTA.Font.Monospace, true));
            radioBackground.Enabled = true;
            radioBackground.Draw();
            radioDisplay.Draw();
        }

        private static void WalkieTalkieMenu(string text, int index, int realIndex)
        {
            if (text == "Follow arm hook" && isGroundCrewFollowingArmHook)
                text = "* Cancel follow";
            var y = (index * 20) + 8;
            var item = new UIContainer(new Point(5, y), new Size(110, 17), System.Drawing.Color.FromArgb(realIndex == radioMenuCurrentHover ? 150 : 50, 0, 0, 0));
            item.Items.Add(new UIText(text, new Point(55, 1), 0.25f, System.Drawing.Color.Black, GTA.Font.ChaletLondon, true));
            radioDisplay.Items.Add(item);
        }


        private static void WalkieTalkieControls()
        {
            Game.DisableControlThisFrame(0, Control.VehicleCinCam);
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                if (currentRadioMenu == "products")
                {
                    ChangeRadioMenu(groundCrewMenu, "");
                }
                else
                {
                    showRadio = false;
                    radioMenuCurrentHover = 0;
                    radioToggleStopwatch = new Stopwatch();
                    radioToggleStopwatch.Start();
                }
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && lastRadioMenuButtonState == ButtonState.Released)
            {
                lastRadioMenuButtonState = ButtonState.Pressed;
                radioMenuCurrentHover -= 1;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && lastRadioMenuButtonState == ButtonState.Released)
            {
                lastRadioMenuButtonState = ButtonState.Pressed;
                radioMenuCurrentHover += 1;
            }
            if (radioMenuCurrentHover > (radioMenu.Count - 1))
                radioMenuCurrentHover = 0;
            if (radioMenuCurrentHover < 0)
                radioMenuCurrentHover = radioMenu.Count - 1;
            if (lastRadioMenuButtonState == ButtonState.Pressed && !radioMenuScrollDelay.IsRunning)
                radioMenuScrollDelay.Start();
            if (radioMenuScrollDelay.ElapsedMilliseconds > 200)
            {
                lastRadioMenuButtonState = ButtonState.Released;
                radioMenuScrollDelay = new Stopwatch();
            }
            if (Game.IsControlJustPressed(0, Control.Sprint))
            {
                var selected = radioMenu[radioMenuCurrentHover];
                selectedMenuItem = selected;
                if (selected == "DRV IN")
                {
                    if(currentTruck == null)
                    {
                        WalkieTalkiePlaySound(selected, "self");
                        LoadTruck();
                        SetRadioChatterDelayedMessage("driver", 5000, 8000, action: selected);
                    }
                }
                if (selected == "DRV OUT")
                {
                    WalkieTalkiePlaySound(selected, "self");
                    var offset = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(300f, 0, 0));
                    Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, currentDriver, currentTruck, offset.X, offset.Y, currentTruck.Position.Z, 5f, 1f, currentTruck.GetHashCode(), 16777216, 1f, true);
                    SetRadioChatterDelayedMessage("driver", 5000, 8000, action: selected);
                    currentTruck = null; 
                }
                if (selected == "DRV IN (EMPTY)")
                {
                    WalkieTalkiePlaySound(selected, "self");
                    LoadTruck(true);
                    SetRadioChatterDelayedMessage("driver", 5000, 8000, action: selected);
                }
            }
        }

        private static void ChangeRadioMenu(List<string> menu, string name)
        {
            radioMenu = menu;
            currentRadioMenu = name;
        }

        private static void WalkieTalkiePlaySound(string action, string chatterType)
        {
            string soundName = "";
            double volume = 0.6;
            string subtitle = "";
            if (chatterType == "self")
            {
                subtitle = "Self: ";
                if (action == "DRV IN")
                {
                    soundName = $"chatter_self_roger_{rnd.Next(1, 3)}";
                    subtitle += "You can come in";
                }
                if (action == "DRV OUT")
                {
                    soundName = $"chatter_self_roger_{rnd.Next(1, 3)}";
                    subtitle += "Ok, ready to go";
                }
            }
            else if (chatterType == "crew")
            {
                subtitle += "Ground crew: ";
                if (action == "Assist confirmation")
                    subtitle += "Ok, Im on my way.";
                if (action == "Hook on")
                    subtitle += "Hook is mounted, and ready to go";
                if (action == "Hook off")
                    subtitle += "Hook is unmounted";
                if (action == "Finish")
                    subtitle += "Ok, understood.";
                volume = 0.7;
                soundName = $"chatter_crew_random_{rnd.Next(1, 12)}";
            }
            else if (chatterType == "driver")
            {
                subtitle += "Driver: ";
                if (action == "DRV IN")
                    subtitle += "Ok, coming in";
                if (action == "DRV OUT")
                    subtitle += "Roger 10-4";
                volume = 0.7;
                soundName = $"chatter_crew_random_{rnd.Next(1, 12)}";
            }
            UI.ShowSubtitle(subtitle, 5000);
            chatterVoiceSound = new MediaPlayer();
            chatterVoiceSound.Open(new Uri(new System.Uri(Path.GetFullPath($"./scripts/chatter_sounds/{soundName}.wav")).AbsoluteUri));
            chatterVoiceSound.Volume = volume;
            chatterVoiceSound.Play();
        }


        private static void ManageTrucks()
        {

        }

        private static List<int> containerList = new List<int>()
        {
            -629735826, 466911544, 772023703, 2140719283, -1857328104, 1525186387, -380625884, 511018606
        };

        private static List<VehicleHash> truckHashes = new List<VehicleHash>() { VehicleHash.Docktug, VehicleHash.Docktug, VehicleHash.Packer, VehicleHash.Phantom, VehicleHash.Hauler, VehicleHash.Docktug };
        private static List<PedHash> pedHashesTruck = new List<PedHash>() { PedHash.Trucker01SMM, PedHash.Dockwork01SMM, PedHash.AirworkerSMY, PedHash.GentransportSMM, PedHash.Salton02AMM };
        private static Random truckSelectRnd = new Random();
        private static Random truckDriverSelectRnd = new Random();
        private static Random truckContainerSelect = new Random();

        private static Vehicle currentTruck = null;
        private static Vehicle currentTrailer;
        private static Ped currentDriver;
        private static Prop currentTrailerContainer;

        private static void LoadTruck(bool isEmpty = false)
        {
            var heading = 18.3f;
            var truckSpawnPoint = mainFrame.GetOffsetInWorldCoords(new Vector3(-50f, -5.2f, 0));
            currentTruck = World.CreateVehicle(new Model(truckHashes[truckSelectRnd.Next(0, truckHashes.Count - 1)]), truckSpawnPoint, heading);
            currentTrailer = World.CreateVehicle(new Model(-877478386), truckSpawnPoint, heading);
            for (var i = 1; i < 6; i++)
            {
                currentTrailer.ToggleExtra(i, false);
            }
            currentTruck.IsInvincible = true;
            currentTrailer.IsInvincible = true;

            currentTruck.TowVehicle(currentTrailer, true);
            currentTruck.PlaceOnGround();
            currentTrailer.PlaceOnGround();
            currentDriver = currentTruck.CreatePedOnSeat(VehicleSeat.Driver, new Model(pedHashesTruck[truckDriverSelectRnd.Next(0, pedHashesTruck.Count - 1)]));

            Function.Call((Hash)0x3C7D42D58F770B54, currentTruck, currentTrailer, 5f);
            currentTruck.Heading = heading;
            currentTrailer.Heading = heading;

            if (!isEmpty)
            {
                var containerPos = new Vector3(0, -1.15f, -1.41f);
                currentTrailerContainer = World.CreateProp(new Model(containerList[truckContainerSelect.Next(0, containerList.Count - 1)]), new Vector3(0, 0, 0), false, false);
                currentTrailerContainer.AttachTo(currentTrailer, 0, containerPos, new Vector3(0, 0, 0));
            }

            var off = mainFrame.GetOffsetInWorldCoords(new Vector3(13f, -5.2f, 0));
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, currentDriver, currentTruck, off.X, off.Y, currentTruck.Position.Z, 5f, 1f, currentTruck.GetHashCode(), 16777216, 1f, true);
        }


        public static float hookY = 1f;
        public static float hookZ = -3f;


        public static float mainFrameX = 0;

        private static float minHookZ = -16.2f;
        private static float maxHookZ = -2f;

        private static float minHookY = 1.5f;
        private static float maxHookY = 16f;
        private static float hookRot = 0f;

        private static bool isArmHookAudio;

        private static void RtgControl()
        {

            //    UI.ShowSubtitle("hookY: " + hookY + " hookZ: " + hookZ);
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.1 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.1)
            {
                hookY += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y / 30;

                if (hookY > maxHookY)
                    hookY = maxHookY;
                if (hookY < minHookY)
                    hookY = minHookY;
                isMovingRtgAudio = true;
            }
            else
            {
                isMovingRtgAudio = false;
            }

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.1 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.1)
            {
                mainFrameX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X / 30;
                mainFrame.Position = mainFrame.GetOffsetInWorldCoords(new Vector3(mainFrameX, 0f, 0f));
                Game.Player.Character.Position = cabin.GetOffsetInWorldCoords(new Vector3(mainFrameX, 3.5f, -6f));
                Game.Player.Character.Rotation = new Vector3(-50f, 0f, 0f);
                Game.Player.Character.Heading = cabin.Heading;
                Function.Call((Hash)0x8F993D26E0CA5E8E, 0f, 0f);
                isZRtgAudio = true;
            }
            else
            {
                isZRtgAudio = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.1 || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.1)
            {
                hookZ -= GamePad.GetState(PlayerIndex.One).Triggers.Right / 30;
                hookZ += GamePad.GetState(PlayerIndex.One).Triggers.Left / 30;

                if (hookZ > maxHookZ)
                    hookZ = maxHookZ;
                if (hookZ < minHookZ)
                    hookZ = minHookZ;
            }

            float rotForce = 0.1f;
            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
            {
                hookRot += rotForce;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
            {
                hookRot -= rotForce;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed && GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
                hookRot = 0f;

            hook.AttachTo(
                cabin,
                0,
                new Vector3(0, hookY, hookZ),
                new Vector3(0, 0, hookRot)
                );

            Handler();
        }

        private static bool isContainerAttached;
        private static Prop currentContainer;

        private static void Handler()
        {
            try
            {
                Game.DisableControlThisFrame(0, Control.Sprint);
                if (Game.IsControlJustPressed(0, Control.Sprint) && !showRadio)
                {
                    if (!isContainerAttached)
                    {
                        var container = World.GetNearbyProps(hook.GetOffsetInWorldCoords(new Vector3(0, 0, -3f)), 2f).Where(c => containerList.Any(cl => cl == c.Model.Hash)).FirstOrDefault();
                        if (container != null)
                        {
                            currentContainer = container;
                            isContainerAttached = true;
                        }
                    }
                    else
                    {
                        currentContainer.Detach();
                        var closestVeh = World.GetClosestVehicle(currentContainer.Position, 4f);
                        if (closestVeh?.Model.Hash == -877478386)
                        {
                            var containerPos = new Vector3(0, -1.15f, -1.41f);
                            currentContainer.AttachTo(closestVeh, 0, containerPos, new Vector3(0, 0, 0));
                        }
                        currentContainer.FreezePosition = true;
                        currentContainer = null;
                        isContainerAttached = false;
                    }
                }
                if (isContainerAttached)
                    currentContainer.AttachTo(hook, 0, new Vector3(0, 0, -4.1f), new Vector3(0, 0, 90f));
            }
            catch (Exception e)
            {
                UI.ShowSubtitle(e.Message);
            }
        }

        private static bool isMovingRtgAudio;
        private static bool isMovingRtgAudioSet;

        private static bool isZRtgAudio;
        private static bool isZRtgAudioSet;

        public static void Audio()
        {
            var offsets = new List<Vector3>()
            {
                new Vector3(-6f, 1.25f, 0),
                new Vector3(6f, 1.25f, 0),
                new Vector3(-6f, -1.25f, 0),
                new Vector3(6f, -1.25f, 0),
            };
            for (var i = 0; i < 4; i++)
            {
                var off = hook.GetOffsetInWorldCoords(offsets[i]);
                Function.Call((Hash)0x6B7256074AE34680, off.X, off.Y, off.Z - 1.3f, off.X, off.Y, 0, 255, 5, 5, 255);
            }

            if (isMovingRtgAudio && !isMovingRtgAudioSet)
            {
                isMovingRtgAudioSet = true;
                Function.Call((Hash)0xE65F427EB70AB1ED, 81, "dryer", cabin, "CARWASH_SOUNDS", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 82, "dryer", cabin, "CARWASH_SOUNDS", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 83, "dryer", cabin, "CARWASH_SOUNDS", 0, 0);
            }
            if (!isMovingRtgAudio && isMovingRtgAudioSet)
            {
                isMovingRtgAudioSet = false;
                Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", cabin, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 82, "Prop_Drop_Water", cabin, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 83, "Prop_Drop_Water", cabin, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
            }

            if (isZRtgAudio && !isZRtgAudioSet)
            {
                isZRtgAudioSet = true;
                Function.Call((Hash)0xE65F427EB70AB1ED, 81, "dryer", cabin, "CARWASH_SOUNDS", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 82, "dryer", cabin, "CARWASH_SOUNDS", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 83, "dryer", cabin, "CARWASH_SOUNDS", 0, 0);
            }
            if (!isZRtgAudio && isZRtgAudioSet)
            {
                isZRtgAudioSet = false;
                Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", cabin, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 82, "Prop_Drop_Water", cabin, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 83, "Prop_Drop_Water", cabin, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
            }
        }

        public static void DeleteRtg()
        {
            mainFrame.Delete();
            cabin.Delete();
            hook.Delete();
            cables.Delete();
        }

        public static void SpawnRtg(Vector3 pos)
        {
            mainFrame = World.CreateProp(new Model(1120043236), pos, true, true);
            mainFrame.Heading = 108.3f;

            cabin = World.CreateProp(new Model(913564566), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), false, true);
            cabin.AttachTo(mainFrame, 0, new Vector3(0, -10f, 18f), new Vector3(0, 0, 0));
            cabin.HasCollision = true;

            hook = World.CreateProp(new Model(1496091510), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
            hook.AttachTo(cabin, 0, new Vector3(0, 5f, -3f), new Vector3(0, 0, 0));

            cables = World.CreateProp(new Model(-1290409783), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
            cables.AttachTo(hook, 0, new Vector3(0, 0f, 6.4f), new Vector3(0, 0, 90f));

            rtgEntryPos = cabin.GetOffsetInWorldCoords(new Vector3(-1.4f, -2.5f, -1.8f));

            /*    baseCrane = World.CreateProp(new Model(1163207500), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
                baseCrane.AttachTo(topLadder, 0, new Vector3(0, 0, 0), new Vector3());
                baseCrane.IsCollisionProof = true;
                baseCrane.HasCollision = true;
                enterCraneOffset = baseCrane.GetOffsetInWorldCoords(new Vector3(1f, -2, 0f));
                armHook = World.CreateProp(new Model(-2128680992), baseCrane.Position, false, false);
                armHook.AttachTo(baseCrane, 0, new Vector3(armX, armY, armZ), new Vector3(0, 0, armRot));
                armHookFake = World.CreateProp(new Model(-2128680992), baseCrane.Position, false, false);
                armHookFake.AttachTo(baseCrane, 0, new Vector3(-3f, -6, -2f), new Vector3(0, 0, 0));
                armHookFake.Alpha = 0;
                bottomStairs.LodDistance = lod;
                topLadder.LodDistance = lod;
                baseCrane.LodDistance = lod;
                armHook.LodDistance = lod;
                airLight1 = World.CreateProp(new Model(-772034186), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
                airLight1.AttachTo(baseCrane, 0, new Vector3(-1f, 21f, 5.6f), new Vector3());
                airLight2 = World.CreateProp(new Model(-772034186), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
                airLight2.AttachTo(baseCrane, 0, new Vector3(0, 0, 14.2f), new Vector3());
                airLight3 = World.CreateProp(new Model(-772034186), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
                airLight3.AttachTo(baseCrane, 0, new Vector3(0, -58f, 7.3f), new Vector3());
                airLight4 = World.CreateProp(new Model(-772034186), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
                airLight4.AttachTo(baseCrane, 0, new Vector3(0, -33f, 7.3f), new Vector3());
                airLight5 = World.CreateProp(new Model(-772034186), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
                airLight5.AttachTo(baseCrane, 0, new Vector3(0, -7f, 7.3f), new Vector3());*/
        }
    }
}
