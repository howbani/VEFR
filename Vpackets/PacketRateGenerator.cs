using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using VSIM_VEFR.Operations;
using VSIM_VEFR.Properties;
using VSIM_VEFR.RoadNet.Components;

namespace VSIM_VEFR.Vpackets
{
    public class PacketRateGenerator
    {
        MainWindow _mw;
        SourceDestinationSelector selector;
        public PacketRateGenerator(MainWindow mw)
        {
            _mw = mw;
            selector = new SourceDestinationSelector(_mw);
        }
        public DispatcherTimer RandomSelectSourceNodeTimer = new DispatcherTimer();

        /// <summary>
        /// change packet rate.
        /// </summary>
        /// <param name="s"></param>
        public void ChangePacketRange(double s)
        {
            if (_mw.MyVehicles.Count > 0)
            {
                if (s == 0)
                {
                    RandomSelectSourceNodeTimer.Stop();
                    RandomSelectSourceNodeTimer.Interval = TimeSpan.FromSeconds(0);

                }
                else
                {
                    RandomSelectSourceNodeTimer.Tick += GeneratePacket_to_rate;
                    RandomSelectSourceNodeTimer.Interval = TimeSpan.FromSeconds(s);
                    RandomSelectSourceNodeTimer.Start();

                }
            }
            else
            {
                MessageBox.Show("Vehicle are not deployed.");
            }
        }

        /// <summary>
        /// generate packet according to the rate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneratePacket_to_rate(object sender, EventArgs e)
        {
            selector.RandomlySourceVechicle(Settings.Default.DistanceImportance, Settings.Default.DistanceBetweenSourceAndDestination);
        }

      

        /// <summary>
        /// set the timer to generate x of packets
        /// </summary>
        /// <param name="xofpackets"></param>
        public void GenerateXofPackets(int xofpackets)
        {
            Settings.Default.NumberofPackets = xofpackets;
            RandomSelectSourceNodeTimer.Tick += GenerateXofPacketTrick;
            RandomSelectSourceNodeTimer.Interval = TimeSpan.FromSeconds(0.1);
            RandomSelectSourceNodeTimer.Start();
        }



        /// <summary>
        /// generate x of packets.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GenerateXofPacketTrick(object sender, EventArgs e)
        {
            if (PublicStatistics.GeneratedPacketsCount >= Settings.Default.NumberofPackets)
            {
                // stop:
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
