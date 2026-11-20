using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UsuariosRestApp.Models;
using UsuariosRestApp.Services;

namespace UsuariosRestApp.ViewModels;

[QueryProperty(nameof(UserId), "UserId")]
public partial class DetalleUsuarioViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    public DetalleUsuarioViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private Usuario? usuarioDetalle;

    [ObservableProperty]
    private bool isBusy;

    partial void OnUserIdChanged(int value)
    {
        _ = CargarDetalleAsync(value);
    }

    private async Task CargarDetalleAsync(int id)
    {
        try
        {
            IsBusy = true;
            UsuarioDetalle = await _apiService.GetUsuarioByIdAsync(id);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo cargar el detalle: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EliminarUsuarioAsync()
    {
        if (UsuarioDetalle == null || IsBusy) return;

        bool confirmar = await Shell.Current.DisplayAlert(
            "Confirmar eliminación",
            $"¿Deseas eliminar a {UsuarioDetalle.Name} (ID #{UsuarioDetalle.Id})?",
            "Eliminar",
            "Cancelar");

        if (!confirmar) return;

        try
        {
            IsBusy = true;
            bool eliminado = await _apiService.DeleteUsuarioAsync(UsuarioDetalle.Id);

            if (eliminado)
            {
                await Shell.Current.DisplayAlert(
                    "Eliminado",
                    $"Usuario #{UsuarioDetalle.Id} eliminado correctamente.",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo eliminar el usuario: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}