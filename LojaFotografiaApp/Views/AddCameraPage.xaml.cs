using LojaFotografiaApp.ViewModels;
using LojaFotografiaApp.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LojaFotografiaApp.Views
{
    public sealed partial class AddCameraPage : Page
    {
        public AddCameraPageViewModel ViewModel { get; private set; }

        public AddCameraPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is CamerasPageViewModel camerasPageViewModel)
            {
                var authService = new AuthService();
                ViewModel = new AddCameraPageViewModel(authService, camerasPageViewModel);
                DataContext = ViewModel;
            }
        }

        private void NavigateToCamerasPage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CamerasPage));
        }
    }
}
