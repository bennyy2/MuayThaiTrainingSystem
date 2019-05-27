﻿using System;
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
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Drawing;
using System.Windows.Interop;
using System.Runtime.ExceptionServices;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for ComparePoseUC.xaml
    /// </summary>
    public partial class ComparePoseUC : UserControl
    {

        private KinectSensor kSensor;
        String poseName;
        String classRoom;
        Comparison comparison = new Comparison();
        Position position = new Position();
        DTW dtw = new DTW();
        Pose pose = new Pose();
        List<Position> motion = new List<Position>();
        Skeleton skel;
        double x;
        double y;
        double z;
        int count = 0;
        string type;
        string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainer");
        string path2 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");
        string file;
        VideoCapture videoCapture;
        VideoWriter videoWriter;
        bool IsReadingFrame;
        double TotalFrame;
        double Fps;
        int FrameNo;
        Image<Bgr, byte> img;

        public ComparePoseUC(String poseName, string room)
        {
            InitializeComponent();

            
            poseNamelb.Content = poseName;
            this.poseName = poseName;
            this.classRoom = room;
            this.type = pose.getPoseDescription(poseName, room)[1];
            //poseType.Content = type;
            this.deslb.Text = pose.getPoseDescription(poseName, room)[0];
            int num = position.lenghtFrame(poseName, room);
            if(type == "Motion")
            {
                string name = path1 + "\\" + poseName.Replace(' ', '_').ToString() + ".mp4";
                videoCapture = new VideoCapture(path1 + "\\" + poseName.Replace(' ', '_').ToString() + ".mp4");
                Mat m = new Mat();
                videoCapture.Read(m);
                exampleImage.Source = ImageSourceForBitmap(m.Bitmap);
                TotalFrame = videoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = videoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

            }
            else
            {
                exampleImage.Source = new BitmapImage(new Uri(path1 + "\\" + poseName.Replace(' ', '_').ToString() + ".png"));
            }
            
            //string name = poseName.Replace(' ', '_') + "7";
            //this.exampleImage.Source = new BitmapImage(new Uri(path1 + "\\" + name + ".png"));
        }

        public ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            
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
                    savePicture();
                    
                    kSensor.Stop();
                    kSensor.ColorStream.Disable();
                    kSensor.SkeletonStream.Disable();
                    kSensor.Dispose();
                    kSensor = null;
                    if (videoWriter != null)
                    {
                        videoWriter.Dispose();
                    }
                    
                    this.recordBtn.Content = "Record";
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

                if (type == "Motion")
                {
                    string destionpath = path2 + "\\" + poseName + ".mp4";

                    //int fourcc = VideoWriter.Fourcc('M', 'P', 'E', 'G');
                    int fourcc = VideoWriter.Fourcc('M', 'P', '4', 'V');

                    videoWriter = new VideoWriter(destionpath, fourcc, 15, new System.Drawing.Size((int)userImage.Width, (int)userImage.Height), true);

                }


                kSensor.SkeletonStream.Enable(new TransformSmoothParameters
                {
                    Smoothing = 0.7f,
                    Correction = 0.3f,
                    Prediction = 1.0f,
                    JitterRadius = 1.0f,
                    MaxDeviationRadius = 1.0f
                });

                kSensor.SkeletonFrameReady += KSensor_SkeletonFrameReady;
            }
        }

        [HandleProcessCorruptedStateExceptions]
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
                    framelb.Content = "Recording..";

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

                    skel = skeleton;
                    //motion.Add(new BodyJoint(skel, count));
                    if (type == "Motion")
                    {
                        try
                        {
                            img = new Image<Bgr, byte>(ImageSourceToBitmap((BitmapSource)userImage.Source));
                            videoWriter.Write(img.Mat);


                        }
                        catch (Exception)
                        {

                        }



                    }

                    count++;
                    Console.Write(count + ". X: " + skeleton.Joints[JointType.HipCenter].Position.X);
                    Console.Write(" Y: " + skeleton.Joints[JointType.HipCenter].Position.Y);
                    Console.Write(" Z: " + skeleton.Joints[JointType.HipCenter].Position.Z);
                    Console.WriteLine(" ");


                }

            }
        }

        public void savePicture()
        {
            string name = poseName.Replace(' ', '_').ToString();
            string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");

            string filePath = path1 + "\\" + name+ ".png";
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)userImage.Source));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fs);
            }

        }

        public void savePicture(int count)
        {
            string name = poseName.Replace(' ', '_').ToString();
            string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainee");
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)userImage.Source));
            using (FileStream fs = File.OpenWrite(path1 + "\\" + name+count.ToString() + ".png"))
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

            motion.Add(new Position(
               skeleton.Joints[joint2].Position.X,
               skeleton.Joints[joint2].Position.Y,
               skeleton.Joints[joint2].Position.Z,
               joint2Point.X,
               joint2Point.Y,
               (int)joint2,
               count
               ));

            Line backBone = new Line();
            backBone.Stroke = new SolidColorBrush(Colors.Yellow);
            backBone.StrokeThickness = 2;

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
                    int zindex = Canvas.GetZIndex(userImage);
                    Canvas.SetZIndex(userPanel, zindex + 1);

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
            ScoreUC scoreUC = new ScoreUC(poseName, classRoom, type, motion);
            comparePanel.Children.Clear();
            comparePanel.Children.Add(scoreUC);

        }

        private void backBtnClick(object sender, RoutedEventArgs e)
        {
            LearningPoseUC learningPoseUC = new LearningPoseUC(classRoom);
            comparePanel.Children.Clear();
            comparePanel.Children.Add(learningPoseUC);
        }

        private void exampleImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (videoCapture == null)
            {
                return;
            }
            IsReadingFrame = true;
            ReadAllFrames();
        }


        private async void ReadAllFrames()
        {

            Mat m = new Mat();

            while (IsReadingFrame == true && FrameNo < TotalFrame - 1)
            {
                FrameNo += 1;
                frame.Content = "TIME: "+((FrameNo/Fps)/100).ToString("0.00");
                videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameNo);
                //if (String.IsNullOrEmpty(frame.Text))
                //{
                    

                //}
                //else
                //{
                //    videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, Convert.ToInt32(frame.Text));

                //}
                videoCapture.Read(m);
                exampleImage.Source = ImageSourceForBitmap(m.Bitmap);
                await Task.Delay(1000 / Convert.ToInt16(30));
            }
            FrameNo = 0;
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
