using ChecklistApp.ViewModel;

namespace ChecklistApp.View;

public partial class ChecklistPage : ContentPage
{
	private Stack<Action> _backButtonActions = [];
	
	public ChecklistPage(ChecklistPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		NavigatedTo += viewModel.RetrieveChecklist;
	}

	private void OptionsButtonClicked(object sender, EventArgs e)
	{
		OptionsPopup.Open(RegisterBackButtonAction);
	}

	private void CreateItemsButtonClicked(object sender, EventArgs e)
	{
		CreateItemPopup.Open(RegisterBackButtonAction);
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