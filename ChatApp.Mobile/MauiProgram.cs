using ChatApp.Mobile.Services.SignalR.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Mobile;

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
			});

		builder.Services.AddSingleton<IMessageConnection, MessageConnection>();
        builder.Services.AddTransient<MainPage>();
        return builder.Build();
	}
}
