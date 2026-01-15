using ChecklistApp.ViewModel;

namespace ChecklistApp;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		NavigatedTo += viewModel.ReloadList;
	}

	private void OnSettingsButtonClicked(object sender, EventArgs e)
	{
		SettingsPopup.Open();
	}

	private void OnExportChecklistsButtonClicked(object sender, EventArgs e)
	{
		ChecklistExportPopup.Open();
	}

	private void NewButtonClicked(object sender, EventArgs e)
	{
		CreateChecklistPopup.Open();
	}
}

