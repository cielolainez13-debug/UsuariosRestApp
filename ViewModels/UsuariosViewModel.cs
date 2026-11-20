using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UsuariosRestApp.Models;
using UsuariosRestApp.Services;
using UsuariosRestApp.Views;

namespace UsuariosRestApp.ViewModels;

public partial class UsuariosViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    public ObservableCollection<Usuario> Usuarios { get; } = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isRefreshing;

    public UsuariosViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    public async Task CargarUsuariosAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var lista = await _apiService.GetUsuariosAsync();
            
            Usuarios.Clear();
            foreach (var user in lista)
            {
                Usuarios.Add(user);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error de Red", $"No se pudieron cargar los usuarios: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task SeleccionarUsuarioAsync(Usuario? usuario)
    {
        if (usuario == null) return;

        // Navega pasando el Id como parámetro de consulta
        await Shell.Current.GoToAsync($"{nameof(DetalleUsuarioPage)}?UserId={usuario.Id}");
    }

    [RelayCommand]
    private async Task IrACrearUsuarioAsync()
    {
        await Shell.Current.GoToAsync(nameof(CrearUsuarioPage));
    }
}