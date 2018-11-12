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

using MuayThaiTraining.Model;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for LearningPoseUC.xaml
    /// </summary>
    public partial class LearningPoseUC : UserControl
    {

        Pose pose = new Pose();
        ClassRoom classRoom = new ClassRoom();
        String room;

        public LearningPoseUC(String room)
        {
            InitializeComponent();
            this.room = room;
            createPoseBtn(room);
           
        }

        private void createPoseBtn(String room)
        {
            // Create a Button margin
            int left = 50;
            int top = 50;
            int right = 0;
            int bottom = 0;

            List<Pose> listpose = pose.getPose(room);
            
            foreach (var i in listpose)
            {
                Button btn = new Button();

                // Set Button properties
                btn.Height = 60;
                btn.Width = 200;
                btn.Margin = new Thickness(left, top, right, bottom);
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.VerticalAlignment = VerticalAlignment.Top;
                btn.Content = i.PoseName;
                btn.Name = i.PoseName.Replace(' ', '_');
                //btn.Name = i.PoseID.ToString();
                top += 100;

                // Add a Button Click Event handler
                btn.Click += poseClick;

                // Add Button to the Form
                posepanel.Children.Add(btn);
            }
        }

        private void poseClick(object sender, RoutedEventArgs e)
        {

            //Button btn = (Button)sender;
            //LearningPoseUC learningPoseUC = new LearningPoseUC(btn.Content.ToString());
            //classpanel.Children.Clear();
            //classpanel.Children.Add(learningPoseUC);
            Button btn = (Button)sender;
            ComparePoseUC comparePoseUC = new ComparePoseUC(btn.Content.ToString(), room);
            posepanel.Children.Clear();
            posepanel.Children.Add(comparePoseUC);
            
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            AddPoseUC addPoseUC = new AddPoseUC(room);
            posepanel.Children.Clear();
            posepanel.Children.Add(addPoseUC);

        }
    }
}
