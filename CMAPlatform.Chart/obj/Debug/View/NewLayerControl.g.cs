﻿#pragma checksum "..\..\..\View\NewLayerControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EB868EC856D89B7B5A8FC248C7D6638675409A11326BAD3D0B737A5DC0BABFD1"
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
    /// NewLayerControl
    /// </summary>
    public partial class NewLayerControl : Digihail.DAD3.Charts.Base.ChartViewBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\View\NewLayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayerGrid;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\View\NewLayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category2;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\View\NewLayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbCheckedBox2;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\..\View\NewLayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category5;
        
        #line default
        #line hidden
        
        
        #line 270 "..\..\..\View\NewLayerControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radbtn_Category6;
        
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
            System.Uri resourceLocater = new System.Uri("/CMAPlatform.Chart;component/view/newlayercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\NewLayerControl.xaml"
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
            this.LayerGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.radbtn_Category2 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 3:
            this.cbCheckedBox2 = ((System.Windows.Controls.CheckBox)(target));
            
            #line 92 "..\..\..\View\NewLayerControl.xaml"
            this.cbCheckedBox2.Checked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            
            #line 93 "..\..\..\View\NewLayerControl.xaml"
            this.cbCheckedBox2.Unchecked += new System.Windows.RoutedEventHandler(this.Layer_CheckedNew);
            
            #line default
            #line hidden
            return;
            case 4:
            this.radbtn_Category5 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            
            #line 222 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 223 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 227 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 228 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 232 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 233 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 238 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 239 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 243 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 244 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 248 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 249 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 253 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 254 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 258 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 259 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 13:
            this.radbtn_Category6 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 14:
            
            #line 295 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 296 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 299 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 300 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 305 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 306 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 310 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.BasicInfo_Click);
            
            #line default
            #line hidden
            
            #line 311 "..\..\..\View\NewLayerControl.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.ToggleButton_OnUnchecked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

