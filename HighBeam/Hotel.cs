using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.AutobahnPropStreamer;
using static HighBeam.Main;

namespace HighBeam
{
    public static  class Hotel
    {
        private static bool isHotelLoaded;
        public static void RunHotel()
        {
            var pos = new Vector3(-5174.8f, 5832f, 11.7f);
            if (pos.DistanceTo(Game.Player.Character.Position) < 530 && !isHotelLoaded && !isOnAutobahn)
            {
                isHotelLoaded = true;
                LoadHotel();
               
            }
            if (pos.DistanceTo(Game.Player.Character.Position) > 560 && isHotelLoaded || (isOnAutobahn && isHotelLoaded))
            {
                isHotelLoaded = false;
                RemoveHotel();
            }
            //  UI.ShowSubtitle(veh.Model.Hash.ToString());
            if (isHotelLoaded)
            {
                Function.Call((Hash)0xB3B3359379FE77D3, 0f);
                Function.Call((Hash)0x245A6883D966D537, 0f);
            }
        }
    }
}
