﻿using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Helpers;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LojaFotografiaApp.ViewModels
{
    public class AddAcessoriosPageViewModel : BindableBase
    {
        private readonly IAuthService _authService;
        private readonly AcessoriosPageViewModel _acessoriosPageViewModel;

        public AcessorioDto NewAcessorio { get; set; } = new AcessorioDto();
        public ICommand SaveAcessorioCommand { get; }

        public AddAcessoriosPageViewModel(IAuthService authService, AcessoriosPageViewModel acessoriosPageViewModel)
        {
            _authService = authService;
            _acessoriosPageViewModel = acessoriosPageViewModel;
            SaveAcessorioCommand = new RelayCommand(async (param) => await SaveAcessorio());
        }

        private async Task SaveAcessorio()
        {
            await _acessoriosPageViewModel.AddAcessorio(NewAcessorio);

            // Navegar de volta para a página de câmeras após salvar
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AcessoriosPage));
        }
    }
}