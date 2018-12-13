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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            getClassRoom();
        }

        private void getClassRoom()
        {
            ClassRoomUC classRoomUC = new ClassRoomUC();
            controlArea.Children.Add(classRoomUC);
        }

        private void logoutClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void classroomClick(object sender, RoutedEventArgs e)
        {
            controlArea.Children.Clear();
            getClassRoom();
        }

        private void profileClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("profile");
        }
    }
}
