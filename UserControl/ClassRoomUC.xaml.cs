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
    /// Interaction logic for ClassRoomUC.xaml
    /// </summary>
    public partial class ClassRoomUC : UserControl
    {
        ClassRoom classRoom = new ClassRoom();

        public ClassRoomUC()
        {
            InitializeComponent();
            createClassRoomBtn();
        }



        private void createClassRoomBtn()
        {
            // Create a Button margin
            int left = 50;
            int top = 50;
            int right = 0;
            int bottom = 0;

            List<ClassRoom> list = classRoom.getClassRoom();
            foreach (var i in list)
            {
                Button btn = new Button();

                // Set Button properties
                btn.Height = 100;
                btn.Width = 100;
                btn.Margin = new Thickness(left, top, right, bottom);
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.VerticalAlignment = VerticalAlignment.Top;
                btn.Content = i.ClassName;
                btn.Name = i.ClassName.Replace(' ', '_');
                //System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
                //if (x + 200 > workingRectangle.Width)
                //{
                //    y += 170;
                //    x = 20;
                //}

                left += 200;

                // Add a Button Click Event handler
                btn.Click += classRoomClick;

                // Add Button to the Form
                classpanel.Children.Add(btn);
            }
        }

        private void classRoomClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            LearningPoseUC learningPoseUC = new LearningPoseUC(btn.Name.ToString());
            classpanel.Children.Clear();
            classpanel.Children.Add(learningPoseUC);

        }

        private void addClassRoomClick(object sender, RoutedEventArgs e)
        {
            addClassRoomUC addClassRoomUC = new addClassRoomUC();
            classpanel.Children.Clear();
            classpanel.Children.Add(addClassRoomUC);

        }

    }
}
