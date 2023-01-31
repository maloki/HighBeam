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
using static HighBeam.Main;
using Color = System.Drawing.Color;

namespace HighBeam
{
    public static class Crane
    {
        public static Stopwatch SpawnCraneStopwatch = new Stopwatch();
        public static Prop basket;
        public static Prop movableBasket = null;
        public static bool isCraneMode = false;
        public static bool isControllingCrane = false;
        public static Prop lf;
        public static Prop lr;
        public static Prop rf;
        public static Prop rr;
        public static Prop baseCrane;
        public static Prop bottomStairs;
        public static List<Prop> condignations = new List<Prop>();
        public static float wheelHeight = 0;
        public static Prop body1;
        public static Prop body2;
        public static Prop crane1;
        public static Prop crane2;
        public static Prop crane3;
        public static Prop crane4;
        public static bool isCraneAttachedToTrailer = false;
        public static Stopwatch craneExitStopwatch = new Stopwatch();
        public static Camera defaultCam;
        public static Camera mainCam;
        public static Prop armHook;
        public static Prop topLadder;
        public static Prop armHookFake;
        public static float armX = 0;
        public static float armY = -20f;
        public static float armZ = 3.8f;
        public static float armRot = 90f;
        public static float armRotX = 0f;
        public static float armRotY = 0f;
        public static Entity attachedToHook = null;
        public static Stopwatch attachToHookStopwatch = new Stopwatch();
        public static float defaultFov = 0;
        public static float hookHeightAboveGround = 0f;
        public static float maxForwardArm = 259.0f;
        public static float minForwardArm = 203.3f;
        public static float forwardArmDist = 0f;
        public static bool operatingCrane = false;
        public static Vector3 enterCraneOffset = new Vector3();
        public static Stopwatch craneEnterCabinStopwatch = new Stopwatch();
        public static bool isMovingBaseCraneAudio = false;
        public static bool isArmHookAudio = false;
        public static bool isCraneAudioSet = false;
        public static bool isArmAudioSet = false;
        public static bool isCloseCam = false;
        public static float maxArmWindThreshold = 30f;
        public static float armWindX = 0f;
        public static float armWindY = 0f;
        public static bool isArmXToLeft = true;
        public static Stopwatch closeCamStopwatch = new Stopwatch();
        public static float baseCraneRot = 0;
        public static float threshUp = 0.05f;
        public static float threshDown = 0.05f;
        private static Random r = new Random();
        public static Stopwatch windStopwatch = new Stopwatch();
        public static Stopwatch windBlowStopwatch = new Stopwatch();
        public static int weightThreshold = 2;
        public static int slowDownThreshold = 6;
        public static bool isWindBlowing = false;
        public static float windSpeed = 0.2f;
        public static int nextWindTime = 5000;
        public static float desiredWindSpeed = 0f;
        public static bool isSettingDesiredWind = false;
        public static float windChangeForce = 0.0002f;
        public static int windBlowTime = 0;
        public static int condigCount = 3;
        public static Prop airLight1;
        public static Prop airLight2;
        public static Prop airLight3;
        public static Prop airLight4;
        public static Prop airLight5;
        public static Color craneLightColor = Color.FromArgb(255, 255, 5, 5);
        public static int radioMenuCurrentHover = 0;
        public static UIContainer radioDisplay;
        public static ButtonState lastRadioMenuButtonState = ButtonState.Released;
        public static Stopwatch radioMenuScrollDelay = new Stopwatch();
        public static bool showRadio = false;
        public static string selectedMenuItem = "";
        public static List<string> groundCrewMenu = new List<string>() { "Confirm", "Manage hook here", "Follow arm hook", "Go to point", "Random chat", "Finish task", "Spawn products" };
        public static List<string> productsMenu = new List<string>() { "Cement", "Pipes", "Shuttering", "Pillar", "Wood planks s", "Wood planks xl" };
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
        public static void RunCrane()
        {

            if (Game.IsControlJustPressed(0, Control.Jump))
            {
                //   GTA.Native.Function.Call((Hash)0x3523634255FC3318, Game.Player.Character, "GENERIC_INSULT_HIGH", "S_M_Y_GENERICWORKER_01_WHITE_01", "Speech_Params_Force_Shouted_Critical", 0);
            }
            if (!SpawnCraneStopwatch.IsRunning && Game.IsControlJustPressed(0, GTA.Control.ScriptPadLeft))
            {
                SpawnCraneStopwatch.Start();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadLeft) && SpawnCraneStopwatch.ElapsedMilliseconds > 2000 && !isCraneMode)
            {
                isCraneMode = true;
                SpawnCraneStopwatch = new Stopwatch();
                SpawnCrane();
                UI.Notify("Crane mode on");
            }
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadLeft))
            {
                SpawnCraneStopwatch = new Stopwatch();
            }
            if (SpawnCraneStopwatch.IsRunning && SpawnCraneStopwatch.ElapsedMilliseconds > 2000 && isCraneMode)
            {
                SpawnCraneStopwatch = new Stopwatch();
                DeleteCrane();
                UI.Notify("Crane mode off");
                isCraneMode = false;
            }
            if (SpawnCraneStopwatch.IsRunning && SpawnCraneStopwatch.ElapsedMilliseconds > 2000 && !isCraneMode)
            {
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 2000)
                {
                    UI.ShowSubtitle("2 small");
                    condigCount = 2;
                }
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 3000)
                {
                    UI.ShowSubtitle("3 normal");
                    condigCount = 3;
                }
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 4000)
                {
                    UI.ShowSubtitle("4 medium");
                    condigCount = 4;
                }
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 5000)
                {
                    UI.ShowSubtitle("5 large");
                    condigCount = 5;
                }
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 6000)
                {
                    UI.ShowSubtitle("6 very high");
                    condigCount = 6;
                }
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 7000)
                {
                    UI.ShowSubtitle("10 top high");
                    condigCount = 10;
                }
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 8000)
                {
                    UI.ShowSubtitle("18 super high");
                    condigCount = 18;
                }
                if (SpawnCraneStopwatch.ElapsedMilliseconds > 9000)
                {
                    UI.ShowSubtitle("30 crazy high");
                    condigCount = 30;
                }
            }
            if (isCraneMode)
            {
                CraneWind();
                CraneLights();
                if (isControllingCrane)
                {
                    Game.Player.Character.IsVisible = false;
                    Game.Player.Character.FreezePosition = true;
                    CraneControls();
                    CraneDisplays();
                    baseCrane.FreezePosition = false;
                }
                else
                {
                    Game.Player.Character.FreezePosition = false;
                    Game.Player.Character.IsVisible = true;
                    baseCrane.FreezePosition = true;
                    baseCrane.HasCollision = true;
                }
                if (!craneEnterCabinStopwatch.IsRunning)
                    craneEnterCabinStopwatch.Start();
                if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed && (enterCraneOffset.DistanceTo(Game.Player.Character.Position) < 5.5f || isControllingCrane) && craneEnterCabinStopwatch.ElapsedMilliseconds > 2000)
                {
                    if (isControllingCrane)
                    {
                        Game.Player.Character.Position = baseCrane.GetOffsetInWorldCoords(new Vector3(-2.4f, 2f, 1f));
                    }
                    isControllingCrane = isControllingCrane ? false : true;
                    craneEnterCabinStopwatch = new Stopwatch();

                }
                //walkie talkie
                if (!monitorGroundCrewTaskStopwatch.IsRunning)
                {
                    monitorGroundCrewTaskStopwatch.Start();
                }
                if (monitorGroundCrewTaskStopwatch.ElapsedMilliseconds > 3000 && groundCrew.Count > 0)
                {
                    monitorGroundCrewTaskStopwatch = new Stopwatch();
                    if (groundCrew[0].TaskSequenceProgress == 2 && groundCrew[1].TaskSequenceProgress == 2)
                    {
                        var attached = attachedToHook != null;
                        SetRadioChatterDelayedMessage("crew", attached ? 6000 : 12000, attached ? 10000 : 20000, "Finish task", attached ? "Hook off" : "Hook on");
                    }
                    if (isGroundCrewFollowingArmHook)
                        RunCraneGroundCrewAction("Follow arm hook");
                }
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
                    if (radioChatterResponseMessageCallbackAction != "")
                        RunCraneGroundCrewAction(radioChatterResponseMessageCallbackAction);
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && !showRadio)
                {
                    lastRadioMenuButtonState = ButtonState.Pressed;
                    showRadio = true;
                    radioToggleStopwatch = new Stopwatch();
                    radioToggleStopwatch.Start();
                }
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

        public static int GenerateRandomNumberBetween(int min, int max, bool isLookingRnd = false)
        {
            // using second random generator if function is called from the multiple threads at the same time, to prevent generating the same numbers
            return r.Next(min, max);
        }

        private static void WalkieTalkieUI()
        {
            var radioBackground = new UIContainer(new Point(UI.WIDTH - 200, UI.HEIGHT - 210), new Size(140, 210), Color.FromArgb(255, 5, 5, 5));
            // antena
            radioBackground.Items.Add(new UIContainer(new Point(20, -80), new Size(22, 80), Color.FromArgb(255, 5, 5, 5)));
            // display background  
            radioDisplay = new UIContainer(new Point(UI.WIDTH - 190, UI.HEIGHT - 160), new Size(120, 95), Color.FromArgb(255, 202, 116, 35));
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
            radioBackground.Items.Add(new UIText("MOTOROLA", new Point(72, 14), 0.46f, Color.White, GTA.Font.Monospace, true));
            radioBackground.Enabled = true;
            radioBackground.Draw();
            radioDisplay.Draw();
        }

        private static void WalkieTalkieMenu(string text, int index, int realIndex)
        {
            if (text == "Follow arm hook" && isGroundCrewFollowingArmHook)
                text = "* Cancel follow";
            var y = (index * 20) + 8;
            var item = new UIContainer(new Point(5, y), new Size(110, 17), Color.FromArgb(realIndex == radioMenuCurrentHover ? 150 : 50, 0, 0, 0));
            item.Items.Add(new UIText(text, new Point(55, 1), 0.25f, Color.Black, GTA.Font.ChaletLondon, true));
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
                if (currentRadioMenu == "products")
                {
                    SpawnProducts(selected);
                }
                else
                {
                    if (selected == "Spawn products")
                    {
                        ChangeRadioMenu(productsMenu, "products");
                    }
                    else
                    {
                        if (selected == "Follow arm hook")
                            isGroundCrewFollowingArmHook = isGroundCrewFollowingArmHook ? false : true;
                        else
                            isGroundCrewFollowingArmHook = false;
                        RunCraneGroundCrewAction(selected);
                        if (selected != "Go to point" && selected != "Follow arm hook")
                        {
                            WalkieTalkiePlaySound(selected, "self");
                            if (selected != "Confirm")
                            {
                                var act = "";
                                if (selected == "Manage hook here")
                                    act = "Assist confirmation";
                                if (selected == "Finish task")
                                    act = "Finish";
                                SetRadioChatterDelayedMessage("crew", 5000, 8000, action: act);
                            }
                        }
                    }
                }
            }
        }

        private static void SpawnProducts(string product)
        {
            int pHash = 0;
            int productsCount = 1;
            float space = 0;
            switch (product)
            {
                case "Pipes":
                    pHash = 525797972;
                    productsCount = 2;
                    space = 0.8f;
                    break;
                case "Cement":
                    pHash = 1962326206;
                    productsCount = 4;
                    space = 2;
                    break;
                case "Shuttering":
                    pHash = 309416120;
                    productsCount = 4;
                    space = 1;
                    break;
                case "Pillar":
                    pHash = 99477918;
                    productsCount = 1;
                    space = 1;
                    break;
                case "Wood planks s":
                    pHash = -1186441238;
                    productsCount = 1;
                    space = 1;
                    break;
                case "Wood planks xl":
                    pHash = -740912282;
                    productsCount = 1;
                    space = 1;
                    break;

            }
            float currentSpace = 4;
            for (var i = 0; i < productsCount; ++i)
            {
                var prop = World.CreateProp(new Model(pHash), armHook.GetOffsetInWorldCoords(new Vector3(0, currentSpace, 0)), true, true);
                prop.Heading = baseCrane.Heading;
                currentSpace += space;
                spawnedPropsList.Add(prop);
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
                if (action == "Confirm")
                {
                    soundName = $"chatter_self_roger_{rnd.Next(1, 3)}";
                    subtitle += "Roger that";
                }
                if (action == "Manage hook here" || action == "Random chat" || action == "Finish task")
                {
                    soundName = $"chatter_self_random_{rnd.Next(1, 4)}";
                    if (action == "Manage hook here")
                    {
                        if (attachedToHook == null)
                            subtitle += "I need someone to assist me at this position.";
                        else
                            subtitle += "Im at this position, ready to take hook off.";
                    }
                    if (action == "Random chat")
                        subtitle += "Random chat.";
                    if (action == "Finish task")
                        subtitle += "Ok, that's enough, thanks for help";
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
            UI.ShowSubtitle(subtitle, 5000);
            chatterVoiceSound = new MediaPlayer();
            chatterVoiceSound.Open(new Uri(new System.Uri(Path.GetFullPath($"./scripts/chatter_sounds/{soundName}.wav")).AbsoluteUri));
            chatterVoiceSound.Volume = volume;
            chatterVoiceSound.Play();
        }

        private static void RunCraneGroundCrewAction(string action)
        {
            if (action == "Manage hook here")
            {
                var seq1 = new TaskSequence();
                var pos1 = armHook.GetOffsetInWorldCoords(new Vector3(0, 3, 0));
                seq1.AddTask.GoTo(new Vector3(pos1.X, pos1.Y, groundCrew[0].Position.Z));
                seq1.AddTask.AchieveHeading((armHook.Heading + 180) % 360);
                seq1.AddTask.StandStill(50000);
                seq1.Close();
                groundCrew[0].AlwaysKeepTask = true;
                groundCrew[0].Task.PerformSequence(seq1);
                seq1.Dispose();

                var pos2 = armHook.GetOffsetInWorldCoords(new Vector3(0, -3, 0));
                var seq2 = new TaskSequence();
                seq2.AddTask.GoTo(new Vector3(pos2.X, pos2.Y, groundCrew[0].Position.Z));
                seq2.AddTask.AchieveHeading(armHook.Heading);
                seq2.AddTask.StandStill(50000);
                seq2.Close();
                groundCrew[1].AlwaysKeepTask = true;
                groundCrew[1].Task.PerformSequence(seq2);
                seq2.Dispose();
            }
            if (action == "Go to point" || action == "Follow arm hook")
            {
                var seq1 = new TaskSequence();
                var pos1 = isControllingCrane ? armHook.GetOffsetInWorldCoords(new Vector3(0, 3, 0)) : Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-1, 1, 0));
                seq1.AddTask.GoTo(new Vector3(pos1.X, pos1.Y, groundCrew[0].Position.Z));
                if (!isControllingCrane)
                    seq1.AddTask.ChatTo(Game.Player.Character);
                else
                    seq1.AddTask.StandStill(50000);
                seq1.Close();
                groundCrew[0].AlwaysKeepTask = true;
                groundCrew[0].Task.PerformSequence(seq1);
                seq1.Dispose();

                var pos2 = isControllingCrane ? armHook.GetOffsetInWorldCoords(new Vector3(0, -3, 0)) : Game.Player.Character.GetOffsetInWorldCoords(new Vector3(1, 1, 0));
                var seq2 = new TaskSequence();
                seq2.AddTask.GoTo(new Vector3(pos2.X, pos2.Y, groundCrew[0].Position.Z));
                if (!isControllingCrane)
                    seq2.AddTask.ChatTo(Game.Player.Character);
                else
                    seq2.AddTask.StandStill(50000);
                seq2.Close();
                groundCrew[1].AlwaysKeepTask = true;
                groundCrew[1].Task.PerformSequence(seq2);
                seq2.Dispose();
            }
            if (action == "Finish task")
            {
                var seq1 = new TaskSequence();
                seq1.AddTask.WanderAround(groundCrew[0].Position, 30f);
                seq1.Close();
                groundCrew[0].AlwaysKeepTask = true;
                groundCrew[0].Task.PerformSequence(seq1);
                seq1.Dispose();

                var seq2 = new TaskSequence();
                seq2.AddTask.WanderAround(groundCrew[1].Position, 30f);
                seq2.Close();
                groundCrew[1].AlwaysKeepTask = true;
                groundCrew[1].Task.PerformSequence(seq2);
                seq2.Dispose();
            }
        }

        private static void SpawnCraneGroundCrew()
        {
            groundCrew = new List<Ped>();
            for (var i = 0; i < 2; ++i)
            {
                var p = World.CreatePed(new Model(PedHash.Construct02SMY), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(i, -10, 0)));
                p.MaxHealth = 1;
                p.RandomizeOutfit();
                p.IsInvincible = true;
                groundCrew.Add(p);
            }
        }

        private static void DeleteCraneGroundCrew()
        {
            for (var i = 0; i < groundCrew.Count; ++i)
            {
                groundCrew[i].Delete();
            }
            groundCrew = new List<Ped>();
        }

        private static void WalkieTalkie()
        {
            var sound = new MediaPlayer();
            sound.Open(new Uri(new System.Uri(Path.GetFullPath($"./scripts/chatter_sounds/chatter_random_{new Random().Next(1, 4)}.wav")).AbsoluteUri));
            sound.Volume = 0.5;
            sound.Play();

            var pedd = World.CreatePed(new Model(PedHash.Construct01SMY), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-10, 40, 0)));
            pedd.IsInvincible = true;
            var poss = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-10, 20, 0));
            Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, pedd, veh, 0, 100, 0, 1.0, -1, 2.0, false);
        }

        public static void CraneLights()
        {
            float bright = 20f;
            if (!craneExitStopwatch.IsRunning)
            {
                craneExitStopwatch.Start();
            }
            if (Game.Player.Character.Position.Z < bottomStairs.Position.Z + 6f && craneLightColor.G > 5 && craneLightColor.B > 5 && craneExitStopwatch.ElapsedMilliseconds % 1 == 0)
            {
                craneLightColor = Color.FromArgb(255, craneLightColor.R, craneLightColor.B - 1, craneLightColor.G - 1);
            }
            if (Game.Player.Character.Position.Z > bottomStairs.Position.Z + 6f && craneLightColor.G < 255 && craneLightColor.B < 255 && craneExitStopwatch.ElapsedMilliseconds % 1 == 0)
            {
                craneLightColor = Color.FromArgb(255, craneLightColor.R, craneLightColor.B + 1, craneLightColor.G + 1);
            }
            // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, armHook.Position.X, armHook.Position.Y, armHook.Position.Z - 7f, 255, 255, 255, 20f, 4f);
            int off = -20;
            for (var i = 0; i < 13; i++)
            {
                var v = baseCrane.GetOffsetInWorldCoords(new Vector3(0, -off, 11f));
                Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, v.X, v.Y, v.Z - 7f, craneLightColor.R, craneLightColor.G, craneLightColor.B, 8f, bright);
                off += 7;
            }
            off = 3;
            for (var i = 0; i < condigCount * 4; i++)
            {
                var v = bottomStairs.GetOffsetInWorldCoords(new Vector3(0, 0, off));
                Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, v.X, v.Y, v.Z - 7f, craneLightColor.R, craneLightColor.G, craneLightColor.B, 6f, bright);
                off += 7;
            }
            var cableZ = baseCrane.GetOffsetInWorldCoords(new Vector3(0, 0, 3.8f)).Z;
            var cable1 = armHook.GetOffsetInWorldCoords(new Vector3(0, 0f, 0f));
            var cable2 = armHook.GetOffsetInWorldCoords(new Vector3(-0.9f, 0f, 0f));
            var cable3 = armHook.GetOffsetInWorldCoords(new Vector3(0.9f, 0f, 0f));
            Function.Call((Hash)0x6B7256074AE34680, cable1.X, cable1.Y, cableZ, cable1.X, cable1.Y, cable1.Z, 0, 0, 0, 255);
            Function.Call((Hash)0x6B7256074AE34680, cable2.X, cable2.Y, cableZ, cable2.X, cable2.Y, cable2.Z, 0, 0, 0, 255);
            Function.Call((Hash)0x6B7256074AE34680, cable3.X, cable3.Y, cableZ, cable3.X, cable3.Y, cable3.Z, 0, 0, 0, 255);
        }

        public static void CraneWind()
        {
            bool isBadWeather = Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Rain")
                  || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Thunder")
                  || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Clearing")
                   || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Foggy");
            if (!windStopwatch.IsRunning)
            {
                windStopwatch.Start();
            }
            if (windStopwatch.ElapsedMilliseconds > nextWindTime && !isSettingDesiredWind)
            {
                windStopwatch = new Stopwatch();
                // nextWindTime = GenerateRandomNumberBetween(45000, 360000);
                nextWindTime = GenerateRandomNumberBetween(10000, 24000);
                float wnd = Function.Call<float>((Hash)0xA8CF1CC0AFCD3F12);
                if (Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Rain"))
                {
                    var wind = GenerateRandomNumberBetween(7, 17);
                    desiredWindSpeed = wind >= 10 ? wind / 10 : float.Parse("0." + wind);
                }
                else if (Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Thunder"))
                {
                    var wind = GenerateRandomNumberBetween(12, 23);
                    desiredWindSpeed = wind / 10;
                }
                else if (wnd > 11.7)
                {
                    var wind = GenerateRandomNumberBetween(9, 15);
                    desiredWindSpeed = wind >= 10 ? wind / 10 : float.Parse("0." + wind);
                }
                else
                {
                    var wind = GenerateRandomNumberBetween(1, 4);
                    desiredWindSpeed = wind >= 10 ? wind / 10 : float.Parse("0." + wind);
                }
                isSettingDesiredWind = true;
            }
            if (isSettingDesiredWind)
            {
                if (windSpeed < desiredWindSpeed)
                    windSpeed += windChangeForce;
                if (windSpeed > desiredWindSpeed)
                    windSpeed -= windChangeForce;
                var wr = Math.Round(windSpeed * 10, 1);
                var dr = Math.Round(desiredWindSpeed * 10, 1);
                if (wr == dr)
                {
                    isSettingDesiredWind = false;
                    threshDown = windSpeed >= 1f ? float.Parse("0." + Math.Round(windSpeed, 0)) : float.Parse("0.0" + (Math.Round(windSpeed * 10, 0)));
                    threshUp = windSpeed >= 1f ? float.Parse("0." + Math.Round(windSpeed, 0)) : float.Parse("0.0" + (Math.Round(windSpeed * 10, 0)));
                }
                if (wr % 3 == 0)
                {
                    threshDown = windSpeed >= 1f ? float.Parse("0." + Math.Round(windSpeed, 0)) : float.Parse("0.0" + (Math.Round(windSpeed * 10, 0)));
                    threshUp = windSpeed >= 1f ? float.Parse("0." + Math.Round(windSpeed, 0)) : float.Parse("0.0" + (Math.Round(windSpeed * 10, 0)));
                }
                if (windSpeed > 0.6f)
                {
                    slowDownThreshold = 0;
                }
                else
                {
                    slowDownThreshold = 6;
                }
            }
            if (!windBlowStopwatch.IsRunning)
            {
                windBlowStopwatch.Start();
            }
            if (windBlowStopwatch.ElapsedMilliseconds > windBlowTime && !isSettingDesiredWind)
            {
                //  windBlowStopwatch = new Stopwatch();
                //  windBlowTime = GenerateRandomNumberBetween(6000, 20000);
                //  desiredWindSpeed -= float.Parse("0.0" + GenerateRandomNumberBetween(1, 7));
                //  isSettingDesiredWind = true;
            }
            var lastArmX = armWindX;
            var lastArmY = armWindY;
            bool isArmXToLeft = false;
            bool isArmYToLeft = false;
            bool overideWind = false;
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0)
            {
                overideWind = true;
                if (armWindX < 3f && armWindX > -3f)
                    armWindX += -(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X / 40);
            }
            if (Game.IsControlJustPressed(0, GTA.Control.Jump))
            {
                /*  if (slowDownThreshold == 0)
                  {
                      windSpeed = 0.2f;
                      slowDownThreshold = 1;
                  }
                  else
                  {
                      windSpeed = 2f;
                      threshDown = float.Parse("0." + windSpeed);
                      threshUp = float.Parse("0." + windSpeed);
                      slowDownThreshold = 0;
                      weightThreshold = 3;
                  }*/
            }
            if (!overideWind)
                armWindX -= threshUp;
            if (lastArmX > armWindX)
            {
                isArmXToLeft = true;
            }
            if (armWindX > 0f && !overideWind)
            {
                threshUp += float.Parse("0.00" + weightThreshold);
                if (!isArmXToLeft)
                    threshUp += float.Parse("0.000" + slowDownThreshold);
            }
            else if (armWindX < 0f && !overideWind)
            {
                threshUp -= float.Parse("0.00" + weightThreshold);
                if (isArmXToLeft)
                    threshUp -= float.Parse("0.000" + slowDownThreshold);
            }
            armWindY -= threshDown;
            if (lastArmY > armWindY)
            {
                isArmYToLeft = true;
            }
            if (armWindY > 0f)
            {
                threshDown += float.Parse("0.00" + weightThreshold);
                if (!isArmYToLeft)
                    threshDown += float.Parse("0.000" + slowDownThreshold);
            }
            else if (armWindY < 0f)
            {
                threshDown -= float.Parse("0.00" + weightThreshold);
                if (isArmYToLeft)
                    threshDown -= float.Parse("0.000" + slowDownThreshold);
            }
        }

        private static void DeleteSpawnedProps()
        {
            for (var i = 0; i < spawnedPropsList.Count; ++i)
            {
                spawnedPropsList[i].Delete();
            }
            spawnedPropsList = new List<Prop>();
        }

        public static void DeleteCrane()
        {

            bottomStairs.Delete();
            bottomStairs = null;
            for (var i = 0; i < condigCount; i++)
            {
                var toDel = condignations.ElementAt(i);
                toDel.Delete();
            }
            condignations = new List<Prop>();
            topLadder.Delete();
            topLadder = null;
            baseCrane.Delete();
            baseCrane = null;
            armHook.Delete();
            armHookFake.Delete();
            airLight1.Delete();
            airLight2.Delete();
            airLight3.Delete();
            airLight4.Delete();
            airLight5.Delete();
            DeleteCraneGroundCrew();
            DeleteSpawnedProps();
        }

        public static void CraneDisplays()
        {

            forwardArmDist = new Vector3(armHook.Position.X, armHook.Position.Y, baseCrane.GetOffsetInWorldCoords(new Vector3(0, 0, -2f)).Z).DistanceTo(baseCrane.GetOffsetInWorldCoords(new Vector3(0, 200, -2f)));

            var minutes = World.CurrentDayTime.Minutes;
            var ground = armHook.GetOffsetInWorldCoords(new Vector3(0, 1.2f, 0));
            hookHeightAboveGround = World.GetDistance(ground, new Vector3(ground.X, ground.Y, 0)) - (World.GetGroundHeight(ground) >= 0 ? World.GetGroundHeight(ground) : 0) - 9.1f;
            // UI.ShowSubtitle(hookHeightAboveGround.ToString() + "m");
            var cont = new UIContainer(new Point(5, UI.HEIGHT - 150), new Size(200, 140), Color.FromArgb(255, 5, 5, 5));
            cont.Items.Add(new UIText(("Wind: " + Math.Round(windSpeed * 10, 1).ToString() + "ms"), new Point(70, 30), 0.4f, windSpeed * 10 >= 8 ? Color.Red : Color.White, GTA.Font.ChaletLondon, true));
            //cont.Items.Add(new UIText(("Arm Heading: " + Math.Round(armHook.Heading - baseCrane.Heading, 1).ToString() + "deg"), new Point(70, 56), 0.41f, Color.White, GTA.Font.ChaletLondon, true));
           // cont.Items.Add(new UIText((World.CurrentDayTime.Hours + ":" + (minutes.ToString().Length == 1 ? 0 + "" + minutes : minutes.ToString())), new Point(100, 7), 0.4f, Color.LawnGreen, GTA.Font.ChaletLondon, true));
            cont.Enabled = true;
            cont.Draw();
        }

        public static void CraneControls()
        {

            Game.DisableControlThisFrame(0, Control.SelectWeapon);
            Game.DisableControlThisFrame(0, Control.Sprint);
            if (!attachToHookStopwatch.IsRunning)
                attachToHookStopwatch.Start();
            if (!closeCamStopwatch.IsRunning)
                closeCamStopwatch.Start();
            // for first person cabin view Game.Player.Character.Position = baseCrane.GetOffsetInWorldCoords(new Vector3(-2f, -1.6f, 42.5f));
            if (!isCloseCam)
            {
                Game.Player.Character.Position = baseCrane.GetOffsetInWorldCoords(new Vector3(-3f, -8f, 1f));
                Game.Player.Character.Heading = baseCrane.Heading;
            }
            else
            {
                Game.Player.Character.Position = armHook.GetOffsetInWorldCoords(new Vector3(6f, 0f, -5f));
                Game.Player.Character.Heading = baseCrane.Heading;
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0)
            {
                baseCraneRot += (-GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X / 15);
                baseCrane.AttachTo(topLadder, 0, new Vector3(), new Vector3(0, 0, baseCraneRot));
                isMovingBaseCraneAudio = true;
            }
            else
            {
                isMovingBaseCraneAudio = false;
            }
            float rotForce = 0.35f;
            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
            {
                armRot += rotForce;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
            {
                armRot -= rotForce;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed && GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
                armRot = 90f;
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y != 0 || GamePad.GetState(PlayerIndex.One).Triggers.Left != 0 || GamePad.GetState(PlayerIndex.One).Triggers.Right != 0)
            {
                isArmHookAudio = true;
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.1 && baseCrane.GetOffsetInWorldCoords(new Vector3(0, 0, 3.8f)).Z > armHook.Position.Z)
                {
                    var val = GamePad.GetState(PlayerIndex.One).Triggers.Left / 30f;
                    armZ += val;
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.1)
                {
                    var val = GamePad.GetState(PlayerIndex.One).Triggers.Right / 30f;
                    armZ -= val;
                }
                if ((forwardArmDist > maxForwardArm && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.8) || (forwardArmDist < minForwardArm && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.8) || (forwardArmDist > minForwardArm && forwardArmDist < maxForwardArm))
                    armY -= GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y / 30;
            }
            else
            {
                isArmHookAudio = false;
            }
            armHook.AttachTo(baseCrane, 0, new Vector3(armX, armY, armZ), new Vector3(armWindX, armWindY, armRot));
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && attachToHookStopwatch.ElapsedMilliseconds > 1000 && !showRadio)
            {
                if (attachedToHook == null)
                {
                    var list = World.GetNearbyEntities(armHook.GetOffsetInWorldCoords(new Vector3(0, 0, -10f)), 5f);
                    var lastPos = 99999f;
                    attachedToHook = null;
                    for (var i = 0; i < list.Length; i++)
                    {
                        var el = list[i];
                        if (el.Model.Hash != -2128680992 && el.Position.DistanceTo(armHook.Position) < lastPos && !el.Model.IsPed)
                        {
                            attachedToHook = el;
                            lastPos = el.Position.DistanceTo(armHook.Position);
                        }
                    }
                    if (attachedToHook != null)
                    {
                        var diff = Math.Abs(attachedToHook.Heading - armHook.Heading) % 360;
                        if (attachedToHook.Heading < armHook.Heading)
                            diff = -diff;
                        attachedToHook.AttachTo(armHook, 0, new Vector3(0, 0, -10f), new Vector3(0, 0, diff));
                        attachedToHook.HasCollision = true;
                        attachedToHook.HasGravity = true;
                    }
                }
                else
                {
                    attachedToHook.Detach();
                    //   var groundHeight = Function.Call<float>(Hash.GET_GROUND_Z_FOR_3D_COORD, armHook.Position.X, armHook.Position.Y, armHook.Position.Z);
                    attachedToHook.ApplyForce(new Vector3(0, 0, -2f));
                    attachedToHook = null;
                }
                attachToHookStopwatch = new Stopwatch();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed && closeCamStopwatch.ElapsedMilliseconds > 1000 && !showRadio && radioToggleStopwatch.ElapsedMilliseconds > 1000)
            {
                isCloseCam = isCloseCam ? false : true;
                if (!isCloseCam)
                    Game.Player.Character.Heading = baseCrane.Heading;
                closeCamStopwatch = new Stopwatch();
            }
            //draw helper for arm hook 
            Function.Call((Hash)0x6B7256074AE34680, armHook.Position.X, armHook.Position.Y, armHook.Position.Z - 7f, armHook.Position.X, armHook.Position.Y, 0, 255, 5, 5, 255);
            if (isMovingBaseCraneAudio && !isCraneAudioSet)
            {
                isCraneAudioSet = true;
                Function.Call((Hash)0xE65F427EB70AB1ED, 81, "dryer", armHookFake, "CARWASH_SOUNDS", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 82, "dryer", armHookFake, "CARWASH_SOUNDS", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 83, "dryer", armHookFake, "CARWASH_SOUNDS", 0, 0);
            }
            if (!isMovingBaseCraneAudio && isCraneAudioSet)
            {
                isCraneAudioSet = false;
                Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", armHookFake, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 82, "Prop_Drop_Water", armHookFake, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                Function.Call((Hash)0xE65F427EB70AB1ED, 83, "Prop_Drop_Water", armHookFake, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
            }
            if (isArmHookAudio && !isArmAudioSet)
            {
                isArmAudioSet = true;
                Function.Call((Hash)0xE65F427EB70AB1ED, 84, "dryer", armHookFake, "CARWASH_SOUNDS", 0, 0);
            }
            if (!isArmHookAudio && isArmAudioSet)
            {
                isArmAudioSet = false;
                Function.Call((Hash)0xE65F427EB70AB1ED, 84, "Prop_Drop_Water", armHookFake, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
            }

            /* try  
             {
                 if (Game.IsControlJustPressed(0, GTA.Control.SelectWeapon) && Game.Player.Character.CurrentVehicle == null)
                 {
                     isControllingCrane = isControllingCrane ? false : true;
                     if (isControllingCrane)
                     {
                         if (movableBasket == null)
                         {
                             movableBasket = World.CreateProp(new Model(1158960338), basket.Position, basket.Rotation, false, false);
                         }
                     }
                     else
                     {
                         Game.Player.Character.FreezePosition = false;
                     }
                 }
                 if (isControllingCrane)
                 {
                     Game.Player.Character.FreezePosition = true;
                     var z = movableBasket.Position.Z;
                     if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                     {
                         z += 0.01f;
                     }
                     else if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                     {
                         z -= 0.01f;
                     }
                     if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0)
                     {
                         movableBasket.Rotation = new Vector3(movableBasket.Rotation.X, movableBasket.Rotation.Y, movableBasket.Rotation.Z + 0.25f);
                         Game.Player.Character.Rotation = new Vector3(Game.Player.Character.Rotation.X, Game.Player.Character.Rotation.Y, movableBasket.Rotation.Z);
                     }
                     if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0)
                     {
                         movableBasket.Rotation = new Vector3(movableBasket.Rotation.X, movableBasket.Rotation.Y, movableBasket.Rotation.Z - 0.25f);
                         Game.Player.Character.Rotation = new Vector3(Game.Player.Character.Rotation.X, Game.Player.Character.Rotation.Y, movableBasket.Rotation.Z);
                     }
                     if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y != 0 || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                     {
                         var off = movableBasket.GetOffsetInWorldCoords(new Vector3((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y / 100), (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X * 2), 0));
                         movableBasket.Position = new Vector3(off.X, off.Y, z);
                         Game.Player.Character.Position = movableBasket.Position;
                     }
                     if (movableBasket.Position.Z < basket.Position.Z)
                     {
                         movableBasket.Delete();
                         movableBasket = null;
                         isControllingCrane = false;
                     }
                 }
                 else
                 {
                     Game.Player.Character.FreezePosition = false;
                     if (isTruckMode)
                     {
                         if(Game.Player.Character.CurrentVehicle != null)
                         {
                             if (Game.IsControlJustPressed(0, GTA.Control.SelectWeapon) && Game.Player.Character.CurrentVehicle.DisplayName == baseVeh.DisplayName && truckTrailer.Position.DistanceTo(baseVeh.Position) < 5)
                             {
                                 if (isCraneAttachedToTrailer) 
                                 {
                                     isCraneAttachedToTrailer = false;
                                     baseVeh.Detach();
                                 }
                                 else
                                 {
                                     craneExitStopwatch.Start();
                                     isCraneAttachedToTrailer = true;
                                 }

                             }
                         }
                     }
                 }
                 if (isCraneAttachedToTrailer && isTruckMode)
                 {
                     basket.HasCollision = false;
                     basket.AttachTo(baseVeh, 0, new Vector3(1.3f, 5.3f, 1f), new Vector3(-89.99978f, 4.935332E-06f, 90f));
                     baseVeh.AttachTo(truckTrailer, 0, new Vector3(0, 1.1f, 0.7f), new Vector3(0, 0, 180f));
                     if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed && Game.Player.Character.Position.DistanceTo(baseVeh.Position) <= 3 && (craneExitStopwatch.ElapsedMilliseconds > 6000 || !craneExitStopwatch.IsRunning))
                     {
                         isCraneAttachedToTrailer = false;
                         craneExitStopwatch = new Stopwatch();
                         baseVeh.Detach();
                     }
                 }
                 else
                 {
                     if (basket != null)
                     {
                         basket.HasCollision = true;
                         basket.AttachTo(baseVeh, 0, new Vector3(1.3f, 5.3f, 1f), new Vector3(-89.99978f, 4.935332E-06f, 90f));
                     }
                 }
             }
             catch(Exception e)
             {
                 UI.ShowSubtitle(e.Message);
             }
            */
        }

        public static void SpawnCrane()
        {
            //Function.Call((Hash)0xFEDB7D269E8C60E3, );

            bottomStairs = World.CreateProp(new Model(1681875160), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
            bottomStairs.Heading = Game.Player.Character.Heading;
            float condOffset = 17.1f;
            var lod = 3000;
            for (var i = 0; i < condigCount; i++)
            {
                var condig = World.CreateProp(new Model(494228293), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
                condig.AttachTo(bottomStairs, 0, new Vector3(0, 0.07f, condOffset), new Vector3(0, 0, 0));
                condig.LodDistance = lod;
                condignations.Add(condig);
                condOffset += 14.5f;
            }
            topLadder = World.CreateProp(new Model(263894992), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
            topLadder.AttachTo(condignations.ElementAt(condignations.Count - 1), 0, new Vector3(0, 0, 9f), new Vector3(0, 0, 0));
            baseCrane = World.CreateProp(new Model(1163207500), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 7, 0)), true, true);
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
            airLight5.AttachTo(baseCrane, 0, new Vector3(0, -7f, 7.3f), new Vector3());
            SpawnCraneGroundCrew();
            RunCraneGroundCrewAction("Finish task");







            /*lf = World.CreateProp(new Model(-890463279), baseVeh.Position, false, false);
            lr = World.CreateProp(new Model(-890463279), baseVeh.Position, false, false);
            rf = World.CreateProp(new Model(-890463279), baseVeh.Position, false, false);
            rr = World.CreateProp(new Model(-890463279), baseVeh.Position, false, false);
            lf.AttachTo(baseVeh, 0, new Vector3(-0.85f, 1.1f, 0.05f), new Vector3(0, 0, 0));
            lr.AttachTo(baseVeh, 0, new Vector3(-0.85f, -1.2f, 0.05f), new Vector3(0, 0, 0)); 
            rf.AttachTo(baseVeh, 0, new Vector3(0.85f, 1.1f, 0.05f), new Vector3(0, 0, 180));
            rr.AttachTo(baseVeh, 0, new Vector3(0.85f, -1.2f, 0.05f), new Vector3(0, 0, 180));
            basket = World.CreateProp(new Model(1158960338), baseVeh.Position, new Vector3(-89.99978f, 4.935332E-06f, 1.000003f), false, false);
            basket.AttachTo(baseVeh, 0, new Vector3(1.3f, 5.3f, 1f), new Vector3(-89.99978f, 4.935332E-06f, 90f));
            basket.HasCollision = true;
            body1 = World.CreateProp(new Model(1366469466), baseVeh.Position, false, false);
            body2 = World.CreateProp(new Model(1366469466), baseVeh.Position, false, false);
            body1.AttachTo(baseVeh, 0, new Vector3(0.02f, 0.5f, -0.3f), new Vector3(0, 0, -90f));
            body2.AttachTo(baseVeh, 0, new Vector3(0.0f, -0.7f, -0.31f), new Vector3(0, 0, -90f));
            crane1 = World.CreateProp(new Model(173177608), baseVeh.Position, false, false);
            crane2 = World.CreateProp(new Model(173177608), baseVeh.Position, false, false);
            crane3 = World.CreateProp(new Model(173177608), baseVeh.Position, false, false);
            crane4 = World.CreateProp(new Model(173177608), baseVeh.Position, false, false);
            crane1.AttachTo(baseVeh, 0, new Vector3(0.0f, 4.7f, 0.1f), new Vector3(0f, -94f, 90f));
            crane2.AttachTo(baseVeh, 0, new Vector3(0.0f, 1.1f, 0.7f), new Vector3(0.0f, -94f, 270f));
            crane3.AttachTo(baseVeh, 0, new Vector3(0.0f, 4.7f, 1.2f), new Vector3(0f, -94f, 90f));
            crane4.AttachTo(baseVeh, 0, new Vector3(0.0f, 1.1f, 1.7f), new Vector3(0.0f, -94f, 270f));*/

        }
        public static Vector3 RotationToDirection(Vector3 Rotation) { float z = Rotation.Z; float num = z * 0.0174532924f; float x = Rotation.X; float num2 = x * 0.0174532924f; float num3 = Math.Abs((float)Math.Cos((double)num2)); return new Vector3 { X = (float)((double)((float)(-(float)Math.Sin((double)num))) * (double)num3), Y = (float)((double)((float)Math.Cos((double)num)) * (double)num3), Z = (float)Math.Sin((double)num2) }; }

    }
}
