using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LojaFotografiaApp.Helpers;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LojaFotografiaApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private string _username;
        private bool _isLoggedIn;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            _authService = new AuthService();
          
            NavigateToLoginCommand = new RelayCommand(param => NavigateToLogin());
            NavigateToCamerasPageCommand = new RelayCommand(param => NavigateToCamerasPage());
            NavigateToAcessoriosPageCommand = new RelayCommand(param => NavigateToAcessoriosPage());
            LogoutCommand = new RelayCommand(param => Logout());
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(WelcomeMessage));
                }
            }
        }
        public bool IsLoggedIn => _authService.IsLoggedIn();
        public string WelcomeMessage => IsLoggedIn ? "Bem-vindo, " + _authService.GetUsername() : string.Empty;


        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToCamerasPageCommand { get; }
        public ICommand NavigateToAcessoriosPageCommand { get; }
        public ICommand LogoutCommand { get; }

        private void NavigateToLogin()
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(LoginPage));
        }

        private void NavigateToCamerasPage()
        {
            var frame = (Frame)Window.Current.Content;
            var camerasPageViewModel = new CamerasPageViewModel(_authService);
            frame.Navigate(typeof(CamerasPage), camerasPageViewModel);
        }

        private void NavigateToAcessoriosPage()
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AcessoriosPage));
        }

        private void Logout()
        {
            _authService.Logout();
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(WelcomeMessage));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
