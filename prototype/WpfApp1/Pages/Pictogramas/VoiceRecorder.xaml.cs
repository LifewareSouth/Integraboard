using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using AForge.Math;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Net.Mime.MediaTypeNames;
using NAudio.Wave.Compression;
using NAudio.Wave;
using System.Windows.Threading;
using System.IO;
using System.Timers;
using System.Diagnostics.Metrics;
using System.Windows.Shell;
using NAudio.Lame;
using System.Net;
using WpfApp1.Assets;

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para VoiceRecorder.xaml
    /// </summary>
    public partial class VoiceRecorder : Page
    {
        string VOICEPATH = @"C:\IntegraBoard\repo\grabaciones\voice.wav";
        public VoiceRecorder()
        {
            InitializeComponent();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                sourceList.Items.Add(WaveIn.GetCapabilities(i).ProductName);
            }
            sourceList.SelectedIndex = 0;
        }
        System.Timers.Timer timer = new System.Timers.Timer();
        static int m_counter = 0;
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }


        private void record_btn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(VOICEPATH))
            {
                File.Delete(VOICEPATH);
            }
            if (!Directory.Exists(@"C:\IntegraBoard\repo\grabaciones"))
            {
                if (!Directory.Exists(@"C:\IntegraBoard\repo"))
                {
                    if (!Directory.Exists(@"C:\IntegraBoard"))
                    {
                        Directory.CreateDirectory(@"C:\IntegraBoard");
                    }
                    Directory.CreateDirectory(@"C:\IntegraBoard\repo");
                }
                Directory.CreateDirectory(@"C:\IntegraBoard\repo\grabaciones");
            }
            record_btn.IsEnabled = false;
            save.IsEnabled = true;
            lbl_rec.Content = "Grabando...";
            lbl_rec.Foreground = Brushes.White;
            //-----------------------------------
            if (m_counter != 0)
            {
                m_counter = -1;
            }
            
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimerElaspsed;
            timer.Start();
            waveSource = new WaveIn();
            waveSource.DeviceNumber = sourceList.SelectedIndex;
            waveSource.WaveFormat = new WaveFormat(44100, 1);

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            waveFile = new WaveFileWriter(VOICEPATH, waveSource.WaveFormat);

            waveSource.StartRecording();
            playbtn.IsEnabled = false;

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

            save.IsEnabled = false;
            lbl_rec.Content = "Grabador de voz";
            lbl_rec.Foreground = Brushes.White;
            guardarbtn.IsEnabled = true;

            timer.Stop();
            playbtn.IsEnabled = true;
            waveSource.Dispose();
            waveSource.StopRecording();
            

        }
        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.WriteData(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }

            record_btn.IsEnabled = true;
        }

        private void OnTimerElaspsed(object sender, ElapsedEventArgs e)
        {
            m_counter = m_counter +1;
            lbl_timer.Dispatcher.Invoke(() =>
            {
                lbl_timer.Content = "Duración: " + m_counter;
            });
        }
        private NAudio.Wave.WaveFileReader wave = null;
        private NAudio.Wave.DirectSoundOut output = null;
        private void playbtn_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(VOICEPATH))
            {
                return;
            }
            wave = new NAudio.Wave.WaveFileReader(VOICEPATH);
            output = new NAudio.Wave.DirectSoundOut();
            output.Init(new NAudio.Wave.WaveChannel32(wave));
            output.Play();
            playbtn.Visibility = Visibility.Hidden;
            playbtn.IsEnabled = false;
            stopbtn.IsEnabled = true;
            stopbtn.Visibility = Visibility.Visible;
            record_btn.IsEnabled = false;

        }
        private void stopbtn_Click(object sender, RoutedEventArgs e)
        {
            if (output != null)
            {
                output.Stop();
                output.Dispose();
                wave.Dispose();
            }
            playbtn.Visibility = Visibility.Visible;
            playbtn.IsEnabled = true;
            stopbtn.IsEnabled = false;
            stopbtn.Visibility = Visibility.Hidden;
            record_btn.IsEnabled = true;
        }
       

        private void guardarbtn_Click(object sender, RoutedEventArgs e)
        {
            string nombreSonido ="voice_"+ Repository.Instance.getVoiceNumber();
            Repository.Instance.CrearSonido(VOICEPATH, nombreSonido,true);
            this.NavigationService.GoBack();
        }
    }
}
