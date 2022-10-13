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

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para VoiceRecorder.xaml
    /// </summary>
    public partial class VoiceRecorder : Page
    {
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
        /*
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        */
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        
        private void record_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(@"C:\IntegraBoard\repo\grabaciones"))
            {
                if (!Directory.Exists(@"C:\IntegraBoard\repo")){
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
            lbl_rec.Foreground = Brushes.Red;
            //-----------------------------------
            m_counter = 0;
            timer.Interval = 1000;
            timer.Elapsed += OnTimerElaspsed;
            timer.Start();

            /*record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);
            */
            waveSource = new WaveIn();
            waveSource.DeviceNumber = sourceList.SelectedIndex;
            waveSource.WaveFormat = new WaveFormat(44100, 1);

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            waveFile = new WaveFileWriter(@"C:\IntegraBoard\repo\grabaciones\sample.wav", waveSource.WaveFormat);

            waveSource.StartRecording();
            
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

            save.IsEnabled = false;
            lbl_rec.Content = "Grabador de voz";
            lbl_rec.Foreground = Brushes.White;

            timer.Stop();
            /*
            record("save recsound C:\\Users\\Tomas\\Desktop\\sample2.wav", "", 0, 0);
            record("close recsound", "", 0, 0);
            */

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
            m_counter++;
            lbl_timer.Dispatcher.Invoke(() =>
            {

                lbl_timer.Content = "Duración: "+ m_counter;
            });
        }
    }
}
