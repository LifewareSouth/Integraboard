using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Lifeware;
using System.Windows;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Lifeware.SoftwareCommon;
using Lifeware.Validation;
using System.Net.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Configuration;

namespace LifewareSoftwareLauncher
{
    public class SoftwareReply
    {
        public string Software { get; set; }

        public string Serial { get; set; }
        public string Status { get; set; }
    }
    public class Software
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public List<Lifeware.Serial> serials { get; set; }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        
        /// <summary>
        /// Define los estados existentes en el proceso de carga. Acá se deben agregar estados adicionales
        /// </summary>
        public enum State {Intro, Login, ValidatingSerial, CheckingSerial, Launch, GenericError }

        static State currentState;
        static string genericErrorDetail;
        private static UserControl currentViewMode;

        private static Lifeware.Validation.Serial serial;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            CurrentState = State.Intro;
            CurrentStateChanged += MainViewModel_CurrentStateChanged;
        }

        private void MainViewModel_CurrentStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("CurrentViewModel");
        }              

        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }
        #region Properties
        public string VersionMessage
        {
            get { return $"{GlobalConfig.AppName} v{GlobalConfig.AppVersion}"; }
        }

        public UserControl CurrentViewModel
        {
            get
            {
                return currentViewMode;
            }
        }
        static event EventHandler CurrentStateChanged;
        public static State CurrentState
        {
            get
            {
                return currentState;
            }

            set
            {
                currentState = value;
                SetCurrentStateView();
                CurrentStateChanged?.Invoke(null, new EventArgs());
            }
        }

        public static Lifeware.Validation.Serial Serial
        {
            get
            {
                return serial;
            }

            set
            {
                serial = value;
            }
        }
        #endregion

        #region States
        /// <summary>
        /// Procesa el estado actual, estableciendo la vista y acciones asociadas. Acá se deben procesar nuevos estados.
        /// </summary>
        private static void SetCurrentStateView()
        {
            switch (currentState)
            {
                case State.Intro:
                    currentViewMode = new LauncherCard()
                    {
                        IconSpin = true,
                        Icon = FontAwesome.WPF.FontAwesomeIcon.Spinner,
                        PrimaryContent = LauncherCard.PrimaryContentType.Text,
                        PrimaryText = Properties.Resources.IntroTitle,
                        SecondaryText = Properties.Resources.IntroMessage,
                        BackgroundAction = CheckIfSerialExists
                    };
                    break;
                case State.Login:
                    currentViewMode = new LauncherCard()
                    {
                        IconSpin = false,
                        Icon = FontAwesome.WPF.FontAwesomeIcon.SignIn,
                        PrimaryContent = LauncherCard.PrimaryContentType.LoginForm,
                        SecondaryText = Properties.Resources.LoginMessage
                    };
                    break;
                case State.ValidatingSerial:
                    currentViewMode = new LauncherCard()
                    {
                        IconSpin = true,
                        Icon = FontAwesome.WPF.FontAwesomeIcon.Spinner,
                        PrimaryContent = LauncherCard.PrimaryContentType.Text,
                        PrimaryText = Properties.Resources.ValidatingSerialTitle,
                        SecondaryText = Properties.Resources.ValidatingSerialMessage,
                        BackgroundAction = Validate
                    };
                    break;
                case State.GenericError:
                    currentViewMode = new LauncherCard()
                    {
                        IconSpin = false,
                        Icon = FontAwesome.WPF.FontAwesomeIcon.Warning,
                        PrimaryContent = LauncherCard.PrimaryContentType.Text,
                        PrimaryText = Properties.Resources.GenericError,
                        SecondaryText = $"{Properties.Resources.ErrorDetail}: {genericErrorDetail}"
                    };

                    break;
                case State.Launch:
                    currentViewMode = new LauncherCard()
                    {
                        IconSpin = false,
                        Icon = FontAwesome.WPF.FontAwesomeIcon.Check,
                        PrimaryContent = LauncherCard.PrimaryContentType.Text,
                        PrimaryText = Properties.Resources.LaunchTitle,
                        SecondaryText = Properties.Resources.LaunchMessage,
                        BackgroundAction = LaunchSoftware
                    };
                    break;
                case State.CheckingSerial:
                    currentViewMode = new LauncherCard()
                    {
                        IconSpin = true,
                        Icon = FontAwesome.WPF.FontAwesomeIcon.Spinner,
                        PrimaryContent = LauncherCard.PrimaryContentType.Text,
                        PrimaryText = Properties.Resources.CheckingSerialTitle,
                        SecondaryText = Properties.Resources.CheckingSerialMessage,
                        BackgroundAction = CheckSerial
                    };
                    break;
            }            
        }

        private static void LaunchSoftware()
        {
            bool inErrorState = false;
            try
            {
                Process.Start(GlobalConfig.ExeName);
                Application.Current.Shutdown();
            }
            catch {
                inErrorState = true;
            }
                
            if(inErrorState)
            {

                //genericErrorDetail = Properties.Resources.LaunchErrorDetail;
                CurrentState = State.GenericError;
            }
        }

        
        private static async void Validate()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , ConfigurationManager.AppSettings["access_token"].ToString());
            
            RegistrationData reg = new RegistrationData();
            var jsonUser = new Dictionary<string, string>
            {
                { "ActivationCode", MainViewModel.Serial.ActivationCode},
                { "PublicKey", MainViewModel.Serial.PublicKey },
                { "SerialNumber", MainViewModel.Serial.SerialNumber },
                { "SoftwareGuid", GlobalConfig.AppGuid},
                { "KeyboardCode", MainViewModel.Serial.KeyboardCode},
                { "ComputerName", reg.ComputerName},
                { "UserName", reg.UserName},
                { "UserContactInfo", reg.UserContactInfo},
                { "ExtraInfo", reg.ExtraInfo},
                { "ComputerSid", Lifeware.Validator.GetComputerSid() }
            };

            var content = new FormUrlEncodedContent(jsonUser);

            var response = await client.PostAsync(ConfigurationManager.AppSettings["base_url"].ToString() + "/api/serial/Validate", content);

            var responseString = await response.Content.ReadAsStringAsync();

            SoftwareReply reply = JsonConvert.DeserializeObject<SoftwareReply>(responseString);
            if (reply.Status == "OK")
            {
                Software software = JsonConvert.DeserializeObject<Software>(reply.Software);
                Lifeware.Serial serial = JsonConvert.DeserializeObject<Lifeware.Serial>(reply.Serial);

                var SerialNumber = serial.SerialNumber;
                var SoftwareGuid = GlobalConfig.AppGuid;
                var ComputerSid = Lifeware.Validator.GetComputerSid();
                var PublicKey = software.PublicKey;
                var PrivateKey = software.PrivateKey;

                string activationCode = GenerateActivationCode(SerialNumber, SoftwareGuid, ComputerSid, PrivateKey);
                MainViewModel.Serial.ActivationCode = activationCode;
                MainViewModel.Serial.KeyboardCode = null;
                MainViewModel.Serial.SerialNumber = SerialNumber;
                MainViewModel.Serial.SoftwareGuid = SoftwareGuid;
                MainViewModel.Serial.PublicKey = PublicKey;
                SerialFileManager.SaveSerial(MainViewModel.Serial.Adapt());

            }
            else 
            {
                genericErrorDetail = reply.Status;
                CurrentState = State.GenericError;
                return;
            }
            
            CurrentState = State.Launch;
           

        }

        private static void CheckIfSerialExists()
        {
            if (SerialFileManager.SerialFileExist())
            {
                //MainViewModel.Serial = LifewareIntegraLogic.SerialFileManager.ReadSerial();
                CurrentState = State.CheckingSerial;
            }
            else
            {
                CurrentState = State.Login;
            }
        }

        private static async void CheckSerial()
        {
            if (await SerialFileManager.IsValidSerial())//Lifeware.Validator.CheckIsSerialValidAsync(SerialFileManager.ReadSerial(), Lifeware.Validator.GetComputerSid()))
            {
                CurrentState = State.Launch;
            }
            else
            {
                Console.WriteLine("Error al checkear serial");
                genericErrorDetail = Properties.Resources.CheckSerialErrorDetail;
                CurrentState = State.GenericError;
            }
        }
        #endregion

        public static string GenerateActivationCode(string SerialNumber, string SoftwareGuid, string sid, string privateKey)
        {
            RSACryptoServiceProvider rsaprovider = new RSACryptoServiceProvider();
            rsaprovider.FromXmlString(privateKey);
            RSAParameters clavePrivada = rsaprovider.ExportParameters(true);
            byte[] datosUser = Encoding.UTF8.GetBytes(SerialNumber + sid + SoftwareGuid);
            AsymmetricSignatureFormatter asf = new RSAPKCS1SignatureFormatter(rsaprovider);
            HashAlgorithm ha = new SHA1Managed();
            asf.SetHashAlgorithm(ha.ToString());
            byte[] hashedDataUser;
            string hashedDataUserString;
            hashedDataUser = ha.ComputeHash(datosUser);
            hashedDataUserString = BitConverter.ToString(hashedDataUser);
            byte[] mySignature = asf.CreateSignature(ha);
            string signatureBlock = Convert.ToBase64String(mySignature);
            return signatureBlock;

        }
    }
}
