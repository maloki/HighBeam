using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.AutobahnPropStreamer;

namespace HighBeam
{
    public static class NyShop
    {
        private static Vector3 shopPos = new Vector3(4713.5f, -3388.4f, 10.1f);
        private static bool isShopRendered = false;
        private static List<Ped> peds = new List<Ped>();

        public static void RunNyShop()
        {
            if (!isShopRendered && shopPos.DistanceTo(Game.Player.Character.Position) < 140)
            {
                isShopRendered = true;
                LoadNyShop();
                LoadPeds();
            }
            if(isShopRendered && shopPos.DistanceTo(Game.Player.Character.Position) > 160)
            {
                isShopRendered = false; 
                RemoveNyShop();
                RemovePeds();
            }
        }

        private static void RemovePeds()
        {
            for(var i = 0; i < peds.Count; ++i)
            {
                peds[i].Delete();
            }
            peds = new List<Ped>();
        }

        private static void LoadPeds()
        {
            var ped = World.CreatePed(new Model(GTA.Native.PedHash.Bevhills01AFY), shopPos);
            ped.Heading = 262.4f;
            ped.Task.StartScenario("WORLD_HUMAN_COP_IDLES", ped.Position);
            peds.Add(ped);
        }
    }
}
