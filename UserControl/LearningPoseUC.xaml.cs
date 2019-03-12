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

using MuayThaiTraining.Model;
using System.Windows.Interop;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for LearningPoseUC.xaml
    /// </summary>
    public partial class LearningPoseUC : UserControl
    {

        Pose pose = new Pose();
        ClassRoom classRoom = new ClassRoom();
        String room;
        VideoCapture videoCapture;
        string path1 = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\MotionTrainer");



        public LearningPoseUC(String room)
        {
            InitializeComponent();
            this.room = room;
            createPoseBtn(room);
            nameLbl.Content = room;
            ScrollViewer viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private void createPoseBtn(String room)
        {
            // Create a Button margin
            int left = 80;
            int top = 150;
            int right = 0;
            int bottom = 0;

            var brush = new SolidColorBrush(Color.FromRgb((byte)31, (byte)30, (byte)27));

            List<Pose> list = pose.getPose(room);
            foreach (var i in list)
            {
                Image img = new Image();
                Label lb = new Label();
                lb.Content = i.PoseName;
                lb.FontSize = 16;
                lb.FontWeight = FontWeights.Bold;
                lb.HorizontalAlignment = HorizontalAlignment.Left;
                lb.VerticalAlignment = VerticalAlignment.Top;
                lb.Margin = new Thickness(left+30, top+130, right, bottom);
                lb.Style = (Style)FindResource("LabelTemplate");
                //border.Child = img;
                string path = "";
                if (i.Type == "Motion")
                {
                    path = path1 + "\\" + i.PoseName.Replace(' ', '_') + ".mp4";
                    videoCapture = new VideoCapture(path);
                    Mat m = new Mat();
                    videoCapture.Read(m);
                    img.Source = ImageSourceForBitmap(m.Bitmap);

                }
                else
                {
                    path = path1 + "\\" + i.PoseName.Replace(' ', '_') + ".png";
                    img.Source = new BitmapImage(new Uri(path));
                }

                img.Style = (Style)FindResource("ImageTemplate");
                img.Height = 200;
                img.Width = 200;
                img.Margin = new Thickness(left, top, right, bottom);
                img.Name = i.PoseName.Replace(' ', '_');
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.VerticalAlignment = VerticalAlignment.Top;
                if (left + 300 > System.Windows.SystemParameters.WorkArea.Width)
                {
                    top += 250;
                    left = -170;
                }

                left += 250;

                // Add a Button Click Event handler
                img.MouseDown += poseClick1;
                lb.MouseDown += poseClick;

                // Add Button to the Form
                posepanel.Children.Add(img);
                posepanel.Children.Add(lb);

            }
        }

        public ImageSource ImageSourceForBitmap(System.Drawing.Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            
        }

        private void poseClick(object sender, RoutedEventArgs e)
        {
                Label lb = (Label)sender;
                ComparePoseUC comparePoseUC = new ComparePoseUC(lb.Content.ToString(), room);
                posepanel.Children.Clear();
                posepanel.Children.Add(comparePoseUC);
            
            
            
        }

        private void poseClick1(object sender, RoutedEventArgs e)
        {
            
                Image img = (Image)sender;
                ComparePoseUC comparePoseUC = new ComparePoseUC(img.Name.Replace(' ', '_'), room);
                posepanel.Children.Clear();
                posepanel.Children.Add(comparePoseUC);
            
            
            
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            AddPoseUC addPoseUC = new AddPoseUC(room);
            posepanel.Children.Clear();
            posepanel.Children.Add(addPoseUC);
        }
    }
}
