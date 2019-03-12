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

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

using Microsoft.Kinect;

using MuayThaiTraining.Model;
using System.Drawing;
using System.Windows.Interop;
using System.Data.OleDb;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for ScoreUC.xaml
    /// </summary>
    public partial class ScoreUC : UserControl
    {
        Comparison comparison = new Comparison();
        Position position = new Position();
        DTW dtw = new DTW();
        List<Tuple<int, int, double, int>> path;
        string poseName;
        string room;
        List<double> minscore = new List<double>();
        string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainer");
        string path2 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");
        List<BodyJoint> motion;
        VideoWriter videoWriter;
        Image<Bgr, byte> img;
        VideoCapture videoCapture;
        VideoCapture videoCapture1;
        double TotalFrame;
        double Fps;
        int FrameNo;

        double TotalFrame1;
        double Fps1;
        int FrameNo1;
        int frameTrainee;
        int frameTrainer;


        double score;
        string content = "IMPROVE YOUR :";
        string text;
        bool IsReadingFrame;
        bool IsReadingFrame1;
        string suggest;
        int col;
        


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
                //getPicture();
                int len = motion.Count();
                trainerImage.Source = new BitmapImage(new Uri(path1 + "\\" + poseName.Replace(' ', '_') + ".png"));
                traineeImage.Source = new BitmapImage(new Uri(path2 + "\\" + poseName.Replace(' ', '_') + ".png"));
                score = comparison.calScore(motion[len-1].Skel, poseName, room, 1).Item2*10;
                suggest = getSuggestion(comparison.calScore(motion[len - 1].Skel, poseName, room, 1).Item1);
                if (!String.IsNullOrEmpty(suggest))
                {
                    Content += suggest;
                }
            }
            else
            {
                //int frame1 = minScore().Item1;
                //int frame2 = minScore().Item2;
                videoCapture = new VideoCapture(path1 + "\\" + poseName.Replace(' ', '_').ToString() + ".mp4");
                Mat m = new Mat();
                videoCapture.Read(m);
                trainerImage.Source = ImageSourceForBitmap(m.Bitmap);
                TotalFrame = videoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = videoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                //traineeImage.Source = new BitmapImage(new Uri(path2 + "\\" + poseName.Replace(' ', '_') + ".png"));

                videoCapture1 = new VideoCapture(path2 + "\\" + poseName.Replace(' ', '_').ToString() + ".mp4");
                Mat m1 = new Mat();
                videoCapture1.Read(m1);
                traineeImage.Source = ImageSourceForBitmap(m1.Bitmap);
                TotalFrame1 = videoCapture1.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps1 = videoCapture1.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                //score = comparison.calScore(motion[frame2].Skel, poseName, room, frame1).Item2;
                score = multiThread(poseName, classRoom, motion);
                suggest = getSuggestion(comparison.calScore(motion[frameTrainee].Skel, poseName, room, frameTrainer).Item1);
                if (!String.IsNullOrEmpty(suggest))
                {
                    Content += suggest;
                    Content += " on frame " + frameTrainer + " : " + frameTrainee;
                }
                
            }
            if (String.IsNullOrEmpty(suggest))
            {
                Content = "Great Job!";
            }
            scoreLB.Content = score.ToString("0.00");
            suggestlbl.Content = content;
            string name = poseName.Replace(' ', '_');
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

        private string getSuggestion(List<Tuple<string, double>> tuples)
        {
            foreach(var i in tuples)
            {
                if(i.Item2 < 0.8)
                {
                    text += " " + i.Item1;
                }
            }
            return text;
        }

        public ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        //private void getPicture()
        //{
        //    this.trainerImage.Source = new BitmapImage(new Uri(path1 + "\\" + poseName.Replace(' ', '_') + ".png"));
        //    this.traineeImage.Source = new BitmapImage(new Uri(path2 + "\\" + poseName.Replace(' ', '_') + ".png"));
        //    double score = comparison.calScore(motion[frame2].Skel, poseName, room, frame1);
        //}


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
        

        private double multiThread(string poseName, string classRoom, List<BodyJoint> motion)
        {
            col = position.lenghtFrame(poseName, classRoom);

            List<Tuple<double[,], double[,]>> result1 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result2 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result3 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result4 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result7 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result8 = new List<Tuple<double[,], double[,]>>();
            List<Tuple<double[,], double[,]>> result9 = new List<Tuple<double[,], double[,]>>();

            //2
            List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft };

            //2
            //List<JointType> legLeft2 = new List<JointType> { JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft };

            //2
            List<JointType> legRight = new List<JointType> { JointType.HipCenter, JointType.HipRight,
            JointType.KneeRight };

            //2
            //List<JointType> legRight2 = new List<JointType> { JointType.KneeRight, JointType.AnkleRight, JointType.FootRight };

            //1
            List<JointType> handLeft = new List<JointType> { JointType.HipCenter, JointType.Spine };

            //2
            List<JointType> handLeft2 = new List<JointType> {JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft };

            //3
            List<JointType> handLeft3 = new List<JointType> { JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft };
            
            //1
            List<JointType> handRight2 = new List<JointType> { JointType.ShoulderCenter,
            JointType.ShoulderRight };

            //3
            List<JointType> handRight3 = new List<JointType> { JointType.ShoulderRight ,JointType.ElbowRight, JointType.WristRight, JointType.HandRight };


            //List<List<JointType>> listsJoint = new List<List<JointType>> { legLeft, legRight, handLeft};

 


            Task thread1 = Task.Factory.StartNew(() =>
            {
                result1 = task1(legLeft, motion, poseName, classRoom);
            });
            //Task thread5 = Task.Factory.StartNew(() =>
            //{
            //    result5 = task1(legLeft2, motion, poseName, classRoom);
            //});



            Task thread2 = Task.Factory.StartNew(() =>
            {
                result2 = task1(legRight, motion, poseName, classRoom);
            });

            //Task thread6 = Task.Factory.StartNew(() =>
            //{
            //    //Some work...
            //    result6 = task1(legRight2, motion, poseName, classRoom);
            //});



            Task thread3 = Task.Factory.StartNew(() =>
            {
                //Some work...
                result3 = task1(handLeft, motion, poseName, classRoom);
            });

            Task thread7 = Task.Factory.StartNew(() =>
            {
                //Some work...
                result7 = task1(handLeft2, motion, poseName, classRoom);
            });

            Task thread8 = Task.Factory.StartNew(() =>
            {
                //Some work...
                result8 = task1(handLeft3, motion, poseName, classRoom);
            });



            Task thread4 = Task.Factory.StartNew(() =>
            {
                //Some work...
                result4 = task1(handRight3, motion, poseName, classRoom);
            });

            Task thread9 = Task.Factory.StartNew(() =>
            {
                //Some work...
                result9 = task1(handRight2, motion, poseName, classRoom);
            });



            Task.WaitAll(thread1, thread2, thread3, thread4, thread7, thread8, thread9);

            //Task.WaitAll(thread1);

            List<List<Tuple<double[,], double[,]>>> result = new List<List<Tuple<double[,], double[,]>>>
            { result1, result2, result3, result4, result7, result8, result9};
            //{ result1};



            return writeConsole(result, motion.Count, col);

        }

        public List<Tuple<double[,], double[,]>> task1(List<JointType> joint, List<BodyJoint> motion, string pose, string room)
        {

            List<Tuple<double[,], double[,]>> result = new List<Tuple<double[,], double[,]>>();
            
      
            for (int j = 0; j < joint.Count - 1; j++)
            {
                Tuple<double[,], double[,]> a = dtw.modifiedDTW(joint[j], joint[j + 1], pose, room,motion, col);
                result.Add(a);
            }

            return result;
        }


        public double writeConsole(List<List<Tuple<double[,], double[,]>>> result, int rows, int columns)
        {

            double[,] table = new double[rows, columns];
            double[,] score = new double[rows, columns];

            foreach (var i in result)
            {
                foreach (var j in i)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < columns; c++)
                        {
                            table[r, c] += j.Item1[r, c];
                            score[r, c] += j.Item2[r, c];
                        }
                    }
                }
            }


            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    table[r, c] /= 18;
                    score[r, c] /= 18;
                }
            }


            //Get DTW frame 0-3 
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.WriteLine("Dtw [" + i + "]" + "[" + j + "] = " + table[i, j]);
                }
            }


            //wrap path
            List<Tuple<int, int, double, int>> wrapPath = dtw.wrapPath(table, score, Convert.ToInt32(rows) - 1, Convert.ToInt32(columns) - 1);

            double sum = 0;

            List<double> minscore = new List<double>();
            List<double> allscore = new List<double>();
            int round = 0;

            foreach (Tuple<int, int, double, int> i in wrapPath)
            {
                if (!Double.IsNaN(i.Item3))
                {
                    sum += i.Item3;
                    round++;
                    minscore.Add(i.Item3);
                }
                allscore.Add(i.Item3);
                Console.WriteLine(i.Item1 + " " + i.Item2 + " " + i.Item3);
            }

            Console.WriteLine("Score : " + sum / round);

            double min = minscore.Min();
            int index = allscore.IndexOf(min);

            //suggest = wrapPath[index].Item1 + " " + wrapPath[index].Item2;
            frameTrainee = wrapPath[index].Item1;
            frameTrainer = wrapPath[index].Item2;
            Console.WriteLine(wrapPath[index].Item1 + " " + wrapPath[index].Item2 + " " + wrapPath[index].Item3);

            return sum / round;
        }

        private void trainerImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (videoCapture == null)
            {
                return;
            }
            IsReadingFrame = true;
            ReadAllFrames();
        }

        private void traineeImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (videoCapture1 == null)
            {
                return;
            }
            IsReadingFrame1 = true;
            ReadAllFrames1();
        }

        private async void ReadAllFrames()
        {

            Mat m = new Mat();
            while (IsReadingFrame == true && FrameNo < TotalFrame - 1)
            {
                FrameNo += 1;
                videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameNo);
                videoCapture.Read(m);
                trainerImage.Source = ImageSourceForBitmap(m.Bitmap);
                await Task.Delay(1000 / Convert.ToInt16(Fps));
            }
            FrameNo = 0;
        }

        private async void ReadAllFrames1()
        {

            Mat m1 = new Mat();
            while (IsReadingFrame1 == true && FrameNo1 < TotalFrame1 - 1)
            {
                FrameNo1 += 1;
                videoCapture1.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameNo1);
                videoCapture1.Read(m1);
                traineeImage.Source = ImageSourceForBitmap(m1.Bitmap);
                await Task.Delay(1000 / Convert.ToInt16(Fps1));
            }
            FrameNo1 = 0;
        }
    }
}
