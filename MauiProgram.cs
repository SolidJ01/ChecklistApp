using ChecklistApp.Controls;
using ChecklistApp.Data;
using ChecklistApp.Services;
using ChecklistApp.View;
using ChecklistApp.ViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChecklistApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("FontAwesome6-Free-Solid-900.otf", "FontAwSolid");
			});

		builder.Services.AddDbContext<ChecklistContext>(options => options.UseSqlite($"DataSource = {Path.Combine(FileSystem.AppDataDirectory, "Checklist.db")}"));
		builder.Services.AddSingleton<NavigationService>();
		builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
		builder.Services.AddSingleton<IPreferences>(Preferences.Default);
		
		#if ANDROID
		builder.Services.AddTransient<INotificationManagerService, NotificationManagerService>();
		#endif

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<SettingsPopupViewModel>();
		builder.Services.AddTransient<CreateChecklistPopupViewModel>();
		builder.Services.AddTransient<MainPage>();


		builder.Services.AddTransient<ChecklistPageViewModel>();
		builder.Services.AddTransient<ChecklistOptionsPageViewModel>();
		builder.Services.AddTransient<CreateItemPageViewModel>();
		builder.Services.AddTransient<ChecklistPage>();

		return builder.Build();
	}
}
