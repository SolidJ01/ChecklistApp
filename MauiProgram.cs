using ChecklistApp.Data;
using ChecklistApp.Services;
using ChecklistApp.View;
using ChecklistApp.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ChecklistApp;

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
				fonts.AddFont("FontAwesome6-Free-Solid-900.otf", "FontAwSolid");
			});

		builder.Services.AddDbContext<ChecklistContext>(options => options.UseSqlite($"DataSource = {Path.Combine(FileSystem.AppDataDirectory, "Checklist.db")}"));
		builder.Services.AddSingleton<NavigationService>();

		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<CreateChecklistPageViewModel>();
		builder.Services.AddSingleton<CreateChecklistPage>();

		return builder.Build();
	}
}
