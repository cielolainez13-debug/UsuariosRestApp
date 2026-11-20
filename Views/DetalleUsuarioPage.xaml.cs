using UsuariosRestApp.ViewModels;

namespace UsuariosRestApp.Views;

public partial class DetalleUsuarioPage : ContentPage
{
    public DetalleUsuarioPage(DetalleUsuarioViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}