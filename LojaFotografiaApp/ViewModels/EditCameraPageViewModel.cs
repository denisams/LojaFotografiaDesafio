using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Helpers;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LojaFotografiaApp.ViewModels
{
    public class EditCameraPageViewModel : BindableBase
    {
        private readonly IAuthService _authService;
        private readonly CamerasPageViewModel _camerasPageViewModel;

        private CameraDto _currentCamera;
        public CameraDto CurrentCamera
        {
            get => _currentCamera;
            set => SetProperty(ref _currentCamera, value);
        }

       //public CameraDto CurrentCamera { get; set; }
        public ICommand SaveCameraCommand { get; }

        public EditCameraPageViewModel(IAuthService authService, CamerasPageViewModel camerasPageViewModel, CameraDto camera)
        {
            _authService = authService;
            _camerasPageViewModel = camerasPageViewModel;
            CurrentCamera = camera;

            SaveCameraCommand = new RelayCommand(async (param) => await SaveCamera());
        }

        private async Task SaveCamera()
        {
            await _camerasPageViewModel.UpdateCamera(CurrentCamera);

            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(CamerasPage), _camerasPageViewModel);
        }
    }
}
