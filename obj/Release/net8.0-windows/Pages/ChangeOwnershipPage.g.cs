﻿#pragma checksum "..\..\..\..\Pages\ChangeOwnershipPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9CA214224F4A5A028C8546D21F76923C4577A428"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using TrackMate.Pages;


namespace TrackMate.Pages {
    
    
    /// <summary>
    /// ChangeOwnershipPage
    /// </summary>
    public partial class ChangeOwnershipPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 71 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ProductNameTextBlock;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox UsernameComboBox;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DatePicker;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TrackMate;component/pages/changeownershippage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 60 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BackButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ProductNameTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.UsernameComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 77 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
            this.UsernameComboBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.UsernameComboBox_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DatePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 86 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
            this.DatePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.DatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 94 "..\..\..\..\Pages\ChangeOwnershipPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Update_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

