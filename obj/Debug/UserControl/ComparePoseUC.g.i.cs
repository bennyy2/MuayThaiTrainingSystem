﻿#pragma checksum "..\..\..\UserControl\ComparePoseUC.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "401906A0CC439BD6B74482E5F57A8A5352225FCA"
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
    /// ComparePoseUC
    /// </summary>
    public partial class ComparePoseUC : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid comparePanel;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label poseNamelb;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image exampleImage;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image userImage;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas userPanel;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button compareBtn;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button connectBtn;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\UserControl\ComparePoseUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label statuslb;
        
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
            System.Uri resourceLocater = new System.Uri("/MuayThaiTraining;component/usercontrol/compareposeuc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControl\ComparePoseUC.xaml"
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
            this.comparePanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.poseNamelb = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.exampleImage = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            this.userImage = ((System.Windows.Controls.Image)(target));
            return;
            case 5:
            this.userPanel = ((System.Windows.Controls.Canvas)(target));
            return;
            case 6:
            this.compareBtn = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\UserControl\ComparePoseUC.xaml"
            this.compareBtn.Click += new System.Windows.RoutedEventHandler(this.compareBtnClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.connectBtn = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\UserControl\ComparePoseUC.xaml"
            this.connectBtn.Click += new System.Windows.RoutedEventHandler(this.connectBtnClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.statuslb = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

