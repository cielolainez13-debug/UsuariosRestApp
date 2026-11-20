using Microsoft.Extensions.Logging;
using UsuariosRestApp.Services;
using UsuariosRestApp.ViewModels;
using UsuariosRestApp.Views;

namespace UsuariosRestApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Registro de Servicios
        builder.Services.AddSingleton<ApiService>();

        // Registro de ViewModels
        builder.Services.AddTransient<UsuariosViewModel>();
        builder.Services.AddTransient<DetalleUsuarioViewModel>();
        builder.Services.AddTransient<CrearUsuarioViewModel>();

        // Registro de Vistas
        builder.Services.AddTransient<UsuariosPage>();
        builder.Services.AddTransient<DetalleUsuarioPage>();
        builder.Services.AddTransient<CrearUsuarioPage>();

        return builder.Build();
    }
}