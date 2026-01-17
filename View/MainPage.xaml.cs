using ChecklistApp.ViewModel;

namespace ChecklistApp;

public partial class MainPage : ContentPage
{
	private Stack<Action> _backButtonActions = [];
	
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		NavigatedTo += viewModel.ReloadList;
	}

	private void OnSettingsButtonClicked(object sender, EventArgs e)
	{
		SettingsPopup.Open(RegisterBackButtonAction);
	}

	private void OnExportChecklistsButtonClicked(object sender, EventArgs e)
	{
		ChecklistExportPopup.Open(RegisterBackButtonAction);
	}

	private void NewButtonClicked(object sender, EventArgs e)
	{
		CreateChecklistPopup.Open(RegisterBackButtonAction);
	}

	private void RegisterBackButtonAction(Action action)
	{
		_backButtonActions.Push(action);
	}

	protected override bool OnBackButtonPressed()
	{
		if (_backButtonActions.Count.Equals(0))
			return false;
		
		_backButtonActions.Pop().Invoke();
		return true;
	}
}

