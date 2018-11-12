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
    /// Interaction logic for ComparePoseUC.xaml
    /// </summary>
    public partial class ComparePoseUC : UserControl
    {

        KinectSensor kSensor;
        String poseName;
        String classRoom;
        Comparison comparison = new Comparison();
        Skeleton skel;
        double x;
        double y;
        double z;


        public ComparePoseUC(String poseName, string room)
        {
            InitializeComponent();
            poseNamelb.Content = poseName;
            this.poseName = poseName;
            this.classRoom = room;
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
                    kSensor.SkeletonStream.Enable();
                    kSensor.ColorFrameReady += KSensor_ColorFrameReady;
                    //kSensor.SkeletonFrameReady += KSensor_SkeletonFrameReady;
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

                }
            }
        }

        private void KSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            userPanel.Children.Clear();
            int count = 0;

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

                    //center bone
                    DrawBone(skeleton, JointType.Head, JointType.ShoulderCenter);
                    DrawBone(skeleton, JointType.ShoulderCenter, JointType.Spine);
                    DrawBone(skeleton, JointType.Spine, JointType.HipCenter);

                    //left arm
                    DrawBone(skeleton, JointType.ShoulderCenter, JointType.ShoulderLeft);
                    DrawBone(skeleton, JointType.ShoulderLeft, JointType.ElbowLeft);
                    DrawBone(skeleton, JointType.ElbowLeft, JointType.WristLeft);
                    DrawBone(skeleton, JointType.WristLeft, JointType.HandLeft);

                    //right arm
                    DrawBone(skeleton, JointType.ShoulderCenter, JointType.ShoulderRight);
                    DrawBone(skeleton, JointType.ShoulderRight, JointType.ElbowRight);
                    DrawBone(skeleton, JointType.ElbowRight, JointType.WristRight);
                    DrawBone(skeleton, JointType.WristRight, JointType.HandRight);

                    //left leg
                    DrawBone(skeleton, JointType.HipCenter, JointType.HipLeft);
                    DrawBone(skeleton, JointType.HipLeft, JointType.KneeLeft);
                    DrawBone(skeleton, JointType.KneeLeft, JointType.AnkleLeft);
                    DrawBone(skeleton, JointType.AnkleLeft, JointType.FootLeft);

                    //Right leg
                    DrawBone(skeleton, JointType.HipCenter, JointType.HipRight);
                    DrawBone(skeleton, JointType.HipRight, JointType.KneeRight);
                    DrawBone(skeleton, JointType.KneeRight, JointType.AnkleRight);
                    DrawBone(skeleton, JointType.AnkleRight, JointType.FootRight);

                    count++;
                    x = skeleton.Joints[JointType.HipCenter].Position.X;
                    y = skeleton.Joints[JointType.HipCenter].Position.Y;
                    z = skeleton.Joints[JointType.HipCenter].Position.Z;

                    skel = skeleton;

                    Console.Write(count + ". X: " + skeleton.Joints[JointType.HipCenter].Position.X);
                    Console.Write(" Y: " + skeleton.Joints[JointType.HipCenter].Position.Y);
                    Console.Write(" Z: " + skeleton.Joints[JointType.HipCenter].Position.Z);
                    Console.WriteLine(" ");
                }

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
            //comparison.calScore(skel, poseName, classRoom);
            ScoreUC scoreUC = new ScoreUC(poseName, classRoom);
            comparePanel.Children.Clear();
            comparePanel.Children.Add(scoreUC);

        }
    }
}
