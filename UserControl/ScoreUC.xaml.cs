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

using Microsoft.Kinect;

using MuayThaiTraining.Model;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for ScoreUC.xaml
    /// </summary>
    public partial class ScoreUC : UserControl
    {
        Comparison comparison = new Comparison();

        string poseName;
        string room;


        public ScoreUC(Skeleton skel, string poseName, string classRoom, int frame)
        {
            InitializeComponent();
            double score = comparison.calScore(skel, poseName, classRoom, frame);
            this.poseName = poseName;
            this.room = classRoom;
            this.scoreLB.Content = score.ToString("0.00");
        }

        private void replayClick(object sender, RoutedEventArgs e)
        {
            ComparePoseUC comparePoseUC = new ComparePoseUC(poseName, room);
            scorePanel.Children.Clear();
            scorePanel.Children.Add(comparePoseUC);
        }



    }
}
