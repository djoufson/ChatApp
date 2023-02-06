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
		builder.Services.RegisterServices();
        builder.Services.RegisterViewModels();
		builder.Services.RegisterPages();
        return builder.Build();
	}

	// PAGES
	private static IServiceCollection RegisterPages(this IServiceCollection services)
	{
		services.AddTransient<LoginPage>();
        services.AddSingleton<HomePage>();
        services.AddTransient<InboxPage>();
        return services;
	}

	// Services
	private static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddSingleton<ShellNavigationService>();
        services.AddSingleton<User>();
        return services;
	}

	// Viewmodels
    private static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<BaseViewModel>();
        services.AddTransient<InboxViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<HomeViewModel>();
        return services;
    }

	// Hub Connections
    private static IServiceCollection RegisterHubConnections(this IServiceCollection services)
    {
        services.AddSingleton<IMessageConnection, MessageConnection>();
        return services;
    }
}
