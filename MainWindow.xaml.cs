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
using Microsoft.Kinect;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Position position = new Position();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

        }

        private void btnLogin(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            //ShowClassRoom showClassRoom = new ShowClassRoom();
            //showClassRoom.Show();
            this.Close();
        }
        private void regis(object sender, RoutedEventArgs e)
        {
            RegisterUC registerUC = new RegisterUC();
            mainArea.Children.Clear();
            mainArea.Children.Add(registerUC);
        }

        private void testBtn(object sender, RoutedEventArgs e)
        {
            TestDTW testDTW = new TestDTW();
            testDTW.Show();
            this.Close();
        }
    }
}
