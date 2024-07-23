using LojaFotografiaApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace LojaFotografiaApp.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            DataContext = new LoginPageViewModel();
        }

        private void OnCancelClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
