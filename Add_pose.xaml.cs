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

using Microsoft.Kinect;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for Add_pose.xaml
    /// </summary>
    public partial class Add_pose : Window
    {
        KinectSensor kSensor;
        ConnectDB connectDB = new ConnectDB();
        Comparison compare = new Comparison();
        int count = 0;
        float x, y, z;
        Skeleton skel;

        public Add_pose()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void btn_connect(object sender, RoutedEventArgs e)
        {
            if (btnConnect.Content.ToString() == "Start Record")
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
                    kSensor.ColorFrameReady += KSensor_ColorFrameReady;
                    kSensor.SkeletonFrameReady += KSensor_SkeletonFrameReady;
                    //kSensor.AllFramesReady += KSensor_AllFramesReady;
                }
                catch
                {
                    this.lbStatus.Content = "Cannot connect Kinect";
                    this.lbKinectID.Content = "-";
                }


            }
            else
            {
                if (kSensor != null && kSensor.IsRunning)
                {
                    //saveFile();
                    compare.calScore(skel);
                    kSensor.Stop();
                    this.statusText.Content = "X :" + x + " Y:" + y + " Z:" + z;
                    this.btnConnect.Content = "Connect";
                    this.lbStatus.Content = "Disconnec t";
                    this.lbKinectID.Content = "-";
                    count = 0;

                }
            }

        }

        private void showPosition()
        {
            
            List<JointType> legLeft = new List<JointType> { JointType.HipCenter, JointType.HipLeft,
            JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft};

            List<JointType> legRight = new List<JointType> { JointType.HipRight,
            JointType.KneeRight, JointType.AnkleRight, JointType.FootRight};

            List<JointType> handLeft = new List<JointType> { JointType.Spine, JointType.ShoulderCenter,
            JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};

            List<JointType> handRight = new List<JointType> { JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight };


            List<List<JointType>> li = new List<List<JointType>> { legLeft, legRight, handLeft, handRight };


            foreach (List<JointType> j in li)
            {

                foreach (JointType i in j)
                {
                    Console.WriteLine(i +" "+ skel.Joints[i].Position.X.ToString());
                }
            }

        }

        private void saveFile()
        {
            string name = this.nameText.Text;
            string des = this.desText.Text;

            //savePicture();
            connectDB.savePosition(skel, name, des);
        }

        public void savePicture()
        {
            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)colorImage.Width, (int)colorImage.Height, 60d, 60d, PixelFormats.Default);
            renderTarget.Render(colorImage);

            //var crop = new CroppedBitmap(rtb, new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));            

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\savePic");
            using (var fs = System.IO.File.OpenWrite(path1 + "\\pic.png"))
            {
                pngEncoder.Save(fs);
            }
        }



        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.lbStatus.Content = kSensor.Status.ToString();

        }


        private void KSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }

                //byte[] colorData = GenerateColorByte(colorFrame);
                byte[] colorData = new byte[colorFrame.PixelDataLength];

                colorFrame.CopyPixelDataTo(colorData);

                int stride = colorFrame.Width * colorFrame.BytesPerPixel;

                try
                {
                    this.colorImage.Source = BitmapSource.Create((int) skelCanvas.Width, (int)skelCanvas.Height,
                    96, 96, PixelFormats.Bgr32, null, colorData, stride);
                    //this.colorImage.Source = colorImageBitmap;
                }
                catch
                {
                    this.statusText.Content = "Cannot open kinect frame.";
                }


            }
        }

        private void KSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            skelCanvas.Children.Clear();

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

                    count += 1;
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





        private byte[] GenerateColorByte(DepthImageFrame colorFrame)
        { 
            short[] rawDepthData = new short[colorFrame.PixelDataLength];
            colorFrame.CopyPixelDataTo(rawDepthData);

            Byte[] pixels = new byte[colorFrame.Height * colorFrame.Width * 4];

            const int BlueIndex = 0;
            const int GreenIndex = 1;
            const int RedIndex = 2;

            for (int depthIndex = 0, colorIndex = 0;
                depthIndex < rawDepthData.Length && colorIndex < pixels.Length;
                depthIndex++, colorIndex += 4)
            {
                int player = rawDepthData[depthIndex] & DepthImageFrame.PlayerIndexBitmask;

                int depth = rawDepthData[depthIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                if (depth <= 900)
                {
                    pixels[colorIndex + BlueIndex] = 255;
                    pixels[colorIndex + GreenIndex] = 0;
                    pixels[colorIndex + RedIndex] = 0;
                }
                else if (depth > 900 && depth < 2000)
                {
                    pixels[colorIndex + BlueIndex] = 0;
                    pixels[colorIndex + GreenIndex] = 255;
                    pixels[colorIndex + RedIndex] = 0;
                }
                else if (depth > 2000)
                {
                    pixels[colorIndex + BlueIndex] = 0;
                    pixels[colorIndex + GreenIndex] = 0;
                    pixels[colorIndex + RedIndex] = 255;
                }

            }

            return pixels;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Comparison compare = new Comparison();
            statusText.Content = compare.calScore(skel);
            //compare.testCalScore(skel);
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

            skelCanvas.Children.Add(backBone);
        }

        private void add_db(object sender, RoutedEventArgs e)
        {
            if (connectDB.saveSkel(skel))
            {
                //Pose pose = new Pose();
                //pose.Show();
                //this.Close();
            }
            else{
                statusText.Content = "Cannot save pose";
            }
            //ConnectDB connectDB = new ConnectDB();
            //connectDB.getDB(this.nameText.Text);
            //this.nameText.Text = "";
            //this.statusText.Content = "Inserted.";
        }
    }
}
