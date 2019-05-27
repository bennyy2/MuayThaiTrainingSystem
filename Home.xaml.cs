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
        Position position = new Position();
        User user = new User();
        public Home()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }
        private void btnLogin(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            //ShowClassRoom showClassRoom = new ShowClassRoom();
            //showClassRoom.Show();
            this.Close();
            //if(user.checkUser(username.Text, pass.Text))
            //{
            //    Home home = new Home();
            //    home.Show();
            //    //ShowClassRoom showClassRoom = new ShowClassRoom();
            //    //showClassRoom.Show();
            //    this.Close();
            //}
            //else
            //{
            //    error.Content = "Username or password is incorrect";
            //}

        }
        private void regis(object sender, RoutedEventArgs e)
        {
            RegisterUC registerUC = new RegisterUC();
            mainArea.Children.Clear();
            mainArea.Children.Add(registerUC);
        }
        

        
    }
}
