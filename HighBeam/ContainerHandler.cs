using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XInputDotNetPure;
using static HighBeam.ContainerWare;

namespace HighBeam
{
    public class ContainerHandler
    {
        private static Prop currentContainer;
        private static bool isContainerAttached;
        private static float armY = 0f;
        public static void RunContainerHandler()
        {
            //  GamePad.GetState(PlayerIndex.One).Triggers.Right
            var veh = Game.Player.LastVehicle;



            Game.DisableControlThisFrame(0, GTA.Control.VehicleMoveUpDown);

            if (veh.DisplayName.ToLower().Contains("qash"))
            {
                Game.DisableControlThisFrame(0, Control.VehicleCinCam);
                if (Game.IsControlJustPressed(0, Control.VehicleCinCam))
                {
                    if (!isContainerAttached)
                    {
                        var container = World.GetNearbyProps(veh.Position, 6f).FirstOrDefault();
                        if (container != null)
                        {
                            currentContainer = container;

                            isContainerAttached = true;
                        }
                    }
                    else
                    {
                        currentContainer.Detach();
                        currentContainer.Position = veh.GetOffsetInWorldCoords(new GTA.Math.Vector3(0, 3.5f, 0f));
                        currentContainer = null;
                        isContainerAttached = false;
                    }
                }
                if (isContainerAttached)
                {
                    currentContainer.AttachTo(veh, 0, new GTA.Math.Vector3(0, 3.5f, armY), new GTA.Math.Vector3(0f, 0, 90f));
                }

                armY += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y / 300;
                if (armY >= 1)
                    armY = 1;
                if (armY <= 0)
                    armY = 0;
                if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0.2 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -0.2) && (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= 0.2 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -0.2))
                {
                    Function.Call((Hash)0xF8EBCCC96ADB9FB7, veh, armY, false);
                }

            }
        }
    }
}
