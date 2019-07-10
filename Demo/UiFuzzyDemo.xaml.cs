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

namespace VSIM_VEFR.Demo
{
    /// <summary>
    /// Interaction logic for UiFuzzyDemo.xaml
    /// </summary>
    public partial class UiFuzzyDemo : Window
    {
        public UiFuzzyDemo()
        {
            InitializeComponent();
        }

        private void Combo_liguesticvar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int max = 59;
            double dm = Convert.ToDouble(max);
            string var = (combo_liguesticvar.SelectedItem as ComboBoxItem).Content.ToString();
            if (var == "Density")
            {
                chart_1.Title = "Density";
                List<KeyValuePair<double, double>> High = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Medium = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Low = new List<KeyValuePair<double, double>>(); 
                for(int i=1;i<= max;i++)
                {
                    double di = Convert.ToDouble(i);
                    double x = di / dm;
                    DensityFuzzySets fuzzy = new DensityFuzzySets(x);
                    High.Add(new KeyValuePair<double, double>(x, fuzzy.High));
                    Medium.Add(new KeyValuePair<double, double>(x, fuzzy.Medium));
                    Low.Add(new KeyValuePair<double, double>(x, fuzzy.Low));

                }

                line1.DataContext = High;
                line1.Title = "High";
                line2.DataContext = Medium;
                line2.Title = "Medium";
                line3.DataContext = Low;
                line3.Title = "Low";
                line1.Visibility = Visibility.Visible;
                line1.Visibility = Visibility.Visible;
                line3.Visibility = Visibility.Visible;


            }
            else if (var == "Valid Distance")
            {
                chart_1.Title = "Valid Distance";
                List<KeyValuePair<double, double>> Close = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Medium = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Far = new List<KeyValuePair<double, double>>();
                for (int i = 1; i <= max; i++)
                {
                    double di = Convert.ToDouble(i);
                    double x = di / dm;
                    ValidDistanceFuzzySets fuzzy = new ValidDistanceFuzzySets(x);
                    Close.Add(new KeyValuePair<double, double>(x, fuzzy.Close));
                    Medium.Add(new KeyValuePair<double, double>(x, fuzzy.Medium));
                    Far.Add(new KeyValuePair<double, double>(x, fuzzy.Far));

                }

                line1.DataContext = Close;
                line1.Title = "Close";
                line2.DataContext = Medium;
                line2.Title = "Medium";
                line3.DataContext = Far;
                line3.Title = "Far";
                line1.Visibility = Visibility.Visible;
                line1.Visibility = Visibility.Visible;
                line3.Visibility = Visibility.Visible;
            }
            else if (var == "Transmission Distance")
            {
                chart_1.Title = "Transmission Distance";
                List<KeyValuePair<double, double>> Near = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Intermediate = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Far = new List<KeyValuePair<double, double>>();
                for (int i = 1; i <= max; i++)
                {
                    double di = Convert.ToDouble(i);
                    double x = di / dm;
                    TransmissiondistanceFuzzySet fuzzy = new TransmissiondistanceFuzzySet(x);
                    Near.Add(new KeyValuePair<double, double>(x, fuzzy.Near));
                    Intermediate.Add(new KeyValuePair<double, double>(x, fuzzy.Intermediate));
                    Far.Add(new KeyValuePair<double, double>(x, fuzzy.Far));

                }

                line1.DataContext = Near;
                line1.Title = "Near";
                line2.DataContext = Intermediate;
                line2.Title = "Intermediate";
                line3.DataContext = Far;
                line3.Title = "Far";

                line1.Visibility = Visibility.Visible;
                line1.Visibility = Visibility.Visible;
                line3.Visibility = Visibility.Visible;
            }
            else if (var == "Speed Difference")
            {
                chart_1.Title = "Speed Difference";
                List<KeyValuePair<double, double>> Small = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Moderate = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Large = new List<KeyValuePair<double, double>>();
                for (int i = 1; i <= max; i++)
                {
                    double di = Convert.ToDouble(i);
                    double x = di / dm;
                    SpeedDifferenceFuzzySet  fuzzy = new SpeedDifferenceFuzzySet(x);
                    Small.Add(new KeyValuePair<double, double>(x, fuzzy.Small));
                    Moderate.Add(new KeyValuePair<double, double>(x, fuzzy.Moderate));
                    Large.Add(new KeyValuePair<double, double>(x, fuzzy.Large));

                }

                line1.DataContext = Small;
                line1.Title = "Small";
                line2.DataContext = Moderate;
                line2.Title = "Moderate";
                line3.DataContext = Large;
                line3.Title = "Large";

                line1.Visibility = Visibility.Visible;
                line1.Visibility = Visibility.Visible;
                line3.Visibility = Visibility.Visible;
            }
            else if (var == "Moving Direction")
            {
                chart_1.Title = "Moving Direction";
                List<KeyValuePair<double, double>> Front = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> Behind = new List<KeyValuePair<double, double>>();

                for (int i = 1; i <= max; i++)
                {
                    double di = Convert.ToDouble(i);
                    double x = di / dm;
                    MovingDirectionFuzzySet fuzzy = new MovingDirectionFuzzySet(x);
                    Front.Add(new KeyValuePair<double, double>(x, fuzzy.Front));
                    Behind.Add(new KeyValuePair<double, double>(x, fuzzy.Behind));

                }

                line1.DataContext = Front;
                line1.Title = "Front";
                line2.DataContext = Behind;
                line2.Title = "Behind";
                

                line1.Visibility = Visibility.Visible;
                line1.Visibility = Visibility.Visible;
                line3.Visibility = Visibility.Collapsed;
            }
        }
    }
}
