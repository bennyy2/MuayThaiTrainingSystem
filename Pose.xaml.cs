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
using System.Windows.Shapes;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for Pose.xaml
    /// </summary>
    public partial class Pose : Window
    {
        public Pose()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void add_button(object sender, RoutedEventArgs e)
        {
            Add_pose add_Pose = new Add_pose();
            add_Pose.Show();
            this.Close();
        }
    }
}
