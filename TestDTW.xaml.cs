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
using System.Drawing;
using System.Windows.Media.Media3D;
using Emgu.CV;
using Emgu.CV.Structure;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for TestDTW.xaml
    /// </summary>
    public partial class TestDTW : Window
    {
        ConnectDB connectDB = new ConnectDB();
        Position position = new Position();
        KinectSensor kSensor;
        List<BodyJoint> skelMotion = new List<BodyJoint>();
        List<BodyJoint> trainerMotion = new List<BodyJoint>();
        Image<Bgr, byte> img;

        VideoWriter writer;

        
        int count = 0;
        double x;
        double y;
        double z;
        int filename = 0;

        public TestDTW()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void connectBtn(object sender, RoutedEventArgs e)
        {
            if (btnConnect.Content.ToString() == "Connect")
            {
                btnConnect.Content = "Stop";
                KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
                kSensor = KinectSensor.KinectSensors[0];
                if (kSensor.Status == KinectStatus.Connected)
                {
                    this.statusLB.Content = kSensor.Status.ToString();
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
                    this.statusLB.Content = kSensor.Status.ToString();
                }
            }
            else if (btnConnect.Content.ToString() == "Stop")
            {
                if (kSensor != null && kSensor.IsRunning)
                {
                    kSensor.ColorStream.Disable();
                    kSensor.SkeletonStream.Disable();
                    kSensor.Stop();

                    skelCanvas.Children.Clear();
                    colorImage.Source = null;
                   
                    //colorImage.Source = null;
                    //skelCanvas.Children.Clear();
                    this.btnRecord.Content = "Record Motion";
                    this.btnConnect.Content = "Connect";
                    this.statusLB.Content = "Disconnect";
                    this.count = 0;
                    Console.WriteLine("Stop");

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

                    Console.WriteLine(count);
                    skelMotion.Add(new BodyJoint(skeleton, count));
                    count++;
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

            skelCanvas.Children.Add(backBone);
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
                    this.colorImage.Source = BitmapSource.Create((int)skelCanvas.Width, (int)skelCanvas.Height,
                    96, 96, PixelFormats.Bgr32, null, colorData, stride);

                }
                catch
                {
                    colorImage.Source = null;
                    this.statusLB.Content = "Cannot open kinect frame.";
                }


            }
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            statusLB.Content = kSensor.Status.ToString();
        }

        private void recordBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                btnRecord.Content = "Recording..";
                kSensor.SkeletonStream.Enable();
                kSensor.SkeletonFrameReady += KSensor_SkeletonFrameReady;
                

            }
            catch
            {

                this.statusLB.Content = kSensor.Status.ToString();
            }
            
        }

        private void saveBtn(object sender, RoutedEventArgs e)
        {
            if(position.saveMotionPoint(skelMotion, "TestMotion"))
            {
                Console.WriteLine(skelMotion.Count);
                skelMotion = new List<BodyJoint>();
            }
        }

        public void savePicture()
        {
            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)colorImage.Width, (int)colorImage.Height, 60d, 60d, PixelFormats.Default);
            renderTarget.Render(colorImage);

            //var crop = new CroppedBitmap(rtb, new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));            

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\savePic");
            using (var fs = System.IO.File.OpenWrite(path1 + "\\" + filename.ToString() + ".png"))
            {
                pngEncoder.Save(fs);
                count++;
            }
        }

        private void recordMotion(object sender, RoutedEventArgs e)
        {
            string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\savePic\\test.mp4");

            string destionpath = "@D:\\output.mp4";
            int fourcc = VideoWriter.Fourcc('M', 'P', 'E', 'G');

            writer = new VideoWriter(path1, fourcc, 15, new System.Drawing.Size((int)skelCanvas.Width, (int)skelCanvas.Height), true);
            recordVideo();
            //int width = (int)skelCanvas.Width;
            //int height = (int)skelCanvas.Height;

            //Bitmap target = new Bitmap(width, height);
            //using (Graphics g = Graphics.FromImage(target))
            //{
            //    g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));
            //}

            //VideoFileWriter writer = new VideoFileWriter();
            //writer.Open("test.avi", width, height, 25, VideoCodec.MPEG4, 1000000);


            //writer.Close();
        }



        private void compareBtn(object sender, RoutedEventArgs e)
        {
            DTW dtw = new DTW();
            Position position = new Position();

            //Tuple<double[,], double[,]> table = dtw.modifiedDTW(skelMotion);
            //foreach (var i in table.Item1)
            //{
            //    Console.WriteLine(i);
            //}
            

        }

        private void recordVideo()
        {
            img = new Image<Bgr, byte>(ImageSourceToBitmap((BitmapSource)colorImage.Source));

            Mat m = img.Mat;

            writer.Write(m);

        }

        private System.Drawing.Bitmap ImageSourceToBitmap(BitmapSource bitmapSource)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(memory);
                Bitmap bitmap = new Bitmap(memory);
                return new Bitmap(bitmap);

            }
        }


    }
}
