using ChecklistApp.View;

namespace ChecklistApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(CreateChecklistPage), typeof(CreateChecklistPage));
		Routing.RegisterRoute(nameof(ChecklistPage), typeof(ChecklistPage));
	}
}
