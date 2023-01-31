using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;
using XInputDotNetPure;
using System.Diagnostics;
using GTA.Native;
using System.Drawing;
using static HighBeam.AutobahnPropStreamer;

namespace HighBeam
{
    public static class House
    {
        private static Prop garageDoor = null;
        private static Stopwatch garageControlerKeyStopwatch = new Stopwatch();
        private static bool isGarageOpen = false;
        private static bool isGarageToggleInProgress = false;
        private static float garageRot = 0f;
        private static Vector3 garagePos;
        private static Vector3 defaultGaragePos;
        private static float garageForwardPos = 0f;
        private static float garageZ = 0f;
        private static bool init;
        private static Stopwatch rotDelay = new Stopwatch();
        private static Stopwatch zDelay = new Stopwatch();
        private static Stopwatch forwDelay = new Stopwatch();
        public static void RunHouse()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == ButtonState.Pressed && !garageControlerKeyStopwatch.IsRunning)
            {

                garageControlerKeyStopwatch.Start();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == ButtonState.Pressed && garageControlerKeyStopwatch.ElapsedMilliseconds > 100)
            {
                isGarageToggleInProgress = true;
                Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", garageDoor, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
            }
            if (garageControlerKeyStopwatch.ElapsedMilliseconds > 500)
            {
                garageControlerKeyStopwatch = new Stopwatch();
            }
            if (garageDoor != null)
            {
                var homelink = new UIContainer(new Point(20, UI.HEIGHT - 215), new Size(70, 21), System.Drawing.Color.FromArgb(80, 0, 0, 0));
                Color color = Color.Gray;
                if (isGarageOpen)
                    color = Color.LawnGreen;
                if (isGarageToggleInProgress)
                    color = Color.Orange;
                homelink.Items.Add(new UIText("HOMELINK", new Point(32, 2), 0.26f, color, GTA.Font.ChaletLondon, true));
                homelink.Enabled = true;
                homelink.Draw();
            }
            OperateGarageDoor();
            GarageDoor(); 
           // AmbientLight();
        }

        private static void AmbientLight()
        {
            var poss = new Vector3(1005.88f, -757.8f, 58.32f);
            if (Game.Player.Character.Position.DistanceTo(poss) < 2.5f)
                Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, poss.X, poss.Y, poss.Z, 197, 36, 126, 7f, 1.6f);
        }

        private static void OperateGarageDoor()
        {
            if (isGarageToggleInProgress && garageDoor != null)
            {
                if (!init)
                {
                    forwDelay = new Stopwatch();
                    zDelay = new Stopwatch();
                    forwDelay.Start();
                    zDelay.Start();
                    init = true;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 81, "dryer", garageDoor, "CARWASH_SOUNDS", 0, 0);
                }
                bool isForward = isGarageOpen ? false : true;
                float zStep = 0.0020f;
                float forwardStep = 0.0021f;
                float tempZ = 0f;
                float tempForward = 0f;
                if (garageRot <= 90)
                {
                    garageRot = isForward ? garageRot + 0.08f : garageRot - 0.08f;
                }
                if (garageRot > 35f && garageZ <= 1.1f && (isForward || zDelay.ElapsedMilliseconds >= 2284))
                {
                    garageZ = isForward ? garageZ + zStep : garageZ - zStep;
                    tempZ = zStep;
                }
                if (garageRot > 30f && garageForwardPos <= 1.5f && (isForward || forwDelay.ElapsedMilliseconds >= 600))
                {
                    garageDoor.Rotation = new Vector3(0, garageDoor.Rotation.Y, garageDoor.Rotation.Z);
                    garageForwardPos = isForward ? garageForwardPos + forwardStep : garageForwardPos - forwardStep;
                    tempForward = forwardStep;
                }
                if (!isForward)
                {
                    if (garageZ < 0 || garageRot <= 35f)
                        garageZ = 0;
                    if (garageForwardPos < 0 || garageRot <= 30f)
                        garageForwardPos = 0;
                    if (garageRot < 0)
                        garageRot = 0;
                }
                garagePos = garageDoor.GetOffsetInWorldCoords(new Vector3(0f, isForward ? -tempForward : tempForward, isForward ? tempZ : -tempZ));
                if (garageRot >= 90f && garageForwardPos >= 1.5f && garageZ >= 1.1f && isForward)
                {
                    isGarageOpen = true;
                    isGarageToggleInProgress = false;
                    garageRot = 90f;
                    garageForwardPos = 1.5f;
                    garageZ = 1.1f;
                    init = false;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", garageDoor, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                }
                if (garageRot <= 0 && garageForwardPos <= 0 && garageZ <= 0 && !isForward)
                {
                    garageForwardPos = 0f;
                    garageZ = 0f;
                    garageRot = 0f;
                    garagePos = defaultGaragePos;
                    isGarageOpen = false;
                    isGarageToggleInProgress = false;
                    init = false;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 81, "Prop_Drop_Water", garageDoor, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                }
                garageDoor.Rotation = new Vector3(garageRot, garageDoor.Rotation.Y, garageDoor.Rotation.Z);
                garageDoor.Position = garagePos;
            }
        }

        public static void GarageDoor()
        {
            var garPos = new Vector3(757.5494f, -185.345825f, 75.25f);
            var dist = Game.Player.Character.Position.DistanceTo(garPos);
            if (dist < 120f && Game.Player.Character.Position.Z >= 70f)
            {
                if (garageDoor == null)
                {
                    garageDoor = World.CreateProp(new Model(30769481), new Vector3(0f, 0f, 0f), false, false);
                    garageDoor.FreezePosition = true;
                    garageDoor.Position = garPos;
                    garagePos = garageDoor.Position;
                    defaultGaragePos = garagePos;
                    garageDoor.Rotation = new Vector3(0f, 0f, -31.7999477f); 
                    garageRot = garageDoor.Rotation.X;
                    garageDoor.Quaternion = new Quaternion(0f, 0f, -0.273958832f, 0.9617414f);
                    LoadHouse(); 

                }
            }
            else if (dist > 140f)
            {
                if (garageDoor != null)
                {
                    garageDoor.Delete();
                    garageDoor = null;
                    garageForwardPos = 0f;
                    RemoveHouse();
                }
            }
        }
    }
}

/*
 
  var dist = Game.Player.Character.Position.DistanceTo(new Vector3(1012.94f, -751.08f, 58.79f));
            if (dist < 60f)
            {
                if (garageDoor == null)
                {
                    garageDoor = World.CreateProp(new Model(30769481), new Vector3(0f, 0f, 0f), false, false);
                    garageDoor.FreezePosition = true;
                    garageDoor.Position = new Vector3(1012.94f, -751.08f, 58.79f);
                    garagePos = garageDoor.Position;
                    defaultGaragePos = garagePos;
                    garageDoor.Rotation = new Vector3(-0.04058862f, 0.000104278784f, -49.59975f);
                    garageRot = garageDoor.Rotation.X;
                    garageDoor.Quaternion = new Quaternion(-0.000321155676f, 0.000149396335f, -0.4194501f, 0.9077783f);
                    LoadHouse();

                }
            }*/
