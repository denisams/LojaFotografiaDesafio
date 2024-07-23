using LojaFotografiaApp.Services;
using LojaFotografiaApp.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LojaFotografiaApp.Views
{
    public sealed partial class AcessoriosPage : Page
    {
        public AcessoriosPageViewModel ViewModel { get; private set; }
        private IAuthService _authService;

        public AcessoriosPage()
        {
            this.InitializeComponent();
            var authService = new AuthService();
            ViewModel = new AcessoriosPageViewModel(authService);
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadAcessoriosCommand.Execute(null);
        }


        private void NavigateToMainPage(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }


        private void NavigateToAddAcessorioPage(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddAcessoriosPage), ViewModel);
        }

        private void NavigateToEditCameraPage(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedAcessorio != null)
            {
                var parameter = Tuple.Create(ViewModel, ViewModel.SelectedAcessorio);
                Frame.Navigate(typeof(EditAcessoriosPage), parameter);
            }
        }
    }
}
