// using Maui.FreakyControls.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Mixology.Services;
using Mixology.Services.Extensions;
using Mixology.ViewModels;

namespace Mixology;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("OpenSans-ExtraBold.ttf", "OpenSansExtraBold");
            });
        
        using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
        builder.Configuration.AddConfiguration(config);
        // builder.InitializeFreakyControls();
        
        builder.Services.AddApiClient<MixService>(config);
        builder.Services.AddApiClient<MaterialService>(config);
        builder.Services.AddApiClient<UserService>(config);
        
        EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
            handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
        });
        
        
        builder.Services.AddSingleton<MainVM>();
        builder.Services.AddTransient<ReactiveExamplesVM>();
        builder.Services.AddTransient<RegisterVM>();
        
        builder.Services.AddTransient<Views.Pages.Main>();
        builder.Services.AddTransient<Views.Pages.ReactiveExamples>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}