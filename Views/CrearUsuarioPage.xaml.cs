using UsuariosRestApp.ViewModels;

namespace UsuariosRestApp.Views;

public partial class CrearUsuarioPage : ContentPage
{
    public CrearUsuarioPage(CrearUsuarioViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}