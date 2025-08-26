using CommunityToolkit.Maui.Views;
using Tokero.Interfaces;
using Tokero.ViewModels;
using Tokero.Views;

namespace Tokero.Services
{
    public class AppPopupService : IAppPopupService
    {
        private LoadingPopup loading { get; set; }
        private CoinSelectionPopup coinSelectionPopup { get; set; }

        public AppPopupService ()
        {
        }

        public async Task ShowLoading (string text)
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var shell = Application.Current?.MainPage as Shell ?? throw new NullReferenceException();
                    var currentPage = shell.CurrentPage ?? throw new NullReferenceException();

                    loading = new LoadingPopup();
                    loading.SetText(text);
                    await currentPage.ShowPopupAsync(loading);
                });
            }
            catch(Exception ex)
            {
                string ss = ex.Message;
            }
        }

        public async Task CloseLoading ()
        {
            try
            {
                if(loading != null)
                    await loading.CloseAsync();

                loading = null;
            }
            catch(Exception ex)
            {
                var ss = ex.Message;
            }
        }

        public async Task ShowCoinSelectionPopup (List<string> coins, List<string> selectedCoins, Action<List<string>> onSelectionChanged)
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var shell = Application.Current?.MainPage as Shell ?? throw new NullReferenceException();
                    var currentPage = shell.CurrentPage ?? throw new NullReferenceException();

                    var popupViewModel = new CoinSelectionViewModel(coins, selectedCoins, onSelectionChanged);
                    coinSelectionPopup = new CoinSelectionPopup(popupViewModel);
                    popupViewModel.SetPopup(coinSelectionPopup);
                    await currentPage.ShowPopupAsync(coinSelectionPopup);
                });
            }
            catch(Exception ex)
            {
                string ss = ex.Message;
            }
        }

        public async Task CloseCoinSelectionPopup ()
        {
            try
            {
                if(coinSelectionPopup != null)
                    await coinSelectionPopup.CloseAsync();

                coinSelectionPopup = null;
            }
            catch(Exception ex)
            {
                var ss = ex.Message;
            }
        }

        public async Task ShowPopup (Popup popup)
        {
            var shell = Application.Current?.MainPage as Shell ?? throw new NullReferenceException();
            var currentPage = shell.CurrentPage ?? throw new NullReferenceException();
            currentPage.ShowPopup(popup);
            await Task.Delay(6000);
        }

        async Task IAppPopupService.ClosePopup (Popup popup)
        {
            try
            {
                await popup.CloseAsync();
            }
            catch(Exception ex)
            {
                string ss = ex.Message;
            }
        }
    }
}