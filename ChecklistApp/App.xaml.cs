using ChecklistApp.Data;
using ChecklistApp.Services;

namespace ChecklistApp;

public partial class App : Application
{
	public App(ColorService service)
	{
		InitializeComponent();

		service.Initialize();

		MainPage = new AppShell();
	}
}
