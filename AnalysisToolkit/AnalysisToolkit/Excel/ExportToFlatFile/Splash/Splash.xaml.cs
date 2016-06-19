using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalysisToolkit.Excel.ExportToFlatFile.Splash
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public delegate void OnCancelHandler(object source, EventArgs e);

        public event OnCancelHandler OnCancel;

        public Splash()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString().ToLower() == "cancel" && OnCancel != null)
            {
                OnCancel(sender, new EventArgs());
            }
            else
            {
                this.Close();
            }
        }
    }
}
