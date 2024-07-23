using LojaFotografiaApp.Services;
using LojaFotografiaApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LojaFotografiaApp.Views
{
    public sealed partial class CamerasPage : Page
    {
        public CamerasPageViewModel ViewModel { get; private set; }
        private IAuthService _authService;

        public CamerasPage()
        {
            this.InitializeComponent();
            var authService = new AuthService();
            ViewModel = new CamerasPageViewModel(authService);
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadCamerasCommand.Execute(null);
        }

        private void NavigateToMainPage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        private void NavigateToAddCameraPage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddCameraPage), ViewModel);
        }

        private void NavigateToEditCameraPage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.SelectedCamera != null)
            {
                var parameter = Tuple.Create(ViewModel, ViewModel.SelectedCamera);
                Frame.Navigate(typeof(EditCameraPage), parameter);
            }
        }
    }
}
