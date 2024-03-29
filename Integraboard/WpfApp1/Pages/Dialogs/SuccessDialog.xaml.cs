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
using WpfApp1.Assets;

namespace WpfApp1.Pages.Dialogs
{
    /// <summary>
    /// Lógica de interacción para SuccessDialog.xaml
    /// </summary>
    public partial class SuccessDialog : Window
    {
        private static BitmapImage success = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.successCheck);
        private static BitmapImage closeButton = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.close_cruz_invertida);
        public SuccessDialog()
        {
            InitializeComponent();
            
        }
        public SuccessDialog(string msj)
        {
            InitializeComponent();
            Mensaje.Content = msj;
            this.Resources["success"] = success;
            this.Resources["closeButton"] = closeButton;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void closebtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
