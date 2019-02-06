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

using System.IO;
using Microsoft.Kinect;
using MuayThaiTraining.Model;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for ComparePoseUC.xaml
    /// </summary>
    public partial class ComparePoseUC : UserControl
    {

        KinectSensor kSensor;
        String poseName;
        String classRoom;
        Comparison comparison = new Comparison();
        Position position = new Position();
        DTW dtw = new DTW();
        Pose pose = new Pose();
        List<BodyJoint> motion = new List<BodyJoint>();
        Skeleton skel;
        double x;
        double y;
        double z;
        int count = 0;
        string type;
        string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainer");
        string path2 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");


        public ComparePoseUC(String poseName, string room)
        {
            InitializeComponent();
            poseNamelb.Content = poseName;
            this.poseName = poseName;
            this.classRoom = room;
            this.type = pose.getPoseDescription(poseName, room)[1];
            this.deslb.Text = pose.getPoseDescription(poseName, room)[0];
            int num = position.lenghtFrame(poseName, room);

            userImage.Source = new BitmapImage(new Uri(path2 + "\\aax71.png"));
            string name = poseName.Replace(' ', '_') + "7";
            this.exampleImage.Source = new BitmapImage(new Uri(path1 + "\\" + name + ".png"));
        }

        private void connectBtnClick(object sender, RoutedEventArgs e)
        {
            if (connectBtn.Content.ToString() == "Connect Kinect")
            {
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    this.connectBtn.Content = "Stop connecting";
                    kSensor = KinectSensor.KinectSensors[0];
                    if (kSensor.Status == KinectStatus.Connected)
                    {
                        this.statuslb.Content = kSensor.Status.ToString();
                    }
                    KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged; ;

                }
                try
                {
                    kSensor.Start();
                    //startRecord();
                    //this.lbKinectID.Content = kSensor.DeviceConnectionId;
                    kSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    
                    kSensor.ColorFrameReady += KSensor_ColorFrameReady;
                    
                }
                catch
                {
                    this.statuslb.Content = "Cannot connect Kinect";
                }


            }
            else
            {
                if (kSensor != null && kSensor.IsRunning)
                {
                    kSensor.Stop();
                    
                    this.connectBtn.Content = "Connect Kinect";
                    this.statuslb.Content = "Disconnect";
                    count = 0;

                }
            }
        }

        private void recordBtnClick(object sender, RoutedEventArgs e)
        {
            if (recordBtn.Content.ToString() == "Record")
            {
                this.recordBtn.Content = "Record...";
                kSensor.SkeletonStream.Enable();
                kSensor.SkeletonFrameReady += KSensor_SkeletonFrameReady;

            }
        }

        private void KSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            userPanel.Children.Clear();
            

            Skeleton[] skeletons = null;

            //copy skeleton data to skeleton array
            using (SkeletonFrame skeletonframe = e.OpenSkeletonFrame())
            {
                if (skeletonframe != null)
                {
                    skeletons = new Skeleton[skeletonframe.SkeletonArrayLength];
                    skeletonframe.CopySkeletonDataTo(skeletons);
                }
            }

            //if no skeleton data
            if (skeletons == null)
                return;

            //for each skeleton
            foreach (Skeleton skeleton in skeletons)
            {
                //if skeleton is tracked
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    //Using drawbone function only when implement
                    
                    skel = skeleton;
                    savePicture(count);
                    motion.Add(new BodyJoint(skel, count));
                    
                    count++;
                    Console.Write(count + ". X: " + skeleton.Joints[JointType.HipCenter].Position.X);
                    Console.Write(" Y: " + skeleton.Joints[JointType.HipCenter].Position.Y);
                    Console.Write(" Z: " + skeleton.Joints[JointType.HipCenter].Position.Z);
                    Console.WriteLine(" ");


                }

            }
        }

        public void savePicture(int frame)
        {
            string name = poseName.Replace(' ', '_') + frame.ToString();
            string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)userImage.Source));
            using (FileStream fs = File.OpenWrite(path1 + "\\" + name + ".png"))
            {
                encoder.Save(fs);
            }
        }

        private void DrawBone(Skeleton skeleton, JointType joint1, JointType joint2)
        {
            //DepthImagePoint joint1Point = kSensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skeleton.Joints[joint1].Position, DepthImageFormat.Resolution320x240Fps30);
            //DepthImagePoint joint2Point = kSensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skeleton.Joints[joint2].Position, DepthImageFormat.Resolution320x240Fps30);

            ColorImagePoint joint1Point = kSensor.CoordinateMapper.MapSkeletonPointToColorPoint(skeleton.Joints[joint1].Position, ColorImageFormat.RgbResolution640x480Fps30);
            ColorImagePoint joint2Point = kSensor.CoordinateMapper.MapSkeletonPointToColorPoint(skeleton.Joints[joint2].Position, ColorImageFormat.RgbResolution640x480Fps30);

            Line backBone = new Line();
            backBone.Stroke = new SolidColorBrush(Colors.Yellow);
            backBone.StrokeThickness = 5;

            backBone.X1 = joint1Point.X;
            backBone.Y1 = joint1Point.Y;
            backBone.X2 = joint2Point.X;
            backBone.Y2 = joint2Point.Y;

            userPanel.Children.Add(backBone);
        }

        private void KSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }


                byte[] colorData = new byte[colorFrame.PixelDataLength];

                colorFrame.CopyPixelDataTo(colorData);

                int stride = colorFrame.Width * colorFrame.BytesPerPixel;

                try
                {
                    this.userImage.Source = BitmapSource.Create((int)userPanel.Width, (int)userPanel.Height,
                    96, 96, PixelFormats.Bgr32, null, colorData, stride);

                }
                catch
                {
                    this.userImage.Source = null;
                    //this.frameStatus.Content = "Cannot open kinect frame.";
                }


            }
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.statuslb.Content = kSensor.Status.ToString();
        }

        private void compareBtnClick(object sender, RoutedEventArgs e)
        {
            //ScoreUC scoreUC = new ScoreUC(score, poseName, classRoom, type, path, motion);
            ScoreUC scoreUC = new ScoreUC(poseName, classRoom, type, motion);
            comparePanel.Children.Clear();
            comparePanel.Children.Add(scoreUC);
            //if (compareBtn.Content.ToString() == "Compare")
            //{
            //    compareBtn.Content = "Comapring...";

            //    position.saveMotionTraineePoint(motion, classRoom);

            //    double score = 0;
            //    List<Tuple<int, int, double, int>> path = new List<Tuple<int, int, double, int>>();

            //    if (type == "Pose")
            //    {
            //        score = comparison.calScore(skel, poseName, classRoom, 1);
            //    }
            //    else
            //    {
            //        Tuple<double[,], double[,]> table = dtw.modifiedDTW(motion, poseName, classRoom);
            //        int rows = motion.Count;
            //        int columns = position.lenghtFrame(poseName, classRoom);
            //        path = dtw.wrapPath(table.Item1, table.Item2, rows, columns);
            //        foreach (Tuple<int, int, double, int> i in path)
            //        {
            //            score += i.Item3;
            //        }
            //        score /= path.Count;



            //        foreach (Tuple<int, int, double, int> i in path)
            //        {
            //            Console.WriteLine(i.Item1 + " " + i.Item2 + " " + i.Item3);
            //        }

            //        Console.WriteLine("Score : " + score);

            //    }

            //    ScoreUC scoreUC = new ScoreUC(score, poseName, classRoom, type, path, motion);
            //    comparePanel.Children.Clear();
            //    comparePanel.Children.Add(scoreUC);
            //}


        }

        private void backBtnClick(object sender, RoutedEventArgs e)
        {
            LearningPoseUC learningPoseUC = new LearningPoseUC(classRoom);
            comparePanel.Children.Clear();
            comparePanel.Children.Add(learningPoseUC);
        }
    }
}
