using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LojaFotografiaApp.DTOs;
using LojaFotografiaApp.Helpers;
using System.Windows.Input;
using Windows.UI.Popups;
using LojaFotografiaApp.Services;
using System.Net.Http.Headers;
using LojaFotografiaApp.Views;
using Windows.UI.Xaml.Controls;

namespace LojaFotografiaApp.ViewModels
{
    public class AcessoriosPageViewModel : BindableBase
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public ObservableCollection<AccessoryDto> Acessorios { get; set; } = new ObservableCollection<AccessoryDto>();
        public ObservableCollection<AccessoryDto> FilteredAcessorios { get; set; } = new ObservableCollection<AccessoryDto>();

        public AccessoryDto SelectedAcessorio { get; set; }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SetProperty(ref _searchQuery, value);
                FilterAcessorios();
            }
        }

        public ICommand LoadAcessoriosCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddAcessorioCommand { get; }
        public ICommand RemoveAcessorioCommand { get; }
        public ICommand UpdateAcessorioCommand { get; }
        public ICommand DeleteAcessorioCommand { get; }

        public AcessoriosPageViewModel()
        {

        }

        public AcessoriosPageViewModel(IAuthService authService)
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

            LoadAcessoriosCommand = new RelayCommand(async (param) => await LoadAcessorios());
            SearchCommand = new RelayCommand(async (param) => await SearchAcessorios());
            AddAcessorioCommand = new RelayCommand(param => NavigateToAddAcessorioPage());
            RemoveAcessorioCommand = new RelayCommand(async (param) => await RemoveAcessorio());
            UpdateAcessorioCommand = new RelayCommand(param => NavigateToEditAcessorioPage(param as AccessoryDto));
            DeleteAcessorioCommand = new RelayCommand(async (param) => await DeleteAcessorio(param as AccessoryDto));

            // Carregar câmeras ao inicializar
            LoadAcessoriosCommand.Execute(null);
        }

        public async Task LoadAcessorios()
        {
            try
            {
                var acessorios = await _httpClient.GetFromJsonAsync<AccessoryDto[]>("api/Acessorio");
                if (acessorios != null)
                {
                    Acessorios.Clear();
                    FilteredAcessorios.Clear();
                    foreach (var acessorio in acessorios)
                    {
                        Acessorios.Add(acessorio);
                        FilteredAcessorios.Add(acessorio);
                    }
                }
                else
                {
                    var dialog = new MessageDialog("Nenhum acessorio encontrado.");
                    await dialog.ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao buscar acessorio: {ex.Message}");
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog($"Erro inesperado: {ex.Message}");
                await dialog.ShowAsync();
            }
        }

        private async Task SearchAcessorios()
        {
            // Este método pode ser expandido para incluir lógica de pesquisa específica
            FilterAcessorios();
        }

        private void FilterAcessorios()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                FilteredAcessorios.Clear();
                foreach (var acessorio in Acessorios)
                {
                    FilteredAcessorios.Add(acessorio);
                }
            }
            else
            {
                var query = SearchQuery.ToLower();
                var filtered = Acessorios.Where(a => a.Name.ToLower().Contains(query) || a.Brand.ToLower().Contains(query)).ToList();
                FilteredAcessorios.Clear();
                foreach (var acessorio in filtered)
                {
                    FilteredAcessorios.Add(acessorio);
                }
            }
        }

        public async Task AddAcessorio(AccessoryDto acessorio)
        {
            try
            {
                var token = _authService.GetToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("api/Acessorio", acessorio);
                if (response.IsSuccessStatusCode)
                {
                    Acessorios.Add(acessorio);
                    FilterAcessorios();
                }
                else
                {
                    var dialog = new MessageDialog($"Erro ao adicionar acessório - {response.Content.ToString()}");
                    await dialog.ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao adicionar acessorio: {ex.Message}");
                await dialog.ShowAsync();
            }

        }

        public async Task RemoveAcessorio()
        {
            try
            {
                if (SelectedAcessorio != null)
                {
                    var token = _authService.GetToken();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.DeleteAsync($"api/Acessorio/{SelectedAcessorio.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        Acessorios.Remove(SelectedAcessorio);
                        FilterAcessorios();
                        await LoadAcessorios();
                    }
                    else
                    {
                        var dialog = new MessageDialog("Erro ao remover acessório.");
                        await dialog.ShowAsync();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao remover acessório: {ex.Message}");
                await dialog.ShowAsync();
            }
        }

        public async Task UpdateAcessorio(AccessoryDto acessorio)
        {
            try
            {
                var token = _authService.GetToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync($"api/acessorio/{acessorio.Id}", acessorio);
                if (response.IsSuccessStatusCode)
                {
                    FilterAcessorios();
                    await LoadAcessorios();
                }
                else
                {
                    var dialog = new MessageDialog($"Erro ao atualizar acessorio - {response.StatusCode.ToString()}");
                    await dialog.ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao atualizar acessorio: {ex.Message}");
                await dialog.ShowAsync();
            }

        }

        private async Task DeleteAcessorio(AccessoryDto acessorio)
        {
            try
            {
                if (acessorio != null)
                {
                    var token = _authService.GetToken();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.DeleteAsync($"api/Acessorio/{acessorio.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        Acessorios.Remove(acessorio);
                        FilterAcessorios();
                        await LoadAcessorios();
                    }
                    else
                    {
                        var dialog = new MessageDialog($"Erro ao remover acessorio - {response.StatusCode}");
                        await dialog.ShowAsync();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                var dialog = new MessageDialog($"Erro ao remover acessorio: {ex.Message}");
                await dialog.ShowAsync();
            }
        }

        private void NavigateToAddAcessorioPage()
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddAcessoriosPage), this);
        }

        private void NavigateToEditAcessorioPage(AccessoryDto acessorio)
        {
            Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(EditAcessoriosPage), new Tuple<AcessoriosPageViewModel, AccessoryDto>(this, acessorio));
        }


    }
}
