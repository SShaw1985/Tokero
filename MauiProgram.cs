using Akavache;
using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;

namespace Tokero
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp ()
        {
            BlobCache.ApplicationName = "Tokero";

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<Tokero.Interfaces.IAppPopupService, Tokero.Services.AppPopupService>();
            builder.Services.AddSingleton<Tokero.Interfaces.ICachingService, Tokero.Services.CachingService>();
            builder.Services.AddSingleton<Tokero.Interfaces.IAuthService, Tokero.Services.AuthService>();
            builder.Services.AddSingleton<Tokero.Interfaces.IPriceService, Tokero.Services.CoinMarketCapService>();

            builder.Services.AddTransient<Tokero.ViewModels.LoginViewModel>();
            builder.Services.AddTransient<Tokero.ViewModels.MainViewModel>();

            builder.Services.AddTransient<Tokero.Views.LoginPage>();
            builder.Services.AddTransient<Tokero.Views.MainPage>();
            builder.Services.AddTransient<Tokero.Views.LoadingPopup>();
            builder.Services.AddTransient<Tokero.Views.CoinSelectionPopup>();

            var app = builder.Build();
            Tokero.Helpers.ServiceHelper.Initialize(app.Services);
            return app;
        }
    }
}