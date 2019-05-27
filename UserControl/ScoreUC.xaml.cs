using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Emgu.CV;
using Emgu.CV.Structure;

using Microsoft.Kinect;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Shapes;

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
        string poseName;
        string room;
        List<double> minscore = new List<double>();
        string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainer");
        string path2 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");
        List<Position> bodymotion;
        List<Position> trainerMotion;
        VideoWriter videoWriter;
        Image<Bgr, byte> img;
        VideoCapture videoCapture;
        VideoCapture videoCapture1;
        double TotalFrame;
        double Fps;
        int FrameNo;
        List<Tuple<int, int, double>> tuples = new List<Tuple<int, int, double>>();
        List<Tuple<int, int, double>> unsimilar = new List<Tuple<int, int, double>>();
        List<Tuple<int, int, double>> sn = new List<Tuple<int, int, double>>();

        KinectSensor ksensor;

        double TotalFrame1;
        double Fps1;
        int FrameNo1;
        int frameTrainee;
        int frameTrainer;
        int page;


        double score;
        string content = "IMPROVE YOUR :";
        string text;
        bool IsReadingFrame;
        bool IsReadingFrame1;
        string suggest;
        int col;
        


        //public ScoreUC(double score, string poseName, string classRoom, string type, List<Tuple<int, int, double, int>> path, List<BodyJoint> motion)
        public ScoreUC(string poseName, string classRoom, string type, List<Position> motion)
        {
            InitializeComponent();
            this.poseName = poseName;
            this.room = classRoom;
            //this.path = path;
            this.poseLB.Content = poseName.ToString();
            int len = motion.Count();
            //Tuple<string, List<JointType>> errorJoint = getSuggestion(comparison.calScore(motion[len - 1].Skel, poseName, room, 1).Item1);

            //this.scoreLB.Content = score.ToString("0.00");
            if (type == "Pose")
            {

                traineeImage.Source = new BitmapImage(new Uri(path2 + "\\" + poseName.Replace(' ', '_') + ".png"));
                trainerImage.Source = new BitmapImage(new Uri(path1 + "\\" + poseName.Replace(' ', '_') + ".png"));
                
                var skel = position.getMotionByFrame(classRoom, poseName);
                var body = skel.Where(x=>x.Frame == 0).ToList();
                int zindex = Canvas.GetZIndex(drawPanel);
                Canvas.SetZIndex(drawPanel, zindex + 1);

                var error = comparison.getErrorJoint(skel, motion, 0, 0);
                foreach (var i in error)
                {
                    Position point = motion.FirstOrDefault(x => x.Frame == 0 && x.Joint == i);
                    List<Position> position = motion.Where(x => x.Frame == 0).ToList();

                    DrawBone(position, JointType.ShoulderCenter, JointType.Spine);
                    DrawBone(position, JointType.Spine, JointType.HipCenter);

                    //left arm
                    DrawBone(position, JointType.ShoulderCenter, JointType.ShoulderLeft);
                    DrawBone(position, JointType.ShoulderLeft, JointType.ElbowLeft);
                    DrawBone(position, JointType.ElbowLeft, JointType.WristLeft);
                    DrawBone(position, JointType.WristLeft, JointType.HandLeft);

                    //right arm
                    DrawBone(position, JointType.ShoulderCenter, JointType.ShoulderRight);
                    DrawBone(position, JointType.ShoulderRight, JointType.ElbowRight);
                    DrawBone(position, JointType.ElbowRight, JointType.WristRight);
                    DrawBone(position, JointType.WristRight, JointType.HandRight);

                    //left leg
                    DrawBone(position, JointType.HipCenter, JointType.HipLeft);
                    DrawBone(position, JointType.HipLeft, JointType.KneeLeft);
                    //DrawBone(position, JointType.KneeLeft, JointType.AnkleLeft);
                    //DrawBone(position, JointType.AnkleLeft, JointType.FootLeft);

                    //Right leg
                    DrawBone(position, JointType.HipCenter, JointType.HipRight);
                    DrawBone(position, JointType.HipRight, JointType.KneeRight);
                    //DrawBone(position, JointType.KneeRight, JointType.AnkleRight);
                    circle(point.Imx - 5, point.Imy - 5, 10, 10);
                }

                //score = comparison.compareFrame(trainerMotion, motion, 1, 1);

                //score = comparison.calScore(motion[len - 1].Skel, poseName, room, 1).Item2 * 10;
                ////suggest = errorJoint.Item1;
                //content += suggest;
                score = comparison.compareFrame(skel, motion, 0, 0);

                scoreLB.Content = (score*10).ToString("0.00");
                //suggestlbl.Content = content;
                string name = poseName.Replace(' ', '_');

                

                if (score < 0.8)
                {
                    suggestlbl.Content = "Improve joints at the red dot";
                }
                Slidelb.Visibility = Visibility.Hidden;
                Replaybtn.Visibility = Visibility.Hidden;
                Playbtn.Visibility = Visibility.Hidden;

                //getPicture();

                //rightbtn.Visibility = Visibility.Hidden;
                //leftbtn.Visibility = Visibility.Hidden;

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


                videoCapture1 = new VideoCapture(path2 + "\\" + poseName.Replace(' ', '_').ToString() + ".mp4");
                //videoCapture1 = new VideoCapture(path1 + "\\ต่อย.mp4");
                Mat m1 = new Mat();
                videoCapture1.Read(m1);
                traineeImage.Source = ImageSourceForBitmap(m1.Bitmap);
                TotalFrame1 = videoCapture1.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps1 = videoCapture1.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                
                int zindex = Canvas.GetZIndex(drawPanel);

                Canvas.SetZIndex(drawPanel, zindex + 1);


                //Tests-------------------------------------------------------------------
                //var a = position.getMotion("ต่อย", classRoom);
                //tuples = calSimilar(poseName, room, a);
                //bodymotion = a;

                tuples = calSimilar(poseName, room, motion);
                bodymotion = motion;

                //--------------------------------------------------------------------------

                unsimilar = GetUnSimilar(tuples);
                scoreLB.Content = (CalScore(tuples)*10).ToString("0.00");

                Slidelb.Maximum = unsimilar.Count()-1;

                if (unsimilar.Count != 0)
                {
                    suggestlbl.Content = "Drag the slider bar to see the error and improve joints at the red dot";


                }
                else
                {
                    Slidelb.Visibility = Visibility.Hidden;
                }

                //if (page == 0)
                //{
                //    leftbtn.Visibility = Visibility.Hidden;
                //}

                //if(page == unsimilar.Count || page == unsimilar.Count-1)
                //{
                //    rightbtn.Visibility = Visibility.Hidden;
                //}

                //score = comparison.calScore(motion[frame2].Skel, poseName, room, frame1).Item2;
                //score = multiThread(poseName, classRoom, motion);
                //content +=  getSuggestion(comparison.calScore(motion[frameTrainee].Skel, poseName, room, frameTrainer).Item1).Item1;

                // suggest;
                //Content += " on frame " + frameTrainer + " : " + frameTrainee;
                //calSimilar(poseName, classRoom, motion);
            }

        }

        private List<Tuple<int,int,double>> GetUnSimilar(List<Tuple<int, int, double>> tuples)
        {
            List<Tuple<int, int, double>> un = new List<Tuple<int, int, double>>();
            sn = new List<Tuple<int, int, double>>();
            foreach (var i in tuples)
            {
                if (i.Item3 < 0.8)
                {
                    un.Add(new Tuple<int, int, double>(i.Item1, i.Item2, i.Item3));
                }
                else
                {
                    sn.Add(new Tuple<int, int, double>(i.Item1, i.Item2, i.Item3));
                }
            }

            return un;
        }

        private double CalScore(List<Tuple<int,int,double>> tuples)
        {
            double score = 0;

            foreach (var i in tuples)
            {
                score += i.Item3;
            }

            return score/tuples.Count;
        }

        private void DrawBone(List<Position> body, JointType joint1, JointType joint2)
        {
            
            Line backBone = new Line();
            backBone.Stroke = new SolidColorBrush(Colors.Yellow);
            backBone.StrokeThickness = 1;
            
            backBone.X1 = body.FirstOrDefault(x => x.Joint == (int)joint1).Imx;
            backBone.Y1 = body.FirstOrDefault(x => x.Joint == (int)joint1).Imy;
            backBone.X2 = body.FirstOrDefault(x => x.Joint == (int)joint2).Imx;
            backBone.Y2 = body.FirstOrDefault(x => x.Joint == (int)joint2).Imy;

            drawPanel.Children.Add(backBone);
        }

        public void circle(int x, int y, int width, int height)
        {

            System.Windows.Shapes.Ellipse circle = new System.Windows.Shapes.Ellipse()
            {
                Width = width,
                Height = height,
                Stroke = System.Windows.Media.Brushes.Red,
                StrokeThickness = 6
            };

            drawPanel.Children.Add(circle);

            circle.SetValue(Canvas.LeftProperty, (double)x);
            circle.SetValue(Canvas.TopProperty, (double)y);
        }

        private List<Tuple<int, int, double>> calSimilar(string poseName, string classRoom, List<Position> motion)
        {
            trainerMotion = position.getMotion(poseName, classRoom);
            var rows = trainerMotion.Max(x => x.Frame);
            var cols = motion.Max(x => x.Frame);

            int rowInit = 0;
            int colInit = 0;

            List<Tuple<int, int, double>> result = new List<Tuple<int, int, double>>();

            double total = 0;

            var score = comparison.compareFrame(trainerMotion, motion, rowInit, colInit);


            result.Add(new Tuple<int, int, double>(rowInit, colInit, score));

            while (rowInit != rows - 1 || colInit != cols - 1)
            {
                if (rowInit == rows - 1)
                {
                    double score4 = comparison.compareFrame(trainerMotion, motion, rowInit, colInit + 1);
                    result.Add(new Tuple<int, int, double>(rowInit, colInit + 1, score4));
                    colInit++;
                }
                else if (colInit == cols - 1)
                {
                    double score5 = comparison.compareFrame(trainerMotion, motion, rowInit + 1, colInit);
                    result.Add(new Tuple<int, int, double>(rowInit + 1, colInit, score5));
                    rowInit++;
                }
                else
                {
                    double score1 = comparison.compareFrame(trainerMotion, motion, rowInit + 1, colInit);
                    double score2 = comparison.compareFrame(trainerMotion, motion, rowInit + 1, colInit + 1);
                    double score3 = comparison.compareFrame(trainerMotion, motion, rowInit, colInit + 1);

                    if (score1 >= score2 && score1 >= score3)
                    {
                        result.Add(new Tuple<int, int, double>(rowInit + 1, colInit, score1));
                        rowInit++;
                    }
                    else if (score2 >= score1 && score2 >= score3)
                    {
                        result.Add(new Tuple<int, int, double>(rowInit + 1, colInit + 1, score2));
                        rowInit++;
                        colInit++;
                    }
                    else if (score3 >= score1 && score3 >= score2)
                    {
                        result.Add(new Tuple<int, int, double>(rowInit, colInit + 1, score3));
                        colInit++;
                    }
                }

            }

            //foreach (var i in result)
            //{
            //    Console.WriteLine(i.Item1 + " " + i.Item2 + " " + i.Item3);
            //}

            return result;
        }





        private void closeConnection()
        {
            IsReadingFrame1 = false;
            IsReadingFrame = false;
            if (videoCapture != null)
            {
                videoCapture.Dispose();
            }
            if (videoCapture1 != null)
            {
                videoCapture1.Dispose();
            }
                
        }

        private void replayClick(object sender, RoutedEventArgs e)
        {
            closeConnection();
            ComparePoseUC comparePoseUC = new ComparePoseUC(poseName, room);
            scorePanel.Children.Clear();
            scorePanel.Children.Add(comparePoseUC);
            
        }
        private void playAllClick(object sender, RoutedEventArgs e)
        {
            traineeImage_MouseDown();
            trainerImage_MouseDown();
        }


        private void leftClick(object sender, RoutedEventArgs e)
        {
            //drawPanel.Children.Clear();
            //if (page > 0)
            //{
            //    page--;
            //    Mat m = new Mat();
            //    videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, unsimilar[page].Item1);
            //    videoCapture.Read(m);
            //    trainerImage.Source = ImageSourceForBitmap(m.Bitmap);

            //    Mat m1 = new Mat();
            //    videoCapture1.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, unsimilar[page].Item2);
            //    videoCapture1.Read(m1);
            //    drawError(bodymotion, unsimilar[page]);
            //    traineeImage.Source = ImageSourceForBitmap(m1.Bitmap);
            //    if (page == 0)
            //    {
            //        leftbtn.Visibility = Visibility.Hidden;
            //    }
            //    else
            //    {
            //        leftbtn.Visibility = Visibility.Visible;
            //    }
            //    rightbtn.Visibility = Visibility.Visible;
            //}

        }
        private void rightClick(object sender, RoutedEventArgs e)
        {
            //drawPanel.Children.Clear();
            //if (page < unsimilar.Count-1)
            //{
            //    page++;
            //    Mat m = new Mat();
            //    videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, unsimilar[page].Item1);
            //    videoCapture.Read(m);
            //    trainerImage.Source = ImageSourceForBitmap(m.Bitmap);

            //    Mat m1 = new Mat();
            //    videoCapture1.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, unsimilar[page].Item2);
            //    videoCapture1.Read(m1);
            //    traineeImage.Source = ImageSourceForBitmap(m1.Bitmap);
            //    drawError(bodymotion, unsimilar[page]);
            //    if (page == unsimilar.Count - 1)
            //    {
            //        rightbtn.Visibility = Visibility.Hidden;
            //    }
            //    else
            //    {
            //        rightbtn.Visibility = Visibility.Visible;
            //    }
            //    leftbtn.Visibility = Visibility.Visible;
            //}
            

        }

        private void drawError(List<Position> motions, Tuple<int,int,double> unsimilar)
        {
            var error = comparison.getErrorJoint(trainerMotion, motions, unsimilar.Item1, unsimilar.Item2);
            foreach (var i in error)
            {
                Position point = motions.FirstOrDefault(x => x.Frame == unsimilar.Item2 && x.Joint == i);
                List<Position> position = motions.Where(x => x.Frame == unsimilar.Item2).ToList();

                DrawBone(position, JointType.ShoulderCenter, JointType.Spine);
                DrawBone(position, JointType.Spine, JointType.HipCenter);

                //left arm
                DrawBone(position, JointType.ShoulderCenter, JointType.ShoulderLeft);
                DrawBone(position, JointType.ShoulderLeft, JointType.ElbowLeft);
                DrawBone(position, JointType.ElbowLeft, JointType.WristLeft);
                DrawBone(position, JointType.WristLeft, JointType.HandLeft);

                //right arm
                DrawBone(position, JointType.ShoulderCenter, JointType.ShoulderRight);
                DrawBone(position, JointType.ShoulderRight, JointType.ElbowRight);
                DrawBone(position, JointType.ElbowRight, JointType.WristRight);
                DrawBone(position, JointType.WristRight, JointType.HandRight);

                //left leg
                DrawBone(position, JointType.HipCenter, JointType.HipLeft);
                DrawBone(position, JointType.HipLeft, JointType.KneeLeft);
                //DrawBone(position, JointType.KneeLeft, JointType.AnkleLeft);
                //DrawBone(position, JointType.AnkleLeft, JointType.FootLeft);

                //Right leg
                DrawBone(position, JointType.HipCenter, JointType.HipRight);
                DrawBone(position, JointType.HipRight, JointType.KneeRight);
                //DrawBone(position, JointType.KneeRight, JointType.AnkleRight);
                circle(point.Imx - 5, point.Imy - 5, 10, 10);
            }
        }



        private void backBtnClick(object sender, RoutedEventArgs e)
        {
            closeConnection();
            LearningPoseUC learningPoseUC = new LearningPoseUC(room);
            scorePanel.Children.Clear();
            scorePanel.Children.Add(learningPoseUC);
        }

        private Tuple<string, List<JointType>> getSuggestion(List<Tuple<JointType, double>> tuples)
        {
            List<JointType> error = new List<JointType>();
            foreach(var i in tuples)
            {
                if(i.Item2 < 0.8)
                {
                    text += " " + i.Item1.ToString();
                    error.Add(i.Item1);
                }
            }
            return new Tuple<string, List<JointType>>(text, error);
        }

        public ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        private void trainerImage_MouseDown()
        {
            if (videoCapture == null)
            {
                return;
            }
            IsReadingFrame = true;
            ReadAllFrames();
        }

        private void traineeImage_MouseDown()
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
                await Task.Delay(1000 / Convert.ToInt16(30));
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

                var body = bodymotion.Where(x => x.Frame == FrameNo1).ToList();

                drawPanel.Children.Clear();
                //center bone
                //DrawBone(body, JointType.Head, JointType.ShoulderCenter);
                DrawBone(body, JointType.ShoulderCenter, JointType.Spine);
                DrawBone(body, JointType.Spine, JointType.HipCenter);

                //left arm
                DrawBone(body, JointType.ShoulderCenter, JointType.ShoulderLeft);
                DrawBone(body, JointType.ShoulderLeft, JointType.ElbowLeft);
                DrawBone(body, JointType.ElbowLeft, JointType.WristLeft);
                DrawBone(body, JointType.WristLeft, JointType.HandLeft);

                //right arm
                DrawBone(body, JointType.ShoulderCenter, JointType.ShoulderRight);
                DrawBone(body, JointType.ShoulderRight, JointType.ElbowRight);
                DrawBone(body, JointType.ElbowRight, JointType.WristRight);
                DrawBone(body, JointType.WristRight, JointType.HandRight);

                //left leg
                DrawBone(body, JointType.HipCenter, JointType.HipLeft);
                DrawBone(body, JointType.HipLeft, JointType.KneeLeft);
                //DrawBone(body, JointType.KneeLeft, JointType.AnkleLeft);
                //DrawBone(body, JointType.AnkleLeft, JointType.FootLeft);

                //Right leg
                DrawBone(body, JointType.HipCenter, JointType.HipRight);
                DrawBone(body, JointType.HipRight, JointType.KneeRight);
                //DrawBone(body, JointType.KneeRight, JointType.AnkleRight);
                await Task.Delay(1000 / Convert.ToInt16(30));
            }
            FrameNo1 = 0;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int page = (int)Slidelb.Value;

            drawPanel.Children.Clear();
            Mat m = new Mat();
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, unsimilar[page].Item1);
            videoCapture.Read(m);
            trainerImage.Source = ImageSourceForBitmap(m.Bitmap);

            Mat m1 = new Mat();
            videoCapture1.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, unsimilar[page].Item2);
            videoCapture1.Read(m1);
            traineeImage.Source = ImageSourceForBitmap(m1.Bitmap);
            drawError(bodymotion, unsimilar[page]);
        }
    }
}
