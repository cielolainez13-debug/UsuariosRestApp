using UsuariosRestApp.Views;

namespace UsuariosRestApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(DetalleUsuarioPage), typeof(DetalleUsuarioPage));
        Routing.RegisterRoute(nameof(CrearUsuarioPage), typeof(CrearUsuarioPage));
    }
}