﻿#pragma checksum "..\..\..\UserControl\AddPoseUC.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B6CABD2A90520119536C663A8C81D6B11FCC73EB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MuayThaiTraining;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MuayThaiTraining {
    
    
    /// <summary>
    /// AddPoseUC
    /// </summary>
    public partial class AddPoseUC : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid addPosePanel;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox nameText;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox desText;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnConnect;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image colorImage;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas skelCanvas;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label connectStatus;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label frameStatus;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton poseradio;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\UserControl\AddPoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton motionradio;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MuayThaiTraining;component/usercontrol/addposeuc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControl\AddPoseUC.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.addPosePanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.nameText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.desText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.btnConnect = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\UserControl\AddPoseUC.xaml"
            this.btnConnect.Click += new System.Windows.RoutedEventHandler(this.btnConnectClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.colorImage = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.skelCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 7:
            
            #line 19 "..\..\..\UserControl\AddPoseUC.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.savePoseClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.connectStatus = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.frameStatus = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.poseradio = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 11:
            this.motionradio = ((System.Windows.Controls.RadioButton)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

