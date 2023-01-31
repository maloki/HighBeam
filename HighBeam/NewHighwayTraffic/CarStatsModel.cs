using GTA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public class CarStatsModel
    {
        public Model Model { get; set; }
        public float Speed { get; set; }
        public float DefaultSpeed { get; set; }
        public string Class { get; set; }
        public float Heading { get; set; }
        public bool isLeftLane { get; set; }
        public string Id { get; set; }
        public string OvertakeId { get; set; }
        public bool IsOvertaking { get; set; }
        public float SteerPercentage { get; set; }
        public bool Deleted { get; set; }
        public bool isTrafficJamService { get; set; }
        public bool isLookingInMirrors { get; set; }
        public bool noOvertake { get; set; }
        public bool shouldReturnToDomesticLane { get; set; }
        public bool deccelerate { get; set; }
        public bool accelerate { get; set; }
        public float adjustSpeedTo { get; set; }
        public bool notDefaultSpeed { get; set; }
        public bool forcedLeftLane { get; set; }
        public List<string> OvertakenCarsIds { get; set; }
        public bool isPlayerCar { get; set; }
        public bool isFlashingHighBeam { get; set; }
        public Stopwatch highbeamStopWatch { get; set; }
        public int highBeamTicks { get; set; }
        public bool isEmpty { get; set; }
        public bool isEmergencyBraking { get; set; }
        public bool isInCorner { get; set; }
        public bool isPlayerTruck { get; set; }
        public float HeadingPercentage { get; set; }
    }
}
