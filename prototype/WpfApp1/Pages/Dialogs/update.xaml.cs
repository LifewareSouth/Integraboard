using System.Windows;
using System.Windows.Forms;

namespace WpfApp1.Pages.Dialogs
{
    /// <summary>
    /// Lógica de interacción para update.xaml
    /// </summary>
    public partial class update : Window
    {
        public update()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();

            // Create the MaskedTextBox control.
            MaskedTextBox mtbDate = new MaskedTextBox("00/00/0000");

            // Assign the MaskedTextBox control as the host control's child.
            host.Child = mtbDate;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            this.updateGrid.Children.Add(host);
        }
    }
}
