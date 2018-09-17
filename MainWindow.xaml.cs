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



namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
        }
        
         
        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            Tutorial tutorial = new Tutorial();
            tutorial.Show();
            this.Close();
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            Mode mode = new Mode();
            mode.Show();
            this.Close();
        }
    }
}
