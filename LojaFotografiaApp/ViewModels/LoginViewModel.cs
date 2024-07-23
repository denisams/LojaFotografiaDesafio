using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Helpers;
using LojaFotografiaApp.Services;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using LojaFotografiaApp.Views;
using System;

namespace LojaFotografiaApp.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;

        private string _username;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }

        public LoginPageViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(async (param) => await Login());
        }

        private async Task Login()
        {
            var loginDto = new LoginDto { Username = Username, Password = Password };
            var result = await _authService.Login(loginDto);

            if (result.Success)
            {
                Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
            }
            else
            {
                var dialog = new MessageDialog(result.ErrorMessage);
                await dialog.ShowAsync();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
