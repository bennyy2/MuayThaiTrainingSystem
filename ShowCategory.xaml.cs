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
    /// Interaction logic for ShowCategory.xaml
    /// </summary>
    public partial class ShowCategory : Window
    {
        ConnectDB connectDB = new ConnectDB();

        public ShowCategory()
        {
            InitializeComponent();
        }

        private void getMode()
        {
            List<String> mode = new List<string>();
            //mode = connectDB.getListMode();

            foreach(var item in mode)
            {
                Console.WriteLine(item);
            }
        }

        private Button createButton()
        {
            Button btn = new Button();
            return btn;
        }


        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            Tutorial tutorial = new Tutorial();
            tutorial.Show();
            this.Close();
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            Pose pose = new Pose();
            pose.Show();
            this.Close();
        }
    }
}
