using Tokero.ViewModels;

namespace Tokero.Views;

public partial class MainPage : ContentPage
{
    private bool _isInitialized = false;

    public MainPage (MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing ()
    {
        base.OnAppearing();

        if(!_isInitialized && BindingContext is MainViewModel viewModel)
        {
            _isInitialized = true;
            await viewModel.InitializeAsync();
        }
    }
}