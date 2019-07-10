using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VSIM_VEFR.Operations;
using VSIM_VEFR.RoadNet.Components;

namespace VSIM_VEFR.db
{
    /// <summary>
    /// Interaction logic for NetwokImport.xaml
    /// </summary>
    public partial class NetwokImport : UserControl
    {
        public MainWindow _MainWindow { set; get; }


        public UiImportTopology UiImportTopology { get; set; }
        public NetwokImport()
        {
            InitializeComponent();
        }


        private void brn_import_Click(object sender, RoutedEventArgs e)
        {
            PublicParamerters.NetworkName = lbl_network_name.Content.ToString();
            DeserilizeNetwork.DesrlizeNetwork(_MainWindow, lbl_network_name.Content.ToString());
            try
            {
                UiImportTopology.Close();
            }
            catch
            {

            }
        }
    }
}
