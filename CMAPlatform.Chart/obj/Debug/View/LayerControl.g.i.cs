﻿#pragma checksum "..\..\..\View\LayerControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "26CEDF1A95183F11CC54AB092EDA2A9C251E92CE"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CMAPlatform.Chart.Converter;
using CMAPlatform.Chart.View;
using Digihail.CCPSOE.Models.Group;
using Digihail.DAD3.Charts.Base;
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


namespace CMAPlatform.Chart.View {
    
    
    /// <summary>
    /// LayerControl
    /// </summary>
    public partial class LayerControl : Digihail.DAD3.Charts.Base.ChartViewBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 75 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category2;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbCheckedBox2;
        
        #line default
        #line hidden
        
        
        #line 198 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category3;
        
        #line default
        #line hidden
        
        
        #line 220 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbCheckedBox3;
        
        #line default
        #line hidden
        
        
        #line 322 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category4;
        
        #line default
        #line hidden
        
        
        #line 345 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category5;
        
        #line default
        #line hidden
        
        
        #line 429 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category6;
        
        #line default
        #line hidden
        
        
        #line 484 "..\..\..\View\LayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category7;
        
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
            System.Uri resourceLocater = new System.Uri("/CMAPlatform.Chart;component/view/layercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\LayerControl.xaml"
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
            this.radbtn_Category2 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 2:
            this.cbCheckedBox2 = ((System.Windows.Controls.CheckBox)(target));
            
            #line 101 "..\..\..\View\LayerControl.xaml"
            this.cbCheckedBox2.Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 102 "..\..\..\View\LayerControl.xaml"
            this.cbCheckedBox2.Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            case 3:
            this.radbtn_Category3 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.cbCheckedBox3 = ((System.Windows.Controls.CheckBox)(target));
            
            #line 225 "..\..\..\View\LayerControl.xaml"
            this.cbCheckedBox3.Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 226 "..\..\..\View\LayerControl.xaml"
            this.cbCheckedBox3.Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            case 5:
            this.radbtn_Category4 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.radbtn_Category5 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 7:
            
            #line 380 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 381 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 385 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 386 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 390 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 391 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 395 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 396 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 400 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 401 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 405 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 406 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 410 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 411 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 415 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 416 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 15:
            this.radbtn_Category6 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 16:
            
            #line 454 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 455 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 459 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 460 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 464 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 465 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 469 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 470 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 20:
            this.radbtn_Category7 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 21:
            
            #line 513 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 514 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 594 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 595 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            case 23:
            
            #line 675 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 676 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            case 24:
            
            #line 756 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 757 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            case 25:
            
            #line 837 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 838 "..\..\..\View\LayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

