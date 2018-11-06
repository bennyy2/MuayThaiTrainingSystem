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
using System.Windows.Shapes;

using MuayThaiTraining.Model;

namespace MuayThaiTraining
{
    /// <summary>
    /// Interaction logic for ShowClassRoom.xaml
    /// </summary>
    public partial class ShowClassRoom : Window
    {
        ClassRoom classRoom = new ClassRoom();

        public ShowClassRoom()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            getClassRoom();
        }

        private void getClassRoom()
        {
            ClassRoomUC classRoomUC = new ClassRoomUC();
            controlArea.Children.Add(classRoomUC);
        }

        private void logoutClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("logout");
        }

        private void classroomClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("classroom");
        }

        private void profileClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("profile");
        }
    }
}
