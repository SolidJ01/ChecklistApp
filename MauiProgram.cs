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
				fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
				fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
			})
			.ConfigureEssentials(essentials =>
			{
				essentials.UseVersionTracking();
			});
		
		Environment.SetEnvironmentVariable(StringHelper.S_EnvironmentReleaseDate, "2026-07-05");

		builder.Services.AddDbContext<ChecklistContext>(options => options.UseSqlite($"DataSource = {Path.Combine(FileSystem.AppDataDirectory, "Checklist.db")}"));
		builder.Services.AddSingleton<NavigationService>();
		builder.Services.AddSingleton<ToastService>();
		builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
		builder.Services.AddSingleton<IPreferences>(Preferences.Default);
		builder.Services.AddSingleton<IVersionTracking>(VersionTracking.Default);
		builder.Services.AddSingleton<HttpClient>();
		builder.Services.AddSingleton<UpdateCheckerService>();
		
		#if ANDROID
		builder.Services.AddTransient<INotificationManagerService, NotificationManagerService>();
		#endif

		builder.Services.AddTransient<SettingsPopupViewModel>();
		builder.Services.AddTransient<CreateChecklistPopupViewModel>();
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<MainPage>();

		builder.Services.AddTransient<ChecklistOptionsPopupViewModel>();
		builder.Services.AddTransient<ChecklistPageViewModel>();
		builder.Services.AddTransient<ChecklistPage>();

		return builder.Build();
	}
}
