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

using MuayThaiTraining.Model;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for ShowPose.xaml
    /// </summary>
    public partial class ShowPose : Window
    {
        public ShowPose(ClassRoom classRoom)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            createPoseBtn(classRoom);
        }

        private void createPoseBtn(ClassRoom classRoom)
        {
            // Create a Button margin
            int left = 50;
            int top = 180;
            int right = 0;
            int bottom = 0;

            List<ClassRoom> list = classRoom.getClassRoom();
            foreach (var i in list)
            {
                Button btn = new Button();

                
            }
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void addButton(object sender, RoutedEventArgs e)
        {
            Add_pose add_Pose = new Add_pose();
            add_Pose.Show();
            this.Close();
        }

    }
}
