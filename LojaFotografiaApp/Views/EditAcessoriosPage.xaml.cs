using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LojaFotografiaApp.Views
{
    public sealed partial class EditAcessoriosPage : Page
    {
        public EditAcessoriosPageViewModel ViewModel { get; private set; }

        public EditAcessoriosPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Tuple<AcessoriosPageViewModel, AccessoryDto> navigationParameter)
            {
                var acessoriosPageViewModel = navigationParameter.Item1;
                var acessorio = navigationParameter.Item2;
                var authService = new AuthService(); 

                DataContext = new EditAcessoriosPageViewModel(authService, acessoriosPageViewModel, acessorio);
            }
            else
            {
                // Lidar com a situação em que o parâmetro é nulo ou do tipo errado
                Frame.GoBack();
            }
        }

        private void NavigateToAcessoriosPage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AcessoriosPage));
        }
    }
}
