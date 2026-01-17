using ChecklistApp.View;
using ChecklistApp.ViewModel;

namespace ChecklistApp;

public partial class MainPage : PopupPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		NavigatedTo += viewModel.ReloadList;
	}

	private void OnSettingsButtonClicked(object sender, EventArgs e)
	{
		SettingsPopup.Open(RegisterBackButtonAction, DeregisterBackButtonAction);
	}

	private void OnExportChecklistsButtonClicked(object sender, EventArgs e)
	{
		ChecklistExportPopup.Open(RegisterBackButtonAction, DeregisterBackButtonAction);
	}

	private void NewButtonClicked(object sender, EventArgs e)
	{
		CreateChecklistPopup.Open(RegisterBackButtonAction, DeregisterBackButtonAction);
	}
}

