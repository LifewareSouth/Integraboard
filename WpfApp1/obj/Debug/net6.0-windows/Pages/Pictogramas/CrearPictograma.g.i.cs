﻿#pragma checksum "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6FF7E6D762E602FA4DB88803B83D612C055E1DE9"
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
using WpfApp1.Pages.Pictogramas;


namespace WpfApp1.Pages.Pictogramas {
    
    
    /// <summary>
    /// CrearPictograma
    /// </summary>
    public partial class CrearPictograma : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame CrearPictos;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button volverMenu;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelectImage;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button TakePicture;
        
        #line default
        #line hidden
        
        
        #line 247 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelectSound;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp1;V1.0.0.0;component/pages/pictogramas/crearpictograma.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CrearPictos = ((System.Windows.Controls.Frame)(target));
            return;
            case 2:
            this.volverMenu = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
            this.volverMenu.Click += new System.Windows.RoutedEventHandler(this.GoToPictogramas);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SelectImage = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
            this.SelectImage.Click += new System.Windows.RoutedEventHandler(this.SelectImage_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TakePicture = ((System.Windows.Controls.Button)(target));
            
            #line 88 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
            this.TakePicture.Click += new System.Windows.RoutedEventHandler(this.TakePicture_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SelectSound = ((System.Windows.Controls.Button)(target));
            
            #line 255 "..\..\..\..\..\Pages\Pictogramas\CrearPictograma.xaml"
            this.SelectSound.Click += new System.Windows.RoutedEventHandler(this.SelectSound_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
