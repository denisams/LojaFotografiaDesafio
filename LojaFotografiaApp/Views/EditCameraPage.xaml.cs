using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LojaFotografiaApp.Views
{
    public sealed partial class EditCameraPage : Page
    {
        public EditCameraPageViewModel ViewModel { get; private set; }

        public EditCameraPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Tuple<CamerasPageViewModel, CameraDto> navigationParameter)
            {
                var camerasPageViewModel = navigationParameter.Item1;
                var camera = navigationParameter.Item2;
                var authService = new AuthService();

                DataContext = new EditCameraPageViewModel(authService, camerasPageViewModel, camera);
            }
            else
            {
                // Lidar com a situação em que o parâmetro é nulo ou do tipo errado
                Frame.GoBack();
            }
        }

        private void NavigateToCamerasPage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(CamerasPage));
        }
    }
}
