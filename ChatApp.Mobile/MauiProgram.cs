using ChatApp.Mobile.Pages;
using ChatApp.Mobile.Services.SignalR.Concrete;
using ChatApp.Mobile.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiCommunityToolkit()
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.RegisterHubConnections();
        builder.Services.RegisterViewModels();
		builder.Services.RegisterPages();
        return builder.Build();
	}

	// PAGES
	private static IServiceCollection RegisterPages(this IServiceCollection services)
	{
		services.AddTransient<LoginPage>();
		return services;
	}

	// Viewmodels
    private static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<BaseViewModel>();
        services.AddSingleton<LoginViewModel>();
        return services;
    }

	// Hub Connections
    private static IServiceCollection RegisterHubConnections(this IServiceCollection services)
    {
        services.AddSingleton<IMessageConnection, MessageConnection>();
        return services;
    }
}
