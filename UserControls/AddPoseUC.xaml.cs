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
using System.Drawing;
using Microsoft.Win32;
using Microsoft.Kinect;
using MuayThaiTraining.Model;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Diagnostics;
using System.Threading;



namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for AddPoseUC.xaml
    /// </summary>
    public partial class AddPoseUC : UserControl
    {
        KinectSensor kSensor;
        ConnectDB connectDB = new ConnectDB();
        Comparison compare = new Comparison();
        Position position = new Position();
        Pose pose = new Pose();
        BodyJoint bodyJoint = new BodyJoint();
        List<BodyJoint> motion = new List<BodyJoint>();
        int count = 0;
        float x, y, z;
        Skeleton skel;
        String room;


        public AddPoseUC(String room)
        {
            InitializeComponent();
            this.room = room;
        }

        private void btnConnectClick(object sender, RoutedEventArgs e)
        {
            if (btnConnect.Content.ToString() == "Connect")
            {
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    this.btnConnect.Content = "Stop";
                    kSensor = KinectSensor.KinectSensors[0];
                    if (kSensor.Status == KinectStatus.Connected)
                    {
                        this.connectStatus.Content = kSensor.Status.ToString();
                    }

                    KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;

                }
                try
                {

                    kSensor.Start();
                    //this.lbKinectID.Content = kSensor.DeviceConnectionId;
                    kSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    kSensor.ColorFrameReady += KSensor_ColorFrameReady;
                }
                catch
                {
                    this.connectStatus.Content = "Cannot connect Kinect";
                }


            }
            else
            {
                if (kSensor != null && kSensor.IsRunning)
                {
                    kSensor.Stop();
                    
                    this.btnConnect.Content = "Start Record";
                    this.connectStatus.Content = "Disconnect";
                    this.frameStatus.Content = "Disconnect";
                    this.count = 0;

                }
            }
        }

        private void recordClick(object sender, RoutedEventArgs e)
        {
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

        private void savePoseClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (poseRadio.IsChecked == true)
                {
                    savePicture(1);
                    pose.savePoseDetail(nameText.Text.ToString(), desText.Text.ToString(), room, "Pose");
                    position.saveSkel(skel, room, 1);
                }
                else if (motionRadio.IsChecked == true)
                {
                    //savePicture();
                    pose.savePoseDetail(nameText.Text.ToString(), desText.Text.ToString(), room, "Motion");
                    position.saveMotionPoint(motion, room);
                }
            }
            catch
            {
                this.frameStatus.Content = "Cannot save pose.";
            }
            finally
            {
                addPosePanel.Children.Clear();
                LearningPoseUC learningPoseUC = new LearningPoseUC(room);
                addPosePanel.Children.Add(learningPoseUC);
            }
        }

        public void savePicture(int frame)
        {
            string name = nameText.Text.Replace(' ', '_') + frame.ToString();
            string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainer");
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)colorImage.Source));
            using (FileStream fs = File.OpenWrite(path1 + "\\" + name + ".png"))
            {
                encoder.Save(fs);
                //count++;
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
                    //DrawBone(skeleton, JointType.Head, JointType.ShoulderCenter);
                    //DrawBone(skeleton, JointType.ShoulderCenter, JointType.Spine);
                    //DrawBone(skeleton, JointType.Spine, JointType.HipCenter);

                    ////left arm
                    //DrawBone(skeleton, JointType.ShoulderCenter, JointType.ShoulderLeft);
                    //DrawBone(skeleton, JointType.ShoulderLeft, JointType.ElbowLeft);
                    //DrawBone(skeleton, JointType.ElbowLeft, JointType.WristLeft);
                    //DrawBone(skeleton, JointType.WristLeft, JointType.HandLeft);

                    ////right arm
                    //DrawBone(skeleton, JointType.ShoulderCenter, JointType.ShoulderRight);
                    //DrawBone(skeleton, JointType.ShoulderRight, JointType.ElbowRight);
                    //DrawBone(skeleton, JointType.ElbowRight, JointType.WristRight);
                    //DrawBone(skeleton, JointType.WristRight, JointType.HandRight);

                    ////left leg
                    //DrawBone(skeleton, JointType.HipCenter, JointType.HipLeft);
                    //DrawBone(skeleton, JointType.HipLeft, JointType.KneeLeft);
                    //DrawBone(skeleton, JointType.KneeLeft, JointType.AnkleLeft);
                    //DrawBone(skeleton, JointType.AnkleLeft, JointType.FootLeft);

                    ////Right leg
                    //DrawBone(skeleton, JointType.HipCenter, JointType.HipRight);
                    //DrawBone(skeleton, JointType.HipRight, JointType.KneeRight);
                    //DrawBone(skeleton, JointType.KneeRight, JointType.AnkleRight);
                    //DrawBone(skeleton, JointType.AnkleRight, JointType.FootRight);

                    
                    x = skeleton.Joints[JointType.HipCenter].Position.X;
                    y = skeleton.Joints[JointType.HipCenter].Position.Y;
                    z = skeleton.Joints[JointType.HipCenter].Position.Z;

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
                    this.frameStatus.Content = "Cannot open kinect frame.";
                }


            }
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.connectStatus.Content = kSensor.Status.ToString();
        }

        private void backBtnClick(object sender, RoutedEventArgs e)
        {
            addPosePanel.Children.Clear();
            LearningPoseUC learningPoseUC = new LearningPoseUC(room);
            addPosePanel.Children.Add(learningPoseUC);
        }
    }
}
