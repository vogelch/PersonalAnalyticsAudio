﻿#pragma checksum "..\..\..\Views\ChooseBluetoothDevice.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A9C82C64E20E257CFDA41C1B6FAA5DEBB62266AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using PolarTracker.Views;
using Shared;
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


namespace PolarTracker.Views {
    
    
    /// <summary>
    /// ChooseBluetoothDevice
    /// </summary>
    public partial class ChooseBluetoothDevice : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\Views\ChooseBluetoothDevice.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FindButton;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Views\ChooseBluetoothDevice.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel DeviceList;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Views\ChooseBluetoothDevice.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Devices;
        
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
            System.Uri resourceLocater = new System.Uri("/PolarTracker;component/views/choosebluetoothdevice.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ChooseBluetoothDevice.xaml"
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
            this.FindButton = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\Views\ChooseBluetoothDevice.xaml"
            this.FindButton.Click += new System.Windows.RoutedEventHandler(this.FindDevices);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DeviceList = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.Devices = ((System.Windows.Controls.ListBox)(target));
            
            #line 38 "..\..\..\Views\ChooseBluetoothDevice.xaml"
            this.Devices.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.OnDeviceSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 41 "..\..\..\Views\ChooseBluetoothDevice.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DisableTracker);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

