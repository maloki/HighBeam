using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public static class ZoneCreatorOld
    {
        private static bool isCreatorOn = false;
        private static HighwayZoneModel zoneModel = new HighwayZoneModel();
        private static int dirCount = 0;
        private static Stopwatch creatorStopWatch = new Stopwatch();
        private static List<PathModel> pathList = new List<PathModel>();

        public static void RunZoneCreator()
        {
            bool no = false;
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadDown) && isCreatorOn)
            {
                isCreatorOn = false;
                UI.ShowSubtitle("path creator OFF " + pathList.Count);
                SavePathToFile();
                no = true;
            }
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadDown) && !no)
            {
                isCreatorOn = true;
                UI.ShowSubtitle("path creator ON");
            }
            if (isCreatorOn && !creatorStopWatch.IsRunning)
            {
                creatorStopWatch.Start();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
            {
                PathCreator();  
                creatorStopWatch = new Stopwatch();
                UI.ShowSubtitle("path added");
            }
        }

        private static void PathCreator()
        {
            Vehicle veh = Game.Player.LastVehicle;
            int x = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).X, 0).ToString());
            int y = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Y, 0).ToString());
            int z = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Z, 0).ToString());
            PathModel path = new PathModel()
            {
                Position = veh.Position,
                Direction = Game.Player.Character.Heading
                
            };
            pathList.Add(path);
        }

        private static void SavePathToFile()
        {
            string path = Path.GetPathRoot(Environment.SystemDirectory);
            TextWriter tsw = new StreamWriter(@"D:\path.txt", false); 
            tsw.Write(@"new LaneModel()
{
Name = ""lane"",
PathList = new List<PathModel>(){"); 
            foreach(var p in pathList)
            {
                tsw.Write($@"new PathModel() 
{"{"}  
Position = new Vector3({p.Position.X}f, {p.Position.Y}f, {p.Position.Z}f),
Direction = {p.Direction.ToString().Replace(',', '.')}f,
{"},"}");
            }
            tsw.Write(@"}
}");

            tsw.Close();
        }
    }
}

