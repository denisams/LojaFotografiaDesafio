using LojaFotografiaApp.ViewModels;
using LojaFotografiaApp.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LojaFotografiaApp.Views
{
    public sealed partial class AddAcessoriosPage : Page
    {
        public AddAcessoriosPageViewModel ViewModel { get; private set; }

        public AddAcessoriosPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is AcessoriosPageViewModel acessoriosPageViewModel)
            {
                var authService = new AuthService();
                ViewModel = new AddAcessoriosPageViewModel(authService, acessoriosPageViewModel);
                DataContext = ViewModel;
            }
        }

        private void NavigateToAcessoriosPage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AcessoriosPage));
        }
    }
}
