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
    
        if (DeviceProperties.IsDesktop)
            builder.Services.RegisterDesktopPages();
        else
    		builder.Services.RegisterMobilePages();
        
        return builder.Build();
	}

	// PAGES
	private static IServiceCollection RegisterMobilePages(this IServiceCollection services)
	{
		services.AddTransient<LoginPage>();
        services.AddSingleton<HomePage>();
        services.AddSingleton<GroupsPage>();
        services.AddTransient<InboxPage>();
        services.AddTransient<NewMessagePage>();
        return services;
	}

    private static IServiceCollection RegisterDesktopPages(this IServiceCollection services)
    {
        services.AddTransient<LoginPageDesktop>();
        services.AddSingleton<HomePageDesktop>();
        services.AddSingleton<GroupsPageDesktop>();
        services.AddTransient<InboxPageDesktop>();
        services.AddTransient<NewMessagePageDesktop>();
        return services;
    }

    // Services
    private static IServiceCollection RegisterServices(this IServiceCollection services)
	{
        services.AddSingleton<User>();
		services.AddSingleton<ShellNavigationService>();
        services.AddSingleton<IDisplayService, DisplayService>();
        services.AddSingleton<IServiceProvider, ServiceProvider>();
        return services;
	}

	// Viewmodels
    private static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddSingleton<HomeViewModel>();
        services.AddTransient<BaseViewModel>();
        services.AddTransient<InboxViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<GroupsViewModel>();
        services.AddTransient<NewMessageViewModel>();
        return services;
    }

	// Hub Connections
    private static IServiceCollection RegisterHubConnections(this IServiceCollection services)
    {
        services.AddSingleton<IGroupConnection, GroupConnection>();
        services.AddSingleton<IMessageConnection, MessageConnection>();
        services.AddSingleton<IOnlineStatusConnection, OnlineStatusConnection>();
        services.AddSingleton<IMessageStatusConnection, MessageStatusConnection>();
        return services;
    }
}
