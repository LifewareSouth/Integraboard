﻿using AForge.Video.DirectShow;
using AForge.Video;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Pages.Pictogramas;
using WpfApp1.Pages.First_Use;

namespace WpfApp1.Pages.Perfil
{
    /// <summary>
    /// Lógica de interacción para TomarFoto.xaml
    /// </summary>
    public partial class TomarFoto : Page
    {
        #region Public properties

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        string origenVista = "";
        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; this.OnPropertyChanged("CurrentDevice"); }
        }
        private FilterInfo _currentDevice;

        #endregion


        #region Private fields

        private IVideoSource _videoSource;

        #endregion

        System.Windows.Controls.Image photoCamera = new System.Windows.Controls.Image();
        public TomarFoto()
        {
            InitializeComponent();
            this.DataContext = this;
            GetVideoDevices();
            this.Unloaded += TakeAPhotoDialog_Unloaded;
        }
        public TomarFoto(string origen)
        {
            InitializeComponent();
            origenVista = origen;
            this.DataContext = this;
            GetVideoDevices();
            this.Unloaded += TakeAPhotoDialog_Unloaded;
        }

        private void TakeAPhotoDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            StopCamera();
        }


        public void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btn_aceptar.IsEnabled = true;
            btnTomarOtra.Visibility = Visibility.Visible;
            btnStart.IsEnabled = false;
            videoPlayer.Source = videoPlayer.Source.CloneCurrentValue();
            photoCamera = videoPlayer;

            StopCamera();

        }

        private void video_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate { videoPlayer.Source = bi; }));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string filename = "C:\\IntegraBoard\\repo\\userProfile\\images\\cameraPhoto2.png";
            RenderTargetBitmap render = new RenderTargetBitmap(500, 500, 96, 96, PixelFormats.Default);
            render.Render(photoCamera);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(render));
            using (Stream stream = File.Create(filename))
            {
                encoder.Save(stream);
            }
            StopCamera();
            if (origenVista == "First Use")
            {
                FirstUse_NombreEdadFoto.Instance.ImagenFromCamera(filename);
            }
            else
            {
                PerfilAlumno.Instance.ImagenFromCamera(filename);
            }
            this.NavigationService.GoBack();
        }

        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            StopCamera();
            this.NavigationService.GoBack();


        }

        private void GetVideoDevices()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();
            foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
            {
                VideoDevices.Add(filterInfo);
            }
            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            StopCamera();
            CurrentDevice = comboBox.SelectedItem as FilterInfo;
            StartCamera();
        }

        private void StartCamera()
        {
            if (CurrentDevice != null)
            {
                _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                _videoSource.NewFrame += video_NewFrame;
                _videoSource.Start();
            }
        }

        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
            }
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion

        private void GoToCrearPictos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CrearPictos());
        }

        private void btnTomarOtra_Click(object sender, RoutedEventArgs e)
        {
            if (!_videoSource.IsRunning)
            {
                StartCamera();
                btn_aceptar.IsEnabled = false;
                btnTomarOtra.Visibility = Visibility.Hidden;
                btnStart.IsEnabled = true;
            }

        }
    }

    static class BitmapHelpers
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
    }
}

