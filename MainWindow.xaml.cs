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

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kSensor;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void btn_connect(object sender, EventArgs e)
        {
            if (btnConnect.Content.ToString() == "Connect")
            {
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    this.btnConnect.Content = "Stop";
                    kSensor = KinectSensor.KinectSensors[0];
                    if (kSensor.Status == KinectStatus.Connected)
                    {
                        this.lbStatus.Content = kSensor.Status.ToString();
                    }

                    KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;

                }
                try
                {
                    kSensor.Start();
                    this.lbKinectID.Content = kSensor.DeviceConnectionId;
                    kSensor.ColorStream.Enable();
                    kSensor.SkeletonStream.Enable();
                    kSensor.AllFramesReady += KSensor_AllFramesReady;
                }
                catch
                {
                    this.lbStatus.Content = kSensor.Status.ToString();
                }


            }
            else
            {
                if (kSensor != null && kSensor.IsRunning)
                {
                    kSensor.Stop();
                    this.btnConnect.Content = "Connect";
                    this.lbStatus.Content = "Disconnect";
                    this.lbKinectID.Content = "-";

                }
            }


        }
        
        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.lbStatus.Content = kSensor.Status.ToString();

        }

        private void KSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }


                byte[] colorData = new byte[colorFrame.PixelDataLength];

                colorFrame.CopyPixelDataTo(colorData);

                int stride = colorFrame.Width * 4;

                imgDetect.Source = BitmapSource.Create((int)imgDetect.Width, (int)imgDetect.Height,
                    96, 96, PixelFormats.Bgr32, null, colorData, stride);
            }

            skelCanvas.Children.Clear();

            Skeleton[] skeletons = null;

            //copy skeleton data to skeleton array
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    skeletons = new Skeleton[frame.SkeletonArrayLength];
                    frame.CopySkeletonDataTo(skeletons);
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

                    //left leg
                    DrawBone(skeleton, JointType.HipCenter, JointType.HipRight);
                    DrawBone(skeleton, JointType.HipRight, JointType.KneeRight);
                    DrawBone(skeleton, JointType.KneeRight, JointType.AnkleRight);
                    DrawBone(skeleton, JointType.AnkleRight, JointType.FootRight);


                    //Console.Write("X: " + headPoint.X);
                    //Console.Write(" Y: " + headPoint.Y);
                    //Console.WriteLine(" ");
                }


            }
        }

        private void DrawBone(Skeleton skeleton, JointType joint1, JointType joint2)
        {
            ColorImagePoint joint1Point = kSensor.CoordinateMapper.MapSkeletonPointToColorPoint(skeleton.Joints[joint1].Position, ColorImageFormat.RgbResolution640x480Fps30);
            ColorImagePoint joint2Point = kSensor.CoordinateMapper.MapSkeletonPointToColorPoint(skeleton.Joints[joint2].Position, ColorImageFormat.RgbResolution640x480Fps30);

            Line backBone = new Line();
            backBone.Stroke = new SolidColorBrush(Colors.Red);
            backBone.StrokeThickness = 5;

            backBone.X1 = joint1Point.X;
            backBone.Y1 = joint1Point.Y;
            backBone.X2 = joint2Point.X;
            backBone.Y2 = joint2Point.Y;

            skelCanvas.Children.Add(backBone);
        }




    }
}
