using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VSIM_VEFR.FuzzySets;
using VSIM_VEFR.Operations;

namespace VSIM_VEFR.Demo
{
    /// <summary>
    /// Interaction logic for UiCrisp.xaml
    /// </summary>
    public partial class UiCrisp : Window
    {
        public class SpeedDifferenceRow
        {
            public int ID { get; set; }
            public double  Si { get; set; }
            public double Sj { get; set; } 
            public double dif { get { return Math.Abs(Si - Sj); } }
            public double Crisp { get; set; }

            public string Item
            {
                get
                {
                    RVSInput xdis = new RVSInput();
                    xdis.SpeedDifferenceCrisp = Crisp;
                    return xdis.SpeedDifference.ToString();
                }
            }
        }
        public class TransmissionDistanceRow
        {
            public int ID { get; set; }
            public Point iloc { get; set; }
            public Point jloc { get; set; }
            public double Dis { get { return Computations.Distance(iloc, jloc); } }
            public double Crisp { get; set; }
            public string Item
            {
                get
                {
                    RVSInput xdis = new RVSInput();
                    xdis.TransmissionDistanceCrisp = Crisp;
                    return xdis.TransmissionDistance.ToString();
                }
            }
        }
        public class MovingDirectionRow
        {
            public int ID { get; set; }
            public Point dloc { get; set; }  
            public Point iloc { get; set; } 
            public Point jloc { get; set; } 
            public double dj { get { return Computations.Distance(dloc, jloc); } }
            public double id { get { return Computations.Distance(dloc, iloc); } }
            public double Dif { get { return id - dj;  } }
            public double Crisp { get; set; }

            public string Item
            {
                get
                {
                    RVSInput xdis = new RVSInput();
                    xdis.MovingDirectionCrisp = Crisp;
                    return xdis.MovingDirection.ToString();
                }
            }
        }

        public class ValidDistanceRow
        {
            public int ID { get; set; }
            public Point dloc { get; set; }
            public Point iloc { get; set; }
            public Point jloc { get; set; } 
            public double dj { get { return Computations.Distance(dloc, jloc); } }
            public double id { get { return Computations.Distance(dloc, iloc); } }
            public double Dif { get { return id - dj; } }
            public double Crisp { get; set; }

            public string Item
            {
                get
                {
                    RSSInput xdis = new RSSInput();
                    xdis.ValidDistanceCrisp = Crisp;
                    return xdis.ValidDistance.ToString();
                }
            }
        }

        public class LengthRow 
        {
            public int ID { get; set; }
            public Point iloc { get; set; }
            public Point jloc { get; set; }
            public double Dis { get { return Computations.Distance(jloc, iloc); } }
            public double Crisp { get; set; } 
        }

        public UiCrisp()
        {
            InitializeComponent();
        }

        private void Combo_crisp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string var = (combo_crisp.SelectedItem as ComboBoxItem).Content.ToString();
            if (var == "Density")
            {



            }
            else if (var == "Valid Distance")
            {
                List<ValidDistanceRow> List = new List<ValidDistanceRow>();
                Point dloc = new Point(100, 100);
                for (int j = 0; j <= 50; j++)
                {
                    Point iloc = new Point(RandomeNumberGenerator.GetUniform(0, 100), RandomeNumberGenerator.GetUniform(0, 100));
                    Point jloc = new Point(RandomeNumberGenerator.GetUniform(0, 100), RandomeNumberGenerator.GetUniform(0, 100));
                    double _Crisp = Crisps.ValidDistance(iloc, jloc, dloc);
                    List.Add(new ValidDistanceRow() { Crisp = _Crisp, ID = j, iloc = iloc, jloc = jloc, dloc = dloc });
                }
                dg.ItemsSource = List;
            }
            else if (var == "Transmission Distance")
            {
                List<TransmissionDistanceRow> List = new List<TransmissionDistanceRow>();
                
                double comrange = 100;
                for (int j = 0; j <= 1000; j++)
                {
                    Point iloc = new Point(RandomeNumberGenerator.GetUniform(0, 100), RandomeNumberGenerator.GetUniform(0, 100));
                    Point jloc = new Point(RandomeNumberGenerator.GetUniform(0, 100), RandomeNumberGenerator.GetUniform(0, 100));
                    double _Crisp = Crisps.TransmissionDistance(iloc, jloc, comrange);
                   
                    List.Add(new TransmissionDistanceRow() { Crisp= _Crisp, ID= j, iloc= iloc, jloc= jloc });
                }
                dg.ItemsSource = List;
            }
            else if (var == "Speed Difference")
            {

                List<SpeedDifferenceRow> List = new List<SpeedDifferenceRow>();
                double maxSpeed = 100;
                double si = 5;
                for (int j = 0; j <= 1000; j++)
                {
                    double sj = RandomeNumberGenerator.GetUniform(0, 100);
                    double _Crisp=Crisps.SpeedDifference(si, sj, maxSpeed);
                    List.Add( new SpeedDifferenceRow() { ID=j, Si= si, Sj= sj, Crisp= _Crisp } );
                }

                dg.ItemsSource = List;
            }
            else if (var == "Moving Direction")
            {
                List<MovingDirectionRow> List = new List<MovingDirectionRow>();
                Point dloc = new Point(100, 100);
                for (int j = 0; j <= 1000; j++)
                {
                    Point iloc = new Point(RandomeNumberGenerator.GetUniform(0, 500), RandomeNumberGenerator.GetUniform(0, 500));
                    Point jloc = new Point(RandomeNumberGenerator.GetUniform(0, 500), RandomeNumberGenerator.GetUniform(0, 500));
                    double _Crisp = Crisps.MovingDirection(iloc, jloc, dloc);
                    List.Add(new MovingDirectionRow() { Crisp = _Crisp, ID = j, iloc = iloc, jloc = jloc, dloc= dloc });
                }
                dg.ItemsSource = List;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(PublicParamerters.RVSInputList.Count!=0)
            {
                dg.ItemsSource = PublicParamerters.RVSInputList;
            }
        }
    }
}
