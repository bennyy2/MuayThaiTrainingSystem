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
using System.Xml;
using Microsoft.Kinect;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TreeNode node;
        

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }


        private void Button_Login(object sender, RoutedEventArgs e)
        {
            ShowCategory showCategory = new ShowCategory();
            showCategory.Show();
            this.Close();
            //XmlTextReader xmlTextReader = new XmlTextReader("SkeletonTree.xml");
            //while (xmlTextReader.Read())
            //{
            //    Console.WriteLine(xmlTextReader.Name);
            //}
            //Console.ReadLine();

            //XmlDocument doc = new XmlDocument();
            //doc.Load("SkeletonTree.xml");
            ////XmlElement root = doc.DocumentElement;
            //XmlNodeList nodes = doc.GetElementsByTagName("skeleton");
            //foreach (XmlNode n in nodes)
            //{
            //    Console.WriteLine(n.Name);
            //    foreach (XmlNode childn in n)
            //    {
            //        Console.WriteLine(childn.Name);
            //    }
            //    Console.WriteLine();
            //}

            //Dictionary<String, List<String>> dict = new Dictionary<String, List<String>>()
            //{
            //    { "hipCenter", list}
            //};


            //for (int i = 0; i < nodes.Count; i++)
            //{

            //    Console.Write(nodes[i].InnerXml + " ");
            //    Console.WriteLine(nodes[i].LastChild.InnerText);
            //    //listBox1.Items.Add(nodes[i].InnerXml);
            //}
            //foreach (XmlNode node in nodes)
            //{
            //   foreach(XmlText name in node["name"])
            //    {
            //        Console.WriteLine(name.InnerText);
            //    }

            //}


            //if (checkUser())
            //{
            //    ShowCategory showCategory = new ShowCategory();
            //    showCategory.Show();
            //    this.Close();
            //}
            //else
            //{

            //}

        }
    }
}
