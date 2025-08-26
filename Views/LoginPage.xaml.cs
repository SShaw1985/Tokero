namespace Tokero.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage (ViewModels.LoginViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed ()
        {
            return true;
        }
    }
}