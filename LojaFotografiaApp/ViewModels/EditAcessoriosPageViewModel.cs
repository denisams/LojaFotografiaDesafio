using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Helpers;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LojaFotografiaApp.ViewModels
{
    public class EditAcessoriosPageViewModel : BindableBase
    {
        private readonly IAuthService _authService;
        private readonly AcessoriosPageViewModel _acessoriosPageViewModel;

        private AcessorioDto _currentAcessorio;
        public AcessorioDto CurrentAcessorio
        {
            get => _currentAcessorio;
            set => SetProperty(ref _currentAcessorio, value);
        }

        public ICommand SaveAcessorioCommand { get; }

        public EditAcessoriosPageViewModel(IAuthService authService, AcessoriosPageViewModel acessoriosPageViewModel, AcessorioDto acessorio)
        {
            _authService = authService;
            _acessoriosPageViewModel = acessoriosPageViewModel;
            CurrentAcessorio = acessorio;

            SaveAcessorioCommand = new RelayCommand(async (param) => await SaveAcessorio());
        }

        private async Task SaveAcessorio()
        {
            await _acessoriosPageViewModel.UpdateAcessorio(CurrentAcessorio);

            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AcessoriosPage), _acessoriosPageViewModel);
        }
    }
}
