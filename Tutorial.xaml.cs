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

using Microsoft.Kinect;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for Tutorial.xaml
    /// </summary>
    public partial class Tutorial : Window
    {
        ConnectDB connect = new ConnectDB();
        Vector vector = new Vector();
        public Tutorial()
        {
            InitializeComponent();
            //vector = connect.getJointPosition(JointType.AnkleLeft);
            //Console.WriteLine(vector.X);
            foreach (var a in connect.getJointPosition(JointType.AnkleLeft))
            {
                Console.WriteLine(a);
            }
            
            

        }





    }
}
