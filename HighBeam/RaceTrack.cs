using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public static class RaceTrack
    {
        private static int x = 0;
        private static int y = 0;
        private static int z = 0;
        private static Stopwatch LapTime = new Stopwatch();
        private static Stopwatch LastTime = new Stopwatch();
        private static Stopwatch LastTimeRender = new Stopwatch();
        private static Stopwatch BeforeNextMeasure = new Stopwatch();
        private static List<int> Times = new List<int>();
        private static bool CanNotifi = false;
        public static void RunRaceTrack()
        {
            var startLeft = new Vector2(1626, 3232);
            var startRight = new Vector2(1621, 3249);
            var finishLeft = new Vector2(1624, 3231);
            var finishRight = new Vector2(1619, 3249);
            UpdateCoords();
            var t1 = PointInTriangle(x, y, (int)startLeft.X, (int)startLeft.Y, (int)finishLeft.X, (int)finishLeft.Y, (int)startRight.X, (int)startRight.Y);
            var t2 = PointInTriangle(x, y, (int)finishLeft.X, (int)finishLeft.Y, (int)finishRight.X, (int)finishRight.Y, (int)startLeft.X, (int)startLeft.Y);
            var t3 = PointInTriangle(x, y, (int)finishRight.X, (int)finishRight.Y, (int)startRight.X, (int)startRight.Y, (int)finishLeft.X, (int)finishLeft.Y);
            var t4 = PointInTriangle(x, y, (int)startRight.X, (int)startRight.Y, (int)startLeft.X, (int)startLeft.Y, (int)finishRight.X, (int)finishRight.Y);
            if ((t1 || t2 || t3 || t4) && !BeforeNextMeasure.IsRunning)
            {
                if (LapTime.IsRunning)
                {
                    LapTime.Stop();
                    CanNotifi = true;
                    LastTime = LapTime;
                    LastTime.Stop();
                    LapTime = new Stopwatch();
                    LapTime.Start();
                    LastTimeRender.Start();
                }
                else
                {
                    LapTime.Start();
                }
                BeforeNextMeasure.Start();
            }
            if (LapTime.IsRunning)
            {
                var cont = new UIContainer(new Point(5, UI.HEIGHT - 250), new Size(100, 20), Color.FromArgb(80, 0, 0, 0));
                var s = Math.Round((double)(LapTime.ElapsedMilliseconds % 60000) / 1000);
                var m = Math.Floor((double)LapTime.ElapsedMilliseconds / 60000);
                var time = (m > 9 ? "" : "0") + m + ":" + (s > 9 ? "" : "0") + s + $":{Math.Abs(LapTime.ElapsedMilliseconds % 1000)}";
                cont.Items.Add(new UIText(time, new Point(40, 2), 0.3f, Color.White, GTA.Font.ChaletLondon, true));
                cont.Enabled = true;
                cont.Draw();
            }
            if (LastTimeRender.Elapsed.Seconds < 15 && LastTimeRender.IsRunning)
            {
                var cont = new UIContainer(new Point(5, UI.HEIGHT - 320), new Size(170, 30), Color.FromArgb(80, 0, 0, 0));
                var s = Math.Round((double)(LastTime.ElapsedMilliseconds % 60000) / 1000);
                var m = Math.Floor((double)LastTime.ElapsedMilliseconds / 60000);
                var time = (m > 9 ? "" : "0") + m + ":" + (s > 9 ? "" : "0") + s + $":{Math.Abs(LastTime.ElapsedMilliseconds % 1000)}";
                cont.Items.Add(new UIText("Last lap: " + time, new Point(80, 2), 0.4f, Color.White, GTA.Font.ChaletLondon, true));
                cont.Enabled = true;
                cont.Draw();
            }
            else
            {
                LastTimeRender = new Stopwatch();
            }
            if (BeforeNextMeasure.Elapsed.Seconds > 15)
            {
                BeforeNextMeasure = new Stopwatch();
            }
            if (CanNotifi)
            {
                if (Times.Count <= 0)
                {
                   UI.Notify("Best Lap");
                }
                foreach (var t in Times)
                {
                    if (LastTime.Elapsed.Milliseconds < t)
                    {
                        UI.Notify("Best Lap");
                        CanNotifi = false;
                        break;
                    }
                }
                Times.Add(LastTime.Elapsed.Milliseconds);
                CanNotifi = false;
            }
            if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown))
            {
                LapTime = new Stopwatch();
                LastTime = new Stopwatch();
                LastTimeRender = new Stopwatch();
                BeforeNextMeasure = new Stopwatch();
            }
        }

        private static double Sign(int p1x, int p1y, int p2x, int p2y, int p3x, int p3y)
        {
            return (p1x - p3x) * (p2y - p3y) - (p2x - p3x) * (p1y - p3y);
        }

        private static void UpdateCoords()
        {
            x = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).X, 0).ToString());
            y = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Y, 0).ToString());
            z = int.Parse(Math.Round((decimal)Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0)).Z, 0).ToString());
        }

        private static bool PointInTriangle(int pX, int pY, int v1x, int v1y, int v2x, int v2y, int v3x, int v3y)
        {
            bool b1;
            bool b2;
            bool b3;

            b1 = Sign(pX, pY, v1x, v1y, v2x, v2y) < 0.0;
            b2 = Sign(pX, pY, v2x, v2y, v3x, v3y) < 0.0;
            b3 = Sign(pX, pY, v3x, v3y, v1x, v1y) < 0.0;

            return ((b1 == b2) && (b2 == b3));
        }
    }
}
