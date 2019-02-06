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
        List<Tuple<int, int, double, int>> path;
        string poseName;
        string room;
        List<double> minscore = new List<double>();
        string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainer");
        string path2 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");
        List<BodyJoint> motion;


        //public ScoreUC(double score, string poseName, string classRoom, string type, List<Tuple<int, int, double, int>> path, List<BodyJoint> motion)
        public ScoreUC(string poseName, string classRoom, string type, List<BodyJoint> motion)
        {
            InitializeComponent();
            this.poseName = poseName;
            this.room = classRoom;
            //this.path = path;
            this.poseLB.Content = poseName.ToString();
            //this.scoreLB.Content = score.ToString("0.00");
            if (type == "Pose")
            {
                getPicture();
            }
            else
            {
                //int frame1 = minScore().Item1;
                //int frame2 = minScore().Item2;

                this.trainerImage.Source = new BitmapImage(new Uri(path1 + "\\" + poseName.Replace(' ', '_') +"7.png"));
                this.traineeImage.Source = new BitmapImage(new Uri(path2 + "\\" + poseName.Replace(' ', '_') +"10.png"));
                //double score = comparison.calScore(motion[frame2].Skel, poseName, room, frame1);

            }
        }

        private void replayClick(object sender, RoutedEventArgs e)
        {
            ComparePoseUC comparePoseUC = new ComparePoseUC(poseName, room);
            scorePanel.Children.Clear();
            scorePanel.Children.Add(comparePoseUC);
        }


        private void backBtnClick(object sender, RoutedEventArgs e)
        {
            LearningPoseUC learningPoseUC = new LearningPoseUC(room);
            scorePanel.Children.Clear();
            scorePanel.Children.Add(learningPoseUC);
        }

        private void getPicture()
        {

        }


        private Tuple<int,int> minScore()
        {
            foreach (Tuple<int, int, double, int> i in path)
            {
                minscore.Add(i.Item3);
            }

            double min = minscore.Min();
            int index = minscore.IndexOf(min);

            Console.WriteLine(path[index].Item1 + " " + path[index].Item2 + " " + path[index].Item3);
            
            return new Tuple<int, int>(path[index].Item1, path[index].Item2);
        }







    }
}
