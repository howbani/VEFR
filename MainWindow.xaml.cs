using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VSIM_VEFR.db;
using VSIM_VEFR.Demo;
using VSIM_VEFR.experments;
using VSIM_VEFR.Operations;
using VSIM_VEFR.Properties;
using VSIM_VEFR.RoadNet.Components;
using VSIM_VEFR.UI;
using VSIM_VEFR.Vpackets;

namespace VSIM_VEFR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UiNetGen uiNetGen;
        UIsetExperment uIsetExperment;
        public PacketRateGenerator packetRateGenerator;
        public PacketRateQueueGnernator packetRateQueueGnernator; 
        public List<Junction> MyJunctions = new List<Junction>();
        public List<RoadSegment> MyRoadSegments = new List<RoadSegment>();
        public List<VehicleUi> MyVehicles = new List<VehicleUi>();
        public double StreenTimes = 1;
        
        public MainWindow()
        {
            InitializeComponent();

            Height = SystemParameters.FullPrimaryScreenHeight;
            Width = SystemParameters.FullPrimaryScreenWidth;

            vanet_scroller.MaxWidth = SystemParameters.FullPrimaryScreenWidth - 10;
            vanet_scroller.MaxHeight = SystemParameters.FullPrimaryScreenHeight - 60;


            canvas_vanet.Width = StreenTimes * SystemParameters.FullPrimaryScreenWidth;
            canvas_vanet.Height = StreenTimes * SystemParameters.FullPrimaryScreenHeight;

           

            PublicStatistics.LiveStatstics.Topmost = true;
            Settings.Default.IsIntialized = false;
            uIsetExperment = new UIsetExperment(this);
            uiNetGen = new UiNetGen(this);

            packetRateGenerator = new PacketRateGenerator(this); // old one
            packetRateQueueGnernator = new PacketRateQueueGnernator(this); // new gen

        }

        public void OpenTopGen()
        {
            try
            {
                uiNetGen.Show();
            }
            catch
            {
                uiNetGen = new UiNetGen(this);
                uiNetGen.Show();
            }
        }


        private void ComponentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Point po = Mouse.GetPosition(canvas_vanet);

            MenuItem item = sender as MenuItem;
            string itemString = item.Header.ToString();
            switch (itemString)
            {

                case "_Network Generator":
                    {
                        OpenTopGen();
                    }
                    break;
                case "_Two Lanes":
                    {
                        RoadSegment rs = new RoadSegment(this, 2, RoadOrientation.Horizontal);
                        rs.Height = rs.LanesCount * PublicParamerters.LaneWidth + 1.5;
                        rs.Margin = new Thickness(po.X + 50, po.Y + 50, 0, 0);
                        canvas_vanet.Children.Add(rs);
                    }
                    break;
                case "_Four Lanes":
                    {
                        RoadSegment rs = new RoadSegment(this, 4, RoadOrientation.Horizontal);
                        rs.Height = rs.LanesCount * PublicParamerters.LaneWidth + 1.5;
                        rs.Margin = new Thickness(po.X + 50, po.Y + 50, 0, 0);
                        canvas_vanet.Children.Add(rs);
                    }
                    break;
                case "_Six Lanes":
                    {
                        RoadSegment rs = new RoadSegment(this, 6, RoadOrientation.Horizontal);
                        rs.Height = rs.LanesCount * PublicParamerters.LaneWidth + 1.5;
                        rs.Margin = new Thickness(po.X + 50, po.Y + 50, 0, 0);
                        canvas_vanet.Children.Add(rs);
                    }
                    break;
                case "_Add Junction":
                    Junction jun = new Junction(this);
                    jun.Margin = new Thickness(po.X + 50, po.Y + 50, 0, 0);
                    canvas_vanet.Children.Add(jun);
                    break;
                case "_Import Vanet":
                    UiImportTopology uim = new db.UiImportTopology(this);
                    uim.Show();
                    break;
                case "_Export Vanet":
                    UiExportTopology win = new UiExportTopology(this);
                    win.Show();
                    break;
                case "_Clear":
                    {
                        Clear();
                    }
                    break;
                
            }
        }



        public void Clear()
        {
            Dispatcher.Invoke((Action)delegate
               {
                   foreach (RoadSegment s in MyRoadSegments)
                   {
                       s.stopGeneratingVehicles();
                   }
                   Settings.Default.IsIntialized = false; // re-intialize
                   canvas_vanet.Children.Clear();
                   MyJunctions.Clear();
                   MyRoadSegments.Clear();
                   MyVehicles.Clear();
                   PublicStatistics.Clear();
               });
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double sctim = StreenTimes / 10;
            double x = _slider.Value;
            if (x <= sctim)
            {
                x = sctim;
                Settings.Default.SliderValue = x;
                Settings.Default.Save();
            }
            var scaler = canvas_vanet.LayoutTransform as ScaleTransform;
            canvas_vanet.LayoutTransform = new ScaleTransform(x, x, SystemParameters.FullPrimaryScreenWidth / 2, SystemParameters.FullPrimaryScreenHeight / 2);
            lbl_zome_percentage.Text = (x * 100).ToString() + "%";


            Settings.Default.SliderValue = x;
            Settings.Default.Save();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _slider.Value = Settings.Default.SliderValue;
        }

        public void DisplayInfo()
        {
            try
            {
                PublicStatistics.LiveStatstics.lbl_number_of_junctions.Content = MyJunctions.Count;
                PublicStatistics.LiveStatstics.lbl_number_of_road_segments.Content = MyRoadSegments.Count;
                PublicStatistics.LiveStatstics.lbl_max_speed.Content = Settings.Default.MaxSpeed + "Kmh";
                PublicStatistics.LiveStatstics.lbl_min_speed.Content = Settings.Default.MinSpeed + "Kmh";
                PublicStatistics.LiveStatstics.lbl_average_speed.Content = PublicParamerters.MeanSpeed + "Kmh";
                PublicStatistics.LiveStatstics.lbl_com_range.Content = Settings.Default.CommunicationRange + "m";
                PublicStatistics.LiveStatstics.lbl_data_packet_size.Content = PublicParamerters.DataPacketLength + "bit";
                PublicStatistics.LiveStatstics.lbl_lanes_count.Content = MyRoadSegments[0].LanesCount +"/Way";
                PublicStatistics.LiveStatstics.lbl_max_retransmit_times.Content = Settings.Default.MaximumAttemps.ToString();
                PublicStatistics.LiveStatstics.lbl_max_store_times.Content = Settings.Default.MaxStoreTime.ToString();

            }
            catch
            {

            }
        }

        public void DeplayVechiles() 
        {
            if (MyRoadSegments.Count > 0 && MyJunctions.Count > 0)
            {
                
                foreach (RoadSegment rs in MyRoadSegments)
                {
                    rs.VehicleInterArrivalMean = Computations.VehicleInterArrivalMean;

                    rs.SetAsEntry();
                }

                DisplayInfo();

                Settings.Default.IsIntialized = true;
            }
            else
            {
                MessageBox.Show("Road Network should be added. Road Network>Import Vanet","Error.");
            }
        }

        private void btn_change_packet_rate(object sender, RoutedEventArgs e)
        {
            // send packets options.
            MenuItem item = sender as MenuItem;
            string Header = item.Header.ToString();
            if (Settings.Default.IsIntialized)
            {
                switch (Header)
                {
                    case "1pck/0": // stop.
                        packetRateGenerator.ChangePacketRange(0);
                        break;
                    case "1pck/1ms": // stop.
                        packetRateGenerator.ChangePacketRange(0.001); // packet per ms.
                        break;
                    case "pck/0.5s": // packet per half second.
                        packetRateGenerator.ChangePacketRange(0.5);
                        break;
                    case "1pck/1s":
                        packetRateGenerator.ChangePacketRange(1);
                        break;
                    case "1pck/3s":
                        packetRateGenerator.ChangePacketRange(2);
                        break;
                    case "1pck/5s":
                        packetRateGenerator.ChangePacketRange(2);
                        break;
                    case "1pck/10s":
                        packetRateGenerator.ChangePacketRange(4);
                        break;
                    case "1pck/15s":
                        packetRateGenerator.ChangePacketRange(6);
                        break;
                    case "1pck/20s":
                        packetRateGenerator.ChangePacketRange(8);
                        break;
                    case "1pck/30s":
                        packetRateGenerator.ChangePacketRange(10);
                        break;

                }
            }
        }

        private void btn_set_number_of_vechiles(object sender, RoutedEventArgs e)
        {
            // send packets options.
            MenuItem item = sender as MenuItem;
            string Header = item.Header.ToString();
            switch (Header)
            {
                case "Unlimited":
                    Settings.Default.IsSetMaxVehicles = false;
                    DeplayVechiles();
                    break;
                case "1":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 1;
                    DeplayVechiles();
                    break;
                case "5":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 5;
                    DeplayVechiles();
                    break;
                case "10":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 10;
                    DeplayVechiles();
                    break;
                case "20":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 20;
                    DeplayVechiles();
                    break;
                case "30":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 30;
                    DeplayVechiles();
                    break;
                case "50":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 50;
                    DeplayVechiles();
                    break;
                case "80":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 80;
                    DeplayVechiles();
                    break;
                case "100":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 100;
                    DeplayVechiles();
                    break;
                case "150":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 150;
                    DeplayVechiles();
                    break;
                case "200":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 200;
                    DeplayVechiles();
                    break;
                case "250":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 250;
                    DeplayVechiles();
                    break;
                case "300":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 300;
                    DeplayVechiles();
                    break;
                case "350":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 350;
                    DeplayVechiles();
                    break;
                case "400":
                    Settings.Default.IsSetMaxVehicles = true;
                    Settings.Default.MaxNumberOfVehicles = 400;
                    DeplayVechiles();
                    break;
            }
        }



        private void btn_generate_x_of_packets(object sender, RoutedEventArgs e)
        {
            // send packets options.
            MenuItem item = sender as MenuItem;
            string Header = item.Header.ToString();
            if (Settings.Default.IsIntialized)
            {
                switch (Header)
                {
                    case "1": // stop.
                        packetRateGenerator.GenerateXofPackets(1);
                        break;
                    case "3": // stop.
                        packetRateGenerator.GenerateXofPackets(3);
                        break;
                    case "10":
                        packetRateGenerator.GenerateXofPackets(10);
                        break;
                    case "100":
                        packetRateGenerator.GenerateXofPackets(100);
                        break;
                    case "300":
                        packetRateGenerator.GenerateXofPackets(300);
                        break;
                    case "500":
                        packetRateGenerator.GenerateXofPackets(500);
                        break;
                    case "1000":
                        packetRateGenerator.GenerateXofPackets(1000);
                        break;
                    case "1500":
                        packetRateGenerator.GenerateXofPackets(1500);
                        break;
                    case "2000":
                        packetRateGenerator.GenerateXofPackets(2000);
                        break;
                    case "3000":
                        packetRateGenerator.GenerateXofPackets(3000);
                        break;
                    case "5000":
                        packetRateGenerator.GenerateXofPackets(5000);
                        break;
                    case "10000":
                        packetRateGenerator.GenerateXofPackets(10000);
                        break;

                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                MessageBoxResult ms =  MessageBox.Show("Sure?", "Vanet", MessageBoxButton.YesNo);
                if (ms == MessageBoxResult.Yes)
                {
                    PublicStatistics.LiveStatstics.IsCloseUable = true;
                    uIsetExperment.isCloseUpbale = true;
                    PublicStatistics.LiveStatstics.Close();
                    uIsetExperment.Close();
                    uiNetGen.Close();
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch
            {

            }
        }

        private void btn_show_results(object sender, RoutedEventArgs e)
        {
            try
            {
                // send packets options.
                MenuItem item = sender as MenuItem;
                string Header = item.Header.ToString();
                if (Settings.Default.IsIntialized)
                {
                    switch (Header)
                    {
                        
                        case "_Show Results Details": // stop.
                            {
                                List<object> List = new List<object>();
                                List.AddRange(PublicStatistics.DeleiverdPacketsList);
                                List.AddRange(PublicStatistics.DropedPacketsList);
                                if (List.Count > 0)
                                {
                                    UiShowResults sh = new UiShowResults(List);
                                    sh.Title = "Details Results";
                                    sh.Show();
                                }
                                else
                                {
                                    MessageBox.Show("No Results Found!");
                                }
                                break;
                            }
                        case "_Print Results": // stop.
                            {
                                List<object> List = new List<object>();
                                List.AddRange(PublicStatistics.PrintResults(this));
                                if (List.Count > 0)
                                {
                                    UiShowResults sh = new UiShowResults(List);
                                    sh.Title = "Final Results";
                                    sh.Show();
                                }
                                else
                                {
                                    MessageBox.Show("No Results Found!");
                                }
                                break;
                            }
                        case "_Show Live Results":
                            {
                                try
                                {
                                    PublicStatistics.LiveStatstics.Show();
                                }
                                catch
                                {
                                    PublicStatistics.LiveStatstics = new UiLiveStatstics();
                                    PublicStatistics.LiveStatstics.Show();
                                }
                                break;
                            }
                    }
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message, "MianWindow-btn_show_results");
            }
        }

        private void btn_show_v_info(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string Header = item.Name.ToString();
            if (Settings.Default.IsIntialized)
            {
                switch (Header)
                {
                    //op_none
                    case "op_none": // stop.
                       
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.None.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_VID": // stop.
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.VID;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.VID.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_SpeedKMH": // stop.
                       // PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.SpeedKMH;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.SpeedKMH.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_SpeedTimer":
                       // PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.SpeedTimer;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.SpeedTimer.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_RID":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.RID;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.RID.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_SJID":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.SJID;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.SJID.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_DJID":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.DJID;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.DJID.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_LaneIndex":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.LaneIndex;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.LaneIndex.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_InstanceLocation":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.InstanceLocation;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.InstanceLocation.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_RemianDistanceToHeadingJunction":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.RemianDistanceToHeadingJunction;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.RemianDistanceToHeadingJunction.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_TravelledDistanceInMeter":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.TravelledDistanceInMeter;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.TravelledDistanceInMeter.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_ExceededDistanceInMeter":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.ExceededDistanceInMeter;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.ExceededDistanceInMeter.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_PacketsQueueLength":
                     //   PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.PacketsQueueLength;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.PacketsQueueLength.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_JunctionQueueIndex":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.JunctionQueueIndex;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.JunctionQueueIndex.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_RIDpluseLaneIndex":
                      //  PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.RIDpluseLaneIndex;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.RIDpluseLaneIndex.ToString();
                        Settings.Default.Save();
                        break;
                    //SwitchDirection, Direction

                    case "op_SwitchDirection":
                     //   PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.SwitchDirection;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.SwitchDirection.ToString();
                        Settings.Default.Save();
                        break;
                    case "op_Direction":
                     //   PublicParamerters.DisplayInfoFlag = VehicleUi.Vehicleinfo.Direction;
                        Settings.Default.DisplayInfoFlag = VehicleUi.Vehicleinfo.Direction.ToString();
                        Settings.Default.Save();
                        break;
                }
            }
        }

        private void Btn_experment_Click(object sender, RoutedEventArgs e)
        {
            if (!Settings.Default.IsIntialized)
            {
                
                uIsetExperment.WindowState = WindowState.Normal;
                uIsetExperment.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                uIsetExperment.Show();
            }
            else
            {
                MessageBox.Show("Exp is running.");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UiFuzzyDemo d = new UiFuzzyDemo();
            d.Show();
        }

        private void Btn_crisptest_Click(object sender, RoutedEventArgs e)
        {
            new UiCrisp().Show(); 
        }
    }
}
