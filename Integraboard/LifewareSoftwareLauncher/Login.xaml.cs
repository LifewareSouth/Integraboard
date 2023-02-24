using Lifeware.SoftwareCommon;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http.Headers;
using System.Configuration;

namespace LifewareSoftwareLauncher
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        /// <summary>
        /// Nombre a mostrar
        /// </summary>
        public string Name { get; set; }
        public string Hash { get; set; }

    }
    public class Reply
    {
        public string Serial { get; set; }
        public string Status { get; set; }
    }
    public partial class Login : UserControl
    {
        HttpClient client = new HttpClient();


        public Action<bool> ShowLoadingAction { get; set; }

        public MainWindow WindowParent { get; set; }
        public Login()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        void TogleLogin(bool isEnabled)
        {
            ShowLoadingAction?.Invoke(!isEnabled);
            buttonSend.IsEnabled = isEnabled;
            textBoxName.IsEnabled = isEnabled;
            passwordBox.IsEnabled = isEnabled;
        }


        private async void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TogleLogin(false);
                string computerSid = string.Empty;
                await Task.Factory.StartNew(() =>
                {
                    computerSid = SerialFileManager.GetComputerSid();
                });
                HttpClient client = new HttpClient();
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , ConfigurationManager.AppSettings["access_token"].ToString());

                var jsonUser = new Dictionary<string, string>
                {
                    { "ComputerSid", computerSid},
                    { "SoftwareGuid", GlobalConfig.AppGuid },
                    { "Username", textBoxName.Text },
                    { "Hash", passwordBox.Password}
                };

                var content = new FormUrlEncodedContent(jsonUser);
                var response = await client.PostAsync(ConfigurationManager.AppSettings["base_url"].ToString() + "/api/serial/GetAvailableSerialFromUserData", content);
                if (!response.IsSuccessStatusCode) {
                    throw new Exception("Revise credenciales de acceso de la aplicación.");
                }

                var responseString = await response.Content.ReadAsStringAsync();

                Reply serial = JsonConvert.DeserializeObject<Reply>(responseString);

                string titleText = string.Empty, messageText = string.Empty;
                switch (serial.Status)
                {
                    case "USERDATA_ERROR":
                        titleText = "AUTENTICACIÓN FALLIDA";
                        messageText = "Error al ingresar usuario y/o contraseña.";
                        break;
                    case "SERIAL_NOT_AVAILABLE":
                        titleText = "ERROR DE SERIAL";
                        messageText = "El usuario no cuenta con un serial válido.";
                        break;
                    case "SOFTWARE_NOT_VALID":
                        titleText = "ERROR DE SERIAL";
                        messageText = "El software no se puede activar con el seria que posee.";
                        break;
                    case "SERIAL_NOT_ASSIGNED":
                        titleText = "ERROR DE SERIAL";
                        messageText = "No ha adquirido ningún serial.";
                        break;
                    case "OK":
                        titleText = "CREDENCIALES VÁLIDOS";
                        messageText = "Instalando aplicación.";
                        break;
                }

                var simpleMessageDialog = new SimpleMessageDialog
                {
                    Title = { Text = titleText },
                    Message = { Text = messageText }
                };
                if (serial.Status == "OK")
                {
                    MainViewModel.CurrentState = MainViewModel.State.ValidatingSerial;
                    Lifeware.Validation.Serial serie = JsonConvert.DeserializeObject<Lifeware.Validation.Serial>(serial.Serial);
                    MainViewModel.Serial = serie;
                }
                else
                {
                    await DialogHost.Show(simpleMessageDialog, "RootDialog");
                }
                TogleLogin(true);
            }
            catch (Exception ex) 
            {
                var simpleMessageDialog = new SimpleMessageDialog
                {
                    Title = { Text = "ERROR DE CONEXIÓN" },
                    Message = { Text = ex.Message }
                };
                await DialogHost.Show(simpleMessageDialog, "RootDialog");
                TogleLogin(true);
            }
        }
    }
}
