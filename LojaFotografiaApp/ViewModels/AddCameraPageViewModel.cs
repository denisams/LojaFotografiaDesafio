using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Helpers;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LojaFotografiaApp.ViewModels
{
    public class AddCameraPageViewModel : BindableBase
    {
        private readonly IAuthService _authService;
        private readonly CamerasPageViewModel _camerasPageViewModel;

        public CameraDto NewCamera { get; set; } = new CameraDto();
        public ICommand SaveCameraCommand { get; }

        public AddCameraPageViewModel(IAuthService authService, CamerasPageViewModel camerasPageViewModel)
        {
            _authService = authService;
            _camerasPageViewModel = camerasPageViewModel;
            SaveCameraCommand = new RelayCommand(async (param) => await SaveCamera());
        }

        private async Task SaveCamera()
        {
            await _camerasPageViewModel.AddCamera(NewCamera);

            // Navegar de volta para a página de câmeras após salvar
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(CamerasPage));
        }
    }
}
