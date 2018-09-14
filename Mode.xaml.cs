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
    /// Interaction logic for Mode.xaml
    /// </summary>
    public partial class Mode : Window
    {
        

        public Mode()
        {
            InitializeComponent();
        }



        private void Add_pose_btn(object sender, RoutedEventArgs e)
        {
            Add_pose add_pose = new Add_pose();
            add_pose.Show();
            this.Close();
        }
    }
}
