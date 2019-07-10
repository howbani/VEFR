using System;
using System.Collections.Generic;
using VSIM_VEFR.FuzzySets;
using VSIM_VEFR.Properties;
using VSIM_VEFR.UI;
using VSIM_VEFR.Vpackets;
using static VSIM_VEFR.Operations.NetSettings;
using static VSIM_VEFR.RoadNet.Components.VehicleUi;

namespace VSIM_VEFR.Operations
{
    public static class PublicParamerters
    {
        public static int sessionID { get; set; }
        public static List<RVSInput> RVSInputList = new List<RVSInput>();
        public static double TransmissionRate = 2 * 1000000;////2Mbps 100 × 10^6 bit/s , //https://en.wikipedia.org/wiki/Transmission_time
        public static double SpeedOfLight = 299792458;//https://en.wikipedia.org/wiki/Speed_of_light // s

        #region Road Network
        public static string NetworkName { get; set; }
        public static double JunctionWidth => 2 * (LaneWidth * 3);
        public static double JunctionHeight => 2 * (LaneWidth * 3);
        public static double LaneWidth = 3.5;
       // public static double NumberOfLanes = 2;
        /// <summary>
        /// the timer to switch the Trafic Signaling
        /// </summary>
       
        /// <summary>
        /// when there is only two lanes in the road segment. the lane can switch to direct  right or left. this timeer is to make an atomatic switch
        /// </summary>
        public static TimeSpan RoadSegmentSwitchDirectionTimerInterval
        {
            get
            {
                double min = 1;
                double max = 2;
                double ran= RandomeNumberGenerator.GetUniform(min, max);
                return TimeSpan.FromSeconds(ran); //ran; // seconds.
            }
        }
        
       
        
        #endregion

        #region  Vehicle
        public static AccellerationType AccellerationType { get; set; }
        public static double VehicleWidth = 2.0;
        public static double VehicleHight = 2.0;
        public static double RemianDistanceToHeadingJunctionThreshold = Settings.Default.CommunicationRange; // the time when the v should select new v.
        #region speed
        public static double MeanSpeed { get { return (Settings.Default.MaxSpeed + Settings.Default.MinSpeed) / 2; } }
        public static double SpeedStandardDeviation = 1;
        #endregion

        #region VehicleInterArrival
        public static double VehicleInterArrivalMean = 2; // seconds. we have 6
        public static double VehicleInterArrivalStandardDeviation { get { return VehicleInterArrivalMean * 0.3; } } // seconds. // for enery.  
        #endregion

        #endregion


        #region Packets
        public static double BufferSize = 50; // the maximum number of packets allowed in the queue.
        public static double PacketQueueTimerInterval = 0.0000001; // in seconds.


        public static double PacketQueueRetryTimerInterval
        {
            get
            {
                return Settings.Default.MaxStoreTime;
            }
        }


        public static double DataPacketLength = 1024;
       
        #endregion




    }
}
