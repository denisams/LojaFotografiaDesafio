using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Helpers;
using LojaFotografiaApp.Services;
using LojaFotografiaApp.Views;
using Windows.UI.Xaml.Controls;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Popups;

namespace LojaFotografiaApp.ViewModels
{
    public class CamerasPageViewModel : BindableBase
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public ObservableCollection<CameraDto> Cameras { get; set; } = new ObservableCollection<CameraDto>();
        public ObservableCollection<CameraDto> FilteredCameras { get; set; } = new ObservableCollection<CameraDto>();
        public CameraDto SelectedCamera { get; set; }
        private string _searchQuery;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SetProperty(ref _searchQuery, value);
                FilterCameras();
            }
        }

        public ICommand LoadCamerasCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCameraCommand { get; }
        public ICommand RemoveCameraCommand { get; }
        public ICommand UpdateCameraCommand { get; }
        public ICommand DeleteCameraCommand { get; }

        public CamerasPageViewModel()
        {

        }
        public CamerasPageViewModel(IAuthService authService)
        {
            _authService = authService;
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7044/") // URL base da sua API
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            LoadCamerasCommand = new RelayCommand(async (param) => await LoadCameras());
            SearchCommand = new RelayCommand(async (param) => await SearchCameras());
            AddCameraCommand = new RelayCommand(param => NavigateToAddCameraPage());
            RemoveCameraCommand = new RelayCommand(async (param) => await RemoveCamera());
            UpdateCameraCommand = new RelayCommand(param => NavigateToEditCameraPage(param as CameraDto));
            DeleteCameraCommand = new RelayCommand(async (param) => await DeleteCamera(param as CameraDto));

            // Carregar câmeras ao inicializar
            LoadCamerasCommand.Execute(null);
        }

        public async Task LoadCameras()
        {
            try
            {
                var cameras = await _httpClient.GetFromJsonAsync<CameraDto[]>("api/camera");
                if (cameras != null)
                {
                    Cameras.Clear();
                    FilteredCameras.Clear();
                    foreach (var camera in cameras)
                    {
                        Cameras.Add(camera);
                        FilteredCameras.Add(camera);
                    }
                }
                else
                {
                    var dialog = new MessageDialog("Nenhuma câmera encontrada.");
                    await dialog.ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao buscar câmeras: {ex.Message}");
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog($"Erro inesperado: {ex.Message}");
                await dialog.ShowAsync();
            }
        }

        private void FilterCameras()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                FilteredCameras.Clear();
                foreach (var camera in Cameras)
                {
                    FilteredCameras.Add(camera);
                }
            }
            else
            {
                var query = SearchQuery.ToLower();
                var filtered = Cameras.Where(c => c.Brand.ToLower().Contains(query) || c.Model.ToLower().Contains(query)).ToList();
                FilteredCameras.Clear();
                foreach (var camera in filtered)
                {
                    FilteredCameras.Add(camera);
                }
            }
        }

        public async Task SearchCameras()
        {
            FilterCameras();
        }

        public async Task AddCamera(CameraDto camera)
        {
            try
            {
                var token = _authService.GetToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("api/camera", camera);
                if (response.IsSuccessStatusCode)
                {
                    Cameras.Add(camera);
                    FilterCameras();
                }
                else
                {
                    var dialog = new MessageDialog($"Erro ao adicionar câmera - {response.StatusCode.ToString()}");
                    await dialog.ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao adicionar câmera: {ex.Message}");
                await dialog.ShowAsync();
            }
        }

        public async Task RemoveCamera()
        {
            try
            {
                if (SelectedCamera != null)
                {
                    var token = _authService.GetToken();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.DeleteAsync($"api/camera/{SelectedCamera.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        Cameras.Remove(SelectedCamera);
                        FilterCameras();
                        await LoadCameras();
                    }
                    else
                    {
                        var dialog = new MessageDialog("Erro ao remover câmera.");
                        await dialog.ShowAsync();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao remover câmera: {ex.Message}");
                await dialog.ShowAsync();
            }
        }

        public async Task UpdateCamera(CameraDto camera)
        {
            try
            {
                var token = _authService.GetToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync($"api/camera/{camera.Id}", camera);
                if (response.IsSuccessStatusCode)
                {
                    FilterCameras();
                    await LoadCameras();
                }
                else
                {
                    var dialog = new MessageDialog($"Erro ao atualizar câmera - {response.StatusCode.ToString()}");
                    await dialog.ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao atualizar câmera: {ex.Message}");
                await dialog.ShowAsync();
            }
        }


        private async Task DeleteCamera(CameraDto camera)
        {
            try
            {
                if (camera != null)
                {
                    var token = _authService.GetToken();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.DeleteAsync($"api/camera/{camera.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        Cameras.Remove(camera);
                        FilterCameras();
                        await LoadCameras();
                    }
                    else
                    {
                        var dialog = new MessageDialog($"Erro ao remover câmera - {response.StatusCode}");
                        await dialog.ShowAsync();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao remover câmera: {ex.Message}");
                await dialog.ShowAsync();
            }
        }

        private void NavigateToAddCameraPage()
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddCameraPage), this);
        }

        private void NavigateToEditCameraPage(CameraDto camera)
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(EditCameraPage), new Tuple<CamerasPageViewModel, CameraDto>(this, camera));
        }
    }
}
