using UsuariosRestApp.ViewModels;

namespace UsuariosRestApp.Views;

public partial class UsuariosPage : ContentPage
{
    private readonly UsuariosViewModel _viewModel;

    public UsuariosPage(UsuariosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.Usuarios.Count == 0)
        {
            await _viewModel.CargarUsuariosAsync();
        }
    }
}