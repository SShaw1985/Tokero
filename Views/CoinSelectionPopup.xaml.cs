using CommunityToolkit.Maui.Views;
using Tokero.ViewModels;

namespace Tokero.Views;

public partial class CoinSelectionPopup : Popup
{
    public CoinSelectionPopup (CoinSelectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}