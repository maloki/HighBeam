using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HighBeam.Main;
using static HighBeam.bus.BusControls;
using System.Diagnostics;
using System.Drawing;

namespace HighBeam.bus
{
    public static class BusRoute
    {
        public static BusRouteModel currentBusRoute = null;
        public static float lastDistToRoute = 9999999999;
        public static BusStopModel nextStop = new BusStopModel();
        public static BusStopModel currentStop = new BusStopModel();
        public static Stopwatch currentStopStopwatch = new Stopwatch();
        public static int stopIndex = 0;
        public static void RunRoute()
        {
            Stops();
            Display();
        }

        public static void Display()
        {
            var main = new UIContainer(new Point(5, UI.HEIGHT - 243), new Size(155, 50), System.Drawing.Color.FromArgb(80, 0, 0, 0));
            // main.Items.Add(new UIContainer(new Point(0, 24), new Size(155, 22), System.Drawing.Color.FromArgb(120, 59, 65, 81)));
            // route number
            var routeNumber = new UIContainer(new Point(3, 3), new Size(24, 20), System.Drawing.Color.FromArgb(160, 172, 0, 91));
            routeNumber.Items.Add(new UIText(currentBusRoute.Number, new Point(2, 0), 0.31f, Color.White, GTA.Font.ChaletLondon, false));
            main.Items.Add(routeNumber);
            // stops list 
            var stopsList = new UIContainer(new Point(38, 3), new Size(120, 43), System.Drawing.Color.FromArgb(0, 0, 0, 0));
            string stopType = "NEXT STOP";
            Color stopFontColor = Color.Silver;
            if (currentStop.Name == nextStop?.Name)
            {
                stopFontColor = Color.Green;
                stopType = "CURRENT STOP";
            }
            if (stopIndex == currentBusRoute.Stops.Count - 1)
            {
                stopType = "LAST STOP";
                stopFontColor = Color.OrangeRed;
            }
            stopsList.Items.Add(new UIText(stopType, new Point(2, -2), 0.15f, stopFontColor, GTA.Font.ChaletLondon, false));
            stopsList.Items.Add(new UIText((nextStop.Name.Length > 22 ? nextStop.Name.Substring(0, 22) + "..." : nextStop.Name), new Point(2, 5), 0.25f, Color.White, GTA.Font.ChaletLondon, false));
            // next stop 1
            var nextStop1 = stopIndex + 1 <= currentBusRoute.Stops.Count - 1 ? currentBusRoute.Stops[stopIndex + 1].Name : "";
            nextStop1 = nextStop1.Length > 28 ? nextStop1.Substring(0, 28) + "..." : nextStop1;
            stopsList.Items.Add(new UIText(nextStop1, new Point(2, 20), 0.18f, Color.Silver, GTA.Font.ChaletLondon, false));
            // next stop 2
            var nextStop2 = stopIndex + 2 <= currentBusRoute.Stops.Count - 1 ? currentBusRoute.Stops[stopIndex + 2].Name : "";
            nextStop2 = nextStop2.Length > 28 ? nextStop2.Substring(0, 28) + "..." : nextStop2;
            stopsList.Items.Add(new UIText(nextStop2, new Point(2, 30), 0.18f, Color.Silver, GTA.Font.ChaletLondon, false));
            // decoration line 
            stopsList.Items.Add(new UIContainer(new Point(-2, 0), new Size(1, 40), System.Drawing.Color.FromArgb(255, 172, 0, 91)));
            main.Items.Add(stopsList);
            //  new UIText(@"v", new Point(14, 28), 0.25f, airSuspensionMode == 1 ? System.Drawing.Color.OrangeRed : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true)
            main.Enabled = true;
            main.Draw();
        }

        public static void SetBusRoute(bool exit = false)
        {
            if (exit)
            {
                currentBusRoute = null;
                lastDistToRoute = 999999999;
            }
            else
            {
                var routes = BusRoutesList.Routes;
                routes.ForEach(r =>
                {
                    var dist = r.Stops[0].Position.DistanceTo(veh.Position);
                    if (dist < lastDistToRoute)
                    {
                        lastDistToRoute = dist;
                        currentBusRoute = r;
                    }
                });
                nextStop = currentBusRoute.Stops[0];
                stopIndex = 0;
                Function.Call(Hash.SET_NEW_WAYPOINT, nextStop.Position.X, nextStop.Position.Y);
            }
        }

        public static void Stops()
        {
            if (nextStop != null)
            {
                // UI.ShowSubtitle(nextStop.Position.DistanceTo(veh.Position).ToString());
            }
            if (nextStop.Position.DistanceTo(veh.Position) < 40f && (veh.Speed * 3.6) < 1 && !currentStopStopwatch.IsRunning)
            {
                currentStopStopwatch = new Stopwatch();
                currentStopStopwatch.Start();
                currentStop = nextStop;
            }
            if (currentStopStopwatch.ElapsedMilliseconds > 10000 && (veh.Speed * 3.6) > 10 && stopIndex != currentBusRoute.Stops.Count - 1)
            {
                currentStop = new BusStopModel();
                currentStopStopwatch = new Stopwatch();
                stopIndex += 1;
                if (stopIndex > currentBusRoute.Stops.Count - 1)
                {
                    SetBusRoute(exit: true);
                }
                else
                {
                    nextStop = currentBusRoute.Stops[stopIndex];
                    Function.Call(Hash.SET_NEW_WAYPOINT, nextStop.Position.X, nextStop.Position.Y);
                }
            }
        }
    }
}
