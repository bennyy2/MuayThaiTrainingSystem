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
    /// Interaction logic for addClassRoomUC.xaml
    /// </summary>
    public partial class addClassRoomUC : UserControl
    {
        ClassRoom classRoom = new ClassRoom();
        public addClassRoomUC()
        {
            InitializeComponent();
        }

        private void addClassRoomClick(object sender, RoutedEventArgs e)
        {
            if (classRoom.addClass(classRoomtxt.Text.ToString()))
            {
                ClassRoomUC classRoom = new ClassRoomUC();
                addpanel.Children.Clear();
                addpanel.Children.Add(classRoom);
            }
            else
            {
                statuslb.Content = "Cannot add classroom.";
            }

        }
    }
}
