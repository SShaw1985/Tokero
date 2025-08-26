namespace Tokero
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow (IActivationState? activationState)
        {
            var shell = new AppShell();
            var window = new Window(shell);

            window.Created += async (_, __) =>
            {
                var auth = Tokero.Helpers.ServiceHelper.GetService<Interfaces.IAuthService>();
                if(auth is not null && !auth.IsAuthenticated)
                {
                    var loginPage = Tokero.Helpers.ServiceHelper.GetService<Views.LoginPage>();
                    if(loginPage != null)
                    {
                        await Shell.Current.Navigation.PushModalAsync(new NavigationPage(loginPage)
                        {
                            BarBackgroundColor = Colors.Transparent,
                            BarTextColor = Colors.Transparent
                        }, animated: false);
                    }
                }
            };

            return window;
        }
    }
}