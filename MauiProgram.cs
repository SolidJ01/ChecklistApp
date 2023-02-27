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

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<MainPage>();

		builder.Services.AddTransient<CreateChecklistPageViewModel>();
		builder.Services.AddTransient<CreateChecklistPage>();

		builder.Services.AddTransient<ChecklistPageViewModel>();
		builder.Services.AddTransient<ChecklistPage>();

		builder.Services.AddTransient<ChecklistOptionsPageViewModel>();
		builder.Services.AddTransient<ChecklistOptionsPage>();

		builder.Services.AddTransient<CreateItemPageViewModel>();
		builder.Services.AddTransient<CreateItemPage>();

		return builder.Build();
	}
}
