using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UsuariosRestApp.Models;
using UsuariosRestApp.Services;

namespace UsuariosRestApp.ViewModels;

public partial class CrearUsuarioViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    public CrearUsuarioViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private string website = string.Empty;

    [ObservableProperty]
    private string? fotoRuta;

    [ObservableProperty]
    private ImageSource? fotoPreview;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task TomarFotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlert("No soportado", "La cámara no está disponible en este dispositivo.", "OK");
                return;
            }

            FileResult? foto = await MediaPicker.Default.CapturePhotoAsync();
            if (foto != null)
            {
                await CargarFotoDesdeFileResultAsync(foto);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error de cámara", $"No se pudo tomar la fotografía: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task SeleccionarFotoAsync()
    {
        try
        {
            FileResult? foto = await MediaPicker.Default.PickPhotoAsync();
            if (foto != null)
            {
                await CargarFotoDesdeFileResultAsync(foto);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error de galería", $"No se pudo seleccionar la imagen: {ex.Message}", "OK");
        }
    }

    private async Task CargarFotoDesdeFileResultAsync(FileResult foto)
    {
        string rutaLocal = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}_{foto.FileName}");

        await using Stream sourceStream = await foto.OpenReadAsync();
        await using FileStream localStream = File.OpenWrite(rutaLocal);
        await sourceStream.CopyToAsync(localStream);

        FotoRuta = rutaLocal;
        FotoPreview = ImageSource.FromFile(rutaLocal);
    }

    [RelayCommand]
    private async Task GuardarUsuarioAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email))
        {
            await Shell.Current.DisplayAlert("Validación", "El nombre y el correo son obligatorios.", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            var nuevo = new Usuario
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                Website = Website
            };

            var respuesta = await _apiService.CreateUsuarioAsync(nuevo);

            if (respuesta != null)
            {
                string mensajeFoto = string.IsNullOrWhiteSpace(FotoRuta)
                    ? "Sin fotografía adjunta."
                    : "Con fotografía adjunta.";

                await Shell.Current.DisplayAlert(
                    "Usuario Creado",
                    $"El servidor registró el usuario con el ID: {respuesta.Id}\n{mensajeFoto}",
                    "Genial");

                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Falló el envío POST: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}