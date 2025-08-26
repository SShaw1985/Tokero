using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using Tokero.Interfaces;
using Tokero.Views;

namespace Tokero.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService auth;
        private readonly IAppPopupService appPopup;

        public LoginViewModel (IAuthService _auth, IAppPopupService _appPopup)
        {
            auth = _auth;
            appPopup = _appPopup;
        }

        private LoadingPopup loadingPopup;

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [RelayCommand]
        private async Task Login ()
        {
            if(string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Login failed", "Please enter both username and password.", "OK");
                return;
            }

            if(!IsValidEmail(Username))
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Email", "Please enter a valid email address.", "OK");
                return;
            }

            try
            {
                appPopup.ShowLoading();
                await Task.Delay(2000);
                var ok = auth != null && await auth.SignInAsync(Username, Password);
                if(!ok)
                {
                    if(loadingPopup != null)
                    {
                        try
                        {
                            await loadingPopup.CloseAsync();
                        }
                        catch { /* Ignore close errors */ }
                        loadingPopup = null;
                    }

                    appPopup.CloseLoading();
                    await Application.Current.MainPage.DisplayAlert("Login failed", "Invalid credentials.", "OK");
                    return;
                }

                appPopup.CloseLoading();
                await Shell.Current.Navigation.PopModalAsync(animated: true);
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch
            {
                appPopup.CloseLoading();
            }
        }

        private bool IsValidEmail (string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return email.Contains("@") && email.Contains(".") && email.Length > 5;
            }
        }
    }
}