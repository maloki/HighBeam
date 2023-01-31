using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using static HighBeam.HighwayTrafficOld;
using static HighBeam.NewHighwayTraffic.Index;
using static HighBeam.HighwayTraffic;
using static HighBeam.ZoneCreator;
using static HighBeam.RaceTrack;
using GTA.Math;
using HighBeam;
using static HighBeam.AutobahnPropStreamer;
using XInputDotNetPure;
using static HighBeam.Crane;
using System.Media;
using System.Windows.Media;
using System.IO;
using System.Reflection;
using static HighBeam.House;
using static HighBeam.bus.BusControls;
using static HighBeam.HybridEngine;
using static HighBeam.Uber;
using static HighBeam.Warehouse;
using static HighBeam.TrafficLight;
using static HighBeam.NyApartment;
using static HighBeam.NyWarehouse;
using static Memory.Mem;
using Memory;
using static HighBeam.ContainerWare;
using static HighBeam.Fuel;
using static HighBeam.Train;
using static HighBeam.NyShop;
using static HighBeam.ContainerHandler;
using static HighBeam.Terminal;
using static HighBeam.ElectricEngine;
using static HighBeam.Plane;
using static HighBeam.Rtg; 
using static HighBeam.Showroom;

namespace HighBeam
{
    public class Main : Script
    {
        // global variables
        public bool isPressed;
        public bool isHighBeamActive;
        public bool isHighBeamPressing;
        public int speedInKmh = 0;
        public static Vehicle veh;
        public bool isAddaptiveBrakeActive;
        public bool isDay;
        public bool addaptiveSequence;
        public bool addaptiveSequenceLightsOff;
        public Stopwatch stopwatch = new Stopwatch();
        public Stopwatch speedDiffStopWatch = new Stopwatch();
        public bool calculatingSpeedDiff;
        public int prevSpeed;
        public bool isLeftIndicatorOn;
        public bool isRightIndicatorOn;
        public bool isHazards = false;
        public Stopwatch vehicleExitingStopWatch = new Stopwatch();
        public bool isInVehicle;
        public bool hazardsAfterAddaptiveBraking;
        public static bool isCruiseControlActive;
        public float cruiseControlSpeed;
        public Stopwatch cruiseControlActivationStopWatch = new Stopwatch();
        public bool hazardsActive = false;
        public Stopwatch hazardsActivationStopWatch = new Stopwatch();
        public Stopwatch afterHazardsTurnedOffStopWatch = new Stopwatch();
        public bool canTurnOnAddaptiveLights;
        public bool isCarGripReducedByRain = false;
        public Stopwatch centralLockStopWatch = new Stopwatch();
        public Stopwatch centralLockHazardsStopWatch = new Stopwatch();
        public bool wasLightsOn = false;
        public bool isAquaplaningOn = false;
        public bool isWaitingForAquaplaning = false;
        public int timeBeforeNextAquaplaning = 999999;
        public Stopwatch waitingForAquaplaningStopWatch = new Stopwatch();
        public Stopwatch aquaplaningStopWatch = new Stopwatch();
        public int aquaplaningDuration = 9999999;
        public Stopwatch tailgateStopWatch = new Stopwatch();
        public bool toggleTailgate = false;
        public Stopwatch tailgateCentralLockHazardsStopWatch = new Stopwatch();
        public Stopwatch tailgateCentralLockStopWatch = new Stopwatch();
        public bool tailgateCentralLockHazardsOpen = false;
        public Stopwatch centralLockRemoteStopWatch = new Stopwatch();
        public bool centralLockProcedure = false;
        public bool centralLockToOpen = false;
        public Stopwatch centralLockCloseIfRemoteDidntStopWatch = new Stopwatch();
        public bool completeStop = false;
        public bool isBrakeLightsOn = false;
        public bool isCarInReverse = false;
        public bool canApplyReverse = false;
        public Stopwatch canApplyReverseStopWatch = new Stopwatch();
        public Stopwatch odometerStopWatch = new Stopwatch();
        public double odometer = 0;
        public bool markedCreated = false;
        public Vehicle markerPointCar;
        public bool isRefPointPlaced = false;
        public Vehicle refPoint;
        public float lightMeters = 0;
        public Stopwatch windStopWatch = new Stopwatch();
        public Stopwatch indicatorsAfterCrash = new Stopwatch();
        public bool isIndicatorsAfterCrash = false;
        public float lastSpeed;
        public int timeBeforeNextWind = 6000;
        public int windTicks = 0;
        public Stopwatch highBeamPressingStopWatch = new Stopwatch();
        public bool isPressingHighBeam = false;
        public Stopwatch HighSteeringAngleStopWatch = new Stopwatch();
        public Stopwatch AquaplaningStopWatch = new Stopwatch();
        public static bool isTruckMode = false;
        public Stopwatch truckModeStopWatch = new Stopwatch();
        public static Vehicle truckTrailer = null;
        public bool hasCreatedDriverForTruckTrailer = false;
        public int AudioId = 0;
        public Stopwatch windSoundStopWatch = new Stopwatch();
        public bool isAudioSet = false;
        public bool isAudioSet2 = false;
        public Stopwatch chillAccelerationSpeedCheck = new Stopwatch();
        public float chillAccelerationLastSpeed = 0f;
        public bool disableAcceleration = false;
        public bool isChillMode = false;
        public Stopwatch SpawnPropsAtNorthYanktonWarehouse = new Stopwatch();
        public bool StaticPropsSpawned = false;
        private static List<Vehicle> StaticVehicleList = new List<Vehicle>();
        private static List<Ped> StaticPedList = new List<Ped>();
        public Stopwatch passengerCarTrailerStopwatch = new Stopwatch();
        public bool isPassengerCarTrailer = false;
        public Vehicle passengerCarTrailer;
        public Stopwatch tailgateClose = new Stopwatch();
        public Stopwatch tailgateOpen = new Stopwatch();
        public Stopwatch flashHighBeamStopwatch = new Stopwatch();
        public Stopwatch waitForFlashHighBeamStopwatch = new Stopwatch();
        public int flashHighBeamTicks = 0;
        public bool canFlashHighBeam = false;
        public bool isFlashingHighBeam = false;
        public Vehicle highBeamSirene = null;
        public bool wasLightsOnHighBeam = false;
        public bool isCreep = false;
        public Stopwatch forceStopwatch = new Stopwatch();
        public bool canApplyForce = false;
        public Stopwatch grassSavePosStopWatch = new Stopwatch();
        public List<VecMod> grassPosArr = new List<VecMod>();
        public bool isAdditionalBrakeLightOn = false;
        public bool isCentralLockLocked = false;
        public bool isRaining = false;
        public float leftIndicatorLedProgress = 0f;
        public float rightIndicatorLedProgress = 0f;
        public Stopwatch ledIndicatorsStayStopwatch = new Stopwatch();
        public int airSuspensionMode = 2;
        public float airSuspensionForce = 0f;
        public bool airSuspensionToggleUp = true;
        public float airSuspensionDesiredForce = 0f;
        public bool adjustAirSuspension = false;
        public Stopwatch l1sw = new Stopwatch();
        public Stopwatch l2sw = new Stopwatch();
        public Stopwatch l3sw = new Stopwatch();
        public Stopwatch l4sw = new Stopwatch();
        public Stopwatch l5sw = new Stopwatch();
        public bool isNewHighBeamActive = false;
        public float lastSteeringAngle = 0;
        public float steeringAngle = 0;
        public float tailgateAngle = 0;
        public bool isReversing = false;
        public Vector3 lastVehPos = new Vector3(999999999999, 0, 0);
        public Vehicle fakePolice = null;
        public Ped fakePoliceDriver = null;
        public Stopwatch fakePoliceUpdateSpeed = new Stopwatch();
        public bool wereLightsBeforeHighbeam;
        public Stopwatch speed0Stopwatch = new Stopwatch();
        public Stopwatch descendStopwatch = new Stopwatch();
        public int lastPitch = 0;
        public bool isParkingGear = true;
        public Stopwatch parkingBrakeStopwatch = new Stopwatch();
        public bool isAquaplaningDirRight = false;
        public Stopwatch aquaplaningFirstPhaseStopwatch = new Stopwatch();
        public static int fakeSpeed = 0;
        public Stopwatch addaptiveCruiseControlStopWatch = new Stopwatch();
        public float addaptiveCarAheadSpeed = 0;
        public Vehicle addaptiveCarAhead = null;
        private int addaptiveSensorActive = 0;
        private Vector3 addaptiveRearPos;
        private Vector3 addaptiveOwnFrontPos;
        public Stopwatch addaptiveDisengagementStopWatch = new Stopwatch();
        public Stopwatch cruiseControlButtonsStopwatch = new Stopwatch();
        public bool canPressCruiseButton = true;
        public bool isEmergencyBraking = false;
        public Vehicle roofBike;
        public int speedZone = 0;
        public bool isSpeedZoneSet = false;
        public Stopwatch preventTrafficBehindCar = new Stopwatch();
        public int preventTrafficBehindZone = 0;
        public static Stopwatch nextTrafficJamStopwatch = new Stopwatch();
        public static int timeToNextTrafficJam = 10000;
        public static int trafficJamSpeedZoneId = 0;
        public static float trafficJamSpeed = 19;
        public static bool inits = false;
        public static float timeScale = 2f;
        public static float defaultTimeScale = 2f;
        public static Stopwatch timeScaleStopwatch = new Stopwatch();
        public static bool isClockPaused = false;
        public static bool isBoostMode = false;
        public int canTurnOnBoostStages = 0;
        public Stopwatch canTurnOnBoostStopwatch = new Stopwatch();
        public Stopwatch canTurnOffBoostStopwatch = new Stopwatch();
        public int canTurnOnParkingStages = 0;
        public Stopwatch canTurnOnParkingStopwatch = new Stopwatch();
        public Stopwatch canTurnOffParkingStopwatch = new Stopwatch();
        public Prop puddle = null;
        public Stopwatch hardSteerAtRain = new Stopwatch();
        public bool isParkingMode = false;
        private Random isEasyAquaRnd = new Random();
        private bool isEasyAqua = false;
        private DateTime sunriseTime = new DateTime(2020, 1, 1, 6, 0, 0);
        private DateTime sunsetTime = new DateTime(2020, 1, 1, 21, 48, 0);
        // private DateTime sunriseTime = new DateTime(2020, 1, 1, 4, 50, 0);
        //private DateTime sunsetTime = new DateTime(2020, 1, 1, 23, 50, 0);
        private DateTime inGameFixedSunrise = new DateTime(2020, 1, 1, 4, 0, 0);
        private DateTime inGameFixedSunset = new DateTime(2020, 1, 1, 1, 10, 0);
        public static int fakeTimeHours = 4;
        public static int fakeTimeMinutes = 59;
        public static int fakeTimeSeconds = 0;

        /* old config
          private DateTime sunriseTime = new DateTime(2020, 1, 1, 6, 0, 0);
        private DateTime sunsetTime = new DateTime(2020, 1, 1, 21, 48, 0);
        // private DateTime sunriseTime = new DateTime(2020, 1, 1, 4, 50, 0);
        //private DateTime sunsetTime = new DateTime(2020, 1, 1, 23, 50, 0);
        private DateTime inGameFixedSunrise = new DateTime(2020, 1, 1, 6, 0, 0);
        private DateTime inGameFixedSunset = new DateTime(2020, 1, 1, 1, 10, 0);
        public static int fakeTimeHours = 6;
        public static int fakeTimeMinutes = 30;
        public static int fakeTimeSeconds = 0;
         */

        private bool sunriseInit = false;
        private bool sunriseAtGameStart = false;
        public static Stopwatch progressTimeStopwatch = new Stopwatch();
        private bool fastForwardTime = false;
        public static Stopwatch toggleTruckModeStopwatch = new Stopwatch();
        public static Stopwatch autoHeadlightsStopwatch = new Stopwatch();
        public static bool wasDarkLastTime;
        public static bool hadHeadlightBeforeWasDark;
        public static bool isOnAutobahn = false;
        public static bool isEngineRunning = true;
        public static Stopwatch ingitionStopwatch = new Stopwatch();
        public static Stopwatch ingitionStartStopwatch = new Stopwatch();
        public static double lc0x100;
        public static double lc0x150;
        public static double lc0x200;
        public static double lc0x250;
        public static double lc0x300;
        public static double lc0x350;
        public static Stopwatch launchControlAccelStopwatch = new Stopwatch();
        public static bool isLaunchControl;
        public static int topSpeed;
        public static float tv = 0;
        public static bool creepBrakeReleased;
        public static int currentWeatherId = 0;
        public static bool isInLC = false;
        private int cruiseControlClusterBackgroundAlpha = 0;
        public static Prop truckTrailerContainer = null;
        private Stopwatch truckTrailerContainerMovingStopwatch = new Stopwatch();
        public static Vector3 containerPos;
        public static bool isContainerAttached = false;
        private Stopwatch espStopwatch = new Stopwatch();
        private bool isESPEnabled = false;
        private Vector3 espSpeedVector;
        private Stopwatch espClusterIconStopwatch = new Stopwatch();
        private Stopwatch espClusterRunningStopwatch = new Stopwatch();
        private Stopwatch tcsClusterIconStopwatch = new Stopwatch();
        private Stopwatch tcsClusterRunningStopwatch = new Stopwatch();
        private Stopwatch espToggleStopwatch = new Stopwatch();
        private bool isESPActive = true;
        private bool initEsp = false;
        private bool isTCSActive = true;
        private Stopwatch tcsCalcStopwatch = new Stopwatch();
        private float tcsLastSpeedDiff = 0f;
        private float tcsLastSpeed = 0f;
        private Stopwatch tcsWheelSlipStopwatch = new Stopwatch();
        private bool isTCSEnabled = false;
        private static Vector3 clearVehiclesFromNyPos = new Vector3(2484.1f, 886.2f, 90f);
        private Prop tempPlate = null;
        private Prop t4rearleft = null;
        private Prop t4rearright = null;
        private List<Prop> t4FrontLights = new List<Prop>();
        private bool isSireneMode = false;
        private Prop rrSirene = null;
        private Prop rlSirene = null;
        private int rrSireneAlpha = 0;
        private int rlSireneAlpha = 255;
        private bool rrSireneUp = true;
        private bool rlSireneUp = false;
        private Prop frSirene = null;
        private Prop flSirene = null;
        private int frSireneAlpha = 0;
        private int flSireneAlpha = 255;
        private bool frSireneUp = true;
        private bool flSireneUp = false;
        private Stopwatch sireneStopwatch = new Stopwatch();
        private bool isInPlane = false;
        private bool hasAirSuspension = false;
        public static bool isSportMode = false;
        private bool veryFirstInit = false;
        private Stopwatch veryFirstInitStopwatch = new Stopwatch();

        // asd
        public Main()
        {
            this.Tick += onTick;
            this.KeyUp += onKeyUp;
            this.KeyDown += onKeyDown;
        }

        public class VecMod
        {
            public Vector3 First { get; set; }
            public Vector3 Second { get; set; }
        }
        private Vector3 initPos = new Vector3(); 
        private void onTick(object sender, EventArgs e)
        {
            if (!veryFirstInitStopwatch.IsRunning)
                veryFirstInitStopwatch.Start();
            if (!veryFirstInit && veryFirstInitStopwatch.ElapsedMilliseconds > 700)
            {
                veryFirstInit = true;
                initPos = Game.Player.LastVehicle.Position;
            }
            if (veryFirstInit)
            {
                try
                {
                      //   UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
                    //Game.Player.Character.Position = new Vector3(4219.2f, -2367.1f, 19f);
                    // Game.Player.Character.FreezePosition = true;
                    var prevVeh = veh;
                    veh = Game.Player.LastVehicle;
                    isWRC = veh.DisplayName.ToLower().Contains("fiesta");
                    isInPlane = veh.ClassType == VehicleClass.Planes;

                    Function.Call((Hash)0x84FD40F56075E816, 0);

                    if ((veh.IsDamaged || veh.BodyHealth < 1000) && !isAqua)
                    {
                        veh.Repair();
                        PorscheWing(force: true);
                    }

                    if (veh?.DisplayName != prevVeh?.DisplayName)
                    {
                        // changed car 
                        isESPActive = true;
                        isTCSActive = true;
                        drivingMode = null;
                        isMatrixLedEnabled = false;
                        isPhevOnElectricEngine = false;
                        PSfrontHigh = false;
                        PSfrontLow = false;
                        PSfrontMiddle = false;
                        PSbackLow = false;
                        PSbackMiddle = false;
                        PSbackHigh = false;
                        ReadFuelFile();
                        if (isWash)
                        {
                            veh.DirtLevel = 12f;
                        }
                        dirtLevel = veh.DirtLevel;
                        hasAirSuspension = carsWithAirSuspension.Any(c => veh.DisplayName.ToLower().Contains(c));
                        hasActiveExhaust = activeExhausts.Any(exha => veh.DisplayName.ToLower().Contains(exha.VehName));
                        isActiveExhaustActivated = false;
                        isSportMode = false;
                        VwHubcaps();
                        DucatoFridge();

                        if (veh.DisplayName.Contains("oycre"))
                        {
                            veh.RoofState = VehicleRoofState.Closed;
                        }
                    }
                    if (!veh.DisplayName.ToLower().Contains("porcaygt"))
                        veh.DashboardColor = VehicleColor.EpsilonBlue; // for warehouse to not delete player cars
                    Weather();
                    TimeScale();

                    if (isTruckMode)
                    {
                        truckTrailer.CanBeVisiblyDamaged = false;
                    }
                    if (Game.Player.Character.IsInVehicle(veh))
                    {
                        isFuelPumpActive = false;
                        UpdateSpeed();
                        if ((isReversing && fakeSpeed < 10) || (isParkingMode))
                        {
                          
                        }
                        else
                        {
                            Steering();
                        }
                        if (speedInKmh <= 2 && !isParkingGear)
                        {
                            isBrakeLightsOn = true;
                            BrakeLightsAfterStop();
                            isAdditionalBrakeLightOn = true;
                        }
                        else
                        {
                            isBrakeLightsOn = false;
                            isAdditionalBrakeLightOn = false;
                            veh.InteriorLightOn = false;
                            if (isTruckMode)
                            {


                                // if (!veh.LightsOn)
                                //  truckTrailer.LightsOn = true;
                                if ((truckTrailer.DisplayName.Contains("TRAILE") || truckTrailer.DisplayName.ToLower().Contains("trans")) && veh.MaxBraking < 0.21f)
                                    truckTrailer.LightsOn = true;
                            }
                            else
                            {
                                // isChillMode = false;
                            }
                        }
                        if (!indicatorsAfterCrash.IsRunning)
                        {
                            indicatorsAfterCrash.Start();
                            lastSpeed = veh.Speed;
                        }
                        if (indicatorsAfterCrash.IsRunning && indicatorsAfterCrash.ElapsedMilliseconds > 100)
                        {
                            double diff = (lastSpeed * 3.6) - (veh.Speed * 3.6);
                            if (diff > 10 && !isCruiseControlActive)
                            {
                                veh.RightIndicatorLightOn = true;
                                veh.LeftIndicatorLightOn = true;
                                isLeftIndicatorOn = true;
                                isRightIndicatorOn = true;
                                if (fakeSpeed < 10)
                                {
                                    isCruiseControlActive = false;
                                }
                            }
                            indicatorsAfterCrash = new Stopwatch();
                        }
                        if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon) && isHighBeamPressing)
                        {
                            isHighBeamPressing = false;
                            veh.SirenActive = false;
                        }
                        CleanCar();

                        ExitingVehicle();
                        AddaptiveBrakeLights();
                        Creep();
                        Wind();
                        HandbrakeAfterCompleteStop();
                        // Trailer();
                        HighBeam.Plane.RunPlane();
                        //  ChillAcceleration();
                        RainSpray();
                        HighBeamLightsNew();
                        AutoHeadlightsWhenDark();
                        SetVehicleLightsOnHour();
                        CruiseControl();
                        Gears();
                        Hybrid();
                        Forklift();
                        KickDown();
                        TCS();
                        ESP();
                        CementMixer();
                        Ingition();
                        LaunchControl();
                        Brakes();
                        ActiveExhaust();
                        SuperCarAcceleration();
                        Hood();
                        DrivingModes();
                        ElEngine();
                        //   RoadSurfaceVibrations();
                        ParkingSensors();
                        Fireflies();
                        //   if (!veh.DisplayName.ToLower().Contains("exped"))
                        //    RunUber();
                        if (isReversing && fakeSpeed > 5)
                        {

                            veh.ApplyForceRelative(new Vector3(0, 0.3f, 0f));
                        }

                    }
                    else
                    {

                        if (!isInPlane)
                        {
                            CentralLockRemote();
                            CentralLockCloseIfRemoteDidnt();
                            isParkingGear = true;
                            parkingSensorSoundLow.Stop();
                            parkingSensorSoundMiddle.Stop();
                            parkingSensorSoundHigh.Stop();
                        }
                        PlayersClock();
                        espToggleStopwatch = new Stopwatch();
                        FuelStation();
                    }
                    if (tailgateCentralLockStopWatch.IsRunning || tailgateCentralLockHazardsStopWatch.IsRunning)
                    {
                        TailgateCentralLock();
                    }
                    TruckMode();
                    // always listening, because functions can be called from outside of vehicle 
                    Tailgate();
                    Indicators();
                    AdditionalLights();
                    LiveTraffic();
                    //  AirSuspension();
                    SireneMode();
                    RunFuel();
                    // for debug 
                    //HighwayTrafficOld.RunHighwayTraffic();
                    // HighwayTraffic.RunTestTraffic();
                    Odometer();
                    RunHouse();
                    AirSuspension();
                    RunNyAparment();
                    RenderS7();
                    RunBusControls();
                   //   RunWarehouse();
                    //    RunNyWarehouse();
                    RunNyShop();
                    RunContainerWarehouse();
                    RunTrafficLightSystem();
                    RunTrain();
                    RunContainerHandler();
                    // ClearVehiclesFromNyBug();
                    //  RoofRack();
                    //   RunZoneCreator();  
                    //  RunRaceTrack();
                    // RunHighwayTraffic();
                    RunAutobahnPropStreamer();
                    PreventTrafficBehindCar();
                    if (isPhevVehicle)
                        ChargerCord();
                    if (isEvVehicle)
                        ChargerCordEv();
                    // LibertyCity();
                    //  RunCrane();
                    //  RunRtg();
                    DashCam();
                    RunShowroom();
                   // Dyno();
                    //  RunTerminal();
                    //LibertyCityPedControl();
                    // veh.LightsOn = false;

                }
                catch (Exception ex)
                {
                    UI.ShowSubtitle(ex.Message.Substring(0, 100));
                }
            }
        }



        private void onKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {

            /*  if (e.KeyCode == Keys.NumPad0)
              {
                  Vehicle vehicle = World.CreateVehicle(VehicleHash.Adder, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f, Game.Player.Character.Heading + 90);
                  vehicle.CanTiresBurst = false;
                  vehicle.CustomPrimaryColor = Color.FromArgb(38, 38, 38);
                  vehicle.CustomSecondaryColor = Color.DarkOrange;
                  vehicle.PlaceOnGround();
                  vehicle.NumberPlate = "SHVDN";
              }*/
        }


        // functions
        private Prop dyno = null;
        private void Dyno()
        {
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleSelectNextWeapon))
            {
                dyno = World.CreateProp(new Model(600369530), veh.Position, false, true);
                dyno.Heading = veh.Heading - 90;
            }
            if (dyno != null)

            {

                Function.Call(Hash.APPLY_FORCE_TO_ENTITY, dyno, 3, 0f, 0f, 10.0f, 0f,0f,0f, 0, false, true, true, false, true);

               
            }
        }


        private Camera carCam = null;
        private Camera cinCarCam = null;
        private Vehicle cinCarCamVeh = null;
        private bool canEnableCinCam = false;
        private void DashCam()
        {
            Game.DisableControlThisFrame(0, GTA.Control.VehicleCinCam);
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleCinCam) && Game.Player.Character.IsInVehicle())
            {
                if (canEnableCinCam)
                {
                    canEnableCinCam = false;
                    World.RenderingCamera = null;
                    cinCarCam = null;
                    cinCarCamVeh = null;
                    World.DestroyAllCameras();
                }
                else
                {
                    var vehs = World.GetAllVehicles();
                    var dist = 100f;
                    for (var i = 0; i < vehs.Length; i++)
                    {
                        var curVeh = vehs[i];
                        if (curVeh.Position.DistanceTo(veh.Position) < dist && curVeh.DisplayName != veh.DisplayName
                            && curVeh.ClassType != VehicleClass.Boats
                            && curVeh.ClassType != VehicleClass.Motorcycles
                            && curVeh.ClassType != VehicleClass.Trains
                            && curVeh.ClassType != VehicleClass.Planes
                            && !curVeh.DisplayName.ToLower().Contains("trailer"))
                        {
                            dist = curVeh.Position.DistanceTo(veh.Position);
                            cinCarCamVeh = curVeh;
                        }
                    }
                    if (cinCarCamVeh?.DisplayName != null)
                    {
                        canEnableCinCam = true;
                    }
                }

            }

            if ((!Game.Player.Character.IsInVehicle() && cinCarCam != null) || cinCarCam?.Position.DistanceTo(Game.Player.Character.Position) > 120f)
            {
                World.RenderingCamera = null;
                cinCarCam = null;
                cinCarCamVeh = null;
                canEnableCinCam = false;
                World.DestroyAllCameras();
            }
            else
            {
                if (cinCarCam == null && canEnableCinCam)
                {
                    cinCarCam = World.CreateCamera(cinCarCamVeh.Position, cinCarCamVeh.Rotation, 52f);
                    var windshield = cinCarCamVeh.GetOffsetFromWorldCoords(cinCarCamVeh.GetBoneCoord("windscreen"));
                    cinCarCam.AttachTo(cinCarCamVeh, new Vector3(windshield.X, windshield.Y, windshield.Z + 0.2f));
                    World.RenderingCamera = cinCarCam;
                }

                if (cinCarCam != null)
                {

                    cinCarCam.PointAt(cinCarCamVeh.GetOffsetInWorldCoords(new Vector3(0, 5f, 1f)));
                }
            }

            if (!Game.Player.Character.IsInVehicle() && carCam != null)
            {
                World.RenderingCamera = null;
                carCam = null;
                World.DestroyAllCameras();
            }
            else
            {
                var n = veh.DisplayName.ToLower();
                if (Function.Call<int>((Hash)0xEE778F8C7E1142E2, Function.Call<int>((Hash)0x19CAFA3C87F7C2FF)) == 4 && carCam == null)
                {
                    carCam = World.CreateCamera(veh.Position, veh.Rotation, 60f);
                    var windshield = veh.GetOffsetFromWorldCoords(veh.GetBoneCoord("windscreen"));
                    carCam.AttachTo(veh, new Vector3(windshield.X, windshield.Y, windshield.Z + 0.2f));

                    if (n.Contains("m4"))
                    {
                        carCam.AttachTo(veh, new Vector3(windshield.X, windshield.Y + 0.5f, windshield.Z + 0.7f));

                    }
                    if (n.Contains("vwcaddy"))
                    {
                        carCam.AttachTo(veh, new Vector3(windshield.X, windshield.Y - 0.6f, windshield.Z + 0.1f));
                    }
                    if (n.Contains("t680"))
                    {
                        carCam.AttachTo(veh, new Vector3(windshield.X, windshield.Y, windshield.Z + 0f));
                    }
                    if (n.Contains("promast"))
                    {
                        carCam.AttachTo(veh, new Vector3(windshield.X - 0f, windshield.Y - 0.5f, windshield.Z + 0.2f));
                        carCam.FieldOfView = 66f;
                    }
                    if (n.Contains("expedi"))
                    {
                        carCam.AttachTo(veh, new Vector3(windshield.X - 0f, windshield.Y - 1.0f, windshield.Z + 1f));
                        carCam.FieldOfView = 66f;
                    }
                    World.RenderingCamera = carCam;
                }

                if (Function.Call<int>((Hash)0xEE778F8C7E1142E2, Function.Call<int>((Hash)0x19CAFA3C87F7C2FF)) != 4 && carCam != null)
                {
                    World.RenderingCamera = null;
                    carCam = null;
                    World.DestroyAllCameras();
                }

                if (carCam != null)
                {
                    if (n.Contains("vwcaddy"))
                    {
                        carCam.PointAt(veh.GetOffsetInWorldCoords(new Vector3(0, 5f, 1.6f)));
                    }
                    else if (n.Contains("promast"))
                    {
                        carCam.PointAt(veh.GetOffsetInWorldCoords(new Vector3(-0f, 5f, 0.8f)));
                    }
                    else if (n.Contains("expedi"))
                    {
                        carCam.PointAt(veh.GetOffsetInWorldCoords(new Vector3(0, 10f, 1.1f)));
                    }
                    else
                    {
                        carCam.PointAt(veh.GetOffsetInWorldCoords(new Vector3(0, 5f, 1f)));
                    }

                }
            }
        }

        private Prop fridge;
        private bool isFridgeAttached = false;
        private void DucatoFridge()
        {
            if (!isFridgeAttached && veh.DisplayName.ToLower().Contains("promast"))
            {
                isFridgeAttached = true;

                var prop = World.CreateProp(new Model(-759879321), new Vector3(), false, false);
                prop.AttachTo(veh, 0, new Vector3(-0.04f, -0.2f, 1.0f), new Vector3(270, 0, 90));
                fridge = prop;
            }
            else if (!veh.DisplayName.ToLower().Contains("promast") && isFridgeAttached)
            {
                isFridgeAttached = false;
                fridge.Delete();
            }

        }

        private List<Prop> vwCaps = new List<Prop>();
        private List<string> vwBones = new List<string>() { "wheel_lf", "wheel_rf", "wheel_lr", "wheel_rr" };
        private bool areCapsAttached = false;
        private void VwHubcaps()
        {
            if (!areCapsAttached && veh.DisplayName.ToLower().Contains("pstgte"))
            {
                areCapsAttached = true;
                foreach (var bone in vwBones)
                {
                    Vector3 offset = new Vector3();
                    Vector3 rotation = new Vector3();
                    if (bone == "wheel_lf")
                    {
                        offset = new Vector3(0.2f, 0, 0);
                        rotation = new Vector3(0f, 90f, 180f);
                    }
                    if (bone == "wheel_rf")
                    {
                        offset = new Vector3(0.2f, 0, 0);
                        rotation = new Vector3(0f, 90f, 180f);
                    }
                    if (bone == "wheel_lr")
                    {
                        offset = new Vector3(0.2f, 0, 0);
                        rotation = new Vector3(0f, 90f, 180f);
                    }
                    if (bone == "wheel_rr")
                    {
                        offset = new Vector3(0.2f, 0, 0);
                        rotation = new Vector3(0f, 90f, 180f);
                    }
                    var prop = World.CreateProp(new Model(1173831889), new Vector3(), false, false);
                    prop.AttachTo(veh, veh.GetBoneIndex(bone), offset, rotation);
                    vwCaps.Add(prop);
                }
            }
            if (!veh.DisplayName.ToLower().Contains("pstgte") && areCapsAttached)
            {
                areCapsAttached = false;
                vwCaps.ForEach(cap => cap.Delete());
                vwCaps = new List<Prop>();
            }
        }

        private List<Vector3> electricPoints = new List<Vector3>();
        private void Electrician()
        {
            Game.DisableControlThisFrame(0, GTA.Control.Attack);
            if (Game.IsDisabledControlJustPressed(0, GTA.Control.Attack))
            {
                var pos = Game.Player.Character.Position;
                electricPoints.Add(new Vector3(pos.X, pos.Y, World.GetGroundHeight(pos) + 0.03f));
            }

            for (var i = 0; i < electricPoints.Count; i++)
            {
                var start = electricPoints[i];
                var end = electricPoints[(i + 1 > electricPoints.Count - 1 ? i : (i + 1))];
                Function.Call((Hash)0x6B7256074AE34680, start.X, start.Y, start.Z, end.X, end.Y, (end.Z), 255, 5, 5, 255);
            }
        }

        private class FireflyModel
        {
            public Vector3 Position { get; set; }
            public float Heading { get; set; }
        };

        private List<FireflyModel> firefliesList = new List<FireflyModel>();
        private Random xRnd = new Random();
        private Random zRnd = new Random();
        private Stopwatch fireflySpawnStopwatch = new Stopwatch();
        private Random headingRnd = new Random();
        private Random spawnRnd = new Random();
        private Random alphaRnd = new Random();
        private Random minusRnd = new Random();
        private int fireflySpawnTime = 1000;
        private void Fireflies()
        {
            var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
            if ((h > 18 || h < 6))
            {
                if (!fireflySpawnStopwatch.IsRunning)
                    fireflySpawnStopwatch.Start();
                if (fireflySpawnStopwatch.ElapsedMilliseconds > fireflySpawnTime)
                {
                    fireflySpawnStopwatch = new Stopwatch();
                    fireflySpawnTime = spawnRnd.Next(50, 4000);
                    if (fireflySpawnTime < 50)
                    {
                        fireflySpawnTime = 50;
                    }
                    var x = xRnd.Next(0, 7);
                    var z = veh.HighBeamsOn ? zRnd.Next(0, 40) : zRnd.Next(0, 10);
                    var minus = minusRnd.Next(1, 10);
                    firefliesList.Add(new FireflyModel() { Heading = headingRnd.Next(0, 360), Position = veh.GetOffsetInWorldCoords(new Vector3(((minus > 5 ? -1 : 1) * x), 29f, (z / 10))) });
                }
                //     UI.ShowSubtitle(((veh.Heading + 270) % 360));
                var frontModelSize = veh.Model.GetDimensions();
                frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, frontModelSize.Z / 2);
                var front = GetEntityOffset(veh, new Vector3(0, frontModelSize.Y, -frontModelSize.Z + 0.5f));

                for (var i = 0; i < firefliesList.Count; i++)
                {
                    var firefly = firefliesList[i];
                    if (firefly.Position.DistanceTo(veh.Position) > 40 || firefly.Position.DistanceTo(front) < 5f)
                    {
                        firefliesList.RemoveAt(i);
                    }
                    else
                    {
                        var x = (float)Math.Cos((Math.PI / 180) * firefly.Heading);
                        var alpha = alphaRnd.Next(150, 255);
                        if (alpha > 255)
                            alpha = 255;
                        var y = (float)Math.Sin((Math.PI / 180) * firefly.Heading);
                        var speed = 0.06f;
                        var mov = new Vector2(x, y) * speed;
                        var p = firefly.Position;
                        firefly.Position = new Vector3(p.X + mov.X, p.Y + mov.Y, p.Z);
                        Function.Call((Hash)0x6B7256074AE34680, firefly.Position.X, firefly.Position.Y, firefly.Position.Z, firefly.Position.X, firefly.Position.Y, (firefly.Position.Z + 0.03f), 255, 255, 255, alpha);
                    }
                }
            }
            else
            {
                if (firefliesList.Count > 0)
                {
                    firefliesList = new List<FireflyModel>();
                }
            }
        }




        private bool PSfrontLow;
        private bool PSfrontMiddle;
        private bool PSfrontHigh;

        private bool PSbackLow;
        private bool PSbackMiddle;
        private bool PSbackHigh;

        private MediaPlayer parkingSensorSoundLow = new MediaPlayer();
        private MediaPlayer parkingSensorSoundMiddle = new MediaPlayer();
        private MediaPlayer parkingSensorSoundHigh = new MediaPlayer();
        private bool parkingSensorSoundInit = false;
        private Stopwatch parkingSensorSoundStopwatch = new Stopwatch();

        private Stopwatch parkingSensorCalcStopwatch = new Stopwatch();
        private void ParkingSensors()
        {
            if (!parkingSensorCalcStopwatch.IsRunning)
                parkingSensorCalcStopwatch.Start();
            if (parkingSensorCalcStopwatch.ElapsedMilliseconds > 250)
            {
                parkingSensorCalcStopwatch = new Stopwatch();
            }

            if (isParkingGear)
            {
                PSfrontHigh = false;
                PSfrontLow = false;
                PSfrontMiddle = false;
                PSbackLow = false;
                PSbackMiddle = false;
                PSbackHigh = false;
            }

            if ((isParkingMode) && !parkingSensorCalcStopwatch.IsRunning && !isParkingGear)
            {

                var frontModelSize = veh.Model.GetDimensions();
                frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, frontModelSize.Z / 2);
                // var addaptiveCenterPoint = GetEntityOffset(veh, new Vector3(-0.0f, -frontModelSize.Y, -frontModelSize.Z * 0));
                //  Function.Call((Hash)0x6B7256074AE34680, addaptiveCenterPoint.X, addaptiveCenterPoint.Y, addaptiveCenterPoint.Z, addaptiveCenterPoint.X, addaptiveCenterPoint.Y, (addaptiveCenterPoint.Z + 1f), 255, 5, 5, 255);



                float frontSensorXOffset = -1f;
                float frontSensorYOffset = 0.4f;
                PSfrontHigh = false;
                PSfrontLow = false;
                PSfrontMiddle = false;
                PSbackLow = false;
                PSbackMiddle = false;
                PSbackHigh = false;

                for (var i = 0; i < 10; i++)
                {
                    var sensorPos = GetEntityOffset(veh, new Vector3(frontSensorXOffset, frontModelSize.Y, -frontModelSize.Z + 0.5f));
                    // Function.Call((Hash)0x6B7256074AE34680, sensorPos.X, sensorPos.Y, sensorPos.Z, sensorPos.X, sensorPos.Y, (sensorPos.Z + 0.3f), 255, 5, 255, 255);

                    for (var ii = 0; ii < 3; ii++)
                    {

                        var sensorPosY = GetEntityOffset(veh, new Vector3(frontSensorXOffset, frontModelSize.Y + frontSensorYOffset, -frontModelSize.Z + 0.5f));
                        var raycast = World.Raycast(sensorPos, sensorPosY, IntersectOptions.Everything);

                        var sensorPosYHigh = GetEntityOffset(veh, new Vector3(frontSensorXOffset, frontModelSize.Y + frontSensorYOffset, -frontModelSize.Z + 0.8f));
                        var raycastHigh = World.Raycast(sensorPos, sensorPosYHigh, IntersectOptions.Everything);

                        if (ii == 0 && !PSfrontHigh)
                            PSfrontHigh = raycast.DitHitAnything || raycastHigh.DitHitAnything;
                        if (ii == 1 && !PSfrontMiddle)
                            PSfrontMiddle = raycast.DitHitAnything || raycastHigh.DitHitAnything;
                        if (ii == 2 && !PSfrontLow)
                            PSfrontLow = raycast.DitHitAnything || raycastHigh.DitHitAnything;


                        /*  if (raycast.DitHitAnything || raycastHigh.DitHitAnything)
                              Function.Call((Hash)0x6B7256074AE34680, sensorPosYHigh.X, sensorPosYHigh.Y, sensorPosYHigh.Z, sensorPosYHigh.X, sensorPosYHigh.Y, (sensorPosYHigh.Z + 0.3f), 255, 5, 5, 255);
                          else
                              Function.Call((Hash)0x6B7256074AE34680, sensorPosYHigh.X, sensorPosYHigh.Y, sensorPosYHigh.Z, sensorPosYHigh.X, sensorPosYHigh.Y, (sensorPosYHigh.Z + 0.3f), 255, 255, 255, 255);
                         */
                        frontSensorYOffset += 0.4f;
                    }
                    frontSensorYOffset = 0.4f;
                    frontSensorXOffset += 0.2f;
                }

                frontSensorXOffset = -1f;
                frontSensorYOffset = -0.4f;
                PSbackHigh = false;
                PSbackMiddle = false;
                PSbackLow = false;

                for (var i = 0; i < 10; i++)
                {
                    var sensorPos = GetEntityOffset(veh, new Vector3(frontSensorXOffset, -frontModelSize.Y, -frontModelSize.Z + 0.5f));
                    // Function.Call((Hash)0x6B7256074AE34680, sensorPos.X, sensorPos.Y, sensorPos.Z, sensorPos.X, sensorPos.Y, (sensorPos.Z + 0.3f), 255, 5, 255, 255);

                    for (var ii = 0; ii < 3; ii++)
                    {

                        var sensorPosY = GetEntityOffset(veh, new Vector3(frontSensorXOffset, -frontModelSize.Y + frontSensorYOffset, -frontModelSize.Z + 0.5f));
                        var raycast = World.Raycast(sensorPos, sensorPosY, IntersectOptions.Everything, veh);

                        var sensorPosYHigh = GetEntityOffset(veh, new Vector3(frontSensorXOffset, -frontModelSize.Y + frontSensorYOffset, -frontModelSize.Z + 0.8f));
                        var raycastHigh = World.Raycast(sensorPos, sensorPosYHigh, IntersectOptions.Everything, veh);

                        if (ii == 0 && !PSbackHigh)
                            PSbackHigh = raycast.DitHitAnything || raycastHigh.DitHitAnything;
                        if (ii == 1 && !PSbackMiddle)
                            PSbackMiddle = raycast.DitHitAnything || raycastHigh.DitHitAnything;
                        if (ii == 2 && !PSbackLow)
                            PSbackLow = raycast.DitHitAnything || raycastHigh.DitHitAnything;


                        /*  if (raycast.DitHitAnything)
                              Function.Call((Hash)0x6B7256074AE34680, sensorPosYHigh.X, sensorPosYHigh.Y, sensorPosYHigh.Z, sensorPosYHigh.X, sensorPosYHigh.Y, (sensorPosYHigh.Z + 0.3f), 255, 5, 5, 255);
                          else
                              Function.Call((Hash)0x6B7256074AE34680, sensorPosYHigh.X, sensorPosYHigh.Y, sensorPosYHigh.Z, sensorPosYHigh.X, sensorPosYHigh.Y, (sensorPosYHigh.Z + 0.3f), 255, 255, 255, 255);
                        */
                        frontSensorYOffset -= 0.4f;
                    }
                    frontSensorYOffset = -0.4f;
                    frontSensorXOffset += 0.2f;
                }
            }
            if (!parkingSensorSoundInit)
            {
                parkingSensorSoundLow = new MediaPlayer();
                parkingSensorSoundMiddle = new MediaPlayer();
                parkingSensorSoundHigh = new MediaPlayer();

                parkingSensorSoundLow.Open(new Uri(new System.Uri(Path.GetFullPath($"./scripts/chatter_sounds/parking-sensor-low.wav")).AbsoluteUri));
                parkingSensorSoundMiddle.Open(new Uri(new System.Uri(Path.GetFullPath($"./scripts/chatter_sounds/parking-sensor-middle.wav")).AbsoluteUri));
                parkingSensorSoundHigh.Open(new Uri(new System.Uri(Path.GetFullPath($"./scripts/chatter_sounds/parking-sensor-high.wav")).AbsoluteUri));

                parkingSensorSoundLow.Volume = 0.4;
                parkingSensorSoundMiddle.Volume = 0.4;
                parkingSensorSoundHigh.Volume = 0.4;
                parkingSensorSoundInit = true;
            }
            var anyPsensor = PSbackLow || PSbackMiddle || PSbackHigh || PSfrontHigh || PSfrontMiddle || PSfrontLow;
            if (!anyPsensor && drivingMode == "parking")
            {
            }
            if (anyPsensor && fakeSpeed < 9)
            {
                drivingMode = "parking";
                isParkingMode = true;
                var soundTime = 500;
                var soundName = "low";
                if (!parkingSensorSoundStopwatch.IsRunning)
                    parkingSensorSoundStopwatch.Start();
                if (PSbackMiddle || PSfrontMiddle)
                {
                    soundName = "middle";
                    soundTime = 200;
                }

                if (PSbackHigh || PSfrontHigh)
                {
                    soundName = "high";
                    soundTime = 80;
                }

                if (true)
                {
                    if (soundName == "low")
                    {
                        parkingSensorSoundHigh.Stop();
                        parkingSensorSoundMiddle.Stop();
                        parkingSensorSoundLow.Play();
                    }
                    if (soundName == "middle")
                    {
                        parkingSensorSoundHigh.Stop();
                        parkingSensorSoundLow.Stop();
                        parkingSensorSoundMiddle.Play();
                    }
                    if (soundName == "high")
                    {
                        parkingSensorSoundLow.Stop();
                        parkingSensorSoundMiddle.Stop();
                        parkingSensorSoundHigh.Play();
                    }

                    parkingSensorSoundStopwatch = new Stopwatch();
                }
            }
            else
            {
                try
                {
                    if (!parkingSensorSoundHigh.IsBuffering && !parkingSensorSoundMiddle.IsBuffering && !parkingSensorSoundLow.IsBuffering)
                    {
                        parkingSensorSoundLow?.Stop();
                        parkingSensorSoundMiddle?.Stop();
                        parkingSensorSoundHigh?.Stop();
                        parkingSensorSoundHigh.Position = TimeSpan.Zero;
                        parkingSensorSoundMiddle.Position = TimeSpan.Zero;
                        parkingSensorSoundLow.Position = TimeSpan.Zero;
                    }
                }
                catch (Exception e)
                {
                    UI.ShowSubtitle(e.Message);
                };
            }
        }

        private static Stopwatch surfaceStopwatch = new Stopwatch();
        private static Stopwatch surfaceBetweenStopwatch = new Stopwatch();
        private static int timeBetweenVibration = 0;
        private static int timeInVibration = 0;
        private static Random sfrRnd = new Random();
        private static Random sfrInRnd = new Random();

        private static Stopwatch hardHitStopwatch = new Stopwatch();

        private static int patchLimit = 8;
        private static int currentPatch = 0;
        private void RoadSurfaceVibrations()
        {
            if (fakeSpeed > 92 && false)
            {
                if (!surfaceBetweenStopwatch.IsRunning)
                {
                    surfaceBetweenStopwatch.Start();
                    timeBetweenVibration = sfrInRnd.Next(4000, 5000);
                }
                if (surfaceBetweenStopwatch.ElapsedMilliseconds > timeBetweenVibration)
                {
                    UI.ShowSubtitle("hitttin");
                    if (timeInVibration == 0)
                    {
                        timeInVibration = sfrInRnd.Next(100, 100);
                    }

                    if (surfaceStopwatch.ElapsedMilliseconds % (400 - fakeSpeed) == 0)
                    {

                        GamePad.SetVibration(PlayerIndex.One, 0.22f, 0.22f);

                    }
                    else
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                    }

                    if (!surfaceStopwatch.IsRunning)
                    {
                        surfaceStopwatch.Start();
                    }
                    if (surfaceStopwatch.ElapsedMilliseconds > 5000)
                    {
                        currentPatch = 0;
                        timeInVibration = 0;
                        surfaceBetweenStopwatch = new Stopwatch();
                        surfaceStopwatch = new Stopwatch();
                    }
                }
            }
            else
            {
                surfaceStopwatch = new Stopwatch();
                surfaceBetweenStopwatch = new Stopwatch();
                currentPatch = 0;
                timeInVibration = 0;
            }


            if (descendStopwatch.IsRunning && descendStopwatch.ElapsedMilliseconds > 500)
            {
                lastPitch = (int)Math.Round(veh.ForwardVector.Z * 100, 0);
                descendStopwatch = new Stopwatch();
            }
            if (!descendStopwatch.IsRunning)
            {
                descendStopwatch.Start();
            }
            int pitchDiff = lastPitch - (int)Math.Round(veh.ForwardVector.Z * 100, 0);
            if ((pitchDiff < -4 || pitchDiff > 4) && !hardHitStopwatch.IsRunning && false)
            {
                hardHitStopwatch.Start();
            }

            if (hardHitStopwatch.ElapsedMilliseconds > 200)
            {
                hardHitStopwatch = new Stopwatch();
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);

            }
            if (hardHitStopwatch.ElapsedMilliseconds < 200 && hardHitStopwatch.IsRunning)
            {
                GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
            }
        }

        public static string drivingMode = null;

        private void DrivingModes()
        {
            if (fakeSpeed > 14)
            {
                Game.DisableControlThisFrame(0, GTA.Control.VehicleHandbrake);

                if (Game.IsControlJustPressed(0, GTA.Control.VehicleHandbrake))
                {
                    if (isPhevVehicle)
                    {
                        if (drivingMode == "sport")
                            drivingMode = null;
                        else if (drivingMode == null)
                            drivingMode = "ev";
                        else if (drivingMode == "ev")
                            drivingMode = "sport";
                    }
                    else if (isEvVehicle)
                    {
                        if (drivingMode == "sport")
                            drivingMode = null;

                        else if (drivingMode == null)
                            drivingMode = "boost";
                        else if (drivingMode == "boost")
                            drivingMode = "sport";
                    }
                    else
                    {
                        if (drivingMode == "sport")
                            drivingMode = null;
                        else if (drivingMode == null)
                            drivingMode = "eco";
                        else if (drivingMode == "eco")
                            drivingMode = "sport";
                    }
                }

                if (drivingMode == "eco")
                {
                    if (fakeSpeed > 27)
                    {
                        veh.EngineTorqueMultiplier = 0.48f;
                    }
                    if (fakeSpeed > 140)
                    {
                        veh.EngineTorqueMultiplier = 0.0f;
                    }
                }
            }
            if (fakeSpeed > 2 && fakeSpeed < 13)
            {
                Game.DisableControlThisFrame(0, GTA.Control.VehicleHandbrake);
                if (Game.IsControlJustPressed(0, GTA.Control.VehicleHandbrake))
                {
                    drivingMode = drivingMode == "parking" ? null : "parking";
                    isParkingMode = drivingMode == "parking";
                }
            }

            if (drivingMode == "parking")
            {
                veh.EnginePowerMultiplier = 0.2f;
                veh.EngineTorqueMultiplier = 0.2f;
            }
        }

        private Stopwatch hoodToggleStopwatch = new Stopwatch();

        private void Hood()
        {
            if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && !hoodToggleStopwatch.IsRunning)
            {
                hoodToggleStopwatch.Start();
            }
            if (hoodToggleStopwatch.ElapsedMilliseconds > 1600)
            {
                hoodToggleStopwatch = new Stopwatch();

                if (veh.IsDoorOpen(VehicleDoor.Hood))
                    veh.CloseDoor(VehicleDoor.Hood, false);
                else
                    veh.OpenDoor(VehicleDoor.Hood, false, false);
            }
            if (Game.IsControlJustReleased(0, GTA.Control.ScriptPadDown) && hoodToggleStopwatch.IsRunning)
                hoodToggleStopwatch = new Stopwatch();
        }

        private void SuperCarAcceleration()
        {
            //  UI.ShowSubtitle(veh.DisplayName);
            if (veh.DisplayName.Contains("720"))
            {
                if (fakeSpeed < 99 && GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.9f)
                {
                    veh.ApplyForceRelative(new Vector3(0, 0.048f, 0));
                }
            }
            if ((veh.DisplayName.Contains("812") || veh.DisplayName.Contains("pts21")) && GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.9f)
            {

                decimal b = (decimal)((decimal)fakeSpeed / 10000);
                var f = (0.035f - (float)b);
                if (fakeSpeed < 352)
                {
                    veh.ApplyForceRelative(new Vector3(0, f, 0));
                }
                else
                {
                    veh.ApplyForceRelative(new Vector3(0, -0.03f, 0));
                }
            }
            if (veh.DisplayName.ToLower().Contains("s8") || veh.DisplayName.ToLower().Contains("a8") || veh.DisplayName.Contains("S63"))
            {
                if (fakeSpeed > 184 && fakeSpeed < 304)
                {
                    decimal b = (decimal)((decimal)fakeSpeed / (decimal)37.4f);
                    veh.EnginePowerMultiplier = (float)b;
                }
                else
                {
                    veh.EnginePowerMultiplier = 1.0f;
                }
            }
            if (veh.DisplayName.ToLower().Contains("m8") && GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.9f)
            {

                if (fakeSpeed > 1 && fakeSpeed < 110)
                {
                    //  decimal b = (decimal)((decimal)fakeSpeed / (decimal)13.7f);
                    // UI.ShowSubtitle(b.ToString());
                    veh.ApplyForceRelative(new Vector3(0, 0.06f, 0));
                }else if(fakeSpeed > 200)
                {
                    if (fakeSpeed > 305)
                    {
                        veh.ApplyForceRelative(new Vector3(0, -0.01f, 0));
                    }
                    else
                    {
                        veh.ApplyForceRelative(new Vector3(0, 0.005f, 0));
                    }
                }
               
            }
        }

        public class Exhaust
        {
            public string VehName { get; set; }
            public string Original { get; set; }
            public string Active { get; set; }
        }

        private bool hasActiveExhaust = false;
        private bool isActiveExhaustActivated = false;
        private List<Exhaust> activeExhausts = new List<Exhaust>()
        {
           new Exhaust(){VehName = "pts21", Original = "comet2", Active = "m113k"},
           new Exhaust(){VehName = "m720", Original = "emerus", Active = "gallardov10"},
            new Exhaust(){VehName = "812", Original = "emerus", Active = "gallardov10"},
          // new Exhaust(){VehName = "63", Original = "windsor", Active = "m113k"},
           new Exhaust(){VehName = "panamer", Original = "dominator", Active = "m113k"},
            new Exhaust(){VehName = "mlbrab", Original = "dominator", Active = "m113k"},
            new Exhaust(){VehName = "s63", Original = "windsor", Active = "m113k"},
        };

        private void ActiveExhaust()
        {
            //  UI.ShowSubtitle(veh.DisplayName);
            if (hasActiveExhaust)
            {
                if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && fakeSpeed < 2)
                {
                    isActiveExhaustActivated = !isActiveExhaustActivated;
                    var exhaus = activeExhausts.Where(e => veh.DisplayName.ToLower().Contains(e.VehName)).FirstOrDefault();
                    if (isActiveExhaustActivated)
                    {
                        Function.Call((Hash)0x4F0C413926060B38, veh, exhaus.Active);
                    }
                    else
                    {
                        Function.Call((Hash)0x4F0C413926060B38, veh, exhaus.Original);
                    }
                }
            }
        }

        private Stopwatch brakeStopwatch = new Stopwatch();
        private int pressure = 0;
        private Vector3 initbrakepos = new Vector3(0, 0, 0);
        public void Brakes()
        {
            /*   if (Game.IsControlJustPressed(0, GTA.Control.VehicleBrake) && initbrakepos.Y == 0)
               {
                   initbrakepos = veh.Position;
               }
               if (fakeSpeed <= 0 && initbrakepos.Y != 0)
               {
                   UI.Notify(initbrakepos.DistanceTo(veh.Position) + "m");
                   initbrakepos = new Vector3();
               }*/
            if (!isInPlane && !isParkingMode)
            {
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.9 && fakeSpeed > 0 && !isReversing)
                {
                    var force = 0.078f;
                    if (isRaining)
                        force = 0.02f;
                    if (veh.DisplayName.ToLower().Contains("panamer"))
                    {
                        force = 0.14f;
                    }
                    veh.ApplyForceRelative(new Vector3(0, -force, 0));
                }
                if (fakeSpeed > 100 && GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.9 || brakeStopwatch.IsRunning)
                {
                    // strong braking
                    var windMult = (((veh.Speed - 20) * 3.6) / 10000);
                    // windMult = windMult >= 0.011f ? 0.011f : windMult;
                    if (Function.Call<float>((Hash)0xAD7E85FC227197C4, veh) >= 0.5)
                    {
                        //  windMult = windMult >= 0.007 ? 0.007 : windMult;
                    }
                    if (isRaining)
                        windMult += 0.009;

                    if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.6)
                    {
                        if (!brakeStopwatch.IsRunning)
                        {
                            brakeStopwatch.Start();
                        }

                        windMult += 0.008;
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.6)
                        {
                            windMult += 0.037f;
                        }
                    }
                    if (brakeStopwatch.ElapsedMilliseconds % 200 < 100)
                    {
                        GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
                    }
                    else
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                    }
                    windMult = windMult / 2;
                    if (steeringAngle == 0)
                        Function.Call((Hash)0x42A8EC77D5150CBE, Game.Player.LastVehicle, (brakeStopwatch.ElapsedMilliseconds % 1000 > 500 ? steeringAngle - windMult : steeringAngle + windMult));
                }
                if ((GamePad.GetState(PlayerIndex.One).Triggers.Left < 0.1 || fakeSpeed < 30) && brakeStopwatch.IsRunning)
                {
                    brakeStopwatch = new Stopwatch();
                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                }

            }
        }
        public void SireneMode()
        {

            if (veh.DisplayName.ToLower().Contains("toua"))
            {
                if (Game.IsControlJustReleased(0, GTA.Control.VehicleHorn))
                {
                    isSireneMode = !isSireneMode;
                    veh.SirenActive = isSireneMode;
                }
                if (isSireneMode)
                {
                    if (!veh.SirenActive)
                        veh.SirenActive = true;
                    if (!veh.LightsOn)
                        veh.LightsOn = true;
                    if (sireneStopwatch.ElapsedMilliseconds > 1000)
                    {
                        if (sireneStopwatch.ElapsedMilliseconds % 500 > 250)
                        {
                            veh.HighBeamsOn = true;
                        }
                        else
                        {
                            veh.HighBeamsOn = false;
                        }
                        if (sireneStopwatch.ElapsedMilliseconds > 2000)
                        {
                            sireneStopwatch = new Stopwatch();
                        }
                    }
                    if (!sireneStopwatch.IsRunning)
                    {
                        sireneStopwatch.Start();
                    }
                    if (rlSirene == null)
                    {
                        rlSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        rrSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        rlSirene.AttachTo(veh, 0, new Vector3(-0.63f, -2.3f, 1.2f), new Vector3(90f, 0, 0));
                        rrSirene.AttachTo(veh, 0, new Vector3(0.63f, -2.3f, 1.2f), new Vector3(90f, 0, 0));
                        rlSirene.LodDistance = 3000;
                        rrSirene.LodDistance = 3000;

                        flSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        frSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        flSirene.AttachTo(veh, 0, new Vector3(-0.25f, 1.9f, 0.1f), new Vector3(90f, 0, 180f));
                        frSirene.AttachTo(veh, 0, new Vector3(0.25f, 1.9f, 0.1f), new Vector3(90f, 0, 180f));
                        flSirene.LodDistance = 3000;
                        frSirene.LodDistance = 3000;

                        /*
                           rlSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        rrSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        rlSirene.AttachTo(veh, 0, new Vector3(-0.45f, -1.0f, 0.44f), new Vector3(90f, 0, 0));
                        rrSirene.AttachTo(veh, 0, new Vector3(0.45f, -1.0f, 0.44f), new Vector3(90f, 0, 0));
                        rlSirene.LodDistance = 3000;
                        rrSirene.LodDistance = 3000;

                        flSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        frSirene = World.CreateProp(new Model(-1035660791), new Vector3(0, 0, 0), false, false);
                        flSirene.AttachTo(veh, 0, new Vector3(-0.35f, 2.1f, 0.0f), new Vector3(90f, 0, 180f));
                        frSirene.AttachTo(veh, 0, new Vector3(0.35f, 2.1f, 0.0f), new Vector3(90f, 0, 180f));
                        flSirene.LodDistance = 3000;
                        frSirene.LodDistance = 3000;
                         */
                    }
                    var thresh = 20;
                    var max = 255;
                    var min = 60;

                    if (rlSireneUp)
                    {
                        rlSireneAlpha += thresh;
                        if (rlSireneAlpha >= max)
                        {
                            rlSireneUp = false;
                        }
                    }
                    if (!rlSireneUp)
                    {
                        rlSireneAlpha -= thresh;
                        if (rlSireneAlpha <= min)
                        {
                            rlSireneUp = true;
                        }
                    }

                    if (rrSireneUp)
                    {
                        rrSireneAlpha += thresh;
                        if (rrSireneAlpha >= max)
                        {
                            rrSireneUp = false;
                        }
                    }
                    if (!rrSireneUp)
                    {
                        rrSireneAlpha -= thresh;
                        if (rrSireneAlpha <= min)
                        {
                            rrSireneUp = true;
                        }
                    }

                    rlSirene.Alpha = rlSireneAlpha;
                    rrSirene.Alpha = rrSireneAlpha;

                    if (flSireneUp)
                    {
                        flSireneAlpha += thresh;
                        if (flSireneAlpha >= max)
                        {
                            flSireneUp = false;
                        }
                    }
                    if (!flSireneUp)
                    {
                        flSireneAlpha -= thresh;
                        if (flSireneAlpha <= min)
                        {
                            flSireneUp = true;
                        }
                    }

                    if (frSireneUp)
                    {
                        frSireneAlpha += thresh;
                        if (frSireneAlpha >= max)
                        {
                            frSireneUp = false;
                        }
                    }
                    if (!frSireneUp)
                    {
                        frSireneAlpha -= thresh;
                        if (frSireneAlpha <= min)
                        {
                            frSireneUp = true;
                        }
                    }

                    flSirene.Alpha = flSireneAlpha;
                    frSirene.Alpha = frSireneAlpha;
                }
                else
                {
                    if (veh.SirenActive)
                        veh.SirenActive = false;
                    if (rlSirene != null)
                    {
                        rlSirene.Delete();
                        rlSirene = null;
                        rrSirene.Delete();
                        rrSirene = null;
                        flSirene.Delete();
                        flSirene = null;
                        frSirene.Delete();
                        frSirene = null;
                    }
                }
            }
        }

        public static List<string> weatherTypes = new List<string> { "FOGGY", "THUNDER", "RAIN", "RAIN", "THUNDER", "THUNDER" };
        private float currentRainAmount = 0f;
        private bool hasCelingAbove = false;
        public void Weather()
        {
            isRaining = Function.Call<float>((Hash)0x96695E368AD855F3) > 0.3f;
            if (currentWeatherId % 2 == 0 && currentWeatherId != 0 && isESPActive && currentRainAmount > 0.3)
            {
                if (fakeSpeed > 20)
                    veh.EngineTorqueMultiplier = 0.8f;
                if (fakeSpeed > 100)
                    veh.EngineTorqueMultiplier = 0.67f;
                if (fakeSpeed > 155)
                {
                    veh.EngineTorqueMultiplier = 0.1f;
                }
                if (fakeSpeed > 160)
                {
                    veh.EngineTorqueMultiplier = 0.0f;
                }
            }
            else
            {
                veh.EngineTorqueMultiplier = 1f;
            }
            var ped = Game.Player.Character;
            RaycastResult ray;
            if (Game.Player.Character.IsInVehicle())
            {
                ray = World.Raycast(veh.GetOffsetInWorldCoords(new Vector3(0, 0, 3)), veh.GetOffsetInWorldCoords(new Vector3(0, 0, 20)), IntersectOptions.Everything, veh);
            }
            else
            {
                ray = World.Raycast(ped.GetOffsetInWorldCoords(new Vector3(0, 0, 0.4f)), veh.GetOffsetInWorldCoords(new Vector3(0, 0, 20)), IntersectOptions.Everything, ped);
            }
            var celing = ray.DitHitAnything;
            if (hasCelingAbove && !celing)
            {
                Function.Call((Hash)0x643E26EA6E024D92, currentRainAmount);
            }
            if (!hasCelingAbove && celing)
            {
                currentRainAmount = Function.Call<float>((Hash)0x96695E368AD855F3);
                Function.Call((Hash)0x643E26EA6E024D92, 0f);
            }
            hasCelingAbove = celing;
            // waves height
            Function.Call((Hash)0xB96B00E976BE977F, 0f);
            if (!isTruckMode)
            {
                if ((Game.IsControlJustPressed(0, GTA.Control.ScriptPadLeft) && !Game.Player.Character.IsInVehicle(veh)) || (Game.Player.Character.IsInVehicle() && Game.IsControlJustPressed(0, GTA.Control.ScriptPadUp) && !isCruiseControlActive))
                {
                    currentWeatherId += 1;
                    if (currentWeatherId <= weatherTypes.Count)
                    {
                        currentRainAmount = currentWeatherId == 2 ? 1f : (currentWeatherId % 2 == 0 ? 0.04f : 0f);
                        Function.Call((Hash)0x643E26EA6E024D92, currentRainAmount);
                        UI.ShowSubtitle($"Weather: {weatherTypes[currentWeatherId - 1]} {(currentWeatherId % 2 == 0 ? "with rain" : "")}");
                        Function.Call<float>((Hash)0x29B487C359E19889, weatherTypes[currentWeatherId - 1]);
                        Function.Call<float>((Hash)0x704983DF373B198F, weatherTypes[currentWeatherId - 1]);
                        Function.Call((Hash)0x11B56FBBF7224868, "RAIN");
                        Function.Call((Hash)0xFC4842A34657BFCB, "RAIN", 1f);
                    }
                    else
                    {
                        currentWeatherId = 0;
                        Function.Call((Hash)0xCCC39339BEF76CF5);
                        UI.ShowSubtitle("No weather");
                    }
                }
            }
        }

        public void PlayersClock()
        {
            var h = fakeTimeHours;
            var m = fakeTimeMinutes;
            var cluster = new UIContainer(new Point(45, UI.HEIGHT - 20), new Size(70, 16), System.Drawing.Color.FromArgb(120, 0, 0, 0));
            cluster.Items.Add(new UIText(h + ":" + (m.ToString().Length == 1 ? 0 + "" + m : m.ToString() + " " + World.CurrentDate.DayOfWeek), new Point(35, 0), 0.24f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
            cluster.Draw();
        }

        public List<string> espTurnedOffByDefaultList = new List<string>()
        {
             "schafter", "315", "850", "fiesta", "z4"
        };

        public void ESP()
        {
            if (espTurnedOffByDefaultList.Any(c => veh.DisplayName.ToLower().Contains(c)))
            {
                isESPActive = false;
                isTCSActive = false;
            }
            if (isESPActive)
            {
                espStopwatch = new Stopwatch();
                var currentModelSize = veh.Model.GetDimensions();
                currentModelSize = new Vector3(currentModelSize.X / 2, currentModelSize.Y / 2, currentModelSize.Z / 2);
                var color = System.Drawing.Color.Yellow;
                var front = GetEntityOffset(veh, new Vector3(0, currentModelSize.Y, -currentModelSize.Z * 0));
                var rear = GetEntityOffset(veh, new Vector3(0, -currentModelSize.Y, currentModelSize.Z * 0));
                var currentL = GetEntityOffset(veh, new Vector3(-2f, currentModelSize.Y, currentModelSize.Z * 0));
                var currentR = GetEntityOffset(veh, new Vector3(2f, currentModelSize.Y, currentModelSize.Z * 0));
                var x = veh.Speed * (-Math.Sin(veh.Rotation.Z));
                var y = veh.Speed * Math.Cos(veh.Rotation.Z);
                Vector3 rotRel = new Vector3((float)x, (float)y, 0);
                espSpeedVector = Function.Call<Vector3>(Hash.GET_ENTITY_SPEED_VECTOR, veh, true);
                var oversteerAngle = Math.Acos(espSpeedVector.Y / veh.Speed);
                var overMin = (5 * Math.PI) / 180;
                var espTreshold = fakeSpeed > 80 ? 1.1f : 1.4f;

                bool isSportEsp = drivingMode == "sport";

                if (isSportEsp)
                {
                    espTreshold = 7.5f;
                }

                if (fakeSpeed > 160)
                    espTreshold = 2.9f;



                var wasEspEnabled = isESPEnabled;
                if ((espSpeedVector.X > espTreshold || espSpeedVector.X < -espTreshold))
                {
                    isESPEnabled = true;
                    if (!espClusterRunningStopwatch.IsRunning)
                        espClusterRunningStopwatch.Start();
                }
                else
                {
                    isESPEnabled = false;
                }
                if (isESPEnabled)
                {
                    var isLeft = espSpeedVector.X > espTreshold;
                    Game.DisableControlThisFrame(0, GTA.Control.VehicleAccelerate);
                    veh.ApplyForceRelative(new Vector3(isLeft ? -0.1f : 0.1f, !isReversing ? -0.09f : 0.09f, 0));
                    if (fakeSpeed < 10)
                        veh.HandbrakeOn = true;
                }
                if (wasEspEnabled && !isESPEnabled)
                {
                    espClusterIconStopwatch = new Stopwatch();
                    espClusterIconStopwatch.Start();
                }
                if (espClusterIconStopwatch.ElapsedMilliseconds > 500)
                {
                    espClusterIconStopwatch = new Stopwatch();
                    espClusterRunningStopwatch = new Stopwatch();
                }
            }
            if (Game.IsControlPressed(0, GTA.Control.VehicleHandbrake) && !espToggleStopwatch.IsRunning && !isParkingGear && fakeSpeed <= 0)
            {
                espToggleStopwatch.Start();

            }
            if (Game.IsControlPressed(0, GTA.Control.VehicleHandbrake) && espToggleStopwatch.ElapsedMilliseconds > (isESPActive ? 3000 : 1400))
            {
                isESPActive = !isESPActive;
                isTCSActive = !isTCSActive;
                espToggleStopwatch = new Stopwatch();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleHandbrake) && espToggleStopwatch.IsRunning)
            {
                espToggleStopwatch = new Stopwatch();
            }
        }

        private Vector3 lcStoppingDistance = new Vector3(0, 0, -99f);

        public void LaunchControl()
        {
            if (veh.IsInBurnout() && !isLaunchControl && fakeSpeed < 4)
            {
                isLaunchControl = true;
                if (!isSportMode)
                    isBoostMode = true;
            }

            if (isLaunchControl)
            {
                veh.HandbrakeOn = true;
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left < 0.1)
                {
                    isLaunchControl = false;
                    launchControlAccelStopwatch.Start();
                }
            }
            if (launchControlAccelStopwatch.IsRunning)
            {
                if (fakeSpeed > topSpeed)
                    topSpeed = fakeSpeed;
                var elapsed = (double)(launchControlAccelStopwatch.ElapsedMilliseconds / 1000.0);
                if (fakeSpeed >= 100 && lc0x100 == 0)
                {
                    lc0x100 = elapsed;
                    UI.Notify($"0-100: {elapsed}s");
                }
                if (fakeSpeed >= 150 && lc0x150 == 0)
                {
                    UI.Notify($"0-150: {elapsed}s");
                    lc0x150 = elapsed;
                }

                if (fakeSpeed >= 200 && lc0x200 == 0)
                {
                    UI.Notify($"0-200: {elapsed}s");
                    lc0x200 = elapsed;
                }

                if (fakeSpeed >= 250 && lc0x250 == 0)
                {
                    UI.Notify($"0-250: {elapsed}s");
                    lc0x250 = elapsed;
                }

                if (fakeSpeed >= 300 && lc0x300 == 0)
                {
                    UI.Notify($"0-300: {elapsed}s");
                    lc0x300 = elapsed;
                }

                if (fakeSpeed >= 350 && lc0x350 == 0)
                {
                    UI.Notify($"0-350: {elapsed}s");
                    lc0x350 = elapsed;
                }
                if (launchControlAccelStopwatch.IsRunning && GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.8 && fakeSpeed < 100 && veh.MaxBraking > 0.8f && false)
                {
                    veh.ApplyForceRelative(new Vector3(0, 0.04f, 0));
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.1 && launchControlAccelStopwatch.IsRunning)
                {
                    isBoostMode = false;
                    UI.Notify($"0-100: {lc0x100}s ~n~ 0-150: {lc0x150}s ~n~ 0-200: {lc0x200}s ~n~ 0-250: {lc0x250}s ~n~ 0-300: {lc0x300}s ~n~ 0-350: {lc0x350}s ~n~ Top speed: {topSpeed} km/h");
                    lc0x100 = 0;
                    lc0x150 = 0;
                    lc0x200 = 0;
                    lc0x250 = 0;
                    lc0x300 = 0;
                    lc0x350 = 0;
                    launchControlAccelStopwatch = new Stopwatch();
                    lcStoppingDistance = veh.Position;
                }
            }
            if (topSpeed < fakeSpeed && !launchControlAccelStopwatch.IsRunning && lcStoppingDistance.Z > -98)
            {
                lcStoppingDistance = new Vector3(0, 0, -99f);
                topSpeed = 0;
            }

            if (fakeSpeed <= 0 && lcStoppingDistance.Z > -98f)
            {
                UI.Notify($"Stopping Dist: {veh.Position.DistanceTo(lcStoppingDistance)}m");
                lcStoppingDistance = new Vector3(0, 0, -99f);
                topSpeed = 0;
            }
        }
        public void TCS()
        {
            var maxAcceleration = Function.Call<float>((Hash)(0x5DD35C8D074E57AE), veh);
            var maxTraction = Function.Call<float>(Hash.GET_VEHICLE_MAX_TRACTION, veh);
            var pitch = veh.ForwardVector.Z * 100;
            var pitchCompensate = pitch > 0 ? (pitch / 10f) : 0f;
            var algo = ((maxAcceleration * maxTraction) * 2.8f);
            if (isTCSActive && drivingMode != "sport" && !isParkingMode)
            {
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.8f && fakeSpeed < 50 && fakeSpeed > 2 && maxAcceleration > 0.12)
                {
                    if (tcsLastSpeedDiff < algo)
                    {
                        if (!tcsWheelSlipStopwatch.IsRunning)
                            tcsWheelSlipStopwatch.Start();
                    }
                    else
                    {
                        tcsWheelSlipStopwatch = new Stopwatch();
                    }
                }
                else
                {
                    tcsWheelSlipStopwatch = new Stopwatch();
                }
                var wasTcsEnabled = isTCSEnabled;
                if (tcsWheelSlipStopwatch.ElapsedMilliseconds % 200 > 150 && tcsWheelSlipStopwatch.IsRunning && !isReversing)
                {
                    if (!isTCSEnabled)
                    {
                        isTCSEnabled = true;
                        Game.DisableControlThisFrame(0, GTA.Control.VehicleAccelerate);
                        if (!tcsClusterRunningStopwatch.IsRunning)
                            tcsClusterRunningStopwatch.Start();
                    }
                }
                else
                {
                    isTCSEnabled = false;
                }
                if (wasTcsEnabled && !isTCSEnabled)
                {
                    tcsClusterIconStopwatch = new Stopwatch();
                    tcsClusterIconStopwatch.Start();
                }
                if (tcsClusterIconStopwatch.ElapsedMilliseconds > 500 && tcsClusterIconStopwatch.IsRunning)
                {
                    tcsClusterIconStopwatch = new Stopwatch();
                    tcsClusterRunningStopwatch = new Stopwatch();
                }
                if (tcsCalcStopwatch.ElapsedMilliseconds > 100)
                {
                    tcsCalcStopwatch = new Stopwatch();
                    tcsLastSpeedDiff = ((veh.Speed - tcsLastSpeed));
                    tcsLastSpeed = veh.Speed;
                    tcsCalcStopwatch.Start();
                }
                if (!tcsCalcStopwatch.IsRunning)
                {
                    tcsCalcStopwatch.Start();
                }
            }
        }

        public void Ingition()
        {
            if (!isInPlane)
            {
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.1
                            && GamePad.GetState(PlayerIndex.One).Buttons.A == XInputDotNetPure.ButtonState.Pressed
                            && !isEngineRunning
                            && fakeSpeed <= 0)
                {
                    isEngineRunning = true;
                    isParkingGear = true;
                    ingitionStartStopwatch.Start();
                }
                if (ingitionStartStopwatch.ElapsedMilliseconds > 1000)
                    ingitionStartStopwatch = new Stopwatch();
                if (ingitionStartStopwatch.IsRunning)
                    isParkingGear = true;
                if (isEngineRunning && GamePad.GetState(PlayerIndex.One).Buttons.A == XInputDotNetPure.ButtonState.Pressed && !ingitionStopwatch.IsRunning && isParkingGear)
                    ingitionStopwatch.Start();
                if (isEngineRunning && ingitionStopwatch.ElapsedMilliseconds > 1000)
                {
                    ingitionStopwatch = new Stopwatch();
                    isEngineRunning = false;
                    isParkingGear = true;

                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.A != XInputDotNetPure.ButtonState.Pressed)
                    ingitionStopwatch = new Stopwatch();
                if (isChargerCordEv || isChargerCord)
                    isEngineRunning = false;
                veh.EngineRunning = isEngineRunning;

            }
        }

        public void AutoHeadlightsWhenDark()
        {
            if (!autoHeadlightsStopwatch.IsRunning)
                autoHeadlightsStopwatch.Start();
            if (autoHeadlightsStopwatch.ElapsedMilliseconds > 600)
            {
                var ray = World.Raycast(veh.GetOffsetInWorldCoords(new Vector3(0, 0, 3)), veh.GetOffsetInWorldCoords(new Vector3(0, 0, 20)), IntersectOptions.Everything, veh);

                if (ray.DitHitAnything)
                {
                    if (wasDarkLastTime)
                    {
                        veh.LightsOn = true;
                    }
                    else
                    {
                        wasDarkLastTime = true;
                    }
                }
                else if (wasDarkLastTime)
                {
                    veh.LightsOn = false;
                    wasDarkLastTime = false;
                }
                autoHeadlightsStopwatch = new Stopwatch();
            }
        }

        public static List<int> containerList = new List<int>()
        {
            -629735826, 466911544, 772023703, 2140719283, -1857328104, 1525186387, -380625884, 511018606
        };

        public void CementMixer()
        {
            if (veh.DisplayName.ToLower().Contains("660"))
            {
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right <= 0 && fakeSpeed > 55 && !isReversing)
                {
                    var f = 0.028f;
                    veh.ApplyForceRelative(new Vector3(0, f, 0));
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0 && fakeSpeed > 97)
                {
                    var f = 0.016f;
                    veh.ApplyForceRelative(new Vector3(0, -f, 0));
                }
            }
            if (veh.DisplayName.ToLower().Contains("680"))
            {
                if (
                    GamePad.GetState(PlayerIndex.One).Triggers.Right <= 0
                    && fakeSpeed > 25
                    && !isReversing
                    && GamePad.GetState(PlayerIndex.One).Triggers.Left < 1)
                {
                    var f = 0.031f;
                    veh.ApplyForceRelative(new Vector3(0, f, 0));
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0 && fakeSpeed > (isTruckMode ? 105 : 110))
                {
                    var f = 0.016f;
                    veh.ApplyForceRelative(new Vector3(0, -f, 0));
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 1 && GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1 && fakeSpeed <= 0 && !toggleTruckModeStopwatch.IsRunning)
                {
                    toggleTruckModeStopwatch.Start();
                    isTruckMode = !isTruckMode;
                    truckTrailer = isTruckMode ? World.GetClosestVehicle(veh.GetOffsetInWorldCoords(new Vector3(0f, -8f, 0f)), 3f) : null;
                    if (isTruckMode && truckTrailer == null)
                        isTruckMode = false;
                }
            }
            if (toggleTruckModeStopwatch.ElapsedMilliseconds > 3000)
            {
                toggleTruckModeStopwatch = new Stopwatch();
            }
        }

        public void KickDown()
        {
            if (!isParkingGear && !isParkingMode)
            {
                if (veh.DisplayName.ToLower().Contains("crossik"))
                {
                    if (isBoostMode)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right <= 0 && !canTurnOffBoostStopwatch.IsRunning)
                        {
                            canTurnOffBoostStopwatch = new Stopwatch();
                            canTurnOffBoostStopwatch.Start();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.1)
                        {
                            canTurnOffBoostStopwatch = new Stopwatch();
                        }
                        if (canTurnOffBoostStopwatch.ElapsedMilliseconds > 2000)
                        {
                            isBoostMode = false;
                            drivingMode = null;
                            canTurnOffBoostStopwatch = new Stopwatch();
                        }
                        //  veh.EngineTorqueMultiplier = 1f;
                        if (isTesla)
                        {
                            //  veh.EngineTorqueMultiplier = 1.7f;
                        }
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 0.1f && canTurnOnBoostStages == 0)
                        {
                            canTurnOnBoostStopwatch = new Stopwatch();
                            canTurnOnBoostStopwatch.Start();
                            canTurnOnBoostStages += 1;
                        }
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Left <= 0 && canTurnOnBoostStages == 1)
                            canTurnOnBoostStages += 1;
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 0.1f && canTurnOnBoostStages == 2)
                            canTurnOnBoostStages += 1;
                        if (canTurnOnBoostStages == 3)
                        {
                            canTurnOnBoostStopwatch = new Stopwatch();
                            isBoostMode = false;
                            canTurnOnBoostStages = 0;
                        }
                        if (canTurnOnBoostStopwatch.ElapsedMilliseconds > 500)
                        {
                            canTurnOnBoostStages = 0;
                            canTurnOnBoostStopwatch = new Stopwatch();
                        }
                    }
                    else
                    {
                        // veh.EngineTorqueMultiplier = 0.64f;
                        //  if (fakeSpeed > 180)
                        //    veh.EngineTorqueMultiplier = 1f;
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 1 && canTurnOnBoostStages == 0)
                        {
                            canTurnOnBoostStopwatch = new Stopwatch();
                            canTurnOnBoostStopwatch.Start();
                            canTurnOnBoostStages += 1;
                        }
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right <= 0 && canTurnOnBoostStages == 1)
                            canTurnOnBoostStages += 1;
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 1 && canTurnOnBoostStages == 2)
                            canTurnOnBoostStages += 1;
                        if (canTurnOnBoostStages == 3)
                        {
                            canTurnOnBoostStopwatch = new Stopwatch();
                            isBoostMode = true;
                            drivingMode = "boost";
                            //   isParkingMode = true;
                            //   isParkingGear = false;
                            canTurnOnBoostStages = 0;
                        }
                        if (canTurnOnBoostStopwatch.ElapsedMilliseconds > 500)
                        {
                            canTurnOnBoostStages = 0;
                            canTurnOnBoostStopwatch = new Stopwatch();
                        }
                    }
                }
            }
        }

        // other timescales for different months in other files in project

        private Vector3 tunelEntryPos = new Vector3(2960.03f, -2386.43f, 6.5f);
        private Vector3 tunelExitPos = new Vector3(1587.3f, -3703.4f, 6.5f);
        private Vector3 tunelCenterPos = new Vector3(2307.8f, -3080.6f, 6.4f);
        private bool isInTunel = false;
        private DateTime beforeTunelTime;

        public void TimeScale()
        {
            if (!isInTunel)
            {
                Game.PauseClock(true);
                if (!timeScaleStopwatch.IsRunning)
                {
                    timeScaleStopwatch.Start();
                }
                if (!progressTimeStopwatch.IsRunning)
                {
                    progressTimeStopwatch.Start();
                }
                var isSlowTime = timeScale > 2f;
                if (timeScaleStopwatch.ElapsedMilliseconds > (((1000 * timeScale)) / 60))
                {

                    if (fastForwardTime)
                    {
                        fakeTimeSeconds += 60;

                        // if (fakeTimeHours < 13 || fakeTimeHours > 13)
                        Function.Call(Hash.ADD_TO_CLOCK_TIME, 0, 0, fakeTimeMinutes % 2 == 0 ? 60 : 60);
                    }
                    else
                    {
                        fakeTimeSeconds += 1;
                        //   if (fakeTimeHours < 13 || fakeTimeHours > 13)
                        Function.Call(Hash.ADD_TO_CLOCK_TIME, 0, 0, fakeTimeMinutes % 2 == 0 ? 1 : 1);
                    }
                    if (fakeTimeSeconds > 59)
                    {
                        fakeTimeSeconds = 0;
                        fakeTimeMinutes += 1;
                    }
                    if (fakeTimeMinutes > 59)
                    {
                        fakeTimeMinutes = 0;
                        fakeTimeHours += 1;
                    }
                    if (fakeTimeHours > 23)
                    {
                        fakeTimeHours = 0;
                    }
                    timeScaleStopwatch = new Stopwatch();
                    string[] fth = { fakeTimeHours.ToString() };
                    System.IO.File.WriteAllLines(@"D:\Steam\steamapps\common\Grand Theft Auto V\scripts\current_time.txt", fth);
                }
                if (
                        ((fakeTimeHours >= sunriseTime.Hour - 1 && fakeTimeMinutes >= sunriseTime.Minute) || fakeTimeHours > sunriseTime.Hour - 1)
                        &&
                        ((fakeTimeHours <= sunsetTime.Hour + 2 && fakeTimeMinutes <= sunsetTime.Minute) || fakeTimeHours < sunsetTime.Hour + 2)
                        )
                {
                    if (!sunriseInit)
                    {
                        sunriseInit = true;
                        World.CurrentDayTime = new TimeSpan(inGameFixedSunrise.Hour, inGameFixedSunrise.Minute, 0);
                    }
                    if (!sunriseAtGameStart)
                    {
                        sunriseAtGameStart = true;
                        inGameFixedSunrise = new DateTime(2020, 1, 1, 4, 0, 0);
                    }
                    /*
                    if (fastForwardTime)
                    {
                        Function.Call(Hash.ADD_TO_CLOCK_TIME, 0, 0, 10);
                    }
                    else
                    {
                        if (isSlowTime)
                            Function.Call(Hash.ADD_TO_CLOCK_TIME, 0, 1, 0);
                        else
                            Function.Call(Hash.ADD_TO_CLOCK_TIME, 0, 0, 1);
                    }*/
                }
                else
                {
                    World.CurrentDate = new DateTime(2020, 5, 10, 0, 0, 0);
                    sunriseInit = false;
                }
                var diff = ((UnixTimestampFromDateTime(sunsetTime) - UnixTimestampFromDateTime(sunriseTime)) / (60));
                if (progressTimeStopwatch.ElapsedMilliseconds > 2000 && !isSlowTime)
                {
                    var addSec = 1;

                    progressTimeStopwatch = new Stopwatch();
                }
                if (Game.IsControlJustPressed(0, GTA.Control.PhoneUp))
                {
                    if ((!Game.Player.Character.IsInVehicle() && !isCraneMode && !isControlingRtg) || (isCraneMode && Game.Player.Character.IsInVehicle(veh)))
                    {
                        fastForwardTime = !fastForwardTime;
                    }
                }
                if (fastForwardTime)
                    UI.ShowSubtitle("Fast Forwarding time ");
            }


            if (isOnAutobahn && veh.Position.Z < 9f)
            {
                if (veh.Position.DistanceTo(tunelCenterPos) < 952f && !isInTunel)
                {
                    isInTunel = true;
                    beforeTunelTime = World.CurrentDate;
                    World.CurrentDate = new DateTime(2020, 5, 10, 0, 0, 0);
                }
                if (veh.Position.DistanceTo(tunelCenterPos) > 952f && isInTunel)
                {
                    isInTunel = false;
                    World.CurrentDate = beforeTunelTime;
                }
            }

            // World.CurrentDate = new DateTime(2020, 1, 2, 7, 0, 0);
            // UI.ShowSubtitle(fakeTimeHours.ToString() + ":" + (fakeTimeMinutes > 9 ? "" : "0") + fakeTimeMinutes.ToString() + ":" + (fakeTimeSeconds > 9 ? "" : "0") + fakeTimeSeconds.ToString() + " " + diff.ToString());
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
        public void PreventTrafficBehindCar()
        {
            if (Game.Player.Character.IsInVehicle(veh))
            {
                if (!preventTrafficBehindCar.IsRunning && Game.Player.Character.IsInVehicle(veh))
                    preventTrafficBehindCar.Start();
                if (preventTrafficBehindCar.ElapsedMilliseconds < 1000)
                {
                    if (fakeSpeed > 70)
                    {
                        // veh.ApplyForceRelative(new Vector3(-0.3f, 0, 0));
                    }
                }
                if (fakeSpeed < 120 && preventTrafficBehindCar.ElapsedMilliseconds > 2000)
                {
                    preventTrafficBehindCar = new Stopwatch();
                    var off = veh.GetOffsetInWorldCoords(new Vector3(0, isTruckMode ? -20f : -5f, 0));
                    Function.Call((Hash)0x1033371FC8E842A7, preventTrafficBehindZone);
                    preventTrafficBehindZone = 0;
                    preventTrafficBehindZone = Function.Call<int>((Hash)0x2CE544C68FB812A0, off.X, off.Y, off.Z, 4f, veh.Speed - 8f, false);

                }
                if (preventTrafficBehindCar.ElapsedMilliseconds > 2000 && preventTrafficBehindZone > 0)

                {
                    Function.Call((Hash)0x1033371FC8E842A7, preventTrafficBehindZone);
                    preventTrafficBehindZone = 0;
                    preventTrafficBehindCar = new Stopwatch();

                }
            }
            else if (preventTrafficBehindZone > 0)
            {
                Function.Call((Hash)0x1033371FC8E842A7, preventTrafficBehindZone);
                preventTrafficBehindZone = 0;
                preventTrafficBehindCar = new Stopwatch();
            }

        }
        private void RoofRack()
        {
            if (roofBike == null)
            {
                roofBike = World.CreateVehicle(new Model(VehicleHash.TriBike2), new Vector3());
                roofBike.AttachTo(veh, 0, new Vector3(-0.33f, -0.9f, 1.38f), new Vector3(0, 0, 0));
            }
            if (fakeSpeed > 150)
            {
                roofBike.Detach();
            }
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleSelectNextWeapon))
            {
                roofBike.Delete();
            }
        }
        private void Forklift()
        {
            if (veh.DisplayName.ToLower().Contains("fork") || veh.DisplayName.ToLower().Contains("qash"))
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.2 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.2)
                {
                    Game.DisableControlThisFrame(0, GTA.Control.VehicleMoveUpDown);
                }
            }
        }
        private static Stopwatch noGearActionStopwatch = new Stopwatch();
        private void Gears()
        {
            if (!showApp && isEngineRunning && !isInPlane)
            {
                if (isWRC)
                {
                    if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed)
                    {
                        veh.HandbrakeOn = true;
                    }
                    else
                    {
                        veh.HandbrakeOn = false;
                    }
                }
                if (fakeSpeed < 2)
                {
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == XInputDotNetPure.ButtonState.Pressed && !parkingBrakeStopwatch.IsRunning)
                    {
                        parkingBrakeStopwatch = new Stopwatch();
                        parkingBrakeStopwatch.Start();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == XInputDotNetPure.ButtonState.Pressed && parkingBrakeStopwatch.ElapsedMilliseconds > 500 && !noGearActionStopwatch.IsRunning)
                    {
                        isParkingGear = false;
                        isParkingMode = true;
                        drivingMode = "parking";
                        parkingBrakeStopwatch = new Stopwatch();
                        noGearActionStopwatch.Start();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == XInputDotNetPure.ButtonState.Released && parkingBrakeStopwatch.ElapsedMilliseconds > 100 && !noGearActionStopwatch.IsRunning)
                    {
                        isParkingGear = isParkingGear ? false : true;
                        isParkingMode = false;
                        drivingMode = null;
                        parkingBrakeStopwatch = new Stopwatch();
                    }
                    if (parkingBrakeStopwatch.ElapsedMilliseconds > 1000)
                    {
                        parkingBrakeStopwatch = new Stopwatch();
                    }
                    if (noGearActionStopwatch.ElapsedMilliseconds > 1200)
                    {
                        noGearActionStopwatch = new Stopwatch();
                    }
                }
                if (speedInKmh > 0)
                {
                    isParkingGear = false;
                }
                if (isParkingGear)
                {
                    veh.HandbrakeOn = true;
                    Game.DisableControlThisFrame(0, GTA.Control.VehicleBrake);
                }
            }
        }

        public void LibertyCity()
        {
            isInLC = Game.Player.Character.Position.DistanceTo(new Vector3(5413.6f, -2737.34f, 15.1f)) < 2745f;
        }
        private void Steering()
        {
            float maxSteeringAngle = 0.18f;
            float steeringSpeedIn = 10f;
            float steeringSpeedOut = 1f;
            float sportModeSterringSpeedOut = 0f;
            bool isSportMode = drivingMode == "sport";

            if (speedInKmh == 0)
            {
                steeringSpeedOut = 100f;
            }
            if (speedInKmh > 39)
            {
                maxSteeringAngle = 0.08f;
            }
            if (speedInKmh < 69 && (steeringAngle > 0.03 || steeringAngle < -0.03))
            {
                steeringSpeedIn = 80f;
                maxSteeringAngle = 0.10f;
                if (isSportMode)
                {
                    steeringSpeedIn = 80f;
                    maxSteeringAngle = 0.10f;
                }
            }
            if (speedInKmh >= 69)
            {
                steeringSpeedIn = 10f;
                float maxSteeringStartValue = 0.040f;
                if (isSportMode)
                {
                    maxSteeringStartValue = 0.0620f;
                }
                if (speedInKmh > 80)
                {
                    maxSteeringStartValue = 0.032f;
                    if (isSportMode)
                    {
                        maxSteeringStartValue = 0.0470f;
                    }
                }
                if (speedInKmh > 140)
                {
                    steeringSpeedIn = 30f;
                    if (isSportMode)
                    {
                        steeringSpeedIn = 15f;
                        maxSteeringStartValue = 0.048f;
                    }
                }
                maxSteeringAngle = maxSteeringStartValue - (float)Math.Round(((double)speedInKmh / 10000), 3);
                if (isSportMode)
                {
                    maxSteeringAngle += 0.0025f;
                }
                if (maxSteeringAngle <= 0.017f)
                    maxSteeringAngle = 0.017f;
            }


            if ((GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1 && fakeSpeed > 50 && (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 0.7f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -0.7f)))
            {
                maxSteeringAngle = 1f;
                steeringSpeedIn = 1f;
                steeringSpeedOut = 1f;
            }
            Game.DisableControlThisFrame(0, GTA.Control.VehicleMoveLeftRight);
            var stickForce = -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X / 10f;

            // left
            if (stickForce > 0)
            {
                if (stickForce >= lastSteeringAngle && stickForce >= steeringAngle)
                {
                    steeringAngle += stickForce / steeringSpeedIn;
                }
                else if (stickForce < lastSteeringAngle && stickForce <= steeringAngle)
                {
                    steeringAngle -= (stickForce / steeringSpeedOut) + sportModeSterringSpeedOut;
                }
            }
            //right
            else if (stickForce < 0)
            {
                if (stickForce <= lastSteeringAngle && stickForce <= steeringAngle)
                {
                    steeringAngle += stickForce / steeringSpeedIn;
                }
                else if (stickForce > lastSteeringAngle && stickForce >= steeringAngle)
                {
                    steeringAngle -= (stickForce / steeringSpeedOut) + sportModeSterringSpeedOut;
                }
            }
            //center
            else if (stickForce == 0 && speedInKmh > 0 && addaptiveCarAhead == null)
            {
                if (steeringAngle > 0)
                    steeringAngle -= 0.01f;
                if (steeringAngle < 0)
                    steeringAngle += 0.01f;
                if (steeringAngle < 0.01 && steeringAngle > -0.01)
                    steeringAngle = 0;
            }
            lastSteeringAngle = stickForce;
            bool isSteeringNegative = steeringAngle < 0;
            if (steeringAngle > maxSteeringAngle || steeringAngle < -maxSteeringAngle)
                steeringAngle = isSteeringNegative ? -maxSteeringAngle : maxSteeringAngle;
            if (isAquaplaningOn && !isESPActive)
            {
                if (isEasyAqua)
                {
                    steeringAngle = isSteeringNegative ? -0.005f : 0.005f;
                }
                else
                {
                    float force = 0.018f;
                    if (speedInKmh <= 125 || true)
                        force = 0.012f;
                    steeringAngle = isAquaplaningDirRight ? steeringAngle + force : steeringAngle - force;
                }
            }
            Function.Call((Hash)0x42A8EC77D5150CBE, veh, steeringAngle);
            //   UI.ShowSubtitle("max: " + maxSteeringAngle + "ster: " + steeringAngle.ToString() + " pad x: " + stickForce);
        }

        private List<string> carsWithAirSuspension = new List<string>() { "caddy" };
        private void AirSuspension()
        {
            if (hasAirSuspension)
            {
                if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && speedInKmh <= 10 && Game.Player.Character.IsInVehicle(veh))
                {
                    if (airSuspensionMode >= 3)
                        airSuspensionToggleUp = false;
                    if (airSuspensionMode == 1)
                        airSuspensionToggleUp = true;
                    if (airSuspensionToggleUp)
                        airSuspensionMode += 1;
                    else
                        airSuspensionMode -= 1;
                    if (airSuspensionMode == 1)
                        airSuspensionDesiredForce = -0.16f;
                    if (airSuspensionMode == 2)
                        airSuspensionDesiredForce = 0.00f;
                    if (airSuspensionMode == 3)
                        airSuspensionDesiredForce = 0.16f;
                    adjustAirSuspension = true;
                }
                if (adjustAirSuspension && Game.Player.Character.IsInVehicle(veh))
                {
                    if (airSuspensionToggleUp)
                        airSuspensionForce += 0.0004f;
                    else
                        airSuspensionForce -= 0.0004f;
                    if (airSuspensionForce < airSuspensionDesiredForce && !airSuspensionToggleUp)
                    {
                        airSuspensionForce = airSuspensionDesiredForce;
                        adjustAirSuspension = false;
                    }
                    if (airSuspensionForce > airSuspensionDesiredForce && airSuspensionToggleUp)
                    {
                        airSuspensionForce = airSuspensionDesiredForce;
                        adjustAirSuspension = false;
                    }
                }
                if ((airSuspensionMode != 2 && !veh.IsInAir) || airSuspensionForce != airSuspensionDesiredForce)
                {
                    veh.ApplyForce(new Vector3(0, 0, airSuspensionForce), new Vector3());
                }
            }
        }
        private bool ptsinit = false;

        private void PorscheWing(bool force = false)
        {
            var n = veh.DisplayName.ToLower();
            if (n.Contains("pts") || n.Contains("720"))
            {
                if (!ptsinit || force)
                {
                    Function.Call((Hash)0xF8EBCCC96ADB9FB7, veh, 1.0f, true);
                }
                ptsinit = true;
                Game.DisableControlThisFrame(0, GTA.Control.VehicleMoveUpDown);
            }
            if (n.Contains("flyin") && !isCruiseControlActive && (((fakeSpeed > 40 && fakeSpeed < 200) && GamePad.GetState(PlayerIndex.One).Triggers.Left < 0.5f)))
            {
                veh.Repair();
            }

        }

        private static bool wasBrakeReleased = true;
        private void AdditionalLights()
        {
            var n = veh.DisplayName.ToLower();
            PorscheWing();

            if ((!isCentralLockLocked && (!Game.Player.Character.IsInVehicle()) && veh.EngineRunning) || (isParkingGear && Game.Player.Character.IsInVehicle()))
            {
                var maxAcceleration = Function.Call<float>((Hash)(0x5DD35C8D074E57AE), veh);
                if (maxAcceleration > 0.15f)
                {
                    var handles = new List<string>() { "handle_dside_f", "handle_dside_r", "handle_pside_f", "handle_pside_r" };
                    for (var i = 0; i < handles.Count; i++)
                    {
                        var bone = veh.GetBoneCoord(handles[i]);
                        var offset = i > 1 ? -0.02f : 0.02f;
                        var off = veh.GetOffsetFromWorldCoords(bone);
                        var off2 = veh.GetOffsetInWorldCoords(new Vector3(off.X + offset, off.Y, off.Z));
                        World.DrawLightWithRange(off2, System.Drawing.Color.FromArgb(255, 255, 255, 255), 0.2f, 100f);
                    }
                }
            }

            if (veh.EngineRunning)
            {

                // day running lights
                var rot = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 0);
                var rott = -Math.Abs(180 + (-Math.Abs(rot.Z - veh.Rotation.Z)));

                if (rott > -90 || !Game.Player.Character.IsInVehicle())
                {
                    if (n.Contains("traf"))
                    {
                        var white = System.Drawing.Color.FromArgb(255, 255, 255, 255);
                        var orange = System.Drawing.Color.FromArgb(255, 255, 212, 0);
                        var neut = System.Drawing.Color.FromArgb(255, 163, 163, 163);
                        var lcolor = white;
                        var rcolor = white;
                        var lalpha = 255;
                        var ralpha = 255;

                        if (!ledIndicatorsStayStopwatch.IsRunning)
                            ledIndicatorsStayStopwatch.Start();
                        if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 800)
                        {
                            ledIndicatorsStayStopwatch = new Stopwatch();
                            ledIndicatorsStayStopwatch.Start();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                        {
                            ledIndicatorsStayStopwatch = new Stopwatch();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                        {
                            ledIndicatorsStayStopwatch = new Stopwatch();
                        }
                        if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 400)
                        {
                            rcolor = orange;
                        }
                        if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 400)
                        {
                            lcolor = orange;
                        }

                        if ((isRightIndicatorOn || isLeftIndicatorOn) && ledIndicatorsStayStopwatch.ElapsedMilliseconds >= 400)
                        {
                            if (isLeftIndicatorOn)
                            {
                                ralpha = 50;
                            }

                            if (isRightIndicatorOn)
                            {
                                lalpha = 50;
                            }

                        }

                        List<Vector3> offsets = new List<Vector3>()
                    {

                        new Vector3(0.6f, 1.25f, 0.13f),
                        new Vector3(0.83f, 1.18f, 0.155f),

                        new Vector3(0.83f, 1.18f, 0.155f),
                        new Vector3(0.78f, 1.3f, -0.05f),

                        new Vector3(0.78f, 1.3f, -0.05f),
                          new Vector3(0.67f, 1.35f, -0.07f),

                            new Vector3(-0.6f, 1.25f, 0.13f),
                        new Vector3(-0.83f, 1.18f, 0.155f),

                        new Vector3(-0.83f, 1.18f, 0.155f),
                        new Vector3(-0.78f, 1.3f, -0.05f),

                        new Vector3(-0.78f, 1.3f, -0.05f),
                          new Vector3(-0.67f, 1.35f, -0.07f),

                    };

                        var off = 0;

                        for (var i = 0; i < 3; ++i)
                        {
                            var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                            off += 1;
                            var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                            var off2 = -0.012;
                            for (var ii = i == 1 ? 8 : 0; ii < 10; ii++)
                            {
                                Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + off2, point2.X, point2.Y, point2.Z + off2, lcolor.R, lcolor.G, lcolor.B, lalpha);
                                off2 += 0.002;
                            }
                            off++;
                        }
                        for (var i = 3; i < 6; ++i)
                        {
                            var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                            off += 1;
                            var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                            var off2 = -0.012;
                            for (var ii = i == 4 ? 8 : 0; ii < 10; ii++)
                            {
                                Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + off2, point2.X, point2.Y, point2.Z + off2, rcolor.R, rcolor.G, rcolor.B, ralpha);
                                off2 += 0.002;
                            }
                            off++;
                        }
                    }

                    if (n.Contains("crossik"))
                    {

                        var white = System.Drawing.Color.FromArgb(255, 255, 255, 255);
                        var orange = System.Drawing.Color.FromArgb(255, 255, 212, 0);
                        var neut = System.Drawing.Color.FromArgb(255, 163, 163, 163);
                        var lcolor = white;
                        var rcolor = white;
                        var lalpha = 255;
                        var ralpha = 255;

                        if (!ledIndicatorsStayStopwatch.IsRunning)
                            ledIndicatorsStayStopwatch.Start();
                        if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 800)
                        {
                            ledIndicatorsStayStopwatch = new Stopwatch();
                            ledIndicatorsStayStopwatch.Start();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                        {
                            ledIndicatorsStayStopwatch = new Stopwatch();
                        }
                        if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                        {
                            ledIndicatorsStayStopwatch = new Stopwatch();
                        }
                        if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 400)
                        {
                            rcolor = orange;
                        }
                        if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 400)
                        {
                            lcolor = orange;
                        }

                        if ((isRightIndicatorOn || isLeftIndicatorOn) && ledIndicatorsStayStopwatch.ElapsedMilliseconds >= 400)
                        {
                            if (isLeftIndicatorOn)
                            {
                                ralpha = 50;
                            }

                            if (isRightIndicatorOn)
                            {
                                lalpha = 50;
                            }

                        }

                        List<Vector3> offsets = new List<Vector3>()
                    {

                         new Vector3(0.62f, 2.09f, 0.24f),
                        new Vector3(0.68f, 2.06f, 0.244f),

                        new Vector3(0.8f, 1.98f, 0.26f),
                        new Vector3(0.86f, 1.94f, 0.264f),

                        new Vector3(0.62f, 2.12f, 0.17f),
                        new Vector3(0.68f, 2.09f, 0.174f),

                        new Vector3(0.8f, 2.0f, 0.18f),
                        new Vector3(0.86f, 1.97f, 0.184f),

                        new Vector3(-0.62f, 2.09f, 0.24f),
                        new Vector3(-0.68f, 2.06f, 0.244f),

                        new Vector3(-0.8f, 1.98f, 0.26f),
                        new Vector3(-0.86f, 1.94f, 0.264f),

                        new Vector3(-0.62f, 2.12f, 0.17f),
                        new Vector3(-0.68f, 2.09f, 0.174f),

                        new Vector3(-0.8f, 2.0f, 0.18f),
                        new Vector3(-0.86f, 1.97f, 0.184f),

                    };

                        var off = 0;

                        for (var i = 0; i < 4; ++i)
                        {
                            var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                            off += 1;
                            var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                            var off2 = -0.012;
                            for (var ii = 0; ii < 2; ii++)
                            {
                                Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + off2, point2.X, point2.Y, point2.Z + off2, lcolor.R, lcolor.G, lcolor.B, lalpha);
                                off2 += 0.002;
                            }
                            off++;
                        }

                        for (var i = 4; i < 8; ++i)
                        {
                            var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                            off += 1;
                            var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                            var off2 = -0.012;
                            for (var ii = 0; ii < 2; ii++)
                            {
                                Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + off2, point2.X, point2.Y, point2.Z + off2, rcolor.R, rcolor.G, rcolor.B, ralpha);
                                off2 += 0.002;
                            }
                            off++;
                        }
                    }
                }



                //tailights
                if (n.Contains("prius") && (veh.LightsOn || isBrakeLightsOn || isAdditionalBrakeLightOn) && false)
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {
                        new Vector3(0.33f, -1.77f, 0.36f),
                        new Vector3(0.38f, -1.78f, 0.315f),
                        new Vector3(0.38f, -1.78f, 0.315f),
                        new Vector3(0.57f, -1.73f, 0.33f),
                           new Vector3(0.57f, -1.73f, 0.33f),
                              new Vector3(0.55f, -1.69f, 0.44f),
                               new Vector3(-0.33f, -1.77f, 0.36f),
                        new Vector3(-0.38f, -1.78f, 0.315f),
                        new Vector3(-0.38f, -1.78f, 0.315f),
                        new Vector3(-0.57f, -1.73f, 0.33f),
                           new Vector3(-0.57f, -1.73f, 0.33f),
                              new Vector3(-0.55f, -1.69f, 0.44f),

                    };
                    var off = 0;
                    for (var i = 0; i < 6; ++i)
                    {
                        var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                        off += 1;
                        var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.007, point2.X, point2.Y, point2.Z + 0.007, 253, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.007, point2.X, point2.Y, point2.Z - 0.007, 253, 0, 0, 255);
                        off++;
                    }
                }

                if (n.Contains("pace") && false)
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {
                        new Vector3(0.347f, -2.48f, 0.263f),
                        new Vector3(0.38f, -2.48f, 0.21f),
                        new Vector3(0.38f, -2.48f, 0.21f),
                        new Vector3(0.56f, -2.43f, 0.20f),
                        new Vector3(0.56f, -2.43f, 0.20f),
                        new Vector3(0.71f, -2.343f, 0.20f),
                        new Vector3(0.71f, -2.343f, 0.20f),
                        new Vector3(0.72f, -2.29f, 0.26f),
                        new Vector3(0.72f, -2.29f, 0.26f),
                        new Vector3(0.71f, -2.343f, 0.20f),
                        new Vector3(0.71f, -2.343f, 0.20f),
                        new Vector3(0.74f, -2.33f, 0.146f),

                        new Vector3(-0.347f, -2.48f, 0.263f),
                        new Vector3(-0.38f, -2.48f, 0.21f),
                        new Vector3(-0.38f, -2.48f, 0.21f),
                        new Vector3(-0.56f, -2.43f, 0.20f),
                        new Vector3(-0.56f, -2.43f, 0.20f),
                        new Vector3(-0.71f, -2.343f, 0.20f),
                        new Vector3(-0.71f, -2.343f, 0.20f),
                        new Vector3(-0.72f, -2.29f, 0.26f),
                        new Vector3(-0.72f, -2.29f, 0.26f),
                        new Vector3(-0.71f, -2.343f, 0.20f),
                        new Vector3(-0.71f, -2.343f, 0.20f),
                        new Vector3(-0.74f, -2.33f, 0.146f),

                    };
                    var off = 0;
                    var alpha = 70;
                    if ((veh.LightsOn))
                        alpha = 255;
                    for (var i = 0; i < 12; ++i)
                    {
                        var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                        off += 1;
                        var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.007, point2.X, point2.Y, point2.Z + 0.007, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.007, point2.X, point2.Y, point2.Z - 0.007, 240, 0, 0, alpha);
                        off++;
                    }
                }

                if (n.Contains("voltic") && veh.LightsOn)
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {

                        new Vector3(-0.46f, -2.4f, 0.4f),
                        new Vector3(-0.72f, -2.28f, 0.42f),

                        new Vector3(-0.72f, -2.28f, 0.42f),
                        new Vector3(-0.77f, -2.24f, 0.4f),

                          new Vector3(-0.77f, -2.24f, 0.4f),
                        new Vector3(-0.79f, -2.24f, 0.315f),

                           new Vector3(-0.79f, -2.24f, 0.315f),
                          new Vector3(-0.65f, -2.33f, 0.31f),

                           new Vector3(-0.65f, -2.33f, 0.31f),
                            new Vector3(-0.42f, -2.4f, 0.31f),

                            new Vector3(0.46f, -2.4f, 0.4f),
                        new Vector3(0.72f, -2.28f, 0.42f),

                        new Vector3(0.72f, -2.28f, 0.42f),
                        new Vector3(0.77f, -2.24f, 0.4f),

                          new Vector3(0.77f, -2.24f, 0.4f),
                        new Vector3(0.79f, -2.24f, 0.315f),

                           new Vector3(0.79f, -2.24f, 0.315f),
                          new Vector3(0.65f, -2.33f, 0.31f),

                           new Vector3(0.65f, -2.33f, 0.31f),
                            new Vector3(0.42f, -2.4f, 0.31f),
                    };
                    var off = 0;
                    var alpha = 255;
                    for (var i = 0; i < 10; ++i)
                    {
                        var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                        off += 1;
                        var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.004, point2.X, point2.Y, point2.Z + 0.004, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.002, point2.X, point2.Y, point2.Z + 0.002, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.002, point2.X, point2.Y, point2.Z - 0.002, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.004, point2.X, point2.Y, point2.Z - 0.004, 240, 0, 0, alpha);
                        off++;
                    }
                }

                if (n.Contains("clio"))
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {

                        new Vector3(0.39f, -1.95f, 0.25f),
                        new Vector3(0.59f, -1.90f, 0.27f),

                        new Vector3(0.59f, -1.90f, 0.27f),
                        new Vector3(0.74f, -1.72f, 0.28f),

                       new Vector3(0.74f, -1.74f, 0.28f),
                        new Vector3(0.74f, -1.8f, 0.18f),

                        new Vector3(0.74f, -1.8f, 0.18f),
                        new Vector3(0.63f, -1.88f, 0.17f),

                          new Vector3(-0.39f, -1.95f, 0.25f),
                        new Vector3(-0.59f, -1.90f, 0.27f),

                        new Vector3(-0.59f, -1.90f, 0.27f),
                        new Vector3(-0.74f, -1.72f, 0.28f),

                       new Vector3(-0.74f, -1.74f, 0.28f),
                        new Vector3(-0.74f, -1.8f, 0.18f),

                        new Vector3(-0.74f, -1.8f, 0.18f),
                        new Vector3(-0.63f, -1.88f, 0.17f),

                    };
                    var off = 0;
                    var alpha = 70;
                    if ((veh.LightsOn))
                        alpha = 255;
                    for (var i = 0; i < 8; ++i)
                    {
                        var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                        off += 1;
                        var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.004, point2.X, point2.Y, point2.Z + 0.004, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.002, point2.X, point2.Y, point2.Z + 0.002, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 240, 0, 0, alpha);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.002, point2.X, point2.Y, point2.Z - 0.002, 240, 0, 0, alpha);
                        off++;
                    }
                }

                if (n.Contains("clio") && (isBrakeLightsOn || isAdditionalBrakeLightOn) && Game.Player.Character.IsInVehicle(veh))
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {

                    new Vector3(0.51f, -1.92f, 0.23f),
                        new Vector3(0.60f, -1.9f, 0.24f),

                          new Vector3(-0.51f, -1.92f, 0.23f),
                        new Vector3(-0.60f, -1.9f, 0.24f),


                    };
                    var off = 0;
                    for (var i = 0; i < 2; ++i)
                    {
                        var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                        off += 1;
                        var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.002, point2.X, point2.Y, point2.Z + 0.002, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.002, point2.X, point2.Y, point2.Z - 0.002, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.004, point2.X, point2.Y, point2.Z - 0.004, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 240, 0, 0, 255);
                        off++;
                    }
                }

                if (n.Contains("pace") && (isBrakeLightsOn || isAdditionalBrakeLightOn) && Game.Player.Character.IsInVehicle(veh) && false)
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {

                        new Vector3(0.38f, -2.48f, 0.25f),
                        new Vector3(0.56f, -2.43f, 0.24f),
                        new Vector3(0.56f, -2.43f, 0.24f),
                        new Vector3(0.71f, -2.343f, 0.24f),


                       new Vector3(-0.38f, -2.48f, 0.25f),
                        new Vector3(-0.56f, -2.43f, 0.24f),
                        new Vector3(-0.56f, -2.43f, 0.24f),
                        new Vector3(-0.71f, -2.343f, 0.24f),

                    };
                    var off = 0;
                    for (var i = 0; i < 4; ++i)
                    {
                        var point1 = veh.GetOffsetInWorldCoords(offsets[off]);
                        off += 1;
                        var point2 = veh.GetOffsetInWorldCoords(offsets[off]);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + 0.003, point2.X, point2.Y, point2.Z + 0.003, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 240, 0, 0, 255);
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.009, point2.X, point2.Y, point2.Z - 0.009, 240, 0, 0, 255);
                        off++;
                    }
                }

                if (n.Contains("rogue") && (veh.LightsOn)) // old
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {
                        new Vector3(-0.84f, -3.14f, 0.48f),
                        new Vector3(-0.87f, -3.13f, 0.51f),
                        new Vector3(-0.87f, -3.13f, 0.51f),
                        new Vector3(-0.818f, -3.086f, 1.03f),

                          new Vector3(0.84f, -3.14f, 0.48f),
                        new Vector3(0.87f, -3.13f, 0.51f),
                        new Vector3(0.87f, -3.13f, 0.51f),
                        new Vector3(0.818f, -3.086f, 1.03f),

                    };
                    var off = 0;
                    for (var i = 0; i < 4; ++i)
                    {
                        var off2 = 0.003f;
                        var off3 = 0.0f;
                        for (var ii = 0; ii < 10; ++ii)
                        {
                            var init1 = offsets[off];
                            var init2 = offsets[off + 1];
                            if (i == 1 || i == 3)
                            {
                                var point1 = veh.GetOffsetInWorldCoords(new Vector3(init1.X + off3, init1.Y, init1.Z + off2));
                                var point2 = veh.GetOffsetInWorldCoords(new Vector3(init2.X + off3, init2.Y, init2.Z + off2));
                                Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 247, 42, 60, 255);
                            }

                            off2 -= 0.003f;
                            if (i == 1 || i == 3)
                            {
                                off3 = i == 1 ? off3 + 0.003f : off3 - 0.003f;
                            }
                        }
                        off++;
                        off++;
                    }
                }
                if (n.Contains("rogue") && (veh.LightsOn) && false)
                {
                    List<Vector3> offsets = new List<Vector3>()
                    {
                        new Vector3(-0.88f, -2.44f, 0.25f),
                        new Vector3(-0.88f, -2.44f, 0.25f),
                        new Vector3(-0.88f, -2.44f, 0.25f),
                        new Vector3(-0.88f, -2.43f, 0.38f),

                        new Vector3(0.88f, -2.44f, 0.25f),
                        new Vector3(0.88f, -2.44f, 0.25f),
                        new Vector3(0.88f, -2.44f, 0.25f),
                        new Vector3(0.88f, -2.43f, 0.38f),

                    };
                    var off = 0;
                    for (var i = 0; i < 4; ++i)
                    {

                        var off2 = 0.005f;
                        var off3 = 0.0f;
                        for (var ii = 0; ii < 16; ++ii)
                        {
                            var init1 = offsets[off];
                            var init2 = offsets[off + 1];
                            var point1 = veh.GetOffsetInWorldCoords(new Vector3(init1.X + off3, init1.Y, init1.Z + off2));
                            var point2 = veh.GetOffsetInWorldCoords(new Vector3(init2.X + off3, init2.Y, init2.Z + off2));
                            Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                            off2 -= 0.0025f;
                            if (i == 1 || i == 3)
                            {
                                off3 = i == 1 ? off3 + 0.005f : off3 - 0.005f;
                            }
                        }
                        off++;
                        off++;
                    }

                    var rlPos = veh.GetOffsetInWorldCoords(new Vector3(-0.78f, -2.2f, -0.38f));
                    var rrPos = veh.GetOffsetInWorldCoords(new Vector3(0.78f, -2.2f, -0.38f));
                    var rlPos2 = veh.GetOffsetInWorldCoords(new Vector3(-0.49f, -2.2f, -0.38f));
                    var rrPos2 = veh.GetOffsetInWorldCoords(new Vector3(0.49f, -2.2f, -0.38f));
                    var rlPos3 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -2.2f, -0.38f));
                    var rrPos3 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -2.2f, -0.38f));
                    var rrAlpha = 0.4f;
                    var rAlpha = 60f;
                    if (isBrakeLightsOn || isAdditionalBrakeLightOn)
                    {
                        rAlpha = 300f;
                        rrAlpha = 1f;
                    }

                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos.X, rlPos.Y, rlPos.Z, 253, 0, 0, 0.48f, rrAlpha);
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos.X, rrPos.Y, rrPos.Z, 253, 0, 0, 0.48f, rrAlpha);

                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos2.X, rlPos2.Y, rlPos2.Z, 253, 0, 0, 0.48f, rrAlpha);
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos2.X, rrPos2.Y, rrPos2.Z, 253, 0, 0, 0.48f, rrAlpha);

                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos3.X, rlPos3.Y, rlPos3.Z, 253, 0, 0, 0.48f, rrAlpha);
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos3.X, rrPos3.Y, rrPos3.Z, 253, 0, 0, 0.48f, rrAlpha);
                }
                if (n.Contains("speedo"))
                {
                    if ((veh.LightsOn || isBrakeLightsOn || isAdditionalBrakeLightOn))
                    {
                        var lPos = veh.GetOffsetInWorldCoords(new Vector3(-0.62f, -2.57f, -0.12f));
                        var rPos = veh.GetOffsetInWorldCoords(new Vector3(0.62f, -2.57f, -0.12f));
                        var rlPos = veh.GetOffsetInWorldCoords(new Vector3(-0.78f, -2.6f, -0.22f));
                        var rrPos = veh.GetOffsetInWorldCoords(new Vector3(0.78f, -2.6f, -0.22f));
                        var rlPos2 = veh.GetOffsetInWorldCoords(new Vector3(-0.49f, -2.6f, -0.22f));
                        var rrPos2 = veh.GetOffsetInWorldCoords(new Vector3(0.49f, -2.6f, -0.38f));
                        var rlPos3 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -2.6f, -0.38f));
                        var rrPos3 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -2.6f, -0.38f));
                        var rrAlpha = 0.4f;
                        var rAlpha = 60f;
                        if (isBrakeLightsOn || isAdditionalBrakeLightOn)
                        {
                            rAlpha = 300f;
                            rrAlpha = 1f;
                        }
                        //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lPos.X, lPos.Y, lPos.Z, 253, 0, 0, 0.125f, rAlpha);
                        //   Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rPos.X, rPos.Y, rPos.Z, 253, 0, 0, 0.125f, rAlpha);

                        /*  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos.X, rlPos.Y, rlPos.Z, 253, 0, 0, 0.48f, rrAlpha);
                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos.X, rrPos.Y, rrPos.Z, 253, 0, 0, 0.48f, rrAlpha);

                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos2.X, rlPos2.Y, rlPos2.Z, 253, 0, 0, 0.48f, rrAlpha);
                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos2.X, rrPos2.Y, rrPos2.Z, 253, 0, 0, 0.48f, rrAlpha);

                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos3.X, rlPos3.Y, rlPos3.Z, 253, 0, 0, 0.48f, rrAlpha);
                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos3.X, rrPos3.Y, rrPos3.Z, 253, 0, 0, 0.48f, rrAlpha);*/
                    }
                    if (!ledIndicatorsStayStopwatch.IsRunning)
                        ledIndicatorsStayStopwatch.Start();
                    if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 800)
                    {
                        ledIndicatorsStayStopwatch = new Stopwatch();
                        ledIndicatorsStayStopwatch.Start();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                    {
                        ledIndicatorsStayStopwatch = new Stopwatch();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                    {
                        ledIndicatorsStayStopwatch = new Stopwatch();
                    }
                    if (true)
                    {
                        veh.LeftIndicatorLightOn = false;
                        veh.RightIndicatorLightOn = false;
                    }
                    var rxAlpha = ledIndicatorsStayStopwatch.ElapsedMilliseconds > 255 ? 500 - ledIndicatorsStayStopwatch.ElapsedMilliseconds : ledIndicatorsStayStopwatch.ElapsedMilliseconds;
                    if (isLeftIndicatorOn)
                    {
                        var lPos = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -4.08f, -0.46f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lPos.X, lPos.Y, lPos.Z, 255, 93, 0, 0.09f, rxAlpha);
                    }
                    if (isRightIndicatorOn)
                    {
                        var lPos = veh.GetOffsetInWorldCoords(new Vector3(0.6f, -4.08f, -0.46f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lPos.X, lPos.Y, lPos.Z, 255, 93, 0, 0.09f, rxAlpha);
                    }
                }

                if (n.Contains("rogue"))
                {
                    if ((veh.LightsOn || isBrakeLightsOn || isAdditionalBrakeLightOn))
                    {
                        var lPos = veh.GetOffsetInWorldCoords(new Vector3(-0.5f, -2.57f, -0.4f));
                        var rPos = veh.GetOffsetInWorldCoords(new Vector3(0.5f, -2.57f, -0.4f));
                        var rlPos = veh.GetOffsetInWorldCoords(new Vector3(-0.78f, -2.6f, -0.22f));
                        var rrPos = veh.GetOffsetInWorldCoords(new Vector3(0.78f, -2.6f, -0.22f));
                        var rlPos2 = veh.GetOffsetInWorldCoords(new Vector3(-0.49f, -2.6f, -0.22f));
                        var rrPos2 = veh.GetOffsetInWorldCoords(new Vector3(0.49f, -2.6f, -0.38f));
                        var rlPos3 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -2.6f, -0.38f));
                        var rrPos3 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -2.6f, -0.38f));
                        var rrAlpha = 0.4f;
                        var rAlpha = 60f;
                        if (isBrakeLightsOn || isAdditionalBrakeLightOn)
                        {
                            rAlpha = 300f;
                            rrAlpha = 1f;
                        }
                        //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lPos.X, lPos.Y, lPos.Z, 253, 0, 0, 0.125f, rAlpha);
                        //   Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rPos.X, rPos.Y, rPos.Z, 253, 0, 0, 0.125f, rAlpha);

                        /*  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos.X, rlPos.Y, rlPos.Z, 253, 0, 0, 0.48f, rrAlpha);
                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos.X, rrPos.Y, rrPos.Z, 253, 0, 0, 0.48f, rrAlpha);

                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos2.X, rlPos2.Y, rlPos2.Z, 253, 0, 0, 0.48f, rrAlpha);
                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos2.X, rrPos2.Y, rrPos2.Z, 253, 0, 0, 0.48f, rrAlpha);

                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rlPos3.X, rlPos3.Y, rlPos3.Z, 253, 0, 0, 0.48f, rrAlpha);
                          Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rrPos3.X, rrPos3.Y, rrPos3.Z, 253, 0, 0, 0.48f, rrAlpha);*/
                    }
                    if (!ledIndicatorsStayStopwatch.IsRunning)
                        ledIndicatorsStayStopwatch.Start();
                    if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 800)
                    {
                        ledIndicatorsStayStopwatch = new Stopwatch();
                        ledIndicatorsStayStopwatch.Start();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                    {
                        ledIndicatorsStayStopwatch = new Stopwatch();
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                    {
                        ledIndicatorsStayStopwatch = new Stopwatch();
                    }
                    if (true)
                    {
                        veh.LeftIndicatorLightOn = false;
                        veh.RightIndicatorLightOn = false;
                    }
                    var rxAlpha = ledIndicatorsStayStopwatch.ElapsedMilliseconds > 255 ? 500 - ledIndicatorsStayStopwatch.ElapsedMilliseconds : ledIndicatorsStayStopwatch.ElapsedMilliseconds;
                    if (isLeftIndicatorOn)
                    {
                        var lPos = veh.GetOffsetInWorldCoords(new Vector3(-0.88f, -2.4f, 0.355f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lPos.X, lPos.Y, lPos.Z, 255, 93, 0, 0.09f, rxAlpha);
                    }
                    if (isRightIndicatorOn)
                    {
                        var lPos = veh.GetOffsetInWorldCoords(new Vector3(0.92f, -2.43f, 0.355f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lPos.X, lPos.Y, lPos.Z, 255, 93, 0, 0.09f, rxAlpha);
                    }
                }

                if (veh.LightsOn)
                {
                    // plate lights 
                    if (n.Contains("rx450"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.5f, 0.48f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.5f, 0.48f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.17f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.17f, 0.5f);
                    }
                    if (n.Contains("pace"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.12f, 0.2f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.12f, 0.2f));
                        //  Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.16f, 0.35f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.16f, 0.35f);
                    }
                    if (n.Contains("as7"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.35f, 0.1f));
                        // var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.35f, 0.1f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.16f, 0.7f);
                        //   Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 250, 171, 80, 0.16f, 0.35f);
                    }
                    if (n.Contains("gle"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.52f, -0.21f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.52f, -0.21f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.16f, 0.35f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.16f, 0.35f);
                    }
                    if (n.Contains("supr"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.14f, -2.1f, 0.1f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.14f, -2.1f, 0.1f));
                         //  Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.5f);
                    }
                    if (n.Contains("xc40"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.12f, -1.85f, -0.12f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.12f, -1.85f, -0.12f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.18f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.18f, 0.5f);
                    }
                    if (n.Contains("porcay"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.11f, -2.45f, 0.71f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.11f, -2.45f, 0.71f));
                        //  Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.5f);
                    }
                    if (n.Contains("touareg"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.11f, -2.46f, 0.2f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.11f, -2.46f, 0.2f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.5f);
                    }
                    if (n.Contains("c300"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.11f, -2.14f, 0.07f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.11f, -2.14f, 0.07f));
                        //   Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.19f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.19f, 0.5f);

                        //blend light 
                        /* var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.44f, -2.148f, 0.258f));
                         var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.44f, -2.148f, 0.258f));
                         var off = 0f;
                         for (var i = 0; i < 4; i++)
                         {
                             Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z + off, point2.X, point2.Y, point2.Z + off, 226, 0, 0, 255);
                             off += 0.002f;
                         }*/
                    }
                    if (n.Contains("m8"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.10f, -2.39f, 0.23f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.10f, -2.39f, 0.23f));
                       // Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.19f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.19f, 0.5f);
                    }
                    if (n.Contains("fpace"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.14f, -2.39f, 0.24f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.14f, -2.39f, 0.24f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.2f, 0.6f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.2f, 0.6f);
                    }
                    if (n.Contains("rs5"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.32f, 0.11f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.32f, 0.11f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.16f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.16f, 0.5f);
                    }
                    if (n.Contains("toycam"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.14f, -2.17f, 0.31f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.14f, -2.17f, 0.31f));
                        //  Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.24f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.24f, 0.5f);
                    }
                    if (n.Contains("720"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.1f, -2.25f, 0.27f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.1f, -2.25f, 0.27f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.24f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.24f, 0.5f);
                    }
                    if (n.Contains("qashqa"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.1f, 0.1f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.1f, 0.1f));
                        //    Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.17f, 0.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.17f, 0.3f);
                    }
                    if (n.Contains("mk1rabbit"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.12f, -2.14f, 0.057f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.12f, -2.14f, 0.057f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.21f, 3.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.21f, 3.5f);
                    }
                    if (n.Contains("m5xx"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.12f, -2.28f, 0.1f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.12f, -2.28f, 0.1f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.17f, 1.9f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.17f, 1.9f);
                    }
                    if (n.Contains("q8"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.14f, -2.40f, 0.12f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.14f, -2.40f, 0.12f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.2f, 0.6f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.2f, 0.6f);
                    }
                    if (n.Contains("oyc"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.15f, -2.32f, -0.03f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.15f, -2.32f, -0.03f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.4f, 0.4f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.4f, 0.4f);
                    }
                    if (n.Contains("superb"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.12f, -2.41f, 0.03f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.12f, -2.41f, 0.03f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 2.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 2.3f);
                    }
                    if (n.Contains("xc90") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.28f, 0.57f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.28f, 0.57f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.19f, 1.0f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.19f, 1.0f);

                    }
                    if (n.Contains("speedo"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.4f, -2.96f, -0.21f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.3f, -2.96f, -0.21f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.3f);
                    }
                    if (n.Contains("hm3f"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.13f, -2.3f, 0.24f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.13f, -2.3f, 0.24f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 158, 206, 255, 0.22f, 0.2f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 158, 206, 255, 0.22f, 0.2f);
                    }
                    if (n.Contains("svr14"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.04f, -2.42f, 0.19f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.04f, -2.42f, 0.19f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.4f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.4f);
                    }
                    if (n.Contains("a6"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.04f, -2.53f, 0.37f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.04f, -2.53f, 0.37f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.3f);
                    }
                    if (n.Contains("s560"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.55f, 0.23f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.55f, 0.23f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.3f);
                    }
                    if (n.Contains("s500"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.11f, -2.41f, 0.23f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.11f, -2.41f, 0.23f));
                        //  Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.3f);

                    }
                    if (n.Contains("e300"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.04f, -2.58f, 0.34f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.04f, -2.58f, 0.34f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.5f);
                    }
                    if (n.Contains("e-class"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.07f, -2.53f, 0.18f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.07f, -2.53f, 0.18f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.5f);
                    }
                    if (n.Contains("ch-r"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.09f, 0.3f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.09f, 0.3f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 250, 215, 149, 0.22f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 250, 215, 149, 0.22f, 0.5f);
                    }
                    if (n.Contains("oycre"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.14f, -2.58f, 0.2f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.14f, -2.58f, 0.2f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.2f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.2f);
                    }
                    if (n.Contains("crossik"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.11f, -2.48f, 0.0f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.11f, -2.48f, 0.0f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.2f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.2f);
                    }
                    if (n.Contains("voltic"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.11f, -2.34f, 0.1f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.11f, -2.34f, 0.1f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.3f);

                    }
                    if (n.Contains("sq7"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.14f, -2.28f, 0.1f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.14f, -2.28f, 0.1f));
                        //Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.3f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.3f);

                    }

                    if (n.Contains("expedition"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.04f, -7.58f, -1.2f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.04f, -7.58f, -1.2f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.5f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.5f);

                    }
                    if (n.Contains("panamera"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.64f, -0.29f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.64f, -0.29f));
                        // Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.2f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.2f);

                    }
                    if (n.Contains("clio"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.03f, -0.14f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.03f, -0.14f));
                        // Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.2f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.2f);

                    }
                    if (n.Contains("e-class") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.4f, -2.5f, 0.43f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.4f, -2.5f, 0.43f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 250, 215, 149, 0.22f, 0.34f);

                    }
                    if (n.Contains("rover"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.54f, 0.08f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.54f, 0.08f));
                        //  Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 250, 171, 80, 0.25f, 0.39f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 250, 171, 80, 0.25f, 0.39f);

                    }
                    if (n.Contains("vela"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.14f, -2.24f, 0.18f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.14f, -2.24f, 0.18f));
                        //Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.34f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.34f);
                    }
                    if (n.Contains("paj"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.46f, -4.045f, -0.11f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.34f, -4.045f, -0.11f));
                        // Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 5f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 250, 171, 80, 0.14f, 0.16f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 250, 171, 80, 0.14f, 0.16f);
                    }
                    if (n.Contains("sahar"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.15f, 0.064f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.080f, -2.15f, 0.064f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.18f, 0.17f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.18f, 0.17f);
                    }
                    if (n.Contains("x6"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.45f, 0.27f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.080f, -2.45f, 0.27f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.18f, 0.17f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.18f, 0.17f);
                    }
                    if (n.Contains("pts21"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.29f, 0.0f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.29f, 0.0f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.2f, 0.17f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.2f, 0.17f);
                    }

                    if (n.Contains("ibiza"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.08f, -2.09f, -0.35f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.08f, -2.09f, -0.35f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.2f, 0.17f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.2f, 0.17f);
                    }
                    if (n.Contains("x7"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.66f, 0.32f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.68f, 0.32f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.34f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.34f);

                    }
                    if (n.Contains("gle") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.32f, 0.03f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.32f, 0.03f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.34f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.34f);

                    }
                    if (n.Contains("golf") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.16f, -2.33f, 0.4f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.16f, -2.33f, 0.4f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.22f, 0.74f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.22f, 0.74f);


                    }
                    if (n.Contains("prius") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.12f, -1.83f, -0.04f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.12f, -1.83f, -0.04f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.12f, 0.74f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.12f, 0.74f);
                        //   Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 4f), 255, 5, 5, 255);
                    }

                    if (n.Contains("caddy"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.13f, -2.17f, 0.095f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.13f, -2.17f, 0.095f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 250, 171, 80, 0.15f, 0.34f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 250, 171, 80, 0.15f, 0.34f);
                        if (tempPlate == null && false)
                        {
                            var plat = World.CreateProp(new Model(2075235594), new Vector3(0, 0, 0), false, false);
                            plat.AttachTo(veh, 0, new Vector3(-0.0f, -2.57f, -0.12f), new Vector3(-15, 0, 0));
                            tempPlate = plat;
                        }
                    }
                    if (n.Contains("pace") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.13f, -2.36f, 0.17f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.13f, -2.36f, 0.17f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 250, 171, 80, 0.24f, 0.14f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 250, 171, 80, 0.24f, 0.14f);
                        // Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 4f), 255, 5, 5, 255);
                    }
                    if (n.Contains("xj"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.67f, -0.2f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.67f, -0.2f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.24f, 0.14f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.24f, 0.14f);
                    }

                    if (n.Contains("paj") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.072f, -2.04f, 0.35f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.072f, -2.04f, 0.35f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.13f, 0.24f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.13f, 0.24f);
                    }
                    if (n.Contains("q30"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.1f, -2.234f, 0.35f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(0.1f, -2.234f, 0.35f));
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.36f, 0.24f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.36f, 0.24f);
                    }
                    if (n.Contains("rogue"))
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(-0.51f, -3.24f, -0.12f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.32f, -3.24f, -0.12f));
                        //  Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 4f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 255, 255, 0.33f, 0.1f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 255, 255, 0.33f, 0.1f);
                    }
                    if (n.Contains("rogue") && false)
                    {
                        var light1 = veh.GetOffsetInWorldCoords(new Vector3(0.13f, -2.48f, 0.36f));
                        var light2 = veh.GetOffsetInWorldCoords(new Vector3(-0.13f, -2.48f, 0.36f));
                        var light3 = veh.GetOffsetInWorldCoords(new Vector3(-0.0f, -2.48f, 0.36f));
                        //    Function.Call((Hash)0x6B7256074AE34680, light1.X, light1.Y, light1.Z, light1.X, light1.Y, (light1.Z + 4f), 255, 5, 5, 255);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light1.X, light1.Y, light1.Z, 255, 241, 173, 0.63f, 0.1f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light2.X, light2.Y, light2.Z, 255, 241, 173, 0.63f, 0.1f);
                        Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, light3.X, light3.Y, light3.Z, 255, 241, 173, 0.63f, 0.1f);
                    }
                }
            }

            //brake lights
            if (n.Contains("rogxue") && false)
            {
                var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.28f, -3.13f, 1.29f));
                var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.28f, -3.13f, 1.29f));
                var off = 0f;
                var off2 = 0f;
                for (var i = 0; i < 44; ++i)
                {
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y + off2, point1.Z - off, point2.X, point2.Y + off2, point2.Z - off, 0, 0, 0, 255);
                    off += 0.003f;
                    off2 += 0.0015f;
                }
            }

            if (isAdditionalBrakeLightOn && Game.Player.Character.IsInVehicle(veh))
            {
                if (n.Contains("qashqa"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.8f, 0.66f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.8f, 0.66f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("xc40"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.15f, -1.55f, 0.62f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.15f, -1.55f, 0.62f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("rs5"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.4f, 0.56f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.4f, 0.56f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("e-clas"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.35f, 0.62f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.35f, 0.62f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("fpace"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.36f, -2.1f, 0.66f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.36f, -2.1f, 0.66f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("c300") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.21f, -1.67f, 0.78f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.21f, -1.67f, 0.78f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("m3"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -1.94f, 0.83f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -1.94f, 0.83f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("supr"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.15f, -1.22f, 0.59f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.15f, -1.22f, 0.59f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("promas"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -2.78f, 1.16f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -2.78f, 1.16f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("touareg"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.17f, -3.015f, 1.57f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.17f, -3.015f, 1.57f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("porsche"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.26f, -1.62f, 0.76f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.26f, -1.62f, 0.76f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("cls"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.426f, 0.78f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.426f, 0.78f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("sq7"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.16f, -1.85f, 0.67f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.16f, -1.85f, 0.67f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("sahara"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -2.33f, 1.24f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -2.33f, 1.24f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("gcmtes"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.02f, 0.81f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.02f, 0.81f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);

                    var point1l = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.02f, 0.81f));
                    var point2l = veh.GetOffsetInWorldCoords(new Vector3(0.51f, -1.02f, 0.785f));
                    Function.Call((Hash)0x6B7256074AE34680, point1l.X, point1l.Y, point1l.Z, point2l.X, point2l.Y, point2l.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1l.X, point1l.Y, point1l.Z - 0.003, point2l.X, point2l.Y, point2l.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1l.X, point1l.Y, point1l.Z - 0.006, point2l.X, point2l.Y, point2l.Z - 0.006, 253, 0, 0, 255);

                    var point1r = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.02f, 0.81f));
                    var point2r = veh.GetOffsetInWorldCoords(new Vector3(-0.51f, -1.02f, 0.785f));
                    Function.Call((Hash)0x6B7256074AE34680, point1r.X, point1r.Y, point1r.Z, point2r.X, point2r.Y, point2r.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1r.X, point1r.Y, point1r.Z - 0.003, point2r.X, point2r.Y, point2r.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1r.X, point1r.Y, point1r.Z - 0.006, point2r.X, point2r.Y, point2r.Z - 0.006, 253, 0, 0, 255);

                }

                if (n.Contains("pace") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.98f, 0.69f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.98f, 0.69f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("63") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -2.15f, 0.71f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -2.15f, 0.71f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("xc90") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -1.89f, 1.07f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -1.89f, 1.07f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("prius") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.16f, -1.5f, 0.78f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.16f, -1.5f, 0.78f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("rogue"))
                {

                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.24f, -3.11f, 1.27f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.24f, -3.11f, 1.27f));
                    var off = 0f;
                    for (var i = 0; i < 8; ++i)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - off, point2.X, point2.Y, point2.Z - off, 253, 0, 0, 255);
                        off += 0.003f;
                    }
                }
                if (n.Contains("caddy") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(-0.09f, -2.14f, 0.67f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(0.09f, -2.14f, 0.67f));
                    var off = point1.Z;
                    for (var i = 0; i < 12; ++i)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, off, point2.X, point2.Y, off, 253, 0, 0, 255);
                        off -= 0.003f;
                    }
                }
                if (n.Contains("speedo"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -2.9f, 1.4f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -2.9f, 1.4f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 254, 4, 46, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 254, 4, 46, 255);
                }
                if (n.Contains("a6"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -1.93f, 0.92f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -1.93f, 0.92f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("a8"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.21f, -1.955f, 0.54f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.21f, -1.955f, 0.54f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("a7"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.21f, -1.34f, 0.68f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.21f, -1.34f, 0.68f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("i4"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.21f, -1.334f, 1.075f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.21f, -1.334f, 1.075f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("m8"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.15f, -1.212f, 0.835f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.15f, -1.212f, 0.835f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("ibiza"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.16f, -1.56f, 0.55f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.16f, -1.56f, 0.55f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("s500"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.21f, -1.334f, 0.68f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.21f, -1.334f, 0.68f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);

                }

                if (n.Contains("cayenneturb"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.22f, -1.99f, 0.717f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.22f, -1.99f, 0.717f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("q8") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -1.83f, 0.76f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -1.83f, 0.76f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("gmax"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.22f, -2.09f, 1.36f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.22f, -2.09f, 1.36f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }

                if (n.Contains("s560"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.18f, -1.42f, 0.73f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.18f, -1.42f, 0.73f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("exped") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.6f, -7.52f, 1.67f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -7.52f, 1.67f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                }
                if (n.Contains("x7"))
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.2f, -2.33f, 1.0f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.2f, -2.33f, 1.0f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 253, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 253, 0, 0, 255);
                }
                if (n.Contains("e-class") && false)
                {
                    var point1 = veh.GetOffsetInWorldCoords(new Vector3(0.42f, -2.176f, 0.92f));
                    var point2 = veh.GetOffsetInWorldCoords(new Vector3(-0.42f, -2.176f, 0.92f));
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.003, point2.X, point2.Y, point2.Z - 0.003, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.006, point2.X, point2.Y, point2.Z - 0.006, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.009, point2.X, point2.Y, point2.Z - 0.009, 221, 0, 0, 255);
                    Function.Call((Hash)0x6B7256074AE34680, point1.X, point1.Y, point1.Z - 0.0012, point2.X, point2.Y, point2.Z - 0.012, 221, 0, 0, 255);
                }
            }
            // indicators 
            if (n.Contains("sq7"))
            {
                veh.LeftIndicatorLightOn = false;
                veh.RightIndicatorLightOn = false;
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 800)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 400)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.4f, -2.26f, 0.163f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.2f, 0.165f));

                    var sidStart = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.2f, 0.165f));
                    var sidEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.73f, -2.11f, 0.165f));

                    var off = 0f;
                    for (var i = 0; i < 6; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, sidStart.X, sidStart.Y, sidStart.Z - off, sidEnd.X, sidEnd.Y, sidEnd.Z - off, 255, 252, 15, 255);
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 400)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.4f, -2.26f, 0.163f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.6f, -2.2f, 0.165f));

                    var sidStart = veh.GetOffsetInWorldCoords(new Vector3(0.6f, -2.2f, 0.165f));
                    var sidEnd = veh.GetOffsetInWorldCoords(new Vector3(0.73f, -2.11f, 0.165f));

                    var off = 0f;
                    for (var i = 0; i < 6; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, sidStart.X, sidStart.Y, sidStart.Z - off, sidEnd.X, sidEnd.Y, sidEnd.Z - off, 255, 252, 15, 255);
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }

                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
            }
            if (n.Contains("m5xx"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.40f, -2.26f, 0.16f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.74f, -2.12f, 0.170f));
                    Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - 0.001, leftEnd.X, leftEnd.Y, leftEnd.Z - 0.001, 255, 252, 15, 255);
                    Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - 0.002, leftEnd.X, leftEnd.Y, leftEnd.Z - 0.002, 255, 252, 15, 255);
                    Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z + 0.001, leftEnd.X, leftEnd.Y, leftEnd.Z + 0.001, 255, 252, 15, 255);
                    Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z + 0.002, leftEnd.X, leftEnd.Y, leftEnd.Z + 0.002, 255, 252, 15, 255);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var rightStart = veh.GetOffsetInWorldCoords(new Vector3(0.40f, -2.28f, 0.16f));
                    var rightEnd = veh.GetOffsetInWorldCoords(new Vector3(0.74f, -2.12f, 0.170f));
                    Function.Call((Hash)0x6B7256074AE34680, rightStart.X, rightStart.Y, rightStart.Z - 0.001, rightEnd.X, rightEnd.Y, rightEnd.Z - 0.001, 255, 252, 15, 255);
                    Function.Call((Hash)0x6B7256074AE34680, rightStart.X, rightStart.Y, rightStart.Z - 0.002, rightEnd.X, rightEnd.Y, rightEnd.Z - 0.002, 255, 252, 15, 255);
                    Function.Call((Hash)0x6B7256074AE34680, rightStart.X, rightStart.Y, rightStart.Z + 0.001, rightEnd.X, rightEnd.Y, rightEnd.Z + 0.001, 255, 252, 15, 255);
                    Function.Call((Hash)0x6B7256074AE34680, rightStart.X, rightStart.Y, rightStart.Z + 0.002, rightEnd.X, rightEnd.Y, rightEnd.Z + 0.002, 255, 252, 15, 255);
                }
            }



            if (n.Contains("caddy") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }

                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var lPos = veh.GetOffsetInWorldCoords(new Vector3(-0.78f, -2.32f, -0.09f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lPos.X, lPos.Y, lPos.Z, 215, 123, 9, 0.12f, 350f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var rPos = veh.GetOffsetInWorldCoords(new Vector3(0.78f, -2.32f, -0.09f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, rPos.X, rPos.Y, rPos.Z, 215, 123, 9, 0.12f, 350f);
                }
            }
            if (n.Contains("fpace"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 750)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.34f, -2.39f, 0.265f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.71f, -2.28f, 0.265f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.34f, -2.39f, 0.265f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.71f, -2.28f, 0.265f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("a6"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.4f, -2.53f, 0.46f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.72f, -2.37f, 0.475f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.4f, -2.53f, 0.46f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.72f, -2.37f, 0.475f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("gcmtes"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 750)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.68f, -1.89f, 0.38f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.72f, -1.87f, 0.38f));
                    var off = 0f;
                    for (var i = 0; i < 12; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.68f, -1.89f, 0.38f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.72f, -1.87f, 0.38f));
                    var off = 0f;
                    for (var i = 0; i < 12; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
            }
            if (n.Contains("supr"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 750)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -1.99f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.64f, -1.97f, 0.2f));
                    var off = 0f;
                    for (var i = 0; i < 8; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.6f, -1.99f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.64f, -1.97f, 0.2f));
                    var off = 0f;
                    for (var i = 0; i < 8; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
            }
            if (n.Contains("xc40"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 750)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.57f, -1.71f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.66f, -1.65f, 0.2f));
                    var off = 0f;
                    for (var i = 0; i < 8; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.57f, -1.71f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.66f, -1.65f, 0.2f));
                    var off = 0f;
                    for (var i = 0; i < 8; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
            }
            if (n.Contains("c300"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 750)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.56f, -2.42f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.74f, -2.31f, 0.22f));
                    var off = 0f;
                    for (var i = 0; i < 9; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 375)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.56f, -2.42f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.74f, -2.31f, 0.22f));
                    var off = 0f;
                    for (var i = 0; i < 9; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("a7"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.35f, -2.39f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.72f, -2.27f, 0.2f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.35f, -2.39f, 0.2f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.72f, -2.27f, 0.2f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("720"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 750)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 450)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.4f, -2.12f, 0.5f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.78f, -2.0f, 0.48f));
                    var off = 0f;
                    for (var i = 0; i < 8; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off -= 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 450)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.4f, -2.12f, 0.5f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.78f, -2.0f, 0.48f));
                    var off = 0f;
                    for (var i = 0; i < 8; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off -= 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("sprinte"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.86f, -3.7f, 0.77f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.94f, -3.66f, 0.77f));
                    var off = 0f;
                    for (var i = 0; i < 20; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.86f, -3.7f, 0.77f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.94f, -3.66f, 0.77f));
                    var off = 0f;
                    for (var i = 0; i < 20; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("clio"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.62f, -1.85f, 0.21f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.7f, -1.79f, 0.21f));
                    var off = 0f;
                    for (var i = 0; i < 4; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.62f, -1.85f, 0.21f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.7f, -1.79f, 0.21f));
                    var off = 0f;
                    for (var i = 0; i < 4; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("63") && false)
            {
                veh.LeftIndicatorLightOn = false;
                veh.RightIndicatorLightOn = false;
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 700)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.38f, -2.41f, 0.23f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.69f, -2.35f, 0.23f));

                    var sidStart = veh.GetOffsetInWorldCoords(new Vector3(-0.69f, -2.35f, 0.23f));
                    var sidEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.79f, -2.27f, 0.23f));

                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, sidStart.X, sidStart.Y, sidStart.Z - off, sidEnd.X, sidEnd.Y, sidEnd.Z - off, 255, 252, 15, 255);
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.38f, -2.41f, 0.23f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.69f, -2.35f, 0.23f));

                    var sidStart = veh.GetOffsetInWorldCoords(new Vector3(0.69f, -2.35f, 0.23f));
                    var sidEnd = veh.GetOffsetInWorldCoords(new Vector3(0.79f, -2.27f, 0.23f));

                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, sidStart.X, sidStart.Y, sidStart.Z - off, sidEnd.X, sidEnd.Y, sidEnd.Z - off, 255, 252, 15, 255);
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }

                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
            }
            if (n.Contains("6xaa3"))
            {
                veh.LeftIndicatorLightOn = false;
                veh.RightIndicatorLightOn = false;
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.54f, -2.50f, 0.085f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.74f, -2.41f, 0.085f));

                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.54f, -2.50f, 0.085f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.74f, -2.41f, 0.085f));

                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }

                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
            }
            if (n.Contains("pansw"))
            {
                veh.LeftIndicatorLightOn = false;
                veh.RightIndicatorLightOn = false;
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.45f, -2.39f, 0.326f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.64f, -2.32f, 0.32f));

                    var sidStart = veh.GetOffsetInWorldCoords(new Vector3(-0.64f, -2.32f, 0.32f));
                    var sidEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.777f, -2.21f, 0.313f));

                    var off = 0f;
                    for (var i = 0; i < 4; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, sidStart.X, sidStart.Y, sidStart.Z - off, sidEnd.X, sidEnd.Y, sidEnd.Z - off, 255, 252, 15, 255);
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.45f, -2.39f, 0.326f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.64f, -2.32f, 0.32f));

                    var sidStart = veh.GetOffsetInWorldCoords(new Vector3(0.64f, -2.32f, 0.32f));
                    var sidEnd = veh.GetOffsetInWorldCoords(new Vector3(0.777f, -2.21f, 0.313f));

                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, sidStart.X, sidStart.Y, sidStart.Z - off, sidEnd.X, sidEnd.Y, sidEnd.Z - off, 255, 252, 15, 255);
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }

                    // Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.2f, 1.0f);
                    //  Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, coronaLow.X, coronaLow.Y, coronaLow.Z, 255, 252, 15, 1f, 0.5f);
                }
            }
            if (n.Contains("pace") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.41f, -2.48f, 0.29f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.74f, -2.34f, 0.29f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.41f, -2.48f, 0.29f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.74f, -2.34f, 0.29f));
                    var off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("x6") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.41f, -2.44f, 0.53f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.77f, -2.33f, 0.535f));
                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.41f, -2.44f, 0.53f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.77f, -2.33f, 0.535f));
                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("clio") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (!veh.LightsOn)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.85f, -3.7f, 0.56f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.95f, -3.66f, 0.56f));
                    var off = 0f;
                    for (var i = 0; i < 20; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.85f, -3.7f, 0.56f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.95f, -3.66f, 0.56f));
                    var off = 0f;
                    for (var i = 0; i < 20; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("rover") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.73f, -2.61f, 0.0f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.8f, -2.57f, 0.0f));
                    var off = 0f;
                    for (var i = 0; i < 24; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.73f, -2.61f, 0.0f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.8f, -2.57f, 0.0f));
                    var off = 0f;
                    for (var i = 0; i < 24; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }



            if (n.Contains("rogue"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.84f, -3.13f, 0.38f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.896f, -3.10f, 0.43f));
                    var off = 0f;
                    for (var i = 0; i < 36; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.84f, -3.13f, 0.38f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.896f, -3.10f, 0.43f));
                    var off = 0f;
                    for (var i = 0; i < 36; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                }
            }

            if (n.Contains("xc90") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.73f, -2.186f, 0.58f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.8f, -2.155f, 0.58f));
                    var off = 0f;
                    for (var i = 0; i < 24; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.73f, -2.186f, 0.58f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.8f, -2.155f, 0.58f));
                    var off = 0f;
                    for (var i = 0; i < 24; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                }
            }
            if (n.Contains("e-class") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.84f, -2.41f, 0.75f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.93f, -2.4f, 0.75f));
                    var off = 0f;
                    for (var i = 0; i < 40; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 174, 41, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.84f, -2.41f, 0.75f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.93f, -2.4f, 0.75f));
                    var off = 0f;
                    for (var i = 0; i < 40; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 174, 41, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                }
            }
            if (n.Contains("s560"))
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 450)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.36f, -2.53f, 0.311f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.71f, -2.42f, 0.315f));
                    var off = 0f;
                    for (var i = 0; i < 14; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.001f;
                    }
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 450)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.36f, -2.53f, 0.311f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.71f, -2.42f, 0.315f));
                    var off = 0f;
                    for (var i = 0; i < 14; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.001f;
                    }
                }
            }
            if (n.Contains("m4") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.74f, -2.10f, 0.39f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.60f, -2.19f, 0.39f));
                    var off = 0f;
                    for (var i = 0; i < 4; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.74f, -2.10f, 0.39f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.60f, -2.19f, 0.39f));
                    var off = 0f;
                    for (var i = 0; i < 4; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("paj") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.418f, -1.98f, 0.558f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-0.68f, -1.81f, 0.530f));
                    var off = 0f;
                    for (var i = 0; i < 2; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 350)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.418f, -1.98f, 0.558f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(0.68f, -1.81f, 0.530f));
                    var off = 0f;
                    for (var i = 0; i < 2; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("exped") && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    veh.LeftIndicatorLightOn = false;
                    veh.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.84f, -7.42f, 1.68f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-1.00f, -7.38f, 1.63f));
                    var off = 0f;
                    for (var i = 0; i < 10; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    leftStart = veh.GetOffsetInWorldCoords(new Vector3(-0.82f, -7.56f, -0.98f));
                    leftEnd = veh.GetOffsetInWorldCoords(new Vector3(-1.13f, -7.49f, -0.87f));
                    off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.87f, -7.42f, 1.68f));
                    var leftEnd = veh.GetOffsetInWorldCoords(new Vector3(1.00f, -7.38f, 1.63f));
                    var off = 0f;
                    for (var i = 0; i < 30; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    leftStart = veh.GetOffsetInWorldCoords(new Vector3(0.82f, -7.56f, -0.98f));
                    leftEnd = veh.GetOffsetInWorldCoords(new Vector3(1.13f, -7.49f, -0.87f));
                    off = 0f;
                    for (var i = 0; i < 3; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                    var corona = veh.GetOffsetInWorldCoords(new Vector3(-0.6f, -2.26f, 0.52f));
                    Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, corona.X, corona.Y, corona.Z, 255, 252, 15, 0.05f, 1.0f);
                }
            }
            if (n.Contains("680") && truckTrailer != null && false)
            {
                if (!ledIndicatorsStayStopwatch.IsRunning)
                    ledIndicatorsStayStopwatch.Start();
                if (ledIndicatorsStayStopwatch.ElapsedMilliseconds > 1000)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                    ledIndicatorsStayStopwatch.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && isLeftIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed && isRightIndicatorOn)
                {
                    ledIndicatorsStayStopwatch = new Stopwatch();
                }
                if (true)
                {
                    truckTrailer.LeftIndicatorLightOn = false;
                    truckTrailer.RightIndicatorLightOn = false;
                }
                if (isLeftIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = truckTrailer.GetOffsetInWorldCoords(new Vector3(-0.75f, -7.99f, -0.81f));
                    var leftEnd = truckTrailer.GetOffsetInWorldCoords(new Vector3(-1.17f, -7.99f, -0.81f));
                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
                if (isRightIndicatorOn && ledIndicatorsStayStopwatch.ElapsedMilliseconds < 500)
                {
                    var leftStart = truckTrailer.GetOffsetInWorldCoords(new Vector3(0.75f, -7.99f, -0.81f));
                    var leftEnd = truckTrailer.GetOffsetInWorldCoords(new Vector3(1.17f, -7.99f, -0.81f));
                    var off = 0f;
                    for (var i = 0; i < 5; i++)
                    {
                        Function.Call((Hash)0x6B7256074AE34680, leftStart.X, leftStart.Y, leftStart.Z - off, leftEnd.X, leftEnd.Y, leftEnd.Z - off, 255, 252, 15, 255);
                        off += 0.002f;
                    }
                }
            }
        }
        private void Creep()
        {
            if (!isParkingGear && !isCruiseControlActive && !isParkingMode && drivingMode != "parking")
            {
                if (speedInKmh == 0 && !speed0Stopwatch.IsRunning)
                    speed0Stopwatch.Start();
                if (speedInKmh > 7 && speed0Stopwatch.IsRunning)
                    speed0Stopwatch = new Stopwatch();
                if (lastVehPos.X == 999999999999)
                    lastVehPos = veh.GetOffsetInWorldCoords(new Vector3(0, -1, 0));
                isReversing = veh.Position.DistanceTo(lastVehPos) < 0.99f;
                lastVehPos = veh.GetOffsetInWorldCoords(new Vector3(0, -1, 0));
                if (isReversing && GamePad.GetState(PlayerIndex.One).Triggers.Right < 1 && speed0Stopwatch.ElapsedMilliseconds > 1000)
                {
                    Function.Call((Hash)0x92B35082E0B42F66, Game.Player.LastVehicle, false);
                    isCreep = true;
                }
                Function.Call((Hash)0x684785568EF26A22, veh, false);
                if (isCreep && GamePad.GetState(PlayerIndex.One).Buttons.A == XInputDotNetPure.ButtonState.Released && veh.ClassType != VehicleClass.Planes)
                {
                    var kmh = veh.Speed * 3.6;
                    var force = 0.03f;
                    if (isTruckMode)
                    {
                        force = 0.051f;
                    }
                    if (GamePad.GetState(PlayerIndex.One).Triggers.Left < 0.1 && kmh < 4)
                    {
                        force = 0.25f;
                    }
                    if ((kmh < (isReversing ? 4 : 10)) && !veh.DisplayName.ToLower().Contains("fork") && !veh.DisplayName.ToLower().Contains("qash"))
                    {
                        veh.ApplyForceRelative(new Vector3(0, isReversing ? -force : force, 0));
                    }
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1 && veh.Speed * 3.6 <= 5 || (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 1 && speedInKmh <= 5))
                {
                    isCreep = false;
                }
                if ((GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.01) && !isReversing && speed0Stopwatch.ElapsedMilliseconds > 1000)
                {
                    isCreep = true;
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left < 0.1)
                {
                    creepBrakeReleased = true;
                }
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.8)
                {
                    creepBrakeReleased = false;
                }
                if (creepBrakeReleased && GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.01 && !isCreep)
                    isCreep = true;

                if (fakeSpeed > 8 && GamePad.GetState(PlayerIndex.One).Triggers.Right <= 0.1f)
                    isCreep = true;
            }
        }

        private Stopwatch matrixLedCalculationStopwatch = new Stopwatch();
        private bool isMatrixCalculation = false;
        private List<int> offLedsIndex = new List<int>();
        private int totalLedAmount = 0;
        private Stopwatch matrixOldLampStartup = new Stopwatch();
        private float[] ledBrightnessList = new float[100];

        private void MatrixLed()
        {

            var debug = false;
            var lBone = veh.GetBoneCoord("headlight_l");
            var rBone = veh.GetBoneCoord("headlight_r");

            var lampInitHeight = veh.GetOffsetFromWorldCoords(lBone).Z;
            var defaultLampHeight = 0.092f;
            var lampDiff = (defaultLampHeight - lampInitHeight);

            var leftLightBone = new Vector3(lBone.X, lBone.Y, lBone.Z + lampDiff);
            var rightLightBone = new Vector3(rBone.X, rBone.Y, rBone.Z + lampDiff);
            if (debug && false)
            {
                Function.Call((Hash)0x6B7256074AE34680, rightLightBone.X, rightLightBone.Y, rightLightBone.Z, rightLightBone.X, rightLightBone.Y, (rightLightBone.Z + 4f), 255, 5, 5, 255);
                Function.Call((Hash)0x6B7256074AE34680, leftLightBone.X, leftLightBone.Y, leftLightBone.Z, leftLightBone.X, leftLightBone.Y, (leftLightBone.Z + 4f), 255, 5, 5, 255);
            }

            /* old config */
            /*
            var offset = -3f;
            var index = 0;
            var genericSensorOffset = 6f;
            var sensorAmount = 16f; 
            var leftLightLedAmount = 16;
            var rightLightLedAmount = 24; 
             */

            var offset = -2f;
            var index = 0;
            var genericSensorOffset = 6f;
            var leftSensorOffset = 6f;
            var sensorAmount = 32;
            var leftLightLedAmount = 8;
            var rightLightLedAmount = 10;
            var fadeout = 10f;
            var matrixCalculationFrequency = 120;
            var matrixDistance = 300f;
            var matrixColor = System.Drawing.Color.FromArgb(255, 99, 137, 198);
            var matrixBrightness = 2.5f;
            var ledDistBetween = 0.15f;
            var vehName = veh?.DisplayName?.ToLower();
            var radius = 4f;

            if (vehName.Contains("rrst") || vehName.Contains("i4"))
                matrixColor = System.Drawing.Color.FromArgb(255, 98, 98, 196);
            if (vehName.Contains("furgon"))
            {
                //  matrixColor = System.Drawing.Color.FromArgb(255, 255, 255, 204);
            }

            if (fakeSpeed > 140 && false)
            {
                ledDistBetween = 0.13f;
                offset = -1.61f;
                leftLightLedAmount = 6;
                rightLightLedAmount = 8;
                genericSensorOffset = 5f;
                leftSensorOffset = 5f;
                matrixCalculationFrequency = 80;
                sensorAmount = 35;
            }
            if ((isOnAutobahn && false) || true)
            {
                offset = -1.1f;
                leftLightLedAmount = 3;
                rightLightLedAmount = 4;
                genericSensorOffset = 5f;
                leftSensorOffset = 5f;
                matrixCalculationFrequency = 100;
                sensorAmount = 38;
            }

            if (true)
            {
                // ledDistBetween = 0.13f;
                ledDistBetween = 0.13f;
                offset = -1.61f;
                leftLightLedAmount = 6;
                rightLightLedAmount = 8;
                genericSensorOffset = 5f;
                leftSensorOffset = 5f;
                matrixCalculationFrequency = 300;
                sensorAmount = 35;
            }

            if (matrixLedCalculationStopwatch.ElapsedMilliseconds > matrixCalculationFrequency)
            {
                matrixLedCalculationStopwatch = new Stopwatch();
                isMatrixCalculation = true;
            }
            if (!matrixLedCalculationStopwatch.IsRunning)
            {
                matrixLedCalculationStopwatch.Start();
            }

            //   isMatrixCalculation = false;
            if (vehName.Contains("rogue") && false)
            {
                isMatrixCalculation = false;
                ledDistBetween = 0.13f;
                offset = -1.61f;
                leftLightLedAmount = 6;
                rightLightLedAmount = 8;
                genericSensorOffset = 5f;
                leftSensorOffset = 5f;
                matrixColor = System.Drawing.Color.FromArgb(255, 255, 245, 188);
                matrixBrightness = 0.1f;
                matrixDistance = 270f;
                if (!veh.HighBeamsOn)
                    veh.HighBeamsOn = true;
            }



            for (var i = 0; i < leftLightLedAmount; ++i)
            {
                if (isMatrixCalculation || offLedsIndex.All(ix => ix != index))
                {
                    var frontModelSize = veh.Model.GetDimensions();
                    frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, 0.854f);
                    var matrixPosition = GetEntityOffset(veh, new Vector3(offset, (-frontModelSize.Y) + 8f, (-frontModelSize.Z * 0)));
                    matrixPosition = new Vector3(matrixPosition.X, matrixPosition.Y, matrixPosition.Z);
                    if (debug)
                        Function.Call((Hash)0x6B7256074AE34680, matrixPosition.X, matrixPosition.Y, matrixPosition.Z, matrixPosition.X, matrixPosition.Y, (matrixPosition.Z + 4f), 255, 5, 5, 255);

                    var matrixDir = matrixPosition - leftLightBone;
                    matrixDir.Normalize();

                    var isCarAtAnySensor = false;
                    if (isMatrixCalculation)
                    {
                        if (index == 0)
                        {
                            offLedsIndex = new List<int>();
                        }
                        var sensorOffset = leftSensorOffset;
                        var sensorZ = 0f;
                        for (var ii = 0; ii < sensorAmount; ++ii)
                        {
                            if (ii == 0)
                                sensorOffset += leftSensorOffset - 3;
                            var sensorSentivity = leftSensorOffset;
                            if (ii < 3)
                            {
                                sensorSentivity = sensorSentivity - 3;
                            }
                            var possibleOffset = leftLightBone + (matrixDir * sensorOffset);
                            if (ii == 0)
                            {
                                sensorZ = possibleOffset.Z;
                            }
                            if (sensorAmount - ii < 16)
                            {
                                sensorSentivity += 3;
                            }
                            else if (sensorAmount - ii < 22)
                            {
                                sensorSentivity += 5;
                            }
                            possibleOffset = new Vector3(possibleOffset.X, possibleOffset.Y, sensorZ);
                            var isEmpty = World.GetClosestVehicle(possibleOffset, sensorSentivity);
                            if (!isCarAtAnySensor && isEmpty?.DisplayName != null)
                            {
                                var zDiff = (isEmpty.Position.Z - veh.Position.Z);
                                var isRightCar = isEmpty.LightsOn && (zDiff < 4 && zDiff > -4);
                                if (isRightCar)
                                {
                                    isCarAtAnySensor = true;
                                    offLedsIndex.Add(index);
                                    ledBrightnessList[i] = 0;
                                    break;
                                }
                            }
                            //   Function.Call((Hash)0x6B7256074AE34680, possibleOffset.X, possibleOffset.Y, possibleOffset.Z, possibleOffset.X, possibleOffset.Y, (possibleOffset.Z + 0.3f), 255, 5, 5, 255);
                            sensorOffset += leftSensorOffset;
                        }
                    }

                    if (!isCarAtAnySensor)
                    {
                        ledBrightnessList[i] += 0.07f;
                    }


                    var individualLedBrightness = ledBrightnessList[i];
                    World.DrawSpotLight(
                                       leftLightBone,
                                       matrixDir,
                                       matrixColor,
                                       matrixDistance,
                                       individualLedBrightness > matrixBrightness ? matrixBrightness : individualLedBrightness,
                                       3f,
                                       radius,
                                       fadeout
                                  );

                }
                offset += ledDistBetween;
                index++;
            }

            offset = 0.6f;

            for (var i = 0; i < rightLightLedAmount; ++i)
            {
                if (isMatrixCalculation || offLedsIndex.All(ix => ix != index))
                {

                    var realIndex = leftLightLedAmount + i;

                    var frontModelSize = veh.Model.GetDimensions();
                    frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, 0.854f);
                    var matrixPosition = GetEntityOffset(veh, new Vector3(offset, (-frontModelSize.Y) + 8f, (-frontModelSize.Z * 0)));
                    matrixPosition = new Vector3(matrixPosition.X, matrixPosition.Y, matrixPosition.Z);
                    if (debug)
                        Function.Call((Hash)0x6B7256074AE34680, matrixPosition.X, matrixPosition.Y, matrixPosition.Z, matrixPosition.X, matrixPosition.Y, (matrixPosition.Z + 4f), 255, 5, 5, 255);

                    var matrixDir = matrixPosition - rightLightBone;
                    matrixDir.Normalize();

                    var isCarAtAnySensor = false;
                    if (isMatrixCalculation)
                    {
                        var sensorOffset = genericSensorOffset;
                        var sensorZ = 0f;
                        for (var ii = 0; ii < sensorAmount; ++ii)
                        {
                            if (ii == 0)
                                sensorOffset += genericSensorOffset - 3;
                            var sensorSentivity = genericSensorOffset;
                            if (ii < 3)
                            {
                                sensorSentivity = sensorSentivity - 3;
                            }
                            var possibleOffset = rightLightBone + (matrixDir * sensorOffset);
                            if (ii == 0)
                            {
                                sensorZ = possibleOffset.Z;
                            }
                            if (sensorAmount - ii < 16)
                            {
                                sensorSentivity += 3;
                            }
                            else if (sensorAmount - ii < 22)
                            {
                                sensorSentivity += 5;
                            }
                            possibleOffset = new Vector3(possibleOffset.X, possibleOffset.Y, sensorZ);
                            var isEmpty = World.GetClosestVehicle(possibleOffset, sensorSentivity);
                            if (!isCarAtAnySensor && isEmpty?.DisplayName != null)
                            {
                                var zDiff = (isEmpty.Position.Z - veh.Position.Z);
                                var isRightCar = isEmpty.LightsOn && (zDiff < 8 && zDiff > -8);
                                if (isRightCar)
                                {
                                    isCarAtAnySensor = true;
                                    offLedsIndex.Add(index);
                                    ledBrightnessList[realIndex] = 0;
                                    break;
                                }
                            }
                            sensorOffset += genericSensorOffset;
                        }
                    }

                    if (!isCarAtAnySensor)
                    {
                        ledBrightnessList[realIndex] += 0.07f;
                    }


                    var individualLedBrightness = ledBrightnessList[realIndex];
                    World.DrawSpotLight(
                                       leftLightBone,
                                       matrixDir,
                                       matrixColor,
                                       matrixDistance,
                                       individualLedBrightness > matrixBrightness ? matrixBrightness : individualLedBrightness,
                                       3f,
                                       radius,
                                       fadeout
                                  );


                }
                offset += ledDistBetween;
                index++;
            }
            isMatrixCalculation = false;
            totalLedAmount = leftLightLedAmount + rightLightLedAmount;
        }

        /* REAL MATRIX LIGHTS

          private void MatrixLed()
        {

            var debug = false;
            var lBone = veh.GetBoneCoord("headlight_l");
            var rBone = veh.GetBoneCoord("headlight_r");

            var lampInitHeight = veh.GetOffsetFromWorldCoords(lBone).Z;
            var defaultLampHeight = 0.092f;
            var lampDiff = (defaultLampHeight - lampInitHeight);

            var leftLightBone = new Vector3(lBone.X, lBone.Y, lBone.Z + lampDiff);
            var rightLightBone = new Vector3(rBone.X, rBone.Y, rBone.Z + lampDiff);
            if (debug && false)
            {
                Function.Call((Hash)0x6B7256074AE34680, rightLightBone.X, rightLightBone.Y, rightLightBone.Z, rightLightBone.X, rightLightBone.Y, (rightLightBone.Z + 4f), 255, 5, 5, 255);
                Function.Call((Hash)0x6B7256074AE34680, leftLightBone.X, leftLightBone.Y, leftLightBone.Z, leftLightBone.X, leftLightBone.Y, (leftLightBone.Z + 4f), 255, 5, 5, 255);
            }

         //    old config 
        
      //  var offset = -3f;
      //  var index = 0;
      //  var genericSensorOffset = 6f;
       // var sensorAmount = 16f; 
       // var leftLightLedAmount = 16;
       // var rightLightLedAmount = 24; 
       

        var offset = -2f;
        var index = 0;
        var genericSensorOffset = 6f;
        var leftSensorOffset = 6f;
        var sensorAmount = 32;
        var leftLightLedAmount = 8;
        var rightLightLedAmount = 10;
        var fadeout = 10f;
        var matrixCalculationFrequency = 120;
        var matrixDistance = 300f;
        var matrixColor = System.Drawing.Color.FromArgb(255, 99, 137, 198);
        var matrixBrightness = 5f;
        var ledDistBetween = 0.15f;
        var vehName = veh?.DisplayName?.ToLower();

            if (vehName.Contains("rrst") || vehName.Contains("i4"))
                matrixColor = System.Drawing.Color.FromArgb(255, 98, 98, 196);

            if (fakeSpeed > 140)
            {
                ledDistBetween = 0.13f;
                offset = -1.61f;
                leftLightLedAmount = 6;
                rightLightLedAmount = 8;
                genericSensorOffset = 5f;
                leftSensorOffset = 5f;
                matrixCalculationFrequency = 80;
                sensorAmount = 35;
            }
            if (isOnAutobahn)
            {
                offset = -1.1f;
                leftLightLedAmount = 3;
                rightLightLedAmount = 4;
                genericSensorOffset = 5f;
                leftSensorOffset = 5f;
                matrixCalculationFrequency = 100;
                sensorAmount = 38;
            }

            if (matrixLedCalculationStopwatch.ElapsedMilliseconds > matrixCalculationFrequency)
            {
                matrixLedCalculationStopwatch = new Stopwatch();
isMatrixCalculation = true;
            }
            if (!matrixLedCalculationStopwatch.IsRunning)
            {
                matrixLedCalculationStopwatch.Start();
            }

            isMatrixCalculation = false;

            for (var i = 0; i<leftLightLedAmount; ++i)
            {
                if (isMatrixCalculation || offLedsIndex.All(ix => ix != index))
                {
                    var frontModelSize = veh.Model.GetDimensions();
frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, 0.854f);
var matrixPosition = GetEntityOffset(veh, new Vector3(offset, (-frontModelSize.Y) + 8f, (-frontModelSize.Z * 0)));
matrixPosition = new Vector3(matrixPosition.X, matrixPosition.Y, matrixPosition.Z);
                    if (debug)
                        Function.Call((Hash)0x6B7256074AE34680, matrixPosition.X, matrixPosition.Y, matrixPosition.Z, matrixPosition.X, matrixPosition.Y, (matrixPosition.Z + 4f), 255, 5, 5, 255);

                    var matrixDir = matrixPosition - leftLightBone;
matrixDir.Normalize();

                    var isCarAtAnySensor = false;
                    if (isMatrixCalculation)
                    {
                        if (index == 0)
                        {
                            offLedsIndex = new List<int>();
                        }
                        var sensorOffset = leftSensorOffset;
var sensorZ = 0f;
                        for (var ii = 0; ii<sensorAmount; ++ii)
                        {
                            if (ii == 0)
                                sensorOffset += leftSensorOffset - 3;
                            var sensorSentivity = leftSensorOffset;
                            if (ii< 3)
                            {
                                sensorSentivity = sensorSentivity - 3;
                            }
                            var possibleOffset = leftLightBone + (matrixDir * sensorOffset);
                            if (ii == 0)
                            {
                                sensorZ = possibleOffset.Z;
                            }
                            if (sensorAmount - ii< 16)
                            {
                                sensorSentivity += 3;
                            }
                            else if (sensorAmount - ii< 22)
                            {
                                sensorSentivity += 5;
                            }
                            possibleOffset = new Vector3(possibleOffset.X, possibleOffset.Y, sensorZ);
var isEmpty = World.GetClosestVehicle(possibleOffset, sensorSentivity);
                            if (!isCarAtAnySensor && isEmpty?.DisplayName != null)
                            {
                                var zDiff = (isEmpty.Position.Z - veh.Position.Z);
var isRightCar = isEmpty.LightsOn && (zDiff < 8 && zDiff > -8);
                                if (isRightCar)
                                {
                                    isCarAtAnySensor = true;
                                    offLedsIndex.Add(index);
                                    break;
                                }
                            }
                            //   Function.Call((Hash)0x6B7256074AE34680, possibleOffset.X, possibleOffset.Y, possibleOffset.Z, possibleOffset.X, possibleOffset.Y, (possibleOffset.Z + 0.3f), 255, 5, 5, 255);
                            sensorOffset += leftSensorOffset;
                        }
                    }

                    if (!isCarAtAnySensor)
                    {
                        World.DrawSpotLight(
                                           leftLightBone,
                                           matrixDir,
                                           matrixColor,
                                           matrixDistance, matrixBrightness, 3f, 4f, fadeout
                                      );
                    }
                }
                offset += ledDistBetween;
                index++;
            }

            offset = 0.6f;

            for (var i = 0; i<rightLightLedAmount; ++i)
            {
                if (isMatrixCalculation || offLedsIndex.All(ix => ix != index))
                {
                    var frontModelSize = veh.Model.GetDimensions();
frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, 0.854f);
var matrixPosition = GetEntityOffset(veh, new Vector3(offset, (-frontModelSize.Y) + 8f, (-frontModelSize.Z * 0)));
matrixPosition = new Vector3(matrixPosition.X, matrixPosition.Y, matrixPosition.Z);
                    if (debug)
                        Function.Call((Hash)0x6B7256074AE34680, matrixPosition.X, matrixPosition.Y, matrixPosition.Z, matrixPosition.X, matrixPosition.Y, (matrixPosition.Z + 4f), 255, 5, 5, 255);

                    var matrixDir = matrixPosition - rightLightBone;
matrixDir.Normalize();

                    var isCarAtAnySensor = false;
                    if (isMatrixCalculation)
                    {
                        var sensorOffset = genericSensorOffset;
var sensorZ = 0f;
                        for (var ii = 0; ii<sensorAmount; ++ii)
                        {
                            if (ii == 0)
                                sensorOffset += genericSensorOffset - 3;
                            var sensorSentivity = genericSensorOffset;
                            if (ii< 3)
                            {
                                sensorSentivity = sensorSentivity - 3;
                            }
                            var possibleOffset = rightLightBone + (matrixDir * sensorOffset);
                            if (ii == 0)
                            {
                                sensorZ = possibleOffset.Z;
                            }
                            if (sensorAmount - ii< 16)
                            {
                                sensorSentivity += 3;
                            }
                            else if (sensorAmount - ii< 22)
                            {
                                sensorSentivity += 5;
                            }
                            possibleOffset = new Vector3(possibleOffset.X, possibleOffset.Y, sensorZ);
var isEmpty = World.GetClosestVehicle(possibleOffset, sensorSentivity);
                            if (!isCarAtAnySensor && isEmpty?.DisplayName != null)
                            {
                                var zDiff = (isEmpty.Position.Z - veh.Position.Z);
var isRightCar = isEmpty.LightsOn && (zDiff < 8 && zDiff > -8);
                                if (isRightCar)
                                {
                                    isCarAtAnySensor = true;
                                    offLedsIndex.Add(index);
                                    break;
                                }
                            }
                            sensorOffset += genericSensorOffset;
                        }
                    }
                    if (!isCarAtAnySensor)
                    {
                        World.DrawSpotLight(
                                           rightLightBone,
                                           matrixDir,
                                           matrixColor,
                                           matrixDistance, matrixBrightness, 3f, 4f, fadeout
                                      );
                    }

                }
                offset += ledDistBetween;
                index++;
            }
            isMatrixCalculation = false;
            totalLedAmount = leftLightLedAmount + rightLightLedAmount;
        }

         */

        private bool isMatrixLedEnabled = false;
        private Stopwatch enableMatrixLedStopwatch = new Stopwatch();

        private void HighBeamLightsNew()
        {
            if ((isMatrixLedEnabled && fakeSpeed > 40 && veh.LightsOn))
            {
                MatrixLed();
            }
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleSelectNextWeapon))
            {
                if (isMatrixLedEnabled)
                {
                    isMatrixLedEnabled = false;
                    veh.HighBeamsOn = false;
                }
                else
                {
                    enableMatrixLedStopwatch.Start();
                    matrixOldLampStartup = new Stopwatch();
                    matrixOldLampStartup.Start();
                }
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
            {
                enableMatrixLedStopwatch = new Stopwatch();
            }
            if (enableMatrixLedStopwatch.ElapsedMilliseconds > 200)
            {
                isMatrixLedEnabled = !isMatrixLedEnabled;
                enableMatrixLedStopwatch = new Stopwatch();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.X == XInputDotNetPure.ButtonState.Pressed)
            {
                veh.HighBeamsOn = true;
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
            {
                veh.HighBeamsOn = false;
                /* var off = veh.GetOffsetInWorldCoords(new Vector3(0, 380, 0));
                 if (isSpeedZoneSet)
                 {
                     Function.Call((Hash)0x1033371FC8E842A7, speedZone);
                     speedZone = 0;
                     speedZone = Function.Call<int>((Hash)0x2CE544C68FB812A0, off.X, off.Y, off.Z, 60f, 14f, false);
                     isSpeedZoneSet = false;
                 }
                 else
                 {
                     isSpeedZoneSet = true;
                     Function.Call((Hash)0x1033371FC8E842A7, speedZone);
                     veh.HighBeamsOn = false;
                     speedZone = Function.Call<int>((Hash)0x2CE544C68FB812A0, off.X, off.Y, off.Z, 60f, 0f, false);
                     UI.ShowSubtitle(speedZone.ToString());
                 }*/

            }
            if ((Game.IsControlJustPressed(0, GTA.Control.VehicleSelectNextWeapon) && fakePolice == null) || !inits || (fakePolice == null && isSireneMode && fakeSpeed < 110))
            {
                inits = true;
                fakePolice = World.CreateVehicle(new Model(VehicleHash.Police), veh.GetOffsetInWorldCoords(new Vector3(0, 20, 0)));
                fakePolice.Heading = veh.Heading;
                fakePolice.SirenActive = true;
                fakePolice.IsSirenSilent = true;
                fakePolice.PlaceOnGround();
                fakePoliceDriver = fakePolice.CreatePedOnSeat(VehicleSeat.Driver, new Model(PedHash.Cop01SFY));
                var driveToPos = veh.GetOffsetInWorldCoords(new Vector3(0, 200, 0));
                fakePolice.Speed = veh.Speed;
                fakePolice.SetNoCollision(veh, true);
                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, fakePoliceDriver, fakePolice, driveToPos.X, driveToPos.Y, driveToPos.Z, 10f, 1f, fakePolice.GetHashCode(), 6, 1f, true);
                flashHighBeamStopwatch.Start();
                fakePoliceUpdateSpeed.Start();
                veh.LightsOn = true;
                fakePolice.Alpha = 0;
                fakePoliceDriver.Alpha = 0;
                fakePolice.HasCollision = false;
            }
            if (flashHighBeamStopwatch.ElapsedMilliseconds > 1000)
            {
                fakePolice.Delete();
                fakePolice = null;
                fakePoliceDriver.Delete();
                fakePoliceDriver = null;
                fakePoliceUpdateSpeed = new Stopwatch();
                flashHighBeamStopwatch = new Stopwatch();
                veh.LightsOn = false;
            }
            if (fakePoliceUpdateSpeed.ElapsedMilliseconds >= 200)
            {
                fakePolice.Speed = veh.Speed;
                fakePolice.Heading = veh.Heading;
            }
            if (false)
            {
                if (l1sw.ElapsedMilliseconds > 3500 && l1sw.IsRunning)
                    l1sw = new Stopwatch();
                if (l2sw.ElapsedMilliseconds > 3500 && l2sw.IsRunning)
                    l2sw = new Stopwatch();
                if (l3sw.ElapsedMilliseconds > 3500 && l3sw.IsRunning)
                    l3sw = new Stopwatch();
                if (l4sw.ElapsedMilliseconds > 3500 && l4sw.IsRunning)
                    l4sw = new Stopwatch();
                if (l5sw.ElapsedMilliseconds > 3500 && l5sw.IsRunning)
                    l5sw = new Stopwatch();
                Vector3 l1 = veh.GetOffsetInWorldCoords(new Vector3(-14f, 60f, -1f));
                Vector3 l2 = veh.GetOffsetInWorldCoords(new Vector3(-7f, 60f, -1f));
                Vector3 l3 = veh.GetOffsetInWorldCoords(new Vector3(0f, 60f, -1f));
                Vector3 l4 = veh.GetOffsetInWorldCoords(new Vector3(7f, 60f, -1f));
                Vector3 l5 = veh.GetOffsetInWorldCoords(new Vector3(14f, 60f, -1f));

                Vector3 ll1 = veh.GetOffsetInWorldCoords(new Vector3(-10f, 40f, 0));
                Vector3 ll2 = veh.GetOffsetInWorldCoords(new Vector3(-5f, 40f, 0));
                Vector3 ll3 = veh.GetOffsetInWorldCoords(new Vector3(0f, 40f, 0));
                Vector3 ll4 = veh.GetOffsetInWorldCoords(new Vector3(5f, 40f, 0));
                Vector3 ll5 = veh.GetOffsetInWorldCoords(new Vector3(10f, 40f, 0));

                Vector3 lll1 = veh.GetOffsetInWorldCoords(new Vector3(-7.5f, 30f, 0));
                Vector3 lll2 = veh.GetOffsetInWorldCoords(new Vector3(-3.5f, 30f, 0));
                Vector3 lll3 = veh.GetOffsetInWorldCoords(new Vector3(0f, 30f, 0));
                Vector3 lll4 = veh.GetOffsetInWorldCoords(new Vector3(3.5f, 30f, 0));
                Vector3 lll5 = veh.GetOffsetInWorldCoords(new Vector3(7.5f, 30f, 0));

                Vector3 llll1 = veh.GetOffsetInWorldCoords(new Vector3(-5f, 20f, 0));
                Vector3 llll2 = veh.GetOffsetInWorldCoords(new Vector3(-2f, 20f, 0));
                Vector3 llll3 = veh.GetOffsetInWorldCoords(new Vector3(0f, 20f, 0));
                Vector3 llll4 = veh.GetOffsetInWorldCoords(new Vector3(2f, 20f, 0));
                Vector3 llll5 = veh.GetOffsetInWorldCoords(new Vector3(5f, 20f, 0));

                Vector3 lllll1 = veh.GetOffsetInWorldCoords(new Vector3(-3.5f, 10f, 0));
                Vector3 lllll2 = veh.GetOffsetInWorldCoords(new Vector3(-1f, 10f, 0));
                Vector3 lllll3 = veh.GetOffsetInWorldCoords(new Vector3(0f, 10f, 0));
                Vector3 lllll4 = veh.GetOffsetInWorldCoords(new Vector3(1f, 10f, 0));
                Vector3 lllll5 = veh.GetOffsetInWorldCoords(new Vector3(3.5f, 10f, 0));

                Vector3 llllll1 = veh.GetOffsetInWorldCoords(new Vector3(-17f, 70f, 0));
                Vector3 llllll2 = veh.GetOffsetInWorldCoords(new Vector3(-9f, 70f, 0));
                Vector3 llllll3 = veh.GetOffsetInWorldCoords(new Vector3(0f, 70f, 0));
                Vector3 llllll4 = veh.GetOffsetInWorldCoords(new Vector3(9f, 70f, 0));
                Vector3 llllll5 = veh.GetOffsetInWorldCoords(new Vector3(17f, 70f, 0));

                Vector3 lllllll1 = veh.GetOffsetInWorldCoords(new Vector3(-22f, 100f, 0));
                Vector3 lllllll2 = veh.GetOffsetInWorldCoords(new Vector3(-13f, 100f, 0));
                Vector3 lllllll3 = veh.GetOffsetInWorldCoords(new Vector3(0f, 100f, 0));
                Vector3 lllllll4 = veh.GetOffsetInWorldCoords(new Vector3(13f, 100f, 0));
                Vector3 lllllll5 = veh.GetOffsetInWorldCoords(new Vector3(22f, 100f, 0));

                var sensorRad = 5f;
                if (false)
                {
                    Function.Call((Hash)0x6B7256074AE34680, l1.X, l1.Y, 0, l1.X, l1.Y, (l1.Z + 4f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, l2.X, l2.Y, 0, l2.X, l2.Y, (l2.Z + 4f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, l3.X, l3.Y, 0, l3.X, l3.Y, (l3.Z + 4f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, l4.X, l4.Y, 0, l4.X, l4.Y, (l4.Z + 4f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, l5.X, l5.Y, 0, l5.X, l5.Y, (l5.Z + 4f), 255, 5, 5, 255);

                    Function.Call((Hash)0x6B7256074AE34680, ll1.X, ll1.Y, 0, ll1.X, ll1.Y, (ll1.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, ll2.X, ll2.Y, 0, ll2.X, ll2.Y, (ll2.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, ll3.X, ll3.Y, 0, ll3.X, ll3.Y, (ll3.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, ll4.X, ll4.Y, 0, ll4.X, ll4.Y, (ll4.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, ll5.X, ll5.Y, 0, ll5.X, ll5.Y, (ll5.Z + 2f), 255, 5, 5, 255);

                    Function.Call((Hash)0x6B7256074AE34680, lll1.X, lll1.Y, 0, lll1.X, lll1.Y, (lll1.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lll2.X, lll2.Y, 0, lll2.X, lll2.Y, (lll2.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lll3.X, lll3.Y, 0, lll3.X, lll3.Y, (lll3.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lll4.X, lll4.Y, 0, lll4.X, lll4.Y, (lll4.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lll5.X, lll5.Y, 0, lll5.X, lll5.Y, (lll5.Z + 2f), 255, 5, 5, 255);

                    Function.Call((Hash)0x6B7256074AE34680, llll1.X, llll1.Y, 0, llll1.X, llll1.Y, (llll1.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llll2.X, llll2.Y, 0, llll2.X, llll2.Y, (llll2.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llll3.X, llll3.Y, 0, llll3.X, llll3.Y, (llll3.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llll4.X, llll4.Y, 0, llll4.X, llll4.Y, (llll4.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llll5.X, llll5.Y, 0, llll5.X, llll5.Y, (llll5.Z + 2f), 255, 5, 5, 255);


                    Function.Call((Hash)0x6B7256074AE34680, lllll1.X, lllll1.Y, 0, lllll1.X, lllll1.Y, (lllll1.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lllll2.X, lllll2.Y, 0, lllll2.X, lllll2.Y, (lllll2.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lllll3.X, lllll3.Y, 0, lllll3.X, lllll3.Y, (lllll3.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lllll4.X, lllll4.Y, 0, lllll4.X, lllll4.Y, (lllll4.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, lllll5.X, lllll5.Y, 0, lllll5.X, lllll5.Y, (lllll5.Z + 2f), 255, 5, 5, 255);


                    Function.Call((Hash)0x6B7256074AE34680, llllll1.X, llllll1.Y, 0, llllll1.X, llllll1.Y, (llllll1.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llllll2.X, llllll2.Y, 0, llllll2.X, llllll2.Y, (llllll2.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llllll3.X, llllll3.Y, 0, llllll3.X, llllll3.Y, (llllll3.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llllll4.X, llllll4.Y, 0, llllll4.X, llllll4.Y, (llllll4.Z + 2f), 255, 5, 5, 255);
                    Function.Call((Hash)0x6B7256074AE34680, llllll5.X, llllll5.Y, 0, llllll5.X, llllll5.Y, (llllll5.Z + 2f), 255, 5, 5, 255);
                }


                Vehicle v1 = World.GetClosestVehicle(l1, sensorRad);
                Vehicle v2 = World.GetClosestVehicle(l2, sensorRad);
                Vehicle v3 = World.GetClosestVehicle(l3, sensorRad);
                Vehicle v4 = World.GetClosestVehicle(l4, sensorRad);
                Vehicle v5 = World.GetClosestVehicle(l5, sensorRad);

                Vehicle vv1 = World.GetClosestVehicle(ll1, sensorRad);
                Vehicle vv2 = World.GetClosestVehicle(ll2, sensorRad);
                Vehicle vv3 = World.GetClosestVehicle(ll3, sensorRad);
                Vehicle vv4 = World.GetClosestVehicle(ll4, sensorRad);
                Vehicle vv5 = World.GetClosestVehicle(ll5, sensorRad);

                Vehicle vvv1 = World.GetClosestVehicle(lll1, sensorRad);
                Vehicle vvv2 = World.GetClosestVehicle(lll2, sensorRad);
                Vehicle vvv3 = World.GetClosestVehicle(lll3, sensorRad);
                Vehicle vvv4 = World.GetClosestVehicle(lll4, sensorRad);
                Vehicle vvv5 = World.GetClosestVehicle(lll5, sensorRad);

                Vehicle vvvv1 = World.GetClosestVehicle(llll1, sensorRad);
                Vehicle vvvv2 = World.GetClosestVehicle(llll2, sensorRad);
                Vehicle vvvv3 = World.GetClosestVehicle(llll3, sensorRad);
                Vehicle vvvv4 = World.GetClosestVehicle(llll4, sensorRad);
                Vehicle vvvv5 = World.GetClosestVehicle(llll5, sensorRad);

                Vehicle vvvvv1 = World.GetClosestVehicle(lllll1, 2f);
                Vehicle vvvvv2 = World.GetClosestVehicle(lllll2, sensorRad);
                Vehicle vvvvv3 = World.GetClosestVehicle(lllll3, sensorRad);
                Vehicle vvvvv4 = World.GetClosestVehicle(lllll4, sensorRad);
                Vehicle vvvvv5 = World.GetClosestVehicle(lllll5, 2f);

                Vehicle vvvvvv1 = World.GetClosestVehicle(llllll1, 10f);
                Vehicle vvvvvv2 = World.GetClosestVehicle(llllll2, 10f);
                Vehicle vvvvvv3 = World.GetClosestVehicle(llllll3, 10f);
                Vehicle vvvvvv4 = World.GetClosestVehicle(llllll4, 10f);
                Vehicle vvvvvv5 = World.GetClosestVehicle(llllll5, 10f);

                Vehicle vvvvvvv1 = World.GetClosestVehicle(lllllll1, 10f);
                Vehicle vvvvvvv2 = World.GetClosestVehicle(lllllll2, 10f);
                Vehicle vvvvvvv3 = World.GetClosestVehicle(lllllll3, 10f);
                Vehicle vvvvvvv4 = World.GetClosestVehicle(lllllll4, 10f);
                Vehicle vvvvvvv5 = World.GetClosestVehicle(lllllll5, 10f);

                Vector3 lt1 = (l1 - veh.Position);
                lt1.Normalize();
                Vector3 lt2 = (l2 - veh.Position);
                lt2.Normalize();
                Vector3 lt3 = (l3 - veh.Position);
                lt3.Normalize();
                Vector3 lt4 = (l4 - veh.Position);
                lt4.Normalize();
                Vector3 lt5 = (l5 - veh.Position);
                lt5.Normalize();
                var brightness = 8f;
                var distance = 400f;
                System.Drawing.Color col = System.Drawing.Color.FromArgb(255, 83, 152, 211);
                //  Color col = Color.FromArgb(255, 42, 255, 0);
                if (v1 == null && vv1 == null && vvv1 == null && vvvv1 == null && vvvvv1 == null && vvvvvv1 == null && vvvvvvv1 == null && !l1sw.IsRunning)
                    Function.Call(Hash.DRAW_SPOT_LIGHT, veh.Position.X, veh.Position.Y, veh.Position.Z - 0, lt1.X, lt1.Y, lt1.Z, col.R, col.G, col.B, distance, brightness, 100f, 5f, 10f);
                else if (v1 != null || vv1 != null || vvv1 != null || vvvv1 != null || vvvvv1 != null || vvvvvv1 != null || vvvvvvv1 != null)
                    l1sw.Start();
                if (v2 == null && vv2 == null && vvv2 == null && vvvv2 == null && vvvvv2 == null && vvvvvv2 == null && vvvvvvv2 == null && !l2sw.IsRunning)
                    Function.Call(Hash.DRAW_SPOT_LIGHT, veh.Position.X, veh.Position.Y, veh.Position.Z - 0, lt2.X, lt2.Y, lt2.Z, col.R, col.G, col.B, distance, brightness, 100f, 5f, 10f);
                else if (v2 != null || vv2 != null || vvv2 != null || vvvv2 != null || vvvvv2 != null || vvvvvv2 != null || vvvvvvv2 != null)
                    l2sw.Start();
                if (v3 == null && vv3 == null && vvv3 == null && vvvv3 == null && vvvvv3 == null && vvvvvv3 == null && vvvvvvv3 == null && !l3sw.IsRunning)
                    Function.Call(Hash.DRAW_SPOT_LIGHT, veh.Position.X, veh.Position.Y, veh.Position.Z - 0, lt3.X, lt3.Y, lt3.Z, col.R, col.G, col.B, distance, brightness, 100f, 5f, 10f);
                else if (v3 != null || vv3 != null || vvv3 != null || vvvv3 != null || vvvvv3 != null || vvvvvv3 != null || vvvvvvv3 != null)
                    l3sw.Start();
                if (v4 == null && vv4 == null && vvv4 == null && vvvv4 == null && vvvvv4 == null && vvvvvv4 == null && vvvvvvv4 == null && !l4sw.IsRunning)
                    Function.Call(Hash.DRAW_SPOT_LIGHT, veh.Position.X, veh.Position.Y, veh.Position.Z - 0, lt4.X, lt4.Y, lt4.Z, col.R, col.G, col.B, distance, brightness, 100f, 5f, 10f);
                else if (v4 != null || vv4 != null || vvv4 != null || vvvv4 != null || vvvvv4 != null || vvvvvv4 != null || vvvvvvv4 != null)
                    l4sw.Start();
                if (v5 == null && vv5 == null && vvv5 == null && vvvv5 == null && vvvvv5 == null && vvvvvv5 == null && vvvvvvv5 == null && !l5sw.IsRunning)
                    Function.Call(Hash.DRAW_SPOT_LIGHT, veh.Position.X, veh.Position.Y, veh.Position.Z - 0, lt5.X, lt5.Y, lt5.Z, col.R, col.G, col.B, distance, brightness, 100f, 5f, 10f);
                else if (v5 != null || vv5 != null || vvv5 != null || vvvv5 != null || vvvvv5 != null || vvvvvv5 != null || vvvvvvv5 != null)
                    l5sw.Start();
            }

        }
        private void HighBeamLights()
        {
            var h = fakeTimeHours;
            if (isHighBeamActive)
            {
                if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
                {
                    veh.HighBeamsOn = false;
                    isHighBeamActive = false;
                    isHighBeamPressing = false;
                    isFlashingHighBeam = false;
                    if (h > 5 && h < 19)
                        veh.LightsOn = false;
                }
            }
            else
            {
                if (Game.IsControlJustPressed(0, GTA.Control.VehicleSelectNextWeapon) && !isHighBeamActive && !isHighBeamPressing)
                {
                    veh.HighBeamsOn = true;
                    highBeamPressingStopWatch = new Stopwatch();
                    highBeamPressingStopWatch.Start();
                }
                if (highBeamPressingStopWatch.IsRunning && highBeamPressingStopWatch.ElapsedMilliseconds >= 300)
                {
                    isHighBeamPressing = true;
                    canFlashHighBeam = true;
                    highBeamPressingStopWatch = new Stopwatch();
                }
                if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon) && highBeamPressingStopWatch.IsRunning && highBeamPressingStopWatch.ElapsedMilliseconds < 300)
                {
                    isHighBeamActive = true;
                    veh.HighBeamsOn = true;
                    isHighBeamPressing = false;
                    highBeamPressingStopWatch = new Stopwatch();

                }
                if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon) && isFlashingHighBeam)
                {
                    veh.HighBeamsOn = false;
                    isHighBeamActive = false;
                    flashHighBeamStopwatch = new Stopwatch();
                    waitForFlashHighBeamStopwatch = new Stopwatch();
                    isFlashingHighBeam = false;
                    isHighBeamPressing = false;
                    canFlashHighBeam = true;
                    flashHighBeamTicks = 0;
                    veh.SirenActive = false;
                    if (h > 5 && h < 19)
                        veh.LightsOn = false;
                }
                /* if(isHighBeamPressing && highBeamSirene == null)
                 {
                     highBeamSirene = World.CreateVehicle(new Model(VehicleHash.Police), veh.GetOffsetInWorldCoords(new Vector3(0, 15, 0)), veh.Heading);
                     highBeamSirene.CreatePedOnSeat(VehicleSeat.Driver, new Model(PedHash.Autoshop01SMM));
                     highBeamSirene.EngineRunning = true;
                     highBeamSirene.Speed = veh.Speed;
                     highBeamSirene.SirenActive = true;
                     highBeamSirene.IsSirenSilent = true;
                     highBeamSirene.PlaceOnGround();

                 }*/
                if (canFlashHighBeam && isHighBeamPressing)
                {
                    if (flashHighBeamTicks == getForceForHighBeam())
                    {
                        canFlashHighBeam = false;
                        flashHighBeamTicks = 0;
                        veh.HighBeamsOn = false;

                    }
                    else
                    {
                        if (isHighBeamPressing && !flashHighBeamStopwatch.IsRunning)
                        {
                            isFlashingHighBeam = true;
                            veh.SirenActive = true;
                            flashHighBeamStopwatch = new Stopwatch();
                            flashHighBeamStopwatch.Start();
                        }
                        if (isHighBeamPressing && flashHighBeamStopwatch.ElapsedMilliseconds > 100 && flashHighBeamStopwatch.IsRunning)
                        {
                            flashHighBeamStopwatch = new Stopwatch();
                            veh.HighBeamsOn = veh.HighBeamsOn ? false : true;
                            veh.LightsOn = true;
                            flashHighBeamTicks++;
                        }
                    }
                }
                if (isHighBeamPressing && !waitForFlashHighBeamStopwatch.IsRunning && !canFlashHighBeam)
                {
                    waitForFlashHighBeamStopwatch = new Stopwatch();
                    waitForFlashHighBeamStopwatch.Start();
                }
                if (isHighBeamPressing && waitForFlashHighBeamStopwatch.IsRunning && waitForFlashHighBeamStopwatch.ElapsedMilliseconds > 600)
                {
                    waitForFlashHighBeamStopwatch = new Stopwatch();
                    canFlashHighBeam = true;
                }
            }
        }

        private int getForceForHighBeam()
        {
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1)
            {
                return 99;
            }
            if (Game.IsControlPressed(0, GTA.Control.VehicleBrake))
            {
                return 6;
            }
            return 4;
        }

        /*
          private void HighBeamOn()
        {
            if (!veh.LightsOn)
            {
                veh.LightsOn = true;
                isDay = true; 
            }
            isHighBeamActive = true;
            veh.LightsMultiplier = 2.0f;
            Function.Call((Hash)0x8B7FD87F0DDB421E, Game.Player.LastVehicle, true);
        }

        private void HighBeamPressing()
        {
            isHighBeamPressing = true;
            veh.SirenActive = true;
        }

        private void HighBeamOff()
        {
            if (isDay)
            {
                veh.LightsOn = false;
                isDay = false;
            }
            isHighBeamActive = false;
            veh.LightsMultiplier = 1.0f;
            Function.Call((Hash)0x8B7FD87F0DDB421E, Game.Player.LastVehicle, false);
            veh.SirenActive = false;
        }
             */
        private Stopwatch chillAccelerationToggleStopwatch = new Stopwatch();
        private void ChillAcceleration()
        {
            if (!chillAccelerationToggleStopwatch.IsRunning)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == XInputDotNetPure.ButtonState.Pressed)
                {
                    chillAccelerationToggleStopwatch.Start();
                    // isChillMode = isChillMode ? false : true;
                    // UI.Notify("Chill mode " + (isChillMode ? "ON" : "OFF"));
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Guide == XInputDotNetPure.ButtonState.Released && chillAccelerationToggleStopwatch.IsRunning)
            {
                chillAccelerationToggleStopwatch = new Stopwatch();
                isChillMode = !isChillMode;
            }

            if (isChillMode)
            {
                if (chillAccelerationSpeedCheck.IsRunning && chillAccelerationSpeedCheck.ElapsedMilliseconds > 100)
                {
                    float factor = 0.17f;
                    float diff = veh.Speed - lastSpeed;
                    if ((diff > factor && fakeSpeed > 20) || (fakeSpeed > 75))
                    {
                        disableAcceleration = true;
                    }
                    else
                    {
                        disableAcceleration = false;
                    }
                }
                if (!chillAccelerationSpeedCheck.IsRunning)
                {
                    chillAccelerationSpeedCheck = new Stopwatch();
                    chillAccelerationSpeedCheck.Start();
                }
            }
            if (disableAcceleration)
            {
                if (Game.IsControlPressed(0, GTA.Control.VehicleBrake))
                {
                    //Game.DisableControlThisFrame(0, GTA.Control.VehicleBrake);
                }
                else
                {
                    Game.DisableControlThisFrame(0, GTA.Control.VehicleAccelerate);
                }
            }
        }

        public static double tempOdo = 0;
        private void Odometer()
        {
            if (Game.Player.Character.IsInVehicle(veh) && veh.ClassType != VehicleClass.Planes)
            {
                if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && cruiseControlActivationStopWatch.ElapsedMilliseconds <= 200 && cruiseControlActivationStopWatch.IsRunning && speedInKmh > 2)
                {
                    cruiseControlActivationStopWatch.Stop();
                  //  odometer = 0;
                }
                if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && speedInKmh > 2)
                {
                    cruiseControlActivationStopWatch = new Stopwatch();
                    cruiseControlActivationStopWatch.Start();
                }
                if (speedInKmh > 0)
                {
                    odometer += (fakeSpeed * (TimeSpan.FromMilliseconds(1).TotalHours * 16)) * 13;
                }
                /*   var cont = new UIContainer(new Point(5, UI.HEIGHT - 137), new Size(100, 20), System.Drawing.Color.FromArgb(80, 0, 0, 0));
                   cont.Items.Add(new UIText((odometer.ToString("0.0") + " km").Replace(",", "."), new Point(40, 2), 0.3f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                   cont.Enabled = true;
                   cont.Draw();*/
                var cluster = new UIContainer(new Point(40, UI.HEIGHT - 200), new Size(140, 60), System.Drawing.Color.FromArgb(80, 0, 0, 0));
                if (!isEngineRunning)
                {
                    if (isEvVehicle && isChargerCordEv)
                    {
                        var start = 14f;
                        var end = 122f;
                        var currentFuel = start + (end - start) * (ElectricEngine.batteryRemainingPercentage / 100);
                        cluster.Items.Add(new UIContainer(new Point(14, 33), new Size((int)(end - start), 16), System.Drawing.Color.FromArgb(40, 65, 105, 225)));
                        cluster.Items.Add(new UIContainer(new Point(14, 33), new Size((int)(currentFuel - start), 16), System.Drawing.Color.RoyalBlue));
                        cluster.Items.Add(new UIText(Math.Round(ElectricEngine.batteryRemainingPercentage, 0).ToString() + "%", new Point(70, 6), 0.46f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                    }
                    else if (isPhevVehicle && isChargerCord)
                    {
                        var start = 14f;
                        var end = 122f;
                        var currentFuel = start + (end - start) * (HybridEngine.batteryRemainingPercentage / 100);
                        cluster.Items.Add(new UIContainer(new Point(14, 33), new Size((int)(end - start), 16), System.Drawing.Color.FromArgb(40, 65, 105, 225)));
                        cluster.Items.Add(new UIContainer(new Point(14, 33), new Size((int)(currentFuel - start), 16), System.Drawing.Color.RoyalBlue));
                        cluster.Items.Add(new UIText(Math.Round(HybridEngine.batteryRemainingPercentage, 0).ToString() + "%", new Point(70, 6), 0.46f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                    }
                    else
                    {
                        cluster.Items.Add(new UIText("OFF", new Point(70, 13), 0.46f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                    }
                }
                else
                {
                    System.Drawing.Color speedColor = System.Drawing.Color.White;
                    if (isPhevOnElectricEngine)
                        speedColor = System.Drawing.Color.RoyalBlue;
                    if (isCruiseControlActive)
                        speedColor = System.Drawing.Color.LawnGreen;
                    if (speedInKmh > (isRaining ? 140 : 230))
                        speedColor = System.Drawing.Color.OrangeRed;
                    if (isBoostMode || drivingMode == "boost")
                    {
                        speedColor = System.Drawing.Color.FromArgb(255, 211, 26, 23);
                    }
                    var h = fakeTimeHours;
                    var m = fakeTimeMinutes;

                    cluster.Items.Add(new UIText(fakeSpeed.ToString(), new Point(70, 13), 0.46f, speedColor, GTA.Font.ChaletLondon, true));
                    cluster.Items.Add(new UIText("KM/H", new Point(70, 33), 0.2f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                    cluster.Items.Add(new UIText(h + ":" + (m.ToString().Length == 1 ? 0 + "" + m : m.ToString()), new Point(124, 2), 0.24f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                    if ((isEv && !isBoostMode && isHybridMode) || isElectricMode)
                    {
                        // cluster.Items.Add(new UIText("EV", new Point(90, 20), 0.24f, System.Drawing.Color.RoyalBlue, GTA.Font.ChaletLondon, true));
                    }
                    if (!isCreep && speedInKmh < 1 && !isParkingGear)
                    {
                        cluster.Items.Add(new UIText("(H)", new Point(14, 2), 0.2f, System.Drawing.Color.Gray, GTA.Font.ChaletLondon, true));
                    }
                    if (isParkingGear)
                    {
                        cluster.Items.Add(new UIText("(P)", new Point(35, 2), 0.2f, System.Drawing.Color.Red, GTA.Font.ChaletLondon, true));
                    }
                    if (!isESPActive)
                    {
                        cluster.Items.Add(new UIText("TCS/ESP", new Point(55, 4), 0.115f, System.Drawing.Color.FromArgb(255, 247, 189, 1), GTA.Font.ChaletLondon, true));
                    }
                    if (veh.LightsOn)
                    {
                        cluster.Items.Add(new UIText("Œ", new Point(81, 2), 0.2f, System.Drawing.Color.LawnGreen, GTA.Font.ChaletLondon, true));
                    }
                    if (veh.HighBeamsOn && !isMatrixLedEnabled)
                    {
                        cluster.Items.Add(new UIText("Œ", new Point(100, 2), 0.2f, System.Drawing.Color.DodgerBlue, GTA.Font.ChaletLondon, true));
                    }
                    if (veh.LightsOn && isMatrixLedEnabled)
                    {
                        cluster.Items.Add(new UIText("Œ", new Point(100, 2), 0.2f, System.Drawing.Color.DodgerBlue, GTA.Font.ChaletLondon, true));
                        cluster.Items.Add(new UIText("MATRIX", new Point(99, 11), 0.07f, System.Drawing.Color.DodgerBlue, GTA.Font.ChaletLondon, true));
                    }
                    if (isParkingMode)
                    {
                        cluster.Items.Add(new UIText(@"|P|", new Point(36, 18), 0.3f, System.Drawing.Color.Blue, GTA.Font.ChaletLondon, true));
                    }
                    if (HighBeam.bus.BusControls.isOpeningDoor || HighBeam.bus.BusControls.doorAngle >= 1f)
                    {
                        cluster.Items.Add(new UIText("<| |>", new Point(34, 22), 0.3f, HighBeam.bus.BusControls.isOpeningDoor ? System.Drawing.Color.Yellow : System.Drawing.Color.OrangeRed, GTA.Font.ChaletLondon, true));
                    }
                    if (isTruckMode)
                    {
                        cluster.Items.Add(new UIText("HAUL", new Point(34, 22), 0.17f, System.Drawing.Color.Gray, GTA.Font.ChaletLondon, true));
                    }
                    if (isLaunchControl)
                    {
                        cluster.Items.Add(new UIText("LC", new Point(34, 22), 0.17f, System.Drawing.Color.Red, GTA.Font.ChaletLondon, true));
                    }
                    if (isCruiseControlActive)
                    {
                        cluster.Items.Add(new UIText("(" + cruiseControlSpeed.ToString() + ")", new Point(114, 17), 0.3f, System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                    }
                    //traffic 
                    cluster.Items.Add(new UIText(isOnAutobahn ? "hwy" : tv.ToString(), new Point(15, 38), 0.17f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));

                    //fuel
                    var start = 14f;
                    var end = 50f;
                    var currentFuel = start + (end - start) * (fuelRemainingPercentage / 100);
                    var heigh = 48;

                    if (true)
                    {
                        cluster.Items.Add(new UIText("E", new Point(6, heigh + 1), 0.13f, System.Drawing.Color.LightGray, GTA.Font.ChaletLondon, true));

                        if (isPhevVehicle)
                        {
                            var currentBattery = start + (end - start) * (HybridEngine.batteryRemainingPercentage / 100);
                            cluster.Items.Add(new UIContainer(new Point(14, heigh + 6), new Size((int)(end - start), isPhevVehicle ? 3 : 5), System.Drawing.Color.FromArgb(40, 65, 105, 225)));
                            cluster.Items.Add(
                                new UIContainer(
                                    new Point(14, heigh + 6),
                                    new Size((int)(currentBattery - start),
                                    isPhevVehicle ? 3 : 5),
                                    isRangeExtenderRunning && !isPhevOnElectricEngine ? System.Drawing.Color.FromArgb(220, 68, 186, 70) : System.Drawing.Color.RoyalBlue)
                                );
                        }
                        if (isEvVehicle)
                        {
                            var currentBattery = start + (end - start) * (ElectricEngine.batteryRemainingPercentage / 100);
                            cluster.Items.Add(new UIContainer(new Point(14, heigh + 3), new Size((int)(end - start), 5), System.Drawing.Color.FromArgb(40, 65, 105, 225)));
                            cluster.Items.Add(new UIContainer(new Point(14, heigh + 3), new Size((int)(currentBattery - start), 5), System.Drawing.Color.RoyalBlue));
                        }
                        else
                        {
                            cluster.Items.Add(new UIContainer(new Point(14, heigh + 3), new Size((int)(end - start), isPhevVehicle ? 3 : 5), System.Drawing.Color.FromArgb(40, 227, 111, 75)));
                            cluster.Items.Add(new UIContainer(new Point(14, heigh + 3), new Size((int)(currentFuel - start), isPhevVehicle ? 3 : 5), System.Drawing.Color.FromArgb(255, 227, 111, 75)));
                        }
                        cluster.Items.Add(new UIText("|", new Point(14, heigh), 0.19f, System.Drawing.Color.Red, GTA.Font.ChaletLondon, true));
                        cluster.Items.Add(new UIText("|", new Point(23, heigh + 1), 0.15f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                        cluster.Items.Add(new UIText("|", new Point(32, heigh), 0.19f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                        cluster.Items.Add(new UIText("|", new Point(41, heigh + 1), 0.15f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                        cluster.Items.Add(new UIText("|", new Point(50, heigh), 0.19f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));

                        cluster.Items.Add(new UIText("F", new Point(59, heigh + 1), 0.13f, System.Drawing.Color.LightGray, GTA.Font.ChaletLondon, true));
                        if (isLowFuel && !isEvVehicle)
                            cluster.Items.Add(new UIText("LOW FUEL", new Point(47, heigh - 5), 0.12f, System.Drawing.Color.Yellow, GTA.Font.ChaletLondon, true));
                        var consump = isPhevOnElectricEngine ? HybridEngine.instantBatteryConsumption : instantFuelConsumption;
                        var color = isPhevOnElectricEngine ? System.Drawing.Color.RoyalBlue : System.Drawing.Color.White;
                        if (isEvVehicle)
                        {
                            consump = ElectricEngine.instantBatteryConsumption;
                            color = System.Drawing.Color.RoyalBlue;
                        }
                        cluster.Items.Add(new UIText((consump == 0 ? "0.0" : consump.ToString()), new Point(81, heigh - 1), 0.19f, color, GTA.Font.ChaletLondon, true));
                    }

                  //  cluster.Items.Add(new UIText((Math.Round(odo, 0).ToString()), new Point(126, heigh), 0.16f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));
                    cluster.Items.Add(new UIText((Math.Floor(tempOdo).ToString()), new Point(126, heigh), 0.16f, System.Drawing.Color.White, GTA.Font.ChaletLondon, true));

                    string gear = "N";
                    if (isCreep || !isCreep || speedInKmh > 0)
                        gear = "D";
                    if (isReversing)
                        gear = "R";
                    if (isParkingGear)
                        gear = "P";
                    if (isParkingMode)
                        gear = "N";
                    System.Drawing.Color driveGearColor = System.Drawing.Color.White;
                    if (drivingMode == "eco")
                        driveGearColor = System.Drawing.Color.SpringGreen;
                    if (drivingMode == "sport")
                        driveGearColor = System.Drawing.Color.OrangeRed;
                    if (drivingMode == "ev")
                        driveGearColor = System.Drawing.Color.RoyalBlue;
                    if (drivingMode == "boost")
                    {
                        driveGearColor = System.Drawing.Color.FromArgb(255, 211, 26, 23);
                    }
                    cluster.Items.Add(new UIText("P", new Point(98, 34), 0.27f, gear == "P" ? System.Drawing.Color.White : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                    cluster.Items.Add(new UIText("R", new Point(110, 34), 0.27f, gear == "R" ? System.Drawing.Color.White : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                    cluster.Items.Add(new UIText("N", new Point(122, 34), 0.27f, gear == "N" ? System.Drawing.Color.White : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                    cluster.Items.Add(new UIText("D", new Point(134, 34), 0.27f, gear == "D" ? driveGearColor : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                    if (hasAirSuspension)
                    {
                        cluster.Items.Add(new UIText(@"^", new Point(14, 20), 0.25f, airSuspensionMode == 3 ? System.Drawing.Color.OrangeRed : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                        cluster.Items.Add(new UIText(@"_", new Point(14, 19), 0.25f, airSuspensionMode == 2 ? System.Drawing.Color.OrangeRed : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                        cluster.Items.Add(new UIText(@"v", new Point(14, 28), 0.25f, airSuspensionMode == 1 ? System.Drawing.Color.OrangeRed : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                    }
                    if (hasActiveExhaust)
                    {
                        cluster.Items.Add(new UIText(@"EXHAUST", new Point(22, 27), 0.12f, isActiveExhaustActivated ? System.Drawing.Color.OrangeRed : System.Drawing.Color.DarkSlateGray, GTA.Font.ChaletLondon, true));
                    }
                    if (isSportMode)
                    {
                        cluster.Items.Add(new UIText(@"SPORT", new Point(22, 27), 0.12f, System.Drawing.Color.OrangeRed, GTA.Font.ChaletLondon, true));
                    }
                }
                if (isCruiseControlActive || (addaptiveDisengagementStopWatch.IsRunning))
                {
                    var c = new UIContainer(new Point(190, UI.HEIGHT - 190), new Size(90, 50), System.Drawing.Color.FromArgb(80, 0, 0, 0));
                    if (addaptiveDisengagementStopWatch.IsRunning)
                    {
                        c.Items.Add(new UIText(@"Regain Control", new Point(3, 2), 0.2f, System.Drawing.Color.White, GTA.Font.ChaletLondon, false));
                        cruiseControlClusterBackgroundAlpha += addaptiveDisengagementStopWatch.ElapsedMilliseconds % 500 > 250 ? -4 : 4;
                        if (cruiseControlClusterBackgroundAlpha > 255)
                            cruiseControlClusterBackgroundAlpha = 255;
                        if (cruiseControlClusterBackgroundAlpha < 0)
                            cruiseControlClusterBackgroundAlpha = 0;
                        c.Items.Add(new UIContainer(new Point(0, 0), new Size(90, 50), System.Drawing.Color.FromArgb(cruiseControlClusterBackgroundAlpha, 246, 52, 33)));
                    }
                    else
                    {
                        cruiseControlClusterBackgroundAlpha = 0;
                    }
                    var linesSpace = "             ";
                    var linesHeight = 40;
                    var linesWidth = 14;
                    var carHeight = 10;
                    var linesCount = 3;
                    var linesColor = System.Drawing.Color.LightGreen;
                    if (addaptiveCarAhead != null)
                    {
                        var dist = addaptiveRearPos.DistanceTo(addaptiveOwnFrontPos);
                        if (dist < 10f)
                        {
                            linesColor = System.Drawing.Color.Orange;
                            linesCount = 2;
                            carHeight = 30;
                        }
                        if (dist < 4f)
                        {
                            linesColor = System.Drawing.Color.Red;
                            linesCount = 1;
                            carHeight = 40;
                        }
                    }

                    for (var i = 0; i < linesCount; ++i)
                    {
                        c.Items.Add(new UIText($@"/{linesSpace}\", new Point(linesWidth, linesHeight), 0.23f, linesColor, GTA.Font.ChaletLondon, false));
                        linesSpace = linesSpace.Substring(0, linesSpace.Length - 2);
                        linesHeight -= 10;
                        linesWidth += 4;
                    }
                    if (addaptiveCarAhead != null)
                    {
                        c.Items.Add(new UIText($@" __", new Point(30, carHeight - 9), 0.23f, System.Drawing.Color.White, GTA.Font.ChaletLondon, false));
                        c.Items.Add(new UIText($@"|__|", new Point(30, carHeight), 0.23f, System.Drawing.Color.White, GTA.Font.ChaletLondon, false));
                    }
                    c.Draw();
                }
                var anyPsensor = PSbackLow || PSbackMiddle || PSbackHigh || PSfrontHigh || PSfrontMiddle || PSfrontLow;
                if (isParkingMode || (anyPsensor && fakeSpeed < 20 && false))
                {
                    var carHeight = 10;
                    var c = new UIContainer(new Point(190, UI.HEIGHT - 190), new Size(90, 50), System.Drawing.Color.FromArgb(80, 0, 0, 0));
                    var anyFront = PSfrontHigh || PSfrontMiddle || PSfrontLow;
                    if (anyFront)
                    {
                        var color = System.Drawing.Color.Yellow;
                        var h = 19;
                        if (PSfrontLow)
                        {
                            color = System.Drawing.Color.YellowGreen;

                        }
                        if (PSfrontMiddle)
                        {
                            color = System.Drawing.Color.Orange;
                            h = 16;
                        }
                        if (PSfrontHigh)
                        {
                            color = System.Drawing.Color.Red;
                            h = 12;
                        }
                        c.Items.Add(new UIText($@" __", new Point(30, carHeight - h), 0.23f, color, GTA.Font.ChaletLondon, false));
                    }

                    c.Items.Add(new UIText($@" __", new Point(30, carHeight - 9), 0.23f, System.Drawing.Color.White, GTA.Font.ChaletLondon, false));
                    c.Items.Add(new UIText($@"|      |", new Point(30, carHeight), 0.23f, System.Drawing.Color.White, GTA.Font.ChaletLondon, false));
                    c.Items.Add(new UIText($@"|      |", new Point(30, carHeight + 9), 0.23f, System.Drawing.Color.White, GTA.Font.ChaletLondon, false));
                    c.Items.Add(new UIText($@"|__|", new Point(30, carHeight + 18), 0.23f, System.Drawing.Color.White, GTA.Font.ChaletLondon, false));

                    anyFront = PSbackLow || PSbackMiddle || PSbackHigh;
                    carHeight += 19;
                    if (anyFront)
                    {
                        var color = System.Drawing.Color.Yellow;
                        var h = 10;
                        if (PSbackLow)
                        {
                            color = System.Drawing.Color.YellowGreen;

                        }
                        if (PSbackMiddle)
                        {
                            color = System.Drawing.Color.Orange;
                            h = 6;
                        }
                        if (PSbackHigh)
                        {
                            color = System.Drawing.Color.Red;
                            h = 2;
                        }
                        c.Items.Add(new UIText($@" __", new Point(30, carHeight + h), 0.23f, color, GTA.Font.ChaletLondon, false));
                    }

                    c.Draw();
                }




                cluster.Enabled = true;
                cluster.Draw();

                if (((espClusterRunningStopwatch.ElapsedMilliseconds % 200 < 100)) && espClusterRunningStopwatch.IsRunning)
                {
                    var espIcon = new UIContainer(new Point(78, UI.HEIGHT - 198), new Size(18, 8), System.Drawing.Color.FromArgb(255, 247, 189, 1));
                    espIcon.Items.Add(new UIText("ESP", new Point(0, 0), 0.14f, System.Drawing.Color.Black, GTA.Font.ChaletLondon, false));
                    espIcon.Draw();
                }

                if (((tcsClusterRunningStopwatch.ElapsedMilliseconds % 200 < 100)) && tcsClusterRunningStopwatch.IsRunning)
                {
                    var espIcon = new UIContainer(new Point(56, UI.HEIGHT - 198), new Size(18, 8), System.Drawing.Color.FromArgb(255, 247, 189, 1));
                    espIcon.Items.Add(new UIText("TCS", new Point(0, 0), 0.14f, System.Drawing.Color.Black, GTA.Font.ChaletLondon, false));
                    espIcon.Draw();
                }
            }
        }

        private void TruckMode()
        {
            if (isTruckMode)
            {
                if (!Game.Player.Character.IsInVehicle(veh)
                    && Game.Player.Character.Position.DistanceTo(truckTrailer.Position) < 4
                    && Game.IsControlJustPressed(0, GTA.Control.ScriptPadRight) && !isContainerWarehouse)
                {
                    if (truckTrailerContainer != null)
                    {
                        isContainerAttached = false;
                        truckTrailerContainerMovingStopwatch.Start();
                    }
                    else
                    {
                        containerPos = new Vector3(0, -1.15f, 3f);
                        truckTrailerContainer = World.CreateProp(new Model(containerList[GenerateRandomNumberBetween(0, containerList.Count - 1)]), new Vector3(0, 0, 0), false, false);
                        truckTrailerContainer.AttachTo(truckTrailer, 0, containerPos, new Vector3(0, 0, 0));
                        truckTrailerContainerMovingStopwatch.Start();
                        isContainerAttached = true;
                    }
                }
                if (truckTrailerContainer != null && !Game.Player.Character.IsInVehicle())
                {
                    TruckTrailerRamp();
                }
                if (truckTrailer != null)
                {
                  //  truckTrailer.HandbrakeOn = GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.8;
                    if (Game.IsControlJustPressed(0, GTA.Control.Jump) && Game.Player.Character.Position.DistanceTo(truckTrailer.Position) < 6 && veh.DisplayName.ToLower().Contains("caddy"))
                    {
                        truckTrailer.Repair();
                    }
                }
            }
            if (truckTrailerContainerMovingStopwatch.IsRunning)
            {
                containerPos = new Vector3(containerPos.X, containerPos.Y, isContainerAttached ? (containerPos.Z - 0.004f) : (containerPos.Z + 0.004f));
                if (isContainerAttached ? (containerPos.Z > -1.41f) : (containerPos.Z < 3f))
                {
                    truckTrailerContainer.AttachTo(truckTrailer, 0, containerPos, new Vector3(0, 0, 0));
                }
                else
                {
                    truckTrailerContainerMovingStopwatch = new Stopwatch();
                    if (!isContainerAttached)
                    {
                        truckTrailerContainer.Delete();
                        truckTrailerContainer = null;
                    }
                }
            }
        }

        private bool isTrailerRampAttached = false;
        private List<Prop> rampProps = new List<Prop>();
        private void TruckTrailerRamp()
        {
            if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadLeft)
                && !truckTrailerContainerMovingStopwatch.IsRunning
                && Game.Player.Character.Position.DistanceTo(truckTrailer.Position) < 4f)
            {
                if (isTrailerRampAttached)
                {
                    isTrailerRampAttached = false;
                    truckTrailerContainer.Alpha = 255;
                    truckTrailerContainer.AttachTo(truckTrailer, 0, containerPos, new Vector3());
                    for (var i = 0; i < rampProps.Count; ++i)
                    {
                        rampProps[i].Delete();
                    }
                    rampProps = new List<Prop>();
                    trailerContent = World.GetNearbyProps(truckTrailer.Position, 15f, new Model(153748523)).ToList();
                    for (var i = 0; i < trailerContent.Count; ++i)
                    {
                        trailerContent[i].Delete();
                    }
                    truckTrailer.HasCollision = true;
                    truckTrailer.FreezePosition = false;
                }
                else
                {

                    isTrailerRampAttached = true;
                    truckTrailerContainer.Alpha = 0;
                    truckTrailerContainer.AttachTo(truckTrailer, 0, new Vector3(0, 0, 50f), new Vector3());
                    var fakeContainer = World.CreateProp(new Model(1022953480), containerPos, false, false);
                    Function.Call((Hash)0x9EBC85ED0FFFE51C, fakeContainer, true, true);
                    fakeContainer.AttachTo(truckTrailer, 0, containerPos, new Vector3(0, 0, 0));
                    rampProps.Add(fakeContainer);
                    var fakeContFloor1 = World.CreateProp(new Model(2067252279), new Vector3(0, 0, 0), false, false);
                    fakeContFloor1.AttachTo(truckTrailer, 0, new Vector3(0, 1.4f, -1.28f), new Vector3(0, 0, 0));
                    fakeContFloor1.HasCollision = true;
                    rampProps.Add(fakeContFloor1);
                    var fakeContFloor2 = World.CreateProp(new Model(2067252279), new Vector3(0, 0, 0), false, false);
                    fakeContFloor2.AttachTo(truckTrailer, 0, new Vector3(0, -3.9f, -1.28f), new Vector3(0, 0, 0));
                    fakeContFloor2.HasCollision = true;
                    rampProps.Add(fakeContFloor2);

                    var fakeContRamp = World.CreateProp(new Model(2067252279), new Vector3(0, 0, 0), false, false);
                    fakeContRamp.AttachTo(truckTrailer, 0, new Vector3(0, -10.2f, -2.27f), new Vector3(18, 0, 0));
                    fakeContRamp.HasCollision = true;
                    rampProps.Add(fakeContRamp);
                    truckTrailer.HasCollision = false;
                    truckTrailer.FreezePosition = true;
                }
            }
        }

        private void Indicators()
        {
            var p = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 80, 10));
            bool isInVeh = Game.Player.Character.IsInVehicle();
            //  var xr = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.08f, 100, 10));
            if ((Game.IsControlJustPressed(0, GTA.Control.VehicleAim)
              || Game.IsControlJustPressed(0, GTA.Control.VehicleAttack)) && hazardsActive && isInVeh)
            {
                hazardsActive = false;
                isLeftIndicatorOn = false;
                isRightIndicatorOn = false;
                veh.RightIndicatorLightOn = false;
                veh.LeftIndicatorLightOn = false;
                if (isTruckMode)
                {
                    truckTrailer.LeftIndicatorLightOn = false;
                    truckTrailer.RightIndicatorLightOn = false;
                }
                afterHazardsTurnedOffStopWatch.Start();
            }
            else if (afterHazardsTurnedOffStopWatch.ElapsedMilliseconds > 500 || !afterHazardsTurnedOffStopWatch.IsRunning)
            {
                afterHazardsTurnedOffStopWatch = new Stopwatch();
                if ((Game.IsControlJustPressed(0, GTA.Control.VehicleAim)
               || Game.IsControlJustPressed(0, GTA.Control.VehicleAttack))
               && ((hazardsActivationStopWatch.ElapsedMilliseconds <= 200
                   && hazardsActivationStopWatch.IsRunning))
               )
                {
                    hazardsActivationStopWatch.Stop();
                    hazardsActive = true;
                    isLeftIndicatorOn = true;
                    isRightIndicatorOn = true;
                    veh.RightIndicatorLightOn = true;
                    veh.LeftIndicatorLightOn = true;
                    if (isTruckMode)
                    {
                        truckTrailer.LeftIndicatorLightOn = true;
                        truckTrailer.RightIndicatorLightOn = true;
                    }
                }
                if ((Game.IsControlJustPressed(0, GTA.Control.VehicleAim) || Game.IsControlJustPressed(0, GTA.Control.VehicleAttack)) && !hazardsActive && isInVeh)
                {
                    hazardsActivationStopWatch = new Stopwatch();
                    hazardsActivationStopWatch.Start();
                }
                if (Game.IsControlJustPressed(0, GTA.Control.VehicleAim) && !hazardsActive && isInVeh)
                {
                    Game.Player.Character.Weapons.RemoveAll();
                    if (isLeftIndicatorOn)
                    {
                        isLeftIndicatorOn = false;
                        veh.LeftIndicatorLightOn = false;
                        isRightIndicatorOn = false;
                        veh.RightIndicatorLightOn = false;
                        if (isTruckMode)
                        {
                            truckTrailer.LeftIndicatorLightOn = false;
                            truckTrailer.RightIndicatorLightOn = false;
                        }
                    }
                    else
                    {
                        isRightIndicatorOn = false;
                        veh.RightIndicatorLightOn = false;
                        isLeftIndicatorOn = true;
                        veh.LeftIndicatorLightOn = true;
                        if (isTruckMode)
                        {
                            truckTrailer.LeftIndicatorLightOn = true;
                            truckTrailer.RightIndicatorLightOn = false;
                        }

                    }
                }
                if (Game.IsControlJustPressed(0, GTA.Control.VehicleAttack) && !hazardsActive && isInVeh)
                {
                    if (isRightIndicatorOn)
                    {
                        isRightIndicatorOn = false;
                        veh.RightIndicatorLightOn = false;
                        isLeftIndicatorOn = false;
                        veh.LeftIndicatorLightOn = false;
                        if (isTruckMode)
                        {
                            truckTrailer.LeftIndicatorLightOn = false;
                            truckTrailer.RightIndicatorLightOn = false;
                        }
                    }
                    else
                    {
                        isLeftIndicatorOn = false;
                        veh.LeftIndicatorLightOn = false;
                        isRightIndicatorOn = true;
                        veh.RightIndicatorLightOn = true;
                        if (isTruckMode)
                        {
                            truckTrailer.LeftIndicatorLightOn = false;
                            truckTrailer.RightIndicatorLightOn = true;
                        }
                    }
                }
            }
            if (veh.DisplayName.Contains("675"))
            {
                //  Function.Call((Hash)0x115722B1B9C14C1C, veh);
            }
        }

        private void AddaptiveBrakeLights()
        {
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1 && !calculatingSpeedDiff && speedInKmh > 80 && !isCruiseControlActive)
            {
                speedDiffStopWatch = new Stopwatch();
                speedDiffStopWatch.Start();
                calculatingSpeedDiff = true;
                prevSpeed = speedInKmh;
            }
            if (calculatingSpeedDiff && speedDiffStopWatch.ElapsedMilliseconds > 400)
            {
                double brakeForce = (double)(Function.Call<float>((Hash)0xAD7E85FC227197C4, veh));
                var currSpeed = speedInKmh;
                var speedDiff = prevSpeed - currSpeed;
                int neededSpeedDiff = 0;
                if (brakeForce >= 0.7)
                    neededSpeedDiff = 10;
                if (brakeForce < 0.7)
                    neededSpeedDiff = 8;
                if (brakeForce <= 0.4)
                    neededSpeedDiff = 7;
                if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1 && veh.MaxBraking >= 0.40f && !isCruiseControlActive)
                {
                    canTurnOnAddaptiveLights = true;
                }
                speedDiffStopWatch = new Stopwatch();
                calculatingSpeedDiff = false;


            }
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left < 1)
            {
                speedDiffStopWatch.Stop();
                calculatingSpeedDiff = false;
                canTurnOnAddaptiveLights = false;
            }

            if (addaptiveSequenceLightsOff && addaptiveSequence)
            {
                veh.BrakeLightsOn = false;
                isAdditionalBrakeLightOn = false;
            }
            if (!addaptiveSequenceLightsOff && addaptiveSequence)
            {
                veh.BrakeLightsOn = true;
                isAdditionalBrakeLightOn = true;
            }
            if (addaptiveSequence)
            {
                stopwatch.Start();
                if (stopwatch.Elapsed.Milliseconds > 100 && !addaptiveSequenceLightsOff)
                {
                    stopwatch = new Stopwatch();
                    addaptiveSequenceLightsOff = true;
                }
                if (stopwatch.Elapsed.Milliseconds > 100 && addaptiveSequenceLightsOff)
                {
                    stopwatch = new Stopwatch();
                    addaptiveSequenceLightsOff = false;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 1 && !addaptiveSequence && canTurnOnAddaptiveLights)
            {
                addaptiveSequence = true;
            }
            if ((GamePad.GetState(PlayerIndex.One).Triggers.Left < 1 || speedInKmh == 0) && addaptiveSequence)
            {
                if (speedInKmh <= 20)
                {
                    veh.RightIndicatorLightOn = true;
                    veh.LeftIndicatorLightOn = true;
                    isLeftIndicatorOn = true;
                    isRightIndicatorOn = true;
                    hazardsAfterAddaptiveBraking = true;
                }
                addaptiveSequence = false;
            }
            if (hazardsAfterAddaptiveBraking && speedInKmh > 20)
            {
                veh.RightIndicatorLightOn = false;
                veh.LeftIndicatorLightOn = false;
                isRightIndicatorOn = false;
                isLeftIndicatorOn = false;
                hazardsAfterAddaptiveBraking = false;
            }
            if ((!addaptiveSequence && GamePad.GetState(PlayerIndex.One).Triggers.Left > 0 && !isReversing) || (isReversing && GamePad.GetState(PlayerIndex.One).Triggers.Right > 0))
            {

                veh.BrakeLightsOn = true;


                isAdditionalBrakeLightOn = true;
                if (isTruckMode)
                {
                    Function.Call((Hash)0x92B35082E0B42F66, truckTrailer, true);
                    //  truckTrailer.BrakeLightsOn = true;
                }

            }
        }

        private void UpdateSpeed()
        {
            speedInKmh = (int)(Math.Round(veh.Speed * 3.6));
            fakeSpeed = (int)Math.Round(speedInKmh / (1.28));
            if (fakeSpeed < (speedInKmh - 14))
            {
                fakeSpeed = speedInKmh - 14;
            }
        }

        public List<string> canHaveDirt = new List<string>()
        {
            "paji", "schafter", "315", "trafic", "toyota", "fork", "680", "850", "rogue", "fiesta"
        };
        private float dirtLevel = 0f;

        private void CleanCar()
        {
            var n = veh.DisplayName.ToLower();
            var except = canHaveDirt.Any(c => n.Contains(c));
            if (!isRaining && !except)
            {
                veh.DirtLevel = 0.0f;
            }

            if (except && fakeSpeed > 0)
            {
                dirtLevel += !isRaining ? 0.000002f : 0.0002f;
                veh.DirtLevel = dirtLevel;
            }
            if (fakeSpeed <= 0)
                dirtLevel = veh.DirtLevel;
        }

        private void BrakeLightsAfterStop()
        {

            Function.Call((Hash)0x92B35082E0B42F66, Game.Player.LastVehicle, true);
            if (isTruckMode)
            {
                truckTrailer.LightsOn = true;
                // Function.Call((Hash)0x92B35082E0B42F66, truckTrailer, true);
            }
        }

        private void ExitingVehicle()
        {
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleCinCam))
            {
                RenderAutobahnStopWatch = new Stopwatch();
                RenderAutobahnStopWatch.Start();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleCinCam) && RenderAutobahnStopWatch.ElapsedMilliseconds < 2000)
            {
                RenderAutobahnStopWatch = new Stopwatch();
            }
            if (RenderAutobahnStopWatch.ElapsedMilliseconds > 2000)
            {
                RenderAutobahnStopWatch = new Stopwatch();
                if (isOnAutobahn)
                {
                    isOnAutobahn = false;
                }
                else
                {
                    isOnAutobahn = true;
                }
            }

            if (Game.IsControlJustReleased(0, GTA.Control.VehicleExit) && vehicleExitingStopWatch.ElapsedMilliseconds >= 600)
            {
                vehicleExitingStopWatch.Stop();
                veh.EngineRunning = true;
                wasLightsOn = veh.LightsOn;
                centralLockCloseIfRemoteDidntStopWatch.Start();
                //  veh.LightsOn = false;
                // centralLockStopWatch.Start();
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleExit) && vehicleExitingStopWatch.ElapsedMilliseconds < 600)
            {
                vehicleExitingStopWatch = new Stopwatch();
                veh.EngineRunning = true;
            }
            if (Game.IsControlJustPressed(0, GTA.Control.VehicleExit))
            {
                HighBeam.bus.BusControls.canGetOutside = true;
                vehicleExitingStopWatch = new Stopwatch();
                vehicleExitingStopWatch.Start();
                isParkingMode = false;
                parkingSensorSoundLow.Stop();
                parkingSensorSoundMiddle.Stop();
                parkingSensorSoundHigh.Stop();
            }
        }
        private void CentralLockCloseIfRemoteDidnt()
        {
            if (centralLockCloseIfRemoteDidntStopWatch.ElapsedMilliseconds > 8000 && centralLockCloseIfRemoteDidntStopWatch.IsRunning)
            {
                centralLockCloseIfRemoteDidntStopWatch = new Stopwatch();
                centralLockProcedure = true;
                centralLockToOpen = false;
            }
        }

        private void CentralLockRemote()
        {
            if (!Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown)
                && centralLockRemoteStopWatch.ElapsedMilliseconds >= 480
                && !toggleTailgate)
            {
                centralLockProcedure = true;
                centralLockToOpen = !Function.Call<bool>((Hash)0xAE31E7DF9B5B132E, veh);
                centralLockRemoteStopWatch = new Stopwatch();
            }
            if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && !centralLockRemoteStopWatch.IsRunning && !toggleTailgate)
            {
                centralLockRemoteStopWatch.Start();
            }
            if (centralLockProcedure)
            {
                if (!centralLockStopWatch.IsRunning)
                {
                    veh.EngineRunning = true;
                    isCentralLockLocked = false;
                    if (!centralLockToOpen)
                    {
                        centralLockCloseIfRemoteDidntStopWatch = new Stopwatch();
                    }
                    veh.LeftIndicatorLightOn = true;
                    veh.RightIndicatorLightOn = true;
                    isLeftIndicatorOn = true;
                    isRightIndicatorOn = true;
                    veh.InteriorLightOn = true;
                    centralLockStopWatch.Start();
                }
                if (centralLockStopWatch.IsRunning && centralLockStopWatch.ElapsedMilliseconds >= 1800)
                {
                    if (!hazardsActive)
                    {
                        veh.LeftIndicatorLightOn = false;
                        veh.RightIndicatorLightOn = false;
                        isLeftIndicatorOn = false;
                        isRightIndicatorOn = false;
                    }
                    if (!centralLockToOpen)
                    {
                        veh.EngineRunning = false;
                        veh.InteriorLightOn = false;
                        isCentralLockLocked = true;
                        isEngineRunning = false;
                        veh.RoofState = VehicleRoofState.Opening;

                    }
                    centralLockProcedure = false;
                    centralLockStopWatch = new Stopwatch();
                    if (hazardsActive)
                    {
                        veh.EngineRunning = true;
                        veh.LightsOn = false;
                    }
                }
            }
        }
        private void CentralLock()
        {
            if (centralLockStopWatch.ElapsedMilliseconds > 4000)
            {
                centralLockStopWatch = new Stopwatch();
                veh.LeftIndicatorLightOn = true;
                veh.RightIndicatorLightOn = true;
                isLeftIndicatorOn = true;
                isRightIndicatorOn = true;
                centralLockHazardsStopWatch.Start();
            }
            if (centralLockHazardsStopWatch.ElapsedMilliseconds > 1800)
            {
                centralLockHazardsStopWatch = new Stopwatch();
                veh.LeftIndicatorLightOn = false;
                veh.RightIndicatorLightOn = false;
                isLeftIndicatorOn = false;
                isRightIndicatorOn = false;
                veh.LightsOn = wasLightsOn;
                veh.EngineRunning = false;
            }
        }
        private void CruiseControl()
        {

            if (addaptiveDisengagementStopWatch.IsRunning && addaptiveDisengagementStopWatch.ElapsedMilliseconds > 2100)
            {
                addaptiveDisengagementStopWatch = new Stopwatch();
            }
            if (addaptiveDisengagementStopWatch.IsRunning)
            {
                if (addaptiveDisengagementStopWatch.ElapsedMilliseconds % 400 < 200 && addaptiveDisengagementStopWatch.ElapsedMilliseconds < 1600)
                {
                    GamePad.SetVibration(PlayerIndex.One, 0.7f, 0.7f);
                }
                else
                {
                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                }
            }
            if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadLeft) && fakeSpeed < 220)
            {
                isCruiseControlActive = true;
                cruiseControlSpeed = fakeSpeed < 50 ? 50 : fakeSpeed;
            }
            if (isCruiseControlActive)
            {
                Game.DisableControlThisFrame(0, GTA.Control.Phone);
                Game.DisableControlThisFrame(0, GTA.Control.ScriptPadDown);
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == XInputDotNetPure.ButtonState.Pressed && canPressCruiseButton && cruiseControlSpeed < 220)
                {
                    canPressCruiseButton = false;
                    cruiseControlButtonsStopwatch.Start();
                    cruiseControlSpeed = (fakeSpeed % 5 == 0 ? cruiseControlSpeed : (RoundUp((int)(cruiseControlSpeed)))) + 5;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == XInputDotNetPure.ButtonState.Pressed && canPressCruiseButton && cruiseControlSpeed > 20)
                {
                    canPressCruiseButton = false;
                    cruiseControlButtonsStopwatch.Start();
                    cruiseControlSpeed = (fakeSpeed % 5 == 0 ? cruiseControlSpeed : (RoundDown((int)(cruiseControlSpeed)))) - 5;
                }
                if (cruiseControlButtonsStopwatch.ElapsedMilliseconds > 100)
                {
                    cruiseControlButtonsStopwatch = new Stopwatch();
                    canPressCruiseButton = true;
                }
                var wasCarAhead = addaptiveCarAhead;
                if (!addaptiveDisengagementStopWatch.IsRunning)
                {
                    AdaptiveCruiseControl();
                }
                if (wasCarAhead != null && addaptiveCarAhead == null)
                {
                    addaptiveDisengagementStopWatch.Start();
                }
                if (addaptiveCarAhead != null && !isEmergencyBraking)
                {
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 1f)
                    {
                        addaptiveDisengagementStopWatch.Start();
                        isCruiseControlActive = false;
                    }
                    AdaptiveCruiseControlSteering();
                    var frontModelSize = addaptiveCarAhead.Model.GetDimensions();
                    frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, frontModelSize.Z / 2);
                    var addaptiveCenterPoint = GetEntityOffset(addaptiveCarAhead, new Vector3(0, -frontModelSize.Y, -frontModelSize.Z * 0));
                    addaptiveRearPos = addaptiveCenterPoint;

                    var currentModel = veh.Model.GetDimensions();
                    currentModel = new Vector3(currentModel.X / 2, currentModel.Y / 2, currentModel.Z / 2);
                    var ownCenter = GetEntityOffset(veh, new Vector3(0, currentModel.Y, -currentModel.Z * 0));
                    addaptiveOwnFrontPos = ownCenter;
                    bool isCarVeryClose = addaptiveOwnFrontPos.DistanceTo(addaptiveCenterPoint) <= (fakeSpeed < 70 ? 4f : 8f);
                    if (addaptiveCarAheadSpeed >= 0 && addaptiveCarAhead.Heading - veh.Heading < 10f && addaptiveCarAhead.Heading - veh.Heading > -10f)
                    {
                        veh.HandbrakeOn = false;
                        var divide = 75;
                        var distToAhead = (int)addaptiveOwnFrontPos.DistanceTo(addaptiveCenterPoint);
                        divide = 30 + distToAhead;
                        if (distToAhead < 12)
                            divide = 8 + distToAhead;

                        var forceDeccel = -(((veh.Speed - addaptiveCarAheadSpeed) / divide));
                        if (veh.Position.DistanceTo(addaptiveCarAhead.Position) <= 25 && ((veh.Speed * 3.6) - (addaptiveCarAhead.Speed * 3.6)) > 40 && fakeSpeed < 60)
                            isEmergencyBraking = true;
                        var diff = veh.Speed - addaptiveCarAheadSpeed;
                        var forceAccel = 0.1f + -(diff / 100);
                        var isStartFrom0 = false;
                        if (fakeSpeed < 5 && addaptiveCarAheadSpeed * 3.6 > 4)
                        {
                            forceAccel = 0.25f;
                            isStartFrom0 = true;
                        }
                        if (fakeSpeed <= 8 && addaptiveCarAheadSpeed * 3.6 < 10 && !isCarVeryClose)
                        {
                            forceAccel = 0.025f;
                            if (fakeSpeed < 3)
                            {
                                forceAccel = 0.25f;
                            }
                            diff = 0;
                        }
                        if (isCarVeryClose && fakeSpeed > 2)
                        {
                            diff = 2f;
                            forceDeccel = -0.12f;
                            forceAccel = 0;
                        }
                        if ((diff > 2) || (!isStartFrom0 && fakeSpeed < 5))
                        {
                            isBrakeLightsOn = true;
                            veh.BrakeLightsOn = true;
                            isAdditionalBrakeLightOn = true;
                        }
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 0.05)
                        {
                            forceAccel = 0;
                        }
                        if (addaptiveCarAheadSpeed > 0 || fakeSpeed > 0)
                            veh.ApplyForceRelative(new Vector3(0, diff > 0 ? forceDeccel : forceAccel, 0));
                        if (fakeSpeed <= 0 && !isStartFrom0)
                            veh.HandbrakeOn = true;
                    }
                }
                else
                {
                    double currentSpeed = fakeSpeed;
                    double cruiseSpeed = cruiseControlSpeed + 0.4;
                    var pitch = Math.Round(veh.ForwardVector.Z * 100, 0);

                    if (currentSpeed < cruiseSpeed)
                    {
                        var force = 0.036f + (fakeSpeed > 100 ? ((float)(fakeSpeed - 40) / 10000) : 0f) + (pitch > 0 ? (float)(((pitch > 7 ? 7 : pitch) / 100)) : 0f);
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 0.05)
                        {
                            force = 0;
                        }
                        veh.ApplyForceRelative(new Vector3(0.0f, force, 0));
                    }
                    if (fakeSpeed >= cruiseControlSpeed && cruiseControlSpeed - fakeSpeed < 2f && cruiseControlSpeed - fakeSpeed > -2f)
                    {
                        fakeSpeed = (int)cruiseControlSpeed;
                    }
                }
                if (isEmergencyBraking && fakeSpeed > 0)
                {
                    isBrakeLightsOn = true;
                    veh.BrakeLightsOn = true;
                    var force = 0.33f;
                    veh.ApplyForceRelative(new Vector3(0.0f, -force, 0));
                    if (speedInKmh <= 5)
                        isEmergencyBraking = false;
                }
            }
            else
            {
                addaptiveCarAhead = null;
            }
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 0.05 && isCruiseControlActive)
            {
                isCruiseControlActive = false;
                addaptiveDisengagementStopWatch.Start();
            }
        }
        float lastAddaptiveDiff = 0;

        private void AdaptiveCruiseControlSteering()
        {
            //  DrawEntityBox(veh, System.Drawing.Color.Red);
            //  DrawEntityBox(addaptiveCarAhead, System.Drawing.Color.Yellow);


            var frontModelSize = addaptiveCarAhead.Model.GetDimensions();
            frontModelSize = new Vector3(frontModelSize.X / 2, frontModelSize.Y / 2, frontModelSize.Z / 2);
            var currentModelSize = veh.Model.GetDimensions();
            currentModelSize = new Vector3(currentModelSize.X / 2, currentModelSize.Y / 2, currentModelSize.Z / 2);

            var color = System.Drawing.Color.Yellow;

            var frontLeftPoint = GetEntityOffset(addaptiveCarAhead, new Vector3(-frontModelSize.X, -frontModelSize.Y, -frontModelSize.Z * 0));
            var frontRightPoint = GetEntityOffset(addaptiveCarAhead, new Vector3(frontModelSize.X, -frontModelSize.Y, -frontModelSize.Z * 0));
            var frontCenterPoint = GetEntityOffset(addaptiveCarAhead, new Vector3(0, -frontModelSize.Y, -frontModelSize.Z * 0));

            var currentLeftPoint = GetEntityOffset(veh, new Vector3(-currentModelSize.X, currentModelSize.Y, -currentModelSize.Z * 0));
            var currentRightPoint = GetEntityOffset(veh, new Vector3(currentModelSize.X, currentModelSize.Y, currentModelSize.Z * 0));
            var leftDist = frontCenterPoint.DistanceTo(currentLeftPoint);
            var rightDist = frontCenterPoint.DistanceTo(currentRightPoint);
            var diff = leftDist - rightDist;
            var steerToLeft = leftDist > rightDist;
            var divid = 30f;
            steeringAngle = -(diff / divid);
            lastAddaptiveDiff = diff;

            if (false)
            {
                DrawLine(currentLeftPoint, frontCenterPoint, color);
                DrawLine(currentRightPoint, frontCenterPoint, color);
                DrawLine(frontCenterPoint, new Vector3(frontCenterPoint.X, frontCenterPoint.Y, frontCenterPoint.Z + 6f), System.Drawing.Color.Red);
                UI.ShowSubtitle($"diff {steerToLeft} ste {steeringAngle}");
            }
        }

        int RoundUp(int toRound)
        {
            if (toRound % 5 == 0) return toRound;
            return (5 - toRound % 5) + toRound;
        }
        int RoundDown(int toRound)
        {
            return toRound - toRound % 5;
        }

        private void DrawEntityBox(Entity ent, System.Drawing.Color color)
        {
            var pos = ent.Position;
            Vector3 min, max;
            ent.Model.GetDimensions(out min, out max);


            //World.DrawMarker(MarkerType.DebugSphere, ent.GetOffsetInWorldCoords(min), new Vector3(), new Vector3(), new Vector3(0.3f, 0.3f, 0.3f), Color.Red);
            //World.DrawMarker(MarkerType.DebugSphere, ent.GetOffsetInWorldCoords(max), new Vector3(), new Vector3(), new Vector3(0.3f, 0.3f, 0.3f), Color.Red);

            var modelSize = ent.Model.GetDimensions();
            modelSize = new Vector3(modelSize.X / 2, modelSize.Y / 2, modelSize.Z / 2);

            var b1 = GetEntityOffset(ent, new Vector3(-modelSize.X, -modelSize.Y, -modelSize.Z * 0));
            var b2 = GetEntityOffset(ent, new Vector3(-modelSize.X, modelSize.Y, -modelSize.Z * 0));
            var b3 = GetEntityOffset(ent, new Vector3(modelSize.X, -modelSize.Y, -modelSize.Z * 0));
            var b4 = GetEntityOffset(ent, new Vector3(modelSize.X, modelSize.Y, -modelSize.Z * 0));

            var a1 = GetEntityOffset(ent, new Vector3(-modelSize.X, -modelSize.Y, modelSize.Z));
            var a2 = GetEntityOffset(ent, new Vector3(-modelSize.X, modelSize.Y, modelSize.Z));
            var a3 = GetEntityOffset(ent, new Vector3(modelSize.X, -modelSize.Y, modelSize.Z));
            var a4 = GetEntityOffset(ent, new Vector3(modelSize.X, modelSize.Y, modelSize.Z));

            DrawLine(a1, a2, color);
            DrawLine(a2, a4, color);
            DrawLine(a4, a3, color);
            DrawLine(a3, a1, color);

            DrawLine(b1, b2, color);
            DrawLine(b2, b4, color);
            DrawLine(b4, b3, color);
            DrawLine(b3, b1, color);

            DrawLine(a1, b1, color);
            DrawLine(a2, b2, color);
            DrawLine(a3, b3, color);
            DrawLine(a4, b4, color);
        }

        private void DrawLine(Vector3 start, Vector3 end, System.Drawing.Color color)
        {
            Function.Call(Hash.DRAW_LINE, start.X, start.Y, start.Z, end.X, end.Y, end.Z, color.R, color.G, color.B, color.A);
        }

        private Vector3 GetEntityOffset(Entity ent, Vector3 offset)
        {
            return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, ent.Handle, offset.X, offset.Y, offset.Z);
        }

        private void AdaptiveCruiseControl()
        {
            if (addaptiveCruiseControlStopWatch.ElapsedMilliseconds > 700)
            {
                var met = 30;
                var sensorsCount = 6;
                if (fakeSpeed < 70)
                {
                    sensorsCount = 5;
                    met = 25;
                }
                addaptiveCarAhead = null;
                addaptiveCarAheadSpeed = 0;
                for (var i = 0; i < sensorsCount; ++i)
                {
                    var sensor = veh.GetOffsetInWorldCoords(new Vector3(0, met, 0));
                    var carA = World.GetClosestVehicle(sensor, 2.7f);
                    if (carA != null)
                    {
                        addaptiveCarAhead = carA;
                        addaptiveCarAheadSpeed = carA.Speed;
                        addaptiveSensorActive = i;
                    }
                    met -= met <= 5 ? 2 : 5;
                }
                addaptiveCruiseControlStopWatch = new Stopwatch();
            }
            if (isOnAutobahn)
            {
                addaptiveCarAhead = null;
                addaptiveCarAheadSpeed = 0;
            }
            if (!addaptiveCruiseControlStopWatch.IsRunning)
                addaptiveCruiseControlStopWatch.Start();
        }

        private void RainSpray()
        {
            /* if (Game.IsControlJustPressed(0, GTA.Control.VehicleSelectNextWeapon))
             {
                 if(puddle != null)
                 {
                     puddle.Delete();
                 }
                 puddle = World.CreateProp(new Model(1566353027), veh.GetOffsetInWorldCoords(new Vector3(1.9f, 4f, -2.93f)), true, false);
                 puddle.Heading = veh.Heading;
                 puddle.Alpha = 0;
             }*/


            if (veh.IsOnAllWheels && isRaining && (isTruckMode ? (speedInKmh > 40) : (speedInKmh > 40)) && veh.ClassType != VehicleClass.Planes)
            {
                var ray = World.Raycast(veh.GetOffsetInWorldCoords(new Vector3(0, 0, 3)), veh.GetOffsetInWorldCoords(new Vector3(0, 0, 20)), IntersectOptions.Everything, veh);

                bool hasCelingAbove = ray.DitHitAnything;
                if (!hasCelingAbove)
                {
                    var lib = "core";
                    //water_splash_vehicle
                    var asset = "water_splash_vehicle";
                    if (Function.Call<bool>(GTA.Native.Hash.HAS_PTFX_ASSET_LOADED, lib))
                    {
                        Function.Call<bool>(GTA.Native.Hash.REQUEST_PTFX_ASSET, lib);
                    }
                    else
                    {
                        decimal frontSprayFactor = Decimal.Divide((decimal)(fakeSpeed > 120 ? 120 : fakeSpeed), 67);
                        decimal rearSprayFactor = Decimal.Divide((decimal)((fakeSpeed > 115 ? 115 : fakeSpeed) * 2), 120);

                        decimal rearAdditionalSprayFactor = Decimal.Divide((decimal)((fakeSpeed > 94 ? 94 : fakeSpeed) * 2), 120);

                        var sprayZCompensate = -0.3f;
                        sprayZCompensate += (float)Decimal.Divide((decimal)(fakeSpeed), 140);

                        if (sprayZCompensate >= 0.6f)
                        {
                            sprayZCompensate = 0.6f;
                        }

                        float alpha = 0.1f;
                        if (fakeSpeed < 100)
                        {
                            alpha = 0.1f;
                        }
                        var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);

                        if (isTruckMode || veh.DisplayName.ToLower().Contains("caddy"))
                            alpha = 0.01f;
                        if (h < 5 || h > 21)
                            alpha += 0.15f;


                        var currentModelSize = veh.Model.GetDimensions();
                        currentModelSize = new Vector3(currentModelSize.X / 2, currentModelSize.Y / 2, currentModelSize.Z / 2);


                        //  var frontCenterPoint = GetEntityOffset(addaptiveCarAhead, new Vector3(0, -frontModelSize.Y, -frontModelSize.Z * 0));

                        var boneFL = veh.GetBoneCoord("wheel_lf");
                        var boneFR = veh.GetBoneCoord("wheel_rf");
                        var boneRL = veh.GetBoneCoord("wheel_lr");
                        var boneRR = veh.GetBoneCoord("wheel_rr");

                        var FL = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneFL.X, boneFL.Y, boneFL.Z);
                        var FR = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneFR.X, boneFR.Y, boneFR.Z);
                        var RL = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneRL.X, boneRL.Y, boneRL.Z);
                        var RR = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, veh, boneRR.X, boneRR.Y, boneRR.Z);

                        var frontL = veh.GetOffsetInWorldCoords(new Vector3(FL.X, FL.Y + 1.7f, FL.Z));
                        var frontR = veh.GetOffsetInWorldCoords(new Vector3(FR.X, FR.Y + 1.7f, FR.Z));
                        var rearL = veh.GetOffsetInWorldCoords(new Vector3(RL.X, RL.Y + 2f, RL.Z + sprayZCompensate));
                        var rearR = veh.GetOffsetInWorldCoords(new Vector3(RR.X, RR.Y + 2f, RR.Z + sprayZCompensate));

                        //FRONT
                        // left
                        Function.Call((Hash)0x6C38AF3693A69A91, lib);
                        Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, frontL.X, frontL.Y, frontL.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)frontSprayFactor, 0, 0, 0);
                        Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha);
                        // right
                        Function.Call((Hash)0x6C38AF3693A69A91, lib);
                        Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, frontR.X, frontR.Y, frontR.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)frontSprayFactor, 0, 0, 0);
                        Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha);

                        // REAR
                        //left 
                        // Function.Call((Hash)0x6C38AF3693A69A91, lib);
                        //  Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, rearL.X, rearL.Y, rearL.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)rearSprayFactor, 0, 0, 0);
                        // Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha + 0.12f);
                        // right
                        // Function.Call((Hash)0x6C38AF3693A69A91, lib);
                        // Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, rearR.X, rearR.Y, rearR.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)rearSprayFactor, 0, 0, 0);
                        // Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha + 0.12f);

                        // UI.ShowSubtitle(sprayZCompensate.ToString() + " " + rearSprayFactor);

                        sprayZCompensate = sprayZCompensate >= 0.4f ? 0.4f : sprayZCompensate;

                        var rearY = 2f;
                        //  if (fakeSpeed > 110)
                        // {
                        rearY += (fakeSpeed / 60f);
                        //   }


                        var rearLAdd = veh.GetOffsetInWorldCoords(new Vector3(RL.X, RL.Y + rearY, (RL.Z + sprayZCompensate)));
                        var rearRAdd = veh.GetOffsetInWorldCoords(new Vector3(RR.X, RR.Y + rearY, (RR.Z + sprayZCompensate)));

                        var leng = (int)(fakeSpeed / 36);
                        if (leng > 4)
                            leng = 4;

                        for (var i = 0; i < leng; ++i)
                        {
                            // REAR
                            //left
                            Function.Call((Hash)0x6C38AF3693A69A91, lib);
                            Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, rearLAdd.X, rearLAdd.Y, rearLAdd.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)rearAdditionalSprayFactor, 0, 0, 0);
                            Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha + 0.12f);
                            // right
                            Function.Call((Hash)0x6C38AF3693A69A91, lib);
                            Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, rearRAdd.X, rearRAdd.Y, rearRAdd.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)rearAdditionalSprayFactor, 0, 0, 0);
                            Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha + 0.12f);

                        }






                        if (truckTrailer != null)
                        {
                            var tboneRL = truckTrailer.GetBoneCoord("wheel_lr");
                            var tboneRR = truckTrailer.GetBoneCoord("wheel_rr");
                            var tRL = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, truckTrailer, tboneRL.X, tboneRL.Y, tboneRL.Z);
                            var tRR = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, truckTrailer, tboneRR.X, tboneRR.Y, tboneRR.Z);
                            var trearL = truckTrailer.GetOffsetInWorldCoords(new Vector3(tRL.X, tRL.Y + 2f, tRL.Z + sprayZCompensate));
                            var trearR = truckTrailer.GetOffsetInWorldCoords(new Vector3(tRR.X, tRR.Y + 2f, tRR.Z + sprayZCompensate));
                            //left
                            Function.Call((Hash)0x6C38AF3693A69A91, lib);
                            Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, trearL.X, trearL.Y, trearL.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)rearSprayFactor, 0, 0, 0);
                            Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha + 0.12f);
                            // right
                            Function.Call((Hash)0x6C38AF3693A69A91, lib);
                            Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, trearR.X, trearR.Y, trearR.Z, veh.Rotation.X, veh.Rotation.Y, veh.Rotation.Z, (float)rearSprayFactor, 0, 0, 0);
                            Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha + 0.12f);
                        }
                    }
                }

            }
        }


        private static float waterAmountUnderneathTire = 0f;
        private static float waterDisplacedPerSecDefault = 30f;
        private static float waterGoingUnderneathTirePerSecDefault = 2f;
        private static Stopwatch aquaplaningCalculationStopwatch = new Stopwatch();

        private static bool isAqua = false;
        private static Stopwatch aquaplaningDurationStopWatch = new Stopwatch();

        private static Stopwatch highPitchStopwatch = new Stopwatch();

        private static Prop aquaProp = null;
        private static Stopwatch aquaPropRemoveStopwatch = new Stopwatch();

        private void NewAquaplaning()
        {
            /* if (Game.IsControlJustPressed(0, GTA.Control.VehicleSelectNextWeapon) && aquaProp == null)
             {
                 aquaPropRemoveStopwatch.Start();
                 aquaProp = World.CreateProp(new Model(1859431100), veh.GetOffsetInWorldCoords(new Vector3(0.6f, 5f, -3.8f)), false, false);
                 aquaProp.Rotation = new Vector3(0f, 90f, 0f);
             }
             if (aquaPropRemoveStopwatch.ElapsedMilliseconds > 2000)
             {
                 aquaPropRemoveStopwatch = new Stopwatch();
                 aquaProp.Delete();
                 aquaProp = null;
             }*/

            if (descendStopwatch.IsRunning && descendStopwatch.ElapsedMilliseconds > 500)
            {
                lastPitch = (int)Math.Round(veh.ForwardVector.Z * 100, 0);
                descendStopwatch = new Stopwatch();
            }
            if (!descendStopwatch.IsRunning)
            {
                descendStopwatch.Start();
            }
            int pitchDiff = lastPitch - (int)Math.Round(veh.ForwardVector.Z * 100, 0);

            if (fakeSpeed > 100 && isRaining && !isWRC)
            {
                if (!isAqua)
                {
                    var hardSteer = false;
                    if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.9f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.9f) && !hardSteerAtRain.IsRunning)
                    {
                        hardSteerAtRain.Start();
                    }
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.9f && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > -0.9f)
                    {
                        hardSteerAtRain = new Stopwatch();
                    }
                    if (hardSteerAtRain.ElapsedMilliseconds > 1170)
                    {
                        hardSteer = true;
                    }

                    if (hardSteer)
                    {
                        waterAmountUnderneathTire += 0.35f;
                    }



                    if (aquaplaningCalculationStopwatch.ElapsedMilliseconds >= 1000)
                    {
                        var pitch = -Math.Abs((int)(Math.Round(veh.ForwardVector.Z * 100, 0)));
                        if (pitch < -3)
                        {
                            pitch = -3;
                        }
                        var speedDivided = (fakeSpeed / 13);
                        var speedDivided2 = (fakeSpeed / 10);

                        waterAmountUnderneathTire += (waterGoingUnderneathTirePerSecDefault + speedDivided) - pitch;
                        waterAmountUnderneathTire -= (waterDisplacedPerSecDefault - speedDivided2) - pitch;

                        if (waterAmountUnderneathTire < 0)
                        {
                            waterAmountUnderneathTire = 0;
                        }
                        aquaplaningCalculationStopwatch = new Stopwatch();
                    }
                    if (!isAqua)
                    {
                        if (!highPitchStopwatch.IsRunning)
                        {
                            var isHighPitchDiff = pitchDiff < -3;
                            if (isHighPitchDiff)
                            {
                                waterAmountUnderneathTire += -(pitchDiff) * 14;
                                highPitchStopwatch.Start();
                            }
                        }

                        isAqua = waterAmountUnderneathTire > 140f;
                        if (isAqua)
                        {
                            aquaplaningDurationStopWatch.Start();
                        }
                    }
                    if (highPitchStopwatch.ElapsedMilliseconds > 600)
                    {
                        highPitchStopwatch = new Stopwatch();
                    }
                }
            }
            else if (fakeSpeed < 100 && isAqua)
            {
                waterAmountUnderneathTire = 0f;
                if (aquaplaningDurationStopWatch.IsRunning)
                {
                    aquaplaningDurationStopWatch = new Stopwatch();
                    isAqua = false;
                    veh.HandbrakeOn = false;

                }
            }

            if (fakeSpeed < 100)
            {
                waterAmountUnderneathTire = 0f;
            }
            // UI.ShowSubtitle(waterAmountUnderneathTire + " isAq: " + isAqua);

            if (!aquaplaningCalculationStopwatch.IsRunning)
            {
                aquaplaningCalculationStopwatch.Start();
            }

            if (aquaplaningDurationStopWatch.ElapsedMilliseconds > 2700 && false)
            {
                aquaplaningDurationStopWatch = new Stopwatch();
                isAqua = false;
                veh.HandbrakeOn = false;
                veh.FixTire(5);
                veh.FixTire(7);
            }

            if (isAqua)
            {
                //if (!veh.IsTireBurst(5))
                //  {
                //    veh.BurstTire(7);
                //    veh.BurstTire(5);
                //  }
                steeringAngle = steeringAngle > 0 ? 1f : -1f;
                veh.HandbrakeOn = true;
            }
        }

        public static bool ispar = false;
        public static Random speedAquaRnd = new Random();
        private void Wind()
        {
            NewAquaplaning();
            int pitchDiff = 0;



            /*  var lib = "core";
              var asset = "water_splash_vehicle"; 
              if (Function.Call<bool>(GTA.Native.Hash.HAS_PTFX_ASSET_LOADED, lib))
              {
                  Function.Call<bool>(GTA.Native.Hash.REQUEST_PTFX_ASSET, lib);
                  UI.ShowSubtitle("loading particle");
              }
              else if(!ispar)
              {   
                //  ispar = true;
                  UI.ShowSubtitle("showing particle");  
                  Function.Call((Hash)0x6C38AF3693A69A91, lib); 
                  var offset = veh.GetOffsetInWorldCoords(new Vector3(0.4f, 2, -0.7f));
                  var offset2 = veh.GetOffsetInWorldCoords(new Vector3(3f, 2, -0.7f));
                  var offset3 = veh.GetOffsetInWorldCoords(new Vector3(-3f, 2, -0.7f));
                //  Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, offset.X, offset.Y, offset.Z, 0, 0, 0, 0.8f, 0, 0, 0);
                      var id = Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, veh, 0f, 0f, 0.0f, 0f, 0f, 0f, 2.0f, 0, 0, 0); 
                  //  Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, offset2.X, offset2.Y, offset2.Z, 0, 0, 0, 0.8f, 0, 0, 0);
                  //  Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, offset3.X, offset3.Y, offset3.Z, 0, 0, 0, 0.8f, 0, 0, 0);
                  // var id = Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, veh, 0, 0f, 0f, 0f, 0, 0f, 0.7f, 0, 0, 0);
                  // Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, veh.Position.X, veh.Position.Y, veh.Position.Z, 0, 0, 0, 0.8f, 0, 0, 0);
                  // Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, veh, 0.5f, 0f, -0.4f, 0, 0, 0f, 0.7f, 0, 0, 0);
                  //   Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, veh, 0f, 0f, -0.4f, 0, 0, 0f, 0.7f, 0, 0, 0); 
                  //  Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, veh, 0, 1f, 0f, -0.1f, 0, 0f, 0.7f, 0, 0, 0);
                  // Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, veh, 0, -1f, 0f, -0.1f, 0, 0f, 0.7f, 0, 0, 0);
                   Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_COLOUR, id, 255f, 255f, 255f, 0f);  
                  //  Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, id, 0.2f);
                  //Function.Call(Hash.SET_PARTICLE_FX_LOOPED_COLOUR, asset, veh, 0, 0, 0, 0, 0, 0, 3.5, 0, 0, 0);
                //  Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, asset, veh.Position.X, veh.Position.Y, veh.Position.Z, 0, 0, 0, 1f, 0, 0, 0);

              }
              if (Game.IsControlJustReleased(0, GTA.Control.VehicleSelectNextWeapon))
              {

              }*/



            //  UI.ShowSubtitle(Game.FPS.ToString());
            //GRAPHICS::START_PARTICLE_FX_LOOPED_ON_ENTITY("ent_amb_car_wash", a_0._f13, 0.0, 0.0, 0.0, l_35, 0x3f800000, 0, 0, 0)
            if (veh.ClassType != VehicleClass.Planes)
            {
                if (speedInKmh > 55 && !isAudioSet)
                {
                    isAudioSet = true;
                    if (isRaining)
                    {
                        // Function.Call((Hash)0xE65F427EB70AB1ED, 65, "SPRAY_CAR", veh, "CARWASH_SOUNDS", 0, 0);
                    }
                    Function.Call((Hash)0xE65F427EB70AB1ED, 67, "PLAYER_AT_SPEED_FREEFALL_MASTER", veh, "", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 68, "PLAYER_AT_SPEED_FREEFALL_MASTER", veh, "", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 69, "PLAYER_AT_SPEED_FREEFALL_MASTER", veh, "", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 70, "PLAYER_AT_SPEED_FREEFALL_MASTER", veh, "", 0, 0);
                    // Audio.PlaySoundFromEntity(veh, "");
                    // Function.Call((Hash)0xE65F427EB70AB1ED, 67, , veh, 0, 0);
                }
                if ((speedInKmh < 55 && isAudioSet) || willChangePhevSound)
                {
                    isAudioSet = false;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 65, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 70, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 67, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 68, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 69, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                }
                if (speedInKmh > 1 && speedInKmh < 55 && !isAudioSet2)
                {
                    //  isAudioSet2 = true;
                    //  Function.Call((Hash)0xE65F427EB70AB1ED, 76, "Seagulls", veh, "JEWEL_HEIST_SOUNDS", 0, 0);
                }
                if (speedInKmh <= 1 && isAudioSet2)
                {
                    // isAudioSet2 = false;
                    //  Function.Call((Hash)0xE65F427EB70AB1ED, 76, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                }
                if (speedInKmh > 110 && !willChangePhevSound)
                {
                    isAudioSet2 = true;
                    int min = 900;
                    if (speedInKmh > 110)
                    {
                        min = 700;
                    }
                    if (speedInKmh > 130)
                    {
                        min = 650;
                    }
                    if (speedInKmh > 150)
                    {
                        min = 500;
                    }
                    if (speedInKmh > 170)
                    {
                        min = 300;
                    }
                    if (speedInKmh > 190)
                    {
                        min = 250;
                    }
                    if (speedInKmh > 205)
                    {
                        min = 100;
                    }
                    if (speedInKmh > 225)
                    {
                        min = 30;
                    }
                    if (speedInKmh > 240)
                    {
                        min = 10;
                    }
                    if (min < 600 && veh.MaxBraking >= 0.5)
                        min = 600;
                    if (windSoundStopWatch.IsRunning && windSoundStopWatch.ElapsedMilliseconds > min)
                    {

                        if (speedInKmh > 196 && veh.MaxBraking < 0.5)
                        {
                            Function.Call((Hash)0xE65F427EB70AB1ED, 57, "Helicopter_Wind", veh, "BASEJUMPS_SOUNDS", 0, 0);
                        }
                        if (speedInKmh > 210 && veh.MaxBraking < 0.5)
                        {
                            Function.Call((Hash)0xE65F427EB70AB1ED, 58, "Helicopter_Wind", veh, "BASEJUMPS_SOUNDS", 0, 0);
                        }
                        Function.Call((Hash)0xE65F427EB70AB1ED, 56, "Helicopter_Wind", veh, "BASEJUMPS_SOUNDS", 0, 0);
                        windSoundStopWatch = new Stopwatch();
                    }
                    if (!windSoundStopWatch.IsRunning)
                    {
                        windSoundStopWatch = new Stopwatch();
                        windSoundStopWatch.Start();
                    }
                }
                else
                {
                    isAudioSet2 = false;
                    Function.Call((Hash)0xE65F427EB70AB1ED, 56, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 57, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                    Function.Call((Hash)0xE65F427EB70AB1ED, 58, "Prop_Drop_Water", veh, "DLC_Dmod_Prop_Editor_Sounds", 0, 0);
                }
            }

            if (isRaining && veh.ClassType != VehicleClass.Planes && false)
            {
                var hardSteer = false;
                if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.9f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.9f) && !hardSteerAtRain.IsRunning)
                {
                    hardSteerAtRain.Start();
                }
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.9f && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > -0.9f)
                {
                    hardSteerAtRain = new Stopwatch();
                }
                if (hardSteerAtRain.ElapsedMilliseconds > 1170)
                {
                    hardSteer = true;
                }
                var speedToAqua = speedAquaRnd.Next(186, 99999);
                var isHighPitchDiff = pitchDiff < (speedInKmh > 110 ? -2 : -4);
                if (isHighPitchDiff && speedInKmh > 110 && !isAquaplaningOn || ((hardSteer && speedInKmh > 140)) || (speedInKmh > speedToAqua))
                {
                    isEasyAqua = (isEasyAquaRnd.Next(0, 12) % 2 == 0);
                    if (((hardSteer && speedInKmh > 140)))
                    {
                        isEasyAqua = false;
                    }
                    if (speedInKmh < 130)
                        isEasyAqua = true;
                    isAquaplaningOn = true;
                    AquaplaningStopWatch.Start();
                    aquaplaningDuration = (int)((((int)Math.Round(veh.Speed * 3.6, 0)) * 11) / 1.2);
                    if (speedInKmh < 147)
                        aquaplaningDuration -= 400;
                    isAquaplaningDirRight = steeringAngle > 0 ? true : false;
                    aquaplaningFirstPhaseStopwatch.Start();
                    if (speedInKmh < 130 && pitchDiff < -5)
                    {
                        isEasyAqua = false;
                    }
                    if (!veh.IsTireBurst(0) && !isEasyAqua)
                    {
                        for (var i = 0; i < 14; ++i)
                        {
                            veh.BurstTire(i);
                        }
                    }
                }
                if (AquaplaningStopWatch.IsRunning && AquaplaningStopWatch.ElapsedMilliseconds > aquaplaningDuration)
                {
                    AquaplaningStopWatch = new Stopwatch();
                    XInputDotNetPure.GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                    isAquaplaningOn = false;
                    veh.HandbrakeOn = false;
                    isEasyAqua = false;
                    aquaplaningFirstPhaseStopwatch = new Stopwatch();
                    for (var i = 0; i < 14; ++i)
                    {
                        veh.FixTire(i);
                    }

                }
                if (aquaplaningFirstPhaseStopwatch.ElapsedMilliseconds > 600 && !isEasyAqua)
                {
                    veh.HandbrakeOn = true;
                }
                if (isAquaplaningOn)
                {
                    XInputDotNetPure.GamePad.SetVibration(PlayerIndex.One, 0.8f, 0.8f);
                    //  Game.DisableControlThisFrame(0, GTA.Control.VehicleAccelerate);
                }
                else
                {
                    Function.Call((Hash)0x222FF6A823D122E2, Game.Player.LastVehicle, false);
                }
                // XInputDotNetPure.GamePad.SetVibration(PlayerIndex.One, 0.3f, 0.3f);
                // Game.DisableControlThisFrame(0, GTA.Control.VehicleMoveLeftRight);
                // Game.DisableControlThisFrame(0, GTA.Control.VehicleAccelerate);
                // AudioId = 98;
                //   Function.Call((Hash)0xE65F427EB70AB1ED, AudioId, "SPRAY", veh, "CARWASH_SOUNDS", 0, 0);
            }
            if ((veh.Speed * 3.6) > 118)
            {
                if (!windStopWatch.IsRunning)
                    windStopWatch.Start();
                var windTicksMax = ((int)Math.Round(veh.Speed * 3.6, 0) * 2) * 10;
                if (windStopWatch.IsRunning && windStopWatch.ElapsedMilliseconds >= timeBeforeNextWind)
                {

                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    int secondsSinceEpoch = (int)t.TotalSeconds;
                    var rndAqua = (secondsSinceEpoch % 10) % 3 == 0;
                    var windMult = (((veh.Speed - 20) * 3.6) / 10000);
                    windMult = windMult >= 0.011f ? 0.011f : windMult;
                    if (Function.Call<float>((Hash)0xAD7E85FC227197C4, veh) >= 0.5)
                    {
                        windMult = windMult >= 0.007 ? 0.007 : windMult;
                    }
                    if (isRaining)
                        windMult += 0.009;

                    if (Game.IsControlPressed(0, GTA.Control.VehicleBrake) && false)
                    {
                        windMult += 0.008;
                        if (GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.6)
                        {
                            windMult += 0.005f;
                        }
                    }

                    windMult = windMult / 2;
                    if (Function.Call<float>((Hash)0xA8CF1CC0AFCD3F12) > 10)
                    {
                        //  windMult = windMult * 3f;
                    }
                    if (steeringAngle == 0)
                        Function.Call((Hash)0x42A8EC77D5150CBE, Game.Player.LastVehicle, (rndAqua ? steeringAngle - windMult : steeringAngle + windMult));
                    if (isRaining && (windTicks * 80) < windTicksMax && (veh.Speed * 3.6) > GenerateRandomNumberBetween(100, 140))
                    {
                        // Function.Call((Hash)0x222FF6A823D122E2, Game.Player.LastVehicle, true);
                        // Function.Call((Hash)0x93A3996368C94158, Game.Player.LastVehicle, 0f);
                        //  Function.Call((Hash)0xB59E4BD37AE292DB, Game.Player.LastVehicle, 0f);

                    }
                    else
                    {
                        // Function.Call((Hash)0x222FF6A823D122E2, Game.Player.LastVehicle, false);
                        //   Function.Call((Hash)0x93A3996368C94158, Game.Player.LastVehicle, 1f);
                        //  Function.Call((Hash)0xB59E4BD37AE292DB, Game.Player.LastVehicle, 1f);
                    }
                    windTicks++;
                    if ((windTicks * 40) >= windTicksMax)
                    {
                        windStopWatch = new Stopwatch();
                        windStopWatch.Start();
                        timeBeforeNextWind = GenerateRandomNumberBetween(9000 - (windTicksMax * 2), 13000 - (windTicksMax * 2));
                        windTicks = 0;
                    }
                }
            }
        }

        private void Aquaplaning()
        {

        }

        private void Tailgate()
        {

            // EL mirrors 
            if (veh.EngineRunning && veh.RoofState == VehicleRoofState.Opened)
            {
                veh.RoofState = VehicleRoofState.Closing;
            }
            if (!veh.EngineRunning && veh.RoofState == VehicleRoofState.Closed)
            {
                veh.RoofState = VehicleRoofState.Opening;
            }
            //
            if (tailgateClose.IsRunning)
            {
                tailgateAngle -= 0.006f;
                var door = 5;
                if (veh.DisplayName.ToLower().Contains("pts"))
                    door = 4;
                Function.Call(Hash.SET_VEHICLE_DOOR_CONTROL, veh, door, 5, tailgateAngle);
                if (tailgateAngle <= 0)
                {
                    veh.CloseDoor(VehicleDoor.Trunk, false);
                    if (veh.DisplayName.ToLower().Contains("pts"))
                        veh.CloseDoor(VehicleDoor.Hood, false);
                    tailgateClose = new Stopwatch();
                }
            }
            if (tailgateOpen.IsRunning)
            {
                tailgateAngle += 0.006f;
                var door = 5;
                if (veh.DisplayName.ToLower().Contains("pts"))
                    door = 4;
                Function.Call(Hash.SET_VEHICLE_DOOR_CONTROL, veh, door, 5, tailgateAngle);
                if (tailgateAngle >= 1)
                    tailgateOpen = new Stopwatch();
            }
            if (speedInKmh == 0 && !showApp)
            {
                if (!Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && tailgateStopWatch.ElapsedMilliseconds > 400 && tailgateStopWatch.IsRunning)
                {
                    tailgateStopWatch = new Stopwatch();
                    toggleTailgate = false;
                    isCentralLockLocked = true;
                }
                if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && tailgateStopWatch.ElapsedMilliseconds <= 400 && tailgateStopWatch.IsRunning)
                {
                    toggleTailgate = true;
                    centralLockRemoteStopWatch = new Stopwatch();
                    isCentralLockLocked = false;
                }
                if (Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && !tailgateStopWatch.IsRunning)
                {
                    tailgateStopWatch.Start();
                }
                if (toggleTailgate)
                {
                    toggleTailgate = false;
                    tailgateStopWatch = new Stopwatch();
                    if (isTruckMode)
                    {
                        if (truckTrailer.IsDoorOpen(VehicleDoor.BackRightDoor))
                        {
                            truckTrailer.CloseDoor(VehicleDoor.BackRightDoor, false);
                            truckTrailer.CloseDoor(VehicleDoor.BackLeftDoor, false);
                        }
                        else
                        {
                            truckTrailer.OpenDoor(VehicleDoor.BackRightDoor, false, false);
                            truckTrailer.OpenDoor(VehicleDoor.BackLeftDoor, false, false);
                        }
                        /* if (Function.Call<float>((Hash)0xFE3F9C29F7B32BD5, truckTrailer, 3) > 0.5f || Function.Call<float>((Hash)0xFE3F9C29F7B32BD5, truckTrailer, 2) > 0.5f)
                         {
                             Function.Call((Hash)0x93D9BD300D7789E5, truckTrailer, 0, false);
                             Function.Call((Hash)0x93D9BD300D7789E5, truckTrailer, 1, false);
                             Function.Call((Hash)0x93D9BD300D7789E5, truckTrailer, 2, false);
                             Function.Call((Hash)0x93D9BD300D7789E5, truckTrailer, 3, false);
                             Function.Call((Hash)0x93D9BD300D7789E5, truckTrailer, 4, false);
                             Function.Call((Hash)0x93D9BD300D7789E5, truckTrailer, 5, false);
                         }
                         else
                         {
                             Function.Call((Hash)0x7C65DAC73C35C862, truckTrailer, 0, false, false);
                             Function.Call((Hash)0x7C65DAC73C35C862, truckTrailer, 1, false, false);
                             Function.Call((Hash)0x7C65DAC73C35C862, truckTrailer, 2, false, false);
                             Function.Call((Hash)0x7C65DAC73C35C862, truckTrailer, 3, false, false);
                             Function.Call((Hash)0x7C65DAC73C35C862, truckTrailer, 4, false, false);
                             Function.Call((Hash)0x7C65DAC73C35C862, truckTrailer, 5, false, false);
                         }*/
                    }
                    if (veh.DisplayName.ToString().ToLower().Contains("ford") || veh.DisplayName.ToString().ToLower().Contains("speed"))
                    {
                        if (veh.IsDoorOpen(VehicleDoor.BackLeftDoor) && veh.IsDoorOpen(VehicleDoor.BackRightDoor))
                        {
                            veh.CloseDoor(VehicleDoor.BackLeftDoor, false);
                            veh.CloseDoor(VehicleDoor.BackRightDoor, false);
                        }
                        else if (!veh.IsDoorOpen(VehicleDoor.BackLeftDoor) && veh.IsDoorOpen(VehicleDoor.BackRightDoor))
                        {
                            veh.OpenDoor(VehicleDoor.BackLeftDoor, false, false);
                        }
                        else
                        {
                            veh.OpenDoor(VehicleDoor.BackRightDoor, false, false);
                        }
                    }


                    else if (
                        Function.Call<float>((Hash)0xFE3F9C29F7B32BD5, veh, 5) > 0.5f
                        || Function.Call<float>((Hash)0xFE3F9C29F7B32BD5, veh, 2) > 0.5f
                        || Function.Call<float>((Hash)0xFE3F9C29F7B32BD5, veh, 3) > 0.5f
                        || Function.Call<float>((Hash)0xFE3F9C29F7B32BD5, veh, 4) > 0.5f)
                    {
                        tailgateAngle = 1;
                        if (veh.DisplayName.ToString().ToLower() == "polo")
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 5, false);
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 4, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("trafic"))
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 2, false);
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 5, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("berlin"))
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 4, false);
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 5, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("gls"))
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 5, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("class") && false)
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 2, false);
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 3, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("toyota"))
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 2, false);
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 3, false);
                        }

                        else
                        {
                            Function.Call((Hash)0xF2BFA0430F0A0FCB, veh, 5, 0, 0f);
                            tailgateClose.Start();
                        }
                        if (!Game.Player.Character.IsInVehicle(veh))
                        {
                            tailgateCentralLockHazardsOpen = false;
                            tailgateCentralLockStopWatch.Start();
                        }
                    }
                    else
                    {
                        tailgateAngle = 0;
                        if (veh.DisplayName.ToString().ToLower() == "polo")
                        {
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 5, false, false);
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 4, false, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("class") && false)
                        {
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 2, false, false);
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 3, false, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("toyota"))
                        {
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 2, false, false);
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 3, false, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("trafic"))
                        {
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 2, false, false);
                            Function.Call((Hash)0x7C65DAC73C35C862, veh, 5, false, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("berlin"))
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 4, false, false);
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 5, false, false);
                        }

                        else if (veh.DisplayName.ToString().ToLower().Contains("gls"))
                        {
                            Function.Call((Hash)0x93D9BD300D7789E5, veh, 5, false, false);
                        }
                        else if (veh.DisplayName.ToString().ToLower().Contains("tesla"))
                        {
                            veh.OpenDoor(VehicleDoor.Trunk, false, false);
                        }
                        else
                        {
                            Function.Call((Hash)0xF2BFA0430F0A0FCB, veh, 5, 0, 999f);
                            tailgateOpen.Start();
                        }
                        if (!Game.Player.Character.IsInVehicle(veh))
                        {
                            tailgateCentralLockHazardsOpen = true;
                            tailgateCentralLockStopWatch.Start();
                        }
                    }
                }
            }
        }


        private void Trailer()
        {
            /*  if(speedInKmh == 0 && !Game.IsControlPressed(0, GTA.Control.VehicleAccelerate) && !canApplyReverse)
              {
                  veh.HandbrakeOn = true;
                  completeStop = true;
                  canApplyReverse = false;
              }
              if(!canApplyReverse && Game.IsControlJustPressed(0, GTA.Control.VehicleBrake))
              {
                  canApplyReverseStopWatch.Start();
              }
              if(!canApplyReverse 
                  && canApplyReverseStopWatch.IsRunning 
                  && canApplyReverseStopWatch.ElapsedMilliseconds >= 200 
                  && Game.IsControlJustReleased(0, GTA.Control.VehicleBrake))
              {
                  canApplyReverse = true;
                  canApplyReverseStopWatch = new Stopwatch();
              }
              if(Game.IsControlPressed(0, GTA.Control.VehicleAccelerate))
              {
                  veh.HandbrakeOn = false;
                  isCarInReverse = false;
              }*/
        }

        private void HandbrakeAfterCompleteStop()
        {
            /*  if(speedInKmh == 0 && !Game.IsControlPressed(0, GTA.Control.VehicleAccelerate) && !canApplyReverse)
              {
                  veh.HandbrakeOn = true;
                  completeStop = true;
                  canApplyReverse = false;
              }
              if(!canApplyReverse && Game.IsControlJustPressed(0, GTA.Control.VehicleBrake))
              {
                  canApplyReverseStopWatch.Start();
              }
              if(!canApplyReverse 
                  && canApplyReverseStopWatch.IsRunning 
                  && canApplyReverseStopWatch.ElapsedMilliseconds >= 200 
                  && Game.IsControlJustReleased(0, GTA.Control.VehicleBrake))
              {
                  canApplyReverse = true;
                  canApplyReverseStopWatch = new Stopwatch();
              }
              if(Game.IsControlPressed(0, GTA.Control.VehicleAccelerate))
              {
                  veh.HandbrakeOn = false;
                  isCarInReverse = false;
              }*/
        }

        private void SetVehicleLightsOnHour()
        {
            bool isBadWeather = Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Rain")
                    || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Thunder")
                    || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Clearing")
                     || Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "Foggy");
            var h = Function.Call<int>((Hash)0x25223CA6B4D20B7F);
            var m = Function.Call<int>((Hash)0x13D2B8ADD79640F2);
            var s = Function.Call<int>((Hash)0x494E97C2EF27C470);

            if ((h > 18 || h < 6) && !veh.LightsOn)
            {
                veh.LightsOn = true;
            }
            if (isBadWeather && !veh.LightsOn)
            {
                veh.LightsOn = true;
            }
        }

        private void TailgateCentralLock()
        {
            if (tailgateCentralLockStopWatch.ElapsedMilliseconds > 500)
            {
                tailgateCentralLockStopWatch = new Stopwatch();
                veh.LeftIndicatorLightOn = true;
                veh.RightIndicatorLightOn = true;
                isLeftIndicatorOn = true;
                isRightIndicatorOn = true;
                tailgateCentralLockHazardsStopWatch.Start();
            }
            if (tailgateCentralLockHazardsStopWatch.ElapsedMilliseconds > 1800)
            {
                tailgateCentralLockHazardsStopWatch = new Stopwatch();
                veh.LeftIndicatorLightOn = false;
                veh.RightIndicatorLightOn = false;
                isLeftIndicatorOn = false;
                isRightIndicatorOn = false;
            }
        }

        private static Random ZeroTrafficRnd = new Random();
        private static Stopwatch ZeroTrafficStopwatch = new Stopwatch();
        private static int ZeroTrafficMS = 0;
        public static void LiveTraffic(bool overide = false)
        {
            var h = fakeTimeHours;
            var m = fakeTimeMinutes;
            var s = fakeTimeSeconds;
            var day = Function.Call<int>((Hash)0xD972E4BD7AEB235F);
            if (isOnAutobahn)
            {
                if (veh.Position.DistanceTo(new Vector3(-366.36f, -3828.408f, 6.6f)) < 4630)
                    timeScale = 16f;
                else
                    timeScale = 2f;
            }
            if (true)
            {
                /*  if (h >= 0 && h <= 2)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.1f);
                      Function.Call((Hash)0x245A6883D966D537, 0.1f);
                  }
                  if (h >= 3 && h <= 5)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.01f);
                      Function.Call((Hash)0x245A6883D966D537, 0.01f);
                  }
                  if (h == 6)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.13f);
                      Function.Call((Hash)0x245A6883D966D537, 0.13f);
                  }
                  if (h >= 7 && h < 9)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.17f);
                      Function.Call((Hash)0x245A6883D966D537, 0.17f);
                  }
                  if (h >= 9 && h < 14)
                  {

                      Function.Call((Hash)0xB3B3359379FE77D3, 0.15f);
                      Function.Call((Hash)0x245A6883D966D537, 0.15f);
                  }
                  if (h == 14)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.16f);
                      Function.Call((Hash)0x245A6883D966D537, 0.16f);
                  }
                  if (h >= 15 && h < 18)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.17f);
                      Function.Call((Hash)0x245A6883D966D537, 0.17f);
                  }
                  if (h >= 18 && h < 20)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.14f);
                      Function.Call((Hash)0x245A6883D966D537, 0.14f);
                  }
                  if (h >= 20 && h < 23)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.10f);
                      Function.Call((Hash)0x245A6883D966D537, 0.10f);
                  }
                  if (h == 23)
                  {
                      Function.Call((Hash)0xB3B3359379FE77D3, 0.08f);
                      Function.Call((Hash)0x245A6883D966D537, 0.08f);
                  }
                  */
                // old for better pc
                //   UI.ShowSubtitle(Game.Player.Character.Position.X + "  " + Game.Player.Character.Position.Y + " " + Game.Player.Character.Position.Z + " " + Game.Player.Character.Heading.ToString());
                Vector3 cityCenterPos = new Vector3(29.4f, -1126.0f, 29.22f);
                Vector3 hwy1Pos = new Vector3(1632f, -950.9f, 62.72f);
                int hwy1ExitMin = 255;
                int hwy1ExitMax = 322;
                int hwy1EnterMax = 146;
                int hwy1EnterMin = 74;
                Vector3 hwy2Pos = new Vector3(1096.4f, 382.3f, 82.72f);
                int hwy2ExitMin = 287;
                int hwy2ExitMax = 355;
                int hwy2EnterMax = 181;
                int hwy2EnterMin = 116;
                Vector3 palPos = new Vector3(542.6f, 6549.7f, 27.4f);
                int palExitMin = 226;
                int palExitMax = 342;
                int palEnterMax = 127;
                int palEnterMin = 38;
                var vp = veh.Position;
                var vh = veh.Heading;
                bool isSpecialTraffic = false;
                bool forceTraff = false;
                //   UI.ShowSubtitle(vp.DistanceTo(palPos).ToString());
                tv = 0f;
                if (h >= 0 && h <= 2)
                {
                    tv = 0.1f;
                }
                if (h >= 3 && h <= 5)
                {
                    tv = 0.01f;
                }
                if (h == 6)
                {
                    if (vp.DistanceTo(cityCenterPos) < 1000)
                    {
                        tv = 0.6f;
                    }
                    else
                        tv = 0.25f;
                }
                if (h >= 7 && h < 9)
                {
                    if (vp.DistanceTo(cityCenterPos) < 1000f)
                    {
                        tv = 50f;
                        forceTraff = true;
                    }
                    if (vh > hwy1ExitMin && vh < hwy1ExitMax && vp.DistanceTo(hwy1Pos) < 850f)
                    {
                        tv = 0.3f;
                        if (vp.DistanceTo(hwy1Pos) < 500f)
                            isSpecialTraffic = true;
                    }
                    if (vh > hwy1EnterMin && vh < hwy1EnterMax && vp.DistanceTo(hwy1Pos) < 850f)
                    {
                        tv = 99f;
                        forceTraff = true;
                        if (vp.DistanceTo(hwy1Pos) < 500f)
                            isSpecialTraffic = true;
                    }
                    if (vh > hwy2ExitMin && vh < hwy2ExitMax && vp.DistanceTo(hwy2Pos) < 850f)
                    {
                        tv = 0.3f;
                        isSpecialTraffic = true;
                    }
                    if (vh > hwy2EnterMin && vh < hwy2EnterMax && vp.DistanceTo(hwy2Pos) < 850f)
                    {
                        tv = 99f;
                        forceTraff = true;
                        if (vp.DistanceTo(hwy2Pos) < 500f)
                            isSpecialTraffic = true;
                    }
                    if (vh > palExitMin && vh < palExitMax && vp.DistanceTo(palPos) < 500f)
                    {
                        tv = 0.3f;
                        isSpecialTraffic = true;
                    }
                    if (vh > palEnterMin && vh < palEnterMax && vp.DistanceTo(palPos) < 1400f)
                    {
                        tv = 99f;
                        forceTraff = true;
                        if (vp.DistanceTo(palPos) < 1100f)
                            isSpecialTraffic = true;
                    }
                    if (!isSpecialTraffic && !forceTraff)
                    {
                        tv = day > 5 && false ? 0.25f : 0.5f;
                    }

                }
                if (h >= 9 && h < 14)
                {
                    if (vp.DistanceTo(cityCenterPos) < 1000f)
                    {
                        tv = day > 5 && false ? 0.25f : 0.6f;
                    }
                    else if (!isSpecialTraffic)
                    {
                        tv = 0.5f;
                    }
                }
                if (h == 14)
                {
                    if (vp.DistanceTo(cityCenterPos) < 1000f)
                    {
                        tv = day > 5 && false ? 0.25f : 0.9f;
                        forceTraff = true;
                    }
                    if (!isSpecialTraffic && !forceTraff)
                    {
                        tv = day > 5 && false ? 0.4f : 0.7f;
                    }
                }
                if (h >= 15 && h < 18)
                {
                    if (vp.DistanceTo(cityCenterPos) < 1000f)
                    {
                        tv = day > 5 && false ? 0.25f : 50f;
                        forceTraff = true;
                    }
                    if (vh > hwy1ExitMin && vh < hwy1ExitMax && vp.DistanceTo(hwy1Pos) < 850f)
                    {
                        tv = 99f;
                        forceTraff = true;
                        if (vp.DistanceTo(hwy1Pos) < 500f)
                            isSpecialTraffic = true;
                    }
                    if (vh > hwy1EnterMin && vh < hwy1EnterMax && vp.DistanceTo(hwy1Pos) < 850f)
                    {
                        tv = 0.4f;
                        isSpecialTraffic = true;
                    }
                    if (vh > hwy2ExitMin && vh < hwy2ExitMax && vp.DistanceTo(hwy2Pos) < 850f)
                    {
                        tv = 99f;
                        forceTraff = true;
                        if (vp.DistanceTo(hwy2Pos) < 500f)
                            isSpecialTraffic = true;
                    }
                    if (vh > hwy2EnterMin && vh < hwy2EnterMax && vp.DistanceTo(hwy2Pos) < 850f)
                    {
                        tv = 0.4f;
                        isSpecialTraffic = true;
                    }
                    if (vh > palExitMin && vh < palExitMax && vp.DistanceTo(palPos) < 500f)
                    {
                        tv = 99f;
                        forceTraff = true;
                        if (vp.DistanceTo(palPos) < 500f)
                            isSpecialTraffic = true;
                    }
                    if (isWarehouse)
                    {
                        tv = 99f;
                        forceTraff = true;
                    }
                    if (vh > palEnterMin && vh < palEnterMax && vp.DistanceTo(palPos) < 700f)
                    {
                        tv = 0.3f;
                        isSpecialTraffic = true;
                    }
                    if (!isSpecialTraffic && !forceTraff)
                    {
                        tv = day > 5 && false ? 0.3f : 0.6f;
                    }
                    if (isInLC)
                    {
                        tv = 50f;
                        forceTraff = true;
                    }
                }
                if (h >= 18 && h < 20)
                {
                    tv = 0.45f;
                    if (vp.DistanceTo(cityCenterPos) < 1000f)
                    {
                        tv = 0.6f;
                    }
                }
                if (h >= 20 && h < 23)
                {
                    tv = 0.3f;
                    if (vp.DistanceTo(cityCenterPos) < 1000f)
                    {
                        tv = 0.37f;
                    }
                }
                if (h == 23)
                {
                    tv = 0.2f;
                    if (vp.DistanceTo(cityCenterPos) < 1000f)
                    {
                        tv = 0.27f;
                    }
                }



                var spec = RoadConsTraffic(tv);
                if (spec > 60f)
                {
                    isSpecialTraffic = true;
                }
                tv = spec;
                if (isSpecialTraffic && tv >= 60f)
                {
                    TrafficJamSlowdown();
                }
                else if (trafficJamSpeedZoneId > 0)
                {
                    nextTrafficJamStopwatch = new Stopwatch();
                    Function.Call((Hash)0x1033371FC8E842A7, trafficJamSpeedZoneId);
                    trafficJamSpeedZoneId = 0;
                }
                if ((isSpecialTraffic && !isWarehouse) || isInCurrentIntersection || isInTrain)
                {
                    timeScale = 12f;
                }
                else
                {
                    timeScale = defaultTimeScale;
                }
                if (isInLC)
                {
                    if (h >= 0 && h <= 2)
                    {
                        tv = 0.25f;
                    }
                    if (h >= 3 && h <= 5)
                    {
                        tv = 0.25f;
                    }
                    if (h == 6)
                    {
                        tv = 0.35f;
                    }
                    if (h >= 7 && h < 9)
                    {
                        tv = 0.5f;
                    }
                    if (h >= 9 && h < 14)
                    {
                        tv = 0.4f;
                    }
                    if (h == 14)
                    {
                        tv = 0.5f;
                    }
                    if (h >= 15 && h < 18)
                    {
                        tv = 0.5f;
                    }
                    if (h >= 18 && h < 20)
                    {
                        tv = 0.4f;
                    }
                    if (h >= 20 && h < 23)
                    {
                        tv = 0.3f;
                    }
                    if (h == 23)
                    {
                        tv = 0.26f;
                    }
                }
                if (veh.Position.DistanceTo(clearVehiclesFromNyPos) < 300)
                {
                    tv = 0f;
                }
                if (isS7Loaded)
                {
                    if (h >= 0 && h <= 2)
                    {
                        tv = 0.01f;
                    }
                    if (h >= 3 && h <= 5)
                    {
                        tv = 0.01f;
                    }
                    if (h == 6)
                    {
                        tv = 0.25f;
                    }
                    if (h >= 7 && h < 9)
                    {
                        tv = 1.5f;
                    }
                    if (h >= 9 && h < 14)
                    {
                        tv = 0.7f;
                    }
                    if (h == 14)
                    {
                        tv = 0.9f;
                    }
                    if (h >= 15 && h < 18)
                    {
                        tv = 99f;
                    }
                    if (h >= 18 && h < 20)
                    {
                        tv = 0.38f;
                    }
                    if (h >= 20 && h < 23)
                    {
                        tv = 0.25f;
                    }
                    if (h == 23)
                    {
                        tv = 0.23f;
                    }
                }

                if (h >= 0 && h < 6 && !isInLC && !isOnAutobahn)
                {
                    if (!ZeroTrafficStopwatch.IsRunning)
                    {
                        ZeroTrafficStopwatch = new Stopwatch();
                        ZeroTrafficStopwatch.Start();
                    }

                    if (ZeroTrafficMS <= 0)
                    {
                        ZeroTrafficMS = ZeroTrafficRnd.Next(10000, 38000);
                    }
                    if (ZeroTrafficStopwatch.ElapsedMilliseconds < ZeroTrafficMS)
                    {
                        tv = 0f;
                    }
                    if (ZeroTrafficStopwatch.ElapsedMilliseconds >= 60000)
                    {
                        ZeroTrafficStopwatch = new Stopwatch();
                        ZeroTrafficMS = 0;
                    }
                }

                Function.Call((Hash)0x95E3D6257B166CF2, 99f); // peds multiplier
                Function.Call((Hash)0xB3B3359379FE77D3, tv);
                Function.Call((Hash)0x245A6883D966D537, tv);
                Function.Call((Hash)0xEAE6DCC7EEE3DB1D, 99f);


                /* old traffic 2x
                  if (h >= 0 && h <= 2)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, 0.2f);
                    Function.Call((Hash)0x245A6883D966D537, 0.2f);
                }
                if (h >= 3 && h <= 5)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, 0.01f);
                    Function.Call((Hash)0x245A6883D966D537, 0.01f);
                }
                if (h == 6)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, 0.5f);
                    Function.Call((Hash)0x245A6883D966D537, 0.5f);
                }
                if (h >= 7 && h < 9)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, day > 5 ? 0.3 : 1.6f);
                    Function.Call((Hash)0x245A6883D966D537, day > 5 ? 0.3 : 1.6f);
                }
                if (h >= 9 && h < 14)
                {

                    Function.Call((Hash)0xB3B3359379FE77D3, 0.4f);
                    Function.Call((Hash)0x245A6883D966D537, 0.4f);
                }
                if (h == 14)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, day > 5 ? 0.7 : 1.4f);
                    Function.Call((Hash)0x245A6883D966D537, day > 5 ? 0.7 : 1.4f);
                }
                if (h >= 15 && h < 18)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, day > 5 ? 0.7 : 1.2f);
                    Function.Call((Hash)0x245A6883D966D537, day > 5 ? 0.7 : 1.2f);
                }
                if (h >= 18 && h < 20)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, 0.5f);
                    Function.Call((Hash)0x245A6883D966D537, 0.5f);
                }
                if (h >= 20 && h < 23)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, 0.4f);
                    Function.Call((Hash)0x245A6883D966D537, 0.4f);
                }
                if (h == 23)
                {
                    Function.Call((Hash)0xB3B3359379FE77D3, 0.3f);
                    Function.Call((Hash)0x245A6883D966D537, 0.3f);
                }*/
            }
        }
        List<Vector3> offsets = new List<Vector3>()
            {
                new Vector3(3f, 80f, 0),
                 new Vector3(-10f, 80f, 0),
            };


        public static float RoadConsTraffic(float currTraff)
        {
            var minHeading = 310f;
            var maxHeading = 39f;
            var dist = veh.Position.DistanceTo(new Vector3(2603.5f, 658.1f, 93.5f));
            var cond = (veh.Heading > minHeading && veh.Heading <= 360) || (veh.Heading < maxHeading && veh.Heading >= 0);
            if (dist < 1000 && cond && false)
            {
                var h = fakeTimeHours;
                bool isSpec = false;
                if (h == 6)
                {
                    currTraff = 1.3f;
                }
                if (h >= 7 && h < 9)
                {
                    currTraff = 99f;
                    isSpec = true;
                }
                if (h >= 9 && h < 14)
                {
                    currTraff = 3f;
                }
                if (h == 14)
                {
                    currTraff = 7f;
                    isSpec = true;
                }
                if (h >= 15 && h < 18)
                {
                    currTraff = 99f;
                    isSpec = true;
                }
                if (h >= 18 && h < 20)
                {
                    currTraff = 3f;
                }
                if (currTraff > 3f)
                {
                    TrafficJamSlowdown();
                }
                else
                {
                    nextTrafficJamStopwatch = new Stopwatch();
                }
            }
            return currTraff;
        }

        private static void TrafficJamSlowdown()
        {
            if (!nextTrafficJamStopwatch.IsRunning)
            {
                nextTrafficJamStopwatch.Start();
            }
            if (nextTrafficJamStopwatch.ElapsedMilliseconds > timeToNextTrafficJam)
            {
                nextTrafficJamStopwatch = new Stopwatch();
                timeToNextTrafficJam = Warehouse.rnd.Next(trafficJamSpeed > 0f ? 17000 : 10000, trafficJamSpeed > 0f ? 23000 : 13000);
                Function.Call((Hash)0x1033371FC8E842A7, trafficJamSpeedZoneId);
                var off = veh.GetOffsetInWorldCoords(new Vector3(0, 290f, 0));
                trafficJamSpeedZoneId = 0;
                trafficJamSpeed = trafficJamSpeed > 0f ? 0f : 14f;
                trafficJamSpeedZoneId = Function.Call<int>((Hash)0x2CE544C68FB812A0, off.X, off.Y, off.Z, 55f, trafficJamSpeed, false);
                // UI.ShowSubtitle("jam speed: " + trafficJamSpeed.ToString() + " next will be: " + timeToNextTrafficJam);
            }
        }

        private int GenerateRandomNumberBetween(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }

        private int GenerateRandomNumberBetween2(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }

    }
}


/*
 * old addaptive braking
 * 
 * private void AddaptiveBrakeLights()
        {
            if (Game.IsControlPressed(0, GTA.Control.VehicleBrake) && !calculatingSpeedDiff && speedInKmh > 100)
            {
                speedDiffStopWatch = new Stopwatch();
                speedDiffStopWatch.Start();
                calculatingSpeedDiff = true;
                prevSpeed = speedInKmh;
            }
            if (calculatingSpeedDiff && speedDiffStopWatch.ElapsedMilliseconds > 150)
            {
                double brakeForce = (double)(Function.Call<float>((Hash)0xAD7E85FC227197C4, veh));
                var currSpeed = speedInKmh;
                var speedDiff = prevSpeed - currSpeed;
                var ratio = speedDiff / brakeForce;
                veh.NumberPlate = ratio.ToString();
                speedDiffStopWatch = new Stopwatch();
                if (speedDiff >= 7)
                { 
                    canTurnOnAddaptiveLights = true;
                }
                UI.ShowSubtitle("diff: " + speedDiff + " ratio: " + ratio);
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleBrake))
            {
                speedDiffStopWatch.Stop();
                calculatingSpeedDiff = false;
                canTurnOnAddaptiveLights = false;
            }

            if (addaptiveSequenceLightsOff && addaptiveSequence)
            {
                veh.BrakeLightsOn = false;
            }
            if (!addaptiveSequenceLightsOff && addaptiveSequence)
            {
                veh.BrakeLightsOn = true;
            }
            if (addaptiveSequence)
            {
                stopwatch.Start();
                if (stopwatch.Elapsed.Milliseconds > 100 && !addaptiveSequenceLightsOff)
                {
                    stopwatch = new Stopwatch();
                    addaptiveSequenceLightsOff = true;
                }
                if (stopwatch.Elapsed.Milliseconds > 100 && addaptiveSequenceLightsOff)
                {
                    stopwatch = new Stopwatch();
                    addaptiveSequenceLightsOff = false;
                }
            }
            if (Game.IsControlPressed(0, GTA.Control.VehicleBrake) && !addaptiveSequence && canTurnOnAddaptiveLights)
            { 
                addaptiveSequence = true;
            }
            if (Game.IsControlJustReleased(0, GTA.Control.VehicleBrake) && addaptiveSequence)
            {
                if (speedInKmh <= 20)
                {
                    veh.RightIndicatorLightOn = true;
                    veh.LeftIndicatorLightOn = true;
                    hazardsAfterAddaptiveBraking = true;
                }
                addaptiveSequence = false;
            }

            if (hazardsAfterAddaptiveBraking && speedInKmh > 20)
            {
                veh.RightIndicatorLightOn = false;
                veh.LeftIndicatorLightOn = false;
                hazardsAfterAddaptiveBraking = false;
            }
        }
*/

// old aquaplaingin 

/* if (Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "XMAS") && !isCarGripReducedByRain)
       {
           UI.ShowSubtitle("snow!");
           isCarGripReducedByRain = true;
           Function.Call((Hash)0x222FF6A823D122E2, Game.Player.LastVehicle, true);
       }
       if (!Function.Call<bool>((Hash)0x2FAA3A30BEC0F25D, "XMAS") && isCarGripReducedByRain)
       { 
           isCarGripReducedByRain = false;
           Function.Call((Hash)0x222FF6A823D122E2, Game.Player.LastVehicle, false);
       }*/
/* if (!isCarGripReducedByRain && Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown))
 {
     string rainLevel = Function.Call<string>((Hash)0x96695E368AD855F3);
     UI.ShowSubtitle("reduced on rain level: " + rainLevel);
     isCarGripReducedByRain = true;
     Function.Call((Hash)0x222FF6A823D122E2, Game.Player.LastVehicle, true);
 }
 if(Game.IsControlJustPressed(0, GTA.Control.ScriptPadDown) && isCarGripReducedByRain)
 {
     string rainLevel = Function.Call<string>((Hash)0x96695E368AD855F3);
     UI.ShowSubtitle("reduced off rain level: " + rainLevel);
     isCarGripReducedByRain = false; 
     Function.Call((Hash)0x222FF6A823D122E2, Game.Player.LastVehicle, false);
 }*/


/* OLD RAINSPRAY
   if (!hasCelingAbove && isRaining && (isTruckMode ? (speedInKmh > 50) : (speedInKmh > 65)) && (((Math.Round(height - 1.8, 0)) < 4 ? (Math.Round(height - 1.8, 1)) : (Math.Round(height - 1.8, 0))) <= 0 || veh.ClassType != VehicleClass.Planes))
            {
                var lib = "core";
                var asset = "water_splash_vehicle";
                if (Function.Call<bool>(GTA.Native.Hash.HAS_PTFX_ASSET_LOADED, lib))
                {
                    Function.Call<bool>(GTA.Native.Hash.REQUEST_PTFX_ASSET, lib);
                }
                else
                {
                    Function.Call((Hash)0x6C38AF3693A69A91, lib);
                    decimal sprayFactor = Decimal.Divide((decimal)(speedInKmh), 50);
                    //  if ((double)sprayFactor > 2.0)
                    //     sprayFactor = (decimal)2.0;
                    var offset = -2.0f;
                    var sprayZ = 0f;
                    float alpha = 0.14f;
                    var h = fakeTimeHours;
                    if (h < 5 || h > 21)
                        alpha = 0.14f;
                    if (speedInKmh < 72)
                        alpha = 0.12f;
                    if (veh.DisplayName.Contains("17magotan") || veh.DisplayName.Contains("x5") || veh.DisplayName.Contains("CH-R") || veh.DisplayName.Contains("mk1") || veh.DisplayName.Contains("esla"))
                    {
                        offset = -2.3f;
                    }
                    if (isTruckMode)
                    {
                        offset = -2f;
                        alpha = 0.06f;
                    }
                    if (veh.DisplayName.ToLower().Contains("superb"))
                    {
                        offset = -2.6f;
                        sprayZ = 0.08f;
                    }
                    if (veh.DisplayName.ToLower().Contains("660"))
                    {
                        offset = -2.8f;
                        sprayZ = 0.1f;
                    }
                    if (veh.DisplayName.ToLower().Contains("a6"))
                    {
                        offset = -2.4f;
                        sprayZ = 0.2f;
                    }
                    if (veh.DisplayName.ToLower().Contains("svr"))
                    {
                        offset = -2.6f;
                        sprayZ = 0.1f;
                    }
                    if (veh.DisplayName.ToLower().Contains("fjcru"))
                    {
                        sprayZ = 0.4f;
                    }
                    if (veh.DisplayName.ToLower().Contains("qash"))
                    {
                        offset = -2.14f;
                        sprayZ = 0.05f;
                    }
                    if (veh.DisplayName.ToLower().Contains("exped"))
                    {
                        offset = -6.0f;
                        sprayZ = -1f;
                        alpha = 0.14f;
                        // sprayFactor += (decimal)0.3;
                    }
                    if (veh.DisplayName.Contains("17magotan"))
                    {
                        sprayZ = 0.3f;
                    }
                    if (veh.DisplayName.Contains("X6"))
                    {
                        sprayZ = 0.35f;
                        offset = -2.2f;
                    }
                    if (veh.DisplayName.ToLower().Contains("speedo"))
                    {
                        offset = -2.8f;
                        sprayZ = 0.1f;
                        sprayFactor += (decimal)0.7;
                    }
                    if (veh.DisplayName.ToLower().Contains("rogue"))
                    {
                        offset = -3.2f;
                        sprayZ = 0.4f;
                    }
                    if (veh.DisplayName.ToLower().Contains("golf"))
                    {
                        sprayZ = 0.28f;
                    }
                    if (veh.DisplayName.ToLower().Contains("paj"))
                    {
                        sprayZ = 0.1f;
                    }

                    if (speedInKmh < 100)
                    {
                        sprayZ -= 0.5f;
                    }
                    else if (speedInKmh < 150)
                    {
                        sprayZ -= (1.5f - (speedInKmh / 100));
                    }
                    var id = Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, veh, 0f, offset, sprayZ, 0f, 0f, 0f, (float)sprayFactor, 0, 0, 0);
                    Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha);
                    if (isTruckMode)
                    {
                        sprayFactor += (decimal)0.15;
                        Function.Call((Hash)0x6C38AF3693A69A91, lib);
                        Function.Call<int>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, asset, truckTrailer, 0f, -2.5f, -1.4f, 0f, 0f, 0f, (float)sprayFactor, 0, 0, 0);
                    }
                    Function.Call(Hash.SET_PARTICLE_FX_NON_LOOPED_ALPHA, alpha);
                }
            }
 
 
 
 */
