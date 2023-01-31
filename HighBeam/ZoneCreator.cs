using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public static class ZoneCreator
    {
        private static bool isCreatorOn = false;
        private static HighwayZoneModel zoneModel = new HighwayZoneModel();
        private static int dirCount = 0;
        public static void RunZoneCreator()
        {
            /* if(Game.IsControlJustReleased(0, GTA.Control.ScriptPadDown) && isCreatorOn)
             {
                 isCreatorOn = false;
                 UI.ShowSubtitle("Creator is turned " + isCreatorOn.ToString().Replace("false", "OFF").Replace("true", "ON"));
             }
             if(Game.IsControlJustReleased(0, GTA.Control.ScriptPadDown) && !isCreatorOn)
             {
                 isCreatorOn = true;
                 UI.ShowSubtitle("Creator is turned " + isCreatorOn.ToString().Replace("false", "OFF").Replace("true", "ON"));
             }*/
            isCreatorOn = true;
            if (isCreatorOn)
            {
                if(Game.IsControlJustReleased(0, GTA.Control.PhoneUp))
                {
                    var x = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).X, 0).ToString());
                    var y = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Y, 0).ToString());
                    var z = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Z, 0).ToString());
                    if (dirCount == 0)
                    {
                        zoneModel.Index = 0;
                        zoneModel.Name = "zone";
                        zoneModel.ZoneBoundary = new ZoneBoundaryModel()
                        {
                            StartLeftX = x,
                            StartLeftY = y
                        };
                        UI.ShowSubtitle("saving start left");
                    }
                    if(dirCount == 1)
                    {
                        zoneModel.ZoneBoundary.StartRightX = x;
                        zoneModel.ZoneBoundary.StartRightY = y;
                        UI.ShowSubtitle("saving start right");
                    }
                    if (dirCount == 2)
                    { 
                        zoneModel.ZoneBoundary.FinishLeftX = x;
                        zoneModel.ZoneBoundary.FinishLeftY = y;
                        UI.ShowSubtitle("saving finish left");
                    }
                    if (dirCount == 3)
                    {
                        zoneModel.ZoneBoundary.FinishRightX = x;
                        zoneModel.ZoneBoundary.FinishRightY = y;
                        UI.ShowSubtitle("saving finish right");
                    }
                    if(dirCount == 4)
                    { 
                        UI.ShowSubtitle("saving model to file, and starting new");
                        dirCount = 0;
                        string path = Path.GetPathRoot(Environment.SystemDirectory);
                        TextWriter tsw = new StreamWriter(@"D:\zones.txt", false); 
                        tsw.Write($@"new HighwayZoneModel()  
{"{"} 
Name = ""zone"",  
 Index = {0}, 
HeadingDirection = {0},  
ZoneBoundary = new ZoneBoundaryModel() 
{"{" }
StartLeftX = {zoneModel.ZoneBoundary.StartLeftX},
StartLeftY = {zoneModel.ZoneBoundary.StartLeftY},
 StartRightX = {zoneModel.ZoneBoundary.StartRightX}, 
 StartRightY = {zoneModel.ZoneBoundary.StartRightY},
FinishLeftX = {zoneModel.ZoneBoundary.FinishLeftX}, 
FinishLeftY = {zoneModel.ZoneBoundary.FinishLeftY}, 
 FinishRightX = {zoneModel.ZoneBoundary.FinishRightX}, 
 FinishRightY = {zoneModel.ZoneBoundary.FinishRightY},  
{"}" }
{"},"}
                          ");
                        tsw.Close();
                        zoneModel = new HighwayZoneModel();
                    } 
                    else
                    {
                        dirCount++;
                    }
                }
            }
        }
    }
}
