using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using VSIM_VEFR.Operations;
using VSIM_VEFR.Properties;

namespace VSIM_VEFR.Vpackets
{
    /// <summary>
    /// Generate the packets such that no two packets are in the same time in the network.
    /// </summary>
    public class PacketRateQueueGnernator
    {
        MainWindow _mw;
        public DispatcherTimer RandomSelectSourceNodeTimer = new DispatcherTimer();
        SourceDestinationSelector selector;
        public PacketRateQueueGnernator(MainWindow mw)
        {
            _mw = mw;
            selector = new SourceDestinationSelector(_mw);
        }

        /// <summary>
        /// set the timer to generate x of packets
        /// </summary>
        /// <param name="xofpackets"></param>
        public void GenerateXofPackets(int xofpackets)
        {
            Settings.Default.NumberofPackets = xofpackets;
            RandomSelectSourceNodeTimer.Tick += GenerateXofPacketTrick;
            RandomSelectSourceNodeTimer.Interval = TimeSpan.FromSeconds(1);
            RandomSelectSourceNodeTimer.Start();


        }

        public void GenerateXofPacketTrick(object sender, EventArgs e)
        {
            if (PublicStatistics.InQueuePackets <= 0)
            {
                if (PublicStatistics.GeneratedPacketsCount >= Settings.Default.NumberofPackets)
                {

                    RandomSelectSourceNodeTimer.Stop();
                    RandomSelectSourceNodeTimer.Interval = TimeSpan.FromSeconds(0);
                    Settings.Default.NumberofPackets = 0;
                }
                else
                {
                    // generate more:
                    selector.RandomlySourceVechicle(Settings.Default.DistanceImportance, Settings.Default.DistanceBetweenSourceAndDestination);
                }
            }
        }






    }
}
