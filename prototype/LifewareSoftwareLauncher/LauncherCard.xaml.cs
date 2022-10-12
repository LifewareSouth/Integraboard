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

namespace LifewareSoftwareLauncher
{
    /// <summary>
    /// Lógica de interacción para LauncherCard.xaml
    /// </summary>
    public partial class LauncherCard : UserControl
    {

        public enum PrimaryContentType{Text, LoginForm }
        
        public Action BackgroundAction { get; set; }

        public string PrimaryText
        {
            get { return (string)GetValue(PrimaryTextProperty); }
            set { SetValue(PrimaryTextProperty, value); }
        }

        public static DependencyProperty PrimaryTextProperty =
           DependencyProperty.Register("PrimaryText", typeof(string), typeof(LauncherCard));

        public string SecondaryText
        {
            get { return (string)GetValue(SecondaryTextProperty); }
            set { SetValue(SecondaryTextProperty, value); }
        }

        public static DependencyProperty SecondaryTextProperty =
           DependencyProperty.Register("SecondaryText", typeof(string), typeof(LauncherCard));

        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static DependencyProperty IconProperty =
           DependencyProperty.Register("Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(LauncherCard));


        public bool IconSpin
        {
            get { return (bool)GetValue(IconSpinProperty); }
            set { SetValue(IconSpinProperty, value); }
        }

        public static DependencyProperty IconSpinProperty =
           DependencyProperty.Register("IconSpin", typeof(bool), typeof(LauncherCard));

        public PrimaryContentType PrimaryContent
        {
            get { return (PrimaryContentType)GetValue(PrimaryContentProperty); }
            set {
                SetValue(PrimaryContentProperty, value);
                CheckPrimaryContent();
            }
        }

        public static DependencyProperty PrimaryContentProperty =
         DependencyProperty.Register("PrimaryContent", typeof(PrimaryContentType), typeof(LauncherCard));

        public Login LoginForm
        {
            get
            {
                return loginForm;
            }

            set
            {
                loginForm = value;
            }
        }

        Login loginForm;

        private void CheckPrimaryContent()
        {
            if (PrimaryContent == PrimaryContentType.Text)
            {
                contentControlLogin.Visibility = Visibility.Hidden;
                textBoxPrimaryText.Visibility = Visibility.Visible;
            }
            else if (PrimaryContent == PrimaryContentType.LoginForm)
            {
                contentControlLogin.Visibility = Visibility.Visible;
                textBoxPrimaryText.Visibility = Visibility.Hidden;
            }
        }   

        public LauncherCard()
        {
            InitializeComponent();
            Login loginForm = new Login();
            loginForm.ShowLoadingAction = SetShowLoading;
            contentControlLogin.Content = loginForm;
            
        }

        void SetShowLoading(bool isShown)
        {
            if (isShown)
            {
                progressBarStatus.Visibility = Visibility.Visible;
            }
            else
            {
                progressBarStatus.Visibility = Visibility.Hidden;
            }
        }

        private void CardControl_Loaded(object sender, RoutedEventArgs e)
        {
            CheckPrimaryContent();
            BackgroundAction?.Invoke();
        }
    }
}
