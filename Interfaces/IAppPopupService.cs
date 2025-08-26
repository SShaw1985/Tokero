using CommunityToolkit.Maui.Views;

namespace Tokero.Interfaces
{
    public interface IAppPopupService
    {
        Task ShowLoading (string text = "Loading...");

        Task CloseLoading ();

        Task ShowPopup (Popup popup);

        Task ClosePopup (Popup popup);

        Task ShowCoinSelectionPopup (List<string> coins, List<string> selectedCoins, Action<List<string>> onSelectionChanged);

        Task CloseCoinSelectionPopup ();
    }
}